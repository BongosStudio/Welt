using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Welt.API;
using Welt.API.Forge;
using Welt.API.Net;
using Welt.API.Physics;
using Welt.API.Windows;
using Welt.Core.Forge;
using Welt.Core.Net;
using Welt.Core.Net.Packets;
using Welt.Core.Physics;
using Welt.Core.Server;
using Welt.Events.Forge;
using Welt.Forge;

namespace Welt
{
    public delegate void PacketHandler(IPacket packet, MultiplayerClient client);

    /// <summary>
    ///     Implements data and events for networking and data received to and from the game server.
    /// </summary>
    public class MultiplayerClient : IAABBEntity, INotifyPropertyChanged, IDisposable // TODO: Make IMultiplayerClient and so on
    {
        public event EventHandler<ChatMessageEventArgs> ChatMessage;
        public event EventHandler<ChunkEventArgs> ChunkModified;
        public event EventHandler<ChunkEventArgs> ChunkLoaded;
        public event EventHandler<ChunkEventArgs> ChunkUnloaded;
        public event EventHandler<BlockChangeEventArgs> BlockChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private long m_Connected;
        private int hotbarSelection;

        public User User { get; set; }
        public ReadOnlyWorld World { get; private set; }
        public PhysicsEngine Physics { get; set; }
        public bool IsLoggedIn { get; internal set; }
        public bool IsPaused { get; internal set; }
        public int EntityID { get; internal set; }
        public InventoryContainer Inventory { get; set; }
        public int Health { get; set; }
        public IWindow CurrentWindow { get; set; }
        public ICraftingRepository CraftingRepository { get; set; }
        public Vector3I? LookingAt { get; set; }
        public BlockFaceDirection? LookingAtFace { get; set; }

        public bool Connected => Interlocked.Read(ref m_Connected) == 1;

        public int HotbarSelection
        {
            get { return hotbarSelection; }
            set
            {
                hotbarSelection = value;
                // send update to server?
            }
        }

        private TcpClient Client { get; set; }
        private IWeltStream Stream { get; set; }
        private PacketReader PacketReader { get; set; }

        private readonly PacketHandler[] m_PacketHandlers;

        private SemaphoreSlim m_Sem = new SemaphoreSlim(1, 1);

        private readonly CancellationTokenSource m_Cancel;

        private SocketAsyncEventArgsPool SocketPool { get; set; }

        public MultiplayerClient(User user)
        {
            User = user;
            Client = new TcpClient();
            PacketReader = new PacketReader();
            PacketReader.RegisterCorePackets();
            m_PacketHandlers = new PacketHandler[0x100];
            Handlers.PacketHandlers.RegisterHandlers(this);
            World = new ReadOnlyWorld();
            Inventory = new InventoryContainer();
            var repo = new BlockRepository();
            repo.DiscoverBlockProviders();
            Physics = new PhysicsEngine(World.World, repo);
            SocketPool = new SocketAsyncEventArgsPool(100, 200, 65536);
            m_Connected = 0;
            m_Cancel = new CancellationTokenSource();
            Health = 20;
            var crafting = new CraftingRepository();
            CraftingRepository = crafting;
            crafting.DiscoverRecipes();
        }

        public void RegisterPacketHandler(byte packetId, PacketHandler handler)
        {
            m_PacketHandlers[packetId] = handler;
        }

        public void Connect(IPEndPoint endPoint)
        {
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += Connection_Completed;
            args.RemoteEndPoint = endPoint;

            if (!Client.Client.ConnectAsync(args))
                Connection_Completed(this, args);
        }

        private void Connection_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                Interlocked.CompareExchange(ref m_Connected, 1, 0);

                Physics.AddEntity(this);

                StartReceive();
                QueuePacket(new HandshakePacket(User.Username));
            }
            else
            {
                throw new Exception("Could not connect to server!");
            }
        }

        public void Disconnect()
        {
            if (!Connected)
                return;

            QueuePacket(new DisconnectPacket("Disconnecting"));

            Interlocked.CompareExchange(ref m_Connected, 0, 1);
        }

        public void SendMessage(string message)
        {
            var parts = message.Split('\n');
            foreach (var part in parts)
                QueuePacket(new ChatMessagePacket(part));
        }

        public void QueuePacket(IPacket packet)
        {
            if (!Connected || (Client != null && !Client.Connected))
                return;

            using (MemoryStream writeStream = new MemoryStream())
            {
                using (WeltStream ms = new WeltStream(writeStream))
                {
                    ms.WriteUInt8(packet.Id);
                    packet.WritePacket(ms);
                }

                byte[] buffer = writeStream.ToArray();

                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.UserToken = packet;
                args.Completed += OperationCompleted;
                args.SetBuffer(buffer, 0, buffer.Length);

                if (Client != null && !Client.Client.SendAsync(args))
                    OperationCompleted(this, args);
            }
        }

        private void StartReceive()
        {
            SocketAsyncEventArgs args = SocketPool.Get();
            args.Completed += OperationCompleted;

            if (!Client.Client.ReceiveAsync(args))
                OperationCompleted(this, args);
        }

        private void OperationCompleted(object sender, SocketAsyncEventArgs e)
        {
            e.Completed -= OperationCompleted;

            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessNetwork(e);

                    SocketPool.Add(e);
                    break;
                case SocketAsyncOperation.Send:
                    IPacket packet = e.UserToken as IPacket;

                    if (packet is DisconnectPacket)
                    {
                        Client.Client.Shutdown(SocketShutdown.Send);
                        Client.Close();

                        m_Cancel.Cancel();
                    }

                    e.SetBuffer(null, 0, 0);
                    break;
            }
        }

        private void ProcessNetwork(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                SocketAsyncEventArgs newArgs = SocketPool.Get();
                newArgs.Completed += OperationCompleted;

                if (Client != null && !Client.Client.ReceiveAsync(newArgs))
                    OperationCompleted(this, newArgs);

                try
                {
                    m_Sem.Wait(m_Cancel.Token);
                }
                catch (OperationCanceledException)
                {
                    return;
                }

                var packets = PacketReader.ReadPackets(this, e.Buffer, e.Offset, e.BytesTransferred, false);

                foreach (IPacket packet in packets)
                {
                    m_PacketHandlers[packet.Id]?.Invoke(packet, this);
                }

                if (m_Sem != null)
                    m_Sem.Release();
            }
            else
            {
                Disconnect();
            }
        }

        protected internal void OnChatMessage(ChatMessageEventArgs e)
        {
            ChatMessage?.Invoke(this, e);
        }

        protected internal void OnChunkLoaded(ChunkEventArgs e)
        {
            ChunkLoaded?.Invoke(this, e);
        }

        protected internal void OnChunkUnloaded(ChunkEventArgs e)
        {
            ChunkUnloaded?.Invoke(this, e);
        }

        protected internal void OnChunkModified(ChunkEventArgs e)
        {
            ChunkModified?.Invoke(this, e);
        }

        protected internal void OnBlockChanged(BlockChangeEventArgs e)
        {
            BlockChanged?.Invoke(this, e);
        }

        #region IAABBEntity implementation

        public const float Width = 0.6f;
        public const float Height = 1.62f;
        public const float Depth = 0.6f;

        public void TerrainCollision(Vector3 collisionPoint, Vector3 collisionDirection)
        {
            // This space intentionally left blank
        }

        public BoundingBox BoundingBox
        {
            get
            {
                var pos = Position - new Vector3(Width / 2, 0, Depth / 2);
                return new BoundingBox(pos, pos + Size);
            }
        }

        public Size Size => new Size(Width, Height, Depth);

        #endregion

        #region IPhysicsEntity implementation

        public bool BeginUpdate()
        {
            return true;
        }

        public void EndUpdate(Vector3 newPosition)
        {
            Position = newPosition;
        }

        public float Yaw { get; set; }
        public float Pitch { get; set; }

        internal Vector3 _Position;
        public Vector3 Position
        {
            get
            {
                return _Position;
            }
            set
            {
                if (_Position != value)
                {
                    QueuePacket(new PlayerPositionAndLookPacket(value.X, value.Y, value.Y + Height,
                        value.Z, Yaw, Pitch, false));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Position"));
                }
                _Position = value;
            }
        }

        public Vector3 Velocity { get; set; }

        public float AccelerationDueToGravity => 1.6f;

        public float Drag => 0.4f;

        public float TerminalVelocity => 78.4f;

        #endregion

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disconnect();

                m_Sem.Dispose();
            }

            m_Sem = null;
        }

        ~MultiplayerClient()
        {
            Dispose(false);
        }
    }
}

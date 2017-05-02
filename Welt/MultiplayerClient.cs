using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
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
using Welt.Events;
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
        public event EventHandler<Events.ChatMessageEventArgs> ChatMessage;
        public event EventHandler<ChunkEventArgs> ChunkModified;
        public event EventHandler<ChunkEventArgs> ChunkLoaded;
        public event EventHandler<ChunkEventArgs> ChunkUnloaded;
        public event EventHandler<BlockChangedEventArgs> BlockChanged;
        public event EventHandler<ServerDiscoveredEventArgs> ServerDiscovered;
        public event PropertyChangedEventHandler PropertyChanged;

        private long m_Connected;
        private int m_HotbarSelection;
        private string m_LastErrorMessage;

        public User User { get; set; }
        public ReadOnlyWorld World { get; private set; }
        public PhysicsEngine Physics { get; set; }
        public bool IsLoggedIn { get; internal set; }
        public bool IsPaused { get; internal set; }
        public int EntityID { get; internal set; }
        public InventoryContainer Inventory { get; set; }
        public int Health { get; set; }
        public IWindow CurrentWindow { get; set; }
        public IBlockRepository BlockRepository { get; set; }
        public IItemRepository ItemRepository { get; set; }
        public ICraftingRepository CraftingRepository { get; set; }
        public Vector3I? LookingAt { get; set; }
        public BlockFaceDirection? LookingAtFace { get; set; }
        public int LoadedChunks { get; private set; }

        public bool IsConnected => Client.ConnectionStatus == NetConnectionStatus.Connected;
        
        public string LastErrorMessage
        {
            get
            {
                return m_LastErrorMessage;
            }
            set
            {
                if (m_LastErrorMessage == value) return;
                m_LastErrorMessage = value;
                OnPropertyChanged();
                
            }
        }

        public int HotbarSelection
        {
            get { return m_HotbarSelection; }
            set
            {
                m_HotbarSelection = value;
                // send update to server?
            }
        }

        public ItemStack SelectedItem => Inventory[HotbarSelection];

        private NetClient Client { get; set; }
        private PacketReader PacketReader { get; set; }
        private List<NetIncomingMessage> m_IncomingMessages;

        private readonly PacketHandler[] m_PacketHandlers;

        private SemaphoreSlim m_Sem = new SemaphoreSlim(1, 4);

        private readonly CancellationTokenSource m_Cancel;


        public MultiplayerClient(User user)
        {
            User = user;
            Client = new NetClient(new NetPeerConfiguration("welt")
            {
                UseMessageRecycling = true,
                AcceptIncomingConnections = false,
                ReceiveBufferSize = ushort.MaxValue,
                SendBufferSize = ushort.MaxValue,
                AutoExpandMTU = true
            });
            PacketReader = new PacketReader();
            PacketReader.RegisterCorePackets();
            
            m_PacketHandlers = new PacketHandler[0x100];
            Handlers.PacketHandlers.RegisterHandlers(this);
            World = new ReadOnlyWorld();
            Inventory = new InventoryContainer();
            m_Connected = 0;
            m_Cancel = new CancellationTokenSource();
            m_IncomingMessages = new List<NetIncomingMessage>();
            Health = 20;
            var blockRepository = new BlockRepository();
            blockRepository.DiscoverBlockProviders();
            BlockRepository = blockRepository;
            var itemRepository = new ItemRepository();
            itemRepository.DiscoverItemProviders();
            ItemRepository = itemRepository;
            BlockProvider.ItemRepository = ItemRepository;
            BlockProvider.BlockRepository = BlockRepository;
            var craftingRepository = new CraftingRepository();
            craftingRepository.DiscoverRecipes();
            CraftingRepository = craftingRepository;
            Physics = new PhysicsEngine(World.World, blockRepository);
            Client.Start();
        }

        public void RegisterPacketHandler(byte packetId, PacketHandler handler)
        {
            m_PacketHandlers[packetId] = handler;
        }

        public void Connect(IPEndPoint endPoint)
        {
            Client.DiscoverKnownPeer(endPoint);
        }
        
        public void Disconnect()
        {
            if (!IsConnected)
                return;

            QueuePacket(new DisconnectPacket("Disconnecting"));
            Client.Disconnect("Client disconnecting");

            Interlocked.CompareExchange(ref m_Connected, 0, 1);
        }

        public void Update(GameTime gameTime)
        {
            var inCount = Client.ReadMessages(m_IncomingMessages);
            if (inCount < 1) return;
            foreach (var message in m_IncomingMessages)
            {
                IPacket packet;
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        packet = PacketReader.ReadPacket(message, false);
                        m_PacketHandlers[packet.Id]?.Invoke(packet, this);
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        Client.Connect(message.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        switch ((NetConnectionStatus)message.ReadByte())
                        {
                            case NetConnectionStatus.Connected:
                                Debug.WriteLine("Connected");
                                QueuePacket(new HandshakePacket(User.Username));
                                break;
                            case NetConnectionStatus.Disconnected:
                                break;
                        }
                        break;
                    default:
                        Debug.WriteLine($"Could not read {message.MessageType}");
                        break;
                }
                Client.Recycle(message);
            }
            m_IncomingMessages.Clear();
            Client.FlushSendQueue();
        }

        public void SendMessage(string message)
        {
            var parts = message.Split('\n');
            foreach (var part in parts)
                QueuePacket(new ChatMessagePacket(part));
        }

        public void QueuePacket(IPacket packet)
        {
            if (!IsConnected)
                return;
            var message = Client.CreateMessage();
            PacketReader.WritePacket(message, packet);
            Client.SendMessage(message, NetDeliveryMethod.ReliableOrdered, 0);
        }

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected internal void OnServerDiscovered(ServerDiscoveredEventArgs e)
        {
            ServerDiscovered?.Invoke(this, e);
        }

        protected internal void OnChatMessage(Events.ChatMessageEventArgs e)
        {
            ChatMessage?.Invoke(this, e);
        }

        protected internal void OnChunkLoaded(ChunkEventArgs e)
        {
            LoadedChunks++;
            ChunkLoaded?.Invoke(this, e);
        }

        protected internal void OnChunkUnloaded(ChunkEventArgs e)
        {
            LoadedChunks--;
            ChunkUnloaded?.Invoke(this, e);
        }

        protected internal void OnChunkModified(ChunkEventArgs e)
        {
            ChunkModified?.Invoke(this, e);
        }

        protected internal void OnBlockChanged(BlockChangedEventArgs e)
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
                    OnPropertyChanged();
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

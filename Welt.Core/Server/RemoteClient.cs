using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Welt.API;
using Welt.API.Entities;
using Welt.API.Forge;
using Welt.API.Logging;
using Welt.API.Net;
using Welt.Core.Entities;
using Welt.Core.Forge;
using Welt.Core.Net;
using Welt.Core.Net.Packets;

namespace Welt.Core.Server
{
    public class RemoteClient : IRemoteClient, IEventSubject, IDisposable
    {
        public RemoteClient(IMultiplayerServer server, IPacketReader packetReader, PacketHandler[] packetHandlers, Socket connection)
        {
            LoadedChunks = new List<Vector3I>();
            Server = server;
            
            KnownEntities = new List<IEntity>();
            Disconnected = false;
            HasLogging = server.EnableClientLogging;
            NextWindowID = 1;
            Connection = connection;
            SocketPool = new SocketAsyncEventArgsPool(100, 200, 65536);
            PacketReader = packetReader;
            PacketHandlers = packetHandlers;

            m_Cancel = new CancellationTokenSource();

            StartReceive();
        }

        public event EventHandler Disposed;

        /// <summary>
        /// A list of entities that this client is aware of.
        /// </summary>
        public List<IEntity> KnownEntities { get; set; }
        internal sbyte NextWindowID { get; set; }
        
        public IWeltStream GameStream { get; internal set; }
        public string Username { get; set; }
        public bool IsLoggedIn { get; internal set; }
        public IMultiplayerServer Server { get; set; }
        public IWorld World { get; internal set; }
        public InventoryContainer Inventory { get; private set; }
        public short SelectedSlot { get; internal set; }
        public bool HasLogging { get; set; }
        public IPacket LastSuccessfulPacket { get; set; }
        public DateTime ExpectedDigComplete { get; set; }

        public Socket Connection { get; private set; }

        private SemaphoreSlim m_Sem = new SemaphoreSlim(1, 8);

        private SocketAsyncEventArgsPool SocketPool { get; set; }

        public IPacketReader PacketReader { get; private set; }

        private PacketHandler[] PacketHandlers { get; set; }

        private IEntity m_Entity;

        private long m_Disconnected;

        private readonly CancellationTokenSource m_Cancel;

        public bool Disconnected
        {
            get
            {
                return Interlocked.Read(ref m_Disconnected) == 1;
            }
            internal set
            {
                Interlocked.CompareExchange(ref m_Disconnected, value ? 1 : 0, value ? 0 : 1);
            }
        }

        public IEntity Entity
        {
            get
            {
                return m_Entity;
            }
            internal set
            {
                m_Entity = value;
            }
        }

        void HandlePickUpItem(object sender, EventArgs e)
        {
            
        }

        public ItemStack SelectedItem => new ItemStack();

        internal int ChunkRadius { get; set; }
        internal IList<Vector3I> LoadedChunks { get; set; }

        public bool DataAvailable => true;

        public bool Load()
        {
            
            return false;
        }

        public void Save()
        {
           
        }
        
        public void Log(string message, params object[] parameters)
        {
            
        }

        public void QueuePacket(IPacket packet)
        {
            if (Disconnected || (Connection != null && !Connection.Connected))
                return;
            using (var writeStream = new MemoryStream())
            {
                using (var ms = new WeltStream(writeStream))
                {
                    writeStream.WriteByte(packet.Id);
                    packet.WritePacket(ms);
                    ms.BaseStream.Flush();
                }

                byte[] buffer = writeStream.ToArray();

                var args = new SocketAsyncEventArgs()
                {
                    UserToken = packet
                };
                args.Completed += OperationCompleted;
                args.SetBuffer(buffer, 0, buffer.Length);

                if (Connection != null)
                {
                    if (!Connection.SendAsync(args))
                        OperationCompleted(this, args);
                }
            }
        }

        private void StartReceive()
        {
            SocketAsyncEventArgs args = SocketPool.Get();
            args.Completed += OperationCompleted;

            if (!Connection.ReceiveAsync(args))
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
                        Server.DisconnectClient(this);

                    e.SetBuffer(null, 0, 0);
                    break;
                case SocketAsyncOperation.Disconnect:
                    Connection.Close();

                    break;
            }

            if (Connection != null)
                if (!Connection.Connected && !Disconnected)
                    Server.DisconnectClient(this);
        }

        private void ProcessNetwork(SocketAsyncEventArgs e)
        {
            if (Connection == null || !Connection.Connected)
                return;

            if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                SocketAsyncEventArgs newArgs = SocketPool.Get();
                newArgs.Completed += OperationCompleted;

                if (!Connection.ReceiveAsync(newArgs))
                    OperationCompleted(this, newArgs);

                try
                {
                    m_Sem.Wait(500, m_Cancel.Token);
                }
                catch (OperationCanceledException)
                {
                }
                catch (NullReferenceException)
                {
                }
                catch (TimeoutException)
                {
                    Server.DisconnectClient(this);
                    return;
                }
                catch (ArgumentNullException)
                {

                }

                var packets = PacketReader.ReadPackets(this, e.Buffer, e.Offset, e.BytesTransferred);

                foreach (IPacket packet in packets)
                {
                    if (packet == null) break;
                    LastSuccessfulPacket = packet;

                    if (PacketHandlers[packet.Id] != null)
                    {
                        try
                        {
                            PacketHandlers[packet.Id](packet, this, Server);
                        }
                        //catch (PlayerDisconnectException)
                        //{
                        //    Server.DisconnectClient(this);
                        //}
                        catch (Exception ex)
                        {
                            Server.Log(LogCategory.Debug, "Disconnecting client due to exception in network worker");
                            Server.Log(LogCategory.Debug, ex.ToString());

                            Server.DisconnectClient(this);
                        }
                    }
                    else
                    {
                        Log("Unhandled packet {0}", packet.GetType().Name);
                    }
                }

                m_Sem?.Release();
            }
            else
            {
                Server.DisconnectClient(this);
            }
        }

        public void Disconnect()
        {
            if (Disconnected)
                return;

            Disconnected = true;

            m_Cancel.Cancel();

            Connection.Shutdown(SocketShutdown.Send);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += OperationCompleted;
            Connection.DisconnectAsync(args);
        }

        public void SendMessage(string message)
        {
            var parts = message.Split('\n');
            foreach (var part in parts)
                QueuePacket(new ChatMessagePacket(part));
        }

        internal void ExpandChunkRadius(IMultiplayerServer server)
        {
            if (this.Disconnected)
                return;
            Task.Factory.StartNew(() =>
            {
                if (ChunkRadius < 8) // TODO: Allow customization of this number
                {
                    ChunkRadius++;
                    UpdateChunks();
                    server.Scheduler.ScheduleEvent("remote.chunks", this, TimeSpan.FromSeconds(1), ExpandChunkRadius);
                }
            });
        }

        internal void SendKeepAlive(IMultiplayerServer server)
        {
            QueuePacket(new KeepAlivePacket());
            server.Scheduler.ScheduleEvent("remote.keepalive", this, TimeSpan.FromSeconds(10), SendKeepAlive);
        }

        internal void UpdateChunks()
        {
            if (!IsLoggedIn) return;
            var newChunks = new List<Vector3I>();
            var center = new Vector3((int)(Entity.Position.X / Chunk.Width), 0, (int)(Entity.Position.Z / Chunk.Depth));
            for (int x = -ChunkRadius; x < ChunkRadius; x++)
            {
                for (int z = -ChunkRadius; z < ChunkRadius; z++)
                {
                    var adjustment = center + new Vector3(x, 0, z);
                    Console.WriteLine($"{adjustment} - {center}");
                    newChunks.Add(adjustment);
                }
            }
            // Unload extraneous columns
            lock (LoadedChunks)
            {
                var currentChunks = new List<Vector3I>(LoadedChunks);
                foreach (var chunk in currentChunks)
                {
                    if (!newChunks.Contains(chunk))
                        UnloadChunk(chunk);
                }
                // Load new columns
                foreach (var chunk in newChunks)
                {
                    if (!LoadedChunks.Contains(chunk))
                        LoadChunk(chunk);
                }
            }
            ((EntityManager)Server.GetEntityManagerForWorld(World)).UpdateClientEntities(this);
        }

        internal void UnloadAllChunks()
        {
            lock (LoadedChunks)
            {
                while (LoadedChunks.Any())
                {
                    UnloadChunk(LoadedChunks[0]);
                }
            }
        }

        internal void LoadChunk(Vector3I index)
        {
            var chunk = World.GetChunk(index);
            
            QueuePacket(new ChunkPreamblePacket(chunk.Index.X, chunk.Index.Z));
            QueuePacket(CreatePacket(chunk));
            LoadedChunks.Add(index);
            
        }

        internal void UnloadChunk(Vector3I index)
        {
            QueuePacket(new ChunkPreamblePacket(index.X, index.Y, false));
            LoadedChunks.Remove(index);
        }

        private static ChunkDataPacket CreatePacket(IChunk chunk)
        {
            var X = chunk.Index.X;
            var Z = chunk.Index.Z;

            return new ChunkDataPacket(X, Z, chunk.GetData());
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                IPacketSegmentProcessor processor;
                while (!PacketReader.Processors.TryRemove(this, out processor))
                    Thread.Sleep(1);

                Disconnect();

                m_Sem.Dispose();

                if (Disposed != null)
                    Disposed(this, null);
            }

            m_Sem = null;
        }

        ~RemoteClient()
        {
            Dispose(false);
        }
    }
}

using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
        public RemoteClient(IMultiplayerServer server, IPacketReader packetReader, PacketHandler[] packetHandlers, IPEndPoint endpoint)
        {
            LoadedChunks = new List<Vector3I>();
            Server = server;
            EndPoint = endpoint;
            KnownEntities = new List<IEntity>();
            Disconnected = false;
            HasLogging = server.EnableClientLogging;
            NextWindowID = 1;
            PacketReader = packetReader;
            PacketHandlers = packetHandlers;

            m_Cancel = new CancellationTokenSource();
            
        }

        public event EventHandler Disposed;

        /// <summary>
        /// A list of entities that this client is aware of.
        /// </summary>
        public List<IEntity> KnownEntities { get; set; }
        internal sbyte NextWindowID { get; set; }

        public string Username { get; set; }
        public bool IsLoggedIn { get; internal set; }
        public IMultiplayerServer Server { get; set; }
        public IWorld World { get; internal set; }
        public InventoryContainer Inventory { get; private set; }
        public short SelectedSlot { get; internal set; }
        public bool HasLogging { get; set; }
        public IPacket LastSuccessfulPacket { get; set; }
        public DateTime ExpectedDigComplete { get; set; }

        public long Identifier { get; private set; }
        public IPEndPoint EndPoint { get; private set; }
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

        public NetServer Client => Server.Server;

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
            if (Disconnected || Client.ConnectionsCount == 0)
                return;
            var message = Client.CreateMessage();
            PacketReader.WritePacket(message, packet);
            Client.SendMessage(message, Server.GetConnection(EndPoint), NetDeliveryMethod.ReliableOrdered, 0);
        }
        
        public void Disconnect()
        {
            if (Disconnected)
                return;
            QueuePacket(new DisconnectPacket("Disconnected by server"));
            Disconnected = true;

            m_Cancel.Cancel();
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


                Disposed?.Invoke(this, null);
            }
        }

        ~RemoteClient()
        {
            Dispose(false);
        }
    }
}

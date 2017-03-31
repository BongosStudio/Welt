using System;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using Welt.API;
using Welt.API.Net;
using Welt.API.Forge;
using Microsoft.Xna.Framework;
using Welt.API.Logging;
using Welt.Core.Net;
using Welt.Core.Forge;
using Welt.Core.Net.Packets;
using System.Runtime.CompilerServices;

namespace Welt.Core.Server
{
    public class MultiplayerServer : IMultiplayerServer, IDisposable
    {
        public event EventHandler<ChatMessageEventArgs> ChatMessageReceived;
        public event EventHandler<PlayerJoinedQuitEventArgs> PlayerJoined;
        public event EventHandler<PlayerJoinedQuitEventArgs> PlayerQuit;

        public IAccessConfiguration AccessConfiguration { get; internal set; }
        public IServerConfiguration ServerConfiguration { get; internal set; }

        public IPacketReader PacketReader { get; private set; }
        public IList<IRemoteClient> Clients { get; private set; }
        public IList<IWorld> Worlds { get; private set; }
        public IList<IEntityManager> EntityManagers { get; private set; }
        public IEventScheduler Scheduler { get; private set; }
        public IBlockRepository BlockRepository { get; private set; }
        public IItemRepository ItemRepository { get; private set; }
        public ICraftingRepository CraftingRepository { get; private set; }
        public WorldLighting WorldLighter { get; private set; }
        public bool EnableClientLogging { get; set; }
        public IPEndPoint EndPoint { get; private set; }

        private static readonly int MillisecondsPerTick = 1000 / 20;

        private bool _BlockUpdatesEnabled = true;

        private struct BlockUpdate
        {
            public Vector3I Coordinates;
            public IWorld World;
        }
        private Queue<BlockUpdate> PendingBlockUpdates { get; set; }
        public bool BlockUpdatesEnabled
        {
            get
            {
                return _BlockUpdatesEnabled;
            }
            set
            {
                _BlockUpdatesEnabled = value;
                if (_BlockUpdatesEnabled)
                {
                    ProcessBlockUpdates();
                }
            }
        }

        private Timer EnvironmentWorker;
        private TcpListener Listener;
        private readonly PacketHandler[] PacketHandlers;
        private IList<ILogProvider> LogProviders;
        private ConcurrentBag<(IWorld World, IChunk Chunk)> ChunksToSchedule;
        internal object ClientLock = new object();
        
        private QueryProtocol QueryProtocol;

        internal bool ShuttingDown { get; private set; }
        
        public MultiplayerServer()
        {
            var reader = new PacketReader();
            PacketReader = reader;
            Clients = new List<IRemoteClient>();
            EnvironmentWorker = new Timer(DoEnvironment);
            PacketHandlers = new PacketHandler[0x100];
            Worlds = new List<IWorld>();
            EntityManagers = new List<IEntityManager>();
            LogProviders = new List<ILogProvider>();
            Scheduler = new EventScheduler(this);
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
            PendingBlockUpdates = new Queue<BlockUpdate>();
            EnableClientLogging = false;
            QueryProtocol = new QueryProtocol(this);
            WorldLighter = new WorldLighting(blockRepository);
            ChunksToSchedule = new ConcurrentBag<(IWorld, IChunk)>();

            AccessConfiguration = new AccessConfiguration();
            ServerConfiguration = new ServerConfiguration();

            reader.RegisterCorePackets();
            Handlers.PacketHandlers.RegisterHandlers(this);
        }

        public void RegisterPacketHandler(byte packetId, PacketHandler handler)
        {
            PacketHandlers[packetId] = handler;
        }

        public void Start(IPEndPoint endPoint)
        {
            ShuttingDown = false;
            Listener = new TcpListener(endPoint);
            Listener.Start();
            EndPoint = (IPEndPoint)Listener.LocalEndpoint;
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += AcceptClient;

            if (!Listener.Server.AcceptAsync(args))
                AcceptClient(this, args);
            
            Log(LogCategory.Notice, $"Server started on {EndPoint}.");
            EnvironmentWorker.Change(MillisecondsPerTick, 0);
            if(ServerConfiguration.Query)
                QueryProtocol.Start();
        }

        public void Stop()
        {
            ShuttingDown = true;
            Listener.Stop();
            if(ServerConfiguration.Query)
                QueryProtocol.Stop();
            //foreach (var w in Worlds)
            //    w.Save();
            foreach (var c in Clients)
                DisconnectClient(c);
        }

        public void AddWorld(IWorld world)
        {
            Worlds.Add(world);
            world.ChunkGenerated += HandleChunkGenerated;
            world.ChunkLoaded += HandleChunkLoaded;
            world.BlockChanged += HandleBlockChanged;
            var manager = new EntityManager(this, world);
            EntityManagers.Add(manager);
            foreach (var chunk in world)
                HandleChunkLoaded(world, new ChunkLoadedEventArgs(chunk));
        }

        public void QueuePacket(IPacket packet)
        {
            foreach (var client in Clients)
            {
                client.QueuePacket(packet);
            }
        }

        void HandleChunkLoaded(object sender, ChunkLoadedEventArgs e)
        {
            ChunksToSchedule.Add((sender as IWorld, e.Chunk));
            WorldLighter.ProcessChunk(e.Chunk as Chunk);
        }

        void HandleBlockChanged(object sender, BlockChangeEventArgs e)
        {
            // TODO: Propegate lighting changes to client (not possible with beta 1.7.3 protocol)
            if (e.NewBlock.Id != e.OldBlock.Id || e.NewBlock.Metadata != e.OldBlock.Metadata)
            {
                for (int i = 0, ClientsCount = Clients.Count; i < ClientsCount; i++)
                {
                    var client = (RemoteClient)Clients[i];
                    // TODO: Confirm that the client knows of this block
                    if (client.IsLoggedIn && client.World == sender)
                    {
                        // send block change packet
                    }
                }
                PendingBlockUpdates.Enqueue(new BlockUpdate { Coordinates = e.Position, World = sender as IWorld });
                ProcessBlockUpdates();
                var chunk = (sender as IWorld)?.ChunkAt(e.Position);
                WorldLighter.ProcessChunk(chunk as Chunk);
            }
        }

        void HandleChunkGenerated(object sender, ChunkLoadedEventArgs e)
        {
            WorldLighter.ProcessChunk(e.Chunk as Chunk);
            
            HandleChunkLoaded(sender, e);
        }

        void ScheduleUpdatesForChunk(IWorld world, IChunk chunk)
        {
            var _x = chunk.Index.X * Chunk.Width;
            var _z = chunk.Index.Z * Chunk.Depth;
            Vector3 coords, _coords;
            for (byte x = 0; x < Chunk.Width; x++)
            {
                for (byte z = 0; z < Chunk.Depth; z++)
                {
                    for (int y = 0; y < chunk.GetHeight(x, z); y++)
                    {
                        _coords.X = x; _coords.Y = y; _coords.Z = z;
                        var id = chunk.GetBlock(x, (byte) y, z).Id;
                        if (id == 0)
                            continue;
                        coords.X = _x + x; coords.Y = y; coords.Z = _z + z;
                        var provider = BlockRepository.GetBlockProvider(id);
                        provider.BlockLoadedFromChunk(coords, this, world);
                    }
                }
            }
        }

        private void ProcessBlockUpdates()
        {
            if (!BlockUpdatesEnabled)
                return;
            var adjacent = new[]
            {
                Vector3.Up, Vector3.Down,
                Vector3.Left, Vector3.Right,
                Vector3.Forward, Vector3.Backward
            };
            while (PendingBlockUpdates.Count != 0)
            {
                var update = PendingBlockUpdates.Dequeue();
                var block = update.World.GetBlock(update.Coordinates);
                foreach (var offset in adjacent)
                {
                    var descriptor = update.World.GetBlockData((Vector3)update.Coordinates + offset);
                    var provider = BlockRepository.GetBlockProvider(descriptor.Id);
                    var source = update.World.GetBlockData(update.Coordinates);
                    if (provider != null)
                        provider.BlockUpdate(descriptor, source, this, update.World);
                }
            }
        }

        public void AddLogProvider(ILogProvider provider)
        {
            LogProviders.Add(provider);
        }

        public void Log(LogCategory category, string text, params object[] parameters)
        {
            for (int i = 0, LogProvidersCount = LogProviders.Count; i < LogProvidersCount; i++)
            {
                var provider = LogProviders[i];
                provider.Log(category, text, parameters);
            }
        }

        public IEntityManager GetEntityManagerForWorld(IWorld world)
        {
            for (int i = 0; i < EntityManagers.Count; i++)
            {
                var manager = EntityManagers[i] as EntityManager;
                if (manager.World == world)
                    return manager;
            }
            return null;
        }

        public void SendMessage(string message, params object[] parameters)
        {
            var compiled = string.Format(message, parameters);
            var parts = compiled.Split('\n');
            foreach (var client in Clients)
            {
                foreach (var part in parts)
                    client.SendMessage(part);
            }
            Log(LogCategory.Notice, compiled);
        }

        protected internal void OnChatMessageReceived(ChatMessageEventArgs e)
        {
            ChatMessageReceived?.Invoke(this, e);
        }

        protected internal void OnPlayerJoined(PlayerJoinedQuitEventArgs e)
        {
            PlayerJoined?.Invoke(this, e);
        }

        protected internal void OnPlayerQuit(PlayerJoinedQuitEventArgs e)
        {
            PlayerQuit?.Invoke(this, e);
        }

        public void DisconnectClient(IRemoteClient _client, [CallerMemberName] string caller = "")
        {
            var client = (RemoteClient)_client;

            lock (ClientLock)
            {
                Clients.Remove(client);
            }

            if (client.Disconnected)
                return;

            client.Disconnected = true;

            if (client.IsLoggedIn)
            {
                SendMessage($"{client.Username} has left the server. {caller}");
                GetEntityManagerForWorld(client.World).DespawnEntity(client.Entity);
                GetEntityManagerForWorld(client.World).FlushDespawns();
            }
            client.Save();
            client.Disconnect();
            OnPlayerQuit(new PlayerJoinedQuitEventArgs(client));

            client.Dispose();
        }

        private void AcceptClient(object sender, SocketAsyncEventArgs args)
        {
            try
            {
                var client = new RemoteClient(this, PacketReader, PacketHandlers, args.AcceptSocket);

                lock (ClientLock)
                    Clients.Add(client);
            }
            catch
            {
                // Who cares
            }
            finally
            {
                args.AcceptSocket = null;

                if (!ShuttingDown && !Listener.Server.AcceptAsync(args))
                    AcceptClient(this, args);
            }
        }

        private void DoEnvironment(object discarded)
        {
            if (ShuttingDown)
                return;
            

            Scheduler.Update();
            
            foreach (var manager in EntityManagers)
            {
                manager.Update();
            }
            foreach (var world in Worlds)
            {
                if (ServerConfiguration.IsRealtime)
                    world.TimeOfDay = (int)DateTime.Now.TimeOfDay.TotalSeconds;
                else
                    world.TimeOfDay += MillisecondsPerTick;

                foreach (var chunk in world)
                {
                    WorldLighter.ProcessChunk(chunk as Chunk);
                }
            }
            if (ChunksToSchedule.TryTake(out var t))
                ScheduleUpdatesForChunk(t.World, t.Chunk);

            EnvironmentWorker.Change(MillisecondsPerTick, 0);
        }

        public bool PlayerIsWhitelisted(string client)
        {
            return AccessConfiguration.Whitelist.Contains(client, StringComparer.CurrentCultureIgnoreCase);
        }

        public bool PlayerIsBlacklisted(string client)
        {
            return AccessConfiguration.Blacklist.Contains(client, StringComparer.CurrentCultureIgnoreCase);
        }

        public bool PlayerIsOp(string client)
        {
            return AccessConfiguration.Oplist.Contains(client, StringComparer.CurrentCultureIgnoreCase);
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
                Stop();
            }
        }

        ~MultiplayerServer()
        {
            Dispose(false);
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Welt.API;
using Welt.Core.Forge;
using Welt.Events.Forge;
using Welt.Extensions;
using Welt.Forge;

namespace Welt.Lighting
{
    public class LightEngine
    {
        public WeltGame Game { get; }
        public MultiplayerClient Client { get; }
        public ReadOnlyWorld World { get; }

        /// <summary>
        ///     Returns a list of active lights within the engine.
        /// </summary>
        public Light[] Lights => m_Lights.ToArray();

        private ConcurrentBag<Light> m_ToAdd;
        private ConcurrentBag<Vector3> m_ToRemove;
        private List<Light> m_Lights;

        private bool m_IsRunning;

        public LightEngine(WeltGame game, MultiplayerClient client, ReadOnlyWorld world)
        {
            Game = game;
            Client = client;
            World = world;
            m_ToAdd = new ConcurrentBag<Light>();
            m_ToRemove = new ConcurrentBag<Vector3>();
            m_Lights = new List<Light>();
            client.ChunkLoaded += HandleChunkLoaded;
            client.ChunkUnloaded += HandleChunkUnloaded;
            client.BlockChanged += HandleBlockChanged;
        }

        public void Initialize()
        {
            m_IsRunning = true;
            new Thread(() =>
            {
                while (m_IsRunning && Game.IsRunning)
                {
                    lock (m_Lights)
                    {
                        while (m_ToRemove.TryTake(out var position))
                        {
                            m_Lights.RemoveAll(l => l.Position == position);
                        }
                        while (m_ToAdd.TryTake(out var light))
                        {
                            m_Lights.Add(light);
                        }
                    }
                    // process update here
                    Thread.Sleep(50);
                }
            })
            { IsBackground = true }.Start();
        }

        public void Stop()
        {
            m_IsRunning = false;
        }

        private void HandleBlockChanged(object sender, BlockChangedEventArgs args)
        {
            var data = args.Chunk.Chunk.GetBlock(args.Position.X % 16, args.Position.Y, args.Position.Z % 16);
            var block = Client.BlockRepository.GetBlockProvider(args.Id);
            var lightLevel = block.GetLightLevel(data.Metadata);
            if (lightLevel == new Vector3B()) return;
            var light = new Light(args.Position, lightLevel.ToVector3(), 15);
            // TODO: create light variables based on block
            m_ToAdd.Add(light);
        }

        private void HandleChunkLoaded(object sender, ChunkEventArgs args)
        {
            Task.Run(() =>
            {
                for (byte x = Chunk.Width; x > 0; --x)
                {
                    for (byte z = Chunk.Depth; z > 0; --z)
                    {
                        for (byte y = Chunk.Height; y > 0; --y)
                        {
                            var data = args.Chunk.GetBlock(x, y, z);
                            var block = Client.BlockRepository.GetBlockProvider(data.Id);
                            var lightLevel = block.GetLightLevel(data.Metadata);
                            if (lightLevel == new Vector3B()) continue;
                            var light = new Light(new Vector3(x, y, z), lightLevel.ToVector3(), 15);
                            // TODO: create light variables based on block
                            m_ToAdd.Add(light);
                        }
                    }
                }
            });
        }

        private void HandleChunkUnloaded(object sender, ChunkEventArgs args)
        {
            Task.Run(() =>
            {
                foreach (var light in m_Lights.ToArray())
                {
                    if (args.Chunk.Chunk.BoundingBox.Contains(light.Position) == ContainmentType.Contains)
                        m_ToRemove.Add(light.Position);
                }
            });
        }
    }
}

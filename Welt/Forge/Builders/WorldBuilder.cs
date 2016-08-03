using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Welt.Core.Forge;
using Welt.Forge.Renderers;
using Welt.Models;
using Welt.Processors;
using Welt.Types;

namespace Welt.Forge.Builders
{
    public class WorldBuilder
    {
        public WorldBuilder(GraphicsDevice graphics, World world, IRenderer renderer)
        {
            World = world;
            Renderer = renderer;
            world.ChunkChanged += AdjustChunk;
            world.BlockChanged += AdjustBlock;

            _chunkBuilders = new ChunkBuilder[world.Size*world.Size];
            _renderingProcessor = new WorldRenderingProcessor(
                new LightingChunkProcessor(),
                new VertexBuildChunkProcessor(graphics)
                );
            Graphics = graphics;
            // TODO: determine how many IMeshBuilders are in the project and loaded plugins
        }

        public readonly World World;
        public readonly GraphicsDevice Graphics;
        public float TimeOfDay;
        public bool IsRealTime;
        public bool IsDay;
        internal IRenderer Renderer;
       
        public Vector4 Nightcolor = Color.Red.ToVector4();
        public Vector4 Suncolor = Color.White.ToVector4();
        public Vector4 Horizoncolor = Color.White.ToVector4();

        public Vector4 Eveningtint = Color.Red.ToVector4();
        public Vector4 Morningtint = Color.Gold.ToVector4();
        
        // TODO: implement build range into these fogs
        public float Fognear = 14*16;
        public float Fogfar = 16*16;

        private readonly ChunkBuilder[] _chunkBuilders;
        private readonly WorldRenderingProcessor _renderingProcessor;

        public void VisitChunks(Func<Vector3I, ChunkBuilder> visitor, uint originx, uint originz, byte radius)
        {
            //+1 is for having the player on a center chunk
            for (var x = originx - radius; x < originx + radius + 1; x++)
            {
                for (var z = originz - radius; z < originz + radius + 1; z++)
                {
                    visitor(new Vector3I(x, 0, z));
                }
            }
        }

        public ChunkBuilder GetChunkBuilder(uint x, uint z)
        {
            return _chunkBuilders[x*World.Size + z];
        }

        public void AdjustChunk(object sender, ChunkChangedEventArgs args)
        {
            switch (args.ChangedAction)
            {
                case ChunkChangedEventArgs.ChunkChangedAction.Created:
                    var builder = new ChunkBuilder(World.GetChunk(args.X, args.Z))
                    {
                        State = ChunkState.AwaitingBuild
                    };
                    _chunkBuilders[args.X*World.Size + args.Z] = builder;
                    break;
                case ChunkChangedEventArgs.ChunkChangedAction.Built:
                    // TODO:
                    break;
                case ChunkChangedEventArgs.ChunkChangedAction.Adjusted:
                    _chunkBuilders[args.X*World.Size + args.Z].State = ChunkState.AwaitingRebuild;
                    break;
                case ChunkChangedEventArgs.ChunkChangedAction.Destroyed:
                    _chunkBuilders[args.X*World.Size + args.Z].State = ChunkState.ToDelete;
                    break;
            }
        }

        public void AdjustBlock(object sender, BlockChangedEventArgs args)
        {
            var cx = args.X/Chunk.Width;
            var cz = args.Z/Chunk.Depth;
            _chunkBuilders[cx*World.Size + cz].State = ChunkState.AwaitingRebuild;
            // TODO: figure this out where only a certain block area is modified because there's no sense in 
            // rebuilding the whole chunk
        }

        public void Update(GameTime time)
        {
            // TODO
        }

        private void VerifyChunk(ChunkBuilder builder)
        {
            if (!builder.CanContinue) return;

            builder.CanContinue = false;
            switch (builder.State)
            {
                case ChunkState.AwaitingGenerate:
                    builder.State = ChunkState.Generating;
                    World.Generator.GenerateChunk(World, builder.OwnedChunk);
                    break;
                case ChunkState.Generating:
                    // we'll peek at the next awaiting chunk to see what it has to process
                    builder.State = ChunkState.AwaitingLighting;
                    break;
                case ChunkState.AwaitingLighting:
                    builder.State = ChunkState.Lighting;
                    _renderingProcessor.Process<LightingChunkProcessor>(builder);
                    break;
                case ChunkState.Lighting:
                    builder.State = ChunkState.AwaitingBuild;
                    break;
                case ChunkState.AwaitingBuild:
                    builder.State = ChunkState.Building;
                    _renderingProcessor.Process<VertexBuildChunkProcessor>(builder);          
                    break;
                case ChunkState.Building:
                    builder.State = ChunkState.Ready;
                    break;
                case ChunkState.Ready:
                    break;
                case ChunkState.AwaitingRelighting:
                    _renderingProcessor.Process<LightingChunkProcessor>(builder);
                    builder.State = ChunkState.Ready;
                    break;
                case ChunkState.AwaitingRebuild:
                    _renderingProcessor.Process(builder);
                    builder.State = ChunkState.Ready;
                    break;
                case ChunkState.ToDelete:
                    DisposeOfChunk(builder);
                    break;
                default:
                    break;
            }
            builder.CanContinue = true;
        }

        private void DisposeOfChunk(ChunkBuilder builder)
        {
            builder.Clear();
            _chunkBuilders[builder.OwnedChunk.X*World.Size + builder.OwnedChunk.Z] = null;
        }
    }
}
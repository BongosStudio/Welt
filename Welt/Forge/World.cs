#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Welt.Forge.Generators;
using Welt.Managers;
using Welt.Models;
using Welt.Persistence;
using Welt.Types;
using Welt.Forge.Renderers;
using Welt.Events.Forge;
using System.Collections.Generic;

#endregion

namespace Welt.Forge
{
    public class World
    {
        #region choose terrain generation

        //public IChunkGenerator Generator = new SimpleTerrain();
        //public IChunkGenerator Generator = new FlatReferenceTerrain();
        //public IChunkGenerator Generator = new TerrainWithCaves();
        public IChunkGenerator Generator = new DualLayerTerrainWithMediumValleysForRivers();

        // Biomes
        //public IChunkGenerator Generator = new Tundra_Alpine();
        //public IChunkGenerator Generator = new Desert_Subtropical();
        //public IChunkGenerator Generator = new Grassland_Temperate();

        #endregion

        public World(string name)
        {
            //Chunks = new Dictionary2<Chunk>();//
            Chunks = new ChunkManager(new ChunkPersistence(), this);
            Name = name;
            Seed = 54321;
            Origin = new Vector2(FastMath.NextRandom((int) Size), FastMath.NextRandom((int) Size));
        }

        public World(string name, int seed) : this(name)
        {
            Seed = seed;
        }

        public void ToggleRasterMode()
        {
            Wireframed = !Wireframed;
        }

        #region visitChunks

        public void VisitChunks(Func<Vector3I, Chunk> visitor, uint radius)
        {
            //+1 is for having the player on a center chunk
            for (var x = Origin.X - radius; x < Origin.X + radius + 1; x++)
            {
                for (var z = Origin.Y - radius; z < Origin.Y + radius + 1; z++)
                {
                    visitor(new Vector3I((uint) x, 0, (uint) z));
                    
                }
            }
        }


        public void VisitChunks(Func<Vector3I, Chunk> visitor, Vector3I center, uint radius)
        {
            for (var x = center.X - radius; x < center.X + radius + 1; x++)
            {
                for (var z = center.Z - radius; z < center.Z + radius + 1; z++)
                {
                    visitor(new Vector3I(x, 0, z));

                }
            }
        }

        #endregion

        #region InView

        public bool InView(uint x, uint y, uint z)
        {
            if (Chunks.GetChunk(x/Chunk.Size.X, 0, z/Chunk.Size.Z, false) == null)
                return false;

            var lx = x%Chunk.Size.X;
            var ly = y%Chunk.Size.Y;
            var lz = z%Chunk.Size.Z;

            return lx < Chunk.Size.X && ly < Chunk.Size.Y && lz < Chunk.Size.Z;
        }

        #endregion

        #region Fields

        public ChunkManager Chunks;
        
        /// <summary>
        ///     The name of the world.
        /// </summary>
        public string Name { get; }
        /// <summary>
        ///     The seed used for generation.
        /// </summary>
        public int Seed { get; }
        /// <summary>
        ///     The size of the world in Chunks.
        /// </summary>
        /// <remarks>
        ///     The size is a single uint value which represents both width and depth. This
        ///     means that a world may only be said amount of chunks wide and deep.
        /// </remarks>
        public uint Size { get; set; } = 256;
        /// <summary>
        ///     The <see cref="WorldType"/> of the world. Generators are bound by this value on
        ///     what may render and what sizes to adhere to.
        /// </summary>
        public WorldType WorldType { get; set; } = WorldType.BioInhabital;
        /// <summary>
        ///     The parent world of the world. The only time this value will not be null is if
        ///     the world is a moon or spacestation.
        /// </summary>
        public World ParentWorld { get; set; }
        /// <summary>
        ///     The spawn chunk of the world. 
        /// </summary>
        public Vector2 Origin;

        public readonly RasterizerState WireframedRaster = new RasterizerState
        {
            CullMode = CullMode.None,
            FillMode = FillMode.WireFrame
        };

        public readonly RasterizerState NormalRaster = new RasterizerState
        {
            CullMode = CullMode.CullCounterClockwiseFace,
            FillMode = FillMode.Solid
        };

        public bool Wireframed;

        // Day/Night
        public float Tod = 12; // Midday
        public Vector3 SunPos = new Vector3(0, 1, 0); // Directly overhead
        public bool RealTime = false;
        public bool DayMode = false;
        public bool NightMode = false;
        internal WorldRenderer Renderer;

        #region Atmospheric settings

        public Vector4 DeepWaterColor = Color.Black.ToVector4();
        public Vector4 Nightcolor = Color.Red.ToVector4();
        public Vector4 Suncolor = Color.White.ToVector4();
        public Vector4 Horizoncolor = Color.White.ToVector4();

        public Vector4 Eveningtint = Color.Red.ToVector4();
        public Vector4 Morningtint = Color.Gold.ToVector4();

        //private float _tod;
        //public bool dayMode = false;
        //public bool nightMode = false;
        public float Fognear = 14*16; //(BUILD_RANGE - 1) * 16;
        public float Fogfar = 16*16; //(BUILD_RANGE + 1) * 16;

        #endregion

        #region Events

        public event EventHandler<BlockChangedEventArgs> BlockChanged;
        public void OnBlockChanged(object sender, BlockChangedEventArgs args)
        {
            BlockChanged?.Invoke(sender, args);
        }

        #endregion

        #endregion

        #region GetBlock

        public Block GetBlock(Vector3 position)
        {
            return GetBlock((uint) position.X, (uint) position.Y, (uint) position.Z);
        }

        public Chunk ChunkAt(Vector3 position)
        {
            var x = (uint) position.X;
            var z = (uint) position.Z;

            var cx = x/Chunk.Size.X;
            var cz = z/Chunk.Size.Z;

            var at = Chunks.GetChunk(cx, 0, cz);

            return at;
        }

        public Block GetBlock(uint x, uint y, uint z)
        {
            if (!InView(x, y, z))
                return new Block(BlockType.NONE);
            //TODO blocktype.unknown ( with matrix films green symbols texture ? ) 
            var chunk = Chunks.GetChunk(x / Chunk.Size.X, 0, z / Chunk.Size.Z);
            return chunk.Blocks[x%Chunk.Size.X*Chunk.FlattenOffset + z%Chunk.Size.Z*Chunk.Size.Y + y%Chunk.Size.Y];
            //Debug.WriteLine("no block at  ({0},{1},{2}) ", x, y, z);
        }

        #endregion

        #region SetBlock

        public Block SetBlock(Vector3I pos, Block newType)
        {
            var x = pos.X;
            var y = pos.Y;
            var z = pos.Z;
            var chunk = Chunks.GetChunk(x / Chunk.Size.X, 0, z / Chunk.Size.Z);

            var localX = (byte)(x % Chunk.Size.X);
            var localY = (byte)(y % Chunk.Size.Y);
            var localZ = (byte)(z % Chunk.Size.Z);
            var old = chunk.Blocks[localX * Chunk.FlattenOffset + localZ * Chunk.Size.Y + localY];

            //chunk.SetBlock is also called by terrain generators for Y loops min max optimisation
            chunk.SetBlock(localX, localY, localZ, newType);

            //Chunk should be responsible for maintaining this
            chunk.State = ChunkState.AwaitingRelighting;
            //var affected = new List<Cardinal>();

            // use Chunk accessors
            if (localX == 0)
            {
                if (chunk.E != null) chunk.E.State = ChunkState.AwaitingRelighting;
                //affected.Add(Cardinal.E);
            }
            if (localX == Chunk.Max.X)
            {
                //viewableChunks[(x / Chunk.SIZE.X) + 1, z / Chunk.SIZE.Z].dirty = true;
                if (chunk.W != null) chunk.W.State = ChunkState.AwaitingRelighting;
                //affected.Add(Cardinal.W);
            }
            if (localZ == 0)
            {
                //viewableChunks[x / Chunk.SIZE.X, (z / Chunk.SIZE.Z) - 1].dirty = true;
                if (chunk.S != null) chunk.S.State = ChunkState.AwaitingRelighting;
                //affected.Add(Cardinal.S);
            }
            if (localZ == Chunk.Max.Z)
            {
                //viewableChunks[x / Chunk.SIZE.X, (z / Chunk.SIZE.Z) + 1].dirty = true;
                if (chunk.N != null) chunk.N.State = ChunkState.AwaitingRelighting;
                //affected.Add(Cardinal.N);
            }
            //OnBlockChanged(this, new BlockChangedEventArgs(newType.Id, new Vector3I(localX, y, localZ), chunk));
            return old;
        }

        public Maybe<Block, Exception> SetBlock(uint x, uint y, uint z, Block newType)
        {
            if (!InView(x, y, z))
                return new Maybe<Block, Exception>(new Block(),
                    new AccessViolationException("Cannot access block at this distance."));
            return Maybe<Block, Exception>.Check(() =>
            {
                var chunk = Chunks.GetChunk(x / Chunk.Size.X, 0, z / Chunk.Size.Z);

                var localX = (byte)(x % Chunk.Size.X);
                var localY = (byte)(y % Chunk.Size.Y);
                var localZ = (byte)(z % Chunk.Size.Z);
                var old = chunk.Blocks[localX*Chunk.FlattenOffset + localZ*Chunk.Size.Y + localY];

                //chunk.SetBlock is also called by terrain generators for Y loops min max optimisation
                chunk.SetBlock(localX, localY, localZ, newType);
                
                //Chunk should be responsible for maintaining this
                chunk.State = ChunkState.AwaitingRelighting;
                //var affected = new List<Cardinal>();
                
                // use Chunk accessors
                if (localX == 0)
                {
                    if (chunk.E != null) chunk.E.State = ChunkState.AwaitingRelighting;
                    //affected.Add(Cardinal.E);
                }
                if (localX == Chunk.Max.X)
                {
                    //viewableChunks[(x / Chunk.SIZE.X) + 1, z / Chunk.SIZE.Z].dirty = true;
                    if (chunk.W != null) chunk.W.State = ChunkState.AwaitingRelighting;
                    //affected.Add(Cardinal.W);
                }
                if (localZ == 0)
                {
                    //viewableChunks[x / Chunk.SIZE.X, (z / Chunk.SIZE.Z) - 1].dirty = true;
                    if (chunk.S != null) chunk.S.State = ChunkState.AwaitingRelighting;
                    //affected.Add(Cardinal.S);
                }
                if (localZ == Chunk.Max.Z)
                {
                    //viewableChunks[x / Chunk.SIZE.X, (z / Chunk.SIZE.Z) + 1].dirty = true;
                    if (chunk.N != null) chunk.N.State = ChunkState.AwaitingRelighting;
                    //affected.Add(Cardinal.N);
                }
                //OnBlockChanged(this, new BlockChangedEventArgs(newType.Id, new Vector3I(localX, y, localZ), chunk));
                return old;
            });
        }
        
        #endregion
    }
}
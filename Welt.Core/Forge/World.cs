#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Welt.API;
using Welt.API.Forge;
using Welt.Core.Forge.Generators;
using Welt.Core.Persistence;
using System.Collections;
using System.Collections.Generic;

namespace Welt.Core.Forge
{
    public class World : IWorld
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
            ChunkManager = new ChunkManager(new ChunkPersistence(), this);
            Name = name;
            Seed = 54321;
            SpawnPoint = new Vector3I(FastMath.NextRandom(Size), 128, FastMath.NextRandom(Size));
        }

        public World(string name, int seed) : this(name)
        {
            Seed = seed;
        }
        
        #region InView

        public bool InView(uint x, uint y, uint z)
        {
            if (ChunkManager.GetChunk(x / Chunk.Size.X, 0, z / Chunk.Size.Z, false) == null)
                return false;

            var lx = x % Chunk.Size.X;
            var ly = y % Chunk.Size.Y;
            var lz = z % Chunk.Size.Z;

            return lx < Chunk.Size.X && ly < Chunk.Size.Y && lz < Chunk.Size.Z;
        }

        #endregion

        #region Fields

        public ChunkManager ChunkManager;

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
        public int Size { get; set; } = 256;
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

        public Action<object, ChunkLoadedEventArgs> ChunkGenerated { get; set; }

        public Action<object, ChunkLoadedEventArgs> ChunkLoaded { get; set; }

        public Action<object, BlockChangeEventArgs> BlockChanged { get; set; }

        /// <summary>
        ///     The spawn chunk of the world. 
        /// </summary>
        public Vector3I SpawnPoint { get; set; }

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
        public int TimeOfDay { get; set; } = 1200;
        public Vector3 SunPos = new Vector3(0, 1000, 0); // Directly overhead
        public bool RealTime = false;
        public bool DayMode = false;
        public bool NightMode = false;

        #region Atmospheric settings

        public Vector4 DeepWaterColor = Color.Black.ToVector4();
        public Vector4 NightColor = Color.Red.ToVector4();
        public Vector4 SunColor = Color.White.ToVector4();
        public Vector4 HorizonColor = Color.White.ToVector4();

        public Vector4 Eveningtint = Color.Red.ToVector4();
        public Vector4 Morningtint = Color.Gold.ToVector4();

        //private float _tod;
        //public bool dayMode = false;
        //public bool nightMode = false;
        public float Fognear = 14 * 16; //(BUILD_RANGE - 1) * 16;
        public float Fogfar = 16 * 16; //(BUILD_RANGE + 1) * 16;

        #endregion

        #region Events
        
        public void OnBlockChanged(object sender, BlockChangeEventArgs args)
        {
            BlockChanged?.Invoke(sender, args);
        }

        #endregion

        public Vector3I FindBlockPosition(Vector3I worldCoords, out IChunk chunk, bool generate = true)
        {
            var x = worldCoords.X;
            var z = worldCoords.Z;

            var cx = x / Chunk.Size.X;
            var cz = z / Chunk.Size.Z;

            var at = ChunkManager.GetChunk(cx, 0, cz, generate);

            chunk = at;
            return new Vector3I(cx, worldCoords.Y, cz);
        }

        #endregion

        public IChunk GetChunk(Vector3I position)
        {
            return ChunkManager.GetChunk(position);
        }

        public void SetChunk(Vector3I position, IChunk value)
        {
            ChunkManager.SetChunk(position, value as Chunk);
        }

        #region GetBlock

        public BlockDescriptor GetBlockData(Vector3I position)
        {
            var d = BlockDescriptor.FromBlock(GetBlock(position));
            d.Position = position;
            d.Chunk = ChunkAt(position);
            return d;
        }

        public Block GetBlock(Vector3I position)
        {
            return GetBlock(position.X, position.Y, position.Z);
        }

        public IChunk ChunkAt(Vector3I position, bool generate = false)
        {
            var x = position.X;
            var z = position.Z;

            var cx = x / Chunk.Size.X;
            var cz = z / Chunk.Size.Z;

            var at = ChunkManager.GetChunk(cx, 0, cz, generate);

            return at;
        }

        public Block GetBlock(uint x, uint y, uint z)
        {
            if (!InView(x, y, z))
                return new Block(BlockType.NONE);
            //TODO blocktype.unknown ( with matrix films green symbols texture ? ) 
            var chunk = ChunkManager.GetChunk(x / Chunk.Size.X, 0, z / Chunk.Size.Z);
            return chunk.Blocks[x % Chunk.Size.X * Chunk.FlattenOffset + z % Chunk.Size.Z * Chunk.Size.Y + y % Chunk.Size.Y];
            //Debug.WriteLine("no block at  ({0},{1},{2}) ", x, y, z);
        }

        #endregion

        #region SetBlock

        public Block SetBlock(Vector3I pos, Block newType)
        {
            var x = pos.X;
            var y = pos.Y;
            var z = pos.Z;
            var chunk = ChunkManager.GetChunk(x / Chunk.Size.X, 0, z / Chunk.Size.Z);

            var localX = (byte)(x % Chunk.Size.X);
            var localY = (byte)(y % Chunk.Size.Y);
            var localZ = (byte)(z % Chunk.Size.Z);
            var old = chunk.Blocks[localX * Chunk.FlattenOffset + localZ * Chunk.Size.Y + localY];
            var oldD = GetBlockData(pos);
            //chunk.SetBlock is also called by terrain generators for Y loops min max optimisation
            chunk.SetBlock(localX, localY, localZ, newType);

            var newD = GetBlockData(pos);
            OnBlockChanged(this, new BlockChangeEventArgs(pos, oldD, newD));
            return old;
        }

        public IEnumerator<IChunk> GetEnumerator()
        {
            return ChunkManager.Chunks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public bool IsValidPosition(Vector3I position)
        {
            return position.Y <= Chunk.Max.Y;
        }
    }
}
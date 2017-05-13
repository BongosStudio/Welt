using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API;
using Welt.API.Forge;
using Welt.Core.Forge.BlockProviders;
using Welt.Core.Forge.Noise;

namespace Welt.Core.Forge.Biomes
{
    public class BiomeSystem : IBiomeSystem
    {
        public World World { get; set; }

        public BiomeSystem(World world)
        {
            World = world;
            HighNoise.Persistance = 1;
            HighNoise.Frequency = 0.034;
            HighNoise.Amplitude = 35;
            HighNoise.Octaves = 3;
            HighNoise.Lacunarity = 2;

            MidNoise.Persistance = 0.3;
            MidNoise.Frequency = 0.045;
            MidNoise.Amplitude = 15;
            MidNoise.Octaves = 1;
            MidNoise.Lacunarity = 5;

            LowNoise.Persistance = 1;
            LowNoise.Frequency = 0.004;
            LowNoise.Amplitude = 15;
            LowNoise.Octaves = 2;
            LowNoise.Lacunarity = 2.5;

            BottomNoise.Persistance = 0.5;
            BottomNoise.Frequency = 0.013;
            BottomNoise.Amplitude = 5;
            BottomNoise.Octaves = 2;
            BottomNoise.Lacunarity = 1.5;

            HighClamp = new ClampNoise(HighNoise)
            {
                MinValue = -30,
                MaxValue = 80
            };
            LowClamp = new ClampNoise(LowNoise)
            {
                MinValue = -30,
                MaxValue = 45
            };
            BottomClamp = new ClampNoise(BottomNoise)
            {
                MinValue = -20,
                MaxValue = 5
            };
            FinalNoise = new ModifyNoise(HighClamp, LowClamp, NoiseModifier.Add);

        }

        private struct BiomeGenerator
        {
            public IBiome Biome;
            public ITerrainGenerator Terrain;
            public IChunkDecorationGenerator Decorator;

            public BiomeGenerator(IBiome biome, ITerrainGenerator terrain, IChunkDecorationGenerator decor)
            {
                Biome = biome;
                Terrain = terrain;
                Decorator = decor;
            }
        }

        private Dictionary<Type, BiomeGenerator> m_RegisteredBiomes = new Dictionary<Type, BiomeGenerator>(64);

        public IChunk GenerateChunk(Vector3I index)
        {
            var chunk = new Chunk(World, index);

            int seed = (int)World.Seed;
            HighNoise.Seed = seed;
            LowNoise.Seed = seed;
            CaveNoise.Seed = seed;

            for (byte x = 0; x < Chunk.Width; x++)
            {
                for (byte z = 0; z < Chunk.Depth; z++)
                {
                    var worldX = (int)(chunk.Position.X + x);
                    var worldZ = (int)(chunk.Position.Z + z);
                    var biome = GetBiome(worldX, worldZ);
                    var height = GetHeight(worldX, worldZ);
#if DEBUG

                    //height = FastMath.NextRandom(60, 80);
#endif
                    chunk.HeightMap[x * Chunk.Width + z] = height;
                    // get the depth of that layer
                    var surfaceDepth = height - FastMath.NextRandom(biome.Terrain.SurfaceDepthMin, biome.Terrain.SurfaceDepthMax);
                    var topBlock = new Block(biome.Biome.TopBlockId, biome.Biome.TopBlockMetadata);
                    var surfaceBlock = new Block(biome.Biome.SurfaceBlockId, biome.Biome.SurfaceBlockMetadata);
                    var subBlock = new Block(biome.Biome.SublayerBlockId, biome.Biome.SublayerBlockMetadata);
                    var comp = height < World.WaterLevel ? World.WaterLevel : height;
                    

                    for (byte y = 0; y < comp; y++)
                    {
                        
                        if (y > height && y <= World.WaterLevel)
                        {
                            chunk.SetBlock(x, y, z, new Block(BlockType.WATER));
                            continue;
                        }
                        if (y <= World.WaterLevel)
                        {
                            if (y >= surfaceDepth)
                            {
                                chunk.SetBlock(x, y, z, new Block(BlockType.SAND));
                            }
                            else
                            {
                                chunk.SetBlock(x, y, z, subBlock);
                            }
                        }
                        else
                        {
                            var interpol = MidNoise.Interpolated3D(worldX, y, worldZ);
                            if (interpol > .25 && interpol < .3)
                                continue;
                            if (y == height - 1)
                            {
                                chunk.SetBlock(x, y, z, topBlock);
                            }
                            else if (y >= surfaceDepth)
                            {
                                chunk.SetBlock(x, y, z, surfaceBlock);
                            }
                            else
                            {
                                chunk.SetBlock(x, y, z, subBlock);
                            }
                        }
                    }
                }
            }
            return chunk;
        }

        public void RegisterBiome<TBiome>(ITerrainGenerator terrain, IChunkDecorationGenerator decorator) where TBiome : IBiome
        {
            if (m_RegisteredBiomes.ContainsKey(typeof(TBiome)))
                throw new ArgumentException($"{typeof(TBiome)} is already registered", nameof(TBiome));
            m_RegisteredBiomes.Add(typeof(TBiome), new BiomeGenerator(Activator.CreateInstance<TBiome>(), terrain, decorator));

        }

        #region Generation

        PerlinNoise HighNoise = new PerlinNoise();

        PerlinNoise MidNoise = new PerlinNoise();

        PerlinNoise LowNoise = new PerlinNoise();

        PerlinNoise BottomNoise = new PerlinNoise();

        PerlinNoise CaveNoise = new PerlinNoise();

        ClampNoise HighClamp;

        ClampNoise LowClamp;

        ClampNoise BottomClamp;

        ModifyNoise FinalNoise;

        bool EnableCaves;

        byte GetHeight(int x, int z)
        {
            var noise = (FinalNoise.Value2D(x, z) + World.WaterLevel) * .75;
            if (noise < 0)
                noise = World.WaterLevel;
            if (noise > Chunk.Height)
                noise = Chunk.Height - 1;
            return (byte)noise;
        }

        BiomeGenerator GetBiome(int x, int z)
        {
            return m_RegisteredBiomes.First().Value;
        }

        #endregion
    }
}

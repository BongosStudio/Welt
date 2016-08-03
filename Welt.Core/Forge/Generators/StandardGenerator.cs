#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Welt.API.Forge;
using Welt.API.Forge.Generators;

namespace Welt.Core.Forge.Generators
{
    public class StandardGenerator : IForgeGenerator
    {
        private static double[] _values;

        public string LevelType => "DEFAULT";
        public string GeneratorOptions { get; }
        public int SpawnX { get; set; }
        public int SpawnY { get; set; }
        public int SpawnZ { get; set; }

        public void Initialize(IWorld world)
        {
            SpawnX = FastMath.NextRandom(world.Size*16);
            SpawnZ = FastMath.NextRandom(world.Size*16);
            SpawnY = 256; // TODO: get the height from the world and use that
        }

        public void GenerateChunk(IWorld world, IChunk chunk)
        {
            chunk.Initialize(world);

            var featureSize = 16;
            var sampleSize = 16;
            var stepSize = 16;
            var scale = 1d;

            _values = new double[stepSize*stepSize];
            for (var y = 0; y < featureSize; y++)
            {
                for (var x = 0; x < featureSize; x++)
                {
                    SetSample(x, y, featureSize, Frand());
                }
            }
            while (sampleSize > 1)
            {
                DiamondSquare(stepSize, scale);
                sampleSize /= 2;
                scale /= 2.0;
            }

            for (var z = 0; z < featureSize; z++)
            {
                for (var x = 0; x < featureSize; x++)
                {
                    var y = (int) (Sample(x, z, featureSize)*255);
                    FastMath.Adjust(100, 150, ref y);
                    chunk.SetBlock(x, y, z, new Block(BlockType.Grass));
                }
            }
            chunk.IsGenerated = true;
            // TODO: get neighboring chunks and set those.
        }

        private void DiamondSquare(int stepsize, double scale)
        {
            var halfstep = stepsize / 2;

            for (var y = halfstep; y < 128 + halfstep; y += stepsize)
            {
                for (var x = halfstep; x < 128 + halfstep; x += stepsize)
                {
                    SampleSquare(x, y, stepsize, Frand() * scale);
                }
            }

            for (var y = 0; y < 128; y += stepsize)
            {
                for (var x = 0; x < 128; x += stepsize)
                {
                    SampleDiamond(x + halfstep, y, stepsize, Frand() * scale);
                    SampleDiamond(x, y + halfstep, stepsize, Frand() * scale);
                }
            }
        }

        private void SampleSquare(int x, int y, int size, double value)
        {
            var hs = size / 2;
            var a = Sample(x - hs, y - hs, size);
            var b = Sample(x + hs, y - hs, size);
            var c = Sample(x - hs, y + hs, size);
            var d = Sample(x + hs, y + hs, size);

            SetSample(x, y, size, ((a + b + c + d) / 4.0) + value);
        }

        private void SampleDiamond(int x, int y, int size, double value)
        {
            var hs = size / 2;
            var a = Sample(x - hs, y, size);
            var b = Sample(x + hs, y, size);
            var c = Sample(x, y - hs, size);
            var d = Sample(x, y + hs, size);

            SetSample(x, y, size, ((a + b + c + d) / 4.0) + value);
        }

        private double Sample(int x, int y, int size)
        {
            return _values[(x & (size - 1)) + (y & (size - 1)) * size];
        }

        private void SetSample(int x, int y, int size, double value)
        {
            _values[(x & (size - 1)) + (y & (size - 1)) * size] = value;
        }

        private static double Frand()
        {
            var b = FastMath.NextRandom(2) == 1;
            var d = FastMath.NextRandomDouble();
            if (b) d = -d;
            return d;
        }
    }
}
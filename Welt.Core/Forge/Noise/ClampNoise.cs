using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.Core.Forge.Noise
{
    public class ClampNoise : NoiseGen
    {
        public INoise Noise { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }

        public ClampNoise(INoise noise)
        {
            Noise = noise;
            MinValue = 0;
            MaxValue = 1;
        }

        public override double Value2D(double x, double y)
        {
            var noise = Noise.Value2D(x, y);
            if (noise < MinValue)
                noise = MinValue;
            if (noise > MaxValue)
                noise = MaxValue;
            return noise;
        }

        public override double Value3D(double x, double y, double z)
        {
            var noise = Noise.Value3D(x, y, z);
            if (noise < MinValue)
                noise = MinValue;
            if (noise > MaxValue)
                noise = MaxValue;
            return noise;
        }
    }

}

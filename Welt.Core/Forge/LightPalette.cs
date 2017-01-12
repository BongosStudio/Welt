using System.Collections.Generic;
using Welt.API;
using Welt.API.Forge;

namespace Welt.Core.Forge
{
    public class LightPalette : ILightPalette
    {
        private bool _isDirty;
        private Dictionary<LightVector, LightStruct> _values;
        // this is most likely not very efficient. But for now, it works.

        public LightPalette()
        {
            _values = new Dictionary<LightVector, LightStruct>();
        }

        public LightStruct GetLightAt(int x, int y, int z)
        {
            throw new System.NotImplementedException();
        }

        public void SetLightAt(int x, int y, int z, LightStruct value)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     Fills the palette with default light values provided in Casl.
        /// </summary>
        public void FillPalette()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     Updates the lighting with the palette, attenuating and propagating the light.
        /// </summary>
        public void Update()
        {
            throw new System.NotImplementedException();
        }

        private bool IsPositionAnEmitter(int x, int y, int z)
        {
            return _values.ContainsKey(new LightVector(x, y, z));
        }

        private class LightVector
        {
            public int X { get; }
            public int Y { get; }
            public int Z { get; }

            public LightVector(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }
    }
}
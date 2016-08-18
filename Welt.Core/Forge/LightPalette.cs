using Welt.API;
using Welt.API.Forge;

namespace Welt.Core.Forge
{
    public class LightPalette : ILightPalette
    {


        public static LightStruct ConvertIntToLight(int value)
        {
            var r = value & 0xF;
            var g = value >> 4 & 0xF;
            var b = value >> 8 & 0xF;
            return new LightStruct(r, g, b);
        }

        public static int ConvertLightToInt(LightStruct value)
        {
            var lite = 0;
            lite |= value.R;
            lite |= (ushort) (value.G << 4);
            lite |= (ushort) (value.B << 8);
            return lite;
        }
    }
}
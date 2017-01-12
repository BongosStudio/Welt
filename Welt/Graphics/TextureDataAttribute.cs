using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.Graphics
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class TextureDataAttribute : Attribute
    {
        public string TextureName;
        public TextureFace Face;

        public TextureDataAttribute(string name)
        {
            TextureName = name;
            Face = TextureFace.All;
        }
    }

    public enum TextureFace
    {
        XIncreasing,
        XDecreasing,
        YIncreasing,
        YDecreasing,
        ZIncreasing,
        ZDecreasing,
        X,
        Y,
        Z,
        Sides,
        All
    }
}

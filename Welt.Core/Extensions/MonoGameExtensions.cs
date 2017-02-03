using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.Core.Extensions
{
    public static class MonoGameExtensions
    {
        public static BoundingBox OffsetBy(this BoundingBox boundingBox, Vector3 value)
        {
            return new BoundingBox(boundingBox.Min + value, boundingBox.Max + value);
        }

        public static float DistanceTo(this Vector3 v1, Vector3 v2)
        {
            return Vector3.Distance(v1, v2);
        }

        public static Vector3 Transform(this Vector3 v1, Matrix m)
        {
            return Vector3.Transform(v1, m);
        }
    }
}

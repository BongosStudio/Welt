using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.Lighting
{
    public struct Light
    {
        public Vector3 Position;
        public Vector3 Color;
        public LightType Type;
        public Vector3? Direction;
        public float Intensity;

        /// <summary>
        ///     Constructor for creating a directional light.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="direction"></param>
        /// <param name="intensity"></param>
        public Light(Vector3 position, Vector3 color, Vector3 direction, float intensity)
        {
            Position = position;
            Color = color;
            Direction = direction;
            Intensity = intensity;
            Type = LightType.Directional;
        }

        /// <summary>
        ///     Constructor for creating a point light.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="intensity"></param>
        public Light(Vector3 position, Vector3 color, float intensity)
        {
            Position = position;
            Color = color;
            Intensity = intensity;
            Type = LightType.Point;
            Direction = null;
        }
    }
}

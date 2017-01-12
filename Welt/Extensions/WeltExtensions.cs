using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Welt.Types;

namespace Welt.Extensions
{
    public static class WeltExtensions
    {
        /// <summary>
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringEnumAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringEnumAttribute), false) as StringEnumAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].String : null;
        }

        public static Vector3 ToVector3(this Vector3B v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }

        public static Vector3 Floor(this Vector3 v)
        {
            return new Vector3((float) Math.Floor(v.X), (float) Math.Floor(v.Y), (float) Math.Floor(v.Z));
        }
    }
}

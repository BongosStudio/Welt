using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeltScript.Objects
{
    public abstract class BaseObject<T> : BaseObject
    {
        /// <summary>
        ///     Gets the value type of the object.
        /// </summary>
        public Type ValueType { get; }

        protected BaseObject()
        {
            ValueType = typeof(T);
        }
    }

    public abstract class BaseObject
    {
        /// <summary>
        ///     Gets the identifier of the object. i.e.: string, bool, int
        /// </summary>
        public abstract string Identifier { get; }

        /// <summary>
        ///     Gets or sets the value of the object.
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        ///     Gets or sets the name of the object.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///     Gets or sets the flags of the object. 
        /// </summary>
        /// <example>
        /// var someObject = WsGC.Mark(new WsBool(false), "someBool", ModifierTypes.IsStatic | ModifierTypes.IsPrivate);
        /// </example>
        public byte Flags { get; set; }
        /// <summary>
        ///     Gets or sets the updates since it was last used. If it passes a certain thresshold, it will be moved tiers.
        /// </summary>
        public int Cycle { get; set; }
    }
}

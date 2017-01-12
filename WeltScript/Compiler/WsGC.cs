using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeltScript.Objects;

namespace WeltScript.Compiler
{
    public static class WsGC
    {
        private static Stack<BaseObject> _tier1 = new Stack<BaseObject>();
        private static Stack<BaseObject> _tier2 = new Stack<BaseObject>();
        private static int THRESHHOLD = 360932; // honestly just chose a random fucking number. Will adjust with debugging.

        /// <summary>
        ///     Marks the object onto the stack.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <param name="name"></param>
        /// <param name="modifierTypes"></param>
        public static void Mark<T>(BaseObject<T> @object, string name, byte modifierTypes)
        {
            @object.Name = name;
            @object.Flags = modifierTypes;
            _tier1.Push(@object);
        }

        /// <summary>
        ///     Marks all static values from the Script onto the stack.
        /// </summary>
        /// <param name="system"></param>
        public static void Mark(Script script)
        {

        }
    }
}

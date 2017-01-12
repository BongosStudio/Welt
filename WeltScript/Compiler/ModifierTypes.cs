using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeltScript.Compiler
{
    [Flags]
    public enum ModifierTypes : byte
    {
        Public = 1 << 0,
        Internal = 1 << 1,
        Protected = 1 << 2,
        Private = 1 << 3,
        IsStatic = 1 << 4,
        IsAbstract = 1 << 5,
        IsFunc = 1 << 6
    }
}

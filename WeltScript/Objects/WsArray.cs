using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeltScript.Objects
{
    public class WsArray<T> : BaseObject<IEnumerable<T>> where T : BaseObject<T>
    {
        public override string Identifier => "[]";
    }
}

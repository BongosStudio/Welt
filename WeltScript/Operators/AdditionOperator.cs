using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeltScript.Operators
{
    public class AdditionOperator : IOperator
    {
        public string Identifier => "+";
    }

    public class ShortAdditionOperator : IOperator
    {
        public string Identifier => "+=";
    }
}

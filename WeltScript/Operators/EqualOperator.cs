using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeltScript.Operators
{
    public class EqualOperator : IOperator
    {
        public string Identifier => "==";
    }

    public class NotEqualOperator : IOperator
    {
        public string Identifier => "!=";
    }
}

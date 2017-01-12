using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeltScript.Operators
{
    public class SubtractionOperator : IOperator
    {
        public string Identifier => "-";
    }

    public class ShortSubstractionOperator : IOperator
    {
        public string Identifier => "-=";
    }
}

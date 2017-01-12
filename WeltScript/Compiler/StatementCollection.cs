using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeltScript.Statements;

namespace WeltScript.Compiler
{
    public class StatementCollection : Queue<IScriptStatement>
    {
        public static IEnumerator<IScriptStatement> Run(ScriptSystem system)
        {
            return null;
        }
    }
}

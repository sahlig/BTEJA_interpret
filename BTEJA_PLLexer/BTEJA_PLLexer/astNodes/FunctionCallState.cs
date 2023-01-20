using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTEJA_PLLexer.astNodes
{
    public class FunctionCallState : Statement
    {
        public Expr? Func { get; set; }

        public override object? State(AST program, Function? func)
        {
            return Func.Express(program, func);
        }
    }
}

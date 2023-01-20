using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTEJA_PLLexer.astNodes
{
    public class Return : Statement
    {
        public Expr? Expression { get; set; }

        public override object? State(AST program, Function? func)
        {
            if(Expression != null)
            {
                return Expression.Express(program, func);
            }
            else
            {
                throw new Exception("Invalid return statement");
            }
        }
    }
}

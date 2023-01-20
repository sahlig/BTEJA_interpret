using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTEJA_PLLexer.astNodes
{
    public class IfExpression : Expr
    {
        public string? Comparison { get; set; }
        public Expr? LeftExpr { get; set; }
        public Expr? RightExpr { get; set; }

        public override object? Express(AST program, Function? func)
        {
            throw new NotImplementedException();
        }
    }
}

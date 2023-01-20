using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTEJA_PLLexer.astNodes
{
    public abstract class Expr
    {
        public abstract object? Express(AST program, Function? func);
    }
}

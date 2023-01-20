using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTEJA_PLLexer.astNodes
{
    public class Variable : Statement
    {
        public VarType? DataType { get; set; }
        public string? Name { get; set; }
        public Expr? Value { get; set; }

        public Variable(VarType type, string nam)
        {
            DataType = type;
            Name = nam;
        }

        public Variable()
        {

        }

        public override object? State(AST program, Function? func)
        {
            if(Value == null)
            {
                return null;
            }
            else
            {
                return Value.Express(program, func);
            }
        }
    }

}

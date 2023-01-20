using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTEJA_PLLexer.astNodes
{
    public class Literal : Expr
    {
        public VarType? DataType { get; set; }
        public string? Value { get; set; }

        public Literal(VarType type, string val)
        {
            DataType = type;
            Value = val;
        }

        public override object? Express(AST program, Function? func)
        {
            if(Value != null)
            {
                if (DataType == VarType.TypeString)
                {
                    return Value;
                }else if (DataType == VarType.TypeDouble)
                {
                    return Double.Parse(Value);
                }else if (DataType == VarType.TypeInteger)
                {
                    return int.Parse(Value);
                }
                else
                {
                    throw new Exception("Unknown Error inside Literal");
                }
            }
            else
            {
                throw new Exception("Literal has no value");
            }
        }
    }
}

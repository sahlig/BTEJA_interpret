using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTEJA_PLLexer.astNodes
{
    public class VariableCall : Expr
    {
        public string? Name { get; set; }

        public VariableCall(string nam)
        {
            Name = nam;
        }

        public override object? Express(AST program, Function? func)
        {
            Variable tempVar = program.findVariable(Name);
            if(tempVar != null)
            {
                return tempVar.State(program, func);
            }
            if(func != null)
            {
                tempVar = func.findVariableFunc(Name);
                if(tempVar != null)
                {
                    return tempVar.State(program, func);
                }
            }
            throw new Exception("Unknown variable " + Name + " was called");
        }
    }
}

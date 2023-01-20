using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTEJA_PLLexer.astNodes
{
    public class VariableEq : Statement
    {
        public string? VariableName { get; set; }
        public Expr? Expression { get; set; }

        public override object? State(AST program, Function? func)
        {
            Variable tempVar = program.findVariable(VariableName);
            if (tempVar != null)
            {
                tempVar.Value = Expression;
                return tempVar;
            }
            if (func != null)
            {
                tempVar = func.findVariableFunc(VariableName);
                if (tempVar != null)
                {
                    tempVar.Value = Expression;
                    return tempVar;
                }
            }
            throw new Exception("Unknown variable " + VariableName + " was referenced");
        }
    }
}

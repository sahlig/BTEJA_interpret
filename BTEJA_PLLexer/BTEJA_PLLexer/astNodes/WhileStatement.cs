using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTEJA_PLLexer.astNodes
{
    public class WhileStatement : Statement
    {
        public List<Statement> InsideStatements { get; set; }
        public List<Statement> LocalVariables { get; set; }
        public CompExpr? WhileExpression { get; set; }

        public WhileStatement()
        {
            InsideStatements = new List<Statement>();
            LocalVariables = new List<Statement>();
        }

        public override object? State(AST program, Function? func)
        {
            while ((Boolean)WhileExpression.Express(program, func))
            {
                foreach (Statement s in InsideStatements)
                {
                    s.State(program, func);
                }
            }
            return null;
        }
    }
}

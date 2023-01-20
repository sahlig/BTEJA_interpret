using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTEJA_PLLexer.astNodes
{
    public class Function : Statement
    {
        public string? Name { get; set; }
        public VarType? ReturnType { get; set; }
        public List<Variable> Parameters { get; set; }
        public List<Statement> InsideStatements { get; set; }
        public List<Statement> LocalVariables { get; set; }

        public Function()
        {
            Parameters = new List<Variable>();
            InsideStatements = new List<Statement>();
            LocalVariables = new List<Statement>();
        }

        public Variable findVariableFunc(string nam)
        {
            foreach (Variable v in LocalVariables)
            {
                if (v.Name.Equals(nam))
                {
                    return v;
                }
            }
            foreach(Variable v in Parameters)
            {
                if(v.Name == nam)
                {
                    return v;
                }
            }
            return null;
        }

        public override object? State(AST program, Function? func)
        {
            Return ret = new Return();
            Boolean hasReturn = false;
            if (ReturnType != null)
            {
                foreach (Statement s in InsideStatements)
                {
                    if(s is Return)
                    {
                        hasReturn = true;
                        ret = (Return)s;
                    }
                }
                if (!hasReturn)
                {
                    throw new Exception("Function " + Name + " needs to return something");
                }
            }
            foreach (Statement s in InsideStatements)
            {
                if(s is Return)
                {
                    return s.State(program, this);
                }
                else
                {
                    s.State(program, this);
                }
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTEJA_PLLexer.astNodes;

namespace BTEJA_PLLexer
{
    public class AST
    {
        public List<Statement> Statements { get; set; }
        public List<Statement> Variables { get; set; }

        public AST()
        {
            Statements = new List<Statement>();
            Variables = new List<Statement>();
        }

        public void Run()
        {
            foreach(Statement s in Statements)
            {
                if(s is Function)
                {
                    continue;
                }
                s.State(this, null);
                //Console.WriteLine("Stated: " + s.ToString());
            }
        }

        public Variable? findVariable(string nam)
        {
            foreach(Variable v in Variables)
            {
                if(v.Name == nam)
                {
                    return v;
                }
            }
            return null;
        }

        public Function? findFunction(string nam)
        {
            foreach(Statement s in Statements)
            {
                if(s is Function)
                {
                    if(((Function)s).Name == nam)
                    {
                        return (Function)s;
                    }
                }
            }
            return null;
        }
    }
}

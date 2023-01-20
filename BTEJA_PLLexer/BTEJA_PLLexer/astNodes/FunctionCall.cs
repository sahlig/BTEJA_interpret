using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTEJA_PLLexer.astNodes
{
    public class FunctionCall : Expr
    {
        public string? Name { get; set; }
        public List<Expr> Parameters { get; set; }

        public FunctionCall()
        {
            Parameters = new List<Expr>();
        }

        private Literal GetLiteral(VarType? type)
        {
            Console.Write("Type input: ");
            string s = Console.ReadLine();
            if(type == VarType.TypeDouble)
            {
                try
                {
                    Double.Parse(s);
                    if (type != VarType.TypeDouble)
                    {
                        throw new Exception("Wrong data type given, should be Double");
                    }
                    return new Literal(VarType.TypeDouble, s);
                }
                catch (Exception ex)
                {

                }
            }else if(type == VarType.TypeInteger)
            {
                try
                {
                    int.Parse(s);
                    if (type != VarType.TypeInteger)
                    {
                        throw new Exception("Wrong data type given, should be Integer");
                    }
                    return new Literal(VarType.TypeInteger, s);
                }
                catch (Exception ex)
                {

                }
            }else if(type == VarType.TypeString)
            {
                return new Literal(VarType.TypeString, s);
            }
            else
            {
                throw new Exception("Unknown type given");
            }
            throw new Exception("Unknown error in scan function");
        }

        public override object? Express(AST program, Function? func)
        {
            if(Name == "print")
            {
                if(Parameters.Count != 0 && Parameters.Count <= 1)
                {
                    Console.WriteLine(Parameters[0].Express(program, func));
                    return null;
                }
                else
                {
                    throw new Exception("Print function needs 1 parameter");
                }
            }else if(Name == "scan")
            {
                if (Parameters.Count != 0 && Parameters.Count <= 1)
                {
                    if(func == null)
                    {
                        Variable v = program.findVariable(((VariableCall)Parameters[0]).Name);
                        if(v != null)
                        {
                            v.Value = GetLiteral(v.DataType);
                        }
                    }
                    else
                    {
                        Variable v = func.findVariableFunc(((VariableCall)Parameters[0]).Name);
                        if( v == null)
                        {
                            v = program.findVariable(((VariableCall)Parameters[0]).Name);
                            if(v != null)
                            {
                                v.Value = GetLiteral(v.DataType);
                            }
                        }else
                        {
                            v.Value = GetLiteral(v.DataType);
                        }
                    }
                    return null;
                }
                else
                {
                    throw new Exception("Scan function needs 1 parameter");
                }
            }
            else
            {
                int i = 0;
                Function tempfun = program.findFunction(Name);
                foreach(Literal l in Parameters)
                {
                    tempfun.Parameters[i].Value = l;
                    i++;
                }
                return tempfun.State(program, func);
            }
        }
    }
}

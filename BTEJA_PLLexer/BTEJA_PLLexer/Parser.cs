using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTEJA_PLLexer.astNodes;

namespace BTEJA_PLLexer
{
    public class Parser
    {
        private List<Token> tokens;
        private int currPos;
        private AST prog;

        public Parser(List<Token> toks)
        {
            tokens = toks;
            currPos = 0;
            prog = new AST();
        }

        private Token Show()
        {
            return tokens[currPos];
        }

        private Token Show(int i)
        {
            return tokens[currPos + i];
        }

        private Token Next()
        {
            currPos++;
            return tokens[currPos - 1];
        }

        private void Back()
        {
            currPos--;
        }


        public AST Parse()
        {
            while (currPos < tokens.Count)
            {
                prog.Statements.Add(GetState(prog.Variables));
            }

            return prog;
        }

        private Statement GetState(List<Statement> vars)
        {
            Token? currTok = null;
            currTok = Show();
            Statement? state = null;

            switch (currTok.TokType)
            {
                case TokenType.tok_var:
                    Next();
                    state = GetVarState(vars);
                    break;
                case TokenType.tok_ident:
                    state = GetVarState(vars);
                    break;
                case TokenType.tok_if:
                    Next();
                    state = new IfStatement();
                    state = FillIfWhile(state);
                    break;
                case TokenType.tok_function:
                    Next();
                    state = new Function();
                    state = GetFunction(state);
                    break;
                case TokenType.tok_return:
                    state = new Return();
                    if(Show(1).TokType == TokenType.tok_ident)
                    {
                        Next();
                        ((Return)state).Expression = GetVarExpr(); 
                    }
                    break;
                default:
                    Console.Write(Show());
                    throw new Exception("Unknown Error");
            }
            return state;
        }

        private void CheckForSemicolon()
        {
            if(Show().TokType != TokenType.tok_semicol)
            {
                throw new Exception("Expected a semicolon");
            }
            Next();
        }

        private Statement GetFunction(Statement statem)
        {
            Statement tempFun = statem;
            if(Show().TokType == TokenType.tok_ident)
            {
                ((Function)tempFun).Name = Next().Value;
                if(Show().TokType == TokenType.tok_leftpar)
                {
                    Next();
                    while (Show().TokType != TokenType.tok_rightpar)
                    {
                        if (Show().TokType == TokenType.tok_ident)
                        {
                            Variable tempVar = new Variable();
                            tempVar.Name = Next().Value;
                            if (Show().TokType == TokenType.tok_colon)
                            {
                                Next();
                                if(Show().TokType == TokenType.tok_string)
                                {
                                    tempVar.DataType = VarType.TypeString;
                                    ((Function)tempFun).Parameters.Add(tempVar);
                                }
                                else if (Show().TokType == TokenType.tok_double)
                                {
                                    tempVar.DataType = VarType.TypeDouble;
                                    ((Function)tempFun).Parameters.Add(tempVar);
                                }
                                else if (Show().TokType == TokenType.tok_integer)
                                {
                                    tempVar.DataType = VarType.TypeInteger;
                                    ((Function)tempFun).Parameters.Add(tempVar);
                                }
                            }
                            else
                            {
                                throw new Exception("Expected a colon");
                            }
                            Next();
                            if(Show().TokType == TokenType.tok_comma)
                            {
                                Next();
                                continue;
                            }else if(Show().TokType == TokenType.tok_rightpar)
                            {
                                continue;
                            }
                            else
                            {
                                throw new Exception("Expected a comma or closing parenthesis");
                            }
                        }
                        else
                        {
                            throw new Exception("Expected an identifier");
                        }
                    }
                    Next();
                    if(Show().TokType == TokenType.tok_colon)
                    {
                        Next();
                        if (Show().TokType == TokenType.tok_string)
                        {
                            ((Function)tempFun).ReturnType = VarType.TypeString;
                        }
                        else if (Show().TokType == TokenType.tok_double)
                        {
                            ((Function)tempFun).ReturnType = VarType.TypeDouble;
                        }
                        else if (Show().TokType == TokenType.tok_integer)
                        {
                            ((Function)tempFun).ReturnType = VarType.TypeInteger;
                        }
                        else
                        {
                            throw new Exception("Expected a return type");
                        }
                        Next();
                    }
                    if(Show().TokType == TokenType.tok_leftbrack)
                    {
                        Next();
                        while(Show().TokType != TokenType.tok_rightbrack)
                        {
                            ((Function)tempFun).InsideStatements.Add(GetState(((Function)tempFun).LocalVariables));
                        }
                        Next();
                        return tempFun;
                    }
                    else
                    {
                        throw new Exception("Expected opening bracket");
                    }
                }
                else
                {
                    throw new Exception("Expected opening parenthesis");
                }

            }
            else
            {
                throw new Exception("Expected an identifier");
            }
        }

        private Statement FillIfWhile(Statement statem)
        {
            Statement tempStat = statem;
            if(Show().TokType == TokenType.tok_leftpar)
            {
                Next();
                if (statem is IfStatement)
                {
                    ((IfStatement)tempStat).IfExpression = GetCompExpr();
                }else if(statem is WhileStatement)
                {
                    ((WhileStatement)tempStat).WhileExpression = GetCompExpr();
                }
                if(Show().TokType == TokenType.tok_leftbrack)
                {
                    Next();
                    while(Show().TokType != TokenType.tok_rightbrack)
                    {
                        if (statem is IfStatement)
                        {
                            ((IfStatement)tempStat).InsideStatements.Add(GetState(((IfStatement)tempStat).LocalVariables));
                        }
                        else if (statem is WhileStatement)
                        {
                            ((WhileStatement)tempStat).InsideStatements.Add(GetState(((WhileStatement)tempStat).LocalVariables));
                        }
                    }
                    Next();
                    if(tempStat is IfStatement)
                    {
                        if(Show().TokType == TokenType.tok_else)
                        {
                            Next();
                            if (Show().TokType == TokenType.tok_leftbrack)
                            {
                                while (Show().TokType != TokenType.tok_rightbrack)
                                {
                                    ((IfStatement)tempStat).Else.Add(GetState(((IfStatement)tempStat).LocalVariables));
                                }
                                Next();
                            }
                            else
                            {
                                throw new Exception("Expected an opening bracket");
                            }
                        }
                        return tempStat;
                    }
                    return tempStat;
                }
                else
                {
                    throw new Exception("Expected an opening bracket");
                }
            }
            else
            {
                throw new Exception("Expected opening parenthesis");
            }
            
        }

        private CompExpr GetCompExpr()
        {
            CompExpr tempExp = new CompExpr();
            switch (Show(1).TokType)
            {
                case TokenType.tok_eqeq:
                    tempExp.Comparison = "==";
                    break;
                case TokenType.tok_lesseq:
                    tempExp.Comparison = "<=";
                    break;
                case TokenType.tok_less:
                    tempExp.Comparison = "<";
                    break;
                case TokenType.tok_moreeq:
                    tempExp.Comparison = ">=";
                    break;
                case TokenType.tok_more:
                    tempExp.Comparison = ">";
                    break;
                case TokenType.tok_noteq:
                    tempExp.Comparison = "!=";
                    break;
                default:
                    throw new Exception("Unknown comparison");
            }
            Next();
            tempExp.LeftExpr = GetExpr(-1);
            tempExp.RightExpr = GetExpr(1);
            Next();
            Next();
            if(Show().TokType != TokenType.tok_rightpar)
            {
                throw new Exception("Expected closing parenthesis");
            }
            else
            {
                Next();
                return tempExp;
            }
        }

        private Statement GetVarState(List<Statement> vars)
        {
            Variable var = new Variable();
            if(Show().TokType == TokenType.tok_ident)
            {
                var.Name = Next().Value;
                if(Show().TokType == TokenType.tok_colon)
                {
                    Next();
                    switch (Show().TokType)
                    {
                        case TokenType.tok_string:
                            Next();
                            var.DataType = VarType.TypeString;
                                break;
                        case TokenType.tok_double:
                            Next();
                            var.DataType = VarType.TypeDouble;
                            break;
                        case TokenType.tok_integer:
                            Next();
                            var.DataType = VarType.TypeInteger;
                            break;
                        default:
                            throw new Exception("Unknown Type");
                    }
                    if(Show().TokType == TokenType.tok_eq)
                    {
                        Next();
                        var.Value = GetVarExpr();
                        vars.Add(var);
                        return var;
                    }
                    else if(Show().TokType == TokenType.tok_semicol)
                    {
                        Next();
                        vars.Add(var);
                        return var;
                    }
                }else if(Show().TokType == TokenType.tok_eq)
                {
                    Next();
                    VariableEq varEq = new VariableEq();
                    varEq.VariableName = var.Name;
                    varEq.Expression = GetVarExpr();
                    return varEq;
                }else if(Show().TokType == TokenType.tok_leftpar)
                {
                    FunctionCallState funCallStat = new FunctionCallState();
                    funCallStat.Func = GetVarExpr();
                    return funCallStat;
                }
                else
                {
                    throw new Exception("Expected a colon or equals");
                }
            }
            else
            {
                throw new Exception("Expected a variable name");
            }
            throw new Exception("Unknown Exception");
        }

        private Expr? GetVarExpr()
        {
            string? tempName;
            Expr? tempEx = null;
            if (Show(1).TokType == TokenType.tok_semicol)
            {
                if (Show().TokType == TokenType.tok_ident)
                {
                    tempEx = new VariableCall(Next().Value);
                    Next();
                    return tempEx;
                }
                else if (Show().TokType == TokenType.tok_stringval)
                {
                    tempEx = new Literal(VarType.TypeString, Next().Value);
                    Next();
                    return tempEx;
                }
                else if (Show().TokType == TokenType.tok_doubleval)
                {
                    tempEx = new Literal(VarType.TypeDouble, Next().Value);
                    Next();
                    return tempEx;
                }
                else if (Show().TokType == TokenType.tok_integerval)
                {
                    tempEx = new Literal(VarType.TypeInteger, Next().Value);
                    Next();
                    return tempEx;
                }
            }
            else if (Show(1).TokType == TokenType.tok_plus || Show(1).TokType == TokenType.tok_minus ||
                Show(1).TokType == TokenType.tok_div || Show(1).TokType == TokenType.tok_multi)
            {
                MathExpression tempMath = new MathExpression();
                Next();
                switch (Show().TokType)
                {
                    case TokenType.tok_plus:
                        tempMath.Operation = "+";
                        break;
                    case TokenType.tok_minus:
                        tempMath.Operation = "-";
                        break;
                    case TokenType.tok_div:
                        tempMath.Operation = "/";
                        break;
                    case TokenType.tok_multi:
                        tempMath.Operation = "*";
                        break;
                }
                tempMath.LeftExpr = GetExpr(-1);
                tempMath.RightExpr = GetExpr(1);
                Next();
                Next();
                if (Show().TokType == TokenType.tok_semicol)
                {
                    Next();
                    return tempMath;
                }
                else
                {
                    throw new Exception("Wrong mathematic semantic");
                }

            }
            else if (Show(1).TokType == TokenType.tok_leftpar || Show().TokType == TokenType.tok_leftpar)
            {
                FunctionCall tempFun = new FunctionCall();
                if(Show().TokType == TokenType.tok_leftpar && Show(-1).TokType == TokenType.tok_ident)
                {
                    Back();
                }
                if (Show().TokType == TokenType.tok_ident)
                {
                    tempFun.Name = Next().Value;
                    Next();
                    if(Show().TokType == TokenType.tok_rightpar)
                    {
                        Next();
                        if(Show().TokType == TokenType.tok_semicol)
                        {
                            Next();
                            return tempFun;
                        }
                        else
                        {
                            throw new Exception("Expected semicolon");
                        }
                        
                    }
                    else
                    {
                        while(Show().TokType != TokenType.tok_rightpar)
                        {
                            if(Show().TokType == TokenType.tok_stringval)
                            {
                                tempFun.Parameters.Add(new Literal(VarType.TypeString, Next().Value));
                            }else if (Show().TokType == TokenType.tok_integerval)
                            {
                                tempFun.Parameters.Add(new Literal(VarType.TypeInteger, Next().Value));
                            }else if(Show().TokType == TokenType.tok_doubleval)
                            {
                                tempFun.Parameters.Add(new Literal(VarType.TypeDouble, Next().Value));
                            }else if(Show().TokType == TokenType.tok_comma)
                            {
                                Next();
                                continue;
                            }else if (Show().TokType == TokenType.tok_ident)
                            {
                                tempFun.Parameters.Add(new VariableCall(Next().Value));
                            }
                            else
                            {
                                throw new Exception("Unknown parameter");
                            }
                        }
                        Next();
                    }
                    if(Show().TokType == TokenType.tok_semicol)
                    {
                        Next();
                        return tempFun;
                    }
                    else
                    {
                        throw new Exception("Semicolon Expected");
                    }
                }
                throw new Exception("Unknown Error");
            }
            else
            {
                throw new Exception("Unknown or Wrong Input");
            }
            throw new Exception("Unknown Error");
        }

        private Expr GetExpr(int i)
        {
            switch (Show(i).TokType)
            {
                case TokenType.tok_stringval:
                    return new Literal(VarType.TypeString, Show(i).Value);
                case TokenType.tok_integerval:
                    return new Literal(VarType.TypeInteger, Show(i).Value);
                case TokenType.tok_doubleval:
                    return new Literal(VarType.TypeDouble, Show(i).Value);
                case TokenType.tok_ident:
                    return new VariableCall(Show(i).Value);
                default:
                    throw new Exception("Unknown or invalid expression");
            }
        }
    }
}

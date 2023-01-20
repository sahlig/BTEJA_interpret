using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTEJA_PLLexer
{
    public enum TokenType
    {
        tok_ident,
        tok_double,
        tok_doubleval,
        tok_integer,
        tok_integerval,
        tok_string,
        tok_stringval,
        tok_var,
        tok_function,
        tok_if,
        tok_else,
        tok_while,
        tok_return,
        tok_comma,
        tok_semicol,
        tok_colon,
        tok_eq,
        tok_eqeq,
        tok_noteq,
        tok_lesseq,
        tok_moreeq,
        tok_less,
        tok_more,
        tok_plus,
        tok_minus,
        tok_multi,
        tok_div,
        tok_leftpar,
        tok_rightpar,
        tok_leftbrack,
        tok_rightbrack,
    }

    public class Token
    {
        public TokenType TokType { get; set; }
        public string? Value { get; set; }

        public Token(TokenType tokType)
        {
            TokType = tokType;
        }

        public Token(TokenType tokType, string value)
        {
            TokType = tokType;
            Value = value;
        }

        public override string ToString()
        {
            if(Value == null)
            {
                return "Token " + TokType.ToString();
            }
            else
            {
                return "Token " + TokType.ToString() + ", Value " + Value;
            }
        }
    }
}

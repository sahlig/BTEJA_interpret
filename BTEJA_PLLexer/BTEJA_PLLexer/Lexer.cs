using BTEJA_PLLexer;

public class Lexer
{
    public static int checkLetters(string input, string buff, int curPos, List<Token> toks)
    {
        if (Char.IsLetter(buff[0])){
            while (Char.IsLetter(input[curPos]))
            {
                buff += input[curPos];
                curPos++;
            }
            switch (buff.ToLower())
            {
                case "var":
                    toks.Add(new Token(TokenType.tok_var));
                    break;
                case "function":
                    toks.Add(new Token(TokenType.tok_function));
                    break;
                case "if":
                    toks.Add(new Token(TokenType.tok_if));
                    break;
                case "while":
                    toks.Add(new Token(TokenType.tok_while));
                    break;
                case "else":
                    toks.Add(new Token(TokenType.tok_else));
                    break;
                case "double":
                    toks.Add(new Token(TokenType.tok_double));
                    break;
                case "integer":
                    toks.Add(new Token(TokenType.tok_integer));
                    break;
                case "string":
                    toks.Add(new Token(TokenType.tok_string));
                    break;
                case "return":
                    toks.Add(new Token(TokenType.tok_return));
                    break;
                default:
                    toks.Add(new Token(TokenType.tok_ident, buff));
                    break;
            }
            return curPos;
        }
        else if (Char.IsDigit(buff[0]))
        {
            while (Char.IsDigit(input[curPos]))
            {
                buff += input[curPos];
                curPos++;
            }
            if(input[curPos] == ',')
            {
                buff += input[curPos];
                curPos++;
                while (Char.IsDigit(input[curPos]))
                {
                    buff += input[curPos];
                    curPos++;
                }
                toks.Add(new Token(TokenType.tok_doubleval, buff));
            }
            else
            {
                toks.Add(new Token(TokenType.tok_integerval, buff));
            }
            
            return curPos;
        } else if(buff[0] == '"')
        {
            while(input[curPos] != '"')
            {
                buff += input[curPos];
                curPos++;
            }
            buff += input[curPos];
            curPos++;
            buff = buff.Trim('"');
            toks.Add(new Token(TokenType.tok_stringval, buff));

            return curPos;
        }
        else
        {
            return curPos;
        }
    }

    public static bool isEquals(string input, string buff, int curPos)
    {
        if(input[curPos] == '=')
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<Token> Run()
    {
        string inputText = System.IO.File.ReadAllText("C:/Users/Radek/source/repos/BTEJA_PLLexer/input.txt");
        string? buffer = null;
        int currPosition = 0;
        List<Token> tokens = new List<Token>();

        while(currPosition < inputText.Length)
        {
            buffer = null;
            buffer += inputText[currPosition];
            currPosition++;
            switch (buffer)
            {
                case "!":
                    if (isEquals(inputText, buffer, currPosition))
                    {
                        tokens.Add(new Token(TokenType.tok_noteq));
                        currPosition++;
                    }
                    break;
                case ",":
                    tokens.Add(new Token(TokenType.tok_comma));
                    break;
                case ";":
                    tokens.Add(new Token(TokenType.tok_semicol));
                    break;
                case ":":
                    tokens.Add(new Token(TokenType.tok_colon));
                    break;
                case "=":
                    if (isEquals(inputText, buffer, currPosition))
                    {
                        tokens.Add(new Token(TokenType.tok_eqeq));
                        currPosition++;
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.tok_eq));
                    }
                    break;
                case "<":
                    if(isEquals(inputText, buffer, currPosition))
                    {
                        tokens.Add(new Token(TokenType.tok_lesseq));
                        currPosition++;
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.tok_less));
                    }
                    break;
                case ">":
                    if (isEquals(inputText, buffer, currPosition))
                    {
                        tokens.Add(new Token(TokenType.tok_moreeq));
                        currPosition++;
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.tok_more));
                    }
                    break;
                case "+":
                    tokens.Add(new Token(TokenType.tok_plus));
                    break;
                case "-":
                    tokens.Add(new Token(TokenType.tok_minus));
                    break;
                case "*":
                    tokens.Add(new Token(TokenType.tok_multi));
                    break;
                case "/":
                    tokens.Add(new Token(TokenType.tok_div));
                    break;
                case "(":
                    tokens.Add(new Token(TokenType.tok_leftpar));
                    break;
                case ")":
                    tokens.Add(new Token(TokenType.tok_rightpar));
                    break;
                case "{":
                    tokens.Add(new Token(TokenType.tok_leftbrack));
                    break;
                case "}":
                    tokens.Add(new Token(TokenType.tok_rightbrack));
                    break;
                case " ":
                    break;
                default:
                    currPosition = checkLetters(inputText, buffer, currPosition, tokens);
                    break;
            }
        }
        return tokens;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTEJA_PLLexer
{
    public class MainClass
    {
        static void Main(string[] args)
        {
            List<Token> tokens = new List<Token>();
            AST ast = new AST();
            Console.WriteLine("Lexing-------");
            Lexer lexer = new Lexer();
            tokens = lexer.Run();
            Console.WriteLine("Lexed-----------");
            Parser parser = new Parser(tokens);
            Console.WriteLine("Parsing--------------");
            ast = parser.Parse();
            Console.WriteLine("Parsed----------------");
            Console.WriteLine("Running---------------------");
            ast.Run();
            Console.WriteLine("Finished-------------------------");
        }
        
    }
}

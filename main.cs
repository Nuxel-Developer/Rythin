using Rythin.src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Rythin
{
    class main
    {
        private static Collector collector = null;
        static void Main(string[] args)
    	{
            //collector = new Collector();
            string path = "C:\\Users\\Rafael\\Visual Studio Projects\\Rythin\\tests\\example.ry";
            string content = null;
            try
            {
                content = File.ReadAllText(path);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

            Tokens token;
            lexer lex = new lexer(content);
            do
            {
                token = lex.nextToken();
                Console.WriteLine(token);
            } while (token.types != Types.EOF);
        }
    }
}

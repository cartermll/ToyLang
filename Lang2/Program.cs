

namespace Lang2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Lexer lexer = new Lexer();
            FileInfo file = new FileInfo(@"C:\Users\carte\Desktop\example_lang_file.txt");

            Console.WriteLine("Tokenizing...");
            lexer.Tokenize(file);

            foreach (Token token in lexer.Tokens)
            {
                Console.WriteLine(token.ToString());
            }
        }
    }
}

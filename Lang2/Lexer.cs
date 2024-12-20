

namespace Lang2
{
    internal class Lexer
    {
        private Dictionary<char, TokenType> opDict = new Dictionary<char, TokenType>();
        private string validNamingChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_";
        private string nums = "0123456789";

        public List<Token> Tokens;

        public Lexer()
        {
            this.Tokens = new List<Token>();

            opDict.Add('(', TokenType.OpenParen);
            opDict.Add(')', TokenType.CloseParen);
            opDict.Add('[', TokenType.OpenBracket);
            opDict.Add(']', TokenType.CloseBracket);
            opDict.Add('{', TokenType.OpenBrace);
            opDict.Add('}', TokenType.CloseBrace);

            opDict.Add('+', TokenType.Plus);
            opDict.Add('-', TokenType.Minus);
            opDict.Add('*', TokenType.Asterisk);
            opDict.Add('/', TokenType.FwdSlash);
            opDict.Add('.', TokenType.Period);
            opDict.Add(';', TokenType.LineEnd);
        }

        public void Tokenize(FileInfo file)
        {
            if (!file.Exists)
                throw new FileNotFoundException();

            try
            {
                Tokenize(File.ReadAllText(file.FullName));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Tokenize(string input)
        {
            // Add a space to the end of the file
            input += " ";

            string temp = "";

            for (int i = 0; i < input.Length - 1; i++)
            {
                // TODO: Check for comments

                // Continue if current char is a space or newline. We don't care about whitespace.
                if (input[i] == ' ' || input[i] == '\n')
                {
                    if (temp.Length > 0)
                    {
                        Tokens.Add(new Token(TokenType.ID, temp));
                        temp = "";
                    }
                    continue;
                }

                // Check for variable names or keywords
                if (validNamingChars.Contains(input[i]))
                {
                    temp += input[i];
                    continue;
                } else if (temp.Length > 0)
                {
                    Tokens.Add(new Token(TokenType.ID, temp));
                    temp = "";
                    continue;
                }

                // Check if the current character is a single character operator
                if (opDict.ContainsKey(input[i])) 
                {
                    Tokens.Add(new Token(opDict[input[i]], input[i].ToString()));
                    continue;
                }

                // Check for multicharacter operators
                switch (input[i].ToString() + input[i + 1].ToString())
                {
                    case "<<":
                        Tokens.Add(new Token(TokenType.BitShiftLeft, "<<"));
                        i += 1;
                        continue;

                    case ">>":
                        Tokens.Add(new Token(TokenType.BitShiftRight, "<<"));
                        i += 1;
                        continue;

                    case "!=":
                        Tokens.Add(new Token(TokenType.NotEqual, "!="));
                        i += 1;
                        continue;

                    case "==":
                        Tokens.Add(new Token(TokenType.Comparison, "=="));
                        i += 1;
                        continue;

                    default:
                        continue;
                }
            }
        }

        public enum TokenType
        {
            // ID in this case represents a variable or function name
            ID, Int, Long,
            Float, Double,
            // Symbols
            LineEnd,
            OpenParen, CloseParen,
            OpenBrace, CloseBrace,
            OpenBracket, CloseBracket,
            Period, FwdSlash,
            Asterisk, Caret,
            Plus, Minus,
            BitShiftLeft, BitShiftRight,
            NotEqual, Comparison,
            
            // Keywords
            If, Elif, Else,

            // Other
            Unknown
        }
    }
}

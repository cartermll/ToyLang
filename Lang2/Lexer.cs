

namespace Lang2
{
    internal class Lexer
    {
        private Dictionary<char, TokenType> opDict = new Dictionary<char, TokenType>();
        private Dictionary<string, TokenType> keywordDict = new Dictionary<string, TokenType>();
        private string validNamingChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_";
        private string nums = "0123456789";

        public List<Token> Tokens;

        public Lexer()
        {
            this.Tokens = new List<Token>();

            // Add operators to dictionary
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
            opDict.Add('?', TokenType.QuestionMark);
            opDict.Add('"', TokenType.QuotationMark);

            // Add keywords to dictionary
            keywordDict.Add("true", TokenType.True);
            keywordDict.Add("false", TokenType.False);
            keywordDict.Add("if", TokenType.If);
            keywordDict.Add("elif", TokenType.Elif);
            keywordDict.Add("else", TokenType.Else);
            keywordDict.Add("do", TokenType.Do);
            keywordDict.Add("while", TokenType.While);
            keywordDict.Add("and", TokenType.And);
            keywordDict.Add("or", TokenType.Or);
            keywordDict.Add("xor", TokenType.Xor);
            keywordDict.Add("byte", TokenType.Byte);
            keywordDict.Add("short", TokenType.Short);
            keywordDict.Add("int", TokenType.Int);
            keywordDict.Add("long", TokenType.Long);
            keywordDict.Add("float", TokenType.Float);
            keywordDict.Add("double", TokenType.Double);
            keywordDict.Add("bool", TokenType.Bool);
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

            // Length - 1 to ignore the space we add at the end
            for (int i = 0; i < input.Length - 1; i++)
            {
                // TODO: Check for comments, maybe do this after tokenization

                // Continue if current char is a space or newline. We don't care about whitespace
                if (input[i] == ' ' || input[i] == '\n')
                {
                    if (temp.Length > 0)
                    {
                        Tokens.Add(new Token(TokenType.ID, temp));
                        temp = "";
                    }
                    continue;
                }

                // Check for variable names or keywords, allowing digits after the first character
                if (validNamingChars.Contains(input[i]) && temp.Length == 0)
                {
                    temp += input[i];
                    continue;
                } else if ((validNamingChars.Contains(input[i]) || nums.Contains(input[i])) && temp.Length > 0)
                {
                    temp += input[i];
                    continue;
                } else if (temp.Length > 0)
                {
                    Tokens.Add(new Token(TokenType.ID, temp));
                    temp = "";
                }

                // Check if the current character is a single character operator
                if (opDict.ContainsKey(input[i])) 
                {
                    Tokens.Add(new Token(opDict[input[i]], input[i].ToString()));
                    continue;
                }

                // Check for multicharacter operators || maybe make this a dictionary like the others
                switch (input[i].ToString() + input[i + 1].ToString())
                {
                    case "<<":
                        Tokens.Add(new Token(TokenType.BitShiftLeft, "<<"));
                        i += 1;
                        continue;

                    case ">>":
                        Tokens.Add(new Token(TokenType.BitShiftRight, ">>"));
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
                        break;
                }
            }

            // TODO: Check for numeric constants


            // Check IDs for keywords
            foreach (Token token in Tokens)
            {
                if (token.type == TokenType.ID)
                {
                    if (keywordDict.ContainsKey(token.raw))
                    {
                        token.type = keywordDict[token.raw];
                        continue;
                    }
                }
            }

            // TODO: Stop ignoring unknown tokens
        }

        public enum TokenType
        {
            // ID in this case represents a variable or function name
            ID,

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
            QuestionMark, QuotationMark,
            
            // Keywords
            If, Elif, Else, While,
            Do, And, Xor, Or,

            // Constant Types
            Int, Long, Float, Double,
            Byte, Short, Char, Decimal,
            Bool, True, False,

            // Constants
            StringConst, IntConst,
            ByteConst, LongConst,
            FloatConst, DoubleConst,
            CharConst,

            // Other
            Unknown
        }
    }
}

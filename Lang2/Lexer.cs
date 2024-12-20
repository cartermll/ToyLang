

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
            opDict.Add('?', TokenType.QuestionMark);
            opDict.Add('"', TokenType.QuotationMark);
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

            // Check IDs for keywords
            foreach (Token token in Tokens)
            {
                if (token.type == TokenType.ID)
                {
                    switch (token.raw)
                    {
                        // Bool constant values
                        case "true":
                            token.type = TokenType.True;
                            break;

                        case "false":
                            token.type = TokenType.False;
                            break;

                        // Number data type keywords
                        case "byte":
                            token.type = TokenType.Byte;
                            break;

                        case "short":
                            token.type = TokenType.Short;
                            break;

                        case "int":
                            token.type = TokenType.Int;
                            break;

                        case "long":
                            token.type = TokenType.Long;
                            break;

                        case "float":
                            token.type = TokenType.Float;
                            break;

                        case "double":
                            token.type = TokenType.Double;
                            break;

                        case "bool":
                            token.type = TokenType.Bool;
                            break;

                        // Other keywords
                        case "if":
                            token.type = TokenType.If;
                            break;

                        case "elif":
                            token.type = TokenType.Elif;
                            break;

                        case "else":
                            token.type = TokenType.Else;
                            break;

                        case "while":
                            token.type = TokenType.While;
                            break;

                        case "do":
                            token.type = TokenType.Do;
                            break;

                        case "and":
                            token.type = TokenType.And;
                            break;

                        case "xor":
                            token.type = TokenType.Xor;
                            break;

                        case "or":
                            token.type = TokenType.Or;
                            break;

                        default:
                            break;
                    }
                }
            }
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

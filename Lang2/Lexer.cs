/*  TODO:
 *      High priority:
 *          Add better error handling. store location data in the tokens to help the user locate the problem
 *          Handle unknown tokens 
 *          Check for comments
 *          Store numeric literals,  maybe find a good way to separate data types in the lexing phase
 *
 *      Low priority:
 *          Stop adding a space to the end of the input, feels weird
 *          Maybe use regex
*/

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

            #region opDict definition
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
            opDict.Add('<', TokenType.LessThan);
            opDict.Add('>', TokenType.GreaterThan);
            opDict.Add('!', TokenType.Not);
            #endregion

            #region keywordDict definition
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
            #endregion
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
                // Continue if current char is whitespace or a newline
                if (char.IsWhiteSpace(input[i]) || input[i] == '\n')
                {
                    if (temp.Length > 0)
                    {
                        AddToken(TokenType.ID, temp);
                        temp = "";
                    }
                    continue;
                }

                // Check for string literals
                if (input[i] == '"')
                {
                    AddStringLiteral(input, i);
                    i += Tokens[Tokens.Count - 1].Raw.Length + 2; // iterate i past the string, adding two to ignore the first and second quote
                }

                // Check for symbol names, allowing digits after the first character
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
                    AddToken(TokenType.ID, temp);
                    temp = "";
                }

                // Check for operators, assumes the first char of our two-character ops are in the dictionary already
                if (opDict.ContainsKey(input[i]))
                {
                    switch (input[i].ToString() + input[i + 1].ToString())
                    {
                        case "<<":
                            AddToken(TokenType.BitShiftLeft, "<<");
                            i += 1;
                            continue;

                        case ">>":
                            AddToken(TokenType.BitShiftRight, ">>");
                            i += 1;
                            continue;

                        case "!=":
                            AddToken(TokenType.NotEqual, "!=");
                            i += 1;
                            continue;

                        case "==":
                            AddToken(TokenType.Comparison, "==");
                            i += 1;
                            continue;

                        default:
                            AddToken(opDict[input[i]], input[i].ToString());
                            continue;
                    }
                }

                // Add unknown token if nothing above works
                AddToken(TokenType.Unknown, input[i].ToString());
            }

            // Check symbols for keywords
            foreach (Token token in Tokens)
            {
                if (token.Type == TokenType.ID)
                {
                    if (keywordDict.ContainsKey(token.Raw))
                    {
                        token.Type = keywordDict[token.Raw];
                        continue;
                    }
                }
            }
        }

        // Just makes adding tokens a little cleaner
        private void AddToken(TokenType type, string raw)
        {
            Tokens.Add(new Token(type, raw));
        }

        // Finds a string literal and returns its value.
        private void AddStringLiteral(string input, int index)
        {
            int start = index;

            while (input[++index] != '"')
            {
                if (index >= input.Length - 1)
                {
                    throw new Exception("Unterminated string");
                } 

                index++;
            }

            AddToken(TokenType.StringLiteral, input.Substring(start + 1, (index - 1) - start));
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
            QuestionMark, Not,
            LessThan, GreaterThan,

            
            // Keywords
            If, Elif, Else, While,
            Do, And, Xor, Or,

            // Types (Keywords)
            Int, Long, Float, Double,
            Byte, Short, Char, Decimal,
            Bool, True, False,

            // Literal Types
            StringLiteral,

            // Other
            Unknown
        }
    }
}

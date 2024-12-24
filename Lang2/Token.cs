

namespace Lang2
{
    internal class Token
    {
        public Lexer.TokenType Type { get; set; }
        public string Raw { get; set; }

        public Token(Lexer.TokenType Type, string Raw)
        {
            this.Type = Type;
            this.Raw = Raw;
        }

        public Token(Lexer.TokenType Type)
        {
            this.Type = Type;
            this.Raw = "";
        }
        
        public override string ToString()
        {
            return $"[Type: {Type.ToString()}, Raw: \"{Raw}\"]";
        }
    }
}



namespace Lang2
{
    internal class Token
    {
        public TokenType Type { get; set; }
        public string Raw { get; }
        public int Line { get; }
        public int Column { get; }

        public Token(TokenType type, string raw, int line, int column)
        {
            this.Type = type;
            this.Raw = raw;
            this.Line = line;
            this.Column = column;
        }

        public Token(TokenType type, string raw)
        {
            this.Type = type;
            this.Raw = raw;
        }

        public Token(TokenType type)
        {
            this.Type = type;
            this.Raw = "";
        }
        
        public override string ToString()
        {
            return $"[Type: {Type.ToString()}, Raw: \"{Raw}\"]";
        }
    }
}

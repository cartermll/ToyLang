

namespace Lang2
{
    internal class Token
    {
        public Lexer.TokenType type;
        public string raw;

        public Token(Lexer.TokenType type, string raw)
        {
            this.type = type;
            this.raw = raw;
        }

        public Token(Lexer.TokenType type)
        {
            this.type = type;
            this.raw = "";
        }
        
        public override string ToString()
        {
            return $"[type: {type.ToString()}, raw: \"{raw}\"]";
        }
    }
}

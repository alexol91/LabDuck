using LabDuck.Domain.Enums;

namespace LabDuck.Domain.Lexical
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string Lexema { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
    }
}

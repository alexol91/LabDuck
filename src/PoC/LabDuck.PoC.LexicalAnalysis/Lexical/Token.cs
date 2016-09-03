using LabDuck.PoC.LexicalAnalysis.Enums;

namespace LabDuck.PoC.LexicalAnalysis.Lexical
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string Lexema { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
    }
}

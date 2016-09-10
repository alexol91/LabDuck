using LabDuck.Domain.Lexical;
using LabDuck.PoC.Syntactic.Rules;
using LabDuck.PoC.Syntactic.Translate;
using System.Collections.Generic;

namespace LabDuck.PoC.Syntactic
{
    public class SyntacticAnalyzer
    {
        private readonly List<Token> tokens;
        public SyntacticAnalyzer(List<Token> tokens)
        {
            this.tokens = tokens;
        }

  
        public TranslationInfo Run()
        {
            SRule s = new SRule(tokens, 0);
            return s.Run();
        }

    }
}

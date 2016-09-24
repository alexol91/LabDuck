using System;
using System.Collections.Generic;
using LabDuck.Domain.Lexical;
using LabDuck.PoC.Syntactic.Translate;
using System.Text;

namespace LabDuck.PoC.Syntactic.Rules
{
    class TermRule : RuleBase
    {
        public TermRule(List<Token> tokens, int index) : base(tokens, index)
        {
        }

        public override TranslationInfo Run()
        {
            return Evaluate(EvaluateFactor);
        }


        private TranslationInfo EvaluateFactor(int current)
        {
            TranslationInfo translationInfo = null;
            StringBuilder translate = new StringBuilder();

            translationInfo = new FactorRule(tokens, current).Run();
            translate.Append(translationInfo.Translate);
            while (true)
            {
                try
                {
                    translationInfo = EvaluateMulopFactors(translationInfo.Index);
                    translate.Append(translationInfo.Translate);
                }
                catch
                {
                    break;
                }
            }

            return new TranslationInfo(translate.ToString(), translationInfo.Index);
        }

        private TranslationInfo EvaluateMulopFactors(int current)
        {
            TranslationInfo translationInfo = null;
            StringBuilder translate = new StringBuilder();

            translationInfo = Match(current, MulOp);
            string multop = translationInfo.Translate == "*" ? "mul" : "div";

            translationInfo = new FactorRule(tokens, translationInfo.Index).Run();

            translate.Append(translationInfo.Translate);
            translate.AppendLine(multop);

            return new TranslationInfo(translate.ToString(), translationInfo.Index);
        }
    }
}

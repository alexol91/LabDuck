using LabDuck.PoC.Syntactic.Rules;
using System;
using System.Collections.Generic;
using LabDuck.Domain.Lexical;
using LabDuck.PoC.Syntactic.Translate;
using System.Text;

namespace LabDuck.PoC.Syntactic
{
    class FactorRule : RuleBase
    {
        public FactorRule(List<Token> tokens, int index) : base(tokens, index)
        {
        }

        public override TranslationInfo Run()
        {
            return Evaluate(EvaluateBase, EvaluateFactor);
        }

        private TranslationInfo EvaluateBase(int current)
        {
            return new BaseRule(tokens, index).Run();
        }

        private TranslationInfo EvaluateFactor(int current)
        {
            TranslationInfo translationInfo = null;
            StringBuilder translate = new StringBuilder();

            translationInfo = Match(current, Pari);

            TranslationInfo addOpTranslationInfo = Match(translationInfo.Index, AddOp);

            translationInfo = new FactorRule(tokens, addOpTranslationInfo.Index).Run();
            translate.Append(translationInfo.Translate);

            if(addOpTranslationInfo.Translate == "-")
            {
                translate.AppendLine("neg");
            }

            translationInfo = Match(translationInfo.Index, Pard);

            return new TranslationInfo(translate.ToString(), translationInfo.Index);
        }
    }
}

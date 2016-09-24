using System;
using System.Collections.Generic;
using LabDuck.Domain.Lexical;
using LabDuck.PoC.Syntactic.Translate;
using System.Linq;
using System.Text;

namespace LabDuck.PoC.Syntactic.Rules
{
    class ExprRule : RuleBase
    {
        public ExprRule(List<Token> tokens, int index) : base(tokens, index)
        {
        }

        public override TranslationInfo Run()
        {
            return Evaluate(EvaluateTerms);
        }


        private TranslationInfo EvaluateTerms(int current)
        {
            TranslationInfo translationInfo = null;
            StringBuilder translate = new StringBuilder();

            translationInfo = new TermRule(tokens, current).Run();
            translate.Append(translationInfo.Translate);
        
            while (true)
            {
                try
                {
                    translationInfo = EvaluateAddopTerms(translationInfo.Index);
                    translate.Append(translationInfo.Translate);
                }
                catch
                {
                    break;
                }
            }

            return new TranslationInfo(translate.ToString(), translationInfo.Index);
        }

        private TranslationInfo EvaluateAddopTerms(int current)
        {
            TranslationInfo translationInfo = null;
            StringBuilder translate = new StringBuilder();

            translationInfo = Match(current, AddOp);
            string addop = translationInfo.Translate == "+" ? "add" : "sub";

            translationInfo = new TermRule(tokens, translationInfo.Index).Run();

            translate.Append(translationInfo.Translate);
            translate.AppendLine(addop);

            return new TranslationInfo(translate.ToString(), translationInfo.Index);
        }
    }
}

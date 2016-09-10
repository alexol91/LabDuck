using System;
using System.Collections.Generic;
using LabDuck.Domain.Lexical;
using LabDuck.Domain.Enums;
using LabDuck.PoC.Syntactic.Translate;
using System.Text;

namespace LabDuck.PoC.Syntactic.Rules
{
    class BaseRule : RuleBase
    {
        public BaseRule(List<Token> tokens, int index) : base(tokens, index)
        {
        }

        public override TranslationInfo Run()
        {
            return Evaluate
                (
                    EvaluateInteger,
                    EvaluateFloat,
                    EvaluatExpression
                );

        }

        private TranslationInfo EvaluateInteger(int current)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var translationInfo = MathType(current, TokenType.Integer);
            stringBuilder.AppendLine($"ldc.i4 {translationInfo.Translate}");
            stringBuilder.AppendLine("conv.r8");
            translationInfo.Translate = stringBuilder.ToString();
            return translationInfo;
        }

        private TranslationInfo EvaluateFloat(int current)
        {
            var translationInfo = MathType(current, TokenType.Float);
            translationInfo.Translate = $"ldc.r8 {translationInfo.Translate}\n";
            return translationInfo;
        }

        private TranslationInfo EvaluatExpression(int current)
        {
            TranslationInfo translationInfo = null;
            StringBuilder translate = new StringBuilder();

            translationInfo = Match(current, Pari);

            translationInfo = new ExprRule(tokens, translationInfo.Index).Run();
            translate.Append(translationInfo.Translate);

            translationInfo = Match(translationInfo.Index, Pard);

            return new TranslationInfo(translate.ToString(), translationInfo.Index);
        }
    }
}

using System;
using System.Collections.Generic;
using LabDuck.Domain.Lexical;
using LabDuck.PoC.Syntactic.Translate;
using System.Text;

namespace LabDuck.PoC.Syntactic.Rules
{
    class SRule : RuleBase
    {
        public SRule(List<Token> tokens, int index) : base(tokens, index)
        {
        }

        public override TranslationInfo Run()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(CreateCilHeader());

            var tralationInfo = new ExprRule(tokens, index).Run();
             builder.AppendLine(tralationInfo.Translate);

            builder.AppendLine("call void [mscorlib]System.Console::WriteLine(float64)");
            builder.AppendLine("ret");
            builder.AppendLine("}");
            builder.AppendLine("}");

            return new TranslationInfo(builder.ToString(), tralationInfo.Index);
        }

        private string CreateCilHeader()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(".assembly extern mscorlib {}");
            builder.AppendLine(".assembly 'LabDuck' {}");
            builder.AppendLine(".class 'LabDuckClass' extends[mscorlib] System.Object {");
            builder.AppendLine(".method public specialname rtspecialname instance void .ctor() cil managed {ret}");

            builder.AppendLine(".method static public void main () cil managed {");
            builder.AppendLine(".entrypoint");
            builder.AppendLine(".maxstack 1000");
            return builder.ToString();
        }
    }
}

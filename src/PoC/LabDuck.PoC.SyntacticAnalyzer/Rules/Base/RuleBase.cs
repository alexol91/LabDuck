using LabDuck.Domain.Enums;
using LabDuck.Domain.Lexical;
using LabDuck.PoC.Syntactic.Translate;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace LabDuck.PoC.Syntactic.Rules
{
    public abstract class RuleBase
    {
        protected const string AddOp = @"^[\+|\-]";
        protected const string MulOp = @"^[\*|\/]";
        protected const string Llavei = @"^\{";
        protected const string Llaved = @"^\}";
        protected const string Pari = @"^\(";
        protected const string Pard = @"^\)";

        protected readonly List<Token> tokens;
        protected int index;

        public RuleBase(List<Token> tokens, int index)
        {
            this.tokens = tokens;
            this.index = index;
        }

        public TranslationInfo Match(int index, string regex)
        {
            if (Regex.IsMatch(tokens[index].Lexema, regex))
            {
                return new TranslationInfo(tokens[index].Lexema, index + 1);
            }
            throw new Exception("Literal no encontrado");
        }

        protected TranslationInfo MathType(int index, TokenType type)
        {
            if (tokens[index].Type == type)
            {
                return new TranslationInfo(tokens[index].Lexema, index + 1);
            }
            throw new Exception("No es de ese tipo");
        }

        public abstract TranslationInfo Run();

        protected TranslationInfo Evaluate(params Func<int, TranslationInfo>[] functions)
        {
            foreach (var function in functions)
            {
                Debug.WriteLine($"{function.ToString().GetType()}: {tokens[index].Lexema}");
                try
                {
                    return function(index);
                }
                catch { continue; }
            }
            throw new Exception("Nothing rule matched");
        }
    }
}

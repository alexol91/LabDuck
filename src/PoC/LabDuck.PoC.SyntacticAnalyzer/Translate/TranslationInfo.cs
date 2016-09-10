using LabDuck.Domain.Lexical;
using System.Collections.Generic;

namespace LabDuck.PoC.Syntactic.Translate
{
    public class TranslationInfo
    {
        public string Translate { get; set; }
        public string Prefix { get; set; }
        public int Index { get; set; }

        public TranslationInfo(string translate, int index)
        {
            Translate = translate;
            Index = index;
        }
    }
}

using LabDuck.PoC.LexicalAnalysis.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LabDuck.PoC.LexicalAnalysis.Lexical
{
    public class TokenDefinition
    {
        [JsonProperty("regex")]
        public string RegularExpression { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TokenType Type { get; set; }
    }
}

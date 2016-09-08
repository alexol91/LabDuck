using LabDuck.Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LabDuck.Domain.Lexical
{
    public class TokenDefinition
    {
        [JsonProperty("regex")]
        public string RegularExpression { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TokenType Type { get; set; }
    }
}

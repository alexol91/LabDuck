using LabDuck.PoC.LexicalAnalysis.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LabDuck.PoC.LexicalAnalysis.Lexical
{
    public class LexicalAnalizer
    {
        private List<TokenDefinition> tokenDefinitions;

        public LexicalAnalizer(string tokenDefinitionsJson)
        {
            LoadTokenDefinitions(tokenDefinitionsJson);
        }

        public List<Token> Analyze(string text)
        {
            List<Token> tokens = new List<Token>();

            while (!string.IsNullOrEmpty(text))
            {
                var token = GetMatchToken(text);
                if (token != null)
                {
                    text = text.Remove(0, token.Lexema.Length);
                    tokens.Add(token);
                }
                else
                {
                    text = text.Remove(0, 1);
                }
            }
            return tokens;
        }

        private Token GetMatchToken(string text)
        {
            List<Token> tokens = new List<Token>();
            foreach (var tokenDefinition in tokenDefinitions)
            {
                Regex regex = new Regex($"^{tokenDefinition.RegularExpression}");
                var match = regex.Match(text);
                if (match.Success)
                {
                    tokens.Add(new Token() { Type = tokenDefinition.Type, Lexema = match.Value });
                }
            }
            return tokens.OrderByDescending(token => token.Lexema.Length).FirstOrDefault();
        }

        private void LoadTokenDefinitions(string tokenDefinitionsJson)
        {
            tokenDefinitions = JsonConvert.DeserializeObject<List<TokenDefinition>>(tokenDefinitionsJson);
        }
    }
}

using LabDuck.Domain.Enums;
using LabDuck.Domain.Lexical;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LabDuck.PoC.LexicalAnalysis.Lexical
{
    public class LexicalAnalizer
    {
        private IEnumerable<IGrouping<TokenType, TokenDefinition>> tokenGroups;

        public LexicalAnalizer(string tokenDefinitionsJson)
        {
            LoadTokenDefinitions(tokenDefinitionsJson);
        }

        public List<Token> Analyze(string text)
        {
            int column = 0;
            int row = 0;
            List<Token> tokens = new List<Token>();

            while (!string.IsNullOrEmpty(text))
            {
                try
                {
                    Token token = GetMatchToken(text);
                    int tokenLength = token.Lexema.Length;
                    column += tokenLength;
                    text = text.Remove(0, tokenLength);
                    if (token.Type != TokenType.Ignored)
                    {
                        if (token.Type != TokenType.EOL)
                        {
                            token.Row = row;
                            token.Column = column - tokenLength;
                            tokens.Add(token);
                        }
                        else
                        {
                            ++row;
                            column = 0;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception($"[Token not found] column: {column} row: {row} in '{text.Substring(0, text.IndexOf('\n'))}'", e);
                }
            }
            tokens.Add(new Token { Type = TokenType.EOL });
            return tokens;
        }

        private Token GetMatchToken(string text)
        {
            foreach (var group in tokenGroups)
            {
                var tokens = group.Select(token => Regex.Match(text, $"^{token.RegularExpression}")).Where(token => token.Success);
                if (tokens.Any())
                {
                    var match = tokens.OrderByDescending(token => token.Length).FirstOrDefault();
                    return new Token { Type = group.Key, Lexema = match.Value };
                }
            }
            throw new Exception("Token not found");
        }

        private void LoadTokenDefinitions(string tokenDefinitionsJson)
        {
            tokenGroups = JsonConvert.DeserializeObject<List<TokenDefinition>>(tokenDefinitionsJson)
                .GroupBy(token => token.Type).OrderBy(group => group.Key).ToList();
        }
    }
}

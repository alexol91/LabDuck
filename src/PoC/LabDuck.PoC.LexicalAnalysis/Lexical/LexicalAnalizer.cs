using LabDuck.PoC.LexicalAnalysis.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LabDuck.PoC.LexicalAnalysis.Lexical
{
    public class LexicalAnalizer
    {
        private IEnumerable<TokenDefinition> tokenDefinitions;

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
                        if(token.Type != TokenType.EOL)
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
            Match match = null;
            var tokenFound = tokenDefinitions.FirstOrDefault(tokenDefinition =>
            {
                Regex regex = new Regex($"^{tokenDefinition.RegularExpression}");
                match = regex.Match(text);
                return match.Success;
            });

            if (tokenFound == null)
            {
                throw new Exception("Token not found");
            }

            return new Token { Type = tokenFound.Type, Lexema = match.Value };
        }

        private void LoadTokenDefinitions(string tokenDefinitionsJson)
        {
            tokenDefinitions = JsonConvert.DeserializeObject<List<TokenDefinition>>(tokenDefinitionsJson)
                .OrderByDescending(toke => toke.Type);
        }
    }
}

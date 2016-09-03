using LabDuck.Domain.Lexical;
using LabDuck.PoC.LexicalAnalysis.Lexical;
using System;
using System.Collections.Generic;
using System.IO;

namespace LabDuck.PoC.LexicalAnalysis
{
    class Program
    {
        private const string LabDuckHeaderMessage = "Lab.Duck - PoC Lexical Analyzer 0.0.1 - (Alejandro Oñate, Sergio Gallardo)";
        private const string ExampleFileName = "example.txt";
        private const string TokeDefinitionFileName = "tokenDefinitions.json";

        static void Main(string[] args)
        {
            var text = File.ReadAllText(ExampleFileName);
            var tokenDefinitionsJson = File.ReadAllText(TokeDefinitionFileName);
            LexicalAnalizer analizer = new LexicalAnalizer(tokenDefinitionsJson);
            try
            {
                var tokens = analizer.Analyze(text);
                Verbose(text, tokens);
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }

        private static void Verbose(string text, List<Token> tokens)
        {
            Console.WriteLine(LabDuckHeaderMessage);
            Console.WriteLine();
            Console.WriteLine(text);
            Console.WriteLine();
            Console.WriteLine("====================================");
            Console.WriteLine();
            foreach (var token in tokens)
            {
                Console.WriteLine($"{token.Lexema}\t\t:{token.Type}\t\t{token.Row}:{token.Column}");
            }
        }
    }
}

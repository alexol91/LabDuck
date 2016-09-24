using LabDuck.PoC.LexicalAnalysis.Lexical;
using System;
using System.IO;

namespace LabDuck.PoC.Syntactic
{
    class Program
    {
        private const string LabDuckHeaderMessage = "Lab.Duck - PoC Lexical Analyzer 0.0.1 - (Alejandro Oñate, Sergio Gallardo)";
        private const string ExampleFileName = "example.txt";
        private const string TokeDefinitionFileName = "tokenDefinitions.json";
        private const string DefaultCilFilePath = @"duck.il";

        static void Main(string[] args)
        {
            var text = File.ReadAllText(ExampleFileName);
            var tokenDefinitionsJson = File.ReadAllText(TokeDefinitionFileName);
            LexicalAnalizer analizer = new LexicalAnalizer(tokenDefinitionsJson);
            try
            {
                var tokens = analizer.Analyze(text);
                SyntacticAnalyzer s = new SyntacticAnalyzer(tokens);
                var translation = s.Run();

                Console.WriteLine("---------------DUCK CODE---------------");
                Console.WriteLine(text);
                Console.WriteLine("---------------------------------------\n");

                Console.WriteLine("------------------CIL------------------");
                Console.WriteLine(translation.Translate);
                Console.WriteLine("---------------------------------------");

                File.WriteAllText(DefaultCilFilePath, translation.Translate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }
    }
}

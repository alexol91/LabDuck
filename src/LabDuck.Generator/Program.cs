using System;
using System.Diagnostics;
using System.Linq;

namespace LabDuck.Generator
{
    public class Program
    {
        private const string ErrorMessage = @"Sorry, we found some errors. Please press any key to close program.";
        private const string BuildSuccessMessage = @"Build success";
        private const string IslmPath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\ilasm.exe";
        private const string DefaultCilFilePath = @"duck.il";

        static void Main(string[] args)
        {
           Console.WriteLine(Build(args.FirstOrDefault()));
        }

        private static string Build(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = DefaultCilFilePath;
            }

            var arguments = $"{filePath} /exe";
            ProcessStartInfo processStartInfo = new ProcessStartInfo(IslmPath, arguments);
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            var process = Process.Start(processStartInfo);
            var standarOutput = process.StandardOutput.ReadToEnd();
            var errorOutput = process.StandardError.ReadToEnd();

            return !string.IsNullOrEmpty(errorOutput) ? errorOutput : standarOutput;
        }
    }
}

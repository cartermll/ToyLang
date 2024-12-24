using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lang2
{
    internal static class Debug
    {
        public static void PrintError(Error error, bool exit = false)
        {
            Console.WriteLine(error.Message);
            Console.WriteLine($"at line {error.Line}, column {error.Column}.\n");

            if (exit)
            {
                Environment.Exit(0);
            }
        }
    }

    internal class Error
    {
        public string Message { get; }
        public string LineText { get; }
        public int Line { get; }
        public int Column { get; }

        public Error(string message, string lineText, int line, int column)
        {
            this.Message = message;
            this.LineText = lineText;
            this.Line = line;
            this.Column = column;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSeqCli
{
    internal class ProgramOptions
    {
        public string PathToSolution { get; set; }
        public string ProjectName { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string OutputFile { get; set; }


        private const string HELP_TEXT =
            "This helper program tries to draw a seqence diagram with a maximal depth of 10 from the given method.\n" +
            "Usage:\n" +
            "\t -s \t Path to the solution with the method to analyze \n" +
            "\t -p \t Project in which the method can be found \n" +
            "\t -c \t Classname with the method to analyze \n" +
            "\t -m \t Methodname to analyze \n" +
            "\t -o \t File to write puml syntax to \n" +
            "\n" +
            "Known issues \n " +
            "- Project to be analyzed must be compiled in debug setting first \n" +
            "- Interfaces are only resolved in the analyzed project \n" +
            "- Needs to be restarted for every new method in the same solution \n" +
            "- Overloaded methods are not tested yet \n";

        public ProgramOptions(string[] args)
        {
            FilterArgs(args);
            Validate();
        }

        private void Validate()
        {
            if (!File.Exists(PathToSolution))
                throw new InvalidOperationException("Solution not found!");
        }

        private void FilterArgs(string[] args)
        {
            for(int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-s":
                        PathToSolution = args[++i];
                        break;

                    case "-p":
                        ProjectName = args[++i];
                        break;

                    case "-c":
                        ClassName = args[++i];
                        break;

                    case "-m":
                        MethodName = args[++i];
                        break;

                    case "-o":
                        OutputFile = args[++i];
                        break;

                    default:
                        PrintHelp();
                        break;
                }

            }
        }

        private void PrintHelp()
        {
            Console.WriteLine(HELP_TEXT);
            throw new InvalidOperationException();
        }
    }
}

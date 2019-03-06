using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WinSeqDiag
{
    internal class ProgramOptions
    {
        public string PathToSolution { get; set; }
        public string ProjectName { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string OutputFile { get; set; }
        public string ExcludingAssemblies { get; private set; }
        public bool VerboseLogging { get; private set; }
        public bool DebugLogging { get; private set; }
        public string DemoCase { get; private set; }
        public bool ListProjectAndExit { get; private set; }

        private const string HELP_TEXT =
            "This helper program tries to draw a seqence diagram with a maximal depth of 10 from the given method.\n" +
            "Usage:\n" +
            "\t -s \t Path to the solution with the method to analyze \n" +
            "\t -p \t Project in which the method can be found \n" +
            "\t -c \t Classname with the method to analyze \n" +
            "\t -m \t Methodname to analyze \n" +
            "\t -o \t File to write puml syntax to \n" +
            "\t -x \t Assemblies including the specifier will be excluded \n" +
            "\t -verbose \t Writes verbose log messages \n" +
            "\t -debug \t Writes debug log messages \n" +
            "\t -list \t Lists projects in the solution \n" + 
            "\n" +
            "Known issues \n " +
            "- Project to be analyzed must be compiled in debug setting first \n" +
            "- Interfaces are only resolved in the analyzed project \n" +
            "- Needs to be restarted for every new method in the same solution \n" +            
            "- OutOfMemoryException if solutions/projects are to big. Try to exclude some references using -x" +
            "- Overloaded methods are not tested yet \n";

        public ProgramOptions(string[] args)
        {
            FilterArgs(args);
            if (string.IsNullOrEmpty(DemoCase))
                Validate();
        }

        private void Validate()
        {
            if (!File.Exists(PathToSolution))
                throw new InvalidOperationException("Solution not found!");
        }

        private void FilterArgs(string[] args)
        {

            if (args.Length == 0)
                PrintHelp();

            for (int i = 0; i < args.Length; i++)
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

                    case "-x":
                        ExcludingAssemblies = args[++i];
                        break;                  

                    case "-verbose":
                        VerboseLogging = true;
                        break;

                    case "-debug":
                        DebugLogging = true;
                        break;

                    case "-d":
                    case "-d1":
                    case "-dev":
                        DemoCase = args[i];
                        break;

                    case "-list":
                        ListProjectAndExit = true;
                        break;

                    default:
                        PrintHelp();
                        break;
                }

            }
        }

        internal void CreateDemoCase()
        {
            if (DemoCase == "-d")
            {
                var basePath = GetBasePath("UI");
                PathToSolution = Path.Combine(basePath, "CodeDocumentations.sln"); //solution
                ProjectName = "DemoNet"; //project
                ClassName = "ClassA"; //class
                MethodName = "IncreaseList"; //method
                OutputFile = Path.Combine(basePath, "demo.wsd"); //outfile
            }

            if (DemoCase == "-d1")
            {
                var basePath = GetBasePath("UI");
                PathToSolution = Path.Combine(basePath, "CodeDocumentations.sln"); //solution
                ProjectName = "DemoNet"; //project
                ClassName = "ClassA"; //class
                MethodName = "ConditionalIncrease"; //method
                OutputFile = Path.Combine(basePath, "demo1.wsd"); //outfile                
            }

            if (DemoCase == "-dev")
            {
                var basePath = GetBasePath("UI");
                PathToSolution = Path.Combine(basePath, "CodeDocumentations.sln"); //solution
                ProjectName = "DemoProject"; //project
                ClassName = "ClassA"; //class
                MethodName = "ConditionalIncrease"; //method
                OutputFile = Path.Combine(basePath, "dev.wsd"); //outfile                
            }
        }

        private void PrintHelp()
        {
            Console.WriteLine(HELP_TEXT);
            throw new InvalidOperationException();
        }

        private static string GetBasePath(string v)
        {
            var assemblyPath = GetAssemblyDirectory();
            var projectPath = assemblyPath.Substring(0, assemblyPath.IndexOf("WinSeqDiag"));
            return projectPath.Split(new string[] { v }, 2, StringSplitOptions.None)[0];
        }

        private static string GetAssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}

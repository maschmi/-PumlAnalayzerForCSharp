using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeAnalyzer;
using CodeAnalyzer.Context;
using CodeAnalyzer.InterfaceResolver;
using Logger;
using SequenceDiagram;

namespace DrawSeqCli
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "-d")
            {
                var basePath = GetBasePath();
                args = new string[] {
                "-s",  Path.Combine(basePath, "codedocumentation.sln"), //solution
                "-p", "DemoProject", //project
                "-c", "ClassA", //class
                "-m", "IncreaseList", //method
                "-o", Path.Combine(basePath, "demo1.wsd") }; //outfile
            }

            if (args.Length == 1 && args[0] == "-d1")
            {
                var basePath = GetBasePath();
                args = new string[] {
                "-s",  Path.Combine(basePath, "codedocumentation.sln"), //solution
                "-p", "DemoProject", //project
                "-c", "ClassA", //class
                "-m", "ConditionalIncrease", //method
                "-o", Path.Combine(basePath, "demo2.wsd") }; //outfile
            }

            if (args.Length == 1 && args[0] == "-dev")
            {
                var basePath = GetBasePath();
                args = new string[] {
                "-s",  Path.Combine(basePath, "codedocumentation.sln"), //solution
                "-p", "TestProject", //project
                "-c", "ClassA", //class
                "-m", "OnlyReturn", //method
                "-o", Path.Combine(basePath, "demo3.wsd") }; //outfile
        }
            var cfgCtx = new ConfigContext(InterfaceResolverType.ProjectLevel);

            try
            {
                var options = new ProgramOptions(args);
            
                IDoLog logger = new ConsoleLogger();
                using (var solutionAnalyzer = new SolutionAnalyzer(options.PathToSolution, logger))
                {

                    await solutionAnalyzer.LoadSolution();

                    var projectAnalyzer = new ProjectAnalyzer(solutionAnalyzer.ParsedSolution, solutionAnalyzer.OutputFiles, logger);
                    await projectAnalyzer.LoadProject(options.ProjectName);

                    var interfaceResolver = InterfaceResolverFactory.GetInterfaceResolver(solutionAnalyzer, projectAnalyzer, logger, cfgCtx);

                    var sequenceDiagram = new SequenceDiagramGenerator(projectAnalyzer.AnalyzedClasses, interfaceResolver, logger);
                    string diagramText = await sequenceDiagram.GetSequenceDiagramForMethod(options.ClassName, options.MethodName);

                    await WriteDiagramToFile(diagramText, options.OutputFile);

                    Console.WriteLine("Wrote to " + options.OutputFile);
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            Console.WriteLine("Finished...");            
        }

        private static async Task WriteDiagramToFile(string diagramText, string outputFile)
        {
            byte[] diagram = Encoding.Unicode.GetBytes(diagramText);
            using (FileStream fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            {
                await fs.WriteAsync(diagram, 0, diagram.Length);
            }
        }

        private static string GetBasePath()
        {
            var assemblyPath = GetAssemblyDirectory();
            return assemblyPath.Substring(0, assemblyPath.IndexOf("DrawSeqCli"));
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
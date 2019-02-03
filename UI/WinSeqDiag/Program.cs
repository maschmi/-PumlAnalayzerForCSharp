using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeAnalyzer.Context;
using CodeAnalyzer.InterfaceResolver;
using Logger;
using SequenceDiagram;
using WorkspaceAnalyzer.Windows;

namespace WinSeqDiag
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var cfgCtx = new ConfigContext(InterfaceResolverType.ProjectLevel);

            try
            {
                var options = new ProgramOptions(args);

                if (!string.IsNullOrEmpty(options.DemoCase))
                    options.CreateDemoCase();


                IDoLog logger = new ConsoleLogger(verbose: options.VerboseLogging, debug: options.DebugLogging);
                using (var solutionAnalyzer = new SolutionAnalyzer(options.PathToSolution, logger))
                {
                    await solutionAnalyzer.LoadSolution(options.ExcludingAssemblies);

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
     
    }
}

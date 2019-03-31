using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeAnalyzer.Context;
using CodeAnalyzer.InterfaceResolver;
using CodeAnalyzer.Service;
using Logger;
using SequenceDiagram;
using WorkspaceAnalyzer.Windows;

namespace WinSeqDiag
{
    public class Program
    {
        private static string MSBUILD_PATH = @"C:\Program Files\dotnet\sdk\2.2.103";
        public static async Task Main(string[] args)
        {
            var cfgCtx = new ConfigContext(InterfaceResolverType.ProjectLevel);

            try
            {
                var options = new ProgramOptions(args);

                if (!string.IsNullOrEmpty(options.DemoCase))
                    options.CreateDemoCase();


                IDoLog logger = new ConsoleLogger(verbose: options.VerboseLogging, debug: options.DebugLogging);
                if(options.ListProjectAndExit)
                {
                    using (var workplaceService = new WorkplaceService())
                    {                        
                        await LoadSolution(workplaceService, options);
                        var classes = await GetClassesInProject(workplaceService, options);
                        PrintMethodsInClasses(workplaceService, classes);
                        return;
                    }
                }
                else
                {
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
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            Console.WriteLine("Finished...");            
        }

        private static void PrintMethodsInClasses(WorkplaceService workplaceService, IEnumerable<string> classes)
        {
            foreach(var cls in classes)
            {
                Console.WriteLine("Methods in Class " + cls);
                PrintClasses(workplaceService, cls);
                
            }
        }

        private static void PrintClasses(WorkplaceService workplaceService, string cls)
        {
            var methods = workplaceService.GetMethodsOfClass(cls);
            foreach (var method in methods)
            {
                Console.WriteLine("\t " + method);
            }
        }

        private static async Task<IEnumerable<string>> GetClassesInProject(WorkplaceService workplaceService, ProgramOptions options)
        {
            await workplaceService.LoadProject(options.ProjectName);
            return workplaceService.GetAnalyzedClasses();           
        }

        private static async Task LoadSolution(WorkplaceService workplaceService, ProgramOptions options)
        {
            
                await workplaceService.LoadSolution(options.PathToSolution, options.ExcludingAssemblies, string.Empty, MSBUILD_PATH);                          
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

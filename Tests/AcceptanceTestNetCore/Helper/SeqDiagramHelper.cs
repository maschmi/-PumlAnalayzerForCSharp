using CodeAnalyzer.Context;
using CodeAnalyzer.InterfaceResolver;
using FluentAssertions;
using Logger;
using SequenceDiagram;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WorkspaceAnalyzer;

namespace AcceptanceTestNetCore.Helper
{
    internal class SeqDiagramHelper
    {
        private const string _resourcesNamespace = "AcceptanceTestNetCore.Resources";
        private const string _testDir = @"..";
        private const string _solutionName = "CodeDocumentations.sln";
        private const string _projectName = "DemoNet";        

        public async Task<string> CreateDiagram(string msBuildPath, string className, string methodName, string excludingAssemblies)
        {
            var solutionDir = Path.Combine(GetTestDataPath(), _testDir, _solutionName);
            
            var cfgCtx = new ConfigContext(InterfaceResolverType.ProjectLevel);
            IDoLog logger = new NullLogger();

            using (var solutionAnalyzer = new SolutionAnalyzer(solutionDir, msBuildPath, logger))
            {
                await solutionAnalyzer.LoadSolution(excludingAssemblies);

                var projectAnalyzer = new ProjectAnalyzer(solutionAnalyzer.ParsedSolution, solutionAnalyzer.OutputFiles, logger);
                await projectAnalyzer.LoadProject(_projectName);

                var interfaceResolver = InterfaceResolverFactory.GetInterfaceResolver(solutionAnalyzer, projectAnalyzer, logger, cfgCtx);

                var sequenceDiagram = new SequenceDiagramGenerator(projectAnalyzer.AnalyzedClasses, interfaceResolver, logger);
                return await sequenceDiagram.GetSequenceDiagramForMethod(className, methodName);
            }       
        }

        public static string GetTestDataPath()
        {
            var assemblyDir =  GetAssemblyDirectory();
            var projectDir = assemblyDir.Split(new string[] { "AcceptanceTest" }, 2, StringSplitOptions.None)[0];
            return projectDir;
        }


        private static string GetAssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        public void AssertResultMeetsExpectation(string result, string methodName)
        {
            string expectation = LoadResource(methodName);
            result.Should().Be(expectation);
        }

        private string LoadResource(string methodName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string result;
            
            using (var streamReader = new StreamReader(assembly.GetManifestResourceStream(_resourcesNamespace + "." + methodName + ".wsd")))
            {
                result = streamReader.ReadToEnd();
            }

            return result;                
        }
    }

}

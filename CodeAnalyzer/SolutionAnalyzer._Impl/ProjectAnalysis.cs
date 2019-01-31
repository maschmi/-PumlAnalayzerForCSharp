using Buildalyzer;
using Buildalyzer.Workspaces;
using CodeAnalyzer;
using CodeAnalyzer.SyntaxAnalysis;
using Logger;
using Microsoft.CodeAnalysis;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WorkspaceAnalyzer
{
    public class ProjectAnalysis : IProjectAnalysis
    {
        
        private readonly IDoLog _logger;
        private AnalyzerManager _manager;

        public IDictionary<string, IFileStructure> AnalyzedFiles { get; private set; }
        public IDictionary<string, IClassStructure> AnalyzedClasses { get; private set; }

        public ProjectAnalysis(AnalyzerManager manager, IDoLog logger)
        {
            _manager = manager;            

            if (logger == null)
                logger = new NullLogger();

            _logger = logger;
        }

        public async Task LoadProject(string project)
        {
            ProjectAnalyzer testProject = _manager.Projects.Where(k => k.Key.Contains(project)).FirstOrDefault().Value;
            if (testProject == null)
                throw new FileNotFoundException("Project file not found!");

            _logger.WriteLine("Loaded " + testProject);
            await AnalyzeProject(testProject);
        }

        private async Task AnalyzeProject(ProjectAnalyzer testProject)
        {
            using (var workspace = testProject.GetWorkspace())
            {                
                var projectToAnalyze = workspace.CurrentSolution.Projects.Where(p => p.FilePath == testProject.ProjectFile.Path).First();
                    
                var documents = projectToAnalyze.Documents
                   .Where(d => d.SourceCodeKind == SourceCodeKind.Regular && d.SupportsSyntaxTree == true)
                   .Where(d => !d.FilePath.Contains("AssemblyInfo"))
                   .Select(d => d.FilePath).ToArray();

                var solutionAssemblies = workspace.CurrentSolution.Projects.Select(p => p.OutputFilePath).ToList();
                AnalyzedFiles = await ParseFiles(documents, solutionAssemblies, projectToAnalyze);                
            }

            AnalyzedClasses = new Dictionary<string, IClassStructure>();
            foreach (var file in AnalyzedFiles)
            {
                var structure = file.Value;
                foreach (var cls in structure.Classes)
                    AnalyzedClasses.Add(cls.Classname, cls);
            }            
        }

        private async Task<IDictionary<string, IFileStructure>> ParseFiles(string[] documents, IEnumerable<string> solutionAssemblies, Project testProject)
        {
            var analyzedFiles = new ConcurrentDictionary<string, IFileStructure>();
            await Task.WhenAll(documents.Select(srcFile => ParseFileClasses(srcFile, solutionAssemblies, testProject, analyzedFiles)));
            return analyzedFiles;
        }

        private async Task ParseFileClasses(string file, IEnumerable<string> solutionAssemblies, Project testProject, ConcurrentDictionary<string, IFileStructure> analyzedFiles)
        {
            var analyzer = new FileStructureAnalyzer(file, solutionAssemblies, testProject.MetadataReferences);
            await analyzer.AnalyzeFile();
            analyzedFiles.TryAdd(file, analyzer.GetFileStrucuture());
        }
    }
}

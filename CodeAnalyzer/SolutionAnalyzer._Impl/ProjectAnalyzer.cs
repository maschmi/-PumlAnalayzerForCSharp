using CodeAnalyzer;
using CodeAnalyzer.SyntaxAnalysis;
using Logger;
using Microsoft.CodeAnalysis;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkspaceAnalyzer
{
    public class ProjectAnalyzer : IProjectAnalyzer
    {
        private Solution _solution;

        private readonly IDoLog _logger;

        public IDictionary<string, IFileStructure> AnalyzedFiles { get; private set; }
        public IDictionary<string, IClassStructure> AnalyzedClasses { get; private set; }

        public ProjectAnalyzer(IWorkspaceManager wsManager, IDoLog logger)
        {
            if (logger == null)
                logger = new NullLogger();

            _logger = logger;
        }

        public ProjectAnalyzer(Solution solution, IDoLog logger)
        {
            _solution = solution;
            if (logger == null)
                logger = new NullLogger();

            _logger = logger;
        }

        public async Task AnalyzeProject(Solution solution, string projectName)
        {
            _solution = solution;
            var project = _solution.Projects.First(p => p.Name == projectName);
            await AnalyzeProject(project);
        }

        public async Task AnalyzeProject(Project testProject)
        {
            if (_solution == null)
                _solution = testProject.Solution;

            var documents = testProject.Documents
                .Where(d => d.SourceCodeKind == SourceCodeKind.Regular && d.SupportsSyntaxTree == true)
                .Where(d => !d.FilePath.Contains("AssemblyInfo"))
                .Select(d => d.FilePath).ToArray();

            var referencedAssembliesInSoltuion = CalculateReferncedSolutionAssemblies(testProject);

            AnalyzedFiles = await ParseFiles(documents, referencedAssembliesInSoltuion, testProject);

            AnalyzedClasses = new Dictionary<string, IClassStructure>();
            foreach (var file in AnalyzedFiles)
            {
                var structure = file.Value;
                foreach (var cls in structure.Classes)
                    AnalyzedClasses.Add(cls.Classname, cls);
            }
        }

        private async Task<IDictionary<string, IFileStructure>> ParseFiles(string[] documents, IEnumerable<string> referencedAssembliesInSoltuion,  Project testProject)
        {
            var analyzedFiles = new ConcurrentDictionary<string, IFileStructure>();
            await Task.WhenAll(documents.Select(srcFile => ParseFileClasses(srcFile, referencedAssembliesInSoltuion, testProject, analyzedFiles)));
            return analyzedFiles;
        }

        private async Task ParseFileClasses(string file, IEnumerable<string> referencedAssembliesInSoltuion, Project testProject, ConcurrentDictionary<string, IFileStructure> analyzedFiles)
        {
            var analyzer = new FileStructureAnalyzer(file, referencedAssembliesInSoltuion, testProject.MetadataReferences);
            await analyzer.AnalyzeFile();
            analyzedFiles.TryAdd(file, analyzer.GetFileStrucuture());
        }

        private IEnumerable<string> CalculateReferncedSolutionAssemblies(Project testProject)
        {
            List<string> outputFiles = new List<string>();
            ProjectReference[] projectRefs = testProject.ProjectReferences.ToArray();

            if (_solution == null)
                return outputFiles;

            foreach (var reference in  projectRefs)
            {
                var referencedProject = _solution.Projects.First(p => p.Id == reference.ProjectId);
                outputFiles.Add(referencedProject.OutputFilePath);
                outputFiles.AddRange(CalculateReferncedSolutionAssemblies(referencedProject));
            }

            return outputFiles;
        }
    }
}

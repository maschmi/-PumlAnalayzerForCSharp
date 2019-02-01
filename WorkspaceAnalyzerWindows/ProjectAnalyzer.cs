using CodeAnalyzer.SyntaxAnalysis;
using Logger;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzer
{
    public class ProjectAnalyzer : IProjectAnalyzer
    {
        private readonly Solution _solution;
        private readonly IDoLog _logger;
        private IEnumerable<string> _outputFiles;

        public IDictionary<string, IFileStructure> AnalyzedFiles { get; private set; }
        public IDictionary<string, IClassStructure> AnalyzedClasses { get; private set; }

        public ProjectAnalyzer(Solution solution, IEnumerable<string> solutionOutputFiles, IDoLog logger)
        {
            _solution = solution;
            _outputFiles = solutionOutputFiles;

            if (logger == null)
                logger = new NullLogger();

            _logger = logger;
        }

        public async Task LoadProject(string project)
        {
            var testProject = _solution.Projects.Where(p => p.Name == project).FirstOrDefault();
            _logger.WriteLine("Loaded " + testProject.Name);
            await AnalyzeProject(testProject, _outputFiles);
        }

        private async Task AnalyzeProject(Project testProject, IEnumerable<string> solutionAssemblies)
        {
            var documents = testProject.Documents
                .Where(d => d.SourceCodeKind == SourceCodeKind.Regular && d.SupportsSyntaxTree == true)
                .Where(d => !d.FilePath.Contains("AssemblyInfo"))
                .Select(d => d.FilePath).ToArray();

            AnalyzedFiles = await ParseFiles(documents, solutionAssemblies, testProject);

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

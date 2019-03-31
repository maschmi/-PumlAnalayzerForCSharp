using System.Collections.Generic;
using System.Threading.Tasks;
using CodeAnalyzer.SyntaxAnalysis;
using Microsoft.CodeAnalysis;

namespace CodeAnalyzer
{
    public interface IProjectAnalyzer
    {
        IDictionary<string, IClassStructure> AnalyzedClasses { get; }
        IDictionary<string, IFileStructure> AnalyzedFiles { get; }

        Task AnalyzeProject(Solution solution, string projectName);
        Task AnalyzeProject(Project project);
    }
}
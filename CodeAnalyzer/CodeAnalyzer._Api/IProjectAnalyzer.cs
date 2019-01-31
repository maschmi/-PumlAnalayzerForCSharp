using System.Collections.Generic;
using System.Threading.Tasks;
using CodeAnalyzer.SyntaxAnalysis;

namespace CodeAnalyzer
{
    public interface IProjectAnalyzer
    {
        IDictionary<string, IClassStructure> AnalyzedClasses { get; }
        IDictionary<string, IFileStructure> AnalyzedFiles { get; }

        Task LoadProject(string project);
    }
}
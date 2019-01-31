using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace CodeAnalyzer.SyntaxAnalysis
{
    public interface IFileStructureAnalyzer
    {
        Task AnalyzeFile();
        string GetFilename();
        IFileStructure GetFileStrucuture();
        IEnumerable<MetadataReference> GetMetadataReferences();
        string GetPath();
    }
}
using System.Text;
using System.Threading.Tasks;
using CodeAnalyzer.SyntaxAnalysis;
using Microsoft.CodeAnalysis;

namespace SequenceDiagram
{
    public interface ISequenceDiagram
    {
        Task<string> GetSequenceDiagramForMethod(string className, string methodName);
    }
}
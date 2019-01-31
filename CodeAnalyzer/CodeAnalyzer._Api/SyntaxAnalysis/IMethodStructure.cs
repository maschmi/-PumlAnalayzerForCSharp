using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace CodeAnalyzer.SyntaxAnalysis
{
    public interface IMethodStructure
    {
        string MethodName { get; }
        ConcurrentQueue<(SyntaxNode, ISymbol)> NodeSymbols { get; }
        string[] Parameters { get; }
        string ReturnType { get; }
        
        IMethodSymbol GetMethodSymbol();
        SemanticModel GetSemanticModel();

        Task ParseMethod();
    }
}
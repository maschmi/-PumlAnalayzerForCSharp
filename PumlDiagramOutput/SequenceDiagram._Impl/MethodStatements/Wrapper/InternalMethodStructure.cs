using CodeAnalyzer.SyntaxAnalysis;
using Microsoft.CodeAnalysis;
using SequenceDiagram.MethodStatements.Scopes;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SequenceDiagram.MethodStatements.Wrapper
{
    internal class InternalMethodStructure : IMethodStructure
    {
        public ConcurrentStack<StatementScope> CurrentScopre { get;  } = new ConcurrentStack<StatementScope>();

        private readonly IMethodStructure _methodStructure;
        public bool IsStartMethod = false;

        public string MethodName => _methodStructure.MethodName;

        public ConcurrentQueue<(SyntaxNode, ISymbol)> NodeSymbols => _methodStructure.NodeSymbols;

        public string[] Parameters => _methodStructure.Parameters;

        public string ReturnType => _methodStructure.ReturnType;

        public IMethodSymbol GetMethodSymbol()
            => _methodStructure.GetMethodSymbol();

        public SemanticModel GetSemanticModel()
            => _methodStructure.GetSemanticModel();

        public async Task ParseMethod()
            => await _methodStructure.ParseMethod();
        
        public InternalMethodStructure(IMethodStructure fromCodeAnalyzer, bool startMethod = false)
        {
            _methodStructure = fromCodeAnalyzer;
            IsStartMethod = startMethod;
        }
    }


}

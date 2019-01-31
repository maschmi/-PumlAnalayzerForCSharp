using CodeAnalyzer.SyntaxAnalysis;
using Microsoft.CodeAnalysis;

namespace SequenceDiagram.MethodStatements.Scopes
{
    internal interface IStatementScope
    {
        string EndStatement { get; }
        IMethodStructure Method { get; }
        int SpanEnd { get; }

        bool WriteEndStatement(IMethodStructure currentMethod, int currentSpan);
        bool WriteEndStatement(IMethodStructure currentMethod, SyntaxNode currentNode);
    }
}
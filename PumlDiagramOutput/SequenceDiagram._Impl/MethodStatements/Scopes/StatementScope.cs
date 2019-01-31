using CodeAnalyzer.SyntaxAnalysis;
using Microsoft.CodeAnalysis;

namespace SequenceDiagram.MethodStatements.Scopes
{
    internal class StatementScope : IStatementScope
    {
        public IMethodStructure Method { get;}
        public int SpanEnd { get; }
        public string EndStatement { get; }        

        public StatementScope(IMethodStructure method, SyntaxNode currentNode, string endStatement)
        {
            Method = method;
            SpanEnd = currentNode.Span.End;
            EndStatement = endStatement;
        }

        public bool WriteEndStatement(IMethodStructure currentMethod, int currentSpan)
        {
            return (Method == currentMethod && currentSpan >= SpanEnd);
        }

        public bool WriteEndStatement(IMethodStructure currentMethod, SyntaxNode currentNode)
        {            
            return (Method == currentMethod && currentNode.Span.Start > SpanEnd);
        }

    }

}

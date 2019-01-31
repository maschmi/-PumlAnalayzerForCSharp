using System;
using System.Text;
using System.Threading.Tasks;
using CodeAnalyzer.InterfaceResolver;
using CodeAnalyzer.SyntaxAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SequenceDiagram.Extensions;
using SequenceDiagram.MethodStatements.Scopes;
using SequenceDiagram.MethodStatements.Wrapper;

namespace SequenceDiagram.MethodStatements
{
    internal class ObjectCreationStatement : AbstractSyntaxDrawer
    {
        private const string _startTag = "group create ";
        private const string _endTag = "end";        

        public override Type[] ValidNode { get; } = new Type[] { typeof(ObjectCreationExpressionSyntax) };

        public ObjectCreationStatement(IInterfaceResolver interfaceResolver, ISyntaxDrawerFactory drawerFactory, StringBuilder diagram, string entryMethod)
                 : base(interfaceResolver, drawerFactory, diagram, entryMethod)
        {
        }

        public override async Task WriteDiagramSyntax((SyntaxNode, ISymbol) syntaxSymbol, IMethodStructure method, int depth, string calledMethod, IStatementScope special = null)
        {
            if (!AmIResponsible(syntaxSymbol.Item1))
                return;

            string caller = caller = method.GetMethodSymbol().ContainingSymbol.ToDisplayString();

            if (((InternalMethodStructure)method).IsStartMethod)
                caller = caller + "." + method.MethodName;

            caller = caller.MaskSpecialChars();

            var newSyntaxTree = (ObjectCreationExpressionSyntax)syntaxSymbol.Item1.TrackNodes();

            Diagram.AppendLine(_startTag + newSyntaxTree.Type.ToString().MaskSpecialChars());

            Diagram.AppendLine(caller + " -> " +
                newSyntaxTree.Type.ToString().MaskSpecialChars() +
                " : " + newSyntaxTree.NewKeyword.ToString().MaskSpecialChars() +
                newSyntaxTree.ArgumentList.ToString().MaskSpecialChars());

            Diagram.AppendLine(newSyntaxTree.Type.ToString().MaskSpecialChars() + " --> " + caller);

            var scope = new StatementScope(method, syntaxSymbol.Item1, _endTag);
            await AnalyzeMethod(method, depth, scope);
        }
    }
}

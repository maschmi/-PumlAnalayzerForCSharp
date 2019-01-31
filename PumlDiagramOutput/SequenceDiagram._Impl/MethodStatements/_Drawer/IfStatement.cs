using System;
using System.Text;
using System.Threading.Tasks;
using CodeAnalyzer.InterfaceResolver;
using CodeAnalyzer.SyntaxAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SequenceDiagram.Extensions;
using SequenceDiagram.MethodStatements.Scopes;

namespace SequenceDiagram.MethodStatements
{
    internal class IfStatement : AbstractSyntaxDrawer
    {        
        private const string _altTag = "alt ";
        private const string _elseTag = "else ";
        private const string _endTag = "end";
        
        public override Type[] ValidNode { get; } = new Type[] { typeof(IfStatementSyntax) };

        public IfStatement(IInterfaceResolver interfaceResolver, ISyntaxDrawerFactory drawerFactory, StringBuilder diagram, string entryMethod)
                  : base(interfaceResolver, drawerFactory, diagram, entryMethod)
        {
        }

        public override async Task WriteDiagramSyntax((SyntaxNode, ISymbol) syntaxSymbol, IMethodStructure method, int depth, string entryMethod, IStatementScope special)
        {
            if(!AmIResponsible(syntaxSymbol.Item1))            
                return;

            var ifSyntaxTree = (IfStatementSyntax)syntaxSymbol.Item1.TrackNodes();
            _logger.Debug(method.MethodName + "\tStart Of If");
            Diagram.AppendLine(_altTag + ifSyntaxTree.Condition.ToString().MaskSpecialChars());

            var scope = new StatementScope(method, syntaxSymbol.Item1, _endTag);
            await AnalyzeMethod(method, depth, scope);
            _logger.Debug(method.MethodName + "\tEnd Of If");            
        }
    }
}

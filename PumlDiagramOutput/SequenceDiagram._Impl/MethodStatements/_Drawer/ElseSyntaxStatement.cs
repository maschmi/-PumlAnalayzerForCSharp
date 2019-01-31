using System;
using System.Text;
using System.Threading.Tasks;
using CodeAnalyzer.InterfaceResolver;
using CodeAnalyzer.SyntaxAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SequenceDiagram.MethodStatements.Scopes;

namespace SequenceDiagram.MethodStatements
{
    internal class ElseSyntaxStatement : AbstractSyntaxDrawer
    {        
        private const string _elseTag = "else ";
        private const string _endTag = "";
        
        public override Type[] ValidNode { get; } = new Type[] { typeof(ElseClauseSyntax) };

        public ElseSyntaxStatement(IInterfaceResolver interfaceResolver, ISyntaxDrawerFactory drawerFactory, StringBuilder diagram, string entryMethod)
                  : base(interfaceResolver, drawerFactory, diagram, entryMethod)
        {
        }

        public override async Task WriteDiagramSyntax((SyntaxNode, ISymbol) syntaxSymbol, IMethodStructure method, int depth, string entryMethod, IStatementScope special)
        {
            if (!AmIResponsible(syntaxSymbol.Item1))
                return;
            
            var ifSyntaxTree = (ElseClauseSyntax)syntaxSymbol.Item1.TrackNodes();
            Diagram.AppendLine(_elseTag);
            _logger.Debug(method.MethodName + "\tStart Of Else");
            var scope = new StatementScope(method, syntaxSymbol.Item1, _endTag);
            await AnalyzeMethod(method, depth, scope);
            _logger.Debug(method.MethodName + "\tEnd Of Else");
        }
    }
}

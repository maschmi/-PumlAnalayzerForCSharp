using CodeAnalyzer.InterfaceResolver;
using CodeAnalyzer.SyntaxAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SequenceDiagram.Extensions;
using SequenceDiagram.MethodStatements.Scopes;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDiagram.MethodStatements
{
    internal class SwitchSyntaxStatement : AbstractSyntaxDrawer
    {
        private const string _startTag = "group switch";
        private const string _altTag = "alt ";
        private const string _elseTag = "else ";
        private const string _elseIfTag = "else alt ";
        private const string _endTag = "end";        

        public override Type[] ValidNode { get; } = new Type[] { typeof(SwitchStatementSyntax) };

        public SwitchSyntaxStatement(IInterfaceResolver interfaceResolver, ISyntaxDrawerFactory drawerFactory, StringBuilder diagram, string entryMethod)
                  : base(interfaceResolver, drawerFactory, diagram, entryMethod)
        {
        }

        public override async Task WriteDiagramSyntax((SyntaxNode, ISymbol) syntaxSymbol, IMethodStructure method, int depth, string entryMethod, IStatementScope special)
        {
            if (!AmIResponsible(syntaxSymbol.Item1))
                return;
                        

            var switchSyntaxTree = (SwitchStatementSyntax)syntaxSymbol.Item1.TrackNodes();
            string switchOn = switchSyntaxTree.Expression.ToString();
            _logger.Debug(method.MethodName + "\tStart Of Switch");
            Diagram.AppendLine(_startTag +  " [" + switchOn +"]");

            var endStatement = syntaxSymbol.Item1.Span.End;
            var scope = new StatementScope(method, syntaxSymbol.Item1, _endTag);
            await AnalyzeMethod(method, depth, scope);
            _logger.Debug(method.MethodName + "\tEnd Of Switch");            
        }
    }
}


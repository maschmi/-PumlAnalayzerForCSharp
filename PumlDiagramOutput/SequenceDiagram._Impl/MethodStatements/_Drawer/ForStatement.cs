
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
    internal class ForStatement : AbstractSyntaxDrawer
    {
        private const string _loopStart = "loop ";
        private const string _loopEnd = "end";

        public override Type[] ValidNode { get; } = new Type[] { typeof(ForStatementSyntax) };

        public ForStatement(IInterfaceResolver interfaceResolver, ISyntaxDrawerFactory drawerFactory, StringBuilder diagram, string entryMethod)
                  : base(interfaceResolver, drawerFactory, diagram, entryMethod)
        {
        }

        public override async Task WriteDiagramSyntax((SyntaxNode, ISymbol) syntaxSymbol, IMethodStructure method, int depth, string calledMethod, IStatementScope special)
        {
            if (!AmIResponsible(syntaxSymbol.Item1))
                return;
            
            var forSyntaxTree = (ForStatementSyntax)syntaxSymbol.Item1.TrackNodes();
            var loopCondition = forSyntaxTree.Condition.ToString();

            _logger.Debug(method.MethodName + "\tStart of For "+loopCondition);

            Diagram.AppendLine(_loopStart + loopCondition.MaskSpecialChars());

            var scope = new StatementScope(method, syntaxSymbol.Item1, _loopEnd);
            await AnalyzeMethod(method, depth, scope);
            _logger.Debug(method.MethodName+"\t End of For");
        }
    }
}

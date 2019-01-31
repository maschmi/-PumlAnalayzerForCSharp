
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
    internal class ForEachStatement : AbstractSyntaxDrawer
    {
        private const string _loopStart = "loop ";
        private const string _loopEnd = "end";

        public override Type[] ValidNode { get; } = new Type[] { typeof(ForEachStatementSyntax) };

      
        public ForEachStatement(IInterfaceResolver interfaceResolver, ISyntaxDrawerFactory drawerFactory, StringBuilder diagram, string entryMethod)
                  : base(interfaceResolver, drawerFactory, diagram, entryMethod)
        {
        }

        public override async Task WriteDiagramSyntax((SyntaxNode, ISymbol) syntaxSymbol, IMethodStructure method, int depth, string calledMethod, IStatementScope special)
        {
            if (!AmIResponsible(syntaxSymbol.Item1))
                return;

            var forEachSyntaxTree = (ForEachStatementSyntax)syntaxSymbol.Item1.TrackNodes();
            var loopCondition = forEachSyntaxTree.Expression.ToString();
            _logger.Debug(method.MethodName + "\tStart of Foreach " + loopCondition);
            Diagram.AppendLine(_loopStart + loopCondition.MaskSpecialChars());

            var scope = new StatementScope(method, syntaxSymbol.Item1, _loopEnd);
            await AnalyzeMethod(method, depth, scope);            
            _logger.Debug(method.MethodName + "\tEnd of ForEach");            
        }
    }
}

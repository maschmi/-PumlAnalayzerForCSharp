using System;
using System.Collections.Generic;
using System.Linq;
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
    internal class ReturnSyntaxStatement : AbstractSyntaxDrawer
    {
        private const string _startTag = "group return ";
        private const string _endTag = "end";

        public override Type[] ValidNode { get; } = new Type[] { typeof(ReturnStatementSyntax) };

        public ReturnSyntaxStatement(IInterfaceResolver interfaceResolver, ISyntaxDrawerFactory drawerFactory, StringBuilder diagram, string entryMethod)
                  : base(interfaceResolver, drawerFactory, diagram, entryMethod)
        {
        }

        public override async Task WriteDiagramSyntax((SyntaxNode, ISymbol) syntaxSymbol, IMethodStructure method, int depth, string calledMethod, IStatementScope special = null)            
        {
            var node = (ReturnStatementSyntax)syntaxSymbol.Item1;
            var expression = node.Expression.ToString();
            Diagram.AppendLine(_startTag + expression.MakeItSingleLine().MaskSpecialChars());
            var scope = new StatementScope(method, node, _endTag);
            await AnalyzeMethod(method, depth, scope);
        }
    }
}

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
    internal class SwitchSectionStatement : AbstractSyntaxDrawer
    {
        private const string _altTag = "group case";
        private const string _elseTag = "group case";        
        private const string _endTag = "end";
        
        public override Type[] ValidNode => new Type[] { typeof(SwitchSectionSyntax) };

        public SwitchSectionStatement(IInterfaceResolver interfaceResolver, ISyntaxDrawerFactory drawerFactory, StringBuilder diagram, string entryMethod)
                  : base(interfaceResolver, drawerFactory, diagram, entryMethod)
        {
        }

        public override async Task WriteDiagramSyntax((SyntaxNode, ISymbol) syntaxSymbol, IMethodStructure method, int depth, string calledMethod, IStatementScope special)
        {
            if (!AmIResponsible(syntaxSymbol.Item1))
                return;

            var switchOn = string.Empty;
            var currentTag = _altTag;
            var conditions = new StringBuilder();
            _logger.Debug(method.MethodName + "\tStart Of SwitchSection");
            while (method.NodeSymbols.TryPeek(out (SyntaxNode, ISymbol) testSymbol) && testSymbol.Item1 is SwitchLabelSyntax)
            {
                if (!method.NodeSymbols.TryDequeue(out (SyntaxNode, ISymbol) nodeSymbol))
                    break;
                
                if (!(nodeSymbol.Item1 is SwitchLabelSyntax currentLabel))
                    break;

                if (currentLabel.Keyword.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.DefaultKeyword))
                {
                    conditions.Clear();
                    conditions.Append("default");
                    switchOn = string.Empty;
                    break;
                }

                if (conditions.Length != 0)
                    conditions.Append(" || ");

                conditions.Append(currentLabel.ToString().Substring(5).TrimEnd(':'));
            }

            Diagram.AppendLine(currentTag + " " + conditions.ToString().MaskSpecialChars() + ":");
            var scope = new StatementScope(method, syntaxSymbol.Item1, _endTag);
            await AnalyzeMethod(method, depth, scope);
            _logger.Debug(method.MethodName + "\tEnd Of SwitchSection");
        }
    }
}
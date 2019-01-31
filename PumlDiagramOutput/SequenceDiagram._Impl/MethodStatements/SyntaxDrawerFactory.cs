using CodeAnalyzer.InterfaceResolver;
using CodeAnalyzer.SyntaxAnalysis;
using Logger;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Text;

namespace SequenceDiagram.MethodStatements
{
    internal class SyntaxDrawerFactory : ISyntaxDrawerFactory
    {
        private readonly IDictionary<string, IClassStructure> _analyzedClasses;
        private readonly string _entryMethod;
        private readonly IDoLog _logger;

        public SyntaxDrawerFactory(IDictionary<string, IClassStructure> analyzedClasses, string entryMethod, IDoLog logger)
        {
            _analyzedClasses = analyzedClasses;
            _entryMethod = entryMethod;
            if (logger == null)
                logger = new NullLogger();

            _logger = logger;
        }

        public string EntryMethod { get; }

        public AbstractSyntaxDrawer GetDrawer(SyntaxNode node, IInterfaceResolver interfaceResolver, StringBuilder diagram)
        {
            
            if (node is InvocationExpressionSyntax)
            {
                var drawer = new CallSyntaxStatement(interfaceResolver, diagram, this, _entryMethod, _analyzedClasses);
                drawer.AppendLogger(_logger);
                return drawer;
            }

            if (node is ElseClauseSyntax)
            {
                var drawer = new ElseSyntaxStatement(interfaceResolver, this, diagram, _entryMethod);
                drawer.AppendLogger(_logger);
                return drawer;
            }

            if (node is ForEachStatementSyntax)
            {
                var drawer = new ForEachStatement(interfaceResolver, this, diagram, _entryMethod);
                drawer.AppendLogger(_logger);
                return drawer;
            }

            if (node is ForStatementSyntax)
            {
                var drawer = new ForStatement(interfaceResolver, this, diagram, _entryMethod);
                drawer.AppendLogger(_logger);
                return drawer;
            }

            if (node is IfStatementSyntax)
            {
                var drawer = new IfStatement(interfaceResolver, this, diagram, _entryMethod);
                drawer.AppendLogger(_logger);
                return drawer;
            }

            if (node is SwitchStatementSyntax)
            {
                var drawer =  new SwitchSyntaxStatement(interfaceResolver, this, diagram, _entryMethod);
                drawer.AppendLogger(_logger);
                return drawer;
            }

            if (node is SwitchSectionSyntax)
            {
                var drawer = new SwitchSectionStatement(interfaceResolver, this, diagram, _entryMethod);
                drawer.AppendLogger(_logger);
                return drawer;
            }

            if (node is ObjectCreationExpressionSyntax)
            {
                var drawer =  new ObjectCreationStatement(interfaceResolver, this, diagram, _entryMethod);
                drawer.AppendLogger(_logger);
                return drawer;
            }

            if (node is ReturnStatementSyntax)
            {
                var drawer = new ReturnSyntaxStatement(interfaceResolver, this, diagram, _entryMethod);
                drawer.AppendLogger(_logger);
                return drawer;
            }

            return new NullDrawer(interfaceResolver, this, diagram, _entryMethod);
        }        
    }
}

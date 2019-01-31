using System.Text;
using CodeAnalyzer.InterfaceResolver;
using Microsoft.CodeAnalysis;

namespace SequenceDiagram.MethodStatements
{
    internal interface ISyntaxDrawerFactory
    {
        AbstractSyntaxDrawer GetDrawer(SyntaxNode node, IInterfaceResolver interfaceResolver, StringBuilder diagram);        
    }
}
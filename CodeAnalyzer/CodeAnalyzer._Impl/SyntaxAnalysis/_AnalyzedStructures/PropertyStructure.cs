using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalyzer.SyntaxAnalysis
{
    public class PropertyStructure : IPropertyStructure
    {
        private readonly ClassStructure _classStructure;
        private readonly PropertyDeclarationSyntax _prop;

        public PropertyStructure(ClassStructure classStructure, PropertyDeclarationSyntax prop)
        {
            _classStructure = classStructure;
            _prop = prop;
        }
    }
}
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalyzer.SyntaxAnalysis
{
    public class ConstructorStructure : IConstructorStructure
    {
        private readonly IClassStructure _classStructure;
        private readonly ConstructorDeclarationSyntax _ctor;

        public ConstructorStructure(ClassStructure classStructure, ConstructorDeclarationSyntax ctor)
        {
            _classStructure = classStructure;
            _ctor = ctor;
        }
    }
}
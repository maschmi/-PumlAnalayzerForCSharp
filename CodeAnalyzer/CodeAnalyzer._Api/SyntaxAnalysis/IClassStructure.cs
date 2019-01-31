using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalyzer.SyntaxAnalysis
{
    public interface IClassStructure
    {
        string Classname { get; }
        List<IConstructorStructure> Constructors { get; }
        string Filename { get; }
        ClassDeclarationSyntax GetClassDeclaration { get; }
        List<IMethodStructure> Methods { get; }
        NamespaceDeclarationSyntax NamespaceDeclaration { get; }
        string Path { get; }
        List<IPropertyStructure> Properties { get; }
        IEnumerable<MetadataReference> References { get; }
        IEnumerable<UsingDirectiveSyntax> Usings { get; }

        CSharpCompilation GetCompilation();
        SemanticModel GetSemanticModel();
        void ParseClass();
    }
}
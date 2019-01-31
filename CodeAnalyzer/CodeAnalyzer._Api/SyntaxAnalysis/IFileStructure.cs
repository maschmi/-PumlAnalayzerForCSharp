using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalyzer.SyntaxAnalysis
{
    public interface IFileStructure
    {
        List<IClassStructure> Classes { get; }
        string Filename { get; }
        List<NamespaceDeclarationSyntax> Namespaces { get; }
        string Path { get; }
        List<MetadataReference> References { get; }
        List<UsingDirectiveSyntax> Usings { get; }
    }
}
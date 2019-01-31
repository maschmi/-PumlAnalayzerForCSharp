using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzer.SyntaxAnalysis
{
    /// <summary>
    /// DTO to hold a file structure
    /// </summary>
    public class FileStructure : IFileStructure
    {
        private readonly string _filename;
        private readonly string _path;

        public string Filename => _filename;
        public string Path => _path;

        public List<UsingDirectiveSyntax> Usings { get; } = new List<UsingDirectiveSyntax>();
        public List<NamespaceDeclarationSyntax> Namespaces { get; } = new List<NamespaceDeclarationSyntax>();
        public List<MetadataReference> References { get; set; } = new List<MetadataReference>();
        public List<IClassStructure> Classes { get; } = new List<IClassStructure>();

        public FileStructure(string path, string filename)
        {
            _filename = filename;
            _path = path;
        }        
    }
}

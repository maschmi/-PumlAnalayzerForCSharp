using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzer.SyntaxAnalysis
{
    public class FileStructureAnalyzer : IFileStructureAnalyzer
    {
        private readonly string _filename;
        private readonly string _path;
        private readonly string _fullPath;
        private string _sourceCode;
        private SyntaxTree _fileSyntaxTree;
        private CompilationUnitSyntax _fileSytanxRoot;
        private IFileStructure _fileStructure;
        private readonly List<MetadataReference> _references;

        
        private readonly object _lock = new object();

        public IEnumerable<MetadataReference> GetMetadataReferences() => _references;

        public string GetPath()
        {
            return _path;
        }

        public string GetFilename()
        {
            return _filename;
        }
        
        public IFileStructure GetFileStrucuture()
        {
            if (_fileStructure == null)
                throw new InvalidOperationException("File needs to be analyzed first");

            return _fileStructure;
        }

        public FileStructureAnalyzer(string filePath, IEnumerable<string> projectAssemblies, IReadOnlyList<MetadataReference> references)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();            

            _filename = Path.GetFileName(filePath);
            _path = Path.GetDirectoryName(filePath);
            _fullPath = filePath;
            _references = references.ToList();
            foreach(var assembly in projectAssemblies)
            {
                if (!File.Exists(assembly))
                    continue;
                _references.Add(MetadataReference.CreateFromFile(assembly));
            }
            
        }       

        public async Task AnalyzeFile()
        {
            await ReadFileContent();
            _fileStructure = new FileStructure(_path, _filename);
            _fileSyntaxTree = CSharpSyntaxTree.ParseText(_sourceCode);
            _fileSytanxRoot = _fileSyntaxTree.GetCompilationUnitRoot();            
            _fileStructure.References.AddRange(_references);
            _fileStructure.Usings.AddRange(await ExtractUsings());
            var nameSpaces = await ExtractNamespaces();
            _fileStructure.Namespaces.AddRange(nameSpaces);

            foreach (var ns in nameSpaces)
                _fileStructure.Classes.AddRange(await ExtractClasses(ns));            
        }

        private async Task<List<ClassStructure>> ExtractClasses(NamespaceDeclarationSyntax ns)
        {            
            List<ClassStructure> parsedClasses = await ParseClasses(ns.Members.OfType<ClassDeclarationSyntax>(), ns);
            return parsedClasses;
        }

        private async Task<List<ClassStructure>> ParseClasses(IEnumerable<ClassDeclarationSyntax> classes, NamespaceDeclarationSyntax ns)
        {
            List<ClassStructure> parsedClasses = new List<ClassStructure>();

            foreach (var cls in classes)
                await Task.Run(() => ParseClass(cls, ns, parsedClasses));

            return parsedClasses;
        }

        private void ParseClass(ClassDeclarationSyntax cls, NamespaceDeclarationSyntax ns, List<ClassStructure> parsedClasses)
        {
            var parsedClass = new ClassStructure(cls, ns, _fileStructure.References, _fileStructure.Usings, _fileStructure.Filename, _fileStructure.Path);
            parsedClass.ParseClass();
            lock (_lock)
                parsedClasses.Add(parsedClass);
        }

        private Task<List<UsingDirectiveSyntax>> ExtractUsings()
        {
            return Task.FromResult(_fileSytanxRoot.Usings.ToList());
        }
        
        private Task<List<NamespaceDeclarationSyntax>> ExtractNamespaces()
        {
            return Task.FromResult(_fileSytanxRoot.Members.OfType<NamespaceDeclarationSyntax>().ToList());
        }

        private async Task ReadFileContent()
        {
            using (var srcFile = File.OpenText(_fullPath))
            {
                _sourceCode = await srcFile.ReadToEndAsync();
            }
        }
    }
}

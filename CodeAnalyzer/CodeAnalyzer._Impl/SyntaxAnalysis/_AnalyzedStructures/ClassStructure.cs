using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzer.SyntaxAnalysis
{
    public class ClassStructure : IClassStructure
    {
        private readonly NamespaceDeclarationSyntax _namespace;
        private readonly ClassDeclarationSyntax _classdeclaration;
        private readonly string _filename;
        private readonly string _path;
        private readonly List<UsingDirectiveSyntax> _usingDirectives;
        private readonly List<MetadataReference> _references;
        private readonly string _classname;
        private Lazy<CSharpCompilation> _compilation;
        private Lazy<SemanticModel> _semanticModel;        

        public string Filename => _filename;
        public string Path => _path;
        public string Classname => _classname;
        public ClassDeclarationSyntax GetClassDeclaration => _classdeclaration;
        public NamespaceDeclarationSyntax NamespaceDeclaration => _namespace;
        public IEnumerable<UsingDirectiveSyntax> Usings => _usingDirectives;
        public IEnumerable<MetadataReference> References => _references;

        public List<IPropertyStructure> Properties { get; } = new List<IPropertyStructure>();
        public List<IConstructorStructure> Constructors { get; } = new List<IConstructorStructure>();
        public List<IMethodStructure> Methods { get; } = new List<IMethodStructure>();

        public ClassStructure(ClassDeclarationSyntax csd, NamespaceDeclarationSyntax ns, List<MetadataReference> references, IEnumerable<UsingDirectiveSyntax> usings, string filename, string path)
        {
            if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(path))
                throw new ArgumentNullException();

            _namespace = ns;
            _filename = filename;
            _path = path;
            _classdeclaration = csd;
            _classname = _classdeclaration.Identifier.ToString();
            _usingDirectives = new List<UsingDirectiveSyntax>(usings);
            _references = references;
            _compilation = new Lazy<CSharpCompilation>(() => GenerateCompilation(_namespace.ToString(), _references, csd.SyntaxTree));
            _semanticModel = new Lazy<SemanticModel>(() => _compilation.Value.GetSemanticModel(csd.SyntaxTree));
        }

        private static CSharpCompilation GenerateCompilation(string nspace, List<MetadataReference> refs, SyntaxTree tree)
        {
            return CSharpCompilation.Create(nspace)
               .WithReferences(refs)
               .AddSyntaxTrees(tree);
        }

        public void ParseClass()
        {
            ParseProperties();
            ParseConstructors();
            ParseMethods();
        }

        public CSharpCompilation GetCompilation() => _compilation.Value;        
        public SemanticModel GetSemanticModel() => _semanticModel.Value;

        private void ParseMethods()
        {
            var methods = _classdeclaration.Members.OfType<MethodDeclarationSyntax>();
            foreach (var method in methods)
            {
                Methods.Add(new MethodStructure(this, method));
            }
        }

        private void ParseConstructors()
        {
            var ctors = _classdeclaration.Members.OfType<ConstructorDeclarationSyntax>();
            foreach(var ctor in ctors)
            {
                Constructors.Add(new ConstructorStructure(this, ctor));
            }            
        }

        private void ParseProperties()
        {
            var properties = _classdeclaration.Members.OfType<PropertyDeclarationSyntax>();
            foreach(var prop in properties)
            {
                Properties.Add(new PropertyStructure(this, prop));
            }            
        }
    }
}

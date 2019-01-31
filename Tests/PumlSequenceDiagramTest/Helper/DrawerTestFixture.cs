using CodeAnalyzer.InterfaceResolver;
using CodeAnalyzer.SyntaxAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;
using SequenceDiagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PumlDiagramTest.Helper
{
    internal class DrawerTestFixture
    {        
        public MethodDeclarationSyntax MethodDeclaration { get; private set; }
        public SequenceDiagramGenerator DiagramGenerator { get; private set; }

        public DrawerTestFixture(string sourceCode, string className, string methodName)
        {        
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            var syntaxRoot = syntaxTree.GetCompilationUnitRoot();
            var ns = syntaxRoot.Members.OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
            var classes = ns.Members.OfType<ClassDeclarationSyntax>().FirstOrDefault();
            var usings = syntaxRoot.Members.OfType<UsingDirectiveSyntax>().ToArray();
            var references = Mock.Of<List<MetadataReference>>();

            var classStructure = new ClassStructure(classes, ns, references, usings, "test", "memory");
            classStructure.ParseClass();
            MethodDeclaration = classes.Members.OfType<MethodDeclarationSyntax>()
                .Where(m => m.Identifier.ToString().Contains(methodName)).FirstOrDefault();
            var methodStructure = new MethodStructure(classStructure, MethodDeclaration);

            var analyzedClasses = Mock.Of<IDictionary<string, IClassStructure>>();
            Mock.Get(analyzedClasses).Setup(d => d[It.IsAny<string>()]).Returns(classStructure);
            var interfaceResolver = Mock.Of<IInterfaceResolver>();
            DiagramGenerator = new SequenceDiagramGenerator(analyzedClasses, interfaceResolver, null);
        }        
    }
}

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
    public class MethodSyntaxWalker : CSharpSyntaxWalker
    {
        private List<SyntaxNode> _methodSyntax = new List<SyntaxNode>();


        public MethodSyntaxWalker() : base(SyntaxWalkerDepth.Node)
        {

        }

        public override void Visit(SyntaxNode node)
        {
            _methodSyntax.Add(node);
            base.Visit(node);
        }

        //public override void VisitToken(SyntaxToken token)
        //{
        //    _methodSyntax.Add(token);
        //    base.VisitToken(token);
        //}

        public async Task<IEnumerable<SyntaxNode>> Run(MethodDeclarationSyntax method)
        {
            SyntaxNode root = GetMethodRoot(method);
            await Task.Run(() => Visit(root));
            return _methodSyntax.AsEnumerable();
        }

        private SyntaxNode GetMethodRoot(MethodDeclarationSyntax method)
        {
            return method.ChildNodes().First().Parent;
        }


    }
}

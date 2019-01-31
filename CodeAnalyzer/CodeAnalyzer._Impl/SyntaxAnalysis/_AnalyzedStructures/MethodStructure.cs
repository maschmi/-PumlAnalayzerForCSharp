using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalyzer.SyntaxAnalysis
{
    public class MethodStructure : IMethodStructure
    {
        private readonly ClassStructure _classStructure;
        private readonly MethodDeclarationSyntax _method;        
        private readonly List<SyntaxNode> _methodSyntax = new List<SyntaxNode>(50);
        
        private readonly Queue<(SyntaxNode, ISymbol)> _originalNodeSymbols = new Queue<(SyntaxNode, ISymbol)>();
        
        private bool _alreadyParsed = false;

        private IMethodSymbol _methodSymbol;
        private SyntaxTree _methodSyntaxTree;
        private readonly string _name;
        
        private ConcurrentQueue<(SyntaxNode, ISymbol)> _workingCopyOfnodeSymbols;

        public IMethodSymbol GetMethodSymbol() => _methodSymbol;
        public string MethodName => _name;

        public string[] Parameters { get; private set; }
        public string ReturnType { get; private set; }
        public ConcurrentQueue<(SyntaxNode, ISymbol)> NodeSymbols => _workingCopyOfnodeSymbols; 
        public SemanticModel GetSemanticModel() => _classStructure.GetSemanticModel();
        
        public MethodStructure(ClassStructure classStructure, MethodDeclarationSyntax method)
        {
            _classStructure = classStructure;
            _method = method;
            _name = _method.Identifier.ToString();            
            _methodSyntaxTree = _method.TrackNodes().SyntaxTree;
        }

        public async Task ParseMethod()
        {
            if (_alreadyParsed)
            {                
                if (_workingCopyOfnodeSymbols.Count == 0)
                    _workingCopyOfnodeSymbols = new ConcurrentQueue<(SyntaxNode, ISymbol)>(_originalNodeSymbols);

                return;
            }


            ParseParameters();         
            ParseReturnType();            
            await ParseCode();
            ParsingFinished();            
        }

        private void ParsingFinished()
        {
            _workingCopyOfnodeSymbols = new ConcurrentQueue<(SyntaxNode, ISymbol)>(_originalNodeSymbols);
            _alreadyParsed = true;
        }

        private async Task ParseCode()
        {
            var walker = new MethodSyntaxWalker();
            var model = GetSemanticModel();
            _methodSyntax.AddRange(await walker.Run(_method));
            var methodSymbol = model.GetDeclaredSymbol(_method);
            _methodSymbol = methodSymbol;
            ParseSyntax(model);
        }

        private void ParseSyntax(SemanticModel model)
        {
            var nodes = _methodSyntax;
            foreach (var currentNode in nodes)
            {
                var symbolInfo = model.GetSymbolInfo(currentNode);                
                _originalNodeSymbols.Enqueue((node: currentNode, symbol: symbolInfo.Symbol));
            }
        }

        private void ParseReturnType()
        {
            ReturnType = _method.ReturnType.ToString();
        }        

        private void ParseParameters()
        {
            var parameters = _method.ParameterList.Parameters;
            var tempParams = new List<string>();
            foreach (var parameter in parameters)
                tempParams.Add(parameter.ToString());

            Parameters = tempParams.ToArray();
        }
    }
}
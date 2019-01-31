using CodeAnalyzer.InterfaceResolver;
using CodeAnalyzer.SyntaxAnalysis;
using Logger;
using Microsoft.CodeAnalysis;
using SequenceDiagram.MethodStatements.Scopes;
using SequenceDiagram.MethodStatements.Wrapper;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDiagram.MethodStatements
{
    internal abstract class AbstractSyntaxDrawer
    {
        private readonly IInterfaceResolver _interfaceResolver;
        private readonly StringBuilder _diagram;
        private readonly string _entryMethod;
        private readonly ISyntaxDrawerFactory _drawerFactory;
        protected IDoLog _logger;
        
        protected StringBuilder Diagram { get { return _diagram; } }

        public abstract Type[] ValidNode { get; }

        protected AbstractSyntaxDrawer(IInterfaceResolver interfaceResolver, ISyntaxDrawerFactory drawerFactory, StringBuilder diagramToDrawTo, string entryMethod)
        {
            _interfaceResolver = interfaceResolver;
            _diagram = diagramToDrawTo;
            _entryMethod = entryMethod;
            _drawerFactory = drawerFactory;
            _logger = new NullLogger();
        }

        internal void AppendLogger(IDoLog logger)
        {
            _logger = logger;
        }

        abstract public Task WriteDiagramSyntax((SyntaxNode, ISymbol) syntaxSymbol, IMethodStructure method, int depth, string calledMethod, IStatementScope special = null);

        public virtual bool AmIResponsible(SyntaxNode node)
        {
            return ValidNode.Contains(node.GetType());
        }

        public async Task AnalyzeMethod(IMethodStructure method, int depth, StatementScope scope = null)
        {
            var scopeStack = ((InternalMethodStructure)method).CurrentScopre;

            await method.ParseMethod();

            if (scope != null)
                scopeStack.Push(scope);
            
            if (method.NodeSymbols.TryDequeue(out (SyntaxNode, ISymbol) nodeSymbol))
            {                
                var drawer = _drawerFactory.GetDrawer(nodeSymbol.Item1, _interfaceResolver, _diagram);
                
                while (drawer is NullDrawer)
                {
                    if (!method.NodeSymbols.TryDequeue(out nodeSymbol))
                        break;

                    WriteScopeEnd(method, nodeSymbol.Item1, scopeStack);
                    drawer = _drawerFactory.GetDrawer(nodeSymbol.Item1, _interfaceResolver, _diagram);

                    if (method.NodeSymbols.Count == 0)
                        break;
                }

                if (method.NodeSymbols.Count > 0)
                {
                    WriteScopeEnd(method, nodeSymbol.Item1, scopeStack);
                    await drawer.WriteDiagramSyntax(nodeSymbol, method, depth, _entryMethod);
                }
            }

            if (method.NodeSymbols.Count == 0)
                CloseMethod(scopeStack);          
        }

        protected void CloseMethod(ConcurrentStack<StatementScope> scopeStack)
        {
            while(!scopeStack.IsEmpty && scopeStack.TryPop(out StatementScope openScopes))
            {
                if (!(string.IsNullOrWhiteSpace(openScopes.EndStatement)))
                    Diagram.AppendLine(openScopes.EndStatement);
            }
        }

        private bool WriteScopeEnd(IMethodStructure method, SyntaxNode node, ConcurrentStack<StatementScope> scopeStack)
        {
            bool wrote = false;
                while(CheckScopeEnd(method, node, scopeStack))
                { 
                    if (scopeStack.TryPop(out StatementScope currentScope))
                    {
                        wrote = true;
                            
                        if (!(string.IsNullOrWhiteSpace(currentScope.EndStatement)))
                        {
                            _logger.Debug(currentScope.EndStatement);
                            Diagram.AppendLine(currentScope.EndStatement);                  
                        }
                    }
                }

            return wrote;
        }

        private bool CheckScopeEnd(IMethodStructure method, SyntaxNode node, ConcurrentStack<StatementScope> scopeStack)
        {
            if (scopeStack.TryPeek(out StatementScope peek))
                return peek.WriteEndStatement(method, node);

            return false;
        }

        public async Task<IMethodSymbol> ResolveInterface(IMethodSymbol call)
        {

            ITypeSymbol typeSmbol = (ITypeSymbol)call.ReceiverType;

            if (typeSmbol.TypeKind == TypeKind.Interface)
                call = await _interfaceResolver.GetImplementation(call);

            return call;
        }
    }
}
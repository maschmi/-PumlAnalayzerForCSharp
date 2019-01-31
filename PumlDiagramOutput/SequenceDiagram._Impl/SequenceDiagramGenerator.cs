using CodeAnalyzer.SyntaxAnalysis;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SequenceDiagram.MethodStatements;
using SequenceDiagram.Exceptions;
using Microsoft.CodeAnalysis;
using SequenceDiagram.MethodStatements.Wrapper;
using Logger;
using CodeAnalyzer.InterfaceResolver;
using System.Linq;
using System;
using SequenceDiagram.Extensions;

namespace SequenceDiagram
{
    public class SequenceDiagramGenerator : ISequenceDiagram
    {
        private readonly IDictionary<string, IClassStructure> _analyzedClasses;
        private readonly StringBuilder _diagram;
        private readonly IInterfaceResolver _ifaceResolver;
        private readonly IDoLog _logger;

        public SequenceDiagramGenerator(IDictionary<string, IClassStructure> analyzedClasses, IInterfaceResolver interfaceResolver, IDoLog logger = null) 
        {
            _analyzedClasses = analyzedClasses;
            _diagram = new StringBuilder();
            _ifaceResolver = interfaceResolver;
            if (_logger == null)
                _logger = new NullLogger();
            
            _logger = logger;
        }
      

        public async Task<string> GetSequenceDiagramForMethod(string className, string methodName)
        {
            var method = _analyzedClasses[className].Methods
                .Where(m => m.MethodName == methodName).FirstOrDefault();

            if (method == null)
                throw new InvalidOperationException("Could not find method to analyze.");
            
            return await CreateSequenceDiagramForMethod(method, 10);           
        }

        private async Task<string> CreateSequenceDiagramForMethod(IMethodStructure startMethod, int depth)
        {
            if (startMethod == null)
                throw new MethodNotFoundException("Start method could not be found!");
            InternalMethodStructure internalStartMethod = new InternalMethodStructure(startMethod, true);
            string entryMethod = await GetEntryMethod(internalStartMethod);            
            WriteHead(entryMethod);
            ISyntaxDrawerFactory drawerFactory = new SyntaxDrawerFactory(_analyzedClasses, entryMethod, _logger);

            await internalStartMethod.ParseMethod();
            while (internalStartMethod.NodeSymbols.TryDequeue(out (SyntaxNode, ISymbol) nodeSymbol))
            {                
                var drawer = drawerFactory.GetDrawer(nodeSymbol.Item1, _ifaceResolver, _diagram);
                await drawer.WriteDiagramSyntax(nodeSymbol, internalStartMethod, 10, entryMethod);
            }

            WriteEnd(entryMethod);
            return _diagram.ToString();
        }

        private async Task<string> GetEntryMethod(IMethodStructure startMethod)
        {
            await startMethod.ParseMethod();
            var caller = startMethod.GetMethodSymbol().ContainingSymbol.ToDisplayString();
            return caller + "." + startMethod.MethodName;
        }

        private void WriteHead(string entryMethod)
        {
            _diagram.Clear();
            _diagram.AppendLine("@startuml");

            _diagram.AppendLine(" --> " + entryMethod.MaskSpecialChars());
        }

        private void WriteEnd(string entryMethod)
        {
            _diagram.AppendLine(entryMethod.MaskSpecialChars() + " --> ");
            _diagram.Append("@enduml");
        }
    }
}


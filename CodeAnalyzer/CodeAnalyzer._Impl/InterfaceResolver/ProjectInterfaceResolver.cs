using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeAnalyzer.SyntaxAnalysis;
using System.Collections.Concurrent;
using Logger;

namespace CodeAnalyzer.InterfaceResolver
{
    internal class ProjectInterfaceResolver : IInterfaceResolver
    {
        private readonly IDictionary<string, IClassStructure> _analyzedClasses;
        private readonly IDictionary<string, IFileStructure> _analyzedFiles;
        private readonly IDoLog _logger;

        private readonly ConcurrentDictionary<string, IClassStructure> _resolver;
        private readonly string[] _ignoreList = new string[] { "IDisposable" };
        private static object _lockObject = new object();

        public ProjectInterfaceResolver(IDictionary<string, IClassStructure> analyzedClasses, IDictionary<string, IFileStructure> analyzedFiles, IDoLog logger)
        {
            _resolver = new ConcurrentDictionary<string, IClassStructure>();
            _analyzedClasses = analyzedClasses;
            _analyzedFiles = analyzedFiles;
            _logger = logger;
        }      

        public async Task<IMethodSymbol> GetImplementation(IMethodSymbol call)
        {
            ITypeSymbol interfaceInfo = call.ReceiverType;
            IClassStructure clsImplementing;
            IMethodSymbol result = null;

            if (_resolver.TryGetValue(interfaceInfo.ToString(), out clsImplementing))
                result = await ImplementingMethod(clsImplementing, call);

            await SearchImplementation(call);
            if(_resolver.TryGetValue(interfaceInfo.ToString(), out clsImplementing))
                result = await ImplementingMethod(clsImplementing, call);

            return result ?? call;
        }

        private async Task SearchImplementation(IMethodSymbol call)
        {
            ITypeSymbol interfaceInfo = call.ReceiverType;
            try
            {
                await Task.Run(() => RunSearchInBackground(interfaceInfo));
            }
            catch (Exception ex)
            {
                _logger.Info(ex.ToString());
            }
        }

        private void RunSearchInBackground(ITypeSymbol interfaceInfo)
        {
            foreach (var cls in _analyzedClasses)
            {
                IClassStructure clsStructure = cls.Value;
                ClassDeclarationSyntax classDeclaration = clsStructure.GetClassDeclaration;
                var compilation = clsStructure.GetCompilation();

                var model = compilation.GetSemanticModel(classDeclaration.SyntaxTree);

                var classSymbol = model.GetDeclaredSymbol(classDeclaration);
                if (classSymbol.TypeKind == TypeKind.Class)
                {
                    var implementedInterfaces = classSymbol.Interfaces;
                    foreach (var itfc in implementedInterfaces)
                    {
                        if (_ignoreList.Contains(itfc.Name))
                            continue;
                        try
                        {
                            _resolver.TryAdd(itfc.ToString(), clsStructure);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine(itfc.Name);
                        }
                    }
                }
            }
        }

        private async Task<IMethodSymbol> ImplementingMethod(IClassStructure clsImplementing, IMethodSymbol call)
        {
            var methodName = call.Name;
            IMethodStructure method = clsImplementing.Methods.Where(m => m.MethodName == methodName).FirstOrDefault();
            await method.ParseMethod();
            return method.GetMethodSymbol();
        }
    }
}

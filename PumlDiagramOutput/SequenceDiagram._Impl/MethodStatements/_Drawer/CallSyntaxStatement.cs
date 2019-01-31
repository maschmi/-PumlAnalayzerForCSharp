using CodeAnalyzer.InterfaceResolver;
using CodeAnalyzer.SyntaxAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SequenceDiagram.Extensions;
using SequenceDiagram.MethodStatements.Scopes;
using SequenceDiagram.MethodStatements.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDiagram.MethodStatements
{
    internal class CallSyntaxStatement : AbstractSyntaxDrawer
    {
        private const string _call = " -> ";
        private const string _ret = " --> ";
        private const string _text = " : ";
        private const string _act = "activate ";
        private const string _deact = "deactivate ";

        private readonly IDictionary<string, IClassStructure> _analyzedClasses;

        public override Type[] ValidNode { get; } = new Type[] { typeof(InvocationExpressionSyntax) };

        public CallSyntaxStatement(IInterfaceResolver interfaceResolver, 
            StringBuilder diagram, 
            SyntaxDrawerFactory drawerFactory,
            string entryMethod, 
            IDictionary<string, IClassStructure> analyzedClasses)
            :base(interfaceResolver, drawerFactory, diagram, entryMethod)
        {
            _analyzedClasses = analyzedClasses;
        }

        public override async Task WriteDiagramSyntax((SyntaxNode, ISymbol) syntaxSymbol, IMethodStructure method, int depth, string entryMethod, IStatementScope special)
        {
            if (!AmIResponsible(syntaxSymbol.Item1))
                return;

            IMethodSymbol call = syntaxSymbol.Item2 as IMethodSymbol;
            if (call == null)            
                _logger.Warning("No IMethodSymbol for: " + syntaxSymbol.Item1.ToFullString());            
            else
                await CalculateMethodCall(call, method, depth, entryMethod);

            await AnalyzeMethod(method, depth, null);
        }

        private async Task CalculateMethodCall(IMethodSymbol call, IMethodStructure method, int depth, string entryMethod)
        {
            var caller = method.GetMethodSymbol().ContainingSymbol.ToDisplayString().MaskSpecialChars();

            if (caller + "." + method.MethodName == entryMethod)
                caller = entryMethod;

            IMethodSymbol currentCall = await ResolveInterface(call);

            string calledMethodString = (currentCall.Name + CalculateParameter(currentCall)).MaskSpecialChars();
            string currentCallType = currentCall.ReceiverType.ToDisplayString().MaskSpecialChars();

            CalculateCall(caller, currentCallType, calledMethodString);

            await DiveDeeper(currentCall, depth);

            CalculateReturnValue(currentCall, currentCallType, caller);
            Diagram.AppendLine(_deact + currentCallType);
        }

        private void CalculateReturnValue(IMethodSymbol currentCall, string currentCallType, string caller)
        {
            if (!currentCall.ReturnsVoid)
            {
                var returnType = currentCall.ReturnType.ToDisplayString().MaskSpecialChars();
                Diagram.AppendLine(currentCallType + _ret + caller + _text + returnType);
            }
            else
                Diagram.AppendLine(currentCallType + _ret + caller);
        }

        private async Task DiveDeeper(IMethodSymbol currentCall, int depth)
        {
            if (depth > 0)
            {
                IMethodStructure calledMethod = SearchCalledMethodInAnalyzedClassed(currentCall);
                if (calledMethod != null)
                {
                    var wrappedMethod = new InternalMethodStructure(calledMethod);
                    await AnalyzeMethod(wrappedMethod, depth - 1);
                    CloseMethod(wrappedMethod.CurrentScopre);
                }
            }
        }

        private void CalculateCall(string caller, string currentCallType, string calledMethodString)
        {
            Diagram.AppendLine(caller + _call + currentCallType + _text + calledMethodString);
            Diagram.AppendLine(_act + currentCallType);
        }

        private string CalculateParameter(IMethodSymbol call)
        {
            var parameters = call.Parameters;

            if (parameters.Count() == 0)
                return string.Empty;

            var sb = new StringBuilder();
            sb.Append("(");
            for (int i = 0; i < parameters.Count(); i++)
            {
                sb.Append(parameters[i].Type.ToString());
                if (i < parameters.Count() - 1)
                    sb.Append(", ");
            }
            sb.Append(")");
            return sb.ToString();
        }

        private IMethodStructure SearchCalledMethodInAnalyzedClassed(IMethodSymbol call)
        {
            var methodName = call.Name;
            var className = call.ContainingSymbol.Name;

            IClassStructure cls;
            if (!_analyzedClasses.TryGetValue(className, out cls))
                return null;

            return cls.Methods.Where(m => m.MethodName == methodName).FirstOrDefault();
        }
    }
}

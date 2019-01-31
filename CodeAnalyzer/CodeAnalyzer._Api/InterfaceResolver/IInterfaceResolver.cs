using Microsoft.CodeAnalysis;
using System.Threading.Tasks;

namespace CodeAnalyzer.InterfaceResolver
{
    public interface IInterfaceResolver
    {
        Task<IMethodSymbol> GetImplementation(IMethodSymbol call);
    }
}
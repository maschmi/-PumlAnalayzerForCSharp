using Buildalyzer;

namespace CodeAnalyzer
{
    public interface ISolutionAnalyzer
    {
        AnalyzerManager AnalyzeManager { get; }
        void LoadSolution();
    }
}
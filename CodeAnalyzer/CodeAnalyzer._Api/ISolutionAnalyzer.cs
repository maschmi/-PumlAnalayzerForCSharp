using System;
using Buildalyzer;
using Microsoft.CodeAnalysis;

namespace CodeAnalyzer
{
    public interface ISolutionAnalyzer
    {
        AnalyzerManager AnalyzeManager { get; }
        AdhocWorkspace AnalyzerWorkspace { get; }

        void LoadSolution();
    }
}
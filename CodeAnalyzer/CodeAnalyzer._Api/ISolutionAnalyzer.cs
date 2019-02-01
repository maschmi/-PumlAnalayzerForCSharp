using System;
using System.Collections.Generic;
using Buildalyzer;
using Microsoft.CodeAnalysis;

namespace CodeAnalyzer
{
    public interface ISolutionAnalyzer
    {
        AnalyzerManager AnalyzeManager { get; }
        AdhocWorkspace AnalyzerWorkspace { get; }
        
        IEnumerable<string> OutputFiles { get; }
        Solution ParsedSolution { get; }
        IEnumerable<Project> Projects { get; }

        void Dispose();
        void LoadSolution();
    }
}
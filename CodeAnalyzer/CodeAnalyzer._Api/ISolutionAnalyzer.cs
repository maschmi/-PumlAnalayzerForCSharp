using System;
using System.Collections.Generic;
using Buildalyzer;
using Microsoft.CodeAnalysis;

namespace CodeAnalyzer
{
    public interface ISolutionAnalyzer
    {   
        IEnumerable<string> OutputFiles { get; }
        Solution ParsedSolution { get; }
        IEnumerable<Project> Projects { get; }

        void Dispose();
        void LoadSolution();
    }
}
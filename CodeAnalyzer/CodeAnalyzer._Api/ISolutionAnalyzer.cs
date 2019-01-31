﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace CodeAnalyzer
{
    public interface ISolutionAnalyzer
    {
        IEnumerable<string> OutputFiles { get; }
        Solution ParsedSolution { get; }
        IEnumerable<Project> Projects { get; }

        void Dispose();        
        Task LoadSolution();
    }
}
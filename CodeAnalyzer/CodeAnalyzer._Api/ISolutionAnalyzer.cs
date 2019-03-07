using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace CodeAnalyzer
{
    public interface ISolutionAnalyzer
    {   
        Solution ParsedSolution { get; }
        IEnumerable<Project> Projects { get; }

        void Dispose();
        Task LoadSolution(string solutionPath, string excludeFiles, string frameworkProperty, string msBuildPath);
        Task LoadSolution(string excludeFiles);
    }
}
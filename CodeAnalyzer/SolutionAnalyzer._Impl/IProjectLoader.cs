using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace WorkspaceAnalyzer
{
    public interface IProjectLoader
    {
        Solution ParsedSolution { get; }
        IEnumerable<Project> Projects { get; }

        void Dispose();
        Task<Project> LoadProject(string projectPath, string excludeFiles, string frameworkProperty, string msBuildPath);
    }
}
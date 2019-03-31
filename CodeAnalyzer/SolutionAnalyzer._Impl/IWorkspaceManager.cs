using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace WorkspaceAnalyzer
{
    public interface IWorkspaceManager
    {
        void Dispose();
        Task<Project> LoadProject(string projectPath);
        Task<Solution> LoadSolution(string solutionPath);
        void SetFrameworkProperty(string frameWorkproperty);
        void SetMSBuildPath(string msBuildPath);
    }
}
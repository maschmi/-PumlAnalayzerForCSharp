using Microsoft.CodeAnalysis.MSBuild;

namespace WorkspaceAnalyzer
{
    public interface IWorkspaceBuilder
    {
        MSBuildWorkspace Build();
        WorkspaceBuilder WithFrameworkProperty(string frameworkProperty);
        WorkspaceBuilder WithMsBuildPath(string msBuildPath);
    }
}
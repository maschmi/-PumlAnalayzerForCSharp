using Logger;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WorkspaceAnalyzer
{
    public class WorkspaceManager : IDisposable, IWorkspaceManager
    {
        private readonly IDoLog _logger;
        private readonly IWorkspaceBuilder _workspaceBuilder;

        private string _frameworkProperty = string.Empty;
        private string _msBuildPath = string.Empty;

        private MSBuildWorkspace _workspace;

        public bool IsCreated { get; private set; } = false;

        public WorkspaceManager(IWorkspaceBuilder wsBuilder, IDoLog logger)
        {
            if (logger == null)
                logger = new NullLogger();
            _logger = logger;

            _workspaceBuilder = wsBuilder;
        }

        public void SetMSBuildPath(string msBuildPath)
        {
            if (string.IsNullOrWhiteSpace(msBuildPath))
                throw new ArgumentException("Path to MSBuild has to be given!");

            _msBuildPath = msBuildPath;
        }

        public void SetFrameworkProperty(string frameWorkproperty)
        {
            _frameworkProperty = frameWorkproperty ?? string.Empty;
        }

        public async Task<Project> LoadProject(string projectPath)
        {
            if (!File.Exists(projectPath))
                throw new FileNotFoundException("File not found! Was looking for " + projectPath);

            CreateWorkspace();

            return await LoadProjectIntoWorkspace(projectPath);
        }

        public async Task<Solution> LoadSolution(string solutionPath)
        {
            if (!File.Exists(solutionPath))
                throw new FileNotFoundException("File not found! Was looking for " + solutionPath);

            CreateWorkspace();

            return await LoadSolutionIntoWorkspace(solutionPath);
        }

        private void CreateWorkspace()
        {
            _workspace = _workspaceBuilder
                .WithMsBuildPath(_msBuildPath)
                .WithFrameworkProperty(_frameworkProperty)
                .Build();
            IsCreated = true;
        }

        private async Task<Solution> LoadSolutionIntoWorkspace(string solutionPath)
        {
            _workspace.WorkspaceFailed += WriteErrorMessage;
            _logger.Info($"Loading solution '{solutionPath}'");

            var solution = await _workspace.OpenSolutionAsync(solutionPath);

            _logger.Info($"Finished loading solution '{solutionPath}'");
            _workspace.WorkspaceFailed -= WriteErrorMessage;

            return solution;
        }

        private async Task<Project> LoadProjectIntoWorkspace(string projectPath)
        {
            _workspace.WorkspaceFailed += WriteErrorMessage;
            _logger.Info($"Loading project '{projectPath}'");

            var project = await _workspace.OpenProjectAsync(projectPath);

            _logger.Info($"Finished loading project '{projectPath}'");
            _workspace.WorkspaceFailed -= WriteErrorMessage;

            return project;
        }

        private void WriteErrorMessage(object sender, WorkspaceDiagnosticEventArgs workspaceDiagnosticEventArgs)
        {
            _logger.Verbose(workspaceDiagnosticEventArgs.Diagnostic.Message);
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _workspace.Dispose();
                    IsCreated = false;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~WorkspaceManager()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}

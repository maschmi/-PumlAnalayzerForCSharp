using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Logger;
using CodeAnalyzer;

namespace WorkspaceAnalyzer
{
    public class SolutionAnalyzer : IDisposable, ISolutionAnalyzer
    {
        private readonly IDoLog _logger;
        private readonly IWorkspaceBuilder _workspaceBuilder;

        private string _solution;       

        private MSBuildWorkspace _workspace;
        private bool disposedValue = false; // To detect redundant calls

        public Solution ParsedSolution { get; private set; }
        public IEnumerable<Project> Projects => ParsedSolution.Projects;

        public SolutionAnalyzer(IWorkspaceBuilder wsBuilder, IDoLog logger = null)
        {
            if (logger == null)
                logger = new NullLogger();
            _logger = logger;

            _workspaceBuilder = wsBuilder;
        }

        public SolutionAnalyzer(string solution, string msBuildPath, IDoLog logger = null)
        {
            if (!File.Exists(solution))
                throw new FileNotFoundException("Solution not found! Was looking for " + solution);
            _solution = solution;
            _workspace = _workspaceBuilder.WithMsBuildPath(msBuildPath).Build();

            if (logger == null)
                logger = new NullLogger();
            _logger = logger;
        }

        public async Task LoadSolution(string solutionPath, string excludeFiles, string frameworkProperty, string msBuildPath)
        {
            if (!File.Exists(solutionPath))
                throw new FileNotFoundException("Solution not found! Was looking for " + solutionPath);
            _solution = solutionPath;

            _workspace = _workspaceBuilder
                .WithMsBuildPath(msBuildPath)
                .WithFrameworkProperty(frameworkProperty)
                .Build();

             await LoadSolutionIntoWorkspace(excludeFiles);
        }
        

        private async Task LoadSolutionIntoWorkspace(string excludeFiles)
        {
            // Print message for WorkspaceFailed event to help diagnosing project load failures.
            _workspace.WorkspaceFailed += WriteErrorMessage;

            var solutionPath = _solution;
            _logger.Info($"Loading solution '{solutionPath}'");
            
            // Attach progress reporter so we print projects as they are loaded.
            //var solution = await workspace.OpenSolutionAsync(solutionPath, new ConsoleProgressReporter());
            ParsedSolution = await _workspace.OpenSolutionAsync(solutionPath);
            
            _logger.Info($"Finished loading solution '{solutionPath}'");
            // TODO: Do analysis on the projects in the loaded solution            

            _workspace.WorkspaceFailed -= WriteErrorMessage;
        }

        private void WriteErrorMessage(object sender, WorkspaceDiagnosticEventArgs workspaceDiagnosticEventArgs)
        {
            _logger.Verbose(workspaceDiagnosticEventArgs.Diagnostic.Message);
        }        

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _workspace.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~CodeAnalyzer() {
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

        public Task LoadSolution(string excludeFiles)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

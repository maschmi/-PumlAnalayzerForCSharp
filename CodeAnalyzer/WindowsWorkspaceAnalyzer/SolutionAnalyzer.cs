using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Logger;
using CodeAnalyzer;

namespace WorkspaceAnalyzer.Windows
{
    public class SolutionAnalyzer : IDisposable, ISolutionLoader
    {
        private IDoLog _logger;
        private readonly string _solution;        
        
        private MSBuildWorkspace _workspace;
        private bool disposedValue = false; // To detect redundant calls

        public Solution ParsedSolution { get; private set; }
        public IEnumerable<string> OutputFiles { get; private set; }
        public IEnumerable<Project> Projects => ParsedSolution.Projects;

        public SolutionAnalyzer(string solution, IDoLog logger = null)
        {
            if (!File.Exists(solution))
                throw new FileNotFoundException("Solution not found! Was looking for " + solution);
            _solution = solution;

            if (logger == null)
                logger = new NullLogger();
            _logger = logger;            
        }        

        public async Task LoadSolution(string excludeFiles)
        {
            PreapreMSBuildWorkspace();
            await LoadSolutionImpl(excludeFiles);
        }

        private async Task LoadSolutionImpl(string excludeFiles)
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

            _logger.Info("Extracting ouputFiles");

            if (string.IsNullOrWhiteSpace(excludeFiles))
                OutputFiles = ParsedSolution.Projects
                    .Select(p => p.OutputFilePath).ToList();
            else
                OutputFiles = ParsedSolution.Projects
                    .Where(p => !(p.OutputFilePath.Contains(excludeFiles)))
                    .Select(p => p.OutputFilePath).ToList();

            _workspace.WorkspaceFailed -= WriteErrorMessage;
        }

        private void WriteErrorMessage(object sender, WorkspaceDiagnosticEventArgs workspaceDiagnosticEventArgs)
        {
            _logger.Error(workspaceDiagnosticEventArgs.Diagnostic.Message);
        }

        private void PreapreMSBuildWorkspace()
        {
            var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
            var instance = visualStudioInstances.Length != 0
                // Take the first MSBuild installation we can find.
                ? visualStudioInstances[0]
                // Else throw exception
                : throw new InvalidOperationException("No MSBuild installation found!");        

            _logger.Info($"Using MSBuild at '{instance.MSBuildPath}' to load projects.");

            // NOTE: Be sure to register an instance with the MSBuildLocator 
            //       before calling MSBuildWorkspace.Create()
            //       otherwise, MSBuildWorkspace won't MEF compose.
            try
            {
               MSBuildLocator.RegisterInstance(instance);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            _workspace = MSBuildWorkspace.Create();
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

        public Task LoadSolution(string solutionPath, string excludeFiles, string msBuildPath)
        {
            throw new NotImplementedException();
        }

        public Task LoadSolution(string solutionPath, string excludeFiles, string frameworkProperty, string msBuildPath)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Logger;
using CodeAnalyzer;
using Buildalyzer;
using Buildalyzer.Workspaces;

namespace WorkspaceAnalyzer
{
    public class SolutionAnalysis : ISolutionAnalyzer, IDisposable
    {
        private IDoLog _logger;
        private readonly string _solution;
        public AdhocWorkspace AnalyzerWorkspace { get; private set; }

        public AnalyzerManager AnalyzeManager { get; private set; }

        public SolutionAnalysis(string solution, IDoLog logger = null)
        {
            if (!File.Exists(solution))
                throw new FileNotFoundException("Solution not found!");
            _solution = solution;

            if (logger == null)
                logger = new NullLogger();
            _logger = logger;            
        }        

        public void LoadSolution()
        {         
            LoadSolutionImpl();
        }

        private void LoadSolutionImpl()
        {
            _logger.Info($"Loading solution '{_solution}'");
            AnalyzeManager = new AnalyzerManager(_solution);
            _logger.Info($"Finished loading solution '{_solution}'");
            AnalyzerWorkspace = AnalyzeManager.GetWorkspace();
        }

        private void WriteErrorMessage(object sender, WorkspaceDiagnosticEventArgs workspaceDiagnosticEventArgs)
        {
            _logger.Error(workspaceDiagnosticEventArgs.Diagnostic.Message);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    AnalyzerWorkspace.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SolutionAnalysis() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}

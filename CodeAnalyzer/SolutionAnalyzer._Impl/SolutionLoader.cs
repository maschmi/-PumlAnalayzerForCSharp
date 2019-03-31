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
    public class SolutionLoader : IDisposable, ISolutionLoader
    {
        private readonly IDoLog _logger;
        private readonly IWorkspaceManager _workspaceManager;

        private string _solution;       

        private bool disposedValue = false; // To detect redundant calls

        public Solution ParsedSolution { get; private set; }
        public IEnumerable<Project> Projects => ParsedSolution.Projects;

        public SolutionLoader(IWorkspaceManager wsManager, IDoLog logger = null)
        {
            if (logger == null)
                logger = new NullLogger();
            _logger = logger;

            _workspaceManager = wsManager;
        }
        //TODO remove
        public SolutionLoader(string solution, string msBuildPath, IDoLog logger = null)
        {
            if (!File.Exists(solution))
                throw new FileNotFoundException("Solution not found! Was looking for " + solution);
            _solution = solution;            

            if (logger == null)
                logger = new NullLogger();
            _logger = logger;
        }
        

        public async Task<Solution> LoadSolution(string solutionPath, string excludeFiles, string frameworkProperty, string msBuildPath)
        {
            _workspaceManager.SetFrameworkProperty(frameworkProperty);
            _workspaceManager.SetMSBuildPath(msBuildPath);

            return await _workspaceManager.LoadSolution(solutionPath);
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _workspaceManager.Dispose();
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


        #endregion
    }
}

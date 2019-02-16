using Logger;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzer.Service
{
    public class WorkplaceService : IDisposable
    {
        private IServiceScope _containerServiceScope;
        private ISolutionAnalyzer _solutionAnalyzer;
        private IProjectAnalyzer _projectAnalyzer;



        public WorkplaceService()
        {
            Configuration.Setup();
            _containerServiceScope = Configuration.Container.GetRequiredService<IServiceScopeFactory>().CreateScope();
        }

        public async Task LoadSolution(string solutionPath, string excludeFiles, string msBuildPath)
        {
            var logger = _containerServiceScope.ServiceProvider.GetService<IDoLog>();
            _solutionAnalyzer = _containerServiceScope.ServiceProvider.GetService<ISolutionAnalyzer>();
            await _solutionAnalyzer.LoadSolution(solutionPath, excludeFiles, msBuildPath);

        }

        public IEnumerable<string> GetOutputFiles()
        {
            CheckInitialization(_solutionAnalyzer);
            return _solutionAnalyzer.OutputFiles;
        }

        public IEnumerable<Project> GetProjects()
        {
            CheckInitialization(_solutionAnalyzer);
            return _solutionAnalyzer.Projects;
        }

        public Solution GetSolution()
        {
            CheckInitialization(_solutionAnalyzer);
            return _solutionAnalyzer.ParsedSolution;
        }        

        private void CheckInitialization(object check)
        {
            if (check == null)
                throw new InvalidOperationException(nameof(check) + " is not yet initialized.");
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _containerServiceScope.Dispose();
                    _solutionAnalyzer.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~WorkplaceService() {
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

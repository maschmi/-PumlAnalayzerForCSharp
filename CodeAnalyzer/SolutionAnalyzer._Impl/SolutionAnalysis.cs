using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Logger;
using CodeAnalyzer;
using Buildalyzer;

namespace WorkspaceAnalyzer
{
    public class SolutionAnalysis : ISolutionAnalyzer
    {
        private IDoLog _logger;
        private readonly string _solution;

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
        }

        private void WriteErrorMessage(object sender, WorkspaceDiagnosticEventArgs workspaceDiagnosticEventArgs)
        {
            _logger.Error(workspaceDiagnosticEventArgs.Diagnostic.Message);
        }        
    }
}

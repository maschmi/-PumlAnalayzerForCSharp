using CodeAnalyzer;
using CodeAnalyzer.SyntaxAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkspaceAnalyzer
{
    public class ClassAnalyzer : IClassAnalyzer
    {
        private readonly IProjectAnalyzer _projectAnalyzer;

        public ClassAnalyzer(IProjectAnalyzer projectAnalyzer)
        {
            _projectAnalyzer = projectAnalyzer;
        }

        public IEnumerable<string> GetMethodNamesForClass(string className)
        {
            if (_projectAnalyzer.AnalyzedClasses.ContainsKey(className))
                return MethodsInClass(_projectAnalyzer.AnalyzedClasses[className]);

            return new string[0];
        }

        private IEnumerable<string> MethodsInClass(IClassStructure classStructure)
        {
            return classStructure.Methods.Select(c => c.MethodName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis.MSBuild;
using Logger;

namespace WorkspaceAnalyzer
{
    public class WorkspaceBuilder : IWorkspaceBuilder
    {

        private string _msBuildPath = string.Empty;
        private string _framework = string.Empty;
        private readonly IDoLog _logger;

        public WorkspaceBuilder(IDoLog logger)
        {
            if (logger == null)
                logger = new NullLogger();

            _logger = logger;
        }
        public WorkspaceBuilder WithMsBuildPath(string msBuildPath)
        {
            msBuildPath = CalculateMsBuildPath(msBuildPath);
            if (VerifyPath(msBuildPath))
                _msBuildPath = msBuildPath;

            return this;
        }

        private string CalculateMsBuildPath(string msBuildPath)
        {
            if (!(msBuildPath.EndsWith("MSBuild.dll", StringComparison.InvariantCultureIgnoreCase)))
                msBuildPath = Path.Combine(msBuildPath, "MSBuild.dll");

            return msBuildPath;
        }

        private bool VerifyPath(string msBuildPath)
        {
            if (File.Exists(msBuildPath))
                return true;

            throw new FileNotFoundException("MSBuild.dll not found at " + msBuildPath);
        }

        public WorkspaceBuilder WithFrameworkProperty(string frameworkProperty)
        {
            if (frameworkProperty == null)
                return this;

            _framework = frameworkProperty;
            return this;
        }

        public MSBuildWorkspace Build()
        {
            _logger.Debug("Building MSBuildWorkspace");
            if (string.IsNullOrEmpty(_msBuildPath))
                throw new InvalidOperationException("Please specify the path to MSBuild.dll");

            Environment.SetEnvironmentVariable("MSBUILD_EXE_PATH", _msBuildPath);

            if (string.IsNullOrEmpty(_framework))
                return MSBuildWorkspace.Create();
            else
            {
                var properties = new Dictionary<string, string>();
                properties.Add("TargetFrameworkVersion", _framework);
                return MSBuildWorkspace.Create(properties);
            }
        }

    }
}

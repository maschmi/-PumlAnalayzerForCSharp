using Logger;
using Microsoft.Extensions.DependencyInjection;
using System;
using WorkspaceAnalyzer;

namespace CodeAnalyzer.Service
{
    internal static class Configuration
    {
        public static ServiceProvider Container { get; private set; }
        private static readonly ServiceCollection _serviceCollection;        

        static Configuration()
        {
            _serviceCollection = new ServiceCollection();
        }

        public static void Setup()
        {
            RegisterLogging();
            RegisterWorkpalaceAnalyzer();
            Container = _serviceCollection.BuildServiceProvider();
        }

        private static void RegisterLogging()
        {
            _serviceCollection.AddSingleton<IDoLog, ConsoleLogger>();
        }

        private static void RegisterWorkpalaceAnalyzer()
        {
            _serviceCollection.AddSingleton<IWorkspaceBuilder, WorkspaceBuilder>();
            _serviceCollection.AddScoped<IWorkspaceManager, WorkspaceManager>();
            _serviceCollection.AddScoped<IProjectAnalyzer, ProjectAnalyzer>();
            _serviceCollection.AddScoped<ISolutionLoader, SolutionLoader>();
            _serviceCollection.AddScoped<IClassAnalyzer, ClassAnalyzer>();
        }
    }
}

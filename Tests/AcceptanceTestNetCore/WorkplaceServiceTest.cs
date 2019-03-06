using AcceptanceTestNetCore.Helper;
using CodeAnalyzer.Service;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTestNetCore.Sercices
{
    [TestFixture]
    public class WorkplaceServiceTest
    {
        private const string _msBuildFallback = @"C:\Program Files\dotnet\sdk\2.2.103";
        private string _solutionDir;

        [SetUp]
        public void Setup()
        {
            var helper = new SeqDiagramHelper();
            _solutionDir = helper.SoltuionDirectory;
        }

        [Test]
        public async Task WorkplaceSevice_LoadSolution_EverythingLoaded()
        {
            using (var service = new WorkplaceService())
            {
                await service.LoadSolution(_solutionDir, null, null, _msBuildFallback);
                service.GetOutputFiles();
            }
            
            
        }
    }
}

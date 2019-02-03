using AcceptanceTestNetCore.Helper;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AcceptanceTestNetCore
{
    [TestFixture]
    public class DemoDrawingTest
    {
        private const string _msBuildFallback = @"C:\Program Files\dotnet\sdk\2.2.103";

        [Test]
        [TestCase(_msBuildFallback, "ClassA", "ConditionalIncrease")]
        [TestCase(_msBuildFallback, "ClassA", "IncreaseList")]
        public async Task DrawSequenceDiagram_FromDemoNet_SequenceDiagramIsOk(string msBuildPath, string className, string methodName)
        {
            var helper = new SeqDiagramHelper();
            var diagram = await helper.CreateDiagram(_msBuildFallback, className, methodName, null);
            helper.AssertResultMeetsExpectation(diagram, methodName);
        }
    }
}

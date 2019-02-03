using AcceptanceTest.Net472.Helper;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AcceptanceTest.Net472
{
    [TestFixture]
    public class DemoDrawingTest
    {
        [Test]
        [TestCase("ClassA", "ConditionalIncrease")]
        [TestCase("ClassA", "IncreaseList")]
        public async Task DrawSequenceDiagram_FromDemoNet_SequenceDiagramIsOk(string className, string methodName)
        {
            var helper = new SeqDiagramHelper();
            var diagram = await helper.CreateDiagram(className, methodName, null);
            helper.AssertResultMeetsExpectation(diagram, methodName);
        }
    }
}

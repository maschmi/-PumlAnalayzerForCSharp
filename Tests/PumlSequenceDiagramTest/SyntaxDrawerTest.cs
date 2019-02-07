using NUnit.Framework;
using PumlDiagramTest.Helper;
using System.Threading.Tasks;

namespace PumlDiagramTest
{
    [TestFixture]
    public class SyntaxDrawerTest
    {        

        [Test]
        [TestCaseSource(typeof(IfSyntaxTests), nameof(IfSyntaxTests.SingleMethod))]
        public async Task<string> IfElseSyntaxStatement_SingleMethod_CorrectDiagramSyntax(string source, string className, string methodName)
        {
            var fixture = new DrawerTestFixture(source, className, methodName);

            return await fixture.DiagramGenerator.GetSequenceDiagramForMethod(className, methodName);            
        }

        [Test]
        [TestCaseSource(typeof(IfSyntaxTests), nameof(IfSyntaxTests.MutlineCondition))]
        public async Task<string> IfSyntaxStatement_MultilineCondition_CorretDiagramSyntax(string source, string className, string methodName)
        {
            var fixture = new DrawerTestFixture(source, className, methodName);
            var diagram = await fixture.DiagramGenerator.GetSequenceDiagramForMethod(className, methodName);
            return diagram;
        }

        [Test]
        [TestCaseSource(typeof(ForSyntaxTests), nameof(ForSyntaxTests.SingleMethod))]
        public async Task<string> ForSyntaxStatement_SingleMethod_CorrectDiagram(string source, string className, string methodName)
        {
            var fixture = new DrawerTestFixture(source, className, methodName);
            var diagram = await fixture.DiagramGenerator.GetSequenceDiagramForMethod(className, methodName);
            return diagram;
        }
    }
}

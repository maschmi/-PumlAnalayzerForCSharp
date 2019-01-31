using FluentAssertions;
using NUnit.Framework;
using PumlDiagramTest.Helper;
using SequenceDiagram;
using System.Threading.Tasks;

namespace PumlDiagramTest
{
    [TestFixture]
    public class SyntaxDrawerTest
    {        

        [Test]
        [TestCaseSource(typeof(IfSyntaxTests), "SingleMethod")]
        public async Task<string> IfSyntaxStatement_SingleLineIfStatementWithReturns(string source, string className, string methodName)
        {
            var fixture = new DrawerTestFixture(source, className, methodName);

            return await fixture.DiagramGenerator.GetSequenceDiagramForMethod("IfClass", "Test");            
        }
    }
}

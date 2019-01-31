using NUnit.Framework;
using PumlDiagramTest.TestData.SingleMethod;
using System.Collections;

namespace PumlDiagramTest
{
    public class IfSyntaxTests
    {
        internal static IEnumerable SingleMethod
        {
            get
            {
                yield return new TestCaseData(IfSyntaxStatementsTestData.SOURCE_IFMETHOD_SINGLELINE, "IfClass", "Test")
                    .Returns(IfSyntaxStatementsTestData.RESULT_IFMETHOD_SINGLELINE);
                yield return new TestCaseData(IfSyntaxStatementsTestData.SOURCE_IFMETHOD_SINGLEBLOCK, "IfClass", "Test")
                    .Returns(IfSyntaxStatementsTestData.RESULT_IFMETHOD_SINGLELINE);
                yield return new TestCaseData(IfSyntaxStatementsTestData.SOURCE_IFELSEMETHOD_SINGLELINE, "IfClass", "Test")
                    .Returns(IfSyntaxStatementsTestData.RESULT_IFELSEMETHOD_SINGLELINE);
                yield return new TestCaseData(IfSyntaxStatementsTestData.SOURCE_IFELSEMETHOD_SINGLEBLOCK, "IfClass", "Test")
                    .Returns(IfSyntaxStatementsTestData.RESULT_IFELSEMETHOD_SINGLELINE);
                yield return new TestCaseData(IfSyntaxStatementsTestData.SOURCE_IFELSENESTED, "IfClass", "Test")
                   .Returns(IfSyntaxStatementsTestData.RESULT_IFELSENESTED);
            }
        } 
    }
}
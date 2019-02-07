using NUnit.Framework;
using PumlDiagramTest.TestData.SingleMethod;
using System.Collections;
using System.IO;
using System.Reflection;

namespace PumlDiagramTest
{
    public class IfSyntaxTests
    {
        private const string SOURCECODEFILE = "PumlSequenceDiagramTest.TestData.SingleMethod.Resource.IfSyntaxTestDataClass.cs";
        private static string _testcode = string.Empty;

        static IfSyntaxTests()
        {
            if (!string.IsNullOrWhiteSpace(_testcode))
                return;

            var assembly = Assembly.GetExecutingAssembly();
            using (var sr = new StreamReader(assembly.GetManifestResourceStream(SOURCECODEFILE)))
            {
                _testcode = sr.ReadToEnd();
            }            
        }

        internal static IEnumerable SingleMethod
        {
            get
            {
                yield return new TestCaseData(_testcode, "IfSyntaxTestDataClass", "IfSingleline")
                    .Returns(IfSyntaxStatementsTestDataResults.RESULT_IFMETHOD_SINGLELINE);
                yield return new TestCaseData(_testcode, "IfSyntaxTestDataClass", "IfSingleblock")
                    .Returns(IfSyntaxStatementsTestDataResults.RESULT_IFMETHOD_SINGLEBLOCK);
                yield return new TestCaseData(_testcode, "IfSyntaxTestDataClass", "IfElseSingleLine")
                    .Returns(IfSyntaxStatementsTestDataResults.RESULT_IFELSEMETHOD_SINGLELINE);
                yield return new TestCaseData(_testcode, "IfSyntaxTestDataClass", "IfElseSingleBlock")
                    .Returns(IfSyntaxStatementsTestDataResults.RESULT_IFELSEMETHOD_SINGLEBLOCK);
                yield return new TestCaseData(_testcode, "IfSyntaxTestDataClass", "IfElseNested")
                   .Returns(IfSyntaxStatementsTestDataResults.RESULT_IFELSENESTED);
            }
        } 

        internal static IEnumerable MutlineCondition
        {
            get
            {
                yield return new TestCaseData(_testcode, "IfSyntaxTestDataClass", "IfMultiline")
                 .Returns(IfSyntaxStatementsTestDataResults.RESULT_IFMETHOD_MULTILINE_CONDITION);
            }
        }
    }
}
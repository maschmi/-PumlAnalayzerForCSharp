using NUnit.Framework;
using PumlDiagramTest.TestData.SingleMethod;
using System.Collections;
using System.IO;
using System.Reflection;

namespace PumlDiagramTest
{
    public class ForSyntaxTests
    {
        private const string SOURCECODEFILE = "PumlSequenceDiagramTest.TestData.SingleMethod.Resource.ForSyntaxTestDataClass.cs";
        private static string _testcode = string.Empty;

        static ForSyntaxTests()
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
                yield return new TestCaseData(_testcode, "ForSyntaxTestDataClass", "ForSingleline")
                    .Returns(ForSyntaxStatementsTestDataResults.RESULT_FORMETHOD_SINGLELINE);
                yield return new TestCaseData(_testcode, "ForSyntaxTestDataClass", "ForEachSingleline")
                    .Returns(ForSyntaxStatementsTestDataResults.RESULT_FOREACHMETHOD_SINGLELINE);
                yield return new TestCaseData(_testcode, "ForSyntaxTestDataClass", "ForMultiline")
                    .Returns(ForSyntaxStatementsTestDataResults.RESULT_FORMETHOD_MULTILINE);
                yield return new TestCaseData(_testcode, "ForSyntaxTestDataClass", "ForEachMultiline")
                    .Returns(ForSyntaxStatementsTestDataResults.RESULT_FOREACHMETHOD_MULTILINE);

            }
        }         
    }
}
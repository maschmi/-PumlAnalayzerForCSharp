using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PumlDiagramTest.TestData.SingleMethod
{
    internal static class ForSyntaxStatementsTestDataResults
    {    

    internal const string RESULT_FORMETHOD_SINGLELINE =
@"@startuml
 --> PumlSequenceDiagramTest.TestData.SingleMethod.ForSyntaxTestDataClass.ForSingleline
loop ""i < list.ToArray().Length""
end
PumlSequenceDiagramTest.TestData.SingleMethod.ForSyntaxTestDataClass.ForSingleline --> 
@enduml";

        internal const string RESULT_FORMETHOD_MULTILINE =
@"@startuml
 --> PumlSequenceDiagramTest.TestData.SingleMethod.ForSyntaxTestDataClass.ForMultiline
loop ""i < list.ToArray().Lenght""
end
PumlSequenceDiagramTest.TestData.SingleMethod.ForSyntaxTestDataClass.ForMultiline --> 
@enduml";


        internal const string RESULT_FOREACHMETHOD_SINGLELINE =
@"@startuml
 --> PumlSequenceDiagramTest.TestData.SingleMethod.ForSyntaxTestDataClass.ForEachSingleline
loop ""list.ToArray()""
end
PumlSequenceDiagramTest.TestData.SingleMethod.ForSyntaxTestDataClass.ForEachSingleline --> 
@enduml";

    internal const string RESULT_FOREACHMETHOD_MULTILINE =
@"@startuml
 --> PumlSequenceDiagramTest.TestData.SingleMethod.ForSyntaxTestDataClass.ForEachMultiline
loop ""list.ToArray()""
end
PumlSequenceDiagramTest.TestData.SingleMethod.ForSyntaxTestDataClass.ForEachMultiline --> 
@enduml";
    }
}


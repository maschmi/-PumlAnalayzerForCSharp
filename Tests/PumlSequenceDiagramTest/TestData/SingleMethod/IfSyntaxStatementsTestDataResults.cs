using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PumlDiagramTest.TestData.SingleMethod
{
    internal static class IfSyntaxStatementsTestDataResults
    {    

    internal const string RESULT_IFMETHOD_SINGLELINE =
@"@startuml
 --> PumlSequenceDiagramTest.TestData.SingleMethod.IfSyntaxTestDataClass.IfSingleline
alt test
group return true
end
end
group return false
end
PumlSequenceDiagramTest.TestData.SingleMethod.IfSyntaxTestDataClass.IfSingleline --> 
@enduml";

        internal const string RESULT_IFMETHOD_SINGLEBLOCK =
@"@startuml
 --> PumlSequenceDiagramTest.TestData.SingleMethod.IfSyntaxTestDataClass.IfSingleblock
alt test
group return true
end
end
group return false
end
PumlSequenceDiagramTest.TestData.SingleMethod.IfSyntaxTestDataClass.IfSingleblock --> 
@enduml";

        internal const string RESULT_IFELSEMETHOD_SINGLELINE =
@"@startuml
 --> PumlSequenceDiagramTest.TestData.SingleMethod.IfSyntaxTestDataClass.IfElseSingleLine
alt test
group return true
end
else 
group return false
end
end
group return true
end
PumlSequenceDiagramTest.TestData.SingleMethod.IfSyntaxTestDataClass.IfElseSingleLine --> 
@enduml";

    internal const string RESULT_IFELSEMETHOD_SINGLEBLOCK =
@"@startuml
 --> PumlSequenceDiagramTest.TestData.SingleMethod.IfSyntaxTestDataClass.IfElseSingleBlock
alt test
group return true
end
else 
group return false
end
end
group return true
end
PumlSequenceDiagramTest.TestData.SingleMethod.IfSyntaxTestDataClass.IfElseSingleBlock --> 
@enduml";

        internal const string RESULT_IFELSENESTED =
@"@startuml
 --> PumlSequenceDiagramTest.TestData.SingleMethod.IfSyntaxTestDataClass.IfElseNested
alt test
alt ""string.IsNullOrEmpty(word)""
group return false
end
end
group return true
end
else 
alt ""!(string.IsNullOrEmpty(word))""
group return false
end
else 
alt test
group return true
end
else 
group return false
end
end
end
end
group return true
end
PumlSequenceDiagramTest.TestData.SingleMethod.IfSyntaxTestDataClass.IfElseNested --> 
@enduml";

    internal const string RESULT_IFMETHOD_MULTILINE_CONDITION =
@"@startuml
 --> PumlSequenceDiagramTest.TestData.SingleMethod.IfSyntaxTestDataClass.IfMultiline
alt ""test && condition2""
group return true
end
end
group return false
end
PumlSequenceDiagramTest.TestData.SingleMethod.IfSyntaxTestDataClass.IfMultiline --> 
@enduml";
    }
}


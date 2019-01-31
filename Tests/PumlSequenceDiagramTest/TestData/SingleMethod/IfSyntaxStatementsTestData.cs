using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PumlDiagramTest.TestData.SingleMethod
{
    internal static class IfSyntaxStatementsTestData
    {
        internal const string SOURCE_IFMETHOD_SINGLELINE =
            @"namespace test 
                {
                public class IfClass
                {
                    public bool Test()
                    {
                        bool test = false;
                        if(test)
                            return true;
                        return false;
                    }
                }
            }";

        internal const string SOURCE_IFMETHOD_SINGLEBLOCK =
          @"namespace test 
                {
                public class IfClass
                {
                    public bool Test()
                    {
                        bool test = false;
                        if(test)
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }";

        internal const string RESULT_IFMETHOD_SINGLELINE =
@"@startuml
 --> test.IfClass.Test
alt test
group return true
end
end
group return false
end
test.IfClass.Test --> 
@enduml";


        internal const string SOURCE_IFELSEMETHOD_SINGLELINE =
               @"namespace test 
                {
                public class IfClass
                {
                    public bool Test()
                    {
                        bool test = false;
                        if(test)
                            return true;
                        else
                            return false;
                        return true;
                    }
                }
            }";

        internal const string SOURCE_IFELSEMETHOD_SINGLEBLOCK =
             @"namespace test 
                {
                public class IfClass
                {
                    public bool Test()
                    {
                        bool test = false;
                        if(test) 
                        {
                            return true;
                        }   
                        else
                        {
                            return false;
                        }
                        return true;
                    }
                }
            }";

        internal const string RESULT_IFELSEMETHOD_SINGLELINE =
@"@startuml
 --> test.IfClass.Test
alt test
group return true
end
else 
group return false
end
end
group return true
end
test.IfClass.Test --> 
@enduml";
    
    internal const string SOURCE_IFELSENESTED =
             @"namespace test 
                {
                public class IfClass
                {
                    public bool Test()
                    {
                        string word = "";
                        bool test = false;
                        if(test) 
                        {
                            if(string.IsNullOrEmpty(word))
                                return false;
                            return true;                            
                        }   
                        else
                        {
                            if(!(string.IsNullOrEmpty(word)))
                                return false;
                            else if (test)
                            {
                                return true;
                            }
                            else
                                return false;
                        }
                        return true;
                    }
                }
            }";
        internal const string RESULT_IFELSENESTED =
@"@startuml
 --> test.IfClass.Test
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
test.IfClass.Test --> 
@enduml";
    }
}


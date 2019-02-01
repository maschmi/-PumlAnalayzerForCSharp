using System;
using System.Collections.Generic;

namespace DemoNet
{
    public class ClassB
    {
        public string MyProperty { get; set; }

        public ClassB(string count)
        {
            MyProperty = count;
        }

        public void WriteToLog()
        {
            Console.WriteLine(MyProperty);
        }

        public void WriteList(IEnumerable<int> stringList)
        {
            foreach(var element in stringList)
                Console.WriteLine(element);
        }

        public string CaseMethod(int test)
        {
            switch (test)
            {
                case 1:
                    return test.ToString();
                
                case 2:
                    WriteToLog();
                    break;

                default:
                    WriteToLog();
                    return "default";
            }

            return "crash";
        }

        public string ReturnStringValue(bool input)
        {
            return input.ToString();
        }
    }
}
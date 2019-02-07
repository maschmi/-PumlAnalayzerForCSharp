using System;
using System.Collections.Generic;
using System.Text;

namespace PumlSequenceDiagramTest.TestData.SingleMethod
{
    public class IfSyntaxTestDataClass
    {
        public bool IfSingleline()
        {
            bool test = false;
            if (test)
                return true;
            return false;
        }

        public bool IfSingleblock()
        {
            bool test = false;
            if (test)
            {
                return true;
            }
            return false;
        }

        public bool IfElseSingleLine()
        {
            bool test = false;
            if (test)
                return true;
            else
                return false;

            return true;
        }

        public bool IfElseSingleBlock()
        {
            bool test = false;
            if (test)
            {
                return true;
            }
            else
            {
                return false;
            }
            return true;
        }

        public bool IfElseNested()
        {
            string word = "";
            bool test = false;
            if (test)
            {
                if (string.IsNullOrEmpty(word))
                    return false;
                return true;
            }
            else
            {
                if (!(string.IsNullOrEmpty(word)))
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

        public bool IfMultiline()
        {
            bool condition2 = false;
            bool test = false;
            if (test &&
                   condition2)
                return true;
            return false;
        }


    }

}

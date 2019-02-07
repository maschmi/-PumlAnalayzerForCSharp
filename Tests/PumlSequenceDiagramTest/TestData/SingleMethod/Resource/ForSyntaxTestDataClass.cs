using System;
using System.Collections.Generic;
using System.Text;

namespace PumlSequenceDiagramTest.TestData.SingleMethod
{
    public class ForSyntaxTestDataClass
    {
        public void ForSingleline()
        {
            IList<string> list;
            for (int i = 0; i < list.ToArray().Length; i++)
            {
                Console.WriteLine(list[i]);
            }
        }

        public void ForEachSingleline()
        {
            IEnumerable<string> list;
            foreach(var line in list.ToArray())
            {
                Console.WriteLine(line);
            }
        }

        public void ForMultiline()
        {
            IEnumerable<string> list;
            for (int i = 0; i < list
                .ToArray().Lenght; i++)
            {
                Console.WriteLine(list[i]);
            }
        }

        public void ForEachMultiline()
        {
            IEnumerable<string> list;
            foreach (var line in list
                .ToArray())
            {
                Console.WriteLine(line);
            }
        }
    }

}

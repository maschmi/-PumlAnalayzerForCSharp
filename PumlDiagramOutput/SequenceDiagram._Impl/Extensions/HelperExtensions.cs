using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDiagram.Extensions
{
    static class HelperExtensions
    {
        public static string MaskSpecialChars(this string original)
        {
            var notMasked = original.Trim(new char[] { '"' });
            if (notMasked.ContainsSpecialChars())
                return '"' + notMasked + '"';

            return notMasked;
        }

        public static bool ContainsSpecialChars(this string toTest)
        {
            foreach(char c in toTest.ToLowerInvariant())
            {
                if (!char.IsLetterOrDigit(c) && c != '.')
                    return true;
                
            }
            return false;
        }
    }
}

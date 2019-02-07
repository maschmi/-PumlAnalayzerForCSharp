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

        public static string MakeItSingleLine(this string original)
        {
            var notSingleLined = original.Trim(new char[] { '"' });
            var lines = notSingleLined.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var sb = new StringBuilder();
            bool addWhitespace = true;

            var lineCount = lines.Count();
            for(int i = 0; i < lineCount; i++)
            {
                var currentLine = lines[i].Trim(new char[] { ' ', '\t' });
                if (currentLine.StartsWith(".") || i == 0)
                {
                    addWhitespace = false;
                }

                if (addWhitespace)
                    sb.Append(" ");
                    
                sb.Append(currentLine);

                addWhitespace = !currentLine.EndsWith(".");
            }                

            return sb.ToString();
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

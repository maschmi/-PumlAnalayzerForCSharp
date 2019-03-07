using System;
using System.Collections.Generic;
using System.Text;

namespace CodeAnalyzer
{
    public interface IClassAnalyzer
    {
        IEnumerable<string> GetMethodNamesForClass(string className);
    }
}

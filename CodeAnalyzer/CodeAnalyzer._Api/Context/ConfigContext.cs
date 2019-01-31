using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzer.Context
{
    //DTO
    public class ConfigContext
    {
        public InterfaceResolverType IFaceResolver { get; }

        public ConfigContext(InterfaceResolverType ifaceResolver = InterfaceResolverType.ProjectLevel)
        {
            IFaceResolver = ifaceResolver;
        }
    }
}

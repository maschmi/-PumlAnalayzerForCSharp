using CodeAnalyzer.Context;
using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzer.InterfaceResolver
{
    public static class InterfaceResolverFactory
    {
        public static IInterfaceResolver GetInterfaceResolver(ISolutionLoader soltion, IProjectAnalyzer project, IDoLog logger, ConfigContext ctx)
        {
            if (ctx.IFaceResolver == InterfaceResolverType.ProjectLevel)
                return new ProjectInterfaceResolver(project.AnalyzedClasses, project.AnalyzedFiles, logger);

            throw new NotImplementedException();
        }
    }
}

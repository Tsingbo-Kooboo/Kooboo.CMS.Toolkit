using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.CMS.Toolkit.HtmlMinify
{
    public class AssemblyInitializer : IDependencyRegistrar
    {
        private class ResolvingObserver : IResolvingObserver
        {
            public object OnResolved(object resolvedObject)
            {
                System.Web.Mvc.GlobalFilters.Filters.Add(new HtmlCompressActionFilterAttribute(), 100);
                return resolvedObject;
            }

            public int Order
            {
                get { return 100; }
            }
        }

        public int Order
        {
            get { return 100; }
        }

        public void Register(IContainerManager containerManager, Common.Runtime.ITypeFinder typeFinder)
        {
            containerManager.AddResolvingObserver(new ResolvingObserver());
        }
    }
}

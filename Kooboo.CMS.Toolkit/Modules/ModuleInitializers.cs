using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Toolkit.Modules
{
    public class ModuleInitializers
    {
        private static List<IModuleInitializer> _initializers;

        static ModuleInitializers()
        {
            _initializers = new List<IModuleInitializer>();
        }

        public static List<IModuleInitializer> Initializers
        {
            get
            {
                return _initializers;
            }
        }

        public static void Include(string moduleName, string siteName)
        {
            Initializers.Each(it => it.Include(moduleName, siteName));
        }

        public static void Exclude(string moduleName, string siteName)
        {
            Initializers.Each(it => it.Exclude(moduleName, siteName));
        }

        //public static void UnInstall(string moduleName)
        //{
        //    Initializers.Each(it => it.UnInstall(moduleName));
        //}
    }
}
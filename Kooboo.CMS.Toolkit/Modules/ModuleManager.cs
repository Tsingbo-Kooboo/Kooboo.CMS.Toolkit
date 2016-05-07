using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using Kooboo.CMS.Sites.Extension.ModuleArea;

namespace Kooboo.CMS.Toolkit.Modules
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(Kooboo.CMS.Sites.Services.ModuleManager), Order = 50)]
    public class ModuleManager : Kooboo.CMS.Sites.Services.ModuleManager
    {
        public override void AddSiteToModule(string moduleName, string siteName)
        {
            base.AddSiteToModule(moduleName, siteName);
            ModuleInitializers.Include(moduleName, siteName);
        }

        public override void RemoveSiteFromModule(string moduleName, string siteName)
        {
            base.RemoveSiteFromModule(moduleName, siteName);
            ModuleInitializers.Exclude(moduleName, siteName);
        }

        [Obsolete("Method Removed in Kooboo CMS 4.3+")]
        public void Uninstall(string moduleName)
        {
            //ModuleInitializers.UnInstall(moduleName);
        }
    }
}
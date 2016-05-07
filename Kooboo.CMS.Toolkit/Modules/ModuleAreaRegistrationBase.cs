using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Web.Mvc;

using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Menu;
using Kooboo.Web.Mvc.WebResourceLoader;

namespace Kooboo.CMS.Toolkit.Modules
{
    public abstract class ModuleAreaRegistrationBase : AreaRegistration
    {
        public override void RegisterArea(AreaRegistrationContext context)
        {
            // Register module menu
            string menuFilePath = AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "CMSMenu.config");
            MenuFactory.RegisterAreaMenu(AreaName, menuFilePath);

            // Register module web resources
            string webResourcesFilePath = AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "WebResources.config");
            ConfigurationManager.RegisterSection(AreaName, webResourcesFilePath);
        }
    }
}
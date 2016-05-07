#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using System.Web.Mvc;
using System.IO;
using Kooboo;
using Kooboo.Web.Mvc;

namespace $RootNamespace$
{
    public class ModuleAreaRegistration : AreaRegistration
    {
        public const string ModuleName = "$RootNamespace$";
        public override string AreaName
        {
            get
            {
                return ModuleName;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               ModuleName + "_default",
                ModuleName + "/{controller}/{action}",
                new { controller = "Admin", action = "Index" }
                , null
                , new[] { "$RootNamespace$.Controllers", "Kooboo.Web.Mvc", "Kooboo.Web.Mvc.WebResourceLoader" }
            );
            var areaPath = AreaHelpers.CombineAreaFilePhysicalPath(AreaName);
            if (Directory.Exists(areaPath))
            {
                var menuFile = AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "CMSMenu.config");
                if (File.Exists(menuFile))
                {
                    Kooboo.Web.Mvc.Menu.MenuFactory.RegisterAreaMenu(AreaName, menuFile);
                }
                var resourceFile = AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "WebResources.config");
                if (File.Exists(resourceFile))
                {
                    Kooboo.Web.Mvc.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, resourceFile);
                }
            }
        }
    }
}

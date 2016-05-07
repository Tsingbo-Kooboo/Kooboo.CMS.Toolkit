using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;

using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Menu;

namespace Kooboo.CMS.Toolkit.Modules
{
    public class ModuleControllerActionMenuItemInitializer : DefaultMenuItemInitializer
    {
        protected override bool GetIsActive(MenuItem menuItem, ControllerContext controllerContext)
        {
            string areaName = AreaHelpers.GetAreaName(controllerContext.RouteData);
            string controllerName = controllerContext.RouteData.Values.GetString("controller");
            string actionName = controllerContext.RouteData.Values.GetString("action");
            if(menuItem.Area.Equals(areaName, StringComparison.OrdinalIgnoreCase) &&
                menuItem.Controller.Equals(controllerName, StringComparison.OrdinalIgnoreCase) &&
                menuItem.Action.Equals(actionName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;

using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Menu;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Toolkit.Modules
{
    public class ModuleSiteMenuItems : MenuItemTemplate
    {
        public override IEnumerable<MenuItem> GetItems(string areaName, ControllerContext controllerContext)
        {
            List<MenuItem> items = new List<MenuItem>();
            MenuItemTemplate template = this;
            Site rootSite = Site.Current.RootSite();
            if(template.Name.Equals(rootSite.Name, StringComparison.OrdinalIgnoreCase))
            {
                if(template.ItemContainers != null)
                {
                    foreach(var itemContainer in template.ItemContainers)
                    {
                        items.AddRange(itemContainer.GetItems(areaName, controllerContext));
                    }
                }
            }
            return items;
        }
    }
}
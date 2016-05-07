using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Toolkit
{
    public static class PageRequestContextExtensions
    {
        public static Page TempPage(this PageRequestContext pageRequestContext)
        {
            var tempPage = pageRequestContext.ControllerContext.Controller.ViewData["TempPage"] as Page;
            if(tempPage == null)
            {
                tempPage = pageRequestContext.Page.Copy();
                pageRequestContext.ControllerContext.Controller.ViewData["TempPage"] = tempPage;
            }

            return tempPage;
        }
    }
}
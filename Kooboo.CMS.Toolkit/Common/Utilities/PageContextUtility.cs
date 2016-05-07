using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Sites.Web;

namespace Kooboo.CMS.Toolkit
{
    public class PageContextUtility
    {
        public static void InitPageContext()
        {
            if(Page_Context.Current.PageRequestContext == null)
            {
                var viewContext = MvcUtility.GetViewContext();
                InitPageContext(viewContext.Controller.ControllerContext);
            }
        }

        public static void InitPageContext(ControllerContext controllerContext)
        {
            if(Page_Context.Current.PageRequestContext == null)
            {
                var site = Site.Current;
                var page = ServiceFactory.PageManager.GetDefaultPage(site);
                var pageRequestContext = new PageRequestContext(
                        controllerContext,
                        site,
                        page,
                        site.FrontRequestChannel(),
                        "/");

                Page_Context.Current.InitContext(pageRequestContext, controllerContext);
                Page_Context.Current.DisableInlineEditing = true;
            }
        }
    }
}
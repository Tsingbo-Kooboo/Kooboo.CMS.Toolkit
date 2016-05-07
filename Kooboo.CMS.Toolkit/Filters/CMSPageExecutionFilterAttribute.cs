using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Toolkit.Filters
{
    public class CMSPageExecutionFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var controllerContext = filterContext.Controller.ControllerContext;
            //
            var siteName = controllerContext.RequestContext.GetRequestValue("siteName");
            var site = Kooboo.CMS.Sites.Services.ServiceFactory.SiteManager.GetSite(new List<string>() { siteName }).AsActual();
            Kooboo.CMS.Sites.Models.Site.Current = site;
            //
            var pageName = controllerContext.RequestContext.GetRequestValue("pageName");
            Kooboo.CMS.Sites.Models.Page page = null;
            if (string.IsNullOrEmpty(pageName))
            {
                page = Kooboo.CMS.Sites.Services.ServiceFactory.PageManager.All(site, string.Empty).First().AsActual();
            }
            else
            {
                page = new Kooboo.CMS.Sites.Models.Page(site, pageName).LastVersion().AsActual();
            }
            //
            var channel = Kooboo.CMS.Sites.Web.FrontRequestChannel.Unknown;
            var pageRequestContext = new PageRequestContext(controllerContext, site, page, channel, "");
            Page_Context.Current.InitContext(pageRequestContext, controllerContext);
        }
    }
}

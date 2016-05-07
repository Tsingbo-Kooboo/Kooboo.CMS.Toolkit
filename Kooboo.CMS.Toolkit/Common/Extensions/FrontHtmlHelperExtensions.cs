using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;

using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Toolkit
{
    public static class FrontHtmlHelperExtensions
    {
        public static IHtmlString PageLink(this Kooboo.CMS.Sites.View.FrontHtmlHelper frontHtmlHelpr, Page page)
        {
            return PageLink(frontHtmlHelpr, page, null);
        }

        public static IHtmlString PageLink(this Kooboo.CMS.Sites.View.FrontHtmlHelper frontHtmlHelpr, Page page, object routeValues)
        {
            return PageLink(frontHtmlHelpr, page, routeValues, null);
        }

        public static IHtmlString PageLink(this Kooboo.CMS.Sites.View.FrontHtmlHelper frontHtmlHelpr, Page page, object routeValues, object htmlAttributes)
        {
            return frontHtmlHelpr.PageLink(page.LinkText, page.FullName, routeValues, htmlAttributes);
        }
    }
}
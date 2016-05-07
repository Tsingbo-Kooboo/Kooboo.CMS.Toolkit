using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;
using System.Web.WebPages;

namespace Kooboo.CMS.Toolkit
{
    public class MvcUtility
    {
        public static UrlHelper GetUrlHelper()
        {
            return ((WebViewPage)WebPageContext.Current.Page).Url;
        }

        public static HtmlHelper GetHtmlHelper()
        {
            return ((WebViewPage)WebPageContext.Current.Page).Html;
        }

        public static ViewContext GetViewContext()
        {
            return ((WebViewPage)WebPageContext.Current.Page).ViewContext;
        }
    }
}
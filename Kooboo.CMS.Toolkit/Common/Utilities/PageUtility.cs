using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Kooboo.Web.Url;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Sites.Web;

namespace Kooboo.CMS.Toolkit
{
    public class PageUtility
    {
        private const string Slash = "/";
        private const string Debug = "/dev~";

        public static string PrependSlash(string url)
        {
            if (!url.StartsWith(Slash))
            {
                url = Slash + url;
            }
            return url;
        }

        public static string AppendSlash(string url)
        {
            return VirtualPathUtility.AppendTrailingSlash(url);
        }

        public static string RemoveDebug(Site site, string url)
        {
            url = PrependSlash(url);
            if (url.StartsWith(Debug, StringComparison.OrdinalIgnoreCase))
            {
                int startIndex = Debug.Length + site.FullName.Length;
                url = url.Substring(startIndex);
            }
            return url;
        }

        public static Page CurrentPage
        {
            get
            {
                return Page_Context.Current.PageRequestContext.Page;
            }
        }

        public static string CurrentPageUrl
        {
            get
            {
                var request = Page_Context.Current.PageRequestContext.ControllerContext.RequestContext.HttpContext.Request as FrontHttpRequestWrapper;
                string currentPageUrl = request.RequestUrl;
                return PrependSlash(currentPageUrl);
            }
        }

        #region Absolute page url

        public static string GetAbsolutePageUrl()
        {
            return GetAbsolutePageUrl(CurrentPageUrl);
        }

        public static string GetAbsolutePageUrl(string pageUrl)
        {
            return GetAbsolutePageUrl(Site.Current, pageUrl);
        }

        public static string GetAbsolutePageUrl(Site site)
        {
            return GetAbsolutePageUrl(site, "/");
        }

        public static string GetAbsolutePageUrl(Site site, string pageUrl)
        {
            FrontRequestChannel requestChannel = site.FrontRequestChannel();
            string wrapperPageUrl = FrontUrlHelper.WrapperUrl(pageUrl, site, requestChannel).ToString();
            string host = GetHost(site);
            return HttpContext.Current.Request.ToAbsoluteUrl(host, wrapperPageUrl);
        }

        public static string GetAbsolutePageUrl(Site site, Page page)
        {
            return GetAbsolutePageUrl(site, page, null);
        }

        public static string GetAbsolutePageUrl(Site site, Page page, object values)
        {
            UrlHelper urlHelper = MvcUtility.GetUrlHelper();
            FrontRequestChannel requestChannel = site.FrontRequestChannel();
            string pageUrl = FrontUrlHelper.GeneratePageUrl(urlHelper, site, page, values, requestChannel).ToString();
            string host = GetHost(site);
            return HttpContext.Current.Request.ToAbsoluteUrl(host, pageUrl);
        }

        public static string GetHost(Site site)
        {
            FrontRequestChannel requestChannel = site.FrontRequestChannel();
            if (requestChannel == FrontRequestChannel.Host || requestChannel == FrontRequestChannel.HostNPath)
            {
                return site.Domains.FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? HttpContext.Current.Request.Headers["Host"];
            }
            else
            {
                return HttpContext.Current.Request.Headers["Host"];
            }
        }

        #endregion
    }
}
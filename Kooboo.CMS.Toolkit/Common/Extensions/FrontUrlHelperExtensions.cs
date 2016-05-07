using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using System.IO;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Web.Url;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Web;
using Kooboo.CMS.Sites.View;

namespace Kooboo.CMS.Toolkit
{
    public static class FrontUrlHelperExtensions
    {
        /// <summary>
        /// The script file url of current site
        /// </summary>
        /// <param name="frontUrlHelper">The front url helper</param>
        /// <param name="relativeScriptFilePath">The relative script file path of current site</param>
        /// <returns></returns>
        public static IHtmlString ScriptFileUrl(this FrontUrlHelper frontUrlHelper, string relativeScriptFilePath)
        {
            Site site = Site.Current;
            return ScriptFileUrl(frontUrlHelper, site, relativeScriptFilePath);
        }

        /// <summary>
        /// The script file url of current site
        /// </summary>
        /// <param name="frontUrlHelper">The front url helper</param>
        /// <param name="baseUri">Base uri</param>
        /// <param name="relativeScriptFilePath">The relative script file path of current site</param>
        /// <returns></returns>
        public static IHtmlString ScriptFileUrl(this FrontUrlHelper frontUrlHelper, string baseUri, string relativeScriptFilePath, bool forceSSL = false)
        {
            Site site = Site.Current;
            return ScriptFileUrl(frontUrlHelper, site, baseUri, relativeScriptFilePath, forceSSL);
        }

        /// <summary>
        /// The script file url of site
        /// </summary>
        /// <param name="frontUrlHelper">The front url helper</param>
        /// <param name="site">The site</param>
        /// <param name="relativeScriptFilePath">The relative script file path of site</param>
        /// <returns></returns>
        public static IHtmlString ScriptFileUrl(this FrontUrlHelper frontUrlHelper, Site site, string relativeScriptFilePath)
        {
            return ScriptFileUrl(frontUrlHelper, site, site.ResourceDomain, relativeScriptFilePath);
        }

        /// <summary>
        /// The script file url o fsite
        /// </summary>
        /// <param name="frontUrlHelper">The front url helper</param>
        /// <param name="site">The site</param>
        /// <param name="baseUri">Base uri</param>
        /// <param name="relativeScriptFilePath">The relative script file path of site</param>
        /// <returns></returns>
        public static IHtmlString ScriptFileUrl(this FrontUrlHelper frontUrlHelper, Site site, string baseUri, string relativeScriptFilePath, bool forceSSL = false)
        {
            string resourceDomain = site.ResourceDomain.FormatUrlWithProtocol(forceSSL);

            bool scriptFileExists = false;
            string scriptFileUrl = String.Empty;
            string scriptFilePhysicalPath = String.Empty;

            do
            {
                site = site.AsActual();

                ScriptFile scriptFile = new ScriptFile(site, String.Empty);
                if (scriptFile != null)
                {
                    scriptFileUrl = UrlUtility.Combine(scriptFile.VirtualPath, relativeScriptFilePath);
                   
                    scriptFilePhysicalPath = HttpContext.Current.Server.MapPath(scriptFileUrl);
                    scriptFileExists = File.Exists(scriptFilePhysicalPath);
                }

                site = site.Parent;
            } while (site != null && !scriptFileExists);

            if (!String.IsNullOrEmpty(resourceDomain))
            {
                baseUri = resourceDomain; // CDN have high priority
            }
            return new HtmlString(UrlUtility.ToHttpAbsolute(baseUri, scriptFileUrl));
        }

        public static IHtmlString SiteScriptsUrl(this FrontUrlHelper frontUrlHelper, Site site)
        {
            return SiteScriptsUrl(frontUrlHelper, site, site.ResourceDomain);
        }

        public static IHtmlString SiteScriptsUrl(this FrontUrlHelper frontUrlHelper, Site site, string baseUri)
        {
            string url = Page_Context.Current.Url.Action("Scripts", "Resource", new { siteName = site.FullName, version = site.Version, area = String.Empty });
            string wrapperUrl = FrontUrlHelper.WrapperUrl(url, site, FrontRequestChannel.Debug).ToString();
            return new HtmlString(UrlUtility.ToHttpAbsolute(baseUri, wrapperUrl));
        }

        /// <summary>
        /// The theme file url of current site
        /// </summary>
        /// <param name="frontUrlHelper">The front url helper</param>
        /// <param name="relativeThemeFilePath">The relative theme file path of current site</param>
        /// <returns></returns>
        public static IHtmlString ThemeFileUrl(this FrontUrlHelper frontUrlHelper, string relativeThemeFilePath)
        {
            Site site = Site.Current;
            return ThemeFileUrl(frontUrlHelper, site, relativeThemeFilePath);
        }

        /// <summary>
        /// The theme file url of current site
        /// </summary>
        /// <param name="frontUrlHelper">The front url helper</param>
        /// <param name="baseUri">Base uri</param>
        /// <param name="relativeThemeFilePath">The relative theme file path of current site</param>
        /// <returns></returns>
        public static IHtmlString ThemeFileUrl(this FrontUrlHelper frontUrlHelper, string baseUri, string relativeThemeFilePath)
        {
            Site site = Site.Current;
            return ThemeFileUrl(frontUrlHelper, site, baseUri, relativeThemeFilePath);
        }

        /// <summary>
        /// The theme file url of site
        /// </summary>
        /// <param name="frontUrlHelper">The front url helper</param>
        /// <param name="site">The site</param>
        /// <param name="relativeThemeFilePath">The relative theme file path of site</param>
        /// <returns></returns>
        public static IHtmlString ThemeFileUrl(this FrontUrlHelper frontUrlHelper, Site site, string relativeThemeFilePath)
        {
            return ThemeFileUrl(frontUrlHelper, site, site.ResourceDomain, relativeThemeFilePath);
        }

        /// <summary>
        /// The theme file url of site
        /// </summary>
        /// <param name="frontUrlHelper">The front url helper</param>
        /// <param name="site">The site</param>
        /// <param name="baseUri">Base uri</param>
        /// <param name="relativeThemeFilePath">The relative theme file path of site</param>
        /// <returns></returns>
        public static IHtmlString ThemeFileUrl(this FrontUrlHelper frontUrlHelper, Site site, string baseUri, string relativeThemeFilePath)
        {
            string resourceDomain = site.ResourceDomain;

            bool themeFileExists = false;
            string themeFileUrl = String.Empty;
            string themeFilePhysicalPath = String.Empty;

            do
            {
                site = site.AsActual();

                Theme theme = new Theme(site, site.Theme).LastVersion();
                themeFileUrl = UrlUtility.Combine(theme.VirtualPath, relativeThemeFilePath);
                themeFilePhysicalPath = HttpContext.Current.Server.MapPath(themeFileUrl);
                themeFileExists = File.Exists(themeFilePhysicalPath);

                site = theme.Site.Parent;
            } while (site != null && !themeFileExists);

            if (!String.IsNullOrEmpty(resourceDomain))
            {
                baseUri = resourceDomain; // CDN have high priority
            }
            return new HtmlString(UrlUtility.ToHttpAbsolute(baseUri, themeFileUrl));
        }
        /// <summary>
        /// The appointed theme file url of site
        /// </summary>
        /// <param name="frontUrlHelper">The front url helper</param>
        /// <param name="themeName">the theme</param>
        /// <param name="relativeUrl">The relative theme file path of site</param>
        /// <returns></returns>
        public static IHtmlString ThemeFileUrl(this FrontUrlHelper frontUrlHelper, string baseUri, string themeName, string relativeUrl)
        {
            var site = Site.Current;
            string resourceDomain = site.ResourceDomain;
            IHtmlString url = new HtmlString("");
            if (!string.IsNullOrEmpty(themeName))
            {
                var fileExists = false;
                var themeFileUrl = "";
                do
                {
                    site = site.AsActual();
                    Theme theme = new Theme(site, themeName).LastVersion();
                    themeFileUrl = Kooboo.Web.Url.UrlUtility.Combine(theme.VirtualPath, relativeUrl);
                    var physicalPath = HttpContext.Current.Server.MapPath(themeFileUrl);
                    fileExists = File.Exists(physicalPath);

                    site = theme.Site.Parent;
                } while (site != null && !fileExists);
                if (!string.IsNullOrEmpty(resourceDomain))
                {
                    baseUri = resourceDomain;
                }
                return new HtmlString(UrlUtility.ToHttpAbsolute(baseUri, themeFileUrl));
            }
            return frontUrlHelper.ThemeFileUrl(baseUri, relativeUrl);
        }
    }
}
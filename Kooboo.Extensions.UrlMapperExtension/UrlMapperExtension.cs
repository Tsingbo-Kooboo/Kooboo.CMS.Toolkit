using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo;
using Kooboo.CMS.Sites.Web;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Sites.Models;
using System.Text.RegularExpressions;

namespace Kooboo.Extensions.UrlMapperExtension
{
    [Dependency(typeof(IUrlMapper), Key = "UrlMapperExtension")]
    public class UrlMapperExtension : IUrlMapper
    {
        #region IUrlMapper Members

        public bool Map(Site site, string inputUrl, out string outputUrl, out RedirectType redirectType)
        {
            outputUrl = string.Empty;
            redirectType = RedirectType.Found_Redirect_302;
            if (string.IsNullOrEmpty(inputUrl))
            {
                return false;
            }
            var mapSettings = Kooboo.CMS.Sites.Services.ServiceFactory.UrlRedirectManager.All(site, "");
            //inputUrl = inputUrl.Trim('/');
            if (!inputUrl.StartsWith("/"))
            {
                inputUrl = "/" + inputUrl;
            }
            foreach (var setting in mapSettings)
            {
                var inputPattern = setting.InputUrl;//.Trim('/');

                if (setting.Regex)
                {
                    try
                    {
                        if (Regex.IsMatch(inputUrl, inputPattern, RegexOptions.IgnoreCase))
                        {
                            outputUrl = wrapOutputUrl(Regex.Replace(inputUrl, inputPattern, setting.OutputUrl, RegexOptions.IgnoreCase));
                            redirectType = setting.RedirectType;
                            return true;
                        }
                    }
                    catch (Exception e)
                    {
                        Kooboo.HealthMonitoring.Log.LogException(e);
                    }

                }
                else
                {
                    if (inputUrl.EqualsOrNullEmpty(inputPattern, StringComparison.CurrentCultureIgnoreCase))
                    {
                        outputUrl = wrapOutputUrl(setting.OutputUrl);
                        redirectType = setting.RedirectType;
                        return true;
                    }
                }
            }
            return false;
        }

        private string wrapOutputUrl(string outputUrl)
        {
            if (outputUrl.StartsWith("~"))
            {
                var currentUri = System.Web.HttpContext.Current.Request.Url;
                var baseUrl = String.Format("{0}://{1}{2}", currentUri.Scheme, currentUri.Host, currentUri.Port != 80 && currentUri.Port != 443 ? ":" + currentUri.Port.ToString() : "");
                return Kooboo.Web.Url.UrlUtility.Combine(baseUrl, outputUrl.Substring(1));
            }
            return outputUrl;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Web;

namespace Kooboo.CMS.Toolkit
{
    public static class SiteExtensions
    {
        public static Site RootSite(this Site site)
        {
            var rootSite = site;
            while(rootSite.Parent != null)
            {
                rootSite = rootSite.Parent;
            }
            return rootSite;
        }

        public static FrontRequestChannel FrontRequestChannel(this Site site)
        {
            if(site.Domains != null && site.Domains.Any())
            {
                if(String.IsNullOrEmpty(site.SitePath))
                {
                    return Sites.Web.FrontRequestChannel.Host;
                }
                else
                {
                    return Sites.Web.FrontRequestChannel.HostNPath;
                }
            }
            else
            {
                return Sites.Web.FrontRequestChannel.Debug;
            }
        }
    }
}
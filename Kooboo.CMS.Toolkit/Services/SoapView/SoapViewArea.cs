using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;

namespace Kooboo.CMS.Toolkit.Services.SoapView
{
    public class SoapViewArea : AreaRegistration
    {
        public override string AreaName
        {
            get { return "SoapView"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SoapView.Default",
                "_SoapView/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "Kooboo.CMS.Toolkit.Services.SoapView" }
            );
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace Kooboo.CMS.Toolkit.Controls
{
    public class ToolkitControlsArea : AreaRegistration
    {
        public override string AreaName
        {
            get { return "ToolkitControls"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "ToolkitControls_default",
               "ToolkitControls/{controller}/{action}", 
               new { action = "Index" }//, 
               , null
               , new[] { "Kooboo.CMS.Toolkit.Controls.Controllers", "Kooboo.Web.Mvc", "Kooboo.Web.Mvc.WebResourceLoader" }
           );
        }
    }
}

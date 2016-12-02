using Kooboo.CMS.Toolkit.Controls.RichTextEditors.Models;
using Kooboo.CMS.Web.Authorizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;
using System.Web.Mvc.Filters;
using System.Web.Mvc.Routing;
using System.Web.Mvc.Ajax;

namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    //[LargeFileAuthorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
    [AllowAnonymous]
    public class UEditorController : Controller
    {
        public enum RequestAction
        {
            unknown,
            config,
            uploadimage,
            uploadscrawl,
            uploadvideo,
            uploadfile,
            listimage,
            listfile,
            catchimage
        }

        public class BrowserModel
        {
            public string action { get; set; }

            public long noCache { get; set; }
        }

        [AllowAnonymous]
        //[Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 99)]
        [HttpGet]
        public ActionResult Browser()
        {
            return Content(Kooboo.Web.Script.Serialization.JsonHelper.ToJSON(new UEditorConfig()), "application/json");
            var actionString = Request.QueryString.Get("action");
            RequestAction action;
            if (Enum.TryParse(actionString, out action))
            {
                switch (action)
                {
                    case RequestAction.config:
                        return Json(new UEditorConfig(), JsonRequestBehavior.AllowGet);
                    case RequestAction.uploadimage:
                        break;
                    case RequestAction.uploadscrawl:
                        break;
                    case RequestAction.uploadvideo:
                        break;
                    case RequestAction.uploadfile:
                        break;
                    case RequestAction.listimage:
                        break;
                    case RequestAction.listfile:
                        break;
                    case RequestAction.catchimage:
                        break;
                    default:
                        break;
                }
            }

            return Content("Hello " + action);
        }
    }
}

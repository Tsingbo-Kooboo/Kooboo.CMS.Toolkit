using Kooboo.CMS.Web.Authorizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [LargeFileAuthorization(AreaName = "Contents", Group = "", Name = "MediaLibrary", Order = 1)]
    public class UEditorController : ContentControllerBase
    {
        public ActionResult Browser(string action)
        {
            return Content("Hello");
        }
    }
}

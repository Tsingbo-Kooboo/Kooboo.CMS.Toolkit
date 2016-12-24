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
using Kooboo.Web.Script.Serialization;
using Newtonsoft.Json;
using Kooboo.CMS.Toolkit.Controls.RichTextEditors.Services;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System.IO;

namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [LargeFileAuthorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
    //[AllowAnonymous]
    public class UEditorController : ContentControllerBase
    {
        private readonly IMediaService _mediaService;

        public UEditorController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

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

        //[AllowAnonymous]
        public ActionResult Browser(string repositoryName,
            string siteName,
            string folderName = "upload",
            int start = 0,
            int size = 20,
            string[] source = null)
        {
            var actionString = Request.QueryString.Get("action");
            RequestAction action;
            if (Enum.TryParse(actionString, out action))
            {
                var repository = new Repository(repositoryName).AsActual();
                var folder = new MediaFolder(repository, folderName);
                var config = _mediaService.GetConfig(repositoryName, siteName, folderName);
                switch (action)
                {
                    case RequestAction.config:
                        return Content(JsonConvert.SerializeObject(config), "application/json");
                    case RequestAction.uploadscrawl:
                        var base64String = Request.Form[config.ScrawlFieldName];
                        var bytes = Convert.FromBase64String(base64String);
                        using (var stream = new MemoryStream(bytes))
                        {
                            var fileName = PathFormatter.Format("temp.png", config.ScrawlPathFormat);
                            var uploadResult = _mediaService.Upload(repository, siteName, folder, fileName, stream);
                            return Content(JsonConvert.SerializeObject(uploadResult), "application/json");
                        }
                    case RequestAction.uploadimage:
                    case RequestAction.uploadvideo:
                    case RequestAction.uploadfile:
                        var file = Request.Files[config.ImageFieldName];
                        if (file != null)
                        {
                            var uploadResult = _mediaService.Upload(repository, siteName, folder, file.FileName, file.InputStream);
                            return Content(JsonConvert.SerializeObject(uploadResult), "application/json");
                        }
                        break;
                    case RequestAction.listimage:
                    case RequestAction.listfile:
                        var data = _mediaService.GetList(repositoryName, siteName, folderName, "", start, size);
                        return Content(JsonConvert.SerializeObject(data), "application/json");
                    case RequestAction.catchimage:
                        var catchResult = _mediaService.Crawler(repository, siteName, folder, source, config.CatcherPathFormat);
                        return Content(JsonConvert.SerializeObject(catchResult), "application/json");
                    default:
                        break;
                }
            }

            return Content("Hello " + action);
        }

        public ActionResult Search(string repositoryName,
            string siteName,
            string folderName = "upload",
            int start = 0,
            int size = 20,
            string[] source = null)
        {

            return null;
        }
    }
}

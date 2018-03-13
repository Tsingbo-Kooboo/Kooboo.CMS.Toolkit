using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Kooboo.CMS.Common;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Web.Authorizations;
using Kooboo.CMS.Content.Query;
using Kooboo.Web.Url;

namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [LargeFileAuthorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
    public class FroalaController : ContentControllerBase
    {
        private readonly IBaseDir baseDir;
        private readonly MediaContentManager mediaContentManager;
        private readonly MediaFolderManager mediaFolderManager;

        public FroalaController(IBaseDir baseDir, MediaContentManager mediaContentManager, MediaFolderManager mediaFolderManager)
        {
            this.baseDir = baseDir;
            this.mediaContentManager = mediaContentManager;
            this.mediaFolderManager = mediaFolderManager;
        }

        [Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 99)]
        public ActionResult Index(string folderName, string search, int? page, int? pageSize, string sortField, string sortDir, string listType)
        {
            var folder = mediaFolderManager.Get(Repository, folderName);
            if (folder == null)
            {
                return new JsonResult
                {
                    Data = new List<object>(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            var result = folder.CreateQuery().ToList().Where(it =>
            {
                string mimeType = System.Web.MimeMapping.GetMimeMapping(it.PhysicalPath);
                return Array.IndexOf(AllowedImageMimetypesDefault, mimeType) >= 0;
            }).Select(it =>
            {
                var virtualPath = Url.Content(it.VirtualPath);
                return new
                {
                    url = virtualPath,
                    thumb = ResizeImageUrl(virtualPath),
                    name = it.FileName
                };
            });
            return new JsonResult
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public static string[] AllowedImageMimetypesDefault = new string[] { "image/gif", "image/jpeg", "image/pjpeg", "image/x-png", "image/png", "image/svg+xml" };

        private string ResizeImageUrl(string imagePath)
        {
            var relativeUrl = Url.Action("ResizeImage", "Resource", new { siteName = Site.FullName, url = imagePath, area = "", width = 100, height = 100 });
            return UrlUtility.ToHttpAbsolute(Site.ResourceDomain, relativeUrl);
        }
    }
}

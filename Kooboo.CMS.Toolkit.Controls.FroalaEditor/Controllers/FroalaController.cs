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
using System.Web;

namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [LargeFileAuthorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
    public class FroalaController : ContentControllerBase
    {
        private readonly IBaseDir baseDir;
        private readonly MediaContentManager mediaContentManager;
        private readonly MediaFolderManager mediaFolderManager;
        private readonly TextFolderManager textFolderManager;
        private readonly SchemaManager schemaManager;

        public FroalaController(
            IBaseDir baseDir,
            MediaContentManager mediaContentManager,
            MediaFolderManager mediaFolderManager,
            TextFolderManager textFolderManager,
            SchemaManager schemaManager)
        {
            this.baseDir = baseDir;
            this.mediaContentManager = mediaContentManager;
            this.mediaFolderManager = mediaFolderManager;
            this.textFolderManager = textFolderManager;
            this.schemaManager = schemaManager;
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
                string mimeType = MimeMapping.GetMimeMapping(it.PhysicalPath);
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
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 99)]
        [HttpPost]
        public ActionResult Upload(string folderName, bool autoRename = true)
        {
            HttpFileCollectionBase files = Request.Files;
            var currentFolder = mediaFolderManager.Get(Repository, folderName);
            foreach (var f in files.AllKeys)
            {
                var file = files[f];
                if (file.ContentLength > 0)
                {
                    //if upload from ie filename will be fullpath include disk symbol 
                    var fileName = Path.GetFileName(file.FileName);
                    if (autoRename)
                    {
                        var ext = Path.GetExtension(file.FileName);
                        fileName = DateTime.Now.ToString("yyyyMMdd-hhmmss-ffff") + ext;
                    }

                    var media = mediaContentManager.Add(Repository, currentFolder, fileName, file.InputStream, true, User.Identity.Name);
                    return Json(new
                    {
                        link = Url.Content(media.VirtualPath)
                    });
                }
            }
            return Json(new
            {
                link = ""
            });
        }

        [Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 99)]
        public ActionResult Options(string folderName, string columnName)
        {
            var result = new JsonResult
            {
                Data = new Dictionary<string, string>
                {

                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            var folder = textFolderManager.Get(Repository, folderName);
            if (folder == null)
            {
                result.Data = new Dictionary<string, string>
                {
                    { "folder" , "null"}
                };
                // default options
                return result;
            }
            var schema = schemaManager.Get(Repository, folder.SchemaName);
            if (schema == null)
            {
                result.Data = new Dictionary<string, string>
                {
                    { "schema" , "null"}
                };
                return result;
            }
            var column = schema.AllColumns.FirstOrDefault(it => it.Name == columnName);
            if (column == null)
            {
                result.Data = new Dictionary<string, string>
                {
                    { "column" , "null"}
                };
                return result;
            }
            if (column.CustomSettings != null && column.CustomSettings.Count > 0)
            {
                result.Data = column.CustomSettings;
            }
            return result;
        }

        public static string[] AllowedImageMimetypesDefault = new string[] { "image/gif", "image/jpeg", "image/pjpeg", "image/x-png", "image/png", "image/svg+xml" };

        private string ResizeImageUrl(string imagePath)
        {
            var relativeUrl = Url.Action("ResizeImage", "Resource", new { siteName = Site.FullName, url = imagePath, area = "", width = 100, height = 100 });
            return UrlUtility.ToHttpAbsolute(Site.ResourceDomain, relativeUrl);
        }
    }
}

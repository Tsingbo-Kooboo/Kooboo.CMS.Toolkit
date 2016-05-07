using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query.Expressions;
using Kooboo.Web.Script.Serialization;

namespace Kooboo.CMS.Toolkit.Controls.Controllers
{
    public class CascadingController : Controller
    {
        public ActionResult Index(string repositoryName, string folder, string parentFolder, string parentUUID)
        {
            IContentQuery<TextContent> contentQuery = null;
            var repository = new Repository(repositoryName);
            var textFolder = new TextFolder(repository, folder);
            if (string.IsNullOrEmpty(parentFolder))
            {
                contentQuery = textFolder.CreateQuery()
                    .Where(new OrElseExpression(new WhereEqualsExpression(null, "ParentUUID", null), new WhereEqualsExpression(null, "ParentUUID", "")));
            }
            else
            {
                contentQuery = textFolder.CreateQuery().WhereEquals("ParentFolder", parentFolder).WhereEquals("ParentUUID", parentUUID);
            }
            var data = contentQuery.ToDictionary(it => it.UUID, it => it.GetSummary());
            return Json(data, JsonRequestBehavior.AllowGet);

        }
    }
}

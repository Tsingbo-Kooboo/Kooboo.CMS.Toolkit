using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Sites.Extension.UI;
using Kooboo.CMS.Sites.Extension.UI.WebResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace Kooboo.CMS.Toolkit.Controls.FroalaEditor.UI
{
    [Dependency(typeof(IWebResourceProvider), Key = "FroalaEditor", Order = 10)]
    public class FroalaEditorWebResource : IWebResourceProvider
    {
        public MvcRoute[] ApplyTo
        {
            get
            {
                return new MvcRoute[]
                {
                    new MvcRoute
                    {
                        Action="Edit",
                        Controller="TextContent",
                        Area="Contents"
                    },
                    new MvcRoute
                    {
                        Action="Create",
                        Controller="TextContent",
                        Area="Contents"
                    }
                };
            }
        }

        public IEnumerable<WebResource> GetScripts(RequestContext requestContext)
        {
            var scripts = new string[] { "vendor.js", "app.js" }
            .Select(it => $"<script src=\"/Areas/FroalaEditor/Scripts/{it}\"></script>")
            .ToList();
            scripts.Add("");
            yield return new WebResource
            {
                Body = new HtmlString(string.Join(string.Empty, scripts))
            };
        }

        public IEnumerable<WebResource> GetStyles(RequestContext requestContext)
        {
            var scripts = new List<string>
            {
                "<link href=\"https://cdn.bootcss.com/font-awesome/4.7.0/css/font-awesome.min.css\" rel=\"stylesheet\">",
                "<link href=\"https://cdn.bootcss.com/froala-editor/2.7.6/css/froala_editor.pkgd.min.css\" rel=\"stylesheet\">",
                "<link href=\"https://cdn.bootcss.com/froala-editor/2.7.6/css/froala_style.min.css\" rel=\"stylesheet\">",
                "<script src=\"https://cdn.bootcss.com/froala-editor/2.7.6/js/froala_editor.pkgd.min.js\"></script>"
            };

            scripts.Add("");
            yield return new WebResource
            {
                Body = new HtmlString(string.Join(string.Empty, scripts))
            };
        }
    }
}

using Kooboo.CMS.Sites.Extension.UI.WebResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Sites.Extension.UI;
using System.Web.Routing;
using Kooboo.CMS.Common.Runtime.Dependency;
using System.Web;

namespace Kooboo.CMS.Toolkit.Controls.RichTextEditors.UI
{
    [Dependency(typeof(IWebResourceProvider), Key = "UEditor", Order = 10)]
    public class UEditorWebResource : IWebResourceProvider
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
            return Enumerable.Empty<WebResource>();
        }

        public IEnumerable<WebResource> GetStyles(RequestContext requestContext)
        {
            var scripts = new string[] { "ueditor.config.js", "ueditor.all.js" }
            .Select(it => $"<script src=\"/Scripts/ueditor/{it}\"></script>")
            .ToList();
            scripts.Add("");
            yield return new WebResource
            {
                Body = new HtmlString(string.Join(string.Empty, scripts))
            };
        }
    }
}

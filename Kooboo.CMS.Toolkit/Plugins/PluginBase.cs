using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;

using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.View;

namespace Kooboo.CMS.Toolkit.Plugins
{
    public abstract class PluginBase : IPagePlugin
    {
        private string _description;
        public virtual string Description
        {
            get
            {
                if (String.IsNullOrEmpty(_description))
                {
                    _description = this.GetType().Name;
                }

                return _description;
            }
        }

        public Page_Context PageContext
        {
            get;
            private set;
        }

        public PagePositionContext PagePositionContext
        {
            get;
            private set;
        }

        public HttpContextBase Context
        {
            get;
            private set;
        }

        public HttpRequestBase Request
        {
            get;
            private set;
        }

        public HttpResponseBase Response
        {
            get;
            private set;
        }

        private ViewDataDictionary _viewData;
        public ViewDataDictionary ViewData
        {
            get
            {
                if (_viewData == null)
                {
                    _viewData = PageContext.ControllerContext.Controller.ViewData;
                }

                return _viewData;
            }
        }

        public bool IsPost
        {
            get
            {
                return Request.HttpMethod.Equals("post", StringComparison.OrdinalIgnoreCase);
            }
        }

        private NameValueCollection _form;
        public NameValueCollection Form
        {
            get
            {
                if (_form == null)
                {
                    _form = Request.Form;
                }

                return _form;
            }
        }

        public virtual ActionResult Execute(Page_Context pageContext, PagePositionContext positionContext)
        {
            PageContext = pageContext;
            PagePositionContext = positionContext;
            Context = pageContext.ControllerContext.HttpContext;
            Request = Context.Request;
            Response = Context.Response;

            return Execute();
        }

        public abstract ActionResult Execute();
    }
}
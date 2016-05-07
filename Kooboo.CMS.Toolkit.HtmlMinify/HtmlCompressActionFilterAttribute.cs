using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using Kooboo.CMS.Sites.Web;

namespace Kooboo.CMS.Toolkit.HtmlMinify
{
    public class HtmlCompressOutputStringWriter : OutputTextWriterWrapper
    {
        string _compressedHtml;
        public HtmlCompressOutputStringWriter(TextWriter httpOutputWriter)
            : base(httpOutputWriter)
        {

        }
        public override string ToString()
        {
            if (string.IsNullOrEmpty(_compressedHtml))
            {
                var html = base.ToString();
                var compressor = new SimpleHtmlCompressor();
                _compressedHtml = compressor.Compress(html);
            }
            return _compressedHtml;
        }
    }
    public class HtmlCompressActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (filterContext.ActionDescriptor.ActionName.ToLower() == "entry"
                && filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower() == "page"
                && filterContext.HttpContext.Request.HttpMethod.ToUpper() == "GET")
            {
                filterContext.HttpContext.Response.Output = new HtmlCompressOutputStringWriter(filterContext.HttpContext.Response.Output);
            }
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            if (filterContext.HttpContext.Response.Output is HtmlCompressOutputStringWriter)
            {
                ((HtmlCompressOutputStringWriter)filterContext.HttpContext.Response.Output).Render(filterContext.HttpContext.Response);
            }
        }
    }
}

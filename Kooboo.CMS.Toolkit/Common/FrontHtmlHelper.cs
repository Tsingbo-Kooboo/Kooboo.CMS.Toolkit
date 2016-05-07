using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Kooboo.CMS.Sites.View;

namespace Kooboo.CMS.Toolkit
{
    /// <summary>
    /// Enable user to use HtmlHelper in controller.
    /// For example: Render a view in the controller.
    /// </summary>
    public static class FrontHtmlHelper
    {
        private class DummyView : IView, IViewDataContainer
        {
            public void Render(ViewContext viewContext, System.IO.TextWriter writer)
            { }

            public ViewDataDictionary ViewData
            {
                get;
                set;
            }
        }

        public static HtmlHelper HtmlHelper(this ControllerContext controllerContext)
        {
            var dummyView = new DummyView();
            var viewContext = new ViewContext(controllerContext, dummyView, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, controllerContext.HttpContext.Response.Output);
            var htmlHelper = new HtmlHelper(viewContext, dummyView);
            return htmlHelper;
        }

        public static string RenderView(this ControllerContext controllerContext, string viewName, ViewDataDictionary viewData)
        {
            return controllerContext.HtmlHelper().FrontHtml().RenderView(viewName, viewData).ToString();
        }

        public static string RenderView(this ControllerContext controllerContext, string viewName, object model)
        {
            return controllerContext.HtmlHelper().FrontHtml().RenderView(viewName, model).ToString();
        }

        public static string RenderView(this ControllerContext controllerContext, string viewName, ViewDataDictionary viewData, object parameters)
        {
            return controllerContext.HtmlHelper().FrontHtml().RenderView(viewName, viewData, parameters).ToString();
        }

        public static string RenderView(this ControllerContext controllerContext, string viewName, ViewDataDictionary viewData, object parameters, bool executeDataRule)
        {
            return controllerContext.HtmlHelper().FrontHtml().RenderView(viewName, viewData, parameters, executeDataRule).ToString();
        }
    }
}
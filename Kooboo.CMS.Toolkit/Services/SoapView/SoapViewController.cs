using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;

namespace Kooboo.CMS.Toolkit.Services.SoapView
{
    public class SoapViewController : Controller
    {
        /// <summary>
        /// Url: /_SoapView/SoapView/View
        /// </summary>
        /// <returns></returns>
        public new ActionResult View()
        {
            StringBuilder sb = new StringBuilder();

            // Service requests
            sb.AppendLine("<p><b>Service requests:<b></p>");
            List<string> serviceRequests = Session["ServiceRequests"] as List<string>;
            if(serviceRequests != null)
            {
                foreach(var serviceRequest in serviceRequests)
                {
                    sb.AppendFormat("<p><pre>{0}</pre></p><br/><br/><br/>", serviceRequest.HtmlEncode());
                }
            }

            sb.Append("<hr />");

            // Service responses
            sb.AppendLine("<p><b>Service responses:<b></p>");
            List<string> serviceResponses = Session["ServiceResponses"] as List<string>;
            if(serviceResponses != null)
            {
                foreach(var serviceResponse in serviceResponses)
                {
                    sb.AppendFormat("<p><pre>{0}</pre></p><br/><br/><br/>", serviceResponse.HtmlEncode());
                }
            }

            Session.Remove("ServiceRequests");
            Session.Remove("ServiceResponses");
            return Content(sb.ToString());
        }
    }
}
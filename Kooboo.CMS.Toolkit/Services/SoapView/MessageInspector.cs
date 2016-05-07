using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Kooboo.CMS.Toolkit.Services.SoapView
{
    public class MessageInspector : IClientMessageInspector
    {
        #region IClientMessageInspector Members

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            List<string> serviceRequests = HttpContext.Current.Session["ServiceRequests"] as List<string>;
            if(serviceRequests == null)
            {
                serviceRequests = new List<string>();
            }

            string remoteAddress = channel.RemoteAddress.Uri.ToString();
            string content = request.ToString();

            serviceRequests.Add(remoteAddress);
            serviceRequests.Add(content);
            HttpContext.Current.Session["ServiceRequests"] = serviceRequests;

            LogRequest(remoteAddress, content);
            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            List<string> serviceResponses = HttpContext.Current.Session["ServiceResponses"] as List<string>;
            if(serviceResponses == null)
            {
                serviceResponses = new List<string>();
            }

            string content = reply.ToString();

            serviceResponses.Add(content);
            HttpContext.Current.Session["ServiceResponses"] = serviceResponses;

            LogResponse(content);
        }

        #endregion

        #region Log

        private void LogRequest(string remoteAddress, string content)
        {
            Log("Request", remoteAddress, content);
        }

        private void LogResponse(string content)
        {
            Log("Response", String.Empty, content);
        }

        private void Log(string type, string remoteAddress, string content)
        {
            try
            {
                string logFilePath = String.Format("{0}\\SoapView\\{1}\\{2}({3}).txt",
                    AppDomain.CurrentDomain.BaseDirectory,
                    DateTime.Now.ToString("yyyy-MM"),
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    type);

                string logFolderPath = Path.GetDirectoryName(logFilePath);
                if(!Directory.Exists(logFolderPath))
                {
                    Directory.CreateDirectory(logFolderPath);
                }

                using(StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    sw.WriteLine("DateTime: {0}", DateTime.Now.ToString());
                    if(!String.IsNullOrEmpty(remoteAddress))
                    {
                        sw.WriteLine("RemoteAddress: {0}", remoteAddress);
                    }

                    sw.WriteLine(content);
                    sw.WriteLine("-------------------------");
                    sw.WriteLine();
                    sw.WriteLine();
                }
            }
            catch { }
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Kooboo.Web.Url;
namespace Kooboo.CMS.Toolkit
{
    public class WebRequestUtility
    {
        /// <summary>
        /// Request web page by GET method
        /// </summary>
        /// <param name="url">base url</param>
        /// <param name="parameters">parameters list</param>
        /// <returns></returns>
        public static string Get(string url, Dictionary<string, string> parameters)
        {
            url = parameters.Aggregate(url, (current, param) => current.AddQueryParam(param.Key, param.Value));
            return Get(url);
        }

        /// <summary>
        /// Request web page by GET method
        /// </summary>
        /// <param name="url">url</param>
        /// <returns></returns>
        public static string Get(string url)
        {
            string strResult;
            try
            {
                WebClient wc = new WebClient();

                wc.Encoding = Encoding.UTF8;
                wc.UseDefaultCredentials = true;
                var result = wc.DownloadString(url);
                return result;
            }
            catch (Exception ee)
            {
                strResult = ee.Message;
            }
            return strResult;
        }

        /// <summary>
        /// Request web page by GET method
        /// </summary>
        /// <param name="url">base url</param>
        /// <param name="postData">parameters,eg:id=123&name=admin</param>
        /// <param name="encodeType">page encodeType</param>
        /// <returns></returns>
        public static string Post(string url, string postData, string encodeType)
        {
            string strResult = null;
            try
            {
                Encoding encoding = Encoding.GetEncoding(encodeType);
                byte[] POST = encoding.GetBytes(postData);
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentLength = POST.Length;
                Stream newStream = myRequest.GetRequestStream();
                newStream.Write(POST, 0, POST.Length); //设置POST
                newStream.Close();
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.Default);
                strResult = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                strResult = ex.Message;
            }
            return strResult;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;

namespace Kooboo.CMS.Toolkit
{
    public static class HttpRequestExtensions
    {
        public static string ToAbsoluteUrl(this HttpRequest request)
        {
            return ToAbsoluteUrl(request, request.RawUrl);
        }

        public static string ToAbsoluteUrl(this HttpRequest request, string relativeUrl)
        {
            return ToAbsoluteUrl(request, relativeUrl, false);
        }

        public static string ToAbsoluteUrl(this HttpRequest request, string host, string relativeUrl)
        {
            return ToAbsoluteUrl(request, host, relativeUrl, false);
        }

        public static string ToAbsoluteUrl(this HttpRequest request, string relativeUrl, bool forceSSL)
        {
            return ToAbsoluteUrl(request, request.Headers["Host"], relativeUrl, forceSSL);
        }

        public static string ToAbsoluteUrl(this HttpRequest request, string host, string relativeUrl, bool forceSSL)
        {
            relativeUrl = relativeUrl ?? string.Empty;
            if (!relativeUrl.StartsWith("http://") &&
                !relativeUrl.StartsWith("https://"))
            {
                if (!host.StartsWith("http://") &&
                    !host.StartsWith("https://"))
                {
                    string scheme = forceSSL ? request.Url.Scheme.Replace("http", "https") : request.Url.Scheme;
                    return String.Format("{0}://{1}/{2}", scheme, host, relativeUrl.TrimStart('~', '/'));
                }
                else
                {
                    return String.Format("{0}/{1}", host, relativeUrl.TrimStart('~', '/'));
                }
            }
            else if (relativeUrl.StartsWith("http://") && forceSSL)
            {
                return relativeUrl.Replace("http://", "https://");
            }

            return relativeUrl;
        }

        public static string ToAbsoluteUrl(this HttpRequestBase request)
        {
            return ToAbsoluteUrl(request, request.RawUrl);
        }

        public static string ToAbsoluteUrl(this HttpRequestBase request, string relativeUrl)
        {
            return ToAbsoluteUrl(request, relativeUrl, false);
        }

        public static string ToAbsoluteUrl(this HttpRequestBase request, string host, string relativeUrl)
        {
            return ToAbsoluteUrl(request, host, relativeUrl, false);
        }

        public static string ToAbsoluteUrl(this HttpRequestBase request, string relativeUrl, bool forceSSL)
        {
            return ToAbsoluteUrl(request, request.Headers["Host"], relativeUrl, forceSSL);
        }

        public static string ToAbsoluteUrl(this HttpRequestBase request, string host, string relativeUrl, bool forceSSL)
        {
            if (!relativeUrl.StartsWith("http://") &&
                !relativeUrl.StartsWith("https://"))
            {
                if (!host.StartsWith("http://") &&
                    !host.StartsWith("https://"))
                {
                    string scheme = forceSSL ? request.Url.Scheme.Replace("http", "https") : request.Url.Scheme;
                    return String.Format("{0}://{1}/{2}", scheme, host, relativeUrl.TrimStart('~', '/'));
                }
                else
                {
                    return String.Format("{0}/{1}", host, relativeUrl.TrimStart('~', '/'));
                }
            }
            return relativeUrl;
        }

        public static string GetUserIp(this HttpRequestBase request)
        {
            string ipList = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return request.ServerVariables["REMOTE_ADDR"];
        }

        public static string GetUserIp(this HttpRequest request)
        {
            string ipList = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return request.ServerVariables["REMOTE_ADDR"];
        }
    }
}
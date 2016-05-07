using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Web;
using System.Text.RegularExpressions;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Toolkit
{
    public static class StringExtensions
    {
        private static readonly Regex StripHTMLExpression = new Regex("<\\S[^><]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        public static string FormatWith(this string source, params object[] args)
        {
            return String.Format(source, args);
        }

        public static string UrlEncode(this string source)
        {
            return HttpUtility.UrlEncode(source);
        }

        public static string UrlDecode(this string source)
        {
            return HttpUtility.UrlDecode(source);
        }

        public static string AttributeEncode(this string source)
        {
            return HttpUtility.HtmlAttributeEncode(source);
        }

        public static string HtmlEncode(this string source)
        {
            return HttpUtility.HtmlEncode(source);
        }

        public static string HtmlDecode(this string source)
        {
            return HttpUtility.HtmlDecode(source);
        }

        public static string[] SplitRemoveEmptyEntries(this string source, char separator)
        {
            return SplitRemoveEmptyEntries(source, new char[] { separator });
        }

        public static string[] SplitRemoveEmptyEntries(this string source, char[] separator)
        {
            return source.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] SplitRemoveEmptyEntries(this string source, string separator)
        {
            return SplitRemoveEmptyEntries(source, new string[] { separator });
        }

        public static string[] SplitRemoveEmptyEntries(this string source, string[] separator)
        {
            return source.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string AsSlug(this string source)
        {
            return AsSlug(source, "-");
        }

        public static string AsSlug(this string source, string separator)
        {
            if (!String.IsNullOrEmpty(source))
            {
                return Regex.Replace(source, @"[^a-zA-Z0-9]+", it => it.Index == 0 ? "" : separator);
            }

            return String.Empty;
        }

        public static string Summary(this string source, int length)
        {
            return Summary(source, length, "...");
        }

        public static string Summary(this string text, int length, string ellipsis)
        {
            string summary = String.Empty;
            if (length > 0)
            {
                summary = text;
                if (!String.IsNullOrEmpty(summary))
                {
                    summary = StripHtml(summary);
                    if (summary.Length > length)
                    {
                        if (summary.IndexOf(' ') != -1)
                        {
                            while (summary[length] != ' ')
                            {
                                length--;
                                if (length == 0) { break; }
                            }
                        }
                        summary = summary.Substring(0, length) + ellipsis;
                    }
                }
            }
            return summary;
        }

        public static string StripHtml(this string source)
        {
            return StripHTMLExpression.Replace(source, String.Empty);
        }

        public static string ToUpperFirstLetter(this string source)
        {
            if (!String.IsNullOrEmpty(source))
            {
                char[] charArray = source.ToCharArray();
                charArray[0] = Char.ToUpper(charArray[0]);
                return new String(charArray);
            }
            return String.Empty;
        }

        /// <summary>
        /// Convert yyyyMMdd to yyyy-MM-dd
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToNormalDateFormat(this string source)
        {
            if (!String.IsNullOrEmpty(source) && source.Length == 8)
            {
                return String.Format("{0}-{1}-{2}", source.Substring(0, 4), source.Substring(4, 2), source.Substring(6, 2));
            }
            return String.Empty;
        }

        /// <summary>
        /// Convert HHmm to HH:mm
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToNormalTimeFormat(this string source)
        {
            if (!String.IsNullOrEmpty(source) && source.Length == 4)
            {
                return String.Format("{0}:{1}", source.Substring(0, 2), source.Substring(2, 2));
            }
            return String.Empty;
        }

        /// <summary>
        /// add prefix http:// or https:// for url
        /// </summary>
        /// <param name="baseUri">eg: www.kooboo.com , http://www.kooboo.com etc.</param>
        /// <param name="forceSSL"></param>
        /// <returns></returns>
        public static string FormatUrlWithProtocol(this string baseUri, bool forceSSL = false)
        {
            const string https = "https://";
            const string http = "http://";
            if (!String.IsNullOrEmpty(baseUri))
            {
                String lowerBaseUri = baseUri.ToLower();
                if (!lowerBaseUri.StartsWith(http) && !lowerBaseUri.StartsWith(https))
                {
                    baseUri = (forceSSL ? https : http) + baseUri;
                }
                else if (lowerBaseUri.StartsWith(http) && forceSSL)
                {
                    baseUri = baseUri.Replace(http, https);
                }
            }
            return baseUri;
        }

        public static MediaContent AsMediaContent(this string url)
        {
            var regex = new Regex(@"(/Cms_Data/Contents/(\w+)/Media/(.*?)/([\w-]+)\.(\w{2,4}))");
            if (regex.IsMatch(url))
            {
                try
                {
                    Match m = regex.Match(url);
                    var groups = m.Groups;
                    var imgUrl = groups[0].ToString();
                    var directoryPath = groups[3].ToString();
                    var query = new Kooboo.CMS.Content.Query.MediaContentQuery(Repository.Current, new MediaFolder(Repository.Current, directoryPath.UrlDecode()));
                    return query.First(item => System.String.Compare(item.Url, imgUrl, System.StringComparison.OrdinalIgnoreCase) == 0);
                }
                catch (Exception)
                {
                    return default(MediaContent);
                }
            }
            return default(MediaContent);
        }

        /// <summary>
        /// prepend "/dev~" for url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string WrapAjaxUrl(this string url)
        {
            if (null == Site.Current || null == url)
            {
                return url ?? "";
            }
            return Kooboo.Web.Url.UrlUtility.Combine("/" + Sites.Models.SiteHelper.PREFIX_FRONT_DEBUG_URL + Sites.Models.Site.Current.FullName, url);
        }

        public static string GetImageContentType(this string imagePath)
        {
            string contentType = String.Empty;
            if (!String.IsNullOrWhiteSpace(imagePath))
            {
                string fileExtension = Path.GetExtension(imagePath).Substring(1);
                switch (fileExtension.ToLower())
                {
                    case "gif":
                        contentType = "image/gif";
                        break;
                    default:
                        contentType = "image/jpeg";
                        break;
                }
            }

            return contentType;
        }

        #region Label Extensions

        [Obsolete]
        public static string EncodedLabel(this string source, string key, string category = "", Site site = null)
        {
            if (site == null) { site = Site.Current; }

            var labelHtmlStr = CMS.Sites.Globalization.SiteLabel.RawLabel(source, key, category, site);

            var label = labelHtmlStr.ToString();
            // replace \' ===> '
            label = label.Replace("\\'", "'");
            // parse encoded to org 
            label = label.HtmlDecode();

            return label.HtmlEncode();
        }

        static string JavascriptStringEncode(this string source, char? quotes = '\'')
        {
            var str = source;
            if (!string.IsNullOrEmpty(str))
            {
                // replace newline with whitespace
                str = str.Replace("\r", "");
                str = str.Replace("\n", " ");
                // wrap slash
                str = str.Replace("\\", "\\\\");
                // wrap quotes
                if (quotes == '\'')
                {
                    str = str.Replace("'", "\\'");
                }
                else if (quotes == '"')
                {
                    str = str.Replace("\"", "\\\"");
                }
            }
            return str;
        }

        public static IHtmlString AttrLabel(this string source, string key, string category = "", Site site = null)
        {
            var rawLabel = CMS.Sites.Globalization.SiteLabel.RawLabel(source, key, category, site ?? Site.Current);
            var labelStr = rawLabel.ToHtmlString();
            // attr encode
            labelStr = AttributeEncode(labelStr);
            // ret
            return new HtmlString(labelStr);
        }

        public static IHtmlString JsLabel(this string source, string key, string category = "", string jsQuotes = "'", Site site = null)
        {
            var rawLabel = CMS.Sites.Globalization.SiteLabel.RawLabel(source, key, category, site ?? Site.Current);
            var labelStr = rawLabel.ToHtmlString();
            // js encode
            char? quotes = null;
            if (!string.IsNullOrEmpty(jsQuotes)) { quotes = jsQuotes[0]; }
            labelStr = JavascriptStringEncode(labelStr, quotes);
            // ret
            return new HtmlString(labelStr);
        }

        public static IHtmlString JsAttrLabel(this string source, string key, string category = "", string jsQuotes = "'", Site site = null)
        {
            var rawLabel = CMS.Sites.Globalization.SiteLabel.RawLabel(source, key, category, site ?? Site.Current);
            var labelStr = rawLabel.ToHtmlString();
            // js encode
            char? quotes = null;
            if (!string.IsNullOrEmpty(jsQuotes)) { quotes = jsQuotes[0]; }
            labelStr = JavascriptStringEncode(labelStr, quotes);
            // attr encode
            labelStr = AttributeEncode(labelStr);
            // ret
            return new HtmlString(labelStr);
        }

        #endregion
    }
}
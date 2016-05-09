using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DotNetOpenAuth.AspNet.Clients;
using System.Net;
using Kooboo.CMS.Membership.China.GraphDatas;
using System.IO;
using Kooboo.Web.Script.Serialization;
using Kooboo.CMS.Membership.China.Extensions;
using Kooboo.Web.Url;
using System.Text.RegularExpressions;

namespace Kooboo.CMS.Membership.China.Core
{
    public sealed class DoubanClient : OAuth2Client
    {
        private const string AuthorizationEndpoint = "https://www.douban.com/service/auth2/auth";
        private const string TokenEndpoint = "https://www.douban.com/service/auth2/token";
        private const string UserInfoEndpoint = "https://api.douban.com/v2/user/~me";

        private readonly string appId;
        private readonly string appSecret;

        private const string providerName = "Douban";

        public DoubanClient(string appId, string appSecret)
            : base(providerName)
        {
            this.appId = appId;
            this.appSecret = appSecret;
        }

        protected override Uri GetServiceLoginUrl(Uri returnUrl)
        {
            var dict = new Dictionary<string, string>();
            dict["client_id"] = this.appId;
            dict["redirect_uri"] = returnUrl.AbsoluteUri;
            dict["response_type"] = "code";
            var url = dict.Aggregate(AuthorizationEndpoint, (current, param) => current.AddQueryParam(param.Key, param.Value));
            return new Uri(url);
        }

        protected override IDictionary<string, string> GetUserData(string accessToken)
        {
            var json = GetResponseText(UserInfoEndpoint, accessToken);            
            var result = JsonHelper.Deserialize<dynamic>(json);

            var dictionary = new Dictionary<string, string>();
            dictionary.AddItemIfNotEmpty("id", result["uid"] as string);
            dictionary.AddItemIfNotEmpty("username", result["name"] as string);
            dictionary.AddItemIfNotEmpty("name", result["name"] as string);
            return dictionary;
        }

        protected override string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
            var dict = new Dictionary<string, string>
            {
                {"client_id",this.appId},				
				{"client_secret",this.appSecret},				
				{"redirect_uri",returnUrl.AbsoluteUri},				
                {"grant_type","authorization_code"},
				{"code",authorizationCode}
            };
            var text = WebRequestExtensions.Post(TokenEndpoint, dict);
            var result = JsonHelper.Deserialize<dynamic>(text);
            return result["access_token"];
        }

        public string GetResponseText(string url,string accessToken)
        {
            string responseText = String.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                responseText = sr.ReadToEnd();
            }
            return responseText;
        }
    }
}

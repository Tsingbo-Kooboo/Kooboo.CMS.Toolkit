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
    public sealed class BaiduClient : OAuth2Client
    {
        private const string AuthorizationEndpoint = "https://openapi.baidu.com/oauth/2.0/authorize";
        private const string TokenEndpoint = "https://openapi.baidu.com/oauth/2.0/token";
        private const string UserInfoEndpoint = "https://openapi.baidu.com/rest/2.0/passport/users/getLoggedInUser";

        private readonly string appId;
        private readonly string appSecret;

        private const string providerName = "Baidu";

        public BaiduClient(string appId, string appSecret)
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
            var dict = new Dictionary<string, string>();
            dict["access_token"] = accessToken;
            dict["format"] = "json";
            var json = WebRequestExtensions.Get(UserInfoEndpoint, dict);
            var result = JsonHelper.Deserialize<dynamic>(json);

            var dictionary = new Dictionary<string, string>();
            dictionary.AddItemIfNotEmpty("id", result["uid"] as string);
            dictionary.AddItemIfNotEmpty("username", result["uname"] as string);
            dictionary.AddItemIfNotEmpty("name", result["uname"] as string);
            return dictionary;
        }

        protected override string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
            var dict = new Dictionary<string, string>
            {
                {"grant_type","authorization_code"},
                {"client_id",this.appId},				
				{"redirect_uri",returnUrl.AbsoluteUri},				
				{"client_secret",this.appSecret},				
				{"code",authorizationCode}
            };
            var text = WebRequestExtensions.Get(TokenEndpoint, dict);
            var result = JsonHelper.Deserialize<dynamic>(text);
            return result["access_token"];
        }
    }
}

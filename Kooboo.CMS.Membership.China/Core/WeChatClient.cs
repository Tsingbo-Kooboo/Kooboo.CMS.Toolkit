using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.AspNet.Clients;
using Kooboo.CMS.Membership.China.GraphDatas;
using Kooboo.Web.Script.Serialization;
using Kooboo.CMS.Membership.China.Extensions;
using Kooboo.Web.Url;
using System.Text.RegularExpressions;

namespace Kooboo.CMS.Membership.China.Core
{
    public sealed class WeChatClient : OAuth2Client
    {
        private const string AuthorizationEndpoint = "https://open.weixin.qq.com/connect/oauth2/authorize";
        private const string TokenEndpoint = "https://api.weixin.qq.com/sns/oauth2/access_token";
        private const string UserInfoEndpoint = "https://api.weixin.qq.com/sns/userinfo";

        private readonly string appId;
        private readonly string appSecret;

        private const string providerName = "WeChat";

        public WeChatClient(string appId, string appSecret)
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

        private string GetOpenid(string accessToken)
        {
            var result = string.Empty;
            var regex = new Regex("\"openid\":\"(.*?)\"");
            if (regex.IsMatch(accessToken))
            {
                result = regex.Match(accessToken).Groups[1].Value;
            }
            return result;
        }

        protected override IDictionary<string, string> GetUserData(string accessToken)
        {
            var openid = GetOpenid(accessToken);
            var url = (UserInfoEndpoint + "?" + accessToken)
                .AddQueryParam("openid", openid)
                .AddQueryParam("lang", "zh_CN");

            var text = WebRequestExtensions.Get(url);
            var graphData = JsonHelper.Deserialize<WeChatGraphData>(text);

            var dictionary = new Dictionary<string, string>();
            dictionary.AddItemIfNotEmpty("id", openid);
            dictionary.AddItemIfNotEmpty("username", graphData.nickname);
            dictionary.AddItemIfNotEmpty("name", graphData.nickname);
            return dictionary;
        }

        protected override string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
            var dict = new Dictionary<string, string>();
            dict["code"] = authorizationCode;
            dict["client_id"] = this.appId;
            dict["client_secret"] = this.appSecret;
            dict["redirect_uri"] = returnUrl.AbsoluteUri.UrlEncode();
            dict["grant_type"] = "authorization_code";
            var json = WebRequestExtensions.Post(TokenEndpoint, dict);
            if (!string.IsNullOrEmpty(json))
            {
                return json;
            }

            throw new HttpException(500, "Get AccessToken failed!");
        }
    }
}

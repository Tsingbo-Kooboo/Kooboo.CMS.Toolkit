using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DotNetOpenAuth.AspNet.Clients;
using System.Net;
using System.IO;
using Kooboo.Web.Script.Serialization;
using Kooboo.CMS.Membership.China.Extensions;
using Kooboo.Web.Url;

namespace Kooboo.CMS.Membership.China.Core
{
    public sealed class RenrenClient : OAuth2Client
    {
        private const string AuthorizationEndpoint = "https://graph.renren.com/oauth/authorize";
        private const string TokenEndpoint = "https://graph.renren.com/oauth/token";
        private const string UserInfoEndpoint = "https://api.renren.com/v2/user/login/get";

        private readonly string appId;
        private readonly string appSecret;

        private const string providerName = "Renren";

        private static RenrenModel RenrenModel { get; set; }

        public RenrenClient(string appId, string appSecret)
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
            var dict = new Dictionary<string, string> { { "access_token", accessToken } };
            dynamic graphData;
            var text = WebRequestExtensions.Get(UserInfoEndpoint, dict);
            graphData = JsonHelper.Deserialize<dynamic>(text);

            var dictionary = new Dictionary<string, string>();
            dictionary.AddItemIfNotEmpty("id", graphData["response"]["id"].ToString() as string);
            dictionary.AddItemIfNotEmpty("username", graphData["response"]["name"] as string);
            dictionary.AddItemIfNotEmpty("name", graphData["response"]["name"] as string);
            //dictionary.AddItemIfNotEmpty("url", graphData["response"]["avatar"][1]["url"] as string);
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
                RenrenModel = JsonHelper.Deserialize<RenrenModel>(json);
                return RenrenModel.Access_token;
            }
            throw new HttpException(500, "Get AccessToken failed!");
        }
    }

    class RenrenModel
    {
        public string Uid { get; set; }
        public int Expires_in { get; set; }
        public string Refresh_Token { get; set; }
        public string Access_token { get; set; }
    }
}

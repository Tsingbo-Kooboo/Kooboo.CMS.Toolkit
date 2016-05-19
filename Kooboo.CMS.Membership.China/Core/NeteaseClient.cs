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

namespace Kooboo.CMS.Membership.China.Core
{
    public sealed class NeteaseClient : OAuth2Client
    {
        private const string AuthorizationEndpoint = "https://api.t.163.com/oauth2/authorize";
        private const string TokenEndpoint = "https://api.t.163.com/oauth2/access_token";
        private const string UserInfoEndpoint = "https://api.t.163.com/users/show.json";

        private readonly string appId;
        private readonly string appSecret;

        private const string providerName = "Netease";

        private static NeteaseModel NeteaseModel { get; set; }

        public NeteaseClient(string appId, string appSecret)
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
            dict["id"] = NeteaseModel.Uid;
            var text = WebRequestExtensions.Get(UserInfoEndpoint, dict);
            var graphData = JsonHelper.Deserialize<NeteaseGraphData>(text);

            var dictionary = new Dictionary<string, string>();
            dictionary.AddItemIfNotEmpty("id", graphData.Id);
            dictionary.AddItemIfNotEmpty("username", graphData.Name);
            dictionary.AddItemIfNotEmpty("name", graphData.Screen_Name);
            //dictionary.AddItemIfNotEmpty("url", graphData.Url);
            //dictionary.AddItemIfNotEmpty("gender", graphData.Gender.ToString());
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
                NeteaseModel = JsonHelper.Deserialize<NeteaseModel>(json);
                return NeteaseModel.Access_token;
            }
            throw new HttpException(500, "Get AccessToken failed!");
        }
    }

    class NeteaseModel
    {
        public string Uid { get; set; }
        public int Expires_in { get; set; }
        public string Refresh_Token { get; set; }
        public string Access_token { get; set; }
    }
}

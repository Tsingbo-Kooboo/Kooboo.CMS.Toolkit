using System;
using System.Collections.Generic;
using DotNetOpenAuth.AspNet.Clients;
using Kooboo.CMS.Membership.China.Extensions;
using DotNetOpenAuth.Messaging;
using Kooboo.Web.Script.Serialization;
using Kooboo.CMS.Membership.China.GraphDatas;

namespace Kooboo.CMS.Membership.China.Core
{
    public sealed class SinaClient : OAuth2Client
    {
        private const string AuthorizationEndpoint = "https://api.weibo.com/oauth2/authorize";
        private const string TokenEndpoint = "https://api.weibo.com/oauth2/access_token";
        private const string UserData = "https://api.weibo.com/2/users/show.json";

        private readonly string appId;
        private readonly string appSecret;

        private const string providerName = "Sina";
        private string uid { get; set; }


        public SinaClient(string appId, string appSecret)
            : base(providerName)
        {
            this.appId = appId;
            this.appSecret = appSecret;
        }
        protected override Uri GetServiceLoginUrl(Uri returnUrl)
        {
            var uriBuilder = new UriBuilder(AuthorizationEndpoint);
            var dict = new Dictionary<string, string>{				
				{
					"client_id",
					this.appId
				},				
				{
					"redirect_uri",
					returnUrl.AbsoluteUri
				},				
				{
					"response_type",
					"code"
				}
            };
            foreach (var item in dict)
            {
                uriBuilder.AppendQueryArgument(item.Key, item.Value);
            }
            return uriBuilder.Uri;
        }

        protected override IDictionary<string, string> GetUserData(string accessToken)
        {
            var dict = new Dictionary<string, string>();
            dict["access_token"] = accessToken;
            dict["uid"] = uid;
            var result = WebRequestExtensions.Get(UserData, dict);
            var graphData = JsonHelper.Deserialize<SinaGraphData>(result);
            var dictionary = new Dictionary<string, string>();
            dictionary.AddItemIfNotEmpty("id", graphData.Id);
            dictionary.AddItemIfNotEmpty("username", graphData.Screen_Name);
            dictionary.AddItemIfNotEmpty("name", graphData.Name);
            //foreach (var item in res)
            //{
            //    dictionary.AddItemIfNotEmpty(item.Key as string, item.Value.ToString() as string);
            //}

            return dictionary;
        }

        protected override string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
            var dict = (new Dictionary<string, string>
			{
				
				{
					"client_id",
					this.appId
				},
				
				{
					"redirect_uri",
					returnUrl.AbsoluteUri.UrlEncode()
				},
				
				{
					"client_secret",
					this.appSecret
				},
				
				{
					"code",
					authorizationCode
				},
				
				{
					"scope",
					"email"
				}
			});
            var text = WebRequestExtensions.Post(TokenEndpoint, dict);
            var result = JsonHelper.Deserialize<dynamic>(text);
            uid = result["uid"].ToString();
            return result["access_token"];
        }
    }
}

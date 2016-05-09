using Kooboo.CMS.Membership.OAuthClients;

namespace Kooboo.CMS.Membership.China.Clients
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IAuthClient), Key = "sina")]
    public class SinaClient : OpenAuthClient, IAuthClient
    {
        public override DotNetOpenAuth.AspNet.IAuthenticationClient GetOpenAuthClient()
        {
            System.Diagnostics.Contracts.Contract.Requires(MembershipConnect != null);

            return new Kooboo.CMS.Membership.China.Core.SinaClient(MembershipConnect.AppId, MembershipConnect.AppSecret);
        }

        public override string ProviderName
        {
            get
            {
                return "Sina";
            }
        }

        public override bool RequiresAppId
        {
            get
            {
                return true;
            }
        }
    }


}

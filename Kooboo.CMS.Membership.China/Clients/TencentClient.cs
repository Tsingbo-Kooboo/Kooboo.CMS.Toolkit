using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Membership.OAuthClients;

namespace Kooboo.CMS.Membership.China.Clients
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IAuthClient), Key = "tencent")]
    public class TencentClient : OpenAuthClient, IAuthClient
    {
        public override DotNetOpenAuth.AspNet.IAuthenticationClient GetOpenAuthClient()
        {
            System.Diagnostics.Contracts.Contract.Requires(MembershipConnect != null);

            return new Kooboo.CMS.Membership.China.Core.TencentClient(MembershipConnect.AppId, MembershipConnect.AppSecret);
        }

        public override string ProviderName
        {
            get
            {
                return "Tencent";
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

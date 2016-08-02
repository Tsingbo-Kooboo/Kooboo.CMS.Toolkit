using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Form;
using Kooboo.CMS.Form.Html;
using Kooboo.CMS.Common.Runtime;

namespace Kooboo.CMS.Toolkit.Controls
{
    public class MembersSelector : Form.Html.Controls.DropDownList
    {
        public override string Name
        {
            get
            {
                return "MembersSelector";
            }
        }

        public override bool HasDataSource
        {
            get
            {
                return true;
            }
        }

        protected override string RenderInput(IColumn column)
        {
            var inputBuilder = new StringBuilder();
            inputBuilder.AppendFormat(@"
            @{{ var dropDownDefault_{0} =  @""{1}"";
                var membership{0} = Site.Current.GetMembership();
                var manager{0} = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<Kooboo.CMS.Membership.Services.MembershipUserManager>();
                var query_{0} = manager{0}.All(membership{0}, """");
                string {0}Value = Model.{0};
                var list{0} = query_{0}.ToArray().Select(it => new System.Web.Mvc.SelectListItem
                {{
                    Value = it.UUID,
                    Text = it.UserName,
                    Selected = {0}Value != null && {0}Value.ToString().Split(',').Contains(it.UUID,StringComparer.OrdinalIgnoreCase)
                }});
            }}
            @Html.DropDownList(""{0}"", list{0}, new {{ @class = ""long select2"",multiple=""multiple"" }})
            <script>
                $(function () {{
                    $(""#{0}"").select2();
                }});
            </script>
", column.Name, column.DefaultValue.EscapeQuote());
            return inputBuilder.ToString();
        }
    }
}

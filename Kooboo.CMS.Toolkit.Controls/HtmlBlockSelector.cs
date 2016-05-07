using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Form;
using Kooboo.CMS.Form.Html;

namespace Kooboo.CMS.Toolkit.Controls
{
    public class HtmlBlockSelector : Form.Html.Controls.DropDownList
    {
        public override string Name
        {
            get
            {
                return "HtmlBlockSelector";
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
            StringBuilder sb = new StringBuilder(string.Format(@"@{{ var dropDownDefault_{0} =  @""{1}"";}}
                <select name=""{0}"" class=""long"">", column.Name, column.DefaultValue.EscapeQuote()));
            string emptyOption = "";
            if (column.AllowNull)
            {
                emptyOption = @"<option value=""""></option>";
            }
            sb.AppendFormat(@"
            @{{
                var query_{0} = ServiceFactory.HtmlBlockManager.All(Site.Current,"""");
            }}
            {1}
            @foreach (var item in query_{0})
            {{                            
                <option value=""@item.UUID"" @((Model.{0} != null && Model.{0}.ToString().ToLower() == @item.UUID.ToLower()) || (Model.{0} == null && dropDownDefault_{0}.ToLower() == @item.UUID.ToLower()) ? ""selected"" : """")>@item.Name</option>
            }}
            ", column.Name,  emptyOption);
            sb.Append("</select>");

            return sb.ToString();
        }
    }
}

using Kooboo.CMS.Form.Html.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Form;

namespace Kooboo.CMS.Toolkit.Controls.RichTextEditors
{
    public class UEditor : ControlBase
    {
        public override string Name { get; } = "UEditor";

        protected override string RenderInput(IColumn column)
        {
            return $@"<div class=""extra-large""><script name=""{column.Name}"" id=""{column.Name}"" class=""{0} ueditor"" 
media_library_url=""@Url.Action(""Selection"",""MediaContent"",ViewContext.RequestContext.AllRouteValues()))"" 
media_library_title =""@(""Selected Files"".Localize())"" type=""text/plain"">@( Model.{column.Name} ?? """")</script></div>
<script>UE.getEditor(""{column.Name}"");</script>
";
        }
    }
}

using Kooboo.CMS.Form.Html.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Form;
using Kooboo.Web.Script.Serialization;
using System.Web;

namespace Kooboo.CMS.Toolkit.Controls.RichTextEditors
{
    public class UEditor : ControlBase
    {
        public override string Name { get; } = "UEditor";

        protected override string RenderInput(IColumn column)
        {
            var columnSettings = column.CustomSettings ?? new Dictionary<string, string>();
            var settingArray = columnSettings.Select(it => $"{it.Key}:{it.Value}");
            return $@"<div class=""extra-large""><script name=""{column.Name}"" id=""{column.Name}"" class=""{0} ueditor"" 
 type=""text/plain"">@Html.Raw(Model.{column.Name} ?? """")</script></div>
<script>UE.getEditor(""{column.Name}"",{{{string.Join(",", settingArray.ToArray()).Replace(" ", "")}}}).addListener(""contentChange"", function () {{$.publish(kooboo.constants.messageToptics.SomeThing_Changed_On_Page);}});</script>
";
        }
    }
}

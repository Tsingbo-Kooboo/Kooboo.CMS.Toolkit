using Kooboo.CMS.Form;
using Kooboo.CMS.Form.Html.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Toolkit.Controls.FroalaEditor
{
    public class FroalaEditorControl : ControlBase
    {
        public override string Name => "FroalaEditor";

        protected override string RenderInput(IColumn column)
        {
            var settings = "";
            if (column.CustomSettings != null && column.CustomSettings.Count > 0)
            {
                var settingArray = column.CustomSettings.Select(it => $"{it.Key}=\"{it.Value}\"");
                settings = string.Join("\r\n", settingArray.ToArray());
            }
            return $@"<div class=""extra-large kb-froala-editor"">
 <kb-froala-editor 
    name=""{column.Name}"" 
    id=""{column.Name}"" 
    class=""{column.Name} froala-editor-column"" 
    {settings}></kb-froala-editor>
</div>";
        }
    }
}

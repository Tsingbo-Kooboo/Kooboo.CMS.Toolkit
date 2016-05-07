using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Form.Html.Controls;
using Kooboo.CMS.Form.Html;
using Kooboo.CMS.Form;
namespace Kooboo.CMS.Toolkit.Controls
{
    /// <summary>
    /// http://www.prodiven.com/jcombo/
    /// Usage:
    /// 1. Add a setting name "Parent" indicate using which parent field in the CustomSetting.
    /// 2. Add a setting name "Folder" indicate using which folder as data source in the CustomSetting.
    /// 3. The DefaultValue as "Default message to select an option. if you set an empty value then does not shows any initial text.".
    /// </summary>
    public class CascadingDropdown : ControlBase
    {
        public override string Name
        {
            get { return "CascadingDropdown"; }
        }

        public override string Render(ISchema schema, IColumn column)
        {
            string html = string.Format(EditorTemplate, column.Name,
                (string.IsNullOrEmpty(column.Label) ? column.Name : column.Label).RazorHtmlEncode(), RenderInput(schema, column),
                string.IsNullOrEmpty(column.Tooltip) ? "" : string.Format(@"<a href=""javascript:;"" class=""tooltip-link"" title='{0}'></a>", (column.Tooltip).RazorHtmlEncode()));

            return html;
        }

        protected string RenderInput(ISchema schema, IColumn column)
        {
            var sb = new StringBuilder();
            sb.Append(@"@if ((bool?)ViewContext.Controller.ViewData[""JComboResourceUrl.Rendered""] != true)
            {
                ViewContext.Controller.ViewData[""JComboResourceUrl.Rendered""] = true;");
            sb.Append("\n\t\t\t\t<script src=\"@Kooboo.CMS.Toolkit.Controls.ControlsScript.GetJComboResourceUrl()\" type=\"text/javascript\" ></script>\n");
            sb.Append("\t\t\t}");
            var id = column.Name;
            var parent = "";
            if (column.CustomSettings != null && column.CustomSettings.ContainsKey("Parent"))
            {
                parent = column.CustomSettings["Parent"];
            }
            var folder = "";
            if (column.CustomSettings != null && column.CustomSettings.ContainsKey("Folder"))
            {
                folder = column.CustomSettings["Folder"];
            }
            var parentColumn = schema.Columns.FirstOrDefault(it => it.Name.EqualsOrNullEmpty(parent, StringComparison.OrdinalIgnoreCase));
            string script = "";
            if (parentColumn != null)
            {
                var parentFolder = "";
                if (parentColumn.CustomSettings != null && parentColumn.CustomSettings.ContainsKey("Folder"))
                {
                    parentFolder = parentColumn.CustomSettings["Folder"];
                }
                script = string.Format(@"$(""#{0}"").jCombo(""@Html.Raw(Url.Action(""Index"",""Cascading"",new {{repositoryName = Request.RequestContext.AllRouteValues()[""repositoryName""],Area=""ToolkitControls"",folder=""{1}"",parentFolder=""{2}""}}))&parentUUID="",{{dataType:""json"",parent:""#{3}"",selected_value:""@Model.{0}"",parent_value:""@Model.{3}"",initial_text:""{4}""}});"
                    , id, folder, parentFolder, parent, column.DefaultValue);
            }
            else
            {
                script = string.Format(@"$(""#{0}"").jCombo(""@Html.Raw(Url.Action(""Index"",""Cascading"",new {{repositoryName = Request.RequestContext.AllRouteValues()[""repositoryName""],Area=""ToolkitControls"",folder=""{1}""}}))"",{{dataType:""json"",selected_value:""@Model.{0}"",initial_text:""{2}""}});", id, folder, column.DefaultValue);
            }
            sb.AppendFormat(@"
            <select name=""{0}"" id=""{1}""></select>
                <script type=""text/javascript"">
                    $(function(){{
                        {2}
                    }});
                </script>"
                 , column.Name, id, script);

            return sb.ToString();
        }

        protected override string RenderInput(IColumn column)
        {
            return "";
        }
    }
}

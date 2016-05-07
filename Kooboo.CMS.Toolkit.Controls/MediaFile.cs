using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Form;
using Kooboo.CMS.Form.Html;
using Kooboo.Globalization;

namespace Kooboo.CMS.Toolkit.Controls
{
    public class MediaFile : Kooboo.CMS.Form.Html.Controls.Input
    {
        public override string Type
        {
            get { return "hidden"; }
        }

        private string _name;
        public override string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    _name = this.GetType().Name;
                }

                return _name;
            }
        }

        /// <summary>
        /// false
        /// </summary>
        protected virtual bool AllowMultipleFiles
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// /^.+\..+$/
        /// </summary>
        protected virtual string Validation
        {
            get
            {
                return @"/^.+\..+$/";
            }
        }

        /// <summary>
        /// Please select a valid file
        /// </summary>
        protected virtual string ValidationErrorMessage
        {
            get
            {
                return "Please select a valid file";
            }
        }
        protected override string RenderInput(Kooboo.CMS.Form.IColumn column)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("<div id='{0}_Container' class='mediafile'>", column.Name);
            var input = @"<input id=""{0}"" name=""{0}"" type=""{1}"" value=""@(Model.{0} ?? """")"" data-bind=""value: fieldValue""/>";
            const string script = "<script src=\"@Kooboo.CMS.Toolkit.Controls.ControlsScript.GetWebResourceUrl()\" type=\"text/javascript\" ></script>";
            string options = @"{
                allowMultipleFiles: " + AllowMultipleFiles.ToString().ToLower() + @",
                validation: " + Validation + @",
                validationErrorMessage: '" + ValidationErrorMessage + @"',
                value: '@(Model." + column.Name + @")'
                }";
            var ul = @"
            <ul class=""clearfix"" data-bind=""foreach: data"">
                <!-- ko if: isImage -->
                <li class=""img"">
                    <span class=""preview""></span>
                    <a class=""action"" data-bind=""click: $parent.removeItem"">@Html.IconImage(""minus small"")</a>
                    <img data-bind=""attr:{src:Url}"" height=""100"" width=""100"">
                </li>
                <!-- /ko -->
                <!-- ko ifnot: isImage -->
                <li>
                    <span class=""left"" data-bind=""{text:FileName}""></span>
                    <a class=""action right"" data-bind=""click: $parent.removeItem"">@Html.IconImage(""minus small"")</a>
                </li>
                <!-- /ko -->
            </ul>";

            var mediaLibraryUrl = String.Format(@"@Url.Action(""Selection"", ViewContext.RequestContext.AllRouteValues().Merge(""Controller"", ""MediaContent"").Merge(""FolderName"", null){0})""", AllowMultipleFiles ? "" : @".Merge(""SingleChoice"", ""true"")");

            var func = String.Format(@"
                {0}
                <a id='addMedia_{1}' columnName='{1}' href='{2}' class='action add'>@Html.IconImage(""plus small"")</a>
                <script type='text/javascript'>
                    $('#{1}_Container').mediaControl({3});
                </script>
            ", script, column.Name, mediaLibraryUrl, options);
            sb.AppendFormat(input, column.Name, Type, ValidationExtensions.GetUnobtrusiveValidationAttributeString(column));
            sb.Append(ul);
            sb.Append(func);
            sb.Append("</div>");
            return sb.ToString();
        }
    }
}
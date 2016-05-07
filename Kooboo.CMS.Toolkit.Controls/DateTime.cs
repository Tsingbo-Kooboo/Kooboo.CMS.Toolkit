using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Form;
using Kooboo.CMS.Form.Html;
using Kooboo.Globalization;

namespace Kooboo.CMS.Toolkit.Controls
{
    public class DateTime : Kooboo.CMS.Form.Html.Controls.Input
    {
        public override string Type
        {
            get { return "text"; }
        }

        public override string DataType
        {
            get
            {
                return "DateTime";
            }
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
                return @"/^\d{2}/\d{2}/\d{4}\s\d{2}\d{2}$/";
            }
        }

        /// <summary>
        /// Please select a valid file
        /// </summary>
        protected virtual string ValidationErrorMessage
        {
            get
            {
                return "The datetime format is incorrect";
            }
        }

        protected override string RenderInput(Kooboo.CMS.Form.IColumn column)
        {
            var sb = new StringBuilder();
            var input = string.Format("<input id=\"{0}\" name=\"{0}\"{3} type=\"{1}\" value=\"@(Model.{0} ==null ? \"\" : Model.{0}.ToLocalTime().ToString())\" {2}/>", column.Name, this.Type,
                ValidationExtensions.GetUnobtrusiveValidationAttributeString(column), column.AllowNull ? "" : " readonly=\"readonly\"");
           
            sb.Append(@"@if ((bool?)ViewContext.Controller.ViewData[""WebResourceUrl.Rendered""] != true)
            {
                ViewContext.Controller.ViewData[""WebResourceUrl.Rendered""] = true;");
            const string script = @"
                <script src=""@Kooboo.CMS.Toolkit.Controls.ControlsScript.GetWebResourceUrl()"" type=""text/javascript"" ></script>";
            const string css = @"
                <link href=""@Kooboo.CMS.Toolkit.Controls.ControlsScript.GetDatetimeResourceUrl()"" type=""text/css"" rel=""stylesheet"" />";
            sb.Append(css);
            sb.Append(script);
            sb.Append("\t\t\t}");
            var func = String.Format(@"
                <script type='text/javascript'>
                    $(function() {{
                        $('input[name=""{0}""]').datetimepicker({{
                            showSecond: true,
                            timeFormat: 'HH:mm:ss'
                        }});
                    }});
                </script>", column.Name);
            sb.Append(func);
            sb.Append(input);
            return sb.ToString();
        }
    }
}
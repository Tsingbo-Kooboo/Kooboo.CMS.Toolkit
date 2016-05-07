#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Kooboo.CMS.Toolkit
{
    public static class CheckboxListExtensions
    {
        public static IHtmlString CheckboxList(this HtmlHelper helper, string name, IEnumerable<string> items)
        {
            var selectList = new SelectList(items);
            return helper.CheckboxList(name, selectList);
        }

        public static IHtmlString CheckboxList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items)
        {
            TagBuilder ulTag = new TagBuilder("ul");
            ulTag.AddCssClass("checkbox-list");

            foreach (var item in items)
            {
                var liTag = new TagBuilder("li");
                var ckValue = item.Value ?? item.Text;
                var ckId = name + "_" + ckValue;
                var checkboxTag = helper.CheckBox(name, item.Selected, new { value = ckValue, id = ckId }); //helper.RadioButton(name, rbValue, item.Selected, new { id = rbId });

                var labelTag = new TagBuilder("label");
                labelTag.MergeAttribute("for", ckId);
                labelTag.MergeAttribute("class", "inline");
                labelTag.InnerHtml = item.Text ?? item.Value;

                liTag.InnerHtml = checkboxTag.ToString() + labelTag.ToString();

                ulTag.InnerHtml += liTag.ToString();
            }

            return new HtmlString(ulTag.ToString());
        }
    }
}

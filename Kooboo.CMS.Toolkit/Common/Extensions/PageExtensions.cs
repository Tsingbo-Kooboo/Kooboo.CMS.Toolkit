using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Toolkit
{
    public static class PageExtensions
    {
        public static Page Copy(this Page page)
        {
            //var tempPage = CloneUtility.DeepClone(page);
            var tempPage = new Page();
            tempPage.Name = page.Name;
            tempPage.FullName = page.FullName;
            tempPage.ContentTitle = page.ContentTitle;

            tempPage.Layout = page.Layout;
            tempPage.PagePositions = page.PagePositions;
            tempPage.EnableTheming = page.EnableTheming;
            tempPage.EnableScript = page.EnableScript;
            tempPage.Searchable = page.Searchable;
            tempPage.OutputCache = page.OutputCache;
            tempPage.PageType = page.PageType;
            tempPage.IsDefault = page.IsDefault;
            tempPage.IsDummy = page.IsDummy;

            tempPage.Navigation = new Navigation();
            tempPage.Navigation.Show = page.Navigation.Show;
            tempPage.Navigation.DisplayText = page.Navigation.DisplayText;
            tempPage.Navigation.Order = page.Navigation.Order;
            tempPage.Navigation.ShowInCrumb = page.Navigation.ShowInCrumb;

            tempPage.HtmlMeta = page.HtmlMeta;
            //tempPage.Route = page.Route;
            tempPage.Route = new PageRoute();
            tempPage.Route.ExternalUrl = page.Route.ExternalUrl;
            tempPage.Route.Identifier = page.Route.Identifier;
            tempPage.Route.RoutePath = page.Route.RoutePath;
            tempPage.Route.Defaults = page.Route.Defaults;

            tempPage.DataRules = page.DataRules;
            tempPage.Plugins = page.Plugins;
            //tempPage.CustomFields = page.CustomFields;
            tempPage.CustomFields = new Dictionary<string, string>();
            if(page.CustomFields != null)
            {
                foreach(var customField in page.CustomFields)
                {
                    tempPage.CustomFields.Add(customField.Key, customField.Value);
                }
            }

            tempPage.Published = page.Published;
            tempPage.Site = page.Site;
            tempPage.Parent = page.Parent;
            tempPage.Permission = page.Permission;
            tempPage.UserName = page.UserName;

            return tempPage;
        }
    }
}
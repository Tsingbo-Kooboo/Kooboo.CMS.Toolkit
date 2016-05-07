using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;

namespace Kooboo.CMS.Toolkit.Models
{
    public class Example : TextContent
    {
        public Example()
            : base(new TextContent())
        { }

        public Example(TextContent textContent)
            : base(textContent)
        { }

        public class FieldNames
        {
            public const string Title = "Title";
            public const string Content = "Content";
        }

        public string Title
        {
            get
            {
                return this.GetString(FieldNames.Title);
            }
            set
            {
                this[FieldNames.Title] = value;
            }
        }

        public string Content
        {
            get
            {
                return this.GetString(FieldNames.Content);
            }
            set
            {
                this[FieldNames.Content] = value;
            }
        }

        private IEnumerable<ExampleCategory> _exampleCategories;
        public IEnumerable<ExampleCategory> ExampleCategories
        {
            get
            {
                if(_exampleCategories == null)
                {
                    _exampleCategories = this.Categories(FolderNames.ExampleCategory)
                        .Published()
                        .DefaultOrder()
                        .MapTo<ExampleCategory>();
                }

                return _exampleCategories;
            }
        }
    }
}
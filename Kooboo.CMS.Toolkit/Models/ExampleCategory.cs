using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;

namespace Kooboo.CMS.Toolkit.Models
{
    public class ExampleCategory : TextContent
    {
        public ExampleCategory()
            : base(new TextContent())
        { }

        public ExampleCategory(TextContent textContent)
            : base(textContent)
        { }

        public class FieldNames
        {
            public const string Name = "Name";
        }

        public string Name
        {
            get
            {
                return this.GetString(FieldNames.Name);
            }
            set
            {
                this[FieldNames.Name] = value;
            }
        }
    }
}
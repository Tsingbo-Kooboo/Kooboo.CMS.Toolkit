using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Toolkit.Models;

namespace Kooboo.CMS.Toolkit.Services
{
    public class ExampleCategoryService : ServiceBase<ExampleCategory>
    {
        public override ExampleCategory Get(TextContent textContent)
        {
            if(textContent != null)
            {
                return new ExampleCategory(textContent);
            }
            return null;
        }

        public ExampleCategory GetByName(string name)
        {
            return this.Get(ExampleCategory.FieldNames.Name, name);
        }
    }
}
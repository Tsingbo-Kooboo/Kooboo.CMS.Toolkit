using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Toolkit.Models;

namespace Kooboo.CMS.Toolkit.Services
{
    public class ExampleService : ServiceBase<Example>
    {
        public override Example Get(TextContent textContent)
        {
            if(textContent != null)
            {
                return new Example(textContent);
            }
            return null;
        }

        public IEnumerable<Example> GetByCategory(string categoryUUID)
        {
            return this.CreateQuery()
                .WhereCategory(FolderNames.ExampleCategory, categoryUUID)
                .DefaultOrder()
                .MapTo<Example>();
        }
    }
}
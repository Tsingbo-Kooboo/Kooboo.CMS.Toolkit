using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.Specialized;

using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Toolkit
{
    public static class NameValueCollectionExtensions
    {
        public static IDictionary<string, string> ToDictionary(this NameValueCollection collection)
        {
            return collection.AllKeys.Where(key => key != null).ToDictionary(key => key, key => collection[key]);
        }

        public static TextContent ToTextContent(this NameValueCollection collection)
        {
            IDictionary<string, object> dictionary = collection.AllKeys.Where(key => key != null).ToDictionary(key => key, key => (object)collection[key]);
            return new TextContent(dictionary);
        }
    }
}
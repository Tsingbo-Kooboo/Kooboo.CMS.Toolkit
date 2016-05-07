using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Toolkit
{
    public class ContextUtility
    {
        public static T Get<T>(string key)
        {
            return CallContext.Current.GetObject<T>(key);
        }

        public static void Set<T>(string key, T value)
        {
            CallContext.Current.RegisterObject<T>(key, value);
        }

        public static T GetOrAdd<T>(string key, Func<T> creator)
        {
            T value = Get<T>(key);
            if (value == null)
            {
                value = creator();
                Set(key, value);
            }
            return value;
        }
    }
}
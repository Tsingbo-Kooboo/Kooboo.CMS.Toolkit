using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Caching;

using Kooboo.CMS.Caching;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Caching;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Caching;

namespace Kooboo.CMS.Toolkit
{
    /// <summary>
    /// GlobalObjectCache utility
    /// </summary>
    public class CacheUtility
    {
        private static CacheEntryRemovedCallback CacheExpiredNotify = (arguments) =>
        {
            if(arguments.RemovedReason == CacheEntryRemovedReason.Removed)
            {
                CacheExpiredNotification.Notify(arguments.Source.Name, arguments.CacheItem.Key);
            }
        };

        private static CacheItemPolicy DefaultCacheItemPolicy = new CacheItemPolicy()
        {
            SlidingExpiration = TimeSpan.Parse("00:30:00"),
            RemovedCallback = CacheExpiredNotify
        };

        public static object Get(string key)
        {
            return CacheManagerFactory.DefaultCacheManager.GlobalObjectCache().Get(key);
        }

        public static T Get<T>(string key)
        {
            var value = Get(key);
            return value != null ? (T)value : default(T);
        }

        public static bool Add(string key, object value)
        {
            return CacheManagerFactory.DefaultCacheManager.GlobalObjectCache().Add(key, value, DefaultCacheItemPolicy);
        }

        public static bool Add(string key, object value, DateTime absoluteExpirationy)
        {
            return CacheManagerFactory.DefaultCacheManager.GlobalObjectCache().Add(key, value, new CacheItemPolicy()
            {
                AbsoluteExpiration = absoluteExpirationy,
                RemovedCallback = CacheExpiredNotify
            });
        }

        public static bool Add(string key, object value, TimeSpan slidingExpiration)
        {
            return CacheManagerFactory.DefaultCacheManager.GlobalObjectCache().Add(key, value, new CacheItemPolicy()
            {
                SlidingExpiration = slidingExpiration,
                RemovedCallback = CacheExpiredNotify
            });
        }

        public static T GetOrAdd<T>(string key, Func<T> creator)
        {
            T value = Get<T>(key);
            if(value == null)
            {
                value = creator();
                if(value != null)
                {
                    Add(key, value);
                }
            }
            return value;
        }

        public static T GetOrAdd<T>(string key, Func<T> creator, DateTime absoluteExpirationy)
        {
            T value = Get<T>(key);
            if(value == null)
            {
                value = creator();
                if(value != null)
                {
                    Add(key, value, absoluteExpirationy);
                }
            }
            return value;
        }

        public static T GetOrAdd<T>(string key, Func<T> creator, TimeSpan slidingExpiration)
        {
            T value = Get<T>(key);
            if(value == null)
            {
                value = creator();
                if(value != null)
                {
                    Add(key, value, slidingExpiration);
                }
            }
            return value;
        }

        public static void Remove(string key)
        {
            CacheManagerFactory.DefaultCacheManager.GlobalObjectCache().Remove(key);
            CacheExpiredNotification.Notify("___GlobalCache___", key);
        }

        public static void RemoveByPrefix(string keyPrefix)
        {
            var keys = CacheManagerFactory.DefaultCacheManager.GlobalObjectCache()
                .Where(it => it.Key.StartsWith(keyPrefix))
                .Select(it => it.Key);

            foreach(var key in keys)
            {
                CacheManagerFactory.DefaultCacheManager.GlobalObjectCache().Remove(key);
                CacheExpiredNotification.Notify("___GlobalCache___", key);
            }
        }

        /// <summary>
        /// Clear site cache
        /// </summary>
        /// <param name="site"></param>
        public static void Clear(Site site)
        {
            site.ClearCache();

            Repository repository = site.GetRepository();
            if(repository != null)
            {
                repository.ClearCache();
            }

            CacheManagerFactory.DefaultCacheManager.ClearGlobalObjectCache();
        }
    }
}
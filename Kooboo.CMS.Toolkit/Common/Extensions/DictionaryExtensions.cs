using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Toolkit
{
    public static class DictionaryExtensions
    {
        public static int GetInt<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.GetValue<TKey, TValue, int>(key);
        }

        public static int GetInt<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, int defaultValue)
        {
            return dictionary.GetValue<TKey, TValue, int>(key, defaultValue);
        }

        public static float GetFloat<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.GetValue<TKey, TValue, float>(key);
        }

        public static float GetFloat<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, float defaultValue)
        {
            return dictionary.GetValue<TKey, TValue, float>(key, defaultValue);
        }

        public static decimal GetDecimal<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.GetValue<TKey, TValue, decimal>(key);
        }

        public static decimal GetDecimal<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, decimal defaultValue)
        {
            return dictionary.GetValue<TKey, TValue, decimal>(key, defaultValue);
        }

        public static double GetDouble<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.GetValue<TKey, TValue, double>(key);
        }

        public static double GetDouble<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, double defaultValue)
        {
            return dictionary.GetValue<TKey, TValue, double>(key, defaultValue);
        }

        public static DateTime GetDateTime<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.GetValue<TKey, TValue, DateTime>(key);
        }

        public static DateTime GetDateTime<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, DateTime defaultValue)
        {
            return dictionary.GetValue<TKey, TValue, DateTime>(key, defaultValue);
        }

        public static string GetString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return GetString(dictionary, key, String.Empty);
        }

        public static string GetString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, string defaultValue)
        {
            if (dictionary != null && dictionary.ContainsKey(key) && dictionary[key] != null)
            {
                return dictionary[key].AsString(defaultValue);
            }

            return defaultValue;
        }

        public static bool GetBool<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return GetBool(dictionary, key, false);
        }

        public static bool GetBool<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, bool defaultValue)
        {
            if (dictionary != null && dictionary.ContainsKey(key) && dictionary[key] != null)
            {
                return dictionary[key].AsBool(defaultValue);
            }

            return defaultValue;
        }

        public static T GetValue<TKey, TValue, T>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.GetValue<TKey, TValue, T>(key, default(T));
        }

        public static T GetValue<TKey, TValue, T>(this IDictionary<TKey, TValue> dictionary, TKey key, T defaultValue)
        {
            if (dictionary != null && dictionary.ContainsKey(key) && dictionary[key] != null)
            {
                return dictionary[key].As<T>(defaultValue);
            }

            return defaultValue;
        }
    }
}
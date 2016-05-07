using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Routing;

namespace Kooboo.CMS.Toolkit
{
    public static class RouteValueDictionaryExtensions
    {
        public static RouteValueDictionary Merge(this RouteValueDictionary dictionary, object values)
        {
            if(values == null)
            {
                return dictionary;
            }
            else
            {
                RouteValueDictionary routeValues;
                if(values is IDictionary<string, object>)
                {
                    routeValues = new RouteValueDictionary((IDictionary<string, object>)values);
                }
                else
                {
                    routeValues = new RouteValueDictionary(values);
                }

                return dictionary.Merge(routeValues);
            }
        }

        public static RouteValueDictionary Merge(this RouteValueDictionary dictionary, RouteValueDictionary dictionaryToMerge)
        {
            if(dictionaryToMerge == null)
                return dictionary;

            var newDictionary = new RouteValueDictionary(dictionary);
            foreach(var valueDictionary in dictionaryToMerge)
            {
                if(newDictionary.ContainsKey(valueDictionary.Key))
                {
                    newDictionary[valueDictionary.Key] = valueDictionary.Value;
                }
                else
                {
                    newDictionary.Add(valueDictionary.Key, valueDictionary.Value);
                }
            }

            return newDictionary;
        }
    }
}
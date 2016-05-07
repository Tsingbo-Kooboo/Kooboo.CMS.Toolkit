using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Toolkit
{
    public static class ObjectExtensions
    {
        private static readonly string[] Booleans = new string[] { "true", "yes", "on", "1" };

        public static int AsInt(this object value)
        {
            return As<int>(value);
        }

        public static int AsInt(this object value, int defaultValue)
        {
            return As<int>(value, defaultValue);
        }

        public static float AsFloat(this object value)
        {
            return As<float>(value);
        }

        public static float AsFloat(this object value, float defaultValue)
        {
            return As<float>(value, defaultValue);
        }

        public static decimal AsDecimal(this object value)
        {
            return As<decimal>(value);
        }

        public static decimal AsDecimal(this object value, decimal defaultValue)
        {
            return As<decimal>(value, defaultValue);
        }

        public static double AsDouble(this object value)
        {
            return As<double>(value);
        }

        public static double AsDouble(this object value, double defaultValue)
        {
            return As<double>(value, defaultValue);
        }

        public static DateTime AsDateTime(this object value)
        {
            return As<DateTime>(value);
        }

        public static DateTime AsDateTime(this object value, DateTime defaultValue)
        {
            return As<DateTime>(value, defaultValue);
        }

        public static DateTime AsDateTime(this object value, string format)
        {
            return AsDateTime(value, format, default(DateTime));
        }

        public static DateTime AsDateTime(this object value, string format, DateTime defaultValue)
        {
            string date = value.AsString();
            if (!String.IsNullOrEmpty(date))
            {
                try
                {
                    return DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
                }
                catch { }
            }

            return defaultValue;
        }

        public static string AsString(this object value)
        {
            return AsString(value, String.Empty);
        }

        public static string AsString(this object value, string defaultValue)
        {
            return As<string>(value, defaultValue);
        }

        public static bool AsBool(this object value)
        {
            return AsBool(value, false);
        }

        public static bool AsBool(this object value, bool defaultValue)
        {
            if (value != null && value != DBNull.Value)
            {
                return Booleans.Contains(value.ToString().ToLower());
            }

            return defaultValue;
        }

        public static T As<T>(this object value)
        {
            return As<T>(value, default(T));
        }

        public static T As<T>(this object value, T defaultValue)
        {
            T convertedValue = defaultValue;
            if (value != null && value != DBNull.Value && value is IConvertible)
            {
                try
                {
                    convertedValue = (T)value;
                }
                catch
                {
                    try
                    {
                        convertedValue = (T)Convert.ChangeType(value, typeof(T));
                    }
                    catch
                    { }
                }
            }

            return convertedValue;
        }
    }
}
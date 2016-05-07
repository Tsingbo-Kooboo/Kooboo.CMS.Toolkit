using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;
using System.Text.RegularExpressions;

namespace Kooboo.CMS.Toolkit
{
    public static class DecimalExtensions
    {
        public static string ToCurrency(this decimal price)
        {
            return ToCurrency(price, "C");
        }

        public static string ToCurrency(this decimal price, string format, string culture = "")
        {
            string currency = price.ToString(format);
            string currencySymbol = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;

            if (!string.IsNullOrEmpty(culture))
            {
                CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(culture);
                currency = price.ToString(format, cultureInfo);
                currencySymbol = cultureInfo.NumberFormat.CurrencySymbol;
            }
            
            if (currency.StartsWith(currencySymbol))
            {
                currency = currency.Replace(currencySymbol, currencySymbol + " ");
            }
            return currency;
        }
    }
}
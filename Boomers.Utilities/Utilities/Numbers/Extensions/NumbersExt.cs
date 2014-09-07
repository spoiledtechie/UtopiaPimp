using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Numbers.Extensions
{
    public static class NumbersExt
    {
        public static string ToAbbreviateNumber(this decimal number)
        {
            if (number >= 1000000)
                return decimal.Divide(number, 1000000).ToString() + "M";
            else if (number >= 1000)
                return decimal.Divide(number, 1000).ToString() + "K";
            else { return number.ToString(); }
        }
        public static string ToAbbreviateNumber(this int number)
        {
            if (number >= 1000000)
                return decimal.Divide(number, 1000000).ToString() + "M";
            else if (number >= 1000)
                return decimal.Divide(number, 1000).ToString() + "K";
            else { return number.ToString(); }
        }
        public static string ToCurrency(this decimal value, string cultureName)
        {
            CultureInfo currentCulture = new CultureInfo(cultureName);
            return (string.Format(currentCulture, "{0:C}", value));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Linq.Extensions
{
    public static partial class LinqExtensions
    {
        public static string Aggregate(this IEnumerable<string> enumeration, string separator)
        {
            // Check to see that enumeration is not null
            if (enumeration == null)
                throw new ArgumentNullException("enumeration");

            // Check to see that separator is not null
            if (separator == null)
                throw new ArgumentNullException("separator");

            string returnValue = string.Join(separator, enumeration.ToArray());

            if (returnValue.Length > separator.Length)
                return returnValue.Substring(separator.Length);

            return returnValue;
        }

        public static string Aggregate<T>(this IEnumerable<T> enumeration, Func<T, string> toString, string separator)
        {
            // Check to see that toString is not null
            if (toString == null)
                throw new ArgumentNullException("toString");

            return Aggregate(enumeration.Select(toString), separator);
        }
    }
}

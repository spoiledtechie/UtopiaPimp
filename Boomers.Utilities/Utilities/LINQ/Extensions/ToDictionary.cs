using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Boomers.Utilities.Linq.Extensions
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// Recreates a dictionary from an enumeration of key-value pairs.
        /// </summary>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> enumeration)
        {
            // Check to see that enumeration is not null
            if (enumeration == null)
                throw new ArgumentNullException("enumeration");

            return enumeration.ToDictionary(item => item.Key, item => item.Value);
        }

        public static Dictionary<TKey, IEnumerable<TElement>> ToDictionary<TKey, TElement>(this IEnumerable<IGrouping<TKey, TElement>> enumeration)
        {
            // Check to see that enumeration is not null
            if (enumeration == null)
                throw new ArgumentNullException("enumeration");

            return enumeration.ToDictionary(item => item.Key, item => item.Cast<TElement>());
        }
    }
}

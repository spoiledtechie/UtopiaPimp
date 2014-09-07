using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Linq.Extensions
{
    public static partial class LinqExtensions
    {
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            // Check to see that dictionary is not null
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            // Check to see that pairs is not null
            if (pairs == null)
                throw new ArgumentNullException("pairs");

            foreach (var pair in pairs)
            {
                dictionary.Add(pair.Key, pair.Value);
            }
        }

        public static void RemoveValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value)
        {
            // Check to see that dictionary is not null
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            foreach (var key in (from pair in dictionary
                                 where System.Collections.Generic.EqualityComparer<TValue>.Default.Equals(value, pair.Value)
                                 select pair.Key).ToArray())
            {
                dictionary.Remove(key);
            }
        }

        public static void RemoveValueRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TValue> values)
        {
            // Check to see that dictionary is not null
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            // Check to see that values is not null
            if (values == null)
                throw new ArgumentNullException("values");

            foreach (var value in values.ToArray())
            {
                RemoveValue(dictionary, value);
            }
        }

        public static void RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys)
        {
            // Check to see that dictionary is not null
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            // Check to see that keys is not null
            if (keys == null)
                throw new ArgumentNullException("keys");

            foreach (var key in keys.ToArray())
            {
                dictionary.Remove(key);
            }
        }
    }
}

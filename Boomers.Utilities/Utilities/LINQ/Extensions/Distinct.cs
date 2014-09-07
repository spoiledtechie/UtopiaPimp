using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Linq.Extensions
{
    public static partial class LinqExtensions
    {
        internal class EqualityComparer<T> : IEqualityComparer<T>
        {
            public Func<T, T, bool> Comparer { get; internal set; }
            public Func<T, int> Hasher { get; internal set; }

            bool IEqualityComparer<T>.Equals(T x, T y)
            {
                return this.Comparer(x, y);
            }

            int IEqualityComparer<T>.GetHashCode(T obj)
            {
                // No hashing capabilities. Default to Equals(x, y).
                if (this.Hasher == null)
                    return 0;

                return this.Hasher(obj);
            }
        }

        /// <summary>
        /// Gets distinct items by a comparer delegate.
        /// </summary>
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> enumeration, Func<T, T, bool> comparer)
        {
            return Distinct(enumeration, comparer, null);
        }

        /// <summary>
        /// Gets distinct items by comparer and hasher delegates (faster than only comparer).
        /// </summary>
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> enumeration, Func<T, T, bool> comparer, Func<T, int> hasher)
        {
            // Check to see that enumeration is not null
            if (enumeration == null)
                throw new ArgumentNullException("enumeration");

            // Check to see that comparer is not null
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return enumeration.Distinct(new EqualityComparer<T> { Comparer = comparer, Hasher = hasher });
        }
    }
}

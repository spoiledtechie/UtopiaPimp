using System;
using System.Collections.Generic;
using System.Linq;

namespace Boomers.Utilities.Linq.Extensions
{
    /// <summary>
    /// Extensions class.
    /// </summary>
    public static partial class LinqExtensions
    {
        /// <summary>
        /// Checks whether the source is in a list.
        /// </summary>
        /// <typeparam name="T">The type of node being traversed by the query.</typeparam>
        /// <param name="source">The item to check.</param>
        /// <param name="values">The values to check against.</param>
        /// <returns>true if the item is in the list; otherwise, false.</returns>
        public static bool In<T>(this T source, params T[] values) where T : IEquatable<T>
        {
            if (values == null)
                throw new ArgumentNullException("values");

            return In(source, ((IEnumerable<T>)values));
        }

        /// <summary>
        /// Checks whether the source is in a subquery.
        /// </summary>
        /// <typeparam name="T">The type of node being traversed by the query.</typeparam>
        /// <param name="source">The item to check.</param>
        /// <param name="values">The resultset from the subquery to check against.</param>
        /// <returns>true if the item is in the subquery's result; otherwise, false.</returns>
        public static bool In<T>(this T source, IEnumerable<T> values) where T : IEquatable<T>
        {
            if (values == null)
                throw new ArgumentNullException("values");

            foreach (T listValue in values)
            {
                if ((default(T).Equals(source) && default(T).Equals(listValue)) ||
                    (!default(T).Equals(source) && source.Equals(listValue)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

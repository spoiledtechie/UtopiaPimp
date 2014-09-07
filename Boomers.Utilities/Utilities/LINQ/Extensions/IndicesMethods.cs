using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Linq.Extensions
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// Gets the indices where the predicate is true.
        /// </summary>
        public static IEnumerable<int> IndicesWhere<T>(this IEnumerable<T> enumeration, Func<T, bool> predicate)
        {
            // Check to see that enumeration is not null
            if (enumeration == null)
                throw new ArgumentNullException("enumeration");

            // Check to see that predicate is not null
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            int i = 0;

            foreach (T item in enumeration)
            {
                if (predicate(item))
                    yield return i;

                i++;
            }
        }

        public static T FirstOrDefault<T>(this IList<T> list)
        {
            // Check to see that list is not null
            if (list == null)
                throw new ArgumentNullException("list");

            if (list.Count == 0)
                return default(T);

            return list[0];
        }

        public static T LastOrDefault<T>(this IList<T> list)
        {
            // Check to see that list is not null
            if (list == null)
                throw new ArgumentNullException("list");

            if (list.Count == 0)
                return default(T);

            return list[list.Count - 1];
        }

        public static T At<T>(this IEnumerable<T> enumeration, int index)
        {
            // Check to see that enumeration is not null
            if (enumeration == null)
                throw new ArgumentNullException("enumeration");

            return enumeration.Skip(index).First();
        }

        public static IEnumerable<T> At<T>(this IEnumerable<T> enumeration, params int[] indices)
        {
            return At(enumeration, (IEnumerable<int>)indices);
        }

        public static IEnumerable<T> At<T>(this IEnumerable<T> enumeration, IEnumerable<int> indices)
        {
            // Check to see that enumeration is not null
            if (enumeration == null)
                throw new ArgumentNullException("enumeration");

            // Check to see that indices is not null
            if (indices == null)
                throw new ArgumentNullException("indices");

            int currentIndex = 0;

            foreach (int index in indices.OrderBy(i => i))
            {
                while (currentIndex != index)
                {
                    enumeration = enumeration.Skip(1);
                    currentIndex++;
                }

                yield return enumeration.First();
            }
        }

        public static IEnumerable<KeyValuePair<int, T>> AsIndexed<T>(this IEnumerable<T> enumeration)
        {
            // Check to see that enumeration is not null
            if (enumeration == null)
                throw new ArgumentNullException("enumeration");

            int i = 0;

            foreach (var item in enumeration)
            {
                yield return new KeyValuePair<int, T>(i++, item);
            }
        }
    }
}

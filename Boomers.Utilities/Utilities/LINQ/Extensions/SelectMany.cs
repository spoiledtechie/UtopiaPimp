using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Linq.Extensions
{
    public static partial class LinqExtensions
    {
        public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> source)
        {
            // Check to see that source is not null
            if (source == null)
                throw new ArgumentNullException("source");

            foreach (var enumeration in source)
            {
                foreach (var item in enumeration)
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<T> SelectMany<T>(this IEnumerable<T[]> source)
        {
            // Check to see that source is not null
            if (source == null)
                throw new ArgumentNullException("source");

            foreach (var enumeration in source)
            {
                foreach (var item in enumeration)
                {
                    yield return item;
                }
            }
        }
    }
}

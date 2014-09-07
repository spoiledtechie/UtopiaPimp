using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Linq.Extensions
{
    public static partial class LinqExtensions
    {
        public static IEnumerable<T[]> GroupEvery<T>(this IEnumerable<T> enumeration, int count)
        {
            // Check to see that enumeration is not null
            if (enumeration == null)
                throw new ArgumentNullException("enumeration");

            if (count <= 0)
                throw new ArgumentOutOfRangeException("count");

            int current = 0;
            T[] array = new T[count];

            foreach (var item in enumeration)
            {
                array[current++] = item;

                if (current == count)
                {
                    yield return array;
                    current = 0;
                    array = new T[count];
                }
            }

            if (current != 0)
            {
                yield return array;
            }
        }
    }
}

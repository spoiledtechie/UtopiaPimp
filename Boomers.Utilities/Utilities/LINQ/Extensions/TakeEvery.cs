using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Linq.Extensions
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// Take items from 'startAt' every at 'hopLength' items.
        /// </summary>
        public static IEnumerable<T> TakeEvery<T>(this IEnumerable<T> enumeration, int startAt, int hopLength)
        {
            // Check to see that enumeration is not null
            if (enumeration == null)
                throw new ArgumentNullException("enumeration");

            int first = 0;
            int count = 0;

            foreach (T item in enumeration)
            {
                if (first < startAt)
                {
                    first++;
                }
                else if (first == startAt)
                {
                    yield return item;

                    first++;
                }
                else
                {
                    count++;

                    if (count == hopLength)
                    {
                        yield return item;

                        count = 0;
                    }
                }
            }
        }
    }
}

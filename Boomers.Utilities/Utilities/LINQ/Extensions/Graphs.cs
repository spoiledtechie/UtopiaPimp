using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Linq.Extensions
{
    public static partial class LinqExtensions
    {
        public static T FirstOrFallback<T>(this IEnumerable<T> enumeration, T fallback)
        {
            // Check to see that enumeration is not null
            if (enumeration == null)
                throw new ArgumentNullException("enumeration");

            IEnumerator<T> enumerator = enumeration.GetEnumerator();

            enumerator.Reset();

            if (enumerator.MoveNext())
                return enumerator.Current;

            return fallback;
        }
    }
}

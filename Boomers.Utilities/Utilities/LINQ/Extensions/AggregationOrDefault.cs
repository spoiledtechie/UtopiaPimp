using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Linq.Extensions
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// Aggregates an enumeration or returnes the default value when it's empty.
        /// </summary>
        public static T AggregationOrDefault<T>(this IEnumerable<T> enumeration,
                                                     Func<IEnumerable<T>, T> aggregationMethod)
        {
            // Check to see that enumeration is not null
            if (enumeration == null)
                throw new ArgumentNullException("enumeration");

            // Check to see that aggregationMethod is not null
            if (aggregationMethod == null)
                throw new ArgumentNullException("aggregationMethod");

            if (!enumeration.ContainsAtLeast(1))
                return default(T);

            return aggregationMethod(enumeration);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Linq.Extensions
{
    public static partial class LinqExtensions
    {
        public static IEnumerable<double> Normalize<T>(this IEnumerable<T> enumeration)
            where T : struct
        {
            // Check to see that enumeration is not null
            if (enumeration == null)
                throw new ArgumentNullException("enumeration");

            double sum = Convert.ToDouble(enumeration.Aggregate(default(T), (s, x) => new NumberWrapper<T>(s) + new NumberWrapper<T>(x)));

            return from value in enumeration
                   let normalized = Convert.ToDouble(value) / sum
                   select normalized;
        }
    }
}

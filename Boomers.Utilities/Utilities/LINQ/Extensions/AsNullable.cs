using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Linq.Extensions
{
    public static partial class LinqExtensions
    {
        public static IEnumerable<T?> AsNullable<T>(this IEnumerable<T> enumeration)
            where T : struct
        {
            return from item in enumeration
                   select new Nullable<T>(item);
        }
    }
}

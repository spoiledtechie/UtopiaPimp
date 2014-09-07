using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Linq.Extensions
{
    public static partial class LinqExtensions
    {
        public static IEnumerable<T> Intersect<T>(this IEnumerable<IEnumerable<T>> enumeration)
        {
            // Check to see that enumeration is not null
            if (enumeration == null)
                throw new ArgumentNullException("enumeration");

            IEnumerable<T> returnValue = null;

            foreach (var e in enumeration)
            {
                if (returnValue != null)
                    returnValue = e;
                else
                    returnValue = returnValue.Intersect(e);
            }

            return returnValue;
        }

    }
}

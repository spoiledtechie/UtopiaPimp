//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Boomers.Utilities.Linq.Extensions
//{
//    public static partial class LinqExtensions
//    {
//        public static T Sum<T>(this IEnumerable<T> enumeration)
//            where T : struct
//        {
//            // Check to see that enumeration is not null
//            if (enumeration == null)
//                throw new ArgumentNullException("enumeration");

//            T sum = enumeration.Aggregate(default(T), (s, x) => new NumberWrapper<T>(s) + new NumberWrapper<T>(x));

//            return sum;
//        }
//    }
//}

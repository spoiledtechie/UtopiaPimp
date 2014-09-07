using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Linq.Extensions
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// Checks whether the enumeration is an ordered superset of a subset enumeration using the type's default comparer.
        /// </summary>
        /// <remarks>See http://weblogs.asp.net/okloeten/archive/2008/04/22/6121373.aspx for more details.</remarks>
        public static bool SequenceSuperset<T>(this IEnumerable<T> enumeration, IEnumerable<T> subset)
        {
            return SequenceSuperset(enumeration, subset, System.Collections.Generic.EqualityComparer<T>.Default.Equals);
        }

        /// <summary>
        /// Checks whether the enumeration is an ordered superset of a subset enumeration.
        /// </summary>
        /// <remarks>See http://weblogs.asp.net/okloeten/archive/2008/04/22/6121373.aspx for more details.</remarks>
        public static bool SequenceSuperset<T>(this IEnumerable<T> enumeration,
                                                    IEnumerable<T> subset,
                                                    Func<T, T, bool> equalityComparer)
        {
            // Check to see that enumeration is not null
            if (enumeration == null)
                throw new ArgumentNullException("enumeration");

            // Check to see that subset is not null
            if (subset == null)
                throw new ArgumentNullException("subset");

            // Check to see that comparer is not null
            if (equalityComparer == null)
                throw new ArgumentNullException("comparer");

            using (IEnumerator<T> big = enumeration.GetEnumerator(), small = subset.GetEnumerator())
            {
                big.Reset(); small.Reset();

                while (big.MoveNext())
                {
                    // End of subset, which means we've gone through it all and it's all equal.
                    if (!small.MoveNext())
                        return true;

                    if (!equalityComparer(big.Current, small.Current))
                    {
                        // Comparison failed. Let's try comparing with the first item.
                        small.Reset();

                        // There's more than one item in the small enumeration. Guess why I know this.
                        small.MoveNext();

                        // No go with the first item? Reset the collection and brace for the next iteration of the big loop.
                        if (!equalityComparer(big.Current, small.Current))
                            small.Reset();
                    }
                }

                // End of both, which means that the small is the end of the big.
                if (!small.MoveNext())
                    return true;
            }

            return false;
        }
    }
}

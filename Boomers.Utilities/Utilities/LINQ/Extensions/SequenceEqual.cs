using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Linq.Extensions
{
    public static partial class LinqExtensions
    {
        public static bool SequenceEqual<T1, T2>(this IEnumerable<T1> left, IEnumerable<T2> right, Func<T1, T2, bool> comparer)
        {
            using (IEnumerator<T1> leftE = left.GetEnumerator())
            {
                using (IEnumerator<T2> rightE = right.GetEnumerator())
                {
                    bool leftNext = leftE.MoveNext(), rightNext = rightE.MoveNext();

                    while (leftNext && rightNext)
                    {
                        // If one of the items isn't the same...
                        if (!comparer(leftE.Current, rightE.Current))
                            return false;

                        leftNext = leftE.MoveNext();
                        rightNext = rightE.MoveNext();
                    }

                    // If left or right is longer
                    if (leftNext || rightNext)
                        return false;
                }
            }

            return true;
        }
    }
}

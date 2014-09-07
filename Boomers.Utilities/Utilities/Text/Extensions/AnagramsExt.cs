using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Boomers.Utilities.Collections;

namespace Boomers.Utilities.Text
{
    public static class AnagramsExt
    {
        /// <summary>
        /// returns all the combinations of the string given.
        /// AT - AT, TA, A, T
        /// </summary>
        /// <param name="characters"></param>
        /// <returns></returns>
        public static IEnumerable<string> Combinations(this String characters)
        {
            //Return all combinations of 1, 2, 3, etc length
            for (int i = 1; i <= characters.Length; i++)
            {
                foreach (string s in CombinationsImpl(characters, i))
                {
                    yield return s;
                }
            }
        }

        /// <summary>
        /// Return all combinations (n choose k, not permutations) for a given length
        /// </summary>
        /// <param name="characters"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static IEnumerable<string> CombinationsImpl(String characters, int length)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                if (length == 1)
                {
                    yield return characters.Substring(i, 1);
                }
                else
                {
                    foreach (string next in CombinationsImpl(characters.Substring(i + 1, characters.Length - (i + 1)), length - 1))
                        yield return characters[i] + next;
                }
            }
        }

        public static string ToWordKey(this string s)
        {
            s = s.ToUpper();
                        return new string(s.ToCharArray().OrderBy(x => x).ToArray());
        }

        public static IEnumerable<IEnumerable<T>> Anagrams<T>(this IEnumerable<T> collection) where T : IComparable<T>
        {
            var total = collection.Count();

            // provided str "cat" get all subsets c, a, ca, at, etc (really nonefficient)
            var subsets = collection.Permutations()
                .SelectMany(c => Enumerable.Range(1, total).Select(i => c.Take(i)))
                .Distinct(new CollectionComparer<T>());

            return subsets;
        }

        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> collection)
        {
            return collection.Count() > 1
                ?
                    from ch in collection
                    let set = new[] { ch }
                    from permutation in collection.Except(set).Permutations()
                    select set.Union(permutation)
                :
                    new[] { collection };
        }

        public static string MergeToStr(this IEnumerable<char> chars)
        {
            return new string(chars.ToArray());
        }
    }
}

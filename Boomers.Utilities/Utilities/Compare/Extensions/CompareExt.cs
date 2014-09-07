using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Compare
{
    public static class CompareExt
    {
        /// <summary>
        /// Checks the list to see if its equal to any of the values in the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool EqualsAnyOf<T>(this T source, params T[] list)
        {
            if (null == source) throw new ArgumentNullException("source");
            return list.Contains(source);
        }
        /// <summary>
        /// Checks if the number is inclusevly inbetween.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actual"></param>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public static bool IsBetween<T>(this T actual, T lower, T upper) where T : IComparable<T>
        {
            return actual.CompareTo(lower) >= 0 && actual.CompareTo(upper) < 0;
        }
        /// <summary>
        /// returns a random True or False
        /// </summary>
        /// <returns></returns>
        public static bool getRandomTrueFalse()
        {
            Random rand = new Random();
            if (rand.Next(0, 2) == 0)
                return false;
            else
                return true;
        }
    }
}

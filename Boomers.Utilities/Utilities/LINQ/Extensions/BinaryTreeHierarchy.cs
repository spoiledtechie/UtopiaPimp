using System;
using System.Collections.Generic;
using System.Linq;

namespace Boomers.Utilities.Linq.Extensions
{
    /// <summary>
    /// Extensions class.
    /// </summary>
    public static partial class LinqExtensions
    {
        #region TreePrefix
        /// <summary>
        /// Scans a binary tree in prefix order.
        /// </summary>
        /// <typeparam name="T">The type of tree node.</typeparam>
        /// <param name="source">The tree's root.</param>
        /// <param name="getLeft">Gets the left child of a tree node.</param>
        /// <param name="getRight">Gets the right child of a tree node.</param>
        /// <returns>A list of nodes in prefix order.</returns>
        public static IEnumerable<T> TreePrefix<T>(
            this T source,
            Func<T, T> getLeft,
            Func<T, T> getRight)
        {
            return source.TreePrefix<T>(getLeft, getRight, level => true);
        }

        /// <summary>
        /// Scans a binary tree in prefix order.
        /// </summary>
        /// <typeparam name="T">The type of tree node.</typeparam>
        /// <param name="source">The tree's root.</param>
        /// <param name="getLeft">Gets the left child of a tree node.</param>
        /// <param name="getRight">Gets the right child of a tree node.</param>
        /// <param name="levelLimit">Gets whether a node on a certain level be returned.</param>
        /// <returns>A list of nodes in prefix order.</returns>
        public static IEnumerable<T> TreePrefix<T>(
            this T source,
            Func<T, T> getLeft,
            Func<T, T> getRight,
            Func<int, bool> levelLimit)
        {
            return source.TreePrefix<T>(getLeft, getRight, levelLimit, 0);
        }

        private static IEnumerable<T> TreePrefix<T>(
            this T source,
            Func<T, T> getLeft,
            Func<T, T> getRight,
            Func<int, bool> levelLimit,
            int level)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (getLeft == null)
                throw new ArgumentNullException("getLeft");

            if (getRight == null)
                throw new ArgumentNullException("getRight");

            if (levelLimit == null)
                throw new ArgumentNullException("levelLimit");

            if (levelLimit(level))
            {
                yield return source;
            }

            T left = getLeft(source);
            T right = getRight(source);

            if (!default(T).Equals(left))
            {
                foreach (T subNode in left.TreePrefix<T>(getLeft, getRight, levelLimit, level + 1))
                {
                    yield return subNode;
                }
            }

            if (!default(T).Equals(right))
            {
                foreach (T subNode in right.TreePrefix<T>(getLeft, getRight, levelLimit, level + 1))
                {
                    yield return subNode;
                }
            }
        }
        #endregion

        #region TreePostfix
        /// <summary>
        /// Scans a binary tree in postfix order.
        /// </summary>
        /// <typeparam name="T">The type of tree node.</typeparam>
        /// <param name="source">The tree's root.</param>
        /// <param name="getLeft">Gets the left child of a tree node.</param>
        /// <param name="getRight">Gets the right child of a tree node.</param>
        /// <returns>A list of nodes in postfix order.</returns>
        public static IEnumerable<T> TreePostfix<T>(
            this T source,
            Func<T, T> getLeft,
            Func<T, T> getRight)
        {
            return source.TreePostfix<T>(getLeft, getRight, level => true);
        }

        /// <summary>
        /// Scans a binary tree in postfix order.
        /// </summary>
        /// <typeparam name="T">The type of tree node.</typeparam>
        /// <param name="source">The tree's root.</param>
        /// <param name="getLeft">Gets the left child of a tree node.</param>
        /// <param name="getRight">Gets the right child of a tree node.</param>
        /// <param name="levelLimit">Gets whether a node on a certain level be returned.</param>
        /// <returns>A list of nodes in postfix order.</returns>
        public static IEnumerable<T> TreePostfix<T>(
            this T source,
            Func<T, T> getLeft,
            Func<T, T> getRight,
            Func<int, bool> levelLimit)
        {
            return source.TreePostfix<T>(getLeft, getRight, levelLimit, 0);
        }

        private static IEnumerable<T> TreePostfix<T>(
            this T source,
            Func<T, T> getLeft,
            Func<T, T> getRight,
            Func<int, bool> levelLimit,
            int level)
        {
            if (getLeft == null)
                throw new ArgumentNullException("getLeft");

            if (getRight == null)
                throw new ArgumentNullException("getRight");

            if (levelLimit == null)
                throw new ArgumentNullException("levelLimit");

            T left = getLeft(source);
            T right = getRight(source);

            if (!default(T).Equals(left))
            {
                foreach (T subNode in left.TreePostfix<T>(getLeft, getRight, levelLimit, level + 1))
                {
                    yield return subNode;
                }
            }

            if (!default(T).Equals(right))
            {
                foreach (T subNode in right.TreePostfix<T>(getLeft, getRight, levelLimit, level + 1))
                {
                    yield return subNode;
                }
            }

            if (levelLimit(level))
            {
                yield return source;
            }
        }
        #endregion

        #region TreeInfix
        /// <summary>
        /// Scans a binary tree in infix order.
        /// </summary>
        /// <typeparam name="T">The type of tree node.</typeparam>
        /// <param name="source">The tree's root.</param>
        /// <param name="getLeft">Gets the left child of a tree node.</param>
        /// <param name="getRight">Gets the right child of a tree node.</param>
        /// <returns>A list of nodes in infix order.</returns>
        public static IEnumerable<T> TreeInfix<T>(
            this T source,
            Func<T, T> getLeft,
            Func<T, T> getRight)
        {
            return source.TreeInfix<T>(getLeft, getRight, level => true);
        }

        /// <summary>
        /// Scans a binary tree in infix order.
        /// </summary>
        /// <typeparam name="T">The type of tree node.</typeparam>
        /// <param name="source">The tree's root.</param>
        /// <param name="getLeft">Gets the left child of a tree node.</param>
        /// <param name="getRight">Gets the right child of a tree node.</param>
        /// <param name="levelLimit">Gets whether a node on a certain level be returned.</param>
        /// <returns>A list of nodes in infix order.</returns>
        public static IEnumerable<T> TreeInfix<T>(
            this T source,
            Func<T, T> getLeft,
            Func<T, T> getRight,
            Func<int, bool> levelLimit)
        {
            return source.TreeInfix<T>(getLeft, getRight, levelLimit, 0);
        }

        private static IEnumerable<T> TreeInfix<T>(
            this T source,
            Func<T, T> getLeft,
            Func<T, T> getRight,
            Func<int, bool> levelLimit,
            int level)
        {
            if (getLeft == null)
                throw new ArgumentNullException("getLeft");

            if (getRight == null)
                throw new ArgumentNullException("getRight");

            if (levelLimit == null)
                throw new ArgumentNullException("levelLimit");

            T left = getLeft(source);
            T right = getRight(source);

            if (!default(T).Equals(left))
            {
                foreach (T subNode in left.TreeInfix<T>(getLeft, getRight, levelLimit, level + 1))
                {
                    yield return subNode;
                }
            }

            if (levelLimit(level))
            {
                yield return source;
            }

            if (!default(T).Equals(right))
            {
                foreach (T subNode in right.TreeInfix<T>(getLeft, getRight, levelLimit, level + 1))
                {
                    yield return subNode;
                }
            }
        }
        #endregion
    }
}

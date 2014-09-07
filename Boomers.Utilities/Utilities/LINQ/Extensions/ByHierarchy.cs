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
        /// <summary>
        /// A node in the traversal.
        /// </summary>
        /// <typeparam name="T">The type of item being traversed.</typeparam>
        public class Node<T>
        {
            internal Node()
            {
            }

            /// <summary>
            /// The node's level in the hierarchy.
            /// </summary>
            public int Level;

            /// <summary>
            /// The node's parent node.
            /// </summary>
            public Node<T> Parent;

            /// <summary>
            /// The item the node represents.
            /// </summary>
            public T Item;
        }

        #region ByHierarchy
        /// <summary>
        /// Scans a source in hierarchical order.
        /// </summary>
        /// <typeparam name="T">The type of item being traversed.</typeparam>
        /// <param name="source">The hierarchy's entry point.</param>
        /// <param name="startWith">Gets an item and returns true if it is one of the root items in the hierarchy.</param>
        /// <param name="connectBy">Gets two items and returns true if the first is the second's parent.</param>
        /// <returns>A list of item nodes in hierarchical order.</returns>
        public static IEnumerable<Node<T>> ByHierarchy<T>(
            this IEnumerable<T> source,
            Func<T, bool> startWith,
            Func<T, T, bool> connectBy)
        {
            return source.ByHierarchy<T>(startWith, connectBy, level => true);
        }

        /// <summary>
        /// Scans a source in hierarchical order.
        /// </summary>
        /// <typeparam name="T">The type of item being traversed.</typeparam>
        /// <param name="source">The hierarchy's entry point.</param>
        /// <param name="startWith">Gets an item and returns true if it is one of the root items in the hierarchy.</param>
        /// <param name="connectBy">Gets two items and returns true if the first is the second's parent.</param>
        /// <param name="levelLimit">Gets a level and returns true if nodes from this level should be returned.</param>
        /// <returns>A list of item nodes in hierarchical order.</returns>
        public static IEnumerable<Node<T>> ByHierarchy<T>(
            this IEnumerable<T> source,
            Func<T, bool> startWith,
            Func<T, T, bool> connectBy,
            Func<int, bool> levelLimit)
        {
            return source.ByHierarchy<T>(startWith, connectBy, levelLimit, null);
        }

        private static IEnumerable<Node<T>> ByHierarchy<T>(
            this IEnumerable<T> source,
            Func<T, bool> startWith,
            Func<T, T, bool> connectBy,
            Func<int, bool> levelLimit,
            Node<T> parent)
        {
            int level = (parent == null ? 0 : parent.Level + 1);

            if (source == null)
                throw new ArgumentNullException("source");

            if (startWith == null)
                throw new ArgumentNullException("startWith");

            if (connectBy == null)
                throw new ArgumentNullException("connectBy");

            if (levelLimit == null)
                throw new ArgumentNullException("levelLimit");

            foreach (T value in from item in source
                                where startWith(item)
                                select item)
            {
                Node<T> newNode = new Node<T> { Level = level, Parent = parent, Item = value };

                if (levelLimit(level))
                {
                    yield return newNode;
                }

                foreach (Node<T> subNode in source.ByHierarchy<T>(possibleSub => connectBy(value, possibleSub),
                                                                  connectBy, levelLimit, newNode))
                {
                    yield return subNode;
                }
            }
        }
        #endregion

        #region ByReverseHierarchy
        /// <summary>
        /// Scans a source in reverse hierarchical order.
        /// </summary>
        /// <typeparam name="T">The type of item being traversed.</typeparam>
        /// <param name="source">The hierarchy's entry point.</param>
        /// <param name="startWith">Gets an item and returns true if it is one of the root items in the hierarchy.</param>
        /// <param name="connectBy">Gets two items and returns true if the first is the second's parent.</param>
        /// <returns>A list of item nodes in reverse hierarchical order.</returns>
        public static IEnumerable<Node<T>> ByReverseHierarchy<T>(
            this IEnumerable<T> source,
            Func<T, bool> startWith,
            Func<T, T, bool> connectBy)
        {
            return source.ByReverseHierarchy<T>(startWith, connectBy, level => true);
        }

        /// <summary>
        /// Scans a source in reverse hierarchical order.
        /// </summary>
        /// <typeparam name="T">The type of item being traversed.</typeparam>
        /// <param name="source">The hierarchy's entry point.</param>
        /// <param name="startWith">Gets an item and returns true if it is one of the root items in the hierarchy.</param>
        /// <param name="connectBy">Gets two items and returns true if the first is the second's parent.</param>
        /// <param name="levelLimit">Gets a level and returns true if nodes from this level should be returned.</param>
        /// <returns>A list of item nodes in reverse hierarchical order.</returns>
        public static IEnumerable<Node<T>> ByReverseHierarchy<T>(
            this IEnumerable<T> source,
            Func<T, bool> startWith,
            Func<T, T, bool> connectBy,
            Func<int, bool> levelLimit)
        {
            return source.ByReverseHierarchy<T>(startWith, connectBy, levelLimit, null);
        }

        private static IEnumerable<Node<T>> ByReverseHierarchy<T>(
            this IEnumerable<T> source,
            Func<T, bool> startWith,
            Func<T, T, bool> connectBy,
            Func<int, bool> levelLimit,
            Node<T> parent)
        {
            int level = (parent == null ? 0 : parent.Level + 1);

            if (source == null)
                throw new ArgumentNullException("source");

            if (startWith == null)
                throw new ArgumentNullException("startWith");

            if (connectBy == null)
                throw new ArgumentNullException("connectBy");

            foreach (T value in from item in source
                                where startWith(item)
                                select item)
            {
                Node<T> newNode = new Node<T> { Level = level, Parent = parent, Item = value };

                foreach (Node<T> subNode in source.ByReverseHierarchy<T>(possibleSub => connectBy(value, possibleSub),
                                                                         connectBy, levelLimit, newNode))
                {
                    yield return subNode;
                }

                if (levelLimit(level))
                {
                    yield return newNode;
                }
            }
        }
        #endregion
    }
}

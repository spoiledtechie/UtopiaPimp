using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Linq.Extensions
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// Asynchronously begins buffering an enumeration, even before it is lazy loaded.
        /// </summary>
        public static IEnumerable<T> Buffered<T>(this IEnumerable<T> enumeration)
        {
            // Check to see that enumeration is not null
            if (enumeration == null)
                throw new ArgumentNullException("enumeration");

            return new BufferedEnumerable<T>(enumeration);
        }

        private class BufferedEnumerable<T> : IEnumerable<T>
        {
            private bool stillBuffering;
            private IEnumerator<T> enumerator;
            private IAsyncResult asyncResult;
            private List<T> buffer;
            private Action bufferAction;

            public BufferedEnumerable(IEnumerable<T> enumeration)
            {
                this.enumerator = enumeration.GetEnumerator();
                this.stillBuffering = true;
                this.buffer = new List<T>();
                this.bufferAction = this.Buffer;

                this.asyncResult = this.bufferAction.BeginInvoke(null, null);
            }

            private void Buffer()
            {
                try
                {
                    bool more;

                    do
                    {
                        more = false;

                        lock (this.enumerator)
                        {
                            if (this.enumerator.MoveNext())
                            {
                                buffer.Add(enumerator.Current);
                                more = true;
                            }
                        }

                    } while (more);
                }
                finally
                {
                    this.stillBuffering = false;
                }
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                // Take care of already buffered values.
                IEnumerable<T> bufferedValues = TryGetBufferedValues();

                if (bufferedValues != null)
                {
                    foreach (var value in bufferedValues)
                    {
                        yield return value;
                    }
                }

                while (stillBuffering)
                {
                    // Try to get new values if there are any.
                    bufferedValues = TryGetBufferedValues();

                    // Free the enumeration to continue while we return the buffered values.
                    if (bufferedValues != null)
                    {
                        foreach (var value in bufferedValues)
                        {
                            yield return value;
                        }
                    }
                }

                // End invocation so that exceptions could be throw here.
                this.bufferAction.EndInvoke(this.asyncResult);

                foreach (var value in this.buffer)
                {
                    yield return value;
                }
            }

            private IEnumerable<T> TryGetBufferedValues()
            {
                IEnumerable<T> bufferedValues = null;

                // Wait until we're not enumerating ( = there are new values).
                lock (this.enumerator)
                {
                    if (buffer.Count > 0)
                    {
                        bufferedValues = buffer.ToArray();
                        buffer.Clear();
                    }
                }

                return bufferedValues;
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return ((IEnumerable<T>)this).GetEnumerator();
            }
        }
    }
}

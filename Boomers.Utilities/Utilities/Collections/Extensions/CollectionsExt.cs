using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Boomers.Utilities.Collections.Extensions
{
    internal static class CollectionsExt
    {
        internal static string ToQueryString(this NameValueCollection values)
        {
            if (values == null)
                return string.Empty;
            var list = (from string key in values.Keys select HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(values[key])).ToList();
            return list.Concat("&");
        }
        internal static string Concat<T>(this IEnumerable<T> values, string delimiter)
        {
            StringBuilder sb = new StringBuilder();
            int c = 0;
            if (values == null) values = new T[0];
            foreach (T k in values)
            {
                if (c++ > 0)
                    sb.Append(delimiter);
                sb.Append(k);
            }
            return sb.ToString();
        }

        internal delegate string StringEncodeHandler<in T>(T input);
        internal static string Concat<T>(this IEnumerable<T> values, StringEncodeHandler<T> encodeValue)
        {
            return values.Concat("", encodeValue);
        }

        internal static string Concat<T>(this IEnumerable<T> values, string delimiter, StringEncodeHandler<T> encodeValue)
        {
            StringBuilder sb = new StringBuilder();
            int c = 0;
            if (values == null) values = new T[0];
            foreach (T k in values)
            {
                if (c++ > 0)
                    sb.Append(delimiter);
                sb.Append(encodeValue(k));
            }
            return sb.ToString();
        }
    }
}

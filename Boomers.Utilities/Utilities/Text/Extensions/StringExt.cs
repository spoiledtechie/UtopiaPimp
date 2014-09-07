using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Boomers.Political.SunlightFoundation.ViewModels;
using System.Web;
using System.Runtime.InteropServices;

namespace Boomers.Utilities.Text
{
    public static class StringExt
    {
        //[DllImport("user32.dll")]
        //static extern short VkKeyScan(char ch);

        //static public Key ResolveKey(char charToResolve)
        //{
        //    return KeyInterop.KeyFromVirtualKey(VkKeyScan(charToResolve));
        //}
        internal static string ResolveUrl(this string str)
        {
            if (HttpContext.Current == null || str.Contains("://"))
                return str;
            if (str.StartsWith("~/"))
            {
                var appPath = HttpContext.Current.Request.ApplicationPath;
                if (appPath == "/")
                    appPath = "";
                str = appPath + str.Substring(1);
            }
            return new Uri(HttpContext.Current.Request.Url, str).ToString();
        }

        public static Suffix ToSuffix(this string s)
        {
            if (s.ToLower().Contains("jr"))
                return Suffix.Jr;
            else if (s.ToLower().Contains("sr"))
                return Suffix.Sr;
            else if (s.ToLower() == "i")
                return Suffix.I;
            else if (s.ToLower() == "ii")
                return Suffix.II;
            else if (s.ToLower() == "iii")
                return Suffix.III;

            return Suffix.I;
        }

        public static string ToPascelCase(this string s)
        {
            // Creates a TextInfo based on the "en-US" culture.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            System.Globalization.TextInfo myTI = new System.Globalization.CultureInfo("en-US", false).TextInfo;
            return myTI.ToTitleCase(s);
        }

        public static string ToSearchEngineFriendly(this string text)
        {
            if (text != null)
            {
                text = text.Replace("$", "").Replace(")", "").Replace("(", "").Replace("=", "").Replace("&", "").Replace("*", "").Replace("`", "").Replace("!", "").Replace(".", " ").Replace(":", " ").Replace("?", "").Replace("'", "").Replace("/", " ").Replace("\"", " ").Replace(",", " ").Trim().Replace(" ", "-").Replace("´","");
                if (text.Length > 100)
                    text = text.Remove(100);
                return text;
            } return string.Empty;
        }
        public static string ToSearchFriendly(this string text)
        {
            if (text != null)
            {
                text = text.Replace("$", "").Replace(")", "").Replace("(", "").Replace("=", "").Replace("&", "").Replace("*", "").Replace("`", "").Replace("!", "").Replace(".", " ").Replace("?", "").Replace(":", " ").Replace("'", "").Replace("/", " ").Replace("\"", " ").Replace(",", " ").Replace("´", "").Trim();
                if (text.Length > 100)
                    text = text.Remove(100);
                return text;
            }
            return string.Empty;
        }
        /// <summary>
        /// Returns a Lower case value of the string.
        /// </summary>
        /// <returns></returns>
        public static String[] ToLower(this String[] s)
        {
            int i = 0;
            String[] stringinterate = s;
            foreach (string item in s)
            {
                stringinterate[i] = item.ToLower();
                i += 1;
            }
            return stringinterate;
        }
        /// <summary>
        /// Encodes the String to certain value.
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encode(this string Text)
        {
            int c = 0;
            string Temp = "";
            for (int i = Text.Length; i >= 1; i--)
            {
                c = System.Convert.ToInt32(Text[i - 1]);
                c = (c - 26) - i;
                if (c < 0)
                    c = 256 + c;

                Temp += (char)(c);
            }
            return Temp;
        }
        /// <summary>
        /// Decodes the string to certain value.
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decode(this string Text)
        {
            int c = 0;
            string Temp = "";
            for (int i = 1; i <= Text.Length; i++)
            {
                c = System.Convert.ToInt32(Text[i - 1]);
                c = (c + 26) + i;
                if (c > 255)
                    c = c - 256;

                Temp += (char)(c);
            }
            return Temp;
        }
        /// <summary>
        /// Removes all HTML attributes in the text.
        /// </summary>
        /// <param name="text">HTML Text.</param>
        /// <returns>No HTML Text</returns>
        public static string htmlDecode(this string text)
        {
            Regex regex = new Regex("<[^>]*>");
            return regex.Replace(text, " ");
        }
        /// <summary>
        /// Parses a string into an Enum
        /// </summary>
        /// <typeparam name="T">The type of the Enum</typeparam>
        /// <param name="value">String value to parse</param>
        /// <returns>The Enum corresponding to the stringExtensions</returns>
        public static T EnumParse<T>(this string value)
        {
            return StringExt.EnumParse<T>(value, false);
        }

        public static T EnumParse<T>(this string value, bool ignorecase)
        {

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            value = value.Trim();

            if (value.Length == 0)
            {
                throw new ArgumentException("Must specify valid information for parsing in the string.", "value");
            }

            Type t = typeof(T);

            if (!t.IsEnum)
            {
                throw new ArgumentException("Type provided must be an Enum.", "T");
            }

            return (T)Enum.Parse(t, value, ignorecase);
        }
        public static string Wordify(this string camelCaseWord)
        {
            // if the word is all upper, just return it
            if (!Regex.IsMatch(camelCaseWord, "[a-z]"))
                return camelCaseWord;

            return string.Join(" ", Regex.Split(camelCaseWord, @"(?<!^)(?=[A-Z])"));
        }
        public static bool IsInteger(this string input)
        {
            int temp;

            return int.TryParse(input, out temp);
        }

        public static bool IsDecimal(this string input)
        {
            decimal temp;

            return decimal.TryParse(input, out temp);
        }

        public static int ToInteger(this string input, int defaultValue)
        {
            int temp;

            return (int.TryParse(input, out temp)) ? temp : defaultValue;
        }

        public static decimal ToDecimal(this string input, decimal defaultValue)
        {
            decimal temp;

            return (decimal.TryParse(input, out temp)) ? temp : defaultValue;
        }
        public static string RemoveDashes(this string text)
        {
            return text.ToString().Replace("-", "");
        }
    }
}

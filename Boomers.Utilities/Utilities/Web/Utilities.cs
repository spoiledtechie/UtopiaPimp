using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Boomers.Utilities.Web
{
    /// <summary>
    /// wwHttp Utility class to provide UrlEncoding without the need to use
    /// the System.Web libraries (too much overhead)
    /// </summary>
    public class Utilities
    {
        private static string domainName = null;
        public static string GetDomain
        {
            get
            {
                if (String.IsNullOrEmpty(domainName))
                {
                    domainName = HttpContext.Current.Request.Url.Host.ToLower();
                }
                return domainName;
            }

        }

        /// <summary>
        /// gets the room URL of a website.
        /// </summary>
        /// <returns></returns>
        public static string GetSiteRoot()
        {
            string port = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            if (port == null || port == "80" || port == "443")
                port = "";
            else
                port = ":" + port;

            string protocol = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (protocol == null || protocol == "0")
                protocol = "http://";
            else
                protocol = "https://";

            string sOut = protocol + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + port + System.Web.HttpContext.Current.Request.ApplicationPath;

            if (sOut.EndsWith("/"))
            {
                sOut = sOut.Substring(0, sOut.Length - 1);
            }

            return sOut;
        }
                    /// <summary>
        /// UrlEncodes a string without the requirement for System.Web
        /// </summary>
        /// <param name="String"></param>
        /// <returns></returns>
        public static string UrlEncode(string InputString)
        {
            StringReader sr = new StringReader(InputString);
            StringBuilder sb = new StringBuilder(InputString.Length);

            while (true)
            {
                int lnVal = sr.Read();
                if (lnVal == -1)
                    break;
                char lcChar = (char)lnVal;

                if (lcChar >= 'a' && lcChar < 'z' ||
                    lcChar >= 'A' && lcChar < 'Z' ||
                    lcChar >= '0' && lcChar < '9')
                    sb.Append(lcChar);
                else if (lcChar == ' ')
                    sb.Append("+");
                else
                    sb.AppendFormat("%{0:X2}", lnVal);
            }

            return sb.ToString();
        }
        /// <summary>
        /// UrlDecodes a string without requiring System.Web
        /// </summary>
        /// <param name="InputString">String to decode.</param>
        /// <returns>decoded string</returns>
        public static string UrlDecode(string InputString)
        {
            char temp = ' ';
            StringReader sr = new StringReader(InputString);
            StringBuilder sb = new StringBuilder(InputString.Length);

            while (true)
            {
                int lnVal = sr.Read();
                if (lnVal == -1)
                    break;
                char TChar = (char)lnVal;
                if (TChar == '+')
                    sb.Append(' ');
                else if (TChar == '%')
                {
                    // *** read the next 2 chars and parse into a char
                    temp = (char)Int32.Parse(((char)sr.Read()).ToString() + ((char)sr.Read()).ToString(),
                                                   System.Globalization.NumberStyles.HexNumber);
                    sb.Append(temp);
                }
                else
                    sb.Append(TChar);
            }

            return sb.ToString();
        }
        /// <summary>
        /// Retrieves a value by key from a UrlEncoded string.
        /// </summary>
        /// <param name="UrlEncodedString">UrlEncoded String</param>
        /// <param name="Key">Key to retrieve value for</param>
        /// <returns>returns the value or "" if the key is not found or the value is blank</returns>
        public static string GetUrlEncodedKey(string UrlEncodedString, string Key)
        {
            UrlEncodedString = "&" + UrlEncodedString + "&";

            int Index = UrlEncodedString.ToLower().IndexOf("&" + Key.ToLower() + "=");
            if (Index < 0)
                return "";

            int lnStart = Index + 2 + Key.Length;

            int Index2 = UrlEncodedString.IndexOf("&", lnStart);
            if (Index2 < 0)
                return "";

            string Result = UrlDecode(UrlEncodedString.Substring(lnStart, Index2 - lnStart));

            return Result;
        }
        //<httpModules>
        //<add type="WwwSubDomainModule" name="WwwSubDomainModule" />
        //</httpModules>
        //You also need to add an appSetting like so:
        //<appSettings>
        //<!-- Values can be 'add' or 'remove' -->
        //<add key="WwwRule" value="add"/>
        //</appSettings> 
        /// <summary>
        /// Handles the BeginRequest event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void context_BeginRequest(object sender, EventArgs e)
        {
            string rule = ConfigurationManager.AppSettings.Get("WwwRule");
            HttpContext context = (sender as HttpApplication).Context;
            if (context.Request.HttpMethod != "GET" || context.Request.IsLocal)
                return;
            if (context.Request.PhysicalPath.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
            {
                string url = context.Request.Url.ToString();
                if (url.Contains("://www.") && rule == "remove")
                    RemoveWww(context);

                if (!url.Contains("://www.") && rule == "add")
                    AddWww(context);
            }
        }
        /// <summary>
        /// Adds the www subdomain to the request and redirects.
        /// </summary>
        private static void AddWww(HttpContext context)
        {
            string url = context.Request.Url.ToString().Replace("://", "://www.");
            PermanentRedirect(url, context);
        }
        private static readonly Regex _Regex = new Regex("(http|https)://www\\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Removes the www subdomain from the request and redirects.
        /// </summary>
        private static void RemoveWww(HttpContext context)
        {
            string url = context.Request.Url.ToString();
            if (_Regex.IsMatch(url))
            {
                url = _Regex.Replace(url, "$1://");
                PermanentRedirect(url, context);
            }
        }
        /// <summary>
        /// Sends permanent redirection headers (301)
        /// </summary>
        private static void PermanentRedirect(string url, HttpContext context)
        {
            context.Response.Clear();
            context.Response.StatusCode = 301;
            context.Response.AppendHeader("location", url);
            context.Response.End();
        }
    }
}

using System.Collections.Generic;
using System.Security.Cryptography;
using System.Web.Mvc;
using System.Web.Routing;

namespace Boomers.Utilities.MVCHelpers
{
    public static class GravatarHtmlHelper
    {
        public static string Gravatar(this HtmlHelper html, string email)
        {
            return GetImageTag(GetGravatar(email));
        }

        public static string Gravatar(this HtmlHelper html, string email, object gravatarAttributes)
        {
            return GetImageTag(GetGravatar(email, gravatarAttributes));
        }

        public static string Gravatar(this HtmlHelper html, string email, object gravatarAttributes, object htmlAttributes)
        {
            return GetImageTag(GetGravatar(email, gravatarAttributes), htmlAttributes);
        }

        private static string GetImageTag(string source)
        {
            return GetImageTag(source, null);
        }

        private static string GetImageTag(string source, object htmlAttributes)
        {

            IDictionary<string, object> attributes =
                (htmlAttributes == null
                    ? new RouteValueDictionary()
                    : new RouteValueDictionary(htmlAttributes));

            string returnVal = "<img src=\"{0}\"";

            foreach (string key in attributes.Keys)
            {
                returnVal += string.Format("{0}=\"{1}\"", key, attributes[key]);
            }
            returnVal += " />";
            return string.Format(returnVal, source);
        }

        private static string GetGravatar(string email)
        {
            return string.Format("http://www.gravatar.com/avatar/{0}", EncryptMD5(email));
        }

        private static string GetGravatar(string email, object gravatarAttributes)
        {
            IDictionary<string, object> attributes = (gravatarAttributes == null
                    ? new RouteValueDictionary()
                    : new RouteValueDictionary(gravatarAttributes));

            string returnVal = GetGravatar(email);
            bool first = true;

            foreach (string key in attributes.Keys)
            {
                if (first)
                {
                    first = false;
                    returnVal += string.Format("?{0}={1}", key, attributes[key].ToString());
                    break;
                }
                returnVal += string.Format("&{0}={1}", key, attributes[key].ToString());
            }

            return returnVal;
        }

        private static string EncryptMD5(string Value)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] valueArray = System.Text.Encoding.ASCII.GetBytes(Value);
            valueArray = md5.ComputeHash(valueArray);
            string encrypted = "";
            for (int i = 0; i < valueArray.Length; i++)
                encrypted += valueArray[i].ToString("x2").ToLower();
            return encrypted;
        }
    }
}
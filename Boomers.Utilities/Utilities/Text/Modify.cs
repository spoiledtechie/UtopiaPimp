using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Boomers.Utilities.Text
{
    public class Modify
    {
        /// <summary>
        /// Returns only the last 4 digits of a credit card number and replaces the other digits with "X"
        /// </summary>
        /// <param name="CCnumber">A credit card number</param>
        /// <returns>A Masked CC number with only the last 4 digits visible</returns>
        public static string MaskCreditCardNumber(string CCnumber)
        {
            int len = CCnumber.Length - 4;
            string mask = "";
            for (int i = 0; i < len; i++)
            {
                mask += "X";
            }
            mask += CCnumber.Substring(CCnumber.Length - 4);
            return mask;
        }
        /// <summary>
        /// Encodes a string for html display
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HtmlEncodeString(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }
        /// <summary>
        /// Converts carriage returns to html breaks for display
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string NewLinesToBreaks(string str)
        {
            return str.Replace("\r", "<br />");
        }

    }
}

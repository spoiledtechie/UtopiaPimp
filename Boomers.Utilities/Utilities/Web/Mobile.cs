using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Boomers.Utilities.Web
{
    public class Mobile
    {
        private static readonly Regex MOBILE_REGEX = new Regex(@"(nokia|sonyericsson|blackberry|samsung|sec-|windows ce|motorola|mot-|up.b)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// checks if the item is a mobile device.
        /// </summary>
        public static bool IsMobile
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context != null)
                {
                    HttpRequest request = context.Request;
                    if (request.Browser.IsMobileDevice)
                        return true;

                    if (!string.IsNullOrEmpty(request.UserAgent) && MOBILE_REGEX.IsMatch(request.UserAgent))
                        return true;
                }
                return false;
            }
        }
    }
}

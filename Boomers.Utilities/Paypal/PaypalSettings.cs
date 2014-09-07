using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Paypal
{
    public class PaypalSettings
    {

        public enum PaypalMode
        {
            test,
            live
        }
        public enum CurrencyCode
        {
            USD
        }
        public enum ReturnStatus
        { 
        Cancel,
            Success
        }
        /// <summary>
        /// gets the paypal base url depending on the mode.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static string GetBaseUrl(PaypalMode mode)
        {
            switch (mode)
            {
                case PaypalSettings.PaypalMode.test:
                    return "https://www.sandbox.paypal.com/cgi-bin/webscr";
                case PaypalSettings.PaypalMode.live:
                    return "https://www.paypal.com/cgi-bin/webscr";
                default:
                    return "https://www.sandbox.paypal.com/cgi-bin/webscr";
            }
        }
    }
}

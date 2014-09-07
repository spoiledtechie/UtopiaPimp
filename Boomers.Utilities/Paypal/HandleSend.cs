using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace Boomers.Paypal
{
    public class HandleSend
    {
        /// <summary>
        /// Compiles the paypal url so it can be response.Redirected to.
        /// </summary>

        private PaypalSettings.PaypalMode _mode;

        public PaypalSettings.PaypalMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        private string _sellerEmailAddress;

        public string SellerEmailAddress
        {
            get { return _sellerEmailAddress; }
            set { _sellerEmailAddress = value; }
        }

        private string _buyerEmailAddress;

        public string BuyerEmailAddress
        {
            get { return _buyerEmailAddress; }
            set { _buyerEmailAddress = value; }
        }

        private PaypalSettings.CurrencyCode _code;

        public PaypalSettings.CurrencyCode Code
        {
            get { return _code; }
            set { _code = value; }
        }
        private double _amount;

        public double Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }
        private string _itemName;

        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }
        private string _invoiceNumber;

        public string InvoiceNumber
        {
            get { return _invoiceNumber; }
            set { _invoiceNumber = value; }
        }
        private string _returnUrl;

        public string ReturnUrl
        {
            get { return _returnUrl; }
            set { _returnUrl = value; }
        }
        private string _cancelUrl;

        public string CancelUrl
        {
            get { return _cancelUrl; }
            set { _cancelUrl = value; }
        }
        private string _logoUrl;

        public string LogoUrl
        {
            get { return _logoUrl; }
            set { _logoUrl = value; }
        }
        /// <summary>
        /// Redirects and sends the compiled variables to paypal.
        /// </summary>
        /// <param name="mode"></param>
        public void SendToPaypal(PaypalSettings.PaypalMode mode)
        {
            HttpContext.Current.Response.Redirect(CompilePaypalUrl(mode));
        }
        /// <summary>
        /// Compiles the URL to paypal standard to get ready to send to paypal.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public string CompilePaypalUrl(PaypalSettings.PaypalMode mode)
        {
            StringBuilder url = new StringBuilder();

            url.Append(PaypalSettings.GetBaseUrl(mode) + "?" + "cmd=_xclick&business=" +
                  HttpUtility.UrlEncode(_sellerEmailAddress));

            if (_code != null)
                url.AppendFormat("&currency_code={0}", HttpUtility.UrlEncode(_code.ToString()));
            
            if (_buyerEmailAddress != null && _buyerEmailAddress != "")
                url.AppendFormat("&email={0}", HttpUtility.UrlEncode(_buyerEmailAddress));

            if (_amount != null)
                url.AppendFormat("&amount={0:f2}", _amount);

            if (LogoUrl != null && LogoUrl != "")
                url.AppendFormat("&image_url={0}", HttpUtility.UrlEncode(LogoUrl));

            if (_itemName != null && _itemName != "")
                url.AppendFormat("&item_name={0}", HttpUtility.UrlEncode(_itemName));

            if (_invoiceNumber != null && _invoiceNumber != "")
                url.AppendFormat("&invoice={0}", HttpUtility.UrlEncode(_invoiceNumber));

            if (_returnUrl != null)
                url.AppendFormat("&return={0}", HttpUtility.UrlEncode(_returnUrl));

            if (_cancelUrl != null)
                url.AppendFormat("&cancel_return={0}", HttpUtility.UrlEncode(_cancelUrl));

            return url.ToString();
        }
    }
}

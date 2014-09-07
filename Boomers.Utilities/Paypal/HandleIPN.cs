using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

using Boomers.Utilities.Communications;
using Boomers.Paypal;


/* This code does not account for some of the variables that are passed back from IPN.
 * Howevever, it does account for most of them. This code does not also account for all shopping
 * cart functionality as you will have to handle the multiple items yourself. In addition,
 * it does not handle refunds in anyway for IPN.
 * Thanks again for using the code and any changes you feel that are needed please email to
 * 
 * support@xdevsoftware.com
 *
 */

namespace Boomers.Paypal
{
    public class HandleIPN
    {
        string _txnID, _txnType, _paymentStatus, _receiverEmail, _itemName, _itemNumber, _quantity, _invoice, _custom,
    _paymentGross, _payerEmail, _pendingReason, _paymentDate, _paymentFee, _firstName, _lastName, _address,
    _city, _state, _zip, _country, _countryCode, _addressStatus, _payerStatus, _payerID, _paymentType, _notifyVersion,
    _verifySign, _response, _payerPhone, _payerBusinessName, _business, _receiverID, _memo, _tax, _qtyCartItems,
    _shippingMethod, _shipping;

        private string _postUrl = "";
        private string _strRequest = "";
        private string _smtpHost, _fromEmail, _toEmail, _fromEmailPassword, _businessWebsite, _fromEmailUserName;
        private int _smtpPort;
        /// <summary>
        /// valid strings are "TEST" for sandbox use 
        /// "LIVE" for production use
        /// </summary>
        /// <param name="mode"></param>
        public HandleIPN(PaypalSettings.PaypalMode mode)
        {
            //Boomers.Utilities.Documents.TextLogger.LogItem("paypalIPN", "getting base url");
            this.PostUrl = PaypalSettings.GetBaseUrl(mode);
            //Boomers.Utilities.Documents.TextLogger.LogItem("paypalIPN", "filling properties");
            this.FillIPNProperties();
            //Boomers.Utilities.Documents.TextLogger.LogItem("paypalIPN", "checking status");

        }

        public enum ResponseTypeEnum
        {
            VERIFIED,
            INVALID
        }

        public enum PaymentStatusEnum
        {
            Completed,
            Pending,
            Failed,
            Denied
        }

        /// <summary>
        /// This checks the status of the order and notifies you via email the status.
        /// </summary>
        public void CheckStatus(PaypalSettings.PaypalMode mode, string messageAddOnToBuyer)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(PaypalSettings.GetBaseUrl(mode));
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] bytes = HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.ContentLength);
            string str = Encoding.ASCII.GetString(bytes) + "&cmd=_notify-validate";
            request.ContentLength = str.Length;
            //Boomers.Utilities.Documents.TextLogger.LogItem("paypalIPN", "writing "+ str);
            StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
            writer.Write(str);
            writer.Close();
            //Boomers.Utilities.Documents.TextLogger.LogItem("paypalIPN", "reading string");
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            
            _response= reader.ReadToEnd();
            //Boomers.Utilities.Documents.TextLogger.LogItem("paypalIPN", "read " + responseType);
            reader.Close();

            //Boomers.Utilities.Documents.TextLogger.LogItem("paypalIPN", this.PaymentStatus +" " +this.TXN_Type +" "+ this.ReceiverEmail+" " + this.ToEmail);
            switch (_response)
            {
                case "VERIFIED":
                    switch (this.PaymentStatus)
                    {
                        case "Completed":
                            if (this.ReceiverEmail == this.ToEmail)
                            {
                                switch (this.TXN_Type)
                                {
                                    case "cart":
                                        this.EmailSeller("PayPal: Successful Order from Cart", this.FromEmail, this.BusinessWebsite);
                                        break;
                                    case "express_checkout":
                                        this.EmailSeller("PayPal: Successful Order from Express Checkout", this.FromEmail, this.BusinessWebsite);
                                        break;
                                    case "send_money":
                                        this.EmailSeller("PayPal: Successful Order from Send Money", this.FromEmail, this.BusinessWebsite);
                                        break;
                                    case "virtual_terminal":
                                        this.EmailSeller("PayPal: Successful Order from Virtual Terminal", this.FromEmail, this.BusinessWebsite);
                                        break;
                                    case "web_accept":
                                        this.EmailSeller("PayPal: Successful Order from Web_Accept", this.FromEmail, this.BusinessWebsite);
                                        break;
                                    default:
                                        this.EmailSeller("PayPal: Order has been placed", this.FromEmail, this.BusinessWebsite);
                                        break;
                                }
                            }
                            else
                            {
                                this.EmailSeller("PayPal: Unknown order...please check your paypal account", this.FromEmail, this.BusinessWebsite);
                            }
                            break;
                        case "Pending":
                            switch (this.PendingReason)
                            {
                                case "address":
                                    this.EmailSeller("PayPal: Pending Order because of address", this.FromEmail, this.BusinessWebsite);
                                    break;
                                case "authorization":
                                    this.EmailSeller("PayPal: Pending Order because of authorization", this.FromEmail, this.BusinessWebsite);
                                    break;
                                case "echeck":
                                    this.EmailSeller("PayPal: Pending Order because of echeck", this.FromEmail, this.BusinessWebsite);
                                    break;
                                case "intl":
                                    this.EmailSeller("PayPal: Pending Order because of non-US Acccount", this.FromEmail, this.BusinessWebsite);
                                    break;
                                case "multi-currency":
                                    this.EmailSeller("PayPal: Pending Order because of multi-currency", this.FromEmail, this.BusinessWebsite);
                                    break;
                                case "unilateral":
                                    this.EmailSeller("PayPal: Pending Order because of Unilateral", this.FromEmail, this.BusinessWebsite);
                                    break;
                                case "upgrade":
                                    this.EmailSeller("PayPal: Pending Order because of Upgrade", this.FromEmail, this.BusinessWebsite);
                                    break;
                                case "verify":
                                    this.EmailSeller("PayPal: Pending Order because of Verification needed", this.FromEmail, this.BusinessWebsite);
                                    break;
                                case "other":
                                    this.EmailSeller("PayPal: Pending Order because of other reason", this.FromEmail, this.BusinessWebsite);
                                    break;
                                default:
                                    this.EmailSeller(string.Format("PayPal: Pending Order because of unknown reason of {0}", this.PendingReason), this.FromEmail, this.BusinessWebsite);
                                    break;
                            }
                            break;
                        case "Failed":
                            this.EmailSeller("PayPal: Failed order", this.FromEmail, this.BusinessWebsite);
                            break;
                        case "Denied":
                            this.EmailSeller("PayPal: Denied order", this.FromEmail, this.BusinessWebsite);
                            break;
                        default:
                            this.EmailSeller("PayPal: ERROR, response is " + this.Response, this.FromEmail, this.BusinessWebsite);
                            break;
                    }

                    this.EmailBuyer("Order Received", "Your order has been received for " + this.BusinessWebsite + " and will begin processing shortly.<br/><br/>" + messageAddOnToBuyer, this.BusinessWebsite);

                    break;
                case "INVALID":
                    this.EmailSeller("PayPal: Invalid order, please review and investigate", this.FromEmail, this.BusinessWebsite);
                    break;
                default:
                    this.EmailSeller("PayPal: ERROR, response is " + this.Response, this.FromEmail, this.BusinessWebsite);
                    break;
            }
        }

        private void EmailBuyer(string subject, string emailBody, string companyName)
        {
            Email.EmailPassword = this.FromEmailPassword;
            Email.EmailUsername = this.FromEmailUserName;
            Email.FromEmail = this.FromEmail;
            Email.Port = this.SmtpPort;
            Email.SmtpServer = this.SmtpHost;

            string message = Boomers.Utilities.Communications.Brief.CreateBriefBodyWithAdvertisement(emailBody);

            Email.SendEmail(false, this.FromEmail, this.PayerEmail, subject, message, companyName, true);

        }


        private void EmailSeller(string subject, string toEmail, string companyName)
        {

            string emailBody = "<br />"
              + "Transaction ID: " + this.TXN_ID + "<br />"
              + "Transaction Type:" + this.TXN_Type + "<br />"
              + "Payment Type: " + this.PaymentType + "<br />"
              + "Payment Status: " + this.PaymentStatus + "<br />"
              + "Pending Reason: " + this.PendingReason + "<br />"
              + "Payment Date: " + this.PaymentDate + "<br />"
              + "Receiver Email: " + this.ReceiverEmail + "<br />"
              + "Invoice: " + this.Invoice + "<br />"
              + "Item Number: " + this.ItemNumber + "<br />"
              + "Item Name: " + this.ItemName + "<br />"
              + "Quantity: " + this.Quantity + "<br />"
              + "Custom: " + this.Custom + "<br />"
              + "Payment Gross: " + this.PaymentGross + "<br />"
              + "Payment Fee: " + this.PaymentFee + "<br />"
              + "Payer Email: " + this.PayerEmail + "<br />"
              + "First Name: " + this.PayerFirstName + "<br />"
              + "Last Name: " + this.PayerLastName + "<br />"
              + "Street Address: " + this.PayerAddress + "<br />"
              + "City: " + this.PayerCity + "<br />"
              + "State: " + this.PayerState + "<br />"
              + "Zip Code: " + this.PayerZipCode + "<br />"
              + "Country: " + this.PayerCountry + "<br />"
              + "Address Status: " + this.PayerAddressStatus + "<br />"
              + "Payer Status: " + this.PayerStatus + "<br />"
              + "Verify Sign: " + this.VerifySign + "<br />"
              + "Notify Version: " + this.NotifyVersion + "<br />";

            Email.EmailPassword = this.FromEmailPassword;
            Email.EmailUsername = this.FromEmailUserName;
            Email.FromEmail = this.FromEmail;
            Email.Port = this.SmtpPort;
            Email.SmtpServer = this.SmtpHost;

            string message = Boomers.Utilities.Communications.Brief.CreateBriefBodyWithAdvertisement(emailBody);

            Email.SendEmail(false, this.FromEmail, toEmail, subject, message, companyName, true);
        }




        /// <summary>
        /// This is the website address of the business.  Mainly used to stuff into email.
        /// </summary>
        public string BusinessWebsite
        {
            get { return _businessWebsite; }
            set { _businessWebsite = value; }
        }

        public string PostUrl
        {
            get { return _postUrl; }
            set { _postUrl = value; }
        }

        /// <summary>
        /// This is the reponse back from the http post back to PayPal.
        /// Possible values are "VERIFIED" or "INVALID"
        /// </summary>
        public string Response
        {
            get { return _response; }
            set { _response = value; }
        }

        public string RequestLength
        {
            get { return _strRequest; }
            set { _strRequest = value; }
        }

        /// <summary>
        /// Provide your outgoing email server to use are your SMTP host
        /// </summary>
        public string SmtpHost
        {
            get { return _smtpHost; }
            set { _smtpHost = value; }
        }

        /// <summary>
        /// Provide the port your outgoing SMTP host uses
        /// </summary>
        public int SmtpPort
        {
            get { return _smtpPort; }
            set { _smtpPort = value; }
        }

        /// <summary>
        /// this is the email specifically for the username for the smtp server.
        /// static could be "email"
        /// </summary>
        public string FromEmailUserName
        {
            get { return _fromEmailUserName; }
            set { _fromEmailUserName = value; }
        }

        /// <summary>
        /// This is the email address that will show to the customer and you. This most likely
        /// needs to be a valid email address that your SMTP server will accept
        /// Examples would be something like no-reply@yourdomain.com
        /// </summary>
        public string FromEmail
        {
            get { return _fromEmail; }
            set { _fromEmail = value; }
        }

        /// <summary>
        /// This is the password that the FromEmail property will use. This needs to be the password
        /// for the email account itself
        /// </summary>
        public string FromEmailPassword
        {
            get { return _fromEmailPassword; }
            set { _fromEmailPassword = value; }
        }

        /// <summary>
        /// This is the email address that you use for yourself. This should be set to
        /// the email that is registered for your PayPal account.
        /// </summary>
        public string ToEmail
        {
            get { return _toEmail; }
            set { _toEmail = value; }
        }

        /// <summary>
        /// Email address or Account ID of the payment recipient.  This is equivalent
        ///  to the value of receiver_email if the payment is sent to the primary account
        /// , which is most cases it is.  This value is that value of what is set in the button html
        /// markup.  This value also get normalized to lowercase when coming back from PayPal
        /// </summary>
        public string Business
        {
            get { return _business; }
            set { _business = value; }
        }


        /// <summary>
        /// Unique transaction ID generated by PayPal. Helpful to use for checking
        ///  against fraud to make sure the transaction hasn't already occured.
        /// </summary>
        public string TXN_ID
        {
            get { return _txnID; }
            set { _txnID = value; }
        }

        /// <summary>
        /// Type of transaction from the customer. Possible values are
        /// "cart", "express_checkout", "send_money", "virtual_terminal", "web-accept"
        /// </summary>
        public string TXN_Type
        {
            get { return _txnType; }
            set { _txnType = value; }
        }

        /// <summary>
        /// This is the status of the payment from the Customer.Possible values are: 
        /// "Canceled_Reversal", "Completed", "Denied", "Expired", "Failed", "Pending",
        ///  "Processed", "Refunded", "Reversed", "Voided"
        /// </summary>
        public string PaymentStatus
        {
            get { return _paymentStatus; }
            set { _paymentStatus = value; }
        }

        /// <summary>
        /// Primary email address of you, the recipient, of the payment.
        /// </summary>
        public string ReceiverEmail
        {
            get { return _receiverEmail; }
            set { _receiverEmail = value; }
        }

        /// <summary>
        /// unique account ID of the payment recipient, which is most likely yourself.
        /// </summary>
        public string ReceiverID
        {
            get { return _receiverID; }
            set { _receiverID = value; }
        }

        /// <summary>
        /// This is the item name passed by yourself or if the customer if you let them enter in an item name
        /// </summary>
        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }

        /// <summary>
        /// This is the item number you set for your own tracking purposes. It is not required by PayPal
        /// so if you didn't set it most likely will come back blank.
        /// </summary>
        public string ItemNumber
        {
            get { return _itemNumber; }
            set { _itemNumber = value; }
        }

        /// <summary>
        /// Quantity of the item ordered by the customer
        /// </summary>
        public string Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        /// <summary>
        /// Quantity of the items in the shopping cart from the Customer
        /// </summary>
        public string QuantityCartItems
        {
            get { return _qtyCartItems; }
            set { _qtyCartItems = value; }
        }

        /// <summary>
        /// Invoice number passed by yourself, if you didn't pass it to PayPal then this is omitted.
        /// </summary>
        public string Invoice
        {
            get { return _invoice; }
            set { _invoice = value; }
        }

        /// <summary>
        /// Custom value passed by yourself with the item.
        /// </summary>
        public string Custom
        {
            get { return _custom; }
            set { _custom = value; }
        }

        /// <summary>
        /// Memo entered in by the customer on PayPal website note field
        /// </summary>
        public string Memo
        {
            get { return _memo; }
            set { _memo = value; }
        }

        /// <summary>
        /// Amount of tax charged on the payment
        /// </summary>
        public string Tax
        {
            get { return _tax; }
            set { _tax = value; }
        }

        /// <summary>
        /// Full USD amount of customer's payment before the PayPal fee is subtracted
        /// </summary>
        public string PaymentGross
        {
            get { return _paymentGross; }
            set { _paymentGross = value; }
        }

        /// <summary>
        /// Date Time stamp created by PayPal in the following format: 
        /// HH:MM:SS DD Mmm YY, YYYY PST
        /// </summary>
        public string PaymentDate
        {
            get { return _paymentDate; }
            set { _paymentDate = value; }
        }

        /// <summary>
        /// PayPal's transaction fees associated with purchase.
        /// </summary>
        public string PaymentFee
        {
            get { return _paymentFee; }
            set { _paymentFee = value; }
        }


        /// <summary>
        /// This is the email that the customer used on PayPal or that
        /// is registered with PayPal
        /// </summary>
        public string PayerEmail
        {
            get { return _payerEmail; }
            set { _payerEmail = value; }
        }

        /// <summary>
        /// Customer's phone number
        /// </summary>
        public string PayerPhone
        {
            get { return _payerPhone; }
            set { _payerPhone = value; }
        }

        /// <summary>
        /// Customer's company name if they represent a business
        /// </summary>
        public string PayerBusinessName
        {
            get { return _payerBusinessName; }
            set { _payerBusinessName = value; }
        }

        /// <summary>
        /// This variable is only set if the payment_status=Pending. Possible values are the following:
        /// "address", "authorization", "echeck", "intl", "multi-currency", "unilateral", "upgrade",
        ///  "verify", other"
        /// </summary>
        public string PendingReason
        {
            get { return _pendingReason; }
            set { _pendingReason = value; }
        }

        /// <summary>
        /// This is indicated from what is set in your PayPal profile settings
        /// </summary>
        public string ShippingMethod
        {
            get { return _shippingMethod; }
            set { _shippingMethod = value; }
        }

        /// <summary>
        /// Shipping charges associated with the order.
        /// </summary>
        public string Shipping
        {
            get { return _shipping; }
            set { _shipping = value; }
        }

        /// <summary>
        /// Customer's First Name
        /// </summary>
        public string PayerFirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        /// <summary>
        /// Customer's Last Name
        /// </summary>
        public string PayerLastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        /// <summary>
        /// Customer's street address
        /// </summary>
        public string PayerAddress
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        /// Customer's city
        /// </summary>
        public string PayerCity
        {
            get { return _city; }
            set { _city = value; }
        }

        /// <summary>
        /// Customer state of address
        /// </summary>
        public string PayerState
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// Zip code of customer's address
        /// </summary>
        public string PayerZipCode
        {
            get { return _zip; }
            set { _zip = value; }
        }

        /// <summary>
        /// Customer's country
        /// </summary>
        public string PayerCountry
        {
            get { return _country; }
            set { _country = value; }
        }

        /// <summary>
        /// Customer's 2 character country code
        /// </summary>
        public string PayerCountryCode
        {
            get { return _countryCode; }
            set { _countryCode = value; }
        }

        /// <summary>
        /// The the address provided is either confirmed or uncomfirmed from PayaPal. Possible values  from PayPal
        /// are going to be "confirmed" or "unconfirmed"
        /// </summary>
        public string PayerAddressStatus
        {
            get { return _addressStatus; }
            set { _addressStatus = value; }
        }

        /// <summary>
        /// Customer either had a verified or unverified account with PayPal. 
        /// Possible return values from PayPal are "verified" or "unverified"
        /// </summary>
        public string PayerStatus
        {
            get { return _payerStatus; }
            set { _payerStatus = value; }
        }

        /// <summary>
        /// Customer's unique ID
        /// </summary>
        public string PayerID
        {
            get { return _payerID; }
            set { _payerID = value; }
        }

        /// <summary>
        /// Type of payment from Customer. Possible values from PayPal are "echeck" and "instant"
        /// </summary>
        public string PaymentType
        {
            get { return _paymentType; }
            set { _paymentType = value; }
        }

        /// <summary>
        /// This is the version number of the IPN that makes the post.
        /// </summary>
        public string NotifyVersion
        {
            get { return _notifyVersion; }
            set { _notifyVersion = value; }
        }

        /// <summary>
        /// An encrypted string that is used to validate the transaction. You don't have to use this for anything
        ///  unless you want to keep it and store it for your records.
        /// </summary>
        public string VerifySign
        {
            get { return _verifySign; }
            set { _verifySign = value; }
        }

        private void FillIPNProperties()
        {
            this.RequestLength = HttpContext.Current.Request.Form.ToString();
            this.PayerCity = HttpContext.Current.Request.Form["address_city"];
            this.PayerCountry = HttpContext.Current.Request.Form["address_country"];
            this.PayerCountryCode = HttpContext.Current.Request.Form["address_country_code"];
            this.PayerState = HttpContext.Current.Request.Form["address_state"];
            this.PayerAddressStatus = HttpContext.Current.Request.Form["address_status"];
            this.PayerAddress = HttpContext.Current.Request.Form["address_street"];
            this.PayerZipCode = HttpContext.Current.Request.Form["address_zip"];
            this.PayerFirstName = HttpContext.Current.Request.Form["first_name"];
            this.PayerLastName = HttpContext.Current.Request.Form["last_name"];
            this.PayerBusinessName = HttpContext.Current.Request.Form["payer_business_name"];
            this.PayerEmail = HttpContext.Current.Request.Form["payer_email"];
            this.PayerID = HttpContext.Current.Request.Form["payer_id"];
            this.PayerStatus = HttpContext.Current.Request.Form["payer_status"];
            this.PayerPhone = HttpContext.Current.Request.Form["contact_phone"];
            this.Business = HttpContext.Current.Request.Form["business"];
            this.ItemName = HttpContext.Current.Request.Form["item_name"];
            this.ItemNumber = HttpContext.Current.Request.Form["item_number"];
            this.Quantity = HttpContext.Current.Request.Form["quantity"];
            this.ReceiverEmail = HttpContext.Current.Request.Form["receiver_email"];
            this.ReceiverID = HttpContext.Current.Request.Form["receiver_id"];
            this.Custom = HttpContext.Current.Request.Form["custom"];
            this.Memo = HttpContext.Current.Request.Form["memo"];
            this.Invoice = HttpContext.Current.Request.Form["invoice"];
            this.Tax = HttpContext.Current.Request.Form["tax"];
            this.QuantityCartItems = HttpContext.Current.Request.Form["num_cart_items"];
            this.PaymentDate = HttpContext.Current.Request.Form["payment_date"];
            this.PaymentStatus = HttpContext.Current.Request.Form["payment_status"];
            this.PaymentType = HttpContext.Current.Request.Form["payment_type"];
            this.PendingReason = HttpContext.Current.Request.Form["pending_reason"];
            this.TXN_ID = HttpContext.Current.Request.Form["txn_id"];
            this.TXN_Type = HttpContext.Current.Request.Form["txn_type"];
            this.PaymentFee = HttpContext.Current.Request.Form["mc_fee"];
            this.PaymentGross = HttpContext.Current.Request.Form["mc_gross"];
            this.NotifyVersion = HttpContext.Current.Request.Form["notify_version"];
            this.VerifySign = HttpContext.Current.Request.Form["verify_sign"];
        }
    }
}

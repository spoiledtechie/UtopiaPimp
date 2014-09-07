using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Web;
using System.ComponentModel;
using Boomers.Config;

namespace Boomers.Utilities.Communications
{
    public class Email
    {
        public delegate void BoomerSendEmailAsync(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
        public static event BoomerSendEmailAsync OnEmailSendAsync;
        /// <summary>
        /// GoDaddy: relay-hosting.secureserver.net
        /// </summary>
        public static string SmtpServer { get; set; }
        public static string FromEmail { get; set; }
        public static string EmailUsername { get; set; }
        public static string EmailPassword { get; set; }
        public static int Port { get; set; }

        private static string showAdvertisments()
        {
            StringBuilder sb = new StringBuilder();
            BoomersSites sites = new BoomersSites();
            var site = sites.getRandomSite();
            sb.Append("<div style=\"font: 11px verdana, arial\">");
            sb.Append("<b>Advertisement</b><br/>");
            sb.Append("<a href='" + site.Url + "'>" + site.Name + "</a>: " + site.Description +"<br/>");
            sb.Append("</div>");
            return sb.ToString();

        }
        public static bool SendGmail(bool showAdvert, string toName, string toUserEmail, string subject, string message, string companyName)
        {
            return SendGmail(showAdvert, toName, toUserEmail, subject, message, companyName, false);
        }
        public static bool SendGmail( bool showAdvert,string toName, string toUserEmail, string subject, string message, string companyName, bool sendAsync)
        {
            try
            {
                using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
                {
                    mail.From = new MailAddress(FromEmail);
                    mail.To.Add(new MailAddress(toUserEmail));
                    mail.Subject = companyName + ": " + subject;

                    mail.Body = "<div style=\"font: 11px verdana, arial\">";
                    mail.Body += message + "<br /><br />";

                    if (showAdvert)
                        mail.Body += showAdvertisments();
                    mail.Body += "<hr /><br />";
                    mail.Body += "<h3>Author information</h3>";
                    mail.Body += "<div style=\"font-size:10px;line-height:16px\">";
                    mail.Body += "<strong>Name:</strong> " + HttpContext.Current.Server.HtmlEncode(toName) + "<br />";
                    mail.Body += "<strong>E-mail:</strong> " + HttpContext.Current.Server.HtmlEncode(FromEmail) + "<br />";

                    if (HttpContext.Current != null)
                    {
                        mail.Body += "<strong>IP address:</strong> " + HttpContext.Current.Request.UserHostAddress + "<br />";
                        mail.Body += "<strong>User-agent:</strong> " + HttpContext.Current.Request.UserAgent;
                    }
                    SendEmailMessageGmail(mail);
                }

                return true;
            }
            catch
            { return false; }
        }
        public static bool SendEmail(bool showAdvert, string userName, string toUserEmail, string subject, string message, string companyName, bool requiresCredentials)
        {
            return SendEmail(showAdvert, userName, toUserEmail, subject, message, companyName, requiresCredentials, false);
        }
        public static bool SendEmail(bool showAdvert, string userName, string toUserEmail, string subject, string message, string companyName, bool requiresCredentials, bool sendAsync)
        {
            try
            {
                using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
                {
                    mail.From = new MailAddress(FromEmail);
                    mail.To.Add(new MailAddress(toUserEmail));
                    mail.Subject = companyName + ": " + subject;

                    mail.Body = "<div style=\"font: 11px verdana, arial\">";
                    mail.Body += message + "<br /><br />";
                    if (showAdvert)
                        mail.Body += showAdvertisments();
                    mail.Body += "<hr /><br />";
                    mail.Body += "<h3>Author information</h3>";
                    mail.Body += "<div style=\"font-size:10px;line-height:16px\">";
                    mail.Body += "<strong>Name:</strong> " + HttpContext.Current.Server.HtmlEncode(userName) + "<br />";
                    mail.Body += "<strong>E-mail:</strong> " + HttpContext.Current.Server.HtmlEncode(FromEmail) + "<br />";

                    if (HttpContext.Current != null)
                    {
                        mail.Body += "<strong>IP address:</strong> " + HttpContext.Current.Request.UserHostAddress + "<br />";
                        mail.Body += "<strong>User-agent:</strong> " + HttpContext.Current.Request.UserAgent;
                    }


                    if (requiresCredentials == false)
                        SendEmailMessage(mail, sendAsync);
                    else
                        SendEmailMessageLoginRequired(mail, sendAsync);
                }

                return true;
            }
            catch
            { return false; }
        }
        public static bool SendEmail(bool showAdvert, string userName, string toUserEmail, string subject, string message, string companyName, bool requiresCredentials, byte[] attachment, string attachExtension, string attachName)
        {
            return SendEmail(showAdvert, userName, toUserEmail, subject, message, companyName, requiresCredentials, attachment, attachExtension, attachName, false);
        }
        public static bool SendEmail(bool showAdvert, string userName, string toUserEmail, string subject, string message, string companyName, bool requiresCredentials, byte[] attachment, string attachExtension, string attachName, bool sendAsync)
        {
            try
            {
                using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
                {
                    mail.From = new MailAddress(FromEmail);
                    mail.To.Add(new MailAddress(toUserEmail));
                    mail.Subject = companyName + ": " + subject;

                    //save the data to a memory stream
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(attachment);
                    //create the attachment from a stream. Be sure to name the data with a file and
                    //media type that is respective of the data
                    switch (attachExtension)
                    {
                        case "xls":
                            mail.Attachments.Add(new System.Net.Mail.Attachment(ms, attachName + ".xls", "application/vnd.ms-excel"));
                            break;
                        case "doc":
                            mail.Attachments.Add(new System.Net.Mail.Attachment(ms, attachName + ".doc", "application/msword"));
                            break;
                    }

                    mail.Body = "<div style=\"font: 11px verdana, arial\">";
                    mail.Body += message + "<br /><br />";
                    if (showAdvert)
                        mail.Body += showAdvertisments();
                    mail.Body += "<hr /><br />";
                    mail.Body += "<h3>Author information</h3>";
                    mail.Body += "<div style=\"font-size:10px;line-height:16px\">";
                    mail.Body += "<strong>Name:</strong> " + HttpContext.Current.Server.HtmlEncode(userName) + "<br />";
                    mail.Body += "<strong>E-mail:</strong> " + HttpContext.Current.Server.HtmlEncode(FromEmail) + "<br />";

                    if (HttpContext.Current != null)
                    {
                        mail.Body += "<strong>IP address:</strong> " + HttpContext.Current.Request.UserHostAddress + "<br />";
                        mail.Body += "<strong>User-agent:</strong> " + HttpContext.Current.Request.UserAgent;
                    }
                    if (requiresCredentials == false)
                        SendEmailMessage(mail, sendAsync);
                    else
                        SendEmailMessageLoginRequired(mail, sendAsync);
                }

                return true;
            }
            catch
            { return false; }
        }
        /// <summary>
        /// Sends a MailMessage object using the SMTP settings.
        /// </summary>
        private static void SendEmailMessage(System.Net.Mail.MailMessage message, bool sendAsync)
        {
            message.IsBodyHtml = true;
            message.BodyEncoding = Encoding.UTF8;
            SmtpClient smtp = new SmtpClient(SmtpServer);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Port = 25;

            if (!sendAsync)
                smtp.Send(message);
            else
            {
                string token = Guid.NewGuid().ToString();
                smtp.SendCompleted += smtp_SendCompleted;
                smtp.SendAsync(message, token);
            }
        }

        //private static void SendEmailMessage(System.Net.Mail.MailMessage message, bool sendAsync)
        //{
        //    message.IsBodyHtml = true;
        //    message.BodyEncoding = Encoding.UTF8;
        //    SmtpClient smtp = new SmtpClient(SmtpServer);
        //    smtp.UseDefaultCredentials = false;
        //    smtp.Credentials = new System.Net.NetworkCredential(EmailUsername, EmailPassword);
        //    smtp.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
        //    smtp.Port = 25;
        //    smtp.Send(message);



        //}
        static void smtp_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (OnEmailSendAsync != null)
                OnEmailSendAsync(sender, e);
        }


        private static void SendEmailMessageLoginRequired(System.Net.Mail.MailMessage message, bool sendAsync)
        {
            message.IsBodyHtml = true;
            message.BodyEncoding = Encoding.UTF8;
            SmtpClient smtp = new SmtpClient(SmtpServer);
            smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new System.Net.NetworkCredential(EmailUsername, EmailPassword);
            smtp.Port = 25;

            if (!sendAsync)
                smtp.Send(message);
            else
            {
                string token = Guid.NewGuid().ToString();
                smtp.SendCompleted += smtp_SendCompleted;
                smtp.SendAsync(message, token);
            }
        }
        private static void SendEmailMessageGmail(System.Net.Mail.MailMessage message)
        {
            message.IsBodyHtml = true;
            message.BodyEncoding = Encoding.UTF8;
            System.Net.NetworkCredential cred = new System.Net.NetworkCredential(EmailUsername, EmailPassword);
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = cred;
            smtp.Port = 587;
          
                smtp.Send(message);
                  }
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Collections.ObjectModel;
using System.Net.Mail;
// For the Email Server to be sent once Submit has been clicked.

public class EmailGlobal
{
    public static string IPAddress()
    {
        settings.LoadSettings();
        return settings.emailserver;
    }

    public static string EmailAddress()
    {
        settings.LoadSettings();
        return settings.defaultemail;
    }

    public static bool SendEmail(string email, string name, string subject, string message)
    {
        try
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(EmailAddress());
                mail.ReplyTo = new MailAddress(EmailAddress());
                mail.Sender = mail.ReplyTo;

                mail.Subject = settings.companyname + ": " + subject;

                mail.Body = "<div style=\"font: 11px verdana, arial\">";
                mail.Body += HttpContext.Current.Server.HtmlEncode(message).Replace("\n", "<br />") + "<br /><br />";
                mail.Body += "<hr /><br />";
                mail.Body += "<h3>Author information</h3>";
                mail.Body += "<div style=\"font-size:10px;line-height:16px\">";
                mail.Body += "<strong>Name:</strong> " + HttpContext.Current.Server.HtmlEncode(name) + "<br />";
                mail.Body += "<strong>E-mail:</strong> " + HttpContext.Current.Server.HtmlEncode(email) + "<br />";

                //if (ViewState["url"] != null)
                //    mail.Body += string.Format("<strong>Website:</strong> <a href=\"{0}\">{0}</a><br />", ViewState["url"]);

                //if (ViewState["country"] != null)
                //    mail.Body += "<strong>Country code:</strong> " + ((string)ViewState["country"]).ToUpperInvariant() + "<br />";

                if (HttpContext.Current != null)
                {
                    mail.Body += "<strong>IP address:</strong> " + HttpContext.Current.Request.UserHostAddress + "<br />";
                    mail.Body += "<strong>User-agent:</strong> " + HttpContext.Current.Request.UserAgent;
                }

                //if (txtAttachment.HasFile)
                //{
                //    Attachment attachment = new Attachment(txtAttachment.PostedFile.InputStream, txtAttachment.FileName);
                //    mail.Attachments.Add(attachment);
                //}

                SendMailMessage(mail);
            }

            return true;
        }
        catch
        { return false; }
    }

    /// <summary>
    /// Sends a MailMessage object using the SMTP settings.
    /// </summary>
    private static void SendMailMessage(MailMessage message)
    {
        if (message == null)
            throw new ArgumentNullException("message");

        try
        {
            message.IsBodyHtml = true;
            message.BodyEncoding = Encoding.UTF8;
            SmtpClient smtp = new SmtpClient(settings.emailserver);
            smtp.UseDefaultCredentials = true;
            smtp.Send(message);
            OnEmailSent(message);
        }
        catch (SmtpException)
        {
            OnEmailFailed(message);
        }
        finally
        {
            // Remove the pointer to the message object so the GC can close the thread.
            message.Dispose();
            message = null;
        }
    }

    /// <summary>
    /// Occurs after an e-mail has been sent. The sender is the MailMessage object.
    /// </summary>
    private static event EventHandler<EventArgs> EmailSent;
    private static void OnEmailSent(MailMessage message)
    {
        if (EmailSent != null)
        {
            EmailSent(message, new EventArgs());
        }
    }

    /// <summary>
    /// Occurs after an e-mail has been sent. The sender is the MailMessage object.
    /// </summary>
    private static event EventHandler<EventArgs> EmailFailed;
    private static void OnEmailFailed(MailMessage message)
    {
        if (EmailFailed != null)
        {
            EmailFailed(message, new EventArgs());
        }
    }

}

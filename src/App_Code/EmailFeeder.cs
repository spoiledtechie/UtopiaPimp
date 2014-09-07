using System;
using System.Collections.Generic;
using System.Web.Services;
using App_Code.CS_Code.Worker;
using Boomers.Utilities.Communications;
using SupportFramework.Data;

namespace App_Code
{
    /// <summary>
    /// Summary description for EmailFeeder
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class EmailFeeder : WebService
    {
        [WebMethod]
        public bool PullEmails(Guid token)
        {
            var pimpToken = new Guid("b17ec769-4440-4b85-a628-71f46d13f6ec");
            if (pimpToken != token)
            {
                //var exception = new ArgumentException();
                //exception.Message = "Invalid token";
                //exception.Source = "App_Code/EmailFeeder.cs";
                //Errors.logError(exception);
                return false;
            }

            try
            {
                var emails = NotifierWorker.GetEmailMessages();
                if (emails == null)
                    return true;


                Email.EmailPassword = "email123";
                Email.EmailUsername = "email";
                Email.FromEmail = "team@utopiapimp.com";
                Email.Port = 25;
                Email.SmtpServer = "localhost";

                foreach (var email in emails)
                {
                    string message = Boomers.Utilities.Communications.Brief.CreateBriefBodyWithAdvertisement(email.Message, "http://www.utopiapimp.com/members/NotifierPreferences.aspx", "Do you want to change your preferences?");
                    Email.SendEmail(true,email.ToName, email.ToEmail, email.Subject, message, email.CompanyName, true);
                                    }
                if (emails.Count > 0)
                    Email.SendEmail(true, "team@utopiapimp.com", "team@utopiapimp.com", "Notifier is Still Running", Boomers.Utilities.Communications.Brief.CreateBriefBodyWithAdvertisement("Sent " + emails.Count + " notifier emails at " + DateTime.UtcNow), "UtopiaPimp", true);


                return true;
            }
            catch (Exception e)
            {
                Errors.logError(e);
                return false;
            }
        }
    }
}

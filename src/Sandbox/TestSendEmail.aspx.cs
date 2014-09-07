using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Boomers.Utilities.Communications;

public partial class Sandbox_TestSendEmail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Email.EmailPassword = "email123";
        Email.EmailUsername = "email";
        Email.FromEmail = "team@utopiapimp.com";
        Email.Port = 25;
        Email.SmtpServer = "localhost";
        string body = "January 18 of YR4 The blasted dog of WD Discover (6:11) has attacked you my lord, he took 171 acers.<br />January 18 of YR4 The blasted dog of Kuruksetra (6:11) has attacked you my lord, he took 171 acers.";
        Email.SendGmail(true,"spoiledtechie", "spoiledtechie@gmail.com", "What do you think??", Brief.CreateBriefBodyWithAdvertisement(body, "http://www.utopiashrimp.com/members/NotifierPreferences.aspx", "If you want to change your email preferences, please "), "Utopiapimp", true);
    }
}
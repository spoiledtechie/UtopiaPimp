using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Boomers.Utilities.Communications;
using System.Collections.Generic;
using System.Text;

using Pimp.UCache;
using Pimp;
using SupportFramework.Users;
using Pimp.Users;
using Pimp.Utopia;
using Pimp.UData;

public partial class members_Profile : MyBasePageCS
{
    PimpUserWrapper pimpUser;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            pimpUser = new PimpUserWrapper();

            ddlChangeTheme.SelectedValue = Page.Theme;
            lblCurrentTheme.Text = Page.Theme;
            lblChangeEmail.Text = "Current Email: " + SupportFramework.Users.Memberships.getUserEmail();

            StringBuilder idNumberBuilder = new StringBuilder();
            idNumberBuilder.AppendFormat("Current province ID: {0}<br />", pimpUser.PimpUser.CurrentActiveProvince.ToString());
            idNumberBuilder.AppendFormat("Current kingdom ID: {0}", pimpUser.PimpUser.StartingKingdom.ToString());
            idNumbers.Text = idNumberBuilder.ToString();
        }
    }
    protected void ddlChangeTheme_SelectedIndexChanged(object sender, EventArgs e)
    {
        Profile.ThemePreference = ddlChangeTheme.SelectedValue;
        Profile.Save();
        Response.Redirect(Page.Request.Url.AbsoluteUri);
    }
    protected void btnChangeUserName_Click(object sender, EventArgs e)
    {
        string email = Memberships.getUserEmail();
        bool changed = Memberships.changeUserName(Memberships.getUserID(), tbChangeUserName.Text.Trim());
        if (changed)
        {
            lblChangeUserName.Text = "Username changed to '" + tbChangeUserName.Text + "', an email has been sent to " + email;

            Email.EmailPassword = "email123";
            Email.EmailUsername = "email";
            Email.SmtpServer = "localhost";
            Email.FromEmail = "team@utopiapimp.com";
            Email.SendEmail(true, tbChangeUserName.Text, email, "Your Username has been changed", tbChangeUserName.Text + ", <br/><br/> You have recently changed your Username on the Site Utopiapimp.com. This email is to keep for your records. <br/><br/>New Username: " + tbChangeUserName.Text, "Utopiapimp.com", true);

        }
        else
            lblChangeUserName.Text = "Looks like that username already exists, please try again.";

        Response.Redirect(Page.Request.Url.AbsoluteUri);
    }
    protected void btnChangeEmail_Click(object sender, EventArgs e)
    {
        PimpUserWrapper pimpUser = new PimpUserWrapper();

        string oldEmail = Memberships.getUserEmail();
        string newEmail = tbChangeEmail.Text.Trim();


        lblChangeEmail.Text = "User Email changed to '" + newEmail + "' from '" + oldEmail + "', an email has been sent to both email addresses.";
        Email.EmailPassword = "email123";
        Email.EmailUsername = "email";
        Email.SmtpServer = "localhost";
        Email.FromEmail = "team@utopiapimp.com";
        Email.SendEmail(true, pimpUser.PimpUser.UserName, oldEmail, "Your email has been changed", pimpUser.PimpUser.UserName + ", <br/><br/> You have recently changed your Email on the Site Utopiapimp.com. This email is to keep for your records. <br/><br/> Old Email: " + oldEmail + "<br/>New Email: " + newEmail, "Utopiapimp.com", true);
        Email.SendEmail(true, pimpUser.PimpUser.UserName, newEmail, "Your email has been changed", pimpUser.PimpUser.UserName + ", <br/><br/> You have recently changed your Email on the Site Utopiapimp.com. This email is to keep for your records. <br/><br/> Old Email: " + oldEmail + "<br/>New Email: " + newEmail, "Utopiapimp.com", true);



        Response.Redirect(Page.Request.Url.AbsoluteUri);
    }
    protected void btnChangeQuestion_Click(object sender, EventArgs e)
    {
        MembershipUser mu = Membership.GetUser();
        if (String.IsNullOrEmpty(tbPassword.Text))
            lblQuestion.Text = "Please supply your password";
        bool changed = mu.ChangePasswordQuestionAndAnswer(tbPassword.Text, tbQuestion.Text, tbAnswer.Text);

        if (changed)
        {
            lblQuestion.Text = "The Question and Answer has been changed, an email has been sent to " + mu.Email;
            Email.EmailPassword = "email123";
            Email.EmailUsername = "email";
            Email.SmtpServer = "localhost";
            Email.FromEmail = "team@utopiapimp.com";
            Email.SendEmail(true, mu.UserName, mu.Email, "Your question and answer has been changed", mu.UserName + ", <br/><br/> You have recently changed your Question and Answer on the site Utopiapimp.com. This email is to keep for your records. <br/><br/> Question: " + tbQuestion.Text + "<br/>Answer: " + tbAnswer.Text, "Utopiapimp.com", true);

        }

        Response.Redirect(Page.Request.Url.AbsoluteUri);
    }
}

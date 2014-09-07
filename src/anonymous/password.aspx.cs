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

public partial class anonymous_password : MyBasePageCS
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (hfUserName.Value.Length == 0)
        {
            divQuestion.Visible = false;
        }
        if (hfUserName.Value.Length > 0)
        {
            divUserName.Visible = false;
        }
    }
    protected void btnUserName_Click(object sender, EventArgs e)
    {

        var user = Membership.GetUser(tbUserName.Text);
        if (tbUserName.Text.Length == 0 || user == null)
            lblWarning.Text = "No user found by that name.";
        else
        {
            hfUserName.Value = tbUserName.Text;
            divUserName.Visible = false;
            lblQuestion.Text = user.PasswordQuestion;
            divQuestion.Visible = true;
        }
    }
    protected void btnQuestion_Click(object sender, EventArgs e)
    {
        var user = Membership.GetUser(hfUserName.Value);
        if (!String.IsNullOrEmpty(tbAnswer.Text))
        {

            try
            {
                string password = user.ResetPassword(tbAnswer.Text);

                string text = user.UserName + ", <br/><br/> You just requested to change your password in Utopiapimp.com. Your new password is below.  Once logged in, <b>Please<b/> be sure to change your password to something you will remember.<br/><br/>Password: " + password;
                //TODO: this needs to be on a different thread.
                
                Email.SmtpServer = "localhost";
                Email.FromEmail = "team@utopiapimp.com";
                if (user.Email.Contains("yahoo"))
                {
                    Email.EmailUsername= "team@utopiapimp.com";
                    Email.EmailPassword = "RPMx1000";
                    Email.SendGmail(true, Page.User.Identity.Name, user.Email, "Requested New Password", text, "UtopiaPimp.com", false);
                }
                else
                {
                    Email.EmailPassword = "email123";
                    Email.EmailUsername = "email";
                    Email.SendEmail(true, Page.User.Identity.Name, user.Email, "Requested New Password", text, "UtopiaPimp.com", true);
                }
                lblWarning.Text = "New Password has been sent to: " + user.Email+". If you don't receive a password within a minute, please email spoiledtechie@gmail.com.";

            }
            catch (MembershipPasswordException exception)
            {
                lblWarning.Text = "The Answer you Supplied is Incorrect. If you can't remember the answer, contact spoiledtechie@gmail.com. ";
            }
        }
        else
        {
            lblWarning.Text = "There was no answer supplied.  Please Try again. ";
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Boomers.Utilities.Communications;

public partial class admin_admin_Email : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lbRoles.DataSource = Roles.GetAllRoles();
            lbRoles.DataBind();
            if (Request.QueryString["emailAdd"] != null)
                tbEmailAddress.Text = Request.QueryString["emailAdd"].ToString();
            if (Request.QueryString["title"] != null)
                tbTitle.Text = Request.QueryString["title"].ToString();
            if (Request.QueryString["userName"] != null)
                tbEmailUser.Text = Request.QueryString["userName"].ToString();
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (tbEmailAddress.Text.Length > 4)
        {
            Email.EmailPassword = "email123";
            Email.EmailUsername = "email";
            Email.SmtpServer = "localhost";
            Email.FromEmail = "team@utopiapimp.com";
            Email.SendEmail(true, Page.User.Identity.Name, tbEmailAddress.Text, tbTitle.Text, taAddInfo.InnerText, "[UtopiaPimp.com]", true);
            lblWarning.Text = "Email sent to " + tbEmailAddress.Text + ".";
        }
        else if (tbEmailUser.Text.Length > 2)
        {
            MembershipUser mu = Membership.GetUser(tbEmailUser.Text);
            Email.EmailPassword = "email123";
            Email.EmailUsername = "email";
            Email.SmtpServer = "localhost";
            Email.FromEmail = "team@utopiapimp.com";

            Email.SendEmail(true, Page.User.Identity.Name, mu.Email, tbTitle.Text, taAddInfo.InnerText, "[UtopiaPimp.com]", true);
            lblWarning.Text = "Email sent to " + tbEmailUser.Text + ".";
        }
        else
        {
            int count = 0;
            for (int i = 0; i < lbRoles.Items.Count; i++)
            {
                if (lbRoles.Items[i].Selected)
                {
                    if (lbRoles.Items[i].Value == "All")
                    {
                        MembershipUserCollection muc = Membership.GetAllUsers();
                        foreach (MembershipUser user in muc)
                        {
                            Email.EmailPassword = "email123";
                            Email.EmailUsername = "email";
                            Email.SmtpServer = "localhost";
                            Email.FromEmail = "team@utopiapimp.com";

                            Email.SendEmail(true, Page.User.Identity.Name, user.Email, tbTitle.Text, taAddInfo.InnerText, "[UtopiaPimp.com]", true);
                            count += 1;
                        }
                    }
                    else
                    {
                        foreach (string user in Roles.GetUsersInRole(lbRoles.Items[i].Text))
                        {
                            MembershipUser mu = Membership.GetUser(user);
                            Email.EmailPassword = "email123";
                            Email.EmailUsername = "email";
                            Email.SmtpServer = "localhost";
                            Email.FromEmail = "team@utopiapimp.com";

                            Email.SendEmail(true, Page.User.Identity.Name, mu.Email, tbTitle.Text, taAddInfo.InnerText, "[UtopiaPimp.com]", true);
                            count += 1;
                        }
                    }
                }
            }
            lblWarning.Text = "Sent " + count + " emails.";
        }
    }
}

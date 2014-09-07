using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Boomers.Admin;
using Boomers.Utilities.Communications;
using SupportFramework;
using SupportFramework.Users;

public partial class admin_admin_UsersLocked : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Update_gvUsers();
            lblUserCount.Text = Membership.GetAllUsers().Cast<MembershipUser>().Where(x => x.IsLockedOut == true).Count().ToString();
        }
    }

    private void Update_gvUsers()
    {
        List<UserItems> ui = new List<UserItems>();

        MembershipUserCollection mu = Membership.GetAllUsers();
        var mmu = (from xx in Membership.GetAllUsers().Cast<MembershipUser>()
                   where xx.IsLockedOut == true
                   select xx);
        foreach (MembershipUser user in mmu)
        {
            UserItems usi = new UserItems();
            usi.userName = user.UserName;
            usi.approved = user.IsApproved ? "Un-App" : "App";
            usi.comments = user.Comment;
            usi.createDate = user.CreationDate.ToShortDateString();
            usi.email = user.Email;
            usi.lastLogin = user.LastLoginDate.ToShortDateString();
            usi.locked = user.IsLockedOut ? "Unlock" : "";
            usi.passwordQuestion = user.PasswordQuestion;
            usi.userOnline = user.IsOnline;
            usi.roles = Roles.GetRolesForUser(user.UserName);
            usi.userErrors =Memberships.getUserErrorCount(user.UserName);
            usi.userPageViews =Memberships. getUserPageViews(user.UserName);
            ui.Add(usi);
        }

        gvUsers.DataSource = ui;
        gvUsers.DataBind();
    }
 
    protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvUsers.PageIndex = e.NewPageIndex;
        Update_gvUsers();
    }

    protected void gvUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        lblResetPassUser.Text = gv.SelectedDataKey.Value.ToString();
        lblResetPassword.Text = string.Empty;


    }
    protected void gvUsers_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        if (e.CommandName == "UnLockUser")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            MembershipUser mu = Membership.GetUser(gvUsers.DataKeys[index].Value.ToString());
            mu.UnlockUser();
            Membership.UpdateUser(mu);

            Email.EmailPassword = "email123";
            Email.EmailUsername = "email";
            Email.SmtpServer = "localhost";
            Email.FromEmail = "team@utopiapimp.com";
            string mess = "Your account has been unlocked in UtopiaPimp 2.1.<br/><br/> It seems you must have either entered a wrong password one too many times or you might have the wrong password in Utopia Angel or Pimp Agent.  Please verify that these passwords are correct. <br/><br/>You can now access UtopiaPimp again.<br/><br/>Thank you. <br/> SpoiledTechie";
            Email.SendEmail(true, mu.UserName, mu.Email, "Your account is unlocked", mess, "UtopiaPimp", true);

            Update_gvUsers();
        }
        else if (e.CommandName == "ApproveUser")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            MembershipUser mu = Membership.GetUser(gvUsers.DataKeys[index].Value.ToString());
            string mess = "Your account has been ";
            switch (mu.IsApproved)
            {
                case true:
                    mu.IsApproved = false;
                    mess += "unApproved ";
                    break;
                case false:
                    mu.IsApproved = true;
                    mess += "Approved ";
                    break;
            }
            Membership.UpdateUser(mu);
            Email.EmailPassword = "email123";
            Email.EmailUsername = "email";
            Email.SmtpServer = "localhost";
            Email.FromEmail = "team@utopiapimp.com";

            mess += "in UtopiaPimp 2.1.<br/><br/> If you think this is in error, please reply to this email with your reason why. <br/><br/>Thank you. <br/> SpoiledTechie";
            Email.SendEmail(true, mu.UserName, mu.Email, "Approval of Pimp", mess, "UtopiaPimp", true);
            Update_gvUsers();
        }
    }
    protected void btnResetPassword_Click(object sender, EventArgs e)
    {
        if (lblResetPassUser.Text.Length > 1)
        {
            lblResetPassword.Text = "Password: " + Membership.Providers["AdminMembershipProvider"].ResetPassword(lblResetPassUser.Text, null);
            Email.EmailPassword = "email123";
            Email.EmailUsername = "email";
            Email.SmtpServer = "localhost";
            Email.FromEmail = "team@utopiapimp.com";

            string mess = "Your password has been reset in UtopiaPimp 2.1.<br/><br/> Your new password is below. <br/><br/>Please write it down and change your password once you log back into UtopiaPimp.<br/><br/>" + lblResetPassword.Text + "<br/><br/> Thank you. <br/> SpoiledTechie";
            Email.SendEmail(true, lblResetPassUser.Text, Membership.GetUser(lblResetPassUser.Text).Email, "Password Reset", mess, "UtopiaPimp", true);
        }
    }
    protected void gvUsers_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
    {
        gvUsers.EditIndex = -1;
        Update_gvUsers();
    }
    protected void gvUsers_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        gvUsers.EditIndex = e.NewEditIndex;
        Update_gvUsers();
    }
    protected void gvUsers_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
    {
        TextBox tbComments = (TextBox)gvUsers.Rows[e.RowIndex].FindControl("tbComments");
        TextBox tbEmail = (TextBox)gvUsers.Rows[e.RowIndex].FindControl("tbEmail");
        MembershipUser mu = Membership.GetUser(gvUsers.DataKeys[e.RowIndex].Value.ToString());
        mu.Comment = tbComments.Text;
        mu.Email = tbEmail.Text;
        Membership.UpdateUser(mu);
        gvUsers.EditIndex = -1;
        Update_gvUsers();
    }
    protected void btnUnlockUsers_Click(object sender, EventArgs e)
    {

        MembershipUserCollection mu = Membership.GetAllUsers();
        var mmu = (from xx in Membership.GetAllUsers().Cast<MembershipUser>()
                   where xx.IsLockedOut == true
                   select xx).ToList();
        foreach (MembershipUser user in mmu)
        {
                        user.UnlockUser();
            Membership.UpdateUser(user);

            Email.EmailPassword = "email123";
            Email.EmailUsername = "email";
            Email.SmtpServer = "localhost";
            Email.FromEmail = "team@utopiapimp.com";

            string mess = "Your account has been unlocked in UtopiaPimp 2.1.<br/><br/> It seems you must have either entered a wrong password one too many times or you might have the wrong password in Utopia Angel or Pimp Agent.  Please verify that these passwords are correct. <br/><br/>You can now access UtopiaPimp again.<br/><br/>Thank you. <br/> SpoiledTechie";
            Email.SendEmail(true, user.UserName, user.Email, "Your account is unlocked", mess, "UtopiaPimp", true);

            Update_gvUsers();
                    }

    }
}

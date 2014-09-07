using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Boomers.Utilities.Communications;
using Boomers.Admin;
using SupportFramework;
using SupportFramework.Users;

public partial class admin_Users : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Update_gvUsers();
            lblUserCount.Text = Membership.GetAllUsers().Count.ToString();
        }
    }

    private void Update_gvUsers()
    {
        List<UserItems> ui = new List<UserItems>();

        MembershipUserCollection mu = Membership.GetAllUsers();
        foreach (MembershipUser user in mu)
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
            usi.userPageViews =Memberships.getUserPageViews(user.UserName);
            ui.Add(usi);
        }

        gvUsers.DataSource = ui;
        gvUsers.DataBind();
    }
    protected void gvRolesNotIn_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddRole")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridView gv = (GridView)sender;
            Roles.AddUserToRole(gvUsers.SelectedDataKey.Value.ToString(), gv.Rows[index].Cells[1].Text);
            Update_gvRolesIn(gvUsers);
            Update_RolesNotIn(gvUsers);
        }
    }
    protected void gvRolesIn_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        Roles.RemoveUserFromRole(gvUsers.SelectedDataKey.Value.ToString(), gvRolesIn.Rows[e.RowIndex].Cells[0].Text);
        Update_gvRolesIn(gvUsers);
        Update_RolesNotIn(gvUsers);
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

        Update_gvRolesIn(gv);
        Update_RolesNotIn(gv);
    }

    private void Update_RolesNotIn(GridView gv)
    {
        List<UserRoles> ur = new List<UserRoles>();
        foreach (string role in Roles.GetAllRoles())
        {
            if (!Roles.IsUserInRole(gv.SelectedDataKey.Value.ToString(), role))
            {
                UserRoles urs = new UserRoles();
                urs.role = role;
                ur.Add(urs);
            }
        }
        gvRolesNotIn.DataSource = ur;
        gvRolesNotIn.DataBind();
    }

    private void Update_gvRolesIn(GridView gv)
    {
        gvRolesIn.DataSource = (from rol in Roles.GetRolesForUser(gv.SelectedDataKey.Value.ToString())
                                select new
                                {
                                    role = rol
                                });

        gvRolesIn.DataBind();
    }
    protected void gvUsers_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        if (e.CommandName == "UnLockUser")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            MembershipUser mu = Membership.GetUser(gvUsers.DataKeys[index].Value.ToString());
            mu.UnlockUser();
            Membership.UpdateUser(mu);
            Update_gvUsers();
        }
        else if (e.CommandName == "ApproveUser")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            MembershipUser mu = Membership.GetUser(gvUsers.DataKeys[index].Value.ToString());

            switch (mu.IsApproved)
            {
                case true:
                    mu.IsApproved = false;
                    break;
                case false:
                    mu.IsApproved = true;
                    break;
            }
            Membership.UpdateUser(mu);

            Update_gvUsers();
        }
    }
    protected void btnResetPassword_Click(object sender, EventArgs e)
    {
        if (lblResetPassUser.Text.Length > 1)
        {
            lblResetPassword.Text = "Password: " + Membership.Providers["AdminMembershipProvider"].ResetPassword(lblResetPassUser.Text, null);
            Email.FromEmail = "spoiledtechie@utopiapimp.com";
            string mess = "Your password has been reset in UtopiaPimp 2.1.<br/><br/> Your new password is below. <br/><br/>Please write it down and change your password once you log back into UtopiaPimp.<br/><br/>" + lblResetPassword.Text + "<br/><br/> Thank you. <br/> SpoiledTechie";
            Email.SendEmail(true, lblResetPassUser.Text, Membership.GetUser(lblResetPassUser.Text).Email, "Password Reset", mess, "UtopiaPimp", false);
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
    protected void btnSearchUserName_Click(object sender, EventArgs e)
    {
        List<UserItems> ui = new List<UserItems>();

        MembershipUserCollection mu = Membership.FindUsersByName(tbUsernameSearch.Text);
        foreach (MembershipUser user in mu)
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
            usi.userPageViews =Memberships.getUserPageViews(user.UserName);
            ui.Add(usi);
        }

        gvUsers.DataSource = ui;
        gvUsers.DataBind();
    }
    protected void btnEmailSearch_Click(object sender, EventArgs e)
    {
        List<UserItems> ui = new List<UserItems>();

        MembershipUserCollection mu = Membership.FindUsersByEmail(tbEmailSearch.Text);
        foreach (MembershipUser user in mu)
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
            usi.userPageViews = Memberships.getUserPageViews(user.UserName);
            ui.Add(usi);
        }

        gvUsers.DataSource = ui;
        gvUsers.DataBind();
    }
}

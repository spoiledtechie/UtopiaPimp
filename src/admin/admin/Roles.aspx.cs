using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SupportFramework;

using SupportFramework.Users;

public partial class admin_Roles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Update_gvRoles();
            ltJavascriptInject.Text = "<script type=\"text/javascript\">";
            ltJavascriptInject.Text += "$(document).ready(function() {$('#" + gvRoles.ClientID + "').tablesorter({widgets: ['zebra'], widgetZebra: {css: ['GridViewAlternatingRowStyle','GridviewRowStyle']}});});";
            ltJavascriptInject.Text += "$(document).ready(function() {$('#" + gvUsers.ClientID + "').tablesorter({widgets: ['zebra'], widgetZebra: {css: ['GridViewAlternatingRowStyle','GridviewRowStyle']}});});";
            ltJavascriptInject.Text += "$(document).ready(function() {$('#" + gvUsersAdd.ClientID + "').tablesorter({widgets: ['zebra'], widgetZebra: {css: ['GridViewAlternatingRowStyle','GridviewRowStyle']}});});";
            ltJavascriptInject.Text += "</script>";
        }
    }

    private void Update_gvRoles()
    {

        CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
        List<RoleItem> Items = (from asr in adb.vw_aspnet_Roles
                                where asr.ApplicationId == Applications.Instance.ApplicationId
                                select new RoleItem
                                {
                                    Role = asr.RoleName,
                                    Count = 0
                                }).ToList();
        foreach (var item in Items)
        {
            item.Count = Roles.GetUsersInRole(item.Role).Count();
        }
        gvRoles.DataSource = Items;
        gvRoles.DataBind();
    }
    protected void btnNewRole_Click(object sender, EventArgs e)
    {
        if (!Roles.RoleExists(tbNewRole.Text))
        {
            Roles.CreateRole(tbNewRole.Text);
            Update_gvRoles();
            lblWarning.Text = "The Role '" + tbNewRole.Text + "' has been created";
        }
        else
        {
            lblWarning.Text = "The Role already exists.  Please try again.";
        }
    }
    protected void gvRoles_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        Roles.DeleteRole(gvRoles.Rows[e.RowIndex].Cells[1].Text);
        Update_gvRoles();

    }
    protected void gvUsers_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        Roles.RemoveUserFromRole(gvUsers.Rows[e.RowIndex].Cells[0].Text, gvRoles.SelectedRow.Cells[1].Text);
        Update_gvRoles();
        Update_gvUsers(gvRoles);
        Update_gvUsersAdd(gvRoles);
    }
    protected void gvUsersAdd_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        Roles.AddUserToRole(gvUsersAdd.Rows[e.RowIndex].Cells[1].Text, gvRoles.SelectedRow.Cells[1].Text);
        Update_gvRoles();
        Update_gvUsers(gvRoles);
        Update_gvUsersAdd(gvRoles);
    }
    protected void gvRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        Update_gvUsers(gv);
        Update_gvUsersAdd(gv);
    }
    private void Update_gvUsersAdd(GridView gv)
    {
        List<UserItem> users = new List<UserItem>();
        MembershipUserCollection muc = Membership.GetAllUsers();
        foreach (MembershipUser name in muc)
        {
            if (!Roles.IsUserInRole(name.UserName, gv.SelectedRow.Cells[1].Text))
            {
                UserItem ui = new UserItem();
                ui.userName = name.UserName;
                ui.email = name.Email;
                users.Add(ui);
            }
        }

        gvUsersAdd.DataSource = users;
        gvUsersAdd.DataBind();
    }
    private void Update_gvUsers(GridView gv)
    {
        var getRoles = (from xx in Roles.GetUsersInRole(gv.SelectedRow.Cells[1].Text)
                        select new
                        {
                            Role = xx.ToString(),
                            Email = Memberships.getUserEmail(xx.ToString())
                        });
        gvUsers.DataSource = getRoles;
        gvUsers.DataBind();
    }
    private string ConvertSortDirectionToSql(SortDirection sortDirection)
    {
        string newSortDirection = String.Empty;
        switch (sortDirection)
        {
            case SortDirection.Ascending:
                newSortDirection = "ASC";
                break;

            case SortDirection.Descending:
                newSortDirection = "DESC";
                break;
        }

        return newSortDirection;
    }

    protected void gvUsersAdd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvUsersAdd.PageIndex = e.NewPageIndex;
        Update_gvUsersAdd(gvRoles);
    }

    protected void gvUsersAdd_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dataTable = gvUsersAdd.DataSource as DataTable;

        if (dataTable != null)
        {
            DataView dataView = new DataView(dataTable);
            dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);

            gvUsersAdd.DataSource = dataView;
            gvUsersAdd.DataBind();
        }
    }
}
public class RoleItem
{
    public string Role { get; set; }
    public int Count { get; set; }
}
public class UserItem
{
    public string userName { get; set; }
    public string email { get; set; }
}
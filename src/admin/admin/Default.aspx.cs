using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using SupportFramework;
using Boomers.Admin.MsSql;

public partial class admin_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        TimeMsg.Text = DateTime.UtcNow.ToString("G");

        if (!IsPostBack)
        {
            int userCount = Membership.GetAllUsers().Count;
            CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
            CS_Code.GlobalDataContext db = CS_Code.GlobalDataContext.Get();
            CS_Code.UtopiaDataContext sdb = CS_Code.UtopiaDataContext.Get();

            //User information
            lblUserCount.Text = userCount.ToString("N0");
            lblUsersOnline.Text = Membership.GetNumberOfUsersOnline() > 1 ? Membership.GetNumberOfUsersOnline().ToString("N0") + " Online" : Membership.GetNumberOfUsersOnline().ToString("N0") + " Online";
            
            
            lblLockedUsers.Text = (from spm in adb.vw_aspnet_MembershipUsers
                                   where spm.IsLockedOut == true
                                   select spm.UserName).Count().ToString("N0");

            lblRolesCount.Text = Roles.GetAllRoles().Count().ToString();
            lblTotalErrors.Text = (from tel in db.Global_Errors_Logs
                                   where tel.Application_Id == Applications.Instance.ApplicationId
                                   select tel.uid).Count().ToString("N0");

            lblUnsolvedErrors.Text = (from tel in db.Global_Errors_Logs
                                      where tel.Application_Id == Applications.Instance.ApplicationId
                                      where tel.Reviewed == 0
                                      select tel.uid).Count().ToString("N0");

            lblSevenDayErrors.Text = (from tel in db.Global_Errors_Logs
                                      where tel.Application_Id == Applications.Instance.ApplicationId
                                      where tel.Date_Time > DateTime.UtcNow.AddDays(-7)
                                      select tel.uid).Count().ToString("N0");

            lblYearErrors.Text = (from tel in db.Global_Errors_Logs
                                  where tel.Application_Id == Applications.Instance.ApplicationId
                                  where tel.Date_Time > DateTime.UtcNow.AddDays(-365)
                                  select tel.uid).Count().ToString("N0");

            int getApprovedUserCount = (from xx in adb.vw_aspnet_MembershipUsers
                                        where xx.IsApproved == true
                                        select xx.UserId).Count();
            lblApprovedUsers.Text = getApprovedUserCount.ToString("N0");
            lblUnapprovedUsers.Text = (from xx in adb.vw_aspnet_MembershipUsers
                                       where xx.IsApproved == false
                                       select xx.UserId).Count().ToString();

            lblLoggedInLately.Text = (from xx in adb.vw_aspnet_MembershipUsers
                                      where xx.LastLoginDate > DateTime.UtcNow.AddDays(-5)
                                      select xx).Count().ToString("N0");
            lblLoggedInWhile.Text = (from xx in adb.vw_aspnet_MembershipUsers
                                     where xx.LastLoginDate > DateTime.UtcNow.AddDays(-10)
                                     select xx).Count().ToString("N0");
            lblLoggedInMonth.Text = (from xx in adb.vw_aspnet_MembershipUsers
                                     where xx.LastLoginDate > DateTime.UtcNow.AddDays(-30)
                                     select xx).Count().ToString("N0");

            //Views information
            lblUsersToday.Text = (from xx in adb.vw_aspnet_MembershipUsers
                                  where xx.LastLoginDate > DateTime.UtcNow.AddHours(-24)
                                  select xx).Count().ToString("N0");
            lblPageViewsToday.Text = (from xx in db.Global_User_Audits
                                      where xx.ApplicationId == Applications.Instance.ApplicationId
                                      where xx.DateTime > DateTime.UtcNow.AddHours(-24)
                                      select xx).Count().ToString("N0");
            lblPageViewsFive.Text = (from xx in db.Global_User_Audits
                                     where xx.ApplicationId == Applications.Instance.ApplicationId
                                     where xx.DateTime > DateTime.UtcNow.AddDays(-5)
                                     select xx).Count().ToString("N0");
            lblPageViewsTen.Text = (from xx in db.Global_User_Audits
                                    where xx.ApplicationId == Applications.Instance.ApplicationId
                                    where xx.DateTime > DateTime.UtcNow.AddDays(-10)
                                    select xx).Count().ToString("N0");
            lblPageViewsMonth.Text = (from xx in db.Global_User_Audits
                                      where xx.ApplicationId == Applications.Instance.ApplicationId
                                      where xx.DateTime > DateTime.UtcNow.AddDays(-30)
                                      select xx).Count().ToString("N0");
            lblPageViewsTotal.Text = (from xx in db.Global_User_Audits
                                      where xx.ApplicationId == Applications.Instance.ApplicationId
                                      select xx).Count().ToString("N0");
            lblBrowserTypes.Text = (from xx in db.Global_User_Audits
                                    where xx.ApplicationId == Applications.Instance.ApplicationId
                                    group xx by xx.Browser_String into g
                                    select g).Count().ToString("N0");
            lblOSTypes.Text = (from xx in db.Global_User_Audits
                               where xx.ApplicationId == Applications.Instance.ApplicationId
                               group xx by xx.Windows_Platform into g
                               select g).Count().ToString("N0");

            var javaScript = (from xx in db.Global_User_Audits
                              where xx.ApplicationId == Applications.Instance.ApplicationId
                              group xx by xx.JavaScript into g
                              select new
                              {
                                  JScript = g.Key,
                                  Users = (from yy in g
                                           where yy.DateTime > DateTime.UtcNow.AddDays(-30)
                                           select yy.User_ID).Distinct().Count()
                              });
            foreach (var item in javaScript)
                lblJavaMonth.Text += item.Users.ToString() + (item.JScript.Value == 1 ? " Users JavaScript Enabled" : " Users JavaScript Disabled") + "<br />";

            //Database View
            IEnumerable<DatabaseInfo> query = adb.ExecuteQuery<DatabaseInfo>("sp_spaceused");
            foreach (var item in query)
            {
                lblDBName.Text = item.database_name;
                lblDBSize.Text = item.database_size;
                lblDBReserved.Text = item.reserved;
                lblDBData.Text = item.data;
                lblDBIndexSize.Text = item.index_size;
                lblDBUnUsed.Text = item.unused;
            }
            IEnumerable<DatabaseInfo> query1 = sdb.ExecuteQuery<DatabaseInfo>("sp_spaceused");
            foreach (var item in query1)
            {
                lblSiteDBName.Text = item.database_name;
                lblSiteDBSize.Text = item.database_size;
                lblSiteDBReserved.Text = item.reserved;
                lblSiteDBData.Text = item.data;
                lblSiteDBIndexSize.Text = item.index_size;
                lblSiteDBUnUsed.Text = item.unused;
            }

        }
    }
    protected void btnRestartTheApp_Click(object sender, EventArgs e)
    {
        HttpRuntime.UnloadAppDomain();
    }
}

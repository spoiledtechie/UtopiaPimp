using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using YAF.Classes.Utils;
using YAF.Classes.Data;


public partial class admin_admin_yafSetup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        MembershipUserCollection muc = Membership.GetAllUsers();
        foreach (var item in muc)
        {
            MembershipUser mu = (MembershipUser)item;
            try { RoleMembershipHelper.SetupUserRoles(YafContext.Current.PageBoardID, mu.UserName); }
            catch { }
        }
    }
}

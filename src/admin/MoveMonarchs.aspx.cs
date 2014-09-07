using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_MoveMonarchs : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {//1-OwnerOfKingdom, 2-sub, 3-admin, 4-MoarchForKingdom, 0-non
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        var getMonarchs = (from xx in db.Utopia_Province_Data_Captured_Gens
                           where xx.Monarch_Display == 1 | xx.Monarch_Display == 2
                           select xx);
        foreach (var item in getMonarchs)
        {
            if (item.Monarch_Display == 1)
                item.Owner = 1;
            if (item.Monarch_Display == 2)
                item.Sub_Monarch = 1;
            db.SubmitChanges();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Boomers.Utilities.Guids;

public partial class admin_UAdmin_Provinces : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnUserName_Click(object sender, EventArgs e)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        var provs = (from xx in db.Utopia_Province_Data_Captured_Gens
                     where xx.Owner_User_ID == SupportFramework.Users.Memberships.getUserID(tbUsername.Text)
                     select new { xx.Province_Name, xx.Province_ID, xx.Kingdom_ID }).ToList();
        divProvinces.InnerHtml = "<ul>";
        foreach (var item in provs)
        {
            divProvinces.InnerHtml += "<li><a href='../../members/ProvinceDetail.aspx?ID=" + item.Province_ID.RemoveDashes() + "'>" + item.Province_Name + "</a>";
            if (item.Kingdom_ID.HasValue)
                divProvinces.InnerHtml += " - <a href='../../members/kd.aspx?kdid=" + ((Guid)item.Kingdom_ID).RemoveDashes() + "&kdty=myaddy'>Kingdom</a>";
            divProvinces.InnerHtml += " - <span class='button' onclick=\"javascript:DisconnectProvFromUser('" + item.Province_ID.RemoveDashes() + "');\">Disconnect User from Province</span></li>";
        }
        divProvinces.InnerHtml += "</ul>";
    }
}

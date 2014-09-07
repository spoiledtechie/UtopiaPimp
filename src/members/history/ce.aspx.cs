using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using Pimp.UCache;
using Pimp;
using Pimp.Users;
using Pimp.UIBuilder;
using Pimp.UData;


public partial class members_history_ce : MyBasePageCS
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            string provID = string.Empty;
            if (Request.QueryString["ID"] != null)
                provID = Request.QueryString["ID"].ToString();
            else
                Response.Redirect("../Default.aspx");

            ltCSS.Text = "<link href=\"http://codingforcharity.org/utopiapimp/css/Default.css?v=" + SupportFramework.StaticContent.CSS.CssVersion + "\" rel='stylesheet' type='text/css' />";

            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            var ProvinceOwnerIDCheck = KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom).Provinces.Where(x => x.Province_ID == new Guid(provID)).FirstOrDefault();
            if (ProvinceOwnerIDCheck == null & !pimpUser.PimpUser.IsUserAdmin)
                Response.Redirect("../Default.aspx");

            Page.Title = "[UtopiaPimp] CE History of " + ProvinceOwnerIDCheck.Province_Name;
            ltText.Text = CE.BuildCEPersonal(CE.GetLastMonthPersonal(ProvinceOwnerIDCheck.Province_Name, pimpUser.PimpUser.StartingKingdom), CE.GetLastYearPersonal(ProvinceOwnerIDCheck.Province_Name, pimpUser.PimpUser.StartingKingdom), "All", pimpUser.PimpUser.StartingKingdom.ToString(), ProvinceOwnerIDCheck.Province_Name, pimpUser.PimpUser.StartingKingdom.ToString(), pimpUser.PimpUser.UserID);
            ltText.Text += "<div id=\"CEID\" style=\"Display:none;\">" + pimpUser.PimpUser.StartingKingdom + "</div>";
            ltText.Text += "<div id=\"divOwnerID\" style=\"Display:none;\">" + pimpUser.PimpUser.StartingKingdom + "</div>";
            ltText.Text += "<div id=\"divProvName\" style=\"Display:none;\">" + ProvinceOwnerIDCheck.Province_Name + "</div>";
        }
    }
}

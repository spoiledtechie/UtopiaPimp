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
using Pimp.Utopia;
using Pimp.UData;

public partial class members_history_attacks : MyBasePageCS
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ltCSS.Text = "<link href=\"http://codingforcharity.org/utopiapimp/css/Default.css?v=" + SupportFramework.StaticContent.CSS.CssVersion + "\" rel='stylesheet' type='text/css' />";

            string provID = string.Empty;
            if (Request.QueryString["ID"] != null)
                provID = Request.QueryString["ID"].ToString();
            else
                Response.Redirect("../Default.aspx");
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            OwnedKingdomProvinces cachedKingdom = KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom);

            var ProvinceOwnerIDCheck = cachedKingdom.Provinces.Where(x => x.Province_ID == new Guid(provID)).FirstOrDefault();

            if (ProvinceOwnerIDCheck == null & !pimpUser.PimpUser.IsUserAdmin)
                Response.Redirect("../Default.aspx");

            Page.Title = "[UtopiaPimp] Attack History of " + ProvinceOwnerIDCheck.Province_Name;
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var getSOSItems = (from xx in
                                   (from xx in db.Utopia_Province_Data_Captured_Attacks
                                    where xx.Owner_Kingdom_ID == pimpUser.PimpUser.StartingKingdom
                                    where xx.Province_ID_Added == new Guid(provID) | xx.Province_ID_Attacked == new Guid(provID)
                                    group xx by xx.DateTime_Added into g
                                    orderby g.Key descending
                                    select new { g }).Distinct()
                               select new
                               {
                                   xx.g.Key,
                                   uid = (from yy in xx.g
                                          select yy.uid).FirstOrDefault(),
                                   type = (from yy in xx.g
                                           from zz in db.Utopia_Province_Data_Captured_Attack_Pulls
                                           where yy.Attack_Type == zz.uid
                                           select zz.Attack_Type_Name).FirstOrDefault()
                               }).OrderByDescending(x => x.uid);
            StringBuilder sb = new StringBuilder();
            sb.Append("<h2>Attack History of " + ProvinceOwnerIDCheck.Province_Name + "</h2>");
            sb.Append("<table><tr><td valign=\"top\">");
            sb.Append("<table class=\"tblKingdomInfo\">");
            sb.Append("<tr><thead><th></th><th>Select SOM</th><th>Type</th><th>Date Uploaded</th></thead></tr>");
            int i = 0;
            foreach (var item in getSOSItems)
            {
                switch (i % 2)
                {
                    case 0:
                        sb.Append("<tr class=\"d0\">");
                        break;
                    case 1:
                        sb.Append("<tr class=\"d1\">");
                        break;
                }
                sb.Append("<td>");
                sb.Append(i);
                sb.Append("</td>");
                sb.Append("<td><a href=\"#\" onclick=\"javascript:SelectAttack('" + provID + "','" + item.uid + "');\">Select</a></td><td>");
                sb.Append(item.type);
                sb.Append("</td><td>");
                sb.Append(item.Key);
                sb.Append("</td></tr>");
                i += 1;
            }
            sb.Append("</table>");
            sb.Append("</td><td valign=\"top\">");
            sb.Append("<div id=\"divHistoryUpdate\" class=\"divOpHistory\"><div>");
            sb.Append("</td></tr></table>");
            divMainHistory.InnerHtml = sb.ToString();
        }
    }
}

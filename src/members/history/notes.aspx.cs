using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Boomers.Utilities.DatesTimes;

using Pimp.UCache;
using Pimp;
using Pimp.Users;
using Pimp.Utopia;
using Pimp.UData;

public partial class members_history_notes : MyBasePageCS
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
            OwnedKingdomProvinces cachedKingdom = KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom);
            //var userID = SupportFramework.Users.Memberships.getUserID();
            //var user = pimpUser.PimpUser.getUser(userID);
            var ProvinceOwnerIDCheck = cachedKingdom.Provinces.Where(x => x.Province_ID == new Guid(provID)).FirstOrDefault();
            if (ProvinceOwnerIDCheck == null & !pimpUser.PimpUser.IsUserAdmin)
                Response.Redirect("../Default.aspx");

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();

            Page.Title = "[UtopiaPimp] Note History of " + ProvinceOwnerIDCheck.Province_Name;

            var getNotes = (from xx in db.Utopia_Province_Notes
                            where xx.Province_ID == new Guid(provID)
                            orderby xx.Added_By_DataTime descending
                            select xx);

            StringBuilder sb = new StringBuilder();
            sb.Append("<h2>Note History on " + ProvinceOwnerIDCheck.Province_Name + "</h2><br />");
            foreach (var item in getNotes)
            {
                sb.Append(cachedKingdom.Provinces.Where(x => x.Province_ID == item.Added_By_Province_ID).FirstOrDefault().Province_Name);
                sb.Append(" noted this about ");
                sb.Append(item.Added_By_DataTime.ToLongRelativeDate());
                sb.Append(" <br />");
                sb.Append(item.Note);
                sb.Append(" <br /><br />");
            }
            divForm.InnerHtml = sb.ToString();
        }
    }
}

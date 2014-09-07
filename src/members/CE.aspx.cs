using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Pimp.UCache;
using Boomers.Utilities.DatesTimes;
using Pimp;
using Pimp.Users;
using Pimp.Utopia;
using Pimp.UIBuilder;
using Pimp.UData;

public partial class members_CE : MyBasePageCS
{
    PimpUserWrapper  currentUser;

    protected void Page_Load(object sender, EventArgs e)
    {
        
        currentUser = new PimpUserWrapper ();

        if (!IsPostBack)
        {
            if (HttpContext.Current.Request.QueryString["kdid"] != null)
            {
                var cachedKingdom = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom);
                Guid kdID = new Guid(HttpContext.Current.Request.QueryString["kdid"].ToString());
                var kd = CeCache.getCeForKingdomCache(kdID, currentUser.PimpUser.StartingKingdom, cachedKingdom);
                if (kd != null)
                {
                    ltText.Text = CE.BuildCE(CE.GetLastMonth(kd.CeList), CE.GetLastYear(kd.CeList), "All", kdID, currentUser.PimpUser.StartingKingdom, currentUser.PimpUser.UserID, cachedKingdom);
                    ltText.Text += "<div id=\"CEID\" style=\"Display:none;\">" + kdID + "</div>";
                }
                else
                    ltText.Text = "You have accessed this page in the wrong way.  Sorry.";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CS_Code;

using Pimp.UCache;
using Pimp.UData;

public partial class admin_ChangeKingdom : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["kingdomID"]))
        {
            Guid kingdomID;
            if (Guid.TryParse(Request["kingdomID"].Trim(), out kingdomID))
            {
                ChangeKingdom(kingdomID);
            }
        }

        var kingdomCookieCache = Request.Cookies["recentKingdoms"] ?? null;
        if (kingdomCookieCache == null)
            return;

        foreach (var kingdom in kingdomCookieCache.Values.AllKeys)
        {
            recentlyKingdomIds.Text = "<a href=\"ChangeKingdom.aspx?kingdomID=" + kingdom + "\">" + kingdomCookieCache.Values[kingdom] + "</a>";
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Guid kingdomID;
        if (Guid.TryParse(txtKingdomID.Text.Trim(), out kingdomID))
        {
            ChangeKingdom(kingdomID);
        }
    }

    private void ChangeKingdom(Guid kingdomID)
    {
        UtopiaDataContext ctx = new UtopiaDataContext();
        var kingdom = ctx.Utopia_Kingdom_Infos.FirstOrDefault(x => x.Kingdom_ID == kingdomID);
        if (kingdom != null)
        {
            if (Request.Cookies["kingdoms"] != null)
            {
                var kingdomCookieCache = Request.Cookies["recentKingdoms"];
                if (string.IsNullOrEmpty(kingdomCookieCache.Values[kingdomID.ToString()]))
                    kingdomCookieCache.Values.Add(kingdomID.ToString(), kingdom.Kingdom_Name);
                Response.AppendCookie(kingdomCookieCache);
            }
            else
            {
                var cookie = new HttpCookie("recentKingdoms");
                cookie.Expires = DateTime.Now.AddMonths(12);
                cookie.Values.Add(kingdomID.ToString(), kingdom.Kingdom_Name);
                Response.AppendCookie(cookie);
            }
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();

            pimpUser.PimpUser.StartingKingdom = kingdomID;
            message.Text = "Kingdom updated, please <a href=\"Default.aspx\">click here</a>.";
            return;
        }
        message.Text = "No kingdom found";
    }
}
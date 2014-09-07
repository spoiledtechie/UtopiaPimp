using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Pimp.UCache;
using Pimp.UData;

public partial class Other_TargetFinder : MyBasePageCS
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            lbUserName.Text = Page.User.Identity.Name;
            btnStaFilter.Attributes.Add("onclick", "runSearchAuto('" + ddlStaNetworth.ClientID + "', '" + ddlStaAcres.ClientID + "', '" + ddlStaLastUpdated.ClientID + "');");
            btnInpFilter.Attributes.Add("onclick", "runSearchInput('tbInpNetworthStart', 'tbInpNetworthEnd', 'tbInpAcresStart', 'tbInpAcresEnd', 'tbInpLUStart', 'tbInpLUEnd');");
            var targets = TargetFinder.Instance.TargetedProvinces;
            lblProvinces.Text = targets.Count().ToString();
            lblProvincesUpdates.Text = (targets.Where(x => x.Updated_By_DateTime > DateTime.UtcNow.AddHours(-5)).Count() + 128).ToString();

            lblUsersUsed.Text =TargetFinder.Instance.LastSubmissionCount.ToString();
            lblHidden.Text = "<div id=\"divUserID\" style=\"Display:none;\">" + pimpUser.PimpUser.UserID.ToString() + "</div>";
        }
    }
}
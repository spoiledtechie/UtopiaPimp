using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Pimp.UCache;
using Pimp.UData;

public partial class anonymous_donate : MyBasePageCS
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ///displays the edit and delete buttons
        if (Page.User.IsInRole("admin") == false)
        {
            gvDonations.Columns[0].Visible = false;
        }
    }
    /// <summary>
    /// This query adds a donator to the list of donators.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddDonator_Click(object sender, EventArgs e)
    {
        PimpUserWrapper  pimpUser = new PimpUserWrapper ();

        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        CS_Code.Utopia_Donation UD = new CS_Code.Utopia_Donation();
        UD.Donator_Name = txtbxDonatorName.Text;
        UD.Donated_Amount = Convert.ToDecimal(txtbxDonatorAmount.Text);
        UD.Date_Donated = Convert.ToDateTime(txtbxDateDonated.Text);
        UD.Inserted_By = pimpUser.PimpUser.UserID;
        UD.Inserted_Date = DateTime.UtcNow;
        db.Utopia_Donations.InsertOnSubmit(UD);
        db.SubmitChanges();

        gvDonations.DataBind(); /// Updates the Gridview.
    }
}

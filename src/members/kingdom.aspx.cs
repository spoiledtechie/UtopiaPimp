using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Text.RegularExpressions;

using Pimp.UCache;
using Pimp.UParser;
using Pimp.UData;
public partial class members_kingdom : MyBasePageCS
{

    protected void Page_Load(object sender, EventArgs e)
    {
        PimpUserWrapper pimpUser = new PimpUserWrapper();
        switch (!IsPostBack)
        {
            case true:
                switch (Request.QueryString["id"].ToString())
                {
                    case "jn":
                        aJoin.Attributes.Add("class", "selected");
                        break;
                    case "ak":
                        aAdd.Attributes.Add("class", "selected");
                        break;
                    case "ctd":
                        aAdd.Attributes.Add("class", "selected");
                        divWarning.InnerHtml = "<h2><b>** " + Request.QueryString["kd"].ToString() + "</b> has been created. Select <b>" + Request.QueryString["pr"].ToString() + "</b> from the dropdown above.</h2>";
                        break;
                    case "jnd":
                        aJoin.Attributes.Add("class", "selected");
                        lblWarningProvinceCode.Text = "You have just been assigned to the Province " + Request.QueryString["name"].ToString() + ", Please make sure you select the province from the dropdown in the top RIGHT of the page.";
                        txtbxProvinceCode.Visible = false;
                        btnSubmit.Visible = false;
                        lblProvinceCode.Visible = false;
                        pimpUser.removeUserCache();
                        break;
                }
                break;
        }
    }
    /// <summary>
    /// Starts a new kingdom for UtopiaPimp
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnStartKingdom_Click(object sender, EventArgs e)
    {
        string re = "good";
        string malformed = string.Empty;
        PimpUserWrapper pimpUser = new PimpUserWrapper();
       
        var cachedKingdom = KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom);
        Session["SubmittedData"] = txtbxKingdomPage.Text + "    ProvinceName: '" + txtbxAddKingdom.Text + "'";
        if (txtbxKingdomPage.Text.Contains("utopiatemple"))
        {
            Regex rgxTestProv = new Regex(txtbxAddKingdom.Text.Trim() + @" \[[A-Z]+\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            if (rgxTestProv.IsMatch(txtbxKingdomPage.Text))
                re = UtopiaParser.UtopiaParsing(txtbxKingdomPage.Text, "StartKingdom", ddlServerName.SelectedValue, txtbxAddKingdom.Text.Trim(), string.Empty, pimpUser, cachedKingdom);
        }
        else
        {
            Regex rgxTestKingdomName = new Regex(@"(The kingdom of|Current kingdom is|The Esteemed kingdom of) " + txtbxAddKingdom.Text.Trim(), RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex rgxTestProvinceName = new Regex(txtbxAddKingdom.Text.Trim() + URegEx._findProvinceLineValidation, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            if (!URegEx.rgxFindIslandLocation.IsMatch(txtbxKingdomPage.Text))
                re = "Can't find Kingdom Island:Location.  Please Select the ENTIRE Kingdom Page.";
            if (rgxTestKingdomName.IsMatch(txtbxKingdomPage.Text))//if the user accidentally enters the kingdom name instead of their province name
                re = "Can't find Kingdom Name in page";
            if (!rgxTestProvinceName.IsMatch(txtbxKingdomPage.Text))
                re = "Kingdom Page Seems Malformed. Couldn't Find Your Province Name";
            if (!txtbxKingdomPage.Text.Contains(txtbxAddKingdom.Text))
                re = "Province Name was not found in Kingdom Page";

            if (re == "good") //go ahead and parse it.
                UtopiaParser.UtopiaParsing(txtbxKingdomPage.Text, "StartKingdom", ddlServerName.SelectedValue, txtbxAddKingdom.Text.Trim(), string.Empty, pimpUser, cachedKingdom);
        }
        Session.Remove("SubmittedData");
        //CachedItems.RemoveUserCache();

        if (re == "good")
            Response.Redirect("kingdom.aspx?id=ctd&kd=" + re + "&pr=" + txtbxAddKingdom.Text);
        else
            divWarning.InnerHtml = "<h2><b>** " + re + "</b></h2>";
    }
    /// <summary>
    /// User Joins a Kingdom
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        txtbxProvinceCode.Text = txtbxProvinceCode.Text.Trim();

        if (Boomers.Utilities.Guids.GuidExt.IsValidGuid(txtbxProvinceCode.Text))
        {
            switch (txtbxProvinceCode.Text.Length)
            {
                case 32:
                    CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
                    var CheckProvinceID = (from UPI in db.Utopia_Province_Data_Captured_Gens
                                           where UPI.Province_ID == new Guid(txtbxProvinceCode.Text)
                                           select new
                                           {
                                               UPI.uid,
                                               UPI.Province_Name,
                                               UPI.Owner_User_ID,
                                               UPI.Owner_Kingdom_ID,
                                               UPI.Province_ID
                                           }).FirstOrDefault();

                    if (CheckProvinceID != null)
                    {
                        if (CheckProvinceID.Owner_User_ID != null)
                            lblWarningProvinceCode.Text = "The Province already has a user attached to it. Please ask your Monarch to make sure no user is attached to this province.";
                        else
                        {
                            UtopiaParser.attachProvinceUser(new Guid(txtbxProvinceCode.Text));
                            Response.Redirect("kingdom.aspx?id=jnd&name=" + CheckProvinceID.Province_Name);
                        }
                    }
                    else
                        lblWarningProvinceCode.Text = "There was no province found with that ID.";
                    break;
                default:
                    lblWarningProvinceCode.Text = "Province Code is not the correct length, please make sure you have it all.";
                    break;
            }
        }
        else
        {
            lblWarningProvinceCode.Text = "This is an Invalid Province Code.  Please Make Sure it was fully copied.";
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Boomers.Utilities.DatesTimes;
using Boomers.Utilities.Web;
using Boomers.Utilities.Guids;

using Pimp.UParser;
using Pimp.UCache;
using PimpLibrary.Static.Enums;
using PimpLibrary.Utopia;
using Pimp;
using Pimp.Utopia;
using Pimp.Users;
using PimpLibrary.UI;
using SupportFramework.Data;
using MvcMiniProfiler;
using Pimp.UData;



public partial class main : System.Web.UI.MasterPage
{


    //Dictionary<string, object> objectsForPassing;

    protected void Page_Load(object sender, EventArgs e)
    {

        Page.Header.DataBind();

        PimpUserWrapper pimpUser = new PimpUserWrapper();

        if (!IsPostBack)
        {
            lblLastBuild.Text = "revision: " + SupportFramework.StaticContent.Versioned.LastBuild;
            spEmailMe.InnerText = "Email team@utopiapimp.com with any questions, comments or concerns.";
            lblOwnerID.Text += "<div id=\"divOwnerID\" style=\"Display:none;\">" + pimpUser.PimpUser.StartingKingdom.ToString() + "</div>";
            lblOwnerID.Text += "<div id=\"divUserID\" style=\"Display:none;\">" + pimpUser.PimpUser.UserID.ToString() + "</div>";
            lblOwnerID.Text += "<div id=\"divProvinceID\" style=\"Display:none;\">" + pimpUser.PimpUser.CurrentActiveProvince.ToString() + "</div>";

            ltIRCWeb.Text = "<span onclick=\"commonPopup('http://" + SupportFramework.StaticContent.URLClass.GetDomain + "/controls/IRC/Mibbit.aspx', '600', '525', 4, 'IRC_Chat_on_UtoNet');\" class='deleteButton'>IRC Web</span>";
            ltIRCJava.Text = "<span onclick=\"commonPopup('http://" + SupportFramework.StaticContent.URLClass.GetDomain + "/controls/IRC/Java/JavaIRC.aspx', '660', '525', 4, 'IRC_Java_Chat_on_UtoNet');\" class='deleteButton'>IRC Java</span>";
            ltCSS.Text = "<link href=\"http://codingforcharity.org/utopiapimp/css/default.css?v=" + SupportFramework.StaticContent.CSS.CssVersion + "\" rel=\"stylesheet\" type=\"text/css\" />";
        }

        List<ProvinceClass> getinfo = pimpUser.PimpUser.ProvincesOwned;
        if (getinfo != null)
        {
            ddlSelectProvince.DataSource = getinfo;
            ddlSelectProvince.DataBind();

            if (Profile.StartingProvince.Length > 2)
            {
                if (!IsPostBack)
                {
                    ddlSelectProvince.SelectedValue = Profile.StartingProvince;
                    if (ddlSelectProvince.SelectedValue == "" | ddlSelectProvince.SelectedValue == new Guid().ToString())
                    {
                        ddlSelectProvince.SelectedIndex = 0;
                        divAddData.Visible = false;
                    }
                    else
                    {
                        lblCurrentDate.Text = UtopiaParser.DisplayUtopianDateTime()[0] + "<script type='text/javascript'>GetTimeLeftInTick_Start('" + (DateTime.UtcNow.SecondsLeftInHour() - 2) + "');</script>";

                        if (pimpUser.PimpUser.MonarchType == MonarchType.owner | pimpUser.PimpUser.MonarchType == MonarchType.admin)
                            spMonarch.InnerText = "Kingdom Owner/Monarch";
                        else if (pimpUser.PimpUser.MonarchType == MonarchType.sub)
                            spMonarch.InnerText = "Sub-Monarch";
                    }
                }
            }
            else
            {
                divAddData.Visible = false;
                ddlSelectProvince.SelectedIndex = 0;
            }
        }

    }
    /// <summary>
    /// currently adds tabs dynamically, will be used to add kingdoms.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (!base.IsPostBack)
        {
            if (!ActionValidator.IsValid(ActionValidator.ActionTypeEnum.ReVisit))
                Response.End();
        }
        else
        {
            // Limit number of postbacks
            if (!ActionValidator.IsValid(ActionValidator.ActionTypeEnum.Postback))
                Response.End();
        }
        PimpUserWrapper pimpUser = new PimpUserWrapper();

        OwnedKingdomProvinces cachedKingdom = KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom);
        var type = pimpUser.getMonarchType(cachedKingdom);

        if (pimpUser.PimpUser.CurrentActiveProvince != new Guid() && pimpUser.PimpUser.StartingKingdom != new Guid())
        {
            PersonalKingdom(pimpUser, cachedKingdom);

            int i = GuestKingdoms(pimpUser.PimpUser.StartingKingdom, pimpUser, cachedKingdom);
            Session.Add("SubmittedData", "monType:" + pimpUser.PimpUser.MonarchType.ToString() + "i:" + i + "startKingdom:" + pimpUser.PimpUser.StartingKingdom);
            kingdomLessProvinces(i, pimpUser.PimpUser.StartingKingdom, pimpUser, cachedKingdom);

        }
    }
    private void PersonalKingdom(PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
    {

        StringBuilder sbTab = new StringBuilder();
        StringBuilder sbDiv = new StringBuilder();
        var kingdom = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom, currentUser.PimpUser.StartingKingdom, cachedKingdom);
        if (kingdom != null)
        {
            sbTab.Append("<li><a");
            if (Request.QueryString["t"] != null)
                if (Request.QueryString["t"].ToString() == "2")
                    sbTab.Append(" class=\"selected\"");
            sbTab.Append(" href=\"#tab2\">");
            sbTab.Append(kingdom.Kingdom_Island + ":" + kingdom.Kingdom_Location);
            sbTab.Append(" (H)</a></li>");
            sbDiv.Append("<div id=\"tab2\"><ul><li>Tools:</li>");
            sbDiv.Append("<li><a href=\"" + Page.ResolveUrl("~/members/Default.aspx") + "?kdid=" + kingdom.Kingdom_ID.RemoveDashes() + "&t=2\">Home</a></li>");
            //c is to remove the session states of CELastYear and CELastMonth
            sbDiv.Append("<li><a href=\"" + UtopiaParser.UtopiaKingdomPage + kingdom.Kingdom_Island + "/" + kingdom.Kingdom_Location + "\" target=\"_blank\">GoTo</a></li>");
            sbDiv.Append("<li><a href=\"" + Page.ResolveUrl("~/members/Contacts.aspx") + "\">Contacts</a></li>");
            sbDiv.Append("<li><a href=\"" + Page.ResolveUrl("~/members/CE.aspx") + "?kdid=" + kingdom.Kingdom_ID.RemoveDashes() + "&c=y&t=2\">CE/News</a></li>");
            sbDiv.Append("<li><a href=\"" + Page.ResolveUrl("~/members/Activity.aspx") + "?kdid=" + kingdom.Kingdom_ID.RemoveDashes() + "&kdty=Reg&t=2\">Activity-Log</a></li>");
            sbDiv.Append("<li><span onclick=\"commonPopup('http://" + SupportFramework.StaticContent.URLClass.GetDomain + "/controls/IRC/Mibbit.aspx', '600', '525', 4, 'IRC_Chat_on_UtoNet');\" class='deleteButton'>IRC Chat</span></li>");
            sbDiv.Append("</ul><ul><li>Columns:</li>");

            List<ColumnSet> getSets = currentUser.PimpUser.UserColumns;
            for (int i = 0; i < getSets.Count; i++)
                sbDiv.Append("<li><a href=\"" + Page.ResolveUrl("~/members/kd.aspx") + "?kdid=" + kingdom.Kingdom_ID.RemoveDashes() + "&kdty=Reg&st=" + getSets[i].setUid.ToString() + "&t=2\">" + getSets[i].setName + "</a></li>");

            sbDiv.Append("</ul></div>");
            ltTabs.Text += sbTab.ToString();
            ltDiv.Text += sbDiv.ToString();
        }
        else
        {
            Errors.failedAt("'PersonalKingdomNull'", currentUser.PimpUser.StartingKingdom.ToString() + ":" + (cachedKingdom == null ? true : false), currentUser.PimpUser.UserID);
            currentUser.clearStartingKingdom();
            Errors.failedAt("'PersonalKingdomNull'", currentUser.PimpUser.StartingKingdom.ToString() + ":" + (cachedKingdom == null ? true : false), currentUser.PimpUser.UserID);
        }
        //TODO: need to make sure the kingdom will never be null.
    }
    private int GuestKingdoms(Guid ownerKingdomID, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
    {
        //if the kingdoms are null return 0, throws an Error at GetOwnedKingdoms if Kingdoms are not null.
        if (ownerKingdomID == new Guid() && cachedKingdom != null && cachedKingdom.Kingdoms != null)
            return 0;

        StringBuilder sbTab = new StringBuilder();
        StringBuilder sbDiv = new StringBuilder();
        var GetOwnedKingdoms = (from UKI in cachedKingdom.Kingdoms.Where(x => x.Kingdom_ID != ownerKingdomID)
                                where UKI.Retired == false
                                select UKI).ToList();

        List<ColumnSet> getSets = currentUser.PimpUser.UserColumns;

        int i = 3;
        foreach (var item in GetOwnedKingdoms)
        {
            sbTab.Append("<li><a");
            if (!String.IsNullOrEmpty(Request.QueryString["t"]))
                if (Convert.ToInt32(Request.QueryString["t"].ToString()) == i)
                    sbTab.Append(" class=\"selected\"");
            sbTab.Append(" href=\"#tab" + i + "\">");

            sbTab.Append(item.Kingdom_Island + ":" + item.Kingdom_Location);
            sbTab.Append("</a></li>");
            sbDiv.Append("<div id=\"tab" + i + "\"><ul><li>Tools:</li>");
            sbDiv.Append("<li><a href=\"" + UtopiaParser.UtopiaKingdomPage + item.Kingdom_Island + "/" + item.Kingdom_Location + "\" target=\"_blank\">GoTo</a></li>");
            sbDiv.Append("<li><a href=\"" + Page.ResolveUrl("~/members/Default.aspx") + "?kdid=" + item.Kingdom_ID.RemoveDashes() + "&t=" + i + "\">At-a-glance</a></li>");
            sbDiv.Append("<li><a href=\"" + Page.ResolveUrl("~/members/CE.aspx") + "?c=y&kdid=" + item.Kingdom_ID.RemoveDashes() + "&t=" + i + "\">CE/News</a></li>");
            sbDiv.Append("<li><a href=\"" + Page.ResolveUrl("~/members/Activity.aspx") + "?kdid=" + item.Kingdom_ID.RemoveDashes() + "&kdty=Reg&t=" + i + "\">Activity-Log</a></li>");
            sbDiv.Append("</ul><ul><li>Columns:</li>");
            for (int j = 0; j < getSets.Count; j++)
                sbDiv.Append("<li><a href=\"" + Page.ResolveUrl("~/members/kd.aspx") + "?kdid=" + item.Kingdom_ID.RemoveDashes() + "&kdty=Reg&st=" + getSets[j].setUid.ToString() + "&t=" + i + "\">" + getSets[j].setName + "</a></li>");
            if (currentUser.PimpUser.MonarchType != MonarchType.none && currentUser.PimpUser.MonarchType != MonarchType.kdMonarch)
            {
                sbDiv.Append("</ul><ul><li>Monarch:</li>");
                sbDiv.Append("<li><span class=\"deleteButton\" onclick=\"RetireKingdom('" + ownerKingdomID.RemoveDashes() + "','" + item.Kingdom_ID.RemoveDashes() + "');\">Retire KD</span></li>");
            }
            sbDiv.Append("</ul></div>");
            i += 1;
        }
        ltTabs.Text += sbTab.ToString();
        ltDiv.Text += sbDiv.ToString();
        return i;
    }
    private void kingdomLessProvinces(int TabID, Guid ownerKingdomID, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
    {
        if (ownerKingdomID == new Guid())
            return;

        List<ColumnSet> getSets = currentUser.PimpUser.UserColumns;
        StringBuilder sbTab = new StringBuilder();
        StringBuilder sbDiv = new StringBuilder();
        sbTab.Append("<li><a");
        if (!String.IsNullOrEmpty( Request.QueryString["t"]) )
            if (Convert.ToInt32(Request.QueryString["t"].ToString()) == TabID)
                sbTab.Append(" class=\"selected\"");
        sbTab.Append(" href=\"#tab" + TabID + "\">");
        var randCount = cachedKingdom.RandomProvinceCount;
        if (randCount == 0)
            sbTab.Append("KD-less Provs (?)");
        else
            sbTab.Append("KD-less Provs (" + randCount + ")");
        sbTab.Append("</a></li>");
        sbDiv.Append("<div id=\"tab" + TabID + "\"><ul><li>Tools:</li>");
        sbDiv.Append("<li><a href=\"" + Page.ResolveUrl("~/members/Activity.aspx") + "?kdid=" + ownerKingdomID.RemoveDashes() + "&kdty=Random&t=" + TabID + "\">Activity-Log</a></li>");
        sbDiv.Append("</ul><ul><li>Columns:</li>");
        for (int j = 0; j < getSets.Count; j++)
            sbDiv.Append("<li><a href=\"" + Page.ResolveUrl("~/members/kd.aspx") + "?kdid=" + ownerKingdomID.RemoveDashes() + "&kdty=Random&st=" + getSets[j].setUid.ToString() + "&t=" + TabID + "\">" + getSets[j].setName + "</a></li>");

        if (currentUser.PimpUser.MonarchType != MonarchType.none && currentUser.PimpUser.MonarchType != MonarchType.kdMonarch)
        {
            sbDiv.Append("</ul><ul><li>Monarch:</li>");
            sbDiv.Append("<li><a href=\"" + Page.ResolveUrl("~/members/monarchs.aspx") + "?id=ot\">Time Limit</a></li>");
        }
        sbDiv.Append("</ul></div>");
        ltTabs.Text += sbTab.ToString();
        ltDiv.Text += sbDiv.ToString();
    }
    private void AdminProvinces(int TabID, Guid userID, PimpUserWrapper user)
    {
        List<ColumnSet> getSets = user.PimpUser.UserColumns;
        StringBuilder sbTab = new StringBuilder();
        StringBuilder sbDiv = new StringBuilder();

        sbTab.Append("<li><a");
        if (!String.IsNullOrEmpty(Request.QueryString["t"]))
            if (Convert.ToInt32(Request.QueryString["t"].ToString()) == TabID)
                sbTab.Append(" class=\"selected\"");

        sbTab.Append(" href=\"#tab" + TabID + "\">");
        sbTab.Append("Admin Provs");
        sbTab.Append("</a></li>");
        sbDiv.Append("<div id=\"tab" + TabID + "\"><ul><li>Columns:</li>");
        for (int j = 0; j < getSets.Count; j++)
            sbDiv.Append("<li><a href=\"" + Page.ResolveUrl("~/members/kd.aspx") + "?kdid=" + new Guid().RemoveDashes() + "&kdty=myaddy&st=" + getSets[j].setUid.ToString() + "&t=" + TabID + "\">" + getSets[j].setName + "</a></li>");

        sbDiv.Append("</ul></div>");
        ltTabs.Text += sbTab.ToString();
        ltDiv.Text += sbDiv.ToString();
    }
    protected void ddlSelectProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        PimpUserWrapper pimpUser = new PimpUserWrapper();
        if (ddlSelectProvince.SelectedValue == "")
            pimpUser.changeCurrentActiveProvinceId(new Guid(), string.Empty);
        else
            pimpUser.changeCurrentActiveProvinceId(new Guid(ddlSelectProvince.SelectedValue), ddlSelectProvince.SelectedItem.Text);

        Response.Redirect(Page.Request.Url.AbsoluteUri);
    }

    protected void lbRemoveProvince_Click(object sender, EventArgs e)
    {
        if (ddlSelectProvince.SelectedItem.Value.Length > 5)
        {
            PimpUserWrapper pimpUser = new PimpUserWrapper();
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var query = (from UPI in db.Utopia_Province_Data_Captured_Gens
                         where UPI.Owner_User_ID == pimpUser.PimpUser.UserID && UPI.Province_ID == new Guid(ddlSelectProvince.SelectedItem.Value)
                         select UPI).FirstOrDefault();
            if (query != null)
            {
                query.Owner_User_ID = null;
                db.SubmitChanges();
            }
            ddlSelectProvince.SelectedIndex = 0;
            if (ddlSelectProvince.SelectedValue == "")
                Profile.SetPropertyValue("StartingProvince", new Guid().ToString());
            else
                Profile.SetPropertyValue("StartingProvince", ddlSelectProvince.SelectedValue);
            pimpUser.RemoveSessionProvinceInfo();

        }
        Response.Redirect(Page.Request.Url.AbsoluteUri);
    }

    protected void LoginStatus1_LoggingOut(object sender, LoginCancelEventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Session.RemoveAll();

    }
    protected void LoginStatus1_LoggingIn(object sender, LoginCancelEventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Session.RemoveAll();

    }

    //#region IMasterPage Members
    /// <summary>
    /// dictionary items to be passed around...
    /// </summary>
    //public Dictionary<string, object> DataObjects
    //{
    //    get
    //    {
    //        if (objectsForPassing != null)
    //            return objectsForPassing;

    //        var currentUser = pimpUser.PimpUser.getUser();
    //        objectsForPassing = new Dictionary<string, object>();
    //        objectsForPassing.Add("ownerKingdomID", currentUser.PimpUser.StartingKingdom);
    //        objectsForPassing.Add("currentUser", currentUser);
    //        objectsForPassing.Add("monType", monType);
    //        return objectsForPassing;
    //    }
    //}

    //#endregion
}
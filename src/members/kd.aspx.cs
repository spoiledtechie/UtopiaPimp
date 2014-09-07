using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using Boomers.Utilities.DatesTimes;
using Boomers.Utilities.Text;
using Pimp.UParser;
using Pimp.UIBuilder;
using Pimp.UCache;
using PimpLibrary.Static.Enums;
using Pimp;
using Pimp.Users;
using Pimp.Utopia;
using Pimp.UData;

public partial class members_kd : MyBasePageCS
{
    PimpUserWrapper currentUser;


    protected void Page_Load(object sender, EventArgs e)
    {
        currentUser = new PimpUserWrapper();

        if (currentUser.PimpUser.CurrentActiveProvince != new Guid() & currentUser.PimpUser.ProvincesOwned.Count > 0)
        {
            if (currentUser.PimpUser.StartingKingdom != null && HttpContext.Current.Request.QueryString["kdid"] != null)
            {
                int setID = 0;
                if (HttpContext.Current.Request.QueryString["st"] != null)
                    setID = Convert.ToInt32(HttpContext.Current.Request.QueryString["st"]);

                switch (!IsPostBack)
                {
                    case true:
                        if (HttpContext.Current.Request.QueryString["cs"] != null)
                            ddlColumnList.SelectedValue = URegEx.rgxNumber.Matches(HttpContext.Current.Request.QueryString["cs"].ToString())[URegEx.rgxNumber.Matches(HttpContext.Current.Request.QueryString["cs"].ToString()).Count - 1].Value;

                        ltKI.Text = HttpContext.Current.Request.QueryString["kdid"].ToString();
                        try { Guid test = new Guid(ltKI.Text); }
                        catch { Response.Redirect("Default.aspx"); }
                        DateTime start = DateTime.UtcNow;
                        //jsTest.InnerHtml = "<script type=\"text/javascript\">$(document).ready(function() {$(\"#tblKingdomInfo\").tablesorter({widgets: ['zebra'], widgetZebra: { css: ['d0', 'd1']} }).tableHover({ clickFunction: 'IdentifyProvince', clickClass: 'click' });    });    </script>";
                        OwnedKingdomProvinces cachedKingdom = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom);
                        if (HttpContext.Current.Request.QueryString["kdty"] == null)
                            Response.Redirect("Default.aspx");
                        switch (HttpContext.Current.Request.QueryString["kdty"].ToString())
                        {
                            case "Random":
                            case "myaddy":
                            case "RandomFilter":
                            case "RandomAdminFilter":
                                btnStaFilter.Attributes.Add("onclick", "FilterStaValues('" + ddlStaNetworth.ClientID + "', '" + ddlStaAcres.ClientID + "', '" + ddlStaLastUpdated.ClientID + "', '" + setID + "');");
                                btnInpFilter.Attributes.Add("onclick", "FilterInpValues('tbInpNetworthStart', 'tbInpNetworthEnd', 'tbInpAcresStart', 'tbInpAcresEnd', 'tbInpLUStart', 'tbInpLUEnd', '" + setID + "');");
                                divFilter.Visible = true;
                                break;
                            default:
                                ltJavascript.Text += "<div id=\"divKingdomID\" style=\"display: none;\">" + HttpContext.Current.Request.QueryString["kdid"].ToString() + "</div>";
                                var uki = cachedKingdom.Kingdoms.Where(x => x.Kingdom_ID == new Guid(HttpContext.Current.Request.QueryString["kdid"].ToString())).FirstOrDefault();

                                if (uki != null)
                                {
                                    if (uki.Updated_By_DateTime.HasValue)
                                        lblTimer.Text += "  Kingdom last updated: " + Convert.ToDateTime(uki.Updated_By_DateTime.Value).ToRelativeDate();
                                    divSummary.Visible = true;
                                    StringBuilder sb = new StringBuilder();
                                    sb.Append("<span class=\"Title\">" + uki.Kingdom_Name + " (" + uki.Kingdom_Island + ":" + uki.Kingdom_Location + ")</span> Win/Loss: " + uki.War_Wins.GetValueOrDefault(0) + "/" + uki.War_Losses.GetValueOrDefault(0) + "<br/>");
                                    sb.Append("<br/><div id=\"divMonarchMess\"><div class=\"divTitles\">" + uki.Kingdom_Message + "</div>");


                                    if (currentUser.PimpUser.MonarchType != MonarchType.none && currentUser.PimpUser.MonarchType != MonarchType.kdMonarch)
                                        sb.Append("<div id=\"loadMonarchInfo\"><input id=\"btnChangeMessage\" onclick=\"javascript:UpdateMonarchMessage();\" type=\"button\" value=\"Change Monarch Message\" /></div>");

                                    sb.Append("</div>");
                                    //sb.Append("Stance: " + getLastUpdated.stance + " (" + getLastUpdated.alt + ")");
                                    divSummary.InnerHtml = sb.ToString();
                                }
                                break;
                        }
                        divOpTimeLimit.InnerHtml = "Ops/Attacks done in last " + -1 * cachedKingdom.KdOpsAttacksTimeLimit.GetValueOrDefault(-24) + " hours (mouseover the entries for details)";
                        //Shows the button to change the ops time.
                        if (currentUser.PimpUser.MonarchType != MonarchType.none && currentUser.PimpUser.MonarchType != MonarchType.kdMonarch)
                            divOpTimeLimit.InnerHtml += " <a href=\"monarchs.aspx?id=ot\">Change Time</a>";

                        break;
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
                ddlColumnList.Visible = false;
                btnExport.Visible = false;
            }
        }
    }
    /// <summary>
    /// Sort by Drop Down.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlColumnList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlColumnList.SelectedValue == "CLEAR")
            Response.Redirect("kd.aspx?kdid=" + HttpContext.Current.Request.QueryString["kdid"].ToString() + "&kdty=" + HttpContext.Current.Request.QueryString["kdty"].ToString() + "&st=" + HttpContext.Current.Request.QueryString["st"].ToString() + "&t=" + HttpContext.Current.Request.QueryString["t"].ToString());

        int setID = 0;

        if (HttpContext.Current.Request.QueryString["st"] != null)
            setID = Convert.ToInt32(HttpContext.Current.Request.QueryString["st"]);
        string getColumns = UtopiaParser.GetUserColumnsSet(setID, currentUser.PimpUser.UserColumns);

        if (HttpContext.Current.Request.QueryString["cs"] != null)
            getColumns += ":" + HttpContext.Current.Request.QueryString["cs"].ToString();

        getColumns += ":" + ddlColumnList.SelectedValue;

        if (HttpContext.Current.Request.QueryString["cs"] == null)
            Response.Redirect(Request.RawUrl + "&cs=:" + ddlColumnList.SelectedValue);
        else
            Response.Redirect(Request.RawUrl + ":" + ddlColumnList.SelectedValue);

    }
    /// <summary>
    /// Calculates the Honor Bonus to be added to the real thing.
    /// </summary>
    /// <param name="sb">StringBuilder</param>
    /// <param name="level">level of Nobility the province has.</param>
    /// <returns></returns>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        OwnedKingdomProvinces cachedKingdom = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom);
        Response.Clear();
        //opens the confirmation window with default file name.
        Response.AddHeader("Content-Disposition", "inline; filename=\"Utopia" + DateTime.UtcNow.ToyyyyMMdd() + ".xls\"");

        Response.ContentType = "application/vnd.ms-excel";
        Response.ContentEncoding = System.Text.Encoding.Default;
        Response.Charset = "";

        EnableViewState = false;
        string columns = UtopiaParser.GetUserColumnsSet(Convert.ToInt32(HttpContext.Current.Request.QueryString["st"].ToString()), currentUser.PimpUser.UserColumns);
        if (HttpContext.Current.Request.QueryString["cs"] != null)
            columns += HttpContext.Current.Request.QueryString["cs"].ToString();
        Response.Write(KdPage.loadDynamicGrid(new Guid(HttpContext.Current.Request.QueryString["kdid"].ToString()), HttpContext.Current.Request.QueryString["kdty"].ToString(), columns, currentUser.PimpUser.StartingKingdom, currentUser.PimpUser.MonarchType, currentUser.PimpUser.UserID, cachedKingdom));
        Response.End();
    }

    private static string LoadFilterKdLessProvs()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<script type=\"text/javascript\">$(function(){$('#tabs').tabs();});</script>");
        sb.Append("<div id=\"tabs\"><div style=\"font-weight:bold;\">Kingdom-less Provinces</div> You can use the below settings to filter the kingdomless provinces. <br/>");
        sb.Append("<ul>");
        sb.Append("<li><a href=\"#tabs-1\">Standard</a></li>");
        sb.Append("<li><a href=\"#tabs-2\">Input Values</a></li>");
        sb.Append("</ul>");
        sb.Append("<div id=\"tabs-1\"><table><tr><td>Networth: </td><td>Acres:</td></tr><tr><td>Last Updated: </td></tr></table></div>");
        sb.Append("<div id=\"tabs-2\"><table><tr><td>Networth:</td><td>Acres:</td></tr><tr><td>Last Updated: </td></tr></table></div>");
        sb.Append("</div>");

        return sb.ToString();
    }
}

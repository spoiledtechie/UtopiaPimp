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
using Boomers.Utilities.Data;
using System.Text;
using Boomers.Utilities.Guids;

using Pimp.UParser;
using Pimp.UCache;
using PimpLibrary.Static.Enums;
using Pimp;
using Pimp.Users;
using Pimp.Utopia;
using Pimp.UIBuilder;
using Pimp.UData;

public partial class members_monarchs : MyBasePageCS
{
    PimpUserWrapper currentUser;


    protected void Page_Load(object sender, EventArgs e)
    {
        currentUser = new PimpUserWrapper();

        if (currentUser.PimpUser.MonarchType == MonarchType.none || currentUser.PimpUser.MonarchType == MonarchType.kdMonarch)
            Response.Redirect("default.aspx");

        //Displays the current Tab.
        switch (!IsPostBack)
        {
            case true:
                StringBuilder sb = new StringBuilder();
                var cachedKingdom = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom);
                sb.Append("<li><a");
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "pc")
                    sb.Append(" class=\"selected\"");
                sb.Append(" href=\"#tab1Monarch\">Province Codes</a></li>");
                sb.Append("<li><a");
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "kl")
                    sb.Append(" class=\"selected\"");
                sb.Append(" href=\"#tab2Monarch\">Retire Kingdoms</a></li>");
                sb.Append("<li><a");
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "rp")
                    sb.Append(" class=\"selected\"");
                sb.Append(" href=\"#tab3Monarch\">Remove Provinces</a></li>");
                sb.Append("<li><a");
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "am")
                    sb.Append(" class=\"selected\"");
                sb.Append(" href=\"#tab4Monarch\">Sub Monarchs</a></li>");
                sb.Append("<li><a");
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "api")
                    sb.Append(" class=\"selected\"");
                sb.Append(" href=\"#tab5Monarch\">API Keys</a></li>");
                sb.Append("<li><a");
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "ot")
                    sb.Append(" class=\"selected\"");
                sb.Append(" href=\"#tab6Monarch\">Other</a></li>");
                sb.Append("<li><a");
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "irc")
                    sb.Append(" class=\"selected\"");
                sb.Append(" href=\"#tab7Monarch\">IRC Settings</a></li>");

                ltTabsMonarch.Text = sb.ToString();

                DisplayProvincestoDelete(currentUser.PimpUser.StartingKingdom, cachedKingdom);
                DisplayKingdomLists(currentUser.PimpUser.StartingKingdom);
                ltProvinceCodes.Text = FrontPage.DisplayProvinceCodesCache(currentUser.PimpUser.StartingKingdom, cachedKingdom);
                DisplaySubMonarchList(currentUser.PimpUser.StartingKingdom, cachedKingdom);
                DisplayMonarchOthers(currentUser.PimpUser.StartingKingdom, cachedKingdom);
                DisplayAPIKeys(currentUser.PimpUser.StartingKingdom, currentUser);
                ltIRC.Text = ListIRCChannels(currentUser.PimpUser.StartingKingdom, cachedKingdom);
                break;
        }
    }
    public void DisplayKingdomLists(Guid ownerKingdomId)
    {
        StringBuilder sb = new StringBuilder();
        var kingdoms = Kingdom.getOwnedKingdoms(ownerKingdomId);
        sb.Append("<table>");
        sb.Append("<tr><th>Retire Kingdom</th><th>Kingdom Name</th><th>Status information for Kingdom</th></tr>");
        for (int i = 0; i < kingdoms.Count; i++)
        {
            sb.Append("<tr>");
            if (kingdoms[i].Retired)
                sb.Append("<td><span class=\"deleteButton\" id=\"RetireKingdom" + kingdoms[i].Kingdom_ID.RemoveDashes() + "\" onclick=\"RetireKingdomMulti('" + kingdoms[i].Owner_Kingdom_ID.RemoveDashes() + "','" + kingdoms[i].Kingdom_ID.RemoveDashes() + "');\">Un-Retire</span></td>");
            else
                sb.Append("<td><span class=\"deleteButton\" id=\"RetireKingdom" + kingdoms[i].Kingdom_ID.RemoveDashes() + "\" onclick=\"RetireKingdomMulti('" + kingdoms[i].Owner_Kingdom_ID.RemoveDashes() + "','" + kingdoms[i].Kingdom_ID.RemoveDashes() + "');\">Retire</span></td>");
            sb.Append("<td>" + kingdoms[i].Kingdom_Name + " " + "(" + kingdoms[i].Kingdom_Island + ":" + kingdoms[i].Kingdom_Location + ")" + "</td>");
            sb.Append("<td><input type=\"text\" id=\"status" + kingdoms[i].Kingdom_ID.RemoveDashes() + "\" value=\"" + kingdoms[i].Kingdom_Message + "\" />   <input type=\"button\" onclick=\"UpdateKingdomStatus('" + kingdoms[i].Owner_Kingdom_ID.RemoveDashes() + "','" + kingdoms[i].Kingdom_ID.RemoveDashes() + "');\" id=\"updateStatus" + kingdoms[i].Kingdom_ID.RemoveDashes() + "\"  value =\"Update Status\" /></td>");
  
            sb.Append("</tr>");
        }
        sb.Append("</table>");
        ltKingdomLists.Text = sb.ToString();

    }

    public static string ListIRCChannels(Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<div>These are the Default Channels Your Kingdom Mates will be placed in when Logging into IRC via UtopiaPimp</div>");
        sb.Append("<ul class=\"ulList ulListExtraPadding\" id='ulIRCChannels'>");
        sb.Append("<li>Add Channel: <input type='text' id='inAddChannel' /> Password:<input type='text' id='inAddChannelPassword' /><input type='button' value='Add Channel' onclick=\"javascript:AddIRCChannel();\"/> - NO NEED for the # Sign in the channel Name</li>");
        var list = IrcCache.getKingdomIRCChannels(ownerKingdomID, cachedKingdom);
        sb.Append("<li><br/>List of IRC Channels for your Kingdom<br/><br/></li>");
        for (int i = 0; i < list.Count; i++)
        {
            sb.Append("<li>Channel: #" + list[i].Channel);
            if (list[i].ChannelPassword != string.Empty && list[i].ChannelPassword != null)
                sb.Append(" - Password: " + list[i].ChannelPassword);
            sb.Append(" - <span name='" + list[i].Channel + "' onclick=\"javascript:DeleteIRCChannel(this,'" + list[i].Channel + "');\" class='deleteButton'>Remove</span></li>");
        }

        sb.Append("</ul>");
        return sb.ToString();
    }
    public void DisplayAPIKeys(Guid ownerKingdomID, PimpUserWrapper currentUser)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<div>API Found Here: <a href='http://utopiapimp.com/PullData.ashx'>http://utopiapimp.com/PullData.ashx</a></div>");
        sb.Append("<ul class=\"ulList\">");
        sb.Append("<li>Kingdoms API Key: " + ownerKingdomID.RemoveDashes() + "</li>");
        sb.Append("<li>Kingdoms Secret Key: " + currentUser.PimpUser.CurrentActiveProvince.RemoveDashes() + "</li>");
        sb.Append("</ul>");
        ltAPIKeys.Text = sb.ToString();
    }
    public void DisplaySubMonarchList(Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
    {
        var getMonarchs = cachedKingdom.Provinces.Where(x => x.Kingdom_ID == ownerKingdomID).OrderBy(x => x.Province_Name).ToList();

        StringBuilder sb = new StringBuilder();
        sb.Append("<ul class=\"ulList\">");
        for (int i = 0; i < getMonarchs.Count; i++)
        {
            sb.Append("<li><span ");
            if (getMonarchs[i].Owner == 1)
            {
                sb.Append(" id=\"mon" + i + "\"");
                if (getMonarchs[i].Owner_User_ID != null && getMonarchs[i].Owner_User_ID != new Guid())
                    sb.Append(" class=\"deleteButton\" onclick=\"javascript:alert('You CANNOT remove the owner of the kingdom.');\"");
                sb.Append("><img class=\"imgLinks\" src=\"http://codingforcharity.org/utopiapimp/img/icons/on.png\">    " + getMonarchs[i].Province_Name + " (O)</span></li>");
            }
            else if (getMonarchs[i].Sub_Monarch == 1)
            {
                sb.Append(" id=\"mon" + i + "\"");
                if (getMonarchs[i].Owner_User_ID != null && getMonarchs[i].Owner_User_ID != new Guid())
                    sb.Append(" class=\"deleteButton\" onclick=\"javascript:AssignMonarch(this, '" + getMonarchs[i].Province_ID + "', '" + getMonarchs[i].Province_Name + "');\"");
                sb.Append("><img class=\"imgLinks\" src=\"http://codingforcharity.org/utopiapimp/img/icons/on.png\">    " + getMonarchs[i].Province_Name + " (SM)</span></li>");
            }
            else
            {
                sb.Append(" id=\"mon" + i + "\"");
                if (getMonarchs[i].Owner_User_ID != null && getMonarchs[i].Owner_User_ID != new Guid())
                    sb.Append(" class=\"deleteButton\" onclick=\"javascript:AssignMonarch(this, '" + getMonarchs[i].Province_ID + "', '" + getMonarchs[i].Province_Name + "');\"");
                sb.Append("  ><img class=\"imgLinks\" src=\"http://codingforcharity.org/utopiapimp/img/icons/off.png\">   " + getMonarchs[i].Province_Name + " </span></li>");
            }
        }
        sb.Append("</ul>");
        ltSubMonarchs.Text = sb.ToString();
    }
    public void DisplayProvincestoDelete(Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
    {
        var getSignedUpProvinces = (from UPII in cachedKingdom.Provinces.Where(x => x.Kingdom_ID == ownerKingdomID)
                                    where UPII.Owner_User_ID != null
                                    where UPII.Owner_User_ID != new Guid()
                                    select new
                                    {
                                        UPII.Province_ID,
                                        UPII.Province_Name,
                                    });

        gvDeleteProvince.DataSource = getSignedUpProvinces;
        gvDeleteProvince.DataBind();
    }
    protected void gvDeleteProvince_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        //handles the gridview button review in error handling gridview.
        //checks to see the command name first.
        //then updates the item in the sql table which places a 1 at the item of interest.
        if (e.CommandName == "cmdDelete")
        {
            PimpUserWrapper pimpUser = new PimpUserWrapper();
            var cachedKingdom = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom);
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var getProvinceInfo = (from tel in db.Utopia_Province_Data_Captured_Gens
                                   where tel.Province_ID == new Guid(e.CommandArgument.ToString())
                                   select tel).FirstOrDefault();
            if (getProvinceInfo != null)
            {
                Guid oldOwner = getProvinceInfo.Owner_User_ID.GetValueOrDefault();

                getProvinceInfo.Owner_User_ID = null;
                db.SubmitChanges();
            }
            KingdomCache.removeProvinceFromKingdomCache(currentUser.PimpUser.StartingKingdom, new Guid(e.CommandArgument.ToString()), cachedKingdom);

            pimpUser.removeUserCache();
            DisplayProvincestoDelete(currentUser.PimpUser.StartingKingdom, cachedKingdom);
        }
    }


    public void DisplayMonarchOthers(Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<ul class=\"ulList\">");

        int limit = -5;
        if (cachedKingdom != null)
            limit = cachedKingdom.KdProvTimeLimit;
        sb.Append("<li>Time Limit on Kd-Less Provinces: <input id=\"inKdProv\" value=\"" + limit + "\" class=\"txtbxCharacterCount\" type=\"text\" /> days<input id=\"btnKdProv\" type=\"button\" onclick=\"javascript:SetKdTimeLimit('inKdProv');\" value=\"Change Time\" /><div id=\"divMonarchTimeLimit\"></div></li>");

        int opsAttackLimit = -24;
        if (cachedKingdom != null && cachedKingdom.KdOpsAttacksTimeLimit.HasValue)
            opsAttackLimit = cachedKingdom.KdOpsAttacksTimeLimit.Value;
        sb.Append("<li>Time Limit on Ops and Attacks: <input id=\"inKdOpAttack\" value=\"" + opsAttackLimit + "\" class=\"txtbxCharacterCount\" type=\"text\" /> hours<input id=\"btnOpAttackTime\" type=\"button\" onclick=\"javascript:SetKdOpAttackTimeLimit('inKdOpAttack');\" value=\"Change Time\" /><div id=\"divMonarchOpAttackTime\"></div></li>");
        sb.Append("</ul>");

        ltKdLessMonarchs.Text = sb.ToString();
    }
}

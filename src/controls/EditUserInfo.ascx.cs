using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Pimp.UCache;
using Pimp;
using Pimp.Users;
using Boomers.UserUtil;
using Pimp.UData;

public partial class controls_EditUserInfo : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
                        StringBuilder sbPN = new StringBuilder();
            StringBuilder sbCN = new StringBuilder();
            List<int> year = new List<int>();
            for (int i = 2010; i > 1900; i--)
                year.Add(i);
            ddlYear.DataSource = year;
            ddlYear.DataBind();

            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            var cachedKingdom = KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom);
            var getUserInfo = UsersCache.GetContact(pimpUser.PimpUser.StartingKingdom, pimpUser, cachedKingdom);
            
            lblUserName.Text =SupportFramework.Users.Memberships.getUserName();
            if (getUserInfo != null)
            {
                tbCity.Value = getUserInfo.City;
                tbCountry.Value = getUserInfo.Country;
                tbNickName.Value = getUserInfo.Nick_Name;
                tbState.Value = getUserInfo.State;
                tbNotes.Text = getUserInfo.Notes;
                if (Convert.ToInt32(getUserInfo.GMT_Offset) > 0)
                    ddlGMT.SelectedValue = "+" + getUserInfo.GMT_Offset;
                else
                    ddlGMT.SelectedValue = getUserInfo.GMT_Offset;

                if (getUserInfo.DOB.HasValue)
                {
                    ddlDay.SelectedValue = getUserInfo.DOB.Value.Day.ToString();
                    ddlMonth.SelectedValue = getUserInfo.DOB.Value.Month.ToString();
                    ddlYear.SelectedValue = getUserInfo.DOB.Value.Year.ToString();
                }
            }
            btnSave.Attributes.Add("onclick", "SaveContactInfo('" + tbCity.ClientID + "','" + tbCountry.ClientID + "','" + ddlGMT.ClientID + "','" + tbNickName.ClientID + "','" + tbState.ClientID + "','" + ddlDay.ClientID + "','" + ddlMonth.ClientID + "','" + ddlYear.ClientID + "', '" + tbNotes.ClientID + "');");

            sbPN.Append("<table id='tblPN'>");

            var getPhoneTypes = UsersData.GetPhoneTypes;
            if (getUserInfo != null)
                if (getUserInfo.phoneNumbers.Count > 0)
                    for (int i = 0; i < getUserInfo.phoneNumbers.Count; i++)
                        WritePhoneNumbers(sbPN, getPhoneTypes, getUserInfo.phoneNumbers, i);
                else
                    WritePhoneNumbers(sbPN, getPhoneTypes, null, 0);
            else
                WritePhoneNumbers(sbPN, getPhoneTypes, null, 0);

            sbPN.Append("</table><a href=\"javascript:addPNRow('tblPN')\">Add Another</a>");
            ltPH.Text = sbPN.ToString();

            var getIMTypes = IM.GetIMTypes;

            sbCN.Append("<table id='tblCN'>");
            if (getUserInfo != null)
                if (getUserInfo.imNames.Count > 0)
                    for (int i = 0; i < getUserInfo.imNames.Count; i++)
                        WriteIMNames(sbCN, getIMTypes, getUserInfo.imNames, i);
                else
                    WriteIMNames(sbCN, getIMTypes, null, 0);
            else
                WriteIMNames(sbCN, getIMTypes, null, 0);
            sbCN.Append("</table><a href=\"javascript:addIMRow('tblCN')\">Add Another</a> <br/> (adding an IRC name allows auto identify with Pimps IRC client)");
            ltCN.Text = sbCN.ToString();
                    }
    }

    private static void WriteIMNames(StringBuilder sbCN, List<IMType> getIMTypes, List<IMType> getIMs, int i)
    {
        sbCN.Append("<tr ");
        //if (getIMs.Count > 0)
        sbCN.Append("id='trCN" + i + "'");
        sbCN.Append(" ><td><input id='");
        sbCN.Append("tbCN" + i);
        sbCN.Append("' type='text'");
        if (getIMs != null)
            sbCN.Append(" value='" + getIMs[i].IM_Name + "'");
        sbCN.Append(" /></td><td><select id='");
        sbCN.Append("ddlCN" + i);
        sbCN.Append("'>");
        for (int j = 0; j < getIMTypes.Count; j++)
        {
            sbCN.Append("<option");
            if (getIMs != null)
                if (getIMTypes[j].IM_Type == getIMs[i].IM_Type)
                    sbCN.Append(" selected='yes'");
            sbCN.Append(" value='" + getIMTypes[j].IM_Type + "'>" + getIMTypes[j].IM_Type + "</option>");
        }
        sbCN.Append("</select></td>");

        //sbCN.Append("<td>Pass: <input id='");
        //sbCN.Append("tbCNPass" + i);
        //sbCN.Append("' type='text'");
        //if (getIMs != null)
        //    sbCN.Append(" value='" + getIMs[i].IM_Password+ "'");
        //sbCN.Append(" /></td>");

        sbCN.Append("<td>Password?<input id='");
        sbCN.Append("cbCN" + i);
        sbCN.Append("' type='checkbox'");

        if (getIMs != null && getIMs[i] != null)
            if (getIMs[i].IM_Password_Bool == 1)
                sbCN.Append(" checked='on'");
        sbCN.Append(" /></td><td>");
        if (getIMs != null)
            sbCN.Append("<a href=\"javascript:DeleteIMRow('" + getIMs[i].uid + "', '" + i + "')\">Delete</a>");
        sbCN.Append("</td></tr>");
    }

    private static void WritePhoneNumbers(StringBuilder sbPN, List<PhoneType> getPhoneTypes, List<PhoneType> getPhoneNumbers, int i)
    {
        sbPN.Append("<tr id='");
        sbPN.Append("trPN" + i);
        sbPN.Append("' ><td><input id='");
        sbPN.Append("tbPN" + i);
        sbPN.Append("' type='text'");
        if (getPhoneNumbers != null)
            sbPN.Append(" value='" + getPhoneNumbers[i].PhoneNumber + "'");
        sbPN.Append(" /></td><td>Type: <select id='");
        sbPN.Append("ddlPN" + i);
        sbPN.Append("'>");

        for (int j = 0; j < getPhoneTypes.Count; j++)
        {
            sbPN.Append("<option");
            if (getPhoneNumbers != null)
                if (getPhoneTypes[j].Phone_Type == getPhoneNumbers[i].Phone_Type)
                    sbPN.Append(" selected='yes'");
            sbPN.Append(" value='" + getPhoneTypes[j].Phone_Type + "'>" + getPhoneTypes[j].Phone_Type + "</option>");
        }
        sbPN.Append("</select></td><td> Text Messages? <input id='");
        sbPN.Append("cbPN" + i);
        sbPN.Append("' type='checkbox'");
        if (getPhoneNumbers != null)
            if (getPhoneNumbers[i].SMS == 1)
                sbPN.Append(" checked='on'");
        sbPN.Append(" /><td>");
        if (getPhoneNumbers != null)
            sbPN.Append("<a href=\"javascript:DeletePNRow('" + getPhoneNumbers[i].uid + "', '" + i + "')\">Delete</a>");
        sbPN.Append("</td></tr>");
    }

}


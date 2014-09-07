using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Pimp.UCache;
using Pimp;
using Pimp.Users;
using Pimp.Utopia;
using Boomers.UserUtil;
using Pimp.UData;

public partial class members_Contacts : MyBasePageCS
{
    
    PimpUserWrapper  currentUser;

    protected void Page_Load(object sender, EventArgs e)
    {
        currentUser = new PimpUserWrapper ();

        if (!IsPostBack)
        {
            OwnedKingdomProvinces cachedKingdom = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom);
            CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
            if (currentUser.PimpUser.IsUserAdmin)
            {
                var countUsers = (from yy in adb.user_Informations
                                  select yy).Count();
                var countPhone = (from yy in adb.user_Phone_Numbers
                                  select yy).Count();
                var countIms = (from yy in adb.user_IMs
                                select yy).Count();
                jsTest.InnerHtml += countUsers + " users, " + countPhone + " phone Numbers, " + countIms + " Im Names";
            }
            List<ProvinceClass> getProvinces = cachedKingdom.Provinces.Where(x => x.Kingdom_ID == currentUser.PimpUser.StartingKingdom).Where(x => x.Owner_User_ID != null).ToList();
            List<Contact> contacts =KingdomCache.getContactsForKingdom(currentUser.PimpUser.StartingKingdom, cachedKingdom);

            if (contacts != null)
            {
                StringBuilder sb = new StringBuilder();
                int provCount = cachedKingdom.ProvincesWithoutUserContactsAdded.Count;
                if (provCount > 0)
                    sb.Append("<div class=\"center\">" + (getProvinces.Count - contacts.Count) + " Provinces have NOT entered their information.</div>");
                sb.Append("<table class=\"tblKingdomInfo center tblExpand watchRightAd\" id=\"tblUserInfo\">");

                sb.Append("<thead><tr>");
                sb.Append("<th>Province Name</th>");
                //sb.Append("<th>User Name</th>");
                sb.Append("<th>Nick Name</th>");
                sb.Append("<th>Country</th>");
                sb.Append("<th>State</th>");
                sb.Append("<th>City</th>");
                sb.Append("<th>GMT Offset</th>");
                sb.Append("<th>Phone Numbers</th>");
                sb.Append("<th>Screen Names</th>");
                sb.Append("<th>Notes</th>");
                sb.Append("</thead><tbody>");
                for (int i = 0; i < contacts.Count; i++)
                {
                    if (getProvinces != null &&  getProvinces.Where(x => x.Owner_User_ID == contacts[i].user_ID).FirstOrDefault() != null)
                    {
                        switch (i % 2)
                        {
                            case 0:
                                sb.Append("<tr class=\"d0\">");
                                break;
                            case 1:
                                sb.Append("<tr class=\"d1\">");
                                break;
                        }

                        sb.Append("<td>" + getProvinces.Where(x => x.Owner_User_ID == contacts[i].user_ID).FirstOrDefault().Province_Name + "</td>");
                        //sb.Append("<td>" + getUserInfo[i].UserName + "</td>");
                        sb.Append("<td>" + contacts[i].Nick_Name + "</td>");
                        sb.Append("<td>" + contacts[i].Country + "</td>");
                        sb.Append("<td>" + contacts[i].State + "</td>");
                        sb.Append("<td>" + contacts[i].City + "</td>");
                        sb.Append("<td>" + (Convert.ToInt32(contacts[i].GMT_Offset) > 0 ? "+" + contacts[i].GMT_Offset : contacts[i].GMT_Offset) + "</td>");
                        sb.Append("<td>");
                        for (int j = 0; j < contacts[i].phoneNumbers.Count; j++)
                        {
                            sb.Append("<div>" + contacts[i].phoneNumbers[j].Phone_Type + " : " + contacts[i].phoneNumbers[j].PhoneNumber);
                            if (contacts[i].phoneNumbers[j].SMS == 1)
                                sb.Append(" : SMS Allowed");
                            sb.Append("</div>");
                        }
                        sb.Append("</td>");
                        sb.Append("<td>");
                        for (int j = 0; j < contacts[i].imNames.Count; j++)
                            sb.Append("<div>" + contacts[i].imNames[j].IM_Type + " : " + contacts[i].imNames[j].IM_Name + "</div>");

                        sb.Append("</td>");
                        sb.Append("<td>" + contacts[i].Notes + "</td>");
                        sb.Append("</tr>");
                    }
                }
                sb.Append("</tbody>");
                sb.Append("</table>");

                ltInfo.Text = sb.ToString();
                jsTest.InnerHtml += "<script type=\"text/javascript\">$(document).ready(function() {$(\"#tblUserInfo\").tablesorter({widgets: ['zebra'], widgetZebra: { css: ['d0', 'd1']} });    });    </script>";

            }
        }
    }

}

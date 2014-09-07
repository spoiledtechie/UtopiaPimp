using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PimpLibrary.Static.Enums;
using Pimp.UCache;
using Boomers.Utilities.Extensions;
using Boomers.Utilities.Services;
using Boomers.Utilities.Guids;
using Pimp.UParser;
using Pimp;
using Pimp.UData;
using MvcMiniProfiler;
using Pimp.Users;
using Pimp.UIBuilder;
using Pimp.Utopia;

public partial class members_Default : MyBasePageCS
{
    PimpUserWrapper currentUser;


    protected void Page_Load(object sender, EventArgs e)
    {



        if (!IsPostBack)
        {
            currentUser = new PimpUserWrapper();


            if (currentUser.PimpUser.StartingKingdom == new Guid())
                return;

            Guid startingKingdom;
            if (HttpContext.Current.Request.QueryString["kdid"] == null)
                startingKingdom = currentUser.PimpUser.StartingKingdom;
            else
            {
                if (HttpContext.Current.Request.QueryString["kdid"].ToString().IsValidGuid())
                    startingKingdom = new Guid(HttpContext.Current.Request.QueryString["kdid"].ToString());
                else
                    startingKingdom = currentUser.PimpUser.StartingKingdom;
            }

            spTargetCounts.InnerText = TargetFinder.Instance.LastSubmissionCount.ToString() + " Provinces updated in the past hour";
            var cachedKingdom = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom);
            if (KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom, startingKingdom, cachedKingdom) != null)
            {

                if (currentUser.PimpUser.NickName != string.Empty)
                {
                    chat.Attributes.Add("onclick", "commonPopup('http://" + SupportFramework.StaticContent.URLClass.GetDomain + "/controls/IMNet/groupChat.aspx?r=" + currentUser.PimpUser.StartingKingdom.RemoveDashes() + "', '605', '400', 4, 'Kingdom_Chat');");
                    chat.InnerText = UsersData.getUsersOnline(currentUser.PimpUser.StartingKingdom, cachedKingdom).Where(x => x.LastUpdated > DateTime.UtcNow.AddMinutes(-5)).Count().ToString() + " kingdom members online to chat";
                    spnChatKWide.Attributes.Add("onclick", "commonPopup('http://" + SupportFramework.StaticContent.URLClass.GetDomain + "/controls/IMNet/groupChat.aspx?r=" + new Guid().RemoveDashes() + "', '605', '400', 4, 'UtopiaPimp_Wide_Chat');");
                    spnChatKWide.InnerText = PimpUserWrapper.getUsersOnline().Count.ToString() + " Pimpers online to chat";
                    spnIRCChat.Text = "<span onclick=\"commonPopup('http://" + SupportFramework.StaticContent.URLClass.GetDomain + "/controls/IRC/Mibbit.aspx', '600', '525', 4, 'IRC_Web_Chat_on_UtoNet');\" class='deleteButton'>IRC Web Chat</span>";
                    spnIRCChat.Text += " - <span onclick=\"commonPopup('http://" + SupportFramework.StaticContent.URLClass.GetDomain + "/controls/IRC/Java/JavaIRC.aspx', '660', '525', 4, 'IRC_Java_Chat_on_UtoNet');\" class='deleteButton'>IRC Java (Installer) Chat</span>";
                    if (currentUser.PimpUser.MonarchType != MonarchType.none && currentUser.PimpUser.MonarchType != MonarchType.kdMonarch)
                        spnIRCChat.Text += " - As Monarch, you can set Default Channels for your kingdom under the Monarch Tab";
                }
                else
                {
                    spnIRCChat.Text = "<span onclick=\"commonPopup('http://" + SupportFramework.StaticContent.URLClass.GetDomain + "/controls/IRC/Mibbit.aspx', '600', '530', 4, 'IRC_Chat_on_UtoNet');\" class='deleteButton'>IRC Web Chat</span>";
                    spnIRCChat.Text += " - <span onclick=\"commonPopup('http://" + SupportFramework.StaticContent.URLClass.GetDomain + "/controls/IRC/Java/JavaIRC.aspx', '660', '525', 4, 'IRC_Java_Chat_on_UtoNet');\" class='deleteButton'>IRC Java (Installer) Chat</span> - Automate Your Nickname being loaded by adding it here: <a href=\"Contacts.aspx\">Add Nickname</a>";
                    chat.InnerHtml = "<a href=\"Contacts.aspx\">Kingdom Chat is live, but first you must fill in your nick name under Tools -> Contacts</a>";
                    spnChatKWide.InnerHtml = "<a href=\"Contacts.aspx\">UtopiPimp Chat is live, but first you must fill in your nick name under Tools -> Contacts</a>";
                }
                if (startingKingdom == currentUser.PimpUser.StartingKingdom)
                {
                    ltProvinceCodes.Text = FrontPage.DisplayProvinceCodesCache(currentUser.PimpUser.StartingKingdom, cachedKingdom);
                    if (ltProvinceCodes.Text == string.Empty)
                        divPnls.Visible = false;
                    ltContacts.Text = FrontPage.DisplayProvincesWithoutContacts(currentUser.PimpUser.StartingKingdom, cachedKingdom);
                    if (ltContacts.Text == string.Empty)
                        divContactsNotSigned.Visible = false;
                }
                else
                {
                    divContactsNotSigned.Visible = false;
                    divPnls.Visible = false;

                }
            }
            else
            {
                //ltKDSum.Text = "You have accessed this page in the wrong way.  Sorry.";
                divContactsNotSigned.Visible = false;
                divPnls.Visible = false;

                pnlExtras.Visible = false;
            }
        }
    }
}

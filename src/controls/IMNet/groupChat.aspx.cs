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
using Pimp.Chat;
using Pimp.UData;

public partial class IM_groupChat : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            string groupID = new Guid(Request.QueryString["r"].ToString()).ToString();//sets the group name

            hfSender.Value = pimpUser.PimpUser.UserID.ToString();
            lblTitle.Text = "Kingdom Chat";
            hfRecipient.Value = groupID;

            var yo = IM.getGroupChatMessagesStartChat(new Guid(hfSender.Value), new Guid(groupID));
            if (yo[1] == null)
                hfLastCheckedUID.Value = "0";
            else
                hfLastCheckedUID.Value = yo[1];
            divChatHistory.InnerHtml = yo[0];
            divGroupChatList.InnerHtml = IM.GetUsersOnline(groupID, KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom));

            hfNickName.Value = pimpUser.PimpUser.NickName;
        }
    }


}

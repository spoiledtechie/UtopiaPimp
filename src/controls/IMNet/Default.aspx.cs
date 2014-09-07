using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

using Pimp.UCache;
using Pimp.UData;
using SupportFramework;

public partial class controls_IMNet_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PimpUserWrapper  pimpUser = new PimpUserWrapper ();

        UsersData.UpdateLastActivityDate( pimpUser.PimpUser);
        chat.Attributes.Add("onclick", "commonPopup('http://localhost:51127/UP/controls/IMNet/groupChat.aspx?r=" + pimpUser.PimpUser.StartingKingdom + "', '515', '400', 4, 'Kingdom_Chat');");
        chat.InnerText = UsersData.getUsersOnline(pimpUser.PimpUser.StartingKingdom, KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom)).Count().ToString() + " kingdom members online to chat";
    }
}
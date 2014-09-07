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
using Pimp.UData;

public partial class controls_IRC_Java_JavaIRC : MyBasePageCS
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ltCSS.Text = "<link href=\"http://codingforcharity.org/utopiapimp/css/Default.css?v=" + SupportFramework.StaticContent.CSS.CssVersion + "\" rel='stylesheet' type='text/css' />";

            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            StringBuilder sb = new StringBuilder();
            sb.Append("<param name=\"command1\" value=\"/join #pimp\">");

            var channels = IrcCache.getKingdomIRCChannels(pimpUser.PimpUser.StartingKingdom, KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom));
            for (int i = 0; i < channels.Count; i++)
            {
                sb.Append("<param name=\"command" + (i + 2) + "\" value=\"/join #" + channels[i].Channel);
                if (channels[i].ChannelPassword != null && channels[i].ChannelPassword != string.Empty)
                    sb.Append(" " + channels[i].ChannelPassword + " ");
                sb.Append("\">");
            }

            
            var imInfo = pimpUser.PimpUser.IMInformation;
            if (imInfo != null && imInfo.Where(x => x.IM_Type == "IRC").FirstOrDefault() != null)
            {
                sb.Append("<param name=\"nick\" value=\"" + imInfo.Where(x => x.IM_Type == "IRC").FirstOrDefault().IM_Name);
                //if (user.IMInformation.Where(x => x.IM_Type == "IRC").FirstOrDefault().IM_Password_Bool == 1)
                sb.Append("\">");
            }
            if (pimpUser.PimpUser.NickName != string.Empty)
                sb.Append("<param name=\"nick\" value=\"" + pimpUser.PimpUser.NickName + "\">");
            else
            {
                Random num = new Random();
                sb.Append("<param name=\"nick\" value=\"pimp_java" + num.Next(1000) + "\">");
            }
            sb.Append("<param name=\"quitmessage\" value=\"Brought to you by UtopiaPimp.Com's IRC Chat! - TELL your friends to join in on Utopia-Game.com!\">");
            ltApplet.Text = sb.ToString();

        }
    }
}
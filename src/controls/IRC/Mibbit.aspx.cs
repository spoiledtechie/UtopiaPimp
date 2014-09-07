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

public partial class controls_IRC_Mibbit : MyBasePageCS
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            ltCSS.Text = "<link href=\"http://codingforcharity.org/utopiapimp/css/Default.css?v=" + SupportFramework.StaticContent.CSS.CssVersion + "\" rel='stylesheet' type='text/css' />";
           
            StringBuilder sb = new StringBuilder();
            sb.Append("<iframe width=\"100%\" style=\"display:block;width:100%\" height=\"460px\" scrolling=\"no\" frameborder=\"0\"");
            sb.Append(" src=\"http://widget.mibbit.com/?settings=268e31fda846ad5f63614eae2afc28f8");

            sb.Append("&server=irc.utonet.org");
            sb.Append("&channel=%23pimp");

            int dirtyPromptChannelPass = 0;
            var channels = IrcCache.getKingdomIRCChannels(pimpUser.PimpUser.StartingKingdom, KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom));
            for (int i = 0; i < channels.Count; i++)
            {
                sb.Append(",%23" + channels[i].Channel);
                if (channels[i].PromptPassword == 1)
                    dirtyPromptChannelPass = 1;
            }
            if (dirtyPromptChannelPass == 1)
                sb.Append("&promptChannelKey=true");

            var imInfo = pimpUser.PimpUser.IMInformation;

            if (imInfo != null && imInfo.Where(x => x.IM_Type == "IRC").FirstOrDefault() != null)
            {
                sb.Append("&nick=" + imInfo.Where(x => x.IM_Type == "IRC").FirstOrDefault().IM_Name);
                if (imInfo.Where(x => x.IM_Type == "IRC").FirstOrDefault().IM_Password_Bool == 1)
                    sb.Append("&promptPass=true");
            }
            if (pimpUser.PimpUser.NickName != string.Empty)
                sb.Append("&nick=" + pimpUser.PimpUser.NickName + "&promptPass=true");
            else
                sb.Append("&nick=Pimp_%3F%3F%3F%3F");

            sb.Append("\"></iframe>");
            ltChat.Text = sb.ToString();
        }
    }
}
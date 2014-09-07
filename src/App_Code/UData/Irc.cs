using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Boomers.Utilities.Communications;

using PimpLibrary;
using Pimp.Utopia;
using System.Threading.Tasks;
using Pimp.UCache;

namespace Pimp.UData
{
    /// <summary>
    /// Summary description for Irc
    /// </summary>
    public class Irc
    {

        public static void DeleteIRCChannel(string name, Guid ownerID, OwnedKingdomProvinces cachedKingdom)
        {
                CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
                var item = (from xx in db.Utopia_IRC_Channels
                            where xx.Owner_Kingdom_ID == ownerID
                            where xx.Channel_Name == name
                            select xx);
                db.Utopia_IRC_Channels.DeleteAllOnSubmit(item);
                db.SubmitChanges();
          
            IrcCache.removeKingdomIRCChannels(name, ownerID, cachedKingdom);
        }
   
   
        public static List<IRCChannel> getKingdomIRCChannels(Guid ownerKingdomID)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            return (from xx in db.Utopia_IRC_Channels
                    where xx.Owner_Kingdom_ID == ownerKingdomID
                    select new IRCChannel
                    {
                        Server = xx.IRC_Server,
                        Channel = xx.Channel_Name,
                        PromptPassword = xx.Channel_Password_Bool,
                        ChannelPassword = xx.Channel_Password
                    }).ToList();
        }
        public static void AddIRCChannel(string name, string channelPassword, Guid ownerID, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_IRC_Channel irc = new CS_Code.Utopia_IRC_Channel();
            irc.Channel_Name = name;
            irc.DateTime_Added = DateTime.UtcNow;
            irc.IRC_Server = "irc.utonet.org";
            irc.Owner_Kingdom_ID = ownerID;
            irc.Channel_Password = channelPassword;
            if (channelPassword != string.Empty)
                irc.Channel_Password_Bool = 1;
            else
                irc.Channel_Password_Bool = 0;
            db.Utopia_IRC_Channels.InsertOnSubmit(irc);
            db.SubmitChanges();
            IrcCache.addKingdomIRCChannel(name, irc.IRC_Server, irc.Channel_Password_Bool, channelPassword, ownerID, cachedKingdom);
        }
    }
}
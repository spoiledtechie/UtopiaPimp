using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Pimp.UData;
using Pimp.Utopia;
using Pimp.Users;
using Boomers.Utilities.Communications;

namespace Pimp.UCache
{
    /// <summary>
    /// Summary description for IrcCache
    /// </summary>
    public class IrcCache
    {

        public static List<IRCChannel> removeKingdomIRCChannels(string name, Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            if (cachedKingdom.IRCChannels != null)
            {
                cachedKingdom.IRCChannels.Remove(cachedKingdom.IRCChannels.Where(x => x.Channel == name).FirstOrDefault());
                HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
                return cachedKingdom.IRCChannels;
            }
            return getKingdomIRCChannels(ownerKingdomID, cachedKingdom);
        }

        public static List<IRCChannel> getKingdomIRCChannels(Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            if (cachedKingdom.IRCChannels == null)
            {
                cachedKingdom.IRCChannels = Irc.getKingdomIRCChannels(ownerKingdomID);
                HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            }
            return cachedKingdom.IRCChannels;
        }

        public static List<IRCChannel> addKingdomIRCChannel(string name, string ircServer, int promptPassword, string channelPassword, Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            if (cachedKingdom.IRCChannels != null)
            {
                IRCChannel cha = new IRCChannel();
                cha.Channel = name;
                cha.Server = ircServer;
                cha.PromptPassword = promptPassword;
                if (channelPassword != string.Empty)
                    cha.ChannelPassword = channelPassword;
                cachedKingdom.IRCChannels.Add(cha);
                HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
                return cachedKingdom.IRCChannels;
            }
            return getKingdomIRCChannels(ownerKingdomID, cachedKingdom);
        }

    }
}
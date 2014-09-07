using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PimpLibrary.UI;
using PimpLibrary.Utopia.Ops;
using Boomers.Utilities.Communications;
using Boomers.UserUtil;

namespace Pimp.Utopia
{
    /// <summary>
    /// Summary description for OwnedKingdomProvinces
    /// </summary>
    public class OwnedKingdomProvinces
    {
        public Guid Owner_Kingdom_ID;
        public int? KdOpsAttacksTimeLimit;
        public int KdProvTimeLimit;
        public List<Contact> Contacts;
        public List<string> ProvincesWithoutUserContactsAdded;
        public List<ProvinceClass> Provinces;
        public List<KingdomClass> Kingdoms;
        public List<ColumnSet> KingdomColumnSets;
        public int RandomProvinceCount;
        public DateTime RandomProvincesLastCheckedDb;
        public List<IRCChannel> IRCChannels;
        public List<Attack> Attacks;
        public List<Op> Effects;
    }
}
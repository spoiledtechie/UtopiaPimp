using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pimp.Utopia
{
    /// <summary>
    /// Summary description for KingdomClass
    /// </summary>
    public class KingdomClass
    {
        public Guid Owner_Kingdom_ID;
        public Guid Kingdom_ID;
        public bool Retired;
        public int Server_ID;
        public string Kingdom_Message;
        public int Kingdom_Island;
        public int Kingdom_Location;
        public string Kingdom_Name;
        public DateTime? Updated_By_DateTime;
        public int? War_Wins;
        public int? War_Losses;
        public Guid? Owner_User_ID;
        //public List<BuildCE> CEList;
        public List<CS_Code.Utopia_Kingdom_CE> CeList;
        //Used for Target Finder;
        public int War;
        public int Networth;
        public int Acres;
        public int ProvinceCount;
        public int? Stance;
        public List<ProvinceClass> Provinces;
    }
}
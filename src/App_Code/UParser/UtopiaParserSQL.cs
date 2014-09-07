using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

using Boomers.Utilities.DatesTimes;
using Pimp.UCache;
using Pimp.UData;
using PimpLibrary.Static.Enums;
using Pimp.UIBuilder;
using System.Threading.Tasks;
using PimpLibrary.UI;
using PimpLibrary.Utopia.Players;
using Pimp.Users;
using PimpLibrary.Utopia.Kingdom;
using Pimp.Utopia;
using PimpLibrary.Utopia.Ops;
using SupportFramework.Data;

namespace Pimp.UParser
{
    /// <summary>
    /// Summary description for UtopiaParserSQL
    /// </summary>
    public partial class UtopiaParser
    {

        /// <summary>
        /// Gets the Island and location of a kingdom.
        /// </summary>
        /// <param name="kingdomID"></param>
        /// <returns></returns>
        public static string GetKingdomIslandLocation(Guid ownerKingdomID, Guid kingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            var uk = KingdomCache.getKingdom(ownerKingdomID, kingdomID, cachedKingdom);
            return "(" + uk.Kingdom_Island + ":" + uk.Kingdom_Location + ")";
        }
        /// <summary>
        /// Gets the RACE ID of the Province from the Race Name.
        /// </summary>
        /// <param name="RaceName"></param>
        /// <returns></returns>
        public static int getRaceID(string raceName, Guid currentUserID)
        {
            var raceID = UtopiaHelper.Instance.Races.Where(x => x.name == raceName.Trim()).Select(x => x.uid).FirstOrDefault();
            switch (raceID)
            {
                case 0:
                    if (raceName.Length > 1)
                    {
                        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
                        CS_Code.Utopia_Province_Race_Pull UPRPP = new CS_Code.Utopia_Province_Race_Pull();
                        UPRPP.Race_Name = raceName.Trim();
                        UPRPP.Enabled = true;
                        db.Utopia_Province_Race_Pulls.InsertOnSubmit(UPRPP);
                        db.SubmitChanges();
                        FailedAtTesting("'BrokenRaceName'", "-" + raceName + "-", currentUserID);
                        HttpRuntime.Cache.Remove("Races");
                        return UPRPP.uid;
                    }
                    break;
            }
            return raceID;
        }
        public static int getNobilityID(string nobilityName, Guid currentUserID)
        {
            var nobilityId = UtopiaHelper.Instance.Ranks.Where(x => x.name == nobilityName).Select(x => x.uid).FirstOrDefault();

            switch (nobilityId)
            {
                case 0:
                    if (nobilityName.Length > 1)
                    {
                        FailedAtTesting("'BrokenNobilityName'", "-" + nobilityName + "-", currentUserID);
                    }
                    break;
            }
            return nobilityId;
        }
        private static int getAttackType(string attackType, PimpUserWrapper currentUser)
        {
            var attackTypeID = UtopiaHelper.Instance.AttackType.Where(x => x.name == attackType.Trim()).Select(x => x.uid).FirstOrDefault();
            switch (attackTypeID)
            {
                case 0:
                    CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
                    CS_Code.Utopia_Province_Data_Captured_Attack_Pull updcap = new CS_Code.Utopia_Province_Data_Captured_Attack_Pull();
                    updcap.Province_ID_Added = currentUser.PimpUser.CurrentActiveProvince;
                    updcap.DateTime_Added = DateTime.UtcNow;
                    updcap.Attack_Type_Name = attackType.Trim();
                    db.Utopia_Province_Data_Captured_Attack_Pulls.InsertOnSubmit(updcap);
                    db.SubmitChanges();
                    HttpRuntime.Cache.Remove("AttackType");
                    return updcap.uid;
            }
            return attackTypeID;
        }

        /// <summary>
        /// Gets the Race ID of the currently logged in user.
        /// </summary>
        /// <returns>Returnes the Race ID</returns>
        private static int GetRaceID(PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            try
            {
                return (int)cachedKingdom.Provinces.Where(x => x.Province_ID == currentUser.PimpUser.CurrentActiveProvince).FirstOrDefault().Race_ID;
            }
            catch { return 0; }
        }
        /// <summary>
        /// Gets the Personallity ID of the province Personality Name.
        /// </summary>
        /// <param name="PersonalityName"></param>
        /// <returns></returns>
        public static int GetPersonalityID(string PersonalityName)
        {
            var PersID = UtopiaHelper.Instance.Personalities.Where(x => x.name == PersonalityName).Select(x => x.uid).FirstOrDefault();
            switch (PersID)
            {
                case 0:
                    CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
                    CS_Code.Utopia_Personality_Pull UPPP = new CS_Code.Utopia_Personality_Pull();
                    UPPP.Personality_Name = PersonalityName.Trim();
                    UPPP.Enabled = true;
                    db.Utopia_Personality_Pulls.InsertOnSubmit(UPPP);
                    db.SubmitChanges();
                    HttpRuntime.Cache.Remove("Personalities");
                    return UPPP.uid;
            }
            return PersID;
        }
        /// <summary>
        /// Gets the Operation name of a thieve or mystic op.
        /// </summary>
        /// <param name="OpName">The op name.</param>
        /// <returns>Gets the Op ID</returns>
        public static int GetOpID(string opName, Guid userID)
        {
            var opID = UtopiaHelper.Instance.Ops.Where(x => x.OP_Name == opName.Trim()).Select(x => x.uid).FirstOrDefault();
            switch (opID)
            {
                case 0:
                    CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
                    CS_Code.Utopia_Province_Ops_Pull uop = new CS_Code.Utopia_Province_Ops_Pull();
                    uop.OP_Name = opName.Trim();
                    uop.Added_By_User_ID = userID;
                    uop.TimeStamp = DateTime.UtcNow;
                    db.Utopia_Province_Ops_Pulls.InsertOnSubmit(uop);
                    db.SubmitChanges();
                    HttpRuntime.Cache.Remove("Ops");
                    return uop.uid;
            }
            return opID;
        }
        public static int GetOpID(OpType opName, Guid userID)
        {
            return GetOpID(opName.ToString(), userID);
        }
        public static void FailedAt(ErrorTypeEnum failedat, string data, Guid userID)
        {
            FailedAt(failedat.ToString(), data, userID);
        }
        /// <summary>
        /// Inserts the data into the table where the parser failed.
        /// </summary>
        /// <param name="failedat"></param>
        /// <param name="data"></param>
        public static void FailedAt(string failedat, string data, Guid userID)
        {

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Distorted_Data DD = new CS_Code.Utopia_Distorted_Data();
            DD.aspnet_ID = userID;
            DD.date_time = DateTime.UtcNow;
            DD.Raw_Data = data.Replace(Environment.NewLine, "");
            DD.Failed_At = "'" + failedat + "'";
            DD.Version = AssemblyID.Version;
            if (HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session["SubmittedData"] != null)
                    DD.rawData = HttpContext.Current.Session["SubmittedData"].ToString();
                HttpContext.Current.Session["Failed"] = 1;
            }
            db.Utopia_Distorted_Datas.InsertOnSubmit(DD);
            db.SubmitChanges();

        }
        /// <summary>
        /// For testing purposes....
        /// </summary>
        /// <param name="failedat"></param>
        /// <param name="data"></param>
        public static void FailedAtTesting(string failedat, string data, Guid userID)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Distorted_Data DD = new CS_Code.Utopia_Distorted_Data();
            DD.aspnet_ID = userID;
            DD.date_time = DateTime.UtcNow;
            DD.Raw_Data = data.Replace(Environment.NewLine, "");
            DD.Failed_At = failedat;
            DD.Version = AssemblyID.Version;
            if (HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["SubmittedData"] != null)
                    DD.rawData = HttpContext.Current.Session["SubmittedData"].ToString();

            db.Utopia_Distorted_Datas.InsertOnSubmit(DD);
            db.SubmitChanges();
        }
        /// <summary>
        /// Gets the UID of the Dragon Name entered.
        /// </summary>
        /// <param name="RawLine">Line to find Dragon name.</param>
        /// <returns></returns>
        private static int SelectDragonType(string RawLine, Guid userID)
        {
            var dragonType = GetDragons.Where(x => x.name == URegEx._findDragonType.Match(RawLine).Value).Select(x => x.uid).FirstOrDefault();
            switch (dragonType)
            {
                case 0:
                    CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
                    CS_Code.Utopia_Dragon_Type_Pull uop = new CS_Code.Utopia_Dragon_Type_Pull();
                    uop.Dragon_Type = URegEx._findDragonType.Match(RawLine).Value;
                    uop.Added_By_UserID = userID;
                    uop.Added_By_DateTime = DateTime.UtcNow;
                    db.Utopia_Dragon_Type_Pulls.InsertOnSubmit(uop);
                    db.SubmitChanges();
                    HttpRuntime.Cache.Remove("Dragons");
                    return uop.uid;
            }
            return dragonType;
        }
        static List<Dragon> _getDragons;
        public static List<Dragon> GetDragons
        {
            get
            {
                if (_getDragons == null)
                {
                    if (HttpRuntime.Cache["Dragons"] == null)
                    {
                        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
                        List<Dragon> dragon = (from UPNP in db.Utopia_Dragon_Type_Pulls
                                               select new Dragon
                                               {
                                                   name = UPNP.Dragon_Type,
                                                   uid = UPNP.uid
                                               }).ToList();
                        HttpRuntime.Cache.Add("Dragons", dragon, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.NotRemovable, null);
                        _getDragons = dragon;
                    }
                    else
                    {
                        try
                        {
                            _getDragons = (List<Dragon>)HttpRuntime.Cache["Dragons"];
                        }
                        catch
                        {
                            HttpRuntime.Cache.Remove("Dragons");
                            return GetDragons;
                        }
                    }
                }
                return _getDragons;
            }
        }
        /// <summary>
        /// gets the stance ID
        /// </summary>
        /// <param name="KingdomStance">Stance of the kingdom.</param>
        /// <returns></returns>
        public static int getStanceID(string kingdomStance)
        {
            var getStance = UtopiaHelper.Instance.KingdomStances.Where(x => x.name == kingdomStance.Trim()).Select(x => x.uid).FirstOrDefault();
            switch (getStance)
            {
                case 0:
                    CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
                    CS_Code.Utopia_Kingdom_Stance_Pull usp = new CS_Code.Utopia_Kingdom_Stance_Pull();
                    usp.stance = kingdomStance.Trim();
                    db.Utopia_Kingdom_Stance_Pulls.InsertOnSubmit(usp);
                    db.SubmitChanges();
                    HttpRuntime.Cache.Remove("KingdomStances");
                    FailedAtTesting("'BrokenkingdomStance'", "-" + kingdomStance + "-", new Guid());
                    return usp.uid;
            }
            return getStance;
        }
        private static string GetStanceName(int kingdomStance)
        {
            return UtopiaHelper.Instance.KingdomStances.Where(x => x.uid == kingdomStance).Select(x => x.name).FirstOrDefault();
        }

        /// <summary>
        /// Inserts an Operation which has days
        /// </summary>
        /// <param name="opName">The abbreviated Op Name</param>
        /// <param name="days">Amount of days the op has left.</param>
        /// <param name="directedTo">Who is the op on.</param>
        public static void InsertOp(OpType opName, int? days, Guid directedTo, string opText, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.Utopia_Province_Op upo = new CS_Code.Utopia_Province_Op();
            upo.Directed_To_Province_ID = directedTo;
            upo.Added_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;

            if (days.HasValue)
            {
                upo.Duration = days.Value;
                upo.Expiration_Date = DateTime.UtcNow.AddHours(days.Value);
            }
            if (!String.IsNullOrEmpty(opText))
                upo.OP_Text = opText;

            upo.Op_ID = GetOpID(opName.ToString(), currentUser.PimpUser.UserID);
            upo.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
            upo.TimeStamp = DateTime.UtcNow;
            OpCache.UpdateOpToCache(upo, cachedKingdom);

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            db.Utopia_Province_Ops.InsertOnSubmit(upo);
            db.SubmitChanges();
        }
        public static void InsertOp(OpType opName, int days, Guid directedTo, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            InsertOp(opName, days, directedTo, null, currentUser, cachedKingdom);
        }
        /// <summary>
        /// Inserts and Op that doesn't have days attached to it.
        /// </summary>
        /// <param name="opName">Operation Name</param>
        /// <param name="directedTo">Province its aimed at.</param>
        public static void InsertOp(OpType opName, Guid directedTo, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            InsertOp(opName, null, directedTo, null, currentUser, cachedKingdom);
        }
        /// <summary>
        /// Inserts and Operation without days but with some info added to it.
        /// </summary>
        /// <param name="opName">Ops Name</param>
        /// <param name="directedTo">Province its directed to</param>
        /// <param name="opText">Any extra info</param>
        public static void InsertOp(OpType opName, Guid directedTo, string opText, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            InsertOp(opName, null, directedTo, opText, currentUser, cachedKingdom);
        }
        public static string InsertOpPersonal(OpType opName, MatchCollection mc, string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            Guid provID = currentUser.PimpUser.CurrentActiveProvince;
            for (int i = 0; i < mc.Count; i++)
            {
                int days = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxDayOps.Match(mc[i].Value).Value).Value);
                CS_Code.Utopia_Province_Op upo = new CS_Code.Utopia_Province_Op();
                upo.Directed_To_Province_ID = provID;
                upo.Added_By_Province_ID = provID;
                upo.Duration = days;
                upo.Expiration_Date = DateTime.UtcNow.AddHours(days);
                upo.Op_ID = GetOpID(opName.ToString(), currentUser.PimpUser.UserID);
                upo.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
                upo.TimeStamp = DateTime.UtcNow;
                OpCache.UpdateOpToCache(upo, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                db.Utopia_Province_Ops.InsertOnSubmit(upo);
            }
            db.SubmitChanges();

            return RawData;
        }
        public static void InsertOpPersonal(OpType opName, string opText, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            Guid provID = currentUser.PimpUser.CurrentActiveProvince;

            CS_Code.Utopia_Province_Op upo = new CS_Code.Utopia_Province_Op();
            upo.Directed_To_Province_ID = provID;
            upo.Added_By_Province_ID = provID;
            upo.OP_Text = opText;
            upo.Op_ID = GetOpID(opName.ToString(), currentUser.PimpUser.UserID);
            upo.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
            upo.TimeStamp = DateTime.UtcNow;
            OpCache.UpdateOpToCache(upo, cachedKingdom);

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            db.Utopia_Province_Ops.InsertOnSubmit(upo);
            db.SubmitChanges();

        }
        public static void AddThiefOp(OpType OpName, Guid DirectedToProvinceID, string text, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            AddThiefOp(OpName.ToString(), DirectedToProvinceID, text, currentUser, cachedKingdom);
        }
        public static void AddThiefOp(string OpName, Guid DirectedToProvinceID, string text, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.Utopia_Province_Op upo = new CS_Code.Utopia_Province_Op();
            upo.Directed_To_Province_ID = DirectedToProvinceID;
            upo.Added_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            upo.Op_ID = GetOpID(OpName, currentUser.PimpUser.UserID);
            upo.OP_Text = text;
            upo.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
            upo.TimeStamp = DateTime.UtcNow;
            OpCache.UpdateOpToCache(upo, cachedKingdom);

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            db.Utopia_Province_Ops.InsertOnSubmit(upo);
            db.SubmitChanges();

        }
        /// <summary>
        /// Helps parse the ingame SOM which allows for this to be called multiple times.
        /// </summary>
        /// <param name="RawData"></param>
        /// <param name="db"></param>
        /// <param name="ProvinceInfo"></param>
        private static void SOMUpdateRaw(string RawData, CS_Code.UtopiaDataContext db, CS_Code.Utopia_Province_Data_Captured_Gen ProvinceInfo, DateTime datetime, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            List<CS_Code.Utopia_Province_Data_Captured_Type_Military> mils = new List<CS_Code.Utopia_Province_Data_Captured_Type_Military>();
            ProvinceInfo.Updated_By_DateTime = datetime;
            ProvinceInfo.Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            ProvinceInfo.SOM_Updated_By_DateTime = datetime;
            ProvinceInfo.SOM_Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            switch (URegEx._findSOMMilEfficiency.IsMatch(RawData))
            {
                case true:
                    ProvinceInfo.Mil_Overall_Efficiency = Convert.ToDecimal(URegEx._findPercentages.Match(URegEx._findSOMMilEfficiency.Match(RawData).Value).Value.Replace("%", ""));
                    break;
            }
            if (URegEx._findSOMNonPeasants.IsMatch(RawData))
                ProvinceInfo.Peasents_Non_Percentage = Convert.ToDecimal(URegEx._findPercentages.Match(URegEx._findSOMNonPeasants.Match(RawData).Value).Value.Replace("%", ""));
            if (URegEx._findSOMWageRate.IsMatch(RawData))
                ProvinceInfo.Mil_Wage = Convert.ToDecimal(URegEx._findPercentages.Match(URegEx._findSOMWageRate.Match(RawData).Value).Value.Replace("%", ""));
            if (URegEx._findSOMMilOffenseEff.IsMatch(RawData))
                ProvinceInfo.Military_Efficiency_Off = Convert.ToDecimal(URegEx._findPercentages.Match(URegEx._findSOMMilOffenseEff.Match(RawData).Value).Value.Replace("%", ""));

            if (URegEx._findSOMMilDefenseEff.IsMatch(RawData))
                ProvinceInfo.Military_Efficiency_Def = Convert.ToDecimal(URegEx._findPercentages.Match(URegEx._findSOMMilDefenseEff.Match(RawData).Value).Value.Replace("%", ""));
            ProvinceInfo.Military_Current_Off = Convert.ToDecimal(URegEx.rgxQuantitiesWithComma.Match(URegEx._findSOMMilNetOff.Match(RawData).Value).Value.Replace(",", ""));
            ProvinceInfo.Military_Current_Def = Convert.ToDecimal(URegEx.rgxQuantitiesWithComma.Match(URegEx._findSOMMilNetDef.Match(RawData).Value).Value.Replace(",", ""));

            if (URegEx._findGeneralsSOM.IsMatch(RawData))
                ProvinceInfo.Mil_Total_Generals = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findGeneralsSOM.Match(RawData).Value).Value);

            ProvinceInfo.Military_Net_Off = ProvinceInfo.Military_Current_Off;
            ProvinceInfo.Military_Net_Def = ProvinceInfo.Military_Current_Def;

            string RawDataTemp = RawData;
            RawDataTemp = RawDataTemp.Remove(0, RawDataTemp.IndexOf(URegEx._findSOMMilNetDef.Match(RawData).Value));
            string offq = string.Empty;
            string defq = string.Empty;
            string elitq = string.Empty;
            string thiefq = string.Empty;
            int offc = 0;
            int defc = 0;
            int elitc = 0;
            int thiefc = 0;
            switch (RawDataTemp.Contains("Training"))
            {
                case true:
                    try
                    {
                        string temp = RawDataTemp.Remove(0, RawDataTemp.IndexOf("Training"));

                        string matchh = URegEx._findSOMRawOff.Match(temp).Value;
                        MatchCollection mc = URegEx.rgxNumber.Matches(matchh);
                        for (int i = 0; i < mc.Count; i++)
                        {
                            offq += mc[i].Value + " ";
                            offc += Convert.ToInt32(mc[i].Value);
                        }
                        offq = "Next " + mc.Count + " hours: " + offq;

                        matchh = URegEx._findSOMRawDef.Match(temp).Value;
                        mc = URegEx.rgxNumber.Matches(matchh);
                        for (int i = 0; i < mc.Count; i++)
                        {
                            defq += mc[i].Value + " ";
                            defc += Convert.ToInt32(mc[i].Value);
                        }
                        defq = "Next " + mc.Count + " hours: " + defq;

                        matchh = URegEx._findSOMRawElite.Match(temp).Value;
                        mc = URegEx.rgxNumber.Matches(matchh);
                        for (int i = 0; i < mc.Count; i++)
                        {
                            elitq += mc[i].Value + " ";
                            elitc += Convert.ToInt32(mc[i].Value);
                        }
                        elitq = "Next " + mc.Count + " hours: " + elitq;

                        matchh = URegEx._findSOMRawThief.Match(temp).Value;
                        mc = URegEx.rgxNumber.Matches(matchh);
                        for (int i = 0; i < mc.Count; i++)
                        {
                            thiefq += mc[i].Value + " ";
                            thiefc += Convert.ToInt32(mc[i].Value);
                        }
                        thiefq = "Next " + mc.Count + " hours: " + thiefq;

                        RawDataTemp = RawDataTemp.Remove(RawDataTemp.IndexOf("Training"));
                    }
                    catch { }
                    break;
            }
            RawDataTemp = RawDataTemp.Remove(0, URegEx._findSOMTimeAvailable.Match(RawDataTemp).Index);

            string soldiers = URegEx._findSOMSoldiersData.Match(RawDataTemp).Value;
            string Horses = "0";
            switch (URegEx._findSOMWarHorses.IsMatch(RawDataTemp))
            {
                case true:
                    Horses = URegEx._findSOMWarHorses.Match(RawDataTemp).Value;
                    break;
            }

            string capland = URegEx._findSOMCapturedLand.Match(RawDataTemp).Value;
            if (Horses.Length > 0)
                RawDataTemp = RawDataTemp.Replace(Horses, "");
            if (soldiers.Length > 0)
                RawDataTemp = RawDataTemp.Replace(soldiers, "");
            if (capland.Length > 0)
                RawDataTemp = RawDataTemp.Replace(capland, "");

            for (int i = 0; i < URegEx._findSOMTimeAvailable.Matches(RawDataTemp).Count; i++)
            {
                CS_Code.Utopia_Province_Data_Captured_Type_Military UPDCTM = new CS_Code.Utopia_Province_Data_Captured_Type_Military();
                UPDCTM.Province_ID = ProvinceInfo.Province_ID;
                UPDCTM.Province_ID_Added = currentUser.PimpUser.CurrentActiveProvince;
                UPDCTM.DateTime_Added = datetime;
                if (URegEx._findSOMOffDefElites.Matches(RawDataTemp).Count > 2)
                    UPDCTM.Elites = Convert.ToInt32(URegEx._findQuantitiesDash.Matches(URegEx._findSOMOffDefElites.Matches(RawDataTemp)[3].Value)[i].Value.Replace(",", ""));
                UPDCTM.Elites_Def_Pts = CalcDefElitePoints(UPDCTM.Elites.GetValueOrDefault(0), ProvinceInfo.Military_Efficiency_Def.GetValueOrDefault(0), ProvinceInfo.Race_ID.GetValueOrDefault());
                UPDCTM.Elites_Off_Pts = CalcOffElitePoints(UPDCTM.Elites.GetValueOrDefault(0), ProvinceInfo.Military_Efficiency_Off.GetValueOrDefault(0), ProvinceInfo.Race_ID.GetValueOrDefault());

                UPDCTM.Horses = Convert.ToInt32(URegEx._findQuantitiesDash.Matches(Horses)[i].Value.Replace(",", ""));
                UPDCTM.Horses_Pts = CalcHorsePoints(UPDCTM.Horses.GetValueOrDefault(0), ProvinceInfo.Military_Efficiency_Off.GetValueOrDefault(0));
                UPDCTM.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
               
                
                UPDCTM.Regs_Off = Convert.ToInt32(URegEx._findQuantitiesDash.Matches(URegEx._findSOMOffDefElites.Matches(RawDataTemp)[1].Value)[i].Value.Replace(",", ""));
                UPDCTM.Regs_Off_Pts = CalcOffRegPoints(UPDCTM.Regs_Off.GetValueOrDefault(0), ProvinceInfo.Military_Efficiency_Off.GetValueOrDefault(0), ProvinceInfo.Race_ID.GetValueOrDefault(0));

                if (i == 0)
                {
                    UPDCTM.Regs_Def = Convert.ToInt32(URegEx._findQuantitiesDash.Matches(URegEx._findSOMOffDefElites.Matches(RawDataTemp)[2].Value)[i].Value.Replace(",", ""));
                    UPDCTM.Regs_Def_Pts = CalcDefRegPoints(UPDCTM.Regs_Def.GetValueOrDefault(0), ProvinceInfo.Military_Efficiency_Def.GetValueOrDefault(0), ProvinceInfo.Race_ID.GetValueOrDefault());
                }
                UPDCTM.Soldiers = Convert.ToInt32(URegEx._findQuantitiesDash.Matches(soldiers)[i].Value.Replace(",", ""));
                UPDCTM.Soldiers_Off_Pts = CalcOffSoldierPoints(UPDCTM.Soldiers.GetValueOrDefault(0), ProvinceInfo.Military_Efficiency_Off.GetValueOrDefault(0));
                UPDCTM.Soldiers_Def_Pts = CalcDefSoldierPoints(UPDCTM.Soldiers.GetValueOrDefault(0), ProvinceInfo.Military_Efficiency_Def.GetValueOrDefault(0));
                UPDCTM.Generals = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findGeneralsName.Match(RawDataTemp).Value).Value);
                switch (URegEx._findSOMTimeAvailable.Matches(RawDataTemp)[i].Value)
                {
                    case "Standing Army":
                        UPDCTM.Military_Location = 1;
                        UPDCTM.Wages = (int)ProvinceInfo.Mil_Wage.GetValueOrDefault(0);
                        UPDCTM.Efficiency_Def = ProvinceInfo.Military_Efficiency_Def.GetValueOrDefault(0);
                        UPDCTM.Efficiency_Off = ProvinceInfo.Military_Efficiency_Off.GetValueOrDefault(0);
                        UPDCTM.Efficiency_Raw = ProvinceInfo.Mil_Overall_Efficiency.GetValueOrDefault(0);
                        UPDCTM.Net_Defense_Pts_Home = (int)ProvinceInfo.Military_Current_Def.GetValueOrDefault(0);
                        UPDCTM.Net_Offense_Pts_Home = (int)ProvinceInfo.Military_Current_Off.GetValueOrDefault(0);
                        UPDCTM.Military_Population = ProvinceInfo.Peasents_Non_Percentage.GetValueOrDefault(0);
                        if (offc > 0)
                        {
                            UPDCTM.Regs_Off_Train_Queue = offq;
                            UPDCTM.Regs_Off_Train = offc;
                        }
                        if (defc > 0)
                        {
                            UPDCTM.Regs_Def_Train_Queue = defq;
                            UPDCTM.Regs_Def_Train = defc;
                        }
                        if (elitc > 0)
                        {
                            UPDCTM.Elites_Train_Queue = elitq;
                            UPDCTM.Elites_Train = elitc;
                        }
                        if (thiefc > 0)
                        {
                            UPDCTM.Thieves_Train_Queue = thiefq;
                            UPDCTM.Thieves_Train = thiefc;
                        }
                        break;
                    default:
                        UPDCTM.Military_Location = 2;
                        UPDCTM.Time_To_Return = datetime.AddMinutes(ConvertUtopiaDaystoMinutes(URegEx._findQuantitiesDecimal.Match(URegEx._findSOMTimeLeft.Matches(RawDataTemp)[i - 1].Value).Value));
                        ProvinceInfo.Army_Out_Expires = UPDCTM.Time_To_Return.Value;
                        ProvinceInfo.Army_Out = 1;
                        break;
                }

                try
                {
                    if (URegEx._findQuantitiesDash.Matches(capland).Count < i)
                    {
                        switch (URegEx._findQuantitiesDash.Matches(capland)[i].Value.Contains("-"))
                        {
                            case true:
                                UPDCTM.CapturedLand = 0;
                                break;
                            default:
                                UPDCTM.CapturedLand = Convert.ToInt32(URegEx._findQuantitiesDash.Matches(capland)[i].Value.Replace(",", ""));
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Errors.logError(e);
                }
                if (UPDCTM.Elites != 0 || UPDCTM.Horses != 0 || UPDCTM.Regs_Off != 0 || UPDCTM.Regs_Def != 0)
                {
                    db.Utopia_Province_Data_Captured_Type_Militaries.InsertOnSubmit(UPDCTM);
                    mils.Add(UPDCTM);
                }
            }
            ProvinceInfo.SOM_Requested = null;
            ProvinceCache.UpdateProvinceSOMToCache(ProvinceInfo, mils, cachedKingdom);
            db.SubmitChanges();
        }
        /// <summary>
        /// Sets the Armies that are away string for SOMs
        /// </summary>
        /// <param name="URegEx.rgxFindNetDONumber"></param>
        /// <param name="URegEx.rgxFindMilOffense"></param>
        /// <param name="URegEx.rgxFindMilDefense"></param>
        /// <param name="URegEx.rgxFindMilOFFDEFELITES"></param>
        /// <param name="Province_ID">Province ID for the army that is away</param>
        /// <param name="RawDataStanding">Raw Data for the SOM.</param>
        public  static void SetMilitaryArmies(Guid Province_ID, string RawDataStanding, DateTime datetime, List<CS_Code.Utopia_Province_Data_Captured_Type_Military> mils, CS_Code.Utopia_Province_Data_Captured_Gen getProv, Guid ownerKingdomID, PimpUserWrapper currentUser)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Province_Data_Captured_Type_Military UPDCTM = new CS_Code.Utopia_Province_Data_Captured_Type_Military();
            RawDataStanding = SetExportLine(RawDataStanding, UPDCTM);



            if (RawDataStanding.Contains("** Troops in Training **"))
                RawDataStanding = RawDataStanding.Remove(RawDataStanding.IndexOf("** Troops in Training **"));
            UPDCTM.Military_Population = 0;
            string returnTime = URegEx._findAngelMilTimeToReturn.Match(RawDataStanding).Value;
            UPDCTM.Time_To_Return = DateTime.UtcNow.AddHours(Convert.ToDouble(URegEx.rgxQuantitiesWithComma.Match(returnTime).Value)).AddMinutes(Convert.ToDouble(URegEx.rgxQuantitiesWithComma.Match(returnTime).NextMatch().Value));

            if (getProv != null)
            {
                getProv.Army_Out_Expires = UPDCTM.Time_To_Return.Value;
                getProv.Army_Out = 1;
                getProv.Updated_By_DateTime = DateTime.UtcNow;
                getProv.Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
                UPDCTM.Efficiency_Def = getProv.Military_Efficiency_Def;
                UPDCTM.Efficiency_Off = getProv.Military_Efficiency_Off;
                UPDCTM.Efficiency_Raw = getProv.Mil_Overall_Efficiency;
            }

            UPDCTM.DateTime_Added = datetime;
            if (URegEx._findGenerals.Match(RawDataStanding).Success)
                UPDCTM.Generals = URegEx._findGenerals.Matches(RawDataStanding).Count;
            RawDataStanding = GetSoldiersData(RawDataStanding, UPDCTM);
            RawDataStanding = GetWarHorseData(RawDataStanding, UPDCTM);
            GetCapturedLandData(RawDataStanding, UPDCTM);
            GetMilOFffDefElitesData(RawDataStanding, UPDCTM);
            UPDCTM.Military_Location = 2;
            UPDCTM.Owner_Kingdom_ID = ownerKingdomID;
            UPDCTM.Province_ID = Province_ID;
            UPDCTM.Province_ID_Added = currentUser.PimpUser.CurrentActiveProvince;


            mils.Add(UPDCTM);
            db.Utopia_Province_Data_Captured_Type_Militaries.InsertOnSubmit(UPDCTM);
            db.SubmitChanges();
        }
        /// <summary>
        /// Sets the Export line for the Data.
        /// </summary>
        /// <param name="RawDataStanding"></param>
        /// <param name="UPDCTM"></param>
        /// <returns></returns>
        private static string SetExportLine(string RawDataStanding, CS_Code.Utopia_Province_Data_Captured_Type_Military UPDCTM)
        {
            switch (URegEx._findExportLine.Match(RawDataStanding).Success)
            {
                case true:
                    UPDCTM.Export_Line = RawDataStanding.Remove(0, URegEx._findExportLine.Match(RawDataStanding).Index).Trim();
                    return RawDataStanding.Remove(RawDataStanding.IndexOf(UPDCTM.Export_Line));
                default:
                    return RawDataStanding;
            }
        }
        /// <summary>
        /// Sets the amount of generals in the amry.
        /// </summary>
        /// <param name="RawDataStanding">Raw Data String</param>
        /// <param name="UPDCTM">Data Set to put generals in.</param>
        //private static void GetGeneralsArmyData(string RawDataStanding, CS_Code.Utopia_Province_Data_Captured_Type_Military UPDCTM)
        //{
        //    if (URegEx.rgxFindGenerals.Match(RawDataStanding).Success)
        //        UPDCTM.Generals = URegEx.rgxFindGenerals.Matches(RawDataStanding).Count;
        //}
        /// <summary>
        /// Sets the Captured land.
        /// </summary>
        /// <param name="URegEx.rgxFindNetDONumber">Regex expression to find number</param>
        /// <param name="RawDataStanding">Raw Data string.</param>
        /// <param name="UPDCTM">Data to put in captured land.</param>
        public  static void GetCapturedLandData(string RawDataStanding, CS_Code.Utopia_Province_Data_Captured_Type_Military UPDCTM)
        {
            if (URegEx.rgxCapturedLand.Match(RawDataStanding).Success)
                UPDCTM.CapturedLand = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx.rgxCapturedLand.Match(RawDataStanding).Value).Value.Replace(",", ""));
        }
        /// <summary>
        /// Sets the Military Off, Def and Elites data.
        /// </summary>
        /// <param name="RawDataStanding">Raw Data String.</param>
        /// <param name="UPDCTM"></param>
        public  static void GetMilOFffDefElitesData(string RawDataStanding, CS_Code.Utopia_Province_Data_Captured_Type_Military UPDCTM)
        {
            foreach (Match match in URegEx._findAngelMilOFFDEFELITES.Matches(RawDataStanding))
            {
                if (URegEx._findAngelMilDefense.Match(match.Value).Success && URegEx._findAngelMilOffense.Match(match.Value).Success)
                {
                    UPDCTM.Elites = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                    UPDCTM.Military_Population += UPDCTM.Elites;
                    UPDCTM.Elites_Off_Pts = Convert.ToDecimal(URegEx.rgxQuantitiesWithComma.Match(match.Value).NextMatch().Value.Replace(",", ""));
                    UPDCTM.Elites_Def_Pts = Convert.ToDecimal(URegEx.rgxQuantitiesWithComma.Match(match.Value).NextMatch().NextMatch().Value.Replace(",", ""));
                }
                else if (URegEx._findAngelMilDefense.Match(match.Value).Success)
                {
                    UPDCTM.Regs_Def = Convert.ToDecimal(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                    UPDCTM.Military_Population += UPDCTM.Regs_Def;
                    //added nextmatch to get second quantity of Archers: 16,760 (128,214 defense)
                    UPDCTM.Regs_Def_Pts = Convert.ToDecimal(URegEx.rgxQuantitiesWithComma.Match(match.Value).NextMatch().Value.Replace(",", ""));
                }
                else if (URegEx._findAngelMilOffense.Match(match.Value).Success)
                {
                    UPDCTM.Regs_Off = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                    UPDCTM.Military_Population += UPDCTM.Regs_Off;
                    UPDCTM.Regs_Off_Pts = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).NextMatch().Value.Replace(",", ""));
                }
            }
        }
        /// <summary>
        /// Gets the Soldiers Data.
        /// </summary>
        /// <param name="URegEx.rgxFindNetDONumber">Finds the number for the data</param>
        /// <param name="RawDataStanding">string for Data</param>
        /// <param name="UPDCTM">Data Soource to fill.</param>
        /// <returns>The Raw Data string without soldiers dat.</returns>
        public  static string GetSoldiersData(string RawDataStanding, CS_Code.Utopia_Province_Data_Captured_Type_Military UPDCTM)
        {
            if (URegEx.rgxFindSoldiersData.Match(RawDataStanding).Success)
            {
                UPDCTM.Soldiers = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx.rgxFindSoldiersData.Match(RawDataStanding).Value).Value.Replace(",", ""));
                UPDCTM.Military_Population += UPDCTM.Soldiers;
                UPDCTM.Soldiers_Off_Pts = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx.rgxFindSoldiersData.Match(RawDataStanding).Value).NextMatch().Value.Replace(",", ""));
                UPDCTM.Soldiers_Def_Pts = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx.rgxFindSoldiersData.Match(RawDataStanding).Value).NextMatch().NextMatch().Value.Replace(",", ""));
                RawDataStanding = RawDataStanding.Replace(URegEx.rgxFindSoldiersData.Match(RawDataStanding).Value, "");
            }
            else if (URegEx.rgxFindSoldiersDataTemple.Match(RawDataStanding).Success)
            {
                UPDCTM.Soldiers = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx.rgxFindSoldiersDataTemple.Match(RawDataStanding).Value).Value.Replace(",", ""));
                UPDCTM.Military_Population += UPDCTM.Soldiers;
                UPDCTM.Soldiers_Off_Pts = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx.rgxFindSoldiersDataTemple.Match(RawDataStanding).Value).NextMatch().Value.Replace(",", ""));
                UPDCTM.Soldiers_Def_Pts = UPDCTM.Soldiers_Off_Pts;
                RawDataStanding = RawDataStanding.Replace(URegEx.rgxFindSoldiersDataTemple.Match(RawDataStanding).Value, "");
            }
            return RawDataStanding;
        }
        /// <summary>
        /// Sets the War horses data
        /// </summary>
        /// <param name="URegEx.rgxFindNetDONumber">Finds to number.</param>
        /// <param name="RawDataStanding">Finds the raw data string.</param>
        /// <param name="UPDCTM"></param>
        /// <returns>Raw data string without war horse data in it.</returns>
        public  static string GetWarHorseData(string RawDataStanding, CS_Code.Utopia_Province_Data_Captured_Type_Military UPDCTM)
        {
            if (URegEx.rgxFindWarHorses.Match(RawDataStanding).Success)
            {
                UPDCTM.Horses = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx.rgxFindWarHorses.Match(RawDataStanding).Value).Value.Replace(",", ""));
                UPDCTM.Horses_Pts = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx.rgxFindWarHorses.Match(RawDataStanding).Value).NextMatch().Value.Replace(",", ""));
                RawDataStanding = RawDataStanding.Replace(URegEx.rgxFindWarHorses.Match(RawDataStanding).Value, "");
            }
            return RawDataStanding;
        }
        /// <summary>
        /// Gets the Type ID number of the CE entered type.
        /// </summary>
        /// <param name="RawLine"></param>
        /// <returns></returns>
        //private static int Sql.GetCeTypeId(string ceType, Guid userID)
        //{
        //    int uid = GetCETypes.Where(x => x.name == ceType.Trim()).Select(x => x.uid).FirstOrDefault();
        //    if (uid == 0)
        //    {
        //        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        //        CS_Code.Utopia_Kingdom_CE_Type_Pull UKCETPInsert = new CS_Code.Utopia_Kingdom_CE_Type_Pull();
        //        UKCETPInsert.CE_Type = ceType.Trim();
        //        UKCETPInsert.Added_By_DateTime = DateTime.UtcNow;
        //        UKCETPInsert.Added_By_UserID = userID;
        //        db.Utopia_Kingdom_CE_Type_Pulls.InsertOnSubmit(UKCETPInsert);
        //        db.SubmitChanges();
        //        HttpRuntime.Cache.Remove("CETypes");
        //        return UKCETPInsert.uid;
        //    }
        //    return uid;
        //}
        //static List<CEType> URegEx._getCETypes;
        //public static List<CEType> GetCETypes
        //{
        //    get
        //    {
        //        if (URegEx._getCETypes == null)
        //        {
        //            if (HttpRuntime.Cache["CETypes"] == null)
        //            {
        //                CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        //                List<CEType> ceTypes = (from UPRP in db.Utopia_Kingdom_CE_Type_Pulls
        //                                        select new CEType
        //                                        {
        //                                            name = UPRP.CE_Type,
        //                                            uid = UPRP.uid,
        //                                        }).ToList();
        //                HttpRuntime.Cache.Add("CETypes", ceTypes, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.NotRemovable, null);
        //                URegEx._getCETypes = ceTypes;
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    URegEx._getCETypes = (List<CEType>)HttpRuntime.Cache["CETypes"];
        //                }
        //                catch
        //                {
        //                    HttpRuntime.Cache.Remove("CETypes");
        //                    return GetCETypes;
        //                }
        //            }
        //        }
        //        return URegEx._getCETypes;
        //    }
        //}
        /// <summary>
        /// Starts the Kingdom insertion for the Kingdoms
        /// </summary>
        /// <param name="KingdomName">Name of the Kingdom to be inserted</param>
        /// <param name="ServerID">Server ID of which Kingdom was selected</param>
        /// <param name="KingdomIsland">Kingdom Island for in game play</param>
        /// <param name="KingdomLocation">Kingdom location for in game play</param>
        /// <param name="ProvinceName">Province name for in game play.</param>
        /// <returns></returns>
        private static Guid StartKingdom(KingdomClass k, int ServerID, string ProvinceName, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            //removes the cached display kingdoms because this code will be adding a kingdom to the list of displayed kingdoms.
            //Inserts Kingdom to start the Kingdom
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Kingdom_Info UKI = new CS_Code.Utopia_Kingdom_Info();
            UKI.Kingdom_Name = k.Kingdom_Name;
            UKI.Server_ID = ServerID;
            UKI.Added_By_User_ID = currentUser.PimpUser.UserID;
            UKI.Owner_User_ID = currentUser.PimpUser.UserID;
            UKI.Added_By_DateTime = DateTime.UtcNow;
            UKI.Kingdom_ID = System.Guid.NewGuid();
            UKI.Kingdom_Island = k.Kingdom_Island;
            UKI.Kingdom_Location = k.Kingdom_Location;
            UKI.War_Wins = k.War_Wins;
            UKI.War_Losses = k.War_Losses;
            UKI.Stance = k.Stance;
            UKI.Owner_Kingdom_ID = UKI.Kingdom_ID;
            UKI.Updated_By_DateTime = DateTime.UtcNow;
            UKI.Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            db.Utopia_Kingdom_Infos.InsertOnSubmit(UKI);
            db.SubmitChanges();
            //Inserts the Province for which the user entered in the new kingdom.
            Guid provinceID = Guid.NewGuid();
            HttpContext.Current.Session["SubmittedData"] += " StartKingdom: ";

            //Iterates through all provinces and adds the provinces information to the two tables below.
            foreach (var prov in k.Provinces)
            {
                switch (prov.Province_Name)
                {
                    case "- Awaiting Activation -":
                    case "- Unclaimed -":
                        break;
                    default:
                        switch (prov.Networth.ToString())
                        {
                            case "Dead": // A dead province
                                break;
                            default:
                                CS_Code.Utopia_Province_Data_Captured_Gen UPDCG = new CS_Code.Utopia_Province_Data_Captured_Gen();
                                UPDCG.Added_By_User_ID = currentUser.PimpUser.UserID;
                                UPDCG.Updated_By_DateTime = DateTime.UtcNow;
                                UPDCG.Kingdom_ID = UKI.Kingdom_ID;
                                UPDCG.Kingdom_Island = UKI.Kingdom_Island;
                                UPDCG.Kingdom_Location = UKI.Kingdom_Location;
                                UPDCG.Owner_Kingdom_ID = UKI.Kingdom_ID;
                                UPDCG.Province_Name = prov.Province_Name;

                                if (prov.Province_Name.ToLower() == ProvinceName.ToLower()) // if the creator of the kingdom is == to the match.
                                {
                                    UPDCG.Province_ID = provinceID;
                                    HttpContext.Current.Session["SubmittedData"] += provinceID.ToString();
                                    UPDCG.Owner = 1;
                                    UPDCG.Nobility_ID = prov.Nobility_ID;
                                }
                                else
                                {
                                    UPDCG.Province_ID = Guid.NewGuid();
                                    UPDCG.Nobility_ID = prov.Nobility_ID;
                                }
                                UPDCG.Updated_By_DateTime = DateTime.UtcNow;
                                UPDCG.Networth = prov.Networth;
                                UPDCG.Updated_By_Province_ID = UKI.Added_By_User_ID;
                                UPDCG.Race_ID = prov.Race_ID;
                                UPDCG.Land = prov.Land;
                                db.Utopia_Province_Data_Captured_Gens.InsertOnSubmit(UPDCG);
                                db.SubmitChanges();
                                break;
                        }
                        break;
                }
            }

            attachProvinceUser(provinceID);
            return UKI.Kingdom_ID;
        }
        /// <summary>
        /// attaches province to user along with adding the current kingdom Columns or the most popular columns
        /// </summary>
        /// <param name="ProvinceCode"></param>
        public static void attachProvinceUser(Guid provinceID)
        {
            PimpUserWrapper pimpUser = new PimpUserWrapper();
            HttpContext.Current.Session["SubmittedData"] += "AttachProvince:" + provinceID + ":" + pimpUser.PimpUser.UserID;
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();


            if (pimpUser.PimpUser.ProvincesOwned != null && pimpUser.PimpUser.ProvincesOwned.Count > 0)
            {
            }
            else
            {       //gets the kingdom default columns if there are any..
                var kdID = (from xx in db.Utopia_Province_Data_Captured_Gens
                            where xx.Province_ID == provinceID
                            select xx.Owner_Kingdom_ID).FirstOrDefault();
                var defaultKingdomColumns = (from xx in db.Utopia_Column_Names
                                             from yy in db.Utopia_Column_Data_Types
                                             where xx.Data_Type_ID == yy.uid
                                             where xx.User_ID == kdID
                                             select new { xx.Column_IDs, xx.Data_Type_ID, yy.Column_Data_Type_Name });
                if (defaultKingdomColumns.FirstOrDefault() != null)
                {
                    foreach (var item in defaultKingdomColumns)
                    {
                        CS_Code.Utopia_Column_Name ucdt = new CS_Code.Utopia_Column_Name();
                        ucdt.Column_IDs = item.Column_IDs;
                        ucdt.Data_Type_ID = item.Data_Type_ID;
                        ucdt.DateTime_Added = DateTime.UtcNow;
                        ucdt.User_ID = pimpUser.PimpUser.UserID;
                        db.Utopia_Column_Names.InsertOnSubmit(ucdt);
                        pimpUser.updateColumnSetsForUser(item.Column_Data_Type_Name, ucdt);
                    }
                    db.SubmitChanges();
                }
                else
                {

                    string temp = string.Empty;
                    temp += UtopiaHelper.Instance.ColumnNames.Where(x => x.columnName == "Province Name - Race/Personality").FirstOrDefault().uid + ":";
                    temp += UtopiaHelper.Instance.ColumnNames.Where(x => x.columnName == "CB").FirstOrDefault().uid + ":";
                    temp += UtopiaHelper.Instance.ColumnNames.Where(x => x.columnName == "Survey").FirstOrDefault().uid + ":";
                    temp += UtopiaHelper.Instance.ColumnNames.Where(x => x.columnName == "SoS").FirstOrDefault().uid + ":";
                    temp += UtopiaHelper.Instance.ColumnNames.Where(x => x.columnName == "SoM").FirstOrDefault().uid + ":";
                    temp += UtopiaHelper.Instance.ColumnNames.Where(x => x.columnName == "Acres").FirstOrDefault().uid + ":";
                    temp += UtopiaHelper.Instance.ColumnNames.Where(x => x.columnName == "NW/Acre").FirstOrDefault().uid + ":";
                    temp += UtopiaHelper.Instance.ColumnNames.Where(x => x.columnName == "Effects").FirstOrDefault().uid + ":";
                    temp += UtopiaHelper.Instance.ColumnNames.Where(x => x.columnName == "Ops").FirstOrDefault().uid + ":";

                    //var getMostPopColumns = (from xx in db.Utopia_Column_Names
                    //                         group xx by xx.Data_Type_ID into gg
                    //                         select new
                    //                         {
                    //                             gg.Key,
                    //                             count = gg.Count(),
                    //                             columns = (from yy in db.Utopia_Column_Names
                    //                                        where yy.Data_Type_ID == gg.Key
                    //                                        select yy.Column_IDs).FirstOrDefault()
                    //                         }).OrderByDescending(p => p.count).FirstOrDefault();

                    CS_Code.Utopia_Column_Data_Type ucd = new CS_Code.Utopia_Column_Data_Type();
                    ucd.Column_Data_Type_Name = "General";
                    ucd.DateTime_Added = DateTime.UtcNow;
                    ucd.User_ID = pimpUser.PimpUser.UserID;
                    db.Utopia_Column_Data_Types.InsertOnSubmit(ucd);
                    db.SubmitChanges();

                    CS_Code.Utopia_Column_Name ucdt = new CS_Code.Utopia_Column_Name();
                    ucdt.Column_IDs = temp;
                    ucdt.Data_Type_ID = ucd.uid;
                    ucdt.DateTime_Added = DateTime.UtcNow;
                    ucdt.User_ID = pimpUser.PimpUser.UserID;
                    db.Utopia_Column_Names.InsertOnSubmit(ucdt);
                    db.SubmitChanges();
                    pimpUser.updateColumnSetsForUser(ucd.Column_Data_Type_Name, ucdt);
                }

            }
            var getProvince = (from xx in db.Utopia_Province_Data_Captured_Gens
                               where xx.Province_ID == provinceID
                               select xx).FirstOrDefault();

            getProvince.Added_By_User_ID = pimpUser.PimpUser.UserID;
            getProvince.Date_Time_User_ID_Linked = DateTime.UtcNow;
            getProvince.Owner_User_ID = pimpUser.PimpUser.UserID;
            db.SubmitChanges();

            pimpUser.addProvinceToProvincesOwned(getProvince);
            KingdomCache.removeProvinceFromKingdomCache((Guid)getProvince.Owner_Kingdom_ID, (Guid)provinceID, KingdomCache.getKingdom((Guid)getProvince.Owner_Kingdom_ID));
        }
        /// <summary>
        /// Updates the Kingdom from the Kingdom Page.
        /// </summary>
        /// <param name="ServerID"></param>
        /// <param name="KingdomName"></param>
        /// <param name="KingdomIsland"></param>
        /// <param name="KingdomLocation"></param>
        /// <param name="DT"></param>
        private static void UpdateKingdom(int ServerID, KingdomClass k, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            //removes the cached display kingdoms because this code will be adding a kingdom to the list of displayed kingdoms.
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            Guid provID = currentUser.PimpUser.CurrentActiveProvince;
            List<Guid> provinceIDs = new List<Guid>();
            var QueryKingdomID = (from UKII in db.Utopia_Kingdom_Infos
                                  where UKII.Owner_Kingdom_ID == currentUser.PimpUser.StartingKingdom
                                  where UKII.Kingdom_Island == k.Kingdom_Island
                                  where UKII.Kingdom_Location == k.Kingdom_Location
                                  select UKII).FirstOrDefault();

            if (QueryKingdomID == null || QueryKingdomID.Kingdom_ID == new System.Guid("00000000-0000-0000-0000-000000000000"))
            {
                CS_Code.Utopia_Kingdom_Info UKI = new CS_Code.Utopia_Kingdom_Info();
                UKI.Kingdom_Name = k.Kingdom_Name;
                UKI.Server_ID = ServerID;
                UKI.Added_By_User_ID = currentUser.PimpUser.UserID;
                UKI.Added_By_DateTime = DateTime.UtcNow;
                UKI.Kingdom_ID = System.Guid.NewGuid();
                UKI.Kingdom_Island = k.Kingdom_Island;
                UKI.Kingdom_Location = k.Kingdom_Location;
                UKI.War_Wins = k.War_Wins;
                UKI.War_Losses = k.War_Losses;
                UKI.Stance = k.Stance;
                UKI.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
                UKI.Updated_By_DateTime = DateTime.UtcNow;
                UKI.Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
                db.Utopia_Kingdom_Infos.InsertOnSubmit(UKI);
                db.SubmitChanges();

                List<ProvinceClass> provId = UtopiaParser.GetProvincesInKingdomToDisplay("All", new Guid(), currentUser.PimpUser.StartingKingdom, cachedKingdom);
                //Iterates through all provinces and adds the provinces information to the two tables below.
                foreach (var prov in k.Provinces)
                {
                    switch (prov.Province_Name)
                    {
                        case "- Awaiting Activation -": //not a real province
                        case "- Unclaimed -": // not a real province
                            break;
                        default:
                            switch (prov.Networth.ToString())
                            {
                                case "Dead": // A dead province
                                    break;
                                default:
                                    var checkProv = (from xx in provId
                                                     where xx.Province_Name == prov.Province_Name
                                                     where xx.Kingdom_Island == UKI.Kingdom_Island
                                                     where xx.Kingdom_Location == UKI.Kingdom_Location
                                                     select xx.Province_ID).FirstOrDefault();
                                    if (checkProv == null || checkProv == new System.Guid("00000000-0000-0000-0000-000000000000"))//Cant find province. So I create one.
                                    {
                                        CS_Code.Utopia_Province_Data_Captured_Gen UPDCG = new CS_Code.Utopia_Province_Data_Captured_Gen();
                                        UPDCG.Kingdom_Island = UKI.Kingdom_Island;
                                        UPDCG.Kingdom_Location = UKI.Kingdom_Location;
                                        UPDCG.Added_By_User_ID = UKI.Added_By_User_ID;
                                        UPDCG.Kingdom_ID = UKI.Kingdom_ID;
                                        UPDCG.Owner_Kingdom_ID = UKI.Owner_Kingdom_ID;
                                        UPDCG.Province_Name = prov.Province_Name;
                                        UPDCG.Province_ID = Guid.NewGuid();
                                        UPDCG.Updated_By_DateTime = DateTime.UtcNow;
                                        UPDCG.Networth = prov.Networth;
                                        UPDCG.Updated_By_Province_ID = UKI.Added_By_User_ID;
                                        UPDCG.Race_ID = prov.Race_ID;
                                        UPDCG.Nobility_ID = prov.Nobility_ID; //DataAccess.NobilityNamePull(SetMonarch(row[2].ToString(), UPDCG), currentUser.PimpUser.UserID);
                                        UPDCG.Land = prov.Land;// Convert.ToInt32(row[4].ToString().Replace(",", "").Replace("-", "0"));
                                        db.Utopia_Province_Data_Captured_Gens.InsertOnSubmit(UPDCG);
                                        provinceIDs.Add(UPDCG.Province_ID);
                                    }
                                    else //Found Province so no need to create one.
                                    {
                                        var getProvince = (from xx in db.Utopia_Province_Data_Captured_Gens
                                                           where xx.Province_ID == checkProv
                                                           select xx).FirstOrDefault();
                                        getProvince.Networth = prov.Networth;// Convert.ToInt32(Static.URegEx.rgxNumber.Match(row[3].ToString().Replace(",", "").Replace("gc", "").Replace("-", "0")).Value);
                                        getProvince.Race_ID = prov.Race_ID;// RaceNamePull(row[1].ToString(), currentUser.PimpUser.UserID);
                                        getProvince.Nobility_ID = prov.Nobility_ID;// DataAccess.NobilityNamePull(SetMonarch(row[2].ToString(), getProvince), currentUser.PimpUser.UserID);
                                        getProvince.Land = prov.Land;// Convert.ToInt32(row[4].ToString().Replace(",", "").Replace("-", "0"));
                                        getProvince.Updated_By_DateTime = DateTime.UtcNow;
                                        getProvince.Updated_By_Province_ID = provID;
                                        getProvince.Kingdom_ID = UKI.Kingdom_ID;
                                        provinceIDs.Add(checkProv);
                                    }
                                    break;
                            }
                            break;
                    }
                }
                db.SubmitChanges();
                KingdomCache.refreshKingdomInKingdomCache(currentUser.PimpUser.StartingKingdom, UKI.Kingdom_ID, cachedKingdom);
            }
            else
            {
                QueryKingdomID.Stance = k.Stance;
                QueryKingdomID.Kingdom_Name = k.Kingdom_Name;
                QueryKingdomID.Updated_By_DateTime = DateTime.UtcNow;
                QueryKingdomID.Updated_By_Province_ID = provID;
                QueryKingdomID.War_Wins = k.War_Wins;
                QueryKingdomID.War_Losses = k.War_Losses;
                foreach (var prov in k.Provinces)
                {
                    switch (prov.Province_Name)
                    {
                        case "- Awaiting Activation -":
                        case "- Unclaimed -":
                            break;
                        default:
                            switch (prov.Networth.ToString())
                            {
                                case "Dead":
                                    break;
                                default:
                                    var queryProvinceID = (from UPIII in db.Utopia_Province_Data_Captured_Gens
                                                           where UPIII.Kingdom_ID == QueryKingdomID.Kingdom_ID
                                                           where UPIII.Province_Name == prov.Province_Name
                                                           where UPIII.Owner_Kingdom_ID == currentUser.PimpUser.StartingKingdom
                                                           select UPIII).FirstOrDefault();

                                    if (queryProvinceID == null) //doesn'tfind province in kingdom...
                                    {
                                        queryProvinceID = (from xx in db.Utopia_Province_Data_Captured_Gens
                                                           where xx.Owner_Kingdom_ID == currentUser.PimpUser.StartingKingdom
                                                           where xx.Province_Name == prov.Province_Name
                                                           where xx.Kingdom_Island == k.Kingdom_Island
                                                           where xx.Kingdom_Location == k.Kingdom_Location
                                                           select xx).FirstOrDefault();
                                        if (queryProvinceID == null)//cantfind a random province with this info.
                                        {
                                            queryProvinceID = new CS_Code.Utopia_Province_Data_Captured_Gen();
                                            queryProvinceID.Province_Name = prov.Province_Name;
                                            queryProvinceID.Kingdom_Island = k.Kingdom_Island;
                                            queryProvinceID.Kingdom_Location = k.Kingdom_Location;
                                            queryProvinceID.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
                                            queryProvinceID.Kingdom_ID = QueryKingdomID.Kingdom_ID;
                                            queryProvinceID.Added_By_User_ID = currentUser.PimpUser.UserID;
                                            queryProvinceID.Province_ID = Guid.NewGuid();
                                            queryProvinceID.Updated_By_Province_ID = provID;
                                            queryProvinceID.Updated_By_DateTime = DateTime.UtcNow;
                                            db.Utopia_Province_Data_Captured_Gens.InsertOnSubmit(queryProvinceID);

                                        }
                                        queryProvinceID.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
                                        queryProvinceID.Kingdom_ID = QueryKingdomID.Kingdom_ID;
                                    }
                                    queryProvinceID.Networth = prov.Networth;// Convert.ToInt32(Static.URegEx.rgxNumber.Match(dr[3].ToString().Replace(",", "").Replace("gc", "").Replace("-", "0")).Value);
                                    queryProvinceID.Land = prov.Land;// Convert.ToInt32(dr[4].ToString().Replace(",", "").Replace("-", "0"));
                                    queryProvinceID.Race_ID = prov.Race_ID;// RaceNamePull(dr[1].ToString(), currentUser.PimpUser.UserID);
                                    queryProvinceID.Nobility_ID = prov.Nobility_ID;// DataAccess.NobilityNamePull(SetMonarch(dr[2].ToString(), queryProvinceID), currentUser.PimpUser.UserID);
                                    queryProvinceID.Updated_By_DateTime = DateTime.UtcNow;
                                    queryProvinceID.Updated_By_Province_ID = provID;
                                    provinceIDs.Add(queryProvinceID.Province_ID);
                                    break;
                            }
                            break;
                    }
                    db.SubmitChanges(); //Dont move submit or it will delete provices before it updates provinces.
                }
                var deleteNonUpdateditem = (from updcg in db.Utopia_Province_Data_Captured_Gens
                                            where updcg.Updated_By_DateTime < DateTime.UtcNow.AddDays(-3)
                                            where updcg.Owner_Kingdom_ID == currentUser.PimpUser.StartingKingdom
                                            where updcg.Kingdom_ID == QueryKingdomID.Kingdom_ID
                                            where updcg.Owner_User_ID == null
                                            select updcg);
                db.Utopia_Province_Data_Captured_Gens.DeleteAllOnSubmit(deleteNonUpdateditem);
                db.SubmitChanges();
                KingdomCache.refreshKingdomInKingdomCache(currentUser.PimpUser.StartingKingdom,QueryKingdomID.Kingdom_ID, cachedKingdom);
            }
            KingdomCache.removeAllProvincesFromKingdomCache(currentUser.PimpUser.StartingKingdom, provinceIDs, cachedKingdom);
            //KingdomCache.addKingdomToKingdomCache(currentUser.PimpUser.StartingKingdom, k, cachedKingdom);
            
        }
        /// <summary>
        /// Gets the current column sets of the user.
        /// </summary>
        /// <returns></returns>
        public static string GetUserColumnsSet(int setID, List<ColumnSet> columnSets)
        {
            if (setID != 0 && setID != null)
            {
                return (from ucs in columnSets
                        where ucs.columnTypeID == setID
                        select ucs.columnIDs).FirstOrDefault();
            }
            else
            {
                return (from ucs in columnSets
                        where ucs.columnTypeID > 1
                        select ucs.columnIDs).FirstOrDefault();
            }
        }
        /// <summary>
        /// Gets the province names when targeting them via the kd page.  Returns the targeted province Names.
        /// </summary>
        /// <param name="kingdomID"></param>
        /// <param name="provinceIDs">Provinces Guids seperated by commas</param>
        /// <returns></returns>
        public static string GetTargetProvinces(Guid kingdomID, string provinceIDs, Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            List<ProvinceClass> pi = UtopiaParser.GetProvincesInKingdomToDisplay("none", kingdomID, ownerKingdomID, cachedKingdom);
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"divTargetProvBox\"><b>Targeting:</b> ");
            foreach (string item in provinceIDs.Split(','))
                if (item.Length > 20) //this bug was showing up, thats now why I check the length  ,1c6a09da-c086-49de-8f9f-fa8bbf0ddc90
                    sb.Append((from xx in pi where xx.Province_ID == new Guid(item.Replace(",", "").Trim()) select xx.Province_Name).FirstOrDefault() + ", ");
            sb.Append("</div>");
            return sb.ToString();
        }
        public static List<ProvinceClass> GetProvincesInKingdomToDisplay(string type, Guid kingdomID, Guid ownerKindomID, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            switch (type)
            {
                case "RandomFilter":
                    return (List<ProvinceClass>)HttpContext.Current.Session["FilteredProvinces"];
                case "Random": //Gets any provinces that are retired or not attached to a kingdom...
                    return ProvinceCache.getProvincesRandomThenAddToCache(ownerKindomID, db, cachedKingdom);
                case "All":
                    return cachedKingdom.Provinces;
                case "myaddy":
                    if (kingdomID != new Guid())
                        return (from zz in db.Utopia_Province_Data_Captured_Gens
                                where zz.Kingdom_ID == kingdomID
                                select new ProvinceClass
                                {
                                    Kingdom_ID = zz.Kingdom_ID,
                                    Province_ID = zz.Province_ID,
                                    Province_Name = zz.Province_Name,
                                    Kingdom_Island = zz.Kingdom_Island,
                                    Kingdom_Location = zz.Kingdom_Location,
                                }).ToList();
                    else
                        return (from zz in db.Utopia_Province_Data_Captured_Gens
                                where zz.Updated_By_DateTime.Value > DateTime.UtcNow.AddDays(-1)
                                select new ProvinceClass
                                {
                                    Kingdom_ID = zz.Kingdom_ID,
                                    Province_ID = zz.Province_ID,
                                    Province_Name = zz.Province_Name,
                                    Kingdom_Island = zz.Kingdom_Island,
                                    Kingdom_Location = zz.Kingdom_Location,
                                }).ToList();
                default:
                    return cachedKingdom.Provinces.Where(x => x.Kingdom_ID == kingdomID).ToList();
            }
        }


        public static string GetActivityLogOps(string provId, string kingID, Guid ownerKingdomID, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            Guid id = new Guid(provId);
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            List<ProvinceClass> provNames = new List<ProvinceClass>();
            if (kingID != "Random")//if the ID is from a kd-less provinces.
            {
                Guid kid = new Guid(kingID);
                provNames = (from xx in cachedKingdom.Provinces
                             where xx.Kingdom_ID == kid | xx.Kingdom_ID == ownerKingdomID
                             select xx).ToList();
            }
            else
            {
                provNames = (from yy in cachedKingdom.Provinces
                             where !(from xx in cachedKingdom.Kingdoms
                                     select xx.Kingdom_ID).Contains((Guid)yy.Kingdom_ID) || yy.Kingdom_ID == ownerKingdomID
                             select yy).ToList();
            }
            var provIDAway = (from xx in provNames
                              where xx.Kingdom_ID != ownerKingdomID
                              select xx.Province_ID);

            var itemProv = (from xx in db.Utopia_Province_Ops
                            from yy in db.Utopia_Province_Ops_Pulls
                            where yy.uid == xx.Op_ID
                            where xx.Owner_Kingdom_ID == ownerKingdomID
                            where xx.Added_By_Province_ID == id
                            where provIDAway.Contains(xx.Directed_To_Province_ID)
                            select new Op
                            {
                                Added_By_Province_ID = xx.Added_By_Province_ID,
                                Expiration_Date = xx.Expiration_Date,
                                OP_Text = xx.OP_Text,
                                Directed_To_Province_ID = xx.Directed_To_Province_ID,
                                OP_Name = yy.OP_Name,
                                TimeStamp = xx.TimeStamp
                            }).ToList();
            List<OpsCompleted> allOps = new List<OpsCompleted>();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < itemProv.Count; i++)
            {
                OpsCompleted item = new OpsCompleted();
                item.PostedBy = provNames.Where(x => x.Province_ID == itemProv[i].Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault();
                item.PostedTime = itemProv[i].TimeStamp;
                item.Target = provNames.Where(x => x.Province_ID == itemProv[i].Directed_To_Province_ID).Select(x => x.Province_Name + " " + "(" + x.Kingdom_Island + ":" + x.Kingdom_Location + ")").FirstOrDefault();
                item.TimeDate = itemProv[i].TimeStamp.ToShortRelativeDate();
                switch (itemProv[i].OP_Name)
                {
                    case "Infiltrated":
                        item.alt = "Found " + itemProv[i].OP_Text + " thieves";
                        item.Type = "<img src=\"" + ImagesStatic.Infiltrated + "\" />";
                        break;
                    case "stoleRunes":
                        item.alt = "Stole " + itemProv[i].OP_Text + " runes " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "TSR";
                        break;
                    case "stoleFood":
                        item.alt = "Stole " + itemProv[i].OP_Text + " bushels " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "SF";
                        break;
                    case "stoleMoney":
                        item.alt = "Stole " + itemProv[i].OP_Text;
                        item.Type = "<img src=\"" + ImagesStatic.StoleMoney + "\" />";
                        break;
                    case "bribedGen":
                        item.alt = "Bribed a General " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "BG";
                        break;
                    case "assasinate":
                        item.alt = "Killed " + itemProv[i].OP_Text + " troops";
                        item.Type = "<img src=\"" + ImagesStatic.Assasinate + "\" />";
                        break;
                    case "kidnapped":
                        item.alt = "Kidnapped " + itemProv[i].OP_Text + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "TKN";
                        break;
                    case "convertTroops":
                        item.alt = "Converted " + itemProv[i].OP_Text + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "CT";
                        break;
                    case "bribed":
                        item.alt = "Bribed " + itemProv[i].OP_Text + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "BT";
                        break;
                    case "freePrisoners":
                        item.alt = "Freed " + itemProv[i].OP_Text + " Prisoners ";
                        item.Type = "<img src=\"" + ImagesStatic.FreedPrisoners+ "\" />";
                        break;
                    case "burnedAcres":
                        item.alt = "Burned " + itemProv[i].OP_Text + " Acres " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "BA";
                        break;
                    case "storms":
                        item.alt = "Storms";
                        item.Type = "<img src=\"" + ImagesStatic.Storms + "\" />";
                        break;
                    case "vermin":
                        item.alt = "Vermin";
                        item.Type = "<img src=\"" + ImagesStatic.Vermin + "\" />";
                        break;
                    case "meteors":
                        item.alt = "Meteors";
                        item.Type = "<img src=\"" + ImagesStatic.Meteors + "\" />";
                        break;
                    case "greedySoldiers":
                        item.alt = "Soldiers are now Greedy";
                        item.Type = "<img src=\"" + ImagesStatic.Greed + "\" />";
                        break;
                    case "highBirth":
                        item.alt = "High Birth Rates from Love and Peace";
                        item.Type = "<img src=\"" + ImagesStatic.HighBirthRates + "\" />";
                        break;
                    case "inspireArmy":
                        item.alt = "Inspired Army to train harder";
                        item.Type = "<img src=\"" + ImagesStatic.InspireArmy + "\" />";
                        break;
                    case "minorProtection":
                        item.alt = "Minor Protection";
                        item.Type = "<img src=\"" + ImagesStatic.MinorProtection + "\" />";
                        break;
                    case "fog":
                        item.alt = "Fog";
                        item.Type = "<img src=\"" + ImagesStatic.Fog + "\" />";
                        break;
                    case "magicShield":
                        item.alt = "Magic Shield on Province";
                        item.Type = "<img src=\"" + ImagesStatic.MagicSheild + "\" />";
                        break;
                    case "fertileLands":
                        item.alt = "Fertile Lands";
                        item.Type = "<img src=\"" + ImagesStatic.FertileLands + "\" />";
                        break;
                    case "naturesBlessing":
                        item.alt = "Casted Natures Blessing";
                        item.Type = "<img src=\"" + ImagesStatic.NaturesBlessing + "\" />";
                        break;
                    case "fastBuilders":
                        item.alt = "Speed Builders";
                        item.Type = "<img src=\"" + ImagesStatic.BuildersBoon + "\" />";
                        break;
                    case "patriotism":
                        item.alt = "Patriots Defend the land";
                        item.Type = "<img src=\"" + ImagesStatic.Patriotism + "\" />";
                        break;
                    case "pitfalls":
                        item.alt = "Pitfalls";
                        item.Type = "<img src=\"" + ImagesStatic.Pitfalls + "\" />";
                        break;
                    case "explosions":
                        item.alt = "Explosions Rock Aid Shipments";
                        item.Type = "<img src=\"" + ImagesStatic.Explosions + "\" />";
                        break;
                    case "reflectMagic":
                        item.alt = "Reflecting Magic";
                        item.Type = "<img src=\"" + ImagesStatic.ReflectingMagic + "\" />";
                        break;
                    case "warSpoils":
                        item.alt = "War Spoils " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "WS";
                        break;
                    case "drought":
                        item.alt = "Drought";
                        item.Type = "<img src=\"" + ImagesStatic.Drought + "\" />";
                        break;
                    case "riots":
                        item.alt = "Riots effect Province";
                        item.Type = "<img src=\"" + ImagesStatic.Riots + "\" />";
                        break;
                    case "landLust":
                        item.alt = "Land Lust " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "LL";
                        break;
                    case "fireball":
                        item.alt = "Fireball";
                        item.Type = "<img src=\"" + ImagesStatic.Fireball + "\" />";
                        break;
                    case "tornadoes":
                        item.alt = "Tornadoes " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "T";
                        break;
                    case "reflectingMagic":
                        item.alt = "Province Reflecting Magic";
                        item.Type = "<img src=\"" + ImagesStatic.ReflectingMagic + "\" />";
                        break;
                    case "wakeDead":
                        item.alt = "Waking Dead to fight";
                        item.Type = "<img src=\"" + ImagesStatic.WakeDead + "\" />";
                        break;
                    case "plague":
                        item.alt = "Province has Plague";
                        item.Type = "<img src=\"" + ImagesStatic.Plague + "\" />";
                        break;
                    case "naturesBlessingFailed":
                        item.alt = "Storms or Drought Failed because of Natures Blessing";
                        item.Type = "<img src=\"" + ImagesStatic.NaturesBlessingFailed + "\" />";
                        break;
                    case "mystVort":
                        item.alt = "Mystic Vortex, " + itemProv[i].OP_Text;
                        item.Type = "<img src=\"" + ImagesStatic.MysticVortex + "\" />";
                        break;
                    case "goldToLead":
                        item.alt = "Gold was turned to Lead ";
                        item.Type = "<img src=\"" + ImagesStatic.GoldToLead + "\" />";
                        break;
                    case "treeGold":
                        item.alt = "Gold fell from the Trees " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "TOG";
                        break;
                    case "convertThieves":
                        item.alt = "Converted some thieves to guild " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "CT";
                        break;
                    case "exposedThieves":
                        item.alt = "Exposed thieves";
                        item.Type ="<img src=\"" + ImagesStatic.ExposedThieves+ "\" />";
                        break;
                    case "chastity":
                        item.alt = "Chastity Affecting the Women Folk. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "CH";
                        break;
                    default:
                        item.Type = "Something Broke, Will be Fixed soon.";
                        UtopiaParser.FailedAtTesting("'OpHistoryActivity'", itemProv[i].OP_Name, currentUser.PimpUser.UserID);
                        break;
                }
                allOps.Add(item);
            }
            allOps.OrderByDescending(xx => xx.PostedTime);
            sb.Append("<table id=\"tableInfo\" class=\"tblKingdomInfo\">");
            sb.Append("<thead><tr>");
            sb.Append("<th>Type</th><th>Targets</th><th>Performed By</th><th class=\"{sorter: 'fancyNumber'}\">When Performed</th>");
            sb.Append("</tr></thead>");
            for (int i = allOps.Count - 1; i > -1; i--)
            {
                switch (i % 2)
                {
                    case 1:
                        sb.Append("<tr title=\"" + allOps[i].alt + "\" class=\"d0\">");
                        break;
                    case 0:
                        sb.Append("<tr title=\"" + allOps[i].alt + "\" class=\"d1\">");
                        break;
                }
                sb.Append("<td>" + allOps[i].Type + "</td>");
                sb.Append("<td>" + allOps[i].Target + "</td>");
                sb.Append("<td>" + allOps[i].PostedBy + "</td>");
                sb.Append("<td>" + allOps[i].TimeDate + "</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }
    }
}
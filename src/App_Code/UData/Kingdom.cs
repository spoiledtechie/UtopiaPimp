using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Pimp.UCache;
using System.Threading.Tasks;

using Pimp.Utopia;
using Pimp.Users;

using PimpLibrary.Utopia.Players;
using Boomers.UserUtil;
using System.Text;

using PimpLibrary.Static.Enums;


namespace Pimp.UData
{
    /// <summary>
    /// Summary description for Kingdom
    /// </summary>
    public class Kingdom
    {
        public static int TAKE_DATA_WITHIN_HOURS = -96;


        public static void updateOwnedKingdoms(List<KingdomClass> kingdoms, Guid ownerKingdomId)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            //removes the cached display kingdoms because this code will be adding a kingdom to the list of displayed kingdoms.
            PimpUserWrapper pimpUser = new PimpUserWrapper();

            OwnedKingdomProvinces cachedKingdom = KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom);
            foreach (var kingdom in kingdoms)
            {
                var GetOwnedKingdoms = (from UKI in db.Utopia_Kingdom_Infos
                                        where UKI.Owner_Kingdom_ID == pimpUser.PimpUser.StartingKingdom && UKI.Kingdom_ID == kingdom.Kingdom_ID
                                        select UKI).FirstOrDefault();
                GetOwnedKingdoms.Retired = kingdom.Retired;
                GetOwnedKingdoms.Kingdom_Message = kingdom.Kingdom_Message;

                var kingdomCache = cachedKingdom.Kingdoms.Where(xx => xx.Kingdom_ID == kingdom.Kingdom_ID).FirstOrDefault();
                if (kingdomCache != null)
                {
                    kingdomCache.Retired = kingdom.Retired;
                    kingdomCache.Kingdom_Message = kingdom.Kingdom_Message;
                    KingdomCache.addKingdomToKingdomCache(ownerKingdomId, kingdomCache, cachedKingdom);
                }
            }
            db.SubmitChanges();
        }

        public static List<KingdomClass> getOwnedKingdoms(Guid ownerKingdomID)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var GetOwnedKingdoms = (from UKI in db.Utopia_Kingdom_Infos
                                    where UKI.Owner_Kingdom_ID == ownerKingdomID
                                    where UKI.Kingdom_ID != ownerKingdomID
                                    select new KingdomClass
                                    {
                                        Kingdom_Island = UKI.Kingdom_Island,
                                        Kingdom_Location = UKI.Kingdom_Location,
                                        Kingdom_ID = UKI.Kingdom_ID,
                                        Kingdom_Name = UKI.Kingdom_Name,
                                        Retired = UKI.Retired,
                                        Kingdom_Message = UKI.Kingdom_Message,
                                        War_Wins = UKI.War_Wins,
                                        War_Losses = UKI.War_Losses,
                                        Owner_Kingdom_ID = UKI.Owner_Kingdom_ID
                                    }).ToList();
            return GetOwnedKingdoms;
        }

        public static void AddKdTimeLimit(string value, Guid ownerKingdomID, Guid userID, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var getTimeSpan = (from xx in db.Utopia_Kingdom_Monarch_Settings
                               where xx.Owner_Kingdom_ID == ownerKingdomID
                               select xx).FirstOrDefault();
            if (getTimeSpan != null)
            {
                getTimeSpan.KdProvTimeLimit = Convert.ToInt32(value);
                getTimeSpan.Last_User_ID = userID;
                getTimeSpan.Last_Updated_DateTime = DateTime.UtcNow;
            }
            else
            {
                CS_Code.Utopia_Kingdom_Monarch_Setting ukms = new CS_Code.Utopia_Kingdom_Monarch_Setting();
                ukms.Owner_Kingdom_ID = ownerKingdomID;
                ukms.KdProvTimeLimit = Convert.ToInt32(value);
                ukms.Last_User_ID = userID;
                ukms.Last_Updated_DateTime = DateTime.UtcNow;
                db.Utopia_Kingdom_Monarch_Settings.InsertOnSubmit(ukms);
            }
            db.SubmitChanges();
            KingdomCache.updateKingdomSettings(ownerKingdomID, cachedKingdom);
        }


        /// <summary>
        /// Creates a time limit for ops to be displayed.
        /// </summary>
        /// <param name="value"></param>
        public static void AddKdOpAttackTimeLimit(string value, Guid ownerKingdomID, Guid userID, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            StringBuilder sb = new StringBuilder();
            var getTimeSpan = (from xx in db.Utopia_Kingdom_Monarch_Settings
                               where xx.Owner_Kingdom_ID == ownerKingdomID
                               select xx).FirstOrDefault();
            if (getTimeSpan != null)
            {
                getTimeSpan.KdOpsAttacksTimeLimit = Convert.ToInt32(value);
                getTimeSpan.Last_User_ID = userID;
                getTimeSpan.Last_Updated_DateTime = DateTime.UtcNow;
            }
            else
            {
                CS_Code.Utopia_Kingdom_Monarch_Setting ukms = new CS_Code.Utopia_Kingdom_Monarch_Setting();
                ukms.Owner_Kingdom_ID = ownerKingdomID;
                ukms.KdOpsAttacksTimeLimit = Convert.ToInt32(value);
                ukms.Last_User_ID = userID;
                ukms.Last_Updated_DateTime = DateTime.UtcNow;
                db.Utopia_Kingdom_Monarch_Settings.InsertOnSubmit(ukms);
            }
            db.SubmitChanges();
            PimpUserWrapper pimpUser = new PimpUserWrapper();
            pimpUser.removeUserCache();
            KingdomCache.updateKingdomSettings(ownerKingdomID, cachedKingdom);
        }
        /// <summary>
        /// sets the monarch message and adds to cache.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="kdID"></param>
        /// <param name="ownerKingdomID"></param>
        /// <param name="currentUser"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static string SetMonarchMessage(string message, string kdID, Guid ownerKingdomID, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var getMessage = (from xx in db.Utopia_Kingdom_Infos
                              where xx.Owner_Kingdom_ID == ownerKingdomID
                              where xx.Kingdom_ID == new Guid(kdID)
                              select xx).FirstOrDefault();
            getMessage.Kingdom_Message = message;
            db.SubmitChanges();

            KingdomClass kingdom = cachedKingdom.Kingdoms.Where(x => x.Kingdom_ID == getMessage.Kingdom_ID).FirstOrDefault();
            kingdom.Kingdom_Message = message;
                        KingdomCache.addKingdomToKingdomCache(getMessage.Owner_Kingdom_ID, kingdom, cachedKingdom);

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"divTitles\">" + message + "</div>");
            if (currentUser.PimpUser.MonarchType == MonarchType.admin)
                sb.Append("<div id=\"loadMonarchInfo\"><input id=\"btnChangeMessage\" onclick=\"javascript:UpdateMonarchMessage();\" type=\"button\" value=\"Change Message\" /></div>");

            return sb.ToString();
        }
        /// <summary>
        /// retires kingdom
        /// </summary>
        /// <param name="kingdomID"></param>
        /// <param name="ownerKingdomID"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static void retireKingdom(Guid kingdomID, Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            
           var kingdom = KingdomCache.getKingdom(ownerKingdomID, kingdomID, cachedKingdom);
           kingdom.Retired = true;
           KingdomCache.addKingdomToKingdomCache(ownerKingdomID, kingdom, cachedKingdom);

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var GetOwnedKingdoms = (from UKI in db.Utopia_Kingdom_Infos
                                    where UKI.Owner_Kingdom_ID == ownerKingdomID
                                    where UKI.Kingdom_ID == kingdomID
                                    select UKI).FirstOrDefault();
            GetOwnedKingdoms.Retired = true;
            db.SubmitChanges();

            
        }
        /// <summary>
        /// brings a kingdom out of retirement.
        /// </summary>
        /// <param name="kingdomID"></param>
        /// <param name="ownerKingdomID"></param>
        /// <param name="cachedKingdom"></param>
        public static void unRetireKingdom(Guid kingdomID, Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            var kingdom = KingdomCache.getKingdom(ownerKingdomID, kingdomID, cachedKingdom);
            kingdom.Retired = false;
            KingdomCache.addKingdomToKingdomCache(ownerKingdomID, kingdom, cachedKingdom);

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var GetOwnedKingdoms = (from UKI in db.Utopia_Kingdom_Infos
                                    where UKI.Owner_Kingdom_ID == ownerKingdomID
                                    where UKI.Kingdom_ID == kingdomID
                                    select UKI).FirstOrDefault();
            GetOwnedKingdoms.Retired = false;
            db.SubmitChanges();
        }
        /// <summary>
        /// updates the kingdom status for monarchs
        /// </summary>
        /// <param name="kingdomID"></param>
        /// <param name="ownerKingdomID"></param>
        /// <param name="cachedKingdom"></param>
        /// <param name="status"></param>
        public static void updateKingdomStatus(Guid kingdomID, Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom, string status)
        {
            var kingdom = KingdomCache.getKingdom(ownerKingdomID, kingdomID, cachedKingdom);
            kingdom.Kingdom_Message = status;
            KingdomCache.addKingdomToKingdomCache(ownerKingdomID, kingdom, cachedKingdom);

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var GetOwnedKingdoms = (from UKI in db.Utopia_Kingdom_Infos
                                    where UKI.Owner_Kingdom_ID == ownerKingdomID
                                    where UKI.Kingdom_ID == kingdomID
                                    select UKI).FirstOrDefault();
            GetOwnedKingdoms.Kingdom_Message = status;
            db.SubmitChanges();
        }
        /// <summary>
        /// gets a kingdom
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="kingdomID"></param>
        /// <returns></returns>
        public static KingdomClass getKingdom(Guid ownerKingdomID, Guid kingdomID)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            return (from xx in db.Utopia_Kingdom_Infos
                    where xx.Owner_Kingdom_ID == ownerKingdomID
                    where xx.Kingdom_ID == kingdomID
                    select new KingdomClass
                    {
                        Kingdom_ID = xx.Kingdom_ID,
                        Owner_Kingdom_ID = xx.Owner_Kingdom_ID,
                        Retired = xx.Retired,
                        Server_ID = xx.Server_ID,
                        Kingdom_Message = xx.Kingdom_Message,
                        Kingdom_Island = xx.Kingdom_Island,
                        Kingdom_Location = xx.Kingdom_Location,
                        Kingdom_Name = xx.Kingdom_Name,
                        Updated_By_DateTime = xx.Updated_By_DateTime,
                        War_Wins = xx.War_Wins,
                        War_Losses = xx.War_Losses,
                        Owner_User_ID = xx.Owner_User_ID,
                        Stance = xx.Stance,
                    }).FirstOrDefault();
        }

        /// <summary>
        /// gets the monarch settings of the kingdom
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static MonarchSettingsClass getMonarchSettings(Guid ownerKingdomID, CS_Code.UtopiaDataContext db)
        {
            return (from xx in db.Utopia_Kingdom_Monarch_Settings
                    where xx.Owner_Kingdom_ID == ownerKingdomID
                    select new MonarchSettingsClass
                    {
                        KdOpsAttacksTimeLimit = xx.KdOpsAttacksTimeLimit,
                        KdProvTimeLimit = xx.KdProvTimeLimit,
                    }).FirstOrDefault();
        }
        /// <summary>
        /// gets the contacts of the users for the kingdom
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static List<Contact> getKingdomContacts(Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
            return (from yy in adb.user_Informations
                    from zz in adb.vw_aspnet_Users
                    where zz.UserId == yy.user_ID
                    where cachedKingdom.Provinces.Where(x => x.Owner_User_ID != null).Select(x => x.Owner_User_ID).Contains(zz.UserId)
                    select new Contact
                    {
                        //zz.UserName,
                        State = yy.State,
                        Nick_Name = yy.Nick_Name,
                        GMT_Offset = yy.GMT_Offset,
                        Country = yy.Country,
                        City = yy.City,
                        user_ID = yy.user_ID,
                        Notes = yy.Notes,
                        phoneNumbers = (from pnp in adb.user_Phone_Number_Pulls
                                        from pn in adb.user_Phone_Numbers
                                        where pnp.uid == pn.Phone_Type
                                        where pn.user_ID == yy.user_ID
                                        select new PhoneType { uid = pn.uid, PhoneNumber = pn.Phone_Number, SMS = pn.SMS, Phone_Type = pnp.Phone_Type }).ToList(),
                        imNames = (from imp in adb.user_IM_Type_Pulls
                                   from im in adb.user_IMs
                                   where imp.uid == im.IM_Type
                                   where im.User_ID == yy.user_ID
                                   select new IMType { uid = im.uid, IM_Password_Bool = im.IM_Password_Bool, IM_Name = im.IM_Name, IM_Type = imp.IM_Type }).ToList()
                    }).ToList();
        }
        /// <summary>
        /// gets the first hit of the DB.  It loads the entire cache into working order.
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <returns></returns>
        public static OwnedKingdomProvinces getKingdomObject(Guid ownerKingdomID)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            OwnedKingdomProvinces ki = new OwnedKingdomProvinces();
            var monarch = getMonarchSettings(ownerKingdomID, db);
            if (monarch != null)
            {
                //if the value is greater than 0.  It needs to be negative as in days.
                if (monarch.KdOpsAttacksTimeLimit.GetValueOrDefault() > 0)
                    ki.KdOpsAttacksTimeLimit = monarch.KdOpsAttacksTimeLimit.GetValueOrDefault() * -1;
                else
                    ki.KdOpsAttacksTimeLimit = monarch.KdOpsAttacksTimeLimit.GetValueOrDefault();

                //if the value is greater than 0.  It needs to be negative as in days. 
                if (monarch.KdProvTimeLimit.GetValueOrDefault() > 0)
                    ki.KdProvTimeLimit = monarch.KdProvTimeLimit.GetValueOrDefault() * -1;
                else
                    ki.KdProvTimeLimit = monarch.KdProvTimeLimit.GetValueOrDefault();
            }
            else
            {
                ki.KdOpsAttacksTimeLimit = -24;
                ki.KdProvTimeLimit = -5;
            }
            ki.KingdomColumnSets = Column.getColumnSets(ownerKingdomID, db);
            ki.Kingdoms = getKingdoms(ownerKingdomID, db);
            List<Guid> kList = ki.Kingdoms.Select(x => x.Kingdom_ID).ToList();
            ki.Provinces = Province.getProvinces(ownerKingdomID, db, kList);
            List<ProvinceClass> randomProvincesList = Province.getProvincesRandomFirstTime(ownerKingdomID, db, ki.KdProvTimeLimit);
            ki.Provinces.AddRange(randomProvincesList);
            ki.RandomProvincesLastCheckedDb = DateTime.UtcNow;
            ki.Owner_Kingdom_ID = ownerKingdomID;
            ki.ProvincesWithoutUserContactsAdded = getNonAddedContacts(ki.Provinces, ownerKingdomID);
            ki.RandomProvinceCount = randomProvincesList.Count;
            ki.Attacks = Ops.getAttacks(ownerKingdomID, db, ki.KdOpsAttacksTimeLimit);
            ki.Effects = Ops.getEffects(ownerKingdomID, db, ki.KdOpsAttacksTimeLimit);
            return ki;
        }




        /// <summary>
        /// gets the provinces that have still not signed on to pimp
        /// </summary>
        /// <param name="provinces"></param>
        /// <param name="ownerKingdomID"></param>
        /// <returns></returns>
        public static List<string> getNonAddedContacts(List<ProvinceClass> provinces, Guid ownerKingdomID)
        {
            List<string> provinceNames = new List<string>();
            List<ProvinceClass> ProvinceNotChanged = provinces.Where(x => x.Kingdom_ID == ownerKingdomID).ToList().Where(x => x.Owner_User_ID != null).ToList();
            var provs = ProvinceNotChanged.Select(x => x.Owner_User_ID.Value).ToList();
            CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
            var getSignedUpIDs = (from xx in adb.user_Informations
                                  where provs.Contains(xx.user_ID)
                                  select xx.user_ID).ToList();
            for (int i = 0; i < ProvinceNotChanged.Count(); i++)
                if (!getSignedUpIDs.Contains(provs[i]))
                    provinceNames.Add(ProvinceNotChanged.Where(x => x.Owner_User_ID == provs[i]).FirstOrDefault().Province_Name);
            return provinceNames;
        }


        public static List<KingdomClass> getKingdoms(Guid ownerKingdomID, CS_Code.UtopiaDataContext db)
        {
            return (from xx in db.Utopia_Kingdom_Infos
                    where xx.Owner_Kingdom_ID == ownerKingdomID
                    where xx.Retired == false
                    select new KingdomClass
                    {
                        Kingdom_ID = xx.Kingdom_ID,
                        Owner_Kingdom_ID = xx.Owner_Kingdom_ID,
                        Retired = xx.Retired,
                        Server_ID = xx.Server_ID,
                        Kingdom_Message = xx.Kingdom_Message,
                        Kingdom_Island = xx.Kingdom_Island,
                        Kingdom_Location = xx.Kingdom_Location,
                        Kingdom_Name = xx.Kingdom_Name,
                        Updated_By_DateTime = xx.Updated_By_DateTime,
                        War_Wins = xx.War_Wins,
                        War_Losses = xx.War_Losses,
                        Owner_User_ID = xx.Owner_User_ID,
                        Stance = xx.Stance,
                    }).ToList();
        }
    }
}
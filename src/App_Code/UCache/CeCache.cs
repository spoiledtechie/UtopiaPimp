using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pimp.UData;
using Pimp.UParser;

using PimpLibrary.Communications;
using PimpLibrary.Utopia;
using PimpLibrary.Utopia.Ops;
using PimpLibrary.Utopia.Kingdom;
using App_Code.CS_Code.Worker;
using Pimp.Utopia;
using PimpLibrary.Utopia.Ce;

namespace Pimp.UCache
{
    /// <summary>
    /// Summary description for CeCache
    /// </summary>
    public class CeCache
    {
        public static void AddNewCeDataToKingdomWorker(List<CS_Code.Utopia_Kingdom_CE> ceList, OwnedKingdomProvinces cachedKingdom, Guid kingdomId, Guid ownerKingdomId)
        {
            var kingdom =getCeForKingdomCache(kingdomId, ownerKingdomId, cachedKingdom);
            if (kingdom != null)
            {
                var ceListOld = kingdom.CeList;
                if (ceListOld != null && ceListOld.Count > 0)
                {
                    CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
                    var tempCEListForDB = new List<CS_Code.Utopia_Kingdom_CE>();
                    Notification notification;
                    NotificationDetail notificationDetail;
                    Notifier notifier = new Notifier();

                    for (int i = 0; i < ceList.Count; i++)
                    {
                        //finds the CE Item in the list.
                        var checkForItem = (from xx in ceListOld
                                            where xx.CE_Type == ceList[i].CE_Type
                                            where xx.Source_Kingdom_Island == ceList[i].Source_Kingdom_Island
                                            where xx.Source_Kingdom_Location == ceList[i].Source_Kingdom_Location
                                            where xx.Source_Province_Name == ceList[i].Source_Province_Name
                                            where xx.Target_Kingdom_Island == ceList[i].Target_Kingdom_Island
                                            where xx.Target_Kingdom_Location == ceList[i].Target_Kingdom_Location
                                            where xx.Target_Province_Name == ceList[i].Target_Province_Name
                                            where xx.Utopia_Date_Day == ceList[i].Utopia_Date_Day
                                            where xx.Utopia_Month == ceList[i].Utopia_Month
                                            where xx.Utopia_Year == ceList[i].Utopia_Year
                                            select xx).FirstOrDefault();
                        if (checkForItem == null) // if it doesn't exist, we can add it to the OldList
                        {
                            var ceItem = ceList[i];
                            ceListOld.Add(ceItem); //adds the row to the cache.
                            db.Utopia_Kingdom_CEs.InsertOnSubmit(ceItem); //adds the row to the DB ready for insert

                            // Get the targetted province
                            var province = cachedKingdom.Provinces.Find(x => x.Province_Name == ceList[i].Target_Province_Name && x.Owner_User_ID != null);
                            if (province != null)
                            {
                                notification = new Notification(); // Notification class, collect and sends the data as an email.
                                notification.ProvinceId = province.Province_ID;
                                notification.ProvinceName = province.Province_Name;
                                notification.UserId = province.Owner_User_ID.Value;

                                notificationDetail = new NotificationDetail();
                                notificationDetail.Attacker = new AttackerOp{ Name = ceItem.Source_Province_Name, Location = new KingdomLocation(ceItem.Source_Kingdom_Island.GetValueOrDefault(), ceItem.Source_Kingdom_Location.GetValueOrDefault()) };
                                notificationDetail.Date = new UtopiaDate { Year = ceItem.Utopia_Year, Month = ceItem.Utopia_Month, Day = ceItem.Utopia_Date_Day };
                                notificationDetail.EventText = ceItem.Raw_Line;
                                notificationDetail.EventType = UtopiaHelper.Instance.CeTypes.Where(x => x.uid == ceItem.CE_Type).FirstOrDefault().name;
                                notificationDetail.Location = new KingdomLocation(ceItem.Target_Kingdom_Island.GetValueOrDefault(), ceItem.Target_Kingdom_Location.GetValueOrDefault());
                                notification.Details.Add(notificationDetail);

                                notifier.SendNotification(notification);
                            }
                        }
                    }
                    db.SubmitChanges(); //inserts thenew CE Rows
                }
                else
                    ceListOld = ceList;

                if (kingdom.CeList == null)
                    kingdom.CeList = new List<CS_Code.Utopia_Kingdom_CE>();

                kingdom.CeList = ceListOld;

                cachedKingdom.Kingdoms.Remove(cachedKingdom.Kingdoms.Where(x => x.Kingdom_ID == kingdomId).FirstOrDefault());
                cachedKingdom.Kingdoms.Add(kingdom);
                HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomId.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            }
        }
    

        /// <summary>
        /// gets the CE for kingdom cache
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="kingdomId"></param>
        /// <param name="ownerKingdomId"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static KingdomClass getCeForKingdomCache(int year, int month, Guid kingdomId, Guid ownerKingdomId, OwnedKingdomProvinces cachedKingdom)
        {
            var kingdom = cachedKingdom.Kingdoms.Where(x => x.Kingdom_ID == kingdomId).FirstOrDefault();
            //Gets the kingdom if its null
            if (kingdom == null)
            {
                kingdom =KingdomCache.getKingdom(ownerKingdomId, kingdomId, cachedKingdom);
                kingdom.CeList =Ce.getCeForKingdom(kingdomId, ownerKingdomId);
                cachedKingdom.Kingdoms.Add(kingdom);
                HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomId.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            }
            //gets the CE for the kingdom of the past 5 days if its null
            else if (kingdom.CeList == null)
            {
                cachedKingdom.Kingdoms.Remove(kingdom);
                kingdom.CeList = Ce.getCeForKingdom(kingdomId, ownerKingdomId);
                cachedKingdom.Kingdoms.Add(kingdom);
                HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomId.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            }
            else
            {
                if (kingdom.CeList.Where(x => x.Utopia_Year == year).Where(x => x.Utopia_Month == month).Count() == 0)
                {
                    cachedKingdom.Kingdoms.Remove(kingdom);
                    var ce = Ce.getCeForKingdom(year, month, kingdomId, ownerKingdomId);
                    for (int i = 0; i < ce.Count; i++)
                        kingdom.CeList.Add(ce[i]);
                    cachedKingdom.Kingdoms.Add(kingdom);
                    HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomId.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
                }
            }
            return kingdom;
        }

        public static KingdomClass getCeForKingdomCache(Guid kingdomId, Guid ownerKingdomId, OwnedKingdomProvinces cachedKingdom)
        {
            var kingdom = cachedKingdom.Kingdoms.Where(x => x.Kingdom_ID == kingdomId).FirstOrDefault();
            //Gets the kingdom if its null
            if (kingdom == null)
            {
                kingdom =Kingdom.getKingdom(ownerKingdomId, kingdomId);
                if (kingdom != null)
                {
                    kingdom.CeList = Ce.getCeForKingdom(kingdomId, ownerKingdomId);
                    cachedKingdom.Kingdoms.Add(kingdom);
                    HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomId.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
                }
            }
            //gets the CE for the kingdom of the past 5 days if its null
            else if (kingdom.CeList == null)
            {
                kingdom.CeList = Ce.getCeForKingdom(kingdomId, ownerKingdomId);
                cachedKingdom.Kingdoms.Add(kingdom);
                HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomId.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            }
            return kingdom;
        }
        public static List<CeCacheClass> getCEPersonalCache(string provinceName, Guid ownerKingdomID)
        {
            DateTime dt = DateTime.UtcNow;
            if (HttpRuntime.Cache["GetCEPersonalCache"] != null)
            {
                List<CeCacheClass> pis = (List<CeCacheClass>)HttpRuntime.Cache["GetCEPersonalCache"];
                var ch = (from xx in pis
                          where xx.Owner_Kingdom_ID == ownerKingdomID
                          where xx.Source_Province_Name == provinceName || xx.Target_Province_Name == provinceName
                          select xx).ToList();
                if (ch.Count() == 0)
                {
                    CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
                    var getCE = (from xx in db.Utopia_Kingdom_CEs
                                                             where xx.Source_Province_Name == provinceName || xx.Target_Province_Name == provinceName
                                 where xx.Owner_Kingdom_ID == ownerKingdomID
                                 select new CeCacheClass
                                 {
                                     //ceType = yy.CE_Type,
                                     //RawLine = xx.Raw_Line,
                                     //sourIL = xx.Source_Kingdom_Island + ":" + xx.Source_Kingdom_Location,
                                     targIL = "(" + xx.Target_Kingdom_Island + ":" + xx.Target_Kingdom_Location + ")",
                                     //size = xx.value,
                                     //provName = xx.Source_Province_Name,
                                     //provAttacked = xx.Target_Province_Name,
                                     Utopia_Month = xx.Utopia_Month,
                                     Utopia_Year = xx.Utopia_Year,
                                     Owner_Kingdom_ID = xx.Owner_Kingdom_ID,
                                     Kingdom_ID = xx.Kingdom_ID,
                                     uid = xx.uid,
                                     sourIL = "(" + xx.Source_Kingdom_Island + ":" + xx.Source_Kingdom_Location + ")",
                                     Source_Province_Name = xx.Source_Province_Name,
                                     Target_Province_Name = xx.Target_Province_Name,
                                     updateDateTime = dt
                                 }).ToList();
                    for (int i = 0; i < getCE.Count(); i++)
                        pis.Add(getCE[i]);
                    pis.RemoveAll((x) => x.updateDateTime < dt.AddHours(-12));
                    HttpRuntime.Cache["GetCEPersonalCache"] = pis;
                    return getCE;
                }
                else
                {
                    foreach (var item in pis.Where(x => x.Owner_Kingdom_ID == ownerKingdomID).Where(x => x.Source_Province_Name == provinceName || x.Target_Province_Name == provinceName))
                        item.updateDateTime = dt;
                    HttpRuntime.Cache["GetCEPersonalCache"] = pis;
                    return ch;
                }
            }
            else
            {
                List<CeCacheClass> pis = new List<CeCacheClass>();
                CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
                var getCE = (from xx in db.Utopia_Kingdom_CEs
                                                     where xx.Source_Province_Name == provinceName || xx.Target_Province_Name == provinceName
                             where xx.Owner_Kingdom_ID == ownerKingdomID
                             select new CeCacheClass
                             {
                                 //ceType = yy.CE_Type,
                                 //RawLine = xx.Raw_Line,
                                 //sourIL = xx.Source_Kingdom_Island + ":" + xx.Source_Kingdom_Location,
                                 targIL = "(" + xx.Target_Kingdom_Island + ":" + xx.Target_Kingdom_Location + ")",
                                 //size = xx.value,
                                 //provName = xx.Source_Province_Name,
                                 //provAttacked = xx.Target_Province_Name,
                                 Utopia_Month = xx.Utopia_Month,
                                 Utopia_Year = xx.Utopia_Year,
                                 Owner_Kingdom_ID = xx.Owner_Kingdom_ID,
                                 Kingdom_ID = xx.Kingdom_ID,
                                 uid = xx.uid,
                                 sourIL = "(" + xx.Source_Kingdom_Island + ":" + xx.Source_Kingdom_Location + ")",
                                 Source_Province_Name = xx.Source_Province_Name,
                                 Target_Province_Name = xx.Target_Province_Name,
                                 updateDateTime = dt
                             }).ToList();
                HttpRuntime.Cache["GetCEPersonalCache"] = getCE;
                return getCE;
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pimp.UData;

using Pimp.Utopia;
using PimpLibrary.Utopia.Ops;
using Boomers.UserUtil;

namespace Pimp.UCache
{
    /// <summary>
    /// Summary description for KingdomCache
    /// </summary>
    public class KingdomCache
    {
        public static KingdomClass getKingdom(Guid ownerKingdomID, Guid kingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            KingdomClass kingdom = cachedKingdom.Kingdoms.Where(x => x.Kingdom_ID == kingdomID).FirstOrDefault();
            if (kingdom == null)
            {
                return refreshKingdomInKingdomCache(ownerKingdomID, kingdomID, cachedKingdom).Kingdoms.Where(x => x.Kingdom_ID == kingdomID).FirstOrDefault();
            }
            return kingdom;
        }

        /// <summary>
        /// removes the contacts in the kingdom not signup up to pimp
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static OwnedKingdomProvinces removeContactsNotSignedUp(Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            cachedKingdom.ProvincesWithoutUserContactsAdded = Kingdom.getNonAddedContacts(cachedKingdom.Provinces, ownerKingdomID);
            HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return cachedKingdom;
        }



        /// <summary>
        /// refreshes the provinces in the kingdom cache
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static OwnedKingdomProvinces removeAllProvincesFromKingdomCache(Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            cachedKingdom.Provinces =Province.getProvinces(ownerKingdomID, CS_Code.UtopiaDataContext.Get(), cachedKingdom.Kingdoms.Select(x => x.Kingdom_ID).ToList());
            HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return cachedKingdom;
        }

        /// <summary>
        /// updates the column sets for the kingdom
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static OwnedKingdomProvinces updateColumnSetsForKingdom(Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            cachedKingdom.KingdomColumnSets =Column.getColumnSets(ownerKingdomID, CS_Code.UtopiaDataContext.Get());
            HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return cachedKingdom;
        }
        /// <summary>
        /// refreshes kingdom in kingdom cache
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="kingdomID"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static OwnedKingdomProvinces refreshKingdomInKingdomCache(Guid ownerKingdomID, Guid kingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            cachedKingdom.Kingdoms.Remove(cachedKingdom.Kingdoms.Where(x => x.Kingdom_ID == kingdomID).FirstOrDefault());
            var king = Kingdom.getKingdom(ownerKingdomID, kingdomID);
            if (king != null)
            {
                cachedKingdom.Kingdoms.Add(king);
                HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            }
            return cachedKingdom;
        }
        /// <summary>
        /// removes the kingdom from kingdom cache.
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="kingdomID"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static OwnedKingdomProvinces removeKingdomFromKingdomCache(Guid ownerKingdomID, Guid kingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            cachedKingdom.Kingdoms.Remove(cachedKingdom.Kingdoms.Where(x => x.Kingdom_ID == kingdomID).FirstOrDefault());
            HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return cachedKingdom;
        }
        /// <summary>
        /// adds a kingdom to the cache
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="kingdom"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static OwnedKingdomProvinces addKingdomToKingdomCache(Guid ownerKingdomID, KingdomClass kingdom, OwnedKingdomProvinces cachedKingdom)
        {
            cachedKingdom.Kingdoms.Remove(cachedKingdom.Kingdoms.Where(x => x.Kingdom_ID == kingdom.Kingdom_ID).FirstOrDefault());
            cachedKingdom.Kingdoms.Add(kingdom);
            HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return cachedKingdom;
        }
        /// <summary>
        /// updates the random province count to the kingdom cache
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="randomProvinceCount"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static OwnedKingdomProvinces UpdateKingdomRandomProvinceCount(Guid ownerKingdomID, int randomProvinceCount, OwnedKingdomProvinces cachedKingdom)
        {
            cachedKingdom.RandomProvinceCount = randomProvinceCount;
            HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return cachedKingdom;
        }

        /// <summary>
        /// refreshes a province in the cache
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="provinceID"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static OwnedKingdomProvinces removeProvinceFromKingdomCache(Guid ownerKingdomID, Guid provinceID, OwnedKingdomProvinces cachedKingdom)
        {
            cachedKingdom.Provinces.Remove(cachedKingdom.Provinces.Where(x => x.Province_ID == provinceID).FirstOrDefault());
            var prov = Province.getProvince(ownerKingdomID, provinceID, CS_Code.UtopiaDataContext.Get());
            if (prov != null)
            {
                cachedKingdom.Provinces.Add(prov);
                HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            }
            return cachedKingdom;
        }
        /// <summary>
        /// refreshes provinces in kingdom cache
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="provinceIDs"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static OwnedKingdomProvinces removeAllProvincesFromKingdomCache(Guid ownerKingdomID, List<Guid> provinceIDs, OwnedKingdomProvinces cachedKingdom)
        {
            foreach (var id in provinceIDs)
            {
                ProvinceClass prov = cachedKingdom.Provinces.Where(x => x.Province_ID == id).FirstOrDefault();
                cachedKingdom.Provinces.Remove(prov);
            }
            cachedKingdom.Provinces.AddRange(Province.getProvinces(ownerKingdomID, provinceIDs, CS_Code.UtopiaDataContext.Get()));
            HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);

            return cachedKingdom;
        }


        /// <summary>
        /// refreshes all kingdoms in cache
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static OwnedKingdomProvinces removeAllKingdomsFromKingdomCache(Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            cachedKingdom.Kingdoms = Kingdom.getKingdoms(ownerKingdomID, CS_Code.UtopiaDataContext.Get());
            HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return cachedKingdom;
        }

        /// <summary>
        /// clears and refreshes kingdom cache
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <returns></returns>
        public static OwnedKingdomProvinces removeKingdomCache(Guid ownerKingdomID)
        {
            HttpRuntime.Cache.Remove("KingdomCache" + ownerKingdomID.ToString());
            return getKingdom(ownerKingdomID);
        }
        /// <summary>
        /// gets the kingdom cache
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <returns></returns>
        public static OwnedKingdomProvinces getKingdom(Guid ownerKingdomID)
        {
            if (HttpRuntime.Cache["KingdomCache" + ownerKingdomID.ToString()] == null)
            {
                var cache = Kingdom.getKingdomObject(ownerKingdomID);
                HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cache, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
                return cache;
            }
            return (OwnedKingdomProvinces)HttpRuntime.Cache["KingdomCache" + ownerKingdomID.ToString()];
        }
        /// <summary>
        /// gets the column sets for the kingdom
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static OwnedKingdomProvinces getColumnSetsForKingdom(Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            if (cachedKingdom.KingdomColumnSets == null)
            {
                cachedKingdom.KingdomColumnSets =Column.getColumnSets(ownerKingdomID, CS_Code.UtopiaDataContext.Get());
                HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            }
            return cachedKingdom;
        }
        /// <summary>
        /// udates the kingdom settings for monarch kingdom
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static OwnedKingdomProvinces updateKingdomSettings(Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var monarch = Kingdom.getMonarchSettings(ownerKingdomID, db);
            if (monarch != null)
            {
                cachedKingdom.KdOpsAttacksTimeLimit = monarch.KdOpsAttacksTimeLimit;
                cachedKingdom.KdProvTimeLimit = monarch.KdProvTimeLimit.GetValueOrDefault(-5);
            }
            else
            {
                cachedKingdom.KdOpsAttacksTimeLimit = -24;
                cachedKingdom.KdProvTimeLimit = -5;
            }
            HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return cachedKingdom;
        }



        /// <summary>
        /// gets effects for the kingdom.
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static List<Op> getEffectsForKingdom(Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            if (cachedKingdom.Effects == null)
            {
                cachedKingdom.Effects =Ops.getEffects(ownerKingdomID, CS_Code.UtopiaDataContext.Get(), cachedKingdom.KdOpsAttacksTimeLimit);
                HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
                return cachedKingdom.Effects;
            }
            return cachedKingdom.Effects;
        }
        /// <summary>
        /// gets the kingdom contacts.
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static List<Contact> getContactsForKingdom(Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            if (cachedKingdom.Contacts == null)
            {
                cachedKingdom.Contacts = Kingdom.getKingdomContacts(ownerKingdomID, cachedKingdom);
                HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
                return cachedKingdom.Contacts;
            }
            return cachedKingdom.Contacts;
        }
    }
}
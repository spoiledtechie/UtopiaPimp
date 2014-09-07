using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Pimp.UData;
using Pimp.Utopia;
using Pimp.Users;


namespace Pimp.UCache
{
    /// <summary>
    /// Summary description for ProvinceCache
    /// </summary>
    public class ProvinceCache
    {
        public static List<CS_Code.Utopia_Province_Data_Captured_Type_Military> UpdateProvinceSOMToCache(CS_Code.Utopia_Province_Data_Captured_Gen gen, List<CS_Code.Utopia_Province_Data_Captured_Type_Military> mils, OwnedKingdomProvinces cachedKingdom)
        {
            var prov = getProvince((Guid)gen.Owner_Kingdom_ID, gen.Province_ID, cachedKingdom);
            if (prov != null)
            {
                prov.Updated_By_DateTime = gen.Updated_By_DateTime;
                prov.Updated_By_Province_ID = gen.Updated_By_Province_ID;
                prov.SOM_Updated_By_DateTime = gen.SOM_Updated_By_DateTime;
                prov.SOM_Updated_By_Province_ID = gen.SOM_Updated_By_Province_ID;

                prov.SOM_Requested = null;
                if (gen.Military_Current_Def.HasValue)
                {
                    prov.Military_Current_Def = gen.Military_Current_Def;
                    prov.Military_Net_Def = gen.Military_Net_Def;
                }
                if (gen.Military_Current_Off.HasValue)
                {
                    prov.Military_Current_Off = gen.Military_Current_Off;
                    prov.Military_Net_Off = gen.Military_Net_Off;
                }
                prov.Soldiers = gen.Soldiers;
                prov.Soldiers_Elites = gen.Soldiers_Elites;
                prov.Soldiers_Regs_Def = gen.Soldiers_Regs_Def;
                prov.Soldiers_Regs_Off = gen.Soldiers_Regs_Off;
                prov.Military_Efficiency_Off = gen.Military_Efficiency_Off;
                prov.Military_Efficiency_Def = gen.Military_Efficiency_Def;
                if (gen.Mil_Overall_Efficiency.HasValue)
                    prov.Mil_Overall_Efficiency = gen.Mil_Overall_Efficiency;
            }
            for (int i = 0; i < mils.Count; i++)
            {
                if (mils[i].Military_Location == 2)
                {
                    if (prov != null)
                    {
                        prov.Army_Out = 1;
                        prov.Army_Out_Expires = mils[i].Time_To_Return;
                    }
                }
                prov.SOM.Add(mils[i]);
            }

            var province = cachedKingdom.Provinces.Where(x => x.Province_ID == prov.Province_ID).FirstOrDefault();

            if (province != null)
                cachedKingdom.Provinces.Remove(province);

            cachedKingdom.Provinces.Add(prov);
            HttpRuntime.Cache.Add("KingdomCache" + ((Guid)prov.Owner_Kingdom_ID).ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);

            return mils;
        }




        public static CS_Code.Utopia_Province_Data_Captured_Gen updateStateAffairsToCache(CS_Code.Utopia_Province_Data_Captured_Gen gen, OwnedKingdomProvinces cachedKingdom)
        {
            var prov = cachedKingdom.Provinces.Where(x => x.Province_ID == gen.Province_ID).FirstOrDefault();

            if (prov != null)
            {
                prov.Land = gen.Land;
                prov.Population = gen.Population;
                prov.Thieves = gen.Thieves;
                prov.Thieves_Value_Type = gen.Thieves_Value_Type;
                prov.Wizards = gen.Wizards;
                prov.Wizards_Value_Type = gen.Wizards_Value_Type;
                prov.Daily_Income = gen.Daily_Income;
                prov.Peasents = gen.Peasents;
                prov.Networth = gen.Networth;
                prov.Honor = gen.Honor;
                prov.Updated_By_DateTime = gen.Updated_By_DateTime;
                prov.Updated_By_Province_ID = gen.Updated_By_Province_ID;
                if (prov.CB != null && prov.CB.LastOrDefault() != null)//Adds another CB of the most recent intel.
                {
                    var cb = prov.CB.LastOrDefault();
                    cb.Land = gen.Land;
                    cb.Population = gen.Population;
                    cb.Thieves = gen.Thieves;
                    cb.Thieves_Value_Type = gen.Thieves_Value_Type;
                    cb.Wizards = gen.Wizards;
                    cb.Wizards_Value_Type = gen.Wizards_Value_Type;
                    cb.Daily_Income = gen.Daily_Income;
                    cb.Peasents = gen.Peasents;
                    cb.Networth = gen.Networth;
                    cb.Updated_By_DateTime = gen.Updated_By_DateTime;
                    cb.Updated_By_Province_ID = gen.Updated_By_Province_ID;
                    prov.CB.Add(cb);
                }
            }
            else
            {
                prov = Province.getProvince((Guid)gen.Owner_Kingdom_ID, (Guid)gen.Province_ID, CS_Code.UtopiaDataContext.Get());
                cachedKingdom.Provinces.Add(prov);
            }
            HttpRuntime.Cache.Add("KingdomCache" + ((Guid)gen.Owner_Kingdom_ID).ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return gen;
        }


        /// <summary>
        /// updates the entire province to the cache.
        /// </summary>
        /// <param name="province"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static ProvinceClass updateProvinceToCache(ProvinceClass province, OwnedKingdomProvinces cachedKingdom)
        {
            var prov = cachedKingdom.Provinces.Where(x => x.Province_ID == province.Province_ID).FirstOrDefault();

            if (prov != null)
            {
                cachedKingdom.Provinces.Remove(prov);
            }
            else
            {
                province = Province.getProvince((Guid)province.Owner_Kingdom_ID, (Guid)province.Province_ID, CS_Code.UtopiaDataContext.Get());
            }
            cachedKingdom.Provinces.Add(prov);
            HttpRuntime.Cache.Add("KingdomCache" + ((Guid)province.Owner_Kingdom_ID).ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return province;
        }

        public static void removeProvinceFromCache(ProvinceClass province, OwnedKingdomProvinces cachedKingdom)
        {
            var prov = cachedKingdom.Provinces.Where(x => x.Province_ID == province.Province_ID).FirstOrDefault();
            if (prov != null)
            {
                cachedKingdom.Provinces.Remove(prov);
                HttpRuntime.Cache.Add("KingdomCache" + ((Guid)province.Owner_Kingdom_ID).ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            }
        }

        public static CS_Code.Utopia_Province_Data_Captured_CB UpdateProvinceCBToCache(CS_Code.Utopia_Province_Data_Captured_CB cb, CS_Code.Utopia_Province_Data_Captured_Gen gen, OwnedKingdomProvinces cachedKingdom)
        {
            var prov = cachedKingdom.Provinces.Where(x => x.Province_ID == cb.Province_ID).FirstOrDefault();

            if (prov != null)
            {
                cachedKingdom.Provinces.Remove(prov);
                prov.CB.Add(cb);
                prov.Updated_By_DateTime = DateTime.UtcNow;
                prov.CB_Requested = null;
                prov.Updated_By_Province_ID = cb.Updated_By_Province_ID;
                prov.CB_Updated_By_Province_ID = cb.Updated_By_Province_ID;
                prov.CB_Updated_By_DateTime = cb.Updated_By_DateTime;
                prov.Ruler_Name = gen.Ruler_Name;
                prov.Kingdom_ID = gen.Kingdom_ID;
                prov.Owner_Kingdom_ID = gen.Owner_Kingdom_ID;
                prov.Province_ID = gen.Province_ID;
                prov.Province_Name = gen.Province_Name;
                prov.Kingdom_Island = gen.Kingdom_Island;
                prov.Kingdom_Location = gen.Kingdom_Location;
                prov.Hit = gen.Hit;
                prov.Race_ID = gen.Race_ID;
                prov.Personality_ID = gen.Personality_ID;
                prov.Land = gen.Land;
                prov.Money = gen.Money;
                prov.Food = gen.Food;
                prov.Runes = gen.Runes;
                prov.Population = gen.Population;
                prov.Peasents = gen.Peasents;
                prov.Trade_Balance = gen.Trade_Balance;
                prov.Thieves = gen.Thieves;
                prov.Thieves_Value_Type = gen.Thieves_Value_Type;
                prov.Wizards = gen.Wizards;
                prov.Wizards_Value_Type = gen.Wizards_Value_Type;
                prov.Soldiers = gen.Soldiers;
                prov.War_Horses = gen.War_Horses;
                //prov.Prisoners = gen.Prisoners;
                prov.Mil_Overall_Efficiency = gen.Mil_Overall_Efficiency;
                prov.Military_Net_Off = gen.Military_Net_Off;
                prov.Military_Net_Def = gen.Military_Net_Def;
                prov.Building_Effectiveness = gen.Building_Effectiveness;
                prov.Soldiers_Regs_Off = gen.Soldiers_Regs_Off;
                prov.Soldiers_Regs_Def = gen.Soldiers_Regs_Def;
                prov.Soldiers_Elites = gen.Soldiers_Elites;
                prov.Daily_Income = gen.Daily_Income;
                prov.Updated_By_DateTime = gen.Updated_By_DateTime;
                prov.Updated_By_Province_ID = gen.Updated_By_Province_ID;
            }
            else
            {
                prov = Province.getProvince((Guid)cb.Owner_Kingdom_ID, (Guid)cb.Province_ID, CS_Code.UtopiaDataContext.Get());
            }
            cachedKingdom.Provinces.Add(prov);
            HttpRuntime.Cache.Add("KingdomCache" + ((Guid)cb.Owner_Kingdom_ID).ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return cb;
        }
        public static List<CS_Code.Utopia_Province_Data_Captured_Type_Military> getProvinceSOMCached(Guid provinceID, Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            var prov = cachedKingdom.Provinces.Where(x => x.Province_ID == provinceID).FirstOrDefault();

            if (prov != null && prov.SOM.Count > 0)
                return prov.SOM;

            if (prov != null)
                cachedKingdom.Provinces.Remove(prov);
            else
                prov = Province.getProvince(ownerKingdomID, provinceID, CS_Code.UtopiaDataContext.Get());

            prov.SOM = Province.GetProvinceSOMNotCached(provinceID, ownerKingdomID);
            cachedKingdom.Provinces.Add(prov);
            HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return prov.SOM;
        }

        public static CS_Code.Utopia_Province_Data_Captured_CB getProvinceCB(Guid provinceID, Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            var prov = cachedKingdom.Provinces.Where(x => x.Province_ID == provinceID).FirstOrDefault();

            if (prov != null && prov.CB != null)
                return prov.CB.OrderByDescending(x => x.Updated_By_DateTime).FirstOrDefault();

            if (prov != null)
                cachedKingdom.Provinces.Remove(prov);
            else
                prov = Province.getProvince(ownerKingdomID, provinceID, CS_Code.UtopiaDataContext.Get());
            
            var cb = Province.GetProvinceCBNotCached(provinceID, ownerKingdomID);
            if (prov.CB == null)
                prov.CB = new List<CS_Code.Utopia_Province_Data_Captured_CB>();

            if (cb != null)
                prov.CB.Add(cb);
            cachedKingdom.Provinces.Add(prov);
            HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return prov.CB.OrderByDescending(x => x.Updated_By_DateTime).FirstOrDefault();
        }
        public static ProvinceClass UpdateProvinceSOSToCache(CS_Code.Utopia_Province_Data_Captured_Science sos, OwnedKingdomProvinces cachedKingdom)
        {
            var prov = cachedKingdom.Provinces.Where(x => x.Province_ID == sos.Province_ID).FirstOrDefault();

            if (prov != null)
            {
                cachedKingdom.Provinces.Remove(prov);
                prov.SOS.Add(sos);
            }
            else
            {
                prov = Province.getProvince(sos.Owner_Kingdom_ID, sos.Province_ID, CS_Code.UtopiaDataContext.Get());
                prov.SOS.Add(sos);
            }

            cachedKingdom.Provinces.Add(prov);
            HttpRuntime.Cache.Add("KingdomCache" + sos.Owner_Kingdom_ID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return prov;
        }
        public static ProvinceClass updateProvinceSOSToCache(CS_Code.Utopia_Province_Data_Captured_Science science, CS_Code.Utopia_Province_Data_Captured_Gen gen, OwnedKingdomProvinces cachedKingdom)
        {
            var prov = cachedKingdom.Provinces.Where(x => x.Province_ID == science.Province_ID).FirstOrDefault();

            if (prov != null)
            {
                cachedKingdom.Provinces.Remove(prov);
                prov.SOS.Add(science);
                prov.Updated_By_DateTime = DateTime.UtcNow;
                prov.SOS_Requested = null;
                prov.Updated_By_Province_ID = science.Province_ID_Added;
                prov.Money = gen.Money;
                prov.Daily_Income = gen.Daily_Income;
                cachedKingdom.Provinces.Add(prov);
            }
            else
            {
                prov = Province.getProvince(science.Owner_Kingdom_ID, science.Province_ID, CS_Code.UtopiaDataContext.Get());
                if (prov != null)
                {
                    prov.SOS.Add(science);
                    cachedKingdom.Provinces.Add(prov);
                }
            }
            HttpRuntime.Cache.Add("KingdomCache" + science.Owner_Kingdom_ID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return prov;
        }
        public static ProvinceClass updateProvinceSurveyToCache(CS_Code.Utopia_Province_Data_Captured_Survey surv, OwnedKingdomProvinces cachedKingdom)
        {
            var prov = cachedKingdom.Provinces.Where(x => x.Province_ID == surv.Province_ID).FirstOrDefault();
            if (prov != null)
            {
                cachedKingdom.Provinces.Remove(prov);
                prov.Survey.Add(surv);
            }
            else
            {
                prov = Province.getProvince(surv.Owner_Kingdom_ID, surv.Province_ID, CS_Code.UtopiaDataContext.Get());
                prov.Survey.Add(surv);
            }
            cachedKingdom.Provinces.Add(prov);
            HttpRuntime.Cache.Add("KingdomCache" + surv.Owner_Kingdom_ID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return prov;
        }
        public static ProvinceClass updateProvinceSurveyToCache(long land, CS_Code.Utopia_Province_Data_Captured_Survey survey, CS_Code.Utopia_Province_Data_Captured_Gen gen, OwnedKingdomProvinces cachedKingdom)
        {
            var prov = cachedKingdom.Provinces.Where(x => x.Province_ID == survey.Province_ID).FirstOrDefault();
            if (prov != null)
            {
                cachedKingdom.Provinces.Remove(prov);
                prov.Survey.Add(survey);
                prov.Land = land;
                prov.Updated_By_DateTime = DateTime.UtcNow;
                prov.Updated_By_Province_ID = survey.Province_ID_Updated_By;
                prov.Survey_Requested = null;
                prov.Survey_Requested_Province_ID = null;
                prov.Ruler_Name = gen.Ruler_Name;
                prov.Race_ID = gen.Race_ID;
                cachedKingdom.Provinces.Add(prov);
            }
            else
            {
                prov = Province.getProvince(survey.Owner_Kingdom_ID, survey.Province_ID, CS_Code.UtopiaDataContext.Get());
                if (prov != null)
                {
                    prov.Survey.Add(survey);
                    cachedKingdom.Provinces.Add(prov);
                }
            }
            HttpRuntime.Cache.Add("KingdomCache" + survey.Owner_Kingdom_ID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return prov;
        }
        /// <summary>
        /// Gets the province ID, debating whether this is faster then just a regular Try Catch Statement.
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="provinceName"></param>
        /// <param name="island"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static Guid getProvinceID(string provinceName, int island, int location, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            if (cachedKingdom.Provinces != null && cachedKingdom.Provinces.Where(x => x.Kingdom_Island == island).Where(x => x.Kingdom_Location == location).Where(x => x.Province_Name == provinceName).FirstOrDefault() != null)
                return cachedKingdom.Provinces.Where(x => x.Kingdom_Island == island).Where(x => x.Kingdom_Location == location).Where(x => x.Province_Name == provinceName).FirstOrDefault().Province_ID;
            else
            {
                var startingKingdom = currentUser.PimpUser.StartingKingdom;

                return UData.Province.getProvinceCantFindInCache(startingKingdom, CS_Code.UtopiaDataContext.Get(), provinceName, island, location, currentUser, cachedKingdom).Province_ID;

            }
        }
        public static ProvinceClass getProvince(Guid ownerKingdomID, Guid provinceID, OwnedKingdomProvinces cachedKingdom)
        {
            if (cachedKingdom.Provinces != null && cachedKingdom.Provinces.Where(x => x.Province_ID == provinceID).FirstOrDefault() != null)
                return cachedKingdom.Provinces.Where(x => x.Province_ID == provinceID).FirstOrDefault();
            else
                return KingdomCache.removeProvinceFromKingdomCache(ownerKingdomID, provinceID, cachedKingdom).Provinces.Where(x => x.Province_ID == provinceID).FirstOrDefault();
        }
        /// <summary>
        /// Gets the random provinces in the cache.  If there aren't any, then it hits the DB for the last 7 days of randoms.
        /// Returns the provinces and adds the randoms to the kingdom cache.
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="db"></param>
        /// <param name="kdProvTimeLimit"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static List<ProvinceClass> getProvincesRandomThenAddToCache(Guid ownerKingdomID, CS_Code.UtopiaDataContext db, OwnedKingdomProvinces cachedKingdom)
        {
            //if the db has been checked for random provinces in the past 48 hours, just return the cache.
            if (cachedKingdom.RandomProvincesLastCheckedDb > DateTime.UtcNow.AddHours(-48))
            {
                var provsList = (from xx in cachedKingdom.Provinces
                                 where !(from yy in cachedKingdom.Kingdoms
                                         where yy.Retired == false
                                         select yy.Kingdom_ID).Contains((Guid)xx.Kingdom_ID)
                                 select xx).ToList();
                if (cachedKingdom.RandomProvinceCount != provsList.Count)
                {
                    cachedKingdom.RandomProvinceCount = provsList.Count;
                    HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
                }
                return provsList;
            }
            //gets the randoms in the cache to clear.
            var provs = (from xx in cachedKingdom.Provinces
                         where !(from yy in cachedKingdom.Kingdoms
                                 where yy.Retired == false
                                 select yy.Kingdom_ID).Contains((Guid)xx.Kingdom_ID)
                         select xx).ToList();
            //Boomers.Utilities.Documents.TextLogger.LogItem("utopiapimp", "RandomProvinceCountProvs " + provs.Count);
            //removes the provinces from cache
            for (int i = 0; i < provs.Count; i++)
            {
                cachedKingdom.Provinces.Remove(provs[i]);
            }
            //Boomers.Utilities.Documents.TextLogger.LogItem("utopiapimp", "RandomProvinceCountProvsCleared " + provs.Count);
            var list = (from xx in db.Utopia_Province_Data_Captured_Gens
                        where xx.Owner_Kingdom_ID == ownerKingdomID
                        where xx.Updated_By_DateTime.GetValueOrDefault(DateTime.UtcNow.AddDays(-7)) > DateTime.UtcNow.AddDays(cachedKingdom.KdProvTimeLimit)
                        where !(from yy in db.Utopia_Kingdom_Infos
                                where yy.Retired == false
                                select yy.Kingdom_ID).Contains((Guid)xx.Kingdom_ID)
                        select new ProvinceClass
                        {
                            Kingdom_ID = xx.Kingdom_ID,
                            Kingdom_Island = xx.Kingdom_Island,
                            Kingdom_Location = xx.Kingdom_Location,
                            Owner_Kingdom_ID = xx.Owner_Kingdom_ID,
                            Province_ID = xx.Province_ID,
                            Province_Name = xx.Province_Name,
                            Owner_User_ID = xx.Owner_User_ID,
                            Race_ID = xx.Race_ID,
                            Updated_By_DateTime = xx.Updated_By_DateTime,
                            Networth = xx.Networth,
                            Land = xx.Land,
                            Monarch_Display = xx.Monarch_Display,
                            Owner = xx.Owner,
                            Sub_Monarch = xx.Sub_Monarch,
                            CB_Updated_By_Province_ID = xx.CB_Updated_By_Province_ID,
                            uid = xx.uid,
                            Formatted_By = xx.Formatted_By,
                            Utopian_Day_Month = xx.Utopian_Day_Month,
                            Utopian_Year = xx.Utopian_Year,
                            Ruler_Name = xx.Ruler_Name,
                            Personality_ID = xx.Personality_ID,
                            Nobility_ID = xx.Nobility_ID,
                            Money = xx.Money,
                            Daily_Income = xx.Daily_Income,
                            Food = xx.Food,
                            Runes = xx.Runes,
                            Population = xx.Population,
                            Peasents = xx.Peasents,
                            Peasents_Non_Percentage = xx.Peasents_Non_Percentage,
                            Trade_Balance = xx.Trade_Balance,
                            Building_Effectiveness = xx.Building_Effectiveness,
                            Military_Efficiency_Off = xx.Military_Efficiency_Off,
                            Military_Efficiency_Def = xx.Military_Efficiency_Def,
                            Draft = xx.Draft,
                            Soldiers = xx.Soldiers,
                            Soldiers_Regs_Off = xx.Soldiers_Regs_Off,
                            Soldiers_Regs_Def = xx.Soldiers_Regs_Def,
                            Soldiers_Elites = xx.Soldiers_Elites,
                            War_Horses = xx.War_Horses,
                            //Prisoners = xx.Prisoners,
                            Military_Net_Off = xx.Military_Net_Off,
                            Military_Net_Def = xx.Military_Net_Def,
                            Military_Current_Off = xx.Military_Current_Off,
                            Military_Current_Def = xx.Military_Current_Def,
                            Mil_Training = xx.Mil_Training,
                            Mil_Wage = xx.Mil_Wage,
                            Mil_Overall_Efficiency = xx.Mil_Overall_Efficiency,
                            Mil_Total_Generals = xx.Mil_Total_Generals,
                            Wizards = xx.Wizards,
                            Wizards_Value_Type = xx.Wizards_Value_Type,
                            Thieves = xx.Thieves,
                            Thieves_Value_Type = xx.Thieves_Value_Type,
                            Plague = xx.Plague,
                            Monarch_Vote_Province_ID = xx.Monarch_Vote_Province_ID,
                            Protected = xx.Protected,
                            Hit = xx.Hit,
                            Honor = xx.Honor,
                            Province_Notes = xx.Province_Notes,
                            CB_Export_Line = xx.CB_Export_Line,
                            Army_Out = xx.Army_Out,
                            Army_Out_Expires = xx.Army_Out_Expires,
                            Updated_By_Province_ID = xx.Updated_By_Province_ID,
                            SOM_Updated_By_Province_ID = xx.SOM_Updated_By_Province_ID,
                            SOM_Updated_By_DateTime = xx.SOM_Updated_By_DateTime,
                            CB_Updated_By_DateTime = xx.CB_Updated_By_DateTime,
                            CB_Requested = xx.CB_Requested,
                            CB_Requested_Province_ID = xx.CB_Requested_Province_ID,
                            SOM_Requested = xx.SOM_Requested,
                            SOM_Requested_Province_ID = xx.SOM_Requested_Province_ID,
                            SOS_Requested = xx.SOS_Requested,
                            SOS_Requested_Province_ID = xx.SOS_Requested_Province_ID,
                            Survey_Requested = xx.Survey_Requested,
                            Survey_Requested_Province_ID = xx.Survey_Requested_Province_ID,
                            Last_Login_For_Province = xx.Last_Login_For_Province,
                            Date_Time_User_ID_Linked = xx.Date_Time_User_ID_Linked,
                            Added_By_User_ID = xx.Added_By_User_ID,
                            NoteCount = (from yy in db.Utopia_Province_Notes
                                         where yy.Province_ID == xx.Province_ID
                                         select yy).Count(),
                            SOM = (from uu in db.Utopia_Province_Data_Captured_Type_Militaries
                                   where uu.Province_ID == xx.Province_ID
                                   where uu.Owner_Kingdom_ID == ownerKingdomID
                                   where uu.DateTime_Added == (from ru in db.Utopia_Province_Data_Captured_Type_Militaries //datetime can be same for multiple items.
                                                               where ru.Province_ID == xx.Province_ID
                                                               where ru.Owner_Kingdom_ID == ownerKingdomID
                                                               orderby ru.uid descending
                                                               select ru.DateTime_Added).FirstOrDefault()
                                   select uu).ToList(),
                            SOS = (from ru in db.Utopia_Province_Data_Captured_Sciences
                                   where ru.Province_ID == xx.Province_ID
                                   where ru.Owner_Kingdom_ID == ownerKingdomID
                                   orderby ru.uid descending
                                   select ru).Take(1).ToList(),
                            Survey = (from ru in db.Utopia_Province_Data_Captured_Surveys
                                      where ru.Province_ID == xx.Province_ID
                                      where ru.Owner_Kingdom_ID == ownerKingdomID
                                      orderby ru.uid descending
                                      select ru).Take(1).ToList(),
                            CB = (from ru in db.Utopia_Province_Data_Captured_CBs
                                  where ru.Province_ID == xx.Province_ID
                                  where ru.Owner_Kingdom_ID == ownerKingdomID
                                  orderby ru.uid descending
                                  select ru).Take(1).ToList()
                        }).ToList();
            cachedKingdom.Provinces.AddRange(list);
            cachedKingdom.RandomProvincesLastCheckedDb = DateTime.UtcNow;
            cachedKingdom.RandomProvinceCount = list.Count;
            HttpRuntime.Cache.Add("KingdomCache" + ownerKingdomID.ToString(), cachedKingdom, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
            return list;
        }
    }
}
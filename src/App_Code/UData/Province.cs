using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Pimp.UCache;
using Pimp.Utopia;
using Pimp.Users;
using PimpLibrary.Utopia.Province;
using System.Threading.Tasks;
using Boomers.Utilities.DatesTimes;
using Pimp.UParser;
using PimpLibrary.Static.Enums;
using SupportFramework.Data;

namespace Pimp.UData
{


    /// <summary>
    /// Summary description for Province
    /// </summary>
    public class Province
    {
        /// <summary>
        /// changing this value to any more than 3 may have deadly consequences for the server.
        /// </summary>
        private static readonly int DAYS_TO_GET_RANDOM_PROVINCES = -3;

        public static void SubMonarchToggle(string provID, Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var getProv = (from xx in db.Utopia_Province_Data_Captured_Gens
                           where xx.Province_ID == new Guid(provID)
                           select xx).FirstOrDefault();

            if (getProv != null)
            {
                if (getProv.Sub_Monarch.GetValueOrDefault(0) == 1)
                    getProv.Sub_Monarch = null;
                else
                    getProv.Sub_Monarch = 1;
                db.SubmitChanges();
                KingdomCache.removeProvinceFromKingdomCache(ownerKingdomID, new Guid(provID), cachedKingdom);
            }
        }

        public static List<ProvinceClass> LoadTargetFinderSearch(int networthMax, int networthMin, int acresMax, int acresMin, int daysMin, int daysMax, string races, string honor, PimpUserWrapper currentUser)
        {

            List<ProvinceClass> ucSet = TargetFinder.Instance.TargetedProvinces;

            if (networthMax != 10000000)
                ucSet = ucSet.Where(x => x.Networth.Value <= networthMax).ToList();
            if (networthMin != 0)
                ucSet = ucSet.Where(x => x.Networth.Value >= networthMin).ToList();
            if (acresMax != 100000)
                ucSet = ucSet.Where(x => x.Land.Value <= acresMax).ToList();
            if (acresMin != 0)
                ucSet = ucSet.Where(x => x.Land.Value >= acresMin).ToList();
            if (daysMax != 100)
                ucSet = ucSet.Where(x => x.Updated_By_DateTime.Value >= DateTime.UtcNow.AddDays(-daysMax)).ToList();
            if (daysMin != 0)
                ucSet = ucSet.Where(x => x.Updated_By_DateTime.Value <= DateTime.UtcNow.AddDays(-daysMin)).ToList();

            string[] racesSplit = races.Split(',');
            string[] honorSplit = honor.Split(',');

            foreach (var item in racesSplit)
                if (item.Length > 2)
                    ucSet = ucSet.Where(x => x.Race_ID != UtopiaParser.getRaceID(item.Remove(0, 2).Replace("1", ""), currentUser.PimpUser.UserID)).ToList();

            foreach (var item in honorSplit)
                if (item.Length > 2)
                {
                    if (URegEx._findNobility.Matches(item).Count == 1)
                        ucSet = ucSet.Where(x => x.Honor != UtopiaHelper.Instance.Ranks.Where(y => y.name == URegEx._findNobility.Match(item).Value).Select(y => y.uid).FirstOrDefault()).ToList();
                    else if (URegEx._findNobility.Matches(item).Count > 1)
                        ucSet = ucSet.Where(x => x.Honor != UtopiaHelper.Instance.Ranks.Where(y => y.name == URegEx._findNobility.Matches(item)[0].Value).Select(y => y.uid).FirstOrDefault()).Where(x => x.Honor != UtopiaHelper.Instance.Ranks.Where(y => y.name == URegEx._findNobility.Matches(item)[1].Value).Select(y => y.uid).FirstOrDefault()).ToList();
                }
            return ucSet;
        }

        public static List<ProvinceClass> LoadFilterStandardProvinces(int networthMax, int networthMin, int acresMax, int acresMin, int daysMin, int daysMax, Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            List<ProvinceClass> ucSet = ProvinceCache.getProvincesRandomThenAddToCache(ownerKingdomID, CS_Code.UtopiaDataContext.Get(), cachedKingdom);

            if (networthMax != 10000000)
                ucSet = ucSet.Where(x => x.Networth.GetValueOrDefault(0)<= networthMax).ToList();
            if (networthMin != 0)
                ucSet = ucSet.Where(x => x.Networth.GetValueOrDefault(0) >= networthMin).ToList();
            if (acresMax != 100000)
                ucSet = ucSet.Where(x => x.Land.GetValueOrDefault(0) <= acresMax).ToList();
            if (acresMin != 0)
                ucSet = ucSet.Where(x => x.Land.GetValueOrDefault(0) >= acresMin).ToList();
            if (daysMax != 100)
                ucSet = ucSet.Where(x => x.Updated_By_DateTime.Value >= DateTime.UtcNow.AddDays(-daysMax)).ToList();
            if (daysMin != 0)
                ucSet = ucSet.Where(x => x.Updated_By_DateTime.Value <= DateTime.UtcNow.AddDays(-daysMin)).ToList();

            HttpContext.Current.Session["FilteredProvinces"] = ucSet;
            HttpContext.Current.Session["GetKingdomData"] = "RandomFilter";
            return UtopiaParser.GetProvincesInKingdomToDisplay("RandomFilter", new Guid(), ownerKingdomID, cachedKingdom);
        }

        /// <summary>
        /// Deletes te province from Utopia....
        /// But it doesn't delete the Province attached with the owner part.
        /// </summary>
        /// <param name="provID"></param>
        /// <returns></returns>
        public static string DeleteProvince(Guid provID, Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var query1 = (from xx in db.Utopia_Province_Data_Captured_Attacks
                          where xx.Owner_Kingdom_ID == ownerKingdomID
                          where xx.Province_ID_Attacked == provID
                          select xx);
            db.Utopia_Province_Data_Captured_Attacks.DeleteAllOnSubmit(query1);

            var query2 = (from xx in db.Utopia_Province_Data_Captured_Gens
                          where xx.Province_ID == provID
                          select xx);
            db.Utopia_Province_Data_Captured_Gens.DeleteAllOnSubmit(query2);

            var query3 = (from xx in db.Utopia_Province_Data_Captured_Sciences
                          where xx.Owner_Kingdom_ID == ownerKingdomID
                          where xx.Province_ID == provID
                          select xx);
            db.Utopia_Province_Data_Captured_Sciences.DeleteAllOnSubmit(query3);

            var query4 = (from xx in db.Utopia_Province_Data_Captured_Surveys
                          where xx.Owner_Kingdom_ID == ownerKingdomID
                          where xx.Province_ID == provID
                          select xx);
            db.Utopia_Province_Data_Captured_Surveys.DeleteAllOnSubmit(query4);

            var query5 = (from xx in db.Utopia_Province_Data_Captured_Type_Militaries
                          where xx.Owner_Kingdom_ID == ownerKingdomID
                          where xx.Province_ID == provID
                          select xx);
            db.Utopia_Province_Data_Captured_Type_Militaries.DeleteAllOnSubmit(query5);

            var query6 = (from xx in db.Utopia_Province_Data_Captured_Gens
                          where xx.Owner_Kingdom_ID == ownerKingdomID
                          where xx.Province_ID == provID
                          select xx);
            db.Utopia_Province_Data_Captured_Gens.DeleteAllOnSubmit(query6);

            var query7 = (from xx in db.Utopia_Province_Notes
                          where xx.Owner_Kingdom_ID == ownerKingdomID
                          where xx.Province_ID == provID
                          select xx);
            db.Utopia_Province_Notes.DeleteAllOnSubmit(query7);

            var query8 = (from xx in db.Utopia_Province_Ops
                          where xx.Owner_Kingdom_ID == ownerKingdomID
                          where xx.Directed_To_Province_ID == provID
                          select xx);
            db.Utopia_Province_Ops.DeleteAllOnSubmit(query8);
            db.SubmitChanges();

            KingdomCache.removeProvinceFromKingdomCache(ownerKingdomID, provID, cachedKingdom);
            return "Success";
        }
        /// <summary>
        /// Inserts a new random province from a name and Ownerinfo from Session.
        /// </summary>
        /// <param name="ProvinceName">Name of New Province</param>
        /// <param name="db"></param>
        /// <param name="Island"></param>
        /// <param name="Location"></param>
        public static ProvinceClass InsertNewRandomProvince(string ProvinceName, int Island, int Location, Guid ownerKingdomID, Guid userID, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Province_Data_Captured_Gen UPDCG = new CS_Code.Utopia_Province_Data_Captured_Gen();
            UPDCG.Added_By_User_ID = userID;
            UPDCG.Kingdom_ID = new Guid();
            UPDCG.Owner_Kingdom_ID = ownerKingdomID;
            UPDCG.Province_ID = Guid.NewGuid();
            UPDCG.Province_Name = ProvinceName;
            UPDCG.Kingdom_Island = Island;
            UPDCG.Kingdom_Location = Location;
            UPDCG.Updated_By_DateTime = DateTime.UtcNow;
            db.Utopia_Province_Data_Captured_Gens.InsertOnSubmit(UPDCG);
            db.SubmitChanges();
            return KingdomCache.removeProvinceFromKingdomCache(ownerKingdomID, UPDCG.Province_ID, cachedKingdom).Provinces.Where(x => x.Province_ID == UPDCG.Province_ID).FirstOrDefault();
        }
        /// <summary>
        /// Deletes the requested intel...
        /// </summary>
        /// <param name="provID"></param>
        /// <param name="type"></param>
        public static bool DeleteRequestedIntelKDPage(Guid provID, string type, Guid ownerKingdomID, PimpUserWrapper currentUser)
        {
            DeleteRequestedIntelKdPageDb(provID, type, currentUser);

            var cachedKingdom = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom);
            var province = cachedKingdom.Provinces.Where(x => x.Province_ID == provID).FirstOrDefault();
            if (province != null)
            {
                switch (type)
                {
                    case "cb":
                        if (province.CB_Requested_Province_ID == currentUser.PimpUser.CurrentActiveProvince | (currentUser.PimpUser.MonarchType != MonarchType.none && currentUser.PimpUser.MonarchType != MonarchType.kdMonarch))
                        {
                            province.CB_Requested = null;
                            province.CB_Requested_Province_ID = null;
                            ProvinceCache.updateProvinceToCache(province, cachedKingdom);
                        }
                        return false;
                    case "som":
                        if (province.SOM_Requested_Province_ID == currentUser.PimpUser.CurrentActiveProvince | (currentUser.PimpUser.MonarchType != MonarchType.none && currentUser.PimpUser.MonarchType != MonarchType.kdMonarch))
                        {
                            province.SOM_Requested = null;
                            province.SOM_Requested_Province_ID = null;
                            ProvinceCache.updateProvinceToCache(province, cachedKingdom);
                            return true;
                        }
                        return false;
                    case "survey":
                        if (province.Survey_Requested_Province_ID == currentUser.PimpUser.CurrentActiveProvince | (currentUser.PimpUser.MonarchType != MonarchType.none && currentUser.PimpUser.MonarchType != MonarchType.kdMonarch))
                        {
                            province.Survey_Requested = null;
                            province.Survey_Requested_Province_ID = null;
                            ProvinceCache.updateProvinceToCache(province, cachedKingdom);
                            return true;
                        }
                        return false;
                    case "sos":
                        if (province.SOS_Requested_Province_ID == currentUser.PimpUser.CurrentActiveProvince | (currentUser.PimpUser.MonarchType != MonarchType.none && currentUser.PimpUser.MonarchType != MonarchType.kdMonarch))
                        {
                            province.SOS_Requested = null;
                            province.SOS_Requested_Province_ID = null;
                            ProvinceCache.updateProvinceToCache(province, cachedKingdom);
                            return true;
                        }
                        return false;
                }
            }
            return false;
        }
        /// <summary>
        /// Deletes the requested intel from the DB.
        /// </summary>
        /// <param name="provID"></param>
        /// <param name="type"></param>
        /// <param name="currentUser"></param>
        private static void DeleteRequestedIntelKdPageDb(Guid provID, string type, PimpUserWrapper currentUser)
        {

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var getProv = (from xx in db.Utopia_Province_Data_Captured_Gens
                           where xx.Province_ID == provID
                           select xx).FirstOrDefault();

            if (getProv != null)
            {
                switch (type)
                {
                    case "cb":
                        if (getProv.CB_Requested_Province_ID == currentUser.PimpUser.CurrentActiveProvince | (currentUser.PimpUser.MonarchType != MonarchType.none && currentUser.PimpUser.MonarchType != MonarchType.kdMonarch))
                        {
                            getProv.CB_Requested = null;
                            getProv.CB_Requested_Province_ID = null;
                            db.SubmitChanges();
                        }
                        break;
                    case "som":
                        if (getProv.SOM_Requested_Province_ID == currentUser.PimpUser.CurrentActiveProvince | (currentUser.PimpUser.MonarchType != MonarchType.none && currentUser.PimpUser.MonarchType != MonarchType.kdMonarch))
                        {
                            getProv.SOM_Requested = null;
                            getProv.SOM_Requested_Province_ID = null;
                            db.SubmitChanges();
                        }
                        break;

                    case "survey":
                        if (getProv.Survey_Requested_Province_ID == currentUser.PimpUser.CurrentActiveProvince | (currentUser.PimpUser.MonarchType != MonarchType.none && currentUser.PimpUser.MonarchType != MonarchType.kdMonarch))
                        {
                            getProv.Survey_Requested = null;
                            getProv.Survey_Requested_Province_ID = null;
                            db.SubmitChanges();
                        }
                        break;
                    case "sos":
                        if (getProv.SOS_Requested_Province_ID == currentUser.PimpUser.CurrentActiveProvince | (currentUser.PimpUser.MonarchType != MonarchType.none && currentUser.PimpUser.MonarchType != MonarchType.kdMonarch))
                        {
                            getProv.SOS_Requested = null;
                            getProv.SOS_Requested_Province_ID = null;
                            db.SubmitChanges();
                        }
                        break;
                }
            }

        }

        /// <summary>
        /// Request intel on the province
        /// </summary>
        /// <param name="provID">province requesting for</param>
        /// <param name="type">cb, som, sos, survey</param>
        /// <returns></returns>
        public static void RequestIntelKDPage(Guid provID, string type, PimpUserWrapper currentUser)
        {

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var getProv = (from xx in db.Utopia_Province_Data_Captured_Gens
                           where xx.Province_ID == provID
                           select xx).FirstOrDefault();
            if (getProv != null)
            {
                switch (type)
                {
                    case "cb":
                        getProv.CB_Requested = DateTime.UtcNow;
                        getProv.CB_Requested_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
                        break;
                    case "som":
                        getProv.SOM_Requested = DateTime.UtcNow;
                        getProv.SOM_Requested_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
                        break;
                    case "survey":
                        getProv.Survey_Requested = DateTime.UtcNow;
                        getProv.Survey_Requested_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
                        break;
                    case "sos":
                        getProv.SOS_Requested = DateTime.UtcNow;
                        getProv.SOS_Requested_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
                        break;
                }
                db.SubmitChanges();

                var cachedKingdom = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom);
                var province = ProvinceCache.getProvince(currentUser.PimpUser.StartingKingdom, provID, cachedKingdom);
                if (province != null)
                {
                    switch (type)
                    {
                        case "cb":
                            province.CB_Requested = DateTime.UtcNow;
                            province.CB_Requested_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
                            break;
                        case "som":
                            province.SOM_Requested = DateTime.UtcNow;
                            province.SOM_Requested_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
                            break;
                        case "survey":
                            province.Survey_Requested = DateTime.UtcNow;
                            province.Survey_Requested_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
                            break;
                        case "sos":
                            province.SOS_Requested = DateTime.UtcNow;
                            province.SOS_Requested_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
                            break;
                    }
                    ProvinceCache.removeProvinceFromCache(province, cachedKingdom);
                }
            }
        }

        /// <summary>
        /// Adds a note to each province.
        /// </summary>
        /// <param name="provID"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public static string AddNote(Guid provID, string note, Guid ownerKingdomID, Guid provinceId)
        {

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Province_Note upn = new CS_Code.Utopia_Province_Note();
            upn.Added_By_DataTime = DateTime.UtcNow;
            upn.Added_By_Province_ID = provinceId;
            upn.Note = note;
            upn.Owner_Kingdom_ID = ownerKingdomID;
            upn.Province_ID = provID;
            db.Utopia_Province_Notes.InsertOnSubmit(upn);
            db.SubmitChanges();

            return "Success";
        }
        /// <summary>
        /// Shows the delete button for the notes.
        /// </summary>
        /// <param name="provID"></param>
        /// <returns></returns>
        public static string ProvinceNotesGetDelete(Guid provID, Guid ownerKingdomID)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var query = (from xx in db.Utopia_Province_Notes
                         from yy in db.Utopia_Province_Data_Captured_Gens
                         where xx.Owner_Kingdom_ID == ownerKingdomID
                         where xx.Added_By_Province_ID == yy.Province_ID
                         where xx.Province_ID == provID
                         orderby xx.Added_By_DataTime descending
                         select new
                         {
                             xx.Note,
                             yy.Province_Name,
                             xx.Added_By_DataTime,
                             xx.uid
                         }).ToList();
            string item = string.Empty;
            for (int i = 0; i < query.Count; i++)
                item += "By: " + query[i].Province_Name + " " + query[i].Added_By_DataTime.ToLongRelativeDate() + " <br />" + query[i].Note + " <span class=\"deleteButton\" onclick=\"DeleteProvNote('" + query[i].uid + "')\">Delete</span><hr />";
            return item;
        }
        /// <summary>
        /// Deletes the province Notes.
        /// </summary>
        /// <param name="noteID">ID of note.</param>
        /// <returns></returns>
        public static string ProvinceNoteDelete(int noteID, Guid ownerKingdomID)
        {
            //TODO: add notes to cache?
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var query = (from xx in db.Utopia_Province_Notes
                         where xx.Owner_Kingdom_ID == ownerKingdomID
                         where xx.uid == noteID
                         select xx).FirstOrDefault();
            db.Utopia_Province_Notes.DeleteOnSubmit(query);
            db.SubmitChanges();
            return ProvinceNotesGetDelete(query.Province_ID, ownerKingdomID);
        }
        /// <summary>
        /// Gets the notes entered for each province.
        /// </summary>
        /// <param name="provID"></param>
        /// <returns></returns>
        public static string getProvinceNotes(Guid provID, Guid ownerKingdomID, PimpUserWrapper currentUser)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            List<Note> provinceNotes;
            if (!currentUser.PimpUser.IsUserAdmin)
            {
                provinceNotes = (from xx in db.Utopia_Province_Notes
                                 from yy in db.Utopia_Province_Data_Captured_Gens
                                 where xx.Owner_Kingdom_ID == ownerKingdomID
                                 where xx.Added_By_Province_ID == yy.Province_ID
                                 where xx.Province_ID == provID
                                 orderby xx.Added_By_DataTime descending
                                 select new Note
                                 {
                                     NoteString = xx.Note,
                                     Province_Name = yy.Province_Name,
                                     DateTime_Added = xx.Added_By_DataTime,
                                 }).ToList();
            }
            else
            {
                provinceNotes = (from xx in db.Utopia_Province_Notes
                                 from yy in db.Utopia_Province_Data_Captured_Gens
                                 where xx.Added_By_Province_ID == yy.Province_ID
                                 where xx.Province_ID == provID
                                 orderby xx.Added_By_DataTime descending
                                 select new Note
                                 {
                                     NoteString = xx.Note,
                                     Province_Name = yy.Province_Name,
                                     DateTime_Added = xx.Added_By_DataTime,
                                 }).ToList();
            }
            string item = string.Empty;
            for (int i = 0; i < provinceNotes.Count; i++)
                item += "By: " + provinceNotes[i].Province_Name + " " + provinceNotes[i].DateTime_Added.ToLongRelativeDate() + " <br />" + provinceNotes[i].NoteString + "<hr />";
            item += "To add/delete Note: Click on Notepad.";
            return item;
        }

        /// <summary>
        /// gets the province spcified
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="provinceID"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static ProvinceClass getProvince(Guid ownerKingdomID, Guid provinceID, CS_Code.UtopiaDataContext db)
        {
            var list = (from xx in db.Utopia_Province_Data_Captured_Gens
                        where xx.Owner_Kingdom_ID == ownerKingdomID
                        where xx.Province_ID == provinceID
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
                        }).FirstOrDefault();

            if (list != null)
            {

                var SOM = (from uu in db.Utopia_Province_Data_Captured_Type_Militaries
                           where list.Province_ID == uu.Province_ID
                           where uu.Owner_Kingdom_ID == ownerKingdomID
                           where uu.DateTime_Added == (from ru in db.Utopia_Province_Data_Captured_Type_Militaries //datetime can be same for multiple items.
                                                       where ru.Province_ID == uu.Province_ID
                                                       where ru.Owner_Kingdom_ID == ownerKingdomID
                                                       orderby ru.uid descending // To get the last most inserted rows
                                                       select ru.DateTime_Added).FirstOrDefault()
                           select uu).ToList();

                var SOS = (from ru in db.Utopia_Province_Data_Captured_Sciences
                           where list.Province_ID == ru.Province_ID
                           where ru.Owner_Kingdom_ID == ownerKingdomID
                           orderby ru.uid descending
                           group ru by ru.Province_ID into grp
                           let maxGrp = grp.Max(x => x.uid)
                           from ru in grp
                           where ru.uid == maxGrp
                           select ru).ToList();
                var Surveyq = (from ru in db.Utopia_Province_Data_Captured_Surveys
                               where list.Province_ID == ru.Province_ID
                               where ru.Owner_Kingdom_ID == ownerKingdomID
                               orderby ru.uid descending
                               group ru by ru.Province_ID into grp
                               let maxGrp = grp.Max(x => x.uid)
                               from ru in grp
                               where ru.uid == maxGrp
                               select ru).ToList();
                var CBq = (from ru in db.Utopia_Province_Data_Captured_CBs
                           where list.Province_ID == ru.Province_ID
                           group ru by ru.Province_ID into grp
                           let maxGrp = grp.Max(x => x.uid)
                           from ru in grp
                           where ru.uid == maxGrp
                           select ru).ToList();


                list.SOM = SOM;
                list.CB = CBq;
                list.SOS = SOS;
                list.Survey = Surveyq;
            }
            return list;
        }
        /// <summary>
        /// gets the provinces of random for the first time
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="db"></param>
        /// <param name="kdProvTimeLimit"></param>
        /// <returns></returns>
        public static List<ProvinceClass> getProvincesRandomFirstTime(Guid ownerKingdomID, CS_Code.UtopiaDataContext db, int kdProvTimeLimit)
        {
            try
            {
                HttpContext.Current.Session["SubmittedData"] += "kdProvLimit:" + kdProvTimeLimit;
                if (kdProvTimeLimit < -15)
                    kdProvTimeLimit = -10;
                HttpContext.Current.Session["SubmittedData"] += ":kdProvLimit:" + kdProvTimeLimit;
                var list = (from xx in db.Utopia_Province_Data_Captured_Gens
                            where xx.Owner_Kingdom_ID == ownerKingdomID
                            where xx.Updated_By_DateTime.GetValueOrDefault(DateTime.UtcNow.AddDays(DAYS_TO_GET_RANDOM_PROVINCES)) > DateTime.UtcNow.AddDays(kdProvTimeLimit)
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
                            }).ToList();

                var SOM = (from uu in db.Utopia_Province_Data_Captured_Type_Militaries
                           where list.Select(x => x.Province_ID).Contains(uu.Province_ID)
                           where uu.Owner_Kingdom_ID == ownerKingdomID
                           where uu.DateTime_Added == (from ru in db.Utopia_Province_Data_Captured_Type_Militaries //datetime can be same for multiple items.
                                                       where ru.Province_ID == uu.Province_ID
                                                       where ru.Owner_Kingdom_ID == ownerKingdomID
                                                       orderby ru.uid descending // To get the last most inserted rows
                                                       select ru.DateTime_Added).FirstOrDefault()
                           select uu).ToList();

                var SOS = (from ru in db.Utopia_Province_Data_Captured_Sciences
                           where list.Select(x => x.Province_ID).Contains(ru.Province_ID)
                           where ru.Owner_Kingdom_ID == ownerKingdomID
                           orderby ru.uid descending
                           group ru by ru.Province_ID into grp
                           let maxGrp = grp.Max(x => x.uid)
                           from ru in grp
                           where ru.uid == maxGrp
                           select ru).ToList();
                var Surveyq = (from ru in db.Utopia_Province_Data_Captured_Surveys
                               where list.Select(x => x.Province_ID).Contains(ru.Province_ID)
                               where ru.Owner_Kingdom_ID == ownerKingdomID
                               orderby ru.uid descending
                               group ru by ru.Province_ID into grp
                               let maxGrp = grp.Max(x => x.uid)
                               from ru in grp
                               where ru.uid == maxGrp
                               select ru).ToList();
                var CBq = (from ru in db.Utopia_Province_Data_Captured_CBs
                           where list.Select(x => x.Province_ID).Contains(ru.Province_ID)
                           group ru by ru.Province_ID into grp
                           let maxGrp = grp.Max(x => x.uid)
                           from ru in grp
                           where ru.uid == maxGrp
                           select ru).ToList();


                for (int i = 0; i < list.Count; i++)
                {
                    var soms = SOM.Where(x => x.Province_ID == list[i].Province_ID).ToList();
                    list[i].SOM = soms;
                    var cbs = CBq.Where(x => x.Province_ID == list[i].Province_ID).ToList();
                    list[i].CB = cbs;
                    var soss = SOS.Where(x => x.Province_ID == list[i].Province_ID).ToList();
                    list[i].SOS = soss;
                    var surveys = Surveyq.Where(x => x.Province_ID == list[i].Province_ID).ToList();
                    list[i].Survey = surveys;
                }
                return list;
            }
            catch (Exception e)
            {
                Errors.logError(e);
            }
            return new List<ProvinceClass>();
        }

        /// <summary>
        /// gets all the random privinces for the past 3 days
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="db"></param>
        /// <param name="kingdomList"></param>
        /// <returns></returns>
        public static List<ProvinceClass> getProvincesRandom(Guid ownerKingdomID, CS_Code.UtopiaDataContext db, List<Guid> kingdomList)
        {
            var list = (from xx in db.Utopia_Province_Data_Captured_Gens
                        where xx.Owner_Kingdom_ID == ownerKingdomID
                        where !kingdomList.Contains((Guid)xx.Kingdom_ID)
                        where xx.Updated_By_DateTime > DateTime.UtcNow.AddDays(DAYS_TO_GET_RANDOM_PROVINCES)
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
                        }).ToList();

            var SOM = (from uu in db.Utopia_Province_Data_Captured_Type_Militaries
                       where list.Select(x => x.Province_ID).Contains(uu.Province_ID)
                       where uu.Owner_Kingdom_ID == ownerKingdomID
                       where uu.DateTime_Added == (from ru in db.Utopia_Province_Data_Captured_Type_Militaries //datetime can be same for multiple items.
                                                   where ru.Province_ID == uu.Province_ID
                                                   where ru.Owner_Kingdom_ID == ownerKingdomID
                                                   orderby ru.uid descending // To get the last most inserted rows
                                                   select ru.DateTime_Added).FirstOrDefault()
                       select uu).ToList();

            var SOS = (from ru in db.Utopia_Province_Data_Captured_Sciences
                       where list.Select(x => x.Province_ID).Contains(ru.Province_ID)
                       where ru.Owner_Kingdom_ID == ownerKingdomID
                       orderby ru.uid descending
                       group ru by ru.Province_ID into grp
                       let maxGrp = grp.Max(x => x.uid)
                       from ru in grp
                       where ru.uid == maxGrp
                       select ru).ToList();
            var Surveyq = (from ru in db.Utopia_Province_Data_Captured_Surveys
                           where list.Select(x => x.Province_ID).Contains(ru.Province_ID)
                           where ru.Owner_Kingdom_ID == ownerKingdomID
                           orderby ru.uid descending
                           group ru by ru.Province_ID into grp
                           let maxGrp = grp.Max(x => x.uid)
                           from ru in grp
                           where ru.uid == maxGrp
                           select ru).ToList();
            var CBq = (from ru in db.Utopia_Province_Data_Captured_CBs
                       where list.Select(x => x.Province_ID).Contains(ru.Province_ID)
                       group ru by ru.Province_ID into grp
                       let maxGrp = grp.Max(x => x.uid)
                       from ru in grp
                       where ru.uid == maxGrp
                       select ru).ToList();


            for (int i = 0; i < list.Count; i++)
            {
                var soms = SOM.Where(x => x.Province_ID == list[i].Province_ID).ToList();
                list[i].SOM = soms;
                var cbs = CBq.Where(x => x.Province_ID == list[i].Province_ID).ToList();
                list[i].CB = cbs;
                var soss = SOS.Where(x => x.Province_ID == list[i].Province_ID).ToList();
                list[i].SOS = soss;
                var surveys = Surveyq.Where(x => x.Province_ID == list[i].Province_ID).ToList();
                list[i].Survey = surveys;
            }
            return list;
        }
        /// <summary>
        /// gets the provinces from the list of kingdoms
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="db"></param>
        /// <param name="kingdomList"></param>
        /// <returns></returns>
        public static List<ProvinceClass> getProvinces(Guid ownerKingdomID, CS_Code.UtopiaDataContext db, List<Guid> kingdomList)
        {

            var list = (from xx in db.Utopia_Province_Data_Captured_Gens
                        where xx.Owner_Kingdom_ID == ownerKingdomID
                        where kingdomList.Contains((Guid)xx.Kingdom_ID)
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
                        }).ToList();

            var SOM = (from uu in db.Utopia_Province_Data_Captured_Type_Militaries
                       where list.Select(x => x.Province_ID).Contains(uu.Province_ID)
                       where uu.Owner_Kingdom_ID == ownerKingdomID
                       where uu.DateTime_Added == (from ru in db.Utopia_Province_Data_Captured_Type_Militaries //datetime can be same for multiple items.
                                                   where ru.Province_ID == uu.Province_ID
                                                   where ru.Owner_Kingdom_ID == ownerKingdomID
                                                   orderby ru.uid descending // To get the last most inserted rows
                                                   select ru.DateTime_Added).FirstOrDefault()
                       select uu).ToList();

            var SOS = (from ru in db.Utopia_Province_Data_Captured_Sciences
                       where list.Select(x => x.Province_ID).Contains(ru.Province_ID)
                       where ru.Owner_Kingdom_ID == ownerKingdomID
                       orderby ru.uid descending
                       group ru by ru.Province_ID into grp
                       let maxGrp = grp.Max(x => x.uid)
                       from ru in grp
                       where ru.uid == maxGrp
                       select ru).ToList();
            var Surveyq = (from ru in db.Utopia_Province_Data_Captured_Surveys
                           where list.Select(x => x.Province_ID).Contains(ru.Province_ID)
                           where ru.Owner_Kingdom_ID == ownerKingdomID
                           orderby ru.uid descending
                           group ru by ru.Province_ID into grp
                           let maxGrp = grp.Max(x => x.uid)
                           from ru in grp
                           where ru.uid == maxGrp
                           select ru).ToList();
            var CBq = (from ru in db.Utopia_Province_Data_Captured_CBs
                       where list.Select(x => x.Province_ID).Contains(ru.Province_ID)
                       group ru by ru.Province_ID into grp
                       let maxGrp = grp.Max(x => x.uid)
                       from ru in grp
                       where ru.uid == maxGrp
                       select ru).ToList();


            for (int i = 0; i < list.Count; i++)
            {
                var soms = SOM.Where(x => x.Province_ID == list[i].Province_ID).ToList();
                list[i].SOM = soms;
                var cbs = CBq.Where(x => x.Province_ID == list[i].Province_ID).ToList();
                list[i].CB = cbs;
                var soss = SOS.Where(x => x.Province_ID == list[i].Province_ID).ToList();
                list[i].SOS = soss;
                var surveys = Surveyq.Where(x => x.Province_ID == list[i].Province_ID).ToList();
                list[i].Survey = surveys;
            }

            return list;
        }



        /// <summary>
        /// gets the provinces for the kingdom
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="provinceIDs"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static List<ProvinceClass> getProvinces(Guid ownerKingdomID, List<Guid> provinceIDs, CS_Code.UtopiaDataContext db)
        {
            var list = (from xx in db.Utopia_Province_Data_Captured_Gens
                        where xx.Owner_Kingdom_ID == ownerKingdomID
                        where provinceIDs.Contains(xx.Province_ID)
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
                        }).ToList();

            var SOM = (from uu in db.Utopia_Province_Data_Captured_Type_Militaries
                       where list.Select(x => x.Province_ID).Contains(uu.Province_ID)
                       where uu.Owner_Kingdom_ID == ownerKingdomID
                       where uu.DateTime_Added == (from ru in db.Utopia_Province_Data_Captured_Type_Militaries //datetime can be same for multiple items.
                                                   where ru.Province_ID == uu.Province_ID
                                                   where ru.Owner_Kingdom_ID == ownerKingdomID
                                                   orderby ru.uid descending // To get the last most inserted rows
                                                   select ru.DateTime_Added).FirstOrDefault()
                       select uu).ToList();

            var SOS = (from ru in db.Utopia_Province_Data_Captured_Sciences
                       where list.Select(x => x.Province_ID).Contains(ru.Province_ID)
                       where ru.Owner_Kingdom_ID == ownerKingdomID
                       orderby ru.uid descending
                       group ru by ru.Province_ID into grp
                       let maxGrp = grp.Max(x => x.uid)
                       from ru in grp
                       where ru.uid == maxGrp
                       select ru).ToList();
            var Surveyq = (from ru in db.Utopia_Province_Data_Captured_Surveys
                           where list.Select(x => x.Province_ID).Contains(ru.Province_ID)
                           where ru.Owner_Kingdom_ID == ownerKingdomID
                           orderby ru.uid descending
                           group ru by ru.Province_ID into grp
                           let maxGrp = grp.Max(x => x.uid)
                           from ru in grp
                           where ru.uid == maxGrp
                           select ru).ToList();
            var CBq = (from ru in db.Utopia_Province_Data_Captured_CBs
                       where list.Select(x => x.Province_ID).Contains(ru.Province_ID)
                       group ru by ru.Province_ID into grp
                       let maxGrp = grp.Max(x => x.uid)
                       from ru in grp
                       where ru.uid == maxGrp
                       select ru).ToList();


            for (int i = 0; i < list.Count; i++)
            {
                var soms = SOM.Where(x => x.Province_ID == list[i].Province_ID).ToList();
                list[i].SOM = soms;
                var cbs = CBq.Where(x => x.Province_ID == list[i].Province_ID).ToList();
                list[i].CB = cbs;
                var soss = SOS.Where(x => x.Province_ID == list[i].Province_ID).ToList();
                list[i].SOS = soss;
                var surveys = Surveyq.Where(x => x.Province_ID == list[i].Province_ID).ToList();
                list[i].Survey = surveys;
            }
            return list;
        }
        /// <summary>
        /// gets the som of a province not cached
        /// </summary>
        /// <param name="provinceID"></param>
        /// <param name="ownerKingdomID"></param>
        /// <returns></returns>
        public static List<CS_Code.Utopia_Province_Data_Captured_Type_Military> GetProvinceSOMNotCached(Guid provinceID, Guid ownerKingdomID)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            return (from uu in db.Utopia_Province_Data_Captured_Type_Militaries
                    where uu.Province_ID == provinceID
                    where uu.Owner_Kingdom_ID == ownerKingdomID
                    where uu.DateTime_Added == (from u in db.Utopia_Province_Data_Captured_Type_Militaries //datetime can be same for multiple items.
                                                where u.Province_ID == provinceID
                                                where u.Owner_Kingdom_ID == ownerKingdomID
                                                orderby u.uid descending  // To get the last most inserted rows
                                                select u.DateTime_Added).FirstOrDefault()
                    select uu).ToList();
        }
        /// <summary>
        /// gets the cb of a province not cached
        /// </summary>
        /// <param name="provinceID"></param>
        /// <param name="ownerKingdomID"></param>
        /// <returns></returns>
        public static CS_Code.Utopia_Province_Data_Captured_CB GetProvinceCBNotCached(Guid provinceID, Guid ownerKingdomID)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            return (from UPI in db.Utopia_Province_Data_Captured_CBs
                    where UPI.Province_ID == provinceID
                    where UPI.Owner_Kingdom_ID == ownerKingdomID
                    select UPI).FirstOrDefault();
        }

        /// <summary>
        /// gets the province that cant find in cache
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="db"></param>
        /// <param name="provinceName"></param>
        /// <param name="island"></param>
        /// <param name="location"></param>
        /// <param name="currentUser"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static ProvinceClass getProvinceCantFindInCache(Guid ownerKingdomID, CS_Code.UtopiaDataContext db, string provinceName, int island, int location, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            var list = (from xx in db.Utopia_Province_Data_Captured_Gens
                        where xx.Owner_Kingdom_ID == ownerKingdomID
                        where xx.Province_Name == provinceName
                        where xx.Kingdom_Island == island
                        where xx.Kingdom_Location == location
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
                        }).FirstOrDefault();
            if (list == null)
                return Province.InsertNewRandomProvince(provinceName, island, location, ownerKingdomID, currentUser.PimpUser.UserID, cachedKingdom);


            var SOM = (from uu in db.Utopia_Province_Data_Captured_Type_Militaries
                       where list.Province_ID == uu.Province_ID
                       where uu.Owner_Kingdom_ID == ownerKingdomID
                       where uu.DateTime_Added == (from ru in db.Utopia_Province_Data_Captured_Type_Militaries //datetime can be same for multiple items.
                                                   where ru.Province_ID == uu.Province_ID
                                                   where ru.Owner_Kingdom_ID == ownerKingdomID
                                                   orderby ru.uid descending // To get the last most inserted rows
                                                   select ru.DateTime_Added).FirstOrDefault()
                       select uu).ToList();

            var SOS = (from ru in db.Utopia_Province_Data_Captured_Sciences
                       where list.Province_ID == ru.Province_ID
                       where ru.Owner_Kingdom_ID == ownerKingdomID
                       orderby ru.uid descending
                       group ru by ru.Province_ID into grp
                       let maxGrp = grp.Max(x => x.uid)
                       from ru in grp
                       where ru.uid == maxGrp
                       select ru).ToList();
            var Surveyq = (from ru in db.Utopia_Province_Data_Captured_Surveys
                           where list.Province_ID == ru.Province_ID
                           where ru.Owner_Kingdom_ID == ownerKingdomID
                           orderby ru.uid descending
                           group ru by ru.Province_ID into grp
                           let maxGrp = grp.Max(x => x.uid)
                           from ru in grp
                           where ru.uid == maxGrp
                           select ru).ToList();
            var CBq = (from ru in db.Utopia_Province_Data_Captured_CBs
                       where list.Province_ID == ru.Province_ID
                       group ru by ru.Province_ID into grp
                       let maxGrp = grp.Max(x => x.uid)
                       from ru in grp
                       where ru.uid == maxGrp
                       select ru).ToList();

            list.SOM = SOM;
            list.CB = CBq;
            list.SOS = SOS;
            list.Survey = Surveyq;

            return KingdomCache.removeProvinceFromKingdomCache(ownerKingdomID, list.Province_ID, cachedKingdom).Provinces.Where(x => x.Province_ID == list.Province_ID).FirstOrDefault();
        }

    }
}
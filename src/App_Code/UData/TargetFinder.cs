using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Pimp;
using Pimp.Utopia;
using Pimp.Users;
using Pimp.UParser;
using PimpLibrary.Static.Enums;
using System.Text;
using Boomers.Utilities.DatesTimes;
using System.Threading.Tasks;

namespace Pimp.UData
{
    /// <summary>
    /// Summary description for TargetFinder
    /// </summary>
    public class TargetFinder
    {

        public int LastSubmissionCount { get; set; }
        public List<ProvinceClass> TargetedProvinces { get; set; }


        static TargetFinder instance = new TargetFinder();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static TargetFinder()
        {
        }

        TargetFinder()
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            LastSubmissionCount = getSubmissionsWithinTime(db);
            TargetedProvinces = getTargetedProvinces(3, db);
        }

        public static TargetFinder Instance
        {
            get
            {
                return instance;
            }
        }



        /// <summary>
        /// removes the list of provinces from the targeted provincescache.
        /// </summary>
        /// <param name="provinceNames"></param>
        /// <param name="island"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public void removeProvinces(List<string> provinceNames, int island, int location)
        {
            foreach (var name in provinceNames)
            {
                var prov = instance.TargetedProvinces.Where(x => x.Province_Name == name).Where(x => x.Kingdom_Island == island).Where(x => x.Kingdom_Location == location).FirstOrDefault();
                instance.TargetedProvinces.Remove(prov);
            }
        }
        /// <summary>
        /// Inserts the kingdom submitted into the target finder.
        /// </summary>
        /// <param name="kingdom">Kingdom Parameters</param>
        /// <returns>The amount of provinces updated.</returns>
        public int insertTargetFinderKingdom(KingdomClass kingdom)
        {
            foreach (ProvinceClass prov in kingdom.Provinces)
            {
                ProvinceClass cP = new ProvinceClass();
                cP.Land = prov.Land;
                cP.Stance = kingdom.Stance;
                cP.Honor = prov.Nobility_ID;
                cP.Kingdom_Island = kingdom.Kingdom_Island;
                cP.Kingdom_Acres = kingdom.Acres;
                cP.Kingdom_Name = kingdom.Kingdom_Name;
                cP.Kingdom_Networth = kingdom.Networth;
                cP.Kingdom_Province_Count = kingdom.ProvinceCount;
                cP.Updated_By_DateTime = DateTime.UtcNow;
                cP.Kingdom_Location = kingdom.Kingdom_Location;
                cP.Monarch_Display = prov.Monarch_Display;
                cP.Networth = prov.Networth;
                cP.OnlineCurrently = prov.OnlineCurrently;
                cP.Protected = prov.Protected;
                cP.Province_Name = prov.Province_Name;
                cP.Race_ID = prov.Race_ID;
                cP.War = kingdom.War;
                if (prov.OnlineCurrently == 1)
                    cP.Last_Login_For_Province = DateTime.UtcNow;
                insertProvincesIntoCache(cP);
            }


            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            foreach (ProvinceClass prov in kingdom.Provinces)
            {
                var cP = (from xx in db.Utopia_Target_Finders
                          where xx.Island == kingdom.Kingdom_Island
                          where xx.Location == kingdom.Kingdom_Location
                          where xx.Province_Name == prov.Province_Name
                          select xx).FirstOrDefault();
                if (cP != null)
                {
                    cP.Acres = prov.Land;
                    cP.Stance = kingdom.Stance;
                    cP.Honor = prov.Nobility_ID;
                    cP.Island = kingdom.Kingdom_Island;
                    cP.Kingdom_Acres = kingdom.Acres;
                    cP.Kingdom_Name = kingdom.Kingdom_Name;
                    cP.Kingdom_Networth = kingdom.Networth;
                    cP.Kingdom_Province_Count = kingdom.ProvinceCount;
                    cP.Last_Updated = DateTime.UtcNow;
                    cP.Location = kingdom.Kingdom_Location;
                    cP.Monarch = prov.Monarch_Display;
                    cP.Networth = prov.Networth;
                    cP.Online = prov.OnlineCurrently;
                    cP.Protection = prov.Protected;
                    cP.Province_Name = prov.Province_Name;
                    cP.Race = prov.Race_ID;
                    cP.War = kingdom.War;
                    if (prov.OnlineCurrently == 1)
                        cP.Last_Logged_In = DateTime.UtcNow;
                }
                else
                {
                    CS_Code.Utopia_Target_Finder tf = new CS_Code.Utopia_Target_Finder();
                    tf.Acres = prov.Land;
                    tf.First_Listed_DateTime = DateTime.UtcNow;
                    tf.Honor = prov.Nobility_ID;
                    tf.Island = kingdom.Kingdom_Island;
                    tf.Kingdom_Acres = kingdom.Acres;
                    tf.Kingdom_Name = kingdom.Kingdom_Name;
                    tf.Kingdom_Networth = kingdom.Networth;
                    tf.Kingdom_Province_Count = kingdom.ProvinceCount;
                    tf.Last_Updated = DateTime.UtcNow;
                    tf.Location = kingdom.Kingdom_Location;
                    tf.Monarch = prov.Monarch_Display;
                    tf.Networth = prov.Networth;
                    tf.Online = prov.OnlineCurrently;
                    tf.Protection = prov.Protected;
                    tf.Province_ID = Guid.NewGuid();
                    tf.Province_Name = prov.Province_Name;
                    tf.Race = prov.Race_ID;
                    tf.Stance = kingdom.Stance;
                    tf.War = kingdom.War;
                    if (prov.OnlineCurrently == 1)
                        tf.Last_Logged_In = DateTime.UtcNow;
                    db.Utopia_Target_Finders.InsertOnSubmit(tf);
                }
            }
            db.SubmitChanges();



            return kingdom.Provinces.Count;
        }
        /// <summary>
        /// inserts a province into the cache to save it there.
        /// </summary>
        /// <param name="provinces"></param>
        private void insertProvincesIntoCache(ProvinceClass provinces)
        {
            var province = TargetedProvinces.Where(x => x.Province_ID == provinces.Province_ID).FirstOrDefault();
            if (province != null)
                TargetedProvinces.Remove(province);

            TargetedProvinces.Add(provinces);
        }

        /// <summary>
        /// gets the submissions of the targetfinder.
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        private int getSubmissionsWithinTime(CS_Code.UtopiaDataContext db)
        {
            return ((from xx in db.Utopia_Target_Finder_Settings
                     where xx.Last_Submission > DateTime.UtcNow.AddHours(-24)
                     select xx.User_ID).Count() + 87);
        }
        /// <summary>
        /// parses the targets and stuffs them into the targeted kingdom db.
        /// </summary>
        /// <param name="rawData"></param>
        public int parseTargets(string rawData)
        {
            KingdomClass kingdom = null;
            PimpUserWrapper currentUser = new PimpUserWrapper();
            switch (UtopiaParser.FromWhatPage(UtopiaParser.GetFormaterType(rawData, currentUser.PimpUser.UserID), rawData, currentUser.PimpUser.UserID))
            {
                case FromWhatPageEnum.Ultima:
                    kingdom = Ultima.ParseKingdomPageUltima(rawData, currentUser.PimpUser.UserID);
                    break;
                case FromWhatPageEnum.AngelKingdomPage:
                case FromWhatPageEnum.TempleKingdomPage:
                    kingdom = UtopiaParser.ParseKingdomPageAngelTemple(rawData, currentUser.PimpUser.UserID);
                    break;
                case FromWhatPageEnum.InGameKingdomPage:
                    kingdom = UtopiaParser.ParseInGameKingdomPage(rawData, currentUser.PimpUser.UserID);
                    break;
                default:
                    return 0;
            }
            if (kingdom != null)
            {
                insertTargetFinderKingdom(kingdom);
                currentUser.updateTargetFinderSettings(currentUser.PimpUser.TargetFinderSettings.Current_Provinces_Submitted += kingdom.Provinces.Count + 15, currentUser.PimpUser.TargetFinderSettings.Total_Provinces_Submitted + kingdom.Provinces.Count, DateTime.UtcNow);
                return kingdom.Provinces.Count;
            }
            return 0;
        }
        /// <summary>
        /// builds the table for the current target finder settings
        /// </summary>
        /// <param name="provinces"></param>
        /// <param name="provGetCount"></param>
        /// <returns></returns>
        public static string[] BuildTargetedHTML(List<ProvinceClass> provinces, int provGetCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table class=\"tblKingdomInfo center\" id=\"tblSearchResults\">");
            sb.Append("<thead><tr>");
            sb.Append("<th>Protected</th>");
            sb.Append("<th class=\"{sorter: 'text'}\">Province Name</th>");
            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">Island:Location</th>");
            sb.Append("<th class=\"{sorter: 'text'}\">Race</th>");
            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">Acres</th>");
            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">Networth</th>");
            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">NW/Acre</th>");
            sb.Append("<th class=\"{sorter: 'text'}\">Honor</th>");
            sb.Append("<th>Online</th>");
            sb.Append("<th class=\"{sorter: 'text'}\">Kingdom Name</th>");
            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">KD Networth</th>");
            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">KD Acres</th>");
            sb.Append("<th>Provinces In Kd</th>");
            sb.Append("<th>War</th>");
            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">Stance</th>");
            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">Last Updated</th>");
            sb.Append("<th>Last Online</th>");
            sb.Append("</tr></thead>");

            PimpUserWrapper currentUser = new PimpUserWrapper();

            int checkCount = 0;
            if (currentUser.PimpUser.TargetFinderSettings != null && currentUser.PimpUser.TargetFinderSettings.Current_Provinces_Submitted < provGetCount)
                provGetCount = currentUser.PimpUser.TargetFinderSettings.Current_Provinces_Submitted;

            List<ProvinceClass> provs;

            if (!currentUser.PimpUser.IsUserAdmin)
                provs = provinces.Take(provGetCount).OrderBy(x => x.Updated_By_DateTime).ToList();
            else
                provs = provinces.Take(100).OrderBy(x => x.Updated_By_DateTime).ToList();

            foreach (var prov in provs)
            {
                switch (checkCount % 2)
                {
                    case 0:
                        sb.Append("<tr class=\"d0\"\">");
                        break;
                    case 1:
                        sb.Append("<tr class=\"d1\"\">");
                        break;
                }

                sb.Append("<td>" + (prov.Protected.GetValueOrDefault() == 1 ? "true" : "-") + "</td>");
                sb.Append("<td>" + prov.Province_Name + "</td>");
                sb.Append("<td>" + prov.Kingdom_Island + ":" + prov.Kingdom_Location + "</td>");
                sb.Append("<td>" + UtopiaHelper.Instance.Races.Where(x => x.uid == prov.Race_ID.Value).Select(x => x.name).FirstOrDefault() + "</td>");
                sb.Append("<td>" + prov.Land.GetValueOrDefault().ToString("N0") + "</td>");
                sb.Append("<td>" + prov.Networth.GetValueOrDefault().ToString("N0") + "</td>");
                sb.Append("<td>" + (prov.Networth.GetValueOrDefault() / prov.Land.GetValueOrDefault()).ToString("N0") + "gc</td>");
                sb.Append("<td>" + UtopiaHelper.Instance.Ranks.Where(x => x.uid == prov.Honor.GetValueOrDefault()).Select(x => x.name).FirstOrDefault() + "</td>");
                sb.Append("<td>" + (prov.OnlineCurrently == 1 ? "true" : "-") + "</td>");
                sb.Append("<td>" + prov.Kingdom_Name + "</td>");
                sb.Append("<td>" + prov.Kingdom_Networth.GetValueOrDefault().ToString("N0") + "gc</td>");
                sb.Append("<td>" + prov.Kingdom_Acres.GetValueOrDefault().ToString("N0") + "</td>");
                sb.Append("<td>" + prov.Kingdom_Province_Count + "</td>");
                sb.Append("<td>" + (prov.War.GetValueOrDefault() == 1 ? "true" : "-") + "</td>");
                if (prov.Stance.HasValue)
                    sb.Append("<td>" + UtopiaHelper.Instance.KingdomStances.Where(x => x.uid == prov.Stance.GetValueOrDefault()).FirstOrDefault().name + "</td>");
                else
                    sb.Append("<td>Normal</td>");
                sb.Append("<td>" + prov.Updated_By_DateTime.Value.ToRelativeDate() + "</td>");
                sb.Append("<td>" + (prov.Last_Login_For_Province.HasValue ? prov.Last_Login_For_Province.Value.ToRelativeDate() : "-") + "</td>");
                sb.Append("</tr>");
                checkCount += 1;
            }
            sb.Append("</table>");

            currentUser.updateTargetFinderSettings(currentUser.PimpUser.TargetFinderSettings.Current_Provinces_Submitted -= checkCount, currentUser.PimpUser.TargetFinderSettings.Total_Provinces_Submitted, DateTime.UtcNow);
            return new string[] { sb.ToString(), currentUser.PimpUser.TargetFinderSettings.Current_Provinces_Submitted.ToString() };
        }

        /// <summary>
        /// returns list of targeted provinces.
        /// </summary>
        /// <param name="provinceNames"></param>
        /// <param name="island"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        private List<ProvinceClass> getTargetedProvinces(List<string> provinceNames, int island, int location)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            return (from xx in db.Utopia_Target_Finders
                    where provinceNames.Contains(xx.Province_Name)
                    where xx.Island == island
                    where xx.Location == location
                    select new ProvinceClass
                    {
                        Land = xx.Acres,
                        Honor = xx.Honor,
                        Kingdom_Island = xx.Island,
                        Kingdom_Acres = xx.Kingdom_Acres,
                        Kingdom_Name = xx.Kingdom_Name,
                        Kingdom_Networth = xx.Kingdom_Networth,
                        Kingdom_Province_Count = xx.Kingdom_Province_Count,
                        Updated_By_DateTime = xx.Last_Updated,
                        Kingdom_Location = xx.Location,
                        Monarch_Display = xx.Monarch.GetValueOrDefault(),
                        Networth = (int)xx.Networth.GetValueOrDefault(),
                        OnlineCurrently = (int)xx.Online.GetValueOrDefault(),
                        Protected = xx.Protection,
                        Province_Name = xx.Province_Name,
                        Race_ID = xx.Race,
                        Stance = xx.Stance,
                        War = xx.War
                    }).ToList();
        }
        /// <summary>
        /// gets all the privinces for the targetfinder.
        /// </summary>
        /// <returns></returns>
        private List<ProvinceClass> getTargetedProvinces(int withinDays, CS_Code.UtopiaDataContext db)
        {
            if (withinDays > 0)
                withinDays = withinDays * -1;
            return (from xx in db.Utopia_Target_Finders
                    where xx.Last_Updated > DateTime.UtcNow.AddDays(withinDays)
                    select new ProvinceClass
                    {
                        Land = xx.Acres,
                        Honor = xx.Honor,
                        Kingdom_Island = xx.Island,
                        Kingdom_Acres = xx.Kingdom_Acres,
                        Kingdom_Name = xx.Kingdom_Name,
                        Kingdom_Networth = xx.Kingdom_Networth,
                        Kingdom_Province_Count = xx.Kingdom_Province_Count,
                        Updated_By_DateTime = xx.Last_Updated,
                        Kingdom_Location = xx.Location,
                        Monarch_Display = xx.Monarch.GetValueOrDefault(),
                        Networth = (int)xx.Networth.GetValueOrDefault(),
                        OnlineCurrently = (int)xx.Online.GetValueOrDefault(),
                        Protected = xx.Protection,
                        Province_Name = xx.Province_Name,
                        Race_ID = xx.Race,
                        Stance = xx.Stance,
                        War = xx.War,
                        Last_Login_For_Province = xx.Last_Logged_In
                    }).ToList();
        }
    }
}
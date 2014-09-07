using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

using Pimp.UCache;
using Pimp.Utopia;
using Pimp.Users;
using Pimp.UData;

namespace Pimp.UParser
{
    /// <summary>
    /// Summary description for UtopiaParserUltima
    /// </summary>
    public class Ultima
    {
        public static KingdomClass ParseKingdomPageUltima(string RawData, Guid currentUserID)
        {
            KingdomClass kingdom = new KingdomClass();
            List<ProvinceClass> provs = new List<ProvinceClass>();
            Regex _findProvincesInKingdom = new Regex(@"Provinces in Kingdom:\s\d+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            kingdom.Kingdom_Name = URegEx.rgxFindIslandLocation.Replace(URegEx._findKingdomProvinceName.Match(RawData).Value, "").Trim();
            kingdom.Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(URegEx._findKingdomProvinceName.Match(RawData).Value).Value).Value).Value);
            kingdom.Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(URegEx._findKingdomProvinceName.Match(RawData).Value).Value).Value).Value);
            kingdom.ProvinceCount = Convert.ToInt32(URegEx.rgxNumber.Match(_findProvincesInKingdom.Match(RawData).Value).Value);
            if (URegEx._findKingdomStanceName.IsMatch(RawData))
                kingdom.Stance = UtopiaParser.getStanceID(URegEx._findKingdomStanceName.Match(RawData).Value.Replace("Stance:", "").Trim());
            kingdom.Acres = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findTotalKingdomLand.Match(RawData).Value).Value.Replace(",", ""));
            kingdom.Networth = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAngelHomeNetworth.Match(RawData).Value).Value.Replace(",", ""));

            string temp;
            foreach (Match m in URegEx._findUltimaKingdomProvinceAcres.Matches(RawData))
            {
                ProvinceClass prov = new ProvinceClass();
                temp = m.Value;
                prov.Race_ID = UtopiaParser.RaceNamePull(URegEx._findAngelKingdomRace.Match(temp).Value.Replace("[", "").Replace("]", ""), currentUserID);
                prov.Land = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAngelKingdomAcres.Match(temp).Value).Value.Replace(",", ""));
                prov.Province_Name = URegEx._findAngelKingdomName.Match(temp).Value.Substring(1).Replace(" [", "").Trim();

                Regex FindProvinceNetworth = new Regex(prov.Province_Name + @"\s\[" + URegEx._races + @"\]\s-\s[\d,]+gc", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                prov.Networth = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findGoldCoins.Match(FindProvinceNetworth.Match(RawData).Value).Value).Value.Replace(",", ""));

                Regex FindNobility = new Regex(prov.Province_Name + @"\s\[" + URegEx._races + @"\]\s-\s" + URegEx._nobilities, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                prov.Nobility_ID = UtopiaParser.getNobilityID(FindNobility.Match(RawData).Value, currentUserID);

                Regex _findAngelOnline = new Regex(@"ONLINE: " + prov.Province_Name, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                Regex _findAngelProtected = new Regex(@"PROTECTION: " + prov.Province_Name, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (_findAngelOnline.IsMatch(RawData))
                    prov.OnlineCurrently = 1;
                else
                    prov.OnlineCurrently = 0;
                if (_findAngelProtected.IsMatch(RawData))
                    prov.Protected = 1;
                else
                    prov.Protected = 0;

                provs.Add(prov);
            }
            kingdom.Provinces = provs;
            return kingdom;
        }
        private static string ParseAngelSurveyHome(string RawData, PimpUserWrapper  currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Province_Data_Captured_Survey UPDCS = new CS_Code.Utopia_Province_Data_Captured_Survey();
            switch (URegEx._findSurveyProvinceName.Match(RawData).Success)
            {
                case true:
                    string provinceName = URegEx.rgxFindIslandLocation.Replace(URegEx._findSurveyProvinceName.Match(RawData).Value, "").Replace("Report of ", "");
                    int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);
                    int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);
                    UPDCS.Province_ID =ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
                    HttpContext.Current.Session["SubmittedData"] += " SurveyProvinceID: " + UPDCS.Province_ID;
                    break;
                default:
                    UPDCS.Province_ID = currentUser.PimpUser.CurrentActiveProvince;
                    HttpContext.Current.Session["SubmittedData"] += " SurveyProvinceID: " + currentUser.PimpUser.CurrentActiveProvince.ToString();
                    break;
            }
            UPDCS.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
            UPDCS.Province_ID_Updated_By = currentUser.PimpUser.CurrentActiveProvince;
            UPDCS.DateTime_Updated = DateTime.UtcNow;
            UPDCS.Building_Efficiency = Convert.ToDecimal(URegEx._findBuildingEfficiency.Match(RawData).Value.Replace("Building Efficiency: ", "").Replace("%", ""));

            foreach (Match match in URegEx._findSurveyAngelBuildingsProgress.Matches(RawData))
            {
                switch (URegEx._findTextFrontOfColon.Match(match.Value).Value.Replace(":", "").Trim())
                {
                    case "Farms":
                        UPDCS.Farms_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                        if (URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Success)
                            UPDCS.Farms_P = Convert.ToInt32(URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Value.Replace(" in progress", "").Replace(",", ""));
                        break;
                    case "Banks":
                        UPDCS.Banks_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                        switch (URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Success)
                        {
                            case true:
                                UPDCS.Banks_P = Convert.ToInt32(URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Value.Replace(" in progress", "").Replace(",", ""));
                                break;
                        }
                        break;
                    case "Armouries":
                        UPDCS.Armories_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                        switch (URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Success)
                        {
                            case true:
                                UPDCS.Armories_P = Convert.ToInt32(URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Value.Replace(" in progress", "").Replace(",", ""));
                                break;
                        }
                        break;
                    case "Military Barracks":
                    case "Barracks":
                        UPDCS.Barracks_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                        switch (URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Success)
                        {
                            case true:
                                UPDCS.Barracks_P = Convert.ToInt32(URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Value.Replace(" in progress", "").Replace(",", ""));
                                break;
                        }
                        break;
                    case "Guilds":
                        UPDCS.Guilds_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                        switch (URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Success)
                        {
                            case true:
                                UPDCS.Guilds_P = Convert.ToInt32(URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Value.Replace(" in progress", "").Replace(",", ""));
                                break;
                        }
                        break;
                    case "Towers":
                        UPDCS.Towers_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                        switch (URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Success)
                        {
                            case true:
                                UPDCS.Towers_P = Convert.ToInt32(URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Value.Replace(" in progress", "").Replace(",", ""));
                                break;
                        }
                        break;
                    case "Stables":
                        UPDCS.Stables_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                        switch (URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Success)
                        {
                            case true:
                                UPDCS.Stables_P = Convert.ToInt32(URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Value.Replace(" in progress", "").Replace(",", ""));
                                break;
                        }
                        break;
                    case "Training Grounds":
                        UPDCS.TG_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                        switch (URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Success)
                        {
                            case true:
                                UPDCS.TG_P = Convert.ToInt32(URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Value.Replace(" in progress", "").Replace(",", ""));
                                break;
                        }
                        break;
                    case "Homes":
                        UPDCS.Homes_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                        switch (URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Success)
                        {
                            case true:
                                UPDCS.Homes_P = Convert.ToInt32(URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Value.Replace(" in progress", "").Replace(",", ""));
                                break;
                        }
                        break;
                    case "Guard Stations":
                        UPDCS.GS_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                        switch (URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Success)
                        {
                            case true:
                                UPDCS.GS_P = Convert.ToInt32(URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Value.Replace(" in progress", "").Replace(",", ""));
                                break;
                        }
                        break;
                    case "Dungeons":
                        UPDCS.Dungeons_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                        switch (URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Success)
                        {
                            case true:
                                UPDCS.Dungeons_P = Convert.ToInt32(URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Value.Replace(" in progress", "").Replace(",", ""));
                                break;
                        }
                        break;
                    case "Watch Towers":
                        UPDCS.WT_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                        switch (URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Success)
                        {
                            case true:
                                UPDCS.WT_P = Convert.ToInt32(URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Value.Replace(" in progress", "").Replace(",", ""));
                                break;
                        }
                        break;
                    case "Forts":
                        UPDCS.Forts_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                        switch (URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Success)
                        {
                            case true:
                                UPDCS.Forts_P = Convert.ToInt32(URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Value.Replace(" in progress", "").Replace(",", ""));
                                break;
                        }
                        break;
                    case "Hospitals":
                        UPDCS.Hospitals_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                        switch (URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Success)
                        {
                            case true:
                                UPDCS.Hostpitals_P = Convert.ToInt32(URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Value.Replace(" in progress", "").Replace(",", ""));
                                break;
                        }
                        break;
                    case "Schools":
                        UPDCS.Schools_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                        switch (URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Success)
                        {
                            case true:
                                UPDCS.Schools_P = Convert.ToInt32(URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Value.Replace(" in progress", "").Replace(",", ""));
                                break;
                        }
                        break;
                    case "Dens":
                        UPDCS.TD_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                        switch (URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Success)
                        {
                            case true:
                                UPDCS.TD_P = Convert.ToInt32(URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Value.Replace(" in progress", "").Replace(",", ""));
                                break;
                        }
                        break;
                    case "Libraries":
                        UPDCS.Library_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                        switch (URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Success)
                        {
                            case true:
                                UPDCS.Library_P = Convert.ToInt32(URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Value.Replace(" in progress", "").Replace(",", ""));
                                break;
                        }
                        break;
                    case "Mills":
                        UPDCS.Mills_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                        switch (URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Success)
                        {
                            case true:
                                UPDCS.Mills_P = Convert.ToInt32(URegEx._findSurveyAngelBuildingsInProgress.Match(match.Value).Value.Replace(" in progress", "").Replace(",", ""));
                                break;
                        }
                        break;
                    default:
                        UtopiaParser.FailedAt("ParseAngelSurveyBuildingsReport", match.Value + ",  '" + URegEx._findTextFrontOfColon.Match(match.Value).Value.Replace(":", "").Trim() + "'", currentUser.PimpUser.UserID);
                        break;
                }
            }
            switch (URegEx._findExportLine.Match(RawData).Success)
            {
                case true:
                    UPDCS.Export_Line = RawData.Remove(0, URegEx._findExportLine.Match(RawData).Index);
                    break;
            }

            db.Utopia_Province_Data_Captured_Surveys.InsertOnSubmit(UPDCS);

            var Province_Info = (from UPDCG in db.Utopia_Province_Data_Captured_Gens
                                 where UPDCG.Province_ID == UPDCS.Province_ID
                                 where UPDCG.Owner_Kingdom_ID == currentUser.PimpUser.StartingKingdom
                                 select UPDCG).FirstOrDefault();
            if (Province_Info != null)
            {
                if (URegEx._findRaceSec.IsMatch(RawData))
                    Province_Info.Race_ID = UtopiaParser.getRaceID(URegEx._findRace.Match(URegEx._findRaceSec.Match(RawData).Value).Value, currentUser.PimpUser.UserID);
                if (URegEx._findRulerNameSurvey.IsMatch(RawData))
                    Province_Info.Ruler_Name = URegEx._findRulerNameSurvey.Match(RawData).Value.Replace("Ruler Name: ", "").Replace("Personality: ", "");
                if (URegEx._findPersonality.IsMatch(RawData))
                    Province_Info.Personality_ID = UtopiaParser.GetPersonalityID(URegEx._findPersonalityNames.Match(URegEx._findPersonality.Match(RawData).Value).Value);
                Province_Info.Land = Convert.ToInt32(URegEx._findTotalLand.Match(RawData).Value.Replace("Total Land:", "").Replace(",", "").Trim());
                Province_Info.Updated_By_DateTime = DateTime.UtcNow;
                Province_Info.Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
                Province_Info.Survey_Requested = null;
                Province_Info.Survey_Requested_Province_ID = null;
                db.SubmitChanges();
            ProvinceCache.updateProvinceSurveyToCache(Province_Info.Land.Value, UPDCS, Province_Info, cachedKingdom);
                return "Survey Submitted " + Province_Info.Province_Name + " (" + Province_Info.Kingdom_Island + ":" + Province_Info.Kingdom_Location + ")";
            }
            return "Survey Submitted";
        }


    }
}
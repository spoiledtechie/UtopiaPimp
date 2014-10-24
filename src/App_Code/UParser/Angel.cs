using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

using Pimp.UCache;
using PimpLibrary.Static.Enums;
using Pimp.Utopia;
using Pimp.Users;
using SupportFramework.Data;
using Pimp.UData;
using PimpLibrary.Utopia.Ops;

namespace Pimp.UParser
{
    /// <summary>
    /// Summary description for UtopiaParserAngel
    /// </summary>
    public partial class UtopiaParser
    {
        /// <summary>
        /// Parses the an Angel Throne Home and Away page.
        /// </summary>
        /// <param name="RawData"></param>
        /// <param name="ClickedFrom"></param>
        /// <param name="ServerID"></param>
        /// <returns></returns>
        private static string ParseAngelThroneHomeAway(string RawData, int ServerID, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();

            int island, location;
            if (!URegEx._findAngelHomeProvinceName.IsMatch(RawData))
                return ReturnErrorsToUser(ErrorTypeEnum.FindProvinceName);

            string provinceName = URegEx._findAngelHomeRemoveIslandLocation.Replace(URegEx._findAngelHomeProvinceName.Match(RawData).Value.Replace("The Province of ", ""), "").Trim();

            island = Convert.ToInt32(URegEx._findAngelHomeKingdomIsland.Match(URegEx._findAngelHomeProvinceName.Match(RawData).Value).Value.Replace(":", ""));
            location = Convert.ToInt32(URegEx._findAngelHomeKingdomIslandLocation.Match(URegEx._findAngelHomeProvinceName.Match(RawData).Value).Value.Substring(1));
            Guid Province_ID = ProvinceCache.getProvinceID(provinceName, island, location, currentUser, cachedKingdom);

            CS_Code.Utopia_Province_Data_Captured_CB cb = new CS_Code.Utopia_Province_Data_Captured_CB();

            var Update = (from UPDCG in db.Utopia_Province_Data_Captured_Gens
                          where UPDCG.Province_ID == Province_ID
                          select UPDCG).FirstOrDefault();



            cb.Kingdom_ID = Update.Kingdom_ID;
            cb.Owner_Kingdom_ID = Update.Owner_Kingdom_ID;
            cb.Province_ID = Update.Province_ID;
            cb.Province_Name = Update.Province_Name;
            cb.Kingdom_Island = Update.Kingdom_Island;
            cb.Kingdom_Location = Update.Kingdom_Location;
            switch (URegEx._findAngelHomeHitHard.Match(RawData).Success)
            {
                case true:
                    string hit = URegEx._findAngelHomeHitHard.Match(RawData).Value;
                    switch (URegEx._findAngelHomeHit.Match(hit).Value)
                    {
                        case "heavily":
                            Update.Hit = "hard";
                            break;
                        case "extremely":
                            Update.Hit = "extreme";
                            break;
                        default:
                            Update.Hit = URegEx._findAngelHomeHit.Match(hit).Value;
                            break;
                    }
                    cb.Hit = Update.Hit;
                    break;
            }

            Update.Race_ID = getRaceID(URegEx._findRace.Match(URegEx._findPersonalityandRace.Match(RawData).Value).Value, currentUser.PimpUser.UserID);
            Update.Personality_ID = GetPersonalityID(URegEx._findPersonalityRacePersonality.Match(URegEx._findPersonalityandRace.Match(RawData).Value).Value.Replace(",", "").Remove(0, 1).Replace("The ", ""));
            Update.Ruler_Name = URegEx._findNewLines.Replace(URegEx._findRulerNameSurvey.Match(RawData).Value, "").Replace("Ruler Name:", "").Replace("Personality", "").Trim();
            Update.Land = Convert.ToInt32(URegEx._findAngelHomeAcres.Match(RawData).Value.Replace("Land: ", "").Replace(",", ""));
            Update.Money = Convert.ToInt64(URegEx._findAngelHomeMoney.Match(RawData).Value.Replace("Money: ", "").Replace(",", "").Replace("gc", ""));
            Update.Food = Convert.ToInt32(URegEx._findAngelHomeFood.Match(RawData).Value.Replace("Food: ", "").Replace(",", ""));
            Update.Runes = Convert.ToInt32(URegEx._findAngelHomeRunes.Match(RawData).Value.Replace("Runes: ", "").Replace(",", ""));
            Update.Population = Convert.ToInt32(URegEx._findAngelHomePopulation.Match(RawData).Value.Replace("Population: ", "").Replace(",", ""));
            Update.Peasents = Convert.ToInt32(URegEx._findAngelHomePeasants.Match(RawData).Value.Replace("Peasants: ", "").Replace(",", ""));

            cb.Race_ID = Update.Race_ID;
            cb.Personality_ID = Update.Personality_ID;
            cb.Ruler_Name = Update.Ruler_Name;
            cb.Land = Update.Land;
            cb.Money = Update.Money;
            cb.Food = Update.Food;
            cb.Runes = Update.Runes;
            cb.Population = Update.Population;
            cb.Peasents = Update.Peasents;

            if (URegEx._findAngelHomeTradeBalance.Match(RawData).Success)
            {
                Update.Trade_Balance = Convert.ToInt32(URegEx._findAngelHomeTradeBalance.Match(RawData).Value.Replace("Trade Balance: ", "").Replace(",", "").Replace("gc", ""));
                cb.Trade_Balance = Update.Trade_Balance;
            }
            if (URegEx._findMaxPossibleThievWizs.IsMatch(RawData))
                RawData = RawData.Replace(URegEx._findMaxPossibleThievWizs.Match(RawData).Value, "");

            switch (Update.Thieves_Value_Type.GetValueOrDefault())
            {
                case 1://If there is a better value than guess like a real value or an infiltrate
                case 2://infiltrate value
                    //if (!_findAngelEstThieves.IsMatch(RawData)) //there is NO guess 
                    if (URegEx._findAngelHomeThieves.IsMatch(RawData)) //Check if there is a real value
                    {
                        Update.Thieves = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAngelHomeThieves.Match(RawData).Value).Value.Replace(",", ""));
                        Update.Thieves_Value_Type = 1;
                        cb.Thieves = Update.Thieves;
                        cb.Thieves_Value_Type = Update.Thieves_Value_Type;
                    }
                    break;
                case 3: //Guess from angel
                case 4: //guess from Raw
                default:
                    if (URegEx._findAngelHomeThieves.IsMatch(RawData))
                    {
                        Update.Thieves = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAngelHomeThieves.Match(RawData).Value).Value.Replace(",", ""));
                        Update.Thieves_Value_Type = 1;
                        cb.Thieves = Update.Thieves;
                        cb.Thieves_Value_Type = Update.Thieves_Value_Type;
                    }
                    else if (URegEx._findAngelEstThieves.IsMatch(RawData))
                    {
                        Update.Thieves = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAngelEstThieves.Match(RawData).Value).Value.Replace(",", ""));
                        Update.Thieves_Value_Type = 3;
                        cb.Thieves = Update.Thieves;
                        cb.Thieves_Value_Type = Update.Thieves_Value_Type;
                    }
                    break;
            }
            switch (Update.Wizards_Value_Type.GetValueOrDefault())
            {
                case 1://This is a solid value.  No guesses..
                case 2://dud, just saving the spot for thieves agreeance
                    //if (!_findAngelEstWizards.IsMatch(RawData))
                    if (URegEx._findAngelHomeWizards.IsMatch(RawData))
                    {
                        Update.Wizards = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAngelHomeWizards.Match(RawData).Value).Value.Replace(",", ""));
                        Update.Wizards_Value_Type = 1;
                        cb.Wizards = Update.Wizards;
                        cb.Wizards_Value_Type = Update.Wizards_Value_Type;
                    }
                    break;
                case 3://guess from angel
                case 4://guess from Raw
                default:
                    if (URegEx._findAngelHomeWizards.IsMatch(RawData))
                    {
                        Update.Wizards = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAngelHomeWizards.Match(RawData).Value).Value.Replace(",", ""));
                        Update.Wizards_Value_Type = 1;
                        cb.Wizards = Update.Wizards;
                        cb.Wizards_Value_Type = Update.Wizards_Value_Type;
                    }
                    else if (URegEx._findAngelEstWizards.IsMatch(RawData))
                    {
                        Update.Wizards = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAngelEstWizards.Match(RawData).Value).Value.Replace(",", ""));
                        Update.Wizards_Value_Type = 3;
                        cb.Wizards = Update.Wizards;
                        cb.Wizards_Value_Type = Update.Wizards_Value_Type;
                    }
                    break;
            }

            if (URegEx._findAngelHomeNetworth.IsMatch(RawData))
            {
                Update.Networth = Convert.ToInt32(URegEx._findAngelHomeNetworth.Match(RawData).Value.Replace("Networth: ", "").Replace(",", ""));
                cb.Networth = Update.Networth;
            }
            if (Update.Peasents.HasValue & Update.Population.HasValue)
            {
                Update.Draft = 1 - ((decimal)Update.Peasents.Value / (decimal)Update.Population.Value);
                cb.Draft = Update.Draft;
            }
            if (URegEx._findAngelHomeSoldiers.IsMatch(RawData))
                Update.Soldiers = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAngelHomeSoldiers.Match(RawData).Value).Value.Replace(",", ""));
            if (URegEx._findAngelHomeWarHorses.IsMatch(RawData))
                Update.War_Horses = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAngelHomeWarHorses.Match(RawData).Value).Value.Replace(",", ""));
            if (URegEx._findAngelHomePrisoners.IsMatch(RawData))
                Update.Prisoners = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAngelHomePrisoners.Match(RawData).Value).Value.Replace(",", ""));

            if (URegEx._findAngelHomeTotalModOff.IsMatch(RawData))
                Update.Military_Net_Off = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAngelHomeTotalModOff.Match(RawData).Value).Value.Replace(",", ""));
            if (URegEx._findAngelHomeTotalModDef.IsMatch(RawData))
                Update.Military_Net_Def = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAngelHomeTotalModDef.Match(RawData).Value).Value.Replace(",", ""));

            Update.Building_Effectiveness = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAngelHomeBuildingEff.Match(RawData).Value).Value.Replace(",", ""));
            cb.Soldiers = Update.Soldiers;
            cb.War_Horses = Update.War_Horses;
            cb.Prisoners = Update.Prisoners;
            cb.Total_Mod_Offense = Update.Military_Net_Off;
            cb.Total_Mod_Defense = Update.Military_Net_Def;
            cb.Building_Effectiveness = Update.Building_Effectiveness;

            string exportLine = URegEx._findExportLine.Match(RawData).Value;
            if (exportLine.Length < 300 & exportLine != string.Empty)
                Update.CB_Export_Line = RawData.Remove(0, RawData.IndexOf(exportLine));
            else
                Update.CB_Export_Line = null;

            cb.CB_Export_Line = Update.CB_Export_Line;
            if (URegEx._findOffense.IsMatch(RawData))
                Update.Soldiers_Regs_Off = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findOffense.Match(RawData).Value).Value.Replace(",", ""));
            if (URegEx._findDefense.IsMatch(RawData))
                Update.Soldiers_Regs_Def = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findDefense.Match(RawData).Value).Value.Replace(",", ""));
            try
            {
                if (URegEx._findElites.IsMatch(RawData))
                    Update.Soldiers_Elites = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findElites.Match(RawData).Value).Value.Replace(",", ""));
            }
            catch (Exception e)
            {
                Errors.logError(e);
            }
            Update.Daily_Income = CalcDailyIncome(Update.Nobility_ID.GetValueOrDefault(0), Update.Prisoners.GetValueOrDefault(0), Update.Peasents.GetValueOrDefault(1), Update.Race_ID.GetValueOrDefault(0), Update.Personality_ID.GetValueOrDefault(0));
            Update.Updated_By_DateTime = DateTime.UtcNow;
            Update.Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            Update.CB_Updated_By_DateTime = DateTime.UtcNow;
            Update.CB_Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;

            cb.Soldiers_Regs_Off = Update.Soldiers_Regs_Off;
            cb.Soldiers_Regs_Def = Update.Soldiers_Regs_Def;
            cb.Soldiers_Elites = Update.Soldiers_Elites;
            cb.Daily_Income = Update.Daily_Income;
            cb.Updated_By_DateTime = DateTime.UtcNow;
            cb.Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            if (RawData.Contains("Plague has spread throughout the people"))
                InsertOp(OpType.plague, Province_ID, currentUser, cachedKingdom);

            //Total Modified Offense: 55,571 (73.60 per Acre) 
            //Practical (25% elites): 22,655 (30.01 per Acre) 
            //Total Modified Defense: 47,717 (63.20 per Acre) 
            //Practical (75% elites): 33,931 (44.94 per Acre) 
            if (URegEx._findAngelPractical.Matches(RawData).Count > 1) //if both practical lines exist.
            {
                cb.Total_Prac_Offense = Convert.ToDecimal(URegEx.rgxQuantitiesWithComma.Matches(URegEx._findAngelPractical.Matches(RawData)[0].Value)[1].Value.Replace(",", ""));
                cb.Total_Prac_Defense = Convert.ToDecimal(URegEx.rgxQuantitiesWithComma.Matches(URegEx._findAngelPractical.Matches(RawData)[1].Value)[1].Value.Replace(",", ""));
            }
            else if (URegEx._findAngelPractical.IsMatch(RawData)) //if only one practical line exists
            {
                string defensePractical = RawData.Remove(0, RawData.IndexOf(URegEx._findAngelHomeTotalModDef.Match(RawData).Value));
                if (URegEx._findAngelPractical.IsMatch(defensePractical))
                    cb.Total_Prac_Defense = Convert.ToDecimal(URegEx.rgxQuantitiesWithComma.Matches(URegEx._findAngelPractical.Match(defensePractical).Value)[1].Value.Replace(",", ""));

                string offensePractical = RawData.Remove(0, RawData.IndexOf(URegEx._findAngelHomeTotalModDef.Match(RawData).Value));
                offensePractical = RawData.Remove(RawData.IndexOf(URegEx._findAngelHomeTotalModDef.Match(offensePractical).Value));
                if (URegEx._findAngelPractical.IsMatch(offensePractical))
                    cb.Total_Prac_Offense = Convert.ToDecimal(URegEx.rgxQuantitiesWithComma.Matches(URegEx._findAngelPractical.Match(offensePractical).Value)[1].Value.Replace(",", ""));
                else
                    cb.Total_Prac_Offense = Update.Military_Net_Off;
            }


            Update.CB_Requested = null;
            db.Utopia_Province_Data_Captured_CBs.InsertOnSubmit(cb);
            db.SubmitChanges();
            ProvinceCache.UpdateProvinceCBToCache(cb, Update, cachedKingdom);
            return "CB Submitted " + provinceName + " (" + island + ":" + location + ")";
        }
        /// <summary>
        /// Parses the Angel Survey Home Page.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        private static string ParseAngelSurveyHome(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Province_Data_Captured_Survey UPDCS = new CS_Code.Utopia_Province_Data_Captured_Survey();
            switch (URegEx._findSurveyProvinceName.Match(RawData).Success)
            {
                case true:
                    string provinceName = URegEx.rgxFindIslandLocation.Replace(URegEx._findSurveyProvinceName.Match(RawData).Value, "").Replace("Report of ", "");
                    int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);
                    int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);
                    UPDCS.Province_ID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
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

                    case "Barren Land":
                        UPDCS.BarrenLands= Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                        break;
                    default:
                        FailedAt("ParseAngelSurveyBuildingsReport", match.Value + ",  '" + URegEx._findTextFrontOfColon.Match(match.Value).Value.Replace(":", "").Trim() + "'", currentUser.PimpUser.UserID);
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
                    Province_Info.Race_ID = getRaceID(URegEx._findRace.Match(URegEx._findRaceSec.Match(RawData).Value).Value, currentUser.PimpUser.UserID);
                if (URegEx._findRulerNameSurvey.IsMatch(RawData))
                    Province_Info.Ruler_Name = URegEx._findRulerNameSurvey.Match(RawData).Value.Replace("Ruler Name: ", "").Replace("Personality: ", "");
                if (URegEx._findPersonality.IsMatch(RawData))
                    Province_Info.Personality_ID = GetPersonalityID(URegEx._findPersonalityNames.Match(URegEx._findPersonality.Match(RawData).Value).Value);
                try
                {
                    if (URegEx._findTotalLand.IsMatch(RawData))
                    {
                        var land = URegEx._findTotalLand.Match(RawData).Value.Replace("Total Land:", "").Replace(",", "").Trim();
                        Province_Info.Land = Convert.ToInt32(land);
                    }
                    else
                    {
                        Province_Info.Land = UPDCS.Armories_B.GetValueOrDefault(0) + UPDCS.Banks_B.GetValueOrDefault(0) + UPDCS.Barracks_B.GetValueOrDefault(0) + UPDCS.BarrenLands.GetValueOrDefault(0) + UPDCS.Dens_B.GetValueOrDefault(0) + UPDCS.Dungeons_B.GetValueOrDefault(0) + UPDCS.Farms_B.GetValueOrDefault(0) + UPDCS.Forts_B.GetValueOrDefault(0) + UPDCS.GS_B.GetValueOrDefault(0) + UPDCS.Guilds_B.GetValueOrDefault(0) + UPDCS.Homes_B.GetValueOrDefault(0) + UPDCS.Hospitals_B.GetValueOrDefault(0) + UPDCS.Library_B.GetValueOrDefault(0) + UPDCS.Mills_B.GetValueOrDefault(0) + UPDCS.Schools_B.GetValueOrDefault(0) + UPDCS.Stables_B.GetValueOrDefault(0) + UPDCS.TD_B.GetValueOrDefault(0) + UPDCS.TG_B.GetValueOrDefault(0) + UPDCS.Towers_B.GetValueOrDefault(0) + UPDCS.WT_B.GetValueOrDefault(0);
                    }
                }
                catch (Exception e)
                {
                    Errors.logError(e);
                }
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
        /// <summary>
        /// Parses the SOS Home Angel Page.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        private static string ParseAngelSOSHome(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            decimal percentage = 0;
            int points = 0;
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Province_Data_Captured_Science UPDCS = new CS_Code.Utopia_Province_Data_Captured_Science();

            if (URegEx._findSOSAngelProvinceName.IsMatch(RawData))
            {
                string provinceName = URegEx.rgxFindIslandLocation.Replace(URegEx._findSOSAngelProvinceName.Match(RawData).Value, "").Replace("Intelligence on ", "").Replace("Science Intel on", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);
                UPDCS.Province_ID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
            }
            else
            {
                UPDCS.Province_ID = currentUser.PimpUser.CurrentActiveProvince;
                HttpContext.Current.Session["SubmittedData"] += " ParseAngelSOSHomeProvID: " + currentUser.PimpUser.CurrentActiveProvince.ToString();
            }

            UPDCS.DateTime_Added = DateTime.UtcNow;
            UPDCS.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
            UPDCS.Province_ID_Added = currentUser.PimpUser.CurrentActiveProvince;

            switch (URegEx._findSOSHomePercentagePoints.Matches(RawData).Count)
            {
                case 0:
                    foreach (Match match in URegEx._findSOSAwayPercentages.Matches(RawData))
                    {
                        percentage = Convert.ToDecimal(URegEx._findPercentages.Match(match.Value).Value.Replace("%", ""));
                        if (match.Value.Contains("Income"))
                            UPDCS.SOS_Alchemy_Percent = percentage;
                        else if (match.Value.Contains("Building Effectiveness"))
                            UPDCS.SOS_Tools_Percent = percentage;
                        else if (match.Value.Contains("Population Limits"))
                            UPDCS.SOS_Housing_Percent = percentage;
                        else if (match.Value.Contains("Food Production"))
                            UPDCS.SOS_Food_Percent = percentage;
                        else if (match.Value.Contains("Gains in Combat"))
                            UPDCS.SOS_Miltary_Percent = percentage;
                        else if (match.Value.Contains("Thievery Effectiveness"))
                            UPDCS.SOS_Thieves_Percent = percentage;
                        else if (match.Value.Contains("Magic Effectiveness & Rune Production"))
                            UPDCS.SOS_Magic_Percent = percentage;
                        else
                            FailedAt("RawSOSAway", match.Value, currentUser.PimpUser.UserID);
                    }
                    break;
            }
            if (URegEx._findSOSHomePercentagePoints.Matches(RawData).Count > 0)
            {
                foreach (Match match in URegEx._findSOSHomePercentagePoints.Matches(RawData))
                {
                    percentage = Convert.ToDecimal(URegEx._findPercentages.Match(match.Value).Value.Replace("%", ""));
                    points = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findSOSHomePoints.Match(match.Value).Value).Value.Replace(",", "").Trim());
                    if (match.Value.Contains("Income"))
                    {
                        UPDCS.SOS_Alchemy_Percent = percentage;
                        UPDCS.SOS_Alchemy = points;
                    }
                    else if (match.Value.Contains("Building Effectiveness"))
                    {
                        UPDCS.SOS_Tools_Percent = percentage;
                        UPDCS.SOS_Tools = points;
                    }
                    else if (match.Value.Contains("Population Limits"))
                    {
                        UPDCS.SOS_Housing_Percent = percentage;
                        UPDCS.SOS_Housing = points;
                    }
                    else if (match.Value.Contains("Food Production"))
                    {
                        UPDCS.SOS_Food_Percent = percentage;
                        UPDCS.SOS_Food = points;
                    }
                    else if (match.Value.Contains("Gains in Combat") || match.Value.Contains("Combat Gains"))
                    {
                        UPDCS.SOS_Miltary_Percent = percentage;
                        UPDCS.SOS_Military = points;
                    }
                    else if (match.Value.Contains("Thievery Effectiveness"))
                    {
                        UPDCS.SOS_Thieves_Percent = percentage;
                        UPDCS.SOS_Thieves = points;
                    }
                    else if (match.Value.Contains("Magic Effectiveness & Rune Production") || match.Value.Contains("Magic Effectiveness"))
                    {
                        UPDCS.SOS_Magic_Percent = percentage;
                        UPDCS.SOS_Magic = points;
                    }
                    else
                        FailedAt("RawSOSAway", match.Value, currentUser.PimpUser.UserID);
                }
            }

            foreach (Match match in URegEx._findSOSHomeInProgress.Matches(RawData))
            {
                if (match.Value.Contains("Alchemy"))
                    UPDCS.SOS_Alchemy_Prog = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                else if (match.Value.Contains("Tools"))
                    UPDCS.SOS_Tools_Prog = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                else if (match.Value.Contains("Housing"))
                    UPDCS.SOS_Housing_Prog = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                else if (match.Value.Contains("Food"))
                    UPDCS.SOS_Food_Prog = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                else if (match.Value.Contains("Military"))
                    UPDCS.SOS_Military_Prog = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                else if (match.Value.Contains("Crime"))
                    UPDCS.SOS_Thieves_Prog = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                else if (match.Value.Contains("Channeling"))
                    UPDCS.SOS_Magic_Prog = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(match.Value).Value.Replace(",", ""));
                else if (match.Value.Contains("Books to Allocate") | match.Value.Contains("Land") | match.Value.Contains("Peasants") | match.Value.Contains("Version"))
                {                //Do nothing
                }
                else
                    FailedAt("PointsInProgressSOS", match.Value, currentUser.PimpUser.UserID);
            }

            if (URegEx._findExportLine.Match(RawData).Success)
                UPDCS.Export_Line = RawData.Remove(0, URegEx._findExportLine.Match(RawData).Index);

            var getProv = (from xx in db.Utopia_Province_Data_Captured_Gens
                           where xx.Province_ID == UPDCS.Province_ID
                           select xx).FirstOrDefault();
            if (getProv != null)
            {
                getProv.SOS_Requested = null;
                getProv.Updated_By_DateTime = DateTime.UtcNow;
                getProv.Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            }
            db.Utopia_Province_Data_Captured_Sciences.InsertOnSubmit(UPDCS);
            db.SubmitChanges();
            ProvinceCache.updateProvinceSOSToCache(UPDCS, getProv, cachedKingdom);
            return "SOS Submitted " + getProv.Province_Name + " (" + getProv.Kingdom_Island + ":" + getProv.Kingdom_Location + ")";
        }
        /// <summary>
        /// Parses the Army at home Angel page.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        private static string ParseAngelMilitaryHome(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            DateTime datetime = DateTime.UtcNow;
            Guid Province_ID;
            if (URegEx._findAngelMilTempleSelfProvinceName.Match(RawData).Success)
                Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            else if (URegEx._findAngelMilProvinceName.Match(RawData).Success)
            {
                string provinceName = URegEx.rgxFindIslandLocation.Replace(URegEx._findAngelMilProvinceName.Match(RawData).Value, "").Replace("Military Intel on", "").Replace("Military Intelligence on ", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);
                Province_ID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
            }
            else
            {
                Province_ID = currentUser.PimpUser.CurrentActiveProvince;
                HttpContext.Current.Session["SubmittedData"] += " AngelMilHomeProvinceID: " + currentUser.PimpUser.CurrentActiveProvince.ToString();
            }
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var getProv = (from frmUPDCG in db.Utopia_Province_Data_Captured_Gens
                           where frmUPDCG.Province_ID == Province_ID
                           select frmUPDCG).FirstOrDefault();

            if (URegEx._findRulerRace.Match(RawData).Success)
            {
                string RulerRace = URegEx._findRulerRace.Match(RawData).Value;
                getProv.Race_ID = getRaceID(URegEx._findRace.Match(RulerRace.Remove(0, RulerRace.IndexOf(","))).Value, currentUser.PimpUser.UserID);
                getProv.Ruler_Name = RulerRace.Remove(RulerRace.IndexOf(",")).Replace("Ruler & Race: ", "");
            }
            else if (URegEx._findPersonalityandRace.Match(RawData).Success)
            {
                getProv.Race_ID = getRaceID(URegEx._findRace.Match(URegEx._findPersonalityandRace.Match(RawData).Value).Value, currentUser.PimpUser.UserID);
                getProv.Personality_ID = GetPersonalityID(URegEx._findPersonalityRacePersonality.Match(URegEx._findPersonalityandRace.Match(RawData).Value).Value.Replace(",", "").Remove(0, 1).Replace("The ", ""));
                getProv.Ruler_Name = URegEx._findNewLines.Replace(URegEx._findRulerName.Match(RawData).Value, "").Replace("Ruler Name: ", "");
            }
            else if (URegEx._findRulerName.Match(RawData).Success)
            {
                getProv.Race_ID = getRaceID(URegEx._findRace.Match(RawData).Value, currentUser.PimpUser.UserID);
                getProv.Ruler_Name = URegEx._findRulerName.Match(RawData).Value.Replace("Ruler Name: ", "");
            }

            if (URegEx._findAngelMilMilitaryEffDef.IsMatch(RawData))
            {
                getProv.Military_Efficiency_Def = Convert.ToDecimal(URegEx._findAngelMilMilitaryEffDef.Match(RawData).Value.Replace("def", "").Replace(",", "").Replace("%", "").Trim());
                getProv.Military_Efficiency_Off = Convert.ToDecimal(URegEx._findAngelMilMilitaryEffOff.Match(RawData).Value.Replace("off", "").Replace(",", "").Replace("%", "").Trim());
            }
            if (URegEx._findAngelMilMilitaryEffOverAll.Match(RawData).Success)
                getProv.Mil_Overall_Efficiency = Convert.ToDecimal(URegEx._findAngelMilMilitaryEffOverAll.Match(RawData).Value.Replace("raw", "").Replace(",", "").Replace("%", "").Trim());

            if (URegEx._findAngelMilNetOffensive.IsMatch(RawData))
            {
                getProv.Military_Current_Off = Convert.ToDecimal(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAngelMilNetOffensive.Match(RawData).Value).Value.Replace(",", ""));
                getProv.Military_Net_Off = getProv.Military_Current_Off;
            }
            if (URegEx._findAngelMilNetDefensive.IsMatch(RawData))
            {
                getProv.Military_Net_Def = Convert.ToDecimal(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAngelMilNetDefensive.Match(RawData).Value).Value.Replace(",", ""));
                getProv.Military_Current_Def = getProv.Military_Net_Def;
            }

            getProv.SOM_Updated_By_DateTime = datetime;
            getProv.SOM_Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            getProv.Updated_By_DateTime = DateTime.UtcNow;
            getProv.Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            getProv.SOM_Requested = null;

            List<CS_Code.Utopia_Province_Data_Captured_Type_Military> mils = new List<CS_Code.Utopia_Province_Data_Captured_Type_Military>();
            if (URegEx._findAngelMilStandingArmy.Match(RawData).Success)
            {
                string RawDataStanding = RawData;
                CS_Code.Utopia_Province_Data_Captured_Type_Military UPDCTM = new CS_Code.Utopia_Province_Data_Captured_Type_Military();
                RawDataStanding = RawDataStanding.Remove(0, URegEx._findAngelMilStandingArmy.Match(RawDataStanding).Index - 1);

                if (RawDataStanding.Contains("** Troops in Training **"))
                {
                    string troopsTraining = RawDataStanding.Remove(0, RawDataStanding.IndexOf("** Troops in Training **"));
                    string temp = string.Empty;
                    if (URegEx._findOffense.IsMatch(troopsTraining))
                    {
                        temp = URegEx._findOffense.Match(troopsTraining).Value;
                        UPDCTM.Regs_Off_Train = Convert.ToInt32(URegEx.rgxNumber.Match(temp).Value);
                        if (URegEx._findTrainingQueue.IsMatch(troopsTraining))
                        {
                            UPDCTM.Regs_Off_Train_Queue = URegEx._findTrainingQueue.Match(troopsTraining).Value;
                            troopsTraining = troopsTraining.Replace(UPDCTM.Regs_Off_Train_Queue, "");
                        }
                        troopsTraining = troopsTraining.Replace(temp, "");
                    }
                    if (URegEx._findDefense.IsMatch(troopsTraining))
                    {
                        temp = URegEx._findDefense.Match(troopsTraining).Value;
                        UPDCTM.Regs_Def_Train = Convert.ToInt32(URegEx.rgxNumber.Match(temp).Value);
                        if (URegEx._findTrainingQueue.IsMatch(troopsTraining))
                        {
                            UPDCTM.Regs_Def_Train_Queue = URegEx._findTrainingQueue.Match(troopsTraining).Value;
                            troopsTraining = troopsTraining.Replace(UPDCTM.Regs_Def_Train_Queue, "");
                        }
                        troopsTraining = troopsTraining.Replace(temp, "");
                    }
                    if (URegEx._findElites.IsMatch(troopsTraining))
                    {
                        temp = URegEx._findElites.Match(troopsTraining).Value;
                        UPDCTM.Elites_Train = Convert.ToInt32(URegEx.rgxNumber.Match(temp).Value);
                        if (URegEx._findTrainingQueue.IsMatch(troopsTraining))
                        {
                            UPDCTM.Elites_Train_Queue = URegEx._findTrainingQueue.Match(troopsTraining).Value;
                            troopsTraining = troopsTraining.Replace(UPDCTM.Elites_Train_Queue, "");
                        }
                        troopsTraining = troopsTraining.Replace(temp, "");
                    }
                    if (URegEx._findThieves.IsMatch(troopsTraining))
                    {
                        temp = URegEx._findThieves.Match(troopsTraining).Value;
                        UPDCTM.Thieves_Train = Convert.ToInt32(URegEx.rgxNumber.Match(temp).Value);
                        if (URegEx._findTrainingQueue.IsMatch(troopsTraining))
                        {
                            UPDCTM.Thieves_Train_Queue = URegEx._findTrainingQueue.Match(troopsTraining).Value;
                            troopsTraining = troopsTraining.Replace(UPDCTM.Thieves_Train_Queue, "");
                        }
                        troopsTraining = troopsTraining.Replace(temp, "");
                    }
                }

                if (URegEx._findAngelMilArmiesGone.Match(RawData).Success)
                    RawDataStanding = RawDataStanding.Remove(URegEx._findAngelMilArmiesGone.Match(RawDataStanding).Index);
                else if (RawDataStanding.Contains("** Troops in Training **"))
                    RawDataStanding = RawDataStanding.Remove(RawDataStanding.IndexOf("** Troops in Training **"));

                UPDCTM.DateTime_Added = datetime;

                if (URegEx._findGenerals.Match(RawDataStanding).Success)
                    UPDCTM.Generals = URegEx._findGenerals.Matches(RawDataStanding).Count;

                RawDataStanding = GetWarHorseData(RawDataStanding, UPDCTM);

                UPDCTM.Military_Location = 1;
                UPDCTM.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
                UPDCTM.Wages = (int)getProv.Mil_Wage.GetValueOrDefault(0);
                //UPDCTM.Military_Population = getProv..Peasents_Non_Percentage.GetValueOrDefault(0);
                UPDCTM.Efficiency_Def = getProv.Military_Efficiency_Def.GetValueOrDefault(0);
                UPDCTM.Efficiency_Off = getProv.Military_Efficiency_Off.GetValueOrDefault(0);
                UPDCTM.Efficiency_Raw = getProv.Mil_Overall_Efficiency.GetValueOrDefault(0);
                UPDCTM.Net_Defense_Pts_Home = (int)getProv.Military_Current_Def.GetValueOrDefault(0);
                UPDCTM.Net_Offense_Pts_Home = (int)getProv.Military_Current_Off.GetValueOrDefault(0);

                UPDCTM.Province_ID = Province_ID;
                UPDCTM.Province_ID_Added = currentUser.PimpUser.CurrentActiveProvince;
                UPDCTM.Military_Population = 0;
                RawDataStanding = GetSoldiersData(RawDataStanding, UPDCTM);
                GetMilOFffDefElitesData(RawDataStanding, UPDCTM);
                GetCapturedLandData(RawDataStanding, UPDCTM);

                SetExportLine(RawDataStanding, UPDCTM);

                db.Utopia_Province_Data_Captured_Type_Militaries.InsertOnSubmit(UPDCTM);
                mils.Add(UPDCTM);
            }
            switch (URegEx._findAngelMilArmiesGone.Match(RawData).Success)
            {
                case true:
                    string RawDataStanding = RawData;
                    switch (URegEx._findAngelMilArmiesGone.Matches(RawData).Count)
                    {
                        case 1:
                            RawDataStanding = RawDataStanding.Remove(0, URegEx._findAngelMilArmiesGone.Match(RawDataStanding).Index);
                            SetMilitaryArmies(Province_ID, RawDataStanding, datetime, mils, getProv, currentUser.PimpUser.StartingKingdom, currentUser);
                            break;
                        default:
                            foreach (Match match in URegEx._findAngelMilArmiesGone.Matches(RawData))
                            {
                                int index1, index2;
                                index1 = match.Index;
                                switch (match.NextMatch().Success)
                                {
                                    case true:
                                        index2 = match.NextMatch().Index;
                                        break;
                                    default:
                                        index2 = RawData.Length;
                                        break;
                                }
                                RawDataStanding = RawData.Remove(index2 - 1).Remove(0, index1);
                                SetMilitaryArmies(Province_ID, RawDataStanding, datetime, mils, getProv, currentUser.PimpUser.StartingKingdom, currentUser);
                            }
                            break;
                    }
                    break;
            }

            db.SubmitChanges();
            ProvinceCache.UpdateProvinceSOMToCache(getProv, mils, cachedKingdom);
            return "SOM Submitted " + getProv.Province_Name + " (" + getProv.Kingdom_Island + ":" + getProv.Kingdom_Location + ")";
        }

        public static KingdomClass ParseKingdomPageAngelTemple(string RawData, Guid currentUserID)
        {
            KingdomClass kingdom = new KingdomClass();
            List<ProvinceClass> provs = new List<ProvinceClass>();

            kingdom.Kingdom_Name = URegEx.rgxFindIslandLocation.Replace(URegEx._findKingdomProvinceName.Match(RawData).Value, "").Trim();
            kingdom.Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(URegEx._findKingdomProvinceName.Match(RawData).Value).Value).Value).Value);
            kingdom.Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(URegEx._findKingdomProvinceName.Match(RawData).Value).Value).Value).Value);
            kingdom.ProvinceCount = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx._findProvincesInKingdom.Match(RawData).Value).Value);
            if (URegEx._findKingdomStanceName.IsMatch(RawData))
                kingdom.Stance = getStanceID(URegEx._findKingdomStanceName.Match(RawData).Value.Replace("Stance:", "").Trim());
            kingdom.Acres = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findTotalKingdomLand.Match(RawData).Value).Value.Replace(",", ""));
            kingdom.Networth = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAngelHomeNetworth.Match(RawData).Value).Value.Replace(",", ""));

            string temp;
            foreach (Match m in URegEx._findAngelKingdomProvinceAcres.Matches(RawData))
            {
                ProvinceClass prov = new ProvinceClass();
                temp = m.Value;
                prov.Race_ID = RaceNamePull(URegEx._findAngelKingdomRace.Match(temp).Value.Replace("[", "").Replace("]", ""), currentUserID);
                prov.Land = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAngelKingdomAcres.Match(temp).Value).Value.Replace(",", ""));
                prov.Province_Name = URegEx._findAngelKingdomName.Match(temp).Value.Substring(1).Replace(" [", "").Trim();

                Regex FindProvinceNetworth = new Regex(prov.Province_Name + @"\s+\[([A-Z]{2})\]\s-\s[\d,]+gc", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                prov.Networth = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findGoldCoins.Match(FindProvinceNetworth.Match(RawData).Value).Value).Value.Replace(",", ""));

                Regex FindNobility = new Regex(prov.Province_Name + @"\s\[([A-Z]{2})\]\s-\s" + URegEx._nobilities, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                prov.Nobility_ID = UtopiaHelper.Instance.Ranks.Where(x => x.name == URegEx._findNobility.Match(FindNobility.Match(RawData).Value).Value).Select(x => x.uid).FirstOrDefault();

                Regex _findAngelOnline = new Regex(@"ONLINE: " + prov.Province_Name, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                Regex _findAngelProtected = new Regex(@"PROTECTION: " + prov.Province_Name, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (_findAngelOnline.IsMatch(RawData))
                {
                    prov.OnlineCurrently = 1;
                    prov.Last_Login_For_Province = DateTime.UtcNow;
                }
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
        /// <summary>
        /// Parses Kingdom Page when delivered from Angel and Temple.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        private static string ParseKingdomPageAngelTemple(string RawData, string ClickedFrom, string Provincename, int ServerID, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            var k = ParseKingdomPageAngelTemple(RawData, currentUser.PimpUser.UserID);

            switch (ClickedFrom)
            {
                case "StartKingdom":
                    StartKingdom(k, ServerID, Provincename, currentUser, cachedKingdom);
                    return k.Kingdom_Name + " (" + k.Kingdom_Island + ":" + k.Kingdom_Location + ")";
                default:
                    UpdateKingdom(ServerID, k, currentUser, cachedKingdom);// KingdomName, Convert.ToInt32(KingdomIsland), Convert.ToInt32(KingdomLocation), getStance, DT, 0, 0, ownerKingdomID, currentUser);
                    return "Updated Kingdom " + k.Kingdom_Name + " (" + k.Kingdom_Island + ":" + k.Kingdom_Location + ")";
            }

        }
        /// <summary>
        /// Parse the Temple CE Page.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        private static string ParseTempleCE(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            Guid provID = currentUser.PimpUser.CurrentActiveProvince;
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            int SourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);
            int SourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);
            string month = GetMonth(RawData);
            string year = GetYear(RawData);

            RawData = RawData.Remove(URegEx._findTempleCEEndLines.Match(RawData).Index).Remove(0, URegEx._findTempleCELines.Match(RawData).Index);

            int i = 1;
            switch (URegEx._findTempleCEFirstLine.Matches(RawData).Count % 2)
            {
                case 0:
                    foreach (Match match in URegEx._findTempleCEFirstLine.Matches(RawData))
                    {
                        switch (i % 2)
                        {
                            case 1:
                                CS_Code.Utopia_Kingdom_CE UKCE = new CS_Code.Utopia_Kingdom_CE();
                                UKCE.CE_Type = Sql.GetCeTypeId(GetCEActionType(match.Value, currentUser.PimpUser.UserID), currentUser.PimpUser.UserID);
                                UKCE.DateTime_Added = DateTime.UtcNow;
                                UKCE.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
                                UKCE.Province_ID_Added = provID;
                                UKCE.Raw_Line = match.Value + " " + match.NextMatch().Value;
                                UKCE.Source_Kingdom_Island = SourceIsland;
                                UKCE.Source_Kingdom_Location = SourceLocation;
                                UKCE.Kingdom_ID = cachedKingdom.Kingdoms.Where(x => x.Kingdom_Island == SourceIsland).Where(x => x.Kingdom_Location == SourceLocation).FirstOrDefault().Kingdom_ID;
                                UKCE.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(URegEx._findKingdomProvinceName.Match(match.Value).Value, "");
                                UKCE.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(match.NextMatch().Value).Value).Value).Value);
                                UKCE.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(match.NextMatch().Value).Value).Value).Value);
                                UKCE.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(URegEx._findKingdomProvinceName.Match(match.NextMatch().Value).Value, "");
                                UKCE.Utopia_Date_Day = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx._findTempleCEDayth.Match(match.Value).Value).Value);
                                UKCE.Utopia_Month = Boomers.Utilities.DatesTimes.Formatting.Month(month);
                                UKCE.Utopia_Year = Convert.ToInt32(year);

                                var queryCheckItem = (from UKCECI in db.Utopia_Kingdom_CEs
                                                      where UKCECI.Owner_Kingdom_ID == UKCE.Owner_Kingdom_ID && UKCECI.Utopia_Month == UKCE.Utopia_Month && UKCECI.Utopia_Year == UKCE.Utopia_Year && UKCECI.Source_Province_Name == UKCE.Source_Province_Name && UKCECI.Source_Kingdom_Island == UKCE.Source_Kingdom_Island && UKCECI.Source_Kingdom_Location == UKCE.Source_Kingdom_Location && UKCECI.Target_Province_Name == UKCE.Target_Province_Name && UKCECI.Target_Kingdom_Island == UKCE.Target_Kingdom_Island && UKCECI.Target_Kingdom_Location == UKCE.Target_Kingdom_Location && UKCECI.CE_Type == UKCE.CE_Type
                                                      select UKCECI.uid).FirstOrDefault();

                                if (queryCheckItem == 0)
                                {
                                    db.Utopia_Kingdom_CEs.InsertOnSubmit(UKCE);
                                    db.SubmitChanges();
                                }
                                break;
                        }
                        i += 1;
                    }
                    break;
            }
            return "CE Submitted (" + SourceIsland + ":" + SourceLocation + ")";
            #region
            //                Kingdom News for (17:38)
            //[http://www.UtopiaTemple.com All-in-One v4.23b]
            //Month of July, YR7

            //** Legend **
            //¤¤ Intra-Kingdom Conflict
            //!!! Actions against our Kingdom
            //&& Actions by our Kingdom
            //>> Relations News
            //*!* Dragon News
            //@@ Obituaries / Defections
            //$$ Aid

            //** July YR7 **
            //&& 2nd: [anonymous province] (17:38) learned from 
            //............. Theos (20:3). 
            //!!! 3rd: abc (20:3) learned from 
            //............. Marley Mountain (17:38). 
            //&& 5th: [anonymous province] (17:38) failed to 
            //............. attack Gnome0 (12:1). 
            //&& 12th: MightyStone Mountain (17:38) attacked 
            //............. abc (20:3): 212 acres. 
            //&& 12th: Mountain of Techies (17:38) attacked 
            //............. CITY OF SHADOWS (22:40): 128 acres. 

            //** Summary **
            //&& Total Attacks Made: 4 (340 acres) 
            //&& -- Traditional March: 2 (340 acres)
            //&& -- Learn Attacks: 1
            //&& -- Failed Attacks: 1 (25% failure)
            //!!! Total Attacks Suffered: 1 
            //!!! -- Learn Attacks: 1

            //** This Kingdom (17:38) Gains/Losses **
            //Overall Gains: 340 acres.
            //  1. +212 acres: MightyStone Mountain (1/)
            //  2. +128 acres: Mountain of Techies (1/)
            //  3. no change: Marley Mountain (/1)
            //  3. no change: [anonymous province] (2/)

            //** Enemy Kingdom (Other) Gains/Losses **
            //Overall Losses: -340 acres.
            //Attacks: 1 made / 4 suffered
            //  1. no change: Gnome0 (12:1) (/1)
            //  1. no change: Theos (20:3) (/1)
            //  3. -128 acres: CITY OF SHADOWS (22:40) (/1)
            //  4. -212 acres: abc (20:3) (1/1)
            #endregion
        }
        /// <summary>
        /// Checks what Page the Angel data is from.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        private static FromWhatPageEnum FromWhatPageAngel(string RawData, Guid currentUserID)
        {
            //The Throne Page
            if (RawData.Contains("Personality & Race:") && RawData.Contains("Land:") && RawData.Contains("Money:") && RawData.Contains("Food:") && RawData.Contains("Runes:") && RawData.Contains("Population:"))
                return FromWhatPageEnum.AngelThroneHome;
            //Internal Affairs
            else if (RawData.Contains("Building Efficiency:") && (RawData.Contains("Farms") || RawData.Contains("Mills") || RawData.Contains("Homes") || RawData.Contains("Banks") || RawData.Contains("Training Grounds") || RawData.Contains("Barracks") || RawData.Contains("Forts") || RawData.Contains("Guard Stations") || RawData.Contains("Hospitals") || RawData.Contains("Guilds") || RawData.Contains("Towers") || RawData.Contains("Watchtowers") || RawData.Contains("Libraries") || RawData.Contains("Schools") || RawData.Contains("Stables") || RawData.Contains("Dungeons")))
                return FromWhatPageEnum.AngelInternalAffairsHome; //Doesn't Do Surveys inside the Thieves Calculator.
            //Sciences
            else if (RawData.Contains("Income") && RawData.Contains("Building Effectiveness") && RawData.Contains("Gains in Combat"))
            {
                if (RawData.Contains("Science cost") && RawData.Contains("** Science Points in Progress **"))
                    return FromWhatPageEnum.AngelScienceHome; //Science Home
                else
                    return FromWhatPageEnum.AngelScienceAway;  //Science Away
            }
            //Miltary
            else if (RawData.Contains("Military Intel on") || RawData.Contains("Standing Army") || RawData.Contains("Military Intelligence Formatted") || RawData.Contains("Military Intelligence on"))
                return FromWhatPageEnum.AngelMiltaryPage; //No difference ebetween pages.
            //Kingdom Page
            else if (RawData.Contains("Statistics") && RawData.Contains("Networth") && RawData.Contains("Land") && RawData.Contains("Kingdom Analysis"))
                return FromWhatPageEnum.AngelKingdomPage;  //Doesn't Differ from Home or Away
            else
                FailedAt("'FromWhatPageAngelFail'", RawData, currentUserID);
            return FromWhatPageEnum.None;
        }
        /// <summary>
        /// Finds What Page The Temple Data is for.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        private static FromWhatPageEnum FromWhatPageTemple(string RawData, Guid currentUserID)
        {
            //The Throne Page
            if (RawData.Contains("Ruler Name:") && RawData.Contains("Personality & Race:") && RawData.Contains("Land:") && RawData.Contains("Money:") && RawData.Contains("Food:") && RawData.Contains("Runes:") && RawData.Contains("Population:"))
            {
                if (RawData.Contains("Origin:") && RawData.Contains("** Thievery Advisor: **"))//Throne Away
                    return FromWhatPageEnum.TempleThroneAway;
                else if (RawData.Contains("Crystal-Ball on your province will show:") && RawData.Contains("Away bonus:")) //Throne Home
                    return FromWhatPageEnum.TempleThroneHome;
                else
                    FailedAt("The Throne PageFromFromWhatPageTemple", RawData, currentUserID);
            }
            //Internal Affairs
            else if (RawData.Contains("Building Efficiency:") && (RawData.Contains("Farms") || RawData.Contains("Mills") || RawData.Contains("Homes") || RawData.Contains("Banks") || RawData.Contains("Training Grounds") || RawData.Contains("Barracks") || RawData.Contains("Forts") || RawData.Contains("Guard Stations") || RawData.Contains("Hospitals") || RawData.Contains("Guilds") || RawData.Contains("Towers") || RawData.Contains("Watchtowers") || RawData.Contains("Libraries") || RawData.Contains("Schools") || RawData.Contains("Stables") || RawData.Contains("Dungeons")))
                return FromWhatPageEnum.TempleInternalAffairsHome; //Doesn't Do Surveys inside the Thieves Calculator.
            ///Sciences
            else if (RawData.Contains("Income") && RawData.Contains("Building Effectiveness") && RawData.Contains("Combat Gains"))
                return FromWhatPageEnum.TempleScienceHome;  //Doesn't Do Science Away.
            //Miltary
            else if (RawData.Contains("Standing Army") && RawData.Contains("Net Offense at Home"))
            {
                if (RawData.Contains("Total Offense Points"))
                    return FromWhatPageEnum.TempleMilitaryHome;  //Military Home
                else
                    return FromWhatPageEnum.TempleMilitaryAway;  //Military away
            }
            //Kingdom Page
            else if (RawData.Contains("** Statistics **") && RawData.Contains("** Relations **") && RawData.Contains("** Races **") && RawData.Contains("** Land **"))
                return FromWhatPageEnum.TempleKingdomPage;  //Doesn't Differ from Home or Away
            //CE Page
            else if (RawData.Contains("** Legend **") && RawData.Contains("** Summary **") && RawData.Contains("Intra-Kingdom Conflict"))
                return FromWhatPageEnum.TempleCEPage;// Doesn't Differ from Home or Away
            else
                FailedAt("FromWhatPageTemple", RawData, currentUserID);
            return FromWhatPageEnum.None;
        }
    }
}
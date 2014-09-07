using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pimp.UData;
using Pimp.Utopia;
using Pimp.UCache;
using System.Text.RegularExpressions;
using PimpLibrary.Static.Enums;

namespace Pimp.UParser
{
    /// <summary>
    /// Summary description for UTools
    /// </summary>
    public class UTools
    {
        /// <summary>
        /// Parses the Army at home Angel page.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        public  static string ParseUToolsMilitaryHome(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
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
                getProv.Race_ID =UtopiaParser. getRaceID(URegEx._findRace.Match(RulerRace.Remove(0, RulerRace.IndexOf(","))).Value, currentUser.PimpUser.UserID);
                getProv.Ruler_Name = RulerRace.Remove(RulerRace.IndexOf(",")).Replace("Ruler & Race: ", "");
            }
            else if (URegEx._findPersonalityandRace.Match(RawData).Success)
            {
                getProv.Race_ID = UtopiaParser.getRaceID(URegEx._findRace.Match(URegEx._findPersonalityandRace.Match(RawData).Value).Value, currentUser.PimpUser.UserID);
                getProv.Personality_ID = UtopiaParser.GetPersonalityID(URegEx._findPersonalityRacePersonality.Match(URegEx._findPersonalityandRace.Match(RawData).Value).Value.Replace(",", "").Remove(0, 1).Replace("The ", ""));
                getProv.Ruler_Name = URegEx._findNewLines.Replace(URegEx._findRulerName.Match(RawData).Value, "").Replace("Ruler Name: ", "");
            }
            else if (URegEx._findRulerName.Match(RawData).Success)
            {
                getProv.Race_ID = UtopiaParser.getRaceID(URegEx._findRace.Match(RawData).Value, currentUser.PimpUser.UserID);
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

                RawDataStanding =UtopiaParser. GetWarHorseData(RawDataStanding, UPDCTM);

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
                RawDataStanding = UtopiaParser.GetSoldiersData(RawDataStanding, UPDCTM);
                UtopiaParser.GetMilOFffDefElitesData(RawDataStanding, UPDCTM);
                UtopiaParser.GetCapturedLandData(RawDataStanding, UPDCTM);

                //SetExportLine(RawDataStanding, UPDCTM);

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
                            UtopiaParser.SetMilitaryArmies(Province_ID, RawDataStanding, datetime, mils, getProv, currentUser.PimpUser.StartingKingdom, currentUser);
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
                                UtopiaParser.SetMilitaryArmies(Province_ID, RawDataStanding, datetime, mils, getProv, currentUser.PimpUser.StartingKingdom, currentUser);
                            }
                            break;
                    }
                    break;
            }

            db.SubmitChanges();
            ProvinceCache.UpdateProvinceSOMToCache(getProv, mils, cachedKingdom);
            return "SOM Submitted " + getProv.Province_Name + " (" + getProv.Kingdom_Island + ":" + getProv.Kingdom_Location + ")";
        }

        /// <summary>
        /// Checks what Page the Angel data is from.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        public  static FromWhatPageEnum FromWhatPageUTools(string RawData, Guid currentUserID)
        {
            //The Throne Page
            if (RawData.Contains("Personality & Race:") && RawData.Contains("Land:") && RawData.Contains("Money:") && RawData.Contains("Food:") && RawData.Contains("Runes:") && RawData.Contains("Population:"))
                return FromWhatPageEnum.UToolsThrone;
            ////Internal Affairs
            //else if (RawData.Contains("Building Efficiency:") && (RawData.Contains("Farms") || RawData.Contains("Mills") || RawData.Contains("Homes") || RawData.Contains("Banks") || RawData.Contains("Training Grounds") || RawData.Contains("Barracks") || RawData.Contains("Forts") || RawData.Contains("Guard Stations") || RawData.Contains("Hospitals") || RawData.Contains("Guilds") || RawData.Contains("Towers") || RawData.Contains("Watchtowers") || RawData.Contains("Libraries") || RawData.Contains("Schools") || RawData.Contains("Stables") || RawData.Contains("Dungeons")))
            //    return FromWhatPageEnum.AngelInternalAffairsHome; //Doesn't Do Surveys inside the Thieves Calculator.
            ////Sciences
            //else if (RawData.Contains("Income") && RawData.Contains("Building Effectiveness") && RawData.Contains("Gains in Combat"))
            //{
            //    if (RawData.Contains("Science cost") && RawData.Contains("** Science Points in Progress **"))
            //        return FromWhatPageEnum.AngelScienceHome; //Science Home
            //    else
            //        return FromWhatPageEnum.AngelScienceAway;  //Science Away
            //}
            ////Miltary
            else  if (RawData.Contains("Military Intel on") || RawData.Contains("Standing Army") || RawData.Contains("Military Intelligence Formatted") || RawData.Contains("Military Intelligence on"))
                return FromWhatPageEnum.UToolsMiltaryPage; //No difference ebetween pages.
            ////Kingdom Page
            //else if (RawData.Contains("** Statistics **") && RawData.Contains("** Networth **") && RawData.Contains("** Land **"))
            //    return FromWhatPageEnum.AngelKingdomPage;  //Doesn't Differ from Home or Away
            else
            UtopiaParser.    FailedAt("'FromWhatPageUToolsFail'", RawData, currentUserID);
            return FromWhatPageEnum.None;
        }


    }
}
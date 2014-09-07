using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App_Code.CS_Code.Worker;
using Boomers.Utilities.DatesTimes;
using Boomers.Utilities.Guids;
using Pimp.UParser;
using Pimp.UCache;

using PimpLibrary.Communications;
using Pimp;
using Pimp.Utopia;
using Pimp.Users;
using SupportFramework.Data;
using Pimp.UData;

/// <summary>
/// Summary description for ProvinceDetails
/// </summary>
public static class ProvinceDetails
{
    public static string PopulateHistory(Guid provID, Guid ownerKingdomID)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class='divProvinceDetailHeader'>History of Province</div>");
        sb.Append("<ul class=\"ulList\">");

        var getAttacksCount = (from xx in db.Utopia_Province_Data_Captured_Attacks
                               where xx.Owner_Kingdom_ID == ownerKingdomID
                               where xx.Province_ID_Added == provID | xx.Province_ID_Attacked == provID
                               select xx.uid).Count();
        sb.Append("<li><a href=\"history/attacks.aspx?ID=" + provID.RemoveDashes() + "\" target=\"_blank\">Attacking History (" + getAttacksCount + ")</a></li>");

        sb.Append("<li><a href=\"history/ce.aspx?ID=" + provID.RemoveDashes() + "\" target=\"_blank\">CE History</a></li>");
        var getNoteCount = (from xx in db.Utopia_Province_Notes
                            where xx.Province_ID == provID
                            where xx.Owner_Kingdom_ID == ownerKingdomID
                            select xx.uid).Count();

        sb.Append("<li><a href=\"history/notes.aspx?ID=" + provID.RemoveDashes() + "\" target=\"_blank\">Note History (" + getNoteCount + ")</a></li>");
        var getOpCount = (from xx in db.Utopia_Province_Ops
                          where xx.Owner_Kingdom_ID == ownerKingdomID
                          where xx.Added_By_Province_ID == provID | xx.Directed_To_Province_ID == provID
                          select xx.uid).Count();
        sb.Append("<li><a href=\"history/ops.aspx?ID=" + provID.RemoveDashes() + "\" target=\"_blank\">Op History (" + getOpCount + ")</a></li>");
        sb.Append("<li><br /></li>");
        var getSOMCount = (from xx in db.Utopia_Province_Data_Captured_Type_Militaries
                           where xx.Owner_Kingdom_ID == ownerKingdomID
                           where xx.Province_ID == provID
                           select xx.uid).Count();
        sb.Append("<li><a href=\"history/som.aspx?ID=" + provID.RemoveDashes() + "\" target=\"_blank\">Military History (" + getSOMCount + ")</a></li>");
        var getSOSCount = (from xx in db.Utopia_Province_Data_Captured_Sciences
                           where xx.Owner_Kingdom_ID == ownerKingdomID
                           where xx.Province_ID == provID
                           select xx.uid).Count();
        sb.Append("<li><a href=\"history/sos.aspx?ID=" + provID.RemoveDashes() + "\" target=\"_blank\">Science History (" + getSOSCount + ")</a></li>");
        var getSurveyCount = (from xx in db.Utopia_Province_Data_Captured_Surveys
                              where xx.Owner_Kingdom_ID == ownerKingdomID
                              where xx.Province_ID == provID
                              select xx.uid).Count();
        sb.Append("<li><a href=\"history/survey.aspx?ID=" + provID.RemoveDashes() + "\" target=\"_blank\">Survey History (" + getSurveyCount + ")</a></li>");


        sb.Append("</ul>");
        return sb.ToString();
    }
    /// <summary>
    /// Used for the Province Detail Page and produces a good SOM.
    /// </summary>
    /// <param name="db"></param>
    /// <param name="provinceID"></param>
    /// <param name="update"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string PopulateSOM(Guid provinceID, Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
    {
        var prov = ProvinceCache.getProvince(ownerKingdomID, provinceID, cachedKingdom);

        if (prov != null && prov.CB != null && prov.CB.Count == 0)
            ProvinceCache.getProvinceCB(provinceID, ownerKingdomID, cachedKingdom);

        if (prov == null || prov.SOM == null || prov.SOM.Count == 0)
            prov.SOM = ProvinceCache.getProvinceSOMCached(prov.Province_ID, ownerKingdomID, cachedKingdom);

        if (prov.SOM.Count > 0)
        {
            //var cb = prov.CB.OrderByDescending(x => x.Updated_By_DateTime).FirstOrDefault();
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='divProvinceDetailHeader'>SOM on " + prov.Province_Name + " - Last Updated: " + prov.SOM_Updated_By_DateTime.GetValueOrDefault().ToLongRelativeDate() + "</div>");

            sb.Append("<ul class=\"ulProvinceDetails\">");

            sb.Append("<li>Military Intelligence on " + prov.Province_Name + " (" + prov.Kingdom_Island + ":" + prov.Kingdom_Location + ")</li>");
            sb.Append("<li>[http://www.utopiapimp.com]</li>");
            sb.Append("<li><br/></li>");
            try
            {
                //TODO: If there are no more errors after adding this line, below, remove try catch.
                if (cachedKingdom != null)
                {
                    var provincePostedBy = cachedKingdom.Provinces.Where(x => x.Province_ID == prov.SOM_Updated_By_Province_ID).FirstOrDefault();
                    if (provincePostedBy != null)
                    {
                        var racePostedBy = UtopiaHelper.Instance.Races.Where(x => x.uid == provincePostedBy.Race_ID).FirstOrDefault();

                        sb.Append("<li>[Posted By: " + provincePostedBy.Province_Name + "]");

                        if (racePostedBy != null)
                            sb.Append(" - " + racePostedBy.name);
                        sb.Append("</li>");
                    }
                }
            }
            catch (Exception exception)
            {
                string races = Boomers.Utilities.Objects.ObjectSerializer.toJson(UtopiaHelper.Instance.Races);
                Errors.logError(exception, races);
            }
            sb.Append("<li>Utopian Date: " + UtopiaParser.getUtopiaDateTime(prov.SOM_Updated_By_DateTime.GetValueOrDefault()) + "</li>");
            sb.Append("<li>Relative Date: " + prov.SOM_Updated_By_DateTime.GetValueOrDefault().ToLongRelativeDate() + "</li>");
            sb.Append("<li>Real Life Date: " + prov.SOM_Updated_By_DateTime.GetValueOrDefault() + "</li>");
            sb.Append("<li><br/></li>");
            sb.Append("<li>** Summary **</li>");
            if (prov.Peasents_Non_Percentage.HasValue)
                sb.Append("<li>Non-Peasants: " + (prov.Peasents_Non_Percentage.Value).ToString("N1") + "%</li>");

            var race = UtopiaHelper.Instance.Races.Where(x => x.uid == prov.Race_ID.GetValueOrDefault()).FirstOrDefault();
            int rawOff = 0;
            int rawDef = 0;
            if (prov.SOM.Count > 0)
            {
                DateTime dt = DateTime.UtcNow;
                var ProvinceSOMHome = prov.SOM.Where(t => t.Military_Location == 1).OrderBy(x => x.DateTime_Added).LastOrDefault();//Home
                if (ProvinceSOMHome != null)
                {
                    dt = ProvinceSOMHome.DateTime_Added;
                    if (ProvinceSOMHome.Efficiency_Off.HasValue | ProvinceSOMHome.Efficiency_Def.HasValue | ProvinceSOMHome.Efficiency_Raw.HasValue)
                    {
                        sb.Append("<li>Efficiency (SOM): ");
                        if (ProvinceSOMHome.Efficiency_Off.GetValueOrDefault(0) > 0)
                            sb.Append(ProvinceSOMHome.Efficiency_Off.Value.ToString("N1") + "% off, ");
                        if (ProvinceSOMHome.Efficiency_Def.GetValueOrDefault(0) > 0)
                            sb.Append(ProvinceSOMHome.Efficiency_Def.Value.ToString("N1") + "% def, ");
                        if (ProvinceSOMHome.Efficiency_Raw.GetValueOrDefault(0) > 0)
                            sb.Append(ProvinceSOMHome.Efficiency_Raw.Value.ToString("N1") + "% raw");
                        sb.Append("</li>");
                    }
                    if (ProvinceSOMHome.Net_Offense_Pts_Home.GetValueOrDefault(0) > 0)
                        sb.Append("<li>Net Offense at Home (from Utopia): " + ProvinceSOMHome.Net_Offense_Pts_Home.Value.ToString("N0") + "</li>");
                    if (ProvinceSOMHome.Net_Defense_Pts_Home.GetValueOrDefault(0) > 0)
                        sb.Append("<li>Net Defense at Home (from Utopia): " + ProvinceSOMHome.Net_Defense_Pts_Home.Value.ToString("N0") + "</li>");



                    sb.Append("<li>Modified Attack Points (calculated): REPLACEMEMODOFFPOINTS");
                    sb.Append("</li>");
                    sb.Append("<li>Modified Defense Points (calculated): REPLACEMEMODDEFPOINTS");
                    sb.Append("</li>");
                    sb.Append("</ul>");

                    sb.Append("<ul class=\"ulProvinceDetails\">");
                    sb.Append("<li>** Standing Army (At Home) **</li>");
                    decimal attackPnts = 0;
                    decimal defPoints = 0;
                    if (ProvinceSOMHome.Soldiers.GetValueOrDefault(0) > 0)
                    {
                        attackPnts += UtopiaParser.CalcOffSoldierPoints(ProvinceSOMHome.Soldiers.Value, prov.Military_Efficiency_Off.GetValueOrDefault(1));
                        defPoints += UtopiaParser.CalcDefSoldierPoints(ProvinceSOMHome.Soldiers.Value, prov.Military_Efficiency_Def.GetValueOrDefault(1) / 100);
                        rawOff += ProvinceSOMHome.Soldiers.Value;
                        sb.Append("<li>Soldiers: " + ProvinceSOMHome.Soldiers.Value.ToString("N0") + " (" + (ProvinceSOMHome.Soldiers.Value * prov.Military_Efficiency_Off.GetValueOrDefault(1) / 100).ToString("N0") + " offense / " + (ProvinceSOMHome.Soldiers.Value * prov.Military_Efficiency_Def.GetValueOrDefault(1) / 100).ToString("N0") + " defense) </li>");
                    }
                    if (ProvinceSOMHome.Regs_Off.GetValueOrDefault(0) > 0)
                    {
                        try
                        {
                            if (race != null)
                            {
                                sb.Append("<li>" + race.soldierOffName + ": " + ProvinceSOMHome.Regs_Off.GetValueOrDefault(0).ToString("N0") + " (" + (ProvinceSOMHome.Regs_Off.Value * 5 * prov.Military_Efficiency_Off.GetValueOrDefault(1) / 100).ToString("N0") + " offense) </li>");
                                attackPnts += UtopiaParser.CalcOffRegPoints(ProvinceSOMHome.Regs_Off.Value, prov.Military_Efficiency_Off.GetValueOrDefault(1), race.uid);
                                rawOff += (int)ProvinceSOMHome.Regs_Off.Value * race.soldierOffMultiplier;
                            }
                        }
                        catch (Exception e)
                        {
                            Errors.logError(e);
                        }
                    }
                    if (ProvinceSOMHome.Regs_Def.GetValueOrDefault(0) > 0 && race != null)
                    {
                        sb.Append("<li>" + race.soldierDefName + ": " + ProvinceSOMHome.Regs_Def.Value.ToString("N0") + " (" + (ProvinceSOMHome.Regs_Def.Value * 5 * prov.Military_Efficiency_Off.GetValueOrDefault(1) / 100).ToString("N0") + " defense) </li>");
                        defPoints += UtopiaParser.CalcDefRegPoints(ProvinceSOMHome.Regs_Def.GetValueOrDefault(0), prov.Military_Efficiency_Def.GetValueOrDefault(1), race.uid);
                        rawDef += (int)ProvinceSOMHome.Regs_Def.Value * race.soldierDefMultiplier;
                    }
                    if (ProvinceSOMHome.Elites.GetValueOrDefault(0) > 0 && race != null)
                    {
                        sb.Append("<li>" + race.eliteName + ": " + ProvinceSOMHome.Elites.GetValueOrDefault(0).ToString("N0") + " (" + (ProvinceSOMHome.Elites.Value * race.eliteOffMulitplier * prov.Military_Efficiency_Off.GetValueOrDefault(1) / 100).ToString("N0") + " offense / " + (ProvinceSOMHome.Elites.Value * race.eliteDefMulitplier * prov.Military_Efficiency_Off.GetValueOrDefault(1) / 100).ToString("N0") + " defense)</li>");
                        attackPnts += UtopiaParser.CalcOffElitePoints(ProvinceSOMHome.Elites.Value, prov.Military_Efficiency_Off.GetValueOrDefault(1), race.uid);
                        defPoints += UtopiaParser.CalcDefElitePoints(ProvinceSOMHome.Elites.Value, prov.Military_Efficiency_Def.GetValueOrDefault(1), race.uid);
                        rawDef += (int)ProvinceSOMHome.Elites.Value * race.eliteDefMulitplier;
                        rawOff += (int)ProvinceSOMHome.Elites.Value * race.eliteOffMulitplier;
                    }
                    if (ProvinceSOMHome.Horses.GetValueOrDefault(0) > 0)
                    {
                        sb.Append("<li>War-Horses: " + ProvinceSOMHome.Horses.GetValueOrDefault(0).ToString("N0") + " (up to " + (ProvinceSOMHome.Horses.Value * prov.Mil_Overall_Efficiency.GetValueOrDefault(0) / 100).ToString("N0") + " additional offense)</li>");
                        attackPnts += UtopiaParser.CalcHorsePoints(ProvinceSOMHome.Horses.Value, ProvinceSOMHome.Efficiency_Off.GetValueOrDefault(1));
                        rawOff += (int)ProvinceSOMHome.Horses.Value;
                    }
                    sb.Append("<li>Total Attack Points: " + attackPnts.ToString("N0") + "</li>");
                    sb.Append("<li>Total Defense Points: " + defPoints.ToString("N0") + "</li>");
                    sb.Append("</ul>");
                }

                var ProvinceSOM2 = prov.SOM.Where(t => t.Military_Location == 2).Where(x => x.DateTime_Added == dt).ToList();
                foreach (var item in ProvinceSOM2)
                {
                    if (item.Time_To_Return.HasValue && item.Time_To_Return.Value < DateTime.UtcNow)
                        break;
                    sb.Append("<ul class=\"ulProvinceDetails\">");
                    sb.Append("<li>** Armies Away ");
                    if (item.Time_To_Return.HasValue)
                        sb.Append("(Back in " + item.Time_To_Return.Value.Subtract(DateTime.UtcNow).Hours + ":" + item.Time_To_Return.Value.Subtract(DateTime.UtcNow).Minutes + " Hours) **</li>");
                    else
                        sb.Append("**</li>");

                    decimal attackPntss = 0;
                    decimal defPointss = 0;
                    if (item.Soldiers.GetValueOrDefault(0) > 0)
                    {
                        attackPntss += UtopiaParser.CalcOffSoldierPoints(item.Soldiers.Value, prov.Military_Efficiency_Off.GetValueOrDefault(1));
                        defPointss += UtopiaParser.CalcDefSoldierPoints(item.Soldiers.Value, prov.Military_Efficiency_Def.GetValueOrDefault(1) / 100);
                        sb.Append("<li>Soldiers: " + item.Soldiers.GetValueOrDefault(0).ToString("N0") + " (" + (item.Soldiers.Value * prov.Military_Efficiency_Off.GetValueOrDefault(1) / 100).ToString("N0") + " offense / " + (item.Soldiers.Value * prov.Military_Efficiency_Def.GetValueOrDefault(1) / 100).ToString("N0") + " defense) </li>");
                        rawOff += item.Soldiers.Value;
                        rawDef += item.Soldiers.Value;
                    }
                    if (item.Regs_Off.GetValueOrDefault(0) > 0 && prov != null && race != null)
                    {
                        try
                        {

                            sb.Append("<li>" + race.soldierOffName + ": " + item.Regs_Off.GetValueOrDefault(0).ToString("N0") + " (" + (item.Regs_Off.GetValueOrDefault(5) * 5 * prov.Military_Efficiency_Off.GetValueOrDefault(1) / 100).ToString("N0") + " offense) </li>");
                            attackPntss += UtopiaParser.CalcOffRegPoints(item.Regs_Off.Value, prov.Military_Efficiency_Off.GetValueOrDefault(1), race.uid);
                            rawOff += (int)item.Regs_Off.Value * race.soldierOffMultiplier;
                        }
                        catch (Exception e)
                        {
                            Errors.logError(e);
                        }
                    }
                    if (item.Regs_Def.GetValueOrDefault(0) > 0)
                    {
                        sb.Append("<li>" + race.soldierDefName + ": " + item.Regs_Def.Value.ToString("N0") + " (" + (item.Regs_Def.Value * 5 * prov.Military_Efficiency_Off.GetValueOrDefault(1) / 100).ToString("N0") + " defense) </li>");
                        defPointss += UtopiaParser.CalcDefRegPoints(item.Regs_Def.GetValueOrDefault(0), prov.Military_Efficiency_Def.GetValueOrDefault(1), race.uid);
                        rawDef += (int)item.Regs_Def.Value * race.soldierDefMultiplier;
                    }
                    if (item.Elites.GetValueOrDefault(0) > 0 && race != null)
                    {
                        sb.Append("<li>" + race.eliteName + ": " + item.Elites.GetValueOrDefault(0).ToString("N0") + " (" + (item.Elites.Value * race.eliteOffMulitplier * prov.Military_Efficiency_Off.GetValueOrDefault(1) / 100).ToString("N0") + " offense / " + (item.Elites.Value * race.eliteDefMulitplier * prov.Military_Efficiency_Off.GetValueOrDefault(1) / 100).ToString("N0") + " defense)</li>");
                        attackPntss += UtopiaParser.CalcOffElitePoints(item.Elites.Value, prov.Military_Efficiency_Off.GetValueOrDefault(1), race.uid);
                        defPointss += UtopiaParser.CalcDefElitePoints(item.Elites.Value, prov.Military_Efficiency_Def.GetValueOrDefault(1), race.uid);
                        rawDef += (int)item.Elites * race.eliteDefMulitplier;
                        rawOff += (int)item.Elites * race.eliteOffMulitplier;
                    }
                    if (item.Horses.GetValueOrDefault(0) > 0)
                    {
                        sb.Append("<li>War-Horses: " + item.Horses.GetValueOrDefault(0).ToString("N0") + " (up to " + (item.Horses.Value * prov.Mil_Overall_Efficiency.GetValueOrDefault(0) / 100).ToString("N0") + " additional offense)</li>");
                        attackPntss += UtopiaParser.CalcHorsePoints(item.Horses.Value, ProvinceSOMHome.Efficiency_Off.GetValueOrDefault(1));
                        rawOff += (int)item.Horses;
                    }
                    sb.Append("<li>Total Attack Points: " + attackPntss.ToString("N0") + "</li>");
                    sb.Append("<li>Total Defense Points: " + defPointss.ToString("N0") + "</li>");
                    sb.Append("<li>Captured Land: " + item.CapturedLand.GetValueOrDefault(0).ToString("N0") + "</li>");
                    if (item.Export_Line != null)
                    {
                        sb.Append("<li><br /></li>");
                        sb.Append("<li>" + item.Export_Line + "</li>");
                    }
                    sb.Append("</ul>");
                }


                if (ProvinceSOMHome != null && race != null)
                {
                    sb.Append("<ul class=\"ulProvinceDetails\">");
                    sb.Append("<li>** Troops in Training **</li>");
                    if (ProvinceSOMHome.Regs_Off_Train.GetValueOrDefault() > 0)
                    {
                        sb.Append("<li>" + race.soldierOffName + ": " + ProvinceSOMHome.Regs_Off_Train.GetValueOrDefault().ToString("N0") + "</li>");
                        sb.Append("<li>" + ProvinceSOMHome.Regs_Off_Train_Queue + "</li>");
                    }
                    if (ProvinceSOMHome.Regs_Def_Train.GetValueOrDefault() > 0)
                    {
                        sb.Append("<li>" + race.soldierDefName + ": " + ProvinceSOMHome.Regs_Def_Train.GetValueOrDefault().ToString("N0") + "</li>");
                        sb.Append("<li>" + ProvinceSOMHome.Regs_Def_Train_Queue + "</li>");
                    }
                    if (ProvinceSOMHome.Elites_Train.GetValueOrDefault() > 0)
                    {
                        sb.Append("<li>" + race.eliteName + ": " + ProvinceSOMHome.Elites_Train.GetValueOrDefault().ToString("N0") + "</li>");
                        sb.Append("<li>" + ProvinceSOMHome.Elites_Train_Queue + "</li>");
                    }
                    if (ProvinceSOMHome.Thieves_Train.GetValueOrDefault() > 0)
                    {
                        sb.Append("<li>Thieves: " + ProvinceSOMHome.Thieves_Train.GetValueOrDefault().ToString("N0") + "</li>");
                        sb.Append("<li>" + ProvinceSOMHome.Thieves_Train_Queue + "</li>");
                    }
                    //                ** Troops in Training **
                    //Trolls: 110 (643 defense)
                    //Next 21 hours: 9 6 6 4 7 5 4 6 3 3 6 6 4 7 5 5 7 4 6 3 1...
                    //Ogres: 464 (4,888 offense / 1,627 defense)
                    //Next 12 hours: 50 51 51 49 52 26 23 14 12 14 12 13...
                    //Thieves: 146
                    //Next 20 hours: 7 7 5 7 8 8 11 7 7 6 5 7 7 8 8 7 8 7 6 6...
                    //*/
                    sb.Append("</ul>");
                }

                if (prov != null && ProvinceSOMHome != null)
                {
                    try
                    {
                        sb.Replace("REPLACEMEMODOFFPOINTS", UtopiaParser.CalcModifiedOffense((double)rawOff, prov.Mil_Total_Generals.GetValueOrDefault(4), "Normal", (double)ProvinceSOMHome.Efficiency_Off.GetValueOrDefault(0) / 100).ToString("N0"));
                    }
                    catch (Exception exception)
                    {
                        sb.Replace("REPLACEMEMODOFFPOINTS", "");
                        Errors.logError(exception);
                    }
                    try
                    {
                        sb.Replace("REPLACEMEMODDEFPOINTS", UtopiaParser.CalcModDefense((double)rawDef, (double)ProvinceSOMHome.Efficiency_Def.GetValueOrDefault(0)).ToString("N0"));
                    }
                    catch (Exception exception)
                    {
                        sb.Replace("REPLACEMEMODDEFPOINTS", "");
                        Errors.logError(exception);
                    }
                }
                else
                {
                    sb.Replace("REPLACEMEMODOFFPOINTS", "");
                    sb.Replace("REPLACEMEMODDEFPOINTS", "");
                }
            }
            return sb.ToString();
        }
        return string.Empty;
    }
    /// <summary>
    /// Used for the Province Detail Page and produces a good Survey
    /// </summary>
    /// <param name="db"></param>
    /// <param name="provinceID"></param>
    /// <param name="update"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string PopulateSurvey(Guid provinceID, Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
    {
        var prov = ProvinceCache.getProvince(ownerKingdomID, provinceID, cachedKingdom);

        if (prov != null && prov.Survey.Count == 0)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var surv = (from u in db.Utopia_Province_Data_Captured_Surveys
                        where u.Province_ID == provinceID
                        orderby u.uid descending
                        select u).FirstOrDefault();
            if (surv != null)
            {
                prov.Survey.Add(surv);
                if (prov.Survey.Count > 0)
                    ProvinceCache.updateProvinceSurveyToCache(surv, cachedKingdom);
            }
        }
        if (prov == null || prov.Survey == null || prov.Survey.Count == 0)
            return string.Empty;
        else
        {
            var survFirst = prov.Survey.OrderByDescending(x => x.DateTime_Updated).FirstOrDefault();
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='divProvinceDetailHeader'>Survey on " + prov.Province_Name + " - Last Updated: " + survFirst.DateTime_Updated.ToLongRelativeDate() + "</div>");
            int built = 0;
            int progress = 0;

            sb.Append("<ul class=\"ulProvinceDetails\">");

            sb.Append("<li>Survey on " + prov.Province_Name + " (" + prov.Kingdom_Island + ":" + prov.Kingdom_Location + ")</li>");
            sb.Append("<li>[http://www.utopiapimp.com]</li>");
            sb.Append("<li><br/></li>");

            try
            {
                //TODO: If there are no more errors after adding this line, below, remove try catch.
                if (cachedKingdom != null)
                {
                    var provincePostedBy = cachedKingdom.Provinces.Where(x => x.Province_ID == prov.Updated_By_Province_ID).FirstOrDefault();
                    if (provincePostedBy != null)
                    {
                        var racePostedBy = UtopiaHelper.Instance.Races.Where(x => x.uid == provincePostedBy.Race_ID).FirstOrDefault();
                        sb.Append("<li>[Posted By: " + provincePostedBy.Province_Name + "]");

                        if (racePostedBy != null)
                            sb.Append(" - " + racePostedBy.name);
                        sb.Append("</li>");
                    }
                }
            }
            catch (Exception exception)
            {
                Errors.logError(exception);
            }


            sb.Append("<li>Utopian Date: " + UtopiaParser.getUtopiaDateTime(survFirst.DateTime_Updated) + "</li>");
            sb.Append("<li>Relative Date: " + survFirst.DateTime_Updated.ToLongRelativeDate() + "</li>");
            sb.Append("<li>Real Life Date: " + survFirst.DateTime_Updated + "</li>");
            sb.Append("<li><br/></li>");
            sb.Append("<li>** Buildings summary **</li>");
            int buildingCount = 0;
            switch (survFirst.Homes_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (survFirst.Homes_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Homes_P.Value;
                            sb.Append("<li>" + (buildingCount += 1) + ". Homes: " + survFirst.Homes_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Homes_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += survFirst.Homes_B.Value;
                    sb.Append("<li>" + (buildingCount += 1) + ". Homes: " + survFirst.Homes_B.Value.ToString("N0") + " (" + ((decimal)survFirst.Homes_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)");
                    switch (survFirst.Homes_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Homes_P.Value;
                            sb.Append(" + " + survFirst.Homes_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Homes_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (survFirst.Farms_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (survFirst.Farms_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Farms_P.Value;
                            sb.Append("<li>" + (buildingCount += 1) + ". Farms: " + survFirst.Farms_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Farms_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += survFirst.Farms_B.Value;
                    sb.Append("<li>" + (buildingCount += 1) + ". Farms: " + survFirst.Farms_B.Value.ToString("N0") + " (" + ((decimal)survFirst.Farms_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)");
                    switch (survFirst.Farms_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Farms_P.Value;
                            sb.Append(" + " + survFirst.Farms_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Farms_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (survFirst.Mills_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (survFirst.Mills_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Mills_P.Value;
                            sb.Append("<li>" + (buildingCount += 1) + ". Mills: " + survFirst.Mills_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Mills_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += survFirst.Mills_B.Value;
                    sb.Append("<li>" + (buildingCount += 1) + ". Mills: " + survFirst.Mills_B.Value.ToString("N0") + " (" + ((decimal)survFirst.Mills_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)");
                    switch (survFirst.Mills_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Mills_P.Value;
                            sb.Append(" + " + survFirst.Mills_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Mills_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (survFirst.Banks_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (survFirst.Banks_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Banks_P.Value;
                            sb.Append("<li>" + (buildingCount += 1) + ". Banks: " + survFirst.Banks_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Banks_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += survFirst.Banks_B.Value;
                    sb.Append("<li>" + (buildingCount += 1) + ". Banks: " + survFirst.Banks_B.Value.ToString("N0") + " (" + ((decimal)survFirst.Banks_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)");
                    switch (survFirst.Banks_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Banks_P.Value;
                            sb.Append(" + " + survFirst.Banks_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Banks_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (survFirst.TG_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (survFirst.TG_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.TG_P.Value;
                            sb.Append("<li>" + (buildingCount += 1) + ". Training Grounds: " + survFirst.TG_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.TG_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += survFirst.TG_B.Value;
                    sb.Append("<li>" + (buildingCount += 1) + ". Training Grounds: " + survFirst.TG_B.Value.ToString("N0") + " (" + ((decimal)survFirst.TG_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)");
                    switch (survFirst.TG_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.TG_P.Value;
                            sb.Append(" + " + survFirst.TG_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.TG_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (survFirst.Armories_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (survFirst.Armories_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Armories_P.Value;
                            sb.Append("<li>" + (buildingCount += 1) + ". Armouries: " + survFirst.Armories_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Armories_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += survFirst.Armories_B.Value;
                    sb.Append("<li>" + (buildingCount += 1) + ". Armouries: " + survFirst.Armories_B.Value.ToString("N0") + " (" + ((decimal)survFirst.Armories_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)");
                    switch (survFirst.Armories_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Armories_P.Value;
                            sb.Append(" + " + survFirst.Armories_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Armories_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (survFirst.Barracks_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (survFirst.Barracks_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Barracks_P.Value;
                            sb.Append("<li>" + (buildingCount += 1) + ". Barracks: " + survFirst.Barracks_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Barracks_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += survFirst.Barracks_B.Value;
                    sb.Append("<li>" + (buildingCount += 1) + ". Barracks: " + survFirst.Barracks_B.Value.ToString("N0") + " (" + ((decimal)survFirst.Barracks_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)");
                    switch (survFirst.Barracks_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Barracks_P.Value;
                            sb.Append(" + " + survFirst.Barracks_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Barracks_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (survFirst.Forts_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (survFirst.Forts_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Forts_P.Value;
                            sb.Append("<li>" + (buildingCount += 1) + ". Forts: " + survFirst.Forts_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Forts_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += survFirst.Forts_B.Value;
                    sb.Append("<li>" + (buildingCount += 1) + ". Forts: " + survFirst.Forts_B.Value.ToString("N0") + " (" + ((decimal)survFirst.Forts_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)");
                    switch (survFirst.Forts_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Forts_P.Value;
                            sb.Append(" + " + survFirst.Forts_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Forts_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (survFirst.GS_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (survFirst.GS_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.GS_P.Value;
                            sb.Append("<li>" + (buildingCount += 1) + ". Guard Stations: " + survFirst.GS_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.GS_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += survFirst.GS_B.Value;
                    sb.Append("<li>" + (buildingCount += 1) + ". Guard Stations: " + survFirst.GS_B.Value.ToString("N0") + " (" + ((decimal)survFirst.GS_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)");
                    switch (survFirst.GS_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.GS_P.Value;
                            sb.Append(" + " + survFirst.GS_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.GS_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (survFirst.Hospitals_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (survFirst.Hostpitals_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Hostpitals_P.Value;
                            sb.Append("<li>" + (buildingCount += 1) + ". Hostpitals: " + survFirst.Hostpitals_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Hostpitals_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += survFirst.Hospitals_B.Value;
                    sb.Append("<li>" + (buildingCount += 1) + ". Hospitals: " + survFirst.Hospitals_B.Value.ToString("N0") + " (" + ((decimal)survFirst.Hospitals_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)");
                    switch (survFirst.Hostpitals_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Hostpitals_P.Value;
                            sb.Append(" + " + survFirst.Hostpitals_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Hostpitals_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (survFirst.Guilds_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (survFirst.Guilds_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Guilds_P.Value;
                            sb.Append("<li>" + (buildingCount += 1) + ". Guilds: " + survFirst.Guilds_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Guilds_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += survFirst.Guilds_B.Value;
                    sb.Append("<li>" + (buildingCount += 1) + ". Guilds: " + survFirst.Guilds_B.Value.ToString("N0") + " (" + ((decimal)survFirst.Guilds_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)");
                    switch (survFirst.Guilds_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Guilds_P.Value;
                            sb.Append(" + " + survFirst.Guilds_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Guilds_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (survFirst.Towers_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (survFirst.Towers_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Towers_P.Value;
                            sb.Append("<li>" + (buildingCount += 1) + ". Towers: " + survFirst.Towers_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Towers_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += survFirst.Towers_B.Value;
                    sb.Append("<li>" + (buildingCount += 1) + ". Towers: " + survFirst.Towers_B.Value.ToString("N0") + " (" + ((decimal)survFirst.Towers_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)");
                    switch (survFirst.Towers_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Towers_P.Value;
                            sb.Append(" + " + survFirst.Towers_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Towers_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (survFirst.TD_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (survFirst.TD_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.TD_P.Value;
                            sb.Append("<li>" + (buildingCount += 1) + ". Thieves' Dens: " + survFirst.TD_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.TD_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += survFirst.TD_B.Value;
                    sb.Append("<li>" + (buildingCount += 1) + ". Thieves' Dens: " + survFirst.TD_B.Value.ToString("N0") + " (" + ((decimal)survFirst.TD_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)");
                    switch (survFirst.TD_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.TD_P.Value;
                            sb.Append(" + " + survFirst.TD_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.TD_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (survFirst.WT_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (survFirst.WT_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.WT_P.Value;
                            sb.Append("<li>" + (buildingCount += 1) + ". Watch Towers: " + survFirst.WT_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.WT_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += survFirst.WT_B.Value;
                    sb.Append("<li>" + (buildingCount += 1) + ". Watch Towers: " + survFirst.WT_B.Value.ToString("N0") + " (" + ((decimal)survFirst.WT_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)");
                    switch (survFirst.WT_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.WT_P.Value;
                            sb.Append(" + " + survFirst.WT_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.WT_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (survFirst.Library_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (survFirst.Library_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Library_P.Value;
                            sb.Append("<li>" + (buildingCount += 1) + ". Libraries: " + survFirst.Library_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Library_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += survFirst.Library_B.Value;
                    sb.Append("<li>" + (buildingCount += 1) + ". Libraries: " + survFirst.Library_B.Value.ToString("N0") + " (" + ((decimal)survFirst.Library_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)");
                    switch (survFirst.Library_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Library_P.Value;
                            sb.Append(" + " + survFirst.Library_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Library_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (survFirst.Schools_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (survFirst.Schools_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Schools_P.Value;
                            sb.Append("<li>" + (buildingCount += 1) + ". Schools: " + survFirst.Schools_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Schools_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += survFirst.Schools_B.Value;
                    sb.Append("<li>" + (buildingCount += 1) + ". Schools: " + survFirst.Schools_B.Value.ToString("N0") + " (" + ((decimal)survFirst.Schools_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)");
                    switch (survFirst.Schools_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Schools_P.Value;
                            sb.Append(" + " + survFirst.Schools_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Schools_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (survFirst.Stables_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (survFirst.Stables_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Stables_P.Value;
                            sb.Append("<li>" + (buildingCount += 1) + ". Stables: " + survFirst.Stables_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Stables_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += survFirst.Stables_B.Value;
                    sb.Append("<li>" + (buildingCount += 1) + ". Stables: " + survFirst.Stables_B.Value.ToString("N0") + " (" + ((decimal)survFirst.Stables_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)");
                    switch (survFirst.Stables_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Stables_P.Value;
                            sb.Append(" + " + survFirst.Stables_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Stables_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (survFirst.Dungeons_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (survFirst.Dungeons_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Dungeons_P.Value;
                            sb.Append("<li>" + (buildingCount += 1) + ". Dungeons: " + survFirst.Dungeons_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Dungeons_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += survFirst.Dungeons_B.Value;
                    sb.Append("<li>" + (buildingCount += 1) + ". Dungeons: " + survFirst.Dungeons_B.Value.ToString("N0") + " (" + ((decimal)survFirst.Dungeons_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)");
                    switch (survFirst.Dungeons_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Dungeons_P.Value;
                            sb.Append(" + " + survFirst.Dungeons_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Dungeons_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (survFirst.Dens_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (survFirst.Dens_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Dens_P.Value;
                            sb.Append("<li>" + (buildingCount += 1) + ". Dens: " + survFirst.Dens_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Dens_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += survFirst.Dens_B.Value;
                    sb.Append("<li>" + (buildingCount += 1) + ". Dens: " + survFirst.Dens_B.Value.ToString("N0") + " (" + ((decimal)survFirst.Dens_B.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)");
                    switch (survFirst.Dens_P.GetValueOrDefault(0) > 0)
                    {
                        case true:
                            progress += survFirst.Dens_P.Value;
                            sb.Append(" + " + survFirst.Dens_P.Value.ToString("N0") + " in progress (" + ((decimal)survFirst.Dens_P.Value / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }

            sb.Append("<li><br/></li>");
            sb.Append("<li>Total Acres Built: " + built.ToString("N0") + "</li>");
            sb.Append("<li>In Progress: " + progress.ToString("N0") + " (" + ((decimal)progress / (decimal)prov.Land.Value * 100).ToString("N1") + "%)</li>");
            sb.Append("<li>Total Acres: " + prov.Land.Value.ToString("N0") + "</li>");
            sb.Append("<li>Un-Built Acres: " + (prov.Land.Value - (built + progress)).ToString("N0") + "</li>");
            sb.Append("<li><br/></li>");
            sb.Append("<li>" + survFirst.Export_Line + "</li>");

            sb.Append("</ul>");
            return sb.ToString();
        }
    }
    /// <summary>
    /// Used for the Province Detail Page and produces a good SOS
    /// </summary>
    /// <param name="db"></param>
    /// <param name="provinceID"></param>
    /// <param name="update"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string PopulateSOS(Guid provinceID, Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
    {
        var prov = ProvinceCache.getProvince(ownerKingdomID, provinceID, cachedKingdom);

        if (prov != null && prov.SOS.Count == 0)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var sos = (from UPDCS in db.Utopia_Province_Data_Captured_Sciences
                       where UPDCS.Province_ID == provinceID
                       orderby UPDCS.uid descending
                       select UPDCS).FirstOrDefault();
            if (sos != null)
            {
                prov.SOS.Add(sos);
                if (sos != null)
                    ProvinceCache.UpdateProvinceSOSToCache(sos, cachedKingdom);
            }
        }

        if (prov.SOS == null || prov.SOS.Count == 0)
            return string.Empty;
        else
        {
            var sosFirst = prov.SOS.LastOrDefault();
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='divProvinceDetailHeader'>SOS on " + prov.Province_Name + " - Last Updated: " + sosFirst.DateTime_Added.ToLongRelativeDate() + "</div>");
            sb.Append("<ul class=\"ulProvinceDetails\">");
            sb.Append("<li>Science Intelligence on " + prov.Province_Name + " (" + prov.Kingdom_Island + ":" + prov.Kingdom_Location + ")</li>");
            sb.Append("<li>[http://www.utopiapimp.com]</li>");
            sb.Append("<li><br/></li>");

            try
            {
                //TODO: If there are no more errors after adding this line, below, remove try catch.
                if (cachedKingdom != null)
                {
                    var provincePostedBy = cachedKingdom.Provinces.Where(x => x.Province_ID == sosFirst.Province_ID_Added).FirstOrDefault();
                    if (provincePostedBy != null)
                    {
                        var racePostedBy = UtopiaHelper.Instance.Races.Where(x => x.uid == provincePostedBy.Race_ID).FirstOrDefault();
                        sb.Append("<li>[Posted By: " + provincePostedBy.Province_Name + "]");

                        if (racePostedBy != null)
                            sb.Append(" - " + racePostedBy.name);
                        sb.Append("</li>");
                    }
                }
            }
            catch (Exception exception)
            {
                Errors.logError(exception);
            }

            sb.Append("<li>Utopian Date: " + UtopiaParser.getUtopiaDateTime(sosFirst.DateTime_Added) + "</li>");
            sb.Append("<li>Relative Date: " + sosFirst.DateTime_Added.ToLongRelativeDate() + "</li>");
            sb.Append("<li>Real Life Date: " + sosFirst.DateTime_Added + "</li>");
            sb.Append("<li><br/></li>");
            sb.Append("<li>** Effects summary **</li>");
            if (sosFirst.SOS_Alchemy_Percent.GetValueOrDefault(0) > 0)
            {
                sb.Append("<li>+" + sosFirst.SOS_Alchemy_Percent.Value + "% Income (" + sosFirst.SOS_Alchemy.GetValueOrDefault(0).ToString("N0") + " points");
                if (sosFirst.SOS_Alchemy_Prog.GetValueOrDefault() > 0)
                    sb.Append(" + " + sosFirst.SOS_Alchemy_Prog.Value.ToString("N0") + " in progress)</li>");
                else
                    sb.Append(")</li>");
            }
            if (sosFirst.SOS_Tools_Percent.GetValueOrDefault(0) > 0)
            {
                sb.Append("<li>+" + sosFirst.SOS_Tools_Percent.Value + "% Building Effectiveness (" + sosFirst.SOS_Tools.GetValueOrDefault(0).ToString("N0") + " points");
                if (sosFirst.SOS_Tools_Prog.GetValueOrDefault() > 0)
                    sb.Append(" + " + sosFirst.SOS_Tools_Prog.Value.ToString("N0") + " in progress)</li>");
                else
                    sb.Append(")</li>");
            }
            if (sosFirst.SOS_Housing_Percent.GetValueOrDefault(0) > 0)
            {
                sb.Append("<li>+" + sosFirst.SOS_Housing_Percent.Value + "% Population Limits (" + sosFirst.SOS_Housing.GetValueOrDefault(0).ToString("N0") + " points");
                if (sosFirst.SOS_Housing_Prog.GetValueOrDefault() > 0)
                    sb.Append(" + " + sosFirst.SOS_Housing_Prog.Value.ToString("N0") + " in progress)</li>");
                else
                    sb.Append(")</li>");
            }
            if (sosFirst.SOS_Food_Percent.GetValueOrDefault(0) > 0)
            {
                sb.Append("<li>+" + sosFirst.SOS_Food_Percent.Value + "% Food Production (" + sosFirst.SOS_Food.GetValueOrDefault(0).ToString("N0") + " points");
                if (sosFirst.SOS_Food_Prog.GetValueOrDefault() > 0)
                    sb.Append(" + " + sosFirst.SOS_Food_Prog.Value.ToString("N0") + " in progress)</li>");
                else
                    sb.Append(")</li>");
            }
            if (sosFirst.SOS_Miltary_Percent.GetValueOrDefault(0) > 0)
            {
                sb.Append("<li>+" + sosFirst.SOS_Miltary_Percent.Value + "% Gains in Combat (" + sosFirst.SOS_Military.GetValueOrDefault(0).ToString("N0") + " points");
                if (sosFirst.SOS_Military_Prog.GetValueOrDefault() > 0)
                    sb.Append(" + " + sosFirst.SOS_Military_Prog.Value.ToString("N0") + " in progress)</li>");
                else
                    sb.Append(")</li>");
            }
            if (sosFirst.SOS_Thieves_Percent.GetValueOrDefault(0) > 0)
            {
                sb.Append("<li>+" + sosFirst.SOS_Thieves_Percent.Value + "% Thievery Effectiveness (" + sosFirst.SOS_Thieves.GetValueOrDefault(0).ToString("N0") + " points");
                if (sosFirst.SOS_Thieves_Prog.GetValueOrDefault() > 0)
                    sb.Append(" + " + sosFirst.SOS_Thieves_Prog.Value.ToString("N0") + " in progress)</li>");
                else
                    sb.Append(")</li>");
            }
            if (sosFirst.SOS_Magic_Percent.GetValueOrDefault(0) > 0)
            {
                sb.Append("<li>+" + sosFirst.SOS_Magic_Percent.Value + "% Magic Effectiveness & Rune Production (" + sosFirst.SOS_Magic.GetValueOrDefault(0).ToString("N0") + " points");
                if (sosFirst.SOS_Magic_Prog.GetValueOrDefault() > 0)
                    sb.Append(" + " + sosFirst.SOS_Magic_Prog.Value.ToString("N0") + " in progress)</li>");
                else
                    sb.Append(")</li>");
            }
            sb.Append("<li><br/></li>");
            sb.Append("<li>" + sosFirst.Export_Line + "</li>");

            sb.Append("</ul>");
            return sb.ToString();
        }
    }
    /// <summary>
    /// Used for the Province Detail Page and produces a good CB.
    /// </summary>
    /// <param name="db"></param>
    /// <param name="provinceID"></param>
    /// <param name="update"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string PopulateCB(Guid provinceID, Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
    {
        var getCB = ProvinceCache.getProvinceCB(provinceID, ownerKingdomID, cachedKingdom);
        var getProvince = ProvinceCache.getProvince(ownerKingdomID, provinceID, cachedKingdom);
        if (getCB != null)
        {
            var raceInfo = UtopiaHelper.Instance.Races.Where(x => x.uid == getCB.Race_ID.GetValueOrDefault()).FirstOrDefault();
            if (getCB.Updated_By_DateTime.HasValue)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<div class='divProvinceDetailHeader'>CB on " + getCB.Province_Name + " - Last Updated: " + getCB.Updated_By_DateTime.GetValueOrDefault().ToLongRelativeDate() + "</div>");
                sb.Append("<ul class=\"ulProvinceDetails\">");
                sb.Append("<li>The Province of " + getCB.Province_Name + " (" + getCB.Kingdom_Island + ":" + getCB.Kingdom_Location + ")</li>");
                sb.Append("<li>[http://www.utopiapimp.com]</li>");
                sb.Append("<li><br/></li>");
                if (getCB.Updated_By_Province_ID.HasValue)
                    try { sb.Append("<li>[Posted by: " + ProvinceCache.getProvince(ownerKingdomID, getCB.Updated_By_Province_ID.Value, cachedKingdom).Province_Name + "] - " + UtopiaHelper.Instance.Races.Where(x => x.uid == ProvinceCache.getProvince(ownerKingdomID, getCB.Updated_By_Province_ID.Value, cachedKingdom).Race_ID).FirstOrDefault().name + "</li>"); }
                    catch { }

                switch (getCB.Updated_By_DateTime.HasValue)
                {
                    case true:
                        sb.Append("<li>Utopian Date: " + UtopiaParser.getUtopiaDateTime(getCB.Updated_By_DateTime.Value) + "</li>");
                        sb.Append("<li>Relative Date: " + getCB.Updated_By_DateTime.Value.ToLongRelativeDate() + "</li>");
                        sb.Append("<li>Real Life Date: " + getCB.Updated_By_DateTime.Value + "</li>");
                        break;
                }
                sb.Append("<li><br/></li>");
                if (getCB.Ruler_Name.Length > 1) { sb.Append("<li>Ruler Name: " + getCB.Ruler_Name + "</li>"); }
                sb.Append("<li>Personality & Race: " + UtopiaHelper.Instance.Personalities.Where(x => x.uid == getCB.Personality_ID.GetValueOrDefault(0)).Select(x => x.name).FirstOrDefault() + ", " + UtopiaHelper.Instance.Races.Where(x => x.uid == getCB.Race_ID.GetValueOrDefault(0)).Select(x => x.name).FirstOrDefault() + "</li>");
                sb.Append("<li>Nobility: " + UtopiaHelper.Instance.Ranks.Where(x => x.uid == getProvince.Nobility_ID.GetValueOrDefault(0)).Select(x => x.name).FirstOrDefault() + "</li>");
                switch (getCB.Land.GetValueOrDefault(0))
                {
                    case 0: break;
                    default:
                        sb.Append("<li>Acres: " + getCB.Land.Value.ToString("N0") + " Acres</li>"); break;
                }
                switch (getCB.Money.GetValueOrDefault(0))
                {
                    case 0: break;
                    default:
                        sb.Append("<li>Money: " + getCB.Money.Value.ToString("N0") + "gc (" + getCB.Daily_Income.GetValueOrDefault(0).ToString("N0") + "gc daily income)</li>"); break;
                }
                switch (getCB.Food.GetValueOrDefault(0))
                {
                    case 0: break;
                    default:
                        sb.Append("<li>Food: " + getCB.Food.Value.ToString("N0") + " bushels</li>"); break;
                }
                switch (getCB.Runes.GetValueOrDefault(0))
                {
                    case 0: break;
                    default:
                        sb.Append("<li>Runes: " + getCB.Runes.Value.ToString("N0") + " runes</li>"); break;
                }
                switch (getCB.Population.GetValueOrDefault(0))
                {
                    case 0: break;
                    default:
                        sb.Append("<li>Population: " + getCB.Population.Value.ToString("N0") + " citzens (" + getCB.Population.GetValueOrDefault(1) / getCB.Land.GetValueOrDefault(1) + " per acre)</li>"); break;
                }
                switch (getCB.Peasents.GetValueOrDefault(0))
                {
                    case 0: break;
                    default:
                        sb.Append("<li>Peasants: " + getCB.Peasents.Value.ToString("N0") + " (" + (((decimal)getCB.Peasents.GetValueOrDefault(0) / (decimal)getCB.Population.GetValueOrDefault(0)) * 100).ToString("N0") + "% Peasants)</li>"); break;
                }
                switch (getCB.Building_Effectiveness.GetValueOrDefault(0))
                {
                    case 0: break;
                    default:
                        sb.Append("<li>Building Efficiency: " + getCB.Building_Effectiveness.Value + "%</li>"); break;
                }
                if (getCB.Trade_Balance.GetValueOrDefault(0) > 0)
                    sb.Append("<li>Trade Balance: " + getCB.Trade_Balance.Value.ToString("N0") + "gc</li>");

                switch (getCB.Networth.GetValueOrDefault(0))
                {
                    case 0: break;
                    default:
                        sb.Append("<li>Networth: " + getCB.Networth.Value.ToString("N0") + "gc (" + (getCB.Networth.Value / getCB.Land.GetValueOrDefault(1)).ToString("N0") + "gc per acre)</li>"); break;
                }
                sb.Append("<li><br/></li>");
                //if (getCB.Military_Efficiency_Off.HasValue & getCB.Military_Efficiency_Def.HasValue) { sb.Append("<li>Military Eff. with Stance: " + getCB.Military_Efficiency_Off.Value + "% off / " + getCB.Military_Efficiency_Def.Value + "% def</li>"); }
                //else if (getCB.Military_Efficiency_Off.HasValue) { sb.Append("<li>Military Eff. with Stance: " + getCB.Military_Efficiency_Off.Value + "% off"); }
                //else if (getCB.Military_Efficiency_Def.HasValue) { sb.Append("<li>Military Eff. with Stance: " + getCB.Military_Efficiency_Def.Value + "% def</li>"); }
                switch (getCB.Soldiers.GetValueOrDefault(0))
                {
                    case 0: break;
                    default:
                        sb.Append("<li>Soldiers: " + getCB.Soldiers.Value.ToString("N0") + " (" + getCB.Draft.GetValueOrDefault(0).ToString("P1") + " estimated draft rate)</li>"); break;
                }
                sb.Append("<li>" + raceInfo.soldierOffName + ": " + getCB.Soldiers_Regs_Off.GetValueOrDefault(0).ToString("N0") + " (" + (raceInfo.soldierOffMultiplier * getCB.Soldiers_Regs_Off.GetValueOrDefault(0)).ToString("N0") + " offense)</li>");
                sb.Append("<li>" + raceInfo.soldierDefName + ": " + getCB.Soldiers_Regs_Def.GetValueOrDefault(0).ToString("N0") + " (" + (raceInfo.soldierDefMultiplier * getCB.Soldiers_Regs_Def.GetValueOrDefault(0)).ToString("N0") + " defense)</li>");
                sb.Append("<li>" + raceInfo.eliteName + ": " + getCB.Soldiers_Elites.GetValueOrDefault(0).ToString("N0") + " (" + (raceInfo.eliteOffMulitplier * getCB.Soldiers_Elites.GetValueOrDefault(0)).ToString("N0") + " offense / " + (raceInfo.eliteDefMulitplier * getCB.Soldiers_Elites.GetValueOrDefault(0)).ToString("N0") + " defense)</li>");
                switch (getCB.War_Horses.HasValue)
                {
                    case true:
                        sb.Append("<li>War Horses: " + getCB.War_Horses.Value.ToString("N0") + "</li>"); break;
                }
                switch (getCB.Prisoners.GetValueOrDefault(0))
                {
                    case 0: break;
                    default:
                        sb.Append("<li>Prisoners: " + getCB.Prisoners.Value.ToString("N0") + "</li>"); break;
                }
                sb.Append("<li><br/></li>");


                switch (getCB.Total_Mod_Offense.HasValue)
                {
                    case true:
                        sb.Append("<li>Total Modified Offense: " + getCB.Total_Mod_Offense.Value.ToString("N0") + " (" + (getCB.Total_Mod_Offense.Value / getCB.Land.GetValueOrDefault(1)).ToString("N0") + " per acre)</li>");
                        break;
                }
                if (getCB.Total_Prac_Offense.HasValue && getCB.Soldiers_Elites.HasValue)
                {
                    sb.Append("<li title=''>Practical Offense (100% elites): " + getCB.Total_Prac_Offense.Value.ToString("N0") + " (" + (getCB.Total_Prac_Offense.Value / getCB.Land.GetValueOrDefault(1)).ToString("N0") + " per acre)</li>");
                }
                switch (getCB.Total_Mod_Defense.HasValue)
                {
                    case true:
                        sb.Append("<li>Total Modified Defense: " + getCB.Total_Mod_Defense.Value.ToString("N0") + " (" + (getCB.Total_Mod_Defense.Value / getCB.Land.GetValueOrDefault(1)).ToString("N0") + " per acre)</li>");
                        break;
                }
                if (getCB.Total_Prac_Defense.HasValue && getCB.Soldiers_Elites.HasValue)
                {
                    sb.Append("<li title=''>Practical Defense (0% elites): " + getCB.Total_Prac_Defense.Value.ToString("N0") + " (" + (getCB.Total_Prac_Defense.Value / getCB.Land.GetValueOrDefault(1)).ToString("N0") + " per acre)</li>");
                }

                switch (getCB.Thieves.HasValue)
                {
                    case true:
                        sb.Append("<li>Thieves: " + getCB.Thieves.Value.ToString("N0") + " (" + ((decimal)getCB.Thieves.Value / (decimal)getCB.Land.Value).ToString("N2") + " per Acre) ");
                        switch (getCB.Thieves_Value_Type.GetValueOrDefault())
                        {
                            case 1:
                                sb.Append(" (from CB)");
                                break;
                            case 2:
                                sb.Append(" (from Infiltration)");
                                break;
                            case 3:
                            case 4:
                            default:
                                sb.Append(" (Guess from Angel/Raw Page)");
                                break;
                        }
                        sb.Append("</li>");
                        break;
                }
                switch (getCB.Wizards.HasValue)
                {
                    case true:
                        sb.Append("<li>Wizards: " + getCB.Wizards.Value.ToString("N0") + " (" + ((decimal)getCB.Wizards.Value / (decimal)getCB.Land.Value).ToString("N2") + " per Acre) ");
                        switch (getCB.Thieves_Value_Type.GetValueOrDefault())
                        {
                            case 1:
                            case 2:
                                sb.Append(" (from CB)");
                                break;
                            case 3:
                            case 4:
                            default:
                                sb.Append(" (Guess from Angel/Raw Page)");
                                break;
                        }
                        sb.Append("</li>");
                        break;
                }
                sb.Append("<li><br/></li>");
                sb.Append("<li><span id=\"export_CB\" class=\"expLne\" onclick=\"cexport(this);\">" + getCB.CB_Export_Line + "</span></li>");

                sb.Append("</ul>");
                return sb.ToString();
            }
        }
        return string.Empty;
    }
    /// <summary>
    /// Builds an SOM for the History Page
    /// </summary>
    /// <param name="db"></param>
    /// <param name="provinceID"></param>
    /// <param name="update"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string PopulateSOM(Guid provinceID, int uid)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        var ProvinceSOMOrig = (from UPDCG in db.Utopia_Province_Data_Captured_Gens
                               where UPDCG.Province_ID == provinceID
                               join UPI1 in db.Utopia_Province_Data_Captured_Gens on UPDCG.SOM_Updated_By_Province_ID equals UPI1.Province_ID
                               select new
                               {
                                   UPDCG.Kingdom_ID,
                                   ProvinceName = UPDCG.Province_Name,
                                   Island = UPDCG.Kingdom_Island,
                                   Location = UPDCG.Kingdom_Location,
                                   Non_Peasents = UPDCG.Peasents_Non_Percentage / 100,
                                   MEOff = UPDCG.Military_Efficiency_Off,
                                   MEDef = UPDCG.Military_Efficiency_Def,
                                   ModOff = UPDCG.Military_Net_Off,
                                   ModDef = UPDCG.Military_Net_Def,
                                   MilEff = UPDCG.Mil_Overall_Efficiency,
                                   Updated_DateTime = UPDCG.SOM_Updated_By_DateTime,
                                   SOM_Updated_By_Province_ID = UPI1.Province_Name
                               }).FirstOrDefault();
        StringBuilder sb = new StringBuilder();
        if (ProvinceSOMOrig != null)
        {
            sb.Append("<ul class=\"ulProvinceDetails\">");

            sb.Append("<li>Military Intelligence on " + ProvinceSOMOrig.ProvinceName + " (" + ProvinceSOMOrig.Island + ":" + ProvinceSOMOrig.Location + ")</li>");
            sb.Append("<li>[http://www.utopiapimp.com]</li>");
            sb.Append("<li><br/></li>");
            sb.Append("<li>[Posted By: " + ProvinceSOMOrig.SOM_Updated_By_Province_ID + "]</li>");
            sb.Append("<li>On RL Date: RLDATE</li>");
            sb.Append("<li><br/></li>");
            sb.Append("<li>** Summary **</li>");
            if (ProvinceSOMOrig.Non_Peasents.GetValueOrDefault(0) > 0)
                sb.Append("<li>Non-Peasants: " + ProvinceSOMOrig.Non_Peasents.Value.ToString("P1") + "</li>");
            if (ProvinceSOMOrig.MEDef.HasValue | ProvinceSOMOrig.MEOff.HasValue | ProvinceSOMOrig.MilEff.HasValue)
            {
                sb.Append("<li>Efficiency (SOM): ");
                if (ProvinceSOMOrig.MEOff.GetValueOrDefault(0) > 0)
                    sb.Append(ProvinceSOMOrig.MEOff.Value.ToString("N1") + "% off, ");
                if (ProvinceSOMOrig.MEDef.GetValueOrDefault(0) > 0)
                    sb.Append(ProvinceSOMOrig.MEDef.Value.ToString("N1") + "% def, ");
                if (ProvinceSOMOrig.MilEff.GetValueOrDefault(0) > 0)
                    sb.Append(ProvinceSOMOrig.MilEff.Value.ToString("N1") + "% raw");
                sb.Append("</li>");
            }

            if (ProvinceSOMOrig.ModOff.GetValueOrDefault(0) > 0)
                sb.Append("<li>Net Offense at Home (from Utopia): " + ProvinceSOMOrig.ModOff.Value.ToString("N0") + "</li>");
            if (ProvinceSOMOrig.ModDef.GetValueOrDefault(0) > 0)
                sb.Append("<li>Net Defense at Home (from Utopia): " + ProvinceSOMOrig.ModDef.Value.ToString("N0") + "</li>");

            sb.Append("</ul>");

            var ProvinceSOM = (from uu in db.Utopia_Province_Data_Captured_Type_Militaries
                               join UPDCG in db.Utopia_Province_Data_Captured_Gens on uu.Province_ID equals UPDCG.Province_ID
                               join UPI1 in db.Utopia_Province_Data_Captured_Gens on uu.Province_ID_Added equals UPI1.Province_ID
                               join UPRMN in db.Utopia_Province_Race_Military_Names on UPDCG.Race_ID equals UPRMN.Race_ID
                               where uu.DateTime_Added == (from u in db.Utopia_Province_Data_Captured_Type_Militaries //There can be multiple queires where datatimeis equal...
                                                           where u.uid == uid
                                                           where u.Province_ID == provinceID
                                                           orderby u.uid descending
                                                           select u.DateTime_Added).FirstOrDefault()
                               select new
                               {
                                   OffName = UPRMN.Soldier_Off_Name,
                                   DefName = UPRMN.Soldier_Def_Name,
                                   EliteName = UPRMN.Elite_Name,
                                   eliteOffMulit = UPRMN.Elite_Off_Multiplier,
                                   eliteDefMulit = UPRMN.Elite_Def_Multiplier,
                                   uu.Military_Location,
                                   uu.Soldiers,
                                   soldiersOffPts = uu.Soldiers * ProvinceSOMOrig.MEOff.GetValueOrDefault(1) / 100,
                                   soldiersDefPts = uu.Soldiers * ProvinceSOMOrig.MEDef.GetValueOrDefault(1) / 100,
                                   uu.Regs_Off,
                                   offensePoints = uu.Regs_Off * 5 * ProvinceSOMOrig.MEOff.GetValueOrDefault(1) / 100,
                                   uu.Regs_Def,
                                   defensePoints = uu.Regs_Def * 5 * ProvinceSOMOrig.MEDef.GetValueOrDefault(1) / 100,
                                   uu.Elites,
                                   elitesOffPts = uu.Elites * UPRMN.Elite_Off_Multiplier * ProvinceSOMOrig.MEOff.GetValueOrDefault(1) / 100,
                                   elitesDefPts = uu.Elites * UPRMN.Elite_Def_Multiplier * ProvinceSOMOrig.MEDef.GetValueOrDefault(1) / 100,
                                   uu.Horses,
                                   horsePts = uu.Horses * ProvinceSOMOrig.MEOff.GetValueOrDefault(1) / 100,
                                   uu.Generals,
                                   land = uu.CapturedLand,
                                   uu.Export_Line,
                                   uu.Time_To_Return,
                                   uu.DateTime_Added,
                                   uu.Owner_Kingdom_ID,
                                   uu.Elites_Train,
                                   uu.Elites_Train_Queue,
                                   uu.Regs_Def_Train,
                                   uu.Regs_Def_Train_Queue,
                                   uu.Regs_Off_Train,
                                   uu.Regs_Off_Train_Queue,
                                   uu.Soldiers_Train,
                                   uu.Soldiers_Train_Queue,
                                   uu.Thieves_Train,
                                   uu.Thieves_Train_Queue,
                                   UpdateProvinceName = UPI1.Province_Name
                               });
            if (ProvinceSOM != null)
            {
                var ProvinceSOMHome = ProvinceSOM.Where(t => t.Military_Location == 1).FirstOrDefault();
                if (ProvinceSOMHome != null)
                {
                    sb.Replace("RLDATE", ProvinceSOMHome.DateTime_Added.ToString());
                    sb.Append("<ul class=\"ulProvinceDetails\">");
                    sb.Append("<li>** Standing Army (At Home) **</li>");
                    int attackPnts = 0;
                    int defPoints = 0;
                    if (ProvinceSOMHome.Soldiers.GetValueOrDefault(0) > 0)
                    {
                        attackPnts += (int)ProvinceSOMHome.soldiersOffPts.GetValueOrDefault(0);
                        defPoints += (int)ProvinceSOMHome.soldiersDefPts.GetValueOrDefault(0);
                        sb.Append("<li>Soldiers: " + ProvinceSOMHome.Soldiers.Value.ToString("N0") + " (" + ProvinceSOMHome.soldiersOffPts.GetValueOrDefault(0).ToString("N0") + " offense / " + ProvinceSOMHome.soldiersDefPts.GetValueOrDefault(0).ToString("N0") + " defense) </li>");
                    }
                    if (ProvinceSOMHome.Regs_Off.GetValueOrDefault(0) > 0)
                    {
                        sb.Append("<li>" + ProvinceSOMHome.OffName + ": " + ProvinceSOMHome.Regs_Off.GetValueOrDefault(0).ToString("N0") + " (" + ProvinceSOMHome.offensePoints.GetValueOrDefault(0).ToString("N0") + " offense) </li>");
                        attackPnts += (int)ProvinceSOMHome.offensePoints.GetValueOrDefault(0);
                    }
                    if (ProvinceSOMHome.Regs_Def.GetValueOrDefault(0) > 0)
                    {
                        sb.Append("<li>" + ProvinceSOMHome.DefName + ": " + ProvinceSOMHome.Regs_Def.Value.ToString("N0") + " (" + ProvinceSOMHome.defensePoints.GetValueOrDefault(0).ToString("N0") + " defense) </li>");
                        defPoints += (int)ProvinceSOMHome.defensePoints.GetValueOrDefault(0);
                    }
                    if (ProvinceSOMHome.Elites.GetValueOrDefault(0) > 0)
                    {
                        sb.Append("<li>" + ProvinceSOMHome.EliteName + ": " + ProvinceSOMHome.Elites.GetValueOrDefault(0).ToString("N0") + " (" + ProvinceSOMHome.elitesOffPts.GetValueOrDefault(0).ToString("N0") + " offense / " + ProvinceSOMHome.elitesDefPts.GetValueOrDefault(0).ToString("N0") + " defense)</li>");
                        attackPnts += (int)ProvinceSOMHome.elitesOffPts.GetValueOrDefault(0);
                        defPoints += (int)ProvinceSOMHome.elitesDefPts.GetValueOrDefault(0);
                    }
                    if (ProvinceSOMHome.Horses.GetValueOrDefault(0) > 0)
                    {
                        sb.Append("<li>War-Horses: " + ProvinceSOMHome.Horses.GetValueOrDefault(0).ToString("N0") + " (up to " + ProvinceSOMHome.horsePts.GetValueOrDefault(0).ToString("N0") + " additional offense)</li>");
                        attackPnts += (int)ProvinceSOMHome.horsePts.GetValueOrDefault(0);
                    }
                    sb.Append("<li>Total Attack Points: " + attackPnts.ToString("N0") + "</li>");
                    sb.Append("<li>Total Defense Points: " + defPoints.ToString("N0") + "</li>");
                    sb.Append("</ul>");
                }

                var ProvinceSOM2 = ProvinceSOM.Where(t => t.Military_Location == 2);
                if (ProvinceSOM2 != null)
                {
                    foreach (var item in ProvinceSOM2)
                    {
                        sb.Append("<ul class=\"ulProvinceDetails\">");
                        sb.Append("<li>** Armies Away ");
                        if (item.Time_To_Return.HasValue)
                            sb.Append("(Back in " + item.Time_To_Return.Value.Subtract(DateTime.UtcNow).Hours + ":" + item.Time_To_Return.Value.Subtract(DateTime.UtcNow).Minutes + " Hours) **</li>");
                        else
                            sb.Append("**</li>");

                        int attackPntss = 0;
                        int defPointss = 0;
                        if (item.Soldiers.GetValueOrDefault(0) > 0)
                        {
                            attackPntss += (int)item.soldiersOffPts.GetValueOrDefault(0);
                            defPointss += (int)item.soldiersDefPts.GetValueOrDefault(0);
                            sb.Append("<li>Soldiers: " + item.Soldiers.GetValueOrDefault(0).ToString("N0") + " (" + item.soldiersOffPts.GetValueOrDefault(0).ToString("N0") + " offense / " + item.soldiersDefPts.GetValueOrDefault(0).ToString("N0") + " defense) </li>");
                        }
                        if (item.Regs_Off.GetValueOrDefault(0) > 0)
                        {
                            sb.Append("<li>" + item.OffName + ": " + item.Regs_Off.GetValueOrDefault(0).ToString("N0") + " (" + item.offensePoints.GetValueOrDefault(0).ToString("N0") + " offense) </li>");
                            attackPntss += (int)item.offensePoints.GetValueOrDefault(0);
                        }
                        if (item.Regs_Def.GetValueOrDefault(0) > 0)
                        {
                            sb.Append("<li>" + item.DefName + ": " + item.Regs_Def.GetValueOrDefault(0).ToString("N0") + " (" + item.defensePoints.GetValueOrDefault(0).ToString("N0") + " defense) </li>");
                            defPointss += (int)item.defensePoints.GetValueOrDefault(0);
                        }
                        if (item.Elites.GetValueOrDefault(0) > 0)
                        {
                            sb.Append("<li>" + item.EliteName + ": " + item.Elites.Value.ToString("N0") + " (" + item.elitesOffPts.GetValueOrDefault(0).ToString("N0") + " offense / " + item.elitesDefPts.GetValueOrDefault(0).ToString("N0") + " defense)</li>");
                            attackPntss += (int)item.elitesOffPts.GetValueOrDefault(0);
                            defPointss += (int)item.elitesDefPts.GetValueOrDefault(0);
                        }
                        if (item.Horses.GetValueOrDefault(0) > 0)
                        {
                            sb.Append("<li>War-Horses: " + item.Horses.GetValueOrDefault(0).ToString("N0") + " (up to " + item.horsePts.GetValueOrDefault(0).ToString("N0") + " additional offense)</li>");
                            attackPntss += (int)item.horsePts.GetValueOrDefault(0);
                        }
                        sb.Append("<li>Total Attack Points: " + attackPntss.ToString("N0") + "</li>");
                        sb.Append("<li>Total Defense Points: " + defPointss.ToString("N0") + "</li>");
                        sb.Append("<li>Captured Land: " + item.land.GetValueOrDefault(0).ToString("N0") + "</li>");
                        if (item.Export_Line != null)
                        {
                            sb.Append("<li><br /></li>");
                            sb.Append("<li>" + item.Export_Line + "</li>");
                        }
                        sb.Append("</ul>");
                    }
                }
                sb.Append("<ul class=\"ulProvinceDetails\">");
                sb.Append("<li>** Troops in Training **</li>");
                if (ProvinceSOMHome.Regs_Off_Train.GetValueOrDefault(0) > 0)
                {
                    sb.Append("<li>" + ProvinceSOMHome.OffName + ": " + ProvinceSOMHome.Regs_Off_Train.Value + "</li>");
                    sb.Append("<li>" + ProvinceSOMHome.Regs_Off_Train_Queue + "</li>");
                }
                if (ProvinceSOMHome.Regs_Def_Train.GetValueOrDefault(0) > 0)
                {
                    sb.Append("<li>" + ProvinceSOMHome.DefName + ": " + ProvinceSOMHome.Regs_Def_Train.Value + "</li>");
                    sb.Append("<li>" + ProvinceSOMHome.Regs_Def_Train_Queue + "</li>");
                }
                if (ProvinceSOMHome.Elites_Train.GetValueOrDefault(0) > 0)
                {
                    sb.Append("<li>" + ProvinceSOMHome.EliteName + ": " + ProvinceSOMHome.Elites_Train.Value + "</li>");
                    sb.Append("<li>" + ProvinceSOMHome.Elites_Train_Queue + "</li>");
                }
                if (ProvinceSOMHome.Thieves_Train.GetValueOrDefault(0) > 0)
                {
                    sb.Append("<li>Thieves: " + ProvinceSOMHome.Thieves_Train.Value + "</li>");
                    sb.Append("<li>" + ProvinceSOMHome.Thieves_Train_Queue + "</li>");
                }
                //                ** Troops in Training **
                //Trolls: 110 (643 defense)
                //Next 21 hours: 9 6 6 4 7 5 4 6 3 3 6 6 4 7 5 5 7 4 6 3 1...
                //Ogres: 464 (4,888 offense / 1,627 defense)
                //Next 12 hours: 50 51 51 49 52 26 23 14 12 14 12 13...
                //Thieves: 146
                //Next 20 hours: 7 7 5 7 8 8 11 7 7 6 5 7 7 8 8 7 8 7 6 6...
                //*/
                sb.Append("</ul>");
                return sb.ToString();
            }
            return sb.ToString();
        }
        else
            return string.Empty;
    }
    /// <summary>
    /// Builds an Survey for the History Page
    /// </summary>
    /// <param name="db"></param>
    /// <param name="provinceID"></param>
    /// <param name="update"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string PopulateSurvey(Guid provinceID, int uid)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        var ProvinceSurvey1 = (from u in db.Utopia_Province_Data_Captured_Surveys
                               where u.Province_ID == provinceID
                               where u.uid == uid
                               join UPI1 in db.Utopia_Province_Data_Captured_Gens on u.Province_ID_Updated_By equals UPI1.Province_ID
                               join UPDCG in db.Utopia_Province_Data_Captured_Gens on u.Province_ID equals UPDCG.Province_ID
                               select new
                               {
                                   ProvinceName = UPDCG.Province_Name,
                                   Island = UPDCG.Kingdom_Island,
                                   Location = UPDCG.Kingdom_Location,
                                   u.Building_Efficiency,
                                   u.Homes_B,
                                   u.Homes_E,
                                   u.Homes_P,
                                   u.Farms_B,
                                   u.Farms_E,
                                   u.Farms_P,
                                   u.Mills_B,
                                   u.Mills_E,
                                   u.Mills_P,
                                   u.Banks_B,
                                   u.Banks_E,
                                   u.Banks_P,
                                   u.TG_B,
                                   u.TG_E,
                                   u.TG_P,
                                   u.Armories_B,
                                   u.Armories_E,
                                   u.Armories_P,
                                   u.Barracks_B,
                                   u.Barracks_E,
                                   u.Barracks_P,
                                   u.Forts_B,
                                   u.Forts_E,
                                   u.Forts_P,
                                   u.GS_B,
                                   u.GS_E,
                                   u.GS_P,
                                   u.Hospitals_B,
                                   u.Hostpitals_E,
                                   u.Hostpitals_P,
                                   u.Guilds_B,
                                   u.Guilds_E,
                                   u.Guilds_P,
                                   u.Towers_B,
                                   u.Towers_E,
                                   u.Towers_P,
                                   u.TD_B,
                                   u.TD_E,
                                   u.TD_P,
                                   u.WT_B,
                                   u.WT_E,
                                   u.WT_P,
                                   u.Library_B,
                                   u.Library_E,
                                   u.Library_P,
                                   u.Schools_B,
                                   u.Schools_E,
                                   u.Schools_P,
                                   u.Stables_B,
                                   u.Stables_E,
                                   u.Stables_P,
                                   u.Dungeons_B,
                                   u.Dungeons_P,
                                   u.Dungeons_E,
                                   u.Dens_B,
                                   u.Dens_E,
                                   u.Dens_P,
                                   u.Acres_In_Progress,
                                   u.Export_Line,
                                   u.Owner_Kingdom_ID,
                                   u.DateTime_Updated,
                                   UpdateProvinceName = UPI1.Province_Name,
                                   UPDCG.Land,
                                   unBuiltLand = UPDCG.Land - u.Acres_In_Progress
                               }).FirstOrDefault();

        if (ProvinceSurvey1 == null)
            return string.Empty;
        else
        {
            int built = 0;
            int progress = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"ulProvinceDetails\">");

            sb.Append("<li>Survey on " + ProvinceSurvey1.ProvinceName + " (" + ProvinceSurvey1.Island + ":" + ProvinceSurvey1.Location + ")</li>");
            sb.Append("<li>[http://www.utopiapimp.com]</li>");
            sb.Append("<li><br/></li>");
            sb.Append("<li>[Posted By:" + ProvinceSurvey1.UpdateProvinceName + "]</li>");
            sb.Append("<li>Utopian Date: " + UtopiaParser.getUtopiaDateTime(ProvinceSurvey1.DateTime_Updated) + "</li>");
            sb.Append("<li>Relative Date: " + ProvinceSurvey1.DateTime_Updated.ToLongRelativeDate() + "</li>");
            sb.Append("<li>Real Life Date: " + ProvinceSurvey1.DateTime_Updated + "</li>");
            sb.Append("<li><br/></li>");
            sb.Append("<li>** Buildings summary **</li>");

            switch (ProvinceSurvey1.Homes_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (ProvinceSurvey1.Homes_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Homes_P.Value;
                            sb.Append("<li>Homes: " + ProvinceSurvey1.Homes_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Homes_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += ProvinceSurvey1.Homes_B.Value;
                    sb.Append("<li>Homes: " + ProvinceSurvey1.Homes_B.Value.ToString("N0") + " (" + ((decimal)ProvinceSurvey1.Homes_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)");
                    switch (ProvinceSurvey1.Homes_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Homes_P.Value;
                            sb.Append(" + " + ProvinceSurvey1.Homes_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Homes_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (ProvinceSurvey1.Farms_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (ProvinceSurvey1.Farms_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Farms_P.Value;
                            sb.Append("<li>Farms: " + ProvinceSurvey1.Farms_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Farms_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += ProvinceSurvey1.Farms_B.Value;
                    sb.Append("<li>Farms: " + ProvinceSurvey1.Farms_B.Value.ToString("N0") + " (" + ((decimal)ProvinceSurvey1.Farms_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)");
                    switch (ProvinceSurvey1.Farms_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Farms_P.Value;
                            sb.Append(" + " + ProvinceSurvey1.Farms_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Farms_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (ProvinceSurvey1.Mills_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (ProvinceSurvey1.Mills_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Mills_P.Value;
                            sb.Append("<li>Mills: " + ProvinceSurvey1.Mills_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Mills_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += ProvinceSurvey1.Mills_B.Value;
                    sb.Append("<li>Mills: " + ProvinceSurvey1.Mills_B.Value.ToString("N0") + " (" + ((decimal)ProvinceSurvey1.Mills_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)");
                    switch (ProvinceSurvey1.Mills_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Mills_P.Value;
                            sb.Append(" + " + ProvinceSurvey1.Mills_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Mills_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (ProvinceSurvey1.Banks_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (ProvinceSurvey1.Banks_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Banks_P.Value;
                            sb.Append("<li>Banks: " + ProvinceSurvey1.Banks_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Banks_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += ProvinceSurvey1.Banks_B.Value;
                    sb.Append("<li>Banks: " + ProvinceSurvey1.Banks_B.Value.ToString("N0") + " (" + ((decimal)ProvinceSurvey1.Banks_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)");
                    switch (ProvinceSurvey1.Banks_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Banks_P.Value;
                            sb.Append(" + " + ProvinceSurvey1.Banks_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Banks_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (ProvinceSurvey1.TG_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (ProvinceSurvey1.TG_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.TG_P.Value;
                            sb.Append("<li>Training Grounds: " + ProvinceSurvey1.TG_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.TG_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += ProvinceSurvey1.TG_B.Value;
                    sb.Append("<li>Training Grounds: " + ProvinceSurvey1.TG_B.Value.ToString("N0") + " (" + ((decimal)ProvinceSurvey1.TG_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)");
                    switch (ProvinceSurvey1.TG_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.TG_P.Value;
                            sb.Append(" + " + ProvinceSurvey1.TG_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.TG_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (ProvinceSurvey1.Armories_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (ProvinceSurvey1.Armories_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Armories_P.Value;
                            sb.Append("<li>Armouries: " + ProvinceSurvey1.Armories_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Armories_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += ProvinceSurvey1.Armories_B.Value;
                    sb.Append("<li>Armouries: " + ProvinceSurvey1.Armories_B.Value.ToString("N0") + " (" + ((decimal)ProvinceSurvey1.Armories_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)");
                    switch (ProvinceSurvey1.Armories_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Armories_P.Value;
                            sb.Append(" + " + ProvinceSurvey1.Armories_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Armories_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (ProvinceSurvey1.Barracks_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (ProvinceSurvey1.Barracks_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Barracks_P.Value;
                            sb.Append("<li>Barracks: " + ProvinceSurvey1.Barracks_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Barracks_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += ProvinceSurvey1.Barracks_B.Value;
                    sb.Append("<li>Barracks: " + ProvinceSurvey1.Barracks_B.Value.ToString("N0") + " (" + ((decimal)ProvinceSurvey1.Barracks_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)");
                    switch (ProvinceSurvey1.Barracks_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Barracks_P.Value;
                            sb.Append(" + " + ProvinceSurvey1.Barracks_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Barracks_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (ProvinceSurvey1.Forts_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (ProvinceSurvey1.Forts_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Forts_P.Value;
                            sb.Append("<li>Forts: " + ProvinceSurvey1.Forts_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Forts_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += ProvinceSurvey1.Forts_B.Value;
                    sb.Append("<li>Forts: " + ProvinceSurvey1.Forts_B.Value.ToString("N0") + " (" + ((decimal)ProvinceSurvey1.Forts_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)");
                    switch (ProvinceSurvey1.Forts_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Forts_P.Value;
                            sb.Append(" + " + ProvinceSurvey1.Forts_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Forts_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (ProvinceSurvey1.GS_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (ProvinceSurvey1.GS_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.GS_P.Value;
                            sb.Append("<li>Guard Stations: " + ProvinceSurvey1.GS_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.GS_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += ProvinceSurvey1.GS_B.Value;
                    sb.Append("<li>Guard Stations: " + ProvinceSurvey1.GS_B.Value.ToString("N0") + " (" + ((decimal)ProvinceSurvey1.GS_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)");
                    switch (ProvinceSurvey1.GS_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.GS_P.Value;
                            sb.Append(" + " + ProvinceSurvey1.GS_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.GS_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (ProvinceSurvey1.Hospitals_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (ProvinceSurvey1.Hostpitals_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Hostpitals_P.Value;
                            sb.Append("<li>Hostpitals: " + ProvinceSurvey1.Hostpitals_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Hostpitals_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += ProvinceSurvey1.Hospitals_B.Value;
                    sb.Append("<li>Hospitals: " + ProvinceSurvey1.Hospitals_B.Value.ToString("N0") + " (" + ((decimal)ProvinceSurvey1.Hospitals_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)");
                    switch (ProvinceSurvey1.Hostpitals_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Hostpitals_P.Value;
                            sb.Append(" + " + ProvinceSurvey1.Hostpitals_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Hostpitals_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (ProvinceSurvey1.Guilds_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (ProvinceSurvey1.Guilds_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Guilds_P.Value;
                            sb.Append("<li>Guilds: " + ProvinceSurvey1.Guilds_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Guilds_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += ProvinceSurvey1.Guilds_B.Value;
                    sb.Append("<li>Guilds: " + ProvinceSurvey1.Guilds_B.Value.ToString("N0") + " (" + ((decimal)ProvinceSurvey1.Guilds_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)");
                    switch (ProvinceSurvey1.Guilds_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Guilds_P.Value;
                            sb.Append(" + " + ProvinceSurvey1.Guilds_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Guilds_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (ProvinceSurvey1.Towers_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (ProvinceSurvey1.Towers_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Towers_P.Value;
                            sb.Append("<li>Towers: " + ProvinceSurvey1.Towers_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Towers_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += ProvinceSurvey1.Towers_B.Value;
                    sb.Append("<li>Towers: " + ProvinceSurvey1.Towers_B.Value.ToString("N0") + " (" + ((decimal)ProvinceSurvey1.Towers_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)");
                    switch (ProvinceSurvey1.Towers_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Towers_P.Value;
                            sb.Append(" + " + ProvinceSurvey1.Towers_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Towers_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (ProvinceSurvey1.TD_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (ProvinceSurvey1.TD_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.TD_P.Value;
                            sb.Append("<li>Thieves' Dens: " + ProvinceSurvey1.TD_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.TD_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += ProvinceSurvey1.TD_B.Value;
                    sb.Append("<li>Thieve Dens: " + ProvinceSurvey1.TD_B.Value.ToString("N0") + " (" + ((decimal)ProvinceSurvey1.TD_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)");
                    switch (ProvinceSurvey1.TD_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.TD_P.Value;
                            sb.Append(" + " + ProvinceSurvey1.TD_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.TD_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (ProvinceSurvey1.WT_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (ProvinceSurvey1.WT_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.WT_P.Value;
                            sb.Append("<li>Watch Towers: " + ProvinceSurvey1.WT_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.WT_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += ProvinceSurvey1.WT_B.Value;
                    sb.Append("<li>Watch Towers: " + ProvinceSurvey1.WT_B.Value.ToString("N0") + " (" + ((decimal)ProvinceSurvey1.WT_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)");
                    switch (ProvinceSurvey1.WT_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.WT_P.Value;
                            sb.Append(" + " + ProvinceSurvey1.WT_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.WT_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (ProvinceSurvey1.Library_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (ProvinceSurvey1.Library_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Library_P.Value;
                            sb.Append("<li>Libraries: " + ProvinceSurvey1.Library_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Library_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += ProvinceSurvey1.Library_B.Value;
                    sb.Append("<li>Libraries: " + ProvinceSurvey1.Library_B.Value.ToString("N0") + " (" + ((decimal)ProvinceSurvey1.Library_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)");
                    switch (ProvinceSurvey1.Library_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Library_P.Value;
                            sb.Append(" + " + ProvinceSurvey1.Library_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Library_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (ProvinceSurvey1.Schools_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (ProvinceSurvey1.Schools_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Schools_P.Value;
                            sb.Append("<li>Schools: " + ProvinceSurvey1.Schools_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Schools_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += ProvinceSurvey1.Schools_B.Value;
                    sb.Append("<li>Schools: " + ProvinceSurvey1.Schools_B.Value.ToString("N0") + " (" + ((decimal)ProvinceSurvey1.Schools_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)");
                    switch (ProvinceSurvey1.Schools_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Schools_P.Value;
                            sb.Append(" + " + ProvinceSurvey1.Schools_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Schools_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (ProvinceSurvey1.Stables_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (ProvinceSurvey1.Stables_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Stables_P.Value;
                            sb.Append("<li>Stables: " + ProvinceSurvey1.Stables_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Stables_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += ProvinceSurvey1.Stables_B.Value;
                    sb.Append("<li>Stables: " + ProvinceSurvey1.Stables_B.Value.ToString("N0") + " (" + ((decimal)ProvinceSurvey1.Stables_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)");
                    switch (ProvinceSurvey1.Stables_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Stables_P.Value;
                            sb.Append(" + " + ProvinceSurvey1.Stables_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Stables_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (ProvinceSurvey1.Dungeons_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (ProvinceSurvey1.Dungeons_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Dungeons_P.Value;
                            sb.Append("<li>Dungeons: " + ProvinceSurvey1.Dungeons_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Dungeons_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += ProvinceSurvey1.Dungeons_B.Value;
                    sb.Append("<li>Dungeons: " + ProvinceSurvey1.Dungeons_B.Value.ToString("N0") + " (" + ((decimal)ProvinceSurvey1.Dungeons_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)");
                    switch (ProvinceSurvey1.Dungeons_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Dungeons_P.Value;
                            sb.Append(" + " + ProvinceSurvey1.Dungeons_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Dungeons_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }
            switch (ProvinceSurvey1.Dens_B.GetValueOrDefault(0))
            {
                case 0:
                    switch (ProvinceSurvey1.Dens_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Dens_P.Value;
                            sb.Append("<li>Dens: " + ProvinceSurvey1.Dens_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Dens_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                    }
                    break;
                default:
                    built += ProvinceSurvey1.Dens_B.Value;
                    sb.Append("<li>Dens: " + ProvinceSurvey1.Dens_B.Value.ToString("N0") + " (" + ((decimal)ProvinceSurvey1.Dens_B.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)");
                    switch (ProvinceSurvey1.Dens_P.HasValue)
                    {
                        case true:
                            progress += ProvinceSurvey1.Dens_P.Value;
                            sb.Append(" + " + ProvinceSurvey1.Dens_P.Value.ToString("N0") + " in progress (" + ((decimal)ProvinceSurvey1.Dens_P.Value / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
                            break;
                        default:
                            sb.Append("</li>");
                            break;
                    }
                    break;
            }

            sb.Append("<li><br/></li>");
            sb.Append("<li>Total Acres Built: " + built.ToString("N0") + "</li>");
            sb.Append("<li>In Progress: " + progress.ToString("N0") + " (" + ((decimal)progress / (decimal)ProvinceSurvey1.Land.Value * 100).ToString("N1") + "%)</li>");
            sb.Append("<li>Total Acres: " + ProvinceSurvey1.Land.Value.ToString("N0") + "</li>");
            sb.Append("<li>Un-Built Acres: " + (ProvinceSurvey1.Land.Value - (built + progress)).ToString("N0") + "</li>");
            sb.Append("<li><br/></li>");
            sb.Append("<li>" + ProvinceSurvey1.Export_Line + "</li>");

            sb.Append("</ul>");
            return sb.ToString();
        }
    }
    /// <summary>
    /// Builds an SOS for the History Page
    /// </summary>
    /// <param name="db"></param>
    /// <param name="provinceID"></param>
    /// <param name="update"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string PopulateSOS(Guid provinceID, int uid)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        var ProvinceSOS = (from UPDCS in db.Utopia_Province_Data_Captured_Sciences
                           where UPDCS.Province_ID == provinceID
                           where UPDCS.uid == uid
                           join UPI in db.Utopia_Province_Data_Captured_Gens on UPDCS.Province_ID equals UPI.Province_ID
                           join UPI1 in db.Utopia_Province_Data_Captured_Gens on UPDCS.Province_ID_Added equals UPI1.Province_ID
                           select new
                           {
                               ProvinceName = UPI.Province_Name,
                               Island = UPI.Kingdom_Island,
                               Location = UPI.Kingdom_Location,
                               UPDCS.SOS_Alchemy,
                               UPDCS.SOS_Alchemy_Percent,
                               UPDCS.SOS_Alchemy_Prog,
                               UPDCS.SOS_Tools,
                               UPDCS.SOS_Tools_Percent,
                               UPDCS.SOS_Tools_Prog,
                               UPDCS.SOS_Housing,
                               UPDCS.SOS_Housing_Percent,
                               UPDCS.SOS_Housing_Prog,
                               UPDCS.SOS_Food,
                               UPDCS.SOS_Food_Percent,
                               UPDCS.SOS_Food_Prog,
                               UPDCS.SOS_Military,
                               UPDCS.SOS_Miltary_Percent,
                               UPDCS.SOS_Military_Prog,
                               UPDCS.SOS_Thieves,
                               UPDCS.SOS_Thieves_Percent,
                               UPDCS.SOS_Thieves_Prog,
                               UPDCS.SOS_Magic,
                               UPDCS.SOS_Magic_Percent,
                               UPDCS.SOS_Magic_Prog,
                               UPDCS.Export_Line,
                               UPDCS.DateTime_Added,
                               UpdateProvinceName = UPI1.Province_Name,
                               UPDCS.Owner_Kingdom_ID
                           }).FirstOrDefault();

        if (ProvinceSOS == null)
            return string.Empty;
        else
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"ulProvinceDetails\">");

            sb.Append("<li>Science Intelligence on " + ProvinceSOS.ProvinceName + " (" + ProvinceSOS.Island + ":" + ProvinceSOS.Location + ")</li>");
            sb.Append("<li>[http://www.utopiapimp.com]</li>");
            sb.Append("<li><br/></li>");
            sb.Append("<li>[Posted By:" + ProvinceSOS.UpdateProvinceName + "]</li>");
            sb.Append("<li>Utopian Date: " + UtopiaParser.getUtopiaDateTime(ProvinceSOS.DateTime_Added) + "</li>");
            sb.Append("<li>Relative Date: " + ProvinceSOS.DateTime_Added.ToLongRelativeDate() + "</li>");
            sb.Append("<li>Real Life Date: " + ProvinceSOS.DateTime_Added + "</li>");

            sb.Append("<li><br/></li>");
            sb.Append("<li>** Effects summary **</li>");
            if (ProvinceSOS.SOS_Alchemy_Percent.GetValueOrDefault(0) > 0)
            {
                sb.Append("<li>+" + ProvinceSOS.SOS_Alchemy_Percent.Value + "% Income (" + ProvinceSOS.SOS_Alchemy.GetValueOrDefault(0).ToString("N0") + " points");
                if (ProvinceSOS.SOS_Alchemy_Prog.HasValue)
                    sb.Append(" + " + ProvinceSOS.SOS_Alchemy_Prog.Value.ToString("N0") + " in progress)</li>");
                else
                    sb.Append(")</li>");
            }
            if (ProvinceSOS.SOS_Tools_Percent.GetValueOrDefault(0) > 0)
            {
                sb.Append("<li>+" + ProvinceSOS.SOS_Tools_Percent.Value + "% Building Effectiveness (" + ProvinceSOS.SOS_Tools.GetValueOrDefault(0).ToString("N0") + " points");
                if (ProvinceSOS.SOS_Tools_Prog.HasValue)
                    sb.Append(" + " + ProvinceSOS.SOS_Tools_Prog.Value.ToString("N0") + " in progress)</li>");
                else
                    sb.Append(")</li>");
            }
            if (ProvinceSOS.SOS_Housing_Percent.GetValueOrDefault(0) > 0)
            {
                sb.Append("<li>+" + ProvinceSOS.SOS_Housing_Percent.Value + "% Population Limits (" + ProvinceSOS.SOS_Housing.GetValueOrDefault(0).ToString("N0") + " points");
                if (ProvinceSOS.SOS_Housing_Prog.HasValue)
                    sb.Append(" + " + ProvinceSOS.SOS_Housing_Prog.Value.ToString("N0") + " in progress)</li>");
                else
                    sb.Append(")</li>");
            }
            if (ProvinceSOS.SOS_Food_Percent.GetValueOrDefault(0) > 0)
            {
                sb.Append("<li>+" + ProvinceSOS.SOS_Food_Percent.Value + "% Food Production (" + ProvinceSOS.SOS_Food.GetValueOrDefault(0).ToString("N0") + " points");
                if (ProvinceSOS.SOS_Food_Prog.HasValue)
                    sb.Append(" + " + ProvinceSOS.SOS_Food_Prog.Value.ToString("N0") + " in progress)</li>");
                else
                    sb.Append(")</li>");
            }
            if (ProvinceSOS.SOS_Miltary_Percent.GetValueOrDefault(0) > 0)
            {
                sb.Append("<li>+" + ProvinceSOS.SOS_Miltary_Percent.Value + "% Gains in Combat (" + ProvinceSOS.SOS_Military.GetValueOrDefault(0).ToString("N0") + " points");
                if (ProvinceSOS.SOS_Military_Prog.HasValue)
                    sb.Append(" + " + ProvinceSOS.SOS_Military_Prog.Value.ToString("N0") + " in progress)</li>");
                else
                    sb.Append(")</li>");
            }
            if (ProvinceSOS.SOS_Thieves_Percent.GetValueOrDefault(0) > 0)
            {
                sb.Append("<li>+" + ProvinceSOS.SOS_Thieves_Percent.Value + "% Thievery Effectiveness (" + ProvinceSOS.SOS_Thieves.GetValueOrDefault(0).ToString("N0") + " points");
                if (ProvinceSOS.SOS_Thieves_Prog.HasValue)
                    sb.Append(" + " + ProvinceSOS.SOS_Thieves_Prog.Value.ToString("N0") + " in progress)</li>");
                else
                    sb.Append(")</li>");
            }
            if (ProvinceSOS.SOS_Magic_Percent.GetValueOrDefault(0) > 0)
            {
                sb.Append("<li>+" + ProvinceSOS.SOS_Magic_Percent.Value + "% Magic Effectiveness & Rune Production (" + ProvinceSOS.SOS_Magic.GetValueOrDefault(0).ToString("N0") + " points");
                if (ProvinceSOS.SOS_Magic_Prog.HasValue)
                    sb.Append(" + " + ProvinceSOS.SOS_Magic_Prog.Value.ToString("N0") + " in progress)</li>");
                else
                    sb.Append(")</li>");
            }
            sb.Append("<li><br/></li>");
            sb.Append("<li>" + ProvinceSOS.Export_Line + "</li>");

            sb.Append("</ul>");
            return sb.ToString();
        }
    }
    public static string PopulateAttack(Guid provinceID, int uid, Guid ownerKingdomID, Guid currentUserID)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        var getAttack = (from xx in db.Utopia_Province_Data_Captured_Attacks
                         from yy in db.Utopia_Province_Data_Captured_Attack_Pulls
                         from px in db.Utopia_Province_Data_Captured_Gens
                         from py in db.Utopia_Province_Data_Captured_Gens
                         where xx.Province_ID_Added == px.Province_ID
                         where xx.Province_ID_Attacked == py.Province_ID
                         where xx.Attack_Type == yy.uid
                         where xx.uid == uid
                         where py.Owner_Kingdom_ID == ownerKingdomID
                         where px.Owner_Kingdom_ID == ownerKingdomID
                         where xx.Owner_Kingdom_ID == ownerKingdomID
                         select new
                         {
                             xx.Captured_Type_Number,
                             yy.Attack_Type_Name,
                             xx.DateTime_Added,
                             xx.Imprsoned_Troops,
                             xx.O_Elites_Died,
                             xx.O_Horses_Died,
                             xx.O_Regs_Def_Died,
                             xx.O_Regs_Off_Died,
                             xx.O_Soldiers_Died,
                             xx.Province_ID_Added,
                             xx.Province_ID_Attacked,
                             xx.Time_To_Return,
                             xx.Troops_Killed,
                             ap = px.Province_Name,
                             api = px.Kingdom_Island,
                             apl = px.Kingdom_Location,
                             tp = py.Province_Name,
                             tpi = py.Kingdom_Island,
                             tpl = py.Kingdom_Location
                         }).FirstOrDefault();
        if (getAttack == null)
            return "N/A";
        else
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"ulProvinceDetails\">");
            sb.Append("<li>Attack From " + getAttack.ap + " (" + getAttack.api + ":" + getAttack.apl + ")</li>");
            sb.Append("<li>[http://www.utopiapimp.com]</li>");
            sb.Append("<li><br/></li>");
            sb.Append("<li>** Attack summary **</li>");
            sb.Append("<li>Province Attacked: " + getAttack.tp + " (" + getAttack.tpi + ":" + getAttack.tpl + ")</li>");
            sb.Append("<li>Attack Type: " + getAttack.Attack_Type_Name + "</li>");
            switch (getAttack.Attack_Type_Name)
            {
                case "Acres":
                    sb.Append("<li>Took " + getAttack.Captured_Type_Number + " acres</li>");
                    break;
                case "razed":
                    sb.Append("<li>Burned " + getAttack.Captured_Type_Number + " acres</li>");
                    break;
                case "massacred":
                    sb.Append("<li>Massacred " + getAttack.Captured_Type_Number + " People</li>");
                    break;
                case "Looted":
                    sb.Append("<li>Looted " + getAttack.Captured_Type_Number + "gc</li>");
                    break;
                case "Knowledge":
                    sb.Append("<li>Learned " + getAttack.Captured_Type_Number.Value.ToString("N0") + " points</li>");
                    break;
                case "weakForces":
                    sb.Append("<li>Army was much too weak</li>");
                    break;
                default:
                    UtopiaParser.FailedAt("'PopulateAttackFailed'", getAttack.Attack_Type_Name + "; " + getAttack.Captured_Type_Number, currentUserID);
                    break;
            }
            sb.Append("<li><br/></li>");
            if (getAttack.O_Soldiers_Died.HasValue)
                sb.Append("<li>Soldiers Died: " + getAttack.O_Soldiers_Died.Value + "</li>");
            if (getAttack.O_Regs_Off_Died.HasValue)
                sb.Append("<li>Offense Died:" + getAttack.O_Regs_Off_Died.Value + "</li>");
            if (getAttack.O_Regs_Def_Died.HasValue)
                sb.Append("<li>Defense Died:" + getAttack.O_Regs_Def_Died.Value + "</li>");
            if (getAttack.O_Elites_Died.HasValue)
                sb.Append("<li>Elites Died:" + getAttack.O_Elites_Died.Value + "</li>");
            if (getAttack.O_Horses_Died.HasValue)
                sb.Append("<li>Horses Died:" + getAttack.O_Horses_Died.Value + "</li>");
            sb.Append("<li><br/></li>");
            if (getAttack.Imprsoned_Troops.HasValue)
                sb.Append("<li>Imprissoned Troops:" + getAttack.Imprsoned_Troops.Value + "</li>");
            if (getAttack.Troops_Killed.HasValue)
                sb.Append("<li>Troops Killed:" + getAttack.Troops_Killed.Value + "</li>");
            return sb.ToString();
        }

    }
    public static List<NotificationDetail> FetchExpiringObjects(Guid provinceID)
    {
        return new List<NotificationDetail>();
    }
}

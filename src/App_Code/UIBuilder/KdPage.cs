using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.SessionState;
using System.Web.Security;
using System.Web;
using Boomers.Utilities.DatesTimes;
using Boomers.Utilities.Guids;

using Pimp.UIBuilder;
using PimpLibrary.Static.Enums;
using Pimp.UParser;
using Pimp.Utopia;
using Pimp.UData;
using Pimp.Users;
using PimpLibrary.Utopia.Ops;
using PimpLibrary.Utopia.Province;

namespace Pimp.UIBuilder
{
    /// <summary>
    /// Summary description for KdPage
    /// </summary>
    public class KdPage
    {
        /// <summary>
        /// loads the kingdom page from scratch for utopiapimp.
        /// </summary>
        /// <param name="kingdomId"></param>
        /// <param name="type"></param>
        /// <param name="columns"></param>
        /// <param name="ownerKingdomID"></param>
        /// <param name="monType"></param>
        /// <param name="currentUserID"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static string loadDynamicGrid(Guid kingdomId, string type, string columns, Guid ownerKingdomID, MonarchType monType, Guid currentUserID, OwnedKingdomProvinces cachedKingdom)
        {

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            bool kingdomlessProvinces = false;
            decimal dragonNetworth = 0;
            decimal totalGold = 0;
            long acres = 0;
            long food = 0;
            long dailyIncome = 0;
            long peasants = 0;
            int tradeBalance = 0;
            long runes = 0;
            int prisoners = 0;
            int soldiers = 0;
            int horses = 0;
            long population = 0;
            int defSpecs = 0;
            int offSpecs = 0;

            if (columns != null)
            {
                MatchCollection mc = URegEx.rgxNumber.Matches(columns);
                if (type == "RandomFilter" || type == "Random" || type == "myaddy")
                {
                    kingdomlessProvinces = true;
                }
                List<ProvinceClass> ownedProvinces = UtopiaParser.GetProvincesInKingdomToDisplay("none", ownerKingdomID, ownerKingdomID, cachedKingdom);
                List<ProvinceClass> provinceIdentifiers = UtopiaParser.GetProvincesInKingdomToDisplay(type, kingdomId, ownerKingdomID, cachedKingdom);

                var AttackQuery = cachedKingdom.Attacks;
                var itemEffects = GetEffects(cachedKingdom.Effects);

                DateTime hfStart = DateTime.UtcNow;
                StringBuilder sb = new StringBuilder();
                StringBuilder sbFooter = new StringBuilder();
                sb.Append("<table class=\"tblKingdomInfo center\" id=\"tblKingdomInfo\">");
                sb.Append("<thead><tr>");

                if (monType != MonarchType.none && monType != MonarchType.kdMonarch) //Displays the trash can to delete provinces in the Kdless Kingdoms tabs.
                {
                    if (kingdomlessProvinces)
                    {
                        sb.Append("<th>");
                        sbFooter.Append("<td></td>");
                    }
                }

                #region Table head
                foreach (Match column in mc)
                {
                    var getCN = (from gcn in UtopiaHelper.Instance.ColumnNames
                                 where gcn.uid == Convert.ToInt32(column.Value)
                                 select gcn.columnName).FirstOrDefault();
                    //Decides how to sort the columns when the table is displayed.
                    switch (getCN)
                    {
                        case "NW":
                            sbFooter.Append("<td>NWFooter</td>");
                            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">");
                            break;
                        case "Gold":
                            sbFooter.Append("<td>GoldFooter</td>");
                            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">");
                            break;
                        case "Acres":
                            sbFooter.Append("<td>AcresFooter</td>");
                            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">");
                            break;
                        case "Food":
                            sbFooter.Append("<td>FoodFooter</td>");
                            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">");
                            break;
                        case "Runes":
                            sbFooter.Append("<td>RunesFooter</td>");
                            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">");
                            break;
                        case "Soldiers":
                            sbFooter.Append("<td>SoldiersFooter</td>");
                            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">");
                            break;
                        case "Trade Balance":
                            sbFooter.Append("<td>TradeFooter</td>");
                            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">");
                            break;
                        case "Prisoners":
                            sbFooter.Append("<td>PrisonersFooter</td>");
                            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">");
                            break;
                        case "Daily Income":
                            sbFooter.Append("<td>DailyIncomeFooter</td>");
                            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">");
                            break;
                        case "Peasants":
                            sbFooter.Append("<td>PeasantsFooter</td>");
                            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">");
                            break;
                        case "Horses":
                            sbFooter.Append("<td>HorsesFooter</td>");
                            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">");
                            break;
                        case "Population":
                            sbFooter.Append("<td>PopulationFooter</td>");
                            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">");
                            break;
                        case "Off specs":
                            sbFooter.Append("<td>OffSpecsFooter</td>");
                            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">");
                            break;
                        case "Def Specs":
                            sbFooter.Append("<td>DefSpecsFooter</td>");
                            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">");
                            break;
                        case "Mod def (dpa)":
                        case "Effects":
                        case "Mod def":
                        case "Max Mod off (opa)":
                        case "Max Mod off":
                        case "Mod def (dpa) no elites":
                        case "Mod def/NW":
                        case "Mod dpa":
                        case "Mod off":
                        case "Mod off (opa) no elites":
                        case "Mod off/NW":
                        case "Prac Mod Off (opa)":
                        case "Elites home/total":
                        case "Prac Mod Off":
                        case "Prac Mod Def":
                        case "Prac Mod Def (dpa)":
                        case "Def ME %":
                        case "Off ME %":
                        case "Mod Arms %":
                        case "Off specs/acre":
                        case "Mod Forts %":
                        case "Mod Dungeons %":
                        case "Mod GS %":
                        case "Buildings Sci %":
                        case "Military Sci %":
                        case "Mod Homes %":
                        case "Income Sci pts":
                        case "Mod Library %":
                        case "Military Sci pts":
                        case "Mod Hospitals %":
                        case "Population Sci pts":
                        case "Mod Schools %":
                        case "Building Sci pts":
                        case "Mod Stables %":
                        case "Thief Sci %":
                        case "Mod T Grounds %":
                        case "Thief Sci pts":
                        case "Mod Taverns %":
                        case "Income Sci %":
                        case "Pop Sci %":
                        case "Mod Towers %":
                        case "Mod WT %":
                        case "Mod TD %":
                        case "Mod Guilds %":
                        case "Mod Mills %":
                        case "Sci pts/acre":
                        case "Total sci":
                        case "Est TPA":
                        case "Est WPA":
                        case "Raw TPA":
                        case "Raw WPA":
                        case "Mod TPA":
                        case "Mod WPA":
                            sbFooter.Append("<td></td>");
                            sb.Append("<th class=\"{sorter: 'fancyNumber'}\">");
                            break;
                        case "CB":
                        case "N":
                        case "SoS":
                        case "SoM":
                        case "Survey":
                        case "Personality":
                        case "Rank":
                            sb.Append("<th class=\"{sorter: 'text'}\">");
                            sbFooter.Append("<td></td>");
                            break;
                        default:
                            sbFooter.Append("<td></td>");
                            sb.Append("<th>");
                            break;
                    }
                    sb.Append(getCN);
                    sb.Append("</th>");
                }
                sb.Append("</tr></thead><tbody>");
                #endregion

                bool provinceCheck, buildingCheck, scienceCheck, milCheck, attackAddedCheck, attackedCheck, cbCheck;
                int i = 0;

                foreach (var item in provinceIdentifiers)
                {
                    DateTime dbPer = DateTime.UtcNow;
                    provinceCheck = true;

                    var cb = new CS_Code.Utopia_Province_Data_Captured_CB();
                    if (item.CB != null && item.CB.LastOrDefault() != null)
                    {
                        cbCheck = true;
                        cb = item.CB.LastOrDefault();
                    }
                    else
                        cbCheck = false;

                    var getBuildings = new CS_Code.Utopia_Province_Data_Captured_Survey();
                    if (item.Survey != null && item.Survey.LastOrDefault() != null && item.Survey.LastOrDefault().DateTime_Updated > DateTime.UtcNow.AddHours(-72))
                    {
                        buildingCheck = true;
                        getBuildings = item.Survey.LastOrDefault();
                    }
                    else
                        buildingCheck = false;

                    var getSciences = new CS_Code.Utopia_Province_Data_Captured_Science();
                    if (item.SOS != null && item.SOS.LastOrDefault() != null)
                    {
                        scienceCheck = true;
                        getSciences = item.SOS.LastOrDefault();
                    }
                    else
                        scienceCheck = false;

                    var getMils = new List<CS_Code.Utopia_Province_Data_Captured_Type_Military>();
                    if (item.SOM != null && item.SOM.Count > 0)
                    {
                        milCheck = true;
                        getMils = item.SOM;
                    }
                    else
                        milCheck = false;

                    var getAttacksAdded = (from xx in AttackQuery
                                           where xx.Province_ID_Added == item.Province_ID
                                           select xx).ToList();
                    if (getAttacksAdded.Count() > 0)
                        attackAddedCheck = true;
                    else
                        attackAddedCheck = false;

                    var getAttacked = (from xx in AttackQuery
                                       where xx.Province_ID_Attacked == item.Province_ID
                                       where xx.Mod_Off_Sent.HasValue
                                       select xx).ToList();
                    if (getAttacked.Count() > 0)
                        attackedCheck = true;
                    else
                        attackedCheck = false;

                    var getEffects = (from xx in itemEffects
                                      where xx.Directed_To_Province_ID == item.Province_ID
                                      select xx).ToList();

                    dragonNetworth += item.Networth.GetValueOrDefault(1);
                    totalGold += item.Money.GetValueOrDefault(0);
                    food += item.Food.GetValueOrDefault(0);
                    acres += item.Land.GetValueOrDefault(0);
                    dailyIncome += item.Daily_Income.GetValueOrDefault(0);
                    peasants += item.Peasents.GetValueOrDefault(0);
                    tradeBalance += item.Trade_Balance.GetValueOrDefault(0);
                    runes += item.Runes.GetValueOrDefault(0);
                    prisoners += item.Prisoners.GetValueOrDefault(0);
                    soldiers += item.Soldiers.GetValueOrDefault(0);
                    horses += item.War_Horses.GetValueOrDefault(0);
                    population += item.Population.GetValueOrDefault(0);
                    defSpecs += item.Soldiers_Regs_Def.GetValueOrDefault(0);
                    offSpecs += item.Soldiers_Regs_Off.GetValueOrDefault(0);

                    switch (i % 2)
                    {
                        case 0:
                            sb.Append("<tr class=\"d0\" id=\"" + item.Province_ID + "\">");
                            break;
                        case 1:
                            sb.Append("<tr class=\"d1\" id=\"" + item.Province_ID + "\">");
                            break;
                    }
                    i += 1;

                    if (monType != MonarchType.none && monType != MonarchType.kdMonarch)
                        if (kingdomlessProvinces)
                            sb.Append("<td  ><a class=\"imgLinks\" href='#' onclick=\"DeleteKdLessProvince('" + item.Province_ID.RemoveDashes() + "')\"><img src=\"http://codingforcharity.org/utopiapimp/img/icons/TC_inline.png\" title=\"Delete Me\" /></a></td>");


                    foreach (Match column in mc)
                    {
                        var getCN = (from gcn in UtopiaHelper.Instance.ColumnNames
                                     where gcn.uid == Convert.ToInt32(column.Value)
                                     select gcn.columnName).FirstOrDefault();
                        sb.Append("<td>");
                        switch (provinceCheck)
                        {
                            case true:
                                switch (getCN)
                                {
                                    case "Honor":
                                        switch (item.Honor.HasValue)
                                        {
                                            case true:
                                                sb.Append(item.Honor.Value.ToString("N0"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "Prisoners":
                                        switch (milCheck)
                                        {
                                            case true:
                                                var query = (from xx in getMils
                                                             where xx.Military_Location == 2
                                                             where xx.Time_To_Return > DateTime.UtcNow
                                                             select xx.uid).FirstOrDefault();
                                                switch (query > 0)
                                                {
                                                    case true:
                                                        var getHomeTroops = (from xx in getMils
                                                                             where xx.Military_Location == 1
                                                                             select xx.Prisoners).LastOrDefault();
                                                        sb.Append("<span title=\"Prisoners at Home\" style=\"color:Red;\">" + getHomeTroops.GetValueOrDefault(0).ToString("N0") + "</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                                        break;
                                                }
                                                break;
                                        }
                                        switch (cbCheck)
                                        {
                                            case true:
                                                sb.Append(cb.Prisoners.GetValueOrDefault(0).ToString("N0"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "Elites/acre":
                                        switch (milCheck)
                                        {
                                            case true:
                                                var query = (from xx in getMils
                                                             where xx.Military_Location == 2
                                                             where xx.Time_To_Return > DateTime.UtcNow
                                                             select xx.uid).FirstOrDefault();

                                                switch (query > 0) //if there is military out
                                                {
                                                    case true:
                                                        sb.Append("<span title=\"Off Specs at Home\" style=\"color:Red;\">" + (getMils.Where(x => x.Military_Location == 1).Select(x => x.Elites).LastOrDefault().GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1") + "</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                                        switch (cbCheck)
                                                        {
                                                            case true:
                                                                sb.Append((cb.Soldiers_Elites.GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1"));
                                                                break;
                                                            default:
                                                                sb.Append("-");
                                                                break;
                                                        }
                                                        break;
                                                    default:
                                                        switch (cbCheck) //if cb exists, check if the last som or last cb is newer.  use the newest information.
                                                        {
                                                            case true:
                                                                if (getMils.Where(x => x.Military_Location == 1).Select(x => x.DateTime_Added).LastOrDefault() > cb.Updated_By_DateTime)
                                                                    sb.Append((getMils.Where(x => x.Military_Location == 1).Select(x => x.Elites).LastOrDefault().GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1"));
                                                                else
                                                                    sb.Append((cb.Soldiers_Elites.GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1"));
                                                                break;
                                                            default://if cb doesn't exist, just use the soms data.
                                                                sb.Append((getMils.Where(x => x.Military_Location == 1).Select(x => x.Elites).LastOrDefault().GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1"));
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            default:
                                                switch (cbCheck)
                                                {
                                                    case true:
                                                        sb.Append((cb.Soldiers_Elites.GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1"));
                                                        break;
                                                    default:
                                                        sb.Append("-");
                                                        break;
                                                }
                                                break;
                                        }


                                        break;
                                    case "Off specs/acre":
                                        switch (milCheck)
                                        {
                                            case true:
                                                var query = (from xx in getMils
                                                             where xx.Military_Location == 2
                                                             where xx.Time_To_Return > DateTime.UtcNow
                                                             select xx.uid).FirstOrDefault();

                                                switch (query > 0) //if there is military out
                                                {
                                                    case true:
                                                        sb.Append("<span title=\"Off Specs at Home\" style=\"color:Red;\">" + (getMils.Where(x => x.Military_Location == 1).Select(x => x.Regs_Off).LastOrDefault().GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1") + "</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                                        switch (cbCheck)
                                                        {
                                                            case true:
                                                                sb.Append((cb.Soldiers_Regs_Off.GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1"));
                                                                break;
                                                            default:
                                                                sb.Append("-");
                                                                break;
                                                        }
                                                        break;
                                                    default:
                                                        switch (cbCheck) //if cb exists, check if the last som or last cb is newer.  use the newest information.
                                                        {
                                                            case true:
                                                                if (getMils.Where(x => x.Military_Location == 1).Select(x => x.DateTime_Added).LastOrDefault() > cb.Updated_By_DateTime)
                                                                    sb.Append((getMils.Where(x => x.Military_Location == 1).Select(x => x.Regs_Off).LastOrDefault().GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1"));
                                                                else
                                                                    sb.Append((cb.Soldiers_Regs_Off.GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1"));
                                                                break;
                                                            default://if cb doesn't exist, just use the soms data.
                                                                sb.Append((getMils.Where(x => x.Military_Location == 1).Select(x => x.Regs_Off).LastOrDefault().GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1"));
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            default:
                                                switch (cbCheck)
                                                {
                                                    case true:
                                                        sb.Append((cb.Soldiers_Regs_Off.GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1"));
                                                        break;
                                                    default:
                                                        sb.Append("-");
                                                        break;
                                                }
                                                break;
                                        }

                                        break;
                                    case "Def specs/acre":
                                        switch (cbCheck)
                                        {
                                            case true:
                                                sb.Append(((decimal)cb.Soldiers_Regs_Def.GetValueOrDefault(0) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "Off specs":
                                        switch (milCheck)
                                        {
                                            case true:
                                                var query = (from xx in getMils
                                                             where xx.Military_Location == 2
                                                             where xx.Time_To_Return > DateTime.UtcNow
                                                             select xx.uid).FirstOrDefault();

                                                switch (query > 0) //if there is military out
                                                {
                                                    case true:
                                                        sb.Append("<span title=\"Off Specs at Home\" style=\"color:Red;\">" + getMils.Where(x => x.Military_Location == 1).Select(x => x.Regs_Off).LastOrDefault().GetValueOrDefault(0).ToString("N0") + "</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                                        switch (cbCheck)
                                                        {
                                                            case true:
                                                                sb.Append(cb.Soldiers_Regs_Off.GetValueOrDefault(0).ToString("N0"));
                                                                break;
                                                            default:
                                                                sb.Append("-");
                                                                break;
                                                        }
                                                        break;
                                                    default:
                                                        switch (cbCheck) //if cb exists, check if the last som or last cb is newer.  use the newest information.
                                                        {
                                                            case true:
                                                                if (getMils.Where(x => x.Military_Location == 1).Select(x => x.DateTime_Added).LastOrDefault() > cb.Updated_By_DateTime)
                                                                    sb.Append(getMils.Where(x => x.Military_Location == 1).Select(x => x.Regs_Off).LastOrDefault().GetValueOrDefault(0).ToString("N0"));
                                                                else
                                                                    sb.Append(cb.Soldiers_Regs_Off.GetValueOrDefault(0).ToString("N0"));
                                                                break;
                                                            default://if cb doesn't exist, just use the soms data.
                                                                sb.Append(getMils.Where(x => x.Military_Location == 1).Select(x => x.Regs_Off).LastOrDefault().GetValueOrDefault(0).ToString("N0"));
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            default:
                                                switch (cbCheck)
                                                {
                                                    case true:
                                                        sb.Append(cb.Soldiers_Regs_Off.GetValueOrDefault(0).ToString("N0"));
                                                        break;
                                                    default:
                                                        sb.Append("-");
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case "Def Specs":
                                        switch (cbCheck)
                                        {
                                            case true:
                                                sb.Append(cb.Soldiers_Regs_Def.GetValueOrDefault(0).ToString("N0"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "M":
                                        switch (item.Monarch_Display)
                                        {
                                            case 4:
                                                sb.Append("<span title=\"Monarch\">M</span>");
                                                break;
                                            //case 1:
                                            //    sb.Append("<span title=\"Owner of Kingdom in Pimp\">O</span>");
                                            //    break;
                                            //case 2:
                                            //    sb.Append("<span title=\"Sub-Monarch\">SM</span>");
                                            //    break;
                                        }
                                        break;
                                    case "N":
                                        sb.Append("<span class=\"spanEff imgHover\" onMouseOver=\"HandleMouseOver('" + item.Province_ID + "')\" onclick=\"SetTip('" + item.Province_ID + "', '" + provinceIdentifiers.Where(x => x.Province_ID == item.Province_ID).FirstOrDefault().Province_Name + "')\" onMouseOut=\"toolTip();\">");
                                        sb.Append(item.NoteCount + "<img src=\"http://codingforcharity.org/utopiapimp/img/icons/note_dark.gif\" id=\"" + item.Province_ID + "\" /></span>");
                                        break;
                                    case "SoM":
                                        sb.Append("<div class=\"sr\" onclick=\"RequestIntel(this, 'som', '" + item.Province_ID.RemoveDashes() + "');\" ");
                                        switch (item.SOM_Updated_By_DateTime.HasValue)
                                        {
                                            case true:
                                                switch (item.SOM_Updated_By_DateTime.Value >= DateTime.UtcNow.AddHours(-72))
                                                {
                                                    case true:
                                                        double time = DateTime.UtcNow.Subtract(item.SOM_Updated_By_DateTime.Value).TotalHours;
                                                        string title = item.SOM_Updated_By_DateTime.Value.ToRelativeDate() + " by " + ownedProvinces.Where(x => x.Province_ID == item.SOM_Updated_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault();
                                                        int red = Convert.ToInt32((255 * (time / 24)));
                                                        sb.Append("title=\"Updated: " + title + "\" style=\"background-color:rgb(" + red + ", " + (255 - red) + ",0);\"><span class=\"CbSosSomSurv\">" + item.SOM_Updated_By_DateTime.Value.ToFileTime() + "</span>");
                                                        if (item.SOM_Requested.HasValue) //if the sos was requested by someone, display a question mark.
                                                            sb.Append("?");
                                                        else
                                                            sb.Append("\t");
                                                        break;
                                                    default:
                                                        switch (item.Army_Out_Expires.HasValue)
                                                        {
                                                            case true:
                                                                switch (item.Army_Out_Expires.Value > DateTime.UtcNow)
                                                                {
                                                                    case true:
                                                                        sb.Append("style=\"background-color:rgb(255,255,255);\">?");
                                                                        break;
                                                                    default:
                                                                        sb.Append("style=\"background-color:rgb(255,255,255);\">");
                                                                        if (item.SOM_Requested.HasValue) //if the sos was requested by someone, display a question mark.
                                                                            sb.Append("?");
                                                                        else
                                                                            sb.Append("\t");
                                                                        break;
                                                                }
                                                                break;
                                                            default:
                                                                sb.Append("style=\"background-color:rgb(255,255,255);\">");
                                                                if (item.SOM_Requested.HasValue) //if the sos was requested by someone, display a question mark.
                                                                    sb.Append("?");
                                                                else
                                                                    sb.Append("\t");
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            default:
                                                switch (item.Army_Out_Expires.HasValue)
                                                {
                                                    case true:
                                                        switch (item.Army_Out_Expires.Value > DateTime.UtcNow)
                                                        {
                                                            case true:
                                                                sb.Append("style=\"background-color:rgb(255,255,255);\">?");
                                                                break;
                                                            default:
                                                                sb.Append("style=\"background-color:rgb(255,255,255);\">");
                                                                if (item.SOM_Requested.HasValue) //if the sos was requested by someone, display a question mark.
                                                                    sb.Append("?");
                                                                else
                                                                    sb.Append("\t");
                                                                break;
                                                        }
                                                        break;
                                                    default:
                                                        sb.Append("style=\"background-color:rgb(255,255,255);\">");
                                                        if (item.SOM_Requested.HasValue) //if the sos was requested by someone, display a question mark.
                                                            sb.Append("?");
                                                        else
                                                            sb.Append("\t");
                                                        break;
                                                }
                                                break;
                                        }
                                        sb.Append("</div>");
                                        break;
                                    case "Personality":
                                        sb.Append(UtopiaHelper.Instance.Personalities.Where(x => x.uid == item.Personality_ID).Select(x => x.name).FirstOrDefault());
                                        break;
                                    case "Rank":
                                        var rankInfo = (from r in UtopiaHelper.Instance.Ranks where item.Nobility_ID == r.uid select new { r.uid, r.name }).FirstOrDefault();
                                        if (rankInfo != null)
                                            sb.Append("<span class=\"CbSosSomSurv\">" + rankInfo.uid + "</span>" + rankInfo.name);
                                        else
                                            sb.Append("-");
                                        break;
                                    case "ME %":
                                        MilitaryKdPage.DisplayMEPercentageColumn(sb, milCheck, getMils);
                                        break;
                                    case "CB":
                                        sb.Append("<div onclick=\"RequestIntel(this, 'cb', '" + item.Province_ID.RemoveDashes() + "');\" class=\"sr\" ");
                                        switch (cbCheck)
                                        {
                                            case true:
                                                double time = DateTime.UtcNow.Subtract(cb.Updated_By_DateTime.Value).TotalHours;
                                                string title = cb.Updated_By_DateTime.Value.ToRelativeDate() + " by " + ownedProvinces.Where(x => x.Province_ID == cb.Updated_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault();
                                                int red = Convert.ToInt32((255 * (time / 24)));
                                                sb.Append("alt=\" Updated: " + title + "\" title=\"Updated: " + title + "\" style=\"background-color:rgb(" + red + ", " + (255 - red) + ",0);\"><span class=\"CbSosSomSurv\">" + cb.Updated_By_DateTime.Value.ToFileTime() + "</span>");
                                                break;
                                            default:
                                                sb.Append(" style=\"background-color:rgb(255,255,255);\">");
                                                break;
                                        }
                                        if (item.CB_Requested.HasValue) //if the CB was requested by someone, display a question mark.
                                            sb.Append("?");
                                        else
                                            sb.Append("\t"); //not requested, ust filling in blank
                                        sb.Append("</div>");
                                        break;
                                    case "SoS":
                                        switch (scienceCheck)
                                        {
                                            case true:
                                                double time = DateTime.UtcNow.Subtract(getSciences.DateTime_Added).TotalHours;
                                                string title = getSciences.DateTime_Added.ToRelativeDate() + " by " + ownedProvinces.Where(x => x.Province_ID == getSciences.Province_ID_Added).Select(x => x.Province_Name).FirstOrDefault();
                                                int red = Convert.ToInt32((255 * (time / 24)));
                                                sb.Append("<div onclick=\"RequestIntel(this, 'sos', '" + item.Province_ID.RemoveDashes() + "');\" class=\"sr\" alt=\" Updated: " + title + "\" title=\"Updated: " + title + "\" style=\"background-color:rgb(" + red + ", " + (255 - red) + ",0); \"><span class=\"CbSosSomSurv\">" + getSciences.DateTime_Added.ToFileTime() + "</span>");
                                                break;
                                            default:
                                                sb.Append("<div onclick=\"RequestIntel(this, 'sos', '" + item.Province_ID.RemoveDashes() + "');\" class=\"sr\" style=\"background-color:rgb(255,255,255);\">");
                                                break;
                                        }
                                        if (item.SOS_Requested.HasValue) //if the sos was requested by someone, display a question mark.
                                            sb.Append("?");
                                        else
                                            sb.Append("\t");
                                        sb.Append("</div>");
                                        break;
                                    case "Province Name - Race/Personality":
                                        switch (kingdomlessProvinces)
                                        {
                                            case true:
                                                sb.Append("<a href=\"ProvinceDetail.aspx?ID=" + item.Province_ID.RemoveDashes() + "\">" + provinceIdentifiers.Where(x => x.Province_ID == item.Province_ID).FirstOrDefault().Province_Name + "</a> <a href=\"" + UtopiaParser.UtopiaKingdomPage + provinceIdentifiers.Where(x => x.Province_ID == item.Province_ID).FirstOrDefault().Kingdom_Island + "/" + provinceIdentifiers.Where(x => x.Province_ID == item.Province_ID).FirstOrDefault().Kingdom_Location + "\" target=\"_blank\">(" + provinceIdentifiers.Where(x => x.Province_ID == item.Province_ID).FirstOrDefault().Kingdom_Island + ":" + provinceIdentifiers.Where(x => x.Province_ID == item.Province_ID).FirstOrDefault().Kingdom_Location + ")</a> " + " (" + (from r in UtopiaHelper.Instance.Races where item.Race_ID == r.uid select r.name).FirstOrDefault() + "/" + (from p in UtopiaHelper.Instance.Personalities where item.Personality_ID == p.uid select p.name).FirstOrDefault() + ")");
                                                break;
                                            default:
                                                sb.Append("<a href=\"ProvinceDetail.aspx?ID=" + item.Province_ID.RemoveDashes() + "\">" + provinceIdentifiers.Where(x => x.Province_ID == item.Province_ID).FirstOrDefault().Province_Name + "</a> " + " (" + (from r in UtopiaHelper.Instance.Races where item.Race_ID == r.uid select r.name).FirstOrDefault() + "/" + (from p in UtopiaHelper.Instance.Personalities where item.Personality_ID == p.uid select p.name).FirstOrDefault() + ")");
                                                break;
                                        }
                                        break;
                                    case "NW/Acre":
                                        KdPageHelper.displayNwPerAcreColumn(sb, item);
                                        break;
                                    case "Pop/acre":
                                        KdPageHelper.displayPopAcreColumn(sb, cbCheck, item, cb);
                                        break;
                                    case "Food/Acre":
                                        switch (cbCheck)
                                        {
                                            case true:
                                                sb.Append(((decimal)cb.Food.GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "gc/Acre":
                                        switch (cbCheck)
                                        {
                                            case true:
                                                sb.Append(((decimal)cb.Money.GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "Peasants/Acre":
                                        switch (cbCheck)
                                        {
                                            case true:
                                                sb.Append(((decimal)cb.Peasents.GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "Runes/Acre":
                                        switch (cbCheck)
                                        {
                                            case true:
                                                sb.Append(((decimal)cb.Runes.GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "Province Name":
                                        switch (kingdomlessProvinces)
                                        {
                                            case true://displays kingdom location
                                                sb.Append("<a href=\"ProvinceDetail.aspx?ID=" + item.Province_ID.RemoveDashes() + "\">" + provinceIdentifiers.Where(x => x.Province_ID == item.Province_ID).FirstOrDefault().Province_Name + "</a> <a href=\"" + UtopiaParser.UtopiaKingdomPage + provinceIdentifiers.Where(x => x.Province_ID == item.Province_ID).FirstOrDefault().Kingdom_Island + "/" + provinceIdentifiers.Where(x => x.Province_ID == item.Province_ID).FirstOrDefault().Kingdom_Location + "\" target=\"_blank\">(" + provinceIdentifiers.Where(x => x.Province_ID == item.Province_ID).FirstOrDefault().Kingdom_Island + ":" + provinceIdentifiers.Where(x => x.Province_ID == item.Province_ID).FirstOrDefault().Kingdom_Location + ")</a>");
                                                break;
                                            default:
                                                sb.Append("<a href=\"ProvinceDetail.aspx?ID=" + item.Province_ID.RemoveDashes() + "\">" + provinceIdentifiers.Where(x => x.Province_ID == item.Province_ID).FirstOrDefault().Province_Name + "</a>");
                                                break;
                                        }
                                        break;
                                    case "Acres":
                                        switch (item.Land.HasValue)
                                        {
                                            case true:
                                                sb.Append(item.Land.Value.ToString("N0"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "NW":
                                        switch (item.Networth.HasValue)
                                        {
                                            case true:
                                                sb.Append(item.Networth.GetValueOrDefault(0).ToString("N0"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "Race":
                                        sb.Append((from r in UtopiaHelper.Instance.Races where item.Race_ID == r.uid select r.name).FirstOrDefault());
                                        break;
                                    case "Daily Income":
                                        switch (item.Daily_Income.HasValue)
                                        {
                                            case true:
                                                sb.Append(item.Daily_Income.Value.ToString("N0"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "Population":
                                        switch (item.Population.HasValue)
                                        {
                                            case true:
                                                sb.Append(item.Population.Value.ToString("N0"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "Ruler":
                                        if (item.Ruler_Name != string.Empty && item.Ruler_Name != null)
                                            sb.Append(item.Ruler_Name);
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Trade Balance":
                                        switch (cbCheck)
                                        {
                                            case true:
                                                sb.Append(cb.Trade_Balance.GetValueOrDefault().ToString("N0"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "Food":
                                        switch (cbCheck)
                                        {
                                            case true:
                                                sb.Append(cb.Food.Value.ToString("N0"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "Gold":
                                        switch (cbCheck)
                                        {
                                            case true:
                                                sb.Append(cb.Money.Value.ToString("N0"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "Peasants":
                                        switch (cbCheck)
                                        {
                                            case true:
                                                sb.Append(cb.Peasents.Value.ToString("N0"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "Runes":
                                        switch (cbCheck)
                                        {
                                            case true:
                                                sb.Append(cb.Runes.Value.ToString("N0"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "Horses":
                                        switch (milCheck)
                                        {
                                            case true:
                                                var query = (from xx in getMils
                                                             where xx.Military_Location == 2
                                                             where xx.Time_To_Return > DateTime.UtcNow
                                                             select xx.uid).FirstOrDefault();
                                                switch (query > 0)
                                                {
                                                    case true:
                                                        var getHomeTroops = (from xx in getMils
                                                                             where xx.Military_Location == 1
                                                                             select xx.Horses).LastOrDefault();
                                                        sb.Append("<span title=\"Horses at Home\" style=\"color:Red;\">" + getHomeTroops.GetValueOrDefault(0).ToString("N0") + "</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                                        break;
                                                }
                                                break;
                                        }
                                        switch (cbCheck)
                                        {
                                            case true:
                                                sb.Append(cb.War_Horses.GetValueOrDefault(0).ToString("N0"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "Soldiers":
                                        switch (milCheck)
                                        {
                                            case true:
                                                var query = (from xx in getMils
                                                             where xx.Military_Location == 2
                                                             where xx.Time_To_Return > DateTime.UtcNow
                                                             select xx.uid).FirstOrDefault();
                                                switch (query > 0)
                                                {
                                                    case true:
                                                        var getHomeTroops = (from xx in getMils
                                                                             where xx.Military_Location == 1
                                                                             select xx.Soldiers).LastOrDefault();
                                                        sb.Append("<span title=\"Soldiers at Home\" style=\"color:Red;\">" + getHomeTroops.GetValueOrDefault(0).ToString("N0") + "</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                                        break;
                                                }
                                                break;
                                        }
                                        switch (cbCheck)
                                        {
                                            case true:
                                                sb.Append(cb.Soldiers.GetValueOrDefault(0).ToString("N0"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "Est TPA":
                                        OpsKdPage.displayEstTpaColumn(sb, item);
                                        break;
                                    case "Est WPA":
                                        OpsKdPage.displayEstWpaColumn(sb, item);
                                        break;
                                    case "Mod TPA":
                                        OpsKdPage.displayModTpaColumn(sb, buildingCheck, scienceCheck, item, getBuildings, getSciences);
                                        break;
                                    case "Mod WPA":
                                        OpsKdPage.displayModWpaColumn(sb, scienceCheck, item, getSciences);
                                        break;
                                    case "Draft %":
                                        switch (cbCheck)
                                        {
                                            case true:
                                                sb.Append((cb.Draft.Value * 100).ToString("N0") + "%");
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "Recently Hit?":
                                        switch (cbCheck)
                                        {
                                            case true:
                                                switch (cb.Updated_By_DateTime.GetValueOrDefault(DateTime.UtcNow.AddHours(-26)) > DateTime.UtcNow.AddHours(-24))
                                                {
                                                    case true://if CB was updated within the past 24 hours
                                                        sb.Append(cb.Hit);
                                                        break;
                                                    default:
                                                        sb.Append("-");
                                                        break;
                                                } break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "Survey":
                                        switch (buildingCheck)
                                        {
                                            case true:
                                                double time = DateTime.UtcNow.Subtract(getBuildings.DateTime_Updated).TotalHours;
                                                string title = getBuildings.DateTime_Updated.ToRelativeDate() + " by " + ownedProvinces.Where(x => x.Province_ID == getBuildings.Province_ID_Updated_By).Select(x => x.Province_Name).FirstOrDefault();
                                                int red = Convert.ToInt32((255 * (time / 24)));
                                                sb.Append("<div onclick=\"RequestIntel(this, 'survey', '" + item.Province_ID.RemoveDashes() + "');\" class=\"sr\" alt=\" Updated: " + title + "\" title=\"Updated: " + title + "\" style=\"background-color:rgb(" + red + ", " + (255 - red) + ",0);\"><span class=\"CbSosSomSurv\">" + getBuildings.DateTime_Updated + "</span>");
                                                break;
                                            default:
                                                sb.Append("<div onclick=\"RequestIntel(this, 'survey', '" + item.Province_ID.RemoveDashes() + "');\" class=\"sr\" style=\"background-color:rgb(255,255,255);\">");
                                                break;
                                        }
                                        if (item.Survey_Requested.HasValue) //if the survey was requested by someone, display a question mark.
                                            sb.Append("?");
                                        else
                                            sb.Append("\t");
                                        sb.Append("</div>");
                                        break;
                                    case "Elites home/total":
                                        switch (milCheck)
                                        {
                                            case true:
                                                var query = (from xx in getMils
                                                             where xx.Military_Location == 2
                                                             where xx.Time_To_Return > DateTime.UtcNow
                                                             select xx.uid).FirstOrDefault();

                                                switch (query > 0) //if there is military out
                                                {
                                                    case true:
                                                        sb.Append("<span title=\"Total Elites at Home\" style=\"color:Red;\">" + getMils.Where(x => x.Military_Location == 1).Select(x => x.Elites).LastOrDefault().GetValueOrDefault(0).ToString("N0") + "</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                                        switch (cbCheck)
                                                        {
                                                            case true:
                                                                sb.Append(cb.Soldiers_Elites.GetValueOrDefault(0).ToString("N0"));
                                                                break;
                                                            default:
                                                                sb.Append("-");
                                                                break;
                                                        }
                                                        break;
                                                    default:
                                                        switch (cbCheck) //if cb exists, check if the last som or last cb is newer.  use the newest information.
                                                        {
                                                            case true:
                                                                if (getMils.Where(x => x.Military_Location == 1).Select(x => x.DateTime_Added).LastOrDefault() > cb.Updated_By_DateTime)
                                                                    sb.Append(getMils.Where(x => x.Military_Location == 1).Select(x => x.Elites).LastOrDefault().GetValueOrDefault(0).ToString("N0"));
                                                                else
                                                                    sb.Append(cb.Soldiers_Elites.GetValueOrDefault(0).ToString("N0"));
                                                                break;
                                                            default://if cb doesn't exist, just use the soms data.
                                                                sb.Append(getMils.Where(x => x.Military_Location == 1).Select(x => x.Elites).LastOrDefault().GetValueOrDefault(0).ToString("N0"));
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            default:
                                                switch (cbCheck)
                                                {
                                                    case true:
                                                        sb.Append(cb.Soldiers_Elites.GetValueOrDefault(0).ToString("N0"));
                                                        break;
                                                    default:
                                                        sb.Append("-");
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case "Mod def (dpa)": //Mod def: everything you have that increase your defence
                                        MilitaryKdPage.DisplayDefdpa(sb, milCheck, cbCheck, item, cb, getMils);
                                        break;
                                    case "Mod def"://Mod def: everything you have that increase your defence
                                        MilitaryKdPage.DisplayModDefColumn(sb, milCheck, cbCheck, item, cb, getMils);
                                        break;
                                    case "Mod off":
                                        MilitaryKdPage.DisplayModOffColumn(sb, milCheck, cbCheck, cb, getMils);
                                        break;
                                    case "Mod dpa"://Mod def: everything you have that increase your defence
                                        switch (milCheck)
                                        {
                                            case true:
                                                MilitaryKdPage.DisplayModDPAWithSOM(sb, cbCheck, item, cb, getMils);
                                                break;
                                            default:
                                                switch (cbCheck)
                                                {
                                                    case true:
                                                        sb.Append((cb.Total_Mod_Defense.GetValueOrDefault(1) / item.Land.GetValueOrDefault(1)).ToString("N0"));
                                                        break;
                                                    default:
                                                        sb.Append("-");
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case "Max Mod off (opa)":
                                        MilitaryKdPage.displayMaxModOffopaColumn(sb, milCheck, cbCheck, item, cb, getMils);
                                        break;
                                    case "Max Mod off":
                                        switch (milCheck)
                                        {
                                            case true:
                                                var query = (from xx in getMils
                                                             where xx.Military_Location == 2
                                                             where xx.Time_To_Return > DateTime.UtcNow
                                                             select xx.uid).FirstOrDefault();

                                                switch (query > 0) //if there is military out
                                                {
                                                    case true:
                                                        sb.Append("<span title=\"Max Mod Off at Home\" style=\"color:Red;\">" + getMils.Where(x => x.Military_Location == 1).Select(x => x.Net_Offense_Pts_Home).LastOrDefault().GetValueOrDefault(0).ToString("N0") + "</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                                        switch (cbCheck)
                                                        {
                                                            case true:
                                                                sb.Append(cb.Total_Mod_Offense.GetValueOrDefault(0).ToString("N0"));
                                                                break;
                                                            default:
                                                                sb.Append("-");
                                                                break;
                                                        }
                                                        break;
                                                    default:
                                                        switch (cbCheck) //if cb exists, check if the last som or last cb is newer.  use the newest information.
                                                        {
                                                            case true:
                                                                if (getMils.Where(x => x.Military_Location == 1).Select(x => x.DateTime_Added).LastOrDefault() > cb.Updated_By_DateTime)
                                                                    sb.Append(getMils.Where(x => x.Military_Location == 1).Select(x => x.Net_Offense_Pts_Home).LastOrDefault().GetValueOrDefault(0).ToString("N0"));
                                                                else
                                                                    sb.Append(cb.Total_Mod_Offense.GetValueOrDefault(0).ToString("N0"));
                                                                break;
                                                            default://if cb doesn't exist, just use the soms data.
                                                                sb.Append(getMils.Where(x => x.Military_Location == 1).Select(x => x.Net_Offense_Pts_Home).LastOrDefault().GetValueOrDefault(0).ToString("N0"));
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            default:
                                                switch (cbCheck)
                                                {
                                                    case true:
                                                        sb.Append(cb.Total_Mod_Offense.GetValueOrDefault(0).ToString("N0"));
                                                        break;
                                                    default:
                                                        sb.Append("-");
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case "Max Mod opa":
                                        switch (milCheck)
                                        {
                                            case true:
                                                var query = (from xx in getMils
                                                             where xx.Military_Location == 2
                                                             where xx.Time_To_Return > DateTime.UtcNow
                                                             select xx.uid).FirstOrDefault();

                                                switch (query > 0) //if there is military out
                                                {
                                                    case true:
                                                        sb.Append("<span title=\"Max Mod Off at Home\" style=\"color:Red;\">" + (getMils.Where(x => x.Military_Location == 1).Select(x => x.Net_Offense_Pts_Home).LastOrDefault().GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N1") + "</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                                        switch (cbCheck)
                                                        {
                                                            case true:
                                                                sb.Append((cb.Total_Mod_Offense.GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N0"));
                                                                break;
                                                            default:
                                                                sb.Append("-");
                                                                break;
                                                        }
                                                        break;
                                                    default:
                                                        switch (cbCheck) //if cb exists, check if the last som or last cb is newer.  use the newest information.
                                                        {
                                                            case true:
                                                                if (getMils.Where(x => x.Military_Location == 1).Select(x => x.DateTime_Added).LastOrDefault() > cb.Updated_By_DateTime)
                                                                    sb.Append((getMils.Where(x => x.Military_Location == 1).Select(x => x.Net_Offense_Pts_Home).LastOrDefault().GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N0"));
                                                                else
                                                                    sb.Append((cb.Total_Mod_Offense.GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N0"));
                                                                break;
                                                            default://if cb doesn't exist, just use the soms data.
                                                                sb.Append((getMils.Where(x => x.Military_Location == 1).Select(x => x.Net_Offense_Pts_Home).LastOrDefault().GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N0"));
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            default:
                                                switch (cbCheck)
                                                {
                                                    case true:
                                                        sb.Append((cb.Total_Prac_Offense.GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N0"));
                                                        break;
                                                    default:
                                                        sb.Append("-");
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case "Prac Mod Off":
                                        switch (milCheck)
                                        {
                                            case true:
                                                var query = (from xx in getMils
                                                             where xx.Military_Location == 2
                                                             where xx.Time_To_Return > DateTime.UtcNow
                                                             select xx.uid).FirstOrDefault();

                                                switch (query > 0) //if there is military out
                                                {
                                                    case true:
                                                        sb.Append("<span title=\"Prac Mod Off at Home\" style=\"color:Red;\">" + getMils.Where(x => x.Military_Location == 1).Select(x => x.Net_Offense_Pts_Home).LastOrDefault().GetValueOrDefault(0).ToString("N0") + "</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                                        switch (cbCheck)
                                                        {
                                                            case true:
                                                                sb.Append(cb.Total_Prac_Offense.GetValueOrDefault(0).ToString("N0"));
                                                                break;
                                                            default:
                                                                sb.Append("-");
                                                                break;
                                                        }
                                                        break;
                                                    default:
                                                        switch (cbCheck) //if cb exists, check if the last som or last cb is newer.  use the newest information.
                                                        {
                                                            case true:
                                                                if (getMils.Where(x => x.Military_Location == 1).Select(x => x.DateTime_Added).LastOrDefault() > cb.Updated_By_DateTime)
                                                                    sb.Append(getMils.Where(x => x.Military_Location == 1).Select(x => x.Net_Offense_Pts_Home).LastOrDefault().GetValueOrDefault(0).ToString("N0"));
                                                                else
                                                                    sb.Append(cb.Total_Prac_Offense.GetValueOrDefault(0).ToString("N0"));
                                                                break;
                                                            default://if cb doesn't exist, just use the soms data.
                                                                sb.Append(getMils.Where(x => x.Military_Location == 1).Select(x => x.Net_Offense_Pts_Home).LastOrDefault().GetValueOrDefault(0).ToString("N0"));
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            default:
                                                switch (cbCheck)
                                                {
                                                    case true:
                                                        sb.Append(cb.Total_Prac_Offense.GetValueOrDefault(0).ToString("N0"));
                                                        break;
                                                    default:
                                                        sb.Append("-");
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case "Prac Mod Def"://Mod def: everything you have that increase your defence;;; practical mod def: like Mod def but assuming you have home a given % of your elites
                                        MilitaryKdPage.displayPracModDefColumn(sb, milCheck, cbCheck, item, cb, getMils);
                                        break;
                                    case "Prac Mod Off (opa)":
                                        switch (milCheck)
                                        {
                                            case true:
                                                var query = (from xx in getMils
                                                             where xx.Military_Location == 2
                                                             where xx.Time_To_Return > DateTime.UtcNow
                                                             select xx.uid).FirstOrDefault();

                                                switch (query > 0) //if there is military out
                                                {
                                                    case true:
                                                        var m = getMils.Where(x => x.Military_Location == 1).Select(x => x.Net_Offense_Pts_Home).LastOrDefault().GetValueOrDefault(0);
                                                        sb.Append("<span title=\"Prac Mod Off at Home\" style=\"color:Red;\">" + m.ToString("N0") + " (" + (m / item.Land.GetValueOrDefault(1)).ToString("N0") + ")</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                                        switch (cbCheck)
                                                        {
                                                            case true:
                                                                sb.Append(cb.Total_Prac_Offense.GetValueOrDefault(0).ToString("N0") + " (" + (cb.Total_Prac_Offense.GetValueOrDefault(0) / item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                                                break;
                                                            default:
                                                                sb.Append("-");
                                                                break;
                                                        }
                                                        break;
                                                    default:
                                                        var m1 = getMils.Where(x => x.Military_Location == 1).Select(x => x.Net_Offense_Pts_Home).LastOrDefault().GetValueOrDefault(0);
                                                        switch (cbCheck) //if cb exists, check if the last som or last cb is newer.  use the newest information.
                                                        {
                                                            case true:
                                                                if (getMils.Where(x => x.Military_Location == 1).Select(x => x.DateTime_Added).LastOrDefault() > cb.Updated_By_DateTime)
                                                                    sb.Append(m1.ToString("N0") + " (" + (m1 / item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                                                else
                                                                    sb.Append(cb.Total_Prac_Offense.GetValueOrDefault(0).ToString("N0") + " (" + (cb.Total_Prac_Offense.GetValueOrDefault(0) / item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                                                break;
                                                            default://if cb doesn't exist, just use the soms data.
                                                                sb.Append(m1.ToString("N0") + " (" + (m1 / item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            default:
                                                switch (cbCheck)
                                                {
                                                    case true:
                                                        sb.Append(cb.Total_Prac_Offense.GetValueOrDefault(0).ToString("N0") + " (" + (cb.Total_Prac_Offense.GetValueOrDefault(0) / item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                                        break;
                                                    default:
                                                        sb.Append("-");
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case "Mod def/NW":
                                        MilitaryKdPage.displayModDefNwColumn(sb, milCheck, cbCheck, item, cb, getMils);
                                        break;
                                    case "Mod off/NW":
                                        switch (milCheck)
                                        {
                                            case true:
                                                var query = (from xx in getMils
                                                             where xx.Military_Location == 2
                                                             where xx.Time_To_Return > DateTime.UtcNow
                                                             select xx.uid).FirstOrDefault();

                                                switch (query > 0) //if there is military out
                                                {
                                                    case true:
                                                        sb.Append("<span title=\"Max Mod Off at Home\" style=\"color:Red;\">" + (getMils.Where(x => x.Military_Location == 1).Select(x => x.Net_Offense_Pts_Home).LastOrDefault().GetValueOrDefault(1) / (decimal)item.Networth.GetValueOrDefault(1)).ToString("N1") + "</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                                        switch (cbCheck)
                                                        {
                                                            case true:
                                                                sb.Append((cb.Total_Mod_Offense.GetValueOrDefault(1) / (decimal)item.Networth.GetValueOrDefault(1)).ToString("N1"));
                                                                break;
                                                            default:
                                                                sb.Append("-");
                                                                break;
                                                        }
                                                        break;
                                                    default:
                                                        switch (cbCheck) //if cb exists, check if the last som or last cb is newer.  use the newest information.
                                                        {
                                                            case true:
                                                                if (getMils.Where(x => x.Military_Location == 1).Select(x => x.DateTime_Added).LastOrDefault() > cb.Updated_By_DateTime)
                                                                    sb.Append((getMils.Where(x => x.Military_Location == 1).Select(x => x.Net_Offense_Pts_Home).LastOrDefault().GetValueOrDefault(1) / (decimal)item.Networth.GetValueOrDefault(1)).ToString("N1"));
                                                                else
                                                                    sb.Append((cb.Total_Mod_Offense.GetValueOrDefault(1) / (decimal)item.Networth.GetValueOrDefault(1)).ToString("N1"));
                                                                break;
                                                            default://if cb doesn't exist, just use the soms data.
                                                                sb.Append((getMils.Where(x => x.Military_Location == 1).Select(x => x.Net_Offense_Pts_Home).LastOrDefault().GetValueOrDefault(1) / (decimal)item.Networth.GetValueOrDefault(1)).ToString("N1"));
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;
                                            default:
                                                switch (cbCheck)
                                                {
                                                    case true:
                                                        sb.Append((cb.Total_Prac_Offense.GetValueOrDefault(1) / (decimal)item.Networth.GetValueOrDefault(1)).ToString("N1"));
                                                        break;
                                                    default:
                                                        sb.Append("-");
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case "Off ME %":
                                        MilitaryKdPage.displayOffMePercentageColumn(sb, milCheck, getMils);
                                        break;
                                    case "Def ME %":
                                        MilitaryKdPage.DisplayDefMEPercentage(sb, milCheck, getMils);
                                        break;
                                    case "Mod off (opa) no elites":
                                        MilitaryKdPage.DisplayModOffNoElitesColumn(sb, milCheck, cbCheck, item, cb, getMils);
                                        break;
                                    case "Mod def (dpa) no elites"://Mod def with no elites: like Mod def but assuming you have 0 elites
                                    case "Prac Mod Def (dpa)":
                                        MilitaryKdPage.displayPracModDefdpa(sb, milCheck, cbCheck, item, cb, getMils);
                                        break;
                                    case "BE %":
                                        if (buildingCheck && cbCheck)
                                        {
                                            if (getBuildings.DateTime_Updated > cb.Updated_By_DateTime) //if Survey is Newer
                                                sb.Append(getBuildings.Building_Efficiency.GetValueOrDefault(0).ToString("N1") + "%");
                                            else //if CB is newer
                                                sb.Append(cb.Building_Effectiveness.GetValueOrDefault(0).ToString("N1") + "%");
                                        }
                                        else if (buildingCheck)
                                            sb.Append(getBuildings.Building_Efficiency.GetValueOrDefault(0).ToString("N1") + "%");
                                        else if (cbCheck)
                                            sb.Append(cb.Building_Effectiveness.GetValueOrDefault(0).ToString("N1") + "%");
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Effects":
                                        switch (item.Army_Out_Expires.HasValue)
                                        {
                                            case true:
                                                switch (item.Army_Out_Expires.Value > DateTime.UtcNow)
                                                {
                                                    case true:
                                                        sb.Append(" <span ");
                                                        switch (attackAddedCheck)
                                                        {
                                                            case true:
                                                                sb.Append("title=\"");
                                                                for (int jj = 0; jj < getAttacksAdded.Count(); jj++)
                                                                    sb.Append(ownedProvinces.Where(x => x.Province_ID == getAttacksAdded[jj].Province_ID_Added).Select(x => x.Province_Name).FirstOrDefault() + " " + getAttacksAdded[jj].Attack_Type_Name + " MO " + getAttacksAdded[jj].Mod_Off_Sent.GetValueOrDefault(0).ToString("N2") + "k sent.; ");
                                                                sb.Append("\" ");
                                                                break;
                                                        }
                                                        sb.Append("class=\"spanEff\"><img src=\"" + ImagesStatic.ElitesOut + "\" />" + item.Army_Out_Expires.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                        break;
                                                }
                                                break;
                                        }
                                        switch (attackedCheck)
                                        {
                                            case true:
                                                sb.Append(" <span ");
                                                sb.Append("title=\"");
                                                for (int jj = 0; jj < getAttacked.Count(); jj++)
                                                    sb.Append(ownedProvinces.Where(x => x.Province_ID == getAttacked[jj].Province_ID_Added).Select(x => x.Province_Name).FirstOrDefault() + " " + getAttacked[jj].Attack_Type_Name + " MO " + getAttacked[jj].Mod_Off_Sent.Value.ToString("N2") + "k sent.;");
                                                sb.Append(" \" class=\"noWrap\"><img src=\"" + ImagesStatic.ModifiedOffense + "\" /><span class=\"spanEff\">" + getAttacked.FirstOrDefault().Mod_Off_Sent.GetValueOrDefault(0).ToString("N2") + "k</span></span>");
                                                break;
                                        }
                                        for (int g = 0; g < getEffects.Count; g++)
                                        {
                                            switch (getEffects[g].Ops.LastOrDefault().Expiration_Date.HasValue)
                                            {
                                                case true:
                                                    DateTime opTime = getEffects[g].Ops.LastOrDefault().Expiration_Date.Value;
                                                    switch (opTime > DateTime.UtcNow)
                                                    {
                                                        case true:
                                                            switch (getEffects[g].OP_Name)
                                                            {
                                                                case "storms":
                                                                    sb.Append(" <span title=\"Storms x " + getEffects[g].Count + "\" class=\"spanEff\"><img src=\"" + ImagesStatic.Storms + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                                    break;
                                                                case "meteors":
                                                                    sb.Append(" <span title=\"Meteors x " + getEffects[g].Count + "\" class=\"spanEff\"><img src=\"" + ImagesStatic.Meteors + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                                    break;
                                                                case "riots":
                                                                    sb.Append(" <span title=\"Riots x " + getEffects[g].Count + " caused by ");
                                                                    foreach (var ite in getEffects[g].Ops)
                                                                        sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + "; ");
                                                                    sb.Append("\" class=\"spanEff\"><img src=\"" + ImagesStatic.Riots + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                                    break;
                                                                case "greedySoldiers":
                                                                    sb.Append(" <span title=\"Greedy Soldiers x " + getEffects[g].Count + "\" class=\"spanEff\"><img src=\"" + ImagesStatic.Greed + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                                    break;
                                                                case "highBirth":
                                                                    sb.Append(" <span title=\"High Birth Rates\" class=\"noWrap\"><img src=\"" + ImagesStatic.HighBirthRates + "\" /><span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "inspireArmy":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Inspire Army x " + getEffects[g].Count + "\"><span class=\"spanEff\"></span><img src=\"" + ImagesStatic.InspireArmy + "\" /><span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "minorProtection":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Minor Protection x " + getEffects[g].Count + "\"><span class=\"spanEff\"><img src=\"" + ImagesStatic.MinorProtection + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "fog":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Fog\"><span class=\"spanEff\"><img src=\"" + ImagesStatic.Fog + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "magicShield":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Magic Shield x " + getEffects[g].Count + " \"><span class=\"spanEff\"><img src=\"" + ImagesStatic.MagicSheild + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "fertileLands":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Fertile Lands\"><span class=\"spanEff\"><img src=\"" + ImagesStatic.FertileLands + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "greatProtection":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Greater Protection x " + getEffects[g].Count + " \"><span class=\"spanEff\"><img src=\"" + ImagesStatic.GreaterProtection + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "naturesBlessing":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Natures Blessing x " + getEffects[g].Count + " \"><span class=\"spanEff\"><img src=\"" + ImagesStatic.NaturesBlessing + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "fastBuilders":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Building Fast x " + getEffects[g].Count + "\"><span class=\"spanEff\"></span><img src=\"" + ImagesStatic.BuildersBoon + "\" /><span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "patriotism":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Patriots x " + getEffects[g].Count + "\"><span class=\"spanEff\"><img src=\"" + ImagesStatic.Patriotism + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "pitfalls":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"PitFalls x " + getEffects[g].Count + "\"><span class=\"spanEff\"><img src=\"" + ImagesStatic.Pitfalls + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "explosions":
                                                                    sb.Append(" <span title=\"Explosions x " + getEffects[g].Count + "\" class=\"spanEff\"><img src=\"" + ImagesStatic.Explosions + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                                    break;
                                                                case "reflectMagic":
                                                                    sb.Append(" <span title=\"Reflecting Magic x " + getEffects[g].Count + "\" class=\"spanEff\"><img src=\"" + ImagesStatic.ReflectingMagic + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                                    break;
                                                                case "warSpoils":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"War Spoils x " + getEffects[g].Count + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>WS<span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "drought":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Drought x " + getEffects[g].Count + "\"><span class=\"spanEff\"><img src=\"" + ImagesStatic.Drought + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "chastity":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Chastity x " + getEffects[g].Count + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>CH<span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "tornadoes":
                                                                    sb.Append(" <span title=\"");
                                                                    foreach (var ite in getEffects[g].Ops)
                                                                        sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + " casted tornadoes, laying waste to " + ite.OP_Text + " acres.; ");
                                                                    sb.Append(KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>T<span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "vermin":
                                                                    sb.Append(" <span title=\"Vermin x " + getEffects[g].Count + "\" class=\"spanEff\"><img src=\"" + ImagesStatic.Vermin + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                                    break;
                                                                case "fountainKnowledge":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Foutain of Knowledge " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>FoK<span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "clearSight":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Clear Sight " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>CS<span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "incineratesRunes":
                                                                    sb.Append(" <span title=\"");
                                                                    foreach (var ite in getEffects[g].Ops)
                                                                        sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + " incinerated " + ite.OP_Text + " runes.; ");
                                                                    sb.Append(KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>IR</span>");
                                                                    break;
                                                                case "townWatch":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Town Watch " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>TW<span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "thievesInvisible":
                                                                    sb.Append(" <span title=\"Partionally Invisible Thieves\" class=\"spanEff\"><img src=\"" + ImagesStatic.InvincibleThieves + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                                    break;
                                                                case "aggression":
                                                                    sb.Append(" <span title=\"Soldiers fight with Aggression " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>AG<span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "fanaticism":
                                                                    sb.Append(" <span title=\"Soldiers fight with Fanatical Fevor " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>FAN<span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "MagesFury":
                                                                    sb.Append(" <span title=\"Your Mages Eyes Burn with Fury " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>MF<span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "WarSpoils":
                                                                    sb.Append(" <span title=\"Your Province Has Been Granted War Spoils " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>WS<span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                //case "fireball":
                                                                //    sb.Append(" <span title=\"FireBall x " + getEffects[g].count + " by ");
                                                                //    foreach (var ite in getEffects[g].Ops)
                                                                //        sb.Append(ownedProvinces.Where(x => x.provinceID == ite.Added_By_Province_ID).Select(x => x.provinceName).FirstOrDefault() + "; ");
                                                                //    sb.Append("\" class=\"spanEff\">" + getEffects[g].count + "x<img src=\""+ImageStatic.Fireball+"\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                                //    break;
                                                                default:
                                                                    string failed = string.Empty;
                                                                    foreach (var ite in getEffects[g].Ops)
                                                                        failed += ite.Added_By_Province_ID + "; " + ite.Expiration_Date + "; " + ite.OP_Text + ";" + ite.TimeStamp.ToString() + "--";
                                                                    UtopiaParser.FailedAt("'BuildingEffectsBroken'", getEffects[g].Directed_To_Province_ID + "; " + getEffects[g].OP_Name + "; " + failed, currentUserID);
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                default:
                                                    switch (getEffects[g].OP_Name)
                                                    {
                                                        case "assasinateWizs":
                                                            sb.Append(" <span title=\"");
                                                            foreach (var ite in getEffects[g].Ops)
                                                                sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + " killed " + ite.OP_Text + " wizards; ");
                                                            sb.Append(KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>AW</span>");
                                                            break;
                                                        case "plague":
                                                            sb.Append(" <span title=\"Plague\" class=\"spanEff\"><img src=\"" + ImagesStatic.Plague + "\" /></span>");
                                                            break;
                                                        case "stormsNoEffects":
                                                            sb.Append(" <span title=\"Storms Have No Effect On this Province" + KdPageHelper.PLEASE_MAKE_ICON + "\" class=\"spanEff\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>SnE</span>");
                                                            break;
                                                        case "shadowlight":
                                                            sb.Append("<span title=\"Shadow Light\" class=\"spanEff\"><img src=\"" + ImagesStatic.ShadowOfLight + "\" /></span>");
                                                            break;
                                                        case "stoleMoney":
                                                        case "stoleFood":
                                                        case "stoleRunes":
                                                        case "sentRunes":
                                                        case "assasinate":
                                                        case "kidnapped":
                                                        case "Infiltrated":
                                                        case "bribedGen":
                                                        case "burnedAcres":
                                                        case "tornadoes":
                                                        case "reflectingMagic":
                                                        case "quickFeet":
                                                        case "sabotageSpells":
                                                        case "foutainKnowledge":
                                                        case "mystVort":
                                                        case "wakeDead":
                                                        case "landLust":
                                                        case "naturesBlessingFailed"://Nature's Blessing will protect your lands from any droughts and storms the world may see fit to place on you. This spell also has a chance of curing the Plague if your lands are affected by it.
                                                        case "fireball":
                                                        case "convertThieves":
                                                        case "bribed":
                                                        case "goldToLead":
                                                        case "exposedThieves":
                                                        case "paradise":
                                                        case "anonymity":
                                                        case "treeGold":
                                                        case "donatedGoldDragon":
                                                        case "incineratesRunes":
                                                        case "forgetBooks":
                                                        case "convertedWizards":
                                                        case "stealHorses":
                                                        case "sentMoney":
                                                        case "freePrisoners":
                                                        case "reflectMagic":
                                                        case "chastity":
                                                        case "killingDragon":
                                                        case "convertedSpecialists":
                                                        case "convertedTroops":
                                                        case "riotsNoEffects":
                                                        case "mystVortFailed":
                                                        case "goldToLeadNoEffects":
                                                        case "triedToBurnAcres":
                                                            break;
                                                        case "Nightmares":
                                                            sb.Append(" <span title=\"");
                                                            foreach (var ite in getEffects[g].Ops)
                                                                sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + " gave " + ite.OP_Text + " men nightmares; ");
                                                            sb.Append(" men had nightmares through the night. " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>NM</span>");
                                                            break;
                                                        case "fog":
                                                            sb.Append(" <span class=\"noWrap\" title=\"Fog was found at this province after an Attack.\"><span class=\"spanEff\"><img src=\"" + ImagesStatic.Fog + "\" /></span></span>");
                                                            break;
                                                        default:
                                                            string failed = string.Empty;
                                                            foreach (var ite in getEffects[g].Ops)
                                                                failed += "From: " + ite.Added_By_Province_ID + "; Expiration:" + ite.Expiration_Date + "; Text:" + ite.OP_Text + ";" + ite.TimeStamp.ToString() + "--";
                                                            UtopiaParser.FailedAt("'BuildingsEffectsNoDate' ", "To:" + getEffects[g].Directed_To_Province_ID + "; Name:" + getEffects[g].OP_Name + "; " + failed, currentUserID);
                                                            break;
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case "Ops":
                                        for (int g = 0; g < getEffects.Count; g++)
                                        {
                                            switch (getEffects[g].Ops.LastOrDefault().Expiration_Date.HasValue)
                                            {
                                                case true:
                                                    DateTime opTime = getEffects[g].Ops.LastOrDefault().Expiration_Date.Value;
                                                    switch (opTime > DateTime.UtcNow)
                                                    {
                                                        case true:
                                                            switch (getEffects[g].OP_Name)
                                                            {

                                                                case "riots":
                                                                    sb.Append(" <span title=\"Riots x " + getEffects[g].Count + " caused by ");
                                                                    foreach (var ite in getEffects[g].Ops)
                                                                        sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + "; ");
                                                                    sb.Append("\" class=\"spanEff\"><img src=\"" + ImagesStatic.Riots + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                                    break;
                                                                case "greedySoldiers":
                                                                    sb.Append(" <span title=\"Greedy Soldiers x " + getEffects[g].Count + "\" class=\"spanEff\"><img src=\"" + ImagesStatic.Greed + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                                    break;
                                                                case "storms":
                                                                case "meteors":
                                                                case "highBirth":
                                                                case "chastity":
                                                                case "inspireArmy":
                                                                case "minorProtection":
                                                                case "fog":
                                                                case "magicShield":
                                                                case "fertileLands":
                                                                case "greatProtection":
                                                                case "naturesBlessing":
                                                                case "fastBuilders":
                                                                case "patriotism":
                                                                case "pitfalls":
                                                                case "explosions":
                                                                case "reflectMagic":
                                                                case "warSpoils":
                                                                case "WarSpoils":
                                                                case "drought":
                                                                case "tornadoes":
                                                                case "vermin":
                                                                case "fountainKnowledge":
                                                                case "clearSight":
                                                                case "incineratesRunes":
                                                                case "townWatch":
                                                                case "thievesInvisible":
                                                                case "fireball":
                                                                case "fanaticism":
                                                                case "aggression":
                                                                case "MagesFury":
                                                                    break;
                                                                default:
                                                                    string failed = string.Empty;
                                                                    foreach (var ite in getEffects[g].Ops)
                                                                        failed += ite.Added_By_Province_ID + "; " + ite.Expiration_Date + "; " + ite.OP_Text + ";" + ite.TimeStamp.ToString() + "--";
                                                                    UtopiaParser.FailedAt("'BuildingOpsBrokenTime'", getEffects[g].Directed_To_Province_ID + "; " + getEffects[g].OP_Name + "; " + failed, currentUserID);
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                default:
                                                    switch (getEffects[g].OP_Name)
                                                    {
                                                        case "stoleMoney":
                                                            sb.Append(" <span title=\"");
                                                            foreach (var ite in getEffects[g].Ops)
                                                                sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + " stole " + ite.OP_Text + "; ");
                                                            sb.Append("\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span><img src=\"" + ImagesStatic.StoleMoney + "\" /></span>");
                                                            break;
                                                        case "stoleFood":
                                                            sb.Append(" <span title=\"");
                                                            foreach (var ite in getEffects[g].Ops)
                                                                sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + " stole " + ite.OP_Text + " bushels; ");
                                                            sb.Append(KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>SF</span>");
                                                            break;
                                                        case "stoleRunes":
                                                            sb.Append(" <span title=\"");
                                                            foreach (var ite in getEffects[g].Ops)
                                                                sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + " stole " + ite.OP_Text + " Runes; ");
                                                            sb.Append(KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>TSR</span>");
                                                            break;
                                                        case "assasinate":
                                                            sb.Append(" <span title=\"");
                                                            foreach (var ite in getEffects[g].Ops)
                                                                sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + " killed " + ite.OP_Text + " troops; ");
                                                            sb.Append(" \"><span class=\"spanEff\">" + getEffects[g].Count + "x</span><img src=\"" + ImagesStatic.Assasinate + "\" /></span>");
                                                            break;
                                                        case "assasinateWizs":
                                                            sb.Append(" <span title=\"");
                                                            foreach (var ite in getEffects[g].Ops)
                                                                sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + " killed " + ite.OP_Text + " wizards; ");
                                                            sb.Append(KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>AW</span>");
                                                            break;
                                                        case "kidnapped":
                                                            sb.Append(" <span title=\"");
                                                            foreach (var ite in getEffects[g].Ops)
                                                                sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + " kidnapped " + ite.OP_Text + " people; ");
                                                            sb.Append(KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>TKN</span>");
                                                            break;
                                                        case "Infiltrated":
                                                            sb.Append(" <span title=\"");
                                                            foreach (var ite in getEffects[g].Ops)
                                                                sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + " Infiltrated to find " + ite.OP_Text + "; ");
                                                            sb.Append(" \"><span class=\"spanEff\"></span><img src=\"" + ImagesStatic.Infiltrated + "\" /></span>");
                                                            break;
                                                        case "bribedGen":
                                                            sb.Append(" <span title=\"");
                                                            foreach (var ite in getEffects[g].Ops)
                                                                sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + " Bribed a General.; ");
                                                            sb.Append(KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>BG</span>");
                                                            break;
                                                        case "burnedAcres":
                                                            sb.Append(" <span title=\"");
                                                            foreach (var ite in getEffects[g].Ops)
                                                                sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + " burned " + ite.OP_Text + "; ");
                                                            sb.Append("\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span><img src=\"" + ImagesStatic.BurnedAcres + "\" /></span>");
                                                            break;
                                                        case "sabotageSpells":
                                                            sb.Append(" <span title=\"Wizards spell casting has been sabatoged.\" class=\"spanEff\"><img src=\"" + ImagesStatic.SabatogedSpells + "\" /></span>");
                                                            break;
                                                        case "bribed":
                                                            sb.Append(" <span title=\"Bribed Thieves x " + getEffects[g].Count + " by ");
                                                            foreach (var ite in getEffects[g].Ops)
                                                                sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + " bribed thieves; ");
                                                            sb.Append(KdPageHelper.PLEASE_MAKE_ICON + "\" class=\"spanEff\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>BT</span>");
                                                            break;
                                                        case "foutainKnowledge":
                                                        case "tornadoes":
                                                        case "reflectingMagic":
                                                        case "plague":
                                                        case "quickFeet":
                                                        case "mystVort":
                                                        case "wakeDead":
                                                        case "landLust":
                                                        case "naturesBlessingFailed"://Nature's Blessing will protect your lands from any droughts and storms the world may see fit to place on you. This spell also has a chance of curing the Plague if your lands are affected by it.
                                                        case "fireball":
                                                        case "convertThieves":
                                                        case "goldToLead":
                                                        case "exposedThieves":
                                                        case "paradise":
                                                        case "shadowlight":
                                                        case "anonymity":
                                                        case "treeGold":
                                                        case "forgetBooks":
                                                        case "fog":
                                                        case "sentMoney":
                                                        case "sentRunes":
                                                        case "reflectMagic":
                                                        case "chastity":
                                                        case "Nightmares":
                                                        case "mystVortFailed":
                                                            break;
                                                        case "stealHorses":
                                                            sb.Append(" <span title=\"Horses Were Stolen x " + getEffects[g].Count + " totallying ");
                                                            int horsesStolen = 0;
                                                            foreach (var ite in getEffects[g].Ops)
                                                                horsesStolen += Convert.ToInt32(ite.OP_Text);
                                                            sb.Append(horsesStolen.ToString("N0") + " horses " + KdPageHelper.PLEASE_MAKE_ICON + "\" class=\"spanEff\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>SH</span>");
                                                            break;
                                                        case "freePrisoners":
                                                            sb.Append(" <span title=\"Prisoners were freed x " + getEffects[g].Count + " totallying ");
                                                            int freePrisoners = 0;
                                                            foreach (var ite in getEffects[g].Ops)
                                                                freePrisoners += Convert.ToInt32(ite.OP_Text);
                                                            sb.Append(freePrisoners.ToString("N0") + " prisoners \" class=\"spanEff\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span><img src=\"" + ImagesStatic.FreedPrisoners + "\" /></span>");
                                                            break;
                                                        case "convertedWizards":
                                                            sb.Append(" <span title=\"Wizards were converted x " + getEffects[g].Count + " totallying ");
                                                            int convertedWizzys = 0;
                                                            foreach (var ite in getEffects[g].Ops)
                                                                convertedWizzys += Convert.ToInt32(ite.OP_Text);
                                                            sb.Append(convertedWizzys.ToString("N0") + " Wizards " + KdPageHelper.PLEASE_MAKE_ICON + "\" class=\"spanEff\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>CW</span>");
                                                            break;
                                                        case "convertedSpecialists":
                                                            sb.Append(" <span title=\"Specialists were converted x " + getEffects[g].Count + " totallying ");
                                                            int convertedSpecs = 0;
                                                            foreach (var ite in getEffects[g].Ops)
                                                                convertedSpecs += Convert.ToInt32(ite.OP_Text);
                                                            sb.Append(convertedSpecs.ToString("N0") + " specialists " + KdPageHelper.PLEASE_MAKE_ICON + "\" class=\"spanEff\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>CS</span>");
                                                            break;
                                                        case "incineratesRunes":
                                                            sb.Append(" <span title=\"Runes were incinerated x " + getEffects[g].Count + " totallying ");
                                                            int runesIncin = 0;
                                                            foreach (var ite in getEffects[g].Ops)
                                                                runesIncin += Convert.ToInt32(ite.OP_Text);
                                                            sb.Append(runesIncin.ToString("N0") + " runes " + KdPageHelper.PLEASE_MAKE_ICON + "\" class=\"spanEff\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>IR</span>");
                                                            break;
                                                        case "killingDragon":
                                                            sb.Append(" <span title=\"Soliders were sent to Kill the Dragon x " + getEffects[g].Count + " totallying ");
                                                            int dragonPoints = 0;
                                                            foreach (var ite in getEffects[g].Ops)
                                                                dragonPoints += Convert.ToInt32(ite.OP_Text);
                                                            sb.Append(dragonPoints.ToString("N0") + " Points " + KdPageHelper.PLEASE_MAKE_ICON + "\" class=\"spanEff\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>KD</span>");
                                                            break;
                                                        case "donatedGoldDragon":
                                                            sb.Append(" <span title=\"Gold was donated to Launch the Dragon x " + getEffects[g].Count + " totallying ");
                                                            int donatedGold = 0;
                                                            foreach (var ite in getEffects[g].Ops)
                                                                donatedGold += Convert.ToInt32(ite.OP_Text);
                                                            sb.Append(donatedGold.ToString("N0") + "gc's " + KdPageHelper.PLEASE_MAKE_ICON + "\" class=\"spanEff\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>DG</span>");
                                                            break;
                                                        case "convertedTroops":
                                                            sb.Append(" <span title=\" ");
                                                            foreach (var ite in getEffects[g].Ops)
                                                                sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + " Converted " + ite.OP_Text + "; ");
                                                            sb.Append(KdPageHelper.PLEASE_MAKE_ICON + "\" class=\"spanEff\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>CT</span>");
                                                            break;
                                                        case "stormsNoEffects":
                                                        case "riotsNoEffects":
                                                        case "goldToLeadNoEffects":
                                                        case "triedToBurnAcres":
                                                            break;
                                                        default:
                                                            //this one has to be a thief op in order to be in the case statement.
                                                            string failed = string.Empty;
                                                            foreach (var ite in getEffects[g].Ops)
                                                                failed += ite.Added_By_Province_ID + "; " + ite.Expiration_Date + "; " + ite.OP_Text + ";" + ite.TimeStamp.ToString() + "--";
                                                            UtopiaParser.FailedAt("'BuildingsOpsNoDate'", getEffects[g].Directed_To_Province_ID + "; " + getEffects[g].OP_Name + "; " + failed, currentUserID);
                                                            break;
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case "Spells":
                                        for (int g = 0; g < getEffects.Count; g++)
                                        {
                                            switch (getEffects[g].Ops.LastOrDefault().Expiration_Date.HasValue)
                                            {
                                                case true:
                                                    DateTime opTime = getEffects[g].Ops.LastOrDefault().Expiration_Date.Value;
                                                    switch (opTime > DateTime.UtcNow)
                                                    {
                                                        case true:
                                                            switch (getEffects[g].OP_Name)
                                                            {
                                                                case "storms":
                                                                    sb.Append(" <span title=\"Storms x " + getEffects[g].Count + "\" class=\"spanEff\"><img src=\"" + ImagesStatic.Storms + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                                    break;
                                                                case "vermin":
                                                                    sb.Append(" <span title=\"Vermin x " + getEffects[g].Count + "\" class=\"spanEff\"><img src=\"" + ImagesStatic.Vermin + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                                    break;
                                                                case "meteors":
                                                                    sb.Append(" <span title=\"Meteors x " + getEffects[g].Count + "\" class=\"spanEff\"><img src=\"" + ImagesStatic.Meteors + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                                    break;
                                                                case "greedySoldiers":
                                                                    sb.Append(" <span title=\"Greedy Soldiers x " + getEffects[g].Count + "\" class=\"spanEff\"><img src=\"" + ImagesStatic.Greed + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                                    break;
                                                                case "highBirth":
                                                                    sb.Append(" <span title=\"High Birth Rates\" class=\"noWrap\"><span class=\"spanEff\"><img src=\"" + ImagesStatic.HighBirthRates + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "inspireArmy":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Inspire Army\"><span class=\"spanEff\"></span><img src=\"" + ImagesStatic.InspireArmy + "\" /><span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "minorProtection":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Minor Protection x " + getEffects[g].Count + "\"><span class=\"spanEff\"><img src=\"" + ImagesStatic.MinorProtection + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "fog":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Fog x " + getEffects[g].Count + "\"><span class=\"spanEff\"><img src=\"" + ImagesStatic.Fog + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "magicShield":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Magic Shield x " + getEffects[g].Count + " \"><span class=\"spanEff\"><img src=\"" + ImagesStatic.MagicSheild + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "fertileLands":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Fertile Lands\"><span class=\"spanEff\"><img src=\"" + ImagesStatic.FertileLands + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "naturesBlessing":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Natures Blessing x " + getEffects[g].Count + " \"><span class=\"spanEff\"><img src=\"" + ImagesStatic.NaturesBlessing + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "fastBuilders":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Building Fast x " + getEffects[g].Count + "\"><span class=\"spanEff\"></span><img src=\"" + ImagesStatic.BuildersBoon + "\" /><span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "patriotism":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Patriots x " + getEffects[g].Count + "\"><span class=\"spanEff\"><img src=\"" + ImagesStatic.BuildersBoon + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "pitfalls":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"PitFalls x " + getEffects[g].Count + "\"><span class=\"spanEff\"><img src=\"" + ImagesStatic.Pitfalls + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "explosions":
                                                                    sb.Append(" <span title=\"Explosions x " + getEffects[g].Count + "\" class=\"spanEff\"><img src=\"" + ImagesStatic.Explosions + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                                    break;
                                                                case "reflectMagic":
                                                                    sb.Append(" <span title=\"Reflecting Magic x " + getEffects[g].Count + "\" class=\"spanEff\"><img src=\"" + ImagesStatic.ReflectingMagic + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                                    break;
                                                                case "fireball":
                                                                    sb.Append(" <span title=\"FireBall x " + getEffects[g].Count + " by ");
                                                                    foreach (var ite in getEffects[g].Ops)
                                                                        sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + "; ");
                                                                    sb.Append("\" class=\"spanEff\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span><img src=\"" + ImagesStatic.Fireball + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                                    break;
                                                                case "warSpoils":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"War Spoils x " + getEffects[g].Count + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>WS<span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "drought":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Droughts x " + getEffects[g].Count + "\"><img src=\"" + ImagesStatic.Drought + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "chastity":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Chastity x " + getEffects[g].Count + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>CH<span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "riots":
                                                                    sb.Append(" <span title=\"Riots x " + getEffects[g].Count + "\" class=\"spanEff\"><img src=\"" + ImagesStatic.Riots + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                                                                    break;
                                                                case "clearSight":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Clear Sight x " + getEffects[g].Count + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>CS<span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "tornadoes":
                                                                    sb.Append(" <span title=\"");
                                                                    foreach (var ite in getEffects[g].Ops)
                                                                        sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + " casted tornadoes, laying waste to " + ite.OP_Text + " acres.; ");
                                                                    sb.Append(KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span><img src=\"" + ImagesStatic.Tornados + "\" /></span>");
                                                                    break;
                                                                case "greatProtection":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Province is protected by Greater Protection \"><span class=\"spanEff\"><img src=\"" + ImagesStatic.GreaterProtection + "\" />" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "fountainKnowledge":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Foutain of Knowledge " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>FoK<span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "thievesInvisible":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Thieves turned Invincible " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>IT<span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "incineratesRunes":
                                                                    sb.Append(" <span title=\"");
                                                                    foreach (var ite in getEffects[g].Ops)
                                                                        sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + " incinerated " + ite.OP_Text + " runes.; ");
                                                                    sb.Append(KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>IR</span>");
                                                                    break;
                                                                case "townWatch":
                                                                    sb.Append(" <span class=\"noWrap\" title=\"Town Watch " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>TW<span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "aggression":
                                                                    sb.Append(" <span title=\"Soldiers fight with Aggression " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>AG<span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "fanaticism":
                                                                    sb.Append(" <span title=\"Troops will fight with Fanatical Fevor " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>FAN<span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                case "MagesFury":
                                                                    sb.Append(" <span title=\"Mages Eyes Burn with Fury" + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>MF<span class=\"spanEff\">" + opTime.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                                                                    break;
                                                                default:
                                                                    string failed = string.Empty;
                                                                    foreach (var ite in getEffects[g].Ops)
                                                                        failed += ite.Added_By_Province_ID + "; " + ite.Expiration_Date + "; " + ite.OP_Text + ";" + ite.TimeStamp.ToString() + "--";
                                                                    UtopiaParser.FailedAt("'BuildingSpells'", getEffects[g].Directed_To_Province_ID + "; " + getEffects[g].OP_Name + "; " + failed, currentUserID);
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                default:
                                                    switch (getEffects[g].OP_Name)
                                                    {
                                                        case "Infiltrated":
                                                        case "stoleRunes":
                                                        case "bribedGen":
                                                        case "stoleMoney":
                                                        case "assasinate":
                                                        case "stoleFood":
                                                        case "kidnapped":
                                                        case "convertTroops":
                                                        case "bribed":
                                                        case "freePrisoners":
                                                        case "burnedAcres":
                                                        case "sabotageSpells":
                                                        case "assasinateWizs":
                                                        case "donatedGoldDragon":
                                                        case "stealHorses":
                                                        case "sentRunes":
                                                        case "chastity":
                                                        case "exposedThieves":
                                                        case "landLust":
                                                        case "fireball":
                                                        case "tornadoes":
                                                        case "reflectMagic":
                                                        case "wakeDead":
                                                        case "plague":
                                                        case "shadowlight":
                                                        case "naturesBlessingFailed"://Nature's Blessing will protect your lands from any droughts and storms the world may see fit to place on you. This spell also has a chance of curing the Plague if your lands are affected by it.
                                                        case "mystVort":
                                                        case "goldToLead":
                                                        case "treeGold":
                                                        case "forgetBooks":
                                                        case "convertThieves":
                                                        case "incineratesRunes":
                                                        case "sentMoney":
                                                        case "convertedWizards":
                                                        case "convertedSpecialists":
                                                        case "convertedTroops":
                                                        case "killingDragon":
                                                        case "riotsNoEffects":
                                                        case "triedToBurnAcres":
                                                            break;
                                                        case "fog":
                                                            sb.Append(" <span class=\"noWrap\" title=\"Fog\"><span class=\"spanEff\"><img src=\"" + ImagesStatic.Fog + "\" /></span></span>");
                                                            break;
                                                        case "anonymity":
                                                            sb.Append(" <span class=\"noWrap\" title=\"Anonymity " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\">Anon</span></span>");
                                                            break;
                                                        case "Nightmares":
                                                            sb.Append(" <span title=\"");
                                                            foreach (var ite in getEffects[g].Ops)
                                                                sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + " gave " + ite.OP_Text + " men nightmares; ");
                                                            sb.Append(" men had nightmares through the night. " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>NM</span>");
                                                            break;
                                                        case "stormsNoEffects":
                                                            sb.Append(" <span title=\" ");
                                                            foreach (var ite in getEffects[g].Ops)
                                                                sb.Append(ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + " Storms Have No Effect; ");
                                                            sb.Append(KdPageHelper.PLEASE_MAKE_ICON + "\" class=\"spanEff\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>SnE</span>");
                                                            break;
                                                        case "mystVortFailed":
                                                            sb.Append(" <span title=\" ");
                                                            foreach (var ite in getEffects[g].Ops)
                                                                sb.Append("Mystic Vortex Failed On " + ownedProvinces.Where(x => x.Province_ID == ite.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault() + "; ");
                                                            sb.Append(KdPageHelper.PLEASE_MAKE_ICON + "\" class=\"spanEff\"><span class=\"spanEff\">" + getEffects[g].Count + "x</span>MVF</span>");
                                                            break;
                                                        default:
                                                            string failed = string.Empty;
                                                            foreach (var ite in getEffects[g].Ops)
                                                                failed += ite.Added_By_Province_ID + "; " + getEffects[g].OP_Name + "; " + ite.Expiration_Date + "; " + ite.OP_Text + ";" + ite.TimeStamp.ToString() + "--";
                                                            UtopiaParser.FailedAt("'BuildingSpellsNoDate'", getEffects[g].Directed_To_Province_ID + "; " + getEffects[g].OP_Name + "; " + failed, currentUserID);
                                                            break;
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    case "Mod Arms %":
                                        if (buildingCheck && getBuildings.Armories_B.HasValue)
                                        {
                                            sb.Remove(sb.Length - 1, 1);
                                            sb.Append(" title=\"" + getBuildings.Armories_B.GetValueOrDefault(0) + "(Arm) / " + item.Land.GetValueOrDefault(0) + "(acres) * " + (double)getBuildings.Building_Efficiency.GetValueOrDefault(1) + "(BE)\">");
                                            var Armories_B = ((double)getBuildings.Armories_B.GetValueOrDefault(0) / (double)item.Land.GetValueOrDefault(0) * (double)getBuildings.Building_Efficiency.GetValueOrDefault(1));
                                            if (Armories_B > 0)
                                                sb.Append(Armories_B.ToString("N1"));
                                        }
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Mod Banks %":
                                        if (buildingCheck && getBuildings.Banks_B.HasValue)
                                        {
                                            sb.Remove(sb.Length - 1, 1);
                                            sb.Append(" title=\"" + getBuildings.Banks_B.GetValueOrDefault(0) + "(Banks) / " + item.Land.GetValueOrDefault(0) + "(acres) * " + (double)getBuildings.Building_Efficiency.GetValueOrDefault(1) + "(BE)\">");
                                            var Banks_B = ((double)getBuildings.Banks_B.GetValueOrDefault(0) / (double)item.Land.GetValueOrDefault(0) * (double)getBuildings.Building_Efficiency.GetValueOrDefault(1));
                                            if (Banks_B > 0)
                                                sb.Append(Banks_B.ToString("N1"));
                                        }
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Mod Barracks %":
                                        if (buildingCheck && getBuildings.Barracks_B.HasValue)
                                        {
                                            sb.Remove(sb.Length - 1, 1);
                                            sb.Append(" title=\"" + getBuildings.Barracks_B.GetValueOrDefault(0) + "(Barracks) / " + item.Land.GetValueOrDefault(0) + "(acres) * " + (double)getBuildings.Building_Efficiency.GetValueOrDefault(1) + "(BE)\">");
                                            var Barracks_B = ((double)getBuildings.Barracks_B.GetValueOrDefault(0) / (double)item.Land.GetValueOrDefault(0) * (double)getBuildings.Building_Efficiency.GetValueOrDefault(1));
                                            if (Barracks_B > 0)
                                                sb.Append(Barracks_B.ToString("N1"));
                                        }
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Mod Dungeons %":
                                        if (buildingCheck && getBuildings.Dungeons_B.HasValue)
                                        {
                                            sb.Remove(sb.Length - 1, 1);
                                            sb.Append(" title=\"" + getBuildings.Dungeons_B.GetValueOrDefault(0) + "(Dungeons) / " + item.Land.GetValueOrDefault(0) + "(acres) * " + (double)getBuildings.Building_Efficiency.GetValueOrDefault(1) + "(BE)\">");
                                            var Dungeons_B = ((double)getBuildings.Dungeons_B.GetValueOrDefault(0) / (double)item.Land.GetValueOrDefault(0) * (double)getBuildings.Building_Efficiency.GetValueOrDefault(1));
                                            if (Dungeons_B > 0)
                                                sb.Append(Dungeons_B.ToString("N1"));
                                        }
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Mod Farms %":
                                        if (buildingCheck && getBuildings.Farms_B.HasValue)
                                        {
                                            sb.Remove(sb.Length - 1, 1);
                                            sb.Append(" title=\"" + getBuildings.Farms_B.GetValueOrDefault(0) + "(Farms) / " + item.Land.GetValueOrDefault(0) + "(acres) * " + (double)getBuildings.Building_Efficiency.GetValueOrDefault(1) + "(BE)\">");
                                            var Farms_B = ((double)getBuildings.Farms_B.GetValueOrDefault(0) / (double)item.Land.GetValueOrDefault(0) * (double)getBuildings.Building_Efficiency.GetValueOrDefault(1));
                                            if (Farms_B > 0)
                                                sb.Append(Farms_B.ToString("N1"));
                                        }
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Mod Forts %":
                                        if (buildingCheck && getBuildings.Forts_B.HasValue)
                                        {
                                            sb.Remove(sb.Length - 1, 1);
                                            sb.Append(" title=\"" + getBuildings.Forts_B.GetValueOrDefault(0) + "(Forts) / " + item.Land.GetValueOrDefault(0) + "(acres) * " + (double)getBuildings.Building_Efficiency.GetValueOrDefault(1) + "(BE)\">");
                                            var Forts_B = ((double)getBuildings.Forts_B.GetValueOrDefault(0) / (double)item.Land.GetValueOrDefault(0) * (double)getBuildings.Building_Efficiency.GetValueOrDefault(1));
                                            if (Forts_B > 0)
                                                sb.Append(Forts_B.ToString("N1"));
                                        }
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Mod GS %":
                                        if (buildingCheck && getBuildings.GS_B.HasValue)
                                        {
                                            sb.Remove(sb.Length - 1, 1);
                                            sb.Append(" title=\"" + getBuildings.GS_B.GetValueOrDefault(0) + "(GS) / " + item.Land.GetValueOrDefault(0) + "(acres) * " + (double)getBuildings.Building_Efficiency.GetValueOrDefault(1) + "(BE)\">");
                                            var GS_B = ((double)getBuildings.GS_B.GetValueOrDefault(0) / (double)item.Land.GetValueOrDefault(0) * (double)getBuildings.Building_Efficiency.GetValueOrDefault(1));
                                            if (GS_B > 0)
                                                sb.Append(GS_B.ToString("N1"));
                                        }
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Mod Guilds %":
                                        if (buildingCheck && getBuildings.Guilds_B.HasValue)
                                        {
                                            sb.Remove(sb.Length - 1, 1);
                                            sb.Append(" title=\"" + getBuildings.Guilds_B.GetValueOrDefault(0) + "(Guilds) / " + item.Land.GetValueOrDefault(0) + "(acres) * " + (double)getBuildings.Building_Efficiency.GetValueOrDefault(1) + "(BE)\">");
                                            var Guilds_B = ((double)getBuildings.Guilds_B.GetValueOrDefault(0) / (double)item.Land.GetValueOrDefault(0) * (double)getBuildings.Building_Efficiency.GetValueOrDefault(1));
                                            if (Guilds_B > 0)
                                                sb.Append(Guilds_B.ToString("N1"));
                                        }
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Mod Homes %":
                                        if (buildingCheck && getBuildings.Homes_B.HasValue)
                                        {
                                            sb.Remove(sb.Length - 1, 1);
                                            sb.Append(" title=\"" + getBuildings.Homes_B.GetValueOrDefault(0) + "(Homes) / " + item.Land.GetValueOrDefault(0) + "(acres) * " + (double)getBuildings.Building_Efficiency.GetValueOrDefault(1) + "(BE)\">");
                                            var Homes_B = ((double)getBuildings.Homes_B.GetValueOrDefault(0) / (double)item.Land.GetValueOrDefault(0) * (double)getBuildings.Building_Efficiency.GetValueOrDefault(1));
                                            if (Homes_B > 0)
                                                sb.Append(Homes_B.ToString("N1"));
                                        }
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Mod Hospitals %":
                                        if (buildingCheck && getBuildings.Hospitals_B.HasValue)
                                        {
                                            sb.Remove(sb.Length - 1, 1);
                                            sb.Append(" title=\"" + getBuildings.Hospitals_B.GetValueOrDefault(0) + "(Hospitals) / " + item.Land.GetValueOrDefault(0) + "(acres) * " + (double)getBuildings.Building_Efficiency.GetValueOrDefault(1) + "(BE)\">");
                                            var Hospitals_B = ((double)getBuildings.Hospitals_B.GetValueOrDefault(0) / (double)item.Land.GetValueOrDefault(0) * (double)getBuildings.Building_Efficiency.GetValueOrDefault(1));
                                            if (Hospitals_B > 0)
                                                sb.Append(Hospitals_B.ToString("N1"));
                                        }
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Mod Library %":
                                        if (buildingCheck && getBuildings.Library_B.HasValue)
                                        {
                                            sb.Remove(sb.Length - 1, 1);
                                            sb.Append(" title=\"" + getBuildings.Library_B.GetValueOrDefault(0) + "(Libraries) / " + item.Land.GetValueOrDefault(0) + "(acres) * " + (double)getBuildings.Building_Efficiency.GetValueOrDefault(1) + "(BE)\">");
                                            var Library_B = ((double)getBuildings.Library_B.GetValueOrDefault(0) / (double)item.Land.GetValueOrDefault(0) * (double)getBuildings.Building_Efficiency.GetValueOrDefault(1));
                                            if (Library_B > 0)
                                                sb.Append(Library_B.ToString("N1"));
                                        }
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Mod Mills %":
                                        if (buildingCheck && getBuildings.Mills_B.HasValue)
                                        {
                                            sb.Remove(sb.Length - 1, 1);
                                            sb.Append(" title=\"" + getBuildings.Mills_B.GetValueOrDefault(0) + "(Mills) / " + item.Land.GetValueOrDefault(0) + "(acres) * " + (double)getBuildings.Building_Efficiency.GetValueOrDefault(1) + "(BE)\">");
                                            var Mills_B = ((double)getBuildings.Mills_B.GetValueOrDefault(0) / (double)item.Land.GetValueOrDefault(0) * (double)getBuildings.Building_Efficiency.GetValueOrDefault(1));
                                            if (Mills_B > 0)
                                                sb.Append(Mills_B.ToString("N1"));
                                        }
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Mod Schools %":
                                        if (buildingCheck && getBuildings.Schools_B.HasValue)
                                        {
                                            sb.Remove(sb.Length - 1, 1);
                                            sb.Append(" title=\"" + getBuildings.Schools_B.GetValueOrDefault(0) + "(Schools) / " + item.Land.GetValueOrDefault(0) + "(acres) * " + (double)getBuildings.Building_Efficiency.GetValueOrDefault(1) + "(BE)\">");
                                            var Schools_B = ((double)getBuildings.Schools_B.GetValueOrDefault(0) / (double)item.Land.GetValueOrDefault(0) * (double)getBuildings.Building_Efficiency.GetValueOrDefault(1));
                                            if (Schools_B > 0)
                                                sb.Append(Schools_B.ToString("N1"));
                                        }
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Mod Stables %":
                                        if (buildingCheck && getBuildings.Stables_B.HasValue)
                                        {
                                            sb.Remove(sb.Length - 1, 1);
                                            sb.Append(" title=\"" + getBuildings.Stables_B.GetValueOrDefault(0) + "(Stables) / " + item.Land.GetValueOrDefault(0) + "(acres) * " + (double)getBuildings.Building_Efficiency.GetValueOrDefault(1) + "(BE)\">");
                                            var Stables_B = ((double)getBuildings.Stables_B.GetValueOrDefault(0) / (double)item.Land.GetValueOrDefault(0) * (double)getBuildings.Building_Efficiency.GetValueOrDefault(1));
                                            if (Stables_B > 0)
                                                sb.Append(Stables_B.ToString("N1"));
                                        }
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Mod T Grounds %":
                                        if (buildingCheck && getBuildings.TG_B.HasValue)
                                        {
                                            sb.Remove(sb.Length - 1, 1);
                                            sb.Append(" title=\"" + getBuildings.TG_B.GetValueOrDefault(0) + "(TG) / " + item.Land.GetValueOrDefault(0) + "(acres) * " + (double)getBuildings.Building_Efficiency.GetValueOrDefault(1) + "(BE)\">");
                                            var TG_B = ((double)getBuildings.TG_B.GetValueOrDefault(0) / (double)item.Land.GetValueOrDefault(0) * (double)getBuildings.Building_Efficiency.GetValueOrDefault(1));
                                            if (TG_B > 0)
                                                sb.Append(TG_B.ToString("N1"));
                                        }
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Mod TD %":
                                        if (buildingCheck && getBuildings.TD_B.HasValue)
                                        {
                                            sb.Remove(sb.Length - 1, 1);
                                            sb.Append(" title=\"" + getBuildings.TD_B.GetValueOrDefault(0) + "(TD) / " + item.Land.GetValueOrDefault(0) + "(acres) * " + (double)getBuildings.Building_Efficiency.GetValueOrDefault(1) + "(BE)\">");
                                            var TD_B = ((double)getBuildings.TD_B.GetValueOrDefault(0) / (double)item.Land.GetValueOrDefault(0) * (double)getBuildings.Building_Efficiency.GetValueOrDefault(1));
                                            if (TD_B > 0)
                                                sb.Append(TD_B.ToString("N1"));
                                        }
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Mod Towers %":
                                        if (buildingCheck && getBuildings.Towers_B.HasValue)
                                        {
                                            sb.Remove(sb.Length - 1, 1);
                                            sb.Append(" title=\"" + getBuildings.Towers_B.GetValueOrDefault(0) + "(Towers) / " + item.Land.GetValueOrDefault(0) + "(acres) * " + (double)getBuildings.Building_Efficiency.GetValueOrDefault(1) + "(BE)\">");
                                            var Towers_B = ((double)getBuildings.Towers_B.GetValueOrDefault(0) / (double)item.Land.GetValueOrDefault(0) * (double)getBuildings.Building_Efficiency.GetValueOrDefault(1));
                                            if (Towers_B > 0)
                                                sb.Append(Towers_B.ToString("N1"));
                                        }
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Mod WT %":
                                        if (buildingCheck && getBuildings.WT_B.HasValue)
                                        {
                                            sb.Remove(sb.Length - 1, 1);
                                            sb.Append(" title=\"" + getBuildings.WT_B.GetValueOrDefault(0) + "(WT) / " + item.Land.GetValueOrDefault(0) + "(acres) * " + (double)getBuildings.Building_Efficiency.GetValueOrDefault(1) + "(BE)\">");
                                            var WT_B = ((double)getBuildings.WT_B.GetValueOrDefault(0) / (double)item.Land.GetValueOrDefault(0) * (double)getBuildings.Building_Efficiency.GetValueOrDefault(1));
                                            if (WT_B > 0)
                                                sb.Append(WT_B.ToString("N1"));
                                        }
                                        else
                                            sb.Append("-");
                                        break;

                                    case "Magic Sci %":
                                        if (scienceCheck && getSciences.SOS_Magic_Percent.HasValue)
                                            sb.Append(getSciences.SOS_Magic_Percent.Value.ToString("N1"));
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Magic Sci pts":
                                        if (scienceCheck && getSciences.SOS_Magic.HasValue)
                                            sb.Append(getSciences.SOS_Magic.Value.ToString("N0"));
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Food Sci %":
                                        if (scienceCheck && getSciences.SOS_Food_Percent.HasValue)
                                            sb.Append(getSciences.SOS_Food_Percent.Value.ToString("N1"));
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Food Sci pts":
                                        if (scienceCheck && getSciences.SOS_Food.HasValue)
                                            sb.Append(getSciences.SOS_Food.Value.ToString("N0"));
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Pop Sci %":
                                        if (scienceCheck && getSciences.SOS_Housing_Percent.HasValue)
                                            sb.Append(getSciences.SOS_Housing_Percent.Value.ToString("N1"));
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Population Sci pts":
                                        if (scienceCheck && getSciences.SOS_Housing.HasValue)
                                            sb.Append(getSciences.SOS_Housing.Value.ToString("N0"));
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Buildings Sci %":
                                        if (scienceCheck && getSciences.SOS_Tools_Percent.HasValue)
                                            sb.Append(getSciences.SOS_Tools_Percent.Value.ToString("N1"));
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Building Sci pts":
                                        if (scienceCheck && getSciences.SOS_Tools.HasValue)
                                            sb.Append(getSciences.SOS_Tools.Value.ToString("N0"));
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Income Sci %":
                                        if (scienceCheck && getSciences.SOS_Alchemy_Percent.HasValue)
                                            sb.Append(getSciences.SOS_Alchemy_Percent.Value.ToString("N1"));
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Income Sci pts":
                                        if (scienceCheck && getSciences.SOS_Alchemy.HasValue)
                                            sb.Append(getSciences.SOS_Alchemy.Value.ToString("N0"));
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Thief Sci %":
                                        if (scienceCheck && getSciences.SOS_Thieves_Percent.HasValue)
                                            sb.Append(getSciences.SOS_Thieves_Percent.Value.ToString("N1"));
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Thief Sci pts":
                                        if (scienceCheck && getSciences.SOS_Thieves.HasValue)
                                            sb.Append(getSciences.SOS_Thieves.Value.ToString("N0"));
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Military Sci %":
                                        if (scienceCheck && getSciences.SOS_Miltary_Percent.HasValue)
                                            sb.Append(getSciences.SOS_Miltary_Percent.Value.ToString("N1"));
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Military Sci pts":
                                        if (scienceCheck && getSciences.SOS_Military.HasValue)
                                            sb.Append(getSciences.SOS_Military.Value.ToString("N0"));
                                        else
                                            sb.Append("-");
                                        break;
                                    case "Total sci": switch (scienceCheck)
                                        {
                                            case true:
                                                sb.Append((getSciences.SOS_Alchemy.GetValueOrDefault(0) + getSciences.SOS_Food.GetValueOrDefault(0) + getSciences.SOS_Housing.GetValueOrDefault(0) + getSciences.SOS_Magic.GetValueOrDefault(0) + getSciences.SOS_Military.GetValueOrDefault(0) + getSciences.SOS_Thieves.GetValueOrDefault(0) + getSciences.SOS_Tools.GetValueOrDefault(0)).ToString("N0"));
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                    case "Sci pts/acre": switch (scienceCheck)
                                        {
                                            case true:
                                                decimal totals = getSciences.SOS_Alchemy.GetValueOrDefault(0) + getSciences.SOS_Food.GetValueOrDefault(0) + getSciences.SOS_Housing.GetValueOrDefault(0) + getSciences.SOS_Magic.GetValueOrDefault(0) + getSciences.SOS_Military.GetValueOrDefault(0) + getSciences.SOS_Thieves.GetValueOrDefault(0) + getSciences.SOS_Tools.GetValueOrDefault(0);
                                                sb.Append((totals / item.Land).HasValue ? ((totals / item.Land).Value.ToString("N2")) : "-");
                                                break;
                                            default:
                                                sb.Append("-");
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }
                        sb.Append("</td>");
                    }
                    sb.Append("</tr>");
                    //}
                    //else
                    //{//If there is a province missing its data, make sure It doesn't happen again by removing all the provinces from the kingdom.
                    //    CachedItems.RemoveProvinceFromKingdomCache(ownerKingdomID, item);
                    //}
                }

                string dragons = string.Empty;
                switch (kingdomlessProvinces)
                {
                    case false:
                        if (totalGold > 1)
                            for (int ii = 0; ii < (int)(totalGold / (dragonNetworth * (decimal)1.75)); ii++)
                                dragons += "<img src=\"http://codingforcharity.org/utopiapimp/img/icons/dragon_dark.gif\"  title=\"dragon = totalGold / (TotalNetworth * 1.75)\" />";
                        break;
                }
                sb.Append("</tbody>");
                sb.Append("<tfoot>" + sbFooter.Replace("OffSpecsFooter", offSpecs.ToString("N0")).Replace("DefSpecsFooter", defSpecs.ToString("N0")).Replace("PeasantsFooter", peasants.ToString("N0")).Replace("HorsesFooter", horses.ToString("N0")).Replace("PopulationFooter", population.ToString("N0")).Replace("DailyIncomeFooter", dailyIncome.ToString("N0")).Replace("PrisonersFooter", prisoners.ToString("N0")).Replace("TradeFooter", tradeBalance.ToString("N0")).Replace("SoldiersFooter", soldiers.ToString("N0")).Replace("RunesFooter", runes.ToString("N0")).Replace("NWFooter", dragonNetworth.ToString("N0")).Replace("FoodFooter", food.ToString("N0")).Replace("AcresFooter", acres.ToString("N0")).Replace("GoldFooter", totalGold.ToString("N0") + "<br/>" + dragons).ToString() + "</tfoot>");
                sb.Append("</table>");

                return sb.ToString();
            }
            else
            {
                string text = "<h3>You must First Choose Your columns to display.  <br/><br/>Go to the <a href=\"Columns.aspx\">Column Chooser</a>.</h3>";
                if (monType != MonarchType.none && monType != MonarchType.kdMonarch)
                    text += "<br/><br/><h3>Kingdom Owners/Monarch:</h3>  Go set up your <a href=\"Columns.aspx?id=dft\">Default Kingdom Columns</a> to help new players out.";
                return text;
            }
        }


        /// <summary>
        /// Loads the ops history of the kingdom
        /// </summary>
        /// <param name="kdType"></param>
        /// <param name="kingdomID"></param>
        /// <param name="ownerKingdomID"></param>
        /// <param name="currentCachedUser"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static string loadOpsHistory(string kdType, Guid kingdomID, Guid ownerKingdomID, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            List<ProvinceClass> provinceIdentifiers = UtopiaParser.GetProvincesInKingdomToDisplay(kdType, kingdomID, ownerKingdomID, cachedKingdom);
            List<ProvinceClass> ownedProvinces = UtopiaParser.GetProvincesInKingdomToDisplay("none", ownerKingdomID, ownerKingdomID, cachedKingdom);
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();

            StringBuilder sb = new StringBuilder();
            StringBuilder sbLoad = new StringBuilder();
            var GetKingdomData = (from UPI in provinceIdentifiers
                                  select UPI.Province_ID).Take(1000).ToList();
            var getProvinceEffectss = cachedKingdom.Effects;
            var AttackQuery = cachedKingdom.Attacks;

            List<OpsCompleted> allOps = new List<OpsCompleted>();
            for (int i = 0; i < AttackQuery.Count; i++)
            {
                OpsCompleted item = new OpsCompleted();
                item.PostedBy = ownedProvinces.Where(x => x.Province_ID == AttackQuery[i].Province_ID_Added).Select(x => x.Province_Name).FirstOrDefault();
                item.PostedTime = AttackQuery[i].DateTime_Added;
                item.Target = provinceIdentifiers.Where(x => x.Province_ID == AttackQuery[i].Province_ID_Attacked).Select(x => x.Province_Name).FirstOrDefault();
                item.TimeDate = AttackQuery[i].DateTime_Added.ToShortRelativeDate();
                item.Type = "<img src=\"" + ImagesStatic.ElitesOut + "\" />";
                item.name = "attack";
                item.ExpirationDate = AttackQuery[i].Time_To_Return.GetValueOrDefault().Subtract(DateTime.UtcNow).ToShortRelativeDate();
                if (AttackQuery[i].Mod_Off_Sent.HasValue)
                    item.alt = AttackQuery[i].Mod_Off_Sent.Value + " sent";
                allOps.Add(item);
            }
            for (int i = 0; i < getProvinceEffectss.Count; i++)
            {
                OpsCompleted item = new OpsCompleted();
                item.PostedBy = ownedProvinces.Where(x => x.Province_ID == getProvinceEffectss[i].Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault();
                item.PostedTime = getProvinceEffectss[i].TimeStamp;
                item.Target = provinceIdentifiers.Where(x => x.Province_ID == getProvinceEffectss[i].Directed_To_Province_ID).Select(x => x.Province_Name).FirstOrDefault();
                item.TimeDate = getProvinceEffectss[i].TimeStamp.ToShortRelativeDate();
                item.name = getProvinceEffectss[i].OP_Name;
                item.ExpirationDate = getProvinceEffectss[i].Expiration_Date.GetValueOrDefault().Subtract(DateTime.UtcNow).ToShortRelativeDate();
                switch (getProvinceEffectss[i].OP_Name)
                {
                    case "Infiltrated":
                        item.alt = "Found " + getProvinceEffectss[i].OP_Text + "";
                        item.Type = "<img src=\"" + ImagesStatic.Infiltrated + "\" />";
                        break;
                    case "stoleRunes":
                        item.alt = "Stole " + getProvinceEffectss[i].OP_Text + " runes" + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "TSR";
                        break;
                    case "sentRunes":
                        item.alt = "Sent " + getProvinceEffectss[i].OP_Text + " runes" + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "SAR";
                        break;
                    case "stoleFood":
                        item.alt = "Stole " + getProvinceEffectss[i].OP_Text + " bushels" + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "SF";
                        break;
                    case "stoleMoney":
                        item.alt = "Stole " + getProvinceEffectss[i].OP_Text;
                        item.Type = "<img src=\"" + ImagesStatic.StoleMoney + "\" />";
                        break;
                    case "bribedGen":
                        item.alt = "Bribed a General" + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "BG";
                        break;
                    case "assasinate":
                        item.alt = "Killed " + getProvinceEffectss[i].OP_Text + " troops";
                        item.Type = "<img src=\"" + ImagesStatic.Assasinate + "\" />";
                        break;
                    case "kidnapped":
                        item.alt = "Kidnapped " + getProvinceEffectss[i].OP_Text + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "TKN";
                        break;
                    case "convertTroops":
                        item.alt = "Converted " + getProvinceEffectss[i].OP_Text + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "CT";
                        break;
                    case "bribed":
                        item.alt = "Bribed " + getProvinceEffectss[i].OP_Text + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "BT";
                        break;
                    case "freePrisoners":
                        item.alt = "Freed " + getProvinceEffectss[i].OP_Text + " Prisoners ";
                        item.Type = "<img src=\"" + ImagesStatic.FreedPrisoners + "\" />";
                        break;
                    case "burnedAcres":
                        item.alt = "Burned " + getProvinceEffectss[i].OP_Text + " Acres " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "BA";
                        break;
                    case "anonymity":
                        item.alt = "Anonymous Op " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "Anon";
                        break;
                    case "storms":
                        item.alt = "Storms affect Province";
                        item.Type = "<img src=\"" + ImagesStatic.Storms + "\" />";
                        break;
                    case "vermin":
                        item.alt = "Vermin";
                        item.Type = "<img src=\"" + ImagesStatic.Vermin + "\" />";
                        break;
                    case "meteors":
                        item.alt = "Meteors affect Province";
                        item.Type = "<img src=\"" + ImagesStatic.Meteors + "\" />";
                        break;
                    case "greedySoldiers":
                        item.alt = "Soldiers are now Greedy";
                        item.Type = "<img src=\"" + ImagesStatic.Greed + "\" />";
                        break;
                    case "highBirth":
                        item.alt = "High Birth Rates from Love and Peace";
                        item.Type = "<img src=\"" + ImagesStatic.HighBirthRates + "\" />";
                        break;
                    case "inspireArmy":
                        item.alt = "Inspired Army to train harder";
                        item.Type = "<img src=\"" + ImagesStatic.InspireArmy + "\" />";
                        break;
                    case "minorProtection":
                        item.alt = "Minor Protection";
                        item.Type = "<img src=\"" + ImagesStatic.MinorProtection + "\" />";
                        break;
                    case "fog":
                        item.alt = "Fog affects Province";
                        item.Type = "<img src=\"" + ImagesStatic.Fog + "\" />";
                        break;
                    case "magicShield":
                        item.alt = "Magic Shield on Province";
                        item.Type = "<img src=\"" + ImagesStatic.MagicSheild + "\" />";
                        break;
                    case "fertileLands":
                        item.alt = "Fertile Lands";
                        item.Type = "<img src=\"" + ImagesStatic.FertileLands + "\" />";
                        break;
                    case "townWatch":
                        item.alt = "Town Watch " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "TW";
                        break;
                    case "naturesBlessing":
                        item.alt = "Casted Natures Blessing";
                        item.Type = "<img src=\"" + ImagesStatic.NaturesBlessing + "\" />";
                        break;
                    case "fastBuilders":
                        item.alt = "Speed Builders";
                        item.Type = "<img src=\"" + ImagesStatic.BuildersBoon + "\" />";
                        break;
                    case "patriotism":
                        item.alt = "Patriots Defend the land";
                        item.Type = "<img src=\"" + ImagesStatic.Patriotism + "\" />";
                        break;
                    case "pitfalls":
                        item.alt = "Pitfalls affect Province";
                        item.Type = "<img src=\"" + ImagesStatic.Pitfalls + "\" />";
                        break;
                    case "explosions":
                        item.alt = "Explosions Rock Aid Shipments";
                        item.Type = "<img src=\"" + ImagesStatic.Explosions + "\" />";
                        break;
                    case "reflectMagic":
                        item.alt = "Reflecting Magic";
                        item.Type = "<img src=\"" + ImagesStatic.ReflectingMagic + "\" />";
                        break;
                    case "warSpoils":
                        item.alt = "War Spoils " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "WS";
                        break;
                    case "drought":
                        item.alt = "Drought affects Province";
                        item.Type = "<img src=\"" + ImagesStatic.Drought + "\" />";
                        break;
                    case "chastity":
                        item.alt = "Chastity affects Province" + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "CH";
                        break;
                    case "clearSight":
                        item.alt = "Clear Sight " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "CS";
                        break;
                    case "riots":
                        item.alt = "Riots affect Province";
                        item.Type = "<img src=\"" + ImagesStatic.Riots + "\" />";
                        break;
                    case "landLust":
                        item.alt = "Land Lust " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "LL";
                        break;
                    case "fireball":
                        item.alt = "Fireball";
                        item.Type = "<img src=\"" + ImagesStatic.Fireball + "\" />";
                        break;
                    case "tornadoes":
                        item.alt = "Tornadoes " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "T";
                        break;
                    case "wakeDead":
                        item.alt = "Waking Dead to fight";
                        item.Type = "<img src=\"" + ImagesStatic.WakeDead + "\" />";
                        break;
                    case "plague":
                        item.alt = "NOTALLOWED";// "Province has Plague";
                        //item.Type = "<img src=\""+ImagesStatic.Plague+"\" />";
                        break;
                    case "naturesBlessingFailed":
                        item.alt = "Storms or Drought Failed because of Natures Blessing";
                        item.Type = "<img src=\"" + ImagesStatic.NaturesBlessingFailed + "\" />";
                        break;
                    case "mystVort":
                        item.alt = "Mystic Vortex, " + getProvinceEffectss[i].OP_Text;
                        item.Type = "<img src=\"" + ImagesStatic.MysticVortex + "\" />";
                        break;
                    case "goldToLead":
                        item.alt = "Gold was turned to Lead ";
                        item.Type = "<img src=\"" + ImagesStatic.GoldToLead + "\" />";
                        break;
                    case "treeGold":
                        item.alt = "Gold fell from the Trees " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "TOG";
                        break;
                    case "convertThieves":
                        item.alt = "Converted some thieves to guild " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "CT";
                        break;
                    case "exposedThieves":
                        item.alt = "Exposed thieves ";
                        item.Type = "<img src=\"" + ImagesStatic.ExposedThieves + "\" />";
                        break;
                    case "landShadowLight":
                    case "shadowlight":
                        item.alt = "Lands Blessed with Shadow of Light";
                        item.Type = "<img src=\"" + ImagesStatic.ShadowOfLight + "\" />";
                        break;
                    case "greatProtection":
                        item.alt = "Province is protected with Greater Protection";
                        item.Type = "<img src=\"" + ImagesStatic.GreaterProtection + "\" />";
                        break;
                    case "sabotageSpells":
                        item.alt = "Wizards spell casting has been sabatoged. ";
                        item.Type = "<img src=\"" + ImagesStatic.SabatogedSpells + "\" />";
                        break;
                    case "paradise":
                        item.alt = "Paradised for " + getProvinceEffectss[i].OP_Text + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "PA";
                        break;
                    case "assasinateWizs":
                        item.alt = "assassinated " + getProvinceEffectss[i].OP_Text + " Wizards " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "AW";
                        break;
                    case "forgetBooks":
                        item.alt = "Forgot " + getProvinceEffectss[i].OP_Text + " Books " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "FB";
                        break;
                    case "donatedGoldDragon":
                        item.alt = "Donated " + getProvinceEffectss[i].OP_Text + "gc to the quest of launching the Dragon. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "DGD";
                        break;
                    case "fountainKnowledge":
                        item.alt = "Foutain of Knowledge allows your students to learn faster " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "FoK";
                        break;
                    case "thievesInvisible":
                        item.alt = "Our Thieves are now Partionally Invincible";
                        item.Type = "<img src=\"" + ImagesStatic.InvincibleThieves + "\" />";
                        break;
                    case "incineratesRunes":
                        item.alt = getProvinceEffectss[i].OP_Text + " runes were incinerated. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "IR";
                        break;
                    case "stealHorses":
                        item.alt = "Stole " + getProvinceEffectss[i].OP_Text + " War Horses. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "SH";
                        break;
                    case "sentMoney":
                        item.alt = "Sent " + getProvinceEffectss[i].OP_Text + "gcs in Aid. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "SM";
                        break;
                    case "aggression":
                        item.alt = "Province has a unique Aggression. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "AG";
                        break;
                    case "convertedWizards":
                        item.alt = "Converted Wizards to Guild. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "CW";
                        break;
                    case "killingDragon":
                        item.alt = "Send Soldiers to Weaken the dragon by " + getProvinceEffectss[i].OP_Text + " points. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "KD";
                        break;
                    case "fanaticism":
                        item.alt = "Troops will fight with Fanatical Fevor. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "FAN";
                        break;
                    case "convertedSpecialists":
                        item.alt = "Converted " + getProvinceEffectss[i].OP_Text + " Specialists. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "CS";
                        break;
                    case "convertedTroops":
                        item.alt = "Converted " + getProvinceEffectss[i].OP_Text + " Troops. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "CT";
                        break;
                    case "armySpeed":
                        item.alt = "Your Army is Blessed with Quick Feet for their next Attack. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "QF";
                        break;
                    case "MagesFury":
                        item.alt = "Your Mages Eyes Burn with Fury. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "MF";
                        break;
                    case "Nightmares":
                        item.alt = "Gave " + getProvinceEffectss[i].OP_Text + " Men Nightmares. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "NM";
                        break;
                    case "WarSpoils":
                        item.alt = "Had War Spoils. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "WS";
                        break;
                    case "stormsNoEffects":
                        item.alt = "Storms Had No Effect on this Province. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "SnE";
                        break;
                    case "riotsNoEffects":
                        item.alt = "Riots Had No Effect on this Province. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "RnE";
                        break;
                    case "goldToLeadNoEffects":
                        item.alt = "Gold was Attempted To Be Turned To Lead. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "GnE";
                        break;
                    case "mystVortFailed":
                        item.alt = "Mystic Vortex Failed On this Province. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "MVF";
                        break;
                    case "triedToBurnAcres":
                        item.alt = "Tried to burn the Acres of this Province. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "TBA";
                        break;
                    case "convertedTroopsFailed":
                        item.alt = "Failed to convert any troops. " + KdPageHelper.PLEASE_MAKE_ICON;
                        item.Type = "FtCT";
                        break;
                    default:
                        item.Type = "Something Broke, Will be Fixed soon.";
                        UtopiaParser.FailedAtTesting("'OpHistoryBroken'", getProvinceEffectss[i].OP_Name + ";" + getProvinceEffectss[i].OP_Text, currentUser.PimpUser.UserID);
                        break;
                }
                allOps.Add(item);
            }
            allOps.OrderByDescending(xx => xx.PostedTime);
            if (allOps.Count > 0)
            {
                //Ops grouped by Target
                var getGroupedOpsByTarget = (from xx in allOps
                                             group xx by xx.Target into yy
                                             select new
                                             {
                                                 yy.Key,
                                                 op = (from zz in yy
                                                       group zz by zz.name into aa
                                                       select new
                                                       {
                                                           aa.Key,
                                                           bb = (from cc in aa
                                                                 select cc).ToList()
                                                       })
                                             });

                sb.Append("<div class='divProvinceDetailsContainer'><b>Sorted By Target</b><ul class='ulProvinceDetails'>");
                foreach (var province in getGroupedOpsByTarget.OrderBy(x => x.Key))
                {
                    sb.Append("<li class='Bold'>- ");
                    sb.Append(province.Key + " (" + province.op.Count() + ")");
                    sb.Append("</li>");
                    sb.Append("<li><table id=\"tableInfo\" class=\"tblKingdomInfo paddingLeft\">");
                    sb.Append("<thead><tr>");
                    sb.Append("<th>Type</th><th>Count</th><th>Last Performed By</th><th class=\"{sorter: 'fancyNumber'}\">Last Performed</th><th class=\"{sorter: 'fancyNumber'}\">Expires In</th>");
                    sb.Append("</tr></thead>");
                    int i = 0;
                    foreach (var op in province.op)
                    {
                        if (op.bb.FirstOrDefault().alt != "NOTALLOWED")
                        {
                            sb.Append("<tr title=\"");
                            for (int j = op.bb.Count() - 1; j > -1; j--)
                                sb.Append(op.bb[j].alt + " by " + op.bb[j].PostedBy + "; ");
                            switch (i % 2)
                            {
                                case 1:
                                    sb.Append("\" class=\"d0\">");
                                    break;
                                case 0:
                                    sb.Append("\" class=\"d0\">");
                                    break;
                            }
                            sb.Append("<td>" + op.bb.FirstOrDefault().Type + "</td>");
                            sb.Append("<td>" + op.bb.Count() + "</td>");
                            sb.Append("<td>" + op.bb.LastOrDefault().PostedBy + "</td>");
                            sb.Append("<td>" + op.bb.LastOrDefault().TimeDate + "</td>");
                            sb.Append("<td>" + op.bb.LastOrDefault().ExpirationDate + "</td>");
                            sb.Append("</tr>");
                        }
                        i += 1;
                    }
                    sb.Append("</table></li>");
                }
                sb.Append("</ul></div>");

                //Ops grouped by Attacker
                var getGroupedOpsByAttacker = (from xx in allOps
                                               group xx by xx.PostedBy into yy
                                               select new
                                               {
                                                   yy.Key,
                                                   op = (from zz in yy
                                                         group zz by zz.name into aa
                                                         select new
                                                         {
                                                             aa.Key,
                                                             bb = (from cc in aa
                                                                   select cc).ToList()
                                                         })
                                               });
                sb.Append("<div class='divProvinceDetailsContainer'><b>Sorted By Attacker</b><ul class='ulProvinceDetails'>");
                foreach (var province in getGroupedOpsByAttacker.OrderBy(x => x.Key))
                {
                    sb.Append("<li class='Bold'>- ");
                    sb.Append(province.Key + " (" + province.op.Count() + ")");
                    sb.Append("</li>");
                    sb.Append("<li><table id=\"tableInfo\" class=\"tblKingdomInfo paddingLeft\">");
                    sb.Append("<thead><tr>");
                    sb.Append("<th>Type</th><th>Count</th><th>Last Performed On</th><th class=\"{sorter: 'fancyNumber'}\">Last Performed</th><th class=\"{sorter: 'fancyNumber'}\">Expires In</th>");
                    sb.Append("</tr></thead>");
                    int i = 0;
                    foreach (var op in province.op)
                    {
                        if (op.bb.FirstOrDefault().alt != "NOTALLOWED")
                        {
                            sb.Append("<tr title=\"");
                            for (int j = op.bb.Count() - 1; j > -1; j--)
                                sb.Append(op.bb[j].alt + " towards " + op.bb[j].Target + "; ");
                            switch (i % 2)
                            {
                                case 1:
                                    sb.Append("\" class=\"d0\">");
                                    break;
                                case 0:
                                    sb.Append("\" class=\"d0\">");
                                    break;
                            }
                            sb.Append("<td>" + op.bb.FirstOrDefault().Type + "</td>");
                            sb.Append("<td>" + op.bb.Count() + "</td>");
                            sb.Append("<td>" + op.bb.LastOrDefault().Target + "</td>");
                            sb.Append("<td>" + op.bb.LastOrDefault().TimeDate + "</td>");
                            sb.Append("<td>" + op.bb.LastOrDefault().ExpirationDate + "</td>");
                            sb.Append("</tr>");
                        }
                        i += 1;
                    }
                    sb.Append("</table></li>");
                }
                sb.Append("</ul></div>");


                sb.Append("<div class='divProvinceDetailsContainer'><b>Most Recent Ops</b>");
                sb.Append("<table id=\"tableInfo\" class=\"tblKingdomInfo paddingLeft\">");
                sb.Append("<thead><tr>");
                sb.Append("<th>Type</th><th>Performed On</th><th>Performed By</th><th class=\"{sorter: 'fancyNumber'}\">Last Performed</th><th class=\"{sorter: 'fancyNumber'}\">Expires In</th>");
                sb.Append("</tr></thead>");
                int z = 0;
                foreach (var op in allOps.OrderByDescending(x => x.PostedTime))
                //for (int i = 0; i < allOps.Count; i++)
                {
                    sb.Append("<tr title=\"");
                    sb.Append(op.alt + " by " + op.PostedBy + "; ");
                    switch (z % 2)
                    {
                        case 1:
                            sb.Append("\" class=\"d0\">");
                            break;
                        case 0:
                            sb.Append("\" class=\"d0\">");
                            break;
                    }
                    sb.Append("<td>" + op.Type + "</td>");
                    sb.Append("<td>" + op.Target + "</td>");
                    sb.Append("<td>" + op.PostedBy + "</td>");
                    sb.Append("<td>" + op.TimeDate + "</td>");
                    sb.Append("<td>" + op.ExpirationDate + "</td>");
                    //sb.Append("<td>" + op + "</td>");
                    sb.Append("</tr>");
                    z += 1;
                }
                sb.Append("</table></div>");
            }
            else
                sb.Append("No Ops to Report at this time");
            return sb.ToString();
        }
        private static List<Op> GetEffects(List<Op> getOps)
        {

            return (from zz in
                        (from xx in getOps
                         select new
                         {
                             Op_ID = xx.Op_ID,
                             Directed_To_Province_ID = xx.Directed_To_Province_ID,
                             OP_Name = xx.OP_Name,
                             Count = (from yy in getOps
                                      where yy.Op_ID == xx.Op_ID
                                      where yy.Directed_To_Province_ID == xx.Directed_To_Province_ID
                                      select yy.uid).Count(),
                         }).Distinct()
                    select new Op
                    {
                        Directed_To_Province_ID = zz.Directed_To_Province_ID,
                        Count = zz.Count,
                        OP_Name = zz.OP_Name,
                        Ops = (from xx in
                                   (from yy in getOps
                                    select new Op
                                    {
                                        OP_Text = yy.OP_Text,
                                        Added_By_Province_ID = yy.Added_By_Province_ID,
                                        Expiration_Date = yy.Expiration_Date,
                                        Op_ID = yy.Op_ID,
                                        TimeStamp = yy.TimeStamp,
                                        Directed_To_Province_ID = yy.Directed_To_Province_ID
                                    })
                               where zz.Op_ID == xx.Op_ID
                               where zz.Directed_To_Province_ID == xx.Directed_To_Province_ID
                               select new Op
                               {
                                   OP_Text = xx.OP_Text,
                                   Added_By_Province_ID = xx.Added_By_Province_ID,
                                   Expiration_Date = xx.Expiration_Date,
                                   Op_ID = xx.Op_ID,
                                   TimeStamp = xx.TimeStamp,
                                   Directed_To_Province_ID = xx.Directed_To_Province_ID
                               }).ToList(),
                    }).ToList();
        }
    }

}
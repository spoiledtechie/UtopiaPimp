using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

using Boomers.Utilities.DatesTimes;
using Pimp.UParser;
using PimpLibrary.Static.Enums;
using Pimp.UCache;
using Pimp;
using Pimp.Utopia;
using Pimp.Users;
using PimpLibrary.Utopia.Ce;
using Pimp.UData;

namespace Pimp.UIBuilder
{
    /// <summary>
    /// Summary description for CE
    /// </summary>
    public class CE
    {
        /// <summary>
        /// Builds the CE for kingdoms.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="kingdomIL"></param>
        /// <param name="CEID"></param>
        /// <param name="ownerKingdomID"></param>
        /// <returns></returns>
        public static string BuildCE(int month, int year, string kingdomIL, Guid ceKingdomId, Guid ownerKingdomID, Guid currentUserID, OwnedKingdomProvinces cachedKingdom)
        {
            var ceKingdomList = CeCache.getCeForKingdomCache(year, month, ceKingdomId, ownerKingdomID, cachedKingdom);
            var ownerKingdom = cachedKingdom.Kingdoms.Where(x => x.Owner_Kingdom_ID == cachedKingdom.Owner_Kingdom_ID).FirstOrDefault();

            StringBuilder sbUpper = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            sbUpper.Append("<div id=\"divCEContents\">");
            sbUpper.Append(BuildYears(year, ceKingdomList.CeList));
            sbUpper.Append(BuildMonths(month, year, ceKingdomList.CeList));
            if (kingdomIL == string.Empty)
                kingdomIL = "All";

            if (ceKingdomList.CeList != null && ceKingdomList.CeList.Count > 0)
            {
                IEnumerable<CS_Code.Utopia_Kingdom_CE> ceListFinal = ceKingdomList.CeList;

                ceListFinal = ceListFinal.Where(x => x.Utopia_Year == year).Where(x => x.Utopia_Month == month);

                if (kingdomIL != "All") //if a current Kingdom Id is selected, otherwise pass right over.
                    ceListFinal = ceListFinal.Where(x => x.Source_Kingdom_Island == Convert.ToInt32(URegEx.rgxNumber.Matches(kingdomIL)[0].Value)).Where(x => x.Source_Kingdom_Location == Convert.ToInt32(URegEx.rgxNumber.Matches(kingdomIL)[1].Value));


                List<BuildProvinceUniques> pu = new List<BuildProvinceUniques>();
                int explorerPoolAttack = 0;//explorer pool for attckers
                int explorerPoolDefend = 0;//explorer pool for defence of kingdom.
                int explorePoolHome = 0; // explorer pool for owner kingdom 
                int homeAway = 0;
                if (ceKingdomId != ownerKingdomID) // 1 = away
                    homeAway = 1;
                int dirtyBitA = 0;
                int dirtyBitD = 0;
                int totalAttacksMade = 0;
                int totalAttacksSuffered = 0;
                int totalLocalAttacks = 0;
                int totalAcresTaken = 0;
                int totalAcresLost = 0;
                int totalAttacksTypeMade = 0; //for learns, razes, pillages....
                int totalAttacksTypeSuffered = 0;
                string homeKingdom = string.Empty;

                sbUpper.Append(BuildKingdomList(month, year, kingdomIL, ceKingdomList.CeList));

                sb.Append("<br/><div style=\"text-align:center;\">The Kingdom Reporter [UtopiaPimp]");
                sb.Append("<br/>" + Formatting.Month(month) + ", YR" + year + " Edition</div>");
                sb.Append("<ul class=\"ulList\">");
                foreach (var item in ceListFinal)
                {
                    if (UtopiaHelper.Instance.CeTypes.Where(x => x.uid == item.CE_Type).FirstOrDefault() != null)
                    {
                        BuildProvinceUniques puu = new BuildProvinceUniques();
                        puu.provName = item.Source_Province_Name;
                        puu.date = URegEx.rgxFindUtopianDateTime.Match(item.Raw_Line).Value;
                        puu.islandLocation = item.Source_Kingdom_Island + ":" + item.Source_Kingdom_Location;

                        dirtyBitA = 0;//attacking bit
                        dirtyBitD = 0;//defending bit
                        sb.Append("<li>");

                        switch (UtopiaHelper.Instance.CeTypes.Where(x => x.uid == item.CE_Type).FirstOrDefault().name)
                        {
                            case CeTypeEnum.CaputeredLand:
                            case CeTypeEnum.Ambush:
                            case CeTypeEnum.CaputeredLandIntraKingdom:
                                if ((item.Target_Kingdom_Island + ":" + item.Target_Kingdom_Location) == (item.Source_Kingdom_Island + ":" + item.Source_Kingdom_Location))
                                {
                                    totalLocalAttacks += 1;
                                    sb.Append("<img src=\"" + ImagesStatic.CapturedLandLocallyCe + "\"/> ");
                                    homeKingdom = (item.Source_Kingdom_Island + ":" + item.Source_Kingdom_Location);
                                }
                                else if ((ceKingdomList.Kingdom_Island + ":" + ceKingdomList.Kingdom_Location) == (item.Source_Kingdom_Island + ":" + item.Source_Kingdom_Location)) // If home kingdom is attacking
                                {
                                    homeKingdom = (item.Source_Kingdom_Island + ":" + item.Source_Kingdom_Location);
                                    sb.Append("<img src=\"" + ImagesStatic.CapturedLandThroughAttackCe + "\"/> ");
                                    explorerPoolAttack += Convert.ToInt32(Convert.ToInt32(item.value) * .1);
                                    totalAttacksMade += 1;
                                    totalAcresTaken += Convert.ToInt32(item.value);
                                    dirtyBitA = 1; //province attacked
                                    if ((ceKingdomList.Kingdom_Island + ":" + ceKingdomList.Kingdom_Location) == (ownerKingdom.Kingdom_Island + ":" + ownerKingdom.Kingdom_Location))
                                        explorePoolHome += Convert.ToInt32(item.value);
                                }
                                else // if Home kingdom is defending
                                {
                                    sb.Append("<img src=\"" + ImagesStatic.CapturedLandDefendedCe + "\"/> ");
                                    int num = ((Convert.ToInt32(item.value) * -100) / 110) + Convert.ToInt32(item.value);
                                    explorerPoolDefend += num;
                                    totalAttacksSuffered += 1;
                                    totalAcresLost += Convert.ToInt32(item.value);
                                    dirtyBitD = 1;//province defended
                                }
                                puu.acres = Convert.ToInt32(item.value);
                                pu.Add(puu);
                                break;
                            case CeTypeEnum.AttemptedToInvade:
                            case CeTypeEnum.AttackedAndStole:
                            case CeTypeEnum.Massacred:
                            case CeTypeEnum.RazedProvince:
                            case CeTypeEnum.RazedProvinceIntraKingdom:
                                if ((item.Target_Kingdom_Island + ":" + item.Target_Kingdom_Location) == (item.Source_Kingdom_Island + ":" + item.Source_Kingdom_Location))
                                {
                                    homeKingdom = (item.Source_Kingdom_Island + ":" + item.Source_Kingdom_Location);
                                    totalLocalAttacks += 1;
                                    sb.Append("<img src=\"" + ImagesStatic.CapturedLandLocallyCe + "\"/> ");
                                }
                                else if ((ceKingdomList.Kingdom_Island + ":" + ceKingdomList.Kingdom_Location) == (item.Source_Kingdom_Island + ":" + item.Source_Kingdom_Location))
                                {
                                    homeKingdom = (item.Source_Kingdom_Island + ":" + item.Source_Kingdom_Location);
                                    sb.Append("<img src=\"" + ImagesStatic.CapturedLandThroughAttackCe + "\"/> ");
                                    totalAttacksTypeMade += 1;
                                    totalAttacksMade += 1;
                                }
                                else
                                {
                                    sb.Append("<img src=\"" + ImagesStatic.CapturedLandDefendedCe + "\"/> ");
                                    totalAttacksTypeSuffered += 1;
                                    totalAttacksSuffered += 1;
                                }
                                pu.Add(puu);
                                break;
                            case CeTypeEnum.AidSent:
                                sb.Append("<img src=\"" + ImagesStatic.AidSentReceivedCe + "\"/> ");
                                break;
                            case CeTypeEnum.AttemptedToInvadeIntraKingdom:
                            case CeTypeEnum.BrokenCeasefireAgreementWithUs:
                            case CeTypeEnum.WeProposedCeasefire:
                            case CeTypeEnum.WeDeclaredWar:
                            case CeTypeEnum.ProposedCeasefireWithOurKingdom:
                            case CeTypeEnum.TheyDeclaredWar:
                            case CeTypeEnum.WeCancelledCeasefire:
                            case CeTypeEnum.EnteredFormalCeaseFire:
                            case CeTypeEnum.JoinedKingdom:
                            case CeTypeEnum.TheyDefectedToKingdom:
                            case CeTypeEnum.TheyWithDrewFromWar:
                            case CeTypeEnum.LeftKingdom:
                            case CeTypeEnum.AbandonedProvince:
                            case CeTypeEnum.MonarchDestroyedProvince:
                            case CeTypeEnum.OrderedEarlyEndToPostWar:
                            case CeTypeEnum.WithDrawnOurCeasefireProposal:
                            case CeTypeEnum.WithDrawnTheirCeasefireProposal:
                            case CeTypeEnum.CancelledCeasefireProposalWithOurKingdom:
                            case CeTypeEnum.TheyProposedMutualPeace:
                            case  CeTypeEnum .WithDrawnTheirPeaceOffer:
                                sb.Append("<img src=\"" + ImagesStatic.KingdomStatusCe + "\"/> ");
                                break;
                            case CeTypeEnum.DragonProjectStartedEmerald:
                            case CeTypeEnum.StartedEmeraldDragonProjectAgainstUs:
                            case CeTypeEnum.DragonFlownAway:
                            case CeTypeEnum.EmeraldDragonRavagingLands:
                            case CeTypeEnum.OurDragonSetFlight:
                            case CeTypeEnum.RubyDragonProjectAgainstUs:
                            case CeTypeEnum.DragonProjectStartedRuby:
                            case CeTypeEnum.RubyDragonRavagingLands:
                            case CeTypeEnum.SlainDragonRavagingLands:
                            case CeTypeEnum.TheyCancelledTheirDragonProjectTowardsUs:
                            case CeTypeEnum.GoldDragonRavagingLands:
                            case CeTypeEnum.StartedGoldDragonProjectAgainstUs:
                            case CeTypeEnum.DragonProjectStartedSapphire:
                            case CeTypeEnum.OurKingdomCancelledDragon:
                            case CeTypeEnum.DragonProjectStartedGold:
                            case CeTypeEnum.SapphireDragonRavagingLands:
                            case CeTypeEnum.StartedSapphireDragonProjectAgainstUs:
                                sb.Append("<img src=\"" + ImagesStatic.DragonsCe + "\"/> ");
                                break;
                            default:
                                UtopiaParser.FailedAt("'CEShowMeFailed1'", UtopiaHelper.Instance.CeTypes.Where(x => x.uid == item.CE_Type).FirstOrDefault().name.ToString() + "; " + item.Raw_Line, currentUserID);
                                break;
                        }

                        sb.Append(item.Raw_Line);
                        switch (homeAway) //is it the owner kingdom
                        {
                            case 0: //home
                                switch (dirtyBitA)
                                {
                                    case 1: //it was an attack
                                        sb.Append(" [total taken: " + Convert.ToInt32(item.value).ToString("N0") + "]"); //adds 10%
                                        break;
                                }
                                switch (dirtyBitD)
                                {
                                    case 1: //it was a defend.
                                        int num = ((Convert.ToInt32(item.value) * -100) / 110) + Convert.ToInt32(item.value);
                                        sb.Append(" [expl: " + (num).ToString("N0") + "; total taken: " + (num + Convert.ToInt32(item.value)).ToString("N0") + "]");
                                        break;
                                }
                                break;
                            case 1: //Away
                                switch (dirtyBitA)
                                {
                                    case 1://it was an attack
                                        int num = Convert.ToInt32(item.value) - ((Convert.ToInt32(item.value) * 100) / 110);
                                        sb.Append(" [expl: " + (num).ToString("N0") + "; total taken: " + (num + Convert.ToInt32(item.value)).ToString("N0") + "]");
                                        break;
                                }
                                switch (dirtyBitD)
                                {
                                    case 1://it was a defend.
                                        int num = ((Convert.ToInt32(item.value) * 110) / 100) - Convert.ToInt32(item.value);
                                        sb.Append(" [expl: " + (num).ToString("N0") + ";]");
                                        break;
                                }
                                break;
                        }
                        sb.Append("</li>");
                    }
                }
                sb.Append("</ul>");
                sb.Append("<br/>");

                switch (homeAway)
                {
                    case 0:
                        sbUpper.Append("<div style=\"text-align:center;\">Kingdom Unique Attacks for our Kingdom");
                        break;
                    case 1:
                        sbUpper.Append("<div style=\"text-align:center;\">Kingdom Unique Attacks for " + (ceKingdomList.Kingdom_Island + ":" + ceKingdomList.Kingdom_Location));
                        break;
                }
                sb.Append("<ul class=\"ulList\">");
                sb.Append("<li>**Explore pools are only taken from in times of War.</li>");
                sb.Append("<li>**There is no way to tell if the opponents explore pool is empty, so as a default option all explore pool information is turned on.</li>");
                sb.Append("</ul>");

                var kdUniques = (from xx in pu
                                 group xx by xx.provName into yy
                                 select new
                                 {
                                     yy.Key,
                                     kd = (from zz in yy
                                           where zz.provName == yy.Key
                                           select zz.islandLocation).FirstOrDefault(),
                                     uniques = (from zz in yy
                                                select zz.date).Distinct().Count()
                                 }).ToList();

                //Uniques Calculator.
                sbUpper.Append("<br/>[http://UtopiaPimp.com CE Uniques]</div>");
                sbUpper.Append("<ul class=\"ulList\">");
                sbUpper.Append("<li>Total Attacks Made: " + totalAttacksMade);
                sbUpper.Append("</li><li>Total Non-Acre Attacks Made: " + totalAttacksTypeMade.ToString("N0"));
                int kdHomeUni = 0;
                foreach (var item in (from xx in kdUniques where xx.kd == homeKingdom select xx)) //gets the kd Uniques
                    kdHomeUni += item.uniques;
                sbUpper.Append("</li><li><a href=\"#\" id=\"dialog_link\" onclick=\"CELoadModal('Taken', '" + month + "', '" + year + "', '" + kingdomIL + "', '" + ceKingdomId.ToString() + "', '" + ownerKingdomID.ToString() + "');return false;\">Total KD Uniques: " + kdHomeUni + "</a>");
                sbUpper.Append("</li><li>Total Acres Taken: " + totalAcresTaken.ToString("N0"));
                if (homeAway == 0) //if its a HOME paper.
                {
                    sbUpper.Append("</li><li>Taken from other kingdoms explorer pool this month: " + explorerPoolAttack.ToString("N0") + " acres **");
                    sbUpper.Append("</li><li>Total Taken (Ex. Pool + Taken): " + (explorerPoolAttack + totalAcresTaken).ToString("N0"));
                }
                else //if its another kingdoms paper
                {
                    sbUpper.Append("</li><li>Taken from (" + (ceKingdomList.Kingdom_Island + ":" + ceKingdomList.Kingdom_Location) + ")'s explorer pool this month: " + explorerPoolDefend.ToString("N0") + " acres **");
                    sbUpper.Append("</li><li>Taken from other kingdoms explorer pool this month: " + explorerPoolAttack.ToString("N0") + " acres **");
                }

                sbUpper.Append("</li><li><br/>");
                sbUpper.Append("</li><li>Total Attacks Suffered: " + totalAttacksSuffered.ToString("N0"));
                sbUpper.Append("</li><li>Total Non-Acre Attacks Suffered: " + totalAttacksTypeSuffered.ToString("N0"));
                int kdEnemyUni = 0;
                foreach (var item in (from xx in kdUniques where xx.kd != homeKingdom select xx))//gets the enemy kd Uniques
                    kdEnemyUni += item.uniques;
                sbUpper.Append("</li><li><a href=\"#\" id=\"dialog_link\" onclick=\"CELoadModal('Lost', '" + month + "', '" + year + "', '" + kingdomIL + "', '" + ceKingdomId.ToString() + "', '" + ownerKingdomID.ToString() + "');return false;\">Total Enemy Uniques: " + kdEnemyUni + "</a>");
                sbUpper.Append("</li><li>Total Acres Lost: " + totalAcresLost.ToString("N0"));
                if (homeAway == 0) //if its a home paper.
                {
                    sbUpper.Append("</li><li>Taken from our explorer pool this month: " + explorerPoolDefend.ToString("N0") + " acres **");
                    sbUpper.Append("</li><li>Total Lost (Ex. Pool + Lost): " + (explorerPoolDefend + totalAcresLost).ToString("N0"));
                }
                else//if its another kingdoms paper
                    sbUpper.Append("</li><li>(" + (ceKingdomList.Kingdom_Island + ":" + ceKingdomList.Kingdom_Location) + ")'s taken from OUR explorer pool this Month: " + explorePoolHome.ToString("N0") + " acres **");

                sbUpper.Append("</li><li><br/>");
                sbUpper.Append("</li><li>Total Local Attacks: " + totalLocalAttacks.ToString("N0"));

                sbUpper.Append("</li></ul>");
            }
            else
                sb.Append("NO CE Yet");
            sb.Append("</div>");

            return sbUpper.ToString() + sb.ToString();
        }
        public static string BuildModalPopUp(string type, int month, int year, string kingdomIL, Guid ceKingdomId, Guid ownerKingdomID, Guid currentUserID, OwnedKingdomProvinces cachedKingdom)
        {
            var ceKingdomList = CeCache.getCeForKingdomCache(year, month, ceKingdomId, ownerKingdomID, cachedKingdom);
            var ownerKingdom = cachedKingdom.Kingdoms.Where(x => x.Owner_Kingdom_ID == cachedKingdom.Owner_Kingdom_ID).FirstOrDefault();

            StringBuilder sb = new StringBuilder();
            if (ceKingdomList != null && ceKingdomList.CeList.Count > 0)
            {
                IEnumerable<CS_Code.Utopia_Kingdom_CE> ceListFinal = ceKingdomList.CeList;

                ceListFinal = ceListFinal.Where(x => x.Utopia_Year == year).Where(x => x.Utopia_Month == month);

                if (kingdomIL != "All") //if a current Kingdom Id is selected, otherwise pass right over.
                    ceListFinal = ceListFinal.Where(x => x.Source_Kingdom_Island == Convert.ToInt32(URegEx.rgxNumber.Matches(kingdomIL)[0].Value)).Where(x => x.Source_Kingdom_Location == Convert.ToInt32(URegEx.rgxNumber.Matches(kingdomIL)[1].Value));

                List<BuildProvinceUniques> pu = new List<BuildProvinceUniques>();
                int totalAttacksMade = 0;
                int totalAttacksSuffered = 0;
                int totalLocalAttacks = 0;
                int totalAcresTaken = 0;
                int totalAcresLost = 0;
                string homeKingdom = string.Empty;

                foreach (var item in ceListFinal)
                {
                    BuildProvinceUniques puu = new BuildProvinceUniques();
                    puu.provName = item.Source_Province_Name;
                    puu.date = URegEx.rgxFindUtopianDateTime.Match(item.Raw_Line).Value;
                    puu.islandLocation = item.Source_Kingdom_Island + ":" + item.Source_Kingdom_Location;

                    switch (UtopiaHelper.Instance.CeTypes.Where(x => x.uid == item.CE_Type).FirstOrDefault().name)
                    {
                        case CeTypeEnum.CaputeredLand:
                        case CeTypeEnum.Ambush:
                            if (item.Target_Kingdom_Island + ":" + item.Target_Kingdom_Location == item.Source_Kingdom_Island + ":" + item.Source_Kingdom_Location)
                            {
                                totalLocalAttacks += 1;
                                homeKingdom = item.Source_Kingdom_Island + ":" + item.Source_Kingdom_Location;
                            }
                            else if (ceKingdomList.Kingdom_Island + ":" + ceKingdomList.Kingdom_Location == item.Source_Kingdom_Island + ":" + item.Source_Kingdom_Location) // If home kingdom is attacking
                            {
                                homeKingdom = item.Source_Kingdom_Island + ":" + item.Source_Kingdom_Location;
                                totalAttacksMade += 1;
                                totalAcresTaken += Convert.ToInt32(item.value);
                            }
                            else // if Home kingdom is defending
                            {
                                totalAttacksSuffered += 1;
                                totalAcresLost += Convert.ToInt32(item.value);
                            }
                            puu.acres = Convert.ToInt32(item.value);
                            pu.Add(puu);
                            break;
                        case CeTypeEnum.AttemptedToInvade:
                        case CeTypeEnum.Massacred:
                        case CeTypeEnum.RazedProvince:
                        case CeTypeEnum.AttackedAndStole:
                        case CeTypeEnum.RazedProvinceIntraKingdom:
                            if (item.Target_Kingdom_Island + ":" + item.Target_Kingdom_Location == item.Source_Kingdom_Island + ":" + item.Source_Kingdom_Location)
                            {
                                homeKingdom = item.Source_Kingdom_Island + ":" + item.Source_Kingdom_Location;
                                totalLocalAttacks += 1;
                            }
                            else if (ceKingdomList.Kingdom_Island + ":" + ceKingdomList.Kingdom_Location == item.Source_Kingdom_Island + ":" + item.Source_Kingdom_Location)
                            {
                                homeKingdom = item.Source_Kingdom_Island + ":" + item.Source_Kingdom_Location;
                                totalAttacksMade += 1;
                            }
                            else
                            {
                                totalAttacksSuffered += 1;
                            }
                            pu.Add(puu);
                            break;
                        case CeTypeEnum.AidSent:
                        case CeTypeEnum.EnteredFormalCeaseFire:
                        case CeTypeEnum.WeProposedCeasefire:
                        case CeTypeEnum.MonarchDestroyedProvince:
                        case CeTypeEnum.JoinedKingdom:
                        case CeTypeEnum.LeftKingdom:
                        case CeTypeEnum.TheyDeclaredWar:
                        case CeTypeEnum.StartedEmeraldDragonProjectAgainstUs:
                        case CeTypeEnum.DragonProjectStartedSapphire:
                        case CeTypeEnum.ProposedCeasefireWithOurKingdom:
                        case CeTypeEnum.WeDeclaredWar:
                        case CeTypeEnum.SlainDragonRavagingLands:
                        case CeTypeEnum.StartedGoldDragonProjectAgainstUs:
                        case CeTypeEnum.DragonProjectStartedGold:
                        case CeTypeEnum.GoldDragonRavagingLands:
                        case CeTypeEnum.OurKingdomCancelledDragon:
                        case CeTypeEnum.OurDragonSetFlight:
                        case CeTypeEnum.RubyDragonProjectAgainstUs:
                        case CeTypeEnum.EmeraldDragonRavagingLands:
                        case CeTypeEnum.DragonProjectStartedRuby:
                        case CeTypeEnum.RubyDragonRavagingLands:
                            break;
                        default:
                            UtopiaParser.FailedAt("'CEShowMeModalPopup1'", UtopiaHelper.Instance.CeTypes.Where(x => x.uid == item.CE_Type).FirstOrDefault().name + "; " + item.Raw_Line, currentUserID);
                            break;
                    }
                }

                var kdUniques = (from xx in pu
                                 group xx by xx.provName into yy
                                 select new
                                 {
                                     yy.Key,
                                     kd = (from zz in yy
                                           where zz.provName == yy.Key
                                           select zz.islandLocation).FirstOrDefault(),
                                     uniques = (from zz in yy
                                                select zz.date).Distinct().Count(),
                                     acres = (from zz in yy
                                              select zz.acres),


                                 }).ToList().OrderByDescending(x => x.uniques);

                sb.Append("<table id=\"ceModalTable\" class=\"tblKingdomInfo\">");
                sb.Append("<thead><tr>");
                sb.Append("<th>Province Name</th><th>Uniques</th><th>Total Attacks</th><th class=\"{sorter: 'fancyNumber'}\">Total Acres</th>");
                sb.Append("</tr></thead><tbody>");
                int i = 0;
                if (type == "Taken")
                {
                    foreach (var item in (from xx in kdUniques where xx.kd == homeKingdom select xx))
                    {
                        switch (i % 2)
                        {
                            case 1:
                                sb.Append("<tr class=\"d0\">");
                                break;
                            case 0:
                                sb.Append("<tr class=\"d1\">");
                                break;
                        }
                        sb.Append("<td>" + item.Key + "</td><td>" + item.uniques + "</td><td>");
                        int acres = 0;
                        int attacks = 0;
                        foreach (var it in item.acres)
                        {
                            acres += it;
                            attacks += 1;
                        }
                        sb.Append(attacks + "</td><td>" + acres.ToString("N0") + "</td></tr>");
                        i += 1;
                    }
                }
                else
                {
                    foreach (var item in (from xx in kdUniques where xx.kd != homeKingdom select xx))
                    {
                        switch (i % 2)
                        {
                            case 1:
                                sb.Append("<tr class=\"d0\">");
                                break;
                            case 0:
                                sb.Append("<tr class=\"d1\">");
                                break;
                        }
                        sb.Append("<td>" + item.Key + " (" + item.kd + ")</td><td>" + item.uniques + "</td><td>");
                        int acres = 0;
                        int attacks = 0;
                        foreach (var it in item.acres)
                        {
                            acres += it;
                            attacks += 1;
                        }
                        sb.Append(attacks + "</td><td>" + acres.ToString("N0") + "</td></tr>");
                        i += 1;
                    }
                }
                sb.Append("</tbody></table>");
            }
            return sb.ToString();
        }
        public static string ExportModalPopUp(string type, int month, int year, string kingdomIL, Guid ceKingdomId, Guid ownerKingdomID, Guid currentUserID, OwnedKingdomProvinces cachedKingdom)
        {
            HttpContext.Current.Response.Clear();
            //opens the confirmation window with default file name.
            HttpContext.Current.Response.AddHeader("Content-Disposition", "inline; filename=\"Utopia" + DateTime.UtcNow.ToyyyyMMdd() + ".xls\"");

            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;
            HttpContext.Current.Response.Charset = "";

            HttpContext.Current.Response.Write(BuildModalPopUp(type, month, year, kingdomIL, ceKingdomId, ownerKingdomID, currentUserID, cachedKingdom));
            return HttpContext.Current.Response.ToString();
        }
        /// <summary>
        /// Builds the list of kingdoms for the current month
        /// </summary>
        /// <param name="month">month in Utopia Months</param>
        /// <param name="year">Utopian Year</param>
        /// <param name="selectedKingdom">If a kingdom is selected, this is it.  Can also be All</param>
        /// <param name="CEID">CE ID info for kingdom id of CE</param>
        /// <param name="ownerKingdomID">Owners kingdom Guid</param>
        /// <returns>A HTML compliant kingdom list.</returns>
        public static string BuildKingdomList(int month, int year, string selectedKingdom, List<CS_Code.Utopia_Kingdom_CE> ceList)
        {
            var getIDs = (from xx in ceList
                          where xx.Utopia_Month == month
                          where xx.Utopia_Year == year
                          select new
                          {
                              souKingdomID = "(" + xx.Source_Kingdom_Island + ":" + xx.Source_Kingdom_Location + ")",
                          }).Distinct().ToList();

            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"ulCEMonths\"><li>Source Kingdoms: </li>");
            sb.Append("</li>");
            switch (selectedKingdom) //if there is a chosen kingdom or if its all kingdoms
            {
                case "All": //All is chosen
                    sb.Append("<a href='#' class=\"ColumnOn\" id=\"akingdom\" onclick=\"CEChooseKingdom(this);return false;\" name=\"All\">All</a>");
                    break;
                default: //All is NOT chosen
                    sb.Append("<a href='#' onclick=\"CEChooseKingdom(this);return false;\" name=\"All\">All</a>");
                    break;
            }
            sb.Append("<li>");
            for (int i = 0; i < getIDs.Count; i++)
            {
                sb.Append("</li>");
                if (getIDs[i].souKingdomID == selectedKingdom & getIDs[i].souKingdomID != "(0:0)")
                    sb.Append("<a href='#' class=\"ColumnOn\" id=\"akingdom\" onclick=\"CEChooseKingdom(this);return false;\" name=\"" + getIDs[i].souKingdomID + "\">" + getIDs[i].souKingdomID + "</a>");
                else if (getIDs[i].souKingdomID != "(0:0)")
                    sb.Append("<a href='#' onclick=\"CEChooseKingdom(this);return false;\" name=\"" + getIDs[i].souKingdomID + "\">" + getIDs[i].souKingdomID + "</a>");
                sb.Append("<li>");
            }
            sb.Append("</ul>");
            return sb.ToString();
        }
        public static string BuildMonths(int month, int year, List<CS_Code.Utopia_Kingdom_CE> ceList)
        {
            var getMonths = (from xx in ceList
                             where xx.Utopia_Year == year
                             select xx.Utopia_Month).Distinct().ToList();

            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"ulCEMonths\"><li>Months: </li>");
            for (int i = 0; i < getMonths.Count; i++)
            {
                sb.Append("</li>");
                if (getMonths[i] == month)
                    sb.Append("<a href='#' class=\"ColumnOn\" id=\"amonth\" onclick=\"CEChooseMonth(this);return false;\" name=\"" + getMonths[i] + "\">" + Formatting.Month(getMonths[i]) + "</a>");
                else
                    sb.Append("<a href='#' onclick=\"CEChooseMonth(this);return false;\" name=\"" + getMonths[i] + "\">" + Formatting.Month(getMonths[i]) + "</a>");
                sb.Append("<li>");
            }
            sb.Append("</ul>");
            return sb.ToString();
        }
        /// <summary>
        /// gets the last month of the list
        /// </summary>
        /// <param name="ceList"></param>
        /// <returns></returns>
        public static int GetLastMonth(List<CS_Code.Utopia_Kingdom_CE> ceList)
        {
            return (from xx in ceList
                    orderby xx.uid descending
                    select xx.Utopia_Month).FirstOrDefault();
        }
        /// <summary>
        /// gets the last year of the list.
        /// </summary>
        /// <param name="ceList"></param>
        /// <returns></returns>
        public static int GetLastYear(List<CS_Code.Utopia_Kingdom_CE> ceList)
        {
            return (from xx in ceList
                    orderby xx.uid descending
                    select xx.Utopia_Year).FirstOrDefault();
        }
        /// <summary>
        /// builds the year string
        /// </summary>
        /// <param name="years"></param>
        /// <param name="ceList"></param>
        /// <returns></returns>
        public static string BuildYears(int years, List<CS_Code.Utopia_Kingdom_CE> ceList)
        {
            var getYears = (from xx in ceList
                            select xx.Utopia_Year).Distinct().OrderBy(x => x).ToList();
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"ulCEYears\"><li>Years: </li>");
            for (int i = 0; i < getYears.Count; i++)
            {
                sb.Append("</li>");
                if (getYears[i] == years)
                    sb.Append("<a href='#' class=\"ColumnOn\" id=\"ayear\" onclick=\"CEChooseYear(this);return false;\" name=\"" + getYears[i] + "\">" + getYears[i] + "</a>");
                else
                    sb.Append("<a href='#' onclick=\"CEChooseYear(this);return false;\" name=\"" + getYears[i] + "\">" + getYears[i] + "</a>");
                sb.Append("<li>");
            }
            sb.Append("</ul>");
            return sb.ToString();
        }
        /// <summary>
        /// Builds the CE for Provinces
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="kingdomIL"></param>
        /// <param name="CEID"></param>
        /// <param name="ownerKingdomID"></param>
        /// <returns></returns>
        public static string BuildCEPersonal(int month, int year, string kingdomIL, string CEID, string provinceName, string ownerKingdomID, Guid currentUserID)
        {
            Guid kingdomID = new Guid(ownerKingdomID);
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=\"divCEContents\">");
            sb.Append(BuildYearsPersonal(year, provinceName, kingdomID));
            sb.Append(BuildMonthsPersonal(month, year, provinceName, kingdomID));
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();

            List<BuildCe> getCE;
            switch (kingdomIL)
            {
                case "All":
                case "":
                    getCE = getAllCeLinesForKingdom(month, year, CEID, provinceName, kingdomID, db);
                    break;
                default:
                    getCE = getCeLinesForSourceKingdom(month, year, kingdomIL, CEID, provinceName, kingdomID, db);
                    break;
            }

            if (getCE.FirstOrDefault() != null)
            {
                int explorerPoolAttack = 0;//explorer pool for attckers
                int explorerPoolDefend = 0;//explorer pool for defence of kingdom.
                int explorePoolHome = 0; // explorer pool for owner kingdom 
                int homeAway = 0;
                if (CEID != kingdomID.ToString()) // 1 = away
                    homeAway = 1;
                int dirtyBitA = 0;
                int dirtyBitD = 0;

                sb.Append(BuildKingdomListPersonal(month, year, kingdomIL, provinceName, kingdomID));

                sb.Append("<br/><div style=\"text-align:center;\">The Kingdom Reporter [UtopiaPimp]");
                sb.Append("<br/>" + Formatting.Month(month) + ", YR" + year + " Edition</div>");
                sb.Append("<ul class=\"ulList\">");
                foreach (var item in getCE)
                {
                    dirtyBitA = 0;//attacking bit
                    dirtyBitD = 0;//defending bit
                    sb.Append("<li>");

                    CeTypeEnum ceType = (CeTypeEnum)Enum.Parse(typeof(CeTypeEnum), item.ceType.Trim());
                    switch (ceType)
                    {
                        case CeTypeEnum.captured:
                        case CeTypeEnum.CaputeredLand:
                            if (item.targIL == item.sourIL)
                                sb.Append("<img src=\"" + ImagesStatic.CapturedLandLocallyCe + "\"/> ");
                            else if (item.kingIL == item.sourIL) // If home kingdom is attacking
                            {
                                sb.Append("<img src=\"" + ImagesStatic.CapturedLandThroughAttackCe + "\"/> ");
                                explorerPoolAttack += Convert.ToInt32(Convert.ToInt32(item.size) * .1);
                                dirtyBitA = 1; //province attacked
                                if (item.kingIL == item.ownerKingdom)
                                    explorePoolHome += Convert.ToInt32(item.size);
                            }
                            else // if Home kingdom is defending
                            {
                                sb.Append("<img src=\"" + ImagesStatic.CapturedLandDefendedCe + "\"/> ");
                                int num = ((Convert.ToInt32(item.size) * -100) / 110) + Convert.ToInt32(item.size);
                                explorerPoolDefend += num;
                                dirtyBitD = 1;//province defended
                            }
                            break;
                        case CeTypeEnum.attemptedinvasion:
                        case CeTypeEnum.AttemptedToInvade:
                        case CeTypeEnum.pillaged:
                        case CeTypeEnum.razed:
                        case CeTypeEnum.learned:
                        case CeTypeEnum.killed:
                        case CeTypeEnum.ambushed:
                        case CeTypeEnum.Massacred:
                            if (item.targIL == item.sourIL)
                                sb.Append("<img src=\"" + ImagesStatic.CapturedLandLocallyCe + "\"/> ");
                            else if (item.kingIL == item.sourIL)
                                sb.Append("<img src=\"" + ImagesStatic.CapturedLandThroughAttackCe + "\"/> ");
                            else
                                sb.Append("<img src=\"" + ImagesStatic.CapturedLandDefendedCe + "\"/> ");
                            break;
                        case CeTypeEnum.defectedfrom:
                        case CeTypeEnum.provincedie:
                        case CeTypeEnum.defectedto:
                        case CeTypeEnum.collapsed:
                            sb.Append("<img src=\"" + ImagesStatic.ProvinceAffectsCe + "\"/> ");
                            break;
                        case CeTypeEnum.aid:
                            sb.Append("<img src=\"" + ImagesStatic.AidSentReceivedCe + "\"/> ");
                            break;
                        case CeTypeEnum.declaredwar:
                        case CeTypeEnum.AidSent:
                        case CeTypeEnum.ceasefire:
                        case CeTypeEnum.brokenceasefire:
                        case CeTypeEnum.withdrawn:
                        case CeTypeEnum.surrendered:
                        case CeTypeEnum.cancelledceasefire:
                        case CeTypeEnum.rejectedceasefire:
                            sb.Append("<img src=\"" + ImagesStatic.KingdomStatusCe + "\"/> ");
                            break;
                        case CeTypeEnum.flightdragon:
                        case CeTypeEnum.slaindragon:
                        case CeTypeEnum.kingdombegundragon:
                        case CeTypeEnum.dragontargetted:
                        case CeTypeEnum.dragonravaginglands:
                            sb.Append("<img src=\"" + ImagesStatic.DragonsCe + "\"/> ");
                            break;
                        default:
                            UtopiaParser.FailedAt("'CEShowMeDead'", item.ceType + "; " + item.RawLine, currentUserID);
                            break;
                    }
                    sb.Append(item.RawLine);
                    switch (homeAway) //is it the owner kingdom
                    {
                        case 0: //home
                            switch (dirtyBitA)
                            {
                                case 1: //it was an attack
                                    sb.Append(" [total taken: " + Convert.ToInt32(item.size).ToString("N0") + "]"); //adds 10%
                                    break;
                            }
                            switch (dirtyBitD)
                            {
                                case 1: //it was a defend.
                                    int num = ((Convert.ToInt32(item.size) * -100) / 110) + Convert.ToInt32(item.size);
                                    sb.Append(" [expl: " + (num).ToString("N0") + "; total taken: " + (num + Convert.ToInt32(item.size)).ToString("N0") + "]");
                                    break;
                            }
                            break;
                        case 1: //Away
                            switch (dirtyBitA)
                            {
                                case 1://it was an attack
                                    int num = Convert.ToInt32(item.size) - ((Convert.ToInt32(item.size) * 100) / 110);
                                    sb.Append(" [expl: " + (num).ToString("N0") + "; total taken: " + (num + Convert.ToInt32(item.size)).ToString("N0") + "]");
                                    break;
                            }
                            switch (dirtyBitD)
                            {
                                case 1://it was a defend.
                                    int num = ((Convert.ToInt32(item.size) * 110) / 100) - Convert.ToInt32(item.size);
                                    sb.Append(" [expl: " + (num).ToString("N0") + ";]");
                                    break;
                            }
                            break;
                    }
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
                sb.Append("<br/>");

                sb.Append("<ul class=\"ulList\">");
                switch (homeAway)
                {
                    case 0:
                        sb.Append("<li>Taken from our explorer pool this month: " + explorerPoolDefend + " acres **</li>");
                        sb.Append("<li>Taken from other kingdoms explorer pool this month: " + explorerPoolAttack + " acres **</li>");
                        break;
                    case 1:
                        sb.Append("<li>Taken from (" + getCE.FirstOrDefault().kingIL + ")'s explorer pool this month: " + explorerPoolDefend + " acres **</li>");
                        sb.Append("<li>Taken from other kingdoms explorer pool this month: " + explorerPoolAttack + " acres **</li>");
                        sb.Append("<li>(" + getCE.FirstOrDefault().kingIL + ")'s taken from OUR explorer pool this Month: " + explorePoolHome + " acres **</li>");
                        break;
                }
                sb.Append("<li><br /></li>");
                sb.Append("<li>**Explore pools are only taken from in times of War.</li>");
                sb.Append("<li>**There is no way to tell if the opponents explore pool is empty, so as a default option all explore pool information is turned on.</li>");
                sb.Append("</ul>");
            }
            else
                sb.Append("NO CE Yet");
            sb.Append("</div>");
            return sb.ToString();
        }

        private static List<BuildCe> getCeLinesForSourceKingdom(int month, int year, string kingdomIL, string CEID, string provinceName, Guid kingdomID, CS_Code.UtopiaDataContext db)
        {
            return (from xx in db.Utopia_Kingdom_CEs
                    from yy in db.Utopia_Kingdom_CE_Type_Pulls
                    from zz in db.Utopia_Kingdom_Infos
                    from aa in db.Utopia_Kingdom_Infos
                    where yy.uid == xx.CE_Type
                    where xx.Source_Province_Name == provinceName || xx.Target_Province_Name == provinceName
                    where xx.Owner_Kingdom_ID == kingdomID
                    where aa.Kingdom_ID == kingdomID
                    where xx.Kingdom_ID == zz.Kingdom_ID
                    where xx.Owner_Kingdom_ID == kingdomID
                    where xx.Kingdom_ID == new Guid(CEID)
                    where xx.Utopia_Month == month
                    where xx.Utopia_Year == year
                    where xx.Source_Kingdom_Island == Convert.ToInt32(URegEx.rgxNumber.Matches(kingdomIL)[0].Value)
                    where xx.Source_Kingdom_Location == Convert.ToInt32(URegEx.rgxNumber.Matches(kingdomIL)[1].Value)
                    orderby xx.Utopia_Date_Day
                    orderby xx.Utopia_Month
                    orderby xx.Utopia_Year
                    select new BuildCe
                    {
                        RawLine = xx.Raw_Line,
                        ceType = yy.CE_Type,
                        sourIL = xx.Source_Kingdom_Island + ":" + xx.Source_Kingdom_Location,
                        kingIL = zz.Kingdom_Island + ":" + zz.Kingdom_Location,
                        targIL = xx.Target_Kingdom_Island + ":" + xx.Target_Kingdom_Location,
                        size = xx.value,
                        ownerKingdom = aa.Kingdom_Island + ":" + aa.Kingdom_Location
                    }).ToList();
        }

        private static List<BuildCe> getAllCeLinesForKingdom(int month, int year, string CEID, string provinceName, Guid kingdomID, CS_Code.UtopiaDataContext db)
        {
            return (from xx in db.Utopia_Kingdom_CEs
                    from yy in db.Utopia_Kingdom_CE_Type_Pulls
                    from zz in db.Utopia_Kingdom_Infos
                    from aa in db.Utopia_Kingdom_Infos
                    where yy.uid == xx.CE_Type
                    where xx.Source_Province_Name == provinceName || xx.Target_Province_Name == provinceName
                    where xx.Owner_Kingdom_ID == kingdomID
                    where aa.Kingdom_ID == kingdomID
                    where xx.Kingdom_ID == zz.Kingdom_ID
                    where xx.Owner_Kingdom_ID == kingdomID
                    where xx.Kingdom_ID == new Guid(CEID)
                    where xx.Utopia_Month == month
                    where xx.Utopia_Year == year
                    orderby xx.Utopia_Date_Day
                    orderby xx.Utopia_Month
                    orderby xx.Utopia_Year
                    select new BuildCe
                    {
                        RawLine = xx.Raw_Line,
                        ceType = yy.CE_Type,
                        sourIL = xx.Source_Kingdom_Island + ":" + xx.Source_Kingdom_Location,
                        kingIL = zz.Kingdom_Island + ":" + zz.Kingdom_Location,
                        targIL = xx.Target_Kingdom_Island + ":" + xx.Target_Kingdom_Location,
                        size = xx.value,
                        ownerKingdom = aa.Kingdom_Island + ":" + aa.Kingdom_Location
                    }).ToList();
        }
        /// <summary>
        /// Builds the list of kingdoms for the current month
        /// </summary>
        /// <param name="month">month in Utopia Months</param>
        /// <param name="year">Utopian Year</param>
        /// <param name="selectedKingdom">If a kingdom is selected, this is it.  Can also be All</param>
        /// <param name="CEID">CE ID info for kingdom id of CE</param>
        /// <param name="ownerKingdomID">Owners kingdom Guid</param>
        /// <returns>A HTML compliant kingdom list.</returns>
        public static string BuildKingdomListPersonal(int month, int year, string selectedKingdom, string provinceName, Guid ownerKingdomID)
        {
            var personalCache = CeCache.getCEPersonalCache(provinceName, ownerKingdomID);
            var getIDs = (from xx in personalCache
                          where xx.Utopia_Month == month
                          where xx.Utopia_Year == year
                          select xx.sourIL).Distinct().ToList();

            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"ulCEMonths\"><li>Source Kingdoms: </li>");
            sb.Append("</li>");
            switch (selectedKingdom) //if there is a chosen kingdom or if its all kingdoms
            {
                case "All": //All is chosen
                    sb.Append("<a href='#' class=\"ColumnOn\" id=\"akingdom\" onclick=\"CEChooseKingdomPersonal(this);return false;\" name=\"All\">All</a>");
                    break;
                default: //All is NOT chosen
                    sb.Append("<a href='#' onclick=\"CEChooseKingdomPersonal(this);return false;\" name=\"All\">All</a>");
                    break;
            }
            sb.Append("<li>");
            for (int i = 0; i < getIDs.Count; i++)
            {
                sb.Append("</li>");
                if (getIDs[i] == selectedKingdom & getIDs[i] != "(0:0)")
                    sb.Append("<a href='#' class=\"ColumnOn\" id=\"akingdom\" onclick=\"CEChooseKingdomPersonal(this);return false;\" name=\"" + getIDs[i] + "\">" + getIDs[i] + "</a>");
                else if (getIDs[i] != "(0:0)")
                    sb.Append("<a href='#' onclick=\"CEChooseKingdomPersonal(this);return false;\" name=\"" + getIDs[i] + "\">" + getIDs[i] + "</a>");
                sb.Append("<li>");
            }
            sb.Append("</ul>");
            return sb.ToString();
        }
        public static string BuildMonthsPersonal(int month, int year, string provinceName, Guid ownerKingdomID)
        {
            var personalCache = CeCache.getCEPersonalCache(provinceName, ownerKingdomID);
            var getMonths = (from xx in personalCache
                             where xx.Utopia_Year == year
                             select xx.Utopia_Month).Distinct().ToList();

            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"ulCEMonths\"><li>Months: </li>");
            for (int i = 0; i < getMonths.Count; i++)
            {
                sb.Append("</li>");
                if (getMonths[i] == month)
                    sb.Append("<a href='#' class=\"ColumnOn\" id=\"amonth\" onclick=\"CEChooseMonthPersonal(this);return false;\" name=\"" + getMonths[i] + "\">" + Formatting.Month(getMonths[i]) + "</a>");
                else
                    sb.Append("<a href='#' onclick=\"CEChooseMonthPersonal(this);return false;\" name=\"" + getMonths[i] + "\">" + Formatting.Month(getMonths[i]) + "</a>");
                sb.Append("<li>");
            }
            sb.Append("</ul>");
            return sb.ToString();
        }
        public static int GetLastMonthPersonal(string provinceName, Guid ownerKingdomID)
        {
            var personalCache =CeCache.getCEPersonalCache(provinceName, ownerKingdomID); 
            return (from xx in personalCache
                    orderby xx.uid descending
                    select xx.Utopia_Month).FirstOrDefault();
        }
        public static int GetLastYearPersonal(string provinceName, Guid ownerKingdomID)
        {
            var personalCache= CeCache.getCEPersonalCache(provinceName, ownerKingdomID);
            return (from xx in personalCache
                    orderby xx.uid descending
                    select xx.Utopia_Year).FirstOrDefault();
        }
        public static string BuildYearsPersonal(int years, string provinceName, Guid ownerKingdomID)
        {
            var personalCache = CeCache.getCEPersonalCache(provinceName, ownerKingdomID);
            var getYears = (from xx in personalCache
                            select xx.Utopia_Year).Distinct().ToList();
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"ulCEYears\"><li>Years: </li>");
            for (int i = 0; i < getYears.Count; i++)
            {
                sb.Append("</li>");
                if (getYears[i] == years)
                    sb.Append("<a href='#' class=\"ColumnOn\" id=\"ayear\" onclick=\"CEChooseYearPersonal(this);return false;\" name=\"" + getYears[i] + "\">" + getYears[i] + "</a>");
                else
                    sb.Append("<a href='#' onclick=\"CEChooseYearPersonal(this);return false;\" name=\"" + getYears[i] + "\">" + getYears[i] + "</a>");
                sb.Append("<li>");
            }
            sb.Append("</ul>");
            return sb.ToString();
        }
    }




}
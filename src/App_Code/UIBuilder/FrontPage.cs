using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Pimp;
using Boomers.Utilities.Guids;
using Pimp.UParser;
using Pimp.UCache;
using PimpLibrary.Static.Enums;
using Pimp.Utopia;
using Pimp.Users;
using PimpLibrary.Utopia.Ce;
using Pimp.UData;


namespace Pimp.UIBuilder
{
    /// <summary>
    /// Summary description for FrontPage
    /// </summary>
    public class FrontPage
    {
        public static string DisplayProvinceCodesCache(Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            var obk = cachedKingdom.Provinces.Where(x => x.Kingdom_ID == ownerKingdomID);
            var test = (from xx in obk
                        where xx.Owner_User_ID == null
                        select new
                        {
                            provinceid = xx.Province_ID.RemoveDashes(),
                            xx.Province_Name
                        }).ToList();

            if (test.Count != 0)
            {
                StringBuilder sbpc = new StringBuilder();
                sbpc.Append("<div class=\"center\"><b>" + test.Count + " Provinces</b> still NOT signed up.</div>");
                sbpc.Append("<ul class=\"ulList\">");
                sbpc.Append("<li><b>Province Name</b> \t-\t Code</li>");
                for (int i = 0; i < test.Count(); i++)
                    sbpc.Append("<li><b>" + test[i].Province_Name + "</b>\t-\t " + test[i].provinceid + "</li>");
                sbpc.Append("</ul>");

                return sbpc.ToString();
            }
            else
                return string.Empty;
        }
        public static string DisplayProvincesWithoutContacts(Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            var obk = cachedKingdom.ProvincesWithoutUserContactsAdded;
            if (obk.Count > 0)
            {
                StringBuilder sbpc = new StringBuilder();
                sbpc.Append("<div class=\"center\"><b>" + obk.Count + " Users</b> have still not added their <a href='Contacts.aspx'>Contact Info.</a></div>");
                sbpc.Append("<ul class=\"ulList\">");
                for (int i = 0; i < obk.Count; i++)
                    sbpc.Append("<li><b>" + obk[i] + "</b></li>");
                sbpc.Append("</ul>");
                return sbpc.ToString();
            }
            else
                return string.Empty;
        }
        public static string kdSummary(Guid kindID, Guid ownerKingdomID, Guid currentProvinceID, Guid currentUserID, OwnedKingdomProvinces cachedKingdom)
        {
            if (currentProvinceID == new Guid())
                return "It doesn't look like you have joined or added your kingdom yet. <br/><br/> You can do this under the menu above. <br/><br/> Tools -> Kingdom -> Join/Add a Kingdom";
            StringBuilder sb = new StringBuilder();

            var getCeInfo = CeCache.getCeForKingdomCache(kindID, ownerKingdomID, cachedKingdom);
            if (getCeInfo != null && getCeInfo.CeList != null)
            {
                DateTime dt = DateTime.UtcNow;
                TimeSpan ts = dt.Subtract(DateTime.UtcNow);
                dt = DateTime.UtcNow;
                sb.Append(getCeInfo.Kingdom_Island + ":" + getCeInfo.Kingdom_Location);
                //sb.Append("<br/><br/><b>Current Relation:</b> Not Working<br/><br/>");
                sb.Append("<br/><br/>");
                sb.Append("<b>Relation Summary:</b><br/>");
                int totalLocalAttacks = 0;
                int totalAttacksMade = 0;
                int totalAttacksSuffered = 0;
                int totalAcresTaken = 0;
                int totalAcresLost = 0;
                string homeKingdom = string.Empty;
                string awayKingdom = string.Empty;
                List<BuildProvinceUniques> pu = new List<BuildProvinceUniques>();
                for (int i = 0; i < getCeInfo.CeList.Count; i++)
                {
                    if (UtopiaHelper.Instance.CeTypes.Where(x => x.uid == getCeInfo.CeList[i].CE_Type).FirstOrDefault() != null)
                    {
                        BuildProvinceUniques puu = new BuildProvinceUniques();

                        puu.provName = getCeInfo.CeList[i].Source_Province_Name;
                        if (cachedKingdom.Provinces.Where(x => x.Province_Name == puu.provName).FirstOrDefault() != null)
                            puu.provID = cachedKingdom.Provinces.Where(x => x.Province_Name == puu.provName).FirstOrDefault().Province_ID;

                        puu.provAttacked = getCeInfo.CeList[i].Target_Province_Name;
                        if (cachedKingdom.Provinces.Where(x => x.Province_Name == puu.provAttacked).FirstOrDefault() != null)
                            puu.provAttackedId = cachedKingdom.Provinces.Where(x => x.Province_Name == puu.provAttacked).FirstOrDefault().Province_ID;

                        puu.date = URegEx.rgxFindUtopianDateTime.Match(getCeInfo.CeList[i].Raw_Line).Value;
                        puu.islandLocation = getCeInfo.CeList[i].Source_Kingdom_Island + ":" + getCeInfo.CeList[i].Source_Kingdom_Location;
                        puu.provAttackedIslandLocation = getCeInfo.CeList[i].Target_Kingdom_Island + ":" + getCeInfo.CeList[i].Target_Kingdom_Location;

                        switch (UtopiaHelper.Instance.CeTypes.Where(x => x.uid == getCeInfo.CeList[i].CE_Type).FirstOrDefault().name)
                        {
                            case CeTypeEnum.CaputeredLand:
                            case CeTypeEnum.captured:
                            case CeTypeEnum.Ambush:
                            case CeTypeEnum.CaputeredLandIntraKingdom:
                                if (puu.provAttackedIslandLocation == puu.islandLocation)
                                {
                                    totalLocalAttacks += 1;
                                    homeKingdom = puu.islandLocation;
                                    awayKingdom = puu.provAttackedIslandLocation;
                                }
                                else if (getCeInfo.Kingdom_Island + ":" + getCeInfo.Kingdom_Location == puu.islandLocation) // If home kingdom is attacking
                                {
                                    homeKingdom = puu.islandLocation;
                                    awayKingdom = puu.provAttackedIslandLocation;
                                    totalAttacksMade += 1;
                                    totalAcresTaken += Convert.ToInt32(getCeInfo.CeList[i].value);
                                }
                                else // if Home kingdom is defending
                                {
                                    awayKingdom = puu.islandLocation;
                                    homeKingdom = puu.provAttackedIslandLocation;
                                    totalAttacksSuffered += 1;
                                    totalAcresLost += Convert.ToInt32(getCeInfo.CeList[i].value);
                                }
                                puu.acres = Convert.ToInt32(getCeInfo.CeList[i].value);
                                pu.Add(puu);
                                break;
                            case CeTypeEnum.AttemptedToInvade:
                            case CeTypeEnum.AttemptedToInvadeIntraKingdom:
                            case CeTypeEnum.AttackedAndStole:
                            case CeTypeEnum.RazedProvince:
                            case CeTypeEnum.RazedProvinceIntraKingdom:
                            case CeTypeEnum.razed:
                            case CeTypeEnum.attemptedinvasion:
                            //case "learned":
                            case CeTypeEnum.killed:
                            case CeTypeEnum.Massacred:
                            case CeTypeEnum.pillaged:
                                if (puu.provAttackedIslandLocation == puu.islandLocation)
                                {
                                    homeKingdom = puu.islandLocation;
                                    awayKingdom = puu.provAttackedIslandLocation;
                                    //totalLocalAttacks += 1;
                                }
                                else if (getCeInfo.Kingdom_Island + ":" + getCeInfo.Kingdom_Location == puu.islandLocation)
                                {
                                    homeKingdom = puu.islandLocation;
                                    awayKingdom = puu.provAttackedIslandLocation;
                                    //totalAttacksMade += 1;
                                }
                                else
                                {
                                    awayKingdom = puu.islandLocation;
                                    homeKingdom = puu.provAttackedIslandLocation;
                                    //totalAttacksSuffered += 1;
                                }
                                //pu.Add(puu);
                                break;
                            case CeTypeEnum.AidSent:
                            case CeTypeEnum.EnteredFormalCeaseFire:
                            case CeTypeEnum.ProposedCeasefireWithOurKingdom:
                            case CeTypeEnum.OurDragonSetFlight:
                            case CeTypeEnum.DragonProjectStartedRuby:
                            case CeTypeEnum.RubyDragonRavagingLands:
                            case CeTypeEnum.TheyDeclaredWar:
                            case CeTypeEnum.SlainDragonRavagingLands:
                            case CeTypeEnum.DragonProjectStartedEmerald:
                            case CeTypeEnum.DragonProjectStartedGold:
                            case CeTypeEnum.OurKingdomCancelledDragon:
                            case CeTypeEnum.WeDeclaredWar:
                            case CeTypeEnum.RubyDragonProjectAgainstUs:
                            case CeTypeEnum.LeftKingdom:
                            case CeTypeEnum.WeProposedCeasefire:
                            case CeTypeEnum.DragonFlownAway:
                            case CeTypeEnum.EmeraldDragonRavagingLands:
                            case CeTypeEnum.BrokenCeasefireAgreementWithUs:
                            case CeTypeEnum.StartedEmeraldDragonProjectAgainstUs:
                            case CeTypeEnum.JoinedKingdom:
                            case CeTypeEnum.TheyWithDrewFromWar:
                            case CeTypeEnum.DragonProjectStartedSapphire:
                            case CeTypeEnum.WeCancelledCeasefire:
                            case CeTypeEnum.TheyCancelledTheirDragonProjectTowardsUs:
                            case CeTypeEnum.GoldDragonRavagingLands:
                            case CeTypeEnum.CancelledCeasefireProposalWithOurKingdom:
                            case CeTypeEnum.TheyDefectedToKingdom:
                            case CeTypeEnum.dragontargetted:
                            case  CeTypeEnum.slaindragon:
                            case CeTypeEnum.aid:
                            case CeTypeEnum.startpeace:
                            case CeTypeEnum.SapphireDragonRavagingLands:
                            case CeTypeEnum.StartedGoldDragonProjectAgainstUs:
                            case CeTypeEnum.StartedSapphireDragonProjectAgainstUs:
                            case CeTypeEnum.flightdragon:
                            case CeTypeEnum.declaredwar:
                            case CeTypeEnum.AbandonedProvince:
                            case CeTypeEnum.withdrawn:
                            case CeTypeEnum.OrderedEarlyEndToPostWar:
                            case CeTypeEnum.MonarchDestroyedProvince:
                            case CeTypeEnum.WithDrawnOurCeasefireProposal:
                            case CeTypeEnum.WithDrawnTheirCeasefireProposal:
                            case CeTypeEnum.TheyProposedMutualPeace:
                            case CeTypeEnum.WithDrawnTheirPeaceOffer:
                            case CeTypeEnum.WeRejectedTheirPeaceOffer:
                                break;
                            default:
                                UtopiaParser.FailedAt("'CEShowMeModalPopup2'", UtopiaHelper.Instance.CeTypes.Where(x => x.uid == getCeInfo.CeList[i].CE_Type).FirstOrDefault().name + "; " + getCeInfo.CeList[i].Raw_Line, currentUserID);
                                break;
                        }
                    }
                  
                }

                sb.Append("Total Attacks Made: " + totalAttacksMade.ToString("N0") + " (" + totalAcresTaken.ToString("N0") + ")<br />");
                sb.Append("Total Attacks Suffered: " + totalAttacksSuffered.ToString("N0") + " (" + totalAcresLost.ToString("N0") + ")<br />");
                sb.Append("Net Change: ");
                if ((totalAcresTaken - totalAcresLost) > 0)
                    sb.Append("<span class=\"green\">+");
                else
                    sb.Append("<span class=\"red\">");
                sb.Append((totalAcresTaken - totalAcresLost).ToString("N0") + "</span><br/>");

                List<BuildAcreChanges> ac = new List<BuildAcreChanges>();//acre changes
                for (int i = 0; i < pu.Count; i++)
                {
                    var checkProv = ac.Where(x => x.provID == pu[i].provID).FirstOrDefault();

                    if (checkProv == null)
                    {
                        BuildAcreChanges bac = new BuildAcreChanges();
                        bac.provID = pu[i].provID;
                        bac.provName = pu[i].provName;
                        bac.islandLocation = pu[i].islandLocation;
                        bac.acres = pu[i].acres;
                        bac.attacksCount = 1;
                        bac.defendCount = 0;
                        ac.Add(bac);
                    }
                    else
                    {
                        checkProv.attacksCount += 1;
                        checkProv.acres += pu[i].acres;
                    }
                    var checkProvDefend = ac.Where(x => x.provID == pu[i].provAttackedId).FirstOrDefault();

                    if (checkProvDefend == null)
                    {
                        BuildAcreChanges bacc = new BuildAcreChanges();
                        bacc.provID = pu[i].provAttackedId;
                        bacc.provName = pu[i].provAttacked;
                        bacc.islandLocation = pu[i].provAttackedIslandLocation;
                        bacc.acres = pu[i].acres * -1;
                        bacc.attacksCount = 0;
                        bacc.defendCount = 1;
                        ac.Add(bacc);
                    }
                    else
                    {
                        checkProvDefend.defendCount += 1;
                        checkProvDefend.acres -= pu[i].acres;
                    }
                }
                
                sb.Append(homeKingdom + " total acre changes<br />");
                sb.Append("<ul>");
                foreach (var item in ac.Where(x => x.islandLocation == homeKingdom).OrderByDescending(x => x.acres))
                {
                    sb.Append("<li>");
                    if (item.acres > 0)
                        sb.Append("<span class=\"green\">+" + item.acres.ToString("N0") + "</span>");
                    else
                        sb.Append("<span class=\"red\">" + item.acres.ToString("N0") + "</span>");
                    sb.Append(" - ");
                    if (cachedKingdom.Provinces.Where(x => x.Province_ID == item.provID).FirstOrDefault() != null)
                        sb.Append("<a href=\"ProvinceDetail.aspx?ID=" + item.provID.RemoveDashes() + "\" target=\"_blank\">" + item.provName + "</a>");
                    else
                        sb.Append(item.provName);
                    sb.Append(" (" + item.attacksCount + "/" + item.defendCount + ")</li>");

                }
                sb.Append("</ul>");

                var uki = KingdomCache.getKingdom(ownerKingdomID, ownerKingdomID, cachedKingdom);
                if (homeKingdom != (uki.Kingdom_Island + ":" + uki.Kingdom_Location) | homeKingdom != (getCeInfo.Kingdom_Island + ":" + getCeInfo.Kingdom_Location))
                {
                    sb.Append(awayKingdom + " total acre changes<br />");
                    sb.Append("<ul>");
                    foreach (var item in ac.Where(x => x.islandLocation == awayKingdom).OrderByDescending(x => x.acres))
                    {
                        sb.Append("<li>");
                        if (item.acres > 0)
                            sb.Append("<span class=\"green\">+" + item.acres.ToString("N0") + "</span>");
                        else
                            sb.Append("<span class=\"red\">" + item.acres.ToString("N0") + "</span>");
                        sb.Append(" - ");
                        if (cachedKingdom.Provinces.Where(x => x.Province_ID == item.provID).FirstOrDefault() != null)
                            sb.Append("<a href=\"ProvinceDetail.aspx?ID=" + item.provID.RemoveDashes() + "\" target=\"_blank\">" + item.provName + "</a>");
                        else
                            sb.Append(item.provName);
                        sb.Append(" (" + item.attacksCount + "/" + item.defendCount + ")</li>");
                    }
                    sb.Append("</ul>");
                }

                TimeSpan tss = DateTime.UtcNow.Subtract(dt);
                PimpUserWrapper  pimpUser = new PimpUserWrapper ();
                if (pimpUser.PimpUser.IsUserAdmin)
                    sb.Append("QueryTime:" + ts.TotalSeconds + " BuildTime:" + tss.TotalSeconds);
            }
            return sb.ToString();
        }

    }
}
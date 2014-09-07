using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Boomers.Utilities.DatesTimes;

using Pimp.UParser;
using Pimp.UCache;
using Pimp.UIBuilder;
using Pimp;
using Pimp.Users;
using Pimp.Utopia;
using Pimp.UData;
using SupportFramework.Data;
using PimpLibrary.Static.Enums;

public partial class members_history_ops : MyBasePageCS
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string provID = string.Empty;
            if (Request.QueryString["ID"] != null)
                provID = new Guid(Request.QueryString["ID"]).ToString();
            else
                Response.Redirect("../Default.aspx");
            PimpUserWrapper pimpUser = new PimpUserWrapper();
            OwnedKingdomProvinces cachedKingdom = KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom);
            ltCSS.Text = "<link href=\"http://codingforcharity.org/utopiapimp/css/Default.css?v=" + SupportFramework.StaticContent.CSS.CssVersion + "\" rel='stylesheet' type='text/css' />";

            var ProvinceOwnerIDCheck = cachedKingdom.Provinces.Where(x => x.Province_ID == new Guid(provID)).FirstOrDefault();
            if (ProvinceOwnerIDCheck == null & !pimpUser.PimpUser.IsUserAdmin)
                Response.Redirect("../Default.aspx");

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();

            Page.Title = "[UtopiaPimp] Op History of " + ProvinceOwnerIDCheck.Province_Name;

            var getOps = (from xx in db.Utopia_Province_Ops
                          from yy in db.Utopia_Province_Ops_Pulls
                          where xx.Owner_Kingdom_ID == pimpUser.PimpUser.StartingKingdom
                          where xx.Added_By_Province_ID == new Guid(provID) | xx.Directed_To_Province_ID == new Guid(provID)
                          where xx.Op_ID == yy.uid
                          orderby xx.TimeStamp descending
                          select new
                          {
                              xx.OP_Text,
                              yy.OP_Name,
                              xx.Directed_To_Province_ID,
                              xx.Duration,
                              xx.Owner_Kingdom_ID,
                              xx.Added_By_Province_ID,
                              xx.negated,
                              xx.TimeStamp,
                              xx.Expiration_Date
                          });

            var provNames = cachedKingdom.Provinces;

            StringBuilder sb = new StringBuilder();
            sb.Append("<h2>Op History on " + ProvinceOwnerIDCheck.Province_Name + "</h2><br />");

            var getAddedBy = (from xx in getOps
                              where xx.Added_By_Province_ID == new Guid(provID)
                              select xx);
            sb.Append("<div class=\"divOpHistory\">");
            sb.Append("<div class=\"Title\">Ops Added by " + ProvinceOwnerIDCheck.Province_Name + "</div><br />");
            sb.Append("<ol class=\"olStyleHistory\">");
            foreach (var item in getAddedBy)
            {
                var toProv = (from xx in provNames
                              where xx.Province_ID == item.Directed_To_Province_ID | xx.Province_ID == item.Added_By_Province_ID
                              select new
                              {
                                  provName = xx.Province_Name + " (" + xx.Kingdom_Island + ":" + xx.Kingdom_Location + ")",
                                  xx.Province_ID
                              });

                if (toProv != null && toProv.Count() > 0)
                {
                    try
                    {
                        sb.Append("<li>");
                        ProvinceOpText(sb, item.Expiration_Date, item.OP_Name, item.OP_Text, toProv.Where(y => y.Province_ID == item.Directed_To_Province_ID).FirstOrDefault().provName, toProv.Where(x => x.Province_ID == item.Added_By_Province_ID).FirstOrDefault().provName, pimpUser.PimpUser.UserID);
                        sb.Append(" posted about ");
                        sb.Append(item.TimeStamp.ToLongRelativeDate());
                        sb.Append("</li>");
                    }
                    catch { }
                }
            }
            sb.Append("</ol>");
            sb.Append("</div>");
            var getDirected = (from xx in getOps
                               where xx.Directed_To_Province_ID == new Guid(provID)
                               select xx);
            sb.Append("<div class=\"divOpHistory\">");
            sb.Append("<div class=\"Title\">Ops Directed to " + ProvinceOwnerIDCheck.Province_Name + "</div><br />");
            sb.Append("<ol class=\"olStyleHistory\">");
            foreach (var item in getDirected)
            {
                sb.Append("<li>");
                try
                {

                    var expirationDate = item.Expiration_Date;
                    var opName = item.OP_Name;
                    var optext = item.OP_Text;
                    var toProvince = provNames.Where(x => x.Province_ID == item.Directed_To_Province_ID).FirstOrDefault().Province_Name;
                    var fromProvince = provNames.Where(x => x.Province_ID == item.Added_By_Province_ID).FirstOrDefault();
                    string fromProvinceName = "anonymous";
                    if (fromProvince != null)
                        fromProvinceName = fromProvince.Province_Name;
                    var userId = pimpUser.PimpUser.UserID;
                    ProvinceOpText(sb, expirationDate, opName, optext, toProvince, fromProvinceName, userId);
                    sb.Append(" posted about ");
                    sb.Append(item.TimeStamp.ToLongRelativeDate());

                }
                catch (Exception exception)
                {
                    Errors.logError(exception);
                }
                sb.Append("</li>");
            }
            sb.Append("</ol>");
            sb.Append("</div>");
            var getDirectedTo = (from xx in getOps
                                 where xx.Directed_To_Province_ID != new Guid(provID)
                                 select xx);
            sb.Append("<div class=\"divOpHistory\">");
            sb.Append("<div class=\"Title\">Ops Directed to Others by " + ProvinceOwnerIDCheck.Province_Name + " </div><br />");
            sb.Append("<ol class=\"olStyleHistory\">");
            foreach (var item in getDirectedTo)
            {
                sb.Append("<li>");
                ProvinceOpText(sb, item.Expiration_Date, item.OP_Name, item.OP_Text, provNames.Where(x => x.Province_ID == item.Directed_To_Province_ID).Select(x => x.Province_Name).FirstOrDefault(), provNames.Where(x => x.Province_ID == item.Added_By_Province_ID).Select(x => x.Province_Name).FirstOrDefault(), pimpUser.PimpUser.UserID);
                sb.Append(" posted about ");
                sb.Append(item.TimeStamp.ToLongRelativeDate());
                sb.Append("</li>");
            }
            sb.Append("</ol>");
            sb.Append("</div>");
            divForm.InnerHtml = sb.ToString();
        }
    }
    private static void ProvinceOpText(StringBuilder sb, DateTime? Expiration_Date, string OP_Name, string Op_Text, string toProvinceName, string fromProvinceName, Guid currentUserID)
    {
        switch (Expiration_Date.HasValue)
        {
            case true:

                switch (OP_Name)
                {
                    case "storms":
                        sb.Append("<span title=\"Storms\" class=\"spanEff\">Storms (<img src=\"" + ImagesStatic.Storms + "\" />" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + ")</span>");
                        break;
                    case "meteors":
                        sb.Append(" <span title=\"Meteors\" class=\"spanEff\">Meteors (<img src=\"" + ImagesStatic.Meteors + "\" />" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + ")</span>");
                        break;
                    case "riots":
                        sb.Append(" Riots caused by " + fromProvinceName + " towards " + toProvinceName + "<span title=\"Riots caused by " + fromProvinceName + " towards " + toProvinceName + "\" class=\"spanEff\"> (<img src=\"" + ImagesStatic.Riots + "\" />" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + ")</span>");
                        break;
                    case "greedySoldiers":
                        sb.Append(" <span title=\"Greedy Soldiers\" class=\"spanEff\">Greedy Soldiers (<img src=\"" + ImagesStatic.Greed + "\" />" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + ")</span>");
                        break;
                    case "highBirth":
                        sb.Append(" <span title=\"High Birth Rates \" class=\"noWrap\">High Birth Rates (<img src=\"" + ImagesStatic.HighBirthRates + "\" /><span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>)</span>");
                        break;
                    case "inspireArmy":
                        sb.Append("<span class=\"noWrap\" title=\"Inspire Army\">Inspire Army (<img src=\"" + ImagesStatic.InspireArmy + "\" /><span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>)</span>");
                        break;
                    case "minorProtection":
                        sb.Append(" <span class=\"noWrap\" title=\"Minor Protection\">Minor Protection (<img src=\"" + ImagesStatic.MinorProtection + "\" /><span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>)</span>");
                        break;
                    case "fog":
                        sb.Append(" <span class=\"noWrap\" title=\"Fog\">Fog (<img src=\"" + ImagesStatic.Fog + "\" /><span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>)</span>");
                        break;
                    case "magicShield":
                        sb.Append(" <span class=\"noWrap\" title=\"Magic Shield \">Magic Shield (<img src=\"" + ImagesStatic.MagicSheild + "\" /><span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>)</span>");
                        break;
                    case "fertileLands":
                        sb.Append(" <span class=\"noWrap\" title=\"Fertile Lands\">Fertile Lands (<img src=\"" + ImagesStatic.FertileLands + "\" /><span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>)</span>");
                        break;
                    case "naturesBlessing":
                        sb.Append(" <span class=\"noWrap\" title=\"Natures Blessing \">Natures Blessing (<img src=\"" + ImagesStatic.NaturesBlessing + "\" /><span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>)</span>");
                        break;
                    case "fastBuilders":
                        sb.Append(" <span class=\"noWrap\" title=\"Building Fast\">Building Fast (<img src=\"" + ImagesStatic.BuildersBoon + "\" /><span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>)</span>");
                        break;
                    case "patriotism":
                        sb.Append(" <span class=\"noWrap\" title=\"Patriots \">Patriots (<img src=\"" + ImagesStatic.Patriotism + "\" /><span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>)</span>");
                        break;
                    case "pitfalls":
                        sb.Append(" <span class=\"noWrap\" title=\"PitFalls\">PitFalls towards " + toProvinceName + " (<img src=\"" + ImagesStatic.Pitfalls + "\" /><span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>)</span>");
                        break;
                    case "explosions":
                        sb.Append(" <span title=\"Explosions\" class=\"spanEff\">Explosions (<img src=\"" + ImagesStatic.Explosions + "\" />" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>)");
                        break;
                    case "reflectMagic":
                        sb.Append(" " + toProvinceName + " Reflecting Magic<span title=\"Reflecting Magic\" class=\"spanEff\"> (<img src=\"" + ImagesStatic.ReflectingMagic + "\" />" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>)");
                        break;
                    case "fireball":
                        sb.Append(" <span title=\"FireBall by " + fromProvinceName + "\" class=\"spanEff\">FireBall by " + fromProvinceName + " (<img src=\"" + ImagesStatic.Fireball + "\" />" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>)");
                        break;
                    case "WarSpoils":
                    case "warSpoils":
                        sb.Append(" <span class=\"noWrap\" title=\"War Spoils " + KdPageHelper.PLEASE_MAKE_ICON + "\">War Spoils (WS<span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>)</span>");
                        break;
                    case "drought":
                        sb.Append(" <span class=\"noWrap\" title=\"Drought \">Drought (<img src=\"" + ImagesStatic.Drought + "\" /><span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>)</span>");
                        break;
                    case "tornadoes":
                        sb.Append("<span class=\"noWrap\" title=\"Tornadoes by " + fromProvinceName + "\" " + KdPageHelper.PLEASE_MAKE_ICON + "\">Tornadoes by " + fromProvinceName + "\" (T<span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>)</span>");
                        break;
                    case "vermin":
                        sb.Append(" <span title=\"Vermin by " + fromProvinceName + "\" class=\"spanEff\">Vermin by " + fromProvinceName + " (<img src=\"" + ImagesStatic.Vermin + "\" />" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>)");
                        break;
                    case "fountainKnowledge":
                        sb.Append(" <span class=\"noWrap\" title=\"Foutain of Knowledge " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>FoK<span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                        break;
                    case "chastity":
                        sb.Append(" <span class=\"noWrap\" title=\"Vow of Chastity " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>VoC<span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                        break;
                    case "landShadowLight":
                        sb.Append(" <span class=\"noWrap\" title=\"Blessed With Shadow of Light \">Shadow of Light (<img src=\"" + ImagesStatic.ShadowOfLight + "\" /><span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>)</span>");
                        break;
                    case "clearSight":
                        sb.Append(" <span class=\"noWrap\" title=\"Clear Sight " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>CS<span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                        break;
                    case "greatProtection":
                        sb.Append(" <span class=\"noWrap\" title=\"Greater Protection \"><span class=\"spanEff\"><img src=\"" + ImagesStatic.GreaterProtection + "\" />" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                        break;
                    case "armySpeed":
                        sb.Append(" <span class=\"noWrap\" title=\"Quick Feet " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>QF<span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                        break;
                    case "thievesInvisible":
                        sb.Append(" <span title=\"Partionally Invisible Thieves\" class=\"spanEff\"><img src=\"" + ImagesStatic.InvincibleThieves + "\" />" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span>");
                        break;
                    case "townWatch":
                        sb.Append(" <span class=\"noWrap\" title=\"Town Watch " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>TW<span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                        break;
                    case "MagesFury":
                        sb.Append(" <span class=\"noWrap\" title=\"Mages Fury " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>MF<span class=\"spanEff\">" + Expiration_Date.Value.Subtract(DateTime.UtcNow).ToShortRelativeDate() + "</span></span>");
                        break;

                    default:
                        UtopiaParser.FailedAt("'FailedHistoryOp'", toProvinceName + "; " + Expiration_Date + "; " + OP_Name + "; " + Op_Text + "; " + fromProvinceName, currentUserID);
                        break;
                } break;

            default:
                switch (OP_Name)
                {
                    case "stoleMoney":
                        sb.Append(" <span title=\" Stole " + Op_Text + "'s from " + toProvinceName + "\">Stole " + Op_Text + "'s from " + toProvinceName + " (<img src=\"" + ImagesStatic.StoleMoney + "\" />)</span>");
                        break;
                    case "stoleRunes":
                        sb.Append(" <span title=\" Stole " + Op_Text + " Runes from " + toProvinceName + KdPageHelper.PLEASE_MAKE_ICON + "\">Stole " + Op_Text + " Runes from " + toProvinceName + " (TSR)</span>");
                        break;
                    case "forgetBooks":
                        sb.Append(" <span title=\"" + toProvinceName + " Forgot " + Op_Text + " Books. " + KdPageHelper.PLEASE_MAKE_ICON + "\">Stole " + Op_Text + " Runes from " + toProvinceName + " (TSR)</span>");
                        break;
                    case "assasinate":
                        sb.Append(" <span title=\"" + fromProvinceName + " killed " + Op_Text + " troops\">" + fromProvinceName + " killed " + Op_Text + " troops (<img src=\"" + ImagesStatic.Assasinate + "\" />)</span>");
                        break;
                    case "kidnapped":
                        sb.Append(" <span title=\"" + fromProvinceName + " kidnapped " + Op_Text + " people " + KdPageHelper.PLEASE_MAKE_ICON + "\">" + fromProvinceName + " kidnapped " + Op_Text + " people (TKN)</span>");
                        break;
                    case "Infiltrated":
                        sb.Append(" <span title=\"Infiltrated " + toProvinceName + " to find " + Op_Text + " \">Infiltrated " + toProvinceName + " to find " + Op_Text + " (<img src=\"" + ImagesStatic.Infiltrated + "\" />)</span>");
                        break;
                    case "bribedGen":
                        sb.Append(" <span title=\"" + fromProvinceName + " Bribed a General. " + KdPageHelper.PLEASE_MAKE_ICON + "\">" + fromProvinceName + " Bribed a General. (BG)</span>");
                        break;
                    case "burnedAcres":
                        sb.Append(" <span title=\"" + fromProvinceName + " burned " + Op_Text + " acres.\">" + fromProvinceName + " burned " + Op_Text + " acres. (<img src=\"" + ImagesStatic.BurnedAcres + "\" />)</span>");
                        break;
                    case "tornadoes":
                        sb.Append(" <span title=\"" + fromProvinceName + " casted tornadoes, laying waste to " + Op_Text + " acres. \">" + fromProvinceName + " casted tornadoes, laying waste to " + Op_Text + " acres. (<img src=\"" + ImagesStatic.Tornados + "\" />)</span>");
                        break;
                    case "reflectingMagic":
                        sb.Append(" " + toProvinceName + " Reflecting Magic <span title=\"Province Reflecting Magic\" class=\"spanEff\"> (<img src=\"" + ImagesStatic.ReflectingMagic + "\" />)</span>");
                        break;
                    case "plague":
                        sb.Append(" <span title=\"Plague\">Plague (<img src=\"" + ImagesStatic.Plague + "\" />)</span>");
                        break;
                    case "naturesBlessingFailed":
                        sb.Append(" <span title=\"Province has Natures Blessing and is denying Storms and Droughts right now.\" class=\"spanEff\">Province has Natures Blessing and is denying Storms and Droughts right now. (<img src=\"" + ImagesStatic.NaturesBlessing + "\" />)</span>");
                        break;
                    case "mystVort":
                        sb.Append(" <span title=\"Mystic Vortex\">Mystic Vortex (<img src=\"" + ImagesStatic.MysticVortex + "\" />) " + Op_Text + "</span>");
                        break;
                    case "riots":
                        sb.Append(" Riots caused by " + fromProvinceName + " towards " + toProvinceName + "<span title=\"Riots caused by " + fromProvinceName + " towards " + toProvinceName + "\" class=\"spanEff\"> (<img src=\"" + ImagesStatic.Riots + "\" />)</span>");
                        break;
                    case "fireball":
                        sb.Append(" <span title=\"FireBall by " + fromProvinceName + "\" class=\"spanEff\">FireBall by " + fromProvinceName + " killed " + Op_Text + " (<img src=\"" + ImagesStatic.Fireball + "\" /></span>)");
                        break;
                    case "wakeDead":
                        sb.Append(" <span title=\"" + toProvinceName + " woke the dead\" class=\"spanEff\">" + toProvinceName + " woke the dead. (<img src=\"" + ImagesStatic.WakeDead + "\" />)");
                        break;
                    case "landLust":
                        sb.Append(" <span title=\"" + toProvinceName + " got land lusted for " + Op_Text + KdPageHelper.PLEASE_MAKE_ICON + "\" class=\"spanEff\">" + toProvinceName + " got land lusted for " + Op_Text + " (LL)");
                        break;
                    case "exposedThieves":
                        sb.Append(" <span title=\"" + toProvinceName + " had their theives exposed. \" class=\"spanEff\">" + toProvinceName + " had their thieves exposed. (<img src=\"" + ImagesStatic.ExposedThieves + "\" />)");
                        break;
                    case "bribed":
                        sb.Append(" <span title=\"" + toProvinceName + " had their theives bribed. " + KdPageHelper.PLEASE_MAKE_ICON + "\" class=\"spanEff\">" + toProvinceName + " had their thieves bribed. (BT)");
                        break;
                    case "stoleFood":
                        sb.Append(" <span title=\"" + fromProvinceName + " stole " + Op_Text + " bushels of food from " + toProvinceName + ". \">" + toProvinceName + " casted stole " + Op_Text + " bushels of food from " + toProvinceName + ". (SF)</span>");
                        break;
                    case "fog":
                        sb.Append(" <span class=\"noWrap\" title=\"Fog\">Fog (<img src=\"" + ImagesStatic.Fog + "\" /> No Date)</span>");
                        break;
                    case "assasinateWizs":
                        sb.Append(" <span title=\"" + fromProvinceName + " assasinated " + Op_Text + " Wizards of " + toProvinceName + KdPageHelper.PLEASE_MAKE_ICON + "\">" + fromProvinceName + " assasinated " + Op_Text + " wizards of " + fromProvinceName + ". (AW)</span>");
                        break;
                    case "sabotageSpells":
                        sb.Append(" <span title=\"Wizards spell casting has been sabatoged. \" class=\"spanEff\">Wizards spell casting has been sabatoged. (<img src=\"" + ImagesStatic.SabatogedSpells + "\" />)</span>");
                        break;
                    case "incineratesRunes":
                        sb.Append(" <span title=\"" + fromProvinceName + " incinerated " + Op_Text + " runes of " + toProvinceName + KdPageHelper.PLEASE_MAKE_ICON + "\">" + fromProvinceName + " incinerated " + Op_Text + " runes of " + toProvinceName + ". (IR)</span>");
                        break;

                    case "stealHorses":
                        sb.Append(" <span title=\"" + fromProvinceName + " Stole " + Op_Text + " war horses from " + toProvinceName + KdPageHelper.PLEASE_MAKE_ICON + "\">" + fromProvinceName + " incinerated " + Op_Text + " runes of " + toProvinceName + ". (IR)</span>");
                        break;
                    case "convertedTroops":
                        sb.Append(" <span title=\"" + fromProvinceName + " Converted " + Op_Text + " Troops from " + toProvinceName + KdPageHelper.PLEASE_MAKE_ICON + "\">" + fromProvinceName + " converted " + Op_Text + " troops of " + toProvinceName + ". (CT)</span>");
                        break;
                    case "convertThieves":
                        sb.Append(" <span title=\"" + fromProvinceName + " Converted " + Op_Text + " Thieves from " + toProvinceName + KdPageHelper.PLEASE_MAKE_ICON + "\">" + fromProvinceName + " converted " + Op_Text + " thieves of " + toProvinceName + ". (CT)</span>");
                        break;
                    case "convertedSpecialists":
                        sb.Append(" <span title=\"" + fromProvinceName + " Converted " + Op_Text + " Specialists from " + toProvinceName + KdPageHelper.PLEASE_MAKE_ICON + "\">" + fromProvinceName + " converted " + Op_Text + " specialists of " + toProvinceName + ". (CS)</span>");
                        break;
                    case "convertedWizards":
                        sb.Append(" <span title=\"" + fromProvinceName + " Converted " + Op_Text + " Wizards from " + toProvinceName + KdPageHelper.PLEASE_MAKE_ICON + "\">" + fromProvinceName + " converted " + Op_Text + " wizards of " + toProvinceName + ". (CW)</span>");
                        break;
                    case "Nightmares":
                        sb.Append(" <span title=\"" + fromProvinceName + " Gave " + Op_Text + " Soldiers Nightmares from " + toProvinceName + KdPageHelper.PLEASE_MAKE_ICON + "\">" + fromProvinceName + " gave " + Op_Text + " soldiers nightmares of " + toProvinceName + ". (N)</span>");
                        break;
                    case "donatedGoldDragon":
                        sb.Append(" <span title=\"Gold was donated to Launch the Dragon totallying ");
                        sb.Append(Op_Text + "gc's " + KdPageHelper.PLEASE_MAKE_ICON + "\" class=\"spanEff\">DG</span>");
                        break;
                    case "sentRunes":
                        sb.Append(" <span title=\"" + fromProvinceName + " sent " + Op_Text + " Runes to " + toProvinceName + KdPageHelper.PLEASE_MAKE_ICON + "\">" + fromProvinceName + " sent " + Op_Text + " runes to " + toProvinceName + ". (SAR)</span>");
                        break;
                    case "goldToLead":
                        sb.Append(" <span title=\"" + fromProvinceName + " Converted " + Op_Text + " Gold To Lead from " + toProvinceName + "\">" + fromProvinceName + " converted " + Op_Text + " gold to lead of " + toProvinceName + ". (<img src=\"" + ImagesStatic.GoldToLead + "\" />)</span>");
                        break;
                    case "sentMoney":
                        sb.Append(" <span title=\"" + fromProvinceName + " sent " + Op_Text + "gcs in Aid to " + toProvinceName + KdPageHelper.PLEASE_MAKE_ICON + "\">" + fromProvinceName + " sent " + Op_Text + " gcs in Aid to " + toProvinceName + ". (SM)</span>");
                        break;
                    case "reflectMagic":
                        sb.Append(" <span title=\"" + fromProvinceName + " Reflected Magic " + KdPageHelper.PLEASE_MAKE_ICON + "\">" + fromProvinceName + " Reflected Magic. (RM)</span>");
                        break;
                    case "killingDragon":
                        sb.Append(" <span title=\"" + fromProvinceName + " Sent " + Op_Text + " Troops Towards Killing The Dragon " + KdPageHelper.PLEASE_MAKE_ICON + "\">" + fromProvinceName + " Sent " + Op_Text + " Troops Towards Killing The Dragon. (KD)</span>");
                        break;
                    case "stormsNoEffects":
                        sb.Append(" <span title=\"" + fromProvinceName + " casted Storms that had no Effect on " + toProvinceName + KdPageHelper.PLEASE_MAKE_ICON + "\">" + fromProvinceName + " casted Storms that had no Effect on " + toProvinceName + ". (SNE)</span>");
                        break;
                    case "freePrisoners":
                        sb.Append(" <span title=\"" + fromProvinceName + " freed " + Op_Text + " Prisoners " + KdPageHelper.PLEASE_MAKE_ICON + "\">" + fromProvinceName + " freed " + Op_Text + " prisoners. (FP)</span>");
                        break;
                    case "chastity":
                        sb.Append(" <span class=\"noWrap\" title=\"Vow of Chastity " + KdPageHelper.PLEASE_MAKE_ICON + "\"><span class=\"spanEff\"></span>Vow of Chastity (VoC)</span>");
                        break;
                    case "triedToBurnAcres":
                        sb.Append(" <span title=\"" + fromProvinceName + " failed at burning the acres of " + toProvinceName + KdPageHelper.PLEASE_MAKE_ICON + "\">" + fromProvinceName + " failed at burning acres of " + toProvinceName + ". (TBA)</span>");
                        break;
                    default:
                        UtopiaParser.FailedAt("'PersonalOpsNoEffectsDate'", fromProvinceName + "; " + toProvinceName + "; " + Expiration_Date + "; " + OP_Name + "; " + Op_Text, currentUserID);
                        break;
                }
                break;
        }
    }
}

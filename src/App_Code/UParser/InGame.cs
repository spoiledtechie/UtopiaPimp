using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Boomers.Utilities.Guids;

using Pimp.UCache;
using PimpLibrary.Static.Enums;
using Pimp.Utopia;
using Pimp.Users;
using PimpLibrary.Utopia.Ops;
using SupportFramework.Data;
using Pimp.UData;

namespace Pimp.UParser
{
    /// <summary>
    /// Summary description for UtopiaParserInGame
    /// </summary>
    public partial class UtopiaParser
    {
        /// <summary>
        /// Checks What page it came from with InGame Layout.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        private static FromWhatPageEnum FromWhatPageInGame(string RawData, Guid currentUserID)
        {
            //The Throne Page
            if (RawData.Contains("Ruler") && RawData.Contains("Trade Balance") && RawData.Contains("Soldiers") && RawData.Contains("Thieves"))
            {
                if (RawData.Contains("From what we see,") && RawData.Contains("Remember,"))//Throne Away
                    return FromWhatPageEnum.InGameThroneAway;
                else
                    return FromWhatPageEnum.InGameThroneHome;//Throne Home
            }
            else if (RawData.Contains("I track some important information about the health of our province"))
                return FromWhatPageEnum.InGameAffairsOfState;
            //Internal Affairs
            else if (RawData.Contains("Current Available Workers") && RawData.Contains("Current Available Jobs") && RawData.Contains("Workers Needed for Max Efficiency"))
            {
                if (URegEx._findKingdomProvinceName.Match(RawData).Success)
                    return FromWhatPageEnum.InGameInternalAffairsPageAway;
                else
                    return FromWhatPageEnum.InGameInternalAffairsPageHome;
            }
            else if (RawData.Contains("Construction Time") && RawData.Contains("Construction Cost") && RawData.Contains("Free Building Credits"))
                return FromWhatPageEnum.InGameSurveyBuildings;
            //Buildings Advisor
            else if (RawData.Contains("You will find that as we build more of certain building types"))
            {
                if (RawData.Contains("Our thieves scour the lands of"))
                    return FromWhatPageEnum.InGameBuildingsAdvisorAway;
                else
                    return FromWhatPageEnum.InGameBuildingsAdvisor;
            }
            //Sciences
            else if (RawData.Contains("Alchemy") && RawData.Contains("Tools") && RawData.Contains("Channeling"))
            {
                if (RawData.Contains("Current Effects of our Research"))
                    return FromWhatPageEnum.InGameScienceAway; //Science Away
                else if (RawData.Contains("If your daily income drops below the Income Threshold, all science research will be temporarily halted.") | RawData.Contains("Allocating Books Knowledge You"))
                    return FromWhatPageEnum.InGameScienceHome;//Science Home
                else if (RawData.Contains("Our thieves visit the research centers of"))
                    return FromWhatPageEnum.InGameScienceAdvisorAway;
                else if (RawData.Contains("Current Effects of Research in the Science & Arts"))
                    return FromWhatPageEnum.InGameScienceAdvisorHome;//Home Science Advisor
                else
                    FailedAt("SciencesFromWhatPageInGame", RawData, currentUserID);
            }
            //Miltary
            else if (RawData.Contains("Offensive Military Effectiveness") && RawData.Contains("Defensive Military Effectivenes"))
                return FromWhatPageEnum.InGameMiltaryPage; //No difference ebetween pages.
            else if (RawData.Contains("you require a military presence to both defend your lands"))
                return FromWhatPageEnum.InGameMilitaryArmyTrainingPage;
            //Kingdom Page
            else if (RawData.Contains("Total Networth") && RawData.Contains("Total Land") && RawData.Contains("Stance"))
                return FromWhatPageEnum.InGameKingdomPage; //Doesn't Differ from Home or Away
            //CE Page
            else if (RawData.Contains("The Kingdom Reporter") | RawData.Contains("Our thieves have stolen the last 2 month's of kingdom news") | RawData.Contains("Our crystal eye has stolen the") | RawData.Contains("We have proposed a ceasefire offer to") | RawData.Contains("attacked and pillaged the lands of") | RawData.Contains("Dragon project targetted") | RawData.Contains("Our dragon has set flight") | RawData.Contains("has sent an aid shipment to") | RawData.Contains("attempted an invasion of") | RawData.Contains("has slain the dragon ravaging"))
                return FromWhatPageEnum.InGameCEPage; // Doesn't Differ from Home or Away
            else if (RawData.Contains("the magical auras affecting our province are detailed"))
                return FromWhatPageEnum.InGameMysticAffairs;
            else if (RawData.Contains("Your wizards gather") || RawData.Contains("Vermin will feast on the granaries") || RawData.Contains("This aid shipment has added") || RawData.Contains("lands are blessed with") || RawData.Contains("expect maintenance costs to be reduced") || RawData.Contains("realm is now under a sphere") || RawData.Contains("Much to the chagrin of their men") || RawData.Contains("A magic vortex") || RawData.Contains("Our mages have caused our") | RawData.Contains("magical calm") | RawData.Contains("extraordinarily fertile") | RawData.Contains("and the spell succeeds") || RawData.Contains("The spell consumes") || RawData.Contains("drought will reign") || RawData.Contains("Storms will ravage") || RawData.Contains("Meteors will rain") || RawData.Contains("Pitfalls will haunt") || RawData.Contains("will rock aid") || RawData.Contains("fireball burns") || RawData.Contains("Tornadoes scour") || RawData.Contains("Land Lust"))
                return FromWhatPageEnum.InGameMystics;
            else if (RawData.Contains("troops have spread the plague") || RawData.Contains("Early indications show that our operation was a success") || RawData.Contains("You send your thieves, and the operation commences") || RawData.Contains("Our thieves"))
                return FromWhatPageEnum.InGameThieves;
            else if (URegEx._sentAid.IsMatch(RawData) || RawData.Contains("gold coins to the quest of launching a dragon") || RawData.Contains("troops to fight the dragon. All are lost in the fight"))
                return FromWhatPageEnum.InGameExtras;
            else if (RawData.Contains("Our forces will be available again in") | RawData.Contains("battle begins quickly") | RawData.Contains("Your forces arrive at") | RawData.Contains("A tough battle took place") | RawData.Contains("Your army marches onto the enemy's lands"))
                return FromWhatPageEnum.InGameAttack;
            else if (RawData.Trim().Length == 32)
                return FromWhatPageEnum.ProvinceCodeGuid;
            else if (RawData.Contains("** Export Line") || RawData.Contains("Utopia Angel Generated Export Line"))
                return FromWhatPageEnum.ExportLineOnly;
            else
                FailedAt("'FromWhatPageInGameProblem'", RawData, currentUserID);
            return FromWhatPageEnum.None;
        }
        /// <summary>
        /// Gets any info from the affairs of the state page.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        private static string InGameAffairsOfState(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var getProv = (from xx in db.Utopia_Province_Data_Captured_Gens
                           where xx.Province_ID == currentUser.PimpUser.CurrentActiveProvince
                           select xx).FirstOrDefault();
            if (getProv != null)
            {
                getProv.Daily_Income = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx._findAffairsdi.Match(RawData).Value).Value);
                getProv.Land = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAffairsland.Match(RawData).Value).Value.Replace(",", ""));
                getProv.Peasents = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx._findAffairspeasants.Match(RawData).Value).Value);
                getProv.Thieves = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx._findAffairstheives.Match(RawData).Value).Value);
                getProv.Thieves_Value_Type = 1;
                getProv.Wizards = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx._findAffairswizards.Match(RawData).Value).Value);
                getProv.Wizards_Value_Type = 1;
                getProv.Population = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx._findAffairstotPop.Match(RawData).Value).Value);
                getProv.Networth = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAffairsnetWorth.Match(RawData).Value).Value.Replace(",", ""));
                getProv.Honor = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAffairshonor.Match(RawData).Value).Value.Replace(",", ""));
                getProv.Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
                getProv.Updated_By_DateTime = DateTime.UtcNow;
                db.SubmitChanges();
                ProvinceCache.updateStateAffairsToCache(getProv, cachedKingdom);
                return "Affairs Submitted " + getProv.Province_Name + " (" + getProv.Kingdom_Island + ":" + getProv.Kingdom_Location + ")";
            }
            Errors.failedAt("inGameAffairsInsertion", "activeProvince: " + currentUser.PimpUser.CurrentActiveProvince, currentUser.PimpUser.UserID);

            return String.Empty;
        }
        private static string ParseInGameMysticAffairs(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            RawData = RawData.Remove(0, URegEx._findMysticAffairsseperator.Match(RawData).Index);
            string item = string.Empty;
            int days = 0;
            switch (URegEx._findMysticAffairsmysticAura.IsMatch(RawData)) //No counter on this one because their is no time limit.
            {
                case true:
                    item = URegEx._findMysticAffairsmysticAura.Match(RawData).Value;
                    InsertOp(OpType.mysticAura, 0, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsmysticAura.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsStorms.IsMatch(RawData)) //Storms 13 days	Storms are ravaging our lands! This will last for 13 days
            {
                case true:
                    item = URegEx._findMysticAffairsStorms.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.storms, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsStorms.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsWarSpoils.IsMatch(RawData)) //War Spoils 3 days Our army has been blessed with immediate War Spoils for 3 days
            {
                case true:
                    item = URegEx._findMysticAffairsWarSpoils.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.warSpoils, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsWarSpoils.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsChastity.IsMatch(RawData)) //Chastity 6 days	The womenfolk's vow of chastity is preventing any population growth for 6 days!
            {
                case true:
                    item = URegEx._findMysticAffairsChastity.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.chastity, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsChastity.Replace(RawData, "");
                    break;
            }

            switch (URegEx._findMysticAffairsVermin.IsMatch(RawData)) //Vermin 3 days Vermin have been discovered eating away our food supplies, and cannot be exterminated for 3 days
            {
                case true:
                    item = URegEx._findMysticAffairsVermin.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.vermin, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsVermin.Replace(RawData, "");
                    break;
            }

            switch (URegEx._findMysticAffairsInvincible.IsMatch(RawData)) //Invisibility 16 days Our thieves have been made partially invisible for 16 days 
            {
                case true:
                    item = URegEx._findMysticAffairsInvincible.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.thievesInvisible, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsInvincible.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsFanaticism.IsMatch(RawData)) //Invisibility 16 days Our thieves have been made partially invisible for 16 days 
            {
                case true:
                    item = URegEx._findMysticAffairsFanaticism.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.fanaticism, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsFanaticism.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsPitfalls.IsMatch(RawData)) //Pitfalls 14 days Pitfalls are haunting our lands for 14 days, causing increased defensive losses during battle. 
            {
                case true:
                    item = URegEx._findMysticAffairsPitfalls.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.pitfalls, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsPitfalls.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsmeteors.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairsmeteors.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.meteors, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsmeteors.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsAggression.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairsAggression.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.aggression, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsAggression.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsfertileLands.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairsfertileLands.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.fertileLands, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsfertileLands.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsGreaterProtection.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairsGreaterProtection.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.greatProtection, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsGreaterProtection.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsreflectSpells.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairsreflectSpells.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.reflectMagic, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsreflectSpells.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsfountainKnowledge.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairsfountainKnowledge.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.fountainKnowledge, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsfountainKnowledge.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsarmySpeed.IsMatch(RawData)) //No counter on this one because their is no time limit.
            {
                case true:
                    item = URegEx._findMysticAffairsarmySpeed.Match(RawData).Value;
                    InsertOp(OpType.armySpeed, 0, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsarmySpeed.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairslandShadowLight.IsMatch(RawData)) //No counter on this one because their is no time limit.
            {
                case true:
                    item = URegEx._findMysticAffairslandShadowLight.Match(RawData).Value;
                    InsertOp(OpType.landShadowLight, 0, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairslandShadowLight.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsbuildHaste.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairsbuildHaste.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.fastBuilders, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsbuildHaste.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsraiseDead.IsMatch(RawData)) //No counter on this one because their is no time limit.
            {
                case true:
                    item = URegEx._findMysticAffairsraiseDead.Match(RawData).Value;
                    InsertOp(OpType.NoRaiseDead, 0, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsraiseDead.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsgreedyArmy.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairsgreedyArmy.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.greedySoldiers, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsgreedyArmy.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsblessedLand.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairsblessedLand.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.naturesBlessing, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsblessedLand.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsclearSight.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairsclearSight.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.clearSight, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsclearSight.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsfog.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairsfog.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.fog, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsfog.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsMagicShield.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairsMagicShield.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.magicShield, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsMagicShield.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsTownWatch.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairsTownWatch.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.townWatch, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsTownWatch.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsprotectBlackMagic.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairsprotectBlackMagic.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.ProtectMagic, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsprotectBlackMagic.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsMinorProtection.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairsMinorProtection.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.minorProtection, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsMinorProtection.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairspeace.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairspeace.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.highBirth, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairspeace.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairspatriotic.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairspatriotic.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.patriotism, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairspatriotic.Replace(RawData, "");
                    break;
            }

            switch (URegEx._findMysticAffairsarmyTrain.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairsarmyTrain.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.inspireArmy, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsarmyTrain.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsdrought.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairsdrought.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.drought, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsdrought.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairstroopStrength.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairstroopStrength.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.TroopStrength, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairstroopStrength.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsExplosions.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairsExplosions.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.explosions, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsExplosions.Replace(RawData, "");
                    break;
            }
            switch (URegEx._findMysticAffairsMagesFury.IsMatch(RawData))
            {
                case true:
                    item = URegEx._findMysticAffairsMagesFury.Match(RawData).Value;
                    days = Convert.ToInt32(URegEx.rgxNumber.Match(item).Value);
                    InsertOp(OpType.MagesFury, days, currentUser.PimpUser.CurrentActiveProvince, currentUser, cachedKingdom);
                    RawData = URegEx._findMysticAffairsMagesFury.Replace(RawData, "");
                    break;
            }
            if (URegEx._findMysticAffairscounter.IsMatch(RawData))
                FailedAt("'FailedInGameMysticAffairs'", RawData, currentUser.PimpUser.UserID);

            var prov = ProvinceCache.getProvince(currentUser.PimpUser.StartingKingdom, currentUser.PimpUser.CurrentActiveProvince, cachedKingdom);
            return "Mystics Submitted " + prov.Province_Name + " (" + prov.Kingdom_Island + ":" + prov.Kingdom_Location + ")";
        }
        /// <summary>
        /// Parses the in game throne page.
        /// </summary>
        /// <param name="RawData">Good Data</param>
        /// <returns>Province ID of throne.</returns>
        private static string ParseInGameThrone(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            if (!URegEx._findThroneProvinceName.IsMatch(RawData))
                return ReturnErrorsToUser(ErrorTypeEnum.FindProvinceName);
            string ProvinceIslandLocation = URegEx._findThroneProvinceName.Match(RawData).Value;
            string ProvinceName = URegEx.rgxFindIslandLocation.Replace(ProvinceIslandLocation, "").Replace("Province of ", "").Trim();
            int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(ProvinceIslandLocation).Value).Value).Value);
            int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(ProvinceIslandLocation).Value).Value).Value);
            Guid ProvinceID = ProvinceCache.getProvinceID(ProvinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
            var getProvince = (from UPDCG in db.Utopia_Province_Data_Captured_Gens
                               where UPDCG.Province_ID == ProvinceID
                               select UPDCG).FirstOrDefault();
            var kingdom = cachedKingdom;

            CS_Code.Utopia_Province_Data_Captured_CB cb = new CS_Code.Utopia_Province_Data_Captured_CB();
            cb.Province_ID = ProvinceID;
            cb.Owner_Kingdom_ID = getProvince.Owner_Kingdom_ID;
            cb.Kingdom_ID = getProvince.Kingdom_ID;
            cb.Kingdom_Island = getProvince.Kingdom_Island;
            cb.Kingdom_Location = getProvince.Kingdom_Location;
            cb.Province_Name = getProvince.Province_Name;

            getProvince.Race_ID = getRaceID(URegEx._findRace.Match(URegEx._findInGameRace.Match(RawData).Value).Value, currentUser.PimpUser.UserID);
            RawData = RawData.Replace(URegEx._findInGameRace.Match(RawData).Value, "");
            getProvince.Ruler_Name = URegEx._findOffense.Replace(new Regex(string.Format(@"\s*Ruler\s+{0}\s*", URegEx._nobilities), RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(URegEx._findInGameRulerName.Match(RawData).Value, ""), "").Trim();
            getProvince.Personality_ID = GetPersonalityID(FindPersonality(URegEx._findPersonalitySearchKey.Match(getProvince.Ruler_Name).Value, currentUser.PimpUser.UserID));

            cb.Race_ID = getProvince.Race_ID;
            cb.Ruler_Name = getProvince.Ruler_Name;
            cb.Personality_ID = getProvince.Personality_ID;

            var GetMilitaryNames = UtopiaHelper.Instance.Races.Where(x => x.uid == getProvince.Race_ID).FirstOrDefault();
            switch (URegEx._findInGameHitHard.Match(RawData).Success)
            {
                case true:
                    string hit = URegEx._findInGameHitHard.Match(RawData).Value;
                    switch (URegEx._findInGameHit.Match(hit).Value)
                    {
                        case "noticably":
                            getProvince.Hit = "none";
                            break;
                        case "extremely":
                            getProvince.Hit = "extreme";
                            break;
                        case "little":
                            getProvince.Hit = "couple";
                            break;
                        default:
                            getProvince.Hit = URegEx._findInGameHit.Match(hit).Value;
                            break;
                    }
                    cb.Hit = getProvince.Hit;
                    break;
            }

            Regex rgxOffNumbers = new Regex(GetMilitaryNames.soldierOffName + @"\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex rgxDefNumbers = new Regex(GetMilitaryNames.soldierDefName + @"\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex rgxEliteNumbers = new Regex(GetMilitaryNames.eliteName + @"(s)?\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            if (!rgxOffNumbers.IsMatch(RawData))
                return ReturnErrorsToUser(ErrorTypeEnum.WordsAndNumbersBunchedUp);
            getProvince.Soldiers_Regs_Off = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(rgxOffNumbers.Match(RawData).Value).Value.Replace(",", ""));
            getProvince.Soldiers_Regs_Def = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(rgxDefNumbers.Match(RawData).Value).Value.Replace(",", ""));
            getProvince.Soldiers_Elites = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(rgxEliteNumbers.Match(RawData).Value).Value.Replace(",", ""));

            cb.Soldiers_Regs_Off = getProvince.Soldiers_Regs_Off;
            cb.Soldiers_Regs_Def = getProvince.Soldiers_Regs_Def;
            cb.Soldiers_Elites = getProvince.Soldiers_Elites;

            if (URegEx._findInGameGetSoldiers.IsMatch(RawData))
            {
                getProvince.Soldiers = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findInGameGetSoldiers.Match(RawData).Value).Value.Replace(",", ""));
                cb.Soldiers = getProvince.Soldiers;
                RawData = RawData.Replace(URegEx._findInGameGetSoldiers.Match(RawData).Value, "");
            }
            getProvince.Land = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findInGameLand.Match(RawData).Value).Value.Replace(",", ""));
            cb.Land = getProvince.Land;
            RawData = RawData.Replace(URegEx._findInGameLand.Match(RawData).Value, "");

            getProvince.Peasents = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findInGamePeasants.Match(RawData).Value.Replace(",", "")).Value);
            cb.Peasents = getProvince.Peasents;
            RawData = RawData.Replace(URegEx._findInGamePeasants.Match(RawData).Value, "");

            getProvince.Building_Effectiveness = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findInGameBuildingEff.Match(RawData).Value).Value.Replace(",", ""));
            cb.Building_Effectiveness = getProvince.Building_Effectiveness;
            RawData = RawData.Replace(URegEx._findInGameBuildingEff.Match(RawData).Value, "");

            getProvince.Money = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findInGameMoney.Match(RawData).Value).Value.Replace(",", ""));
            cb.Money = getProvince.Money;
            RawData = RawData.Replace(URegEx._findInGameMoney.Match(RawData).Value, "");

            getProvince.Food = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findInGameFood.Match(RawData).Value).Value.Replace(",", ""));
            cb.Food = getProvince.Food;
            RawData = RawData.Replace(URegEx._findInGameFood.Match(RawData).Value, "");

            getProvince.Runes = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findInGameRunes.Match(RawData).Value).Value.Replace(",", ""));
            cb.Runes = getProvince.Runes;
            RawData = RawData.Replace(URegEx._findInGameRunes.Match(RawData).Value, "");

            getProvince.Trade_Balance = Convert.ToInt32(URegEx._findInGameTradeB.Match(RawData).Value.Replace("Trade Balance", "").Replace(",", "").Replace(" ", "").Replace("gc", ""));
            cb.Trade_Balance = getProvince.Trade_Balance;
            RawData = RawData.Replace(URegEx._findInGameTradeB.Match(RawData).Value, "");

            if (URegEx._findInGameThieves.IsMatch(RawData) && !URegEx._findInGameThieves.Match(RawData).Value.Contains("Unknown"))
            {
                getProvince.Thieves = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findInGameThieves.Match(RawData).Value).Value.Replace(",", ""));
                getProvince.Thieves_Value_Type = 1;
                cb.Thieves = getProvince.Thieves;
                cb.Thieves_Value_Type = getProvince.Thieves_Value_Type;
                RawData = RawData.Replace(URegEx._findInGameThieves.Match(RawData).Value, "");

                getProvince.Wizards = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findInGameWizards.Match(RawData).Value).Value.Replace(",", ""));
                getProvince.Wizards_Value_Type = 1;
                cb.Wizards = getProvince.Wizards;
                cb.Wizards_Value_Type = getProvince.Wizards_Value_Type;
                RawData = RawData.Replace(URegEx._findInGameWizards.Match(RawData).Value, "");
            }
            getProvince.War_Horses = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findInGameWarHorses.Match(RawData).Value).Value.Replace(",", ""));
            cb.War_Horses = getProvince.War_Horses;
            RawData = RawData.Replace(URegEx._findInGameWarHorses.Match(RawData).Value, "");

            getProvince.Prisoners = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findInGamePrisoners.Match(RawData).Value).Value.Replace(",", ""));
            cb.Prisoners = getProvince.Prisoners;
            RawData = RawData.Replace(URegEx._findInGamePrisoners.Match(RawData).Value, "");

            getProvince.CB_Updated_By_DateTime = DateTime.UtcNow;
            getProvince.CB_Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            cb.Kingdom_Island = getProvince.Kingdom_Island;
            cb.Kingdom_Location = getProvince.Kingdom_Location;
            cb.Province_Name = getProvince.Province_Name;
            cb.Updated_By_DateTime = DateTime.UtcNow;
            cb.Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;

            getProvince.Military_Net_Off = Convert.ToDecimal(URegEx.rgxQuantitiesWithComma.Match(URegEx._findInGameOffPoints.Match(RawData).Value).Value.Replace(",", ""));
            cb.Total_Mod_Offense = Convert.ToDecimal(URegEx.rgxQuantitiesWithComma.Match(URegEx._findInGameOffPoints.Match(RawData).Value).Value.Replace(",", ""));

            RawData = RawData.Replace(URegEx._findInGameOffPoints.Match(RawData).Value, "");

            if (URegEx._findInGameDefPoints.IsMatch(RawData))
            {
                getProvince.Military_Net_Def = Convert.ToDecimal(URegEx.rgxQuantitiesWithComma.Match(URegEx._findInGameDefPoints.Match(RawData).Value).Value.Replace(",", ""));
                cb.Total_Mod_Defense = getProvince.Military_Net_Def;
                RawData = RawData.Replace(URegEx._findInGameDefPoints.Match(RawData).Value, "");
            }
            else
                cb.Total_Mod_Defense = (decimal)CalcModDefense(CalcRawDefense(getProvince.Race_ID.GetValueOrDefault(), getProvince.Soldiers.GetValueOrDefault(), getProvince.Soldiers_Regs_Def.GetValueOrDefault(), getProvince.Soldiers_Elites.GetValueOrDefault(), getProvince.Peasents.GetValueOrDefault()), (double)getProvince.Mil_Overall_Efficiency.GetValueOrDefault());

            getProvince.Population = getProvince.Peasents.GetValueOrDefault(0) + getProvince.Soldiers.GetValueOrDefault(0) + getProvince.Soldiers_Elites.GetValueOrDefault(0) + getProvince.Soldiers_Regs_Def.GetValueOrDefault(0) + getProvince.Soldiers_Regs_Off.GetValueOrDefault(0) + getProvince.Wizards.GetValueOrDefault(0) + getProvince.Thieves.GetValueOrDefault(0);
            cb.Population = getProvince.Population;

            //(Soldiers + Offensive_Units + Defensive_Units + Elite_Units + Thieves) / Population * 10000) / 100
            getProvince.Draft = CalcDraftRate(getProvince.Soldiers.GetValueOrDefault(0), getProvince.Soldiers_Regs_Off.GetValueOrDefault(0), getProvince.Soldiers_Regs_Def.GetValueOrDefault(0), getProvince.Soldiers_Elites.GetValueOrDefault(0), getProvince.Thieves.GetValueOrDefault(0), getProvince.Population.GetValueOrDefault(1));
            cb.Draft = getProvince.Draft;

            getProvince.Daily_Income = CalcDailyIncome(getProvince.Nobility_ID.GetValueOrDefault(0), getProvince.Prisoners.GetValueOrDefault(0), getProvince.Peasents.GetValueOrDefault(1), getProvince.Race_ID.GetValueOrDefault(0), getProvince.Personality_ID.GetValueOrDefault(0));
            cb.Daily_Income = getProvince.Daily_Income;

            if (RawData.Contains("Plague has spread throughout our people"))
                InsertOp(OpType.plague, getProvince.Province_ID, currentUser, cachedKingdom);

            getProvince.Updated_By_DateTime = DateTime.UtcNow;
            getProvince.Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            getProvince.CB_Requested = null;

            cb.Building_Effectiveness = getProvince.Building_Effectiveness;
            //Raw Off = Soldiers + (Offensive Specs * Offspec value) + (Elites * Elite Attack value) + Horses + [(Mercs+Prisoners)*3]
            var kingdomAway = kingdom.Kingdoms.Where(x => x.Kingdom_Island == sourceIsland).Where(x => x.Kingdom_Location == sourceLocation).FirstOrDefault();
            int stance = 1;
            if (kingdomAway != null)
                stance = kingdomAway.Stance.GetValueOrDefault();
            var provinceSurv = kingdom.Provinces.Where(x => x.Province_ID == getProvince.Province_ID).FirstOrDefault();
            int tgs = 0;
            decimal be = 0;

            if (provinceSurv != null)
            {
                var surv = provinceSurv.Survey.FirstOrDefault();
                if (surv != null)
                {
                    tgs = surv.TG_B.GetValueOrDefault();
                    be = surv.Building_Efficiency.GetValueOrDefault();
                }
            }

            var tgb = CalcTrainingGroundBonus(tgs, (double)be);
            var ro = CalcRawOffense(getProvince.Race_ID.GetValueOrDefault(), getProvince.Soldiers.GetValueOrDefault(), false, getProvince.Soldiers_Regs_Off.GetValueOrDefault(), getProvince.Soldiers_Elites.GetValueOrDefault(), getProvince.War_Horses.GetValueOrDefault(), 0, CalcOptimizedPrisoners(getProvince.Prisoners.GetValueOrDefault(), getProvince.Soldiers.GetValueOrDefault(), getProvince.Soldiers_Regs_Off.GetValueOrDefault(), getProvince.Soldiers_Elites.GetValueOrDefault(), 0));

            cb.Total_Prac_Offense = cb.Total_Mod_Offense;
            cb.Total_Prac_Defense = GetPracDefense(GetMilitaryNames, cb.Soldiers_Regs_Def, cb.Soldiers, getProvince.Mil_Overall_Efficiency, stance);

            db.Utopia_Province_Data_Captured_CBs.InsertOnSubmit(cb);
            db.SubmitChanges();
            ProvinceCache.UpdateProvinceCBToCache(cb, getProvince, cachedKingdom);
            return "CB Submitted " + ProvinceName + " (" + sourceIsland + ":" + sourceLocation + ")";
        }


        /// <summary>
        /// Parses the In Game Survey Away Page.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        private static string ParseInGameSurveyAway(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            string provinceName = URegEx.rgxFindIslandLocation.Replace(URegEx._findSurveyInGameProvinceName.Match(RawData).Value, "").Replace("lands of ", "").Trim();
            int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);
            int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Province_Data_Captured_Survey UPDCS = new CS_Code.Utopia_Province_Data_Captured_Survey();
            UPDCS.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
            UPDCS.Province_ID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
            UPDCS.Province_ID_Updated_By = currentUser.PimpUser.CurrentActiveProvince;
            UPDCS.DateTime_Updated = DateTime.UtcNow;
            UPDCS.Building_Efficiency = Convert.ToDecimal(URegEx._findPercentages.Match(URegEx._findBuildingEfficiency.Match(RawData).Value).Value.Replace("%", ""));

            int Acres = 0;
            string temp = "";
            foreach (Match match in URegEx._findSurveyInGameLines.Matches(RawData))
            {
                temp = match.Value.Replace(URegEx._findPercentages.Match(match.Value).Value.Replace("%", ""), "");
                switch (URegEx.rgxQuantitiesWithComma.Match(temp).Success)
                {
                    case true:
                        Acres += Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        break;
                }
                SurveyInGameBuildingType(UPDCS, temp, match, currentUser.PimpUser.UserID);
            }
            db.Utopia_Province_Data_Captured_Surveys.InsertOnSubmit(UPDCS);

            var Province_Info = (from UPDCG in db.Utopia_Province_Data_Captured_Gens
                                 where UPDCG.Province_ID == UPDCS.Province_ID
                                 select UPDCG).FirstOrDefault();
            Province_Info.Land = Acres;
            Province_Info.Updated_By_DateTime = DateTime.UtcNow;
            Province_Info.Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            Province_Info.Survey_Requested = null;
            db.SubmitChanges();
            ProvinceCache.updateProvinceSurveyToCache(Acres, UPDCS, Province_Info, cachedKingdom);
            return "Survey Submitted " + provinceName + " (" + sourceIsland + ":" + sourceLocation + ")";
        }
        /// <summary>
        /// Parses the Ingame Survey for Home.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        private static string ParseInGameSurveyHome(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Province_Data_Captured_Survey UPDCS = new CS_Code.Utopia_Province_Data_Captured_Survey();
            UPDCS.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
            UPDCS.Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            UPDCS.Province_ID_Updated_By = UPDCS.Province_ID;
            UPDCS.DateTime_Updated = DateTime.UtcNow;
            UPDCS.Building_Efficiency = Convert.ToDecimal(URegEx._findPercentages.Match(URegEx._findBuildingEfficiency.Match(RawData).Value).Value.Replace("%", ""));

            int Acres = 0;
            string temp = "";
            foreach (Match match in URegEx._findSurveyInGameLines.Matches(RawData))
            {
                temp = match.Value.Replace(URegEx._findPercentages.Match(match.Value).Value.Replace("%", ""), "");
                switch (URegEx.rgxQuantitiesWithComma.Match(temp).Success)
                {
                    case true:
                        Acres += Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        break;
                }
                SurveyInGameBuildingType(UPDCS, temp, match, currentUser.PimpUser.UserID);
            }
            db.Utopia_Province_Data_Captured_Surveys.InsertOnSubmit(UPDCS);

            var Province_Info = (from UPDCG in db.Utopia_Province_Data_Captured_Gens
                                 where UPDCG.Province_ID == UPDCS.Province_ID
                                 select UPDCG).FirstOrDefault();
            Province_Info.Land = Acres;
            Province_Info.Updated_By_DateTime = DateTime.UtcNow;
            Province_Info.Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            Province_Info.Survey_Requested = null;
            db.SubmitChanges();
            ProvinceCache.updateProvinceSurveyToCache(Acres, UPDCS, Province_Info, cachedKingdom);
            return "Survey Submitted " + Province_Info.Province_Name + " (" + Province_Info.Kingdom_Island + ":" + Province_Info.Kingdom_Location + ")";
        }
        private static string InGameBuildingsAdvisorAway(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            string provinceName = URegEx.rgxFindIslandLocation.Replace(URegEx._findSurveyInGameAwayProvinceName.Match(RawData).Value, "").Replace("scour the lands of", "").Trim();
            int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);
            int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);
            Guid provID = currentUser.PimpUser.CurrentActiveProvince;
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Province_Data_Captured_Survey UPDCS = new CS_Code.Utopia_Province_Data_Captured_Survey();
            UPDCS.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
            UPDCS.Province_ID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
            UPDCS.Province_ID_Updated_By = UPDCS.Province_ID;
            UPDCS.DateTime_Updated = DateTime.UtcNow;
            UPDCS.Building_Efficiency = Convert.ToDecimal(URegEx._findQuantitiesDecimal.Match(URegEx._findBuildingsAdvisorbuildEffic.Match(RawData).Value).Value);
            string temp;
            int land = 0;
            MatchCollection mc = URegEx._findBuildingsAdvisorBuildingLines.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                temp = mc[i].Value;
                switch (URegEx._findBuildingTypes.Match(temp).Value)
                {
                    case "Homes":
                        UPDCS.Homes_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Homes_B;
                        break;
                    case "Farms":
                        UPDCS.Farms_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Farms_B;
                        break;
                    case "Mills":
                        UPDCS.Mills_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Mills_B;
                        break;
                    case "Banks":
                        UPDCS.Banks_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Banks_B;
                        break;
                    case "Training Grounds":
                        UPDCS.TG_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.TG_B;
                        break;
                    case "Armouries":
                        UPDCS.Armories_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Armories_B;
                        break;
                    case "Barracks":
                        UPDCS.Barracks_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Barracks_B;
                        break;
                    case "Forts":
                        UPDCS.Forts_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Forts_B;
                        break;
                    case "Guard Stations":
                        UPDCS.GS_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.GS_B;
                        break;
                    case "Hospitals":
                        UPDCS.Hospitals_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Hospitals_B;
                        break;
                    case "Guilds":
                        UPDCS.Guilds_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Guilds_B;
                        break;
                    case "Towers":
                        UPDCS.Towers_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Towers_B;
                        break;
                    case "Thieves' Dens":
                        UPDCS.TD_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.TD_B;
                        break;
                    case "Watch Towers":
                        UPDCS.WT_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.WT_B;
                        break;
                    case "Libraries":
                        UPDCS.Library_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Library_B;
                        break;
                    case "Schools":
                        UPDCS.Schools_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Schools_B;
                        break;
                    case "Stables":
                        UPDCS.Stables_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Stables_B;
                        break;
                    case "Dungeons":
                        UPDCS.Dungeons_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Dungeons_B;
                        break;
                    case "Barren Land":
                        UPDCS.BarrenLands = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.BarrenLands;
                        break;
                    default:
                        UtopiaParser.FailedAt("FailedBuildingsAdvisorInGame", temp, currentUser.PimpUser.UserID);
                        break;
                }
            }

            db.Utopia_Province_Data_Captured_Surveys.InsertOnSubmit(UPDCS);

            var Province_Info = (from UPDCG in db.Utopia_Province_Data_Captured_Gens
                                 where UPDCG.Province_ID == UPDCS.Province_ID
                                 select UPDCG).FirstOrDefault();
            Province_Info.Land = land;
            Province_Info.Updated_By_DateTime = DateTime.UtcNow;
            Province_Info.Updated_By_Province_ID = provID;
            Province_Info.Survey_Requested = null;
            db.SubmitChanges();
            KingdomCache.removeProvinceFromKingdomCache(currentUser.PimpUser.StartingKingdom, Province_Info.Province_ID, cachedKingdom);
            return "Building Affairs for " + provinceName + "(" + sourceIsland + ":" + sourceLocation + ")";
        }
        private static string InGameBuildingsAdvisor(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Province_Data_Captured_Survey UPDCS = new CS_Code.Utopia_Province_Data_Captured_Survey();
            UPDCS.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
            UPDCS.Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            UPDCS.Province_ID_Updated_By = UPDCS.Province_ID;
            UPDCS.DateTime_Updated = DateTime.UtcNow;
            if (URegEx._findBuildingsAdvisorbuildEffic.IsMatch(RawData))
                UPDCS.Building_Efficiency = Convert.ToDecimal(URegEx._findQuantitiesDecimal.Match(URegEx._findBuildingsAdvisorbuildEffic.Match(RawData).Value).Value);
            string temp;
            int land = 0;
            MatchCollection mc = URegEx._findBuildingsAdvisorBuildingLines.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                temp = mc[i].Value;
                switch (URegEx._findBuildingTypes.Match(temp).Value)
                {
                    case "Homes":
                        UPDCS.Homes_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Homes_B;
                        break;
                    case "Farms":
                        UPDCS.Farms_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Farms_B;
                        break;
                    case "Mills":
                        UPDCS.Mills_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Mills_B;
                        break;
                    case "Banks":
                        UPDCS.Banks_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Banks_B;
                        break;
                    case "Training Grounds":
                        UPDCS.TG_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.TG_B;
                        break;
                    case "Armouries":
                        UPDCS.Armories_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Armories_B;
                        break;
                    case "Barracks":
                        UPDCS.Barracks_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Barracks_B;
                        break;
                    case "Forts":
                        UPDCS.Forts_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Forts_B;
                        break;
                    case "Guard Stations":
                        UPDCS.GS_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.GS_B;
                        break;
                    case "Hospitals":
                        UPDCS.Hospitals_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Hospitals_B;
                        break;
                    case "Guilds":
                        UPDCS.Guilds_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Guilds_B;
                        break;
                    case "Towers":
                        UPDCS.Towers_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Towers_B;
                        break;
                    case "Thieves' Dens":
                        UPDCS.TD_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.TD_B;
                        break;
                    case "Watch Towers":
                        UPDCS.WT_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.WT_B;
                        break;
                    case "Libraries":
                        UPDCS.Library_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Library_B;
                        break;
                    case "Schools":
                        UPDCS.Schools_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Schools_B;
                        break;
                    case "Stables":
                        UPDCS.Stables_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Stables_B;
                        break;
                    case "Dungeons":
                        UPDCS.Dungeons_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.Dungeons_B;
                        break;
                    case "Barren Land":
                        UPDCS.BarrenLands = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                        land += (int)UPDCS.BarrenLands;
                        break;
                    default:
                        FailedAt("FailedBuildingsAdvisorInGame", temp, currentUser.PimpUser.UserID);
                        break;
                }
            }
            if (RawData.Contains("Exploration/Construction Schedules"))
            {
                RawData = RawData.Remove(0, RawData.IndexOf("Exploration/Construction Schedules"));
                mc = URegEx._findBuildingsAdvisorBuildingTraining.Matches(RawData);
                UPDCS.Acres_In_Progress = 0;
                for (int i = 0; i < mc.Count; i++)
                {
                    temp = mc[i].Value;
                    switch (URegEx._findBuildingTypes.Match(temp).Value)
                    {
                        case "Homes":
                            UPDCS.Homes_P = AddBuildingsAdvisorTrainingMatches(temp);
                            land += (int)UPDCS.Homes_P;
                            UPDCS.Acres_In_Progress += (int)UPDCS.Homes_P;
                            break;
                        case "Farms":
                            UPDCS.Farms_P = AddBuildingsAdvisorTrainingMatches(temp);
                            land += (int)UPDCS.Farms_P;
                            UPDCS.Acres_In_Progress += (int)UPDCS.Farms_P;
                            break;
                        case "Mills":
                            UPDCS.Mills_P = AddBuildingsAdvisorTrainingMatches(temp);
                            land += (int)UPDCS.Mills_P;
                            UPDCS.Acres_In_Progress += (int)UPDCS.Mills_P;
                            break;
                        case "Banks":
                            UPDCS.Banks_P = AddBuildingsAdvisorTrainingMatches(temp);
                            land += (int)UPDCS.Banks_P;
                            UPDCS.Acres_In_Progress += (int)UPDCS.Banks_P;
                            break;
                        case "Training Grounds":
                            UPDCS.TG_P = AddBuildingsAdvisorTrainingMatches(temp);
                            land += (int)UPDCS.TG_P;
                            UPDCS.Acres_In_Progress += (int)UPDCS.TG_P;
                            break;
                        case "Armouries":
                            UPDCS.Armories_P = AddBuildingsAdvisorTrainingMatches(temp);
                            land += (int)UPDCS.Armories_P;
                            UPDCS.Acres_In_Progress += (int)UPDCS.Armories_P;
                            break;
                        case "Barracks":
                            UPDCS.Barracks_P = AddBuildingsAdvisorTrainingMatches(temp);
                            land += (int)UPDCS.Barracks_P;
                            UPDCS.Acres_In_Progress += (int)UPDCS.Barracks_P;
                            break;
                        case "Forts":
                            UPDCS.Forts_P = AddBuildingsAdvisorTrainingMatches(temp);
                            land += (int)UPDCS.Forts_P;
                            UPDCS.Acres_In_Progress += (int)UPDCS.Forts_P;
                            break;
                        case "Guard Stations":
                            UPDCS.GS_P = AddBuildingsAdvisorTrainingMatches(temp);
                            land += (int)UPDCS.GS_P;
                            UPDCS.Acres_In_Progress += (int)UPDCS.GS_P;
                            break;
                        case "Hospitals":
                            UPDCS.Hostpitals_P = AddBuildingsAdvisorTrainingMatches(temp);
                            land += (int)UPDCS.Hostpitals_P;
                            UPDCS.Acres_In_Progress += (int)UPDCS.Hostpitals_P;
                            break;
                        case "Guilds":
                            UPDCS.Guilds_P = AddBuildingsAdvisorTrainingMatches(temp);
                            land += (int)UPDCS.Guilds_P;
                            UPDCS.Acres_In_Progress += (int)UPDCS.Guilds_P;
                            break;
                        case "Towers":
                            UPDCS.Towers_P = AddBuildingsAdvisorTrainingMatches(temp);
                            land += (int)UPDCS.Towers_P;
                            UPDCS.Acres_In_Progress += (int)UPDCS.Towers_P;
                            break;
                        case "Thieves' Dens":
                            UPDCS.TD_P = AddBuildingsAdvisorTrainingMatches(temp);
                            land += (int)UPDCS.TD_P;
                            UPDCS.Acres_In_Progress += (int)UPDCS.TD_P;
                            break;
                        case "Watch Towers":
                            UPDCS.WT_P = AddBuildingsAdvisorTrainingMatches(temp);
                            land += (int)UPDCS.WT_P;
                            UPDCS.Acres_In_Progress += (int)UPDCS.WT_P;
                            break;
                        case "Libraries":
                            UPDCS.Library_P = AddBuildingsAdvisorTrainingMatches(temp);
                            land += (int)UPDCS.Library_P;
                            UPDCS.Acres_In_Progress += (int)UPDCS.Library_P;
                            break;
                        case "Schools":
                            UPDCS.Schools_P = AddBuildingsAdvisorTrainingMatches(temp);
                            land += (int)UPDCS.Schools_P;
                            UPDCS.Acres_In_Progress += (int)UPDCS.Schools_P;
                            break;
                        case "Stables":
                            UPDCS.Stables_P = AddBuildingsAdvisorTrainingMatches(temp);
                            land += (int)UPDCS.Stables_P;
                            UPDCS.Acres_In_Progress += (int)UPDCS.Stables_P;
                            break;
                        case "Dungeons":
                            UPDCS.Dungeons_P = AddBuildingsAdvisorTrainingMatches(temp);
                            land += (int)UPDCS.Dungeons_P;
                            UPDCS.Acres_In_Progress += (int)UPDCS.Dungeons_P;
                            break;
                        case "Barren Land":
                            break;
                        default:
                            FailedAt("FailedBuildingsAdvisorInGameSecond", temp, currentUser.PimpUser.UserID);
                            break;
                    }
                }
            }
            db.Utopia_Province_Data_Captured_Surveys.InsertOnSubmit(UPDCS);

            var Province_Info = (from UPDCG in db.Utopia_Province_Data_Captured_Gens
                                 where UPDCG.Province_ID == UPDCS.Province_ID
                                 select UPDCG).FirstOrDefault();
            Province_Info.Land = land;
            Province_Info.Updated_By_DateTime = DateTime.UtcNow;
            Province_Info.Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            Province_Info.Survey_Requested = null;
            Province_Info.Survey_Requested_Province_ID = null;
            db.SubmitChanges();

            ProvinceCache.updateProvinceSurveyToCache(land, UPDCS, Province_Info, cachedKingdom);
            return "Building Affairs for " + Province_Info.Province_Name + " (" + Province_Info.Kingdom_Island + ":" + Province_Info.Kingdom_Location + ")";
        }
        /// <summary>
        /// Adds up the numbers of buildings in training for the Buildings Advisor.
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        private static int AddBuildingsAdvisorTrainingMatches(string temp)
        {
            int jj = 0;
            foreach (Match mm in URegEx.rgxQuantitiesWithComma.Matches(temp))
                jj += Convert.ToInt32(mm.Value.Replace(",", ""));
            return jj;
        }
        private static string ParseInGameSurveyHomeBuildings(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Province_Data_Captured_Survey UPDCS = new CS_Code.Utopia_Province_Data_Captured_Survey();
            UPDCS.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
            UPDCS.Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            UPDCS.Province_ID_Updated_By = UPDCS.Province_ID;
            UPDCS.DateTime_Updated = DateTime.UtcNow;
            UPDCS.BarrenLands = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findUnbuiltLand.Match(RawData).Value).Value);

            UPDCS.Acres_In_Progress = 0;
            foreach (Match match in URegEx._findBuildingLines.Matches(RawData))
                UPDCS.Acres_In_Progress += SurveyInGameBuildingType(UPDCS, match.Value, match, currentUser.PimpUser.UserID);

            db.Utopia_Province_Data_Captured_Surveys.InsertOnSubmit(UPDCS);

            var Province_Info = (from UPDCG in db.Utopia_Province_Data_Captured_Gens
                                 where UPDCG.Province_ID == UPDCS.Province_ID
                                 select UPDCG).FirstOrDefault();
            Province_Info.Land = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findTotalLand.Match(RawData).Value).Value.Replace(",", ""));
            Province_Info.Updated_By_DateTime = DateTime.UtcNow;
            Province_Info.Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            Province_Info.Survey_Requested = null;
            Province_Info.Survey_Requested_Province_ID = null;
            db.SubmitChanges();

            ProvinceCache.updateProvinceSurveyToCache(Province_Info.Land.GetValueOrDefault(), UPDCS, Province_Info, cachedKingdom);
            return "Building Affairs for " + Province_Info.Province_Name + " (" + Province_Info.Kingdom_Island + ":" + Province_Info.Kingdom_Location + ")";
        }
        /// <summary>
        /// Gets the Building Type for the In Game Survey.
        /// </summary>
        /// <param name="_findQuantities"></param>
        /// <param name="_findBuildingTypes"></param>
        /// <param name="UPDCS"></param>
        /// <param name="temp"></param>
        /// <param name="match"></param>
        private static int SurveyInGameBuildingType(CS_Code.Utopia_Province_Data_Captured_Survey UPDCS, string temp, Match match, Guid currentUserID)
        {
            switch (URegEx._findBuildingTypes.Match(temp).Value)
            {
                case "Homes":
                    UPDCS.Homes_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                    switch (URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Success)
                    {
                        case true:
                            switch (Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", "")) > 0)
                            {
                                case true:
                                    UPDCS.Homes_P = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", ""));
                                    return (int)UPDCS.Homes_P;
                            }
                            return 0;
                    }
                    return 0;
                case "Farms":
                    UPDCS.Farms_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                    switch (URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Success)
                    {
                        case true:
                            switch (Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", "")) > 0)
                            {
                                case true:
                                    UPDCS.Farms_P = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", ""));
                                    return (int)UPDCS.Farms_P;
                            }
                            return 0;
                    }
                    return 0;
                case "Mills":
                    UPDCS.Mills_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                    switch (URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Success)
                    {
                        case true:
                            switch (Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", "")) > 0)
                            {
                                case true:
                                    UPDCS.Mills_P = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", ""));
                                    return (int)UPDCS.Mills_P;
                            }
                            return 0;
                    }
                    return 0;
                case "Banks":
                    UPDCS.Banks_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                    switch (URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Success)
                    {
                        case true:
                            switch (Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", "")) > 0)
                            {
                                case true:
                                    UPDCS.Banks_P = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", ""));
                                    return (int)UPDCS.Banks_P;
                            }
                            return 0;
                    }
                    return 0;
                case "Training Grounds":
                    UPDCS.TG_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                    switch (URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Success)
                    {
                        case true:
                            switch (Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", "")) > 0)
                            {
                                case true:
                                    UPDCS.TG_P = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", ""));
                                    return (int)UPDCS.TG_P;
                            }
                            return 0;
                    }
                    return 0;
                case "Armouries":
                    UPDCS.Armories_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                    switch (URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Success)
                    {
                        case true:
                            switch (Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", "")) > 0)
                            {
                                case true:
                                    UPDCS.Armories_P = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", ""));
                                    return (int)UPDCS.Armories_P;
                            }
                            return 0;
                    }
                    return 0;
                case "Barracks":
                    UPDCS.Barracks_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                    switch (URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Success)
                    {
                        case true:
                            switch (Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", "")) > 0)
                            {
                                case true:
                                    UPDCS.Barracks_P = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", ""));
                                    return (int)UPDCS.Barracks_P;
                            }
                            return 0;
                    }
                    return 0;
                case "Forts":
                    UPDCS.Forts_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                    switch (URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Success)
                    {
                        case true:
                            switch (Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", "")) > 0)
                            {
                                case true:
                                    UPDCS.Forts_P = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", ""));
                                    return (int)UPDCS.Forts_P;
                            }
                            return 0;
                    }
                    return 0;
                case "Guard Stations":
                    UPDCS.GS_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                    switch (URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Success)
                    {
                        case true:
                            switch (Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", "")) > 0)
                            {
                                case true:
                                    UPDCS.GS_P = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", ""));
                                    return (int)UPDCS.GS_P;
                            }
                            return 0;
                    }
                    return 0;
                case "Hospitals":
                    UPDCS.Hospitals_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                    switch (URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Success)
                    {
                        case true:
                            switch (Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", "")) > 0)
                            {
                                case true:
                                    UPDCS.Hostpitals_P = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", ""));
                                    return (int)UPDCS.Hostpitals_P;
                            }
                            return 0;
                    }
                    return 0;
                case "Guilds":
                    UPDCS.Guilds_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                    switch (URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Success)
                    {
                        case true:
                            switch (Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", "")) > 0)
                            {
                                case true:
                                    UPDCS.Guilds_P = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", ""));
                                    return (int)UPDCS.Guilds_P;
                            }
                            return 0;
                    }
                    return 0;
                case "Towers":
                    UPDCS.Towers_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                    switch (URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Success)
                    {
                        case true:
                            switch (Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", "")) > 0)
                            {
                                case true:
                                    UPDCS.Towers_P = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", ""));
                                    return (int)UPDCS.Towers_P;
                            }
                            return 0;
                    }
                    return 0;
                case "Thieves' Dens":
                    UPDCS.TD_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                    switch (URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Success)
                    {
                        case true:
                            switch (Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", "")) > 0)
                            {
                                case true:
                                    UPDCS.TD_P = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", ""));
                                    return (int)UPDCS.TD_P;
                            }
                            return 0;
                    }
                    return 0;
                case "Watch Towers":
                    UPDCS.WT_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                    switch (URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Success)
                    {
                        case true:
                            switch (Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", "")) > 0)
                            {
                                case true:
                                    UPDCS.WT_P = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", ""));
                                    return (int)UPDCS.WT_P;
                            }
                            return 0;
                    }
                    return 0;
                case "Libraries":
                    UPDCS.Library_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                    switch (URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Success)
                    {
                        case true:
                            switch (Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", "")) > 0)
                            {
                                case true:
                                    UPDCS.Library_P = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", ""));
                                    return (int)UPDCS.Library_P;
                            }
                            return 0;
                    }
                    return 0;
                case "Schools":
                    UPDCS.Schools_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                    switch (URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Success)
                    {
                        case true:
                            switch (Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", "")) > 0)
                            {
                                case true:
                                    UPDCS.Schools_P = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", ""));
                                    return (int)UPDCS.Schools_P;
                            }
                            return 0;
                    }
                    return 0;
                case "Stables":
                    UPDCS.Stables_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                    switch (URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Success)
                    {
                        case true:
                            switch (Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", "")) > 0)
                            {
                                case true:
                                    UPDCS.Stables_P = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", ""));
                                    return (int)UPDCS.Stables_P;
                            }
                            return 0;
                    }
                    return 0;
                case "Dungeons":
                    UPDCS.Dungeons_B = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                    switch (URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Success)
                    {
                        case true:
                            switch (Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", "")) > 0)
                            {
                                case true:
                                    UPDCS.Dungeons_P = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).NextMatch().Value.Replace(",", ""));
                                    return (int)UPDCS.Dungeons_P;
                            }
                            return 0;
                    }
                    return 0;
                case "Barren Land":
                    UPDCS.BarrenLands = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(temp).Value.Replace(",", ""));
                    return 0;
                default:
                    FailedAt("FailedSurveyAwayInGame", match.Value, currentUserID);
                    return 0;
            }
        }
        /// <summary>
        /// Parses In game SOS Away.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        private static string ParseInGameSOSAway(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            decimal number = 0;
            Guid provID = currentUser.PimpUser.CurrentActiveProvince;
            string provinceName = URegEx.rgxFindIslandLocation.Replace(URegEx._findSOSProvinceName.Match(RawData).Value, "").Replace("centers of ", "").Trim();
            int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);
            int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Province_Data_Captured_Science UPDCS = new CS_Code.Utopia_Province_Data_Captured_Science();
            UPDCS.DateTime_Added = DateTime.UtcNow;
            UPDCS.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
            UPDCS.Province_ID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
            UPDCS.Province_ID_Added = provID;

            foreach (Match match in URegEx._findSOSAwayPercentages.Matches(RawData))
            {
                number = Convert.ToDecimal(URegEx._findPercentages.Match(match.Value).Value.Replace("%", ""));
                if (match.Value.Contains("Income"))
                    UPDCS.SOS_Alchemy_Percent = number;
                else if (match.Value.Contains("Building Effectiveness"))
                    UPDCS.SOS_Tools_Percent = number;
                else if (match.Value.Contains("Population Limits"))
                    UPDCS.SOS_Housing_Percent = number;
                else if (match.Value.Contains("Food Production"))
                    UPDCS.SOS_Food_Percent = number;
                else if (match.Value.Contains("Gains in Combat"))
                    UPDCS.SOS_Miltary_Percent = number;
                else if (match.Value.Contains("Thievery Effectiveness"))
                    UPDCS.SOS_Thieves_Percent = number;
                else if (match.Value.Contains("Magic Effectiveness & Rune Production") || match.Value.Contains("Magic Effectiveness &amp; Rune Production"))
                    UPDCS.SOS_Magic_Percent = number;
                else if (match.Value.Contains("Kingdom"))
                    number += 1;
                else
                    FailedAt("RawSOSAway", match.Value + ";  " + RawData, currentUser.PimpUser.UserID);
            }
            var getProv = (from xx in db.Utopia_Province_Data_Captured_Gens
                           where xx.Province_ID == UPDCS.Province_ID
                           select xx).FirstOrDefault();
            getProv.SOS_Requested = null;
            getProv.Updated_By_DateTime = DateTime.UtcNow;
            getProv.Updated_By_Province_ID = provID;
            db.Utopia_Province_Data_Captured_Sciences.InsertOnSubmit(UPDCS);
            db.SubmitChanges();
            ProvinceCache.updateProvinceSOSToCache(UPDCS, getProv, cachedKingdom);
            return "SOS Submitted " + provinceName + " (" + sourceIsland + ":" + sourceLocation + ")";
        }
        private static string InGameScienceAdvisorAway(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            Guid provID = currentUser.PimpUser.CurrentActiveProvince;
            string provinceName = URegEx.rgxFindIslandLocation.Replace(URegEx._findScienceInGameAwayProvinceName.Match(RawData).Value, "").Replace("research centers of", "").Trim();
            int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);
            int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Province_Data_Captured_Science UPDCS = new CS_Code.Utopia_Province_Data_Captured_Science();
            UPDCS.DateTime_Added = DateTime.UtcNow;
            UPDCS.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
            UPDCS.Province_ID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
            UPDCS.Province_ID_Added = provID;
            MatchCollection mc = URegEx._findScienceAdvisorBookLines.Matches(RawData);

            for (int i = 0; i < mc.Count; i++)
            {
                string line = mc[i].Value;
                switch (URegEx._findSOSTypes.Match(line).Value)
                {
                    case "Alchemy":
                        UPDCS.SOS_Alchemy = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Alchemy_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[2].Value);
                        break;
                    case "Food":
                        UPDCS.SOS_Food = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Food_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[2].Value);
                        break;
                    case "Housing":
                        UPDCS.SOS_Housing = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Housing_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[2].Value);
                        break;
                    case "Channeling":
                        UPDCS.SOS_Magic = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Magic_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[2].Value);
                        break;
                    case "Military":
                        UPDCS.SOS_Military = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Miltary_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[2].Value);
                        break;
                    case "Crime":
                        UPDCS.SOS_Thieves = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Thieves_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[2].Value);
                        break;
                    case "Tools":
                        UPDCS.SOS_Tools = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Tools_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[2].Value);
                        break;
                    default:
                        FailedAt("SOSInGameHome", line, currentUser.PimpUser.UserID);
                        break;
                }
            }
            var getProv = (from xx in db.Utopia_Province_Data_Captured_Gens
                           where xx.Province_ID == UPDCS.Province_ID
                           select xx).FirstOrDefault();
            getProv.SOS_Requested = null;
            db.Utopia_Province_Data_Captured_Sciences.InsertOnSubmit(UPDCS);
            db.SubmitChanges();
            KingdomCache.removeProvinceFromKingdomCache(currentUser.PimpUser.StartingKingdom, getProv.Province_ID, cachedKingdom);
            return "SOS Advisor Submitted " + getProv.Province_Name + " (" + getProv.Kingdom_Island + ":" + getProv.Kingdom_Location + ")";
        }
        private static string InGameScienceAdvisorHome(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Province_Data_Captured_Science UPDCS = new CS_Code.Utopia_Province_Data_Captured_Science();
            UPDCS.DateTime_Added = DateTime.UtcNow;
            UPDCS.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
            UPDCS.Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            UPDCS.Province_ID_Added = currentUser.PimpUser.CurrentActiveProvince;
            MatchCollection mc = URegEx._findScienceAdvisorBookLines.Matches(RawData);

            for (int i = 0; i < mc.Count; i++)
            {
                string line = mc[i].Value;
                switch (URegEx._findSOSTypes.Match(line).Value)
                {
                    case "Alchemy":
                        UPDCS.SOS_Alchemy = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Alchemy_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[2].Value);
                        break;
                    case "Food":
                        UPDCS.SOS_Food = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Food_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[2].Value);
                        break;
                    case "Housing":
                        UPDCS.SOS_Housing = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Housing_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[2].Value);
                        break;
                    case "Channeling":
                        UPDCS.SOS_Magic = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Magic_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[2].Value);
                        break;
                    case "Military":
                        UPDCS.SOS_Military = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Miltary_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[2].Value);
                        break;
                    case "Crime":
                        UPDCS.SOS_Thieves = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Thieves_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[2].Value);
                        break;
                    case "Tools":
                        UPDCS.SOS_Tools = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Tools_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[2].Value);
                        break;
                    default:
                        FailedAt("SOSInGameHome", line, currentUser.PimpUser.UserID);
                        break;
                }
            }
            try
            {
                RawData = RawData.Remove(0, RawData.IndexOf("Schedule"));
            }
            catch { }
            mc = URegEx._findScienceAdvisorBookTrainingLines.Matches(RawData);

            for (int i = 0; i < mc.Count; i++)
            {
                string line = mc[i].Value;
                switch (URegEx._findSOSTypes.Match(line).Value)
                {
                    case "Alchemy":
                        UPDCS.SOS_Alchemy_Prog = AddBuildingsAdvisorTrainingMatches(line);
                        break;
                    case "Food":
                        UPDCS.SOS_Food_Prog = AddBuildingsAdvisorTrainingMatches(line);
                        break;
                    case "Housing":
                        UPDCS.SOS_Housing_Prog = AddBuildingsAdvisorTrainingMatches(line);
                        break;
                    case "Channeling":
                        UPDCS.SOS_Magic_Prog = AddBuildingsAdvisorTrainingMatches(line);
                        break;
                    case "Military":
                        UPDCS.SOS_Military_Prog = AddBuildingsAdvisorTrainingMatches(line);
                        break;
                    case "Crime":
                        UPDCS.SOS_Thieves_Prog = AddBuildingsAdvisorTrainingMatches(line);
                        break;
                    case "Tools":
                        UPDCS.SOS_Tools_Prog = AddBuildingsAdvisorTrainingMatches(line);
                        break;
                    default:
                        FailedAt("SOSInGameHome", line, currentUser.PimpUser.UserID);
                        break;
                }
            }
            var getProv = (from xx in db.Utopia_Province_Data_Captured_Gens
                           where xx.Province_ID == UPDCS.Province_ID
                           select xx).FirstOrDefault();
            getProv.SOS_Requested = null;
            db.Utopia_Province_Data_Captured_Sciences.InsertOnSubmit(UPDCS);
            db.SubmitChanges();
            ProvinceCache.updateProvinceSOSToCache(UPDCS, getProv, cachedKingdom);
            return "SOS Advisor Submitted " + getProv.Province_Name + " (" + getProv.Kingdom_Island + ":" + getProv.Kingdom_Location + ")";
        }
        /// <summary>
        /// Parses the In Game SOS.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        private static string ParseInGameSOSHome(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Province_Data_Captured_Science UPDCS = new CS_Code.Utopia_Province_Data_Captured_Science();
            UPDCS.DateTime_Added = DateTime.UtcNow;
            UPDCS.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
            UPDCS.Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            UPDCS.Province_ID_Added = currentUser.PimpUser.CurrentActiveProvince;

            //moved the update to the top of the method because of the rawdata.Remove line.  I was removing the Money and Daily Income
            // part of the raw data before it could be found.
            var getProv = (from xx in db.Utopia_Province_Data_Captured_Gens
                           where xx.Province_ID == UPDCS.Province_ID
                           select xx).FirstOrDefault();
            if (URegEx._findMilPagetotMoney.IsMatch(RawData))
                getProv.Money = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findMilPagetotMoney.Match(RawData).Value).Value.Replace(",", ""));
            //getProv.Daily_Income = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAffairsdi.Match(RawData).Value).Value.Replace(",", ""));
            getProv.SOS_Requested = null;
            getProv.Updated_By_DateTime = DateTime.UtcNow;

            RawData = RawData.Remove(0, URegEx._findSOSTypes.Match(RawData).Index);

            MatchCollection mc = URegEx._rgxFindLinesSOS.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string line = mc[i].Value;
                switch (URegEx._findSOSTypes.Match(line).Value)
                {
                    case "Alchemy":
                        UPDCS.SOS_Alchemy = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Alchemy_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[1].Value);
                        UPDCS.SOS_Alchemy_Prog = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[2].Value.Replace(",", ""));
                        break;
                    case "Food":
                        UPDCS.SOS_Food = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Food_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[1].Value);
                        UPDCS.SOS_Food_Prog = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[2].Value.Replace(",", ""));
                        break;
                    case "Housing":
                        UPDCS.SOS_Housing = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Housing_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[1].Value);
                        UPDCS.SOS_Housing_Prog = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[2].Value.Replace(",", ""));
                        break;
                    case "Channeling":
                        UPDCS.SOS_Magic = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Magic_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[1].Value);
                        UPDCS.SOS_Magic_Prog = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[2].Value.Replace(",", ""));
                        break;
                    case "Military":
                        UPDCS.SOS_Military = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Miltary_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[1].Value);
                        UPDCS.SOS_Military_Prog = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[2].Value.Replace(",", ""));
                        break;
                    case "Crime":
                        UPDCS.SOS_Thieves = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Thieves_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[1].Value);
                        UPDCS.SOS_Thieves_Prog = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[2].Value.Replace(",", ""));
                        break;
                    case "Tools":
                        UPDCS.SOS_Tools = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[0].Value.Replace(",", ""));
                        UPDCS.SOS_Tools_Percent = Convert.ToDecimal(URegEx._findSciencefindQuants.Matches(line)[1].Value);
                        UPDCS.SOS_Tools_Prog = Convert.ToInt32(URegEx._findSciencefindQuants.Matches(line)[2].Value.Replace(",", ""));
                        break;
                    default:
                        FailedAt("SOSInGameHome", line, currentUser.PimpUser.UserID);
                        break;
                }
            }

            getProv.Updated_By_Province_ID = UPDCS.Province_ID;
            db.Utopia_Province_Data_Captured_Sciences.InsertOnSubmit(UPDCS);
            db.SubmitChanges();
            ProvinceCache.updateProvinceSOSToCache(UPDCS, getProv, cachedKingdom);
            return "SOS Submitted " + getProv.Province_Name + " (" + getProv.Kingdom_Island + ":" + getProv.Kingdom_Location + ")";
        }
        /// <summary>
        /// The military page.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        private static string InGameMilitaryArmyTrainingPage(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            DateTime datetime = DateTime.UtcNow;
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var getProv = (from xx in db.Utopia_Province_Data_Captured_Gens
                           where xx.Province_ID == currentUser.PimpUser.CurrentActiveProvince
                           select xx).FirstOrDefault();


            getProv.Population = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findMilPagetotPop.Match(RawData).Value).Value.Replace(",", ""));
            getProv.Peasents = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findMilPagetotPeas.Match(RawData).Value).Value.Replace(",", ""));
            getProv.Wizards = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findMilPagetotWizs.Match(RawData).Value).Value.Replace(",", ""));
            getProv.Soldiers = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findMilPagetotSolds.Match(RawData).Value).Value.Replace(",", ""));
            getProv.Peasents_Non_Percentage = Convert.ToDecimal(URegEx._findPercentages.Match(URegEx._findMilPagetotMil.Match(RawData).Value).Value.Replace("%", ""));
            MatchCollection mc = URegEx._findMilPageallLines.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        getProv.Soldiers_Regs_Off = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findOffDef.Replace(mc[i].Value, "")).Value.Replace(",", ""));
                        break;
                    case 1:
                        getProv.Soldiers_Regs_Def = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findOffDef.Replace(mc[i].Value, "")).Value.Replace(",", ""));
                        break;
                    case 2:
                        getProv.Soldiers_Elites = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findOffDef.Replace(mc[i].Value, "")).Value.Replace(",", ""));
                        break;
                    case 3:
                        getProv.Thieves = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findOffDef.Replace(mc[i].Value, "")).Value.Replace(",", ""));
                        break;
                }
            }
            string stance = GetStanceName(cachedKingdom.Kingdoms.Where(x => x.Kingdom_ID == currentUser.PimpUser.StartingKingdom).FirstOrDefault().Stance.GetValueOrDefault());
            var prov = cachedKingdom.Provinces.Where(x => x.Province_ID == getProv.Province_ID).FirstOrDefault();
            var ro = CalcRawOffense(getProv.Race_ID.GetValueOrDefault(), getProv.Soldiers.GetValueOrDefault(), false, getProv.Soldiers_Regs_Off.GetValueOrDefault(), getProv.Soldiers_Elites.GetValueOrDefault(), getProv.War_Horses.GetValueOrDefault(), 0, CalcOptimizedPrisoners(getProv.Prisoners.GetValueOrDefault(), getProv.Soldiers.GetValueOrDefault(), getProv.Soldiers_Regs_Off.GetValueOrDefault(), getProv.Soldiers_Elites.GetValueOrDefault(), 0));
            double tgb = 0;
            int forts = 0;

            if (prov != null && prov.Survey.FirstOrDefault() != null)
            {
                tgb = prov.Survey.FirstOrDefault().TG_B.GetValueOrDefault();
                forts = prov.Survey.FirstOrDefault().Forts_B.GetValueOrDefault();
            }
            tgb = CalcTrainingGroundBonus((int)tgb, getProv.Building_Effectiveness.GetValueOrDefault());
            var rb = CalcRankBonus(getProv.Nobility_ID.GetValueOrDefault(), UtopiaHelper.Instance.Races.Where(x => x.uid == getProv.Race_ID).FirstOrDefault().name);
            bool f = false;
            if (cachedKingdom.Effects.Where(x => x.OP_Name.ToLower() == "fanatacism").Where(x => x.Directed_To_Province_ID == getProv.Province_ID).FirstOrDefault() != null)
                f = true;
            var mro = CalcOffensiveMilEffciency(Convert.ToDouble(getProv.Mil_Overall_Efficiency.GetValueOrDefault()), getProv.Mil_Total_Generals.GetValueOrDefault(), tgb, rb, false, f);

            getProv.Military_Current_Off = Convert.ToDecimal(CalcModifiedOffense(ro, getProv.Mil_Total_Generals.GetValueOrDefault(), stance, mro));
            bool p = false;
            if (cachedKingdom.Effects.Where(x => x.OP_Name.ToLower() == "plague").Where(x => x.Directed_To_Province_ID == getProv.Province_ID).FirstOrDefault() != null)
                p = true;

            var rd = CalcRawDefense(getProv.Race_ID.GetValueOrDefault(), getProv.Soldiers.GetValueOrDefault(), getProv.Soldiers_Regs_Def.GetValueOrDefault(), getProv.Soldiers_Elites.GetValueOrDefault(), getProv.Peasents.GetValueOrDefault());
            var dme = CalcDefensiveMilEfficiency(stance, (double)getProv.Mil_Overall_Efficiency.GetValueOrDefault(), (double)getProv.Building_Effectiveness.GetValueOrDefault(), getProv.Monarch_Display > 0 ? true : false, false, f, p, false, forts);

            getProv.Military_Current_Off = Convert.ToDecimal(CalcModDefense(rd, dme));

            getProv.Updated_By_DateTime = DateTime.UtcNow;
            getProv.Updated_By_Province_ID = currentUser.PimpUser.CurrentActiveProvince;
            db.SubmitChanges();
            KingdomCache.removeProvinceFromKingdomCache(currentUser.PimpUser.StartingKingdom, getProv.Province_ID, cachedKingdom);
            return "Mil. Page Submitted " + getProv.Province_Name + " (" + getProv.Kingdom_Island + ":" + getProv.Kingdom_Location + ")";
        }
        /// <summary>
        /// Parses in game SOM.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        private static string ParseInGameSOM(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            //Guid provID = CachedItems.GetUserCurrentActiveProvince();
            DateTime datetime = DateTime.UtcNow;
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            if (RawData.Contains("Our thieves listen in on a report from the Military Elders"))
            {
                string provinceName = URegEx.rgxFindIslandLocation.Replace(URegEx._findSOMProvinceName.Match(RawData).Value, "").Replace("from the Military Elders of", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(RawData).Value).Value).Value);
                Guid provID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
                var ProvinceInfos = (from UPDCG in db.Utopia_Province_Data_Captured_Gens
                                     where UPDCG.Province_ID == provID
                                     select UPDCG).FirstOrDefault();

                SOMUpdateRaw(RawData, db, ProvinceInfos, datetime, currentUser, cachedKingdom);
                return "SOM Submitted " + provinceName + " (" + sourceIsland + ":" + sourceLocation + ")";
            }
            else
            {
                var ProvinceInfo = (from UPDCG in db.Utopia_Province_Data_Captured_Gens
                                    where UPDCG.Province_ID == currentUser.PimpUser.CurrentActiveProvince
                                    select UPDCG).FirstOrDefault();

                SOMUpdateRaw(RawData, db, ProvinceInfo, datetime, currentUser, cachedKingdom);
                return "SOM Submitted " + ProvinceInfo.Province_Name + " (" + ProvinceInfo.Kingdom_Island + ":" + ProvinceInfo.Kingdom_Location + ")";
            }
        }

        /// <summary>
        /// Parses the Kingdom Page for inGame play.
        /// </summary>
        /// <param name="RawData">Raw Data of page.</param>
        /// <param name="ClickedFrom">Where the page was entered from</param>
        /// <param name="ProvinceName">Province name of persons kingdom</param>
        /// <param name="ServerID">Server ID of kingdom.</param>
        /// <returns></returns>
        private static string ParseKingdomPageInGame(string RawData, string ClickedFrom, string ProvinceName, int ServerID, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            var k = ParseInGameKingdomPage(RawData, currentUser.PimpUser.UserID);
            if (k == null)
                return ReturnErrorsToUser(ErrorTypeEnum.FindKingdomName);

            switch (ClickedFrom)
            {
                case "StartKingdom":
                    StartKingdom(k, ServerID, ProvinceName, currentUser, cachedKingdom);
                    return k.Kingdom_Name + " (" + k.Kingdom_Island + ":" + k.Kingdom_Location + ")";
                default:
                    UpdateKingdom(ServerID, k, currentUser, cachedKingdom);
                    return "Updated Kingdom " + k.Kingdom_Name + " (" + k.Kingdom_Island + ":" + k.Kingdom_Location + ")";
            }
        }
        /// <summary>
        /// parses an in game kingdom page.
        /// </summary>
        /// <param name="rawData"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        public static KingdomClass ParseInGameKingdomPage(string rawData, Guid currentUserID)
        {
            KingdomClass kingdom = new KingdomClass();
            List<ProvinceClass> provs = new List<ProvinceClass>();


            if (URegEx.findKingdomName.IsMatch(rawData))
            {
                kingdom.Kingdom_Name = URegEx.findKingdomName.Match(rawData).Value.Trim();
                kingdom.Kingdom_Name = kingdom.Kingdom_Name.Replace(URegEx.rgxFindIslandLocation.Match(kingdom.Kingdom_Name).Value, "").Trim();
                rawData = rawData.Replace(kingdom.Kingdom_Name, "");
                kingdom.Kingdom_Name = kingdom.Kingdom_Name.Replace("The Famous kingdom of", "").Replace("The Esteemed kingdom of", "").Replace("The kingdom of", "").Replace("Current kingdom is", "").Trim();
            }
            else
                return null;

            //cant' find kingdom island or location... We need that specifially to locate where the kingdom is from.
            if (!URegEx.rgxFindIslandLocation.IsMatch(rawData))
                return null;
            kingdom.Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(rawData).Value).Value).Value);
            kingdom.Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(rawData).Value).Value).Value);
            kingdom.ProvinceCount = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx._findTotalProvinces.Match(rawData).Value).Value);
            kingdom.Stance = getStanceID(URegEx._findKingdomStanceName.Match(rawData).Value.Replace("Stance", "").Trim());
            kingdom.War_Wins = Convert.ToInt32(URegEx.rgxNumber.Matches(URegEx._findKingdomWarWinsLosses.Match(rawData).Value)[0].Value);
            kingdom.War_Losses = Convert.ToInt32(URegEx.rgxNumber.Matches(URegEx._findKingdomWarWinsLosses.Match(rawData).Value)[1].Value);
            kingdom.Networth = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findTotalKingdomNetworth.Match(rawData).Value).Value.Replace(",", ""));
            kingdom.Acres = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findTotalKingdomLand.Match(rawData).Value).Value.Replace(",", ""));

            rawData = rawData.Replace("Acre Honour Gains", "");

            MatchCollection mc = URegEx._findKingdomProvinceLines.Matches(rawData);

            for (int i = 0; i < mc.Count; i++)
            {

                string temp = mc[i].Value;
                ProvinceClass prov = new ProvinceClass();
                prov.Province_Name = URegEx._findLineNumber.Replace(temp.Replace(URegEx._findEverythingButName.Match(mc[i].Value).Value, "").Replace("(M)", "").Replace("(S)", "").Replace("^", "").Replace("*", "").Replace("+", ""), "").Trim();

                temp = temp.Replace(prov.Province_Name, "");

                //if its the first line in the kingdom, it has a chance of pulling in the Nobility string from the kingdom header. like so:
                //Nobility Skeletons cant whistle^ Undead 400 acres 21,050gc 52gc Peasant
                if (i == 0)
                    prov.Province_Name = prov.Province_Name.Replace("Nobility ", "");

                prov.Race_ID = getRaceID(URegEx._findRace.Match(temp).Value, currentUserID);
                prov.Nobility_ID = getNobilityID(URegEx._findNobility.Match(temp).Value, currentUserID);
                prov.Land = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAcres.Match(temp).Value).Value.Replace(",", ""));
                prov.Networth = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findGoldCoins.Match(temp).Value.Replace(",", "")).Value);
                if (mc[i].Value.Contains("^"))
                    prov.Protected = 1;
                else
                    prov.Protected = 0;
                if (mc[i].Value.Contains("(M)"))
                    prov.Monarch_Display = 4;
                if (mc[i].Value.Contains("*"))
                {
                    prov.OnlineCurrently = 1;
                    prov.Last_Login_For_Province = DateTime.UtcNow;
                }
                else
                    prov.OnlineCurrently = 0;
                provs.Add(prov);
            }
            kingdom.Provinces = provs;
            return kingdom;
        }

        /// <summary>
        /// Parses any attacks that get posted to pimp.
        /// </summary>
        /// <param name="RawData">Good data</param>
        /// <returns>fake guid</returns>
        private static string ParseAttack(string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            Guid provID = currentUser.PimpUser.CurrentActiveProvince;
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var getMilInfo = UtopiaHelper.Instance.Races.Where(x => x.uid == GetRaceID(currentUser, cachedKingdom)).FirstOrDefault();
            string ProvinceAttacked = string.Empty;
            var getProvinceInfo = (from updcg in db.Utopia_Province_Data_Captured_Gens
                                   where updcg.Province_ID == provID
                                   select updcg).FirstOrDefault();

            CS_Code.Utopia_Province_Data_Captured_Attack updca = new CS_Code.Utopia_Province_Data_Captured_Attack();

            if (RawData.Contains("books of knowledge"))
            {
                switch (URegEx._findAttackFindQuantityBooks.Match(RawData).Success)
                {
                    case true:
                        updca.Captured_Type_Number = Convert.ToInt32(URegEx._findAttackFindQuantityBooks.Match(RawData).Value.Replace(" books of knowledge", "").Replace(",", "").Trim());
                        break;
                    default:
                        FailedAt("ParseAttackKnowledge", RawData, currentUser.PimpUser.UserID);
                        break;
                }
                updca.Attack_Type = getAttackType("Knowledge", currentUser);
            }
            else if (RawData.Contains("Your army has taken") || RawData.Contains("Taking full control of our new land") || RawData.Contains("Taking full control of your new land") || RawData.Contains("Our army broke our enemy's initial defenses") || RawData.Contains("Your army has recaptured"))
            {
                Regex rgxFindQuantityAcres = new Regex(URegEx._findQuantitiesString + " (new acres|acres|acre!)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                switch (rgxFindQuantityAcres.Match(RawData).Success)
                {
                    case true:
                        updca.Captured_Type_Number = Convert.ToInt32(rgxFindQuantityAcres.Match(RawData).Value.Replace(" new acres", "").Replace("acres", "").Replace("acre!", "").Replace(",", "").Trim());
                        getProvinceInfo.Land += updca.Captured_Type_Number;
                        break;
                    default:
                        FailedAt("'ParseAttackAcres'", RawData, currentUser.PimpUser.UserID);
                        break;
                }
                switch (URegEx._findAttackFindQuantitesDecimal.Match(RawData).Success)
                {
                    case true:
                        updca.Time_To_Return = DateTime.UtcNow.AddHours(Convert.ToDouble(URegEx._findAttackFindQuantitesDecimal.Match(RawData).Value.Replace(",", "").Replace(" days", "")));
                        getProvinceInfo.Army_Out = 1;
                        getProvinceInfo.Army_Out_Expires = updca.Time_To_Return;
                        break;
                    default:
                        switch (URegEx.rgxFindUtopianDateTime.Match(RawData).Success)
                        {
                            case true:
                                updca.Time_To_Return = UtopiaParser.RealTime(URegEx.rgxFindUtopianDateTime.Match(RawData).Value);
                                getProvinceInfo.Army_Out = 1;
                                getProvinceInfo.Army_Out_Expires = updca.Time_To_Return;
                                break;
                            default:
                                FailedAt("ParseAttackCheckAcres", RawData, currentUser.PimpUser.UserID);
                                break;
                        }
                        break;
                }
                updca.Attack_Type = getAttackType("Acres", currentUser);
            }
            else if (RawData.Contains("was much too weak to break their") | RawData.Contains("army appears to have failed") | RawData.Contains("Your army was no match for the defenses") | RawData.Contains("battlefield and are quickly driven back"))
            {
                //Alas, Knight Tiger it appears our army was much too weak to break their defenses! We lost 37 Drakes in this battle. We killed about 9 enemy troops. Our forces will be available again in 7.35 days (on May 10 of YR3).
                switch (URegEx._findAttackFindQuantitesDecimal.Match(RawData).Success)
                {
                    case true:
                        updca.Time_To_Return = DateTime.UtcNow.AddHours(Convert.ToDouble(URegEx._findAttackFindQuantitesDecimal.Match(RawData).Value.Replace(",", "").Replace(" days", "")));
                        getProvinceInfo.Army_Out = 1;
                        getProvinceInfo.Army_Out_Expires = updca.Time_To_Return;
                        break;
                    default:
                        switch (URegEx.rgxFindUtopianDateTime.Match(RawData).Success)
                        {
                            case true:
                                updca.Time_To_Return = UtopiaParser.RealTime(URegEx.rgxFindUtopianDateTime.Match(RawData).Value);
                                getProvinceInfo.Army_Out = 1;
                                getProvinceInfo.Army_Out_Expires = updca.Time_To_Return;
                                break;
                            default:
                                FailedAt("'NoTimeTooWeak'", RawData, currentUser.PimpUser.UserID);
                                break;
                        }
                        break;
                }
                updca.Attack_Type = getAttackType("weakForces", currentUser);
            }
            else if (RawData.Contains("Your army looted"))
            {
                switch (URegEx._findAttackFindQuantitesDecimal.Match(RawData).Success)
                {
                    case true:
                        updca.Time_To_Return = DateTime.UtcNow.AddHours(Convert.ToDouble(URegEx._findAttackFindQuantitesDecimal.Match(RawData).Value.Replace(",", "").Replace(" days", "")));
                        getProvinceInfo.Army_Out = 1;
                        getProvinceInfo.Army_Out_Expires = updca.Time_To_Return;
                        break;
                    default:
                        switch (URegEx.rgxFindUtopianDateTime.Match(RawData).Success)
                        {
                            case true:
                                updca.Time_To_Return = UtopiaParser.RealTime(URegEx.rgxFindUtopianDateTime.Match(RawData).Value);
                                getProvinceInfo.Army_Out = 1;
                                getProvinceInfo.Army_Out_Expires = updca.Time_To_Return;
                                break;
                            default:
                                FailedAt("NoTimeLootedArmy", RawData, currentUser.PimpUser.UserID);
                                break;
                        }
                        break;
                }
                updca.Attack_Type = getAttackType("Looted", currentUser);
            }
            else if (RawData.Contains("Your army massacred"))
            {
                switch (URegEx._findAttackFindQuantitesDecimal.Match(RawData).Success)
                {
                    case true:
                        updca.Time_To_Return = DateTime.UtcNow.AddHours(Convert.ToDouble(URegEx._findAttackFindQuantitesDecimal.Match(RawData).Value.Replace(",", "").Replace(" days", "")));
                        getProvinceInfo.Army_Out = 1;
                        getProvinceInfo.Army_Out_Expires = updca.Time_To_Return;
                        break;
                    default:
                        switch (URegEx.rgxFindUtopianDateTime.Match(RawData).Success)
                        {
                            case true:
                                updca.Time_To_Return = UtopiaParser.RealTime(URegEx.rgxFindUtopianDateTime.Match(RawData).Value);
                                getProvinceInfo.Army_Out = 1;
                                getProvinceInfo.Army_Out_Expires = updca.Time_To_Return;
                                break;
                            default:
                                FailedAt("NoTimeMassacred", RawData, currentUser.PimpUser.UserID);
                                break;
                        }
                        break;
                }
                updca.Attack_Type = getAttackType("massacred", currentUser);
            }
            else if (RawData.Contains("Your army burned and razed") | RawData.Contains("Your army burned and destroyed"))
            {
                switch (URegEx._findAttackFindQuantitesDecimal.Match(RawData).Success)
                {
                    case true:
                        updca.Time_To_Return = DateTime.UtcNow.AddHours(Convert.ToDouble(URegEx._findAttackFindQuantitesDecimal.Match(RawData).Value.Replace(",", "").Replace(" days", "")));
                        getProvinceInfo.Army_Out = 1;
                        getProvinceInfo.Army_Out_Expires = updca.Time_To_Return;
                        break;
                    default:
                        switch (URegEx.rgxFindUtopianDateTime.Match(RawData).Success)
                        {
                            case true:
                                updca.Time_To_Return = UtopiaParser.RealTime(URegEx.rgxFindUtopianDateTime.Match(RawData).Value);
                                getProvinceInfo.Army_Out = 1;
                                getProvinceInfo.Army_Out_Expires = updca.Time_To_Return;
                                break;
                            default:
                                FailedAt("NoTimeburned", RawData, currentUser.PimpUser.UserID);
                                break;
                        }
                        break;
                }

                Regex rgxFindQuantityAcres = new Regex(URegEx._findQuantitiesString + " (acres)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                updca.Captured_Type_Number = Convert.ToInt32(rgxFindQuantityAcres.Match(RawData).Value.Replace("acres", "").Replace(",", "").Trim());
                updca.Attack_Type = getAttackType("razed", currentUser);
            }
            else
                FailedAt("'ParseAttackFailed'", RawData, currentUser.PimpUser.UserID);

            switch (URegEx._findAttackImprisonedTroops.Match(RawData).Success)
            {
                case true:
                    updca.Imprsoned_Troops = Convert.ToInt32(URegEx._findAttackImprisonedTroops.Match(RawData).Value.Replace("imprisoned ", "").Replace(",", "").Trim());
                    getProvinceInfo.Prisoners = getProvinceInfo.Prisoners.GetValueOrDefault(0) + updca.Imprsoned_Troops;
                    break;
            }
            switch (URegEx._findAttackEnemyKills.Match(RawData).Success)
            {
                case true:
                    updca.Troops_Killed = Convert.ToInt32(URegEx._findAttackEnemyKills.Match(RawData).Value.Replace(" enemy troops", "").Replace(",", "").Trim());
                    break;
            }

            //Checks if military was killed.
            if (RawData.Contains("this battle"))
            {
                if (getMilInfo != null)
                {
                    string SoildersData = RawData;
                    SoildersData = SoildersData.Remove(SoildersData.IndexOf("this battle"));
                    SoildersData = SoildersData.Remove(0, SoildersData.IndexOf("We lost"));

                    int getTotalOffense = 0, getTotalDef = 0;

                    switch (URegEx._findAttackDefense.Match(SoildersData).Success)
                    {
                        case true:
                            updca.O_Regs_Def_Died = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAttackDefense.Match(SoildersData).Value).Value.Replace(",", ""));
                            getProvinceInfo.Soldiers_Regs_Def -= updca.O_Regs_Def_Died;
                            getTotalDef += (int)updca.O_Regs_Def_Died * getMilInfo.soldierDefMultiplier;
                            break;
                    }
                    switch (URegEx._findAttackOffense.Match(SoildersData).Success)
                    {
                        case true:
                            updca.O_Regs_Off_Died = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAttackOffense.Match(SoildersData).Value).Value.Replace(",", ""));
                            getProvinceInfo.Soldiers_Regs_Off -= updca.O_Regs_Off_Died;
                            getTotalOffense += (int)updca.O_Regs_Off_Died * getMilInfo.soldierOffMultiplier;
                            break;
                    }
                    switch (URegEx._findAttackElites.Match(SoildersData).Success)
                    {
                        case true:
                            updca.O_Elites_Died = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAttackElites.Match(SoildersData).Value).Value.Replace(",", ""));
                            getProvinceInfo.Soldiers_Elites -= updca.O_Elites_Died;
                            getTotalDef += (int)updca.O_Elites_Died * getMilInfo.eliteDefMulitplier;
                            getTotalOffense += (int)updca.O_Regs_Off_Died.GetValueOrDefault(0) * getMilInfo.eliteOffMulitplier;
                            break;
                    }
                    switch (URegEx._findAttackWarHorses.Match(SoildersData).Success)
                    {
                        case true:
                            updca.O_Horses_Died = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAttackWarHorses.Match(SoildersData).Value).Value.Replace(",", ""));
                            getProvinceInfo.War_Horses -= (int)updca.O_Horses_Died;
                            getTotalOffense += (int)updca.O_Horses_Died;
                            break;
                    }
                    switch (URegEx._findAttackSoldiers.Match(SoildersData).Success)
                    {
                        case true:
                            updca.O_Soldiers_Died = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findAttackSoldiers.Match(SoildersData).Value).Value.Replace(",", ""));
                            getProvinceInfo.Soldiers -= (int)updca.O_Soldiers_Died;
                            getTotalOffense += (int)updca.O_Soldiers_Died;
                            getTotalDef += (int)updca.O_Soldiers_Died;
                            break;
                    }
                    getProvinceInfo.Military_Net_Def -= getTotalDef;
                    getProvinceInfo.Military_Efficiency_Off -= getTotalOffense;
                }
            }

            getProvinceInfo.Updated_By_DateTime = DateTime.UtcNow;
            getProvinceInfo.Updated_By_Province_ID = provID;

            if (!RawData.Contains("army was much too weak to break their defenses") & !RawData.Contains("Our army appears to have failed") & !RawData.Contains("march onto the battlefield and are quickly driven back"))
            {
                ProvinceAttacked = URegEx._findAttackProvinceName.Match(RawData).Value.Replace("forces arrive at", "").Replace("Your army was no match for the defenses of", "");
                string provinceName = URegEx.rgxFindIslandLocation.Replace(ProvinceAttacked, "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(ProvinceAttacked).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(ProvinceAttacked).Value).Value).Value);

                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
                updca.Province_ID_Attacked = ProvinceID;

                if (RawData.Contains("Our troops have spread the plague"))
                    InsertOp(OpType.plague, ProvinceID, currentUser, cachedKingdom);
                if (RawData.Contains("Our forces got delayed by dense fog"))
                    InsertOp(OpType.fog, ProvinceID, currentUser, cachedKingdom);
                ProvinceAttacked += "; ";
            }
            else
            {
                updca.Province_ID_Attacked = new Guid();
                ProvinceAttacked = "To Weak to Tell; ";
            }

            updca.Province_ID_Added = provID;
            updca.DateTime_Added = DateTime.UtcNow;
            if (URegEx._modOffSent.IsMatch(RawData))
                updca.Mod_Off_Sent = Convert.ToDecimal(URegEx._findDecimalNumbers.Match(URegEx._modOffSent.Match(RawData).Value).Value);
            else
                ProvinceAttacked += " You didn't document how much offense you sent. Check the Legend to find out how.";

            updca.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;

            db.Utopia_Province_Data_Captured_Attacks.InsertOnSubmit(updca);
            db.SubmitChanges();
            OpCache.UpdateAttackToCache(updca, cachedKingdom);
            KingdomCache.removeProvinceFromKingdomCache(currentUser.PimpUser.StartingKingdom, provID, cachedKingdom);
            return ProvinceAttacked;
        }

    }
}
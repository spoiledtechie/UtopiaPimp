using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pimp.UData;
using Pimp.Utopia;
using System.Text.RegularExpressions;
using Pimp.UCache;

using Boomers.Utilities.Guids;
using PimpLibrary.Utopia.Ops;
using SupportFramework.Data;
using PimpLibrary.Static.Enums;

namespace Pimp.UParser
{
    /// <summary>
    /// Summary description for InGameOps
    /// </summary>
    public class InGameOps
    {
        /// <summary>
        /// Gets the Thieves lost in the operation...
        /// </summary>fv
        /// <param name="matchValue"></param>
        /// <param name="thievesLost"></param>
        /// <returns></returns>
        private static string GetLostThieves(string matchValue, PimpUserWrapper currentUser)
        {
            if (URegEx._thievesLostInOperation.IsMatch(matchValue))
            {
                Guid provID = currentUser.PimpUser.CurrentActiveProvince;
                CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
                var GetOwner = (from xx in db.Utopia_Province_Data_Captured_Gens
                                where xx.Province_ID == provID
                                select xx).FirstOrDefault();
                if (GetOwner != null && GetOwner.Thieves > 0)
                    GetOwner.Thieves -= Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._thievesLostInOperation.Match(matchValue).Value).Value);
                db.SubmitChanges();
                return matchValue.Replace(URegEx._thievesLostInOperation.Match(matchValue).Value, "");
            }
            return matchValue;
        }
        /// <summary>
        /// Handles all thieve ops.
        /// </summary>
        /// <param name="RawData">Raw Thieve op.</param>
        /// <returns>Fake Guid</returns>
        public static string ParseMyticThieveOps(string RawData, string provinceIDs, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            Guid provID = currentUser.PimpUser.CurrentActiveProvince;
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            string ProvinceAttacked = string.Empty;
            string opsAdded = string.Empty;
            string[] provinces = provinceIDs.Split(',');

            ThievesAssasinated(ref RawData, currentUser, cachedKingdom, ref opsAdded);
            thievesAssasinated2(ref RawData, currentUser, cachedKingdom, ref opsAdded, provinces);
            ThievesAssasinatedWizards(ref RawData, currentUser, cachedKingdom, db, ref opsAdded, provinces);
            thievesKidnapped(ref RawData, currentUser, cachedKingdom, ref opsAdded, provinces);
            thievesStoleMoney(ref RawData, currentUser, cachedKingdom, provID, db, ref opsAdded, provinces);
            thievesCreatedRiots(ref RawData, currentUser, cachedKingdom, ref opsAdded, provinces);
            thievesCreatedRiotsNoEffects(ref RawData, currentUser, cachedKingdom, ref opsAdded, provinces);
            thievesCreatedRiots2(ref RawData, currentUser, cachedKingdom, ProvinceAttacked, ref opsAdded);
            thievesBribed(ref RawData, currentUser, cachedKingdom, ref opsAdded, provinces);
            thievesBribedGeneral(ref RawData, currentUser, cachedKingdom, ref opsAdded, provinces);
            thievesSabatogedSpells(ref RawData, currentUser, cachedKingdom, ref opsAdded, provinces);
            thievesStoleRunes(ref RawData, currentUser, cachedKingdom, provID, db, ref opsAdded, provinces);
            thievesStoleFood(ref RawData, currentUser, cachedKingdom, provID, db, ref opsAdded, provinces);
            thievesStoleFood2(ref RawData, currentUser, cachedKingdom, provID, db, ref opsAdded, provinces);
            thievesBurnedAcres(ref RawData, currentUser, cachedKingdom, ref opsAdded, provinces);
            thievesTriedToBurnedAcres(ref RawData, currentUser, cachedKingdom, ref opsAdded, provinces);
            thievesStoleHorses(ref RawData, currentUser, cachedKingdom, provID, db, ref opsAdded, provinces);
            thievesFreedPrisoners(ref RawData, currentUser, cachedKingdom, db, ref opsAdded, provinces);
            thievesInfiltrated(ref RawData, currentUser, cachedKingdom, provID, db, ProvinceAttacked, ref opsAdded);
            thievesConvertedArmy(ref RawData, currentUser, cachedKingdom, ref opsAdded, provinces);
            thievesFailedToConvertedArmy(ref RawData, currentUser, cachedKingdom, ref opsAdded, provinces);
            thievesWakeDead(ref RawData, currentUser, cachedKingdom, provID, ref opsAdded);

            RawData = UtopiaParser.InsertOpPersonal(OpType.warSpoils, URegEx._warSpoils.Matches(RawData), RawData, currentUser, cachedKingdom);
            RawData = UtopiaParser.InsertOpPersonal(OpType.minorProtection, URegEx._minorProtection.Matches(RawData), RawData, currentUser, cachedKingdom);
            RawData = UtopiaParser.InsertOpPersonal(OpType.fog, URegEx._fog.Matches(RawData), RawData, currentUser, cachedKingdom);
            RawData = UtopiaParser.InsertOpPersonal(OpType.magicShield, URegEx._magicShield.Matches(RawData), RawData, currentUser, cachedKingdom);
            RawData = UtopiaParser.InsertOpPersonal(OpType.fertileLands, URegEx._fertileLands.Matches(RawData), RawData, currentUser, cachedKingdom);
            RawData = UtopiaParser.InsertOpPersonal(OpType.aggression, URegEx._aggression.Matches(RawData), RawData, currentUser, cachedKingdom);
            RawData = UtopiaParser.InsertOpPersonal(OpType.naturesBlessing, URegEx._naturesBlessing.Matches(RawData), RawData, currentUser, cachedKingdom);
            RawData = UtopiaParser.InsertOpPersonal(OpType.highBirth, URegEx._highBirth.Matches(RawData), RawData, currentUser, cachedKingdom);
            RawData = UtopiaParser.InsertOpPersonal(OpType.fastBuilders, URegEx._fastBuilders.Matches(RawData), RawData, currentUser, cachedKingdom);
            RawData = UtopiaParser.InsertOpPersonal(OpType.inspireArmy, URegEx._inspireArmy.Matches(RawData), RawData, currentUser, cachedKingdom);
            RawData = UtopiaParser.InsertOpPersonal(OpType.patriotism, URegEx._patriotism.Matches(RawData), RawData, currentUser, cachedKingdom);
            RawData = UtopiaParser.InsertOpPersonal(OpType.reflectMagic, URegEx._reflectMagic.Matches(RawData), RawData, currentUser, cachedKingdom);
            RawData = UtopiaParser.InsertOpPersonal(OpType.speedAttack, URegEx._speedAttack.Matches(RawData), RawData, currentUser, cachedKingdom);
            RawData = UtopiaParser.InsertOpPersonal(OpType.townWatch, URegEx._townWatch.Matches(RawData), RawData, currentUser, cachedKingdom);
            RawData = UtopiaParser.InsertOpPersonal(OpType.clearSight, URegEx._clearSight.Matches(RawData), RawData, currentUser, cachedKingdom);
            RawData = UtopiaParser.InsertOpPersonal(OpType.thievesInvisible, URegEx._thievesTurnedInvisible.Matches(RawData), RawData, currentUser, cachedKingdom);

            wizardsReflectMagic(ref RawData, currentUser, cachedKingdom, ref opsAdded, provinces);
            wizardsCastedTreeOfGold(ref RawData, currentUser, cachedKingdom, provID, db, ref opsAdded);
            wizardsCastedAnonmity(ref RawData, currentUser, cachedKingdom, provID, ref opsAdded);
            castedPitfalls(ref RawData, currentUser, cachedKingdom, ref opsAdded);
            castedIncinerateRunes(ref RawData, currentUser, cachedKingdom, db, ref opsAdded);
            castedTornadoes(ref RawData, currentUser, cachedKingdom, ref opsAdded);
            castedFireballs(ref RawData, currentUser, cachedKingdom, db, ref opsAdded);
            castedNightmares(ref RawData, currentUser, cachedKingdom, db, ref opsAdded);
            castedGreedySoldiers(ref RawData, currentUser, cachedKingdom, ref opsAdded, provinces);
            castedFountainOfKnowledge(ref RawData, currentUser, cachedKingdom, ref opsAdded, provinces);
            castedGreedySoldiers2(ref RawData, currentUser, cachedKingdom, ref opsAdded);
            castedGreedySoldiers3(ref RawData, currentUser, cachedKingdom, ref opsAdded);
            castedTurnedGoldToLead(ref RawData, currentUser, cachedKingdom, db, ref opsAdded);
            castedTurnedGoldToLeadNoEffects(ref RawData, currentUser, cachedKingdom, ref opsAdded, provinces);
            castedGoldToLead(ref RawData, currentUser, cachedKingdom, db, ref opsAdded, provinces);
            exposedThieves(ref RawData, currentUser, cachedKingdom, ref opsAdded, provinces);
            castedStorms(ref RawData, currentUser, cachedKingdom, ref opsAdded);
            castedStormsNoEffects(ref RawData, currentUser, cachedKingdom, ref opsAdded, provinces);
            castedExplosions(ref RawData, currentUser, cachedKingdom, ref opsAdded);
            castedMeteors(ref RawData, currentUser, cachedKingdom, ref opsAdded);
            castedVermin(ref RawData, currentUser, cachedKingdom, ref opsAdded);
            castedDroughts(ref RawData, currentUser, cachedKingdom, ref opsAdded);
            castedDroughtsNoEffects(ref RawData, currentUser, cachedKingdom, ref opsAdded, provinces);
            castedChastity(ref RawData, currentUser, cachedKingdom, ref opsAdded);
            castedForgotBooks(ref RawData, currentUser, cachedKingdom, ref opsAdded);
            castedLandLust(ref RawData, currentUser, cachedKingdom, provID, db, ref opsAdded);
            castedConvertedThieves(ref RawData, currentUser, cachedKingdom, provID, db, ref opsAdded);
            castedConvertWizardsTroops(ref RawData, currentUser, cachedKingdom, provID, db, ref opsAdded, provinces);
            castedShadowLight(ref RawData, currentUser, cachedKingdom, provID, ref opsAdded);
            castedParadise(ref RawData, currentUser, cachedKingdom, provID, ref opsAdded);
            castedNaturesBlessingButFailed(ref RawData, currentUser, cachedKingdom, provID, ref opsAdded);
            castedNaturesBlessingButFailed2(ref RawData, currentUser, cachedKingdom, ref opsAdded);
            castedMysticVortex(ref RawData, currentUser, cachedKingdom, db, ref opsAdded);
            castedMysticVortexButFailed(ref RawData, currentUser, cachedKingdom, ref opsAdded);
            donatedToStartDragon(ref RawData, currentUser, cachedKingdom, provID, db, ref opsAdded);
            sentTroopsToKillDragon(ref RawData, currentUser, cachedKingdom, ref opsAdded);
            sentAid(ref RawData, currentUser, cachedKingdom, provID, db, ref opsAdded);

            if (URegEx._validateCheck.IsMatch(RawData))
            {
                UtopiaParser.FailedAt(ErrorTypeEnum.failedMyticsOps, RawData, currentUser.PimpUser.UserID);
                return UtopiaParser.ReturnErrorsToUser(ErrorTypeEnum.CouldntFindFullOp);
            }
            if (URegEx._rgxFailedCheck.IsMatch(RawData))
            {
                UtopiaParser.FailedAt(ErrorTypeEnum.ParseThieveOperations, RawData, currentUser.PimpUser.UserID);
                return UtopiaParser.ReturnErrorsToUser(ErrorTypeEnum.CouldntFindFullOp);
            }
            return "Ops Submitted " + opsAdded;
        }

        private static void thievesAssasinated2(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._thievesAssasinate.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                        UtopiaParser.AddThiefOp(OpType.assasinate, new Guid(item), URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(matchValue).Value).Value, currentUser, cachedKingdom);

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Assasination; ";
            }

        }

        private static void sentAid(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, Guid provID, CS_Code.UtopiaDataContext db, ref string opsAdded)
        {
            MatchCollection mc = URegEx._sentAid.Matches(RawData);//We have sent 6,792 runes to YHWH (11:34)
            for (int i = 0; i < mc.Count; i++)
            {
                string tempGoldRunes = URegEx.rgxQuantitiesWithComma.Match(mc[i].Value).Value;
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("We have sent " + tempGoldRunes + " gold coins to", "").Replace("We have sent " + tempGoldRunes + " runes to", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);

                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);

                var getProvOwner = (from xx in db.Utopia_Province_Data_Captured_Gens
                                    where xx.Province_ID == provID
                                    select xx).FirstOrDefault();
                var getProvSent = (from xx in db.Utopia_Province_Data_Captured_Gens
                                   where xx.Province_ID == ProvinceID
                                   select xx).FirstOrDefault();
                if (mc[i].Value.Contains("runes"))
                {
                    getProvOwner.Runes += Convert.ToInt32(tempGoldRunes.Replace(",", ""));
                    if (getProvOwner.Runes > 0)
                    {
                        getProvOwner.Runes -= Convert.ToInt32(tempGoldRunes.Replace(",", ""));
                        db.SubmitChanges();
                    }
                    UtopiaParser.InsertOp(OpType.sentRunes, ProvinceID, tempGoldRunes.Replace(",", ""), currentUser, cachedKingdom);
                }
                else if (mc[i].Value.Contains("gold coins"))
                {
                    getProvOwner.Money += Convert.ToInt32(tempGoldRunes.Replace(",", ""));
                    if (getProvOwner.Money > 0)
                    {
                        getProvOwner.Money -= Convert.ToInt32(tempGoldRunes.Replace(",", ""));
                        db.SubmitChanges();
                    }
                    UtopiaParser.InsertOp(OpType.sentMoney, ProvinceID, tempGoldRunes.Replace(",", ""), currentUser, cachedKingdom);
                }
                db.SubmitChanges();

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Sent Aid; ";
            }
        }

        private static void sentTroopsToKillDragon(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded)
        {
            MatchCollection mc = URegEx._sentKillDragon.Matches(RawData);//You send out 688 troops to fight the dragon. All are lost in the fight, but the dragon is weakened by 3440 points.
            for (int i = 0; i < mc.Count; i++)
            {
                UtopiaParser.InsertOpPersonal(OpType.killingDragon, URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value.Replace(",", ""), currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Killing Dragon; ";
            }

        }

        private static void donatedToStartDragon(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, Guid provID, CS_Code.UtopiaDataContext db, ref string opsAdded)
        {
            MatchCollection mc = URegEx._findDonatedCoinsDragon.Matches(RawData);//You have donated 148,707 gold coins to the quest of launching a dragon.  
            for (int i = 0; i < mc.Count; i++)
            {
                var getProvOwner = (from xx in db.Utopia_Province_Data_Captured_Gens
                                    where xx.Province_ID == provID
                                    select xx).FirstOrDefault();
                if (getProvOwner.Money > 0)
                {
                    getProvOwner.Money -= Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value.Replace(",", ""));
                    db.SubmitChanges();
                } UtopiaParser.InsertOpPersonal(OpType.donatedGoldDragon, URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value.Replace(",", ""), currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Donated to Dragon; ";
            }

        }

        private static void castedMysticVortex(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, CS_Code.UtopiaDataContext db, ref string opsAdded)
        {
            //Keep at the end of all spells.  Due to searching RawData for the word Negated.
            //Your wizards gather 4,095 runes and begin casting, and the spell succeeds. A magic vortex overcomes the province of But your sister swallows(9:28), negating 6 active spells (Vermin, Pitfalls, Nature's Blessing, Minor Protection, Fog and Love and Peace). .
            //Your wizards gather 3,903 runes and begin casting, and the spell succeeds. A magic vortex overcomes the province of Deadly Swine Flu(8:8), negating 2 active spells (Inspire Army and Fertile Lands).  
            MatchCollection mc = URegEx._mysticVortex.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("A magic vortex overcomes the province of", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);

                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
                string mcString = string.Empty;
                MatchCollection mcMV = URegEx._mysticVotexNegates.Matches(mc[i].Value);
                if (mcMV.Count > 0) // if there are actual negates.
                {
                    for (int y = 0; y < mcMV.Count; y++)
                        mcString += mcMV[y].Value + "; ";
                    var checkOps = (from xx in db.Utopia_Province_Ops
                                    where xx.Directed_To_Province_ID == ProvinceID
                                    where xx.Owner_Kingdom_ID == currentUser.PimpUser.StartingKingdom
                                    where xx.Expiration_Date.Value > DateTime.UtcNow
                                    select xx);
                    if (checkOps.Count() > 0) // if there are any spells on the province.
                    {
                        for (int y = 0; y < mcMV.Count; y++)
                        {

                            //What spells the MV negated.
                            if (mcMV[y].Value.Contains("Blessing"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.naturesBlessing, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Nature's Blessing"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.naturesBlessing, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Mage's Fury"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.MagesFury, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Patriotism"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.patriotism, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Chastity"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.chastity, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Fog"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.fog, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Peace"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.highBirth, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Storms"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.storms, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Meteor"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.meteors, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Magic Shield"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.magicShield, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Fertile Lands"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.fertileLands, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Greed"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.greedySoldiers, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Minor Protection"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.minorProtection, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Builders Boon"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.fastBuilders, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Inspire Army"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.inspireArmy, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Town Watch"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.townWatch, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Vermin"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.vermin, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Greed"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.greedySoldiers, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Greater Protection"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.greatProtection, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Pitfalls"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.pitfalls, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Quick Feet"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.quickFeet, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Fountain of Knowledge"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.fountainKnowledge, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Reflect Magic"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.reflectMagic, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Clear Sight"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.clearSight, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Animate Dead"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.animateDead, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Explosions"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.explosions, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("Anonymity"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.anonymity, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else if (mcMV[y].Value.Contains("War Spoils"))
                            {
                                var getOp = (from xx in checkOps
                                             where xx.Op_ID == UtopiaParser.GetOpID(OpType.warSpoils, currentUser.PimpUser.UserID)
                                             select xx).FirstOrDefault();
                                if (getOp != null)
                                    getOp.negated = 1;
                            }
                            else
                                UtopiaParser.FailedAt("mysticVortexFailedNagate", mc[i].Value, currentUser.PimpUser.UserID);
                        }
                    }
                }
                UtopiaParser.InsertOp(OpType.mystVort, ProvinceID, mcString, currentUser, cachedKingdom);
                db.SubmitChanges();
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Mystic Votrex; ";
                if (mcMV.Count != Convert.ToInt32(URegEx.rgxNumber.Match(URegEx._mysticVortexCount.Match(mc[i].Value).Value).Value)) //checks for any more negates in the spell.
                    UtopiaParser.FailedAt("mystVortexSpell", RawData, currentUser.PimpUser.UserID);
            }

        }

        private static void castedNaturesBlessingButFailed2(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded)
        {
            //Natures blessing with province name.
            MatchCollection mc = URegEx._naturesBlessingFailedProvince.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("Unfortunately,", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);

                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);

                UtopiaParser.InsertOp(OpType.naturesBlessingFailed, ProvinceID, currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Natures Blessing; ";
            }

        }
        private static void castedMysticVortexButFailed(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded)
        {
            //Natures blessing with province name.
            MatchCollection mc = URegEx._mysticVortexFailed.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("A magic vortex overcomes the province of", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);

                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);

                UtopiaParser.InsertOp(OpType.mystVortFailed, ProvinceID, currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Mystic Vortex Failed; ";
            }

        }
        private static void castedNaturesBlessingButFailed(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, Guid provID, ref string opsAdded)
        {
            MatchCollection mc = URegEx._naturesBlessingFailed.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                UtopiaParser.InsertOp(OpType.naturesBlessingFailed, provID, currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Nature Blessing; ";
            }

        }

        private static void castedParadise(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, Guid provID, ref string opsAdded)
        {
            MatchCollection mc = URegEx._paradise.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                UtopiaParser.InsertOp(OpType.paradise, provID, URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampFront.Match(mc[i].Value).Value).Value.Replace(",", ""), currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Paradise; ";
            }

        }

        private static void castedShadowLight(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, Guid provID, ref string opsAdded)
        {
            MatchCollection mc = URegEx._shadowlight.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                UtopiaParser.InsertOp(OpType.shadowlight, provID, currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Shadow Light; ";
            }

        }

        private static void thievesWakeDead(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, Guid provID, ref string opsAdded)
        {
            MatchCollection mc = URegEx._wakeDead.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                UtopiaParser.InsertOp(OpType.wakeDead, provID, currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Woke Dead; ";
            }

        }

        private static void thievesConvertedArmy(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded, string[] provinces)
        {
            //We have converted 4 enemy specialists to our army.
            //We have converted 0 enemy elite troops to our army
            MatchCollection mc = URegEx._convertedArmy.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                        UtopiaParser.InsertOp(OpType.convertedTroops, new Guid(item), URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value, currentUser, cachedKingdom);

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Converted Troops; ";
            }

        }
        private static void thievesFailedToConvertedArmy(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded, string[] provinces)
        {
            //We have converted 4 enemy specialists to our army.
            //We have converted 0 enemy elite troops to our army
            MatchCollection mc = URegEx._convertedArmyFailed.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                        UtopiaParser.InsertOp(OpType.convertedTroopsFailed, new Guid(item), currentUser, cachedKingdom);

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Failed Convert Troops; ";
            }

        }

        private static void castedConvertWizardsTroops(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, Guid provID, CS_Code.UtopiaDataContext db, ref string opsAdded, string[] provinces)
        {
            //We have converted 13 enemy thieves to our guild.
            MatchCollection mc = URegEx._convertedArmyThievesWizards.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                foreach (string item in provinces)
                {
                    if (item.IsValidGuid())
                    {
                        if (mc[i].Value.Contains("thieves from the enemy") || mc[i].Value.Contains("thief from the enemy"))
                        {
                            var getProvince = (from xx in db.Utopia_Province_Data_Captured_Gens
                                               where xx.Province_ID == new Guid(item)
                                               select xx).FirstOrDefault();

                            var getProvOwner = (from xx in db.Utopia_Province_Data_Captured_Gens
                                                where xx.Province_ID == provID
                                                select xx).FirstOrDefault();
                            if (getProvince.Thieves != null)
                                getProvince.Thieves -= Convert.ToInt32(URegEx.rgxNumber.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value);
                            getProvOwner.Thieves += Convert.ToInt32(URegEx.rgxNumber.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value);
                            db.SubmitChanges();
                            UtopiaParser.InsertOp(OpType.convertThieves, new Guid(item), URegEx.rgxNumber.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value, currentUser, cachedKingdom);
                        }
                        else if (mc[i].Value.Contains("wizards from the enemy"))
                        {
                            var getProvince = (from xx in db.Utopia_Province_Data_Captured_Gens
                                               where xx.Province_ID == new Guid(item)
                                               select xx).FirstOrDefault();

                            var getProvOwner = (from xx in db.Utopia_Province_Data_Captured_Gens
                                                where xx.Province_ID == provID
                                                select xx).FirstOrDefault();
                            if (getProvince.Wizards != null)
                                getProvince.Wizards -= Convert.ToInt32(URegEx.rgxNumber.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value);
                            getProvOwner.Wizards += Convert.ToInt32(URegEx.rgxNumber.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value);
                            db.SubmitChanges();
                            UtopiaParser.InsertOp(OpType.convertedWizards, new Guid(item), URegEx.rgxNumber.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value, currentUser, cachedKingdom);
                        }
                        else if (mc[i].Value.Contains("soldiers from the enemy") || mc[i].Value.Contains("soldier from the enemy"))
                        {
                            var getProvince = (from xx in db.Utopia_Province_Data_Captured_Gens
                                               where xx.Province_ID == new Guid(item)
                                               select xx).FirstOrDefault();

                            var getProvOwner = (from xx in db.Utopia_Province_Data_Captured_Gens
                                                where xx.Province_ID == provID
                                                select xx).FirstOrDefault();
                            if (getProvince.Soldiers != null)
                                getProvince.Soldiers -= Convert.ToInt32(URegEx.rgxNumber.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value);
                            getProvOwner.Soldiers += Convert.ToInt32(URegEx.rgxNumber.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value);
                            db.SubmitChanges();
                            UtopiaParser.InsertOp(OpType.convertedTroops, new Guid(item), URegEx.rgxNumber.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value, currentUser, cachedKingdom);
                        }
                        else if (mc[i].Value.Contains("enemy's specialist troops"))
                            UtopiaParser.InsertOp(OpType.convertedSpecialists, new Guid(item), URegEx.rgxNumber.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value, currentUser, cachedKingdom);
                        else if (mc[i].Value.Contains("Drake from the enemy") || mc[i].Value.Contains("Beastmasters from the enemy") || mc[i].Value.Contains("Knights from the enemy") || mc[i].Value.Contains("Ogres from the enemy") || mc[i].Value.Contains("Berserkers from the enemy") || mc[i].Value.Contains("Brute from the enemy") || mc[i].Value.Contains("Brutes from the enemy") || mc[i].Value.Contains("Ghouls from the enemy") || mc[i].Value.Contains("Ghoul from the enemy"))
                            UtopiaParser.InsertOp(OpType.convertedTroops, new Guid(item), URegEx.rgxNumber.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value, currentUser, cachedKingdom);
                        else if (mc[i].Value.Contains("from the enemy"))
                            UtopiaParser.InsertOp(OpType.convertedTroops, new Guid(item), URegEx.rgxQuantitiesWithComma.Match(mc[i].Value).Value.Replace(",", ""), currentUser, cachedKingdom);
                        else
                            UtopiaParser.FailedAt("'convertedProblems'", mc[i].Value, currentUser.PimpUser.UserID);
                    }
                }
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Converted Troops; ";
            }

        }

        private static void castedConvertedThieves(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, Guid provID, CS_Code.UtopiaDataContext db, ref string opsAdded)
        {
            MatchCollection mc = URegEx._convertedThieves.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string convertedThieves = URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampFront.Match(mc[i].Value).Value).Value;
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("We have converted " + convertedThieves + " of", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);

                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
                var getProvince = (from xx in db.Utopia_Province_Data_Captured_Gens
                                   where xx.Province_ID == ProvinceID
                                   select xx).FirstOrDefault();
                if (getProvince.Thieves != null)
                    getProvince.Thieves -= Convert.ToInt32(convertedThieves.Replace(",", ""));
                var getProvOwner = (from xx in db.Utopia_Province_Data_Captured_Gens
                                    where xx.Province_ID == provID
                                    select xx).FirstOrDefault();
                getProvOwner.Thieves += Convert.ToInt32(convertedThieves.Replace(",", ""));
                db.SubmitChanges();
                UtopiaParser.InsertOp(OpType.convertThieves, ProvinceID, convertedThieves.Replace(",", ""), currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");

                opsAdded += "Converted Thieves; ";
            }

        }

        private static void castedLandLust(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, Guid provID, CS_Code.UtopiaDataContext db, ref string opsAdded)
        {
            //The spell consumes 4941 Runes and ... is successful! Our Land Lust over World of Grim Lands (12:21) has given us 9 new acres of land!  
            MatchCollection mc = URegEx._landLust.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("Our Land Lust over", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);

                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
                var getProvince = (from xx in db.Utopia_Province_Data_Captured_Gens
                                   where xx.Province_ID == ProvinceID
                                   select xx).FirstOrDefault();
                if (getProvince.Land != null)
                    getProvince.Land -= Convert.ToInt32(URegEx.rgxNumber.Match(URegEx._findAcres.Match(mc[i].Value).Value).Value);
                var getProvOwner = (from xx in db.Utopia_Province_Data_Captured_Gens
                                    where xx.Province_ID == provID
                                    select xx).FirstOrDefault();
                getProvOwner.Land += Convert.ToInt32(URegEx.rgxNumber.Match(URegEx._findAcres.Match(mc[i].Value).Value).Value);
                db.SubmitChanges();
                UtopiaParser.InsertOp(OpType.landLust, ProvinceID, URegEx._findAcres.Match(mc[i].Value).Value, currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Land Lusted; ";
            }

        }

        private static void castedForgotBooks(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded)
        {
            MatchCollection mc = URegEx._forgotBooks.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("You were able to make the people of", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);

                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);

                UtopiaParser.InsertOp(OpType.forgetBooks, ProvinceID, URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value.Replace(",", ""), currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Books of Knowledge; ";
            }

        }

        private static void castedChastity(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded)
        {
            //Your wizards gather 642 runes and begin casting, and the spell succeeds. Much to the chagrin of their men, the womenfolk of nomnom11 (11:21) have taken a vow of chastity for 6 days!
            MatchCollection mc = URegEx._chastity.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("the womenfolk of", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);

                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
                if (URegEx.rgxDayOps.IsMatch(mc[i].Value))
                    UtopiaParser.InsertOp(OpType.chastity, Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxDayOps.Match(mc[i].Value).Value).Value), ProvinceID, currentUser, cachedKingdom);
                else
                    UtopiaParser.InsertOp(OpType.chastity, ProvinceID, currentUser, cachedKingdom);

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Chastity; ";
            }

        }

        private static void castedDroughts(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded)
        {
            //Your wizards gather their runes and begin casting. The spell consumes 1110 Runes and ... is successful! A drought will reign over the lands of pestis (6:12) for about 9 days!  
            MatchCollection mc = URegEx._drought.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("A drought will reign over the lands of", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);

                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);

                UtopiaParser.InsertOp(OpType.drought, Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxDayOps.Match(mc[i].Value).Value).Value), ProvinceID, currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Drought; ";
            }

        }
        private static void castedDroughtsNoEffects(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded, string[] provinces)
        {

            MatchCollection mc = URegEx._droughtsNoEffects.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                        UtopiaParser.InsertOp(OpType.stormsNoEffects, new Guid(item), currentUser, cachedKingdom);

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Droughts No Effects; ";
            }

        }

        private static void castedVermin(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded)
        {
            //Your wizards gather their runes and begin casting. The spell consumes 1332 Runes and ... is successful! Vermin will feast on the granaries of Ntis (6:12) for about 12 days!  
            MatchCollection mc = URegEx._vermin.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("Vermin will feast on the granaries of", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);

                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);

                UtopiaParser.InsertOp(OpType.vermin, Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxDayOps.Match(mc[i].Value).Value).Value), ProvinceID, currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Vermin; ";
            }

        }

        private static void castedMeteors(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded)
        {
            //Your wizards gather 3,570 runes and begin casting, and the spell succeeds. Meteors will rain across the lands of Crouching Tiger Hidden Pussy(6:39) for 10 days.
            MatchCollection mc = URegEx._meteors.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("Meteors will rain across the lands of", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);

                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);

                UtopiaParser.InsertOp(OpType.meteors, Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxDayOps.Match(mc[i].Value).Value).Value), ProvinceID, currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Meteors; ";
            }

        }

        private static void castedExplosions(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded)
        {
            //Explosions will rock aid shipments to and from Otho (12:1) for about 10 days!  
            MatchCollection mc = URegEx._explosions.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("Explosions will rock aid shipments to and from", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);

                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);

                UtopiaParser.InsertOp(OpType.explosions, Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxDayOps.Match(mc[i].Value).Value).Value), ProvinceID, currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Explosions; ";
            }

        }
        private static void castedStormsNoEffects(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded, string[] provinces)
        {
            //Your wizards gather their runes and begin casting. The spell consumes 926 Runes and ... is successful! Storms will ravage drome (6:12)'s lands for the next 14 days!  
            MatchCollection mc = URegEx._stormsNoEffects.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                        UtopiaParser.InsertOp(OpType.stormsNoEffects, new Guid(item), currentUser, cachedKingdom);

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Storms No Effects; ";
            }

        }
        private static void castedStorms(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded)
        {
            //Your wizards gather their runes and begin casting. The spell consumes 926 Runes and ... is successful! Storms will ravage drome (6:12)'s lands for the next 14 days!  
            MatchCollection mc = URegEx._storms.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("Storms will ravage", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);

                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);

                UtopiaParser.InsertOp(OpType.storms, Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxDayOps.Match(mc[i].Value).Value).Value), ProvinceID, currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Storms; ";
            }

        }

        private static void exposedThieves(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded, string[] provinces)
        {
            //The spell consumes 1963 Runes and ... is successful! Our mages have illuminated the lands of our enemies and exposed the thieves that walk through their lands. 
            MatchCollection mc = URegEx._exposedThieves.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                        UtopiaParser.InsertOp(OpType.exposedThieves, new Guid(item), currentUser, cachedKingdom);

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Exposed Thieves; ";
            }

        }

        private static void castedGoldToLead(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, CS_Code.UtopiaDataContext db, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._lead.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                foreach (string item in provinces)
                {
                    if (item.IsValidGuid())
                    {
                        var getProvince = (from xx in db.Utopia_Province_Data_Captured_Gens
                                           where xx.Province_ID == new Guid(item)
                                           select xx).FirstOrDefault();
                        if (getProvince.Money > 0)
                            getProvince.Money -= Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value.Replace(",", ""));
                        UtopiaParser.InsertOp(OpType.goldToLead, new Guid(item), URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value.Replace(",", ""), currentUser, cachedKingdom);
                        db.SubmitChanges();
                    }
                }
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Gold Turned to Lead; ";
            }

        }

        private static void castedTurnedGoldToLead(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, CS_Code.UtopiaDataContext db, ref string opsAdded)
        {
            //Your wizards gather 2,185 runes and begin casting, and the spell succeeds. Our mages have turned 57 gold coins in Tunstall square (2:15) to worthless lead.
            MatchCollection mc = URegEx._lead2.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("gold coins in", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
                var getProvince = (from xx in db.Utopia_Province_Data_Captured_Gens
                                   where xx.Province_ID == ProvinceID
                                   select xx).FirstOrDefault();
                if (getProvince.Peasents > 0)
                {
                    getProvince.Money -= Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value.Replace(",", ""));
                    db.SubmitChanges();
                }
                UtopiaParser.InsertOp(OpType.goldToLead, ProvinceID, URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value.Replace(",", ""), currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Gold Turned to Lead; ";
            }

        }

        private static void castedGreedySoldiers3(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded)
        {
            MatchCollection mc = URegEx._greedySoldiersss.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("Our mages have caused our enemy's soldiers of", "").Replace("Our mages have caused our enemy's soldiers to turn greedy the lands of", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);

                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);

                UtopiaParser.InsertOp(OpType.greedySoldiers, Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxDayOps.Match(mc[i].Value).Value).Value), ProvinceID, currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Greedy Soldiers; ";
            }

        }

        private static void castedGreedySoldiers2(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded)
        {
            //Your wizards gather 1,449 runes and begin casting, and the spell succeeds. Our mages have caused our enemy's soldiers to turn greedy the lands of Deadly Swine Flu(8:8) 6 days.  
            MatchCollection mc = URegEx._greedySoldierss.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("Our mages have caused our enemy's soldiers to turn greedy the lands of", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);

                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);

                UtopiaParser.InsertOp(OpType.greedySoldiers, Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxDayOps.Match(mc[i].Value).Value).Value), ProvinceID, currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Greedy Soldiers; ";
            }

        }

        private static void castedFountainOfKnowledge(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._foutainOfKnowledge.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                        UtopiaParser.InsertOp(OpType.fountainKnowledge, Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxDayOps.Match(mc[i].Value).Value).Value), new Guid(item), currentUser, cachedKingdom);

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Fountain of Knowledge; ";
            }

        }

        private static void castedGreedySoldiers(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._greedySoldiers.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                        UtopiaParser.InsertOp(OpType.greedySoldiers, Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxDayOps.Match(mc[i].Value).Value).Value), new Guid(item), currentUser, cachedKingdom);

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Greedy Soldiers; ";
            }

        }

        private static void castedNightmares(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, CS_Code.UtopiaDataContext db, ref string opsAdded)
        {
            MatchCollection mc = URegEx._nightmares.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("and thieves' guilds of", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
                var getProvince = (from xx in db.Utopia_Province_Data_Captured_Gens
                                   where xx.Province_ID == ProvinceID
                                   select xx).FirstOrDefault();

                UtopiaParser.InsertOp(OpType.Nightmares, ProvinceID, URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value.Replace(",", ""), currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += OpType.Nightmares.ToString() + "; ";
            }

        }

        private static void castedFireballs(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, CS_Code.UtopiaDataContext db, ref string opsAdded)
        {
            //The spell consumes 1724 Runes and ... is successful! A fireball burns through the skies of Spaceman Spiff (16:31). 310 people are killed in the destruction  
            MatchCollection mc = URegEx._fireball.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("A fireball burns through the skies of", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
                var getProvince = (from xx in db.Utopia_Province_Data_Captured_Gens
                                   where xx.Province_ID == ProvinceID
                                   select xx).FirstOrDefault();
                if (getProvince.Peasents > 0)
                {
                    getProvince.Peasents -= Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value.Replace(",", ""));
                    db.SubmitChanges();
                }
                UtopiaParser.InsertOp(OpType.fireball, ProvinceID, URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value.Replace(",", ""), currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Fireball; ";
            }

        }

        private static void castedTornadoes(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded)
        {
            MatchCollection mc = URegEx._tornadoes.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("Tornadoes scour the lands of", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);

                if (URegEx._findAcres.IsMatch(mc[i].Value))//checks if it destroyed any acres.
                    UtopiaParser.InsertOp(OpType.tornadoes, ProvinceID, URegEx.rgxNumber.Match(URegEx._findAcres.Match(mc[i].Value).Value).Value + " Homes", currentUser, cachedKingdom);
                else
                    UtopiaParser.InsertOp(OpType.tornadoes, ProvinceID, "0 Homes", currentUser, cachedKingdom);

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Tornadoes; ";
            }

        }
        /// <summary>
        /// Your wizards gather 2,180 runes and begin casting, and the spell succeeds. Lightning strikes the Towers in To be reseted (9:1) and incinerates 4,180 runes!  
        /// </summary>
        /// <param name="RawData"></param>
        /// <param name="currentUser"></param>
        /// <param name="cachedKingdom"></param>
        /// <param name="db"></param>
        /// <param name="opsAdded"></param>
        /// <param name="mc"></param>
        private static void castedIncinerateRunes(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, CS_Code.UtopiaDataContext db, ref string opsAdded)
        {

            MatchCollection mc = URegEx._incinerateRunes.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value;
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Replace("Lightning strikes the Towers in", "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);

                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);

                var getProvince = (from xx in db.Utopia_Province_Data_Captured_Gens
                                   where xx.Province_ID == ProvinceID
                                   select xx).FirstOrDefault();
                if (getProvince.Runes > 0)
                {
                    getProvince.Runes -= Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampFront.Match(mc[i].Value).Value).Value.Replace(",", ""));
                    db.SubmitChanges();
                }
                UtopiaParser.InsertOp(OpType.incineratesRunes, ProvinceID, URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampFront.Match(mc[i].Value).Value).Value.Replace(",", ""), currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Incinerate Runes; ";
            }

        }
        /// <summary>
        /// Your wizards gather their runes and begin casting. The spell consumes 1487 Runes and ... is successful! Pitfalls will haunt the lands of Ohhh (3:34) for about 18 days!  
        /// The spell consumes 1542 Runes and ... is successful! Pitfalls will haunt the lands of MtnDew Ppsi DrPper (3:34) for about 13 days! 
        /// </summary>
        /// <param name="RawData"></param>
        /// <param name="currentUser"></param>
        /// <param name="cachedKingdom"></param>
        /// <param name="opsAdded"></param>
        /// <param name="mc"></param>
        private static void castedPitfalls(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded)
        {
            MatchCollection mc = URegEx._pitfalls.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string provinceNameIsLo = URegEx._findMysticThiefProvinceNameRevamp.Match(mc[i].Value).Value.Replace("Pitfalls will haunt the lands of", "");
                string provinceName = provinceNameIsLo.Replace(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value, "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(provinceNameIsLo).Value).Value).Value);

                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);

                UtopiaParser.InsertOp(OpType.pitfalls, Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxDayOps.Match(mc[i].Value).Value).Value), ProvinceID, currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Pitfalls; ";
            }

        }

        private static void wizardsCastedAnonmity(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, Guid provID, ref string opsAdded)
        {
            MatchCollection mc = URegEx._anonymity.Matches(RawData);//There are no days attached to this item.  Thats why its not a personal Op.
            for (int i = 0; i < mc.Count; i++)
            {
                UtopiaParser.InsertOp(OpType.anonymity, provID, currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "anonymity; ";
            }

        }

        private static void wizardsCastedTreeOfGold(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, Guid provID, CS_Code.UtopiaDataContext db, ref string opsAdded)
        {
            MatchCollection mc = URegEx._treeOfGold.Matches(RawData);//There are no days attached to this item.  Thats why its not a personal Op.
            for (int i = 0; i < mc.Count; i++)
            {
                var getProvOwner = (from xx in db.Utopia_Province_Data_Captured_Gens
                                    where xx.Province_ID == provID
                                    select xx).FirstOrDefault();
                getProvOwner.Money += Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value.Replace(",", ""));
                db.SubmitChanges();
                UtopiaParser.InsertOpPersonal(OpType.treeGold, URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(mc[i].Value).Value).Value.Replace(",", ""), currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Tree Of Gold; ";
            }

        }

        private static void wizardsReflectMagic(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._reflectedMagic.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                        UtopiaParser.InsertOp(OpType.reflectMagic, new Guid(item), currentUser, cachedKingdom);

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Reflected Magic; ";
            }

        }

        private static void thievesInfiltrated(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, Guid provID, CS_Code.UtopiaDataContext db, string ProvinceAttacked, ref string opsAdded)
        {
            MatchCollection mc = URegEx._rgxInfiltrated.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);
                ProvinceAttacked = URegEx._findMysticThiefProvinceNameRevamp.Match(matchValue).Value.Replace("Our thieves have infiltrated the Thieves' Guilds of", "").Trim();
                string provinceName = URegEx.rgxFindIslandLocation.Replace(ProvinceAttacked, "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(ProvinceAttacked).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(ProvinceAttacked).Value).Value).Value);
                Guid Province_ID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
                var getProvinceInfo = (from upi in db.Utopia_Province_Data_Captured_Gens
                                       where upi.Province_ID == Province_ID
                                       select upi).FirstOrDefault();
                switch (getProvinceInfo.Thieves_Value_Type.GetValueOrDefault(0))
                {
                    case 1:
                        break;
                    case 2:
                    case 3:
                    case 4:
                    default:
                        getProvinceInfo.Thieves_Value_Type = 2;
                        getProvinceInfo.Thieves = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefOpFindThieveQuantitie.Match(matchValue).Value).Value.Replace(",", ""));
                        getProvinceInfo.Updated_By_DateTime = DateTime.UtcNow;
                        getProvinceInfo.Updated_By_Province_ID = provID;
                        db.SubmitChanges();
                        break;
                }
                UtopiaParser.AddThiefOp(OpType.Infiltrated, Province_ID, URegEx._findMysticThiefOpFindThieveQuantitie.Match(mc[i].Value).Value.Replace(",", ""), currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Infiltration; ";
            }

        }

        private static void thievesFreedPrisoners(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, CS_Code.UtopiaDataContext db, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._thievesFreePrisoners.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                    {
                        UtopiaParser.AddThiefOp(OpType.freePrisoners, new Guid(item), URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(matchValue).Value).Value, currentUser, cachedKingdom);
                        var getProvinceInfo = (from upi in db.Utopia_Province_Data_Captured_Gens
                                               where upi.Province_ID == new Guid(item)
                                               select upi).FirstOrDefault();
                        if (getProvinceInfo != null)
                            if (getProvinceInfo.Prisoners.GetValueOrDefault() > 0)
                            {
                                getProvinceInfo.Prisoners -= Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(matchValue).Value).Value.Replace(",", ""));
                                db.SubmitChanges();
                            }
                    }
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Freed Prisoners; ";
            }

        }

        private static void thievesStoleHorses(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, Guid provID, CS_Code.UtopiaDataContext db, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._thievesStealWarHorses.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);
                int horses = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampFront.Match(matchValue).Value).Value) / 2;
                foreach (string item in provinces)
                {
                    if (item.IsValidGuid())
                    {
                        UtopiaParser.AddThiefOp(OpType.stealHorses, new Guid(item), horses.ToString(), currentUser, cachedKingdom);
                        var getProvince = (from xx in db.Utopia_Province_Data_Captured_Gens
                                           where xx.Province_ID == new Guid(item)
                                           select xx).FirstOrDefault();
                        if (getProvince.War_Horses > 0)
                            getProvince.Food -= horses;
                        var getProvOwner = (from xx in db.Utopia_Province_Data_Captured_Gens
                                            where xx.Province_ID == provID
                                            select xx).FirstOrDefault();
                        getProvOwner.Food += horses;
                        db.SubmitChanges();
                    }
                }
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Stole Horses; ";
            }

        }

        private static void thievesBurnedAcres(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._thievesBurnedAcres.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                        UtopiaParser.AddThiefOp(OpType.burnedAcres, new Guid(item), URegEx._findMysticThiefQualityRevampBack.Match(matchValue).Value.Trim(), currentUser, cachedKingdom);

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Burned Acres; ";
            }

        }

        private static void thievesTriedToBurnedAcres(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._thievesBurnedAcresFailed.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                        UtopiaParser.AddThiefOp(OpType.triedToBurnAcres, new Guid(item), string.Empty, currentUser, cachedKingdom);

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Treid To Burn Acres; ";
            }

        }

        private static void thievesStoleFood(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, Guid provID, CS_Code.UtopiaDataContext db, ref string opsAdded, string[] provinces)
        {
            int tempFood;
            MatchCollection mc = URegEx._rgxStoleFood.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);
                tempFood = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(matchValue).Value).Value.Replace(",", ""));
                foreach (string item in provinces)
                {
                    if (item.IsValidGuid())
                    {
                        var getProvince = (from xx in db.Utopia_Province_Data_Captured_Gens
                                           where xx.Province_ID == new Guid(item)
                                           select xx).FirstOrDefault();
                        if (getProvince.Food > 0)
                            getProvince.Food -= tempFood;
                        var getProvOwner = (from xx in db.Utopia_Province_Data_Captured_Gens
                                            where xx.Province_ID == provID
                                            select xx).FirstOrDefault();
                        getProvOwner.Food += tempFood;
                        db.SubmitChanges();
                        UtopiaParser.AddThiefOp(OpType.stoleFood, new Guid(item), URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(matchValue).Value).Value.Replace(",", ""), currentUser, cachedKingdom);
                    }
                }
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += tempFood.ToString("N0") + " bushels stolen; ";
            }

        }

        private static void thievesStoleFood2(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, Guid provID, CS_Code.UtopiaDataContext db, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._rgxStoleFoods.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);
                int tempFood = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(matchValue).Value).Value.Replace(",", "")));
                foreach (string item in provinces)
                {
                    if (item.IsValidGuid())
                    {
                        var getProvince = (from xx in db.Utopia_Province_Data_Captured_Gens
                                           where xx.Province_ID == new Guid(item)
                                           select xx).FirstOrDefault();
                        if (getProvince.Food > 0)
                            getProvince.Food -= tempFood;
                        var getProvOwner = (from xx in db.Utopia_Province_Data_Captured_Gens
                                            where xx.Province_ID == provID
                                            select xx).FirstOrDefault();
                        getProvOwner.Food += tempFood;
                        db.SubmitChanges();
                        UtopiaParser.AddThiefOp(OpType.stoleFood, new Guid(item), tempFood.ToString(), currentUser, cachedKingdom);
                    }
                }
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += tempFood.ToString("N0") + " bushels stolen; ";
            }


        }

        private static void thievesStoleRunes(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, Guid provID, CS_Code.UtopiaDataContext db, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._rgxStealRunes.Matches(RawData);
            int tempRunes;
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);
                tempRunes = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(matchValue).Value).Value.Replace(",", ""));
                foreach (string item in provinces)
                {
                    if (item.IsValidGuid())
                    {
                        var getProvince = (from xx in db.Utopia_Province_Data_Captured_Gens
                                           where xx.Province_ID == new Guid(item)
                                           select xx).FirstOrDefault();
                        if (getProvince.Runes > 0)
                            getProvince.Runes -= tempRunes;

                        var getProvOwner = (from xx in db.Utopia_Province_Data_Captured_Gens
                                            where xx.Province_ID == provID
                                            select xx).FirstOrDefault();
                        getProvOwner.Runes += tempRunes;
                        db.SubmitChanges();
                        UtopiaParser.AddThiefOp(OpType.stoleRunes, new Guid(item), URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(matchValue).Value).Value.Replace(",", ""), currentUser, cachedKingdom);
                    }
                }
                opsAdded += tempRunes.ToString("N0") + " runes stolen; ";
                RawData = RawData.Replace(mc[i].Value, "");
            }

        }
        /// <summary>
        /// Early indications show that our operation was a success. Our thieves have sabotaged their wizards' ability to cast spells.
        /// </summary>
        /// <param name="RawData"></param>
        /// <param name="currentUser"></param>
        /// <param name="cachedKingdom"></param>
        /// <param name="opsAdded"></param>
        /// <param name="provinces"></param>
        /// <param name="mc"></param>
        private static void thievesSabatogedSpells(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._sabatogeSpells.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                        UtopiaParser.AddThiefOp(OpType.sabotageSpells, new Guid(item), string.Empty, currentUser, cachedKingdom);

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Sabotages; ";
            }

        }

        private static void thievesBribedGeneral(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._bribedGeneral.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                        UtopiaParser.AddThiefOp(OpType.bribedGen, new Guid(item), string.Empty, currentUser, cachedKingdom);

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Bribed Gens; ";
            }

        }

        private static void thievesBribed(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._rgxBribed.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                        UtopiaParser.AddThiefOp(OpType.bribed, new Guid(item), string.Empty, currentUser, cachedKingdom);

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Bribes; ";
            }

        }
        /// <summary>
        /// Our thieves have caused rioting at Yersinia pestis (6:12). It is expected to last 1 days. 
        ///Our thieves have caused rioting at idea (10:21). It is expected to last 7 days.  
        /// </summary>
        /// <param name="RawData"></param>
        /// <param name="currentUser"></param>
        /// <param name="cachedKingdom"></param>
        /// <param name="ProvinceAttacked"></param>
        /// <param name="opsAdded"></param>
        private static void thievesCreatedRiots2(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, string ProvinceAttacked, ref string opsAdded)
        {

            MatchCollection mc = URegEx._rgxRiotss.Matches(RawData); //riots with Province name.
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);
                ProvinceAttacked = URegEx._findMysticThiefProvinceNameRevamp.Match(matchValue).Value.Replace("Our thieves have caused rioting at", "");
                string provinceName = URegEx.rgxFindIslandLocation.Replace(ProvinceAttacked, "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(ProvinceAttacked).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(ProvinceAttacked).Value).Value).Value);
                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
                UtopiaParser.InsertOp(OpType.riots, Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(matchValue).Value).Value.Replace(",", "")), ProvinceID, currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Riots; ";
            }

        }

        private static void thievesCreatedRiots(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._rgxRiots.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                        UtopiaParser.InsertOp(OpType.riots, Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx.rgxDayOps.Match(matchValue).Value).Value.Replace(",", "")), new Guid(item), currentUser, cachedKingdom);

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Riots; ";
            }

        }
        private static void thievesCreatedRiotsNoEffects(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._rgxRiotsNoEffects.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                        UtopiaParser.InsertOp(OpType.riotsNoEffects, new Guid(item), currentUser, cachedKingdom);

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Riots No Effects; ";
            }

        }
        /// <summary>
        /// Your wizards gather 2,291 runes and begin casting, and the spell succeeds. Our spell found no gold to turn to lead. 
        /// </summary>
        /// <param name="RawData"></param>
        /// <param name="currentUser"></param>
        /// <param name="cachedKingdom"></param>
        /// <param name="opsAdded"></param>
        /// <param name="provinces"></param>
        private static void castedTurnedGoldToLeadNoEffects(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._rgxGoldToLeadNoEffects.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                        UtopiaParser.InsertOp(OpType.goldToLeadNoEffects, new Guid(item), currentUser, cachedKingdom);

                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Gold No Effects; ";
            }

        }

        private static void thievesStoleMoney(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, Guid provID, CS_Code.UtopiaDataContext db, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._thievesStoleMoney.Matches(RawData);
            int tempCash;
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);
                tempCash = Convert.ToInt32(URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(matchValue).Value).Value.Replace(",", ""));
                foreach (string item in provinces)
                {
                    if (item.IsValidGuid())
                    {
                        var getProvince = (from xx in db.Utopia_Province_Data_Captured_Gens
                                           where xx.Province_ID == new Guid(item)
                                           select xx).FirstOrDefault();
                        if (getProvince.Money > 0)
                            getProvince.Money -= tempCash;
                        opsAdded += tempCash.ToString("N0") + "gc stolen; ";
                        var getProvOwner = (from xx in db.Utopia_Province_Data_Captured_Gens
                                            where xx.Province_ID == provID
                                            select xx).FirstOrDefault();
                        getProvOwner.Money += tempCash;

                        db.SubmitChanges();
                        UtopiaParser.AddThiefOp(OpType.stoleMoney, new Guid(item), tempCash.ToString("N0") + "gc stolen", currentUser, cachedKingdom);
                    }
                }
                RawData = RawData.Replace(mc[i].Value, "");
            }

        }

        private static void thievesKidnapped(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._thievesKidnapped.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                    {
                        UtopiaParser.AddThiefOp(OpType.kidnapped, new Guid(item), URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampFront.Match(matchValue).Value).Value, currentUser, cachedKingdom);

                    }
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "kidnapped; ";
            }

        }

        private static void ThievesAssasinatedWizards(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, CS_Code.UtopiaDataContext db, ref string opsAdded, string[] provinces)
        {
            MatchCollection mc = URegEx._thievesAssasinateWizards.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);
                string tempWiz = URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(matchValue).Value).Value;
                foreach (string item in provinces)
                    if (item.IsValidGuid())
                    {
                        var getProvince = (from xx in db.Utopia_Province_Data_Captured_Gens
                                           where xx.Province_ID == new Guid(item)
                                           select xx).FirstOrDefault();
                        if (getProvince.Wizards > 0)
                            getProvince.Wizards -= Convert.ToInt32(tempWiz.Replace(",", ""));
                        db.SubmitChanges();
                        UtopiaParser.AddThiefOp(OpType.assasinateWizs, new Guid(item), tempWiz, currentUser, cachedKingdom);
                    }
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Assasinated Wizs; ";
            }

        }

        private static void ThievesAssasinated(ref string RawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom, ref string opsAdded)
        {
            MatchCollection mc = URegEx._theivesAssasinates.Matches(RawData);
            for (int i = 0; i < mc.Count; i++)
            {
                string matchValue = GetLostThieves(mc[i].Value, currentUser);

                string ProvinceAttacked = URegEx._findMysticThiefProvinceNameRevamp.Match(matchValue).Value.Replace("enemy troops at", "");
                string provinceName = URegEx.rgxFindIslandLocation.Replace(ProvinceAttacked, "").Trim();
                int sourceLocation = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(ProvinceAttacked).Value).Value).Value);
                int sourceIsland = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(ProvinceAttacked).Value).Value).Value);
                Guid ProvinceID = ProvinceCache.getProvinceID(provinceName, sourceIsland, sourceLocation, currentUser, cachedKingdom);
                UtopiaParser.AddThiefOp(OpType.assasinate, ProvinceID, URegEx.rgxQuantitiesWithComma.Match(URegEx._findMysticThiefQualityRevampBack.Match(matchValue).Value).Value, currentUser, cachedKingdom);
                RawData = RawData.Replace(mc[i].Value, "");
                opsAdded += "Assasination on " + provinceName + "; ";
            }

            //return opsAdded;
        }

    }
}
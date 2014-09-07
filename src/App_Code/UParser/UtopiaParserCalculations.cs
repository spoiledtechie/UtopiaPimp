using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PimpLibrary.Utopia.Players;
using Pimp.UData;
using PimpLibrary.Utopia.Province;
using SupportFramework.Data;

namespace Pimp.UParser
{
    /// <summary>
    /// Summary description for UtopiaParserCalculations
    /// </summary>
    public partial class UtopiaParser
    {

        private static decimal CalcDraftRate(decimal soldiers, decimal off, decimal def, decimal elites, decimal thieves, decimal population)
        {
            return (decimal)((soldiers + off + def + elites + thieves) / (population));
        }

        /// <summary>
        /// AGECHANGECALC
        /// </summary>
        /// <param name="nobilityID"></param>
        /// <param name="prisoners"></param>
        /// <param name="peasants"></param>
        /// <param name="raceID"></param>
        /// <param name="personalityID"></param>
        /// <returns></returns>
        private static int CalcDailyIncome(int nobilityID, int prisoners, long peasants, int raceID, int personalityID)
        {
            var multi = (from r in UtopiaHelper.Instance.Ranks where nobilityID == r.uid select r.income).FirstOrDefault();
            double di = 2.25 * (peasants + (.5 * prisoners)) * (double)(1 + (multi / 100));
            if (raceID == (from r in UtopiaHelper.Instance.Races where r.name == "Human" select r.uid).FirstOrDefault())
                di *= 1.2;
            if (nobilityID == (from r in UtopiaHelper.Instance.Ranks where r.name == "King" select r.uid).FirstOrDefault() | nobilityID == (from r in UtopiaHelper.Instance.Ranks where r.name == "Queen" select r.uid).FirstOrDefault())
                di *= 1.1;
            if (personalityID == (from r in UtopiaHelper.Instance.Personalities where r.name == "Merchant" select r.uid).FirstOrDefault())
                di *= 1.3;

            return (int)di;
        }



        //private static decimal? GetModOffense(Race GetMilitaryNames, int? soldiers_Elites, int? soldiers_Regs_Off, int? soldiers, int? prisoners, decimal? mil_Overall_Efficiency, decimal stanceID)
        //{
        //    var stance = GetKingdomStances.Where(x => x.uid == (int)stanceID).FirstOrDefault();
        //    stanceID = 1;
        //    if (stance != null)
        //        if (stance.name.ToLower() == "fortified")
        //            stanceID = (decimal).85;
        //    return ((GetMilitaryNames.eliteOffMulitplier * soldiers_Elites) + soldiers + (soldiers_Regs_Off * GetMilitaryNames.soldierOffMultiplier) + (prisoners * 3)) * (mil_Overall_Efficiency.GetValueOrDefault(100) / 100) * (decimal)1.09 * stanceID;
        //}
        //private static decimal? GetModDefense(Race GetMilitaryNames, int? soldiers_Elites, int? soldiers_Regs_Def, int? soldiers, int? prisoners, decimal? mil_Overall_Efficiency, decimal stanceID)
        //{
        //    //Raw Defense = (Defense Specs * Def Spec Points) +                 + (Soldiers * Def Points Sold * Aggression) + Townwatch
        //    //MAX( Raw Defense * Defensive Military Efficiency, Land) * Monarchy Bonus[Hostile/War] * Stance
        //    var stance = GetKingdomStances.Where(x => x.uid == (int)stanceID).FirstOrDefault();
        //    stanceID = 1;
        //    if (stance != null)
        //        if (stance.name.ToLower() == "fortified")
        //            stanceID = (decimal)1.1;
        //    return ((GetMilitaryNames.eliteDefMulitplier * soldiers_Elites) + soldiers + (soldiers_Regs_Def * GetMilitaryNames.soldierDefMultiplier)) * (mil_Overall_Efficiency.GetValueOrDefault(100) / 100) * stanceID;
        //}
        private static decimal? GetPracDefense(Race GetMilitaryNames, int? soldiers_Regs_Def, int? soldiers, decimal? mil_Overall_Efficiency, decimal stanceID)
        {
            //Raw Defense = (Defense Specs * Def Spec Points) + (Elites at Home* Elite's Defense)                 + (Soldiers * Def Points Sold * Aggression) + Townwatch
            //MAX( Raw Defense * Defensive Military Efficiency, Land) * Monarchy Bonus[Hostile/War] * Stance
            var stance = UtopiaHelper.Instance.KingdomStances.Where(x => x.uid == (int)stanceID).FirstOrDefault();
            stanceID = 1;
            if (stance != null)
                if (stance.name.ToLower() == "fortified")
                    stanceID = (decimal)1.1;
            return (soldiers + (soldiers_Regs_Def * GetMilitaryNames.soldierDefMultiplier)) * (mil_Overall_Efficiency.GetValueOrDefault(100) / 100) * stanceID;
        }
        public static double CalcRawDefense(int raceID, int soldiers, int defensiveUnits, long peasents)
        {
            var race = UtopiaHelper.Instance.Races.Where(x => x.uid == raceID).FirstOrDefault();

            if (race != null) //if race is null then use the default value
                return soldiers + defensiveUnits * race.soldierDefMultiplier + peasents / 4;
            return soldiers + defensiveUnits * 5 + peasents / 4;
        }
        public static double CalcRawDefense(int raceID, int soldiers, int defensiveUnits, int eliteUnits, long peasents)
        {
            var race = UtopiaHelper.Instance.Races.Where(x => x.uid == raceID).FirstOrDefault();
            if (race != null)//if it can't find the race type
                return soldiers + defensiveUnits * race.soldierDefMultiplier + eliteUnits * race.eliteDefMulitplier + peasents / 4;
            else
                return soldiers + defensiveUnits * 5 + peasents / 4;
        }
        public static double CalcDefensiveMilEfficiency(string stance, double rawMilitaryEfficency, double buildingEfficiency, bool monarch, bool rubyDragon, bool fanaticism, bool plague, bool protectioned, int forts)
        {
            double c = (rawMilitaryEfficency);
            if (monarch)
                c = c * (1 + 10 / 100);
            if (rubyDragon)
                c = c * (1 - 8 / 100);
            if (fanaticism)
                c = c * (1 - 3 / 100);
            if (plague)
                c = c * (1 - 15 / 100);
            if (stance == "fortified")
                c = c * 1.1;

            if (forts > 50)
                forts = 50;

            double ef = 1.5 * (forts - forts * forts / 100) * buildingEfficiency / 100;
            c = c * (1 + ef / 100);

            return c;
        }
        public static double CalcModDefense(double enemyRawDefense, double defensiveMilEfficiency)
        {
            return enemyRawDefense * defensiveMilEfficiency / 100;
        }
        public static double CalcRawOffense(int raceID, int soldiers, bool aggressive, int offensiveUnits, int eliteUnits, int horses, int mercenaries, int prisoners)
        {
            int aggressiveBonus = 1;
            if (aggressive)
                aggressiveBonus = 2;

            var race = UtopiaHelper.Instance.Races.Where(x => x.uid == raceID).FirstOrDefault();

            return (soldiers * aggressiveBonus) + (offensiveUnits * race.soldierOffMultiplier) + (eliteUnits * race.eliteOffMulitplier) + CalcOptimizedHorses(horses, soldiers, offensiveUnits, eliteUnits) + (CalcOptimizedMercenaries(prisoners, soldiers, offensiveUnits, eliteUnits, mercenaries) * 3) + (CalcOptimizedPrisoners(prisoners, soldiers, offensiveUnits, eliteUnits, mercenaries) * 3);
        }
        public static double CalcRawOffense(int raceID, int soldiers, bool aggressive, int offensiveUnits, int horses, int mercenaries, int prisoners)
        {
            try
            {
                int aggressiveBonus = 1;
                if (aggressive)
                    aggressiveBonus = 2;

                int raceMult = 5;
                var race = UtopiaHelper.Instance.Races.Where(x => x.uid == raceID).FirstOrDefault();
                if (race != null)
                    raceMult = race.soldierOffMultiplier;
                var horsesopt = CalcOptimizedHorses(horses, soldiers, offensiveUnits);
                var optMercs = CalcOptimizedMercenaries(prisoners, soldiers, offensiveUnits, mercenaries);
                var optPrisoner = CalcOptimizedPrisoners(prisoners, soldiers, offensiveUnits, mercenaries);
                return (soldiers * aggressiveBonus) + (offensiveUnits * raceMult) + horsesopt + (optMercs * 3) + (optPrisoner * 3);
            }
            catch (Exception e)
            {
                Errors.logError(e);
                return 0;
            }
        }
        public static double CalcModifiedOffense(double rawOffense, int generals, string stance, double offensiveMilEfficiency)
        {
            //Raw Offense * OME * General Bonus * Stance
            double bonus = 1;
            switch (generals)
            {
                case 5:
                case 4:
                    bonus = 1.09;
                    break;
                case 3:
                    bonus = 1.06;
                    break;
                case 2:
                    bonus = 1.03;
                    break;
            }
            if (!String.IsNullOrEmpty(stance) && stance.ToLower() == "fortified")
                bonus = bonus * .85;
            if (offensiveMilEfficiency == 0.0)
                offensiveMilEfficiency = 1;
            if (offensiveMilEfficiency > 80)
                offensiveMilEfficiency = offensiveMilEfficiency / 100;
            return bonus * offensiveMilEfficiency * rawOffense;
        }
        public static int CalcOptimizedHorses(int horses, int soldiers, int offensiveUnits, int eliteUnits)
        {
            var horsesTotal = soldiers + offensiveUnits + eliteUnits;
            if (horses > horsesTotal)
                return horsesTotal;
            else
                return horses;
        }
        public static int CalcOptimizedHorses(int horses, int soldiers, int offensiveUnits)
        {
            var horsesTotal = soldiers + offensiveUnits;
            if (horses > horsesTotal)
                return horsesTotal;
            else
                return horses;
        }
        /// <summary>
        /// See If the Max Prisoners are greater than the army.
        /// Cannot send more prisoners than you can army.
        /// </summary>
        /// <param name="prisoners"></param>
        /// <param name="soidlers"></param>
        /// <param name="offensiveUnits"></param>
        /// <param name="eliteUnits"></param>
        /// <param name="mercenaries"></param>
        /// <returns></returns>
        private static int CalcOptimizedMercenaries(int prisoners, int soidlers, int offensiveUnits, int eliteUnits, int mercenaries)
        {
            var optimize = ((soidlers + offensiveUnits + eliteUnits) / 5) - prisoners;

            if (optimize < 0)
                optimize = 0;

            if (mercenaries != optimize)
                return optimize;
            else
                return mercenaries;
        }
        private static int CalcOptimizedMercenaries(int prisoners, int soidlers, int offensiveUnits, int mercenaries)
        {
            var optimize = ((soidlers + offensiveUnits) / 5) - prisoners;

            if (optimize < 0)
                optimize = 0;

            if (mercenaries != optimize)
                return optimize;
            else
                return mercenaries;
        }
        /// <summary>
        /// See If the Max Mercenaries are greater than the army.
        /// Cannot send more merc than you can army.
        /// </summary>
        /// <param name="prisoners"></param>
        /// <param name="soidlers"></param>
        /// <param name="offensiveUnits"></param>
        /// <param name="eliteUnits"></param>
        /// <param name="mercenaries"></param>
        /// <returns></returns>
        public static int CalcOptimizedPrisoners(int prisoners, int soidlers, int offensiveUnits, int eliteUnits, int mercenaries)
        {
            var optimize = ((soidlers + offensiveUnits + eliteUnits) / 5) - mercenaries;

            if (optimize < 0)
                optimize = 0;

            if (prisoners != optimize)
                return optimize;
            else
                return prisoners;
        }
        public static int CalcOptimizedPrisoners(int prisoners, int soidlers, int offensiveUnits, int mercenaries)
        {
            var optimize = ((soidlers + offensiveUnits) / 5) - mercenaries;

            if (optimize < 0)
                optimize = 0;

            if (prisoners != optimize)
                return optimize;
            else
                return prisoners;
        }
        public static int CalcRawMilitaryEfficiency(bool war, int stance, int rawMilEfficiency)
        {
            if ((stance == 2) && (!war))
                return rawMilEfficiency - 15;
            else
                return rawMilEfficiency;
        }
        public static double CalcTrainingGroundBonus(int trainingGrounds, double buildingEfficency)
        {
            if (trainingGrounds > 50)
                trainingGrounds = 50;
            return 1.5 * trainingGrounds * (1 - trainingGrounds / 100) * buildingEfficency / 100;
        }
        public static int CalcRankBonus(int rank, string race)
        {
            if (rank > 8)
                return 2;
            if (rank > 4)
                return rank * 4 - 10;

            return rank * 2 - 2;
            //if ( Your_Race == 3 ) Your_Rank_Bonus = Math.round( Your_Rank_Bonus / 2 );
        }
        public static double CalcMinimumOffToWin(double modDefense)
        {
            return modDefense * 1.040362694;
        }
        public static double CalcOffensiveMilEffciency(double rawMilEfficiency, int generals, double trainingGroundBonus, int rankBonus, bool rubyDragon, bool fanatacism)
        {
            double c = (rawMilEfficiency) * (1 + generals / 100) * (1 + trainingGroundBonus / 100) * (1 + rankBonus / 100);
            if (rubyDragon)
                c = c * (1 - 8 / 100); //8=rubydragon
            if (fanatacism)
                c = c * (1 + 5 / 100); // 5for fanatacism
            return c;
        }
        /// <summary>
        /// Calculates the Regular Offensive Points for Military
        /// </summary>
        /// <param name="regs"></param>
        /// <param name="milOffEfficieny"></param>
        /// <param name="raceID"></param>
        /// <returns></returns>
        public static decimal CalcOffRegPoints(decimal regs, decimal milOffEfficieny, int raceID)
        {
            return regs * milOffEfficieny / 100 * UtopiaHelper.Instance.Races.Where(x => x.uid == raceID).Select(x => x.soldierOffMultiplier).FirstOrDefault();
        }
        public static decimal CalcDefRegPoints(decimal regs, decimal milDefEfficiency, int raceID)
        {
            return regs * milDefEfficiency / 100 * UtopiaHelper.Instance.Races.Where(x => x.uid == raceID).Select(x => x.soldierDefMultiplier).FirstOrDefault();
        }
        public static decimal CalcOffSoldierPoints(decimal soldiers, decimal milOffEfficieny)
        {
            return soldiers * milOffEfficieny / 100;
        }
        public static decimal CalcDefSoldierPoints(decimal soldiers, decimal milDefEfficiency)
        {
            return soldiers * milDefEfficiency / 100;
        }
        public static decimal CalcHorsePoints(decimal horses, decimal milOffEfficieny)
        {
            return horses * milOffEfficieny / 100;
        }
        public static decimal CalcOffElitePoints(decimal elites, decimal milOffEfficieny, int raceID)
        {
            return elites * milOffEfficieny / 100 * UtopiaHelper.Instance.Races.Where(x => x.uid == raceID).Select(x => x.eliteOffMulitplier).FirstOrDefault();
        }
        public static decimal CalcDefElitePoints(decimal elites, decimal milDefEfficiency, int raceID)
        {
            return elites * milDefEfficiency / 100 * UtopiaHelper.Instance.Races.Where(x => x.uid == raceID).Select(x => x.eliteDefMulitplier).FirstOrDefault();
        }
    }
}
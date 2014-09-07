using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using Pimp.UParser;
using Pimp.Utopia;
using Pimp.Users;
using SupportFramework.Data;

namespace Pimp.UIBuilder
{
    /// <summary>
    /// Summary description for MilitaryKdPage
    /// </summary>
    public class MilitaryKdPage
    {
        public static void displayModDefNwColumn(StringBuilder sb, bool milCheck, bool cbCheck, ProvinceClass item, CS_Code.Utopia_Province_Data_Captured_CB cb, List<CS_Code.Utopia_Province_Data_Captured_Type_Military> getMils)
        {
            try
            {
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
                                var tempMil = getMils.Where(x => x.Military_Location == 1).LastOrDefault();
                                var modDef = UtopiaParser.CalcModDefense(UtopiaParser.CalcRawDefense(item.Race_ID.GetValueOrDefault(0), (int)tempMil.Soldiers.GetValueOrDefault(0), (int)tempMil.Regs_Def.GetValueOrDefault(0), (int)tempMil.Elites.GetValueOrDefault(0), item.Peasents.GetValueOrDefault()), (double)tempMil.Efficiency_Def.GetValueOrDefault());
                                sb.Append("<span title=\"Mod Def at Home\" style=\"color:Red;\">" + (modDef / item.Networth.GetValueOrDefault(1)).ToString("N1") + "</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                switch (cbCheck)
                                {
                                    case true:
                                        sb.Append((cb.Total_Mod_Defense.GetValueOrDefault(1) / item.Networth.GetValueOrDefault(1)).ToString("N1"));
                                        break;
                                    default:
                                        sb.Append("-");
                                        break;
                                }
                                break;
                            default:
                                var tempMil1 = getMils.Where(x => x.Military_Location == 1).LastOrDefault();
                                if (tempMil1 != null)
                                {
                                    var raceId = item.Race_ID.GetValueOrDefault(0);
                                    var sold = (int)tempMil1.Soldiers.GetValueOrDefault(0);
                                    var regDef = (int)tempMil1.Regs_Def.GetValueOrDefault(0);
                                    var elites = (int)tempMil1.Elites.GetValueOrDefault(0);
                                    var pessy = item.Peasents.GetValueOrDefault();
                                    var effi = (double)tempMil1.Efficiency_Def.GetValueOrDefault();
                                    var modDef1 = UtopiaParser.CalcModDefense(UtopiaParser.CalcRawDefense(raceId, sold, regDef, elites, pessy), effi);

                                    switch (cbCheck) //if cb exists, check if the last som or last cb is newer.  use the newest information.
                                    {
                                        case true:
                                            if (tempMil1.DateTime_Added > cb.Updated_By_DateTime)
                                                sb.Append((modDef1 / item.Networth.GetValueOrDefault(1)).ToString("N1"));
                                            else
                                                sb.Append((cb.Total_Mod_Defense.GetValueOrDefault(1) / item.Networth.GetValueOrDefault(1)).ToString("N1"));
                                            break;
                                        default://if cb doesn't exist, just use the soms data.
                                            sb.Append((modDef1 / item.Networth.GetValueOrDefault(1)).ToString("N1"));
                                            break;
                                    }
                                }
                                else
                                    sb.Append("-");
                                break;
                        }
                        break;
                    default:
                        switch (cbCheck)
                        {
                            case true:
                                sb.Append((cb.Total_Mod_Defense.GetValueOrDefault(1) / item.Networth.GetValueOrDefault(1)).ToString("N1"));
                                break;
                            default:
                                sb.Append("-");
                                break;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Errors.logError(e);
            }
        }


        /// <summary>
        /// Mod off (opa) no elites
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="milCheck"></param>
        /// <param name="cbCheck"></param>
        /// <param name="item"></param>
        /// <param name="cb"></param>
        /// <param name="getMils"></param>
        public static void DisplayModOffNoElitesColumn(StringBuilder sb, bool milCheck, bool cbCheck, ProvinceClass item, CS_Code.Utopia_Province_Data_Captured_CB cb, List<CS_Code.Utopia_Province_Data_Captured_Type_Military> getMils)
        {
            try
            {
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
                                var m = getMils.Where(x => x.Military_Location == 1).LastOrDefault();
                                var race1 = item.Race_ID.GetValueOrDefault();
                                var sold1 = m.Soldiers.GetValueOrDefault();
                                var regsOff1 = (int)m.Regs_Off.GetValueOrDefault();
                                var horses1 = (int)m.Horses.GetValueOrDefault();
                                var effi1 = (double)m.Efficiency_Off.GetValueOrDefault();
                                var gens1 = m.Generals.GetValueOrDefault();
                                var prisoners1 = (int)m.Prisoners.GetValueOrDefault();
                                var modOff = UtopiaParser.CalcModifiedOffense(UtopiaParser.CalcRawOffense(race1, sold1, false, regsOff1, horses1, 0, prisoners1), gens1, "Normal", effi1);
                                sb.Append("<span title=\"Max Mod Off at Home\" style=\"color:Red;\">" + modOff.ToString("N0") + " (" + (modOff / (double)item.Land.GetValueOrDefault(1)).ToString("N0") + ")</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                switch (cbCheck)
                                {
                                    case true:
                                        var modOff1 = UtopiaParser.CalcModifiedOffense(UtopiaParser.CalcRawOffense(item.Race_ID.GetValueOrDefault(), cb.Soldiers.GetValueOrDefault(), false, (int)cb.Soldiers_Regs_Off.GetValueOrDefault(), (int)cb.War_Horses.GetValueOrDefault(), 0, (int)cb.Prisoners.GetValueOrDefault()), 4, "Normal", (double)item.Military_Efficiency_Off.GetValueOrDefault());
                                        sb.Append(modOff1.ToString("N0") + " (" + (modOff1 / (double)item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                        break;
                                    default:
                                        sb.Append("-");
                                        break;
                                }
                                break;
                            default:
                                var m2 = getMils.Where(x => x.Military_Location == 1).LastOrDefault();
                                if (m2 != null)
                                {
                                    var race = item.Race_ID.GetValueOrDefault();
                                    var soldiers = m2.Soldiers.GetValueOrDefault();
                                    var regsOff = (int)m2.Regs_Off.GetValueOrDefault();
                                    var horses = (int)m2.Horses.GetValueOrDefault();
                                    var prisoners = (int)m2.Prisoners.GetValueOrDefault();
                                    var generals = m2.Generals.GetValueOrDefault();
                                    var effOff = (double)m2.Efficiency_Off.GetValueOrDefault();
                                    var modOff2 = UtopiaParser.CalcModifiedOffense(UtopiaParser.CalcRawOffense(race, soldiers, false, regsOff, horses, 0, prisoners), generals, "Normal", effOff);
                                    switch (cbCheck) //if cb exists, check if the last som or last cb is newer.  use the newest information.
                                    {
                                        case true:
                                            if (getMils.Where(x => x.Military_Location == 1).Select(x => x.DateTime_Added).LastOrDefault() > cb.Updated_By_DateTime)
                                                sb.Append(modOff2.ToString("N0") + " (" + (modOff2 / (double)item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                            else
                                            {
                                                var modOff1 = UtopiaParser.CalcModifiedOffense(UtopiaParser.CalcRawOffense(item.Race_ID.GetValueOrDefault(), cb.Soldiers.GetValueOrDefault(), false, (int)cb.Soldiers_Regs_Off.GetValueOrDefault(), (int)cb.War_Horses.GetValueOrDefault(), 0, (int)cb.Prisoners.GetValueOrDefault()), 4, "Normal", (double)item.Military_Efficiency_Off.GetValueOrDefault());
                                                sb.Append(modOff1.ToString("N0") + " (" + (modOff1 / (double)item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                            } break;
                                        default://if cb doesn't exist, just use the soms data.
                                            sb.Append(modOff2.ToString("N0") + " (" + (modOff2 / (double)item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                            break;
                                    }
                                }
                                else
                                    sb.Append("-");
                                break;
                        }
                        break;
                    default:
                        switch (cbCheck)
                        {
                            case true:
                                var modOff1 = UtopiaParser.CalcModifiedOffense(UtopiaParser.CalcRawOffense(item.Race_ID.GetValueOrDefault(), cb.Soldiers.GetValueOrDefault(), false, (int)cb.Soldiers_Regs_Off.GetValueOrDefault(), (int)cb.War_Horses.GetValueOrDefault(), 0, (int)cb.Prisoners.GetValueOrDefault()), 4, "Normal", (double)item.Military_Efficiency_Off.GetValueOrDefault());
                                sb.Append(modOff1.ToString("N0") + " (" + (modOff1 / (double)item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                break;
                            default:
                                sb.Append("-");
                                break;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                sb.Append("-");
                Errors.logError(e);
            }
        }


        public static void displayPracModDefColumn(StringBuilder sb, bool milCheck, bool cbCheck, ProvinceClass item, CS_Code.Utopia_Province_Data_Captured_CB cb, List<CS_Code.Utopia_Province_Data_Captured_Type_Military> getMils)
        {
            try
            {
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
                                var tempMil = getMils.Where(x => x.Military_Location == 1).LastOrDefault();
                                var modDef = UtopiaParser.CalcModDefense(UtopiaParser.CalcRawDefense(item.Race_ID.GetValueOrDefault(0), (int)tempMil.Soldiers.GetValueOrDefault(0), (int)tempMil.Regs_Def.GetValueOrDefault(0), item.Peasents.GetValueOrDefault()), (double)tempMil.Efficiency_Def.GetValueOrDefault());
                                sb.Append("<span title=\"Mod Def at Home\" style=\"color:Red;\">" + modDef.ToString("N0") + "</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                switch (cbCheck)
                                {
                                    case true:
                                        sb.Append(UtopiaParser.CalcModDefense(UtopiaParser.CalcRawDefense(item.Race_ID.GetValueOrDefault(0), (int)cb.Soldiers.GetValueOrDefault(0), (int)cb.Soldiers_Regs_Def.GetValueOrDefault(0), cb.Peasents.GetValueOrDefault()), (double)item.Military_Efficiency_Def.GetValueOrDefault()).ToString("N0"));
                                        break;
                                    default:
                                        sb.Append("-");
                                        break;
                                }
                                break;
                            default:
                                var tempMil1 = getMils.Where(x => x.Military_Location == 1).LastOrDefault();
                                if (tempMil1 != null)
                                {
                                    var modDef1 = UtopiaParser.CalcModDefense(UtopiaParser.CalcRawDefense(item.Race_ID.GetValueOrDefault(0), (int)tempMil1.Soldiers.GetValueOrDefault(0), (int)tempMil1.Regs_Def.GetValueOrDefault(0), item.Peasents.GetValueOrDefault()), (double)tempMil1.Efficiency_Def.GetValueOrDefault());

                                    switch (cbCheck) //if cb exists, check if the last som or last cb is newer.  use the newest information.
                                    {
                                        case true:
                                            if (tempMil1.DateTime_Added > cb.Updated_By_DateTime)
                                                sb.Append(modDef1.ToString("N0"));
                                            else
                                                sb.Append(UtopiaParser.CalcModDefense(UtopiaParser.CalcRawDefense(item.Race_ID.GetValueOrDefault(0), (int)cb.Soldiers.GetValueOrDefault(0), (int)cb.Soldiers_Regs_Def.GetValueOrDefault(0), cb.Peasents.GetValueOrDefault()), (double)item.Military_Efficiency_Def.GetValueOrDefault()).ToString("N0"));
                                            break;
                                        default://if cb doesn't exist, just use the soms data.
                                            sb.Append((modDef1 / item.Land.GetValueOrDefault(1)).ToString("N0"));
                                            break;
                                    }
                                }
                                else
                                {
                                    sb.Append("-");
                                }

                                break;
                        }
                        break;
                    default:
                        switch (cbCheck)
                        {
                            case true:
                                sb.Append(UtopiaParser.CalcModDefense(UtopiaParser.CalcRawDefense(item.Race_ID.GetValueOrDefault(0), (int)cb.Soldiers.GetValueOrDefault(0), (int)cb.Soldiers_Regs_Def.GetValueOrDefault(0), cb.Peasents.GetValueOrDefault()), (double)item.Military_Efficiency_Def.GetValueOrDefault()).ToString("N0"));
                                break;
                            default:
                                sb.Append("-");
                                break;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                sb.Append("-");
                Errors.logError(e);
            }
        }


        /// <summary>
        /// dispalys the mod off column for the kd page.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="milCheck"></param>
        /// <param name="cbCheck"></param>
        /// <param name="cb"></param>
        /// <param name="getMils"></param>
        public static void DisplayModOffColumn(StringBuilder sb, bool milCheck, bool cbCheck, CS_Code.Utopia_Province_Data_Captured_CB cb, List<CS_Code.Utopia_Province_Data_Captured_Type_Military> getMils)
        {
            try
            {
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
                                sb.Append("<span title=\"Mod Off at Home\" style=\"color:Red;\">" + getMils.Where(x => x.Military_Location == 1).Select(x => x.Net_Offense_Pts_Home).LastOrDefault().GetValueOrDefault(1).ToString("N0") + "</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                switch (cbCheck)
                                {
                                    case true:
                                        sb.Append(cb.Total_Mod_Offense.GetValueOrDefault(1).ToString("N0"));
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
                                            sb.Append(getMils.Where(x => x.Military_Location == 1).Select(x => x.Net_Offense_Pts_Home).LastOrDefault().GetValueOrDefault(1).ToString("N0"));
                                        else
                                            sb.Append(cb.Total_Mod_Offense.GetValueOrDefault(1).ToString("N0"));
                                        break;
                                    default://if cb doesn't exist, just use the soms data.
                                        sb.Append(getMils.Where(x => x.Military_Location == 1).Select(x => x.Net_Offense_Pts_Home).LastOrDefault().GetValueOrDefault(1).ToString("N0"));
                                        break;
                                }
                                break;
                        }
                        break;
                    default:
                        switch (cbCheck)
                        {
                            case true:
                                sb.Append(cb.Total_Prac_Offense.GetValueOrDefault(1).ToString("N0"));
                                break;
                            default:
                                sb.Append("-");
                                break;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                sb.Append("-");
                Errors.logError(e);
            }
        }

        /// <summary>
        /// displays the Mod Def Column on the kd page.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="milCheck"></param>
        /// <param name="cbCheck"></param>
        /// <param name="item"></param>
        /// <param name="cb"></param>
        /// <param name="getMils"></param>
        public static void DisplayModDefColumn(StringBuilder sb, bool milCheck, bool cbCheck, ProvinceClass item, CS_Code.Utopia_Province_Data_Captured_CB cb, List<CS_Code.Utopia_Province_Data_Captured_Type_Military> getMils)
        {
            try
            {
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
                                var tempMil = getMils.Where(x => x.Military_Location == 1).LastOrDefault();
                                if (tempMil != null)
                                {
                                    var modDef = UtopiaParser.CalcModDefense(UtopiaParser.CalcRawDefense(item.Race_ID.GetValueOrDefault(0), (int)tempMil.Soldiers.GetValueOrDefault(0), (int)tempMil.Regs_Def.GetValueOrDefault(0), (int)tempMil.Elites.GetValueOrDefault(0), item.Peasents.GetValueOrDefault()), (double)tempMil.Efficiency_Def.GetValueOrDefault());
                                    sb.Append("<span title=\"Mod Def at Home\" style=\"color:Red;\">" + modDef.ToString("N0") + "</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                    switch (cbCheck)
                                    {
                                        case true:
                                            sb.Append(cb.Total_Mod_Defense.GetValueOrDefault(0).ToString("N0"));
                                            break;
                                        default:
                                            sb.Append("-");
                                            break;
                                    }
                                }
                                else
                                { sb.Append("-"); }
                                break;
                            default:
                                var tempMil1 = getMils.Where(x => x.Military_Location == 1).LastOrDefault();
                                //this happens at the beginning of the age when there is no military at all.
                                if (tempMil1 != null)
                                {
                                    HttpContext.Current.Session["SubmittedData"] = tempMil1;
                                    HttpContext.Current.Session["SubmittedData"] = item;
                                    var raceId = item.Race_ID.GetValueOrDefault(0);
                                    var soldiers = (int)tempMil1.Soldiers.GetValueOrDefault(0);
                                    var regsDef = (int)tempMil1.Regs_Def.GetValueOrDefault(0);
                                    var elites = (int)tempMil1.Elites.GetValueOrDefault(0);
                                    var peasants = item.Peasents.GetValueOrDefault(0);
                                    var milEfficiency = (double)tempMil1.Efficiency_Def.GetValueOrDefault();
                                    var modDef1 = UtopiaParser.CalcModDefense(UtopiaParser.CalcRawDefense(raceId, soldiers, regsDef, elites, peasants), milEfficiency);

                                    switch (cbCheck) //if cb exists, check if the last som or last cb is newer.  use the newest information.
                                    {
                                        case true:
                                            if (tempMil1.DateTime_Added > cb.Updated_By_DateTime)
                                                sb.Append(modDef1.ToString("N0"));
                                            else
                                                sb.Append(cb.Total_Mod_Defense.GetValueOrDefault(0).ToString("N0"));
                                            break;
                                        default://if cb doesn't exist, just use the soms data.
                                            sb.Append(modDef1.ToString("N0"));
                                            break;
                                    }
                                }
                                else
                                { sb.Append("-"); }
                                break;
                        }
                        break;
                    default:
                        switch (cbCheck)
                        {
                            case true:
                                sb.Append(cb.Total_Mod_Defense.GetValueOrDefault(0).ToString("N0"));
                                break;
                            default:
                                sb.Append("-");
                                break;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                sb.Append("-");
                Errors.logError(e);

            }
        }

        /// <summary>
        /// displays the Def dpa Column
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="milCheck"></param>
        /// <param name="cbCheck"></param>
        /// <param name="item"></param>
        /// <param name="cb"></param>
        /// <param name="getMils"></param>
        public static void DisplayDefdpa(StringBuilder sb, bool milCheck, bool cbCheck, ProvinceClass item, CS_Code.Utopia_Province_Data_Captured_CB cb, List<CS_Code.Utopia_Province_Data_Captured_Type_Military> getMils)
        {
            try
            {
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
                                var tempMil = getMils.Where(x => x.Military_Location == 1).LastOrDefault();
                                var land = item.Land.GetValueOrDefault(1);
                                double modDef;
                                if (tempMil != null)
                                    modDef = UtopiaParser.CalcModDefense(UtopiaParser.CalcRawDefense(item.Race_ID.GetValueOrDefault(0), (int)tempMil.Soldiers.GetValueOrDefault(0), (int)tempMil.Regs_Def.GetValueOrDefault(0), (int)tempMil.Elites.GetValueOrDefault(0), item.Peasents.GetValueOrDefault()), (double)tempMil.Efficiency_Def.GetValueOrDefault());
                                else
                                    modDef = UtopiaParser.CalcModDefense(UtopiaParser.CalcRawDefense(item.Race_ID.GetValueOrDefault(0), 0, 0, 0, item.Peasents.GetValueOrDefault()), 100.00);

                                sb.Append("<span title=\"Mod Def at Home\" style=\"color:Red;\">" + modDef.ToString("N0") + " (" + (modDef / land).ToString("N0") + ")</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                switch (cbCheck)
                                {
                                    case true:
                                        sb.Append(cb.Total_Mod_Defense.GetValueOrDefault(0).ToString("N0") + " (" + (cb.Total_Mod_Defense.GetValueOrDefault(1) / item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                        break;
                                    default:
                                        sb.Append("-");
                                        break;
                                }
                                break;
                            default:
                                var tempMil1 = getMils.Where(x => x.Military_Location == 1).LastOrDefault();
                                double modDef1;
                                if (tempMil1 != null)
                                    modDef1 = UtopiaParser.CalcModDefense(UtopiaParser.CalcRawDefense(item.Race_ID.GetValueOrDefault(0), (int)tempMil1.Soldiers.GetValueOrDefault(0), (int)tempMil1.Regs_Def.GetValueOrDefault(0), (int)tempMil1.Elites.GetValueOrDefault(0), item.Peasents.GetValueOrDefault()), (double)tempMil1.Efficiency_Def.GetValueOrDefault());
                                else
                                    modDef1 = UtopiaParser.CalcModDefense(UtopiaParser.CalcRawDefense(item.Race_ID.GetValueOrDefault(0), 0, 0, 0, item.Peasents.GetValueOrDefault()), 100.00);
                                switch (cbCheck) //if cb exists, check if the last som or last cb is newer.  use the newest information.
                                {
                                    case true:
                                        if (tempMil1 != null && tempMil1.DateTime_Added > cb.Updated_By_DateTime)
                                            sb.Append(modDef1.ToString("N0") + " (" + (modDef1 / item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                        else
                                            sb.Append(cb.Total_Mod_Defense.GetValueOrDefault(0).ToString("N0") + " (" + (cb.Total_Mod_Defense.GetValueOrDefault(1) / item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                        break;
                                    default://if cb doesn't exist, just use the soms data.
                                        sb.Append(modDef1.ToString("N0") + " (" + (modDef1 / item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                        break;
                                }
                                break;
                        }
                        break;
                    default:
                        if (cb != null && item.Land.GetValueOrDefault(1) != 0)
                        {
                            sb.Append(cb.Total_Mod_Defense.GetValueOrDefault(0).ToString("N0") + " (" + (cb.Total_Mod_Defense.GetValueOrDefault(1) / item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                        }
                        else
                        {
                            sb.Append("-");
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Errors.logError(e);
                sb.Append("-");
            }
        }

        /// <summary>
        /// display the DefME % for the kingdom page.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="milCheck"></param>
        /// <param name="getMils"></param>
        public static void DisplayDefMEPercentage(StringBuilder sb, bool milCheck, List<CS_Code.Utopia_Province_Data_Captured_Type_Military> getMils)
        {
            try
            {
                switch (milCheck)
                {
                    case true:
                        sb.Append(getMils.Where(x => x.Military_Location == 1).Select(x => x.Efficiency_Def).LastOrDefault().GetValueOrDefault(0).ToString("N1"));
                        break;
                    default:
                        sb.Append("-");
                        break;
                }
            }
            catch (Exception e)
            {
                Errors.logError(e);
                sb.Append("-");
            }
        }

        public static void DisplayMEPercentageColumn(StringBuilder sb, bool milCheck, List<CS_Code.Utopia_Province_Data_Captured_Type_Military> getMils)
        {
            try
            {
                switch (milCheck)
                {
                    case true:
                        var homeMil = getMils.Where(x => x.Military_Location == 1).FirstOrDefault();
                        if (homeMil != null)
                        {
                            var eff = homeMil.Efficiency_Raw.GetValueOrDefault().ToString("N1");
                            sb.Append(eff);
                        }
                        else
                            sb.Append("-");
                        break;
                    default:
                        sb.Append("-");
                        break;
                }
            }
            catch (Exception e)
            {
                Errors.logError(e);
                sb.Append("-");
            }
        }


        /// <summary>
        /// Calculates the modified DPA that has an SOM to use.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="cbCheck"></param>
        /// <param name="item"></param>
        /// <param name="cb"></param>
        /// <param name="getMils"></param>
        public static void DisplayModDPAWithSOM(StringBuilder sb, bool cbCheck, ProvinceClass item, CS_Code.Utopia_Province_Data_Captured_CB cb, List<CS_Code.Utopia_Province_Data_Captured_Type_Military> getMils)
        {
            try
            {
                //checks for militart that is out
                var query = (from xx in getMils
                             where xx.Military_Location == 2
                             where xx.Time_To_Return > DateTime.UtcNow
                             select xx.uid).FirstOrDefault();

                switch (query > 0) //if there is military out
                {
                    case true:
                        var tempMil = getMils.Where(x => x.Military_Location == 1).LastOrDefault();
                        if (tempMil != null)
                        {
                            var race1 = item.Race_ID.GetValueOrDefault(0);
                            var soldiers1 = (int)tempMil.Soldiers.GetValueOrDefault(0);
                            var regsDef1 = (int)tempMil.Regs_Def.GetValueOrDefault(0);
                            var elites1 = (int)tempMil.Elites.GetValueOrDefault(0);
                            var peasants1 = item.Peasents.GetValueOrDefault(1);
                            var efficiency1 = (double)tempMil.Efficiency_Def.GetValueOrDefault(0);
                            var rawDef1 = UtopiaParser.CalcRawDefense(race1, soldiers1, regsDef1, elites1, peasants1);
                            var modDef = UtopiaParser.CalcModDefense(rawDef1, efficiency1);
                            sb.Append("<span title=\"Mod Def at Home\" style=\"color:Red;\">" + (modDef / item.Land.GetValueOrDefault(1)).ToString("N0") + "</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                            switch (cbCheck)
                            {
                                case true:
                                    sb.Append((cb.Total_Mod_Defense.GetValueOrDefault(1) / item.Land.GetValueOrDefault(1)).ToString("N0"));
                                    break;
                                default:
                                    sb.Append("-");
                                    break;
                            }
                        }
                        else
                        {
                            sb.Append("-");
                        }
                        break;
                    default:
                        var tempMil1 = getMils.Where(x => x.Military_Location == 1).LastOrDefault();
                        if (tempMil1 != null)
                        {
                            var raceId = item.Race_ID.GetValueOrDefault(0);
                            var soldiers = (int)tempMil1.Soldiers.GetValueOrDefault(0);
                            var regsDef = (int)tempMil1.Regs_Def.GetValueOrDefault(0);
                            var elities = (int)tempMil1.Elites.GetValueOrDefault(0);
                            var peasants = item.Peasents.GetValueOrDefault();
                            var efffDef = (double)tempMil1.Efficiency_Def.GetValueOrDefault();

                            var modDef1 = UtopiaParser.CalcModDefense(UtopiaParser.CalcRawDefense(raceId, soldiers, regsDef, elities, peasants), efffDef);

                            switch (cbCheck) //if cb exists, check if the last som or last cb is newer.  use the newest information.
                            {
                                case true:
                                    if (tempMil1.DateTime_Added > cb.Updated_By_DateTime)
                                        sb.Append((modDef1 / item.Land.GetValueOrDefault(1)).ToString("N0"));
                                    else
                                        sb.Append((cb.Total_Mod_Defense.GetValueOrDefault(1) / item.Land.GetValueOrDefault(1)).ToString("N0"));
                                    break;
                                default://if cb doesn't exist, just use the soms data.
                                    sb.Append((modDef1 / item.Land.GetValueOrDefault(1)).ToString("N0"));
                                    break;
                            }
                        }
                        else
                            sb.Append("-");
                        break;
                }
            }
            catch (Exception exception)
            {
                Errors.logError(exception);
            }
        }
        /// <summary>
        /// displays practivle mod def (dpa)
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="milCheck"></param>
        /// <param name="cbCheck"></param>
        /// <param name="item"></param>
        /// <param name="cb"></param>
        /// <param name="getMils"></param>
        public static void displayPracModDefdpa(StringBuilder sb, bool milCheck, bool cbCheck, ProvinceClass item, CS_Code.Utopia_Province_Data_Captured_CB cb, List<CS_Code.Utopia_Province_Data_Captured_Type_Military> getMils)
        {
            try
            {
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
                                var tempMil = getMils.Where(x => x.Military_Location == 1).LastOrDefault();
                                if (tempMil != null)
                                {
                                    var modDef = UtopiaParser.CalcModDefense(UtopiaParser.CalcRawDefense(item.Race_ID.GetValueOrDefault(0), (int)tempMil.Soldiers.GetValueOrDefault(0), (int)tempMil.Regs_Def.GetValueOrDefault(0), item.Peasents.GetValueOrDefault()), (double)tempMil.Efficiency_Def.GetValueOrDefault());
                                    sb.Append("<span title=\"Mod Def at Home\" style=\"color:Red;\">" + modDef.ToString("N0") + " (" + (modDef / item.Land.GetValueOrDefault(1)).ToString("N0") + ")</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                    switch (cbCheck)
                                    {
                                        case true:
                                            var modDef2 = UtopiaParser.CalcModDefense(UtopiaParser.CalcRawDefense(item.Race_ID.GetValueOrDefault(0), (int)cb.Soldiers.GetValueOrDefault(0), (int)cb.Soldiers_Regs_Def.GetValueOrDefault(0), cb.Peasents.GetValueOrDefault()), (double)item.Military_Efficiency_Def.GetValueOrDefault());
                                            sb.Append(modDef2.ToString("N0") + " (" + (modDef2 / item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                            break;
                                        default:
                                            sb.Append("-");
                                            break;
                                    }
                                }
                                else { sb.Append("-"); }
                                break;
                            default:
                                var tempMil1 = getMils.Where(x => x.Military_Location == 1).LastOrDefault();
                                if (tempMil1 != null)
                                {
                                    var raceId = item.Race_ID.GetValueOrDefault(0);
                                    var soldiers = (int)tempMil1.Soldiers.GetValueOrDefault(0);
                                    var regsDef = (int)tempMil1.Regs_Def.GetValueOrDefault(0);
                                    var peasants = item.Peasents.GetValueOrDefault();
                                    var milEff = (double)tempMil1.Efficiency_Def.GetValueOrDefault();

                                    var modDef1 = UtopiaParser.CalcModDefense(UtopiaParser.CalcRawDefense(raceId, soldiers, regsDef, peasants), milEff);

                                    switch (cbCheck) //if cb exists, check if the last som or last cb is newer.  use the newest information.
                                    {
                                        case true:
                                            if (tempMil1.DateTime_Added > cb.Updated_By_DateTime)
                                                sb.Append(modDef1.ToString("N0") + " (" + (modDef1 / item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                            else
                                            {
                                                var modDef2 = UtopiaParser.CalcModDefense(UtopiaParser.CalcRawDefense(item.Race_ID.GetValueOrDefault(0), (int)cb.Soldiers.GetValueOrDefault(0), (int)cb.Soldiers_Regs_Def.GetValueOrDefault(0), cb.Peasents.GetValueOrDefault()), (double)item.Military_Efficiency_Def.GetValueOrDefault());
                                                sb.Append(modDef2.ToString("N0") + " (" + (modDef2 / item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                            } break;
                                        default://if cb doesn't exist, just use the soms data.
                                            sb.Append((modDef1 / item.Land.GetValueOrDefault(1)).ToString("N0"));
                                            break;
                                    }
                                }
                                else
                                { sb.Append("-"); }
                                break;
                        }
                        break;
                    default:
                        switch (cbCheck)
                        {
                            case true:
                                var modDef2 = UtopiaParser.CalcModDefense(UtopiaParser.CalcRawDefense(item.Race_ID.GetValueOrDefault(0), (int)cb.Soldiers.GetValueOrDefault(0), (int)cb.Soldiers_Regs_Def.GetValueOrDefault(0), cb.Peasents.GetValueOrDefault()), (double)item.Military_Efficiency_Def.GetValueOrDefault());
                                sb.Append(modDef2.ToString("N0") + " (" + (modDef2 / item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                break;
                            default:
                                sb.Append("-");
                                break;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Errors.logError(e);
                sb.Append("-");
            }
        }


        /// <summary>
        /// displays the Off ME % column
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="milCheck"></param>
        /// <param name="getMils"></param>
        public static void displayOffMePercentageColumn(StringBuilder sb, bool milCheck, List<CS_Code.Utopia_Province_Data_Captured_Type_Military> getMils)
        {
            try
            {
                var mils = getMils.Where(x => x.Military_Location == 1).LastOrDefault();
                if (mils != null)
                    sb.Append(mils.Efficiency_Off.GetValueOrDefault().ToString("N1"));
                else
                    sb.Append("-");
            }
            catch (Exception e)
            {
                Errors.logError(e);
                sb.Append("-");
            }
        }
        public static void displayMaxModOffopaColumn(StringBuilder sb, bool milCheck, bool cbCheck, ProvinceClass item, CS_Code.Utopia_Province_Data_Captured_CB cb, List<CS_Code.Utopia_Province_Data_Captured_Type_Military> getMils)
        {
            try
            {
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
                                var m = getMils.Where(x => x.Military_Location == 1).Select(x => x.Net_Offense_Pts_Home).LastOrDefault().GetValueOrDefault(1);
                                sb.Append("<span title=\"Max Mod Off at Home\" style=\"color:Red;\">" + m.ToString("N0") + " (" + (m / (decimal)item.Land.GetValueOrDefault(1)).ToString("N0") + ")</span><img src=\"" + ImagesStatic.ElitesOut + "\" /> ");
                                switch (cbCheck)
                                {
                                    case true:
                                        sb.Append(cb.Total_Mod_Offense.GetValueOrDefault(1).ToString("N0") + " (" + (cb.Total_Mod_Offense.GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                        break;
                                    default:
                                        sb.Append("-");
                                        break;
                                }
                                break;
                            default:
                                var m1 = getMils.Where(x => x.Military_Location == 1).Select(x => x.Net_Offense_Pts_Home).LastOrDefault().GetValueOrDefault(1);
                                switch (cbCheck) //if cb exists, check if the last som or last cb is newer.  use the newest information.
                                {
                                    case true:
                                        if (getMils.Where(x => x.Military_Location == 1).Select(x => x.DateTime_Added).LastOrDefault() > cb.Updated_By_DateTime)
                                            sb.Append(m1.ToString("N0") + " (" + (m1 / (decimal)item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                        else
                                            sb.Append(cb.Total_Mod_Offense.GetValueOrDefault(1).ToString("N0") + " (" + (cb.Total_Mod_Offense.GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                        break;
                                    default://if cb doesn't exist, just use the soms data.
                                        sb.Append(m1.ToString("N0") + " (" + (m1 / (decimal)item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                        break;
                                }
                                break;
                        }
                        break;
                    default:
                        switch (cbCheck)
                        {
                            case true:
                                if (item.Land.GetValueOrDefault(1) == 0)
                                    sb.Append(cb.Total_Mod_Offense.GetValueOrDefault(1).ToString("N0") + " (" + (cb.Total_Mod_Offense.GetValueOrDefault(1) / 1).ToString("N0") + ")");
                                else
                                    sb.Append(cb.Total_Mod_Offense.GetValueOrDefault(1).ToString("N0") + " (" + (cb.Total_Mod_Offense.GetValueOrDefault(1) / (decimal)item.Land.GetValueOrDefault(1)).ToString("N0") + ")");
                                break;
                            default:
                                sb.Append("-");
                                break;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Errors.logError(e);
                sb.Append("-");
            }
        }

    }

}
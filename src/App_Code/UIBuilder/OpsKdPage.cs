using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Pimp.Utopia;
using Pimp.UData;
using SupportFramework.Data;

namespace Pimp.UIBuilder
{
    /// <summary>
    /// Summary description for OpsKdPage
    /// </summary>
    public class OpsKdPage
    {
        public static void displayModTpaColumn(StringBuilder sbKdPage, bool buildingCheck, bool scienceCheck, ProvinceClass item, CS_Code.Utopia_Province_Data_Captured_Survey getBuildings, CS_Code.Utopia_Province_Data_Captured_Science getSciences)
        {
            try
            {
                StringBuilder sb = new StringBuilder();


                decimal TPA = 0;
                decimal land = (decimal)item.Land.GetValueOrDefault(1);
                if (land == 0)
                    land = 1;
                if (item.Thieves.GetValueOrDefault(0) != 0)
                    TPA = ((decimal)item.Thieves.Value / land);
                var raceTPA = (from r in UtopiaHelper.Instance.Races where item.Race_ID == r.uid select r.name).FirstOrDefault();
                sb.Append(" title=\"(T/Acres)");
                switch (raceTPA) //AGECHANGECALC
                {
                    case "Gnome":
                        TPA *= (decimal)1.4;
                        sb.Append("*Gnome(1.4)");
                        break;
                    case "Human":
                        TPA *= (decimal)1.20;
                        sb.Append("*Human(1.20)");
                        break;
                    //case "Halfling":
                    //    TPA *= (decimal)1.40;
                    //    sb.Append("*Halfling(1.40)");
                    //    break;
                    case "Dwarf":
                        TPA *= (decimal).8;
                        sb.Append("*Dwarf(.8)");
                        break;
                    case "Orc":
                        TPA *= (decimal).8;
                        sb.Append("*Orc(.8)");
                        break;
                }
                switch (scienceCheck)
                {
                    case true:
                        decimal sciTPA = (decimal)(getSciences.SOS_Thieves_Percent.GetValueOrDefault(0) / 100) + 1;
                        sb.Append("*Sci(" + sciTPA + ")");
                        TPA = TPA * sciTPA;
                        break;
                }
                switch (buildingCheck)
                {
                    case true:
                        switch (getBuildings.TD_B.GetValueOrDefault(0))
                        {
                            case 0:
                                sb.Append("*TD(0)");
                                break;
                            default:
                                decimal builDens = (decimal)getBuildings.TD_B.GetValueOrDefault(0) / (decimal)item.Land.GetValueOrDefault(1);
                                builDens = (decimal)(builDens * (1 - builDens) * 3) + 1;
                                sb.Append("*TD(" + builDens.ToString("N2") + ")");
                                TPA = TPA * builDens;
                                break;
                        }
                        break;
                }
                string tpa = (TPA * (from r in UtopiaHelper.Instance.Ranks where item.Nobility_ID == r.uid select r.tpaMulti).FirstOrDefault()).ToString("N1");
                switch (tpa)
                {
                    case "0.0":
                        switch (TPA.ToString("N1"))
                        {
                            case "0.0":
                                sb.Append(" \">-");
                                break;
                            default:
                                switch (item.Thieves_Value_Type.GetValueOrDefault())
                                {
                                    case 1:
                                        sb.Append(" (direct value from CB)\">" + TPA.ToString("N1"));
                                        break;
                                    case 2:
                                        sb.Append(" (Taken from an Infiltration)\"><u>" + TPA.ToString("N1") + "i</u>");
                                        break;
                                    case 3:
                                    case 4:
                                    default:
                                        sb.Append(" (Guesses taken from Angel/Raw pages)\"><u>" + TPA.ToString("N1") + "g</u>");
                                        break;
                                }
                                break;
                        }

                        break;
                    default:
                        switch (item.Thieves_Value_Type.GetValueOrDefault())
                        {
                            case 1:
                                sb.Append("*Honor(" + (from r in UtopiaHelper.Instance.Ranks where item.Nobility_ID == r.uid select r.tpaMulti).FirstOrDefault() + ") (direct value from CB)\">" + tpa);
                                break;
                            case 2:
                                sb.Append("*Honor(" + (from r in UtopiaHelper.Instance.Ranks where item.Nobility_ID == r.uid select r.tpaMulti).FirstOrDefault() + ") (Taken from an Infiltration)\"><u>" + tpa + "</u>");
                                break;
                            case 3:
                            case 4:
                            default:
                                sb.Append("*Honor(" + (from r in UtopiaHelper.Instance.Ranks where item.Nobility_ID == r.uid select r.tpaMulti).FirstOrDefault() + ") (Guesses taken from Angel/Raw pages)\"><u>" + tpa + "</u>");
                                break;
                        }
                        break;
                }
                sbKdPage.Remove(sbKdPage.Length - 1, 1);
                sbKdPage.Append(sb);
            }
            catch (Exception e)
            {
                sbKdPage.Append(" title=\"Thieves/Land\">-");
                Errors.logError(e);
            }
        }


        public static void displayEstWpaColumn(StringBuilder sb, ProvinceClass item)
        {
            try
            {
                sb.Remove(sb.Length - 1, 1);
                if ((decimal)item.Land.GetValueOrDefault(1) != 0)
                {
                    decimal ewpa = (decimal)item.Wizards.GetValueOrDefault(0) / (decimal)item.Land.GetValueOrDefault(1);
                    switch (ewpa.ToString())
                    {
                        case "0":
                            sb.Append(" title=\"Wizards/Land\">-");
                            break;
                        default:
                            sb.Append(" title=\"Wizards/Land " + item.Wizards.GetValueOrDefault(0) + "/" + item.Land.GetValueOrDefault(1));
                            switch (item.Wizards_Value_Type.GetValueOrDefault())
                            {
                                case 1:
                                case 2:
                                    sb.Append(" (direct value from CB)\">" + ewpa.ToString("N1"));
                                    break;
                                case 3:
                                case 4:
                                default:
                                    sb.Append(" (Guesses taken from Angel/Raw pages)\"><u>" + ewpa.ToString("N1") + "g</u>");
                                    break;
                            }
                            break;
                    }
                }
                else { sb.Append(" title=\"Wizards/Land\">-"); }
            }
            catch (Exception e)
            {
                sb.Append(" title=\"Wizards/Land\">-");
                Errors.logError(e);
            }
        }

        /// <summary>
        /// dispalys the Mod WPA Column
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="scienceCheck"></param>
        /// <param name="item"></param>
        /// <param name="getSciences"></param>
        public static void displayModWpaColumn(StringBuilder sb, bool scienceCheck, ProvinceClass item, CS_Code.Utopia_Province_Data_Captured_Science getSciences)
        {
            try
            {
                sb.Remove(sb.Length - 1, 1);
                decimal WPA = 0;
                if (item.Wizards.GetValueOrDefault(0) != 0 && (decimal)item.Land.GetValueOrDefault(1) != 0)
                    WPA = ((decimal)item.Wizards.GetValueOrDefault(0) / (decimal)item.Land.GetValueOrDefault(1));
                var raceWPA = (from r in UtopiaHelper.Instance.Races where item.Race_ID == r.uid select r.name).FirstOrDefault();
                sb.Append(" title=\"(W/Acres)");
                switch (raceWPA) //AGECHANGECALC
                {
                    case "Dark Elf":
                        WPA *= (decimal)1.3;
                        sb.Append("*Dark Elf(1.3)");
                        break;
                    case "Orc":
                        WPA *= (decimal).8;
                        sb.Append("*Human(.8)");
                        break;
                }
                switch (scienceCheck)
                {
                    case true:
                        decimal sciWPA = (decimal)(getSciences.SOS_Magic_Percent.GetValueOrDefault(0) / 100) + 1;
                        sb.Append("*Sci(" + sciWPA + ")");
                        WPA = WPA * (decimal)sciWPA;
                        break;
                }
                string levelWPA = (WPA * (from r in UtopiaHelper.Instance.Ranks where item.Nobility_ID == r.uid select r.wpaMulti).FirstOrDefault()).ToString("N1");
                switch (levelWPA)
                {
                    case "0.0":
                        switch (WPA.ToString("N1"))
                        {
                            case "0.0":
                                sb.Append("\">-");
                                break;
                            default:
                                switch (item.Wizards_Value_Type.GetValueOrDefault())
                                {
                                    case 1:
                                    case 2:
                                        sb.Append(" (direct value from CB)\">" + WPA.ToString("N1"));
                                        break;
                                    case 3:
                                    case 4:
                                    default:
                                        sb.Append(" (Guesses taken from Angel/Raw pages)\"><u>" + WPA.ToString("N1") + "g</u>");
                                        break;
                                }
                                break;
                        }
                        break;
                    default:
                        switch (item.Wizards.GetValueOrDefault())
                        {
                            case 1:
                            case 2:
                                sb.Append("*Honor(" + (from r in UtopiaHelper.Instance.Ranks where item.Nobility_ID == r.uid select r.wpaMulti).FirstOrDefault() + " (direct value from CB)\">" + WPA.ToString("N1"));
                                break;
                            case 3:
                            case 4:
                            default:
                                sb.Append("*Honor(" + (from r in UtopiaHelper.Instance.Ranks where item.Nobility_ID == r.uid select r.wpaMulti).FirstOrDefault() + " (Guesses taken from Angel/Raw pages)\"><u>" + WPA.ToString("N1") + "</u>");
                                break;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Errors.logError(e);
                sb.Append(">-");
            }
        }


        /// <summary>
        /// displays the Est TPA column
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="item"></param>
        public static void displayEstTpaColumn(StringBuilder sb, ProvinceClass item)
        {
            sb.Remove(sb.Length - 1, 1);
            try
            {
                if (item.Land.GetValueOrDefault(1) != 0)
                {
                    decimal etpa = (decimal)item.Thieves.GetValueOrDefault(0) / (decimal)item.Land.GetValueOrDefault(1);
                    switch (etpa.ToString())
                    {
                        case "0":
                            sb.Append(" title=\"Thieves/Land\">-");
                            break;
                        default:
                            sb.Append(" title=\"Thieves/Land " + item.Thieves.GetValueOrDefault(0) + "/" + item.Land.GetValueOrDefault(1));
                            switch (item.Thieves_Value_Type.GetValueOrDefault())
                            {
                                case 1:
                                    sb.Append(" (direct value from CB)\">" + etpa.ToString("N1"));
                                    break;
                                case 2:
                                    sb.Append(" (Taken from an Infiltration)\"><u>" + etpa.ToString("N1") + "i</u>");
                                    break;
                                case 3:
                                case 4:
                                default:
                                    sb.Append(" (Guesses taken from Angel/Raw pages)\"><u>" + etpa.ToString("N1") + "g</u>");
                                    break;
                            }
                            break;
                    }
                }
                else
                { sb.Append(">-"); }
            }
            catch (Exception e)
            {
                Errors.logError(e);
                sb.Append(">-");
            }
        }
    }
}
using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.SessionState;
using SupportFramework;
using Boomers.Utilities.DatesTimes;
using Boomers.Utilities.Guids;

using PimpLibrary.Utopia;
using PimpLibrary.UI;
using PimpLibrary.Utopia.Players;
using Pimp.UData;


namespace Pimp.UParser
{
    /// <summary>
    /// Summary description for UtopiaParser
    /// </summary>
    public partial class UtopiaParser
    {
        /// <summary>
        /// used for the wol ticker and ce posts.
        /// AGECHANGECALC
        /// </summary>
        public static DateTime WORLD_OF_LEGENDS_START_DATE =  Convert.ToDateTime(ConfigurationManager.AppSettings["WORLD_OF_LEGENDS_START_DATE"]);
        /// <summary>
        /// used to delete old intel.
        /// </summary>
        public static DateTime WORLD_OF_LEGENDS_OLD_START_DATE = Convert.ToDateTime(ConfigurationManager.AppSettings["WORLD_OF_LEGENDS_OLD_START_DATE"]);

        public static DateTime GENERATIONS_START_DATE = Convert.ToDateTime(ConfigurationManager.AppSettings["GENERATIONS_START_DATE"]);

                private static DateTime GetLastDayOfMonth(DateTime dtDate)
        {
            DateTime dtTo = dtDate;
            dtTo = dtTo.AddMonths(1);
            return dtTo.AddDays(-(dtTo.Day));
        }
        /// <summary>
        /// Get the last day of a month expressed by it's
        /// integer value
        /// </summary>
        /// <param name="iMonth"></param>
        /// <returns></returns>
        private static DateTime GetLastDayOfMonth(int iMonth)
        {
            // set return value to the last day of the month
            // for any date passed in to the method
            // create a datetime variable set to the passed in date
            DateTime dtTo = new DateTime(DateTime.UtcNow.Year, iMonth, 1);
            // overshoot the date by a month
            dtTo = dtTo.AddMonths(1);
            // remove all of the days in the next month
            // to get bumped down to the last day of the
            // previous month
            dtTo = dtTo.AddDays(-(dtTo.Day));
            // return the last day of the month
            return dtTo;
        }

        public static DateTime RealTime(int UtopiaD, int UtopiaM, int UtopiaY)
        {

            // UtopiaY :: 0-?  :: starting from year 0
            // UtopiaM :: 1-7  :: utopia has only 7 Months in one year
            // UtopiaD :: 1-24 :: there is 24 hours in a day

            DateTime dts = GetServerDateTime();
            // UtopiaY :: 0-?  :: starting from year 0
            // UtopiaM :: 1-7  :: utopia has only 7 Months in one year
            // UtopiaD :: 1-24 :: there is 24 hours in a day
            int StartD = dts.Day; //01.34.6789
            int StartM = dts.Month;
            int StartY = dts.Year;
            List<int> MaxDay = new List<int> { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            //var MaxDay = new Array(){31,28,31,30,31,30,31,31,30,31,30,31};
            var RealY = 0;    // RealYear
            var RealM = 0;    // RealMonth
            var RealD = 0;    // RealDay
            var AllDays = UtopiaM + UtopiaY * 7;
            var DaysIFM = MaxDay[StartM - 1] - StartD + 1; // DaysInFirstMonth

            if (AllDays <= DaysIFM) { RealY = StartY; RealM = StartM; RealD = StartD + AllDays; }
            else
            {

                var YCounter = 0;
                var MCounter = 0;
                var DCounter = AllDays - DaysIFM;

                while (DCounter > 0)
                {
                    MCounter = MCounter + 1;
                    if (StartM + MCounter > 12) { MCounter = MCounter - 12; YCounter = YCounter + 1; }
                    DCounter = DCounter - MaxDay[StartM - 1 + MCounter];
                    if (DCounter == 0) { MCounter = MCounter + 1; DCounter = -MaxDay[StartM + MCounter - 1]; }
                }

                RealY = StartY + YCounter;
                RealM = StartM + MCounter;
                RealD = MaxDay[RealM - 1] + DCounter + 1;

            }
            int UtopiaHH = UtopiaD - 1;

            DateTime now = DateTime.UtcNow;
            if (DateTime.TryParse(RealM + "/" + RealD + "/" + RealY + " " + UtopiaHH + ":" + DateTime.UtcNow.Minute + ":00", out now))
                return now.AddHours(-6); //GMT Time mnues 6 hours...
            else
                return new DateTime(RealY, RealM, RealD, UtopiaD, 0, 0).AddHours(-6);
        }
        //    function RealTime(UtopiaY,UtopiaM,UtopiaD,UtopiaHH,UtopiaMM) {

        //// UtopiaY :: 0-?  :: starting from year 0
        //// UtopiaM :: 1-7  :: utopia has only 7 Months in one year
        //// UtopiaD :: 1-24 :: there is 24 hours in a day

        //var StartD = Number( Age_Start_Date.charAt(0) + Age_Start_Date.charAt(1) ); //01.34.6789
        //var StartM = Number( Age_Start_Date.charAt(3) + Age_Start_Date.charAt(4) );
        //var StartY = Number( Age_Start_Date.charAt(6) + Age_Start_Date.charAt(7) + Age_Start_Date.charAt(8) + Age_Start_Date.charAt(9) );

        //var MaxDay = new Array(31,28,31,30,31,30,31,31,30,31,30,31);
        //var RealY   = 0;    // RealYear
        //var RealM   = 0;    // RealMonth
        //var RealD   = 0;    // RealDay
        //var AllDays = UtopiaM - 1 + UtopiaY * 7;
        //var DaysIFM = MaxDay[StartM-1] - StartD + 1; // DaysInFirstMonth

        //if ( AllDays <= DaysIFM ) { RealY = StartY; RealM = StartM; RealD = StartD + AllDays; } else {

        //var YCounter = 0;
        //var MCounter = 0;
        //var DCounter = AllDays - DaysIFM;

        //while ( DCounter > 0 ) {
        //MCounter = MCounter + 1;
        //if ( StartM+MCounter > 12 ) { MCounter = MCounter - 12; YCounter = YCounter + 1; }
        //DCounter = DCounter - MaxDay[StartM-1+MCounter];
        //if ( DCounter == 0 ) { MCounter = MCounter + 1; DCounter = -MaxDay[StartM + MCounter-1]; }
        //}

        //RealY = StartY + YCounter;
        //RealM = StartM + MCounter;
        //RealD = MaxDay[RealM-1] + DCounter +1;

        //}

        //var temp = "th";

        //if ( RealD ==  1 ) temp = "st";
        //if ( RealD ==  2 ) temp = "nd";
        //if ( RealD ==  3 ) temp = "rd";
        //if ( RealD == 21 ) temp = "st";
        //if ( RealD == 22 ) temp = "nd";
        //if ( RealD == 23 ) temp = "rd";
        //if ( RealD == 31 ) temp = "st";

        //RealD = RealD + temp;

        //if ( UtopiaMM < 10 ) UtopiaMM = "0" + UtopiaMM ;
        //UtopiaHH = UtopiaD - 1 ;

        //return Months[RealM-1]+" "+RealD+", "+RealY+" ("+UtopiaHH+":"+UtopiaMM+" UTC/GMT -8)";

        //}


        public static DateTime RealTime(string RawUtopiaDate)
        {
            DateTime dts = GetServerDateTime();
            int UtopiaD = Convert.ToInt32(GetDay(RawUtopiaDate));
            int UtopiaM = Boomers.Utilities.DatesTimes.Formatting.Month(URegEx.rgxMonth.Match(RawUtopiaDate).Value);
            int UtopiaY = Convert.ToInt32(URegEx.rgxYear.Match(RawUtopiaDate).Value.Replace("YR", "").Trim());
            // UtopiaY :: 0-?  :: starting from year 0
            // UtopiaM :: 1-7  :: utopia has only 7 Months in one year
            // UtopiaD :: 1-24 :: there is 24 hours in a day

            int StartD = dts.Day; //01.34.6789
            int StartM = dts.Month;
            int StartY = dts.Year;
            List<int> MaxDay = new List<int> { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            //var MaxDay = new Array(){31,28,31,30,31,30,31,31,30,31,30,31};
            var RealY = 0;    // RealYear
            var RealM = 0;    // RealMonth
            var RealD = 0;    // RealDay
            var AllDays = UtopiaM - 1 + UtopiaY * 7;
            var DaysIFM = MaxDay[StartM - 1] - StartD + 1; // DaysInFirstMonth

            if (AllDays <= DaysIFM) { RealY = StartY; RealM = StartM; RealD = StartD + AllDays; }
            else
            {

                var YCounter = 0;
                var MCounter = 0;
                var DCounter = AllDays - DaysIFM;

                while (DCounter > 0)
                {
                    MCounter = MCounter + 1;
                    if (StartM + MCounter > 12) { MCounter = MCounter - 12; YCounter = YCounter + 1; }
                    DCounter = DCounter - MaxDay[StartM - 1 + MCounter];
                    if (DCounter == 0) { MCounter = MCounter + 1; DCounter = -MaxDay[StartM + MCounter - 1]; }
                }

                RealY = StartY + YCounter;
                RealM = StartM + MCounter;
                RealD = MaxDay[RealM - 1] + DCounter + 1;
            }
            int UtopiaHH = UtopiaD - 1;

            DateTime now = DateTime.UtcNow;
            if (DateTime.TryParse(RealM + "/" + RealD + "/" + RealY + " " + UtopiaHH + ":" + DateTime.UtcNow.Minute + ":00", out now))
                return now.AddHours(-6); //GMT Time mnues 6 hours...
            else
                return new DateTime(RealY, RealM, RealD, UtopiaD, 0, 0).AddHours(-6);
        }

        private static string GetDay(string UTDate)
        {
            return URegEx.rgxQuantitiesWithComma.Match(URegEx.rgxDay.Match(UTDate).Value).Value;
        }
        public static string GetServerName()
        {
            //var ownerKingdomID = UtopiaParser.GetOwnerKingdomID();
            //int serverID=2;
            //try { serverID = KingdomCache.getKingdom(ownerKingdomID).Kingdoms.Where(x => x.Kingdom_ID == ownerKingdomID).FirstOrDefault().Server_ID; 


            //}
            //catch
            //{
            //    serverID = 2;
            //}
            //switch (serverID)
            //{
            //    case 2:
            return "wol";
            //    case 3:
            //        return "gen";
            //    default:
            //        return "none";
            //}
        }
        public static string GetServerName(int id)
        {
            switch (id)
            {
                case 2:
                    return "wol";
                case 3:
                    return "gen";
                default:
                    return "none";
            }
        }
        private static DateTime GetServerDateTime()
        {
            switch (GetServerName())
            {
                case "gen":
                    return GENERATIONS_START_DATE;
                default:
                    return WORLD_OF_LEGENDS_START_DATE;
            }
        }
        private static DateTime GetServerDateTime(string wolGenTourn)
        {
            switch (wolGenTourn)
            {
                case "gen":
                    return GENERATIONS_START_DATE;
                default:
                    return WORLD_OF_LEGENDS_START_DATE;
            }
        }
        public static string getUtopiaDateTime(string WolGenTourn)
        {
            DateTime now = DateTime.UtcNow;
            DateTime startDate = GetServerDateTime(WolGenTourn);
            TimeSpan ts = now.Subtract(startDate);
            //Divides the base time with the number of secs in a utopian year to get the number of years
            //Divides the fraction of years with the number of secs in a utopian month to get the number of months
            // Divides the fraction of months with the number of secs in a utopian day to get the number of days.
            string month = Boomers.Utilities.DatesTimes.Formatting.Month(Convert.ToInt32(Math.Ceiling((ts.TotalSeconds % 604800) / 86400)));
            return month + " " + Convert.ToInt32(ts.TotalHours % 24) + " of YR" + Math.Floor(ts.TotalSeconds / 604800);
        }
        public static string getUtopiaDateTime(DateTime rlDateTime)
        {
            DateTime startDate = GetServerDateTime(GetServerName());
            TimeSpan ts = rlDateTime.Subtract(startDate);
            //Divides the base time with the number of secs in a utopian year to get the number of years
            //Divides the fraction of years with the number of secs in a utopian month to get the number of months
            // Divides the fraction of months with the number of secs in a utopian day to get the number of days.
            string month = Boomers.Utilities.DatesTimes.Formatting.Month(Convert.ToInt32(Math.Ceiling((ts.TotalSeconds % 604800) / 86400)));
            return month + " " + Convert.ToInt32(ts.TotalHours % 24) + " of YR" + Math.Floor(ts.TotalSeconds / 604800);
        }
        public static UtopiaDate getUtopiaDateTime2(DateTime rlDateTime)
        {
            DateTime startDate = GetServerDateTime(GetServerName());
            TimeSpan ts = rlDateTime.Subtract(startDate);
            //Divides the base time with the number of secs in a utopian year to get the number of years
            //Divides the fraction of years with the number of secs in a utopian month to get the number of months
            // Divides the fraction of months with the number of secs in a utopian day to get the number of days.        
            return new UtopiaDate { Month = Convert.ToInt32(Math.Ceiling((ts.TotalSeconds % 604800) / 86400)), Day = Convert.ToInt32(ts.TotalHours % 24), Year = Convert.ToInt32(Math.Floor(ts.TotalSeconds / 604800)) };
        }
        public static String[] DisplayUtopianDateTime()
        {
            return new String[] { UtopiaParser.getUtopiaDateTime(UtopiaParser.GetServerName()) + ", <span id='timeLeft'>" + DateTime.UtcNow.TimeLeftInHour().ToRelativeDate() + "</span> Left in Tick", DateTime.Now.SecondsLeftInHour().ToString() };
        }

        /// <summary>
        /// Finds the Utopia Date And Time in a string.
        /// (January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}((st|rd|th|nd),|\sof)\sYR\d+
        /// </summary>
        //public static Regex Static.rgxFindUtopianDateTime = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        //private static Regex rgxDay = new Regex(@"\d+ of", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Find the Month.
        /// </summary>
        //private static Regex Static.rgxMonth = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Finds the Key Search for CBs
        /// </summary>
       private static string FindPersonality(string searchKey, Guid currentUserID)
        {
            switch (searchKey)
            {
                case "Wealthy":
                    return "Merchant";
                case "Sorcerer":
                    return "Mystic";
                case "Heroic":
                case "Conniving":
                    return "Tactician";
                case "Warrior":
                    return "Warrior";
                case "Rogue":
                    return "Rogue";
                case "Humble":
                    return "Shepherd";
                case "Wise":
                    return "Sage";
                case "Crafts":
                    return "Artisan";
                case "":
                    return string.Empty;
                default:
                    FailedAt("FindPersonalityBroken", searchKey, currentUserID);
                    return string.Empty;
            }
        }
        public static string UtopiaKingdomPage = "http://utopia-game.com/wol/game/kingdom_details/";
       
         /// <summary>
        /// Gets the Year of the string Input.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        private static string GetYear(string RawData)
        { return URegEx.rgxYear.Match(RawData).Value.Replace("YR", ""); }
        /// <summary>
        /// Gets the Month of a string input.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        private static string GetMonth(string RawData)
        {
            return URegEx.rgxMonth.Match(RawData).Value;
        }
       
        /// <summary>
        /// Converts Utopian Days to Minutes
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static double ConvertUtopiaDaystoMinutes(string days)
        {
            return Convert.ToDouble(60 * Convert.ToDecimal(URegEx. _utopiaDays.Match(days).Value));
        }
        /// <summary>
        /// Gets the Owner Kingdom ID of the currently logged in user.
        /// </summary>
        /// <returns></returns>
        //public static Guid GetOwnerKingdomID()
        //{
        //    Guid ownerKingdomID = new Guid();
        //    if (HttpContext.Current.Session["OwnerKingdomID"] != null)
        //    {
        //        ownerKingdomID = new Guid(HttpContext.Current.Session["OwnerKingdomID"].ToString());
        //        if (ownerKingdomID != new Guid())
        //            return ownerKingdomID;
        //    }

        //    if (HttpContext.Current.Profile.PropertyValues["OwnerKingdomID"] != null)
        //        if (!HttpContext.Current.Profile.PropertyValues["OwnerKingdomID"].UsingDefaultValue)
        //            if (HttpContext.Current.Profile.PropertyValues["OwnerKingdomID"].PropertyValue.ToString().IsValidGuid())
        //            {
        //                HttpContext.Current.Session["OwnerKingdomID"] = HttpContext.Current.Profile.PropertyValues["OwnerKingdomID"].PropertyValue.ToString();
        //                ownerKingdomID = new Guid(HttpContext.Current.Session["OwnerKingdomID"].ToString());
        //                if (ownerKingdomID != new Guid())
        //                    return ownerKingdomID;
        //            }

        //    if (HttpContext.Current.Profile.PropertyValues["StartingProvince"] != null)
        //        if (!HttpContext.Current.Profile.PropertyValues["StartingProvince"].UsingDefaultValue)
        //            if (HttpContext.Current.Profile.PropertyValues["StartingProvince"].PropertyValue.ToString().IsValidGuid())
        //            {
        //                CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        //                var provs = (from xx in db.Utopia_Province_Data_Captured_Gens
        //                             where xx.Province_ID == new System.Guid(HttpContext.Current.Profile.PropertyValues["StartingProvince"].PropertyValue.ToString())
        //                             select new
        //                             {
        //                                 xx.Owner_Kingdom_ID,
        //                                 xx.Province_ID
        //                             }).FirstOrDefault();
        //                if (provs != null)
        //                {
        //                    HttpContext.Current.Profile.SetPropertyValue("StartingProvince", provs.Province_ID.ToString());
        //                    HttpContext.Current.Profile.SetPropertyValue("OwnerKingdomID", provs.Owner_Kingdom_ID.Value.ToString());
        //                    HttpContext.Current.Session["OwnerKingdomID"] = provs.Owner_Kingdom_ID.Value.ToString();
        //                    HttpContext.Current.Session["StartingProvince"] = provs.Province_ID.ToString();
        //                    CachedItems.SetStartingKingdom(provs.Owner_Kingdom_ID.Value, pimpUser.PimpUser.getUser());
        //                    return provs.Owner_Kingdom_ID.Value;
        //                }
        //                else
        //                    HttpContext.Current.Profile.SetPropertyValue("StartingProvince", new Guid().ToString());
        //            }

        //    return new Guid();
        //}

        public static Guid GetOwnerKingdomID(Guid provID)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var provs = (from xx in db.Utopia_Province_Data_Captured_Gens
                         where xx.Province_ID == provID
                         select new
                         {
                             xx.Owner_Kingdom_ID,
                             xx.Province_ID
                         }).FirstOrDefault();
            if (provs != null)
                return provs.Owner_Kingdom_ID.Value;

            return new Guid();
        }
        /// <summary>
        /// Gets the Race ID of the Name given.
        /// </summary>
        /// <param name="RaceName">The Race Name to use.</param>
        /// <returns></returns>
        public static int RaceNamePull(string RaceName, Guid currentUserID)
        {
            switch (RaceName.Trim())
            {
                case "Orc":
                case "OR":
                    RaceName = "Orc";
                    break;
                case "Dark Elf":
                case "DE":
                    RaceName = "Dark Elf";
                    break;
                case "Elf":
                case "EL":
                    RaceName = "Elf";
                    break;
                case "Human":
                case "HU":
                    RaceName = "Human";
                    break;
                case "Gnome":
                case "GN":
                    RaceName = "Gnome";
                    break;
                case "Dwarf":
                case "DW":
                    RaceName = "Dwarf";
                    break;
                case "UD":
                case "Undead":
                    RaceName = "Undead";
                    break;
                case "Avian":
                case "AV":
                    RaceName = "Avian";
                    break;
                case "Halfling":
                case "HA":
                    RaceName = "Halfling";
                    break;
                case "FA":
                case "Faery":
                    RaceName = "Faery";
                    break;
                default:
                    //if none of the items are cased correctly, then it will revert back to the race name used.
                    UtopiaParser.FailedAt("'RaceNamePullProblem'", RaceName, currentUserID);
                    break;
            }
            return UtopiaHelper.Instance.Races.Where(x => x.name == RaceName).Select(x => x.uid).FirstOrDefault();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;



namespace Pimp.UParser
{
    /// <summary>
    /// Summary description for Static
    /// </summary>
    public static class URegEx
    {
        public static Regex _findPersonalitySearchKey = new Regex(@"(Wealthy|Sorcerer|Heroic|Warrior|Rogue|Humble|Wise|Crafts|Conniving)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findPersonalityNames = new Regex(@"(Merchant|Mystic|Tactician|War Hero|Warrior|Rogue|Shepherd|Sage|Artisan)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
       
        public static Regex rgxNumber = new Regex(@"\d+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public  static Regex rgxQuantitiesWithComma = new Regex(@"[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        //Utopia Related Regexes
        public static Regex rgxYear = new Regex(@"YR\d+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex rgxDay = new Regex(@"\d+\sof", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex rgxMonth = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        //Your wizards begin casting, and the spell succeeds. We have made our lands extraordinarily fertile for 15 days!
        public static Regex rgxDayOps = new Regex(@"\d+\sday", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // Finds the Island/Location.
        public static Regex rgxFindIslandLocation = new Regex(@"\(?(\#?\d+:\#?\d+\s?)\)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        // Finds the Island from the Island/Location Regex.
        public static Regex rgxFindIsland = new Regex(@"\d{1,2}:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        // Finds the location from the island/location Regex.
        public static Regex rgxFindLocation = new Regex(@":\d{1,2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);


        //Universal Province Name
        public static string ProvinceNameRegex = @"[a-zA-Z\s\d\-_]+";
        public static string ProvinceNameWithIslandLocation = @"[a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)";
        public static string ProvinceNameFirstLetterWithoutSpace = @"[a-zA-Z\d\-_]";
        public static string ProvinceUnknown = "An unknown province from";
        public static Regex rgxProvinceUnknown = new Regex(ProvinceUnknown, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //Universal Utopian Date and time.
        public static string UtopianDateTimeRegex = @"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+";
        public static Regex rgxFindUtopianDateTime = new Regex(UtopianDateTimeRegex, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Finds New lines.
        /// </summary>
        public static Regex _findNewLines = new Regex(@"\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Find in Game SOS lines.
        /// </summary>
        public static Regex _rgxFindLinesSOS = new Regex(@"(Alchemy|Tools|Housing|Food|Military|Crime|Channeling)\s+[\d,]+\s+(\+)?(\d+,\d+,\d+\.\d+|\d+,\d+\.\d+|\d+\.\d+|\d\.\d)%\s[a-zA-Z\s&;]+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex findKingdomName = new Regex(@"(Current kingdom is|The kingdom of|The Esteemed kingdom of|The Famous kingdom of) [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Kingdom or Province Name.
        /// </summary>
        public static string _findProvinceLineValidation = @"[ \(M\)S\*\^]+?\s+(Orc|Gnome|Dark Elf|Elf|Human|Avian|Dwarf|Faery|Halfling|Undead)\s+([\d{1,5}]+\sacres|-|DEAD)\s+(\d*,\d*,\d*gc|\d*,\d*gc|\d*gc|-|DEAD)\s+(\d+gc)\s+((Baroness|Baron|Countess|Count|King|Knight|Noble Lady|Lady|Lord|Marquis|Duke|Duchess|Princess|Prince|Peasant|Queen|Viscountess|Viscount|DEAD)(\s\d+)?)";
        public static string _races = "(Orc|Gnome|Dark Elf|Elf|Human|Avian|Dwarf|Faery|Halfling|Undead)";
        public static Regex _findKingdomProvinceName = new Regex(@"(-\sUnclaimed\s-|-\sAwaiting Activation\s-|[a-zA-Z\d\-\!'_]['a-zA-Z\d\-\! _]{0,30}\s*\(?(\d+:\d+)\)?)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findKingdomProvinceLines = new Regex(@"(-\sUnclaimed\s-|-\sAwaiting Activation\s-|[a-zA-Z\d\-\!'_]['a-zA-Z\d\-\! _]{0,30})(\(M\))?(\(S\))?(\^)?(\+)?(\*)?\s+(Orc|Gnome|Dark Elf|Elf|Human|Avian|Dwarf|Faery|Halfling|Undead)\s+([\d{1,5}]+\sacres|-|DEAD)\s+(\d*,\d*,\d*gc|\d*,\d*gc|\d*gc|-|DEAD)\s+(\d+gc)\s+((Baroness|Baron|Countess|Count|King|Knight|Noble Lady|Lady|Lord|Marquis|Duke|Duchess|Princess|Prince|Peasant|Queen|Viscountess|Viscount|DEAD)(\s\d+)?)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findKingdomProvinceNames = new Regex(@"(-\sUnclaimed\s-|-\sAwaiting Activation\s-|[a-zA-Z\d\-\!'_]['a-zA-Z\d\-\! _]{0,30})(\(M\))?(\(S\))?(\^)?(\+)?(\*)?\s+(Orc|Gnome|Dark Elf|Elf|Human|Avian|Dwarf|Faery|Halfling|Undead)\s+\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findKingdomProvinceNamesRemove = new Regex(@"(\(M\))?(\(S\))?(\^)?(\+)?(\*)?\s+(Orc|Gnome|Dark Elf|Elf|Human|Avian|Dwarf|Faery|Halfling|Undead)\s+\d", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findKingdomWarWinsLosses = new Regex(@"Wars\sWon\s/\sConcluded\sWars\s+\d+\s/\s\d+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Province name within an SOM.
        /// </summary>
        public static Regex _findSOMProvinceName = new Regex(@"from the Military Elders of [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Province Name of the Throne Page.
        /// </summary>
        public static Regex _findThroneProvinceName = new Regex(@"Province\sof\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Province name for Surveys. Remove Report\sof\s
        /// </summary>
        public static Regex _findSurveyProvinceName = new Regex(@"Report\sof\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Province name for Surveys. Remove lands\sof\s
        /// </summary>
        public static Regex _findSurveyInGameProvinceName = new Regex(@"lands\sof\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// gets the province name of the survey shown away.
        /// </summary>
        public static Regex _findSurveyInGameAwayProvinceName = new Regex(@"scour the lands of [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findScienceInGameAwayProvinceName = new Regex(@"research centers of [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Province name within an SOS.
        /// </summary>
        public static Regex _findSOSProvinceName = new Regex(@"centers\sof\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Province name within an SOS.
        /// </summary>
        /// <returns>centers\sof\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)</returns>
        public static Regex _findSOSAngelProvinceName = new Regex(@"(Intelligence\son\s|Science Intel on\s)[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Please add your own helper text in front on this find.
        /// </summary>
        /// <returns>[a-zA-Z\s\d\-]{0,24}\((\#?\d+:\#?\d+)\)</returns>
        public static string _findGenericProvinceName = @"[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)";
        /// <summary>
        /// Regex for Mod Off Sent.
        /// </summary>
        public static Regex _modOffSent = new Regex(@"(!?(MO|mo)\s?[\d]+[\d\.]+k?|!?(MO|mo)?\s?[\d]+[\d\.]+k)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findAttacks = new Regex(@"((Y)?our forces arrive at|Alas,|Your army was no match for the defenses of|Our army appears to have failed,|Your troops march onto the battlefield and are quickly driven back,) [a-zA-Z\s\d\-_]{0,35}(\((\#?\d+:\#?\d+)\))?(\.|!)?[A-Z\sa-z,\d!\.']+Our forces will be available again in [\d\.]+ days \(on [\dA-Za-z\s]+\)(\.)?(\s+)?(!?(MO|mo)\s?[\d\.]+k?|!?(MO|mo)?\s?[\d\.]+k)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Export line.
        /// </summary>
        public static Regex _findExportLine = new Regex(@"[*]{0,2} Export Line\s[\W\w]{0,40} [*]{0,2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static Regex _findOffDef = new Regex(@"\(\d+/\d+\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findTotalProvinces = new Regex(@"Total Provinces\s+\d+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Number of a particular line.
        /// </summary>
        //public static Regex Static.rgxNumber = new Regex(@"\d+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        //public static Regex rgxDay = new Regex(@"\d+\sday", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAcres = new Regex(@"[\d,]+\s(new\s)?acre(s|)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Ruler and Race in the same string.
        /// </summary>
        public static Regex _findRulerRace = new Regex(@"Ruler & Race:\s[a-zA-Z\s\d\-]{0,30},\s[a-zA-Z\s\d\-]{2,10}.", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Race of the Item.
        /// </summary>
        public static Regex _findRace = new Regex(@"(Orc|Gnome|Dark Elf|Human|Avian|Dwarf|Faery|Halfling|Undead|Elf|-)", RegexOptions.Compiled); //Only want to see Orc and not orc so I took off ignored case.
        public static Regex _findRaceSec = new Regex(@"Race: (Orc|Gnome|Dark Elf|Human|Avian|Dwarf|Faery|Halfling|Undead|Elf|-)", RegexOptions.Compiled);//Only want to see Orc and not orc so I took off ignored case.
        /// <summary>
        /// Finds the amount of armies in the SOM Data.
        /// </summary>
        public static Regex _findGenerals = new Regex(@"#\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findGeneralsName = new Regex(@"Generals\s+\d", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Personality and Race of SOM Data.
        /// </summary>
        public static Regex _findPersonalityandRace = new Regex(@"Personality & Race:\s[a-zA-Z\s\d\-]{0,30},\s[a-zA-Z\s\d\-]{2,10}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Personality from Finding just Personality and Race.
        /// </summary>
        public static Regex _findPersonalityRacePersonality = new Regex(@"[a-zA-Z\s\d\-]{0,30},", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// returns kingdom stance
        /// </summary>
        public static Regex _findKingdomStanceName = new Regex(@"Stance(:)?\s+(Normal|Peaceful|Fortified|Aggressive)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Personality
        /// </summary>
        public static Regex _findPersonality = new Regex(@"Personality: [a-zA-Z\s\d\-]{0,30}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds Rulers Name.
        /// </summary>
        public static Regex _findRulerName = new Regex(@"Ruler Name: [a-zA-Z\s\d\-]{0,30}\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findRulerNameSurvey = new Regex(@"Ruler Name: [a-zA-Z\s\d\-]{0,50}Personality", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the buiding efficiency
        /// </summary>
        public static Regex _findBuildingEfficiency = new Regex(@"Building Efficiency:\s(\d{0,3}\.\d{0,2}%|\d{0,3}%)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Building Types mainly useds in Surveys
        /// </summary>
        public static Regex _findBuildingTypes = new Regex(@"(Farms|Banks|Armouries|Barracks|Guilds|Stables|Homes|Mills|Training Grounds|Forts|Guard Stations|Hospitals|Thieves' Dens|Watch Towers|Towers|Libraries|Schools|Dungeons|Barren Land)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Returns Kingdom Stance, Remove Kingdom\sStance:\s
        /// </summary>
        public static Regex _findKingdomStance = new Regex(@"Kingdom\sStance:\s[a-zA-Z\s\d\-]{0,30}\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Total Land.  Remove Total\sLand:\s
        /// </summary>
        public static Regex _findTotalLand = new Regex(@"Total\sLand(:)?\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findUnbuiltLand = new Regex(@"Total Undeveloped land\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds all quantities.
        /// </summary>

        /// <summary>
        /// Find Quantities string
        /// </summary>
        public static string _findQuantitiesString = @"[\d,]+";
        /// <summary>
        /// Finds Quantities with a decimal place.
        /// </summary>
        public static Regex _findQuantitiesDecimal = new Regex(@"(\d+,\d+,\d+\.\d+|\d+,\d+\.\d+|\d+\.\d+|\d\.\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds Quantities with a decimal place.
        /// </summary>
        public static string _findQuantitiesDecimalString = @"(\d+,\d+,\d+\.\d+|\d+,\d+\.\d+|\d+\.\d+|\d\.\d)";
        /// <summary>
        /// Finds all quantities includes the dash.
        /// </summary>
        public static Regex _findQuantitiesDash = new Regex(@"(-+|\d+,\d+,\d+,\d+|\d+,\d+,\d+|\d+,\d+|\d+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds Percentages in text.
        /// </summary>
        public static Regex _findPercentages = new Regex(@"(\d{0,3}\.\d{0,2}%|\d{0,3}%)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// finds the utopia days like 18.24
        /// </summary>
        public static Regex _utopiaDays = new Regex(@"[\d\.]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Rulers name and amount of armies in SOM.
        /// </summary>
        public static Regex _findGeneralsSOM = new Regex(@"we\shave\s\d\sgenerals", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Rulers name in the SOM.
        /// </summary>
        public static Regex _findSOMRulerName = new Regex(@"[a-zA-Z\s\d\-]{0,35},", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds SOM Efficeincy.
        /// </summary>
        public static Regex _findSOMMilEfficiency = new Regex(@"our\smilitary\sis\sfunctioning\sat\s\d{0,3}\.\d{0,2}%", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Total Non Peaseants in an SOM.
        /// </summary>
        public static Regex _findSOMNonPeasants = new Regex(@"approximately\s\d{0,3}\.\d{0,2}%\sof our maximum\spopulation", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Wages of an SOM.
        /// </summary>
        public static Regex _findSOMWageRate = new Regex(@"Our\swage\srate\sis\s\d{0,3}\.\d+%", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds Offensive Effiecency in SOM.
        /// </summary>
        public static Regex _findSOMMilOffenseEff = new Regex(@"Offensive\sMilitary\sEffectiveness\s+\d{0,3}\.\d{0,2}%", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Defense Mil Effiecinecy in SOM.
        /// </summary>
        public static Regex _findSOMMilDefenseEff = new Regex(@"Defensive\sMilitary\sEffectiveness\s+\d{0,3}\.\d{0,2}%", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds Net Offensive for SOM.
        /// </summary>
        public static Regex _findSOMMilNetOff = new Regex(@"Net\sOffensive\sPoints\sat\sHome\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Net Defensive in SOMs.
        /// </summary>
        public static Regex _findSOMMilNetDef = new Regex(@"Net\sDefensive\sPoints\sat\sHome\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Soldiers data for SOMs.
        /// </summary>
        public static Regex _findSOMSoldiersData = new Regex(@"Soldiers(\s+(-+|\d+,\d+,\d+,\d+|\d+,\d+,\d+|\d+,\d+|\d+))+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the War Horses Data for SOM's
        /// </summary>
        public static Regex _findSOMWarHorses = new Regex(@"War\sHorses(\s+(-+|\d+,\d+,\d+,\d+|\d+,\d+,\d+|\d+,\d+|\d+))+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds Captured land for SOM's
        /// </summary>
        public static Regex _findSOMCapturedLand = new Regex(@"Captured\sLand([-\d+,\s]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Offense, Defense and Elites Strings in SOM's
        /// </summary>
        public static Regex _findSOMOffDefElites = new Regex(@"[A-Za-z\s]+(\s+(-+|\d+,\d+,\d+,\d+|\d+,\d+,\d+|\d+,\d+|\d+))+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Time availbe till the army returns.
        /// </summary>
        public static Regex _findSOMTimeAvailable = new Regex(@"(Standing Army|Army\s#\d)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findSOMTimeLeft = new Regex(@"(\((-)?\d+\.\d+\sdays\sleft\))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds any decimals in a string.
        /// </summary>
        public static Regex _findDecimalNumbers = new Regex(@"[\d\.]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds a string with unavailable or available.
        /// </summary>
        public static Regex _findSOMUnAvailable = new Regex(@"Available|Unavailable", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the First line of a RAW SOM.
        /// </summary>
        public static Regex _findSOMTrainingString = new Regex(@".*?Training - Number of Days.*?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Training for the military that is an away game.
        /// </summary>
        public static Regex _findSOMTrainingAway = new Regex(@"(-|[\d\,]+)[\s\t]+(-|[\d\,]+)[\s\t]+(-|[\d\,]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Traing for the military that is a Home Game.
        /// Must check the string for "Trained by your military" to use this Regex.
        /// </summary>
        public static Regex _findSOMTrainingHome = new Regex(@"(-|[\d\,]+)[\s\t]+(-|[\d\,]+)[\s\t]+(-|[\d\,]+)[\s\t]+(-|[\d\,]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the SOS Science Types.
        /// </summary>
        public static Regex _findSOSTypes = new Regex(@"(Alchemy|Tools|Housing|Food|Military|Crime|Channeling)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Percentages and extra strings to match Science Type.
        /// </summary>
        public static Regex _findSOSAwayPercentages = new Regex(@"(\d{0,3}\.\d{0,2}%|\d{0,3}%)\s[A-Za-z\s&;]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Percentages and points for SOS.
        /// </summary>
        public static Regex _findSOSHomePercentagePoints = new Regex(@"(\d{0,3}\.\d{0,2}%|\d{0,3}%)\s[A-Za-z\s&]+\([\d,]+\s(books|points)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// finds the points for Sciences.
        /// </summary>
        public static Regex _findSOSHomePoints = new Regex(@"[\d,]+\s(books|points)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the line for each points in progress.
        /// </summary>
        public static Regex _findSOSHomeInProgress = new Regex(@"[A-Za-z\s&]+:\s[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Building lines.
        /// </summary>
        public static Regex _findSurveyAngelBuildingsProgress = new Regex(@"[A-Za-z\s]*:\s([\d,]+) \( ?([\d.]+)%\)( \+ ([\d\,]+) in progress \(\+?([\d.]+)%\))?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the number in progress part of buildings line for Surveys, remove in progress part.
        /// </summary>
        public static Regex _findSurveyAngelBuildingsInProgress = new Regex(@"[\d,]+\sin\sprogress", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findDragonType = new Regex(@"(Emerald|Gold|Sapphire|Ruby)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Line Numbers.
        /// </summary>
        public static Regex _findLineNumbers = new Regex(@"\n\d{1,2}\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds the Text in front of Colon, Remove Colon along with Leading Whitespace
        /// </summary>
        public static Regex _findTextFrontOfColon = new Regex(@"[A-Za-z\s]*:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Finds Lines in the Survey In Game Data.
        /// </summary>
        public static Regex _findSurveyInGameLines = new Regex(@"(Barren\sLands|Homes|Farms|Mills|Banks|Training\sGrounds|Armouries|Barracks|Forts|Guard\sStations|Hostpitals|Guilds|Towers|Thieves'\sDens|Watchtowers|Libraries|Schools|Stables|Dungeons)\s*[\d,]+\s*(\d{0,3}\.\d{0,2}%|\d{0,3}%)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findBuildingLines = new Regex(@"(Homes|Farms|Mills|Banks|Training\sGrounds|Armouries|Barracks|Forts|Guard\sStations|Guilds|Watch Towers|Towers|Thieves'\sDens|Libraries|Schools|Stables|Dungeons)\s+[\d,]+\s+[\d,]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// Thieves assasinate troops.
        /// </summary>
        public static Regex _thievesAssasinate = new Regex(@"(You send your thieves, and the operation commences\.+)?(\s+)?(We\slost\s\d+\sthieves\sin\sthe\soperation\.\s+)?Early\sindications\sshow\sthat\sour\soperation\swas\sa\ssuccess\.\s?Our\sthieves\sassassinated\s[\d,]+\senemy\stroops", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _theivesAssasinates = new Regex(@"(You send your thieves, and the operation commences\.+)?(\s+)?(We\slost\s\d+\sthieves\sin\sthe\soperation(,\sbut\s)?\.+\s)?(Early indications show that our operation was a success\.\s?)?Our\sthieves\sassassinated\s[\d,]+\senemy\stroops\sat[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _thievesAssasinateWizards = new Regex(@"(You send your thieves, and the operation commences\.+)?(\s+)?(We\slost\s\d+\sthieves\sin\sthe\soperation\.+\s)?(Early\sindications\sshow\sthat\sour\soperation\swas\sa\ssuccess\.\s+)?Our\sthieves\sassassinated\s[\d,]+\swizard(s)?\sof\sthe\senemy's\sguilds", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _thievesLostInOperation = new Regex(@"We\slost\s\d+\sthieves\sin\sthe\soperation\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Thieves Kidnapped people.
        /// Early indications show that our operation was a success. Unfortunately, our thieves did not find many people, and were unable to return with any of them
        /// </summary>
        public static Regex _thievesKidnapped = new Regex(@"(We lost \d+ thieves in the operation\.\s+)?(Early indications show that our operation was a success\.)?(\s+)?(Our\sthieves\skidnapped\smany\speople,\sbut\sonly\swere\sable\sto\sreturn\swith\s[\d,]+|Unfortunately, our thieves did not find many people, and were unable to return with any of them)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Thieves Stole money.
        /// </summary>
        //public static Regex _provinceNameInfil = new Regex("Guilds of " + _findGenericProvinceName, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _convertedThieves = new Regex(@"(You send your thieves, and the operation commences\.+)?(We\slost\s\d+\sthieves\sin\sthe\soperation,\sbut\s\.+\s)?(Early\sindications\sshow\sthat\sour\soperation\swas\sa\sstunning\ssuccess\.)?(\s)?We\shave\sconverted\s[\d,]+\sof[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)'s thieves to our guild\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// We lost 2 thieves in the operation. Early indications show that our operation was a success. We have converted 34 Berserkers from the enemy to our army
        /// </summary>
        public static Regex _convertedArmyThievesWizards = new Regex(@"(We lost \d+ thieves in the operation.\s)?(Early indications show that our operation was a success\.)?(\s)?We\shave\sconverted\s\d+\s((thief|thieves|wizard(s)?|soldier(s)?) from the enemy\sto\sour\sguild\.|of the enemy's specialist troops to our army|(Elf Lord(s)?|Drows|Drake(s)?|Knights|Ogre(s)?|Berserkers|Beastmaster(s)?|Brute(s)?|Ghoul(s)?) from the enemy to our army)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _convertedArmy = new Regex(@"(You send your thieves, and the operation commences\.+)?\s+?(We\slost\s\d+\sthieves\sin\sthe\soperation,\sbut\s\.+\s)?(Early\sindications\sshow\sthat\sour\soperation\swas\sa\sstunning\ssuccess\.)?(\s)?We\shave\sconverted\s\d+\s(specialists|Knight|enemy\sspecialists|enemy\selite\stroops|soldier(s)?)\sto\sour\sarmy\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _convertedArmyFailed = new Regex(@"(You send your thieves, and the operation commences\.+)?\s+?(We\slost\s\d+\sthieves\sin\sthe\soperation,\sbut\s\.+\s)?(Early\sindications\sshow\sthat\sour\soperation\swas\sa\sstunning\ssuccess\.)?(\s)?Unfortunately, so few soldiers were found that none of them converted\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _thievesStealWarHorses = new Regex(@"(Early indications show that our operation was a success\.)?(\s+)?Our thieves were able to release [\d,]+ horses but could only bring back [\d,]+ of them", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _thievesFreePrisoners = new Regex(@"(You send your thieves, and the operation commences\.+)?(\s+)?(We\slost\s\d+\sthieves\sin\sthe\soperation,\sbut\s\.\.\.\s)?(Early indications show that our operation was a success\.)?(\s)?Our thieves freed [\d,]+ prisoners from enemy dungeons", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _thievesBurnedAcres = new Regex(@"(We lost \d+ thieves in the operation\.\s+)?(Early indications show that our operation was a success\.\s+)?Our thieves burned down \d+ (hospitals|acre(s)? of buildings|home(s)?|watch towers|banks|guilds|guard stations|training grounds|towers|thieves' dens|forts|military barrack(s)?|farm(s)?|armoury(s)?)\.?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _thievesBurnedAcresFailed = new Regex(@"(We lost \d+ thieves in the operation\.\s+)?(Early indications show that our operation was a success\.\s+)?Unfortunately, our thieves were too few in number to find any buildings to burn\.?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        
        /// <summary>
        /// Early indications show that our operation was a success. Unfortunately, our thieves were too few in number to find any buildings to burn. 
        /// </summary>
        public static Regex _thievesTriedToBurnedAcres = new Regex(@"(We lost \d+ thieves in the operation\.\s+)?(Early indications show that our operation was a success\.\s+)?Unfortunately, our thieves were too few in number to find any buildings to burn\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _thievesStoleMoney = new Regex(@"(You send your thieves, and the operation commences\.+)?(\s+)?(We\slost\s\d+\sthieves\sin\sthe\soperation\.\s+)?(Early\sindications\sshow\sthat\sour\soperation\swas\sa\ssuccess\.)?(\s)?Our\sthieves\shave\sreturned\swith\s[\d,]+ gold coin(s)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _rgxRiots = new Regex(@"(You send your thieves, and the operation commences\.+)?(\s+)?(We\slost\s\d+\sthieves\sin\sthe\soperation\.\s+)?Early\sindications\sshow\sthat\sour\soperation\swas\sa\ssuccess\.(\s)?(Our\sthieves\shave\scaused\srioting\.\sIt\sis\sexpected|Our thieves have caused rioting expected)\sto\slast\s\d+\sday(s)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Early indications show that our operation was a success. Our thieves have caused rioting. However, it was quickly calmed and will have no lasting effect. 
        /// </summary>
        public static Regex _rgxRiotsNoEffects = new Regex(@"(You send your thieves, and the operation commences\.+)?(\s+)?(We\slost\s\d+\sthieves\sin\sthe\soperation\.\s+)?Early\sindications\sshow\sthat\sour\soperation\swas\sa\ssuccess\.(\s)?Our thieves have caused rioting. However, it was quickly calmed and will have no lasting effect\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Your wizards gather 2,291 runes and begin casting, and the spell succeeds. Our spell found no gold to turn to lead. 
        /// </summary>
        public static Regex _rgxGoldToLeadNoEffects = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Our spell found no gold to turn to lead\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _rgxRiotss = new Regex(@"(You send your thieves, and the operation commences\.+)?(We\slost\s\d+\sthieves\sin\sthe\soperation,\sbut\s\.+\s)?(Early indications show that our operation was a success\.)?(\s)?Our\sthieves\shave\scaused\srioting\s(at\s)?[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)\.\s+It is expected to last \d+ days.", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _bribedGeneral = new Regex(@"(You\ssend\syour\sthieves,\sand\sthe\soperation\scommences\.+\s+)?(We\slost\s\d+\sthieves\sin\sthe\soperation\.\s)?Early\sindications\sshow\sthat\sour\soperation\swas\sa\ssuccess\.\s+Our\sthieves\shave\sbribed\san\senemy\sgeneral!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _sabatogeSpells = new Regex(@"(Early indications show that our operation was a success\. )?Our thieves have sabotaged their wizards' ability to cast spells\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _rgxFailedCheck = new Regex(@"(Early\sindications\sshow\sthat\sour\soperation|We\slost\s\d+\sthieves\sin\sthe\soperation)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _rgxBribed = new Regex(@"(We\slost\s\d+\sthieves\sin\sthe\soperation\.\s+)?(Early indications show that our operation was a success\.)?\s+Our thieves have bribed members of our enemies' guild(!)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _rgxStealRunes = new Regex(@"(You\ssend\syour\sthieves,\sand\sthe\soperation\scommences\.\.\.\s?)?(We\slost\s\d+\sthieves\sin\sthe\soperation\.\s+)?Early\sindications\sshow\sthat\sour\soperation\swas\sa\ssuccess\.\s?Our\sthieves\swere\sable\sto\ssteal\s[\d,]+\srunes", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// 
        /// </summary>
        public static Regex _rgxStoleFoods = new Regex(@"(You\ssend\syour\sthieves,\sand\sthe\soperation\scommences\.+\s+)?(We\slost\s\d+\sthieves\sin\sthe\soperation,\sbut\s\.\.\.\s)?Early indications show that our operation was a stunning success\.\s+Our thieves have returned with [\d,]+ bushels of food\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// We lost 2 thieves in the operation. Early indications show that our operation was a success. Our thieves have returned with 44,599 bushels.
        /// We lost 2 thieves in the operation. Early indications show that our operation was a success. Our thieves have returned with 30,767 bushels.
        /// </summary>
        public static Regex _rgxStoleFood = new Regex(@"(You\ssend\syour\sthieves,\sand\sthe\soperation\scommences\.+\s+)?(We\slost\s\d+\sthieves\sin\sthe\soperation\.\s+)?Early indications show that our operation was a success\.\s+Our thieves have returned with [\d,]+ bushels\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// Early indications show that our operation was a success. Our thieves have infiltrated the Thieves' Guilds of Freedom Fighter (3:21). They appear to have about 1,912 thieves employed across their lands
        /// </summary>
        public static Regex _rgxInfiltrated = new Regex(@"(We lost \d+ thieves in the operation.\s+)?(Early\sindications\sshow\sthat\sour\soperation\swas\sa\ssuccess\.\s+)?Our\sthieves\shave\sinfiltrated\sthe\sThieves'\sGuilds\sof\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)\.\s+They\sappear\sto\shave\sabout\s[\d,]+\s(thieves|thief)\semployed\sacross\stheir\slands", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _minorProtection = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Our\srealm\sis\snow\sunder\sa\ssphere\sof\sprotection\sfor\s\d+\sdays", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _treeOfGold = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?[\d,]+ gold coins have fallen from the trees!", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _fog = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Our\slands\sare\sfilled\swith\sfog,\sslowing\sopposing\sarmies\sfor\s\d+\sdays!", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _reflectMagic = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Some of the spells cast upon our lands will be reflected back upon their creators for \d+ days!", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _magicShield = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\. )?The magical auras within our province will protect us from the black magic of our enemies for \d+ days!", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _aggression = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\. )?Our soldiers will fight with unique aggression for \d+ days!", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _fertileLands = new Regex(@"(Your wizards gather [\d]+ runes and begin casting, and the spell succeeds\.\s+)?We\shave\smade\sour\slands\sextraordinarily\sfertile\sfor\s\d+\sdays!", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _naturesBlessing = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\. )?Our lands have been blessed by nature for \d+ days, and will be protected from drought and storms\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _highBirth = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Our\speasantry\sis\sinfluenced\sby\sa\smagical\scalm\.\sWe\sexpect\sbirth\srates\sto\sbe\shigher\sfor\s\d+\sdays!", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _fastBuilders = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Our\sbuilders\shave\sbeen\sblessed\swith\sunnatural\sspeed\sfor\s\d+\sdays!", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _inspireArmy = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Our army has been inspired to train harder\.\sWe\sexpect\smaintenance\scosts\sto\sbe\sreduced\sfor\s\d+\sdays!", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _anonymity = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Our\snext\sattack\swill\sbe\scloaked\sunder\sthe\sshades\sof\sanonymity\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _wakeDead = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds.\s)?Our\sdead\swill\sbe\sawakened\sthe\snext\stime\sour\slands\sare\sattacked!", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _shadowlight = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds.\s)?Our\slands\sare\sblessed\swith\sShadowlight\.(\sThe next time thieves enter our lands their identities will be revealed\.)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _patriotism = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Our\speople\sare\sexcited\sabout\sthe\smilitary\sand\swill\ssignup\smore\squickly\sfor\s\d+\sdays!", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _paradise = new Regex(@"(Your\swizards\sgather\stheir\srunes\sand\sbegin\scasting\.\sThe\sspell\sconsumes\s\d+\sRunes\sand\s[\.]+\sis\ssuccessful!\s)?Our\smages\screated\s\d+\snew\sacres\sof\sland\sfor\sus\sto\suse\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _validateCheck = new Regex(@"(Your\swizards\sgather\stheir\srunes\sand\sbegin\scasting|The\sspell\sconsumes\s\d+\sRunes\sand|Your wizards gather [\d,]+ runes and begin casting)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _tornadoes = new Regex(@"((Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?)?Tornadoes\sscour\sthe\slands\sof\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\),\s(laying\swaste\sto\s\d+\sacre(s)?\sof\sbuildings|but destroy no buildings)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _pitfalls = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s+)?Pitfalls\swill\shaunt\sthe\slands\sof\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)\sfor\s\d+\sdays\.( They will suffer increased defensive losses during battle\.)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Your wizards gather their runes and begin casting. The spell consumes 926 Runes and ... is successful! Storms will ravage drome (6:12)'s lands for the next 14 days!  
        /// </summary>
        public static Regex _storms = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Storms\swill\sravage\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)\sfor\s\d+\sdays!?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _stormsNoEffects = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Unfortunately, storms have no effect on the inhabitants of that land\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        ///  Your wizards gather 1,394 runes and begin casting, and the spell succeeds. Unfortunately, droughts have no effect on the inhabitants of that land. 
        /// </summary>
        public static Regex _droughtsNoEffects = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Unfortunately, droughts have no effect on the inhabitants of that land\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Your wizards gather 6,856 runes and begin casting, and the spell succeeds. A magic vortex overcomes the province of Ixiz (2:6), but evaporates as it was unable to negate any spells.. 
        /// </summary>
        public static Regex _mysticVortexFailed = new Regex(@"((Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?)?A magic vortex overcomes the province of[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\), but evaporates as it was unable to negate any spells\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _naturesBlessingFailed = new Regex(@"((Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?)?Unfortunately,\sthat\sland\shas\sbeen\sblessed\sby\snature,\sand\sour\sspell\shad\sno\seffect!", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _naturesBlessingFailedProvince = new Regex(@"(Your\swizards\sgather\stheir\srunes\sand\sbegin\scasting\.\s+)?(The\sspell\sconsumes\s\d+\sRunes\sand\s[\.]+\s+is\ssuccessful!\s+)?Unfortunately, [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)'s\sland\shas\sbeen\sblessed\sby\snature,\sand\sour\sspell\shad\sno\seffect!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// Your wizards gather 2,743 runes and begin casting, and the spell succeeds. Our mages have caused our enemy's soldiers to turn greedy for 8 days. 
        /// </summary>
        public static Regex _greedySoldiers = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Our mage(s)? have caused our enemy's soldiers to turn greedy for \d+ day(s)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _greedySoldierss = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Our mages have caused our enemy's soldiers to turn greedy the lands of\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)\s\d+\sdays", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _greedySoldiersss = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Our mages have caused our enemy's soldiers of\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)\sto turn greedy for \d+\sdays", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static Regex _nightmares = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s+)?During the night, [\d,]+ of the men in the armies and thieves' guilds of\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\) had nightmares\.(\sSome were forced into rehabilitation, but the soldiers simply quit the army)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _fireball = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s+)?A\sfireball\sburns\sthrough\sthe\sskies\sof\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)\.(\s[\d,]+\speasants\sare\skilled\sin\sthe\sdestruction)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _meteors = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Meteors will rain across (the lands of\s)?[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)\sfor\s\d+\sdays!?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _explosions = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Explosions\swill\srock\said\sshipments\sto\sand\sfrom\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)\sfor\s\d+\sdays(!)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _lead = new Regex(@"(Your\swizards\sgather\stheir\srunes\sand\sbegin\scasting\.\sThe\sspell\sconsumes\s\d+\sRunes\sand\s[\.]+\s+is\ssuccessful!\s)?Our\smages\shave\sturned\s[\d,]+\sof\sour\senemy's\sgold\sinto\sworthless\slead", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Your wizards gather 2,185 runes and begin casting, and the spell succeeds. Our mages have turned 57 gold coins in Tunstall square (2:15) to worthless lead.
        /// </summary>
        public static Regex _lead2 = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\. )?Our mages have turned [\d,]+ gold coins in [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\) to worthless lead\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _warSpoils = new Regex(@"(Your\swizards\sgather\stheir\srunes\sand\sbegin\scasting\.\sThe\sspell\sconsumes\s\d+\sRunes\sand\s[\.]+\sis\ssuccessful!\s)?Our\sarmy\shas\sbeen\sblessed\swith\simmediate\sWar\sSpoils\sfor\s\d+\sdays!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _reflectedMagic = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\. )?This spell was reflected back upon ourselves!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _chastity = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Much to the chagrin of their men, the womenfolk of\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)\shave taken a vow of chastity( for\s\d+\sdays!)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _drought = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?A drought will reign over the lands of\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)\sfor\s\d+\sdays!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _mysticVortex = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s+)?A magic vortex overcomes the province of\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\), negating \d+ active spell(s)? \((Vermin[,\sand]*|Pitfalls[,\sand]*|Nature's Blessing[,\sand]*|Minor Protection[,\sand]*|Fog[,\sand]*|Fertile Lands[,\sand]*|Inspire Army[,\sand]*|Love and Peace[,\sand]*|Quick Feet[,\sand]*|Fountain of Knowledge[,\sand]*|Reflect Magic[,\sand]*|Storms[,\sand]*|Meteor Showers[,\sand]*|Town Watch[,\sand]*|Greater Protection[,\sand]*|Greed[,\sand]*|Animate Dead[,\sand]*|Minor Protection[,\sand]*|Clear Sight[,\sand]*|Patriotism[,\sand]*|Explosions[,\sand]*|Anonymity[,\sand]*|Magic Shield[,\sand]*|Chastity[,\sand]*|Builders Boon[,\sand]*|Mage's Fury[,\sand]*|Nature's Blessing[,\sand]*|War Spoils[,\sand]*)*\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _mysticVotexNegates = new Regex(@"(Nature's Blessing|Animate Dead|Vermin|Pitfalls|Nature's Blessing|Minor Protection|Fog|Love and Peace|Inspire Army|Fertile Lands|Quick Feet|Fountain of Knowledge|Reflect Magic|Clear Sight|Storms|Magic Shield|Town Watch|Meteor Showers|Greater Protection|Greed|Builders Boon|Patriotism|Explosions|Anonymity|Chastity|Mage's Fury|War Spoils)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _mysticVortexCount = new Regex(@"negating \d+ active spell", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _landLust = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Our\sLand\sLust\sover\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)\shas\sgiven\sus\s(another\s+)\d+\s(new\s+)?acre(s)?\sof\sland", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _vermin = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Vermin will feast on the granaries of\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)\sfor\s\d+\sdays!?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _speedAttack = new Regex(@"(Your\swizards\sgather\stheir\srunes\sand\sbegin\scasting\.\sThe\sspell\sconsumes\s\d+\sRunes\sand\s[\.]+\sis\ssuccessful!\s)?Our troops have been blessed with excellent speed for their next battle", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _exposedThieves = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\. )?Our mages have illuminated the lands of our enemies and exposed the thieves that walk through their lands", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _forgotBooks = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\. )?You were able to make the people of [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\) temporarily forget [\d,]+ books of knowledge!", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _incinerateRunes = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\. )?Lightning strikes the Towers in [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\) and incinerates [\d,]+ runes", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findDonatedCoinsDragon = new Regex(@"You have donated [\d,]+ gold coins to the quest of launching a dragon\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _sentAid = new Regex(@"We have sent [\d,]+ (runes|gold coins) to [a-zA-Z\s\d\-_]{0,35}(\((\#?\d+:\#?\d+)\))?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _sentKillDragon = new Regex(@"You send out \d+ troops to fight the dragon\. All are lost in the fight, but the dragon is weakened by \d+ points\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _townWatch = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Our peasants will help defend our lands for \d+ days!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _clearSight = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Our police have been blessed with Clear Sight for \d+ days!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _thievesTurnedInvisible = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s)?Our thieves have been made partially invisible for \d+ days!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMysticThiefProvinceNameRevamp = new Regex(@"(We have sent [\d,]+ (runes|gold coins) to|A magic vortex overcomes the province of|Unfortunately,|We\shave\sconverted\s[\d,]+\sof|Our\sLand\sLust\sover|A drought will reign over the lands of|Vermin will feast on the granaries of|Meteors will rain across the lands of|Explosions\swill\srock\said\sshipments\sto\sand\sfrom|Storms\swill\sravage|Our mages have caused our enemy's soldiers to turn greedy the lands of|A\sfireball\sburns\sthrough\sthe\sskies\sof|Tornadoes\sscour\sthe\slands\sof|Lightning strikes the Towers in|Pitfalls\swill\shaunt\sthe\slands\sof|Our\sthieves\shave\sinfiltrated\sthe\sThieves'\sGuilds\sof|enemy\stroops\sat|Our\sthieves\shave\scaused\srioting\sat|You were able to make the people of|the womenfolk of|and thieves' guilds of|gold coins in)\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMysticThiefQualityRevampBack = new Regex(@"[\d,]+\s(specialists|peasants|military barrack(s)?|thieves' dens|of the enemy's specialist|points|Knight(s)?|enemy\sspecialists|enemy\selite\stroops|books|of\sour|people|prisoners|enemy\stroops|wizard(s)?\s(of|from)|of the men in the|gold\scoin(s)?|day|runes|bushels|hospitals|acre(s)? of buildings|home(s)?|watch towers|banks|guilds|thieves|peasants|soldier(s)?|guard stations|Ogre(s)?|training grounds|towers|forts|Berserkers|thief|Elf Lord(s)?|Beastmaster(s)?|Ghoul(s)?|Drake(s)?|farm(s)?|armoury(s)?)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMysticThiefQualityRevampFront = new Regex(@"(incinerates|created|converted|bring\sback|return\swith)\s[\d,]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _foutainOfKnowledge = new Regex(@"(Your wizards gather [\d,]+ runes and begin casting, and the spell succeeds\.\s+)?Our students are blessed with excellent concentration for\s\d+\sdays!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// Affairs of the State Regex's...
        /// </summary>
        public static Regex _findAffairsdi = new Regex(@"(Our|Daily)\sIncome(\s+)?[\d,]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findAffairsland = new Regex(@"Land\s+[\d,]+\sacres", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findAffairspeasants = new Regex(@"Peasants\s+\d+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findAffairstheives = new Regex(@"Thieves\s+\d+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findAffairswizards = new Regex(@"Wizards\s+\d+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findAffairstotPop = new Regex(@"Total\s+\d+\s+Daily", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findAffairsnetWorth = new Regex(@"Current Networth\s+[\d,]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findAffairshonor = new Regex(@"Honor\s+[\d,]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Kingdom Page In Game
        /// </summary>
        public static Regex _findKingdomPageNumberRemove = new Regex(@"\s\((\#?\d+:\#?\d+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findKingdomPageRaces = new Regex(@"(Orc|Gnome|Dark Elf|Elf|Human|Avian|Dwarf|Faery|Halfling|Undead)", RegexOptions.Compiled);
        public static Regex _findNobility = new Regex(@"(Baroness|Baron|Countess|Count|King|Knight|Noble Lady|Lady|Lord|Marquis|Duke|Duchess|Princess|Prince|Peasant|Queen|Viscountess|Viscount)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findGoldCoins = new Regex(@"[\d,]+gc", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findKingdomPageProvinceLand = new Regex(@"[\d,]+\sacres", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// For the Buildings Advisor Method.
        /// </summary>
        public static Regex _findBuildingsAdvisorbuildEffic = new Regex(@"Building\sEfficiency\s+[\d\.]+%", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findBuildingsAdvisorBuildingLines = new Regex(@"(Barren\sLand|Homes|Farms|Mills|Banks|Training\sGrounds|Armouries|Barracks|Forts|Guard\sStations|Hospitals|Guilds|Watch Towers|Towers|Thieves'\sDens|Libraries|Schools|Stables|Dungeons)\s+[\d,]+\s+[\d+\.]+%", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findBuildingsAdvisorBuildingTraining = new Regex(@"(Farms|Banks|Armouries|Barracks|Guilds|Watch Towers|Towers|Stables|Homes|Mills|Training Grounds|Forts|Guard Stations|Hospitals|Thieves' Dens|Libraries|Schools|Dungeons|Barren Land)\s+[\d\s]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// For the Sciences Advisor Method.
        /// </summary>
        public static Regex _findScienceAdvisorBookLines = new Regex(@"(Alchemy|Tools|Housing|Food|Military|Crime|Channeling)\s+[\d,]+\s+[\d\.]+\s+\+[\d\.]+%", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findSciencefindQuants = new Regex(@"(\d+,\d+,\d+,\d+(\.\d+)?|\d+,\d+,\d+(\.\d+)?|\d+,\d+(\.\d+)?|\d+(\.\d+)?)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findScienceAdvisorBookTrainingLines = new Regex(@"(Alchemy|Tools|Housing|Food|Military|Crime|Channeling)\s+[\d\s]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static Regex _findMilPagetotMoney = new Regex(@"Total money\s+[\d,]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMilPagetotPop = new Regex(@"Total population\s+[\d,]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMilPagetotPeas = new Regex(@"Peasant population\s+[\d,]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMilPagetotWizs = new Regex(@"Wizard population\s+[\d,]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMilPagetotSolds = new Regex(@"Number of soldiers\s+[\d,]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMilPagetotMil = new Regex(@"Military & Thief population\s+[\d,]+\s+\([\d\.%]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMilPageallLines = new Regex(@"[a-zA-Z]+\s+\(\d/\d\)\s+[\d,]+\s+[\d,]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// For the Mystics Affaris page...
        /// </summary>
        public static Regex _findMysticAffairsMagesFury = new Regex(@"Mage's Fury\s+\d+ day(s)?\s+(The fire of Mage's Fury burns in our wizards' eyes for \d+ day(s)?)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMysticAffairsExplosions = new Regex(@"Explosions\s+\d+ day(s)?\s+(Explosions are hampering aid shipments from other provinces for \d+ day(s)?)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMysticAffairsMagicShield = new Regex(@"Magic Shield\s+\d+ day(s)?\s+(The magical auras within our province will protect us from the black magic of our enemies for \d+ day(s)?)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMysticAffairsFanaticism = new Regex(@"Fanaticism\s+\d+ day(s)?\s+(Our army will fight with fanatical fervor for \d+ day(s)?)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMysticAffairsMinorProtection = new Regex(@"Minor Protection\s+\d+\sday(s)?\s+(Our realm is now under a sphere of protection for\s\d+ day(s)?)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairsarmyTrain = new Regex(@"Inspire Army\s+[\d]+ day(s)?\s+(Our army has been inspired to train harder\. We expect maintenance costs to be reduced for \d+ day(s)?)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairsdrought = new Regex(@"Drought\s+\d+ day(s)?\s+(Droughts are reducing our daily harvests and slowing our soldier draft! This will last for \d+ day(s)?)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairstroopStrength = new Regex(@"Our\stroops\sfight\swith\senormously\simpressive\sstrength\s\(Estimated:\s\d{0,2}\smore\sDays\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairspatriotic = new Regex(@"Patriotism\s+\d+ day(s)?\s+(Our people are excited about the military and will signup more quickly for \d+ day(s)?)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairspeace = new Regex(@"Love and Peace\s+\d+ day(s)?\s+(Our peasantry is influenced by a magical calm. We expect birth rates to be higher for \d+ day(s)?)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairsfog = new Regex(@"Fog\s+\d+ day(s)?\s+Our lands are filled with fog, slowing opposing armies for \d+ day(s)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairsraiseDead = new Regex(@"We\swill\sbe\sable\sto\sraise\sour\sdead\sduring\sour\snext\sbattle", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairsgreedyArmy = new Regex(@"Greed\s+\d+\s+day(s)?\s+(Enemies have convinced our soldiers to demand more money for upkeep for \d+ day(s)?)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairsprotectBlackMagic = new Regex(@"A\sshield\sprotects\sus\sfrom\sthe\sblack\smagic\sof\sour\senemies\s\(Estimated:\s\d{0,2}\smore\sDays\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairsclearSight = new Regex(@"Clear Sight\s+\d+ day(s)?\s+(Our police have been blessed with Clear Sight for \d+ day(s)?)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairsblessedLand = new Regex(@"Nature's Blessing\s+\d+ day(s)?\s+(Our lands have been blessed by nature for \d+ day(s), and will be protected from drought and storms)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairsbuildHaste = new Regex(@"Builders Boon\s+\d+ day(s)?\s+Our builders have been blessed with unnatural speed for \d+ day(s)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairslandShadowLight = new Regex(@"Our lands are blessed with Shadowlight", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairsarmySpeed = new Regex(@"Quick Feet\s+-\s+(Our armies have been blessed with excellent speed for their next battle)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairsfountainKnowledge = new Regex(@"Fountain of Knowledge\s+\d+ day(s)?\s+(Our students are blessed with excellent concentration for \d+ day(s)?)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairsreflectSpells = new Regex(@"Reflect Magic\s+\d+ days\s+(Some of the spells cast upon our lands will be reflected back upon their creators for \d+ day(s)?)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairsmysticAura = new Regex(@"Our\sland\sis\sprotected\sby\sa\sMystic\sAura", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairsmeteors = new Regex(@"Meteor Showers\s+\d+ day(s)?\s+Meteors rain across our lands, and are not expected to stop for \d+ day(s)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairsfertileLands = new Regex(@"Fertile Lands\s+\d+ day(s)?\s+(We have made our lands extraordinarily fertile for \d+ day(s)?)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMysticAffairsGreaterProtection = new Regex(@"Greater Protection\s+\d+ day(s)?\s+Our realm is now under a sphere of protection for \d+ day(s)?!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMysticAffairscounter = new Regex(@"\d{0,2}\sdays", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairsseperator = new Regex(@"---", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findMysticAffairsTownWatch = new Regex(@"Town Watch\s+\d+ day(s)?\s+(Our peasants will help defend our lands for \d+ day(s)?)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMysticAffairsPitfalls = new Regex(@"Pitfalls\s+\d+ day(s)?\s+(Pitfalls are haunting our lands for \d+ day(s)?, causing increased defensive losses during battle)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMysticAffairsInvincible = new Regex(@"Invisibility\s+\d+ day(s)?\s+(Our thieves have been made partially invisible for \d+ day(s)?)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);


        public static Regex _findMysticAffairsVermin = new Regex(@"Vermin\s+\d+ day(s)?\s+(Vermin have been discovered eating away our food supplies, and cannot be exterminated for \d+ day(s)?)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMysticAffairsAggression = new Regex(@"Aggression\s+\d+ day(s)?\s+Our soldiers will fight with unique aggression for \d+ day(s)?!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMysticAffairsStorms = new Regex(@"Storms\s+\d+ day(s)?\s+Storms are ravaging our lands! This will last for \d+ day(s)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMysticAffairsWarSpoils = new Regex(@"War Spoils\s+[\d]+ day(s)?\s+(Our army has been blessed with immediate War Spoils for \d+ day(s)?)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMysticAffairsChastity = new Regex(@"Chastity\s+\d+ day(s)?\s+The womenfolk's vow of chastity is preventing any population growth for \d+ day(s)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// For the Angel Home Away page
        /// </summary>
        public static Regex _findAngelHomeProvinceName = new Regex(@"The Province of [a-zA-Z\s\d\-\?_]{0,35}\((\#?\d+:\#?\d+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomeRemoveIslandLocation = new Regex(@"\s\((\#?\d+:\#?\d+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomeAcres = new Regex(@"Land:\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomeMoney = new Regex(@"Money:\s+[\d,]+gc", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomeFood = new Regex(@"Food:\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomeRunes = new Regex(@"Runes:\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomePopulation = new Regex(@"Population:\s[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomePeasants = new Regex(@"Peasants:\s[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomeTradeBalance = new Regex(@"Trade Balance:\s(-|)[\d,]+gc", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomeNetworth = new Regex(@"Networth:\s[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomeSoldiers = new Regex(@"Soldiers:\s[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomeWarHorses = new Regex(@"War-Horses:\s[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomePrisoners = new Regex(@"Prisoners:\s[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomeOffDefElite = new Regex(@":\s[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomeKingdomIsland = new Regex(@"\#?\d+:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomeBuildingEff = new Regex(@"\d+%\sBuilding Efficiency", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomeKingdomIslandLocation = new Regex(@":\#?\d+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomeTotalModOff = new Regex(@"Total\sModified\sOffense:\s[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomeTotalModDef = new Regex(@"Total\sModified\sDefense:\s[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomeHitHard = new Regex(@"Province\swas\s(hit\sa\s|hit\s)?(moderately\shit|extremely\shard|couple\sof\stimes|pretty\sheavily)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findAngelHomeHit = new Regex(@"(moderately|couple|heavily|extremely)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomeThieves = new Regex(@"Thieves:\s[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelHomeWizards = new Regex(@"Wizards:\s[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelEstThieves = new Regex(@"Estimated Thieves Number: [\d,]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findAngelEstWizards = new Regex(@"Estimated Wizards Number: [\d,]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findMaxPossibleThievWizs = new Regex(@"Max\. Possible Thieves/Wizards: [\d,]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findAngelPractical = new Regex(@"Practical \(\d+% elites\):\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// For ingame Throne page.
        /// </summary>
        public static Regex _findInGameRace = new Regex(@"Race(\s+)?(Orc|Gnome|Dark Elf|Elf|Human|Avian|Dwarf|Faery|Halfling|Undead|-)", RegexOptions.Compiled);
        public static Regex _findInGameGetSoldiers = new Regex(@"Soldiers\s+\d[\d+,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findInGameLand = new Regex(@"Land\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findInGamePeasants = new Regex(@"Peasants\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findInGameBuildingEff = new Regex(@"Building\sEff\.\s+\d+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findInGameMoney = new Regex(@"Money\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findInGameFood = new Regex(@"Food\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findInGameRunes = new Regex(@"Runes\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findInGameTradeB = new Regex(@"Trade Balance\s+(-|)[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findInGameThieves = new Regex(@"Thieves\s+([\d,]+|Unknown)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findInGameWizards = new Regex(@"Wizards\s+([\d,]+|Unknown)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findInGameWarHorses = new Regex(@"War\sHorses\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findInGamePrisoners = new Regex(@"Prisoners\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findInGameOffPoints = new Regex(@"Off\.\sPoints\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findInGameDefPoints = new Regex(@"Def\.\sPoints\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findInGameRulerName = new Regex(@"Ruler\s+" + _nobilities + @"\s[a-zA-Z\d\.][a-zA-Z\s\d\.]{0,25}[a-zA-Z\d\.]\s+(Strongarms|Griffins|Rangers|Quickblades|Swordsmen|Night Rangers|Goblins|Warriors|Offense|Skeletons|Magicians):?\s+[\d,]*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findInGameHitHard = new Regex(@"(attacked\smoderately|\snoticably\sattacked\srecently|attacked\sa\slittle|hit\sextremely\shard)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findInGameHit = new Regex(@"(moderately|noticably|little|extremely)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// For the kingdom parsing page in angel
        /// </summary>
        public static Regex _findAngelKingdomProvinceAcres = new Regex(@"([a-z\s\d\#\-\!A-Z_]+)\s\[([A-Z]{2})\]\s-\s[\d,]+\sAcres", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findUltimaKingdomProvinceAcres = new Regex(@"([a-z\s\d\#\-\!A-Z_]+)\s\[" + _races + @"\]\s-\s[\d,]+\sAcres", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static Regex _findAngelKingdomRace = new Regex(@"\[([A-Z]{2})\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelKingdomAcres = new Regex(@"-\s[\d,]+\sAcres", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelKingdomName = new Regex(@"[a-z\s\d\#\-\!A-Z_]+\[", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Parses Temple CE
        /// </summary>
        public static Regex _findTempleCELines = new Regex(@"\*{2}\s(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\sYR\d{1,2}\s\*{2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findTempleCEEndLines = new Regex(@"\*{2}\sSummary\s\*{2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findTempleCEDayth = new Regex(@"\d{1,2}(nd|rd|th|st)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findTempleCEFirstLine = new Regex(@"(\d{1,2}(nd|rd|th|st))?:?\s([a-zA-Z\d\-\!][a-zA-Z\d\-\! ]{0,22}[a-zA-Z\d\-\!]|[a-zA-Z\d\-\!]|\[[a-z\s]{0,19}\]|)\s*\(?(\d+:\d+)\)?\s?(learned from|attacked|failed to attack|pillaged|ambushed|razed|massacred|conquested|sent aid to|failed to)?(:\s\d* acres)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findTempleCEFindCEType = new Regex(@"(killed|destroyed|captured|recaptured|attacked|razed|ambushed|invaded and pillaged|invaded and stole|learned from|aid shipment|attempted an invasion|attempted to invade|failed to|cancelled the dragon|kingdom has begun|Dragon project targetted|begun ravaging our lands|slain the dragon|dragon has set flight|declared WAR|cancelled all relations|Hostile Kingdom|state of Peace|offer of Peace|surrendered|Unable to achieve victory|withdrawn|proposed a ceasefire|formal ceasefire|accepted our ceasefire proposal|rejected a ceasefire|cancelled our ceasefire|broken their ceasefire|A war has started|victory in our war|surrendered to us|stalemate|collapsed and lies in ruins|defected to|ended their state of hostility|official state of peace|declared Peace with us|declared us to be a Hostile Kingdom|proposed a Mutual Peace|degraded into a state of WAR)", RegexOptions.Compiled | RegexOptions.IgnoreCase);


        /// <summary>
        /// For the Angel Military Page
        /// </summary>
        public static Regex _findAngelMilProvinceName = new Regex(@"Military (Intelligence|Intel) on ([a-zA-Z\d\-\!][a-zA-Z\d\-\! ]{0,35}[a-zA-Z\d\-\!]|[a-zA-Z\d\-\!]|)\s*\(?(\d+:\d+)\)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelMilNonPeasants = new Regex(@"Non-Peasants:\s\d{1,2}%", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelMilMilitaryEffOff = new Regex(@"(\d+|\d+\.\d)%\soff", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelMilMilitaryEffDef = new Regex(@"(\d+|\d+\.\d)%\sdef", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelMilMilitaryEffOverAll = new Regex(@"(\d+|\d+\.\d)%\sraw", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelMilNetOffensive = new Regex(@"Net Offense at Home [()a-zA-Z\s]*:\s[\d,]*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelMilNetDefensive = new Regex(@"Net Defense at Home [()a-zA-Z\s]*:\s[\d,]*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelMilStandingArmy = new Regex(@"\*{2}\sStanding\sArmy\s\(At\sHome\)\s\*{2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelMilArmiesGone = new Regex(@"\*{2}\sArm(ies|y) [#\d/]*\s[A-Za-z()\s\d:]*\*{2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelMilQuantities = new Regex(@": [\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelMilOffense = new Regex(@"[\d,]+\s(offense|off)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelMilDefense = new Regex(@"[\d,]+\s(defense|def)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelMilWarHorses = new Regex(@"War Horses:\s[\d,]+\s\(up to [\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelMilOFFDEFELITES = new Regex(@"[A-Za-z\s]*:\s[\d,]+\s\(([\d,]+\s(offense|off\.|off)\s\/\s[\d,]+\s(defense|def\.|def)|[\d,]+\s(defense|offense))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAngelMilTimeToReturn = new Regex(@"Back\sin\s\d+:?\d+\shours", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findAngelMilTempleSelfProvinceName = new Regex(@"Military Intelligence of \[SELF\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static string _getEliteNames = "(Brutes|Drakes|Elf Lords|Golems|Knights|Drow|Drows|Orges|Ogres|Berserkers|Elite|Ghouls|Beastmasters)";
        public static string _getOffNames = "(Strongarms|Griffins|Rangers|Quickblades|Swordsmen|Night Rangers|Goblins|Warriors|Offense|Skeletons|Magicians)";
        public static string _getDefNames = "(Slingers|Harpies|Archers|Pikemen|Archers|Druids|Trolls|Axemen|Defense|Zombies)";
        public static string _nobilities = "(Baroness|Baron|Countess|Count|King|Knight|Noble Lady|Lady|Lord|Marquis|Duke|Duchess|Princess|Prince|Peasant|Queen|Viscountess|Viscount|DEAD)";
        public static Regex _findTrainingQueue = new Regex(@"Next\s\d+\shours:\s[\d,\s]*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findElites = new Regex(@"(Brutes|Drakes|Elf Lords|Golems|Knights|Drow|Drows|Orges|Ogres|Berserkers|Elite|Ghouls|Beastmasters):?\s+[\d,]*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findOffense = new Regex(@"(Strongarms|Griffins|Rangers|Halflings|Swordsmen|Night Rangers|Goblins|Warriors|Offense|Skeletons|Quickblades|Magicians):?\s+[\d,]*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findDefense = new Regex(@"(Slingers|Harpies|Archers|Pikemen|Archers|Druids|Trolls|Axemen|Defense|Zombies):?\s+[\d,]*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findThieves = new Regex(@"Thieves:?\s[\d,]*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static Regex _findSOMRawOff = new Regex(_getOffNames + @"(\s+(\d+)?)+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findSOMRawDef = new Regex(_getDefNames + @"(\s+(\d+)?)+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findSOMRawElite = new Regex(_getEliteNames + @"(\s+(\d+)?)+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findSOMRawThief = new Regex(@"Thieves(\s+(\d+)?)+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static Regex _findTotalKingdomNetworth = new Regex(@"Total Networth\s+[\d,]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _findTotalKingdomLand = new Regex(@"(Total )?Land:?\s+[\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findEverythingButName = new Regex(@"((Orc|Gnome|Dark Elf|Elf|Human|Avian|Dwarf|Faery|Halfling|Undead)\s+([\d{1,5}]+\sacres|-|DEAD)\s+(\d*,\d*,\d*gc|\d*,\d*gc|\d*gc|-|DEAD)\s+(\d+gc)\s+(Baroness|Baron|Countess|Count|King|Knight|Noble Lady|Lady|Lord|Marquis|Duke|Duchess|Princess|Prince|Peasant|Queen|Viscountess|Viscount|DEAD)(\s\d+)?)", RegexOptions.IgnoreCase | RegexOptions.Compiled);


        public static Regex _findAttackOffense = new Regex(_findQuantitiesString + " " + _getOffNames, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAttackDefense = new Regex(_findQuantitiesString + " " + _getDefNames, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAttackElites = new Regex(_findQuantitiesString + " " + _getEliteNames, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAttackFindQuantitesDecimal = new Regex(_findQuantitiesDecimalString + " days", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAttackSoldiers = new Regex(_findQuantitiesString + " Soldiers", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAttackWarHorses = new Regex(_findQuantitiesString + " horses", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAttackEnemyKills = new Regex(_findQuantitiesString + " enemy troops", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAttackImprisonedTroops = new Regex("imprisoned " + _findQuantitiesString, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAttackProvinceName = new Regex("(Your army was no match for the defenses of|forces arrive at) " + _findGenericProvinceName, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findAttackFindQuantityBooks = new Regex(_findQuantitiesString + " books of knowledge", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static Regex _findMysticThiefOpFindThieveQuantitie = new Regex("about " + _findQuantitiesString + " (thieves|thief)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static Regex _findProvincesInKingdom = new Regex(@"Provinces in Kingdom:\s\d+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex _findSOMUpdateMilOverallEfficiency = new Regex(@"our\smilitary\sis\sfunctioning\sat\s(\d+,\d+,\d+\.\d+|\d+,\d+\.\d+|\d+\.\d+|\d\.\d)%\sefficiency", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex rgxCapturedLand = new Regex(@"Captured Land: [\d,]+ Acres", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex rgxFindSoldiersData = new Regex(@"Soldiers:\s[\d,]+\s\([\d,]+\soffense\s/\s[\d,]+\sdefense\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex rgxFindSoldiersDataTemple = new Regex(@"Soldiers:\s[\d,]+\s(\([\d,]+\soffense\sand\sdefense)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex rgxFindWarHorses = new Regex(@"War Horses:\s[\d,]+\s\([\d,]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);


    }

}
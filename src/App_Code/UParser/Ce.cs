using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

using Boomers.Utilities.DatesTimes;
using App_Code.CS_Code.Worker;
using Pimp.UCache;
using PimpLibrary.Static.Enums;
using PimpLibrary.Utopia;
using PimpLibrary.Utopia.Kingdom;
using Pimp.UData;
using Pimp.Utopia;
using Pimp.Users;
using SupportFramework.Data;


namespace Pimp.UParser
{
    /// <summary>
    /// Summary description for CE
    /// </summary>
    public class CE
    {
        private string _rawData;
        private PimpUserWrapper _currentUser;
        private OwnedKingdomProvinces _cachedKingdom;
        private List<CS_Code.Utopia_Kingdom_CE> _ceList;

        private KingdomClass _kingdomOfCe;
        public CE(string rawData, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            _rawData = rawData;
            _currentUser = currentUser;
            _cachedKingdom = cachedKingdom;
            _ceList = new List<CS_Code.Utopia_Kingdom_CE>();
        }


        private static Regex rgxFindOwnerKingdom = new Regex(@"'s of kingdom news from [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// May 1 of YR7	 Johnny Drama has sent an aid shipment to Ralph Macchio.
        /// May 10 of YR7	 Jean-Claude Van Damme has sent an aid shipment to John Paul Jones.
        /// </summary>
        private static string aidLine = "has sent an aid shipment to";
        private static Regex rgxAidSent = new Regex(URegEx.UtopianDateTimeRegex + URegEx.ProvinceNameRegex + aidLine + URegEx.ProvinceNameRegex + @"\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex rgxAidSentSourceProvName = new Regex(URegEx.ProvinceNameRegex + aidLine, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex rgxAidSentTargetProvName = new Regex(aidLine + URegEx.ProvinceNameRegex, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// January 1 of YR8	 Ralph Macchio (7:20) recaptured 80 acres of land from Lynx (6:11).
        /// </summary>
        private static Regex rgxAmbushed = new Regex(URegEx.UtopianDateTimeRegex + URegEx.ProvinceNameWithIslandLocation + @" recaptured " + URegEx.rgxNumber + " acres of land from " + URegEx.ProvinceNameWithIslandLocation + @"\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// April 2 of YR8	 Chamaeleon (6:11) ambushed armies from Milli Vanilli (7:20) and took 148 acres of land.
        /// </summary>
        private static Regex rgxAmbushed2 = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\sambushed armies from [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\) and took \d+ acre(s)? of land\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// January 3 of YR8	 An unknown province from F u n S h o w (7:20) captured 458 acres of land from Ursa Major (6:11).
        ///January 3 of YR8	 An unknown province from F u n S h o w (7:20) captured 480 acres of land from Draco (6:11).
        ///January 3 of YR8	 Jean-Claude Van Damme (7:20) captured 242 acres of land from Chamaeleon (6:11).
        ///February 5 of YR4 The Hole (9:5) captured 1 acre of land from Kardel Sharpeye (8:20).
        /// </summary>
        private static Regex rgxCapturedLand = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\scaptured\s\d+\sacre(s)? of land from [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static Regex rgxCapturedLand2 = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\),\scaptured\s\d+\sacres of land from [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// February 15 of YR9 In local kingdom strife, Cellulan (9:9) invaded Prophecy (9:9) and captured 97 acres of land.
        /// </summary>
        private static Regex rgxCapturedLandIntraKingdom = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+In local kingdom strife, [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\) invaded [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\) and captured \d+\sacres of land\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// January 2 of YR8	 An unknown province from -- Evilution Hostile -- (6:11) invaded Pamela Anderson (7:20) and captured 395 acres of land.
        /// January 5 of YR8	 An unknown province from -- Evilution Hostile -- (6:11) invaded Milli Vanilli (7:20) and captured 303 acres of land.
        /// January 7 of YR8	 An unknown province from -- Evilution Hostile -- (6:11) invaded Alice Cooper (7:20) and captured 267 acres of land.
        /// </summary>
        private static Regex rgxInvadedAndCapturedLand = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\sinvaded [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\) and captured \d+\sacre(s)? of land\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// April 6 of YR8	 An unknown province from -- EVILution Hostile -- (6:11) attempted to invade Burt Reynolds (7:20).
        /// </summary>
        private static Regex rgxAttemptedToInvade = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\sattempted to invade [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// June 19 of YR8 Disgust (3:2) attempted an invasion of Snow Aspire (3:20), but was repelled.
        /// </summary>
        private static Regex rgxAttemptedToInvade2 = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\sattempted an invasion of [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\), but was repelled\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// January 19 of YR9 In intra-kingdom war AdEvil (9:9) attempted to invade Prophecy (9:9), but failed.
        /// </summary>
        private static Regex rgxAttemptedToInvadeIntraKingdom = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+In intra-kingdom war [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\sattempted to invade [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\), but failed\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// June 3 of YR8 The leader of Pure Panic has chosen to join Molon Lave (7:2). All in Pure Panic gather their possessions and depart this kingdom forever.
        /// </summary>
        private static Regex rgxLeftKingdom = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+The leader of [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+ has chosen to join [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\. All in [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+gather their possessions and depart this kingdom forever\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        ///  February 1 of YR5 As the ultimate betrayal, Reality destroys all in the land of _Shakuras_ before leaving for a new kingdom.
        /// </summary>
        private static Regex rgxLeftKingdom2 = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+As the ultimate betrayal, [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+ destroys all in the land of [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+before leaving for a new kingdom\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// March 7 of YR5 The truant Hakan of Plug in Baby has been a neglectful leader, and the peasants have risen up and cast them out. Maybe someday a new leader will reinvigorate this once mighty province.
        /// </summary>
        private static Regex rgxLeftKingdom3 = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+The truant [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+ of [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+ has been a neglectful leader, and the peasants have risen up and cast them out. Maybe someday a new leader will reinvigorate this once mighty province\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// February 1 of YR4 Alas, the once proud province of Nostalgia has collapsed and lies in ruins.
        /// </summary>
        private static Regex _abandonedProvince = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+Alas, the once proud province of [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+[a-zA-Z\d\-_] has collapsed and lies in ruins\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// February 17 of YR6	 The Lords of Utopia have swept down upon the province Land of Vikings (8:7) and crushed it.
        /// </summary>
        private static Regex _monarchDestroyedProvince2 = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+The Lords of Utopia have swept down upon the province [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\((\#?\d+:\#?\d+)\) and crushed it\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// March 7 of YR5 Our monarch feels that this glorious kingdom is tarnished by the abandoned province of Plug in Baby. They order it destroyed and erased from our history books.
        /// </summary>
        private static Regex _monarchDestroyedProvince = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+Our monarch feels that this glorious kingdom is tarnished by the abandoned province of [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+[a-zA-Z\d\-_]\. They order it destroyed and erased from our history books\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// February 14 of YR12 The leader of Bikerville has wisely chosen to join us from Home Gnome and Pwn WAR STFO (6:30).
        /// </summary>
        private static Regex rgxJoinedKingdom = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+The (leader|province) of [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+[a-zA-Z\d\-_] has (wisely chosen to join|defected to) us from [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// May 2 of YR13 happy gilmore has defected to Unnamed kingdom (12:14)! 
        /// </summary>
        private static Regex rgxTheyDefectedToKingdom = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+[a-zA-Z\d\-_] has defected to [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)!", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// June 4 of YR8 Melancholy (3:2) killed 2,008 people within Snow Aspire (3:20). 
        /// June 4 of YR8 Rage (3:2) killed 859 people within Snow Aspire (3:20). 
        /// June 4 of YR8 Lust (3:2) killed 3,057 people within Those Meddling Kids (3:20).
        /// </summary>
        private static Regex rgxMassacred = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\skilled\s[\d,]+\speople within [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// June 8 of YR8 Tangleland (3:20) invaded Suffering (3:2) and killed 836 people.
        /// </summary>
        private static Regex rgxMassacred2 = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\sinvaded [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\) and killed [\d,]+ people\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// June 5 of YR8 An unknown province from Animation Domination (3:20) attacked and stole from Lust (3:2).
        /// </summary>
        private static Regex rgxAttackedAndStole = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\sattacked and stole from [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// July 2 of YR8 An unknown province from Hositle HaJ STAY OUT (3:2) invaded and pillaged Pack your stain stick (3:33).
        /// </summary>
        private static Regex rgxAttackedAndStole2 = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\sinvaded and pillaged [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// January 14 of YR9 Loke (8:23) attacked and pillaged the lands of Bermatingen DE (3:24).
        /// February 14 of YR9 Inception 2 of Erin (7:17) attacked and pillaged the lands of Worcester MA USA (3:24).
        /// </summary>
        private static Regex rgxAttackedAndStole3 = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\sattacked and pillaged the lands of [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// January 15 of YR9 An unknown province from Home Sweet Home (3:24) invaded and stole from Divine Eregion (2:17).
        /// February 10 of YR9 An unknown province from Home Sweet Home (3:24) invaded and stole from Mr Clown (7:5).
        /// </summary>
        private static Regex rgxAttackedAndStole4 = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\sinvaded and stole from [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// June 7 of YR8 Snow Aspire (3:20) razed 48 acres of Melancholy (3:2).
        /// </summary>
        private static Regex rgxRazedAcres = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\srazed [\d,]+ acres of [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// February 3 of YR9	 Johnny Drama (7:20) invaded Phoenix (6:11) and razed 221 acres of land.
        /// </summary>
        private static Regex rgxRazedAcres2 = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\sinvaded [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\) and razed [\d,]+ acres of land\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// February 17 of YR9 In local kingdom strife Mookshire (9:9) invaded Prophecy (9:9) and razed 71 acres of land.
        /// </summary>
        private static Regex rgxRazedAcresIntraKingdom = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+In local kingdom strife [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\) invaded [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\) and razed \d+\sacres of land\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// June 21 of YR8 Our kingdom has begun a Emerald Dragon project targetted at Animation Domination (3:20).
        /// </summary>
        private static Regex rgxStartedEmeraldDragonProject = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+Our kingdom has begun a Emerald Dragon project targetted at [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// July 23 of YR8 The Panty Brigade____STFO (2:18) has begun a Emerald Dragon project against us!
        /// </summary>
        private static Regex rgxTheyStartedEmeraldDragon = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\) has begun a Emerald Dragon project against us!", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static Regex rgxTheyStartedSapphireDragon = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\) has begun a Sapphire Dragon project against us!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// May 23 of YR0 Like fighting Gods eh (9:12) has begun a Gold Dragon project against us!
        /// </summary>
        private static Regex rgxTheyStartedGoldDragon = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\) has begun a Gold Dragon project against us!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// January 21 of YR9 Our kingdom has begun a Ruby Dragon project targetted at Recruiting Attackers (3:35).
        /// </summary>
        private static Regex rgxStartedRubyDragonProject = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+Our kingdom has begun a Ruby Dragon project targetted at [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// April 18 of YR12	 Our kingdom has begun a Sapphire Dragon project targetted at James Bond (3:9).
        /// </summary>
        private static Regex rgxStartedSapphireDragonProjecct = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+Our kingdom has begun a Sapphire Dragon project targetted at [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// March 14 of YR12 Our kingdom has begun a Gold Dragon project targetted at kingdom of storms (5:10).
        /// </summary>
        private static Regex rgxStartedGoldDragonProject = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+Our kingdom has begun a Gold Dragon project targetted at [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// January 21 of YR9 Recruiting Attackers (3:35) has begun a Ruby Dragon project against us!
        /// </summary>
        private static Regex rgxStartedRubyDragonAgainstUs = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\) has begun a Ruby Dragon project against us!", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// January 5 of YR14 Poetry of WaR (3:37) has cancelled their dragon project targetted at us.
        /// </summary>
        private static Regex rgxCancelledDragonTargettedAtUs = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\) has cancelled their dragon project targetted at us\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);


        /// <summary>
        /// June 22 of YR8 Our kingdom has cancelled the dragon project to Animation Domination (3:20).
        /// </summary>
        private static Regex rgxCancelledDragonProject = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+Our kingdom has cancelled the dragon project to [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// January 20 of YR9 We have declared WAR on Recruiting Attackers (3:35)!
        /// </summary>
        private static Regex rgxDeclaredKingdomWar = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+We have declared WAR on [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// January 20 of YR9 Our dragon has set flight to ravage Recruiting Attackers (3:35)!
        /// </summary>
        private static Regex rgxDragonSetFlight = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+Our dragon has set flight to ravage [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// January 21 of YR9 A Ruby Dragon from Recruiting Attackers (3:35) has begun ravaging our lands!
        /// </summary>
        private static Regex rgxRubyDragonRavagingLands = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+A Ruby Dragon from [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\) has begun ravaging our lands!", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static Regex rgxSapphireDragonRavagingLands = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+A Sapphire Dragon from [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\) has begun ravaging our lands!", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// May 6 of YR13 A Gold Dragon from Crayons WAR STFO (3:26) has begun ravaging our lands!
        /// </summary>
        private static Regex rgxGoldDragonRavagingLands = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+A Gold Dragon from [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\) has begun ravaging our lands!", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// February 12 of YR12 A Emerald Dragon from PoD WAR (1:35) has begun ravaging our lands!
        /// </summary>
        private static Regex rgxEmeraldDragonRavagingLands = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+A Emerald Dragon from [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\) has begun ravaging our lands!", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// January 21 of YR9 Mr4sG4rDCrayon has slain the dragon ravaging our lands!
        /// </summary>
        private static Regex rgxSlainDragonRavagingLands = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+ has slain the dragon ravaging our lands!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// June 21 of YR8 Animation Domination (3:20) has proposed a formal ceasefire with our kingdom.
        /// </summary>
        private static Regex rgxProposedCeaseFire = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\shas proposed a formal ceasefire with our kingdom\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// May 15 of YR13 We So Serious (2:23) has declined our ceasefire proposal!
        /// </summary>
        private static Regex rgxTheyDeclinedOurCeaseFireProposal = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\shas declined our ceasefire proposal!", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// June 10 of YR6 We have rejected a ceasefire offer from The KINDRED HOSTILE (1:21).
        /// </summary>
        private static Regex rgxWeRejectedCeaseFireProposal = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+We have rejected a ceasefire offer from [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// February 19 of YR9 Old School (6:2) has accepted our ceasefire proposal!
        /// </summary>
        private static Regex rgxAcceptedOurCeaseFire = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\shas accepted our ceasefire proposal!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// March 18 of YR7 Broken Thrones (1:26) has proposed a Mutual Peace to end our war!
        /// </summary>
        private static Regex _proposedMutualPeace = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\shas proposed a Mutual Peace to end our war!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// May 1 of YR10 We have proposed a Mutual Peace to end our war with FEARLESS (8:40)! 
        /// </summary>
        private static Regex _proposedWeMutualPeace = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+We have proposed a Mutual Peace to end our war with [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// March 1 of YR7 The offer of peace from Unorganized Kingdom (1:36) has been withdrawn.
        /// </summary>
        private static Regex _proposedPeaceWithdrawn = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+The offer of peace from [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\shas been withdrawn\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// March YR12 Edition March 15 of YR12 Incinerators (3:17) has broken their ceasefire agreement with us! 
        /// </summary>
        private static Regex rgxBrokenOurCeaseFire = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\shas broken their ceasefire agreement with us!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// April 4 of YR12 Battle of the Bands On Tour (2:3) has withdrawn from war. Our people rejoice at our victory!
        /// </summary>
        private static Regex rgxTheyWithDrawnFromWar = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\shas withdrawn from war\. Our people rejoice at our victory!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// January 4 of YR9 We have proposed a ceasefire offer to Chaos in Theory (5:22).
        /// </summary>
        private static Regex rgxProposedCeaseFire2 = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+We have proposed a ceasefire offer to [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// July 19 of YR5 We have ordered an early end to the post-war period with UnderworID (1:16).
        /// </summary>
        private static Regex _earlyEndPostWar = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+We have ordered an early end to the post-war period with [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// February 7 of YR6	 We have withdrawn our ceasefire proposal to Random souls (1:39).
        /// </summary>
        private static Regex rgxWithdrawnOurCeaseFireProposal = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+We have withdrawn our ceasefire proposal to [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// July YR6 Edition July 3 of YR6 Brotherhood Rebuilding (2:17) has withdrawn their ceasefire proposal. 
        /// </summary>
        private static Regex rgxWithdrawnTheirCeaseFireProposal = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\) has withdrawn their ceasefire proposal\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// February 3 of YR10	 The offer of peace from Phoenix on 3 30 Forever (8:37) has been withdrawn.
        /// </summary>
        private static Regex rgxWithdrawnTheirPeaceOffer = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+The offer of peace from [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\) has been withdrawn\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// March 6 of YR12	 We have rejected an offer of Peace by Scary Kingdom (3:4). The war still rages on!
        /// </summary>
        private static Regex rgxWeRejectedTheirPeaceOffer = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+We have rejected an offer of Peace by [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\. The war still rages on!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// 
        /// </summary>
        private static Regex rgxCancelledCeaseFire = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+We have cancelled our ceasefire with [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// June 22 of YR8 We have entered into a formal ceasefire with Animation Domination (3:20).
        /// </summary>
        private static Regex rgxEnteredFormalCeasefire = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+We have entered into a formal ceasefire with [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// February 15 of YR9 Stronghold HOSTILE (9:27) has declared WAR with our kingdom!
        /// </summary>
        private static Regex rgxTheyDeclaredWar = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)\shas declared WAR with our kingdom!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// March 13 of YR12 The dragon ravaging our lands has flown away.
        /// </summary>
        private static Regex rgxDragonFlownAway = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+The dragon ravaging our lands has flown away\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static Regex rgxFindFirstProvinceNameInLine = new Regex(@"(In intra-kingdom war [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)|An unknown province from [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)|[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\))(,)?\s(recaptured|captured|invaded|ambushed armies|attempted|killed|attacked and stole from|razed|has proposed a|has begun a Sapphire|has begun a Ruby|attacked and pillaged|has declared WAR|has accepted our|has begun a Emerald|has begun a Gold|has broken their|has withdrawn from war|has declined our|has cancelled their dragon|has withdrawn their)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// February 1 of YR4 Alas, the once proud province of Nostalgia has collapsed and lies in ruins.
        /// 
        /// </summary>
        private static Regex rgxFindFirstProvinceNameInLine2 = new Regex(@"(In intra-kingdom war [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)|An unknown province from [a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)|[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\))(,)?\s(recaptured|captured|invaded|ambushed armies|attempted|killed|attacked and stole from|razed|has proposed a|has begun a Sapphire|has begun a Ruby|attacked and pillaged|has declared WAR|has accepted our|has begun a Emerald|has begun a Gold|has broken their|has withdrawn from war|has declined our|has cancelled their dragon|has withdrawn)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex rgxFindFirstProvinceNameInLine3 = new Regex(@"([a-zA-Z\d\-_][a-zA-Z\s\d\-_]+)\s(has slain the dragon ravaging our|has defected to)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// The truant Hakan of Plug in Baby has been a neglectful leader, and the peasants have risen up and cast them out. Maybe someday a new leader will reinvigorate this once mighty province.
        /// </summary>
        private static Regex rgxFindFirstProvinceNameInLine4 = new Regex(@"The truant ([a-zA-Z\d\-_][a-zA-Z\s\d\-_]+)\sof ([a-zA-Z\d\-_][a-zA-Z\s\d\-_]+)\s(has been a neglectful leader)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// The truant Hakan of Plug in Baby has been a neglectful leader, and the peasants have risen up and cast them out. Maybe someday a new leader will reinvigorate this once mighty province.
        /// </summary>
        private static Regex rgxFindFirstProvinceNameInLine5 = new Regex(@"The truant ([a-zA-Z\d\-_][a-zA-Z\s\d\-_]+)\sof", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex rgxFindLastProvinceNameInLine = new Regex(@"(Our dragon has set flight to ravage |attempted an invasion of |invaded |acres of |attacked and stole from | acres of land from |ambushed armies from |attempted to invade |has chosen to join |people within |project targetted at |formal ceasefire with |invaded and pillaged |dragon project to |We have declared WAR on |A Ruby Dragon from |A Sapphire Dragon from |our ceasefire with |pillaged the lands of |ceasefire offer to |A Gold Dragon from |A Emerald Dragon from |acre of land from |ceasefire proposal to |post-war period with |upon the province |ceasefire offer from |offer of peace from |end our war with |rejected an offer of Peace by )[a-zA-Z\d\-_][a-zA-Z\s\d\-_]+\(?(\#?\d+:\#?\d+)\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex rgxFindLastProvinceNameInLine2 = new Regex(@" invaded " + URegEx.ProvinceNameFirstLetterWithoutSpace + URegEx.ProvinceNameWithIslandLocation + " and captured ", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex rgxFindValueForLine = new Regex(@"[,\d]+ (people within|acres of|acre of|people\.)", RegexOptions.Compiled | RegexOptions.IgnoreCase);



        private static Regex rgxFindGeneralLine = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+[a-zA-Z\d\-_\s\(\):]+(\.|!)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Parse()
        {
            if (rgxFindOwnerKingdom.IsMatch(_rawData))
            {
                string owner = rgxFindOwnerKingdom.Match(_rawData).Value.Replace("'s of kingdom news from", "").Trim();
                KingdomLocation kdLocation = new KingdomLocation();
                kdLocation.Island = Convert.ToInt32(URegEx.rgxFindIsland.Match(URegEx.rgxFindIslandLocation.Match(owner).Value).Value.Replace(":", ""));
                kdLocation.Kingdom = Convert.ToInt32(URegEx.rgxFindLocation.Match(URegEx.rgxFindIslandLocation.Match(owner).Value).Value.Replace(":", ""));

                _kingdomOfCe = KingdomCache.getKingdom(_currentUser.PimpUser.StartingKingdom).Kingdoms
                                                                                        .Where(x => x.Kingdom_Island == kdLocation.Island && x.Kingdom_Location == kdLocation.Kingdom)
                    //.Where(x => x.Kingdom_Location == kdloc)
                                                                                        .FirstOrDefault();
                if (_kingdomOfCe == null)
                    return "0,Couldn't find " + owner + " in the list of your kingdoms.";
            }
            else
                _kingdomOfCe = KingdomCache.getKingdom(_currentUser.PimpUser.StartingKingdom, _currentUser.PimpUser.StartingKingdom, _cachedKingdom);

            parseRubyDragonProjectStarted();
            parseDragonCancelledAgainstUs();
            parseSlainDragonRavagingLands();

            parseRubyDragonRavagingLands();
            parseGoldDragonRavagingLands();
            parseEmeraldDragonRavagingLands();
            parseSapphireDragonRavagingLands();

            parseDragonSetFlight();
            parseDeclaredWarOnKingdom();
            parseProposedCeasefire();
            parseCancelledCeasefire();
            parseEnteredFormalCeasfire();
            parseBrokeCeasefire();
            parseWithdrawnFromWar();
            parseProposedFormalCeasefire();
            parseProposedFormalCeasefire2();
            parseDeclinedCeasefire();
            parseTheyDeclaredWar();
            parseWithdrawnOurCeasefireProposal();
            parseWithdrawnThierCeasefireProposal();
            parseEarlyEndToPostWar();
            parseWeRejectedCeasefireOffer();
            parseProposedMutualPeace();
            parseWeProposedMutualPeace();
            parseWithdrawnTheirPeaceOffer();
            parseWithdrawnThierPeaceOffer();

            parseStartEmeraldDragonProject();
            parseStartRubyDragon();
            parseStartSaphireDragon();
            parseStartGoldDragon();

            parseTheyStartedEmeraldDragon();
            parseTheyStartedEmeraldDragon2();
            parseTheyStartedGoldDragon();
            parseTheyStartedSapphireDragon();

            parseCanceledDragon();
            parseDragonFlownAway();

            parseProvinceRazed();
            parseRazedProvinceIntraKingdom();
            parseProvinceRazed2();

            parseAttackAndStoleFrom();
            parseAttackAndStoleFrom2();
            parseAttackAndStoleFrom3();
            parseAttackAndStoleFrom4();

            parseMassacredProvince2();
            parseMasacredProvince();
            parseProvinceLeftKingdom();
            parseProvinceLeftKingdom2();
            parseProvinceLeftKingdom3();
            parseAbandonedProvince();
            parseJoinedKingdom();
            parseDefectedKingdom();
            parseMonarchDestroyedProvince();
            parseMonarchDestroyedProvince2();

            parseAmbushedProvince();
            parseAmbushedProvince2();

            parseAidSent();
            parseAttemptedToInvade2();
            parseAttemptedToInvadeIntraKingdom();
            parseAttemptedToInvade();
            parseCapturedLand();
            parseCapturedLand2();
            parseCapturedLandIntraKingdom();
            parseCapturedAndInvadedLand();

            if (rgxFindGeneralLine.Matches(_rawData).Count > 0)
                Errors.failedAt("'CELinesStillExistNewCe'", _rawData, _currentUser.PimpUser.UserID);

            //adds the CE just parsed to the Cache
            if (_ceList.Count > 0)
                CeCache.AddNewCeDataToKingdomWorker(_ceList, _cachedKingdom, _kingdomOfCe.Kingdom_ID, _currentUser.PimpUser.StartingKingdom);


            if (_kingdomOfCe != null)
                return "CE Submitted " + _kingdomOfCe.Kingdom_Name + " (" + _kingdomOfCe.Kingdom_Island + ":" + _kingdomOfCe.Kingdom_Location + ")";
            else
                return "CE Submitted";
        }
        /// <summary>
        /// finds captured and invaded land lines
        /// </summary>
        private void parseCapturedAndInvadedLand()
        {
            //Invaded and Captured Acres Line Found.
            MatchCollection mcLines = rgxInvadedAndCapturedLand.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;

                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline
                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.CaputeredLand, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace(" invaded", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        if (URegEx.rgxProvinceUnknown.IsMatch(ceItem.Source_Province_Name))
                            ceItem.Source_Province_Name = "Unknown Province";
                        else
                            ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine2.Match(rawLine).Value.Remove(0, 9).Replace("and captured", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        ceItem.value = URegEx.rgxNumber.Match(rgxFindValueForLine.Match(rawLine).Value).Value;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception exception)
                    {
                        Errors.logError(exception);
                    }
                }
            }
        }
        /// <summary>
        /// find captured land from intra kingdom conflict.
        /// </summary>
        /// <param name="_rawData"></param>
        /// <param name="_currentUser"></param>
        /// <param name="mcLines"></param>
        /// <param name="rawLine"></param>
        /// <param name="_ceList"></param>
        /// <param name="_kingdomOfCe"></param>
        private void parseCapturedLandIntraKingdom()
        {
            MatchCollection mcLines = rgxCapturedLandIntraKingdom.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;

                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline
                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.CaputeredLandIntraKingdom, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("invaded", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        if (URegEx.rgxProvinceUnknown.IsMatch(ceItem.Source_Province_Name))
                            ceItem.Source_Province_Name = "Unknown Province";
                        else
                            ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("invaded", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        ceItem.value = URegEx.rgxNumber.Match(rgxFindValueForLine.Match(rawLine).Value).Value;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// finds the captured lands line.
        /// </summary>
        private void parseCapturedLand2()
        {
            MatchCollection mcLines = rgxCapturedLand2.Matches(_rawData);
            if (mcLines.Count > 0)
            {

                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;

                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline
                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.CaputeredLand, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace(", captured", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        if (URegEx.rgxProvinceUnknown.IsMatch(ceItem.Source_Province_Name))
                            ceItem.Source_Province_Name = "Unknown Province";
                        else
                            ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("acres of land from", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        ceItem.value = URegEx.rgxNumber.Match(rgxFindValueForLine.Match(rawLine).Value).Value;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// finds the captured land lines
        /// February 5 of YR4 The Hole (9:5) captured 1 acre of land from Kardel Sharpeye (8:20). 
        /// </summary>
        private void parseCapturedLand()
        {
            //Captured Acres Line Found.
            MatchCollection mcLines = rgxCapturedLand.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;

                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline
                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.CaputeredLand, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace(" captured", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        if (URegEx.rgxProvinceUnknown.IsMatch(ceItem.Source_Province_Name))
                            ceItem.Source_Province_Name = "Unknown Province";
                        else
                            ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("acres of land from", "").Replace("acre of land from", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        ceItem.value = URegEx.rgxNumber.Match(rgxFindValueForLine.Match(rawLine).Value).Value;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// find the attempted to invade lines
        /// </summary>
        private void parseAttemptedToInvade()
        {
            MatchCollection mcLines = rgxAttemptedToInvade.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;

                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline
                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.AttemptedToInvade, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace(" attempted", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        if (URegEx.rgxProvinceUnknown.IsMatch(ceItem.Source_Province_Name))
                            ceItem.Source_Province_Name = "Unknown Province";
                        else
                            ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("attempted to invade", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// finds attempted to invade lines
        /// </summary>
        private void parseAttemptedToInvadeIntraKingdom()
        {
            MatchCollection mcLines = rgxAttemptedToInvadeIntraKingdom.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.AttemptedToInvadeIntraKingdom, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace(" attempted", "").Replace("In intra-kingdom war", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        if (URegEx.rgxProvinceUnknown.IsMatch(ceItem.Source_Province_Name))
                            ceItem.Source_Province_Name = "Unknown Province";
                        else
                            ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("attempted to invade", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// finds attempted to invade lines
        /// </summary>
        private void parseAttemptedToInvade2()
        {
            MatchCollection mcLines = rgxAttemptedToInvade2.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.AttemptedToInvade, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace(" attempted", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        if (URegEx.rgxProvinceUnknown.IsMatch(ceItem.Source_Province_Name))
                            ceItem.Source_Province_Name = "Unknown Province";
                        else
                            ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("attempted an invasion of", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// finds aid sent lines
        /// </summary>
        private void parseAidSent()
        {
            MatchCollection mcLines = rgxAidSent.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;
                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;

                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline
                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.AidSent, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        ceItem.Source_Province_Name = rgxAidSentSourceProvName.Match(rawLine).Value.Replace(aidLine, "").Trim();
                        ceItem.Target_Province_Name = rgxAidSentTargetProvName.Match(rawLine).Value.Replace(aidLine, "").Trim();

                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// finds ambushed provinces lines
        /// </summary>
        private void parseAmbushedProvince2()
        {
            //Ambushed Line Found.
            MatchCollection mcLines = rgxAmbushed2.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;

                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline
                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.Ambush, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace(" ambushed armies", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("ambushed armies from", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        ceItem.value = URegEx.rgxNumber.Match(rgxFindValueForLine.Match(rawLine).Value).Value;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// finds the ambushed province lines
        /// </summary>
        private void parseAmbushedProvince()
        {
            MatchCollection mcLines = rgxAmbushed.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;

                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline
                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.Ambush, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace(" recaptured", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("acres of land from", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        ceItem.value = URegEx.rgxNumber.Match(rgxFindValueForLine.Match(rawLine).Value).Value;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// finds the defected province lines
        /// </summary>
        private void parseDefectedKingdom()
        {
            MatchCollection mcLines = rgxTheyDefectedToKingdom.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.TheyDefectedToKingdom, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine3.Match(rawLine).Value.Replace("has defected to", "").Trim(); //must have space just incase name contains the word

                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e) { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// finds province joined kingdom line
        /// February 1 of YR5 The province of _Shakuras_ has defected to us from AggressioNs (8:14). 
        /// </summary>
        private void parseJoinedKingdom()
        {
            MatchCollection mcLines = rgxJoinedKingdom.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.JoinedKingdom, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine2.Match(rawLine).Value.Replace("has wisely chosen", "").Replace("The leader of", "").Replace("has defected to", "").Replace("The province of ", "").Trim(); //must have space just incase name contains the word

                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// finds province joined kingdom line 
        /// <remarks>
        /// February 1 of YR4 Alas, the once proud province of Nostalgia has collapsed and lies in ruins.</remarks>
        /// </summary>
        private void parseAbandonedProvince()
        {
            MatchCollection mcLines = _abandonedProvince.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.AbandonedProvince, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine2.Match(rawLine).Value.Replace("proud province of", "").Replace("has collapsed and", "").Trim(); //must have space just incase name contains the word
                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// monarch destroys the province
        /// 
        /// March 7 of YR5 Our monarch feels that this glorious kingdom is tarnished by the abandoned province of Plug in Baby. They order it destroyed and erased from our history books.
        /// </summary>
        private void parseMonarchDestroyedProvince()
        {
            MatchCollection mcLines = _monarchDestroyedProvince.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;
                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.MonarchDestroyedProvince, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine2.Match(rawLine).Value.Replace(". They order it", "").Replace("abandoned province of", "").Trim(); //must have space just incase name contains the word
                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// February 17 of YR6	 The Lords of Utopia have swept down upon the province Land of Vikings (8:7) and crushed it.
        /// </summary>
        private void parseMonarchDestroyedProvince2()
        {
            MatchCollection mcLines = _monarchDestroyedProvince2.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;
                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.MonarchDestroyedProvince, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("upon the province", "").Trim(); //must have space just incase name contains the word
                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }

        /// <summary>
        /// find province left kingdom lines
        /// </summary>
        private void parseProvinceLeftKingdom()
        {
            MatchCollection mcLines = rgxLeftKingdom.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.LeftKingdom, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine4.Match(rawLine).Value.Replace("has been a neglectful leader", "").Trim(); //must have space just incase name contains the word

                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(rgxFindLastProvinceNameInLine.Match(rawLine).Value).Value; //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        private void parseProvinceLeftKingdom3()
        {
            MatchCollection mcLines = rgxLeftKingdom3.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.AbandonedProvince, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine4.Match(rawLine).Value.Replace("has been a neglectful leader", ""); //must have space just incase name contains the word
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine5.Replace(ceItem.Source_Province_Name, "").Trim();

                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// find province left kingdom lines
        /// </summary>
        private void parseProvinceLeftKingdom2()
        {
            MatchCollection mcLines = rgxLeftKingdom2.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.LeftKingdom, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine2.Match(rawLine).Value.Replace("all in the land of", "").Replace("before leaving for", "").Trim(); //must have space just incase name contains the word

                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// finds masacred province line
        /// </summary>
        private void parseMasacredProvince()
        {
            MatchCollection mcLines = rgxMassacred.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.Massacred, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace(" killed", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        if (URegEx.rgxProvinceUnknown.IsMatch(ceItem.Source_Province_Name))
                            ceItem.Source_Province_Name = "Unknown Province";
                        else
                            ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("people within", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        ceItem.value = URegEx.rgxQuantitiesWithComma.Match(rgxFindValueForLine.Match(rawLine).Value).Value.Replace(",", "");

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// finds the masacred province lines
        /// </summary>
        private void parseMassacredProvince2()
        {
            MatchCollection mcLines = rgxMassacred2.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.Massacred, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("invaded", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        if (URegEx.rgxProvinceUnknown.IsMatch(ceItem.Source_Province_Name))
                            ceItem.Source_Province_Name = "Unknown Province";
                        else
                            ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("invaded", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        ceItem.value = URegEx.rgxQuantitiesWithComma.Match(rgxFindValueForLine.Match(rawLine).Value).Value.Replace(",", "");

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// finds attack and stole lines
        /// </summary>
        private void parseAttackAndStoleFrom4()
        {
            MatchCollection mcLines = rgxAttackedAndStole4.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.AttackedAndStole, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("invaded", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        if (URegEx.rgxProvinceUnknown.IsMatch(ceItem.Source_Province_Name))
                            ceItem.Source_Province_Name = "Unknown Province";
                        else
                            ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("invaded and stole from", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// finds attack and stole lines.
        /// </summary>
        private void parseAttackAndStoleFrom3()
        {
            MatchCollection mcLines = rgxAttackedAndStole3.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.AttackedAndStole, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("attacked and pillaged", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        if (URegEx.rgxProvinceUnknown.IsMatch(ceItem.Source_Province_Name))
                            ceItem.Source_Province_Name = "Unknown Province";
                        else
                            ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("pillaged the lands of", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// finds attack and stole from lines
        /// </summary>
        private void parseAttackAndStoleFrom2()
        {
            MatchCollection mcLines = rgxAttackedAndStole2.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.AttackedAndStole, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("invaded", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        if (URegEx.rgxProvinceUnknown.IsMatch(ceItem.Source_Province_Name))
                            ceItem.Source_Province_Name = "Unknown Province";
                        else
                            ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("invaded and pillaged", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// finds attack and stole from lines
        /// </summary>
        private void parseAttackAndStoleFrom()
        {
            MatchCollection mcLines = rgxAttackedAndStole.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.AttackedAndStole, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("attacked and stole from", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        if (URegEx.rgxProvinceUnknown.IsMatch(ceItem.Source_Province_Name))
                            ceItem.Source_Province_Name = "Unknown Province";
                        else
                            ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("attacked and stole from", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// finds province razed lines
        /// </summary>
        private void parseProvinceRazed2()
        {
            MatchCollection mcLines = rgxRazedAcres2.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.RazedProvince, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("invaded", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        if (URegEx.rgxProvinceUnknown.IsMatch(ceItem.Source_Province_Name))
                            ceItem.Source_Province_Name = "Unknown Province";
                        else
                            ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("invaded ", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        ceItem.value = URegEx.rgxQuantitiesWithComma.Match(rgxFindValueForLine.Match(rawLine).Value).Value.Replace(",", "");

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// finds province razed intra kingdom lines
        /// </summary>
        private void parseRazedProvinceIntraKingdom()
        {
            MatchCollection mcLines = rgxRazedAcresIntraKingdom.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;

                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline
                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.RazedProvinceIntraKingdom, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("In local kingdom strife", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        if (URegEx.rgxProvinceUnknown.IsMatch(ceItem.Source_Province_Name))
                            ceItem.Source_Province_Name = "Unknown Province";
                        else
                            ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("invaded", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        ceItem.value = URegEx.rgxNumber.Match(rgxFindValueForLine.Match(rawLine).Value).Value;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// find province razed lines
        /// </summary>
        private void parseProvinceRazed()
        {
            MatchCollection mcLines = rgxRazedAcres.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.RazedProvince, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("razed", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        if (URegEx.rgxProvinceUnknown.IsMatch(ceItem.Source_Province_Name))
                            ceItem.Source_Province_Name = "Unknown Province";
                        else
                            ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("acres of", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        ceItem.value = URegEx.rgxQuantitiesWithComma.Match(rgxFindValueForLine.Match(rawLine).Value).Value.Replace(",", "");

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// finds the dragon flown away lines
        /// </summary>
        private void parseDragonFlownAway()
        {
            MatchCollection mcLines = rgxDragonFlownAway.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.DragonFlownAway, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Source_Kingdom_Island = _cachedKingdom.Kingdoms.Where(x => x.Kingdom_ID == _currentUser.PimpUser.StartingKingdom).FirstOrDefault().Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _cachedKingdom.Kingdoms.Where(x => x.Kingdom_ID == _currentUser.PimpUser.StartingKingdom).FirstOrDefault().Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    {
                        Errors.logError(e);
                    }
                }
            }
        }
        /// <summary>
        /// finds the canceled drgon lines
        /// </summary>
        private void parseCanceledDragon()
        {
            MatchCollection mcLines = rgxCancelledDragonProject.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.OurKingdomCancelledDragon, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("dragon project to", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// finds the start of a gold drgaon line
        /// </summary>
        private void parseStartGoldDragon()
        {
            MatchCollection mcLines = rgxStartedGoldDragonProject.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.DragonProjectStartedGold, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("project targetted at", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// fins the start of a sahpire dragon line
        /// </summary>
        private void parseStartSaphireDragon()
        {
            MatchCollection mcLines = rgxStartedSapphireDragonProjecct.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.DragonProjectStartedSapphire, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("project targetted at", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// finds teh start of a ruby dragon line
        /// </summary>
        private void parseStartRubyDragon()
        {
            MatchCollection mcLines = rgxStartedRubyDragonProject.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.DragonProjectStartedRuby, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("project targetted at", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// finds thes tart of a emerald dragon line
        /// </summary>
        private void parseTheyStartedEmeraldDragon2()
        {
            MatchCollection mcLines = rgxTheyStartedEmeraldDragon.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.StartedGoldDragonProjectAgainstUs, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Target province name, kingdom 
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("has begun a Gold", "");
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Source province name, kingdom etc...
                        ceItem.Target_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// finds teh start of an emeral dragon line
        /// </summary>
        private void parseTheyStartedEmeraldDragon()
        {
            MatchCollection mcLines = rgxTheyStartedEmeraldDragon.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.StartedEmeraldDragonProjectAgainstUs, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Target province name, kingdom 
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("has begun a Emerald", "");
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Source province name, kingdom etc...
                        ceItem.Target_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }

        /// <summary>
        /// finds teh start of an emeral dragon line
        /// </summary>
        private void parseTheyStartedSapphireDragon()
        {
            MatchCollection mcLines = rgxTheyStartedSapphireDragon.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.StartedSapphireDragonProjectAgainstUs, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Target province name, kingdom 
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("has begun a Sapphire", "");
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Source province name, kingdom etc...
                        ceItem.Target_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// finds teh start of an Gold dragon line
        /// </summary>
        private void parseTheyStartedGoldDragon()
        {
            MatchCollection mcLines = rgxTheyStartedGoldDragon.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.StartedGoldDragonProjectAgainstUs, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Target province name, kingdom 
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("has begun a Gold", "");
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Source province name, kingdom etc...
                        ceItem.Target_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }

        /// <summary>
        /// finds the start of an emerald dragon projectline
        /// </summary>
        private void parseStartEmeraldDragonProject()
        {
            MatchCollection mcLines = rgxStartedEmeraldDragonProject.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.DragonProjectStartedEmerald, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("project targetted at", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// finds the they declared war lines
        /// </summary>
        private void parseTheyDeclaredWar()
        {
            MatchCollection mcLines = rgxTheyDeclaredWar.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.TheyDeclaredWar, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("has declared WAR", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = "Our Kingdom";
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// finds they declined ceasefire lines
        /// </summary>
        private void parseDeclinedCeasefire()
        {
            MatchCollection mcLines = rgxTheyDeclinedOurCeaseFireProposal.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.CancelledCeasefireProposalWithOurKingdom, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("has declined our", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = "Our Kingdom";
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }

        private void parseWeRejectedCeasefireOffer()
        {
            MatchCollection mcLines = rgxWeRejectedCeaseFireProposal.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.CancelledCeasefireProposalWithOurKingdom, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("ceasefire offer from", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Source_Province_Name = "Our Kingdom";
                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }

        /// <summary>
        /// finds proposed informal ceasefire lines
        /// </summary>
        private void parseProposedFormalCeasefire2()
        {
            MatchCollection mcLines = rgxProposedCeaseFire.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.ProposedCeasefireWithOurKingdom, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("has proposed a", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = "Our Kingdom";
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// finds the proposed ceasefire lines
        /// </summary>
        private void parseProposedFormalCeasefire()
        {
            MatchCollection mcLines = rgxAcceptedOurCeaseFire.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.ProposedCeasefireWithOurKingdom, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("has proposed a", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = "Our Kingdom";
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// March 18 of YR7 Broken Thrones (1:26) has proposed a Mutual Peace to end our war!
        /// </summary>
        private void parseProposedMutualPeace()
        {
            MatchCollection mcLines = _proposedMutualPeace.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.TheyProposedMutualPeace, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("has accepted our", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = "Our Kingdom";
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// May 1 of YR10 We have proposed a Mutual Peace to end our war with FEARLESS (8:40)! 
        /// </summary>
        private void parseWeProposedMutualPeace()
        {
            MatchCollection mcLines = _proposedWeMutualPeace.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.TheyProposedMutualPeace, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Target province name, kingdom 
                        ceItem.Source_Province_Name = "Our Kingdom";
                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        //gets the Source province name, kingdom etc...
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("end our war with", ""); //must have space just incase name contains the word
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location= Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);


                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// March 1 of YR7 The offer of peace from Unorganized Kingdom (1:36) has been withdrawn. 
        /// </summary>
        private void parseProposedPeaceWithdrawn()
        {
            MatchCollection mcLines = _proposedPeaceWithdrawn.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.TheyProposedPeaceWithdrawn, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("offer of peace from", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = "Our Kingdom";
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// finds the withdrawn from war lines
        /// </summary>
        private void parseWithdrawnFromWar()
        {
            MatchCollection mcLines = rgxTheyWithDrawnFromWar.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.TheyWithDrewFromWar, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("has withdrawn from war", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = "Our Kingdom";
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// finds the broke ceasefire lines
        /// </summary>
        private void parseBrokeCeasefire()
        {
            MatchCollection mcLines = rgxBrokenOurCeaseFire.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.BrokenCeasefireAgreementWithUs, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("has broken their", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = "Our Kingdom";
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// finds the entered formal ceasefire lines
        /// </summary>
        private void parseEnteredFormalCeasfire()
        {
            MatchCollection mcLines = rgxEnteredFormalCeasefire.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.EnteredFormalCeaseFire, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("formal ceasefire with", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// finds the cancelled formal ceasefire lines
        /// </summary>
        private void parseCancelledCeasefire()
        {
            MatchCollection mcLines = rgxCancelledCeaseFire.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.WeCancelledCeasefire, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("our ceasefire with", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// finds proposed ceasefire lines
        /// </summary>
        private void parseProposedCeasefire()
        {
            MatchCollection mcLines = rgxProposedCeaseFire2.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.WeProposedCeasefire, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("ceasefire offer to", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// July 19 of YR5 We have ordered an early end to the post-war period with UnderworID (1:16).
        /// </summary>
        private void parseEarlyEndToPostWar()
        {
            MatchCollection mcLines = _earlyEndPostWar.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.OrderedEarlyEndToPostWar, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("post-war period with", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// finds proposed ceasefire lines
        /// </summary>
        private void parseWithdrawnOurCeasefireProposal()
        {
            MatchCollection mcLines = rgxWithdrawnOurCeaseFireProposal.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.WithDrawnOurCeasefireProposal, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("ceasefire proposal to", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// July 3 of YR6 Brotherhood Rebuilding (2:17) has withdrawn their ceasefire proposal.
        /// </summary>
        private void parseWithdrawnThierCeasefireProposal()
        {
            MatchCollection mcLines = rgxWithdrawnTheirCeaseFireProposal.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.WithDrawnTheirCeasefireProposal, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Target province name, kingdom 
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("has withdrawn their", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);


                        //gets the Source province name, kingdom etc...
                        ceItem.Target_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;


                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        private void parseWithdrawnTheirPeaceOffer()
        {
            MatchCollection mcLines = rgxWithdrawnTheirPeaceOffer.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.WithDrawnTheirPeaceOffer, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Target province name, kingdom 
                        ceItem.Source_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("offer of peace from", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);


                        //gets the Source province name, kingdom etc...
                        ceItem.Target_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;


                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// March 6 of YR12	 We have rejected an offer of Peace by Scary Kingdom (3:4). The war still rages on!
        /// </summary>
        private void parseWithdrawnThierPeaceOffer()
        {
            MatchCollection mcLines =rgxWeRejectedTheirPeaceOffer.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.WeRejectedTheirPeaceOffer, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("rejected an offer of", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);


                     


                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }

        /// <summary>
        /// finds we delcared war on lines
        /// </summary>
        private void parseDeclaredWarOnKingdom()
        {
            MatchCollection mcLines = rgxDeclaredKingdomWar.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.WeDeclaredWar, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("We have declared WAR on", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// find dragon set flight lines
        /// </summary>
        private void parseDragonSetFlight()
        {
            MatchCollection mcLines = rgxDragonSetFlight.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.OurDragonSetFlight, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("Our dragon has set flight to ravage", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Target_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Target_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Target_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Target_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Target_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// find emerald dragon ravaging lands line
        /// </summary>
        private void parseEmeraldDragonRavagingLands()
        {
            MatchCollection mcLines = rgxEmeraldDragonRavagingLands.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.EmeraldDragonRavagingLands, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Target province name, kingdom 
                        ceItem.Source_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("A Ruby Dragon from", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        ceItem.Target_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// finds gold dragon ravaging lands line
        /// </summary>
        private void parseGoldDragonRavagingLands()
        {
            MatchCollection mcLines = rgxGoldDragonRavagingLands.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.GoldDragonRavagingLands, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Target province name, kingdom 
                        ceItem.Source_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("A Gold Dragon from", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        ceItem.Target_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// find ruby dragon ravaging lands line
        /// </summary>
        private void parseRubyDragonRavagingLands()
        {
            MatchCollection mcLines = rgxRubyDragonRavagingLands.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.RubyDragonRavagingLands, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Target province name, kingdom 
                        ceItem.Source_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("A Ruby Dragon from", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        ceItem.Target_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }

        private void parseSapphireDragonRavagingLands()
        {
            MatchCollection mcLines = rgxSapphireDragonRavagingLands.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.SapphireDragonRavagingLands, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Target province name, kingdom 
                        ceItem.Source_Province_Name = rgxFindLastProvinceNameInLine.Match(rawLine).Value.Replace("A Sapphire Dragon from", "");
                        string targetIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption
                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(targetIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(targetIslandLocation).Value).Value);

                        ceItem.Target_Province_Name = "Our Kingdom"; //must have space just incase name contains the word
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// find slain dragon ravaging lands line
        /// </summary>
        private void parseSlainDragonRavagingLands()
        {
            MatchCollection mcLines = rgxSlainDragonRavagingLands.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.SlainDragonRavagingLands, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine3.Match(rawLine).Value.Replace("has slain the dragon ravaging our", "").Trim(); //must have space just incase name contains the word
                        ceItem.Source_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Source_Kingdom_Location = _kingdomOfCe.Kingdom_Location;


                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }
        }
        /// <summary>
        /// find dragon project cancelled line
        /// </summary>
        /// <returns></returns>
        private void parseDragonCancelledAgainstUs()
        {
            MatchCollection mcLines = rgxCancelledDragonTargettedAtUs.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.TheyCancelledTheirDragonProjectTowardsUs, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("has cancelled their dragon", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = "Our Kingdom";
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception e)
                    { Errors.logError(e); }
                }
            }

        }
        /// <summary>
        /// parses to see if a ruby dragon project has started.
        /// </summary>
        private void parseRubyDragonProjectStarted()
        {
            MatchCollection mcLines = rgxStartedRubyDragonAgainstUs.Matches(_rawData);
            if (mcLines.Count > 0)
            {
                for (int i = 0; i < mcLines.Count; i++)
                {
                    try
                    {
                        CS_Code.Utopia_Kingdom_CE ceItem = new CS_Code.Utopia_Kingdom_CE();
                        ceItem.Raw_Line = mcLines[i].Value;

                        string rawLine = mcLines[i].Value;

                        UtopiaDate date = FindUtopianDate(rawLine); //gets the Utopian DateTime in Raw Line.
                        ceItem.Utopia_Year = date.Year;
                        ceItem.Utopia_Month = date.Month;
                        ceItem.Utopia_Date_Day = date.Day;
                        rawLine = URegEx.rgxFindUtopianDateTime.Replace(rawLine, ""); //replaces the Utopian DateTime in rawline

                        ceItem.CE_Type = Sql.GetCeTypeId(CeTypeEnum.RubyDragonProjectAgainstUs, _currentUser.PimpUser.UserID);//gets the CEType then the CEType Id
                        ceItem.DateTime_Added = DateTime.UtcNow;
                        ceItem.Owner_Kingdom_ID = _currentUser.PimpUser.StartingKingdom;
                        ceItem.Kingdom_ID = _kingdomOfCe.Kingdom_ID;
                        ceItem.Province_ID_Added = _currentUser.PimpUser.CurrentActiveProvince;

                        //gets the Source province name, kingdom etc...
                        ceItem.Source_Province_Name = rgxFindFirstProvinceNameInLine.Match(rawLine).Value.Replace("has begun a Ruby", ""); //must have space just incase name contains the word
                        string sourceIslandLocation = URegEx.rgxFindIslandLocation.Match(ceItem.Source_Province_Name).Value; //Cleans up the province name for consumption
                        ceItem.Source_Province_Name = URegEx.rgxFindIslandLocation.Replace(ceItem.Source_Province_Name, "").Trim(); //Cleans up the province name for consumption

                        ceItem.Source_Kingdom_Island = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindIsland.Match(sourceIslandLocation).Value).Value);
                        ceItem.Source_Kingdom_Location = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxFindLocation.Match(sourceIslandLocation).Value).Value);

                        //gets the Target province name, kingdom 
                        ceItem.Target_Province_Name = "Our Kingdom";
                        ceItem.Target_Kingdom_Island = _kingdomOfCe.Kingdom_Island;
                        ceItem.Target_Kingdom_Location = _kingdomOfCe.Kingdom_Location;

                        _rawData = _rawData.Replace(mcLines[i].Value, ""); //removes the line from the CE.
                        _ceList.Add(ceItem); //Adds it to the list of CEs
                    }
                    catch (Exception exception)
                    {
                        Errors.logError(exception);
                    }
                }
            }

        }

        private static UtopiaDate FindUtopianDate(string rawLine)
        {
            UtopiaDate date = new UtopiaDate
            {
                Year = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxYear.Match(rawLine).Value).Value),
                Day = Convert.ToInt32(URegEx.rgxNumber.Match(URegEx.rgxDay.Match(rawLine).Value).Value),
                Month = Formatting.Month(URegEx.rgxMonth.Match(rawLine).Value),
            };
            return date;
        }
    }

}
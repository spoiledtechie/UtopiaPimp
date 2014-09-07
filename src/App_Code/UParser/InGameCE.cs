using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using App_Code.CS_Code.Worker;
using Boomers.Utilities.DatesTimes;
using CS_Code;

using PimpLibrary.Utopia;
using PimpLibrary.Utopia.Ce;
using PimpLibrary.Static.Enums;

namespace Pimp.UParser
{
    /// <summary>
    /// Summary description for UtopiaParserInGame
    /// </summary>
    public partial class UtopiaParser
    {
        #region Regex classes
        private static Regex main = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+[A-Za-z\s()\d+:,]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex topNews = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+\s+to\s+(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #region SourceTargetTypeAmt

        //    /* SOURCE > TARGET > TYPE > AMT
        //*  June 2nd, YR7 Rael (7:36) invaded Cradle to Enslave (20:37) and killed 2055 people. 
        //* February 17th, YR7 Thirsty Mosquito (16:46) invaded the notebook (14:31) and destroyed 98 acres of land.
        //* June 17th, YR7 	In local kingdom strife, Dr Julius Hibbert (18:44) invaded Teluk Intan (18:44) and captured 187 acres of land. [intra kd raze]
        //* June 10th, YR7 	Spirit Of Diablo (14:48) invaded Maggie (18:44) and captured 112 acres of land. [conq]
        //* In local kingdom strife, Prof John Frink (18:44) invaded Vast (18:44) and razed 15 acres of land.
        //* March 1st, YR7 A (3:33) ambushed armies from Merciless Shankster (3:1) and took 83 acres of land. 
        //*/
        #endregion
        private static Regex SourceTargetType = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?(In intra-kingdom war, | In local kingdom strife, )?[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\) (?:invaded|ambushed armies from) [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)\s+and\s+(killed|destroyed|captured|razed|took) \d+\sacres\sof\sland", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        #region SourceTypeAmtTarget
        //    /* SOURCE > TYPE > AMT > TARGET
        //* March 1st, YR7 	darkenwood (17:50) recaptured 89 acres of land from Da Rastamons Hutcore (16:27). [ambush]
        //* February 15th, YR7 Die by Retribution (12:28) razed 63 acres of Green Midget (16:20). 
        //* June 6th, YR7 	Maggie (18:44) captured 104 acres of land from christmas of peasant (14:48). [trad march]
        //June 9th, YR1 Filthy Camper (25:36) captured 89 acres of land from  Knights of Solamnia (10:36).
        //* February 14th, YR7 	Simply Marx (20:31) killed 2427 people within GREAT Nightmare (8:42).
        //* February 7th, YR7 	HitsujidoshiPhobia (9:39), captured 45 acres of land from Ghetto Fantasy (5:20).
        //*/
        #endregion
        private static Regex SourceTypeAmountTarget = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?(In local kingdom strife, )?([a-zA-Z\s\d\-'’_]{0,35}\((\#?\d+:\#?\d+)\)|An unknown province from [a-zA-Z\s\d\-'’_]{0,35}\((\#?\d+:\#?\d+)\))( recaptured| razed| captured|, captured| killed) [\d,]+ (?:acre(s)? of land|people within|acres of|)\s+(?:of\s+|from\s+|within\s+)?[a-zA-Z\s\d\-'’_]{0,35}\((\#?\d+:\#?\d+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex SourceTargetTypeAmount = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?(In local kingdom strife(,)? )?([a-zA-Z\s\d\-'’_]{0,35}\((\#?\d+:\#?\d+)\)|An unknown province from [a-zA-Z\s\d\-'’_]{0,35}\((\#?\d+:\#?\d+)\))( recaptured| razed| captured|, captured| killed| invaded)\s[a-zA-Z\s\d\-'’_]{0,35}\((\#?\d+:\#?\d+)\)\s(and\srazed\s|and\scaptured\s|and\skilled\s)[\d,]+\s(people|acre(s)?\sof\sland)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        #region SourceTypeTarget
        //        /* SOURCE > TYPE > TARGET
        //* June 17th, YR7 	Mire of the Damned (5:12) invaded and pillaged Teluk Intan (18:44) [pillage]
        //* June 10th, YR7 	Sabretooth (12:48) invaded and stole from Moes Magic Mushrooms (18:44). [learn]
        //* June 10th, YR7 	The Eclipse (18:44) has sent an aid shipment to Prof John Frink (18:44). [aid]
        //* February 15th, YR7 King of Moonshine (5:44) attempted an invasion of Fallen Lucifer (1:26), but was repelled. 
        //* February 14th, YR7 In intra-kingdom war, Empire of Shell (14:11) attempted to invade Camelyn (14:11), but failed. 
        //* February 17th, YR7 An Unknown Province from 1:26 invaded and pillaged King of Randomness (5:44).
        //source > type > target
        //source > type > amt > target
        #endregion
        private static Regex SourceTypeTarget = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?(In intra-kingdom war )?(An unknown province from [a-zA-Z\d\-\!_][_a-zA-Z\d\-\! ]{0,35}[_a-zA-Z\d\-\!]|[a-zA-Z\d\-\!_][a-zA-Z\d\-\! _]{0,35}[a-zA-Z\d\-\!_]|[a-zA-Z\d\-\!_]|)\s*\(?(\d+:\d+)\)?\s* (attacked and stole from|invaded and pillaged|attacked and pillaged the lands of|invaded and stole from|has sent an aid shipment to|attempted an invasion of|attempted to invade our|attempted to invade) [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static Regex aidShipment = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?[a-zA-Z\s\d\-_]{0,35} has sent an aid shipment to [a-zA-Z\s\d\-_]{0,35}\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex aidShipmentSourceProvName = new Regex(@"[a-zA-Z\s\d\-_]{0,35} has sent an aid shipment to", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex aidShipmentTargetProvName = new Regex(@"has sent an aid shipment to [a-zA-Z\s\d\-_]{0,35}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static Regex CanceledDragonProject = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?(Our kingdom has cancelled the dragon project to)?[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)( has cancelled their dragon project targetted at us)?\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);//Canceled Dragon Project.
        private static Regex StartDragonProject = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?Our kingdom has begun a (Emerald|Gold|Sapphire|Ruby) Dragon project targetted at [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);//January 7th, YR2 Our kingdom has begun a Emerald Dragon project targetted at WAR STAYOUT (2:35).
        private static Regex StartDragonProjectAtHome = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\) has begun a (Emerald|Gold|Sapphire|Ruby) Dragon project against us", RegexOptions.IgnoreCase | RegexOptions.Compiled);//June 17th, YR3  free (13:30) has begun a Ruby Dragon project targetted against us!
        private static Regex DragonRavagingLands = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?A (Emerald|Gold|Sapphire|Ruby) Dragon from [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)\s+has begun ravaging our lands", RegexOptions.IgnoreCase | RegexOptions.Compiled);        //July 1st, YR3   A Ruby Dragon from War stay out (24:1) has begun ravaging our lands!
        private static Regex dragonSetFlight = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?Our\sdragon\shas\sset\sflight\sto\sravage\s[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        #region WarStats
        //War Stats
        ////January 21st, YR12 We have declared war on (30:4). The war will begin on February 12, YR12.
        ///* (relations from us)
        //* June 2nd, YR7 	We have cancelled all relations with Pwnage Reincarnated (3:38).
        //June 3rd, YR7 	We have declared Spirits Of Legends (14:48) to be a Hostile Kingdom!
        //* March 15th, YR7 We have declared (12:33) to be a Hostile Kingdom! 
        //June 3rd, YR7 	We have declared a state of Peace with Marvelous (12:48).
        //*  February 8th, YR7 Our Kingdom has surrendered to The Plagues of IMP (7:14). Our failed war has finally ended! 
        //* May 1st, YR7 Our kingdom has begun a dragon project targetted at Casus Belli of HaLL (7:36).
        //* February 8th, YR7 Our dragon has set flight to ravage ULTIMATE WARRIORS (10:43)!  
        //* March 3rd, YR7 We have accepted an offer of Peace by IMMORTALS TDC (16:36). The people celebrate the end of our War! 
        //* March 7th, YR7 	Our kingdom has cancelled the dragon project to .
        //* Our Kingdom has withdrawn from Armoured Ascension (23:13). The war has ended!

        //January 17th, YR0 We have proposed a ceasefire offer to The Most Notorious (14:44).
        //January 24th, YR0    We have entered into a formal ceasefire with WeAreNotGoodPeople (4:8).
        //January 16th, YR0    We have rejected a ceasefire offer from Hall of Heroes (1:48).
        //January 4th, YR0    We have declared WAR on Last Words (2:3). War is expected to commence on January 16th, YR0.

        //July 19th, YR0     Unable to achieve victory, our Kingdom has withdrawn from war with insertkingdomnamehere (xx:xx). Our failed war has finally ended!

        //June 15th, YR0 A war has started with Pwn or be Pwned (9:10)!

        //June 10th, YR5 We have achieved victory in our war with Fear our Dragon WAR (13:3). 

        //July 17th, YR12 We have cancelled our ceasefire with Worn down body partS (14:21)!

        // July 10th, YR9  Our war with xXNuBsInWaiTinGXxWAR (4:4) has been cancelled. Please see the intro page for details.
        //*/
        #endregion
        private static Regex WarStats = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?(A war|Our kingdom has|We have|Our dragon has|Unable to|Our Kingdom has)? (declared WAR on|withdrawn from|accepted an offer of Peace by|set flight to ravage|cancelled all relations with|declared a state of Peace with|declared|surrendered to|begun a dragon project targetted at|proposed a ceasefire offer to|rejected a ceasefire offer from|entered into a formal ceasefire with|achieve victory\, our Kingdom has withdrawn from war with|has started with|achieved victory in our war with|cancelled our ceasefire with|Our war with|proposed a Mutual Peace to end our war with|ordered an early end to the post-war period with|The offer of peace from|We have rejected an offer of Peace by)(\s+)?([a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)\.?)?( The war still rages on!)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        //June 10th, YR5 Our war with The Imperial Guards (7:4) has ended in a stalemate.
        private static Regex StaleMate = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?(Our) (war with) [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\) has ended in a stalemate", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        #region ProvinceDoingSomething
        //    /* 
        //* (province a doing something)
        //* June 6th, YR7 	Alas, the once proud province of ReturnoftheDaYWalkeR (18:44) has collapsed and lies in ruins. [die]
        //*  February 16th, YR7 Rider of the Wind (25:2) has defected to MMOrder of HeroesMM (25:2)! [defect out]
        //*  February 7th, YR7 DJ Land (1:12) has slain the dragon ravaging our lands! 
        //*/ 
        #endregion
        private static Regex SlainDragon = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?[a-zA-Z\s\d\-_]{0,35}(\((\#?\d+:\#?\d+)\))? has slain the dragon ravaging our lands", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex DragonFlownAway = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?The dragon ravaging our lands has flown away", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex slainDragonProvince = new Regex(@"([a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)|[a-zA-Z\d\-\!']['a-zA-Z\d\-\! ]{0,35})?has slain the", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex ProvinceDead = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?Alas, (the truant|the once proud province of) [a-zA-Z\s\d\-_\.]+(\((\#?\d+:\#?\d+)\))? collapsed and lies in ruins", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex DefectedProvince = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?(?:The province of |The leader of )?[a-zA-Z\s\d\-_]{0,35} (has defected to us from|has defected to|has chosen to join) [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)(\.\sAll in [a-zA-Z\s\d\-_]{0,35} gather their possessions and depart this kingdom forever)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex DefectedProvinceSlipped = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?(Staying in the darkest shadows, )?[a-zA-Z\s\d\-_]{0,35} (slips out of) [a-zA-Z\s\d\-_]{0,35} unnoticed, never to be seen again", RegexOptions.IgnoreCase | RegexOptions.Compiled);


        //March 16 of YR2 As the ultimate betrayal, King Kong destroys all in the land of CrazyMonkey before leaving for a new kingdom.
        //March 7 of YR3 The leader of TOFO has wisely chosen to join us from The Unnamed Kingdom (3:33).
        private static Regex DefectedProvinceOne = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?(?:The province of |The leader of |As the ultimate betrayal, )?[a-zA-Z\s\d\-_]{0,35} (destroys all in the land of|has wisely chosen to join us from) [a-zA-Z\s\d\-_]{0,35}(\((\#?\d+:\#?\d+)\))?(before leaving for a new kingdom)?\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex DefectedProvinceSource = new Regex(@"(The province of |The leader of |destroys all in the land of |Staying in the darkest shadows, )?[a-zA-Z\s\d\-_]{0,35} (has|before|slips out)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex DefectedProvinceTarget = new Regex(@"(chosen to join us from|to us from|defected to) [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        #region StartEndWarPeaceComments
        //    /*  (relations to us)
        //* February 2nd, YR9 	Fallen INQ TDC (21:48) has ended their state of hostility with us.
        //* June 24th, YR7 	Elemental Ghetto (15:40) has ended their official state of peace with us.
        //*  February 5th, YR7 Fallen BenevolenceKA (1:26) has declared us to be a Hostile Kingdom! 
        //* February 7th, YR7	Heroes of DT (3:4) has proposed a Mutual Peace to end our war!
        //* February 15th, YR7 	Hostilities with Just Us HaJ (3:8) have degraded into a state of WAR!
        //* February 22nd, YR7 	CandyLand (12:23) has declared Peace with us.
        //* February 24th, YR7 	Endless Errors (23:33) has surrendered to us! Our people rejoice at our victory!
        //*  April 9th, YR7 The Asylum (12:44) has entered a state of war with The Violent Family (23:44), cancelling our relations with them! 
        //*  June 2nd, YR7 A Dragon from Casus Belli of HaLL (7:36) has begun ravaging our lands!
        //*  February 11th, YR7 gypsy caravan (14:20) has cancelled their mutual peace proposal to end our War! 
        //* March 15th, YR7 Lothlorien IMP (22:43) has accepted our offer of Peace. The people celebrate the end of our War!  
        // May 17th, YR1 Sands of Time (22:30) has declared WAR with our kingdom! War is expected to commence on June 4th, YR1. 

        //January 4th, YR0 Saints United 1943 (19:43) has proposed a formal ceasefire with our kingdom.
        //January 5th, YR0 Saints United 1943 (19:43) has withdrawn their ceasefire proposal. 
        //January 7th, YR0 PantaRei (10:1) has declined our ceasefire proposal! 
        //January 17th, YR0 Domination Nation 1 (17:14) has accepted our ceasefire proposal!

        //July 19th, YR12 RABID GERBILS (7:2) has broken their ceasefire agreement with us!

        ////June 1st, YR1 bugz at WAR (4:19) has withdrawn from war. Our people rejoice at our victory!

        //June 10th, YR5 Godless (4:16) has defeated us soundly in war.
        #endregion
        private static Regex StartEndWarPeace = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?[a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)\s+(?:has|have) (accepted our offer|cancelled|ended their state|ended their official|declared us|proposed a Mutual Peace to end our war!|degraded|declared Peace|surrendered|entered|begun|proposed a formal|withdrawn their|declined our|accepted our ceasefire proposal!|accepted our ceasefire|declared WAR|withdrawn from war\.?|defeated us soundly|broken their ceasefire)\s+(ceasefire\sproposal!|ceasefire\swith\sour\skingdom\.)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static Regex FindProvinceNameCE = new Regex(@"(dragon project to |attempted to invade\s|end our war with\s|rejected an offer of Peace by |The offer of peace from\s|acres\sof\s|ceasefire\swith\s|stole\sfrom\s|invaded\s|pillaged\s|people\swithin\s|to\sus\sfrom\s|province\sof\s|attempted\sto\sinvade\sour\s|attempted\san\sinvasion\sof\s|Dragon\sfrom\s|project\stargetted\sat\s|\sto\sravage\s|have declared\sWAR\son\s|ravage\sthe\s|an\sinvasion\sof\s|to\sinvade\sour\s|an\said\sshipment\sto\s|period with\s|acres\sof land from\s|proud\sprovince\sof\s|and pillaged\sthe\slands\sof\s)?(YR\d+\s+)?(An unknown province from [a-zA-Z\d\-\!'_][_'a-zA-Z\d\-\! ]{0,35}\s*\(?(\d+:\d+)\)?|[a-zA-Z\d\-\!'_]['a-zA-Z\d\-\! _]{0,35}\s*\(?(\d+:\d+)\)?|-\sAwaiting Activation\s-|[a-zA-Z\d\-\!'_][_'a-zA-Z\d\-\! ]{0,35} has collapsed|has lead [a-zA-Z\d\-\!'_][_'a-zA-Z\d\-\! ]{0,35} into a state of neglect)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex FindAidProvinceName = new Regex(@"(January|February|March|April|May|June|July|August|September|October|Novemeber|December)\s\d{0,2}\sof\sYR\d+(\s+)?[a-zA-Z\d\-\!][a-zA-Z\d\-\! ]{0,35} has sent an aid shipment to [a-zA-Z\d\-\!][a-zA-Z\d\-\! ]{0,35}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex SourceTypeTargetCheck = new Regex(@"\s+(In intra-kingdom war, )?([a-zA-Z\d\-\!][a-zA-Z\d\-\! ]{0,22}[a-zA-Z\d\-\!]|[a-zA-Z\d\-\!]|)\s*\(?(\d+:\d+)\)?\s* has sent an aid shipment to [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex SourceTargetTypeCheck = new Regex(@"(In intra-kingdom war, | In local kingdom strife, ) [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\) (?:invaded|ambushed armies from) [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)\s+and\s+(killed|destroyed|captured|razed|took) \d{0,6}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex ProvinceDeadCheck = new Regex(@"Alas, the once proud province of [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\) has collapsed", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex rgxFindCEType = new Regex(@"(has collapsed|attacked and pillaged|killed|destroyed|captured|recaptured|attacked|razed|ambushed|invaded and pillaged|invaded and stole|learned from|aid shipment|attempted an invasion|attempted to invade|failed to|cancelled the dragon|kingdom has begun|Dragon project against us|begun ravaging our lands|slain the dragon|dragon has set flight|declared WAR|cancelled all relations|Hostile Kingdom|state of Peace|offer of Peace|surrendered|Unable to achieve victory|withdrawn|proposed a ceasefire|formal ceasefire|accepted our ceasefire proposal|rejected a ceasefire|cancelled our ceasefire|broken their ceasefire|A war has started|victory in our war|surrendered to us|stalemate|collapsed and lies in ruins|defected to|ended their state of hostility|official state of peace|declared Peace with us|declared us to be a Hostile Kingdom|proposed a Mutual Peace|degraded into a state of WAR|declined\sour\sceasefire|gather their possessions|never to be seen again|cancelled their dragon project|destroys all in the land|early end to the post-war|wisely chosen to join us|dragon ravaging our lands has flown away|slips out of)", RegexOptions.Compiled | RegexOptions.IgnoreCase);


        private static Regex rgxRazed = new Regex(@"\d+\sacres\sof", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex rgxReplaceLine = new Regex(@"( recaptured| razed| captured|, captured| killed| invaded| ambushed armies from)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex rgxUnknownProv = new Regex(@"( Unknown Province | unknown province )", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex FindCEAmount = new Regex(@"(killed|destroyed|recaptured|captured|razed|took)\s\d+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static Regex rgxCheckOwner = new Regex(@"'s of kingdom news from [a-zA-Z\s\d\-_]{0,35}\((\#?\d+:\#?\d+)\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion
        static DateTime now = DateTime.UtcNow;

        /// <summary>
        /// Parses the In GAME CE Page to extract information and use.
        /// </summary>
        /// <param name="RawData"></param>
        /// <param name="ClickedFrom">Page of which the item was uploaded from.</param>
        /// <param name="ProvinceName">Province Name of Person uploading.</param>
        /// <param name="ServerID">Server ID</param>
        /// <returns></returns>    
        //private static string ParseGameCE(string RawData, PimpUserWrapper  currentUser, OwnedKingdomProvinces cachedKingdom)
        //{
        //    Notification notification;
        //    NotificationDetail notificationDetail;
        //    Notifier notifier = new Notifier();
        //    #region ActionsArray
        //    //$actionTrans = array('killed' => 'massacre', 'destroyed' => 'raze', 'captured' => 'tradmarch', 'recaptured' => 'ambush', 'razed' => 'raze', ', captured' => 'conquest',
        //    //                    'invaded and pillaged' => 'plunder', 'invaded and stole from' => 'learn', 'has sent an aid shipment to' => 'aid', 
        //    //                    'attempted an invasion of'=>'bounce', 'attempted to invade' => 'bounce', 'attempted to invade our' => 'bounce',
        //    //                    'has defected to us from' => 'defectin', 'has defected to' => 'defectout', 'attacked and stole from' => 'learn',
        //    //                    'attacked and pillaged the lands of' => 'plunder','set flight to ravage' => 'senddragon','took' => 'ambush',
        //    //                    'cancelled the dragon project to' => 'canceldragonout','begun a dragon project targetted at' => 'begindragonout','begun' => 'senddragon',

        //    //                    'cancelled' => 'withdraw-mpin','accepted an offer of peace by' => 'end-war-mp','ended their official' => 'end-peace','declared a state of peace with' => 'begin-peace',
        //    //                    'accepted our offer' => 'end-war-mp','proposed a mutual' => 'offer-mp', 'declared' => 'begin-hostile', 'ended their state' => 'end-hostile',
        //    //                    'declared us' => 'begin-hostile', 'declared peace' => 'begin-peace',
        //    //                    'withdrawn from' => 'end-war-withdraw','surrendered' => 'end-war-surrender','surrendered to' => 'end-war-surrender',
        //    //                    'degraded' => 'begin-war', 'entered' => 'begin-otherwar', 'cancelled all relations with' => 'end-all', 
        //    //                    'provdie' => 'provdie', 'slaydragon' => 'slaydragon',
        //    //                    'proposed a ceasefire offer to' => 'propose-cf', 'rejected a ceasefire offer from' => 'reject-cf', 'declared war on' => 'begin-war-date',
        //    //                    'proposed a formal' => 'propose-cf', 'withdrawn their' => 'withdraw-cf','declined our' => 'reject-cf', 'accepted our ceasefire' => 'accept-cf',
        //    //                    'entered into a formal ceasefire with' => 'enter-cf', 'declared war' => 'begin-war-date', 
        //    //                    'withdrawn from war' => 'withdraw-war',
        //    //                    'achieve victory, our kingdom has withdrawn from war with' => 'withdraw-war',
        //    //                    'has started with' => 'begin-war',
        //    //                    'achieved victory in our war with' => 'end-war-victory',
        //    //                    'defeated us soundly' => 'end-war-defeat', 'broken their ceasefire' => 'break-cf', 'cancelled our ceasefire with' => 'cancel-cf', 'our war with' => 'cancel-war'

        //    //                    );
        //    #endregion

        //    int Amount = 0;
        //    MatchCollection MC;

        //    string CE_Type, item, TargetProvinceName, SourceProvinceName = string.Empty;
        //    var Source_Kingdom = new UtopiaLocation(0, 0);
        //    var Target_Kingdom = new UtopiaLocation(0, 0);

        //    // Regex spellCheck = new Regex(@"Your wizards gather their runes and begin casting", RegexOptions.Compiled | RegexOptions.IgnoreCase); // Not in use
        //    Guid startingProvince = currentUser.PimpUser.CurrentActiveProvince;

        //    UtopiaDataContext db = new UtopiaDataContext();
        //    Guid kingdomId;

        //    if (rgxCheckOwner.Match(RawData).Success)
        //    {
        //        string owner = rgxCheckOwner.Match(RawData).Value.Replace("'s of kingdom news from", "").Trim();
        //        UtopiaLocation kdLocation = new UtopiaLocation();
        //        kdLocation.Island = Convert.ToInt32(Static.rgxFindIsland.Match(Static.rgxFindIslandLocation.Match(owner).Value).Value.Replace(":", ""));
        //        kdLocation.Kingdom = Convert.ToInt32(Static.rgxFindLocation.Match(Static.rgxFindIslandLocation.Match(owner).Value).Value.Replace(":", ""));
        //        try
        //        {
        //            kingdomId = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom).Kingdoms
        //                                                                                    .Where(x => x.Kingdom_Island == kdLocation.Island && x.Kingdom_Location == kdLocation.Kingdom)
        //                //.Where(x => x.Kingdom_Location == kdloc)
        //                                                                                    .FirstOrDefault().Kingdom_ID;
        //        }
        //        catch
        //        {
        //            return "0,Couldn't find " + owner + " in the list of your kingdoms.";
        //        }
        //    }
        //    else
        //        kingdomId = currentUser.PimpUser.StartingKingdom;

        //    // Removes the beginning of the CE until it finds a date.
        //    RawData = RawData.Remove(0, Static.rgxFindUtopianDateTime.Match(RawData).Index);

        //    if (topNews.IsMatch(RawData))
        //        RawData = RawData.Replace(topNews.Match(RawData).Value, "");

        //    List<CERawCache> ceCache = (from UKCECI in db.Utopia_Kingdom_CEs
        //                                where UKCECI.Owner_Kingdom_ID == currentUser.PimpUser.StartingKingdom
        //                                where UKCECI.DateTime_Added >= now.AddHours(TAKE_DATA_WITHIN_HOURS)
        //                                select new CERawCache
        //                                {
        //                                    uid = UKCECI.uid,
        //                                    month = UKCECI.Utopia_Month,
        //                                    year = UKCECI.Utopia_Year,
        //                                    sProvinceName = UKCECI.Source_Province_Name,
        //                                    sKingIsland = (int)UKCECI.Source_Kingdom_Island,
        //                                    sKingLocation = (int)UKCECI.Source_Kingdom_Location,
        //                                    tProvinceName = UKCECI.Target_Province_Name,
        //                                    tKingIsland = (int)UKCECI.Target_Kingdom_Island,
        //                                    tKingLocation = (int)UKCECI.Target_Kingdom_Location,
        //                                    ceType = UKCECI.CE_Type,
        //                                    value = UKCECI.value
        //                                }).ToList();

        //    // Matches: (In intra-kingdom war, | In local kingdom strife, ) (?:invaded|ambushed armies from) (killed|destroyed|captured|razed|took)           
        //    // March 6 of YR12	 Ydrunken (6:2) invaded Brutus (4:40) and captured 141 acres of land.
        //    #region Enemy invades your kingdom. Only named provinces.
        //    MatchCollection mcLines = SourceTargetType.Matches(RawData);
        //    for (int i = 0; i < mcLines.Count; i++) //same code as SourceTargetType, SourceTypeAmountTarget, and SourceTargetTypeAmount        
        //    {
        //        Amount = 0;
        //        item = mcLines[i].Value; // Returns the current line from the CE that matches

        //        UtopiaDate date = new UtopiaDate
        //        {
        //            Year = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxYear.Match(item).Value).Value),
        //            Month = Formatting.Month(Static.rgxMonth.Match(item).Value),
        //            Day = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxDay.Match(item).Value).Value)
        //        };

        //        Utopia_Kingdom_CE UKCE = StartCreateKingdomCE(currentUser.PimpUser.StartingKingdom, startingProvince, date, item);

        //        item = Static.rgxFindUtopianDateTime.Replace(item, "").Trim();
        //        CE_Type = GetCEActionType(item, currentUser.PimpUser.UserID); // Get the type of action, ex. razed, captured, aid, killed etc

        //        string itemCheck = item;
        //        itemCheck = itemCheck.Replace(rgxReplaceLine.Match(itemCheck).Value, ""); // Remove the date, ex. January 15

        //        // Finds the province names
        //        MC = FindProvinceNameCE.Matches(itemCheck);

        //        // Check if the name is unknown (or nothing), otherwise use the name from the CE
        //        // Ex. An unknown province from --Sanctuary FUn War-- (6:2) invaded Augustus (4:40) and captured 438 acres of land.
        //        switch (rgxUnknownProv.IsMatch(MC[0].Value))
        //        {
        //            case false:
        //                SourceProvinceName = Static.rgxFindIslandLocation.Replace(MC[0].Value, "").Trim();
        //                break;
        //            default:
        //                SourceProvinceName = "Unknown Province";
        //                break;
        //        }
        //        switch (CE_Type)
        //        {
        //            case "razed":
        //                TargetProvinceName = Static.rgxFindIslandLocation.Replace(MC[1].Value, "").Trim();
        //                switch (rgxRazed.IsMatch(MC[1].Value))
        //                {
        //                    case true: TargetProvinceName = rgxRazed.Replace(MC[1].Value, "").Trim();
        //                        break;
        //                }
        //                break;
        //            default:
        //                TargetProvinceName = Static.rgxFindIslandLocation.Replace(MC[1].Value, "").Replace("acres of land from", "").Replace("people within", "").Trim();
        //                break;
        //        }

        //        Source_Kingdom.Island = Convert.ToInt32(Static.rgxFindIsland.Match(Static.rgxFindIslandLocation.Match(MC[0].Value).Value).Value.Replace(":", ""));
        //        Source_Kingdom.Kingdom = Convert.ToInt32(Static.rgxFindLocation.Match(Static.rgxFindIslandLocation.Match(MC[0].Value).Value).Value.Replace(":", ""));
        //        Target_Kingdom.Island = Convert.ToInt32(Static.rgxFindIsland.Match(Static.rgxFindIslandLocation.Match(MC[1].Value).Value).Value.Replace(":", ""));
        //        Target_Kingdom.Kingdom = Convert.ToInt32(Static.rgxFindLocation.Match(Static.rgxFindIslandLocation.Match(MC[1].Value).Value).Value.Replace(":", ""));
        //        Amount = Convert.ToInt32(Static.rgxNumber.Match(FindCEAmount.Match(item).Value).Value);

        //        // Sets when the army expires. An unknown attack is expected to last the default time, 12 hours.
        //        SetArmyExpires(now, SourceProvinceName, CE_Type, Source_Kingdom.Island, Source_Kingdom.Kingdom, currentUser.PimpUser.StartingKingdom, db, UKCE);

        //        UKCE.Source_Province_Name = SourceProvinceName;
        //        UKCE.Source_Kingdom_Island = Source_Kingdom.Island;
        //        UKCE.Source_Kingdom_Location = Source_Kingdom.Kingdom;
        //        UKCE.Target_Province_Name = TargetProvinceName;
        //        UKCE.Target_Kingdom_Island = Target_Kingdom.Island;
        //        UKCE.Target_Kingdom_Location = Target_Kingdom.Kingdom;
        //        // Get the type of picture to be displayed on the CE page.
        //        UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);
        //        UKCE.value = Amount.ToString();
        //        UKCE.Kingdom_ID = kingdomId;

        //        // Check if the data is in the database/cache, otherwise add it.
        //        bool result = CheckCECache(db, ceCache, UKCE);
        //        if (result)
        //        {
        //            // Get the targetted province
        //            var province = cachedKingdom.Provinces.Find(x => x.Province_Name == TargetProvinceName && x.Owner_User_ID != null);
        //            if (province != null)
        //            {
        //                notification = new Notification(); // Notification class, collect and sends the data as an email.
        //                notification.ProvinceId = province.Province_ID;
        //                notification.ProvinceName = province.Province_Name;
        //                notification.UserId = province.Owner_User_ID.Value;

        //                notificationDetail = new NotificationDetail();
        //                notificationDetail.Attacker = new AttackerData { Location = Source_Kingdom, Name = SourceProvinceName };
        //                notificationDetail.Date = new UtopiaDate { Year = date.Year, Month = date.Month, Day = date.Day };
        //                notificationDetail.EventText = "he took " + Amount.ToString() + " acers.";
        //                notificationDetail.EventType = EventType.CaputeredLand;
        //                notificationDetail.Location = Target_Kingdom;
        //                notification.Details.Add(notificationDetail);

        //                notifier.SendNotification(notification);
        //            }
        //        }

        //        RawData = RawData.Replace(UKCE.Raw_Line, "");
        //    }
        //    #endregion

        //    // June 3 of YR12 Octavian has sent an aid shipment to Britannia.
        //    #region Aid shipments
        //    mcLines = aidShipment.Matches(RawData); //Same code as startingdragonproject and Dragon Set Flight, and Canceled Dragon project.
        //    for (int i = 0; i < mcLines.Count; i++)
        //    {
        //        item = mcLines[i].Value; // Returns the current line from the CE that matches

        //        UtopiaDate date = new UtopiaDate
        //        {
        //            Year = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxYear.Match(item).Value).Value),
        //            Month = Formatting.Month(Static.rgxMonth.Match(item).Value),
        //            Day = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxDay.Match(item).Value).Value)
        //        };

        //        Utopia_Kingdom_CE UKCE = StartCreateKingdomCE(currentUser.PimpUser.StartingKingdom, startingProvince, date, item);


        //        item = Static.rgxFindUtopianDateTime.Replace(item, "").Trim();  // Remove the date, ex. January 15
        //        CE_Type = GetCEActionType(item, currentUser.PimpUser.UserID); // Get the type of action, ex. razed, captured, aid, killed etc

        //        SourceProvinceName = aidShipmentSourceProvName.Match(item).Value.Replace("has sent an aid shipment to", "").Trim();
        //        TargetProvinceName = aidShipmentTargetProvName.Match(item).Value.Replace("has sent an aid shipment to", "").Trim();

        //        var uki = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom, currentUser.PimpUser.StartingKingdom, cachedKingdom);

        //        UKCE.Target_Kingdom_Island = uki.Kingdom_Island;
        //        UKCE.Target_Kingdom_Location = uki.Kingdom_Location;
        //        UKCE.Source_Kingdom_Island = uki.Kingdom_Island;
        //        UKCE.Source_Kingdom_Location = uki.Kingdom_Location;

        //        UKCE.Target_Province_Name = TargetProvinceName;
        //        UKCE.Source_Province_Name = SourceProvinceName;

        //        // Get the type of picture to be displayed on the CE page.
        //        UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);
        //        UKCE.Kingdom_ID = kingdomId;

        //        // Check if the data is in the database/cache, otherwise add it.
        //        CheckCECache(db, ceCache, UKCE);
        //        RawData = RawData.Replace(UKCE.Raw_Line, "");
        //    }
        //    #endregion

        //    // (In local kingdom strife, )|An unknown province from ( recaptured| razed| captured|, captured| killed) (?:acre(s)? of land|people within|acres of|)(?:of\s+|from\s+|within\s+)
        //    // March 7 of YR12	 Patrician (4:40) captured 104 acres of land from Just plain drunk (6:2).
        //    // March 8 of YR12	 An unknown province from funshow WAR (4:40) captured 220 acres of land from Wrenched (6:2).
        //    #region Attacks from kingdom. Both known and unknown provinces.
        //    mcLines = SourceTypeAmountTarget.Matches(RawData);
        //    for (int i = 0; i < mcLines.Count; i++) //same code as SourceTargetType, SourceTypeAmountTarget, and SourceTargetTypeAmount
        //    {
        //        Amount = 0;
        //        item = mcLines[i].Value; // Returns the current line from the CE that matches

        //        UtopiaDate date = new UtopiaDate
        //        {
        //            Year = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxYear.Match(item).Value).Value),
        //            Month = Formatting.Month(Static.rgxMonth.Match(item).Value),
        //            Day = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxDay.Match(item).Value).Value)
        //        };

        //        Utopia_Kingdom_CE UKCE = StartCreateKingdomCE(currentUser.PimpUser.StartingKingdom, startingProvince, date, item);

        //        item = Static.rgxFindUtopianDateTime.Replace(item, "").Trim();
        //        CE_Type = GetCEActionType(item, currentUser.PimpUser.UserID); // Get the type of action, ex. razed, captured, aid, killed etc

        //        string itemCheck = item;
        //        itemCheck = itemCheck.Replace(rgxReplaceLine.Match(itemCheck).Value, "");

        //        // Finds the province names
        //        MC = FindProvinceNameCE.Matches(itemCheck);

        //        // Check if the name is unknown (or nothing), otherwise use the name from the CE
        //        // Ex. An unknown province from --Sanctuary FUn War-- (6:2) invaded Augustus (4:40) and captured 438 acres of land.
        //        switch (rgxUnknownProv.IsMatch(MC[0].Value))
        //        {
        //            case false:
        //                SourceProvinceName = Static.rgxFindIslandLocation.Replace(MC[0].Value, "").Trim();
        //                break;
        //            default:
        //                SourceProvinceName = "Unknown Province";
        //                break;
        //        }
        //        switch (CE_Type)
        //        {
        //            case "razed":
        //                TargetProvinceName = Static.rgxFindIslandLocation.Replace(MC[1].Value, "").Trim();
        //                switch (rgxRazed.IsMatch(MC[1].Value))
        //                {
        //                    case true: TargetProvinceName = rgxRazed.Replace(MC[1].Value, "").Trim();
        //                        break;
        //                }
        //                break;
        //            default:
        //                Regex rgxkilledPeople = new Regex(@"(\d+\s)?(people within|acres of land from)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        //                TargetProvinceName = Static.rgxFindIslandLocation.Replace(MC[1].Value, "").Trim();
        //                if (rgxkilledPeople.IsMatch(TargetProvinceName))
        //                    TargetProvinceName = TargetProvinceName.Replace(rgxkilledPeople.Match(TargetProvinceName).Value, "").Trim();
        //                break;
        //        }

        //        Source_Kingdom.Island = Convert.ToInt32(Static.rgxFindIsland.Match(Static.rgxFindIslandLocation.Match(MC[0].Value).Value).Value.Replace(":", ""));
        //        Source_Kingdom.Kingdom = Convert.ToInt32(Static.rgxFindLocation.Match(Static.rgxFindIslandLocation.Match(MC[0].Value).Value).Value.Replace(":", ""));
        //        Target_Kingdom.Island = Convert.ToInt32(Static.rgxFindIsland.Match(Static.rgxFindIslandLocation.Match(MC[1].Value).Value).Value.Replace(":", ""));
        //        Target_Kingdom.Kingdom = Convert.ToInt32(Static.rgxFindLocation.Match(Static.rgxFindIslandLocation.Match(MC[1].Value).Value).Value.Replace(":", ""));
        //        Amount = Convert.ToInt32(Static.rgxNumber.Match(FindCEAmount.Match(item).Value).Value);

        //        // Sets when the army expires. An unknown attack is expected to last the default time, 12 hours.
        //        SetArmyExpires(now, SourceProvinceName, CE_Type, Source_Kingdom.Island, Source_Kingdom.Kingdom, currentUser.PimpUser.StartingKingdom, db, UKCE);

        //        UKCE.Source_Province_Name = SourceProvinceName;
        //        UKCE.Source_Kingdom_Island = Source_Kingdom.Island;
        //        UKCE.Source_Kingdom_Location = Source_Kingdom.Kingdom;
        //        UKCE.Target_Province_Name = TargetProvinceName;
        //        UKCE.Target_Kingdom_Island = Target_Kingdom.Island;
        //        UKCE.Target_Kingdom_Location = Target_Kingdom.Kingdom;
        //        // Get the type of picture to be displayed on the CE page.
        //        UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);
        //        UKCE.value = Amount.ToString();
        //        UKCE.Kingdom_ID = kingdomId;

        //        // Check if the data is in the database/cache, otherwise add it.
        //        CheckCECache(db, ceCache, UKCE);
        //        RawData = RawData.Replace(UKCE.Raw_Line, "");
        //    }
        //    #endregion

        //    // (In local kingdom strife(,)? ) |An unknown province from ( recaptured| razed| captured|, captured| killed| invaded)(and\srazed\s|and\scaptured\s|and\skilled\s)(people|acre(s)?\sof\sland)
        //    // March 19 of YR12	 An unknown province from --Sanctuary FUn War-- (6:2) invaded Aeneid (4:40) and captured 437 acres of land.
        //    // May 16 of YR12	 Fitshaced (6:2) invaded Spartacus (4:40) and killed 2,039 people
        //    #region Enemy unknown province attacks. Enemy invaded and killed people.
        //    mcLines = SourceTargetTypeAmount.Matches(RawData);
        //    for (int i = 0; i < mcLines.Count; i++) //same code as SourceTargetType, SourceTypeAmountTarget, and SourceTargetTypeAmount
        //    {
        //        Amount = 0;
        //        item = mcLines[i].Value;

        //        UtopiaDate date = new UtopiaDate
        //        {
        //            Year = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxYear.Match(item).Value).Value),
        //            Month = Formatting.Month(Static.rgxMonth.Match(item).Value),
        //            Day = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxDay.Match(item).Value).Value)
        //        };

        //        Utopia_Kingdom_CE UKCE = StartCreateKingdomCE(currentUser.PimpUser.StartingKingdom, startingProvince, date, item);

        //        item = Static.rgxFindUtopianDateTime.Replace(item, "").Trim();
        //        CE_Type = GetCEActionType(item, currentUser.PimpUser.UserID);

        //        string itemCheck = item;
        //        itemCheck = itemCheck.Replace(rgxReplaceLine.Match(itemCheck).Value, "");

        //        MC = FindProvinceNameCE.Matches(itemCheck);

        //        switch (rgxUnknownProv.IsMatch(MC[0].Value))
        //        {
        //            case false:
        //                SourceProvinceName = Static.rgxFindIslandLocation.Replace(MC[0].Value, "").Trim();
        //                break;
        //            default:
        //                SourceProvinceName = "Unknown Province";
        //                break;
        //        }
        //        switch (CE_Type)
        //        {
        //            case "razed":
        //                TargetProvinceName = Static.rgxFindIslandLocation.Replace(MC[1].Value, "").Trim();
        //                switch (rgxRazed.IsMatch(MC[1].Value))
        //                {
        //                    case true: TargetProvinceName = rgxRazed.Replace(MC[1].Value, "").Trim();
        //                        break;
        //                }
        //                break;
        //            default:
        //                TargetProvinceName = Static.rgxFindIslandLocation.Replace(MC[1].Value, "").Replace("acres of land from", "").Replace("people within", "").Trim();
        //                break;
        //        }

        //        Source_Kingdom.Island = Convert.ToInt32(Static.rgxFindIsland.Match(Static.rgxFindIslandLocation.Match(MC[0].Value).Value).Value.Replace(":", ""));
        //        Source_Kingdom.Kingdom = Convert.ToInt32(Static.rgxFindLocation.Match(Static.rgxFindIslandLocation.Match(MC[0].Value).Value).Value.Replace(":", ""));
        //        Target_Kingdom.Island = Convert.ToInt32(Static.rgxFindIsland.Match(Static.rgxFindIslandLocation.Match(MC[1].Value).Value).Value.Replace(":", ""));
        //        Target_Kingdom.Kingdom = Convert.ToInt32(Static.rgxFindLocation.Match(Static.rgxFindIslandLocation.Match(MC[1].Value).Value).Value.Replace(":", ""));
        //        Amount = Convert.ToInt32(Static.rgxNumber.Match(FindCEAmount.Match(item).Value).Value);

        //        SetArmyExpires(now, SourceProvinceName, CE_Type, Source_Kingdom.Island, Source_Kingdom.Kingdom, currentUser.PimpUser.StartingKingdom, db, UKCE);

        //        UKCE.Source_Province_Name = SourceProvinceName;
        //        UKCE.Source_Kingdom_Island = Source_Kingdom.Island;
        //        UKCE.Source_Kingdom_Location = Source_Kingdom.Kingdom;
        //        UKCE.Target_Province_Name = TargetProvinceName;
        //        UKCE.Target_Kingdom_Island = Target_Kingdom.Island;
        //        UKCE.Target_Kingdom_Location = Target_Kingdom.Kingdom;
        //        UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);
        //        UKCE.value = Amount.ToString();
        //        UKCE.Kingdom_ID = kingdomId;

        //        bool result = CheckCECache(db, ceCache, UKCE);

        //        if (result)
        //        {
        //            // Get the targetted province
        //            var province = cachedKingdom.Provinces.Find(x => x.Province_Name == TargetProvinceName && x.Owner_User_ID != null);
        //            if (province != null)
        //            {
        //                notification = new Notification(); // Notification class, collect and sends the data as an email.
        //                notification.ProvinceId = province.Province_ID;
        //                notification.ProvinceName = province.Province_Name;
        //                notification.UserId = province.Owner_User_ID.Value;

        //                notificationDetail = new NotificationDetail();
        //                notificationDetail.Attacker = new AttackerData { Location = Source_Kingdom, Name = SourceProvinceName };
        //                notificationDetail.Date = new UtopiaDate { Year = date.Year, Month = date.Month, Day = date.Day };
        //                if (CE_Type == "killed")
        //                    notificationDetail.EventText = "he killed " + Amount.ToString() + " people within your lands.";
        //                else if (CE_Type == "captured")
        //                    notificationDetail.EventText = "he captured " + Amount.ToString() + " acers.";
        //                else if (CE_Type == "razed")
        //                    notificationDetail.EventText = "he razed " + Amount.ToString() + " acers.";
        //                else
        //                    notificationDetail.EventText = "unknown event, please report what the true event is to funkypiffo@gmail.com"; // No idea if this is ever hit.
        //                notificationDetail.EventType = EventType.CaputeredLand;
        //                notificationDetail.Location = Target_Kingdom;
        //                notification.Details.Add(notificationDetail);

        //                notifier.SendNotification(notification);
        //            }
        //        }

        //        RawData = RawData.Replace(UKCE.Raw_Line, "");
        //    }
        //    #endregion

        //    // (In intra-kingdom war )?(An unknown province from | or NAMED PROVINCE) (attacked and stole from|invaded and pillaged|attacked and pillaged the lands of|invaded and stole from|has sent an aid shipment to|attempted an invasion of|attempted to invade our|attempted to invade)
        //    // May 9 of YR12 SipSippin with Screw (6:2) attempted to invade Britannia (4:40).
        //    // May 16 of YR12 An unknown province from funshow WAR (4:40) attempted an invasion of Fitshaced (6:2), but was repelled.
        //    // March 3 of YR12 Cicero (4:40) attempted an invasion of Embalmed (6:2)}
        //    // June 1 of YR0	 ICE Twice as nice (3:40) attempted an invasion of Shades of Tron (4:40)}
        //    // June 12 of YR0 non orcs are noob (7:26) attacked and pillaged the lands of IceIcebaby (3:40)}
        //    #region Attemped invasions from both sides. Pillaged from both sides.
        //    mcLines = SourceTypeTarget.Matches(RawData);
        //    for (int i = 0; i < mcLines.Count; i++)
        //    {
        //        item = mcLines[i].Value;

        //        UtopiaDate date = new UtopiaDate
        //        {
        //            Year = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxYear.Match(item).Value).Value),
        //            Month = Formatting.Month(Static.rgxMonth.Match(item).Value),
        //            Day = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxDay.Match(item).Value).Value)
        //        };

        //        Utopia_Kingdom_CE UKCE = StartCreateKingdomCE(currentUser.PimpUser.StartingKingdom, startingProvince, date, item);

        //        item = Static.rgxFindUtopianDateTime.Replace(item, "").Trim();
        //        CE_Type = GetCEActionType(item, currentUser.PimpUser.UserID);

        //        MC = FindProvinceNameCE.Matches(item);
        //        switch (rgxUnknownProv.IsMatch(MC[0].Value))
        //        {
        //            case false:
        //                SourceProvinceName = Static.rgxYear.Replace(Static.rgxFindIslandLocation.Replace(MC[0].Value, ""), "").Trim();
        //                break;
        //            default:
        //                SourceProvinceName = "Unknown Province";
        //                break;
        //        }
        //        Source_Kingdom.Island = Convert.ToInt32(Static.rgxFindIsland.Match(Static.rgxFindIslandLocation.Match(MC[0].Value).Value).Value.Replace(":", ""));
        //        Source_Kingdom.Kingdom = Convert.ToInt32(Static.rgxFindLocation.Match(Static.rgxFindIslandLocation.Match(MC[0].Value).Value).Value.Replace(":", ""));
        //        TargetProvinceName = Static.rgxFindIslandLocation.Replace(MC[1].Value, "").Replace("invaded and stole from", "").Replace("stole from", "").Replace("attempted to invade", "").Replace("attempted to invade our", "").Replace("attempted an invasion of", "").Replace("an invasion of", "").Replace("an aid shipment to", "").Replace("and pillaged the lands of", "").Replace("to invade our", "").Replace("invaded and pillaged", "").Replace("pillaged", "").Trim();
        //        Target_Kingdom.Island = Convert.ToInt32(Static.rgxFindIsland.Match(Static.rgxFindIslandLocation.Match(MC[1].Value).Value).Value.Replace(":", ""));
        //        Target_Kingdom.Kingdom = Convert.ToInt32(Static.rgxFindLocation.Match(Static.rgxFindIslandLocation.Match(MC[1].Value).Value).Value.Replace(":", ""));

        //        SetArmyExpires(now, SourceProvinceName, CE_Type, Source_Kingdom.Island, Source_Kingdom.Kingdom, currentUser.PimpUser.StartingKingdom, db, UKCE);

        //        UKCE.Source_Province_Name = SourceProvinceName;
        //        UKCE.Source_Kingdom_Island = Source_Kingdom.Island;
        //        UKCE.Source_Kingdom_Location = Source_Kingdom.Kingdom;
        //        UKCE.Target_Province_Name = TargetProvinceName;
        //        UKCE.Target_Kingdom_Island = Target_Kingdom.Island;
        //        UKCE.Target_Kingdom_Location = Target_Kingdom.Kingdom;
        //        UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);
        //        UKCE.Kingdom_ID = kingdomId;

        //        bool result = CheckCECache(db, ceCache, UKCE);

        //        if (result)
        //        {
        //            var province = cachedKingdom.Provinces.Find(x => x.Province_Name == TargetProvinceName && x.Owner_User_ID != null);
        //            if (province != null)
        //            {
        //                notification = new Notification(); // Notification class, collect and sends the data as an email.
        //                notification.ProvinceId = province.Province_ID;
        //                notification.ProvinceName = province.Province_Name;
        //                notification.UserId = province.Owner_User_ID.Value;

        //                notificationDetail = new NotificationDetail();
        //                notificationDetail.Attacker = new AttackerData { Location = Source_Kingdom, Name = SourceProvinceName };
        //                notificationDetail.Date = new UtopiaDate { Year = date.Year, Month = date.Month, Day = date.Day };
        //                if (CE_Type == "pillaged")
        //                    notificationDetail.EventText = "he managed to overrun our defences and pillaged your lands.";
        //                else
        //                    notificationDetail.EventText = "but his forces was no match for your defenders.";
        //                notificationDetail.EventType = EventType.CaputeredLand;
        //                notificationDetail.Location = Target_Kingdom;
        //                notification.Details.Add(notificationDetail);

        //                notifier.SendNotification(notification);
        //            }
        //        }

        //        RawData = RawData.Replace(UKCE.Raw_Line, "");
        //    }
        //    #endregion

        //    // February 8 of YR7 Our dragon has set flight to ravage ULTIMATE WARRIORS (10:43)
        //    #region Dragon set flight
        //    mcLines = dragonSetFlight.Matches(RawData);//Same code as startingdragonproject and Dragon Set Flight, and Canceled Dragon project.
        //    for (int i = 0; i < mcLines.Count; i++)
        //    {
        //        Amount = 0;
        //        item = mcLines[i].Value;
        //        switch (FindProvinceNameCE.Match(item).Success) // Matches kingdom name, NOT province name
        //        {
        //            case true:
        //                CS_Code.Utopia_Kingdom_CE UKCE = new CS_Code.Utopia_Kingdom_CE();
        //                UKCE.DateTime_Added = now;
        //                UKCE.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
        //                UKCE.Province_ID_Added = startingProvince;
        //                UKCE.Utopia_Month = Formatting.Month(Static.rgxMonth.Match(item).Value);
        //                UKCE.Utopia_Year = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxYear.Match(item).Value).Value);
        //                UKCE.Utopia_Date_Day = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxDay.Match(item).Value).Value);
        //                UKCE.Raw_Line = item;
        //                item = Static.rgxFindUtopianDateTime.Replace(item, "").Trim();
        //                CE_Type = GetCEActionType(item, currentUser.PimpUser.UserID);

        //                Amount = SelectDragonType(item, currentUser.PimpUser.UserID);

        //                TargetProvinceName = FindProvinceNameCE.Match(item).Value.Replace("to ravage", "").Replace("project targetted at", "").Replace("ravage the", "").Trim();
        //                Target_Kingdom.Island = Convert.ToInt32(Static.rgxFindIsland.Match(Static.rgxFindIslandLocation.Match(TargetProvinceName).Value).Value.Replace(":", ""));
        //                Target_Kingdom.Kingdom = Convert.ToInt32(Static.rgxFindLocation.Match(Static.rgxFindIslandLocation.Match(TargetProvinceName).Value).Value.Replace(":", ""));
        //                TargetProvinceName = Static.rgxFindIslandLocation.Replace(TargetProvinceName, "").Trim();

        //                UKCE.Target_Province_Name = TargetProvinceName;
        //                UKCE.Target_Kingdom_Island = Target_Kingdom.Island;
        //                UKCE.Target_Kingdom_Location = Target_Kingdom.Kingdom;
        //                UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);
        //                UKCE.value = Amount.ToString();
        //                UKCE.Kingdom_ID = kingdomId;

        //                CheckCECache(db, ceCache, UKCE);
        //                RawData = RawData.Replace(UKCE.Raw_Line, "");
        //                break;
        //        }
        //    }
        //    #endregion

        //    // No data present
        //    #region Canceled dragon project
        //    mcLines = CanceledDragonProject.Matches(RawData);//Same code as startingdragonproject and Dragon Set Flight, and Canceled Dragon project.
        //    for (int i = 0; i < mcLines.Count; i++)
        //    {
        //        item = mcLines[i].Value;
        //        switch (FindProvinceNameCE.Match(item).Success) // Matches kingdom name, NOT province name
        //        {
        //            case true:
        //                CS_Code.Utopia_Kingdom_CE UKCE = new CS_Code.Utopia_Kingdom_CE();
        //                UKCE.DateTime_Added = now;
        //                UKCE.Owner_Kingdom_ID = currentUser.PimpUser.StartingKingdom;
        //                UKCE.Province_ID_Added = startingProvince;
        //                UKCE.Utopia_Month = Formatting.Month(Static.rgxMonth.Match(item).Value);
        //                UKCE.Utopia_Year = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxYear.Match(item).Value).Value);
        //                UKCE.Utopia_Date_Day = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxDay.Match(item).Value).Value);
        //                UKCE.Raw_Line = item;
        //                item = Static.rgxFindUtopianDateTime.Replace(item, "").Trim();
        //                CE_Type = GetCEActionType(item, currentUser.PimpUser.UserID);

        //                TargetProvinceName = FindProvinceNameCE.Match(item).Value.Replace("dragon project to", "").Replace("to ravage", "").Replace("project targetted at", "").Replace("ravage the", "").Trim();
        //                Target_Kingdom.Island = Convert.ToInt32(Static.rgxFindIsland.Match(Static.rgxFindIslandLocation.Match(TargetProvinceName).Value).Value.Replace(":", ""));
        //                Target_Kingdom.Kingdom = Convert.ToInt32(Static.rgxFindLocation.Match(Static.rgxFindIslandLocation.Match(TargetProvinceName).Value).Value.Replace(":", ""));
        //                TargetProvinceName = Static.rgxFindIslandLocation.Replace(TargetProvinceName, "").Trim();

        //                UKCE.Target_Province_Name = TargetProvinceName;
        //                UKCE.Target_Kingdom_Island = Target_Kingdom.Island;
        //                UKCE.Target_Kingdom_Location = Target_Kingdom.Kingdom;
        //                UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);
        //                UKCE.Kingdom_ID = kingdomId;

        //                CheckCECache(db, ceCache, UKCE);
        //                RawData = RawData.Replace(UKCE.Raw_Line, "");
        //                break;
        //        }
        //    }
        //    #endregion

        //    // No data present
        //    #region Enemy starts a dragon project against kingdom
        //    mcLines = StartDragonProject.Matches(RawData); //Same code as startingdragonproject and Dragon Set Flight, and Canceled Dragon project.
        //    for (int i = 0; i < mcLines.Count; i++)
        //    {
        //        Amount = 0;
        //        item = mcLines[i].Value;
        //        switch (FindProvinceNameCE.Match(item).Success) // Matches kingdom name, NOT province name
        //        {
        //            case true:
        //                UtopiaDate date = new UtopiaDate
        //                {
        //                    Year = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxYear.Match(item).Value).Value),
        //                    Month = Formatting.Month(Static.rgxMonth.Match(item).Value),
        //                    Day = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxDay.Match(item).Value).Value)
        //                };

        //                Utopia_Kingdom_CE UKCE = StartCreateKingdomCE(currentUser.PimpUser.StartingKingdom, startingProvince, date, item);

        //                item = Static.rgxFindUtopianDateTime.Replace(item, "").Trim();
        //                CE_Type = GetCEActionType(item, currentUser.PimpUser.UserID);
        //                Amount = SelectDragonType(item, currentUser.PimpUser.UserID);

        //                TargetProvinceName = FindProvinceNameCE.Match(item).Value.Replace("to ravage", "").Replace("project targetted at", "").Replace("ravage the", "").Trim();
        //                Target_Kingdom.Island = Convert.ToInt32(Static.rgxFindIsland.Match(Static.rgxFindIslandLocation.Match(TargetProvinceName).Value).Value.Replace(":", ""));
        //                Target_Kingdom.Kingdom = Convert.ToInt32(Static.rgxFindLocation.Match(Static.rgxFindIslandLocation.Match(TargetProvinceName).Value).Value.Replace(":", ""));
        //                TargetProvinceName = Static.rgxFindIslandLocation.Replace(TargetProvinceName, "").Trim();

        //                UKCE.Target_Province_Name = TargetProvinceName;
        //                UKCE.Target_Kingdom_Island = Target_Kingdom.Island;
        //                UKCE.Target_Kingdom_Location = Target_Kingdom.Kingdom;
        //                UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);
        //                UKCE.value = Amount.ToString();
        //                UKCE.Kingdom_ID = kingdomId;

        //                CheckCECache(db, ceCache, UKCE);
        //                RawData = RawData.Replace(UKCE.Raw_Line, "");
        //                break;
        //        }
        //    }
        //    #endregion

        //    // No data present
        //    #region Start dragon project against an enemy
        //    mcLines = StartDragonProjectAtHome.Matches(RawData); //same code as dragon ravaging lands
        //    for (int i = 0; i < mcLines.Count; i++)
        //    {
        //        item = mcLines[i].Value;
        //        UtopiaDate date = new UtopiaDate
        //        {
        //            Year = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxYear.Match(item).Value).Value),
        //            Month = Formatting.Month(Static.rgxMonth.Match(item).Value),
        //            Day = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxDay.Match(item).Value).Value)
        //        };

        //        Utopia_Kingdom_CE UKCE = StartCreateKingdomCE(currentUser.PimpUser.StartingKingdom, startingProvince, date, item);

        //        item = Static.rgxFindUtopianDateTime.Replace(item, "").Trim();
        //        CE_Type = GetCEActionType(item, currentUser.PimpUser.UserID);

        //        SourceProvinceName = Static.rgxYear.Replace(FindProvinceNameCE.Match(item).Value.Replace("Dragon from", "").Trim(), "");
        //        Source_Kingdom.Island = Convert.ToInt32(Static.rgxFindIsland.Match(Static.rgxFindIslandLocation.Match(SourceProvinceName).Value).Value.Replace(":", ""));
        //        Source_Kingdom.Kingdom = Convert.ToInt32(Static.rgxFindLocation.Match(Static.rgxFindIslandLocation.Match(SourceProvinceName).Value).Value.Replace(":", ""));
        //        SourceProvinceName = Static.rgxFindIslandLocation.Replace(SourceProvinceName, "").Trim();

        //        UKCE.Source_Province_Name = SourceProvinceName;
        //        UKCE.Source_Kingdom_Island = Source_Kingdom.Island;
        //        UKCE.Source_Kingdom_Location = Source_Kingdom.Kingdom;
        //        UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);
        //        UKCE.value = SelectDragonType(item, currentUser.PimpUser.UserID).ToString();
        //        UKCE.Kingdom_ID = kingdomId;

        //        bool result = CheckCECache(db, ceCache, UKCE);

        //        if (result)
        //        {
        //            var provinces = cachedKingdom.Provinces.Where(x => x.Owner_User_ID != null);
        //            if (provinces.Count() > 0)
        //            {
        //                foreach (var province in provinces)
        //                {
        //                    notification = new Notification(); // Notification class, collect and sends the data as an email.
        //                    notification.ProvinceId = province.Province_ID;
        //                    notification.ProvinceName = province.Province_Name;
        //                    notification.UserId = province.Owner_User_ID.Value;

        //                    notificationDetail = new NotificationDetail();
        //                    notificationDetail.Date = new UtopiaDate { Year = date.Year, Month = date.Month, Day = date.Day };
        //                    notificationDetail.EventText = "We have started a dragon project.";
        //                    notificationDetail.EventType = EventType.DragonProjectStarted;
        //                    notificationDetail.Location = Source_Kingdom;
        //                    notification.Details.Add(notificationDetail);

        //                    notifier.SendNotification(notification);
        //                }
        //            }
        //        }

        //        RawData = RawData.Replace(UKCE.Raw_Line, "");
        //    }
        //    #endregion

        //    // No data present
        //    #region Dragon ravaging lands
        //    mcLines = DragonRavagingLands.Matches(RawData); //same code as starting dragon at home
        //    for (int i = 0; i < mcLines.Count; i++)
        //    {
        //        item = mcLines[i].Value;
        //        UtopiaDate date = new UtopiaDate
        //        {
        //            Year = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxYear.Match(item).Value).Value),
        //            Month = Formatting.Month(Static.rgxMonth.Match(item).Value),
        //            Day = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxDay.Match(item).Value).Value)
        //        };

        //        Utopia_Kingdom_CE UKCE = StartCreateKingdomCE(currentUser.PimpUser.StartingKingdom, startingProvince, date, item);

        //        item = Static.rgxFindUtopianDateTime.Replace(item, "").Trim();
        //        CE_Type = GetCEActionType(item, currentUser.PimpUser.UserID);

        //        SourceProvinceName = Static.rgxYear.Replace(FindProvinceNameCE.Match(item).Value.Replace("Dragon from", "").Trim(), "");
        //        Source_Kingdom.Island = Convert.ToInt32(Static.rgxFindIsland.Match(Static.rgxFindIslandLocation.Match(SourceProvinceName).Value).Value.Replace(":", ""));
        //        Source_Kingdom.Kingdom = Convert.ToInt32(Static.rgxFindLocation.Match(Static.rgxFindIslandLocation.Match(SourceProvinceName).Value).Value.Replace(":", ""));
        //        SourceProvinceName = Static.rgxFindIslandLocation.Replace(SourceProvinceName, "").Trim();

        //        UKCE.Source_Province_Name = SourceProvinceName;
        //        UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);
        //        UKCE.Source_Kingdom_Island = Source_Kingdom.Island;
        //        UKCE.Source_Kingdom_Location = Source_Kingdom.Kingdom;
        //        UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);
        //        UKCE.value = SelectDragonType(item, currentUser.PimpUser.UserID).ToString();
        //        UKCE.Kingdom_ID = kingdomId;

        //        bool result = CheckCECache(db, ceCache, UKCE);

        //        if (result)
        //        {
        //            var provinces = cachedKingdom.Provinces.Where(x => x.Owner_User_ID != null);
        //            if (provinces.Count() > 0)
        //            {
        //                foreach (var province in provinces)
        //                {
        //                    notification = new Notification(); // Notification class, collect and sends the data as an email.
        //                    notification.ProvinceId = province.Province_ID;
        //                    notification.ProvinceName = province.Province_Name;
        //                    notification.UserId = province.Owner_User_ID.Value;

        //                    notificationDetail = new NotificationDetail();
        //                    notificationDetail.Date = new UtopiaDate { Year = date.Year, Month = date.Month, Day = date.Day };
        //                    notificationDetail.EventText = "A dragon is ravaging our lands.";
        //                    notificationDetail.EventType = EventType.DragonRavagingLands;
        //                    notificationDetail.Location = Source_Kingdom;
        //                    notification.Details.Add(notificationDetail);

        //                    notifier.SendNotification(notification);
        //                }
        //            }
        //        }

        //        RawData = RawData.Replace(UKCE.Raw_Line, "");
        //    }
        //    #endregion

        //    // June 3 of YR7 	We have declared a state of Peace with Marvelous (12:48).
        //    // June 15 of YR0 A war has started with Pwn or be Pwned (9:10)
        //    // June 2 of YR7 	We have cancelled all relations with Pwnage Reincarnated (3:38).
        //    // February 8 of YR7 Our Kingdom has surrendered to The Plagues of IMP (7:14).
        //    // June 10 of YR5 We have achieved victory in our war with Fear our Dragon WAR (13:3).
        //    // January 24 of YR0    We have entered into a formal ceasefire with WeAreNotGoodPeople (4:8).
        //    // March 3 of YR7 We have accepted an offer of Peace by IMMORTALS TDC (16:36).
        //    // January 17 of YR0 We have proposed a ceasefire offer to The Most Notorious (14:44).
        //    #region War actions like started, ended, ceasefire, state of peace, surrender
        //    mcLines = WarStats.Matches(RawData);
        //    for (int i = 0; i < mcLines.Count; i++)
        //    {
        //        item = mcLines[i].Value;
        //        CE_Type = GetCEActionType(item, currentUser.PimpUser.UserID);
        //        if (CE_Type == "peace" || CE_Type == "startpeace" || CE_Type == "endEarlyPostWar" || CE_Type == "cancelledceasefire" || CE_Type == "declaredwar" || CE_Type == "cancelledrelations" || CE_Type == "hostilekingdom" || CE_Type == "peace" || CE_Type == "surrendered" || CE_Type == "withdrawn" || CE_Type == "rejectedceasefire" || CE_Type == "ceasefire" || CE_Type == "warwin" || CE_Type == "startwar")
        //        {
        //            UtopiaDate date = new UtopiaDate
        //            {
        //                Year = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxYear.Match(item).Value).Value),
        //                Month = Formatting.Month(Static.rgxMonth.Match(item).Value),
        //                Day = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxDay.Match(item).Value).Value)
        //            };

        //            Utopia_Kingdom_CE UKCE = StartCreateKingdomCE(currentUser.PimpUser.StartingKingdom, startingProvince, date, item);

        //            item = Static.rgxFindUtopianDateTime.Replace(item, "").Trim();

        //            TargetProvinceName = FindProvinceNameCE.Match(item).Value.Replace("rejected an offer of Peace by", "").Replace("The offer of peace from", "").Replace("end our war with", "").Replace("period with", "").Replace("ceasefire with", "").Replace("have declared WAR on", "").Replace("a ceasefire offer to", "").Trim();
        //            Target_Kingdom.Island = Convert.ToInt32(Static.rgxFindIsland.Match(Static.rgxFindIslandLocation.Match(TargetProvinceName).Value).Value.Replace(":", ""));
        //            Target_Kingdom.Kingdom = Convert.ToInt32(Static.rgxFindLocation.Match(Static.rgxFindIslandLocation.Match(TargetProvinceName).Value).Value.Replace(":", ""));
        //            TargetProvinceName = Static.rgxFindIslandLocation.Replace(TargetProvinceName, "").Trim();

        //            UKCE.Target_Province_Name = TargetProvinceName;
        //            UKCE.Target_Kingdom_Island = Target_Kingdom.Island;
        //            UKCE.Target_Kingdom_Location = Target_Kingdom.Kingdom;
        //            UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);
        //            UKCE.Kingdom_ID = kingdomId;

        //            bool result = CheckCECache(db, ceCache, UKCE);

        //            if (result)
        //            {
        //                if (CE_Type == "endEarlyPostWar" || CE_Type == "declaredwar" || CE_Type == "surrendered" || CE_Type == "withdrawn" || CE_Type == "ceasefire" || CE_Type == "warwin" || CE_Type == "startwar")
        //                {
        //                    var provinces = cachedKingdom.Provinces.Where(x => x.Owner_User_ID != null);
        //                    if (provinces.Count() > 0)
        //                    {
        //                        foreach (var province in provinces)
        //                        {
        //                            notification = new Notification(); // Notification class, collect and sends the data as an email.
        //                            notification.ProvinceId = province.Province_ID;
        //                            notification.ProvinceName = province.Province_Name;
        //                            notification.UserId = province.Owner_User_ID.Value;

        //                            notificationDetail = new NotificationDetail();
        //                            notificationDetail.Attacker = new AttackerData { Location = Source_Kingdom, Name = SourceProvinceName };
        //                            notificationDetail.Date = new UtopiaDate { Year = date.Year, Month = date.Month, Day = date.Day };
        //                            if (CE_Type == "endEarlyPostWar")
        //                                notificationDetail.EventText = "The war has ended.";
        //                            else if (CE_Type == "declaredwar")
        //                                notificationDetail.EventText = "We have declared war.";
        //                            else if (CE_Type == "surrendered")
        //                                notificationDetail.EventText = "We have surrendered.";
        //                            else if (CE_Type == "withdrawn")
        //                                notificationDetail.EventText = "We have withdrawn from this war.";
        //                            else if (CE_Type == "ceasefire")
        //                                notificationDetail.EventText = "We have proposed a ceasefire/accepted a proposed ceasefire.";
        //                            else if (CE_Type == "warwin")
        //                                notificationDetail.EventText = "The war has ended with a victory, our people rejoice.";
        //                            else if (CE_Type == "startwar")
        //                                notificationDetail.EventText = "We have started a war.";
        //                            //notificationDetail.EventType = EventType.WarAction;
        //                            notificationDetail.Location = Target_Kingdom;
        //                            notification.Details.Add(notificationDetail);

        //                            notifier.SendNotification(notification);
        //                        }
        //                    }
        //                }
        //            }

        //            RawData = RawData.Replace(UKCE.Raw_Line, "");
        //        }
        //        else { FailedAt("WarStatsIsMatchCETypeCheck", CE_Type + "; " + item, currentUser.PimpUser.UserID); }
        //    }
        //    #endregion

        //    // No data present
        //    #region Stale mate
        //    mcLines = StaleMate.Matches(RawData);
        //    for (int i = 0; i < mcLines.Count; i++)
        //    {
        //        item = mcLines[i].Value;
        //        CE_Type = GetCEActionType(item, currentUser.PimpUser.UserID);

        //        UtopiaDate date = new UtopiaDate
        //        {
        //            Year = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxYear.Match(item).Value).Value),
        //            Month = Formatting.Month(Static.rgxMonth.Match(item).Value),
        //            Day = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxDay.Match(item).Value).Value)
        //        };

        //        Utopia_Kingdom_CE UKCE = StartCreateKingdomCE(currentUser.PimpUser.StartingKingdom, startingProvince, date, item);

        //        item = Static.rgxFindUtopianDateTime.Replace(item, "").Trim();

        //        TargetProvinceName = _findKingdomProvinceName.Match(item).Value;
        //        Target_Kingdom.Island = Convert.ToInt32(Static.rgxFindIsland.Match(Static.rgxFindIslandLocation.Match(TargetProvinceName).Value).Value.Replace(":", ""));
        //        Target_Kingdom.Kingdom = Convert.ToInt32(Static.rgxFindLocation.Match(Static.rgxFindIslandLocation.Match(TargetProvinceName).Value).Value.Replace(":", ""));
        //        TargetProvinceName = Static.rgxFindIslandLocation.Replace(TargetProvinceName, "").Trim();

        //        UKCE.Target_Province_Name = TargetProvinceName;
        //        UKCE.Target_Kingdom_Island = Target_Kingdom.Island;
        //        UKCE.Target_Kingdom_Location = Target_Kingdom.Kingdom;
        //        UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);
        //        UKCE.Kingdom_ID = kingdomId;

        //        CheckCECache(db, ceCache, UKCE);
        //        RawData = RawData.Replace(UKCE.Raw_Line, "");
        //    }
        //    #endregion

        //    // No data present
        //    #region Dragon slain
        //    mcLines = SlainDragon.Matches(RawData);  //same code as province dead
        //    for (int i = 0; i < mcLines.Count; i++)
        //    {
        //        item = mcLines[i].Value;
        //        CE_Type = GetCEActionType(item, currentUser.PimpUser.UserID);

        //        UtopiaDate date = new UtopiaDate
        //        {
        //            Year = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxYear.Match(item).Value).Value),
        //            Month = Formatting.Month(Static.rgxMonth.Match(item).Value),
        //            Day = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxDay.Match(item).Value).Value)
        //        };

        //        Utopia_Kingdom_CE UKCE = StartCreateKingdomCE(currentUser.PimpUser.StartingKingdom, startingProvince, date, item);

        //        item = Static.rgxFindUtopianDateTime.Replace(item, "").Trim();

        //        SourceProvinceName = Static.rgxYear.Replace(slainDragonProvince.Match(item).Value.Replace("has slain the", "").Trim(), "").Trim();
        //        if (Static.rgxFindIslandLocation.IsMatch(SourceProvinceName))
        //        {
        //            Source_Kingdom.Island = Convert.ToInt32(Static.rgxFindIsland.Match(Static.rgxFindIslandLocation.Match(SourceProvinceName).Value).Value.Replace(":", ""));
        //            Source_Kingdom.Kingdom = Convert.ToInt32(Static.rgxFindLocation.Match(Static.rgxFindIslandLocation.Match(SourceProvinceName).Value).Value.Replace(":", ""));
        //            SourceProvinceName = Static.rgxFindIslandLocation.Replace(SourceProvinceName, "").Trim();
        //        }
        //        else
        //        {
        //            var uki = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom, kingdomId, cachedKingdom);

        //            Source_Kingdom.Island = uki.Kingdom_Island;
        //            Source_Kingdom.Kingdom = uki.Kingdom_Location;
        //        }

        //        UKCE.Source_Province_Name = SourceProvinceName;
        //        UKCE.Source_Kingdom_Island = Source_Kingdom.Island;
        //        UKCE.Source_Kingdom_Location = Source_Kingdom.Kingdom;
        //        UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);
        //        UKCE.Kingdom_ID = kingdomId;

        //        CheckCECache(db, ceCache, UKCE);
        //        RawData = RawData.Replace(UKCE.Raw_Line, "");
        //    }
        //    #endregion

        //    // No data present
        //    #region Dragon flown away
        //    mcLines = DragonFlownAway.Matches(RawData);  //same code as province dead
        //    for (int i = 0; i < mcLines.Count; i++)
        //    {
        //        item = mcLines[i].Value;
        //        CE_Type = GetCEActionType(item, currentUser.PimpUser.UserID);

        //        UtopiaDate date = new UtopiaDate
        //        {
        //            Year = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxYear.Match(item).Value).Value),
        //            Month = Formatting.Month(Static.rgxMonth.Match(item).Value),
        //            Day = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxDay.Match(item).Value).Value)
        //        };

        //        Utopia_Kingdom_CE UKCE = StartCreateKingdomCE(currentUser.PimpUser.StartingKingdom, startingProvince, date, item);

        //        item = Static.rgxFindUtopianDateTime.Replace(item, "").Trim();


        //        var uki = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom, kingdomId, cachedKingdom);

        //        UKCE.Source_Kingdom_Island = uki.Kingdom_Island;
        //        UKCE.Source_Kingdom_Location = uki.Kingdom_Location;
        //        UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);
        //        UKCE.Kingdom_ID = kingdomId;

        //        CheckCECache(db, ceCache, UKCE);
        //        RawData = RawData.Replace(UKCE.Raw_Line, "");
        //    }
        //    //July 2 of YR2 Alas, the once proud province of Cuddly Little Titan has collapsed and lies in ruins. .  
        //    #endregion

        //    //July 2 of YR2 Alas, the once proud province of Cuddly Little Titan has collapsed and lies in ruins.
        //    #region Collapsed province
        //    mcLines = ProvinceDead.Matches(RawData);
        //    for (int i = 0; i < mcLines.Count; i++)
        //    {
        //        item = mcLines[i].Value;
        //        CE_Type = GetCEActionType(item, currentUser.PimpUser.UserID);

        //        UtopiaDate date = new UtopiaDate
        //        {
        //            Year = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxYear.Match(item).Value).Value),
        //            Month = Formatting.Month(Static.rgxMonth.Match(item).Value),
        //            Day = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxDay.Match(item).Value).Value)
        //        };

        //        Utopia_Kingdom_CE UKCE = StartCreateKingdomCE(currentUser.PimpUser.StartingKingdom, startingProvince, date, item);

        //        item = Static.rgxFindUtopianDateTime.Replace(item, "").Trim();

        //        SourceProvinceName = Static.rgxYear.Replace(FindProvinceNameCE.Match(item).Value.Replace("has lead", "").Replace("into a state of neglect", "").Trim(), "");
        //        SourceProvinceName = Static.rgxFindIslandLocation.Replace(SourceProvinceName, "").Trim();
        //        var uki = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom, kingdomId, cachedKingdom);


        //        if (uki != null)
        //        {
        //            UKCE.Source_Kingdom_Island = uki.Kingdom_Island;
        //            UKCE.Source_Kingdom_Location = uki.Kingdom_Location;
        //        }

        //        UKCE.Source_Province_Name = SourceProvinceName;
        //        UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);
        //        UKCE.Kingdom_ID = kingdomId;

        //        CheckCECache(db, ceCache, UKCE);
        //        RawData = RawData.Replace(UKCE.Raw_Line, "");
        //    }
        //    #endregion

        //    //February 5 of YR2 PandorasWorld has defected to 25 Tales (1:30)
        //    #region Province defected
        //    mcLines = DefectedProvince.Matches(RawData);
        //    for (int i = 0; i < mcLines.Count; i++)
        //    {
        //        item = mcLines[i].Value;
        //        CE_Type = GetCEActionType(item, currentUser.PimpUser.UserID);

        //        UtopiaDate date = new UtopiaDate
        //        {
        //            Year = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxYear.Match(item).Value).Value),
        //            Month = Formatting.Month(Static.rgxMonth.Match(item).Value),
        //            Day = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxDay.Match(item).Value).Value)
        //        };

        //        Utopia_Kingdom_CE UKCE = StartCreateKingdomCE(currentUser.PimpUser.StartingKingdom, startingProvince, date, item);

        //        item = Static.rgxFindUtopianDateTime.Replace(item, "").Trim();
        //        var uki = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom, kingdomId, cachedKingdom);

        //        SourceProvinceName = DefectedProvinceSource.Match(item).Value.Replace("The leader of", "").Replace("The province of", "");
        //        SourceProvinceName = SourceProvinceName.Remove(SourceProvinceName.Length - 3).Trim();
        //        TargetProvinceName = DefectedProvinceTarget.Match(item).Value.Replace("has chosen to join", "").Replace("defected to", "").Replace("to us from", "").Trim();
        //        try
        //        {
        //            Target_Kingdom.Island = Convert.ToInt32(Static.rgxFindIsland.Match(Static.rgxFindIslandLocation.Match(TargetProvinceName).Value).Value.Replace(":", ""));
        //            Target_Kingdom.Kingdom = Convert.ToInt32(Static.rgxFindLocation.Match(Static.rgxFindIslandLocation.Match(TargetProvinceName).Value).Value.Replace(":", ""));
        //            TargetProvinceName = Static.rgxFindIslandLocation.Replace(TargetProvinceName, "").Trim();
        //        }
        //        catch { }

        //        UKCE.Source_Province_Name = SourceProvinceName;
        //        UKCE.Source_Kingdom_Island = uki.Kingdom_Island;
        //        UKCE.Source_Kingdom_Location = uki.Kingdom_Location;
        //        UKCE.Target_Province_Name = TargetProvinceName;
        //        UKCE.Target_Kingdom_Island = Target_Kingdom.Island;
        //        UKCE.Target_Kingdom_Location = Target_Kingdom.Kingdom;
        //        UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);

        //        UKCE.Kingdom_ID = kingdomId;

        //        CheckCECache(db, ceCache, UKCE);
        //        RawData = RawData.Replace(UKCE.Raw_Line, "");
        //    }
        //    #endregion

        //    // No data present
        //    #region Province slips out
        //    mcLines = DefectedProvinceSlipped.Matches(RawData);
        //    for (int i = 0; i < mcLines.Count; i++)
        //    {
        //        item = mcLines[i].Value;
        //        CE_Type = GetCEActionType(item, currentUser.PimpUser.UserID);

        //        UtopiaDate date = new UtopiaDate
        //        {
        //            Year = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxYear.Match(item).Value).Value),
        //            Month = Formatting.Month(Static.rgxMonth.Match(item).Value),
        //            Day = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxDay.Match(item).Value).Value)
        //        };

        //        Utopia_Kingdom_CE UKCE = StartCreateKingdomCE(currentUser.PimpUser.StartingKingdom, startingProvince, date, item);

        //        item = Static.rgxFindUtopianDateTime.Replace(item, "").Trim();
        //        var uki = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom, kingdomId, cachedKingdom);


        //        SourceProvinceName = DefectedProvinceSource.Match(item).Value.Replace("Staying in the darkest shadows,", "").Replace("slips out", "");


        //        UKCE.Source_Province_Name = SourceProvinceName;
        //        UKCE.Source_Kingdom_Island = uki.Kingdom_Island;
        //        UKCE.Source_Kingdom_Location = uki.Kingdom_Location;
        //        UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);

        //        UKCE.Kingdom_ID = kingdomId;

        //        CheckCECache(db, ceCache, UKCE);
        //        RawData = RawData.Replace(UKCE.Raw_Line, "");
        //    }
        //    #endregion

        //    // No data present
        //    #region Defected ??
        //    mcLines = DefectedProvinceOne.Matches(RawData);
        //    for (int i = 0; i < mcLines.Count; i++)
        //    {
        //        item = mcLines[i].Value;
        //        CE_Type = GetCEActionType(item, currentUser.PimpUser.UserID);

        //        UtopiaDate date = new UtopiaDate
        //        {
        //            Year = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxYear.Match(item).Value).Value),
        //            Month = Formatting.Month(Static.rgxMonth.Match(item).Value),
        //            Day = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxDay.Match(item).Value).Value)
        //        };

        //        Utopia_Kingdom_CE UKCE = StartCreateKingdomCE(currentUser.PimpUser.StartingKingdom, startingProvince, date, item);

        //        item = Static.rgxFindUtopianDateTime.Replace(item, "").Trim();
        //        var uki = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom, kingdomId, cachedKingdom);

        //        SourceProvinceName = DefectedProvinceSource.Match(item).Value.Replace("The leader of", "").Replace("destroys all in the land of", "").Replace("before", "");
        //        SourceProvinceName = SourceProvinceName.Remove(SourceProvinceName.Length - 3).Trim();

        //        UKCE.Source_Province_Name = SourceProvinceName;
        //        UKCE.Source_Kingdom_Island = uki.Kingdom_Island;
        //        UKCE.Source_Kingdom_Location = uki.Kingdom_Location;
        //        UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);
        //        UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);

        //        UKCE.Kingdom_ID = kingdomId;

        //        CheckCECache(db, ceCache, UKCE);
        //        RawData = RawData.Replace(UKCE.Raw_Line, "");
        //    }
        //    #endregion

        //    // No data present
        //    #region End war peace
        //    mcLines = StartEndWarPeace.Matches(RawData);
        //    for (int i = 0; i < mcLines.Count; i++)
        //    {
        //        item = mcLines[i].Value;
        //        CE_Type = GetCEActionType(item, currentUser.PimpUser.UserID);

        //        UtopiaDate date = new UtopiaDate
        //        {
        //            Year = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxYear.Match(item).Value).Value),
        //            Month = Formatting.Month(Static.rgxMonth.Match(item).Value),
        //            Day = Convert.ToInt32(Static.rgxNumber.Match(Static.rgxDay.Match(item).Value).Value)
        //        };

        //        Utopia_Kingdom_CE UKCE = StartCreateKingdomCE(currentUser.PimpUser.StartingKingdom, startingProvince, date, item);

        //        item = Static.rgxFindUtopianDateTime.Replace(item, "").Trim();

        //        SourceProvinceName = _findKingdomProvinceName.Match(item).Value;
        //        Source_Kingdom.Island = Convert.ToInt32(Static.rgxFindIsland.Match(Static.rgxFindIslandLocation.Match(SourceProvinceName).Value).Value.Replace(":", ""));
        //        Source_Kingdom.Kingdom = Convert.ToInt32(Static.rgxFindLocation.Match(Static.rgxFindIslandLocation.Match(SourceProvinceName).Value).Value.Replace(":", ""));
        //        SourceProvinceName = Static.rgxFindIslandLocation.Replace(SourceProvinceName, "").Trim();

        //        UKCE.Source_Province_Name = SourceProvinceName;
        //        UKCE.Source_Kingdom_Island = Source_Kingdom.Island;
        //        UKCE.Source_Kingdom_Location = Source_Kingdom.Kingdom;
        //        UKCE.CE_Type = Sql.GetCeTypeId(CE_Type, currentUser.PimpUser.UserID);
        //        UKCE.Kingdom_ID = kingdomId;

        //        // Get all provinces
        //        //var province = cachedKingdom.Provinces.Find(x => x.Province_Name == TargetProvinceName);
        //        //if (province != null)
        //        //{
        //        //    notificationWrapper = new NotificationWrapper(); // Notification class, collect and sends the data as an email.
        //        //    notificationWrapper.Attacker.Location = Source_Kingdom;
        //        //    notificationWrapper.Attacker.Name = SourceProvinceName;
        //        //    notificationWrapper.Date = new UtopiaDate { Year = date.Year, Month = date.Month, Day = date.Day };
        //        //    notificationWrapper.EventText = UKCE.Raw_Line;
        //        //    notificationWrapper.EventType = NotificationEmailEvents.AttackOnSelf;
        //        //    notificationWrapper.Location = Target_Kingdom;
        //        //    notificationWrapper.ProvinceID = province.Province_ID;
        //        //    notificationWrapper.UserID = province.Owner_User_ID.Value;
        //        //    notifier.SendData(notificationWrapper);
        //        //}            

        //        CheckCECache(db, ceCache, UKCE);
        //        RawData = RawData.Replace(UKCE.Raw_Line, "");
        //    }
        //    #endregion

        //    if (main.IsMatch(RawData))
        //        FailedAt("'CETypeDataIsMatch'", RawData, currentUser.PimpUser.UserID);
        //    db.SubmitChanges();

        //    //var ukii = CachedItems.RemoveKingdomCEFromKingdomCache(currentUser.PimpUser.StartingKingdom, kingdomId, cachedKingdom).Kingdoms.Where(x => x.Kingdom_ID == kingdomId).FirstOrDefault();

        //    try
        //    {
        //        notifier.Commit(); // Sends data to be notified
        //    }
        //    catch (Exception e)
        //    {
        //        Errors.logError(e, notifier.ToString());
        //    }

        //    if (ukii != null)
        //        return "CE Submitted " + ukii.Kingdom_Name + " (" + ukii.Kingdom_Island + ":" + ukii.Kingdom_Location + ")";
        //    else
        //        return "CE Submitted";
        //}

        private static Utopia_Kingdom_CE StartCreateKingdomCE(Guid startingKingdom, Guid startingProvince, UtopiaDate date, string rawLine)
        {
            Utopia_Kingdom_CE UKCE = new Utopia_Kingdom_CE();
            UKCE.DateTime_Added = now;
            UKCE.Owner_Kingdom_ID = startingKingdom;
            UKCE.Province_ID_Added = startingProvince;
            UKCE.Utopia_Month = date.Month;
            UKCE.Utopia_Year = date.Year;
            UKCE.Utopia_Date_Day = date.Day;
            UKCE.Raw_Line = rawLine; // Save the line unformatted
            return UKCE;
        }

        /// <summary>
        /// Gets the CE Action for the CE Type to be used.
        /// </summary>
        /// <param name="RawLine"></param>
        /// <returns></returns>
        private static CeTypeEnum GetCEActionType(string RawLine, Guid currentUserID)
        {
            switch (rgxFindCEType.IsMatch(RawLine))
            {
                case true:
                    switch (rgxFindCEType.Match(RawLine).Value.Trim())
                    {
                        case "killed":
                            return CeTypeEnum.killed;
                        case "has collapsed":
                            return CeTypeEnum.collapsed;
                        case "destroyed":
                            return CeTypeEnum.destroyed;
                        case "invaded and pillaged":
                        case "attacked and pillaged":
                        case "invaded and stole":
                            return CeTypeEnum.pillaged;
                        case "captured":
                        case "recaptured":
                        case "attacked":
                            return CeTypeEnum.captured;
                        case "razed":
                            return CeTypeEnum.razed;
                        case "ambushed":
                            return CeTypeEnum.ambushed;
                        case "learned from":
                            return CeTypeEnum.learned;
                        case "aid shipment":
                            return CeTypeEnum.aid;
                        case "attempted an invasion":
                        case "attempted to invade":
                        case "failed to":
                            return CeTypeEnum.attemptedinvasion;
                        case "cancelled the dragon":
                        case "cancelled their dragon project":
                            return CeTypeEnum.cancelleddragon;
                        case "Dragon project against us":
                        case "kingdom has begun":
                            return CeTypeEnum.dragontargetted;
                        case "begun ravaging our lands":
                            return CeTypeEnum.dragonravaginglands;
                        case "slain the dragon":
                            return CeTypeEnum.slaindragon;
                        case "dragon has set flight":
                            return CeTypeEnum.flightdragon;
                        case "dragon ravaging our lands has flown away":
                            return CeTypeEnum.dragonFlownAway;
                        case "declared WAR":
                            return CeTypeEnum.declaredwar;
                        case "cancelled all relations":
                            return CeTypeEnum.cancelledrelations;
                        case "Hostile Kingdom":
                            return CeTypeEnum.hostilekingdom;
                        case "state of Peace":
                        case "offer of Peace":
                        case "offer of peace":
                            return CeTypeEnum.peace;
                        case "surrendered":
                        case "Unable to achieve victory":
                            return CeTypeEnum.surrendered;
                        case "early end to the post-war":
                            return CeTypeEnum.endEarlyPostWar;
                        case "withdrawn":
                            return CeTypeEnum.withdrawn;
                        case "proposed a ceasefire":
                        case "formal ceasefire":
                        case "accepted our ceasefire":
                        case "accepted our ceasefire proposal":
                            return CeTypeEnum.ceasefire;
                        case "rejected a ceasefire":
                        case "declined our ceasefire":
                        case "rejected an offer of Peace":
                            return CeTypeEnum.rejectedceasefire;
                        case "cancelled our ceasefire":
                            return CeTypeEnum.cancelledceasefire;
                        case "broken their ceasefire":
                            return CeTypeEnum.brokenceasefire;
                        case "A war has started":
                            return CeTypeEnum.startwar;
                        case "cancelled their mutual peace proposal":
                            return CeTypeEnum.canceledCFProposal;
                        case "victory in our war":
                        case "surrendered to us":
                            return CeTypeEnum.warwin;
                        case "stalemate":
                            return CeTypeEnum.stalemate;
                        case "collapsed and lies in ruins":
                            return CeTypeEnum.provincedie;
                        case "defected to":
                        case "slips out of":
                        case "never to be seen again":
                        case "destroys all in the land":
                        case "wisely chosen to join us":
                        case "gather their possessions":
                            {
                                if (RawLine.Contains("defected to us") | RawLine.Contains("wisely chosen to join us"))
                                    return CeTypeEnum.defectedfrom;
                                else
                                    return CeTypeEnum.defectedto;
                            }
                        case "ended their state of hostility":
                            return CeTypeEnum.endhostile;
                        case "official state of peace":
                        case "declared Peace with us":
                            return CeTypeEnum.endpeace;
                        case "declared us to be a Hostile Kingdom":
                            return CeTypeEnum.starthostile;
                        case "proposed a Mutual Peace":
                            return CeTypeEnum.startpeace;
                        case "degraded into a state of WAR":
                            return CeTypeEnum.degradedwar;
                        default:
                            FailedAt("'CETypeCheckProblem'", RawLine, currentUserID);
                            return CeTypeEnum.CETypeCheckProblem;
                    }
                default:
                    FailedAt("'CETypeCheckCantFind'", RawLine, currentUserID);
                    return CeTypeEnum.CETypeCheckCantFind;
            }
        }

        /// <summary>
        /// Checks the CECache for already entered info.  If no info, it add it to the list of insertable items
        /// </summary>
        /// <param name="db"></param>
        /// <param name="ceCache"></param>
        /// <param name="UKCE"></param>
        private static bool CheckCECache(UtopiaDataContext db, List<CeRawCache> ceCache, Utopia_Kingdom_CE UKCE)
        {
            var queryCheckItem = (from UKCECI in ceCache
                                  where UKCECI.month == UKCE.Utopia_Month
                                  where UKCECI.year == UKCE.Utopia_Year
                                  where UKCECI.sProvinceName == UKCE.Source_Province_Name
                                  where UKCECI.sKingIsland == UKCE.Source_Kingdom_Island
                                  where UKCECI.sKingLocation == UKCE.Source_Kingdom_Location
                                  where UKCECI.tProvinceName == UKCE.Target_Province_Name
                                  where UKCECI.tKingIsland == UKCE.Target_Kingdom_Island
                                  where UKCECI.tKingLocation == UKCE.Target_Kingdom_Location
                                  where UKCECI.ceType == UKCE.CE_Type
                                  where UKCECI.value == UKCE.value
                                  select UKCECI.uid).FirstOrDefault();

            switch (queryCheckItem)
            {
                case 0:
                    db.Utopia_Kingdom_CEs.InsertOnSubmit(UKCE);
                    return true;
            }
            return false;
        }

        /// <summary>
        /// If it has time sensitive armies out information, this is where the CE adds info to the database.
        /// </summary>
        /// <param name="now"></param>
        /// <param name="SourceProvinceName"></param>
        /// <param name="CE_Type"></param>
        /// <param name="Source_Kingdom_Island"></param>
        /// <param name="Source_Kingdom_Location"></param>
        /// <param name="ownerKingdomID"></param>
        /// <param name="db"></param>
        /// <param name="UKCE"></param>
        private static void SetArmyExpires(DateTime now, string SourceProvinceName, string CE_Type, int Source_Kingdom_Island, int Source_Kingdom_Location, Guid ownerKingdomID, UtopiaDataContext db, Utopia_Kingdom_CE UKCE)
        {
            switch (CE_Type)
            {
                case "killed":
                case "destroyed":
                case "captured":
                case "razed":
                case "ambushed":
                case "pillaged":
                case "learned":
                case "attemptedinvasion":
                    DateTime realTime = UtopiaParser.RealTime(UKCE.Utopia_Date_Day, UKCE.Utopia_Month, UKCE.Utopia_Year).AddHours(12);
                    if (realTime >= now)
                    {
                        var ProvinceCheck = (from yy in db.Utopia_Province_Data_Captured_Gens
                                             where yy.Province_Name == SourceProvinceName
                                             where yy.Owner_Kingdom_ID == ownerKingdomID
                                             where yy.Kingdom_Island == Source_Kingdom_Island
                                             where yy.Kingdom_Location == Source_Kingdom_Location
                                             select yy).FirstOrDefault();
                        if (ProvinceCheck != null)
                        {
                            //if (ProvinceCheck.Army_Out_Expires.HasValue)
                            //{
                            //    if (ProvinceCheck.Army_Out_Expires.Value < now)
                            //    {
                            ProvinceCheck.Army_Out = 1;
                            ProvinceCheck.Army_Out_Expires = realTime;
                            ProvinceCheck.Army_Out = 1;
                            ProvinceCheck.Army_Out_Expires = realTime;
                            //    }
                            //}
                            //else
                            //{
                            //    ProvinceCheck.Army_Out = 1;
                            //    ProvinceCheck.Army_Out_Expires = realTime;
                            //}
                        }
                    }
                    break;
            }
        }
    }
}
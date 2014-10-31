using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.SessionState;

using PimpLibrary.Static.Enums;
using Pimp.Utopia;
using Pimp.Users;
using Pimp.UData;


namespace Pimp.UParser
{
    /// <summary>
    /// Summary description for UtopiaParser
    /// </summary>
    public partial class UtopiaParser
    {
        public UtopiaParser()
        { }
        /// <summary>
        /// handles all parsing from the games and Utopia Angel.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        public static string UtopiaParsing(string RawData, string ClickedFrom, string ServerID, string ProvinceName, string targetedGuids, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            switch (FromWhatPage(GetFormaterType(RawData, currentUser.PimpUser.UserID), RawData, currentUser.PimpUser.UserID))
            {
                case FromWhatPageEnum.InGameKingdomPage:
                    return ParseKingdomPageInGame(RawData, ClickedFrom, ProvinceName, Convert.ToInt32(ServerID), currentUser, cachedKingdom);
                case FromWhatPageEnum.TempleKingdomPage:
                case FromWhatPageEnum.AngelKingdomPage:
                    return ParseKingdomPageAngelTemple(RawData, ClickedFrom, ProvinceName, Convert.ToInt32(ServerID), currentUser, cachedKingdom);
                case FromWhatPageEnum.AngelThroneHome:
                    return ParseAngelThroneHomeAway(RawData, Convert.ToInt32(ServerID), currentUser, cachedKingdom);
                case FromWhatPageEnum.InGameThroneAway:
                case FromWhatPageEnum.InGameThroneHome:
                    return ParseInGameThrone(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.InGameAffairsOfState:
                    return InGameAffairsOfState(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.TempleCEPage:
                    return ParseTempleCE(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.InGameCEPage:
                    UParser.CE ceParser = new CE(RawData, currentUser, cachedKingdom);
                    return ceParser.Parse();
                case FromWhatPageEnum.TempleMilitaryHome:
                case FromWhatPageEnum.TempleMilitaryAway:
                case FromWhatPageEnum.AngelMiltaryPage:
                    return ParseAngelMilitaryHome(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.InGameMiltaryPage:
                    return ParseInGameSOM(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.InGameMilitaryArmyTrainingPage:
                    return InGameMilitaryArmyTrainingPage(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.InGameScienceHome:
                    return ParseInGameSOSHome(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.InGameScienceAway:
                    return ParseInGameSOSAway(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.InGameScienceAdvisorHome:
                    return InGameScienceAdvisorHome(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.InGameScienceAdvisorAway:
                    return InGameScienceAdvisorAway(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.AngelScienceAway:
                    return ParseAngelSOSHome(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.TempleScienceHome:
                case FromWhatPageEnum.AngelScienceHome:
                    return ParseAngelSOSHome(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.TempleInternalAffairsHome:
                case FromWhatPageEnum.AngelInternalAffairsHome:
                    return ParseAngelSurveyHome(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.InGameInternalAffairsPageHome:
                    return ParseInGameSurveyHome(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.InGameInternalAffairsPageAway:
                    return ParseInGameSurveyAway(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.InGameSurveyBuildings:
                    return ParseInGameSurveyHomeBuildings(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.InGameBuildingsAdvisor:
                    return InGameBuildingsAdvisor(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.InGameBuildingsAdvisorAway:
                    return InGameBuildingsAdvisorAway(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.InGameMysticAffairs:
                    return ParseInGameMysticAffairs(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.InGameMystics:
                case FromWhatPageEnum.InGameThieves:
                case FromWhatPageEnum.InGameExtras:
                    return InGameOps.ParseMyticThieveOps(RawData, targetedGuids, currentUser, cachedKingdom);
                case FromWhatPageEnum.InGameAttack:
                    MatchCollection mc = URegEx._findAttacks.Matches(RawData);
                    string tempData = "Attack added for ";
                    foreach (Match item in mc)
                    {
                        tempData += ParseAttack(item.Value, currentUser, cachedKingdom);
                        RawData = RawData.Replace(item.Value, "");
                    }
                    if (RawData.Contains("Our forces will be available again"))
                    {
                        FailedAt("'InGameAttackFailed'", RawData, currentUser.PimpUser.UserID);
                        return ReturnErrorsToUser(ErrorTypeEnum.CouldntFindAttack);
                    }
                    return tempData;
                case FromWhatPageEnum.UToolsMiltaryPage:
                    return UTools.ParseUToolsMilitaryHome(RawData, currentUser, cachedKingdom);
                case FromWhatPageEnum.ProvinceCodeGuid:
                    return ReturnErrorsToUser(ErrorTypeEnum.ProvinceCodeSubmittedWrongPlace);
                case FromWhatPageEnum.SeraphimPage:
                case FromWhatPageEnum.Seraphim:
                    return ReturnErrorsToUser(ErrorTypeEnum.SeraphimPage);
                case FromWhatPageEnum.Ultima:
                    return ReturnErrorsToUser(ErrorTypeEnum.Ultima);
                case FromWhatPageEnum.ExportLineOnly:
                    return ReturnErrorsToUser(ErrorTypeEnum.ExportLineOnly);
                default:
                    FailedAt("'fromwhatpageCase'", RawData, currentUser.PimpUser.UserID);
                    return ReturnErrorsToUser(ErrorTypeEnum.NotRepresent);
            }

        }

        /// <summary>
        /// Checks for what page it was sent.
        /// </summary>
        /// <param name="Typed">checks for the formatter type.</param>
        /// <param name="RawData">Raw uploaded Data.</param>
        /// <returns></returns>
        public static FromWhatPageEnum FromWhatPage(FromWhatPageEnum Typed, string RawData, Guid currentUserID)
        {
            switch (Typed)
            {
                case FromWhatPageEnum.None:
                    FailedAt("FromWhatPage", Typed.ToString(), currentUserID);
                    return FromWhatPageEnum.None;
                case FromWhatPageEnum.Temple:
                    return FromWhatPageTemple(RawData, currentUserID);
                case FromWhatPageEnum.Angel:
                    return FromWhatPageAngel(RawData, currentUserID);
                case FromWhatPageEnum.InGame:
                    return FromWhatPageInGame(RawData, currentUserID);
                case FromWhatPageEnum.UTools:
                    return UTools.FromWhatPageUTools(RawData, currentUserID);
                case FromWhatPageEnum.Seraphim:
                case FromWhatPageEnum.Ultima:
                    return Typed;
                default:
                    FailedAt("'FromWhatPage'", Typed.ToString(), currentUserID);
                    return FromWhatPageEnum.None;
            }
        }
        /// <summary>
        /// Gets the Type of Formater for the Raw Data.
        /// </summary>
        /// <param name="RawData"></param>
        /// <returns></returns>
        public static FromWhatPageEnum GetFormaterType(string RawData, Guid currentUserID)
        {
            if (RawData.Contains("http://www.UtopiaTemple.com All-in-One v"))
                return FromWhatPageEnum.Temple;
            else if (RawData.Contains("http://www.utopiatemple.com Angel v") || RawData.Contains("Utopia Angel's Forum Agent"))
                return FromWhatPageEnum.Angel;
            else if (RawData.Contains("[http://thedragonportal.net Ultima") || RawData.Contains("[http://thedragonportal.eu Ultima")
                     || RawData.Contains("[http://www.thedragonportal.net Ultima") || RawData.Contains("[http://www.thedragonportal.eu Ultima"))
                return FromWhatPageEnum.Ultima;
            else if (RawData.Contains("[Utopia Seraphim v"))
                return FromWhatPageEnum.Seraphim;
            else if (!RawData.Contains("[http://www.utopiatemple.com"))
                return FromWhatPageEnum.InGame;
            else if (!RawData.Contains("[http://www.k3ltic.com/utools/ uTools"))
                return FromWhatPageEnum.UTools;
            else
                FailedAt("GetFormaterType", RawData, currentUserID);
            return FromWhatPageEnum.None;
        }
        /// <summary>
        /// 0 = don't refresh the page...  So the user can read what happened.
        /// </summary>
        /// <param name="ErrorType"></param>
        /// <returns></returns>
        public static string ReturnErrorsToUser(ErrorTypeEnum ErrorType)
        {
            switch (ErrorType)
            {
                case ErrorTypeEnum.NotRepresent:
                    return "0,The data submitted didn't represent anything Pimp understands. Its been saved and will be looked at by ST. REMINDER: Did you select the ENTIRE page? If not, Try again.";
                case ErrorTypeEnum.FindProvinceName:
                    return "0,Couldn't find the Province Name. Please select the ENTIRE page.";
                case ErrorTypeEnum.FindKingdomName:
                    return "0,Couldn't find the Kingdom Name. Please select the ENTIRE page.";
                case ErrorTypeEnum.CouldntFindAttack:
                    return "0,Couldn't find the FULL attack. Please select the ENTIRE Attack.";
                case ErrorTypeEnum.WordsAndNumbersBunchedUp:
                    return "0,The Words and Numbers Contained No Spaces, Please Paste a Proper Page. Email spoiledtechie@gmail.com if this doesn't make sense.";
                case ErrorTypeEnum.CouldntFindFullOp:
                    return "0,Couldn't find the FULL Op. Please select the ENTIRE OP/Page.";
                case ErrorTypeEnum.CurrentActiveProvinceNotFound:
                    return "0,Couldn't find Active Province, Please make sure its selected in the Drop Down.";
                case ErrorTypeEnum.ProvinceCodeSubmittedWrongPlace:
                    return "0,You Submitted The Province Code In the Wrong Place. Goto: Tools -> Kingdom -> Join a Kingdom";
                case ErrorTypeEnum.SeraphimPage:
                    return "0,We Don't Currently Format Seraphim Pages. We need these pages To be exactly identical to Utopia Angel Formatted pages. Please have the developer email SpoiledTechie.";
                case ErrorTypeEnum.Ultima:
                    return "0,We Don't Currently Format Ultima Pages. We need these pages To be exactly identical to Utopia Angel Formatted pages. Please have the developer email SpoiledTechie.";
                case ErrorTypeEnum.ExportLineOnly:
                    return "0,No Export Lines, Please <a style=\"color:Black;\" href=\"mailto:bg@utopiatemple.com?subject=Utopia Angel Export Line API&body=Please get the Angel Export Line API working again, I would like to use it with Pimp. Thank You.&bcc=spoiledtechie@gmail.com\">Contact Brother Green to Make The API work again. </a>";
                case ErrorTypeEnum.SomethingWentWrong:
                default:
                    return "0,Something went wrong and will be looked at by ST. REMINDER: Did you select the ENTIRE page? If not, Try again.";
            }
        }
    }
}
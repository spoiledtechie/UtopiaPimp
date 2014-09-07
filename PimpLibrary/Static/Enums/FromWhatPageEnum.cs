using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace PimpLibrary.Static.Enums
{
    /// <summary>
    /// Summary description for FromWhatPageEnum
    /// </summary>
    public enum FromWhatPageEnum
    {
           Temple,
        Angel,
        InGame,
        Seraphim,
        UTools,
        

        None,
        SeraphimPage,
        Ultima,
        //Temple pages
        TempleThroneAway,
        TempleThroneHome,
        TempleInternalAffairsHome,
        TempleScienceHome,
        TempleMilitaryHome,
        TempleMilitaryAway,
        TempleKingdomPage,
        TempleCEPage,

        //Angel pages
        AngelThroneHome,
        AngelInternalAffairsHome,
        AngelScienceHome,
        AngelScienceAway,
        AngelMiltaryPage,
        AngelKingdomPage,

        //UTools Pages
        UToolsMiltaryPage,
        UToolsThrone,

        //InGame pages
        InGameThroneAway,
        InGameThroneHome,
        InGameAffairsOfState,
        InGameInternalAffairsPageAway,
        InGameInternalAffairsPageHome,
        InGameSurveyBuildings,
        InGameBuildingsAdvisorAway,
        InGameBuildingsAdvisor,
        InGameScienceAway,
        InGameScienceHome,
        InGameScienceAdvisorAway,
        InGameScienceAdvisorHome,
        InGameMiltaryPage,
        InGameMilitaryArmyTrainingPage,
        InGameKingdomPage,
        InGameCEPage,
        InGameMysticAffairs,
        InGameMystics,
        InGameThieves,
        InGameExtras,
        InGameAttack,

        //Errors Submitted that can be caught to the game
        ProvinceCodeGuid,
        ExportLineOnly
    }
}
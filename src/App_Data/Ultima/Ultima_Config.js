// ** Main Modifiers **

var Age_Start_Date = "26/05/2010"; // Age Start Date [DD/MM/YYYY]

// Race In Use ? 0 [none] , 1 [wol] , 2 [gen] , 3 [both]
// Old Halfling :: "Spearmen" :: "Archers" :: "Half-Giants"

var Race_In_Use = new Array(1, 1, 1, 1, 1, 1, 0, 1, 0, 1);
var Race_Name = new Array("Human", "Elf", "Dwarf", "Orc", "Gnome", "Dark Elf", "Undead", "Avian", "Faery", "Halfling");
var Offensive_Specialist_Name = new Array("Swordsmen", "Rangers", "Warriors", "Goblins", "Halflings", "Night Rangers", "Skeletons", "Harpies", "Magicians", "Strongarms");
var Defensive_Specialist_Name = new Array("Archers", "Archers", "Axemen", "Trolls", "Pikemen", "Druids", "Zombies", "Griffins", "Druids", "Slingers");
var Elite_Unit_Name = new Array("Knights", "Elf Lords", "Berserkers", "Ogres", "Golems", "Drow", "Ghouls", "Drakes", "Beastmasters", "Brutes");

var Offensive_Specialist_Strength = new Array(4, 5, 6, 5, 5, 5, 5, 5, 5, 5);
var Defensive_Specialist_Strength = new Array(6, 5, 5, 4, 5, 5, 5, 5, 5, 5);
var Offensive_Elite_Unit_Strength = new Array(8, 7, 7, 9, 5, 6, 9, 8, 3, 6);
var Defensive_Elite_Unit_Strength = new Array(3, 4, 4, 2, 5, 4, 3, 3, 6, 5);

var Offensive_Specialist_Networth = new Array(4, 4, 4, 4, 4, 4.8, 4, 4, 4, 4);
var Defensive_Specialist_Networth = new Array(6, 5, 5, 5, 5, 5, 5, 5, 5, 5);
var Elite_Unit_Networth = new Array(6.5, 6, 6, 6.75, 4, 5, 8, 6.5, 5.5, 5);

var Personality_In_Use = new Array(1, 1, 0, 1, 1, 1, 1, 0, 1, 0, 0, 1);
var Personality_Name = new Array("Merchant", "Mystic", "War Hero", "Warrior", "Rogue", "Shepherd", "Sage", "Cleric", "Artisan", "Freak", "General", "Tactician");
var Personality_Search_Key = new Array("Wealthy", "Sorcere", "Heroic", "Warrior", "Rogue", "Humble", "Wise", "Blessed", "Crafts", "Crazy", "Great", "Conniving");

var Rank_Name = new Array(2);
var Rank_Search_Key = new Array(2);

Rank_Name[0] = new Array("Peasant", "Knight", "Lord", "Baron", "Viscount", "Count", "Marquis", "Duke", "Prince", "King");
Rank_Name[1] = new Array("Peasant", "Lady", "Noble Lady", "Baroness", "Viscountess", "Countess", "Marchioness", "Duchess", "Princess", "Queen");
Rank_Search_Key[0] = new Array("Mr.", "Sir", "Lord", "Baron", "Viscount", "Count", "Marquis", "Duke", "Prince", "King");
Rank_Search_Key[1] = new Array("Ms.", "Lady", "Noble", "Baroness", "Viscountess", "Countess", "Marchioness", "Duchess", "Princess", "Queen");

var War_Wins_Title = new Array("", "Esteemed", "Famous", "Notorious", "World Reknowned");

var Honor_Milestone = new Array(0, 801, 1501, 2251, 3001, 3751, 4501, 5501, 7000);

var Trade_Balance_Resource = new Array("Gold", "Food", "Soldier", "Rune");
var Trade_Balance_Value = new Array(1, 0.2, 100, 3);

var Kingdom_Attitude = new Array("Ceasefire", "Unfriendly", "Hostile", "War");
var Kingdom_Stance_Name = new Array("(no bonuses)", "(-15% attack gains)", "(+10% defense, -15% offense)", "(+10% gains, -10% attack time)");
var Kingdom_Stance_Search_Key = new Array("Normal", "Peaceful", "Fortified", "Aggressive");

var Money_Networth = 0.001;
var Peasants_Networth = 1;
var Soldiers_Networth = 1.5;
var War_Horses_Networth = 0.6;
var Thieves_Networth = 4;
var Wizards_Networth = 4;
var Science_Networth = 1 / 92;

var OverPop_Name = new Array("Riots due to housing shortages from overpopulation are hampering tax collection efforts!");
var OverPop_Search_Key = new Array("Riots due to housing shortages from overpopulation are hampering tax collection efforts!");

var War_Name = new Array("Province is at WAR! (-75% gains and effectiveness)");
var War_Search_Key = new Array("Our Kingdom is at WAR!");

var Plague_Name = new Array("The Plague has spread throughout the people!<br />(No growth, -15% def & tax, quicker prisoners death)");
var Plague_Search_Key = new Array("The Plague has spread throughout our people!");

var Dragon_Name = new Array("", "A Ruby Dragon ravages the lands!<br />(-8% ME, -10% income, -20% draftees)", "An Emerald Dragon ravages the lands!<br />(+10% military losses, -10% combat gains, -10% income, -20% draftees)", "A Sapphire Dragon ravages the lands!<br />(-20% thievery & magic effectiveness, -10% income, -20% draftees)", "A Gold Dragon ravages the lands!<br />(-20% BE, -10% income, -20% draftees)");
var Dragon_Search_Key = new Array("", "Ruby Dragon", "Emerald Dragon", "Sapphire Dragon", "Gold Dragon");

var Hit_Name = new Array("", "Province was hit a couple of times recently!", "Province was moderately hit in the last month!", "Province was hit pretty heavily in the last month!", "Province was hit extremely hard in the last month!");
var Hit_Search_Key = new Array("", "this province has been attacked a little recently", "this province has been attacked moderately", "this province has been attacked pretty heavily", "this province has been hit extremely");

// ** Main Modifiers **



// ** Buildings Modifiers **

var Building_Modifier_Birth_Rate = 4;
var Building_Modifier_Construction_Cost = 4;
var Building_Modifier_Exploration_Cost = 3;
var Building_Modifier_Increased_Income = 1.25;
var Building_Modifier_Offensive_Military = 1.5;
var Building_Modifier_Training_Costs = 1.5;
var Building_Modifier_Daily_Wages = 2;
var Building_Modifier_Draft_Costs = 2;
var Building_Modifier_Attack_Time = 1.5;
var Building_Modifier_Defensive_Military = 1.5;
var Building_Modifier_Attacked_Losses = 2;
var Building_Modifier_Cure_Plague = 2;
var Building_Modifier_Military_Losses = 3;
var Building_Modifier_Thievery_Losses = 4;
var Building_Modifier_Thievery_Effectiveness = 3;
var Building_Modifier_Catching_Thieves = 2;
var Building_Modifier_Thievery_Damage = 3;
var Building_Modifier_Science_Bonus = 2;
var Building_Modifier_Science_Costs = 1.5;
var Building_Modifier_Books_Protected = 3.5;

// ** Buildings Modifiers **



// ** Science Modifiers **

var Sci_Mult_Alchemy = 1.4;
var Sci_Mult_Tools = 1;
var Sci_Mult_Housing = 0.65;
var Sci_Mult_Food = 8;
var Sci_Mult_Military = 1.4;
var Sci_Mult_Crime = 6;
var Sci_Mult_Channeling = 6;

// ** Science Modifiers **



// ** Thievery Modifiers **

var Thievery_Penalty_OSW = 75;
var Thievery_Penalty_None = 30;
var Thievery_Penalty_Unfriendly = 15;
var Thievery_Penalty_Hostile = 15;
var Thievery_Penalty_War = 0;

var Thief_Per_Resource_RobtheVaults = 750;
var Thief_Per_Resource_RobtheGranaries = 250;
var Thief_Per_Resource_RobtheTowers = 75;
var Thief_Per_Resource_Kidnapping = 5.5;
var Thief_Per_Resource_AssassinateWizards = 0.5;
var Thief_Per_Resource_StealHorses = 1.1;
var Thief_Per_Resource_Arson = 10;
var Thief_Per_Resource_GreaterArson = 10;
var Thief_Per_Resource_FreePrisoners = 0.4;
var Thief_Per_Resource_NightStrike_Soldiers = 5;
var Thief_Per_Resource_NightStrike_OffSpecs = 6;
var Thief_Per_Resource_NightStrike_DefSpecs = 6;
var Thief_Per_Resource_NightStrike_Elites = 5;

var Thief_Max_Resource_RobtheVaults = 8.25;
var Thief_Max_Resource_RobtheGranaries = 50;
var Thief_Max_Resource_RobtheTowers = 35;
var Thief_Max_Resource_Kidnapping = 2.5;
var Thief_Max_Resource_AssassinateWizards = 1.2;
var Thief_Max_Resource_StealHorses = 15.5;
var Thief_Max_Resource_Arson = 3;
var Thief_Max_Resource_GreaterArson = 6;
var Thief_Max_Resource_FreePrisoners = 17;
var Thief_Max_Resource_NightStrike_Soldiers = 13;
var Thief_Max_Resource_NightStrike_OffSpecs = 0.45;
var Thief_Max_Resource_NightStrike_DefSpecs = 0.55;
var Thief_Max_Resource_NightStrike_Elites = 0.3;

// ** Thievery Modifiers **



var c0 = "<font color='#0088FF'>";
var c1 = "<font color='#B0B0B0'>"; //404060
var c2 = "<font color='#FFFFFF'>";
var c3 = "<font color='#FF8888'>";
var cc = "</font>";

var Copyrights = "<br />" + c0 + "[http://thedragonportal.net Ultima v1.02]" + cc + "<br />" + "<br />" + c1 + "Server: " + cc + c2 + "World of Legends (Age of Distinction)" + cc;
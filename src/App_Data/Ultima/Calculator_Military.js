var Your_Province_Name = "Attacker's Information";
var Enemy_Province_Name = "Defender's Information";

var Your_Race = 0; var Enemy_Race = 0;
var Your_Personality = 0; var Enemy_Personality = 0;
var Your_Relations = 0; var Enemy_Relations = 0;
var Your_Stance = 0; var Enemy_Stance = 0;
var Your_Dragon = 0; var Enemy_Dragon = 0;
var Your_Soldiers = 0; var Enemy_Soldiers = 0;
var Your_Offensive_Units = 0; var Enemy_Offensive_Units = 0;
var Your_Defensive_Units = 0; var Enemy_Defensive_Units = 0;
var Your_Elite_Units = 0; var Enemy_Elite_Units = 0;
var Your_Land = 0; var Enemy_Land = 0;
var Your_NW = 0; var Enemy_NW = 0;
var Your_KD_NW = 0; var Enemy_KD_NW = 0;
var Your_OME = 0; var Enemy_OME = 0;
var Your_DME = 0; var Enemy_DME = 0;
var Your_Generals = 0; var Enemy_Generals = 0;
var Your_Attack_Time = 0; var Enemy_Attack_Time = 0;
var Your_Horses = 0; var Enemy_Horses = 0;
var Your_Mercenaries = 0; var Enemy_Mercenaries = 0;
var Your_Prisoners = 0; var Enemy_Prisoners = 0;
var Your_Military_Sci = 0; var Enemy_Military_Sci = 0;
var Your_Recently_Hit = 0; var Enemy_Recently_Hit = 0;
var Your_Guard_Station = 0; var Enemy_Guard_Station = 0;
var Your_Peasants = 0; var Enemy_Peasants = 0;
var Your_Raw_ME = 0; var Enemy_Raw_ME = 0;
var Your_BE = 0; var Enemy_BE = 0;
var Your_TG = 0; var Enemy_TG = 0;
var Your_Forts = 0; var Enemy_Forts = 0;
var Your_Rank = 0; var Enemy_Rank = 0;

var Your_Monarch_Box = 0; var Enemy_Monarch_Box = 0; var Your_Monarch_Bonus = 0; var Enemy_Monarch_Bonus = 0;
var Your_Peasants_Box = 0; var Enemy_Peasants_Box = 0; var Your_Peasants_Bonus = 0; var Enemy_Peasants_Bonus = 0;
var Your_Fanaticism_Box = 0; var Enemy_Fanaticism_Box = 0; var Your_Fanaticism_Bonus = 0; var Enemy_Fanaticism_Bonus = 0;
var Your_Aggression_Box = 0; var Enemy_Aggression_Box = 0; var Your_Aggression_Bonus = 0; var Enemy_Aggression_Bonus = 0;
var Your_Protection_Box = 0; var Enemy_Protection_Box = 0; var Your_Protection_Bonus = 0; var Enemy_Protection_Bonus = 0;
var Your_Plague_Box = 0; var Enemy_Plague_Box = 0; var Your_Plague_Bonus = 0; var Enemy_Plague_Bonus = 0;

var Your_TG_Bonus = 0; var Enemy_TG_Bonus = 0;
var Your_Forts_Bonus = 0; var Enemy_Forts_Bonus = 0;
var Your_OME_Bonus = 0; var Enemy_OME_Bonus = 0;
var Your_DME_Bonus = 0; var Enemy_DME_Bonus = 0;
var Your_Raw_Offense = 0; var Enemy_Raw_Offense = 0;
var Your_Mod_Offense = 0; var Enemy_Mod_Offense = 0;
var Your_Raw_Defense = 0; var Enemy_Raw_Defense = 0;
var Your_Mod_Defense = 0; var Enemy_Mod_Defense = 0;
var Your_Ruby_Dragon = 0; var Enemy_Ruby_Dragon = 0;

var Your_Optimize1 = 0;
var Your_Optimize2 = 0;
var Your_Rank_Bonus = 0;

var Your_Optimize_Horses = 0;
var Your_Optimize_Mercenaries = 0;
var Your_Optimize_Prisoners = 0;

var Gain_Formula_Cap_Value = 0;
var Gain_Formula_Cap = 0;
var Gain_Formula_Land = 0;
var Gain_Formula_PNWF = 1;
var Gain_Formula_KNWF = 1;
var Gain_Formula_Race = 1;
var Gain_Formula_Stance = 1;
var Gain_Formula_Relations = 1;
var Gain_Formula_Sciences = 1;
var Gain_Formula_GS = 1;
var Gain_Formula_Dragon = 1;
var Gain_Formula_GBP = 1;
var Gain_Formula_Time = 1;

var Chance = 0;
var Relative_NW = 1;
var Relative_KD_NW = 1;
var Main_Gain_Formula = 1;

function Optimize_Horses() { document.Calculator.YourHorses.value = Your_Optimize_Horses; document.Calculator.YourOptimizeHorses.disabled = true; }
function Max_Merc() { document.Calculator.YourMerc.value = Your_Optimize_Mercenaries; document.Calculator.YourMaxMerc.disabled = true; }
function Max_Pris() { document.Calculator.YourPris.value = Your_Optimize_Prisoners; document.Calculator.YourMaxPris.disabled = true; }

function Task_Military() {

    Your_Province_Name = document.getElementById('YourProvinceName').innerHTML;
    Enemy_Province_Name = document.getElementById('EnemyProvinceName').innerHTML;

    Your_Race = Number(document.Calculator.YourRace.value); Enemy_Race = Number(document.Calculator.EnemyRace.value);
    Your_Personality = Number(document.Calculator.YourPersonality.value); Enemy_Personality = Number(document.Calculator.EnemyPersonality.value);
    Your_Relations = Number(document.Calculator.YourRelations.value); Enemy_Relations = Number(document.Calculator.EnemyRelations.value);
    Your_Stance = Number(document.Calculator.YourStance.value); Enemy_Stance = Number(document.Calculator.EnemyStance.value);
    Your_Dragon = Number(document.Calculator.YourDragon.value); Enemy_Dragon = Number(document.Calculator.EnemyDragon.value);
    Your_Soldiers = Number(document.Calculator.YourSoldiers.value); Enemy_Soldiers = Number(document.Calculator.EnemySoldiers.value);
    Your_Offensive_Units = Number(document.Calculator.YourOffSpecs.value); Enemy_Offensive_Units = Number(document.Calculator.EnemyOffSpecs.value);
    Your_Defensive_Units = Number(document.Calculator.YourDefSpecs.value); Enemy_Defensive_Units = Number(document.Calculator.EnemyDefSpecs.value);
    Your_Elite_Units = Number(document.Calculator.YourElites.value); Enemy_Elite_Units = Number(document.Calculator.EnemyElites.value);
    Your_Land = Number(document.Calculator.YourLand.value); Enemy_Land = Number(document.Calculator.EnemyLand.value);
    Your_NW = Number(document.Calculator.YourNW.value); Enemy_NW = Number(document.Calculator.EnemyNW.value);
    Your_KD_NW = Number(document.Calculator.YourKDNW.value); Enemy_KD_NW = Number(document.Calculator.EnemyKDNW.value);
    Your_OME = Number(document.Calculator.YourOME.value); Enemy_OME = Number(document.Calculator.EnemyOME.value);
    Your_DME = Number(document.Calculator.YourDME.value); Enemy_DME = Number(document.Calculator.EnemyDME.value);
    Your_Generals = Number(document.Calculator.YourGenerals.value); Enemy_Generals = Number(document.Calculator.EnemyGenerals.value);
    Your_Attack_Time = Number(document.Calculator.YourAttackTime.value); Enemy_Attack_Time = Number(document.Calculator.EnemyAttackTime.value);
    Your_Horses = Number(document.Calculator.YourHorses.value); Enemy_Horses = Number(document.Calculator.EnemyHorses.value);
    Your_Mercenaries = Number(document.Calculator.YourMerc.value); Enemy_Mercenaries = Number(document.Calculator.EnemyMerc.value);
    Your_Prisoners = Number(document.Calculator.YourPris.value); Enemy_Prisoners = Number(document.Calculator.EnemyPris.value);
    Your_Military_Sci = Number(document.Calculator.YourMilitarySci.value); Enemy_Military_Sci = Number(document.Calculator.EnemyMilitarySci.value);
    Your_Recently_Hit = Number(document.Calculator.YourRecentlyHit.value); Enemy_Recently_Hit = Number(document.Calculator.EnemyRecentlyHit.value);
    Your_Guard_Station = Number(document.Calculator.YourGuardStation.value); Enemy_Guard_Station = Number(document.Calculator.EnemyGuardStation.value);
    Your_Peasants = Number(document.Calculator.YourPeasants.value); Enemy_Peasants = Number(document.Calculator.EnemyPeasants.value);
    Your_Raw_ME = Number(document.Calculator.YourRawME.value); Enemy_Raw_ME = Number(document.Calculator.EnemyRawME.value);
    Your_BE = Number(document.Calculator.YourBE.value); Enemy_BE = Number(document.Calculator.EnemyBE.value);
    Your_TG = Number(document.Calculator.YourTG.value); Enemy_TG = Number(document.Calculator.EnemyTG.value);
    Your_Forts = Number(document.Calculator.YourForts.value); Enemy_Forts = Number(document.Calculator.EnemyForts.value);
    Your_Rank = Number(document.Calculator.YourRank.value); Enemy_Rank = Number(document.Calculator.EnemyRank.value);

    Your_Monarch_Box = 0; Enemy_Monarch_Box = 0; Your_Monarch_Bonus = 0; Enemy_Monarch_Bonus = 0;
    Your_Peasants_Box = 0; Enemy_Peasants_Box = 0; Your_Peasants_Bonus = 0; Enemy_Peasants_Bonus = 0;
    Your_Fanaticism_Box = 0; Enemy_Fanaticism_Box = 0; Your_Fanaticism_Bonus = 0; Enemy_Fanaticism_Bonus = 0;
    Your_Aggression_Box = 0; Enemy_Aggression_Box = 0; Your_Aggression_Bonus = 1; Enemy_Aggression_Bonus = 1;
    Your_Protection_Box = 0; Enemy_Protection_Box = 0; Your_Protection_Bonus = 0; Enemy_Protection_Bonus = 0;
    Your_Plague_Box = 0; Enemy_Plague_Box = 0; Your_Plague_Bonus = 0; Enemy_Plague_Bonus = 0;

    if (document.Calculator.YourMonarchBox.checked == true) Your_Monarch_Box = 1;
    if (document.Calculator.YourPeasantsBox.checked == true) Your_Peasants_Box = 1;
    if (document.Calculator.YourFanaticismBox.checked == true) Your_Fanaticism_Box = 1;
    if (document.Calculator.YourAggressionBox.checked == true) Your_Aggression_Box = 1;
    if (document.Calculator.YourProtectionBox.checked == true) Your_Protection_Box = 1;
    if (document.Calculator.YourPlagueBox.checked == true) Your_Plague_Box = 1;

    if (document.Calculator.EnemyMonarchBox.checked == true) Enemy_Monarch_Box = 1;
    if (document.Calculator.EnemyPeasantsBox.checked == true) Enemy_Peasants_Box = 1;
    if (document.Calculator.EnemyFanaticismBox.checked == true) Enemy_Fanaticism_Box = 1;
    if (document.Calculator.EnemyAggressionBox.checked == true) Enemy_Aggression_Box = 1;
    if (document.Calculator.EnemyProtectionBox.checked == true) Enemy_Protection_Box = 1;
    if (document.Calculator.EnemyPlagueBox.checked == true) Enemy_Plague_Box = 1;

    Your_Raw_Offense = 0; Enemy_Raw_Defense = 0;
    Your_Mod_Offense = 0; Enemy_Mod_Defense = 0;
    Your_Ruby_Dragon = 0; Enemy_Ruby_Dragon = 0;

    Your_Optimize1 = 0;
    Your_Optimize2 = 0;
    Your_Rank_Bonus = 0;



    // **************************
    //   Attacker's Information
    // **************************

    if ((Your_Race != 0) && (Your_Race != 3)) {
        document.Calculator.YourAggressionBox.disabled = true;
        document.getElementById('YourAggressionName').innerHTML = '<input Style="font-weight:bold;font-size:12;BackGround-Color:Transparent;Border:Solid 0px" disabled size="9" type="text" value="Aggression">';
    } else {
        document.Calculator.YourAggressionBox.disabled = false;
        document.getElementById('YourAggressionName').innerHTML = "Aggression";
    }

    document.getElementById('YourOffSpecsName').innerHTML = Offensive_Specialist_Name[Your_Race];
    document.getElementById('YourElitesName').innerHTML = Elite_Unit_Name[Your_Race];

    Your_Optimize1 = Your_Soldiers + Your_Offensive_Units + Your_Elite_Units;

    Your_Optimize_Horses = Your_Horses;

    if (Your_Horses > Your_Optimize1) { Your_Optimize_Horses = Your_Optimize1; document.Calculator.YourOptimizeHorses.disabled = false; }

    if ((document.Calculator.YourAggressionBox.disabled == false) && (Your_Aggression_Box == 1) && (document.getElementById('YourBonusesBox').style.display == "block")) Your_Aggression_Bonus = 2;

    Your_Optimize_Mercenaries = Your_Mercenaries;
    Your_Optimize_Prisoners = Your_Prisoners;

    Your_Optimize2 = Math.round(Your_Optimize1 / 5) - Your_Prisoners; if (Your_Optimize2 < 0) Your_Optimize2 = 0;
    if (Your_Mercenaries != Your_Optimize2) { Your_Optimize_Mercenaries = Your_Optimize2; document.Calculator.YourMaxMerc.disabled = false; }

    Your_Optimize2 = Math.round(Your_Optimize1 / 5) - Your_Mercenaries; if (Your_Optimize2 < 0) Your_Optimize2 = 0;
    if (Your_Prisoners != Your_Optimize2) { Your_Optimize_Prisoners = Your_Optimize2; document.Calculator.YourMaxPris.disabled = false; }

    if (Your_Personality == 9) {
        document.getElementById('YourPersonalityName').innerHTML = '<font color="red">Personality</font>';
    } else { document.getElementById('YourPersonalityName').innerHTML = 'Personality'; }

    if (Your_Relations == 5) {
        document.getElementById('MinimumGain').innerHTML = "Burnt Land";
    } else {
        document.getElementById('MinimumGain').innerHTML = "Gain With Exploration Pool (War Only)";
    }

    if (Your_Relations == 3) document.Calculator.EnemyRelations.value = 3;

    if ((Your_Stance == 2) && (Your_Relations != 3)) Your_Raw_ME = Your_Raw_ME - 15;

    if (Your_TG > 50) { Your_TG = 50; document.Calculator.YourTG.value = 50; }

    Your_TG_Bonus = 1.5 * Your_TG * (1 - Your_TG / 100) * Your_BE / 100;

    if (Your_Rank > 0) Your_Rank_Bonus = Your_Rank * 2 - 2;
    if (Your_Rank > 4) Your_Rank_Bonus = Your_Rank * 4 - 10;
    if (Your_Rank > 8) Your_Rank_Bonus = 2; // King is Unknown so i set it to Lord
    if (Your_Race == 3) Your_Rank_Bonus = Math.round(Your_Rank_Bonus / 2);

    if (Your_Dragon == 1) Your_Ruby_Dragon = 8;
    if (Your_Dragon != 1) Your_Ruby_Dragon = 0;

    if (Your_Race == 3) {
        document.Calculator.YourFanaticismBox.disabled = false;
        document.getElementById('YourFanaticism').innerHTML = 'Fanaticism';
    }
    else {
        document.Calculator.YourFanaticismBox.disabled = true;
        document.getElementById('YourFanaticism').innerHTML = '<input Style="font-weight:bold;font-size:12;BackGround-Color:Transparent;Border:Solid 0px" disabled size=9 type="text" value="Fanaticism" disabled>';
    }

    if ((document.Calculator.YourFanaticismBox.disabled == false) && (Your_Fanaticism_Box == 1)) Your_Fanaticism_Bonus = 5;

    Your_Raw_Offense = Your_Soldiers * Your_Aggression_Bonus + Your_Offensive_Units * Offensive_Specialist_Strength[Your_Race] + Your_Elite_Units * Offensive_Elite_Unit_Strength[Your_Race] + Your_Optimize_Horses + document.Calculator.YourMerc.value * 3 + document.Calculator.YourPris.value * 3;

    if (document.getElementById('YourBonusesBox').style.display == "block") {
        Your_OME_Bonus = (Your_Raw_ME) * (1 + Your_Generals / 100) * (1 + Your_TG_Bonus / 100) * (1 + Your_Rank_Bonus / 100) * (1 - Your_Ruby_Dragon / 100) * (1 + Your_Fanaticism_Bonus / 100);
    } else { Your_OME_Bonus = (Your_OME) * (1 + Your_Generals / 100); }

    document.getElementById('YourRawOffense').innerHTML = Your_Raw_Offense;
    document.getElementById('YourBonuses').innerHTML = Math.round((Your_OME_Bonus / 100 - 1) * 10000) / 100 + "%";
    Your_Mod_Offense = Math.round(Your_Raw_Offense * Your_OME_Bonus / 100);
    document.getElementById('YourModifiedOffense').innerHTML = Your_Mod_Offense;

    if (Your_Land == 0) { document.getElementById('YourModifiedOPA').innerHTML = "?"; } else {
        document.getElementById('YourModifiedOPA').innerHTML = Math.round(Your_Mod_Offense / Your_Land * 100) / 100;
    }



    // **************************
    //   Defender's Information
    // **************************



    document.getElementById('EnemyDefSpecsName').innerHTML = Defensive_Specialist_Name[Enemy_Race];
    document.getElementById('EnemyElitesName').innerHTML = Elite_Unit_Name[Enemy_Race];

    if (Enemy_Peasants_Box == 0) { document.Calculator.EnemyPeasants.disabled = true; Enemy_Peasants_Bonus = 0; }
    if (Enemy_Peasants_Box == 1) { document.Calculator.EnemyPeasants.disabled = false; Enemy_Peasants_Bonus = Math.round(Enemy_Peasants / 4); }

    if (Enemy_Race == 4) {
        document.Calculator.EnemyPeasantsBox.disabled = false;
        document.getElementById('EnemyPeasantsName').innerHTML = 'Peasants';
    }
    else {
        Enemy_Peasants_Bonus = 0;
        document.Calculator.EnemyPeasantsBox.disabled = true;
        document.getElementById('EnemyPeasantsName').innerHTML = '<input Style="font-weight:bold;font-size:12;BackGround-Color:Transparent;Border:Solid 0px" disabled size=9 type="text" value="Peasants" disabled>';
    }


    if (Enemy_Personality == 9) {
        document.getElementById('EnemyPersonalityName').innerHTML = "<font color=red>Personality</font>";
    } else { document.getElementById('EnemyPersonalityName').innerHTML = "Personality"; }

    if ((Enemy_Personality == 2) || (Enemy_Race == 6)) {
        document.Calculator.EnemyPlagueBox.disabled = true;
        document.getElementById('EnemyPlagueName').innerHTML = '<input Style="font-weight:bold;font-size:12;BackGround-Color:Transparent;Border:Solid 0px" disabled size=9 type="text" value="Plague" disabled>';
    }
    else {
        document.Calculator.EnemyPlagueBox.disabled = false;
        document.getElementById('EnemyPlagueName').innerHTML = 'Plague';
    }

    if (Enemy_Race == 3) {
        document.Calculator.EnemyFanaticismBox.disabled = false;
        document.getElementById('EnemyFanaticism').innerHTML = 'Fanaticism';
    }
    else {
        document.Calculator.EnemyFanaticismBox.disabled = true;
        document.getElementById('EnemyFanaticism').innerHTML = '<input Style="font-weight:bold;font-size:12;BackGround-Color:Transparent;Border:Solid 0px" disabled size=9 type="text" value="Fanaticism" disabled>';
    }

    if ((document.Calculator.EnemyFanaticismBox.disabled == false) && (Enemy_Fanaticism_Box == 1)) Enemy_Fanaticism_Bonus = 3;

    if ((Enemy_Stance == 2) && (Enemy_Relations != 3)) Enemy_Raw_ME = Math.round(Enemy_Raw_ME + 10);

    if (Enemy_Dragon == 1) Enemy_Ruby_Dragon = 8;
    if (Enemy_Dragon != 1) Enemy_Ruby_Dragon = 0;

    if (Enemy_Protection_Box == 1) Enemy_Protection_Bonus = 5;

    if (document.Calculator.EnemyPlagueBox.disabled == false) {
        if (Enemy_Plague_Box == 1) { document.getElementById('EnemyPlagueImage').style.display = "block"; Enemy_Plague_Bonus = 15; }
        if (Enemy_Plague_Box == 0) { document.getElementById('EnemyPlagueImage').style.display = "none"; Enemy_Plague_Bonus = 0; }
    }

    if ((Your_Relations == 3) || (Your_Relations == 2)) {
        document.Calculator.EnemyMonarchBox.disabled = false;
        document.getElementById('EnemyMonarchName').innerHTML = 'Monarch';
    }
    else {
        document.Calculator.EnemyMonarchBox.disabled = true;
        document.getElementById('EnemyMonarchName').innerHTML = '<input Style="font-weight:bold;font-size:12;BackGround-Color:Transparent;Border:Solid 0px" disabled size=9 type="text" value="Monarch" disabled>';
    }

    if ((document.Calculator.EnemyMonarchBox.disabled == false) && (Enemy_Monarch_Box == 1)) Enemy_Monarch_Bonus = 10;

    if (Enemy_Forts > 50) { Enemy_Forts = 50; document.Calculator.EnemyForts.value = 50; }
    Enemy_Forts_Bonus = 1.5 * (Enemy_Forts - Enemy_Forts * Enemy_Forts / 100) * Enemy_BE / 100;

    Enemy_Raw_Defense = Enemy_Soldiers + Enemy_Defensive_Units * Defensive_Specialist_Strength[Enemy_Race] + Enemy_Elite_Units * Defensive_Elite_Unit_Strength[Enemy_Race] + Enemy_Peasants_Bonus;
    document.getElementById('EnemyRawDefense').innerHTML = Enemy_Raw_Defense;

    if (document.getElementById('EnemyBonusesBox').style.display == "block") {
        Enemy_DME_Bonus = (Enemy_Raw_ME) * (1 + Enemy_Monarch_Bonus / 100) * (1 - Enemy_Ruby_Dragon / 100) * (1 - Enemy_Fanaticism_Bonus / 100) * (1 + Enemy_Protection_Bonus / 100) * (1 - Enemy_Plague_Bonus / 100) * (1 + Enemy_Forts_Bonus / 100);
    } else { Enemy_DME_Bonus = (Enemy_DME) * (1 + Enemy_Monarch_Bonus / 100); }

    document.getElementById('EnemyBonuses').innerHTML = Math.round((Enemy_DME_Bonus / 100 - 1) * 10000) / 100 + "%";
    Enemy_Mod_Defense = Math.round(Enemy_Raw_Defense * Enemy_DME_Bonus / 100);
    document.getElementById('EnemyModifiedDefense').innerHTML = Enemy_Mod_Defense;

    if (Enemy_Land == 0) { document.getElementById('EnemyModifiedDPA').innerHTML = "?"; } else {
        document.getElementById('EnemyModifiedDPA').innerHTML = Math.round(Enemy_Mod_Defense / Enemy_Land * 100) / 100;
    }



    // **************************
    //   Gain Formula Modifiers
    // **************************



    if (Your_NW > 0) {
        Relative_NW = Enemy_NW / Your_NW;
        if (Relative_NW < 0.9) Gain_Formula_PNWF = 3 * Relative_NW - 1.7;
        if ((0.9 <= Relative_NW) && (Relative_NW <= 1.1)) Gain_Formula_PNWF = 1;
        if (Relative_NW > 1.1) Gain_Formula_PNWF = -2 * Relative_NW + 3.2;
    } else { Gain_Formula_PNWF = 1; }

    if (Your_KD_NW > 0) {
        Relative_KD_NW = Enemy_KD_NW / Your_KD_NW;
        if (Relative_KD_NW < 0.4) Gain_Formula_KNWF = 2 / 3;
        if ((0.4 <= Relative_KD_NW) && (Relative_KD_NW <= 0.9)) Gain_Formula_KNWF = Relative_KD_NW * 2 / 3 + 0.4;
        if (Relative_KD_NW > 0.9) Gain_Formula_KNWF = 1;
    } else { Gain_Formula_KNWF = 1; }

    if (Gain_Formula_PNWF < 0) Gain_Formula_PNWF = 0;

    Gain_Formula_Land = Enemy_Land * 0.12;

    Gain_Formula_Race = 1;
    if (Your_Race == 3) Gain_Formula_Race = 1.3;
    if (Your_Race == 7) Gain_Formula_Race = 1.3;

    if (Your_Stance == 3) Gain_Formula_Stance = 1.1;
    if (Your_Stance != 3) Gain_Formula_Stance = 1;

    if (Enemy_Relations == 2) Gain_Formula_Relations = 1.1;
    if (Enemy_Relations != 2) Gain_Formula_Relations = 1;

    Gain_Formula_Time = 1 + Your_Attack_Time / 100;
    Gain_Formula_Sciences = 1 + Your_Military_Sci / 100;

    if (Enemy_Guard_Station > 50) { Enemy_Guard_Station = 50; document.Calculator.EnemyGuardStation.value = 50; }
    Gain_Formula_GS = Building_Modifier_Attacked_Losses * (Enemy_Guard_Station / 100) * Enemy_BE * (1 - (Enemy_Guard_Station / 100));
    Gain_Formula_GS = 1 - Gain_Formula_GS / 100;

    if (Your_Dragon == 2) Gain_Formula_Dragon = 0.9;
    if (Your_Dragon != 2) Gain_Formula_Dragon = 1;

    if (Enemy_Relations != 3) Gain_Formula_GBP = 100 - 20 * Enemy_Recently_Hit;
    if (Enemy_Relations == 3) Gain_Formula_GBP = 100 - 5 * Enemy_Recently_Hit;

    Gain_Formula_GBP = Gain_Formula_GBP / 100;

    document.getElementById('GainFormulaLand').innerHTML = Math.round(Gain_Formula_Land * 100) / 100;
    document.getElementById('GainFormulaPNWF').innerHTML = Math.round(Gain_Formula_PNWF * 100) / 100;
    document.getElementById('GainFormulaKNWF').innerHTML = Math.round(Gain_Formula_KNWF * 100) / 100;
    document.getElementById('GainFormulaRace').innerHTML = Math.round(Gain_Formula_Race * 100) / 100;
    document.getElementById('GainFormulaStance').innerHTML = Math.round(Gain_Formula_Stance * 100) / 100;
    document.getElementById('GainFormulaRelations').innerHTML = Math.round(Gain_Formula_Relations * 100) / 100;
    document.getElementById('GainFormulaSciences').innerHTML = Math.round(Gain_Formula_Sciences * 100) / 100;
    document.getElementById('GainFormulaGS').innerHTML = Math.round(Gain_Formula_GS * 100) / 100;
    document.getElementById('GainFormulaDragon').innerHTML = Math.round(Gain_Formula_Dragon * 100) / 100;
    document.getElementById('GainFormulaGBP').innerHTML = Math.round(Gain_Formula_GBP * 100) / 100;
    document.getElementById('GainFormulaTime').innerHTML = Math.round(Gain_Formula_Time * 100) / 100;

    Main_Gain_Formula = Gain_Formula_Land * Gain_Formula_PNWF * Gain_Formula_KNWF * Gain_Formula_Race * Gain_Formula_Stance * Gain_Formula_Relations * Gain_Formula_Sciences * Gain_Formula_GS * Gain_Formula_Dragon * Gain_Formula_GBP * Gain_Formula_Time;

    if (Your_Relations == 3) { Gain_Formula_Cap_Value = 0.2; } else { Gain_Formula_Cap_Value = 0.162; }

    if (Your_Land < Enemy_Land) { Gain_Formula_Cap = Your_Land * Gain_Formula_Cap_Value; } else { Gain_Formula_Cap = Enemy_Land * Gain_Formula_Cap_Value; }


    if (Main_Gain_Formula > Gain_Formula_Cap) Main_Gain_Formula = Gain_Formula_Cap;


    if (Your_Relations == 5) {

        document.getElementById('MainGainFormula').innerHTML = Math.round(Main_Gain_Formula * 0.2);
        document.getElementById('MainGainFormula2').innerHTML = Math.round(Main_Gain_Formula * 0.8);

    } else {

        document.getElementById('MainGainFormula').innerHTML = Math.round(Main_Gain_Formula);
        document.getElementById('MainGainFormula2').innerHTML = Math.round(Main_Gain_Formula / Gain_Formula_Cap_Value * (Gain_Formula_Cap_Value + 0.02));

    }


    // ****************************
    //   Attack Result Estimation
    // ****************************



    if ((Your_Mod_Offense == 0) && (Enemy_Mod_Defense == 0)) { Chance = 0; } else { Chance = (Your_Mod_Offense / Enemy_Mod_Defense - 1) * 100; }

    if ((Chance > 0) && (Enemy_Mod_Defense > 0)) document.getElementById('ChanceInfo').innerHTML = "You have " + Math.round(Chance * 100) / 100 + "% more then enemy's Defensive strength";
    if ((Chance > 0) && (Enemy_Mod_Defense == 0)) document.getElementById('ChanceInfo').innerHTML = "Enemy has no Defense";

    if ((Chance < 0) && (Your_Mod_Offense > 0)) document.getElementById('ChanceInfo').innerHTML = "You have " + Math.round(Chance * -100) / 100 + "% less then enemy's Defensive strength";
    if ((Chance < 0) && (Your_Mod_Offense == 0)) document.getElementById('ChanceInfo').innerHTML = "You dont have any Offense";

    if (Chance == 0) document.getElementById('ChanceInfo').innerHTML = "You have the same strength as your enemy";

    Chance = Chance / 3.5;

    if (Chance > 1) Chance = 1;
    if (Chance < -1) Chance = -1;

    Chance = -25 * (Chance) * (Chance) * (Chance) + 75 * (Chance) + 50;

    Chance = Math.floor(Chance * 10) / 10;

    if ((0 < Chance) && (Chance <= 25)) document.getElementById('ChanceBar').style.background = "#AF0000";
    if ((25 < Chance) && (Chance <= 50)) document.getElementById('ChanceBar').style.background = "#C8009B";
    if ((50 < Chance) && (Chance <= 75)) document.getElementById('ChanceBar').style.background = "#9A00DA";
    if ((75 < Chance) && (Chance < 100)) document.getElementById('ChanceBar').style.background = "#0000AF";

    if ((Your_Mod_Offense == 0) && (Enemy_Mod_Defense == 0)) {
        document.getElementById('ChanceBar').style.width = "0px";
        document.getElementById('Chance').innerHTML = "<font color='#000000'>?</font>";
    } else {

        if ((0 < Chance) && (Chance <= 25)) document.getElementById('Chance').innerHTML = "<font color='#AF0000'>" + Chance + "%</font>";
        if ((25 < Chance) && (Chance <= 50)) document.getElementById('Chance').innerHTML = "<font color='#C8009B'>" + Chance + "%</font>";
        if ((50 < Chance) && (Chance <= 75)) document.getElementById('Chance').innerHTML = "<font color='#9A00DA'>" + Chance + "%</font>";
        if ((75 < Chance) && (Chance < 100)) document.getElementById('Chance').innerHTML = "<font color='#0000AF'>" + Chance + "%</font>";

        document.getElementById('ChanceBar').style.width = Chance + "%";

        if (Chance == 0) {
            document.getElementById('Chance').innerHTML = "<font color='#000000'>" + Chance + "%</font>";
            document.getElementById('ChanceBar').style.width = "100%";
            document.getElementById('ChanceBar').style.background = "#000000";
        }

        if (Chance == 100) {
            document.getElementById('Chance').innerHTML = "<font color='#006400'>" + Chance + "%</font>";
            document.getElementById('ChanceBar').style.width = "100%";
            document.getElementById('ChanceBar').style.background = "#006400";
        }

    }





}


function YourBonuses() {

    if (document.getElementById('YourBonusesBox').style.display == "block") {
        document.getElementById('YourBonusesBox').style.display = "none";
    } else {
        document.getElementById('SpecialBox').style.display = "none";
        document.getElementById('GainFormulaBox').style.display = "none";
        document.getElementById('YourBonusesBox').style.display = "block";
    }

    if ((document.getElementById('YourBonusesBox').style.display == "none") && (document.getElementById('EnemyBonusesBox').style.display == "none")) {
        if (document.Calculator.Special.value == "Special") {
            document.getElementById('SpecialBox').style.display = "none";
            document.getElementById('GainFormulaBox').style.display = "block";
        } else {
            document.getElementById('GainFormulaBox').style.display = "none";
            document.getElementById('SpecialBox').style.display = "block";
        }
    }


}

function EnemyBonuses() {

    if (document.getElementById('EnemyBonusesBox').style.display == "block") {
        document.getElementById('EnemyBonusesBox').style.display = "none";
    } else {
        document.getElementById('SpecialBox').style.display = "none";
        document.getElementById('GainFormulaBox').style.display = "none";
        document.getElementById('EnemyBonusesBox').style.display = "block";
    }

    if ((document.getElementById('YourBonusesBox').style.display == "none") && (document.getElementById('EnemyBonusesBox').style.display == "none")) {
        if (document.Calculator.Special.value == "Special") {
            document.getElementById('SpecialBox').style.display = "none";
            document.getElementById('GainFormulaBox').style.display = "block";
        } else {
            document.getElementById('GainFormulaBox').style.display = "none";
            document.getElementById('SpecialBox').style.display = "block";
        }
    }


}



function SpecialBox() {

    if (document.Calculator.Special.value == "Special") {
        if ((document.getElementById('YourBonusesBox').style.display == "none") && (document.getElementById('EnemyBonusesBox').style.display == "none")) {
            document.getElementById('GainFormulaBox').style.display = "none";
            document.getElementById('SpecialBox').style.display = "block";
        }
        document.Calculator.Special.value = "Modifiers";
    } else {
        if ((document.getElementById('YourBonusesBox').style.display == "none") && (document.getElementById('EnemyBonusesBox').style.display == "none")) {
            document.getElementById('SpecialBox').style.display = "none";
            document.getElementById('GainFormulaBox').style.display = "block";
        }
        document.Calculator.Special.value = "Special";
    }

}






function YourReset() {

    document.getElementById('YourProvinceName').innerHTML = "Attacker's Information";

    document.Calculator.YourRace.value = 0;
    document.Calculator.YourPersonality.value = 0;
    document.Calculator.YourRelations.value = 0;
    document.Calculator.YourStance.value = 0;
    document.Calculator.YourDragon.value = 0;
    document.Calculator.YourSoldiers.value = 0;
    document.Calculator.YourOffSpecs.value = 0;
    document.Calculator.YourDefSpecs.value = 0;
    document.Calculator.YourElites.value = 0;
    document.Calculator.YourLand.value = 0;
    document.Calculator.YourNW.value = 0;
    document.Calculator.YourKDNW.value = 0;
    document.Calculator.YourOME.value = 100;
    document.Calculator.YourDME.value = 100;
    document.Calculator.YourGenerals.value = 0;
    document.Calculator.YourAttackTime.value = 0;
    document.Calculator.YourHorses.value = 0; document.Calculator.YourOptimizeHorses.disabled = true;
    document.Calculator.YourMerc.value = 0; document.Calculator.YourMaxMerc.disabled = true;
    document.Calculator.YourPris.value = 0; document.Calculator.YourMaxPris.disabled = true;
    document.Calculator.YourMilitarySci.value = 0;
    document.Calculator.YourRecentlyHit.value = 0;
    document.Calculator.YourGuardStation.value = 0;
    document.Calculator.YourPeasants.value = 0;
    document.Calculator.YourRawME.value = 100;
    document.Calculator.YourBE.value = 100;
    document.Calculator.YourTG.value = 0;
    document.Calculator.YourForts.value = 0;
    document.Calculator.YourRank.value = 0;
    document.Calculator.YourFanaticismBox.checked = false;
    document.Calculator.YourAggressionBox.checked = false;
    document.Calculator.YourProtectionBox.checked = true;
    document.Calculator.YourPlagueBox.checked = false;

}

function EnemyReset() {

    document.getElementById('EnemyProvinceName').innerHTML = "Defender's Information";
    document.getElementById('EnemyPlagueImage').style.display = "none";

    document.Calculator.EnemyRace.value = 0;
    document.Calculator.EnemyPersonality.value = 0;
    document.Calculator.EnemyRelations.value = 0;
    document.Calculator.EnemyStance.value = 0;
    document.Calculator.EnemyDragon.value = 0;
    document.Calculator.EnemySoldiers.value = 0;
    document.Calculator.EnemyOffSpecs.value = 0;
    document.Calculator.EnemyDefSpecs.value = 0;
    document.Calculator.EnemyElites.value = 0;
    document.Calculator.EnemyLand.value = 0;
    document.Calculator.EnemyNW.value = 0;
    document.Calculator.EnemyKDNW.value = 0;
    document.Calculator.EnemyOME.value = 100;
    document.Calculator.EnemyDME.value = 100;
    document.Calculator.EnemyGenerals.value = 0;
    document.Calculator.EnemyAttackTime.value = 0;
    document.Calculator.EnemyHorses.value = 0;
    document.Calculator.EnemyMerc.value = 0;
    document.Calculator.EnemyPris.value = 0;
    document.Calculator.EnemyMilitarySci.value = 0;
    document.Calculator.EnemyRecentlyHit.value = 0;
    document.Calculator.EnemyGuardStation.value = 0;
    document.Calculator.EnemyPeasants.value = 0;
    document.Calculator.EnemyRawME.value = 100;
    document.Calculator.EnemyBE.value = 100;
    document.Calculator.EnemyTG.value = 0;
    document.Calculator.EnemyForts.value = 0;
    document.Calculator.EnemyRank.value = 0;
    document.Calculator.EnemyFanaticismBox.checked = false;
    document.Calculator.EnemyAggressionBox.checked = false;
    document.Calculator.EnemyProtectionBox.checked = true;
    document.Calculator.EnemyPlagueBox.checked = false;

}

function YourMove() {

    if (Enemy_Province_Name != "Defender's Information") document.getElementById('YourProvinceName').innerHTML = Enemy_Province_Name;

    document.Calculator.YourRace.value = Enemy_Race;
    document.Calculator.YourPersonality.value = Enemy_Personality;
    document.Calculator.YourRelations.value = Enemy_Relations;
    document.Calculator.YourStance.value = Enemy_Stance;
    document.Calculator.YourDragon.value = Enemy_Dragon;
    document.Calculator.YourSoldiers.value = Enemy_Soldiers;
    document.Calculator.YourOffSpecs.value = Enemy_Offensive_Units;
    document.Calculator.YourDefSpecs.value = Enemy_Defensive_Units;
    document.Calculator.YourElites.value = Enemy_Elite_Units;
    document.Calculator.YourLand.value = Enemy_Land;
    document.Calculator.YourNW.value = Enemy_NW;
    document.Calculator.YourKDNW.value = Enemy_KD_NW;
    document.Calculator.YourOME.value = Enemy_OME;
    document.Calculator.YourDME.value = Enemy_DME;
    document.Calculator.YourGenerals.value = Enemy_Generals;
    document.Calculator.YourAttackTime.value = Enemy_Attack_Time;
    document.Calculator.YourHorses.value = Enemy_Horses;
    document.Calculator.YourMerc.value = Enemy_Mercenaries;
    document.Calculator.YourPris.value = Enemy_Prisoners;
    document.Calculator.YourMilitarySci.value = Enemy_Military_Sci;
    document.Calculator.YourRecentlyHit.value = Enemy_Recently_Hit;
    document.Calculator.YourGuardStation.value = Enemy_Guard_Station;
    document.Calculator.YourPeasants.value = Enemy_Peasants;
    document.Calculator.YourRawME.value = Enemy_Raw_ME;
    document.Calculator.YourBE.value = Enemy_BE;
    document.Calculator.YourTG.value = Enemy_TG;
    document.Calculator.YourForts.value = Enemy_Forts;
    document.Calculator.YourRank.value = Enemy_Rank;

    if (Enemy_Monarch_Box == 0) document.Calculator.YourMonarchBox.checked = false;
    if (Enemy_Peasants_Box == 0) document.Calculator.YourPeasantsBox.checked = false;
    if (Enemy_Fanaticism_Box == 0) document.Calculator.YourFanaticismBox.checked = false;
    if (Enemy_Aggression_Box == 0) document.Calculator.YourAggressionBox.checked = false;
    if (Enemy_Protection_Box == 0) document.Calculator.YourProtectionBox.checked = false;
    if (Enemy_Plague_Box == 0) document.Calculator.YourPlagueBox.checked = false;

    if (Enemy_Monarch_Box == 1) document.Calculator.YourMonarchBox.checked = true;
    if (Enemy_Peasants_Box == 1) document.Calculator.YourPeasantsBox.checked = true;
    if (Enemy_Fanaticism_Box == 1) document.Calculator.YourFanaticismBox.checked = true;
    if (Enemy_Aggression_Box == 1) document.Calculator.YourAggressionBox.checked = true;
    if (Enemy_Protection_Box == 1) document.Calculator.YourProtectionBox.checked = true;
    if (Enemy_Plague_Box == 1) document.Calculator.YourPlagueBox.checked = true;

}

function EnemyMove() {

    if (Your_Province_Name != "Attacker's Information") document.getElementById('EnemyProvinceName').innerHTML = Your_Province_Name;

    document.Calculator.EnemyRace.value = Your_Race;
    document.Calculator.EnemyPersonality.value = Your_Personality;
    document.Calculator.EnemyRelations.value = Your_Relations;
    document.Calculator.EnemyStance.value = Your_Stance;
    document.Calculator.EnemyDragon.value = Your_Dragon;
    document.Calculator.EnemySoldiers.value = Your_Soldiers;
    document.Calculator.EnemyOffSpecs.value = Your_Offensive_Units;
    document.Calculator.EnemyDefSpecs.value = Your_Defensive_Units;
    document.Calculator.EnemyElites.value = Your_Elite_Units;
    document.Calculator.EnemyLand.value = Your_Land;
    document.Calculator.EnemyNW.value = Your_NW;
    document.Calculator.EnemyKDNW.value = Your_KD_NW;
    document.Calculator.EnemyOME.value = Your_OME;
    document.Calculator.EnemyDME.value = Your_DME;
    document.Calculator.EnemyGenerals.value = Your_Generals;
    document.Calculator.EnemyAttackTime.value = Your_Attack_Time;
    document.Calculator.EnemyHorses.value = Your_Horses;
    document.Calculator.EnemyMerc.value = Your_Mercenaries;
    document.Calculator.EnemyPris.value = Your_Prisoners;
    document.Calculator.EnemyMilitarySci.value = Your_Military_Sci;
    document.Calculator.EnemyRecentlyHit.value = Your_Recently_Hit;
    document.Calculator.EnemyGuardStation.value = Your_Guard_Station;
    document.Calculator.EnemyPeasants.value = Your_Peasants;
    document.Calculator.EnemyRawME.value = Your_Raw_ME;
    document.Calculator.EnemyBE.value = Your_BE;
    document.Calculator.EnemyTG.value = Your_TG;
    document.Calculator.EnemyForts.value = Your_Forts;
    document.Calculator.EnemyRank.value = Your_Rank;

    if (Your_Monarch_Box == 0) document.Calculator.EnemyMonarchBox.checked = false;
    if (Your_Peasants_Box == 0) document.Calculator.EnemyPeasantsBox.checked = false;
    if (Your_Fanaticism_Box == 0) document.Calculator.EnemyFanaticismBox.checked = false;
    if (Your_Aggression_Box == 0) document.Calculator.EnemyAggressionBox.checked = false;
    if (Your_Protection_Box == 0) document.Calculator.EnemyProtectionBox.checked = false;
    if (Your_Plague_Box == 0) document.Calculator.EnemyPlagueBox.checked = false;

    if (Your_Monarch_Box == 1) document.Calculator.EnemyMonarchBox.checked = true;
    if (Your_Peasants_Box == 1) document.Calculator.EnemyPeasantsBox.checked = true;
    if (Your_Fanaticism_Box == 1) document.Calculator.EnemyFanaticismBox.checked = true;
    if (Your_Aggression_Box == 1) document.Calculator.EnemyAggressionBox.checked = true;
    if (Your_Protection_Box == 1) document.Calculator.EnemyProtectionBox.checked = true;
    if (Your_Plague_Box == 1) document.Calculator.EnemyPlagueBox.checked = true;

}

function Swap() {

    if (Enemy_Province_Name != "Defender's Information") document.getElementById('YourProvinceName').innerHTML = Enemy_Province_Name;
    if (Your_Province_Name != "Attacker's Information") document.getElementById('EnemyProvinceName').innerHTML = Your_Province_Name;

    if (Enemy_Province_Name == "Defender's Information") document.getElementById('YourProvinceName').innerHTML = "Attacker's Information";
    if (Your_Province_Name == "Attacker's Information") document.getElementById('EnemyProvinceName').innerHTML = "Defender's Information";

    document.Calculator.YourRace.value = Enemy_Race; document.Calculator.EnemyRace.value = Your_Race;
    document.Calculator.YourPersonality.value = Enemy_Personality; document.Calculator.EnemyPersonality.value = Your_Personality;
    document.Calculator.YourRelations.value = Enemy_Relations; document.Calculator.EnemyRelations.value = Your_Relations;
    document.Calculator.YourStance.value = Enemy_Stance; document.Calculator.EnemyStance.value = Your_Stance;
    document.Calculator.YourDragon.value = Enemy_Dragon; document.Calculator.EnemyDragon.value = Your_Dragon;
    document.Calculator.YourSoldiers.value = Enemy_Soldiers; document.Calculator.EnemySoldiers.value = Your_Soldiers;
    document.Calculator.YourOffSpecs.value = Enemy_Offensive_Units; document.Calculator.EnemyOffSpecs.value = Your_Offensive_Units;
    document.Calculator.YourDefSpecs.value = Enemy_Defensive_Units; document.Calculator.EnemyDefSpecs.value = Your_Defensive_Units;
    document.Calculator.YourElites.value = Enemy_Elite_Units; document.Calculator.EnemyElites.value = Your_Elite_Units;
    document.Calculator.YourLand.value = Enemy_Land; document.Calculator.EnemyLand.value = Your_Land;
    document.Calculator.YourNW.value = Enemy_NW; document.Calculator.EnemyNW.value = Your_NW;
    document.Calculator.YourKDNW.value = Enemy_KD_NW; document.Calculator.EnemyKDNW.value = Your_KD_NW;
    document.Calculator.YourOME.value = Enemy_OME; document.Calculator.EnemyOME.value = Your_OME;
    document.Calculator.YourDME.value = Enemy_DME; document.Calculator.EnemyDME.value = Your_DME;
    document.Calculator.YourGenerals.value = Enemy_Generals; document.Calculator.EnemyGenerals.value = Your_Generals;
    document.Calculator.YourAttackTime.value = Enemy_Attack_Time; document.Calculator.EnemyAttackTime.value = Your_Attack_Time;
    document.Calculator.YourHorses.value = Enemy_Horses; document.Calculator.EnemyHorses.value = Your_Horses;
    document.Calculator.YourMerc.value = Enemy_Mercenaries; document.Calculator.EnemyMerc.value = Your_Mercenaries;
    document.Calculator.YourPris.value = Enemy_Prisoners; document.Calculator.EnemyPris.value = Your_Prisoners;
    document.Calculator.YourMilitarySci.value = Enemy_Military_Sci; document.Calculator.EnemyMilitarySci.value = Your_Military_Sci;
    document.Calculator.YourRecentlyHit.value = Enemy_Recently_Hit; document.Calculator.EnemyRecentlyHit.value = Your_Recently_Hit;
    document.Calculator.YourGuardStation.value = Enemy_Guard_Station; document.Calculator.EnemyGuardStation.value = Your_Guard_Station;
    document.Calculator.YourPeasants.value = Enemy_Peasants; document.Calculator.EnemyPeasants.value = Your_Peasants;
    document.Calculator.YourRawME.value = Enemy_Raw_ME; document.Calculator.EnemyRawME.value = Your_Raw_ME;
    document.Calculator.YourBE.value = Enemy_BE; document.Calculator.EnemyBE.value = Your_BE;
    document.Calculator.YourTG.value = Enemy_TG; document.Calculator.EnemyTG.value = Your_TG;
    document.Calculator.YourForts.value = Enemy_Forts; document.Calculator.EnemyForts.value = Your_Forts;
    document.Calculator.YourRank.value = Enemy_Rank; document.Calculator.EnemyRank.value = Your_Rank;

    if (Enemy_Monarch_Box == 0) document.Calculator.YourMonarchBox.checked = false;
    if (Enemy_Peasants_Box == 0) document.Calculator.YourPeasantsBox.checked = false;
    if (Enemy_Fanaticism_Box == 0) document.Calculator.YourFanaticismBox.checked = false;
    if (Enemy_Aggression_Box == 0) document.Calculator.YourAggressionBox.checked = false;
    if (Enemy_Protection_Box == 0) document.Calculator.YourProtectionBox.checked = false;
    if (Enemy_Plague_Box == 0) document.Calculator.YourPlagueBox.checked = false;

    if (Enemy_Monarch_Box == 1) document.Calculator.YourMonarchBox.checked = true;
    if (Enemy_Peasants_Box == 1) document.Calculator.YourPeasantsBox.checked = true;
    if (Enemy_Fanaticism_Box == 1) document.Calculator.YourFanaticismBox.checked = true;
    if (Enemy_Aggression_Box == 1) document.Calculator.YourAggressionBox.checked = true;
    if (Enemy_Protection_Box == 1) document.Calculator.YourProtectionBox.checked = true;
    if (Enemy_Plague_Box == 1) document.Calculator.YourPlagueBox.checked = true;

    if (Your_Monarch_Box == 0) document.Calculator.EnemyMonarchBox.checked = false;
    if (Your_Peasants_Box == 0) document.Calculator.EnemyPeasantsBox.checked = false;
    if (Your_Fanaticism_Box == 0) document.Calculator.EnemyFanaticismBox.checked = false;
    if (Your_Aggression_Box == 0) document.Calculator.EnemyAggressionBox.checked = false;
    if (Your_Protection_Box == 0) document.Calculator.EnemyProtectionBox.checked = false;
    if (Your_Plague_Box == 0) document.Calculator.EnemyPlagueBox.checked = false;

    if (Your_Monarch_Box == 1) document.Calculator.EnemyMonarchBox.checked = true;
    if (Your_Peasants_Box == 1) document.Calculator.EnemyPeasantsBox.checked = true;
    if (Your_Fanaticism_Box == 1) document.Calculator.EnemyFanaticismBox.checked = true;
    if (Your_Aggression_Box == 1) document.Calculator.EnemyAggressionBox.checked = true;
    if (Your_Protection_Box == 1) document.Calculator.EnemyProtectionBox.checked = true;
    if (Your_Plague_Box == 1) document.Calculator.EnemyPlagueBox.checked = true;

}
function ThiefMoney() {
    document.getElementById('Thief_Money').checked = true;
    document.getElementById('Thievery_Resource_Title').innerHTML = "Money";
    document.getElementById('Thievery_Operation_Title').innerHTML = "Stolen";
    document.getElementById('Thievery_Info').innerHTML = "Steals gold from enemy coffers.";
    document.Calculator6.Thievery_Operation.value = "RobtheVaults";
}

function ThiefFood() {
    document.getElementById('Thief_Food').checked = true;
    document.getElementById('Thievery_Resource_Title').innerHTML = "Food";
    document.getElementById('Thievery_Operation_Title').innerHTML = "Stolen";
    document.getElementById('Thievery_Info').innerHTML = "Steals food from your opponent's storages.";
    document.Calculator6.Thievery_Operation.value = "RobtheGranaries";
}

function ThiefRunes() {
    document.getElementById('Thief_Runes').checked = true;
    document.getElementById('Thievery_Resource_Title').innerHTML = "Runes";
    document.getElementById('Thievery_Operation_Title').innerHTML = "Stolen";
    document.getElementById('Thievery_Info').innerHTML = "Steals runes from your target's wizards.";
    document.Calculator6.Thievery_Operation.value = "RobtheTowers";
}

function ThiefPeasants() {
    document.getElementById('Thief_Peasants').checked = true;
    document.getElementById('Thievery_Resource_Title').innerHTML = "Peasants";
    document.getElementById('Thievery_Operation_Title').innerHTML = "Kidnapped";
    document.getElementById('Thievery_Info').innerHTML = "Kidnaps peasants from your enemy and brings them to your province.";
    document.Calculator6.Thievery_Operation.value = "Kidnapping";
}

function ThiefWizards() {
    document.getElementById('Thief_Wizards').checked = true;
    document.getElementById('Thievery_Resource_Title').innerHTML = "Wizards";
    document.getElementById('Thievery_Operation_Title').innerHTML = "Killed";
    document.getElementById('Thievery_Info').innerHTML = "Attempts to assassinate enemy wizards to permanently weaken their ability to cast spells.";
    document.Calculator6.Thievery_Operation.value = "AssassinateWizards";
}

function ThiefSoldiers() {
    document.getElementById('Thief_Soldiers').checked = true;
    document.getElementById('Thievery_Resource_Title').innerHTML = "Soldiers";
    document.getElementById('Thievery_Operation_Title').innerHTML = "Killed";
    document.getElementById('Thievery_Info').innerHTML = "Assassinates a portion of your enemy's military, both at home and away.";
    document.Calculator6.Thievery_Operation.value = "NightStrike";
}

function ThiefOffSpecs() {
    document.getElementById('Thief_OffSpecs').checked = true;
    document.getElementById('Thievery_Resource_Title').innerHTML = "OffSpecs";
    document.getElementById('Thievery_Operation_Title').innerHTML = "Killed";
    document.getElementById('Thievery_Info').innerHTML = "Assassinates a portion of your enemy's military, both at home and away.";
    document.Calculator6.Thievery_Operation.value = "NightStrike";
}

function ThiefDefSpecs() {
    document.getElementById('Thief_DefSpecs').checked = true;
    document.getElementById('Thievery_Resource_Title').innerHTML = "DefSpecs";
    document.getElementById('Thievery_Operation_Title').innerHTML = "Killed";
    document.getElementById('Thievery_Info').innerHTML = "Assassinates a portion of your enemy's military, both at home and away.";
    document.Calculator6.Thievery_Operation.value = "NightStrike";
}

function ThiefElites() {
    document.getElementById('Thief_Elites').checked = true;
    document.getElementById('Thievery_Resource_Title').innerHTML = "Elites";
    document.getElementById('Thievery_Operation_Title').innerHTML = "Killed";
    document.getElementById('Thievery_Info').innerHTML = "Assassinates a portion of your enemy's military, both at home and away.";
    document.Calculator6.Thievery_Operation.value = "NightStrike";
}

function ThiefHorses() {
    document.getElementById('Thief_Horses').checked = true;
    document.getElementById('Thievery_Resource_Title').innerHTML = "Horses";
    document.getElementById('Thievery_Operation_Title').innerHTML = "Stolen";
    document.getElementById('Thievery_Info').innerHTML = "Steals an enemy's horses for your own use.";
    document.Calculator6.Thievery_Operation.value = "StealHorses";
}

function ThiefBuildings() {
    document.getElementById('Thief_Buildings').checked = true;
    document.getElementById('Thievery_Resource_Title').innerHTML = "Buildings";
    document.getElementById('Thievery_Operation_Title').innerHTML = "Burnt";
    document.getElementById('Thievery_Info').innerHTML = "Burns down enemy buildings to disrupt an enemy's stability.";
    if (document.Calculator6.Thievery_Operation.value != "GreaterArson") document.Calculator6.Thievery_Operation.value = "Arson";
    if (document.Calculator6.Thievery_Operation.value == "GreaterArson") document.getElementById('Thievery_Info').innerHTML = "A more powerful version of Arson, allows targetting of a specific type of building to burn down.";
}

function ThiefPrisoners() {
    document.getElementById('Thief_Prisoners').checked = true;
    document.getElementById('Thievery_Resource_Title').innerHTML = "Prisoners";
    document.getElementById('Thievery_Operation_Title').innerHTML = "Freed";
    document.getElementById('Thievery_Info').innerHTML = "Releases prisoners from an opponent's dungeons.";
    document.Calculator6.Thievery_Operation.value = "FreePrisoners";
}

function Task_Thievery() {

    var Thievery_Operation = document.Calculator6.Thievery_Operation.value;

    if (Thievery_Operation == "RobtheVaults") ThiefMoney();
    if (Thievery_Operation == "RobtheGranaries") ThiefFood();
    if (Thievery_Operation == "RobtheTowers") ThiefRunes();
    if (Thievery_Operation == "Kidnapping") ThiefPeasants();
    if (Thievery_Operation == "AssassinateWizards") ThiefWizards();
    if (Thievery_Operation == "NightStrike") if ((document.getElementById('Thief_OffSpecs').checked == false) && (document.getElementById('Thief_DefSpecs').checked == false) && (document.getElementById('Thief_Elites').checked == false)) ThiefSoldiers();
    if (Thievery_Operation == "StealHorses") ThiefHorses();
    if (Thievery_Operation == "Arson") ThiefBuildings();
    if (Thievery_Operation == "GreaterArson") ThiefBuildings();
    if (Thievery_Operation == "FreePrisoners") ThiefPrisoners();

    if (document.Calculator6.EnemyRelations.value != 0) {

        if (document.Calculator6.YourPersonality.value == "Rogue") {
            document.getElementById('Thief_Wizards_Name').innerHTML = "Wizards";
            document.Calculator6.Thievery_Wizards.disabled = false;
        } else {
            document.getElementById('Thief_Wizards_Name').innerHTML = '<input style="border: 0px solid ; font-weight: bold; background-color: transparent;" size="9" value="Wizards"  disabled="disabled" type="text">';
            document.Calculator6.Thievery_Wizards.disabled = true;
        }

        document.getElementById('Thief_Soldiers_Name').innerHTML = "Soldiers";
        document.getElementById('Thief_OffSpecs_Name').innerHTML = "OffSpecs";
        document.getElementById('Thief_DefSpecs_Name').innerHTML = "DefSpecs";
        document.getElementById('Thief_Elites_Name').innerHTML = "Elites";

        document.Calculator6.Thievery_Soldiers.disabled = false;
        document.Calculator6.Thievery_OffSpecs.disabled = false;
        document.Calculator6.Thievery_DefSpecs.disabled = false;
        document.Calculator6.Thievery_Elites.disabled = false;

    } else {

        document.getElementById('Thief_Wizards_Name').innerHTML = '<input style="border: 0px solid ; font-weight: bold; background-color: transparent;" size="9" value="Wizards"  disabled="disabled" type="text" />';
        document.getElementById('Thief_Soldiers_Name').innerHTML = '<input style="border: 0px solid ; font-weight: bold; background-color: transparent;" size="9" value="Soldiers" disabled="disabled" type="text" />';
        document.getElementById('Thief_OffSpecs_Name').innerHTML = '<input style="border: 0px solid ; font-weight: bold; background-color: transparent;" size="9" value="OffSpecs" disabled="disabled" type="text" />';
        document.getElementById('Thief_DefSpecs_Name').innerHTML = '<input style="border: 0px solid ; font-weight: bold; background-color: transparent;" size="9" value="DefSpecs" disabled="disabled" type="text" />';
        document.getElementById('Thief_Elites_Name').innerHTML = '<input style="border: 0px solid ; font-weight: bold; background-color: transparent;" size="9" value="Elites"   disabled="disabled" type="text" />';

        document.Calculator6.Thievery_Wizards.disabled = true;
        document.Calculator6.Thievery_Soldiers.disabled = true;
        document.Calculator6.Thievery_OffSpecs.disabled = true;
        document.Calculator6.Thievery_DefSpecs.disabled = true;
        document.Calculator6.Thievery_Elites.disabled = true;

    }


    var Thievery_Resource = 0;
    var Thievery_Resources_Gains = 0;
    var Thievery_Relative_NW = 0;
    var Thievery_WarBonus = 1;
    var Thievery_YourThieves = 0;
    var Thievery_YourNetworth = 0;
    var Thievery_EnemyNetworth = 0;
    var Thievery_Gains = 0;
    var Thievery_Losts = 0;
    var Thievery_Rate = 0;
    var Thievery_Max = 0;
    var Thievery_Optimal = 0;
    var Thievery_Penalty = 0;
    var Thief_Per_Resource = 0;
    var Thief_Max_Resource = 0;

    Thievery_YourThieves = Number(document.Calculator6.YourThieves.value);
    Thievery_YourNetworth = Number(document.Calculator6.YourNetworth.value);
    Thievery_EnemyNetworth = Number(document.Calculator6.EnemyNetworth.value);

    if (document.Calculator6.EnemyRelations.value == 0) Thievery_Penalty = 1 - Thievery_Penalty_None / 100;
    if (document.Calculator6.EnemyRelations.value == 1) Thievery_Penalty = 1 - Thievery_Penalty_Unfriendly / 100;
    if (document.Calculator6.EnemyRelations.value == 2) Thievery_Penalty = 1 - Thievery_Penalty_Hostile / 100;
    if (document.Calculator6.EnemyRelations.value == 3) Thievery_Penalty = 1 - Thievery_Penalty_War / 100;
    if (document.Calculator6.EnemyRelations.value == 4) Thievery_Penalty = 1 - Thievery_Penalty_OSW / 100;

    if (Thievery_YourNetworth > Thievery_EnemyNetworth) Thievery_Relative_NW = Thievery_EnemyNetworth / Thievery_YourNetworth;
    if (Thievery_YourNetworth < Thievery_EnemyNetworth) Thievery_Relative_NW = Thievery_YourNetworth / Thievery_EnemyNetworth;
    if (Thievery_YourNetworth == Thievery_EnemyNetworth) Thievery_Relative_NW = 1;

    if (Thievery_Operation == "RobtheVaults") {
        Thievery_Resources_Gains = 0.9; Thief_Per_Resource = Thief_Per_Resource_RobtheVaults; Thief_Max_Resource = Thief_Max_Resource_RobtheVaults; Thievery_Max = Thievery_Penalty * Thief_Max_Resource / 100; Thievery_Rate = Thievery_Max * Thief_Per_Resource; Thievery_Resource = Number(document.Calculator6.Thievery_Money.value);

        if (document.Calculator6.EnemyRelations.value == 2) Thievery_WarBonus = 1.45;
        if (document.Calculator6.EnemyRelations.value == 3) Thievery_WarBonus = 1.9;

    }
    if (Thievery_Operation == "RobtheGranaries") { Thievery_Resources_Gains = 0.9; Thief_Per_Resource = Thief_Per_Resource_RobtheGranaries; Thief_Max_Resource = Thief_Max_Resource_RobtheGranaries; Thievery_Max = Thievery_Penalty * Thief_Max_Resource / 100; Thievery_Rate = Thievery_Max * Thief_Per_Resource; Thievery_Resource = Number(document.Calculator6.Thievery_Food.value); }
    if (Thievery_Operation == "RobtheTowers") { Thievery_Resources_Gains = 1; Thief_Per_Resource = Thief_Per_Resource_RobtheTowers; Thief_Max_Resource = Thief_Max_Resource_RobtheTowers; Thievery_Max = Thievery_Penalty * Thief_Max_Resource / 100; Thievery_Rate = Thievery_Max * Thief_Per_Resource; Thievery_Resource = Number(document.Calculator6.Thievery_Runes.value); }
    if (Thievery_Operation == "Kidnapping") { Thievery_Resources_Gains = 0.8; Thief_Per_Resource = Thief_Per_Resource_Kidnapping; Thief_Max_Resource = Thief_Max_Resource_Kidnapping; Thievery_Max = Thievery_Penalty * Thief_Max_Resource / 100; Thievery_Rate = Thievery_Max * Thief_Per_Resource; Thievery_Resource = Number(document.Calculator6.Thievery_Peasants.value); if (document.Calculator6.EnemyRelations.value == 3) Thievery_WarBonus = 2; }
    if (Thievery_Operation == "AssassinateWizards") { Thievery_Resources_Gains = 1; Thief_Per_Resource = Thief_Per_Resource_AssassinateWizards; Thief_Max_Resource = Thief_Max_Resource_AssassinateWizards; Thievery_Max = Thievery_Penalty * Thief_Max_Resource / 100; Thievery_Rate = Thievery_Max * Thief_Per_Resource; Thievery_Resource = Number(document.Calculator6.Thievery_Wizards.value); }
    if (Thievery_Operation == "StealHorses") { Thievery_Resources_Gains = 0.5; Thief_Per_Resource = Thief_Per_Resource_StealHorses; Thief_Max_Resource = Thief_Max_Resource_StealHorses; Thievery_Max = Thievery_Penalty * Thief_Max_Resource / 100; Thievery_Rate = Thievery_Max * Thief_Per_Resource; Thievery_Resource = Number(document.Calculator6.Thievery_Horses.value); }
    if (Thievery_Operation == "Arson") { Thievery_Resources_Gains = 1; Thief_Per_Resource = Thief_Per_Resource_Arson; Thief_Max_Resource = Thief_Max_Resource_Arson; Thievery_Max = Thievery_Penalty * Thief_Max_Resource / 100; Thievery_Rate = Thievery_Max * Thief_Per_Resource; Thievery_Resource = Number(document.Calculator6.Thievery_Buildings.value); }
    if (Thievery_Operation == "GreaterArson") { Thievery_Resources_Gains = 1; Thief_Per_Resource = Thief_Per_Resource_GreaterArson; Thief_Max_Resource = Thief_Max_Resource_GreaterArson; Thievery_Max = Thievery_Penalty * Thief_Max_Resource / 100; Thievery_Rate = Thievery_Max * Thief_Per_Resource; Thievery_Resource = Number(document.Calculator6.Thievery_Buildings.value); }
    if (Thievery_Operation == "FreePrisoners") { Thievery_Resources_Gains = 1; Thief_Per_Resource = Thief_Per_Resource_FreePrisoners; Thief_Max_Resource = Thief_Max_Resource_FreePrisoners; Thievery_Max = Thievery_Penalty * Thief_Max_Resource / 100; Thievery_Rate = Thievery_Max * Thief_Per_Resource; Thievery_Resource = Number(document.Calculator6.Thievery_Prisoners.value); }

    if (Thievery_Operation == "NightStrike") {

        if (document.getElementById('Thief_Soldiers').checked == true) { Thievery_Resources_Gains = 1; Thief_Per_Resource = Thief_Per_Resource_NightStrike_Soldiers; Thief_Max_Resource = Thief_Max_Resource_NightStrike_Soldiers; Thievery_Max = Thievery_Penalty * Thief_Max_Resource / 100; Thievery_Rate = Thievery_Max * Thief_Per_Resource; Thievery_Resource = Number(document.Calculator6.Thievery_Soldiers.value); }
        if (document.getElementById('Thief_OffSpecs').checked == true) { Thievery_Resources_Gains = 1; Thief_Per_Resource = Thief_Per_Resource_NightStrike_OffSpecs; Thief_Max_Resource = Thief_Max_Resource_NightStrike_OffSpecs; Thievery_Max = Thievery_Penalty * Thief_Max_Resource / 100; Thievery_Rate = Thievery_Max * Thief_Per_Resource; Thievery_Resource = Number(document.Calculator6.Thievery_OffSpecs.value); }
        if (document.getElementById('Thief_DefSpecs').checked == true) { Thievery_Resources_Gains = 1; Thief_Per_Resource = Thief_Per_Resource_NightStrike_DefSpecs; Thief_Max_Resource = Thief_Max_Resource_NightStrike_DefSpecs; Thievery_Max = Thievery_Penalty * Thief_Max_Resource / 100; Thievery_Rate = Thievery_Max * Thief_Per_Resource; Thievery_Resource = Number(document.Calculator6.Thievery_DefSpecs.value); }
        if (document.getElementById('Thief_Elites').checked == true) { Thievery_Resources_Gains = 1; Thief_Per_Resource = Thief_Per_Resource_NightStrike_Elites; Thief_Max_Resource = Thief_Max_Resource_NightStrike_Elites; Thievery_Max = Thievery_Penalty * Thief_Max_Resource / 100; Thievery_Rate = Thievery_Max * Thief_Per_Resource; Thievery_Resource = Number(document.Calculator6.Thievery_Elites.value); }

    }

    Thievery_Optimal = Thievery_Resource * Thievery_Max / Thievery_Rate;
    if (Thievery_Optimal > Thievery_YourThieves) Thievery_Optimal = Thievery_YourThieves;
    Thievery_Gains = Math.round(Thievery_Optimal * Thievery_Relative_NW * Thievery_Rate * Thievery_WarBonus * Thievery_Resources_Gains);
    Thievery_Losts = Math.round(Thievery_Optimal * Thievery_Relative_NW * Thievery_Rate * Thievery_WarBonus);

    Thievery_Optimal = Math.round(Thievery_Optimal);


    document.getElementById('Thievery_Relative_NW').innerHTML = Math.round(Thievery_Relative_NW * 100) / 100;
    document.getElementById('Thievery_Rate').innerHTML = Math.round(Thievery_Rate * 100) / 100;
    document.getElementById('Thievery_Max').innerHTML = Math.round(Thievery_WarBonus * Thievery_Max * 10000) / 100;
    document.getElementById('Thievery_Losts').innerHTML = Math.round((1 - Thievery_Resources_Gains) * 10000) / 100;

    document.getElementById('Thievery_Penalty').innerHTML = Math.round((1 - Thievery_Penalty) * 100);
    document.getElementById('Thievery_Resources_Gains').innerHTML = Math.round((Thievery_Resources_Gains) * 10000) / 100;
    document.getElementById('Thief_Per_Resource').innerHTML = Thief_Per_Resource;
    document.getElementById('Thief_Max_Resource').innerHTML = Thief_Max_Resource;


    temp = "";
    temp += "<table cellpadding='2' cellspacing='0' width='100%'>";

    var List_Thieves = Thievery_YourThieves;


    textonly = c0 + "Thievery Report Information" + cc + Copyrights +
"<br />" +
"<br />" + c0 + "** Thief's Information **" + cc +
"<br />" + c1 + "Thieves: " + cc + c2 + Thievery_YourThieves + cc +
"<br />" + c1 + "Optimal Thieves: " + cc + c2 + Thievery_Optimal + cc +
"<br />" + c1 + "Networth: " + cc + c2 + Thievery_YourNetworth + cc +
"<br />" +
"<br />" + c0 + "** Defender's Information **" + cc +
"<br />" + c1 + "Networth: " + cc + c2 + Thievery_EnemyNetworth + cc +
"<br />" +
"<br />" + c0 + "** Operation Analysis **" + cc +
"<br />" + c1 + "Operation: " + cc + c2 + Thievery_Operation + cc +
"<br />" + c1 + "Resource Per Thief: " + cc + c2 + Thief_Per_Resource + cc +
"<br />" + c1 + "Thief Max Resource: " + cc + c2 + Thief_Max_Resource + cc +
"<br />" + c1 + "Penalty: " + cc + c2 + Thievery_Penalty + cc +
"<br />" +
"<br />";


    for (i = 1; i <= 10; i++) {
        temp += "<tr><td align='center' width='10'>" + i + "</td><td align='center' width='70'>" + Add_Commas(Thievery_Resource) + "</td><td align='center' width='50'>" + Add_Commas(Thievery_Optimal) + "</td><td align='center' width='70'>" + Add_Commas(Thievery_Gains) + "</td><td align='center' width='50'>" + Add_Commas(Thievery_Losts) + "</td></tr>";
        textonly = textonly + c1 + i + ". " + Add_Commas(Thievery_Resource) + " - " + cc + c2 + Add_Commas(Thievery_Optimal) + cc + c1 + " thieves - " + cc + c2 + Add_Commas(Thievery_Gains) + cc + c1 + " (" + Add_Commas(Thievery_Losts) + ")" + cc + "<br />";
        Thievery_Resource = Thievery_Resource - Thievery_Losts;
        Thievery_Optimal = Math.round(Thievery_Resource * Thievery_Max / Thievery_Rate);
        if (Thievery_Optimal > Thievery_YourThieves) Thievery_Optimal = Thievery_YourThieves;
        Thievery_Gains = Math.round(Thievery_Optimal * Thievery_Relative_NW * Thievery_Rate * Thievery_WarBonus * Thievery_Resources_Gains);
        Thievery_Losts = Math.round(Thievery_Optimal * Thievery_Relative_NW * Thievery_Rate * Thievery_WarBonus);

    }

    temp += "</table>";


    document.getElementById('Thievery_Main_Table').innerHTML = temp;

}


function YourThiefReport() {

    document.getElementById('CBtext2').innerHTML = textonly;

    textonly = textonly.replace(/\//g, "");
    textonly = textonly.replace(/http:/g, "http://");
    textonly = textonly.replace(/  /g, " / ");
    textonly = textonly.replace(/<font color='#0088CC'>/g, "");
    textonly = textonly.replace(/<font color='#666666'>/g, "");
    textonly = textonly.replace(/<font color='#cccccc'>/g, "");
    textonly = textonly.replace(/<font color='#cc8888'>/g, "");
    textonly = textonly.replace(/<font>/g, "");
    textonly = textonly.replace(/<br >/g, "bijoforever");

    document.Calculator2.CBtext1.value = textonly;

    copy(document.Calculator2.CBtext1.value);

    document.getElementById('Ultima_floating_window0').style.display = "block";

}



function YourThiefReset() {

    document.getElementById('YourThief').innerHTML = "Thief's Information";
    document.Calculator6.YourThieves.value = 0;
    document.Calculator6.YourNetworth.value = 0;
    document.Calculator6.YourPersonality.value = "Other";

}


function EnemyThiefReset() {

    document.getElementById('EnemyThief').innerHTML = "Defender's Information";
    document.Calculator6.EnemyNetworth.value = 0;
    document.Calculator6.EnemyRelations.value = 0;

}

function ThiefResetAll() {

    YourThiefReset();
    EnemyThiefReset();
    ThiefMoney();

    document.Calculator6.Thievery_Money.value = 0;
    document.Calculator6.Thievery_Food.value = 0;
    document.Calculator6.Thievery_Runes.value = 0;
    document.Calculator6.Thievery_Peasants.value = 0;
    document.Calculator6.Thievery_Wizards.value = 0;
    document.Calculator6.Thievery_Horses.value = 0;
    document.Calculator6.Thievery_Buildings.value = 0;
    document.Calculator6.Thievery_Prisoners.value = 0;
    document.Calculator6.Thievery_Soldiers.value = 0;
    document.Calculator6.Thievery_OffSpecs.value = 0;
    document.Calculator6.Thievery_DefSpecs.value = 0;
    document.Calculator6.Thievery_Elites.value = 0;

}


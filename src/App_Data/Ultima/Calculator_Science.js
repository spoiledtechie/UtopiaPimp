var browser = navigator.appName;


var Sci_X_Alchemy = 0;
var Sci_X_Tools = 0;
var Sci_X_Housing = 0;
var Sci_X_Food = 0;
var Sci_X_Military = 0;
var Sci_X_Crime = 0;
var Sci_X_Channeling = 0;


var Sci_Daily_Income = 0;
var Sci_Total_Money = 0;
var Sci_Research_Cost = 0;
var Science_Calculator_NW = 0;
var Sci_Total_Known = 0;
var Sci_Total_Progress = 0;

var Sci_Prov = "";
var Sci_Kingdom = 0;
var Sci_Island = 0;

var Sci_Race = "Human";
var Sci_Personality = 0;

var Sci_Acres = 0;
var Sci_Learnable = 0;
var Sci_Libraries = 0;
var Sci_Available_Alchemy = 0;
var Sci_Available_Tools = 0;
var Sci_Available_Housing = 0;
var Sci_Available_Food = 0;
var Sci_Available_Military = 0;
var Sci_Available_Crime = 0;
var Sci_Available_Channeling = 0;
var Sci_Progress_Alchemy = 0;
var Sci_Progress_Tools = 0;
var Sci_Progress_Housing = 0;
var Sci_Progress_Food = 0;
var Sci_Progress_Military = 0;
var Sci_Progress_Crime = 0;
var Sci_Progress_Channeling = 0;

var Sci_Bonus_Alchemy = 0;
var Sci_Bonus_Tools = 0;
var Sci_Bonus_Housing = 0;
var Sci_Bonus_Food = 0;
var Sci_Bonus_Military = 0;
var Sci_Bonus_Crime = 0;
var Sci_Bonus_Channeling = 0;

var Sci_Bonus1_Alchemy = 0;
var Sci_Bonus1_Tools = 0;
var Sci_Bonus1_Housing = 0;
var Sci_Bonus1_Food = 0;
var Sci_Bonus1_Military = 0;
var Sci_Bonus1_Crime = 0;
var Sci_Bonus1_Channeling = 0;

var Sci_Bonus2_Alchemy = 0;
var Sci_Bonus2_Tools = 0;
var Sci_Bonus2_Housing = 0;
var Sci_Bonus2_Food = 0;
var Sci_Bonus2_Military = 0;
var Sci_Bonus2_Crime = 0;
var Sci_Bonus2_Channeling = 0;

var Sci_Bonus3_Alchemy = 0;
var Sci_Bonus3_Tools = 0;
var Sci_Bonus3_Housing = 0;
var Sci_Bonus3_Food = 0;
var Sci_Bonus3_Military = 0;
var Sci_Bonus3_Crime = 0;
var Sci_Bonus3_Channeling = 0;

var Sci_Race_Bonus = 0;
var Sci_Personality_Bonus = 0;
var Sci_Libraries_Effects = 1;

var Sci_Points = 0;
var Sci_PPA = 0;
var Sci_Bonuses = 0;
var Sci_Points_Needed = 0;
var Sci_Points_To_Learn = 0;



function Sci_Get_Alchemy(event) {
    Sci_X_Alchemy = 0;
    Sci_X_Tools = 0;
    Sci_X_Housing = 0;
    Sci_X_Food = 0;
    Sci_X_Military = 0;
    Sci_X_Crime = 0;
    Sci_X_Channeling = 0;
    Sci_Points_Needed = "?";
    Sci_Points_To_Learn = "?";
    Sci_X_Alchemy = event.screenX - document.getElementById("Sci_Offset_Alchemy").offsetLeft - document.getElementById('Ultima_floating_window2').offsetLeft;
    if (browser == "Microsoft Internet Explorer") Sci_X_Alchemy = event.screenX - document.getElementById("Sci_Offset_Alchemy").offsetLeft - document.getElementById('Ultima_floating_window2').offsetLeft - 2;
    if (Sci_X_Alchemy < 0) Sci_X_Alchemy = 0;
    if (Sci_X_Alchemy > 100) Sci_X_Alchemy = 100;
    document.getElementById('Sci_Bonuses').innerHTML = Sci_X_Alchemy;

    if (Sci_X_Alchemy > 0) Sci_Points_Needed = Sci_Acres * Math.pow(Sci_X_Alchemy / (Sci_Mult_Alchemy * Sci_Personality_Bonus * Sci_Race_Bonus * Sci_Libraries_Effects), 2);
    if (Sci_X_Alchemy > 0) Sci_Points_To_Learn = Sci_Points_Needed - Sci_Available_Alchemy;
    if (Sci_Points_Needed != "?") document.getElementById('Sci_Points_Needed').innerHTML = Math.round(Sci_Points_Needed);
    if (Sci_Points_To_Learn < 0) Sci_Points_To_Learn = 0;
    if (Sci_Points_To_Learn != "?") document.getElementById('Sci_Points_To_Learn').innerHTML = Math.round(Sci_Points_To_Learn);

}

function Sci_Get_Tools(event) {
    Sci_X_Alchemy = 0;
    Sci_X_Tools = 0;
    Sci_X_Housing = 0;
    Sci_X_Food = 0;
    Sci_X_Military = 0;
    Sci_X_Crime = 0;
    Sci_X_Channeling = 0;
    Sci_Points_Needed = "?";
    Sci_Points_To_Learn = "?";
    Sci_X_Tools = event.screenX - document.getElementById("Sci_Offset_Tools").offsetLeft - document.getElementById('Ultima_floating_window2').offsetLeft;
    if (browser == "Microsoft Internet Explorer") Sci_X_Tools = event.screenX - document.getElementById("Sci_Offset_Tools").offsetLeft - document.getElementById('Ultima_floating_window2').offsetLeft - 2;
    if (Sci_X_Tools < 0) Sci_X_Tools = 0;
    if (Sci_X_Tools > 100) Sci_X_Tools = 100;
    document.getElementById('Sci_Bonuses').innerHTML = Sci_X_Tools;

    if (Sci_X_Tools > 0) Sci_Points_Needed = Sci_Acres * Math.pow(Sci_X_Tools / (Sci_Mult_Tools * Sci_Personality_Bonus * Sci_Race_Bonus * Sci_Libraries_Effects), 2);
    if (Sci_X_Tools > 0) Sci_Points_To_Learn = Sci_Points_Needed - Sci_Available_Tools;
    if (Sci_Points_Needed != "?") document.getElementById('Sci_Points_Needed').innerHTML = Math.round(Sci_Points_Needed);
    if (Sci_Points_To_Learn < 0) Sci_Points_To_Learn = 0;
    if (Sci_Points_To_Learn != "?") document.getElementById('Sci_Points_To_Learn').innerHTML = Math.round(Sci_Points_To_Learn);

}

function Sci_Get_Housing(event) {
    Sci_X_Alchemy = 0;
    Sci_X_Tools = 0;
    Sci_X_Housing = 0;
    Sci_X_Food = 0;
    Sci_X_Military = 0;
    Sci_X_Crime = 0;
    Sci_X_Channeling = 0;
    Sci_Points_Needed = "?";
    Sci_Points_To_Learn = "?";
    Sci_X_Housing = event.screenX - document.getElementById("Sci_Offset_Housing").offsetLeft - document.getElementById('Ultima_floating_window2').offsetLeft;
    if (browser == "Microsoft Internet Explorer") Sci_X_Housing = event.screenX - document.getElementById("Sci_Offset_Housing").offsetLeft - document.getElementById('Ultima_floating_window2').offsetLeft - 2;
    if (Sci_X_Housing < 0) Sci_X_Housing = 0;
    if (Sci_X_Housing > 100) Sci_X_Housing = 100;
    document.getElementById('Sci_Bonuses').innerHTML = Sci_X_Housing;

    if (Sci_X_Housing > 0) Sci_Points_Needed = Sci_Acres * Math.pow(Sci_X_Housing / (Sci_Mult_Housing * Sci_Personality_Bonus * Sci_Race_Bonus * Sci_Libraries_Effects), 2);
    if (Sci_X_Housing > 0) Sci_Points_To_Learn = Sci_Points_Needed - Sci_Available_Housing;
    if (Sci_Points_Needed != "?") document.getElementById('Sci_Points_Needed').innerHTML = Math.round(Sci_Points_Needed);
    if (Sci_Points_To_Learn < 0) Sci_Points_To_Learn = 0;
    if (Sci_Points_To_Learn != "?") document.getElementById('Sci_Points_To_Learn').innerHTML = Math.round(Sci_Points_To_Learn);

}

function Sci_Get_Food(event) {
    Sci_X_Alchemy = 0;
    Sci_X_Tools = 0;
    Sci_X_Housing = 0;
    Sci_X_Food = 0;
    Sci_X_Military = 0;
    Sci_X_Crime = 0;
    Sci_X_Channeling = 0;
    Sci_Points_Needed = "?";
    Sci_Points_To_Learn = "?";
    Sci_X_Food = event.screenX - document.getElementById("Sci_Offset_Food").offsetLeft - document.getElementById('Ultima_floating_window2').offsetLeft;
    if (browser == "Microsoft Internet Explorer") Sci_X_Food = event.screenX - document.getElementById("Sci_Offset_Food").offsetLeft - document.getElementById('Ultima_floating_window2').offsetLeft - 2;
    if (Sci_X_Food < 0) Sci_X_Food = 0;
    if (Sci_X_Food > 100) Sci_X_Food = 100;
    document.getElementById('Sci_Bonuses').innerHTML = Sci_X_Food;

    if (Sci_X_Food > 0) Sci_Points_Needed = Sci_Acres * Math.pow(Sci_X_Food / (Sci_Mult_Food * Sci_Personality_Bonus * Sci_Race_Bonus * Sci_Libraries_Effects), 2);
    if (Sci_X_Food > 0) Sci_Points_To_Learn = Sci_Points_Needed - Sci_Available_Food;
    if (Sci_Points_Needed != "?") document.getElementById('Sci_Points_Needed').innerHTML = Math.round(Sci_Points_Needed);
    if (Sci_Points_To_Learn < 0) Sci_Points_To_Learn = 0;
    if (Sci_Points_To_Learn != "?") document.getElementById('Sci_Points_To_Learn').innerHTML = Math.round(Sci_Points_To_Learn);

}

function Sci_Get_Military(event) {
    Sci_X_Alchemy = 0;
    Sci_X_Tools = 0;
    Sci_X_Housing = 0;
    Sci_X_Food = 0;
    Sci_X_Military = 0;
    Sci_X_Crime = 0;
    Sci_X_Channeling = 0;
    Sci_Points_Needed = "?";
    Sci_Points_To_Learn = "?";
    Sci_X_Military = event.screenX - document.getElementById("Sci_Offset_Military").offsetLeft - document.getElementById('Ultima_floating_window2').offsetLeft;
    if (browser == "Microsoft Internet Explorer") Sci_X_Military = event.screenX - document.getElementById("Sci_Offset_Military").offsetLeft - document.getElementById('Ultima_floating_window2').offsetLeft - 2;
    if (Sci_X_Military < 0) Sci_X_Military = 0;
    if (Sci_X_Military > 100) Sci_X_Military = 100;
    document.getElementById('Sci_Bonuses').innerHTML = Sci_X_Military;

    if (Sci_X_Military > 0) Sci_Points_Needed = Sci_Acres * Math.pow(Sci_X_Military / (Sci_Mult_Military * Sci_Personality_Bonus * Sci_Race_Bonus * Sci_Libraries_Effects), 2);
    if (Sci_X_Military > 0) Sci_Points_To_Learn = Sci_Points_Needed - Sci_Available_Military;
    if (Sci_Points_Needed != "?") document.getElementById('Sci_Points_Needed').innerHTML = Math.round(Sci_Points_Needed);
    if (Sci_Points_To_Learn < 0) Sci_Points_To_Learn = 0;
    if (Sci_Points_To_Learn != "?") document.getElementById('Sci_Points_To_Learn').innerHTML = Math.round(Sci_Points_To_Learn);

}

function Sci_Get_Crime(event) {
    Sci_X_Alchemy = 0;
    Sci_X_Tools = 0;
    Sci_X_Housing = 0;
    Sci_X_Food = 0;
    Sci_X_Military = 0;
    Sci_X_Crime = 0;
    Sci_X_Channeling = 0;
    Sci_Points_Needed = "?";
    Sci_Points_To_Learn = "?";
    Sci_X_Crime = event.screenX - document.getElementById("Sci_Offset_Crime").offsetLeft - document.getElementById('Ultima_floating_window2').offsetLeft;
    if (browser == "Microsoft Internet Explorer") Sci_X_Crime = event.screenX - document.getElementById("Sci_Offset_Crime").offsetLeft - document.getElementById('Ultima_floating_window2').offsetLeft - 2;
    if (Sci_X_Crime < 0) Sci_X_Crime = 0;
    if (Sci_X_Crime > 100) Sci_X_Crime = 100;
    document.getElementById('Sci_Bonuses').innerHTML = Sci_X_Crime;

    if (Sci_X_Crime > 0) Sci_Points_Needed = Sci_Acres * Math.pow(Sci_X_Crime / (Sci_Mult_Crime * Sci_Personality_Bonus * Sci_Race_Bonus * Sci_Libraries_Effects), 2);
    if (Sci_X_Crime > 0) Sci_Points_To_Learn = Sci_Points_Needed - Sci_Available_Crime;
    if (Sci_Points_Needed != "?") document.getElementById('Sci_Points_Needed').innerHTML = Math.round(Sci_Points_Needed);
    if (Sci_Points_To_Learn < 0) Sci_Points_To_Learn = 0;
    if (Sci_Points_To_Learn != "?") document.getElementById('Sci_Points_To_Learn').innerHTML = Math.round(Sci_Points_To_Learn);

}

function Sci_Get_Channeling(event) {
    Sci_X_Alchemy = 0;
    Sci_X_Tools = 0;
    Sci_X_Housing = 0;
    Sci_X_Food = 0;
    Sci_X_Military = 0;
    Sci_X_Crime = 0;
    Sci_X_Channeling = 0;
    Sci_Points_Needed = "?";
    Sci_Points_To_Learn = "?";
    Sci_X_Channeling = event.screenX - document.getElementById("Sci_Offset_Channeling").offsetLeft - document.getElementById('Ultima_floating_window2').offsetLeft;
    if (browser == "Microsoft Internet Explorer") Sci_X_Channeling = event.screenX - document.getElementById("Sci_Offset_Channeling").offsetLeft - document.getElementById('Ultima_floating_window2').offsetLeft - 2;
    if (Sci_X_Channeling < 0) Sci_X_Channeling = 0;
    if (Sci_X_Channeling > 100) Sci_X_Channeling = 100;
    document.getElementById('Sci_Bonuses').innerHTML = Sci_X_Channeling;

    if (Sci_X_Channeling > 0) Sci_Points_Needed = Sci_Acres * Math.pow(Sci_X_Channeling / (Sci_Mult_Channeling * Sci_Personality_Bonus * Sci_Race_Bonus * Sci_Libraries_Effects), 2);
    if (Sci_X_Channeling > 0) Sci_Points_To_Learn = Sci_Points_Needed - Sci_Available_Channeling;
    if (Sci_Points_Needed != "?") document.getElementById('Sci_Points_Needed').innerHTML = Math.round(Sci_Points_Needed);
    if (Sci_Points_To_Learn < 0) Sci_Points_To_Learn = 0;
    if (Sci_Points_To_Learn != "?") document.getElementById('Sci_Points_To_Learn').innerHTML = Math.round(Sci_Points_To_Learn);

}


function Task_Science() {


    Sci_Race = "Human";
    Sci_Personality = 0;
    Sci_Acres = 0;
    Sci_Learnable = 0;
    Sci_Libraries = 0;
    Sci_Available_Alchemy = 0;
    Sci_Available_Tools = 0;
    Sci_Available_Housing = 0;
    Sci_Available_Food = 0;
    Sci_Available_Military = 0;
    Sci_Available_Crime = 0;
    Sci_Available_Channeling = 0;
    Sci_Progress_Alchemy = 0;
    Sci_Progress_Tools = 0;
    Sci_Progress_Housing = 0;
    Sci_Progress_Food = 0;
    Sci_Progress_Military = 0;
    Sci_Progress_Crime = 0;
    Sci_Progress_Channeling = 0;

    Sci_Bonus_Alchemy = 0;
    Sci_Bonus_Tools = 0;
    Sci_Bonus_Housing = 0;
    Sci_Bonus_Food = 0;
    Sci_Bonus_Military = 0;
    Sci_Bonus_Crime = 0;
    Sci_Bonus_Channeling = 0;

    Sci_Bonus1_Alchemy = 0;
    Sci_Bonus1_Tools = 0;
    Sci_Bonus1_Housing = 0;
    Sci_Bonus1_Food = 0;
    Sci_Bonus1_Military = 0;
    Sci_Bonus1_Crime = 0;
    Sci_Bonus1_Channeling = 0;

    Sci_Bonus2_Alchemy = 0;
    Sci_Bonus2_Tools = 0;
    Sci_Bonus2_Housing = 0;
    Sci_Bonus2_Food = 0;
    Sci_Bonus2_Military = 0;
    Sci_Bonus2_Crime = 0;
    Sci_Bonus2_Channeling = 0;

    Sci_Bonus3_Alchemy = 0;
    Sci_Bonus3_Tools = 0;
    Sci_Bonus3_Housing = 0;
    Sci_Bonus3_Food = 0;
    Sci_Bonus3_Military = 0;
    Sci_Bonus3_Crime = 0;
    Sci_Bonus3_Channeling = 0;

    Sci_Race_Bonus = 0;
    Sci_Personality_Bonus = 0;
    Sci_Libraries_Effects = 1;

    Sci_Points = 0;
    Sci_PPA = 0;
    Sci_Bonuses = 0;

    Sci_Race = document.Calculator3.Sci_Race.value;
    Sci_Personality = document.Calculator3.Sci_Personality.value;
    Sci_Acres = document.Calculator3.Sci_Acres.value;
    Sci_Learnable = document.Calculator3.Sci_Avail.value;
    Sci_Libraries = document.Calculator3.Sci_Libraries.value;
    Sci_Available_Alchemy = document.Calculator3.Sci_Available_Alchemy.value;
    Sci_Available_Tools = document.Calculator3.Sci_Available_Tools.value;
    Sci_Available_Housing = document.Calculator3.Sci_Available_Housing.value;
    Sci_Available_Food = document.Calculator3.Sci_Available_Food.value;
    Sci_Available_Military = document.Calculator3.Sci_Available_Military.value;
    Sci_Available_Crime = document.Calculator3.Sci_Available_Crime.value;
    Sci_Available_Channeling = document.Calculator3.Sci_Available_Channeling.value;
    Sci_Progress_Alchemy = document.Calculator3.Sci_Progress_Alchemy.value;
    Sci_Progress_Tools = document.Calculator3.Sci_Progress_Tools.value;
    Sci_Progress_Housing = document.Calculator3.Sci_Progress_Housing.value;
    Sci_Progress_Food = document.Calculator3.Sci_Progress_Food.value;
    Sci_Progress_Military = document.Calculator3.Sci_Progress_Military.value;
    Sci_Progress_Crime = document.Calculator3.Sci_Progress_Crime.value;
    Sci_Progress_Channeling = document.Calculator3.Sci_Progress_Channeling.value;

    Sci_Acres = Number(Sci_Acres);
    Sci_Learnable = Number(Sci_Learnable);
    Sci_Libraries = Number(Sci_Libraries);

    Sci_Available_Alchemy = Number(Sci_Available_Alchemy);
    Sci_Available_Tools = Number(Sci_Available_Tools);
    Sci_Available_Housing = Number(Sci_Available_Housing);
    Sci_Available_Food = Number(Sci_Available_Food);
    Sci_Available_Military = Number(Sci_Available_Military);
    Sci_Available_Crime = Number(Sci_Available_Crime);
    Sci_Available_Channeling = Number(Sci_Available_Channeling);

    Sci_Progress_Alchemy = Number(Sci_Progress_Alchemy);
    Sci_Progress_Tools = Number(Sci_Progress_Tools);
    Sci_Progress_Housing = Number(Sci_Progress_Housing);
    Sci_Progress_Food = Number(Sci_Progress_Food);
    Sci_Progress_Military = Number(Sci_Progress_Military);
    Sci_Progress_Crime = Number(Sci_Progress_Crime);
    Sci_Progress_Channeling = Number(Sci_Progress_Channeling);

    document.getElementById('Sci_Acres_Note').src = "Ultima_None.gif";

    if (Sci_Acres < 300) { document.getElementById('Sci_Acres_Note').src = "Ultima_Note.gif"; Sci_Acres = 300; }

    if (Sci_Libraries < 0) Sci_Libraries = 0;
    if (Sci_Libraries > 50) Sci_Libraries = 50;
    document.Calculator3.Sci_Libraries.value = Sci_Libraries;

    // Libraries Eff = x*(2-0.02*x)
    Sci_Libraries_Effects = 1 + Sci_Libraries * (2 - 0.02 * Sci_Libraries) / 100;


    if (Sci_Learnable < 0) Sci_Learnable = 0;

    Sci_Points = Sci_Available_Alchemy + Sci_Available_Tools + Sci_Available_Housing + Sci_Available_Food + Sci_Available_Military + Sci_Available_Crime + Sci_Available_Channeling;

    Science_Calculator_NW = Math.round(Math.floor(Sci_Points / 100) / 0.92);
    Sci_Total_Known = Math.round(Sci_Points);
    Sci_Total_Progress = Math.round(Sci_Progress_Alchemy + Sci_Progress_Tools + Sci_Progress_Housing + Sci_Progress_Food + Sci_Progress_Military + Sci_Progress_Crime + Sci_Progress_Channeling);

    if (Sci_Race == "Human") Sci_Race_Bonus = 1;
    if (Sci_Race == "Elf") Sci_Race_Bonus = 1;
    if (Sci_Race == "Dwarf") Sci_Race_Bonus = 1;
    if (Sci_Race == "Orc") Sci_Race_Bonus = 1;
    if (Sci_Race == "Gnome") Sci_Race_Bonus = 1;
    if (Sci_Race == "Dark Elf") Sci_Race_Bonus = 1;
    if (Sci_Race == "Undead") Sci_Race_Bonus = 0.7;
    if (Sci_Race == "Avian") Sci_Race_Bonus = 1;
    if (Sci_Race == "Faery") Sci_Race_Bonus = 1;
    if (Sci_Race == "Halfling") Sci_Race_Bonus = 1;


    if (Sci_Personality == 0) Sci_Personality_Bonus = 1;
    if (Sci_Personality == 1) Sci_Personality_Bonus = 1;
    if (Sci_Personality == 2) Sci_Personality_Bonus = 1;
    if (Sci_Personality == 3) Sci_Personality_Bonus = 1;
    if (Sci_Personality == 4) Sci_Personality_Bonus = 1;
    if (Sci_Personality == 5) Sci_Personality_Bonus = 1;
    if (Sci_Personality == 6) Sci_Personality_Bonus = 1.3;
    if (Sci_Personality == 7) Sci_Personality_Bonus = 1;
    if (Sci_Personality == 8) Sci_Personality_Bonus = 1;
    if (Sci_Personality == 9) Sci_Personality_Bonus = 1;


    Sci_Bonuses = Math.round((Sci_Personality_Bonus * Sci_Race_Bonus * Sci_Libraries_Effects - 1) * 100);

    if (Sci_Acres >= 300) {



        Sci_PPA = Math.round(Sci_Points / Sci_Acres);

        Sci_Bonus1_Alchemy = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Alchemy * Math.sqrt(Sci_Available_Alchemy / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;
        Sci_Bonus1_Tools = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Tools * Math.sqrt(Sci_Available_Tools / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;
        Sci_Bonus1_Housing = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Housing * Math.sqrt(Sci_Available_Housing / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;
        Sci_Bonus1_Food = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Food * Math.sqrt(Sci_Available_Food / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;
        Sci_Bonus1_Military = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Military * Math.sqrt(Sci_Available_Military / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;
        Sci_Bonus1_Crime = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Crime * Math.sqrt(Sci_Available_Crime / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;
        Sci_Bonus1_Channeling = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Channeling * Math.sqrt(Sci_Available_Channeling / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;

        Sci_Bonus_Alchemy = Sci_Bonus1_Alchemy;
        Sci_Bonus_Tools = Sci_Bonus1_Tools;
        Sci_Bonus_Housing = Sci_Bonus1_Housing;
        Sci_Bonus_Food = Sci_Bonus1_Food;
        Sci_Bonus_Military = Sci_Bonus1_Military;
        Sci_Bonus_Crime = Sci_Bonus1_Crime;
        Sci_Bonus_Channeling = Sci_Bonus1_Channeling;

        if (Sci_Bonus1_Alchemy > 100) Sci_Bonus1_Alchemy = 100;
        if (Sci_Bonus1_Tools > 100) Sci_Bonus1_Tools = 100;
        if (Sci_Bonus1_Housing > 100) Sci_Bonus1_Housing = 100;
        if (Sci_Bonus1_Food > 100) Sci_Bonus1_Food = 100;
        if (Sci_Bonus1_Military > 100) Sci_Bonus1_Military = 100;
        if (Sci_Bonus1_Crime > 100) Sci_Bonus1_Crime = 100;
        if (Sci_Bonus1_Channeling > 100) Sci_Bonus1_Channeling = 100;

        Sci_Bonus2_Alchemy = Sci_Available_Alchemy + Sci_Progress_Alchemy;
        Sci_Bonus2_Tools = Sci_Available_Tools + Sci_Progress_Tools;
        Sci_Bonus2_Housing = Sci_Available_Housing + Sci_Progress_Housing;
        Sci_Bonus2_Food = Sci_Available_Food + Sci_Progress_Food;
        Sci_Bonus2_Military = Sci_Available_Military + Sci_Progress_Military;
        Sci_Bonus2_Crime = Sci_Available_Crime + Sci_Progress_Crime;
        Sci_Bonus2_Channeling = Sci_Available_Channeling + Sci_Progress_Channeling;

        Sci_Bonus2_Alchemy = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Alchemy * Math.sqrt(Sci_Bonus2_Alchemy / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;
        Sci_Bonus2_Tools = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Tools * Math.sqrt(Sci_Bonus2_Tools / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;
        Sci_Bonus2_Housing = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Housing * Math.sqrt(Sci_Bonus2_Housing / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;
        Sci_Bonus2_Food = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Food * Math.sqrt(Sci_Bonus2_Food / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;
        Sci_Bonus2_Military = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Military * Math.sqrt(Sci_Bonus2_Military / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;
        Sci_Bonus2_Crime = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Crime * Math.sqrt(Sci_Bonus2_Crime / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;
        Sci_Bonus2_Channeling = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Channeling * Math.sqrt(Sci_Bonus2_Channeling / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;

        if (Sci_Bonus2_Alchemy > 100) Sci_Bonus2_Alchemy = 100;
        if (Sci_Bonus2_Tools > 100) Sci_Bonus2_Tools = 100;
        if (Sci_Bonus2_Housing > 100) Sci_Bonus2_Housing = 100;
        if (Sci_Bonus2_Food > 100) Sci_Bonus2_Food = 100;
        if (Sci_Bonus2_Military > 100) Sci_Bonus2_Military = 100;
        if (Sci_Bonus2_Crime > 100) Sci_Bonus2_Crime = 100;
        if (Sci_Bonus2_Channeling > 100) Sci_Bonus2_Channeling = 100;

        Sci_Bonus2_Alchemy = Sci_Bonus2_Alchemy - Sci_Bonus1_Alchemy;
        Sci_Bonus2_Tools = Sci_Bonus2_Tools - Sci_Bonus1_Tools;
        Sci_Bonus2_Housing = Sci_Bonus2_Housing - Sci_Bonus1_Housing;
        Sci_Bonus2_Food = Sci_Bonus2_Food - Sci_Bonus1_Food;
        Sci_Bonus2_Military = Sci_Bonus2_Military - Sci_Bonus1_Military;
        Sci_Bonus2_Crime = Sci_Bonus2_Crime - Sci_Bonus1_Crime;
        Sci_Bonus2_Channeling = Sci_Bonus2_Channeling - Sci_Bonus1_Channeling;

        if (Sci_Bonus2_Alchemy < 0) Sci_Bonus2_Alchemy = 0;
        if (Sci_Bonus2_Tools < 0) Sci_Bonus2_Tools = 0;
        if (Sci_Bonus2_Housing < 0) Sci_Bonus2_Housing = 0;
        if (Sci_Bonus2_Food < 0) Sci_Bonus2_Food = 0;
        if (Sci_Bonus2_Military < 0) Sci_Bonus2_Military = 0;
        if (Sci_Bonus2_Crime < 0) Sci_Bonus2_Crime = 0;
        if (Sci_Bonus2_Channeling < 0) Sci_Bonus2_Channeling = 0;

        Sci_Bonus3_Alchemy = Sci_Available_Alchemy + Sci_Progress_Alchemy + Sci_Learnable;
        Sci_Bonus3_Tools = Sci_Available_Tools + Sci_Progress_Tools + Sci_Learnable;
        Sci_Bonus3_Housing = Sci_Available_Housing + Sci_Progress_Housing + Sci_Learnable;
        Sci_Bonus3_Food = Sci_Available_Food + Sci_Progress_Food + Sci_Learnable;
        Sci_Bonus3_Military = Sci_Available_Military + Sci_Progress_Military + Sci_Learnable;
        Sci_Bonus3_Crime = Sci_Available_Crime + Sci_Progress_Crime + Sci_Learnable;
        Sci_Bonus3_Channeling = Sci_Available_Channeling + Sci_Progress_Channeling + Sci_Learnable;

        Sci_Bonus3_Alchemy = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Alchemy * Math.sqrt(Sci_Bonus3_Alchemy / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;
        Sci_Bonus3_Tools = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Tools * Math.sqrt(Sci_Bonus3_Tools / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;
        Sci_Bonus3_Housing = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Housing * Math.sqrt(Sci_Bonus3_Housing / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;
        Sci_Bonus3_Food = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Food * Math.sqrt(Sci_Bonus3_Food / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;
        Sci_Bonus3_Military = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Military * Math.sqrt(Sci_Bonus3_Military / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;
        Sci_Bonus3_Crime = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Crime * Math.sqrt(Sci_Bonus3_Crime / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;
        Sci_Bonus3_Channeling = Math.round(Sci_Race_Bonus * Sci_Personality_Bonus * Sci_Mult_Channeling * Math.sqrt(Sci_Bonus3_Channeling / Sci_Acres) * Sci_Libraries_Effects * 10) / 10;

        if (Sci_Bonus3_Alchemy > 100) Sci_Bonus3_Alchemy = 100;
        if (Sci_Bonus3_Tools > 100) Sci_Bonus3_Tools = 100;
        if (Sci_Bonus3_Housing > 100) Sci_Bonus3_Housing = 100;
        if (Sci_Bonus3_Food > 100) Sci_Bonus3_Food = 100;
        if (Sci_Bonus3_Military > 100) Sci_Bonus3_Military = 100;
        if (Sci_Bonus3_Crime > 100) Sci_Bonus3_Crime = 100;
        if (Sci_Bonus3_Channeling > 100) Sci_Bonus3_Channeling = 100;

        Sci_Bonus3_Alchemy = Sci_Bonus3_Alchemy - Sci_Bonus2_Alchemy - Sci_Bonus1_Alchemy;
        Sci_Bonus3_Tools = Sci_Bonus3_Tools - Sci_Bonus2_Tools - Sci_Bonus1_Tools;
        Sci_Bonus3_Housing = Sci_Bonus3_Housing - Sci_Bonus2_Housing - Sci_Bonus1_Housing;
        Sci_Bonus3_Food = Sci_Bonus3_Food - Sci_Bonus2_Food - Sci_Bonus1_Food;
        Sci_Bonus3_Military = Sci_Bonus3_Military - Sci_Bonus2_Military - Sci_Bonus1_Military;
        Sci_Bonus3_Crime = Sci_Bonus3_Crime - Sci_Bonus2_Crime - Sci_Bonus1_Crime;
        Sci_Bonus3_Channeling = Sci_Bonus3_Channeling - Sci_Bonus2_Channeling - Sci_Bonus1_Channeling;

        if (Sci_Bonus3_Alchemy < 0) Sci_Bonus3_Alchemy = 0;
        if (Sci_Bonus3_Tools < 0) Sci_Bonus3_Tools = 0;
        if (Sci_Bonus3_Housing < 0) Sci_Bonus3_Housing = 0;
        if (Sci_Bonus3_Food < 0) Sci_Bonus3_Food = 0;
        if (Sci_Bonus3_Military < 0) Sci_Bonus3_Military = 0;
        if (Sci_Bonus3_Crime < 0) Sci_Bonus3_Crime = 0;
        if (Sci_Bonus3_Channeling < 0) Sci_Bonus3_Channeling = 0;

    }

    document.getElementById('Sci_Alchemy').innerHTML = Sci_Bonus_Alchemy + "%";
    document.getElementById('Sci_Tools').innerHTML = Sci_Bonus_Tools + "%";
    document.getElementById('Sci_Housing').innerHTML = Sci_Bonus_Housing + "%";
    document.getElementById('Sci_Food').innerHTML = Sci_Bonus_Food + "%";
    document.getElementById('Sci_Military').innerHTML = Sci_Bonus_Military + "%";
    document.getElementById('Sci_Crime').innerHTML = Sci_Bonus_Crime + "%";
    document.getElementById('Sci_Channeling').innerHTML = Sci_Bonus_Channeling + "%";

    document.getElementById('Sci_Available_Alchemy_Bar').width = Sci_Bonus1_Alchemy;
    document.getElementById('Sci_Available_Tools_Bar').width = Sci_Bonus1_Tools;
    document.getElementById('Sci_Available_Housing_Bar').width = Sci_Bonus1_Housing;
    document.getElementById('Sci_Available_Food_Bar').width = Sci_Bonus1_Food;
    document.getElementById('Sci_Available_Military_Bar').width = Sci_Bonus1_Military;
    document.getElementById('Sci_Available_Crime_Bar').width = Sci_Bonus1_Crime;
    document.getElementById('Sci_Available_Channeling_Bar').width = Sci_Bonus1_Channeling;

    document.getElementById('Sci_Progress_Alchemy_Bar').width = Sci_Bonus2_Alchemy;
    document.getElementById('Sci_Progress_Tools_Bar').width = Sci_Bonus2_Tools;
    document.getElementById('Sci_Progress_Housing_Bar').width = Sci_Bonus2_Housing;
    document.getElementById('Sci_Progress_Food_Bar').width = Sci_Bonus2_Food;
    document.getElementById('Sci_Progress_Military_Bar').width = Sci_Bonus2_Military;
    document.getElementById('Sci_Progress_Crime_Bar').width = Sci_Bonus2_Crime;
    document.getElementById('Sci_Progress_Channeling_Bar').width = Sci_Bonus2_Channeling;

    document.getElementById('Sci_Learnable_Alchemy_Bar').width = Sci_Bonus3_Alchemy;
    document.getElementById('Sci_Learnable_Tools_Bar').width = Sci_Bonus3_Tools;
    document.getElementById('Sci_Learnable_Housing_Bar').width = Sci_Bonus3_Housing;
    document.getElementById('Sci_Learnable_Food_Bar').width = Sci_Bonus3_Food;
    document.getElementById('Sci_Learnable_Military_Bar').width = Sci_Bonus3_Military;
    document.getElementById('Sci_Learnable_Crime_Bar').width = Sci_Bonus3_Crime;
    document.getElementById('Sci_Learnable_Channeling_Bar').width = Sci_Bonus3_Channeling;

    document.getElementById('Sci_PPA').innerHTML = Sci_PPA;
    document.getElementById('Sci_Total_Bonuses').innerHTML = Sci_Bonuses;
    document.getElementById('Sci_Daily_Income').innerHTML = Sci_Daily_Income;
    document.getElementById('Sci_Total_Money').innerHTML = Sci_Total_Money;
    document.getElementById('Sci_Research_Cost').innerHTML = Sci_Research_Cost;
    document.getElementById('Science_Calculator_NW').innerHTML = Science_Calculator_NW;
    document.getElementById('Sci_Total_Known').innerHTML = Sci_Total_Known;
    document.getElementById('Sci_Total_Progress').innerHTML = Sci_Total_Progress;

    if (document.Calculator3.Sci_War.checked == true) {

        document.getElementById('Sci_Marginal_Focused').style.fontStyle = "italic";
        document.getElementById('Sci_Marginal_Accelerated').style.fontStyle = "italic";
        document.getElementById('Sci_Marginal_Intensive').style.fontStyle = "italic";
        document.getElementById('Sci_Marginal_Rushed').style.fontStyle = "italic";
        document.getElementById('Sci_Marginal_Extreme').style.fontStyle = "italic";

        document.getElementById('Sci_Marginal_Focused').style.textDecoration = "line-through";
        document.getElementById('Sci_Marginal_Accelerated').style.textDecoration = "line-through";
        document.getElementById('Sci_Marginal_Intensive').style.textDecoration = "line-through";
        document.getElementById('Sci_Marginal_Rushed').style.textDecoration = "line-through";
        document.getElementById('Sci_Marginal_Extreme').style.textDecoration = "line-through";


    } else {

        document.getElementById('Sci_Marginal_Focused').style.fontStyle = "normal";
        document.getElementById('Sci_Marginal_Accelerated').style.fontStyle = "normal";
        document.getElementById('Sci_Marginal_Intensive').style.fontStyle = "normal";
        document.getElementById('Sci_Marginal_Rushed').style.fontStyle = "normal";
        document.getElementById('Sci_Marginal_Extreme').style.fontStyle = "normal";

        document.getElementById('Sci_Marginal_Focused').style.textDecoration = "none";
        document.getElementById('Sci_Marginal_Accelerated').style.textDecoration = "none";
        document.getElementById('Sci_Marginal_Intensive').style.textDecoration = "none";
        document.getElementById('Sci_Marginal_Rushed').style.textDecoration = "none";
        document.getElementById('Sci_Marginal_Extreme').style.textDecoration = "none";

    }





}

document.getElementById('scibox1').style.display = "block";
document.getElementById('scibox2').style.display = "none";
document.getElementById('scitab1').style.borderTop = "0px solid transparent";
document.getElementById('scitab2').style.borderTop = "1px solid #ACA899";

function scibox1() {
    document.getElementById('scibox1').style.display = "block";
    document.getElementById('scibox2').style.display = "none";
    document.getElementById('scitab1').style.borderTop = "0px solid transparent";
    document.getElementById('scitab2').style.borderTop = "1px solid #ACA899";
}

function scibox2() {
    document.getElementById('scibox1').style.display = "none";
    document.getElementById('scibox2').style.display = "block";
    document.getElementById('scitab2').style.borderTop = "0px solid transparent";
    document.getElementById('scitab1').style.borderTop = "1px solid #ACA899";
}

function sciReport() {

    textonly = "" + c0 + "Science Intelligence Formatted Report" + cc + Copyrights +
"<br />" + c1 + "Land: " + cc + c2 + Sci_Acres + cc + c1 + " acres" + cc +
"<br />" + c1 + "Daily Income: " + cc + c2 + Sci_Daily_Income + cc + c1 + "gc" + cc +
"<br />" + c1 + "Total Money: " + cc + c2 + Sci_Total_Money + cc + c1 + "gc" + cc +
"<br />" + c1 + "PPA: " + cc + c2 + Sci_PPA + cc + c1 + " pts" + cc +
"<br />" + c1 + "Total Bonuses: " + cc + c2 + Sci_Bonuses + cc + c1 + "%" + cc +
"<br />" + c1 + "Estimated Research Cost: " + cc + c2 + Sci_Research_Cost + cc + c1 + "gc" + cc +
"<br />" + c1 + "Science NW: " + cc + c2 + Science_Calculator_NW + cc + c1 + "gc" + cc +
"<br />" + c1 + "Total Known: " + cc + c2 + Sci_Total_Known + cc + c1 + " pts" + cc +
"<br />" + c1 + "Total In Progress: " + cc + c2 + Sci_Total_Progress + cc + c1 + " pts" + cc +
"<br />" + c1 + "Books to Allocate: " + cc + c2 + Sci_Learnable + cc + c1 + " books" + cc;

    if ((Sci_Available_Alchemy != 0) || (Sci_Available_Tools != 0) || (Sci_Available_Housing != 0) || (Sci_Available_Food != 0) || (Sci_Available_Military != 0) || (Sci_Available_Crime != 0) || (Sci_Available_Channeling != 0)) textonly += "<br /><br />" + c0 + "** Effects Summary (Known Science Only) **" + cc;

    if (Sci_Available_Alchemy != 0) textonly += "<br />" + c2 + Sci_Bonus_Alchemy + "%" + cc + c1 + " Income (" + cc + c2 + Sci_Available_Alchemy + cc + c1 + " points)" + cc;
    if (Sci_Available_Tools != 0) textonly += "<br />" + c2 + Sci_Bonus_Tools + "%" + cc + c1 + " Building Effectiveness (" + cc + c2 + Sci_Available_Tools + cc + c1 + " points)" + cc;
    if (Sci_Available_Housing != 0) textonly += "<br />" + c2 + Sci_Bonus_Housing + "%" + cc + c1 + " Population Limits (" + cc + c2 + Sci_Available_Housing + cc + c1 + " points)" + cc;
    if (Sci_Available_Food != 0) textonly += "<br />" + c2 + Sci_Bonus_Food + "%" + cc + c1 + " Food Production (" + cc + c2 + Sci_Available_Food + cc + c1 + " points)" + cc;
    if (Sci_Available_Military != 0) textonly += "<br />" + c2 + Sci_Bonus_Military + "%" + cc + c1 + " Gains in Combat (" + cc + c2 + Sci_Available_Military + cc + c1 + " points)" + cc;
    if (Sci_Available_Crime != 0) textonly += "<br />" + c2 + Sci_Bonus_Crime + "%" + cc + c1 + " Thievery Effectiveness (" + cc + c2 + Sci_Available_Crime + cc + c1 + " points)" + cc;
    if (Sci_Available_Channeling != 0) textonly += "<br />" + c2 + Sci_Bonus_Channeling + "%" + cc + c1 + " Magic Effectiveness & Rune Production (" + cc + c2 + Sci_Available_Channeling + cc + c1 + " points)" + cc;

    if ((Sci_Progress_Alchemy != 0) || (Sci_Progress_Tools != 0) || (Sci_Progress_Housing != 0) || (Sci_Progress_Food != 0) || (Sci_Progress_Military != 0) || (Sci_Progress_Crime != 0) || (Sci_Progress_Channeling != 0)) textonly += "<br /><br />" + c0 + "** Science Points in Progress **" + cc;

    if (Sci_Progress_Alchemy != 0) textonly += "<br />" + c1 + "Alchemy: " + cc + c2 + Sci_Progress_Alchemy + cc + c1 + " points in progress" + cc;
    if (Sci_Progress_Tools != 0) textonly += "<br />" + c1 + "Tools: " + cc + c2 + Sci_Progress_Tools + cc + c1 + " points in progress" + cc;
    if (Sci_Progress_Housing != 0) textonly += "<br />" + c1 + "Housing: " + cc + c2 + Sci_Progress_Housing + cc + c1 + " points in progress" + cc;
    if (Sci_Progress_Food != 0) textonly += "<br />" + c1 + "Food: " + cc + c2 + Sci_Progress_Food + cc + c1 + " points in progress" + cc;
    if (Sci_Progress_Military != 0) textonly += "<br />" + c1 + "Military: " + cc + c2 + Sci_Progress_Military + cc + c1 + " points in progress" + cc;
    if (Sci_Progress_Crime != 0) textonly += "<br />" + c1 + "Crime: " + cc + c2 + Sci_Progress_Crime + cc + c1 + " points in progress" + cc;
    if (Sci_Progress_Channeling != 0) textonly += "<br />" + c1 + "Channeling: " + cc + c2 + Sci_Progress_Channeling + cc + c1 + " points in progress" + cc;

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
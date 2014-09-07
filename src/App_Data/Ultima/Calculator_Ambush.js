function Task_Ambush() {

    var Ambush_Your_Race = "";
    var Ambush_Your_Soldiers = 0;
    var Ambush_Your_OffSpecs = 0;
    var Ambush_Your_Elites = 0;
    var Ambush_Your_Horses = 0;
    var Ambush_Your_Merc_Pris = 0;
    var Ambush_Your_Offense = 0;

    var Ambush_Enemy_Race = "";
    var Ambush_Enemy_Soldiers = 0;
    var Ambush_Enemy_OffSpecs = 0;
    var Ambush_Enemy_Elites = 0;
    var Ambush_Enemy_Defense = 0;

    var Ambush_Your_Race = document.Calculator7.Ambush_Your_Race.value;
    var Ambush_Your_Soldiers = Number(document.Calculator7.Ambush_Your_Soldiers.value);
    var Ambush_Your_OffSpecs = Number(document.Calculator7.Ambush_Your_OffSpecs.value);
    var Ambush_Your_Elites = Number(document.Calculator7.Ambush_Your_Elites.value);
    var Ambush_Your_Horses = Number(document.Calculator7.Ambush_Your_Horses.value);
    var Ambush_Your_Merc_Pris = Number(document.Calculator7.Ambush_Your_Merc_Pris.value);

    var Ambush_Enemy_Race = document.Calculator7.Ambush_Enemy_Race.value;
    var Ambush_Enemy_Soldiers = Number(document.Calculator7.Ambush_Enemy_Soldiers.value);
    var Ambush_Enemy_OffSpecs = Number(document.Calculator7.Ambush_Enemy_OffSpecs.value);
    var Ambush_Enemy_Elites = Number(document.Calculator7.Ambush_Enemy_Elites.value);

    for (i = 0; i < 10; i++) if (Race_Name[i] == Ambush_Your_Race) Ambush_Your_Race = i;
    for (i = 0; i < 10; i++) if (Race_Name[i] == Ambush_Enemy_Race) Ambush_Enemy_Race = i;

    Ambush_Your_Offense = Ambush_Your_Soldiers + Offensive_Specialist_Strength[Ambush_Your_Race] * Ambush_Your_OffSpecs + Offensive_Elite_Unit_Strength[Ambush_Your_Race] * Ambush_Your_Elites + Ambush_Your_Horses + Ambush_Your_Merc_Pris * 3;
    Ambush_Enemy_Defense = Ambush_Enemy_Soldiers + Offensive_Specialist_Strength[Ambush_Enemy_Race] * Ambush_Enemy_OffSpecs + Defensive_Elite_Unit_Strength[Ambush_Enemy_Race] * Ambush_Enemy_Elites;

    Ambush_Enemy_Defense = Math.round(Ambush_Enemy_Defense * 0.8);

    document.getElementById('Ambush_Your_Offense').innerHTML = Ambush_Your_Offense;
    document.getElementById('Ambush_Enemy_Defense').innerHTML = Ambush_Enemy_Defense;

}
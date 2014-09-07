function Clipboard_Buildings() {

    if ((content.indexOf("Building Effects") != -1) || (EL == 3)) {

        if (document.getElementById('Ultima_Popups').checked == false) document.getElementById('Ultima_floating_window0').style.display = "block";

        if ((content.indexOf("Our thieves scour the lands") == -1) && (EL == 0)) {

            if (document.getElementById('Ultima_Popups').checked == false) document.getElementById('Ultima_floating_window3').style.display = "block";



            content = content.slice(content.indexOf("Worth/Acre"));
            content = content.slice(content.indexOf(eol) + 1);

            Building_Total_Money = Number(content.slice(0, content.indexOf(" ")));


            content = content.slice(content.indexOf(" ") + 1); Building_Available_Total = content.slice(0, content.indexOf(" "));
            content = content.slice(content.indexOf(" ") + 1); Building_Available_Total = content.slice(0, content.indexOf(" "));
            content = content.slice(content.indexOf(" ") + 1); Building_Available_Total = content.slice(0, content.indexOf(" "));
            content = content.slice(content.indexOf(" ") + 1); Building_Available_Total = content.slice(0, content.indexOf(" "));
            content = content.slice(content.indexOf(" ") + 1); Building_Available_Total = Number(content.slice(0, content.indexOf(" ")));

            //content = content.slice(content.indexOf("Internal Affairs")); content = content.slice(content.indexOf(eol)+1);
            Building_Prov = "Self Province"//content.slice(1,content.indexOf("I am in charge")-1);

            content = content.slice(content.indexOf("Available Workers"));
            content = content.slice(content.indexOf(" ") + 1);
            content = content.slice(content.indexOf(" ") + 1);
            Building_Available_Workers = content.slice(0, content.indexOf(eol));
            Building_Available_Workers = Number(Building_Available_Workers);

            content = content.slice(content.indexOf("Building Efficiency"));
            content = content.slice(content.indexOf(" ") + 1);
            content = content.slice(content.indexOf(" ") + 1);
            Building_Effectiveness = content.slice(0, content.indexOf("%"));
            Building_Effectiveness = Number(Building_Effectiveness);

            content = content.slice(content.indexOf("Building Effects"));
            content = content.slice(content.indexOf("Barren Land"));

            if (content.indexOf("Barren Land") != -1) { content = content.slice("Barren Land"); Building_Available_Barren = CB_Find(content, "Barren Land", " "); content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Homes") != -1) { content = content.slice("Homes"); Building_Available_Homes = CB_Find(content, "Homes", " "); content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Farms") != -1) { content = content.slice("Farms"); Building_Available_Farms = CB_Find(content, "Farms", " "); content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Mills") != -1) { content = content.slice("Mills"); Building_Available_Mills = CB_Find(content, "Mills", " "); content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Banks") != -1) { content = content.slice("Banks"); Building_Available_Banks = CB_Find(content, "Banks", " "); content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Training Grounds") != -1) { content = content.slice("Training Grounds"); Building_Available_TG = CB_Find(content, "Training Grounds", " "); content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Armouries") != -1) { content = content.slice("Armouries"); Building_Available_Armouries = CB_Find(content, "Armouries", " "); content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Military Barracks") != -1) { content = content.slice("Military Barracks"); Building_Available_Barracks = CB_Find(content, "Military Barracks", " "); content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Forts") != -1) { content = content.slice("Forts"); Building_Available_Forts = CB_Find(content, "Forts", " "); content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Guard Stations") != -1) { content = content.slice("Guard Stations"); Building_Available_GS = CB_Find(content, "Guard Stations", " "); content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Hospitals") != -1) { content = content.slice("Hospitals"); Building_Available_Hospitals = CB_Find(content, "Hospitals", " "); content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Guilds") != -1) { content = content.slice("Guilds"); Building_Available_Guilds = CB_Find(content, "Guilds", " "); content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Towers") != -1) { content = content.slice("Towers"); Building_Available_Towers = CB_Find(content, "Towers", " "); content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Thieves' Dens") != -1) { content = content.slice("Thieves' Dens"); Building_Available_TD = CB_Find(content, "Thieves' Dens", " "); content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Watch Towers") != -1) { content = content.slice("Watch Towers"); Building_Available_WT = CB_Find(content, "Watch Towers", " "); content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Libraries") != -1) { content = content.slice("Libraries"); Building_Available_Libraries = CB_Find(content, "Libraries", " "); content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Schools") != -1) { content = content.slice("Schools"); Building_Available_Schools = CB_Find(content, "Schools", " "); content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Stables") != -1) { content = content.slice("Stables"); Building_Available_Stables = CB_Find(content, "Stables", " "); content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Dungeons") != -1) { content = content.slice("Dungeons"); Building_Available_Dungeons = CB_Find(content, "Dungeons", " "); content = content.slice(content.indexOf(eol) + 1); }

            Building_Available_Barren = Number(Building_Available_Barren);
            Building_Available_Homes = Number(Building_Available_Homes);
            Building_Available_Farms = Number(Building_Available_Farms);
            Building_Available_Mills = Number(Building_Available_Mills);
            Building_Available_Banks = Number(Building_Available_Banks);
            Building_Available_TG = Number(Building_Available_TG);
            Building_Available_Armouries = Number(Building_Available_Armouries);
            Building_Available_Barracks = Number(Building_Available_Barracks);
            Building_Available_Forts = Number(Building_Available_Forts);
            Building_Available_GS = Number(Building_Available_GS);
            Building_Available_Hospitals = Number(Building_Available_Hospitals);
            Building_Available_Guilds = Number(Building_Available_Guilds);
            Building_Available_Towers = Number(Building_Available_Towers);
            Building_Available_TD = Number(Building_Available_TD);
            Building_Available_WT = Number(Building_Available_WT);
            Building_Available_Libraries = Number(Building_Available_Libraries);
            Building_Available_Schools = Number(Building_Available_Schools);
            Building_Available_Stables = Number(Building_Available_Stables);
            Building_Available_Dungeons = Number(Building_Available_Dungeons);

            document.Calculator4.Building_Available_Barren.value = Building_Available_Barren;
            document.Calculator4.Building_Available_Homes.value = Building_Available_Homes;
            document.Calculator4.Building_Available_Farms.value = Building_Available_Farms;
            document.Calculator4.Building_Available_Mills.value = Building_Available_Mills;
            document.Calculator4.Building_Available_Banks.value = Building_Available_Banks;
            document.Calculator4.Building_Available_TG.value = Building_Available_TG;
            document.Calculator4.Building_Available_Armouries.value = Building_Available_Armouries;
            document.Calculator4.Building_Available_Barracks.value = Building_Available_Barracks;
            document.Calculator4.Building_Available_Forts.value = Building_Available_Forts;
            document.Calculator4.Building_Available_GS.value = Building_Available_GS;
            document.Calculator4.Building_Available_Hospitals.value = Building_Available_Hospitals;
            document.Calculator4.Building_Available_Guilds.value = Building_Available_Guilds;
            document.Calculator4.Building_Available_Towers.value = Building_Available_Towers;
            document.Calculator4.Building_Available_TD.value = Building_Available_TD;
            document.Calculator4.Building_Available_WT.value = Building_Available_WT;
            document.Calculator4.Building_Available_Libraries.value = Building_Available_Libraries;
            document.Calculator4.Building_Available_Schools.value = Building_Available_Schools;
            document.Calculator4.Building_Available_Stables.value = Building_Available_Stables;
            document.Calculator4.Building_Available_Dungeons.value = Building_Available_Dungeons;

            Building_Percentage_Barren = 0;
            Building_Percentage_Homes = 0;
            Building_Percentage_Farms = 0;
            Building_Percentage_Mills = 0;
            Building_Percentage_Banks = 0;
            Building_Percentage_TG = 0;
            Building_Percentage_Armouries = 0;
            Building_Percentage_Barracks = 0;
            Building_Percentage_Forts = 0;
            Building_Percentage_GS = 0;
            Building_Percentage_Hospitals = 0;
            Building_Percentage_Guilds = 0;
            Building_Percentage_Towers = 0;
            Building_Percentage_TD = 0;
            Building_Percentage_WT = 0;
            Building_Percentage_Libraries = 0;
            Building_Percentage_Schools = 0;
            Building_Percentage_Stables = 0;
            Building_Percentage_Dungeons = 0;

            content = content.slice(content.indexOf(" ") + 1);

            Building_Percentage_Barren = Math.round(Building_Available_Barren / Building_Available_Total * 1000) / 10;
            Building_Percentage_Homes = Math.round(Building_Available_Homes / Building_Available_Total * 1000) / 10;
            Building_Percentage_Farms = Math.round(Building_Available_Farms / Building_Available_Total * 1000) / 10;
            Building_Percentage_Mills = Math.round(Building_Available_Mills / Building_Available_Total * 1000) / 10;
            Building_Percentage_Banks = Math.round(Building_Available_Banks / Building_Available_Total * 1000) / 10;
            Building_Percentage_TG = Math.round(Building_Available_TG / Building_Available_Total * 1000) / 10;
            Building_Percentage_Armouries = Math.round(Building_Available_Armouries / Building_Available_Total * 1000) / 10;
            Building_Percentage_Barracks = Math.round(Building_Available_Barracks / Building_Available_Total * 1000) / 10;
            Building_Percentage_Forts = Math.round(Building_Available_Forts / Building_Available_Total * 1000) / 10;
            Building_Percentage_GS = Math.round(Building_Available_GS / Building_Available_Total * 1000) / 10;
            Building_Percentage_Hospitals = Math.round(Building_Available_Hospitals / Building_Available_Total * 1000) / 10;
            Building_Percentage_Guilds = Math.round(Building_Available_Guilds / Building_Available_Total * 1000) / 10;
            Building_Percentage_Towers = Math.round(Building_Available_Towers / Building_Available_Total * 1000) / 10;
            Building_Percentage_TD = Math.round(Building_Available_TD / Building_Available_Total * 1000) / 10;
            Building_Percentage_WT = Math.round(Building_Available_WT / Building_Available_Total * 1000) / 10;
            Building_Percentage_Libraries = Math.round(Building_Available_Libraries / Building_Available_Total * 1000) / 10;
            Building_Percentage_Schools = Math.round(Building_Available_Schools / Building_Available_Total * 1000) / 10;
            Building_Percentage_Stables = Math.round(Building_Available_Stables / Building_Available_Total * 1000) / 10;
            Building_Percentage_Dungeons = Math.round(Building_Available_Dungeons / Building_Available_Total * 1000) / 10;

            var temp = 0;

            Building_Progress_Barren = 0;
            Building_Progress_Homes = 0;
            Building_Progress_Farms = 0;
            Building_Progress_Mills = 0;
            Building_Progress_Banks = 0;
            Building_Progress_TG = 0;
            Building_Progress_Armouries = 0;
            Building_Progress_Barracks = 0;
            Building_Progress_Forts = 0;
            Building_Progress_GS = 0;
            Building_Progress_Hospitals = 0;
            Building_Progress_Guilds = 0;
            Building_Progress_Towers = 0;
            Building_Progress_TD = 0;
            Building_Progress_WT = 0;
            Building_Progress_Libraries = 0;
            Building_Progress_Schools = 0;
            Building_Progress_Stables = 0;
            Building_Progress_Dungeons = 0;

            content = content.slice(content.indexOf("Barren Land"));

            if (content.indexOf("Barren Land") != -1) { temp2 = content.slice(content.indexOf("Barren Land") + 12, content.indexOf(eol)); for (temp = 1; temp <= 24; temp++) { Building_Progress_Barren += Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp2.slice(temp2.indexOf(" ") + 1); } content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Homes") != -1) { temp2 = content.slice(content.indexOf("Homes") + 6, content.indexOf(eol)); for (temp = 1; temp <= 24; temp++) { Building_Progress_Homes += Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp2.slice(temp2.indexOf(" ") + 1); } content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Farms") != -1) { temp2 = content.slice(content.indexOf("Farms") + 6, content.indexOf(eol)); for (temp = 1; temp <= 24; temp++) { Building_Progress_Farms += Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp2.slice(temp2.indexOf(" ") + 1); } content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Mills") != -1) { temp2 = content.slice(content.indexOf("Mills") + 6, content.indexOf(eol)); for (temp = 1; temp <= 24; temp++) { Building_Progress_Mills += Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp2.slice(temp2.indexOf(" ") + 1); } content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Banks") != -1) { temp2 = content.slice(content.indexOf("Banks") + 6, content.indexOf(eol)); for (temp = 1; temp <= 24; temp++) { Building_Progress_Banks += Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp2.slice(temp2.indexOf(" ") + 1); } content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Training Grounds") != -1) { temp2 = content.slice(content.indexOf("Training Grounds") + 17, content.indexOf(eol)); for (temp = 1; temp <= 24; temp++) { Building_Progress_TG += Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp2.slice(temp2.indexOf(" ") + 1); } content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Armouries") != -1) { temp2 = content.slice(content.indexOf("Armouries") + 10, content.indexOf(eol)); for (temp = 1; temp <= 24; temp++) { Building_Progress_Armouries += Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp2.slice(temp2.indexOf(" ") + 1); } content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Military Barracks") != -1) { temp2 = content.slice(content.indexOf("Military Barracks") + 18, content.indexOf(eol)); for (temp = 1; temp <= 24; temp++) { Building_Progress_Barracks += Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp2.slice(temp2.indexOf(" ") + 1); } content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Forts") != -1) { temp2 = content.slice(content.indexOf("Forts") + 6, content.indexOf(eol)); for (temp = 1; temp <= 24; temp++) { Building_Progress_Forts += Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp2.slice(temp2.indexOf(" ") + 1); } content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Guard Stations") != -1) { temp2 = content.slice(content.indexOf("Guard Stations") + 15, content.indexOf(eol)); for (temp = 1; temp <= 24; temp++) { Building_Progress_GS += Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp2.slice(temp2.indexOf(" ") + 1); } content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Hospitals") != -1) { temp2 = content.slice(content.indexOf("Hospitals") + 10, content.indexOf(eol)); for (temp = 1; temp <= 24; temp++) { Building_Progress_Hospitals += Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp2.slice(temp2.indexOf(" ") + 1); } content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Guilds") != -1) { temp2 = content.slice(content.indexOf("Guilds") + 7, content.indexOf(eol)); for (temp = 1; temp <= 24; temp++) { Building_Progress_Guilds += Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp2.slice(temp2.indexOf(" ") + 1); } content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Towers") != -1) { temp2 = content.slice(content.indexOf("Towers") + 7, content.indexOf(eol)); for (temp = 1; temp <= 24; temp++) { Building_Progress_Towers += Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp2.slice(temp2.indexOf(" ") + 1); } content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Thieves' Dens") != -1) { temp2 = content.slice(content.indexOf("Thieves' Dens") + 14, content.indexOf(eol)); for (temp = 1; temp <= 24; temp++) { Building_Progress_TD += Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp2.slice(temp2.indexOf(" ") + 1); } content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Watch Towers") != -1) { temp2 = content.slice(content.indexOf("Watch Towers") + 13, content.indexOf(eol)); for (temp = 1; temp <= 24; temp++) { Building_Progress_WT += Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp2.slice(temp2.indexOf(" ") + 1); } content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Libraries") != -1) { temp2 = content.slice(content.indexOf("Libraries") + 10, content.indexOf(eol)); for (temp = 1; temp <= 24; temp++) { Building_Progress_Libraries += Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp2.slice(temp2.indexOf(" ") + 1); } content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Schools") != -1) { temp2 = content.slice(content.indexOf("Schools") + 8, content.indexOf(eol)); for (temp = 1; temp <= 24; temp++) { Building_Progress_Schools += Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp2.slice(temp2.indexOf(" ") + 1); } content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Stables") != -1) { temp2 = content.slice(content.indexOf("Stables") + 8, content.indexOf(eol)); for (temp = 1; temp <= 24; temp++) { Building_Progress_Stables += Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp2.slice(temp2.indexOf(" ") + 1); } content = content.slice(content.indexOf(eol) + 1); }
            if (content.indexOf("Dungeons") != -1) { temp2 = content.slice(content.indexOf("Dungeons") + 9, content.indexOf(eol)); for (temp = 1; temp <= 24; temp++) { Building_Progress_Dungeons += Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp2.slice(temp2.indexOf(" ") + 1); } content = content.slice(content.indexOf(eol) + 1); }

            document.Calculator4.Building_Progress_Barren.value = Building_Progress_Barren;
            document.Calculator4.Building_Progress_Homes.value = Building_Progress_Homes;
            document.Calculator4.Building_Progress_Farms.value = Building_Progress_Farms;
            document.Calculator4.Building_Progress_Mills.value = Building_Progress_Mills;
            document.Calculator4.Building_Progress_Banks.value = Building_Progress_Banks;
            document.Calculator4.Building_Progress_TG.value = Building_Progress_TG;
            document.Calculator4.Building_Progress_Armouries.value = Building_Progress_Armouries;
            document.Calculator4.Building_Progress_Barracks.value = Building_Progress_Barracks;
            document.Calculator4.Building_Progress_Forts.value = Building_Progress_Forts;
            document.Calculator4.Building_Progress_GS.value = Building_Progress_GS;
            document.Calculator4.Building_Progress_Hospitals.value = Building_Progress_Hospitals;
            document.Calculator4.Building_Progress_Guilds.value = Building_Progress_Guilds;
            document.Calculator4.Building_Progress_Towers.value = Building_Progress_Towers;
            document.Calculator4.Building_Progress_TD.value = Building_Progress_TD;
            document.Calculator4.Building_Progress_WT.value = Building_Progress_WT;
            document.Calculator4.Building_Progress_Libraries.value = Building_Progress_Libraries;
            document.Calculator4.Building_Progress_Schools.value = Building_Progress_Schools;
            document.Calculator4.Building_Progress_Stables.value = Building_Progress_Stables;
            document.Calculator4.Building_Progress_Dungeons.value = Building_Progress_Dungeons;

            Building_Progress_Total = Building_Progress_Homes + Building_Progress_Farms + Building_Progress_Mills + Building_Progress_Banks + Building_Progress_TG + Building_Progress_Armouries + Building_Progress_Barracks + Building_Progress_Forts + Building_Progress_GS + Building_Progress_Hospitals + Building_Progress_Guilds + Building_Progress_Towers + Building_Progress_TD + Building_Progress_WT + Building_Progress_Libraries + Building_Progress_Schools + Building_Progress_Stables + Building_Progress_Dungeons;

            Building_Available_Total = Building_Progress_Total + Building_Available_Barren + Building_Available_Homes + Building_Available_Farms + Building_Available_Mills + Building_Available_Banks + Building_Available_TG + Building_Available_Armouries + Building_Available_Barracks + Building_Available_Forts + Building_Available_GS + Building_Available_Hospitals + Building_Available_Guilds + Building_Available_Towers + Building_Available_TD + Building_Available_WT + Building_Available_Libraries + Building_Available_Schools + Building_Available_Stables + Building_Available_Dungeons;

            document.Calculator4.Building_Effectiveness.value = Building_Effectiveness;

            document.Calculator4.Building_Expand_Compact.value = "Expand";
            BuildingExpandCompact();
            document.Calculator4.Building_Expand_Compact.value = "Compact";
            BuildingExpandCompact();


            textonly = "" + c0 + "Survey / Buildings Report Information" + cc + Copyrights +
"<br />" + c1 + "Ruler Name: " + cc + c2 + Building_Prov + cc +
"<br />" + c1 + "Building Efficiency: " + cc + c2 + Building_Effectiveness + "%" + cc +
"<br />" + c1 + "Kingdom Stance: " + cc + c2 + "Normal" + cc +
"<br />";

            var xx = 0;

            if ((Building_Available_Barren != 0) || (Building_Progress_Barren != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Barren: " + cc; } if (Building_Available_Barren != 0) textonly += c2 + Building_Available_Barren + cc + c1 + " (" + cc + c2 + Building_Percentage_Barren + "%" + cc + c1 + ") "; if ((Building_Available_Barren != 0) && (Building_Progress_Barren != 0)) textonly += "+ "; if (Building_Progress_Barren != 0) textonly += cc + c2 + Building_Progress_Barren + cc + c1 + " in progress (+" + Math.round(Building_Progress_Barren / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Homes != 0) || (Building_Progress_Homes != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Homes: " + cc; } if (Building_Available_Homes != 0) textonly += c2 + Building_Available_Homes + cc + c1 + " (" + cc + c2 + Building_Percentage_Homes + "%" + cc + c1 + ") "; if ((Building_Available_Homes != 0) && (Building_Progress_Homes != 0)) textonly += "+ "; if (Building_Progress_Homes != 0) textonly += cc + c2 + Building_Progress_Homes + cc + c1 + " in progress (+" + Math.round(Building_Progress_Homes / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Farms != 0) || (Building_Progress_Farms != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Farms: " + cc; } if (Building_Available_Farms != 0) textonly += c2 + Building_Available_Farms + cc + c1 + " (" + cc + c2 + Building_Percentage_Farms + "%" + cc + c1 + ") "; if ((Building_Available_Farms != 0) && (Building_Progress_Farms != 0)) textonly += "+ "; if (Building_Progress_Farms != 0) textonly += cc + c2 + Building_Progress_Farms + cc + c1 + " in progress (+" + Math.round(Building_Progress_Farms / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Mills != 0) || (Building_Progress_Mills != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Mills: " + cc; } if (Building_Available_Mills != 0) textonly += c2 + Building_Available_Mills + cc + c1 + " (" + cc + c2 + Building_Percentage_Mills + "%" + cc + c1 + ") "; if ((Building_Available_Mills != 0) && (Building_Progress_Mills != 0)) textonly += "+ "; if (Building_Progress_Mills != 0) textonly += cc + c2 + Building_Progress_Mills + cc + c1 + " in progress (+" + Math.round(Building_Progress_Mills / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Banks != 0) || (Building_Progress_Banks != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Banks: " + cc; } if (Building_Available_Banks != 0) textonly += c2 + Building_Available_Banks + cc + c1 + " (" + cc + c2 + Building_Percentage_Banks + "%" + cc + c1 + ") "; if ((Building_Available_Banks != 0) && (Building_Progress_Banks != 0)) textonly += "+ "; if (Building_Progress_Banks != 0) textonly += cc + c2 + Building_Progress_Banks + cc + c1 + " in progress (+" + Math.round(Building_Progress_Banks / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_TG != 0) || (Building_Progress_TG != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". TG: " + cc; } if (Building_Available_TG != 0) textonly += c2 + Building_Available_TG + cc + c1 + " (" + cc + c2 + Building_Percentage_TG + "%" + cc + c1 + ") "; if ((Building_Available_TG != 0) && (Building_Progress_TG != 0)) textonly += "+ "; if (Building_Progress_TG != 0) textonly += cc + c2 + Building_Progress_TG + cc + c1 + " in progress (+" + Math.round(Building_Progress_TG / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Armouries != 0) || (Building_Progress_Armouries != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Armouries: " + cc; } if (Building_Available_Armouries != 0) textonly += c2 + Building_Available_Armouries + cc + c1 + " (" + cc + c2 + Building_Percentage_Armouries + "%" + cc + c1 + ") "; if ((Building_Available_Armouries != 0) && (Building_Progress_Armouries != 0)) textonly += "+ "; if (Building_Progress_Armouries != 0) textonly += cc + c2 + Building_Progress_Armouries + cc + c1 + " in progress (+" + Math.round(Building_Progress_Armouries / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Barracks != 0) || (Building_Progress_Barracks != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Barracks: " + cc; } if (Building_Available_Barracks != 0) textonly += c2 + Building_Available_Barracks + cc + c1 + " (" + cc + c2 + Building_Percentage_Barracks + "%" + cc + c1 + ") "; if ((Building_Available_Barracks != 0) && (Building_Progress_Barracks != 0)) textonly += "+ "; if (Building_Progress_Barracks != 0) textonly += cc + c2 + Building_Progress_Barracks + cc + c1 + " in progress (+" + Math.round(Building_Progress_Barracks / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Forts != 0) || (Building_Progress_Forts != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Forts: " + cc; } if (Building_Available_Forts != 0) textonly += c2 + Building_Available_Forts + cc + c1 + " (" + cc + c2 + Building_Percentage_Forts + "%" + cc + c1 + ") "; if ((Building_Available_Forts != 0) && (Building_Progress_Forts != 0)) textonly += "+ "; if (Building_Progress_Forts != 0) textonly += cc + c2 + Building_Progress_Forts + cc + c1 + " in progress (+" + Math.round(Building_Progress_Forts / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_GS != 0) || (Building_Progress_GS != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". GS: " + cc; } if (Building_Available_GS != 0) textonly += c2 + Building_Available_GS + cc + c1 + " (" + cc + c2 + Building_Percentage_GS + "%" + cc + c1 + ") "; if ((Building_Available_GS != 0) && (Building_Progress_GS != 0)) textonly += "+ "; if (Building_Progress_GS != 0) textonly += cc + c2 + Building_Progress_GS + cc + c1 + " in progress (+" + Math.round(Building_Progress_GS / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Hospitals != 0) || (Building_Progress_Hospitals != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Hospitals: " + cc; } if (Building_Available_Hospitals != 0) textonly += c2 + Building_Available_Hospitals + cc + c1 + " (" + cc + c2 + Building_Percentage_Hospitals + "%" + cc + c1 + ") "; if ((Building_Available_Hospitals != 0) && (Building_Progress_Hospitals != 0)) textonly += "+ "; if (Building_Progress_Hospitals != 0) textonly += cc + c2 + Building_Progress_Hospitals + cc + c1 + " in progress (+" + Math.round(Building_Progress_Hospitals / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Guilds != 0) || (Building_Progress_Guilds != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Guilds: " + cc; } if (Building_Available_Guilds != 0) textonly += c2 + Building_Available_Guilds + cc + c1 + " (" + cc + c2 + Building_Percentage_Guilds + "%" + cc + c1 + ") "; if ((Building_Available_Guilds != 0) && (Building_Progress_Guilds != 0)) textonly += "+ "; if (Building_Progress_Guilds != 0) textonly += cc + c2 + Building_Progress_Guilds + cc + c1 + " in progress (+" + Math.round(Building_Progress_Guilds / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Towers != 0) || (Building_Progress_Towers != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Towers: " + cc; } if (Building_Available_Towers != 0) textonly += c2 + Building_Available_Towers + cc + c1 + " (" + cc + c2 + Building_Percentage_Towers + "%" + cc + c1 + ") "; if ((Building_Available_Towers != 0) && (Building_Progress_Towers != 0)) textonly += "+ "; if (Building_Progress_Towers != 0) textonly += cc + c2 + Building_Progress_Towers + cc + c1 + " in progress (+" + Math.round(Building_Progress_Towers / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_TD != 0) || (Building_Progress_TD != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". TD: " + cc; } if (Building_Available_TD != 0) textonly += c2 + Building_Available_TD + cc + c1 + " (" + cc + c2 + Building_Percentage_TD + "%" + cc + c1 + ") "; if ((Building_Available_TD != 0) && (Building_Progress_TD != 0)) textonly += "+ "; if (Building_Progress_TD != 0) textonly += cc + c2 + Building_Progress_TD + cc + c1 + " in progress (+" + Math.round(Building_Progress_TD / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_WT != 0) || (Building_Progress_WT != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". WT: " + cc; } if (Building_Available_WT != 0) textonly += c2 + Building_Available_WT + cc + c1 + " (" + cc + c2 + Building_Percentage_WT + "%" + cc + c1 + ") "; if ((Building_Available_WT != 0) && (Building_Progress_WT != 0)) textonly += "+ "; if (Building_Progress_WT != 0) textonly += cc + c2 + Building_Progress_WT + cc + c1 + " in progress (+" + Math.round(Building_Progress_WT / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Libraries != 0) || (Building_Progress_Libraries != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Libraries: " + cc; } if (Building_Available_Libraries != 0) textonly += c2 + Building_Available_Libraries + cc + c1 + " (" + cc + c2 + Building_Percentage_Libraries + "%" + cc + c1 + ") "; if ((Building_Available_Libraries != 0) && (Building_Progress_Libraries != 0)) textonly += "+ "; if (Building_Progress_Libraries != 0) textonly += cc + c2 + Building_Progress_Libraries + cc + c1 + " in progress (+" + Math.round(Building_Progress_Libraries / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Schools != 0) || (Building_Progress_Schools != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Schools: " + cc; } if (Building_Available_Schools != 0) textonly += c2 + Building_Available_Schools + cc + c1 + " (" + cc + c2 + Building_Percentage_Schools + "%" + cc + c1 + ") "; if ((Building_Available_Schools != 0) && (Building_Progress_Schools != 0)) textonly += "+ "; if (Building_Progress_Schools != 0) textonly += cc + c2 + Building_Progress_Schools + cc + c1 + " in progress (+" + Math.round(Building_Progress_Schools / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Stables != 0) || (Building_Progress_Stables != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Stables: " + cc; } if (Building_Available_Stables != 0) textonly += c2 + Building_Available_Stables + cc + c1 + " (" + cc + c2 + Building_Percentage_Stables + "%" + cc + c1 + ") "; if ((Building_Available_Stables != 0) && (Building_Progress_Stables != 0)) textonly += "+ "; if (Building_Progress_Stables != 0) textonly += cc + c2 + Building_Progress_Stables + cc + c1 + " in progress (+" + Math.round(Building_Progress_Stables / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Dungeons != 0) || (Building_Progress_Dungeons != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Dungeons: " + cc; } if (Building_Available_Dungeons != 0) textonly += c2 + Building_Available_Dungeons + cc + c1 + " (" + cc + c2 + Building_Percentage_Dungeons + "%" + cc + c1 + ") "; if ((Building_Available_Dungeons != 0) && (Building_Progress_Dungeons != 0)) textonly += "+ "; if (Building_Progress_Dungeons != 0) textonly += cc + c2 + Building_Progress_Dungeons + cc + c1 + " in progress (+" + Math.round(Building_Progress_Dungeons / Building_Available_Total * 1000) / 10 + "%)" + cc;

            textonly += "<br />" +
"<br />" + c1 + "Total Land: " + cc + c2 + Building_Available_Total + cc + c1 + " Acres" + cc +
"<br />" + c1 + "In Progress: " + cc + c2 + Building_Progress_Total + cc + c1 + " Acres" + cc;

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




        }









        if ((content.indexOf("Our thieves scour the lands") != -1) || (EL == 3)) {

            if (EL == 3) {

                Export_Line = Export_Line_Decryption(content);

                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Prov = ABC_Decryption(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Kingdom = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Island = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_Workers = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Effectiveness = Number(Export_Line.slice(0, Export_Line.indexOf("_"))) / 10;
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_Barren = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_Homes = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_Farms = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_Mills = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_Banks = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_TG = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_Armouries = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_Barracks = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_Forts = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_GS = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_Hospitals = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_Guilds = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_Towers = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_TD = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_WT = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_Libraries = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_Schools = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_Stables = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Available_Dungeons = Number(Export_Line.slice(0));

            } else {

                content = content.slice(content.indexOf("Our thieves scour the lands of") + 32);
                temp = content.slice(0, content.indexOf(")") + 1);

                Building_Prov = temp.slice(0, temp.indexOf("("));
                Building_Kingdom = temp.slice(temp.indexOf("(") + 1, temp.indexOf(":"));
                Building_Island = temp.slice(temp.indexOf(":") + 1, temp.indexOf(")"));

                content = content.slice(content.indexOf("Available Workers"));
                content = content.slice(content.indexOf(" ") + 1);
                content = content.slice(content.indexOf(" ") + 1);
                Building_Available_Workers = content.slice(0, content.indexOf(eol));
                Building_Available_Workers = Number(Building_Available_Workers);

                content = content.slice(content.indexOf("Building Efficiency"));
                content = content.slice(content.indexOf(" ") + 1);
                content = content.slice(content.indexOf(" ") + 1);
                Building_Effectiveness = content.slice(0, content.indexOf("%"));
                Building_Effectiveness = Number(Building_Effectiveness);

                content = content.slice("Barren Land"); Building_Available_Barren = CB_Find(content, "Barren Land", " "); content = content.slice(content.indexOf(eol) + 1);
                content = content.slice("Homes"); Building_Available_Homes = CB_Find(content, "Homes", " "); content = content.slice(content.indexOf(eol) + 1);
                content = content.slice("Farms"); Building_Available_Farms = CB_Find(content, "Farms", " "); content = content.slice(content.indexOf(eol) + 1);
                content = content.slice("Mills"); Building_Available_Mills = CB_Find(content, "Mills", " "); content = content.slice(content.indexOf(eol) + 1);
                content = content.slice("Banks"); Building_Available_Banks = CB_Find(content, "Banks", " "); content = content.slice(content.indexOf(eol) + 1);
                content = content.slice("Training Grounds"); Building_Available_TG = CB_Find(content, "Training Grounds", " "); content = content.slice(content.indexOf(eol) + 1);
                content = content.slice("Armouries"); Building_Available_Armouries = CB_Find(content, "Armouries", " "); content = content.slice(content.indexOf(eol) + 1);
                content = content.slice("Military Barracks"); Building_Available_Barracks = CB_Find(content, "Military Barracks", " "); content = content.slice(content.indexOf(eol) + 1);
                content = content.slice("Forts"); Building_Available_Forts = CB_Find(content, "Forts", " "); content = content.slice(content.indexOf(eol) + 1);
                content = content.slice("Guard Stations"); Building_Available_GS = CB_Find(content, "Guard Stations", " "); content = content.slice(content.indexOf(eol) + 1);
                content = content.slice("Hospitals"); Building_Available_Hospitals = CB_Find(content, "Hospitals", " "); content = content.slice(content.indexOf(eol) + 1);
                content = content.slice("Guilds"); Building_Available_Guilds = CB_Find(content, "Guilds", " "); content = content.slice(content.indexOf(eol) + 1);
                content = content.slice("Towers"); Building_Available_Towers = CB_Find(content, "Towers", " "); content = content.slice(content.indexOf(eol) + 1);
                content = content.slice("Thieves' Dens"); Building_Available_TD = CB_Find(content, "Thieves' Dens", " "); content = content.slice(content.indexOf(eol) + 1);
                content = content.slice("Watch Towers"); Building_Available_WT = CB_Find(content, "Watch Towers", " "); content = content.slice(content.indexOf(eol) + 1);
                content = content.slice("Libraries"); Building_Available_Libraries = CB_Find(content, "Libraries", " "); content = content.slice(content.indexOf(eol) + 1);
                content = content.slice("Schools"); Building_Available_Schools = CB_Find(content, "Schools", " "); content = content.slice(content.indexOf(eol) + 1);
                content = content.slice("Stables"); Building_Available_Stables = CB_Find(content, "Stables", " "); content = content.slice(content.indexOf(eol) + 1);
                content = content.slice("Dungeons"); Building_Available_Dungeons = CB_Find(content, "Dungeons", " "); content = content.slice(content.indexOf(eol) + 1);

            }

            Export_Line = "545653_" + ABC_Encryption(Building_Prov) + "_" + Building_Kingdom + "_" + Building_Island + "_" + Building_Available_Workers + "_" + Math.round(Building_Effectiveness * 10) + "_" + Building_Available_Barren + "_" + Building_Available_Homes + "_" + Building_Available_Farms + "_" + Building_Available_Mills + "_" + Building_Available_Banks + "_" + Building_Available_TG + "_" + Building_Available_Armouries + "_" + Building_Available_Barracks + "_" + Building_Available_Forts + "_" + Building_Available_GS + "_" + Building_Available_Hospitals + "_" + Building_Available_Guilds + "_" + Building_Available_Towers + "_" + Building_Available_TD + "_" + Building_Available_WT + "_" + Building_Available_Libraries + "_" + Building_Available_Schools + "_" + Building_Available_Stables + "_" + Building_Available_Dungeons;




            Building_Available_Total = Building_Available_Barren + Building_Available_Homes + Building_Available_Farms + Building_Available_Mills + Building_Available_Banks + Building_Available_TG + Building_Available_Armouries + Building_Available_Barracks + Building_Available_Forts + Building_Available_GS + Building_Available_Hospitals + Building_Available_Guilds + Building_Available_Towers + Building_Available_TD + Building_Available_WT + Building_Available_Libraries + Building_Available_Schools + Building_Available_Stables + Building_Available_Dungeons;

            if (Building_Available_Total > 0) {

                Building_Percentage_Barren = Math.round(Building_Available_Barren / Building_Available_Total * 1000) / 10;
                Building_Percentage_Homes = Math.round(Building_Available_Homes / Building_Available_Total * 1000) / 10;
                Building_Percentage_Farms = Math.round(Building_Available_Farms / Building_Available_Total * 1000) / 10;
                Building_Percentage_Mills = Math.round(Building_Available_Mills / Building_Available_Total * 1000) / 10;
                Building_Percentage_Banks = Math.round(Building_Available_Banks / Building_Available_Total * 1000) / 10;
                Building_Percentage_TG = Math.round(Building_Available_TG / Building_Available_Total * 1000) / 10;
                Building_Percentage_Armouries = Math.round(Building_Available_Armouries / Building_Available_Total * 1000) / 10;
                Building_Percentage_Barracks = Math.round(Building_Available_Barracks / Building_Available_Total * 1000) / 10;
                Building_Percentage_Forts = Math.round(Building_Available_Forts / Building_Available_Total * 1000) / 10;
                Building_Percentage_GS = Math.round(Building_Available_GS / Building_Available_Total * 1000) / 10;
                Building_Percentage_Hospitals = Math.round(Building_Available_Hospitals / Building_Available_Total * 1000) / 10;
                Building_Percentage_Guilds = Math.round(Building_Available_Guilds / Building_Available_Total * 1000) / 10;
                Building_Percentage_Towers = Math.round(Building_Available_Towers / Building_Available_Total * 1000) / 10;
                Building_Percentage_TD = Math.round(Building_Available_TD / Building_Available_Total * 1000) / 10;
                Building_Percentage_WT = Math.round(Building_Available_WT / Building_Available_Total * 1000) / 10;
                Building_Percentage_Libraries = Math.round(Building_Available_Libraries / Building_Available_Total * 1000) / 10;
                Building_Percentage_Schools = Math.round(Building_Available_Schools / Building_Available_Total * 1000) / 10;
                Building_Percentage_Stables = Math.round(Building_Available_Stables / Building_Available_Total * 1000) / 10;
                Building_Percentage_Dungeons = Math.round(Building_Available_Dungeons / Building_Available_Total * 1000) / 10;

            }

            if (Building_Available_Total == 0) {

                Building_Percentage_Barren = 0;
                Building_Percentage_Homes = 0;
                Building_Percentage_Farms = 0;
                Building_Percentage_Mills = 0;
                Building_Percentage_Banks = 0;
                Building_Percentage_TG = 0;
                Building_Percentage_Armouries = 0;
                Building_Percentage_Barracks = 0;
                Building_Percentage_Forts = 0;
                Building_Percentage_GS = 0;
                Building_Percentage_Hospitals = 0;
                Building_Percentage_Guilds = 0;
                Building_Percentage_Towers = 0;
                Building_Percentage_TD = 0;
                Building_Percentage_WT = 0;
                Building_Percentage_Libraries = 0;
                Building_Percentage_Schools = 0;
                Building_Percentage_Stables = 0;
                Building_Percentage_Dungeons = 0;
                Building_Percentage_Total = 0;

            }


            textonly = "" + c0 + "Buildings Report of " + Building_Prov + " (" + Building_Kingdom + ":" + Building_Island + ")" + cc + Copyrights +
"<br />" + c1 + "Building Efficiency: " + cc + c2 + Building_Effectiveness + "%" + cc +
"<br />" + c1 + "Kingdom Stance: " + cc + c2 + "Normal" + cc +
"<br />";

            var xx = 0;

            if ((Building_Available_Barren != 0) || (Building_Progress_Barren != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Barren: " + cc; } if (Building_Available_Barren != 0) textonly += c2 + Building_Available_Barren + cc + c1 + " (" + cc + c2 + Building_Percentage_Barren + "%" + cc + c1 + ") "; if ((Building_Available_Barren != 0) && (Building_Progress_Barren != 0)) textonly += "+ "; if (Building_Progress_Barren != 0) textonly += cc + c2 + Building_Progress_Barren + cc + c1 + " in progress (+" + Math.round(Building_Progress_Barren / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Homes != 0) || (Building_Progress_Homes != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Homes: " + cc; } if (Building_Available_Homes != 0) textonly += c2 + Building_Available_Homes + cc + c1 + " (" + cc + c2 + Building_Percentage_Homes + "%" + cc + c1 + ") "; if ((Building_Available_Homes != 0) && (Building_Progress_Homes != 0)) textonly += "+ "; if (Building_Progress_Homes != 0) textonly += cc + c2 + Building_Progress_Homes + cc + c1 + " in progress (+" + Math.round(Building_Progress_Homes / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Farms != 0) || (Building_Progress_Farms != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Farms: " + cc; } if (Building_Available_Farms != 0) textonly += c2 + Building_Available_Farms + cc + c1 + " (" + cc + c2 + Building_Percentage_Farms + "%" + cc + c1 + ") "; if ((Building_Available_Farms != 0) && (Building_Progress_Farms != 0)) textonly += "+ "; if (Building_Progress_Farms != 0) textonly += cc + c2 + Building_Progress_Farms + cc + c1 + " in progress (+" + Math.round(Building_Progress_Farms / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Mills != 0) || (Building_Progress_Mills != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Mills: " + cc; } if (Building_Available_Mills != 0) textonly += c2 + Building_Available_Mills + cc + c1 + " (" + cc + c2 + Building_Percentage_Mills + "%" + cc + c1 + ") "; if ((Building_Available_Mills != 0) && (Building_Progress_Mills != 0)) textonly += "+ "; if (Building_Progress_Mills != 0) textonly += cc + c2 + Building_Progress_Mills + cc + c1 + " in progress (+" + Math.round(Building_Progress_Mills / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Banks != 0) || (Building_Progress_Banks != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Banks: " + cc; } if (Building_Available_Banks != 0) textonly += c2 + Building_Available_Banks + cc + c1 + " (" + cc + c2 + Building_Percentage_Banks + "%" + cc + c1 + ") "; if ((Building_Available_Banks != 0) && (Building_Progress_Banks != 0)) textonly += "+ "; if (Building_Progress_Banks != 0) textonly += cc + c2 + Building_Progress_Banks + cc + c1 + " in progress (+" + Math.round(Building_Progress_Banks / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_TG != 0) || (Building_Progress_TG != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". TG: " + cc; } if (Building_Available_TG != 0) textonly += c2 + Building_Available_TG + cc + c1 + " (" + cc + c2 + Building_Percentage_TG + "%" + cc + c1 + ") "; if ((Building_Available_TG != 0) && (Building_Progress_TG != 0)) textonly += "+ "; if (Building_Progress_TG != 0) textonly += cc + c2 + Building_Progress_TG + cc + c1 + " in progress (+" + Math.round(Building_Progress_TG / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Armouries != 0) || (Building_Progress_Armouries != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Armouries: " + cc; } if (Building_Available_Armouries != 0) textonly += c2 + Building_Available_Armouries + cc + c1 + " (" + cc + c2 + Building_Percentage_Armouries + "%" + cc + c1 + ") "; if ((Building_Available_Armouries != 0) && (Building_Progress_Armouries != 0)) textonly += "+ "; if (Building_Progress_Armouries != 0) textonly += cc + c2 + Building_Progress_Armouries + cc + c1 + " in progress (+" + Math.round(Building_Progress_Armouries / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Barracks != 0) || (Building_Progress_Barracks != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Barracks: " + cc; } if (Building_Available_Barracks != 0) textonly += c2 + Building_Available_Barracks + cc + c1 + " (" + cc + c2 + Building_Percentage_Barracks + "%" + cc + c1 + ") "; if ((Building_Available_Barracks != 0) && (Building_Progress_Barracks != 0)) textonly += "+ "; if (Building_Progress_Barracks != 0) textonly += cc + c2 + Building_Progress_Barracks + cc + c1 + " in progress (+" + Math.round(Building_Progress_Barracks / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Forts != 0) || (Building_Progress_Forts != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Forts: " + cc; } if (Building_Available_Forts != 0) textonly += c2 + Building_Available_Forts + cc + c1 + " (" + cc + c2 + Building_Percentage_Forts + "%" + cc + c1 + ") "; if ((Building_Available_Forts != 0) && (Building_Progress_Forts != 0)) textonly += "+ "; if (Building_Progress_Forts != 0) textonly += cc + c2 + Building_Progress_Forts + cc + c1 + " in progress (+" + Math.round(Building_Progress_Forts / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_GS != 0) || (Building_Progress_GS != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". GS: " + cc; } if (Building_Available_GS != 0) textonly += c2 + Building_Available_GS + cc + c1 + " (" + cc + c2 + Building_Percentage_GS + "%" + cc + c1 + ") "; if ((Building_Available_GS != 0) && (Building_Progress_GS != 0)) textonly += "+ "; if (Building_Progress_GS != 0) textonly += cc + c2 + Building_Progress_GS + cc + c1 + " in progress (+" + Math.round(Building_Progress_GS / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Hospitals != 0) || (Building_Progress_Hospitals != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Hospitals: " + cc; } if (Building_Available_Hospitals != 0) textonly += c2 + Building_Available_Hospitals + cc + c1 + " (" + cc + c2 + Building_Percentage_Hospitals + "%" + cc + c1 + ") "; if ((Building_Available_Hospitals != 0) && (Building_Progress_Hospitals != 0)) textonly += "+ "; if (Building_Progress_Hospitals != 0) textonly += cc + c2 + Building_Progress_Hospitals + cc + c1 + " in progress (+" + Math.round(Building_Progress_Hospitals / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Guilds != 0) || (Building_Progress_Guilds != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Guilds: " + cc; } if (Building_Available_Guilds != 0) textonly += c2 + Building_Available_Guilds + cc + c1 + " (" + cc + c2 + Building_Percentage_Guilds + "%" + cc + c1 + ") "; if ((Building_Available_Guilds != 0) && (Building_Progress_Guilds != 0)) textonly += "+ "; if (Building_Progress_Guilds != 0) textonly += cc + c2 + Building_Progress_Guilds + cc + c1 + " in progress (+" + Math.round(Building_Progress_Guilds / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Towers != 0) || (Building_Progress_Towers != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Towers: " + cc; } if (Building_Available_Towers != 0) textonly += c2 + Building_Available_Towers + cc + c1 + " (" + cc + c2 + Building_Percentage_Towers + "%" + cc + c1 + ") "; if ((Building_Available_Towers != 0) && (Building_Progress_Towers != 0)) textonly += "+ "; if (Building_Progress_Towers != 0) textonly += cc + c2 + Building_Progress_Towers + cc + c1 + " in progress (+" + Math.round(Building_Progress_Towers / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_TD != 0) || (Building_Progress_TD != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". TD: " + cc; } if (Building_Available_TD != 0) textonly += c2 + Building_Available_TD + cc + c1 + " (" + cc + c2 + Building_Percentage_TD + "%" + cc + c1 + ") "; if ((Building_Available_TD != 0) && (Building_Progress_TD != 0)) textonly += "+ "; if (Building_Progress_TD != 0) textonly += cc + c2 + Building_Progress_TD + cc + c1 + " in progress (+" + Math.round(Building_Progress_TD / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_WT != 0) || (Building_Progress_WT != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". WT: " + cc; } if (Building_Available_WT != 0) textonly += c2 + Building_Available_WT + cc + c1 + " (" + cc + c2 + Building_Percentage_WT + "%" + cc + c1 + ") "; if ((Building_Available_WT != 0) && (Building_Progress_WT != 0)) textonly += "+ "; if (Building_Progress_WT != 0) textonly += cc + c2 + Building_Progress_WT + cc + c1 + " in progress (+" + Math.round(Building_Progress_WT / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Libraries != 0) || (Building_Progress_Libraries != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Libraries: " + cc; } if (Building_Available_Libraries != 0) textonly += c2 + Building_Available_Libraries + cc + c1 + " (" + cc + c2 + Building_Percentage_Libraries + "%" + cc + c1 + ") "; if ((Building_Available_Libraries != 0) && (Building_Progress_Libraries != 0)) textonly += "+ "; if (Building_Progress_Libraries != 0) textonly += cc + c2 + Building_Progress_Libraries + cc + c1 + " in progress (+" + Math.round(Building_Progress_Libraries / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Schools != 0) || (Building_Progress_Schools != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Schools: " + cc; } if (Building_Available_Schools != 0) textonly += c2 + Building_Available_Schools + cc + c1 + " (" + cc + c2 + Building_Percentage_Schools + "%" + cc + c1 + ") "; if ((Building_Available_Schools != 0) && (Building_Progress_Schools != 0)) textonly += "+ "; if (Building_Progress_Schools != 0) textonly += cc + c2 + Building_Progress_Schools + cc + c1 + " in progress (+" + Math.round(Building_Progress_Schools / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Stables != 0) || (Building_Progress_Stables != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Stables: " + cc; } if (Building_Available_Stables != 0) textonly += c2 + Building_Available_Stables + cc + c1 + " (" + cc + c2 + Building_Percentage_Stables + "%" + cc + c1 + ") "; if ((Building_Available_Stables != 0) && (Building_Progress_Stables != 0)) textonly += "+ "; if (Building_Progress_Stables != 0) textonly += cc + c2 + Building_Progress_Stables + cc + c1 + " in progress (+" + Math.round(Building_Progress_Stables / Building_Available_Total * 1000) / 10 + "%)" + cc;
            if ((Building_Available_Dungeons != 0) || (Building_Progress_Dungeons != 0)) { xx += 1; textonly += "<br />" + c1 + xx + ". Dungeons: " + cc; } if (Building_Available_Dungeons != 0) textonly += c2 + Building_Available_Dungeons + cc + c1 + " (" + cc + c2 + Building_Percentage_Dungeons + "%" + cc + c1 + ") "; if ((Building_Available_Dungeons != 0) && (Building_Progress_Dungeons != 0)) textonly += "+ "; if (Building_Progress_Dungeons != 0) textonly += cc + c2 + Building_Progress_Dungeons + cc + c1 + " in progress (+" + Math.round(Building_Progress_Dungeons / Building_Available_Total * 1000) / 10 + "%)" + cc;

            textonly += "<br />" +
"<br />" + c1 + "Total Land: " + cc + c2 + Building_Available_Total + cc + c1 + " Acres (" + Math.round((100 - Building_Percentage_Barren) * 10) / 10 + "% built)" + cc +
"<br />" +
"<br />" + c0 + "** Export Line **" + cc + "<br />";

            document.getElementById('CBtext2').innerHTML = textonly;
            //document.getElementById('CBtext2').innerHTML = textonly + c0+"<xmp>"+ Export_Line_Encryption(Export_Line) +"</xmp>"+cc;
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
            //document.Calculator2.CBtext1.value = textonly + Export_Line_Encryption(Export_Line);
            copy(document.Calculator2.CBtext1.value);
            CopyExportLine(Export_Line_Encryption(Export_Line));

        }

    }

}
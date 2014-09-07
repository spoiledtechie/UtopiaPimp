function Clipboard_Throne() {

    if ((content.indexOf("Trade Balance") != -1) || (EL == 1)) {

        if (document.getElementById('Ultima_Popups').checked == false) document.getElementById('Ultima_floating_window0').style.display = "block";
        if (document.getElementById('Ultima_Popups').checked == false) document.getElementById('Ultima_floating_window1').style.display = "block";

        var kingdom = 0;
        var island = 0;
        var Rank = 2; // Rank Set to Lord for monarchy
        var Gender = 0;
        var Race = 0;
        var Personality = 0;
        var Land = 0;
        var Money = 0;
        var Food = 0;
        var Runes = 0;
        var Peasants = 0;
        var Building_Effectiveness = 0;
        var Trade_Balance = 0;
        var Networth = 0;
        var Soldiers = 0;
        var War_Horses = 0;
        var Prisoners = 0;
        var Offensive_Points = 0;
        var Defensive_Points = 0;
        var Offensive_Units = 0;
        var Defensive_Units = 0;
        var Elite_Units = 0;
        var Thieves = 0;
        var Wizards = 0;
        var Stealth = 0;
        var Mana = 0;
        var Tax_Percentage = 0;
        var Daily_Income = 0;
        var MEO = 0;
        var MED = 0;
        var SCI = 0;
        var npa = 0;
        var OverPop = 0;
        var Plague = 0;
        var War = 0;
        var Dragon = 0;
        var Hit = 0;
        var Crystal_Ball = 0;
        var Ruler_Name = "";
        var Unbuilt = "";
        var Additional_Name = "";
        var Networth_Name = "";
        var Export_Line = "";

        var Utopia_Year = 0;
        var Utopia_Month = 0;
        var Utopia_Day = 0;
        var Utopia_Hour = 0;
        var Utopia_Minute = 0;
        var Real_Date = "";
        var Utopian_Date = "";
        var temp = "";

        if (EL == 1) {
            Export_Line = Export_Line_Decryption(content);

            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Province_Name = ABC_Decryption(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); kingdom = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); island = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Utopia_Year = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Utopia_Month = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Utopia_Day = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Utopia_Minute = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Race = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Gender = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Rank = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Personality = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Land = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Peasants = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Building_Effectiveness = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Offensive_Points = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Defensive_Points = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Money = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Food = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Runes = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Soldiers = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Offensive_Units = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Defensive_Units = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Elite_Units = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Thieves = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Stealth = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Wizards = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Mana = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Trade_Balance = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); War_Horses = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Prisoners = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Networth = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Dragon = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Hit = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Crystal_Ball = Number(Export_Line.slice(0));

        } else {

            temp = content.slice(content.indexOf(") as of") + 8); temp = temp.slice(0, temp.indexOf(" (")); Utopian_Date = temp;

            Utopia_Month = temp.slice(0, temp.indexOf(" ")); for (i = 0; i < 7; i++) { if (Utopia_Month == Months[i]) Utopia_Month = i + 1; }

            temp = temp.slice(temp.indexOf(" ") + 1);

            Utopia_Day = Number(temp.slice(0, temp.indexOf(" ")));
            Utopia_Year = Number(temp.slice(temp.indexOf(" YR") + 3));
            Utopia_Minute = 0;

            if (content.indexOf("next tick:") != -1) {
                temp = content.slice(content.indexOf("next tick:") + 11);

                if (temp.indexOf("minute") != -1) Utopia_Minute = Math.round(60 - Number(temp.slice(0, temp.indexOf(" "))));
                if (temp.indexOf("second") != -1) Utopia_Minute = Math.round(60 - Number(temp.slice(0, temp.indexOf(" "))) / 60);

            }



            content = content.slice(content.indexOf("Worth/Acre"));
            content = content.slice(content.indexOf(eol) + 1);

            content = content.slice(content.indexOf(" ") + 1); temp = content.slice(0, content.indexOf(" "));
            content = content.slice(content.indexOf(" ") + 1); temp = content.slice(0, content.indexOf(" "));
            content = content.slice(content.indexOf(" ") + 1); temp = content.slice(0, content.indexOf(" "));
            content = content.slice(content.indexOf(" ") + 1); Networth = Math.round(Number(content.slice(0, content.indexOf(" "))));

            if (content.indexOf("Message from Our Monarch") != -1) {

                content = content.slice(content.indexOf("Message from Our Monarch"));
                content = content.slice(content.indexOf(eol) + 1);
                content = content.slice(content.indexOf(eol) + 1);

            }

            if (content.indexOf("operation") != -1) Crystal_Ball = 1;
            content = content.slice(content.indexOf("The Province of"));


            temp = content.slice(content.indexOf("The Province of") + 16, content.indexOf(") as of ") + 1);
            Province_Name = temp.slice(0, temp.indexOf("("));
            kingdom = temp.slice(temp.indexOf("(") + 1, temp.indexOf(":"));
            island = temp.slice(temp.indexOf(":") + 1, temp.indexOf(")"));

            content = content.slice(content.indexOf(")"));

            temp = content.slice(content.indexOf("Race") + 5); temp = temp.slice(0, temp.indexOf("Soldiers") - 1);
            for (i = 0; i < Race_Name.length; i++) if (Race_Name[i] == temp) Race = i;

            Soldiers = CB_Find(content, "Soldiers", eol);

            temp = content.slice(content.indexOf("Ruler") + 6); temp = temp.slice(0, temp.indexOf(eol));

            Offensive_Units = Number(temp.slice(temp.lastIndexOf(" ") + 1));

            Ruler_Name = temp.slice(0, temp.lastIndexOf(Offensive_Specialist_Name[Race]));

            for (i = 0; i < Personality_Search_Key.length; i++) if (Ruler_Name.indexOf(Personality_Search_Key[i]) != -1) Personality = i;
            for (t = 0; t < 2; t++) for (i = 0; i < Rank_Search_Key.length; i++) if (Ruler_Name.indexOf(Rank_Search_Key[t][i]) != -1) { Gender = t; Rank = i; }

            content = content.slice(content.indexOf("Ruler"));
            content = content.slice(content.indexOf(eol) + 1);



            Building_Effectiveness = CB_Find(content, "Building Eff.", "%");
            Money = CB_Find(content, "Money", " ");
            Trade_Balance = CB_Find(content, "Trade Balance", " ");
            Land = CB_Find(content, "Land", " ");
            Food = CB_Find(content, "Food", " ");
            Runes = CB_Find(content, "Runes", " ");
            Peasants = CB_Find(content, "Peasants", " ");
            //Soldiers               = CB_Find( content, "Soldiers"      , eol );
            War_Horses = CB_Find(content, "War Horses", eol);
            Prisoners = CB_Find(content, "Prisoners", eol);
            Offensive_Points = CB_Find(content, "Off. Points", eol);
            Defensive_Points = CB_Find(content, "Def. Points", eol);

            //Offensive_Units        = CB_Find( content, Offensive_Specialist_Name[Race] , eol );

            temp = content.slice(content.indexOf("Land "));
            temp = temp.slice(temp.indexOf(" ") + 1);
            temp = temp.slice(temp.indexOf(" ") + 1);
            temp = temp.slice(temp.indexOf(" ") + 1);
            temp = temp.slice(0, temp.indexOf(eol));

            Defensive_Units = Number(temp);

            //Defensive_Units        = CB_Find( content, Defensive_Specialist_Name[Race] , eol );
            Elite_Units = CB_Find(content, Elite_Unit_Name[Race], eol);

            if (content.indexOf(Plague_Search_Key) != -1) Plague = 1;
            if (content.indexOf(War_Search_Key) != -1) War = 1;
            if (content.indexOf(OverPop_Search_Key) != -1) OverPop = 1;

            for (i = 1; i < 5; i++) if (content.indexOf(Dragon_Search_Key[i]) != -1) Dragon = i;
            for (i = 1; i < 5; i++) if (content.indexOf(Hit_Search_Key[i]) != -1) Hit = i;

            if (Crystal_Ball == 0) {

                Thieves = CB_Find(content, "Thieves", " ");
                Wizards = CB_Find(content, "Wizards", " ");
                Stealth = CB_Find(content, "Thieves " + Thieves + " ", "%");
                Mana = CB_Find(content, "Wizards " + Wizards + " ", "%");

            }

        }


        // *******************************************************
        //       Extended options for external sites - Start
        // *******************************************************

        var DataSaveForm = document.getElementById('SendData');
        for (i = 0; i < DataSaveForm.elements.length; i++) DataSaveForm.elements[i].value = "";

        DataSaveForm.Throne_Province_Name.value = Province_Name;
        DataSaveForm.Throne_kingdom.value = kingdom;
        DataSaveForm.Throne_island.value = island;
        DataSaveForm.Throne_Utopia_Year.value = Utopia_Year;
        DataSaveForm.Throne_Utopia_Month.value = Utopia_Month;
        DataSaveForm.Throne_Utopia_Day.value = Utopia_Day;
        DataSaveForm.Throne_Utopia_Minute.value = Utopia_Minute;
        DataSaveForm.Throne_Race.value = Race;
        DataSaveForm.Throne_Gender.value = Gender;
        DataSaveForm.Throne_Rank.value = Rank;
        DataSaveForm.Throne_Personality.value = Personality;
        DataSaveForm.Throne_Land.value = Land;
        DataSaveForm.Throne_Peasants.value = Peasants;
        DataSaveForm.Throne_BE.value = Building_Effectiveness;
        DataSaveForm.Throne_Offensive_Points.value = Offensive_Points;
        DataSaveForm.Throne_Defensive_Points.value = Defensive_Points;
        DataSaveForm.Throne_Money.value = Money;
        DataSaveForm.Throne_Food.value = Food;
        DataSaveForm.Throne_Runes.value = Runes;
        DataSaveForm.Throne_Soldiers.value = Soldiers;
        DataSaveForm.Throne_Offensive_Units.value = Offensive_Units;
        DataSaveForm.Throne_Defensive_Units.value = Defensive_Units;
        DataSaveForm.Throne_Elite_Units.value = Elite_Units;
        DataSaveForm.Throne_Thieves.value = Thieves;
        DataSaveForm.Throne_Stealth.value = Stealth;
        DataSaveForm.Throne_Wizards.value = Wizards;
        DataSaveForm.Throne_Mana.value = Mana;
        DataSaveForm.Throne_Trade_Balance.value = Trade_Balance;
        DataSaveForm.Throne_War_Horses.value = War_Horses;
        DataSaveForm.Throne_Prisoners.value = Prisoners;
        DataSaveForm.Throne_Networth.value = Networth;
        DataSaveForm.Throne_Dragon.value = Dragon;
        DataSaveForm.Throne_Hit.value = Hit;
        DataSaveForm.Throne_Crystal_Ball.value = Crystal_Ball;

        // *******************************************************
        //       Extended options for external sites - End
        // *******************************************************


        Export_Line = "3837_" + ABC_Encryption(Province_Name) + "_" + kingdom + "_" + island + "_" + Utopia_Year + "_" + Utopia_Month + "_" + Utopia_Day + "_" + Utopia_Minute + "_" + Race + "_" + Gender + "_" + Rank + "_" +
Personality + "_" + Land + "_" + Peasants + "_" + Building_Effectiveness + "_" + Offensive_Points + "_" + Defensive_Points + "_" +
Money + "_" + Food + "_" + Runes + "_" + Soldiers + "_" + Offensive_Units + "_" + Defensive_Units + "_" + Elite_Units + "_" + Thieves + "_" + Stealth + "_" +
Wizards + "_" + Mana + "_" + Trade_Balance + "_" + War_Horses + "_" + Prisoners + "_" + Networth + "_" + Dragon + "_" + Hit + "_" + Crystal_Ball;

        Utopia_Hour = Utopia_Day - 1;

        Real_Date = RealTime(Utopia_Year, Utopia_Month, Utopia_Day, Utopia_Hour, Utopia_Minute);

        var Population = Peasants + Soldiers + Offensive_Units + Defensive_Units + Elite_Units + Thieves + Wizards;
        var Draft_Rate = Math.round((Soldiers + Offensive_Units + Defensive_Units + Elite_Units + Thieves) / Population * 10000) / 100;

        temp = Offensive_Units * Offensive_Specialist_Strength[Race] + Elite_Units * Offensive_Elite_Unit_Strength[Race] + Soldiers + War_Horses;

        if (temp > 0) MEO = Math.round(Offensive_Points / temp * 10000) / 100;
        if (temp == 0) MEO = 100;

        temp = Defensive_Units * Defensive_Specialist_Strength[Race] + Elite_Units * Defensive_Elite_Unit_Strength[Race] + Soldiers;

        if (temp > 0) MED = Math.round(Defensive_Points / temp * 10000) / 100;
        if (temp == 0) MED = 100;

        var horm = War_Horses; if (horm > Soldiers + Offensive_Units + Elite_Units * 0.5) horm = Math.round(Soldiers + Offensive_Units + Elite_Units * 0.5);

        var el75 = Math.round((Offensive_Units * Offensive_Specialist_Strength[Race] + Elite_Units * 0.5 * Offensive_Elite_Unit_Strength[Race] + Soldiers + horm) * MEO / 100);
        var el25 = Math.round((Defensive_Units * Defensive_Specialist_Strength[Race] + Elite_Units * 0.5 * Defensive_Elite_Unit_Strength[Race] + Soldiers) * MED / 100);

        var horm = War_Horses; if (horm > Soldiers + Offensive_Units + Elite_Units) horm = Soldiers + Offensive_Units + Elite_Units;
        horm = Math.round(horm * MEO / 100);


        var OPA = Math.round(Offensive_Points / Land * 100) / 100;
        var DPA = Math.round(Defensive_Points / Land * 100) / 100;
        var TPA = Math.round(Thieves / Land * 100) / 100;
        var WPA = Math.round(Wizards / Land * 100) / 100;
        var PPA = Math.round(Population / Land * 100) / 100;
        var pa75 = Math.round(el75 / Land * 100) / 100;
        var pa25 = Math.round(el25 / Land * 100) / 100;

        var offm = Math.round(Offensive_Units * Offensive_Specialist_Strength[Race] * MEO / 100);
        var defm = Math.round(Defensive_Units * Defensive_Specialist_Strength[Race] * MED / 100);
        var oelm = Math.round(Elite_Units * Offensive_Elite_Unit_Strength[Race] * MEO / 100);
        var delm = Math.round(Elite_Units * Defensive_Elite_Unit_Strength[Race] * MED / 100);
        var prim = Math.round(Prisoners * 3 * MEO / 100);

        temp = 0;

        if (Rank == 0) temp = 0;
        if (Rank == 1) temp = 0;
        if (Rank == 2) temp = 2;
        if (Rank == 3) temp = 4;
        if (Rank == 4) temp = 6;
        if (Rank == 5) temp = 10;
        if (Rank == 6) temp = 14;
        if (Rank == 7) temp = 18;
        if (Rank == 8) temp = 22;
        if (Race == 3) temp = temp / 2;

        Daily_Income = (2.25 * Peasants + 0.5 * Prisoners) * (1 + temp / 100);

        if (Race == 0) Daily_Income = Daily_Income * 1.3;
        if (Rank == 9) Daily_Income = Daily_Income * 1.1;
        if (Personality == 0) Daily_Income = Daily_Income * 1.15;

        if ((Personality != 0) && (Plague == 1)) Daily_Income = Daily_Income * 0.85;
        if ((Personality != 0) && (Dragon == 1)) Daily_Income = Daily_Income * 0.9;

        Daily_Income = Math.round(Daily_Income);




        if (Crystal_Ball == 0) {

            if (Trade_Balance < (-4) * Networth) Tax_Percentage = Math.round((-4 * Trade_Balance / Networth - 16) * 100) / 100;
            Tax_Percentage = " (" + Tax_Percentage + "% tax rate)";

            SCI = Math.round(Networth - (Peasants + Math.round(1.5 * Soldiers) + Offensive_Specialist_Networth[Race] * Offensive_Units + Defensive_Specialist_Networth[Race] * Defensive_Units + Elite_Unit_Networth[Race] * Elite_Units + Math.floor(0.001 * Money) + 0.6 * War_Horses + 4 * Thieves + 4 * Wizards));
            npa = Math.round(Networth / Land * 100) / 100;

            var minsci = 0;
            var maxsci = 0;
            var minland = Land * 15;
            var maxland = Land * 55;

            if (SCI > maxland) minsci = SCI - maxland;
            if (SCI > minland) maxsci = SCI - minland;
            if (SCI < maxland) maxland = SCI - minsci;

            var sciland = minland + minsci;
            var minlandnw = Land * 15;
            var maxlandnw = Land * 55;
            var minbooks = Math.round(minsci * 92);
            var maxbooks = Math.round(maxsci * 93);

            var BarrenLands = 0;
            var InProgress = Math.round(maxsci / 15);
            var Owned = Math.round(maxsci / 40);

            if (InProgress > Land) InProgress = Land;
            if (Owned > Land) Owned = Land;

            if (Land * 15 - 14 > maxsci) {
                Unbuilt = 0;
                Unbuilt = Math.round(Land - maxsci / 15);
                Unbuilt = Add_Commas(Unbuilt);
                BarrenLands = Unbuilt;
                Unbuilt = c1 + "(at least " + cc + c2 + Unbuilt + cc + c1 + " unbuilt acres)" + cc;
            }

            if (SCI <= Land * 15) Unbuilt = c3 + "(no science & all land is unbuilt!)" + cc;

            if ((SCI > Land * 15) && (SCI < Land * 15 + 15)) Unbuilt = c3 + "(all land is unbuilt!)" + cc;

            var estimated_sci = Math.round(SCI * 0.025);
            if (estimated_sci < minsci) estimated_sci = minsci;
            var estimated_books = Math.round(estimated_sci * 92);


            var NW_Networth = Math.round(Networth);
            var NW_Money = Math.floor(Money_Networth * Money);
            var NW_Peasants = Math.round(Peasants_Networth * Peasants);
            var NW_Soldiers = Math.round(Soldiers_Networth * Soldiers);
            var NW_Off = Math.round(Offensive_Specialist_Networth[Race] * Offensive_Units);
            var NW_Def = Math.round(Defensive_Specialist_Networth[Race] * Defensive_Units);
            var NW_Elites = Math.round(Elite_Unit_Networth[Race] * Elite_Units);
            var NW_Horses = Math.round(War_Horses_Networth * War_Horses);
            var NW_Thieves = Math.round(Thieves_Networth * Thieves);
            var NW_Wizards = Math.round(Wizards_Networth * Wizards);

            var NWP_Networth = Math.round(NW_Networth / Networth * 10000) / 100;
            var NWP_Money = Math.round(NW_Money / Networth * 10000) / 100;
            var NWP_Peasants = Math.round(NW_Peasants / Networth * 10000) / 100;
            var NWP_Soldiers = Math.round(NW_Soldiers / Networth * 10000) / 100;
            var NWP_Off = Math.round(NW_Off / Networth * 10000) / 100;
            var NWP_Def = Math.round(NW_Def / Networth * 10000) / 100;
            var NWP_Elites = Math.round(NW_Elites / Networth * 10000) / 100;
            var NWP_Horses = Math.round(NW_Horses / Networth * 10000) / 100;
            var NWP_Thieves = Math.round(NW_Thieves / Networth * 10000) / 100;
            var NWP_Wizards = Math.round(NW_Wizards / Networth * 10000) / 100;
            var NWP_SCI = Math.round(SCI / Networth * 10000) / 100;

            Thieves = Add_Commas(Thieves);
            Wizards = Add_Commas(Wizards);
            SCI = Add_Commas(SCI);
            minsci = Add_Commas(minsci);
            maxsci = Add_Commas(maxsci);
            minland = Add_Commas(minland);
            maxland = Add_Commas(maxland);
            sciland = Add_Commas(sciland);
            minlandnw = Add_Commas(minlandnw);
            maxlandnw = Add_Commas(maxlandnw);
            minbooks = Add_Commas(minbooks);
            maxbooks = Add_Commas(maxbooks);
            InProgress = Add_Commas(InProgress);
            Owned = Add_Commas(Owned);
            estimated_sci = Add_Commas(estimated_sci);
            estimated_books = Add_Commas(estimated_books);
            NW_Networth = Add_Commas(NW_Networth);
            NW_Money = Add_Commas(NW_Money);
            NW_Peasants = Add_Commas(NW_Peasants);
            NW_Soldiers = Add_Commas(NW_Soldiers);
            NW_Off = Add_Commas(NW_Off);
            NW_Def = Add_Commas(NW_Def);
            NW_Elites = Add_Commas(NW_Elites);
            NW_Horses = Add_Commas(NW_Horses);
            NW_Thieves = Add_Commas(NW_Thieves);
            NW_Wizards = Add_Commas(NW_Wizards);
            NW_SCI = Add_Commas(SCI);

            Networth_Name = "<br />" +
"<br />" + c1 + "Networth: " + cc + c2 + NW_Networth + cc + c1 + " (" + npa + " per Acre)" + cc +
"<br />" + c1 + "Building Networth+Science Networth: " + cc + c2 + SCI + cc + c1 + "gc" + cc +
"<br />" + c1 + "Min. Building Networth+Science Networth: " + cc + c2 + sciland + cc + c1 + "gc" + cc +
"<br />" + c1 + "Min. Building Networth: " + cc + c2 + minland + cc + c1 + "gc (Min. Possible " + minlandnw + "gc)" + cc +
"<br />" + c1 + "Max. Building Networth: " + cc + c2 + maxland + cc + c1 + "gc (Max. Possible " + maxlandnw + "gc)" + cc +
"<br />" + c1 + "Min. Science Networth: " + cc + c2 + minsci + cc + c1 + "gc (" + minbooks + " books)" + cc +
"<br />" + c1 + "Max. Science Networth: " + cc + c2 + maxsci + cc + c1 + "gc (" + maxbooks + " books)" + cc +
"<br />" + c3 + "estimated" + cc + c1 + " Science Networth: " + cc + c2 + estimated_sci + cc + c1 + "gc (" + estimated_books + " books)" + cc +
"<br />" +
"<br />" + c3 + "(Exploration not included in Owned Barren Lands)" + cc +
"<br />" + c1 + "Min. Owned Barren Lands: " + cc + c2 + BarrenLands + cc + c1 + " unbuilt acres" + cc +
"<br />" + c1 + "Max. Owned Barren Lands is always " + Land + " unbuilt acres" + cc +
"<br />" + c1 + "Min. In Progress is always 0 buildings" + cc +
"<br />" + c1 + "Max. In Progress: " + cc + c2 + InProgress + cc + c1 + " buildings" + cc +
"<br />" + c1 + "Min. Owned is always 0 buildings" + cc +
"<br />" + c1 + "Max. Owned: " + cc + c2 + Owned + cc + c1 + " buildings" + cc +
"<br />" +
"<br />" + c0 + "** Networth Table: **" + cc +
"<br />" + c1 + "Total Networth: " + cc + c2 + NW_Networth + cc + c1 + "gc (" + NWP_Networth + "%)" + cc +
"<br />" + c1 + "Money: " + cc + c2 + NW_Money + cc + c1 + "gc (" + NWP_Money + "%)" + cc +
"<br />" + c1 + "Peasants: " + cc + c2 + NW_Peasants + cc + c1 + "gc (" + NWP_Peasants + "%)" + cc +
"<br />" + c1 + "Soldiers: " + cc + c2 + NW_Soldiers + cc + c1 + "gc (" + NWP_Soldiers + "%)" + cc +
"<br />" + c1 + Offensive_Specialist_Name[Race] + ": " + cc + c2 + NW_Off + cc + c1 + "gc (" + NWP_Off + "%)" + cc +
"<br />" + c1 + Defensive_Specialist_Name[Race] + ": " + cc + c2 + NW_Def + cc + c1 + "gc (" + NWP_Def + "%)" + cc +
"<br />" + c1 + Elite_Unit_Name[Race] + ": " + cc + c2 + NW_Elites + cc + c1 + "gc (" + NWP_Elites + "%)" + cc +
"<br />" + c1 + "War-Horses: " + cc + c2 + NW_Horses + cc + c1 + "gc (" + NWP_Horses + "%)" + cc +
"<br />" + c1 + "Thieves: " + cc + c2 + NW_Thieves + cc + c1 + "gc (" + NWP_Thieves + "%)" + cc +
"<br />" + c1 + "Wizards: " + cc + c2 + NW_Wizards + cc + c1 + "gc (" + NWP_Wizards + "%)" + cc +
"<br />" + c1 + "Building+Science: " + cc + c2 + NW_SCI + cc + c1 + "gc (" + NWP_SCI + "%)" + cc;

            Additional_Name += "<br />" + c1 + "Thieves: " + cc + c2 + Thieves + cc + c1 + " (" + TPA + " per Acre / " + cc + c2 + Stealth + "%" + cc + c1 + " Stealth)" + cc;
            Additional_Name += "<br />" + c1 + "Wizards: " + cc + c2 + Wizards + cc + c1 + " (" + WPA + " per Acre / " + cc + c2 + Mana + "%" + cc + c1 + " Mana)" + cc + "<br />";


        } else {

            Networth = 0;
            Networth_Name = "";
            Additional_Name = "";
            Tax_Percentage = "";
            SCI = 0;
            Thieves = 0;
            Wizards = 0;


            document.getElementById('EnemyThief').innerHTML = Province_Name + " (" + kingdom + ":" + island + ")";
            document.Calculator6.EnemyNetworth.value = Networth;
            document.Calculator6.Thievery_Money.value = Money;
            document.Calculator6.Thievery_Food.value = Food;
            document.Calculator6.Thievery_Runes.value = Runes;
            document.Calculator6.Thievery_Peasants.value = Peasants;
            document.Calculator6.Thievery_Wizards.value = Wizards;
            document.Calculator6.Thievery_Soldiers.value = Soldiers;
            document.Calculator6.Thievery_OffSpecs.value = Offensive_Units;
            document.Calculator6.Thievery_DefSpecs.value = Defensive_Units;
            document.Calculator6.Thievery_Elites.value = Elite_Units;
            document.Calculator6.Thievery_Horses.value = War_Horses;
            document.Calculator6.Thievery_Buildings.value = 0;
            document.Calculator6.Thievery_Prisoners.value = Prisoners;

        }



        var Construction = Math.round(8 / 31 * (Land + 2900));
        var Raze = Math.round((1400 + Land) / 4);

        if (Race == 2) Construction = 0;

        var Bonus_12h = Peasants * 5;
        var Bonus_20h = Peasants * 15;


        if (War == 1) Additional_Name += "<br />" + c3 + War_Name + cc + "<br />";
        if (Dragon != 0) Additional_Name += "<br />" + c3 + Dragon_Name[Dragon] + cc + "<br />";
        if (Plague == 1) Additional_Name += "<br />" + c3 + Plague_Name + cc + "<br />";
        if (Hit != 0) Additional_Name += "<br />" + c3 + Hit_Name[Hit] + cc + "<br />";
        if (OverPop == 1) Additional_Name += "<br />" + c3 + OverPop_Name + cc + "<br />";


        if (Crystal_Ball == 0) {

            document.getElementById('YourProvinceName').innerHTML = Province_Name + " (" + kingdom + ":" + island + ")";

            document.Calculator.YourRace.value = Race;
            document.Calculator.YourPersonality.value = Personality;
            document.Calculator.YourRelations.value = 0;
            document.Calculator.YourStance.value = 0;
            document.Calculator.YourDragon.value = Dragon;
            document.Calculator.YourSoldiers.value = Soldiers;
            document.Calculator.YourOffSpecs.value = Offensive_Units;
            document.Calculator.YourDefSpecs.value = Defensive_Units;
            document.Calculator.YourElites.value = Elite_Units;
            document.Calculator.YourLand.value = Land;
            document.Calculator.YourNW.value = Networth;
            document.Calculator.YourKDNW.value = 0;
            document.Calculator.YourOME.value = MEO;
            document.Calculator.YourDME.value = MED;
            document.Calculator.YourGenerals.value = 0;
            document.Calculator.YourAttackTime.value = 0;
            document.Calculator.YourHorses.value = War_Horses;
            document.Calculator.YourMerc.value = 0;
            document.Calculator.YourPris.value = Prisoners;
            document.Calculator.YourMilitarySci.value = 0;
            document.Calculator.YourRecentlyHit.value = 0;
            document.Calculator.YourGuardStation.value = 0;
            document.Calculator.YourPeasants.value = Peasants;
            document.Calculator.YourRawME.value = 100;
            document.Calculator.YourBE.value = Building_Effectiveness;
            document.Calculator.YourTG.value = 0;
            document.Calculator.YourForts.value = 0;
            document.Calculator.YourRank.value = Rank;
            document.Calculator.YourFanaticismBox.checked = false;
            document.Calculator.YourAggressionBox.checked = false;
            document.Calculator.YourProtectionBox.checked = true;
            document.Calculator.YourPlagueBox.checked = false;

            if (War == 1) document.Calculator.YourRelations.value = 3;
            if (Rank == 9) document.Calculator.YourRank.value = 2;
            if (Rank != 9) document.Calculator.YourMonarchBox.checked = false;
            if (Rank == 9) document.Calculator.YourMonarchBox.checked = true;
            if (Plague == 1) document.Calculator.YourPlagueBox.checked = true;

            Thieves = Thieves.replace(/,/g, "");

            document.getElementById('YourThief').innerHTML = Province_Name + " (" + kingdom + ":" + island + ")";
            document.Calculator6.YourThieves.value = Thieves;
            document.Calculator6.YourNetworth.value = Networth;
            document.Calculator6.YourPersonality.value = Personality_Name[Personality];

        } else {

            document.getElementById('EnemyProvinceName').innerHTML = Province_Name + " (" + kingdom + ":" + island + ")";

            document.Calculator.EnemyRace.value = Race;
            document.Calculator.EnemyPersonality.value = Personality;
            document.Calculator.EnemyRelations.value = 0;
            document.Calculator.EnemyStance.value = 0;
            document.Calculator.EnemyDragon.value = Dragon;
            document.Calculator.EnemySoldiers.value = Soldiers;
            document.Calculator.EnemyOffSpecs.value = Offensive_Units;
            document.Calculator.EnemyDefSpecs.value = Defensive_Units;
            document.Calculator.EnemyElites.value = Elite_Units;
            document.Calculator.EnemyLand.value = Land;
            document.Calculator.EnemyNW.value = 0;
            document.Calculator.EnemyKDNW.value = 0;
            document.Calculator.EnemyOME.value = MEO;
            document.Calculator.EnemyDME.value = MED;
            document.Calculator.EnemyGenerals.value = 0;
            document.Calculator.EnemyAttackTime.value = 0;
            document.Calculator.EnemyHorses.value = War_Horses;
            document.Calculator.EnemyMerc.value = 0;
            document.Calculator.EnemyPris.value = Prisoners;
            document.Calculator.EnemyMilitarySci.value = 0;
            document.Calculator.EnemyRecentlyHit.value = 0;
            document.Calculator.EnemyGuardStation.value = 0;
            document.Calculator.EnemyPeasants.value = Peasants;
            document.Calculator.EnemyRawME.value = 100;
            document.Calculator.EnemyBE.value = Building_Effectiveness;
            document.Calculator.EnemyTG.value = 0;
            document.Calculator.EnemyForts.value = 0;
            document.Calculator.EnemyRank.value = Rank;
            document.Calculator.EnemyFanaticismBox.checked = false;
            document.Calculator.EnemyAggressionBox.checked = false;
            document.Calculator.EnemyProtectionBox.checked = true;
            document.Calculator.EnemyPlagueBox.checked = false;

            if (War == 1) document.Calculator.EnemyRelations.value = 3;
            if (Rank == 9) document.Calculator.EnemyRank.value = 2;
            if (Rank != 9) document.Calculator.EnemyMonarchBox.checked = false;
            if (Rank == 9) document.Calculator.EnemyMonarchBox.checked = true;
            if (Plague == 1) document.Calculator.EnemyPlagueBox.checked = true;

            if (War == 1) document.Calculator6.EnemyRelations.value = 3;

        }


        Land = Add_Commas(Land);
        Money = Add_Commas(Money);
        Daily_Income = Add_Commas(Daily_Income);
        Food = Add_Commas(Food);
        Runes = Add_Commas(Runes);
        Population = Add_Commas(Population);
        Peasants = Add_Commas(Peasants);
        Trade_Balance = Add_Commas(Trade_Balance);
        Soldiers = Add_Commas(Soldiers);
        Offensive_Units = Add_Commas(Offensive_Units);
        Defensive_Units = Add_Commas(Defensive_Units);
        Elite_Units = Add_Commas(Elite_Units);
        War_Horses = Add_Commas(War_Horses);
        Prisoners = Add_Commas(Prisoners);
        Networth = Add_Commas(Networth);
        offm = Add_Commas(offm);
        defm = Add_Commas(defm);
        oelm = Add_Commas(oelm);
        delm = Add_Commas(delm);
        horm = Add_Commas(horm);
        prim = Add_Commas(prim);
        Offensive_Points = Add_Commas(Offensive_Points);
        Defensive_Points = Add_Commas(Defensive_Points);
        el75 = Add_Commas(el75);
        el25 = Add_Commas(el25);
        Construction = Add_Commas(Construction);
        Raze = Add_Commas(Raze);
        Bonus_12h = Add_Commas(Bonus_12h);
        Bonus_20h = Add_Commas(Bonus_20h);



        textonly = c0 + "The Province of " + Province_Name + " (" + kingdom + ":" + island + ")" + cc + Copyrights +
"<br />" + c1 + "Utopian Date: " + cc + c2 + Utopian_Date + cc + c1 + " (" + cc + c2 + Math.round(Utopia_Minute * 100 / 60) + "%" + cc + c1 + " in the day)" + cc +
"<br />" + c1 + "Real Date: " + cc + c2 + Real_Date + cc +
"<br />" +
"<br />" + c1 + "Personality & Race: " + cc + c2 + "The " + Personality_Name[Personality] + cc + c1 + ", " + cc + c2 + Race_Name[Race] + cc +
"<br />" + c1 + "Land: " + cc + c2 + Land + cc + c1 + " Acres " + Unbuilt + cc +
"<br />" + c1 + "Money: " + cc + c2 + Money + cc + c1 + "gc (" + cc + c3 + "estimated" + cc + c1 + " daily income " + Daily_Income + "gc)" + cc +
"<br />" + c1 + "Food: " + cc + c2 + Food + cc + c1 + " bushels" + cc +
"<br />" + c1 + "Runes: " + cc + c2 + Runes + cc + c1 + " runes" + cc +
"<br />" + c1 + "Population: " + cc + c2 + Population + cc + c1 + " citizens (" + PPA + " per Acre)" + cc +
"<br />" + c1 + "Peasants: " + cc + c2 + Peasants + cc + c1 + " (" + cc + c2 + Building_Effectiveness + "%" + cc + c1 + " Building Efficiency)" + cc +
"<br />" + c1 + "Trade Balance: " + cc + c2 + Trade_Balance + cc + c1 + "gc" + Tax_Percentage + cc +

Networth_Name +

"<br />" +
"<br />" + c1 + "Military Eff. with Stance: " + cc + c2 + MEO + "%" + cc + c1 + " off. / " + cc + c2 + MED + "%" + cc + c1 + " def." + cc +
"<br />" + c1 + "Soldiers: " + cc + c2 + Soldiers + cc + c1 + " (" + Draft_Rate + "% estimated draft rate)" + cc +
"<br />" + c1 + Offensive_Specialist_Name[Race] + ": " + cc + c2 + Offensive_Units + cc + c1 + " (" + offm + " offense)" + cc +
"<br />" + c1 + Defensive_Specialist_Name[Race] + ": " + cc + c2 + Defensive_Units + cc + c1 + " (" + defm + " defense)" + cc +
"<br />" + c1 + Elite_Unit_Name[Race] + ": " + cc + c2 + Elite_Units + cc + c1 + " (" + oelm + " offense / " + delm + " defense)" + cc +
"<br />" + c1 + "War-Horses: " + cc + c2 + War_Horses + cc + c1 + " (up to " + horm + " additional offense)" + cc +
"<br />" + c1 + "Prisoners: " + cc + c2 + Prisoners + cc + c1 + " (" + prim + " offense)" + cc +
"<br />" +
"<br />" + c1 + "Total Modified Offense: " + cc + c2 + Offensive_Points + cc + c1 + " (" + OPA + " per Acre)" + cc +
"<br />" + c1 + "Practical (50% elites): " + cc + c2 + el75 + cc + c1 + " (" + pa75 + " per Acre)" + cc +
"<br />" + c1 + "Total Modified Defense: " + cc + c2 + Defensive_Points + cc + c1 + " (" + DPA + " per Acre)" + cc +
"<br />" + c1 + "Practical (50% elites): " + cc + c2 + el25 + cc + c1 + " (" + pa25 + " per Acre)" + cc +
"<br />" +

Additional_Name +

"<br />" + c1 + "Buildings: " + cc + c2 + Construction + cc + c1 + "gc to build, " + cc + c2 + Raze + cc + c1 + "gc to raze" + cc +
        //"<br />"+c1+ "Away bonus: "                       +cc+c2+ Bonus_12h        +cc+c1+ "gc (12h) / "   +cc+c2+ Bonus_20h +cc+c1+ "gc (20h)"   +cc+
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
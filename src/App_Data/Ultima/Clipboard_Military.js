function Clipboard_Military() {

    if ((content.indexOf("Military Effectiveness") != -1) || (EL == 4)) {

        if (document.getElementById('Ultima_Popups').checked == false) document.getElementById('Ultima_floating_window0').style.display = "block";

        var Military_Title = "";
        var Military_Province_Name = "Self SOM";
        var Military_Personality = "";
        var Military_Kingdom = 0;
        var Military_Island = 0;
        var Military_Race = 0;
        var Military_Raw = 0;
        var Military_Total_Captured_Land = 0;
        var Military_Total_Summary_MEO = 0;
        var Military_Total_Summary_MED = 0;
        var Military_Offensive_Points = 0;
        var Military_Defensive_Points = 0;
        var Military_Non_Peasants = 0;
        var Military_MEO = 0;
        var Military_MED = 0;
        var Military_SOM = 0;

        var Military_Training_Offensive_Units = 0;
        var Military_Training_Defensive_Units = 0;
        var Military_Training_Elite_Units = 0;
        var Military_Training_Thieves = 0;

        var Military_Training_Offensive_Units_MEO = 0;
        var Military_Training_Defensive_Units_MED = 0;
        var Military_Training_Elite_Units_MEO = 0;
        var Military_Training_Elite_Units_MED = 0;

        var Military_General = new Array(0, 0, 0, 0, 0); // Available = 0 , Unavailable = 1
        var Military_Soldiers = new Array(0, 0, 0, 0, 0);
        var Military_Soldiers_MEO = new Array(0, 0, 0, 0, 0);
        var Military_Soldiers_MED = new Array(0, 0, 0, 0, 0);
        var Military_Offensive_Units = new Array(0, 0, 0, 0, 0);
        var Military_Defensive_Units = new Array(0, 0, 0, 0, 0);
        var Military_Offensive_Units_MEO = new Array(0, 0, 0, 0, 0);
        var Military_Defensive_Units_MED = new Array(0, 0, 0, 0, 0);
        var Military_Elite_Units = new Array(0, 0, 0, 0, 0);
        var Military_Elite_Units_MEO = new Array(0, 0, 0, 0, 0);
        var Military_Elite_Units_MED = new Array(0, 0, 0, 0, 0);
        var Military_War_Horses = new Array(0, 0, 0, 0, 0);
        var Military_War_Horses_MEO = new Array(0, 0, 0, 0, 0);
        var Military_Captured_Land = new Array(0, 0, 0, 0, 0);
        var Military_Total_MEO = new Array(0, 0, 0, 0, 0);
        var Military_Total_MED = new Array(0, 0, 0, 0, 0);
        var Military_Time = new Array(0, 0, 0, 0, 0);



        if (EL == 4) {
            Export_Line = Export_Line_Decryption(content);

            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_Province_Name = ABC_Decryption(Export_Line.slice(0, Export_Line.indexOf("_")));

            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_Kingdom = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_Island = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_Race = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_Offensive_Points = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_Defensive_Points = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_Raw = Number(Export_Line.slice(0, Export_Line.indexOf("_"))) / 10;
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_Non_Peasants = Number(Export_Line.slice(0, Export_Line.indexOf("_"))) / 10;
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_MEO = Number(Export_Line.slice(0, Export_Line.indexOf("_"))) / 10;
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_MED = Number(Export_Line.slice(0, Export_Line.indexOf("_"))) / 10;

            for (i = 0; i < 5; i++) {

                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_General[i] = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_Soldiers[i] = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_Offensive_Units[i] = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_Defensive_Units[i] = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_Elite_Units[i] = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_War_Horses[i] = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_Captured_Land[i] = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_Time[i] = Number(Export_Line.slice(0, Export_Line.indexOf("_"))) / 10;

            }

            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_Training_Offensive_Units = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_Training_Defensive_Units = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_Training_Elite_Units = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_Training_Thieves = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
            Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Military_SOM = Number(Export_Line.slice(0));

        } else {


            if (content.indexOf("Our thieves listen in on a report") != -1) Military_SOM = 1;

            if (Military_SOM == 1) {

                content = content.slice(content.indexOf("Our thieves listen in on a report") + 62);
                temp = content.slice(0, content.indexOf(")") + 1);
                Military_Province_Name = temp.slice(0, temp.indexOf("("));
                Military_Kingdom = temp.slice(temp.indexOf("(") + 1, temp.indexOf(":"));
                Military_Island = temp.slice(temp.indexOf(":") + 1, temp.indexOf(")"));

                temp = content.slice(content.indexOf("Military Affairs") + 18);
                temp = temp.slice(0, temp.indexOf("generals available") - 11);
                for (i = 0; i < 11; i++) if (temp.indexOf(Personality_Search_Key[i]) != -1) Military_Personality = i;

            }

            for (i = 0; i < 10; i++) if (content.indexOf(Offensive_Specialist_Name[i]) != -1) Military_Race = i;

            Military_Offensive_Points = CB_Find(content, "Net Offensive Points at Home", eol);
            Military_Defensive_Points = CB_Find(content, "Net Defensive Points at Home", eol);

            Military_MEO = CB_Find(content, "Offensive Military Effectiveness", "%");
            Military_MED = CB_Find(content, "Defensive Military Effectiveness", "%");
            Military_Raw = CB_Find(content, "our military is functioning at", "%");
            Military_Non_Peasants = CB_Find(content, "approximately", "%");

            content = content.slice(content.indexOf("approximately "));
            content = content.slice(content.indexOf("Standing Army"));
            content = content.slice(content.indexOf(" ") + 1);
            content = content.slice(content.indexOf(" ") + 1);

            temp = content.slice(0, content.indexOf("Soldiers "));


            Military_General[1] = temp.charAt(0); if (Military_General[1] == "A") { Military_Time[1] = temp.slice(temp.indexOf("(") + 1, temp.indexOf("days left)") - 1); temp = temp.slice(temp.indexOf("days left")); }
            temp = temp.slice(temp.indexOf(" ") + 1); temp = temp.slice(temp.indexOf(" ") + 1);
            Military_General[2] = temp.charAt(0); if (Military_General[2] == "A") { Military_Time[2] = temp.slice(temp.indexOf("(") + 1, temp.indexOf("days left)") - 1); temp = temp.slice(temp.indexOf("days left")); }
            temp = temp.slice(temp.indexOf(" ") + 1); temp = temp.slice(temp.indexOf(" ") + 1);
            Military_General[3] = temp.charAt(0); if (Military_General[3] == "A") { Military_Time[3] = temp.slice(temp.indexOf("(") + 1, temp.indexOf("days left)") - 1); temp = temp.slice(temp.indexOf("days left")); }
            temp = temp.slice(temp.indexOf(" ") + 1); temp = temp.slice(temp.indexOf(" ") + 1);
            Military_General[4] = temp.charAt(0); if (Military_General[4] == "A") { Military_Time[4] = temp.slice(temp.indexOf("(") + 1, temp.indexOf("days left)") - 1); temp = temp.slice(temp.indexOf("days left")); }


            if (Military_General[1] == "A") { Military_General[1] = 1; } else { Military_General[1] = 0; }
            if (Military_General[2] == "A") { Military_General[2] = 1; } else { Military_General[2] = 0; }
            if (Military_General[3] == "A") { Military_General[3] = 1; } else { Military_General[3] = 0; }
            if (Military_General[4] == "A") { Military_General[4] = 1; } else { Military_General[4] = 0; }


            temp = content.slice(content.indexOf("Soldiers ")); temp = temp.slice(0, temp.indexOf(eol) + 1);

            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[0] == 0) Military_Soldiers[0] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[1] == 1) Military_Soldiers[1] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[2] == 1) Military_Soldiers[2] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[3] == 1) Military_Soldiers[3] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[4] == 1) Military_Soldiers[4] = temp.slice(0, temp.indexOf(eol));

            temp = content.slice(content.indexOf(Offensive_Specialist_Name[Military_Race] + " ")); temp = temp.slice(0, temp.indexOf(eol) + 1);
            temp = temp.slice(Offensive_Specialist_Name[Military_Race].length);

            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[0] == 0) Military_Offensive_Units[0] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[1] == 1) Military_Offensive_Units[1] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[2] == 1) Military_Offensive_Units[2] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[3] == 1) Military_Offensive_Units[3] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[4] == 1) Military_Offensive_Units[4] = temp.slice(0, temp.indexOf(eol));

            temp = content.slice(content.indexOf(Defensive_Specialist_Name[Military_Race] + " ")); temp = temp.slice(0, temp.indexOf(eol) + 1);
            temp = temp.slice(Defensive_Specialist_Name[Military_Race].length);

            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[0] == 0) Military_Defensive_Units[0] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[1] == 1) Military_Defensive_Units[1] = 0;
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[2] == 1) Military_Defensive_Units[2] = 0;
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[3] == 1) Military_Defensive_Units[3] = 0;
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[4] == 1) Military_Defensive_Units[4] = 0;

            temp = content.slice(content.indexOf(Elite_Unit_Name[Military_Race] + " ")); temp = temp.slice(0, temp.indexOf(eol) + 1);
            temp = temp.slice(Elite_Unit_Name[Military_Race].length);

            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[0] == 0) Military_Elite_Units[0] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[1] == 1) Military_Elite_Units[1] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[2] == 1) Military_Elite_Units[2] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[3] == 1) Military_Elite_Units[3] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[4] == 1) Military_Elite_Units[4] = temp.slice(0, temp.indexOf(eol));

            temp = content.slice(content.indexOf("War Horses ")); temp = temp.slice(0, temp.indexOf(eol) + 1);
            temp = temp.slice(temp.indexOf(" ") + 1);

            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[0] == 0) Military_War_Horses[0] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[1] == 1) Military_War_Horses[1] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[2] == 1) Military_War_Horses[2] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[3] == 1) Military_War_Horses[3] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[4] == 1) Military_War_Horses[4] = temp.slice(0, temp.indexOf(eol));

            temp = content.slice(content.indexOf("Captured Land ")); temp = temp.slice(0, temp.indexOf(eol) + 1);
            temp = temp.slice(temp.indexOf(" ") + 1);

            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[0] == 0) Military_Captured_Land[0] = 0;
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[1] == 1) Military_Captured_Land[1] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[2] == 1) Military_Captured_Land[2] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[3] == 1) Military_Captured_Land[3] = temp.slice(0, temp.indexOf(" "));
            temp = temp.slice(temp.indexOf(" ") + 1); if (Military_General[4] == 1) Military_Captured_Land[4] = temp.slice(0, temp.indexOf(eol));


            for (i = 0; i < 5; i++) {

                Military_Soldiers[i] = Number(Military_Soldiers[i]);
                Military_Offensive_Units[i] = Number(Military_Offensive_Units[i]);
                Military_Defensive_Units[i] = Number(Military_Defensive_Units[i]);
                Military_Elite_Units[i] = Number(Military_Elite_Units[i]);
                Military_War_Horses[i] = Number(Military_War_Horses[i]);
                Military_Captured_Land[i] = Number(Military_Captured_Land[i]);

            }


            content = content.slice(content.indexOf("for the next 24 days.")); content = content.slice(content.indexOf(eol) + 1);

            content = content.slice(content.indexOf(eol) + 1);
            temp = content.slice(content.indexOf(Offensive_Specialist_Name[Military_Race] + " ")); temp = temp.slice(0, temp.indexOf(eol) + 1);
            temp = temp.slice(Offensive_Specialist_Name[Military_Race].length);
            for (i = 0; i < 24; i++) { temp = temp.slice(temp.indexOf(" ") + 1); Military_Training_Offensive_Units += Number(temp.slice(0, temp.indexOf(" "))); }

            content = content.slice(content.indexOf(eol) + 1);
            temp = content.slice(content.indexOf(Defensive_Specialist_Name[Military_Race] + " ")); temp = temp.slice(0, temp.indexOf(eol) + 1);
            temp = temp.slice(Defensive_Specialist_Name[Military_Race].length);
            for (i = 0; i < 24; i++) { temp = temp.slice(temp.indexOf(" ") + 1); Military_Training_Defensive_Units += Number(temp.slice(0, temp.indexOf(" "))); }

            content = content.slice(content.indexOf(eol) + 1);
            temp = content.slice(content.indexOf(Elite_Unit_Name[Military_Race] + " ")); temp = temp.slice(0, temp.indexOf(eol) + 1);
            temp = temp.slice(Elite_Unit_Name[Military_Race].length);
            for (i = 0; i < 24; i++) { temp = temp.slice(temp.indexOf(" ") + 1); Military_Training_Elite_Units += Number(temp.slice(0, temp.indexOf(" "))); }

            if (Military_SOM == 0) {

                content = content.slice(content.indexOf(eol) + 1);
                temp = content.slice(content.indexOf("Thieves ")); temp = temp.slice(0, temp.indexOf(eol) + 1);
                temp = temp.slice(7);
                for (i = 0; i < 24; i++) { temp = temp.slice(temp.indexOf(" ") + 1); Military_Training_Thieves += Number(temp.slice(0, temp.indexOf(" "))); }

            }


        }


        // *******************************************************
        //       Extended options for external sites - Start
        // *******************************************************

        var DataSaveForm = document.getElementById('SendData');
        for (i = 0; i < DataSaveForm.elements.length; i++) DataSaveForm.elements[i].value = "";

        for (i = 0; i < 5; i++) {

            DataSaveForm.elements.namedItem("Military_General_" + i.toString()).value = Military_General[i];
            DataSaveForm.elements.namedItem("Military_Soldiers_" + i.toString()).value = Military_Soldiers[i];
            DataSaveForm.elements.namedItem("Military_Offensive_Units_" + i.toString()).value = Military_Offensive_Units[i];
            DataSaveForm.elements.namedItem("Military_Defensive_Units_" + i.toString()).value = Military_Defensive_Units[i];
            DataSaveForm.elements.namedItem("Military_Elite_Units_" + i.toString()).value = Military_Elite_Units[i];
            DataSaveForm.elements.namedItem("Military_War_Horses_" + i.toString()).value = Military_War_Horses[i];
            DataSaveForm.elements.namedItem("Military_Captured_Land_" + i.toString()).value = Military_Captured_Land[i];
            DataSaveForm.elements.namedItem("Military_Time_" + i.toString()).value = Military_Time[i];

        }

        DataSaveForm.Military_Province_Name.value = Military_Province_Name;
        DataSaveForm.Military_Kingdom.value = Military_Kingdom;
        DataSaveForm.Military_Island.value = Military_Island;
        DataSaveForm.Military_Race.value = Military_Race;
        DataSaveForm.Military_Offensive_Points.value = Military_Offensive_Points;
        DataSaveForm.Military_Defensive_Points.value = Military_Defensive_Points;
        DataSaveForm.Military_Raw.value = Military_Raw;
        DataSaveForm.Military_Non_Peasants.value = Military_Non_Peasants;
        DataSaveForm.Military_MEO.value = Military_MEO;
        DataSaveForm.Military_MED.value = Military_MED;
        DataSaveForm.Military_Training_Off_Units.value = Military_Training_Offensive_Units;
        DataSaveForm.Military_Training_Def_Units.value = Military_Training_Defensive_Units;
        DataSaveForm.Military_Training_Elite.value = Military_Training_Elite_Units;
        DataSaveForm.Military_Training_Thieves.value = Military_Training_Thieves;
        DataSaveForm.Military_SOM.value = Military_SOM;

        // *******************************************************
        //       Extended options for external sites - End
        // *******************************************************


        Export_Line = "545048_" + ABC_Encryption(Military_Province_Name);

        Export_Line += "_" + Military_Kingdom;
        Export_Line += "_" + Military_Island;
        Export_Line += "_" + Military_Race;
        Export_Line += "_" + Military_Offensive_Points;
        Export_Line += "_" + Military_Defensive_Points;
        Export_Line += "_" + Math.round(Military_Raw * 10);
        Export_Line += "_" + Math.round(Military_Non_Peasants * 10);
        Export_Line += "_" + Math.round(Military_MEO * 10);
        Export_Line += "_" + Math.round(Military_MED * 10);

        for (i = 0; i < 5; i++) {

            Export_Line += "_" + Military_General[i];
            Export_Line += "_" + Military_Soldiers[i];
            Export_Line += "_" + Military_Offensive_Units[i];
            Export_Line += "_" + Military_Defensive_Units[i];
            Export_Line += "_" + Military_Elite_Units[i];
            Export_Line += "_" + Military_War_Horses[i];
            Export_Line += "_" + Military_Captured_Land[i];
            Export_Line += "_" + Math.round(Military_Time[i] * 10);

        }

        Export_Line += "_" + Military_Training_Offensive_Units;
        Export_Line += "_" + Military_Training_Defensive_Units;
        Export_Line += "_" + Military_Training_Elite_Units;
        Export_Line += "_" + Military_Training_Thieves;
        Export_Line += "_" + Military_SOM;


        if (Province_Name != Military_Province_Name) EnemyReset();






        // *************************************************
        //       Old Code Cb+Som With 2 Decimal Method
        // *************************************************
        // 
        //  function Max_CB_SOM(Home,Out,Real){
        //  for (i=-25; i<=25; i++) for (j=-25; j<=25; j++) if ( Math.round(Home/(1+i/100))+Math.round(Out/(1+j/100)) == Real ) return Math.round(Home/(1+i/100));
        //  return "";
        //  }
        // 
        //  if ( Province_Name == Military_Province_Name ) {
        //  document.Calculator.EnemySoldiers.value = Max_CB_SOM(Military_Soldiers[0],Math.round(Military_Soldiers[1]+Military_Soldiers[2]+Military_Soldiers[3]+Military_Soldiers[4]),document.Calculator.EnemySoldiers.value);
        //  document.Calculator.EnemyElites.value = Max_CB_SOM(Military_Elite_Units[0],Math.round(Military_Elite_Units[1]+Military_Elite_Units[2]+Military_Elite_Units[3]+Military_Elite_Units[4]),document.Calculator.EnemyElites.value);
        //  if ( document.getElementById('Ultima_Popups').checked == false ) document.getElementById('Ultima_floating_window0').style.display = "block";
        //  }
        //  
        // *************************************************
        //       Old Code Cb+Som With 2 Decimal Method
        // *************************************************




        // ***********************************************************
        //       Cb+Som With 3 % Range Method (REMOVED)
        // ***********************************************************
        // 
        // var CB_SOM_Summary = "";
        // 
        // function CB_SOM_Range(Range){
        // 
        // var Soldiers_At_Home = Military_Soldiers[0] ;
        // var Soldiers_Out     = Math.round( Military_Soldiers[1]+Military_Soldiers[2]+Military_Soldiers[3]+Military_Soldiers[4] ) ;
        // var Total_Soldiers   = Number( document.Calculator.EnemySoldiers.value ) ;
        // 
        // var Elites_At_Home   = Military_Elite_Units[0] ;
        // var Elites_Out       = Math.round( Military_Elite_Units[1]+Military_Elite_Units[2]+Military_Elite_Units[3]+Military_Elite_Units[4] ) ;
        // var Total_Elites     = Number( document.Calculator.EnemyElites.value ) ;
        // 
        // var Soldiers_Check1  = Math.max( (Soldiers_At_Home)/(1-Range/100)            , Total_Soldiers-(Soldiers_Out)/(1+Range/100)                    );
        // var Soldiers_Check2  = Math.max( Total_Soldiers-(Soldiers_Out)/(1-Range/100) , Total_Soldiers-(Total_Soldiers-Soldiers_At_Home)/(1+Range/100) );
        // 
        // var Elites_Check1    = Math.max( (Elites_At_Home)/(1-Range/100)              , Total_Elites-(Elites_Out)/(1+Range/100)                        );
        // var Elites_Check2    = Math.max( Total_Elites-(Elites_Out)/(1-Range/100)     , Total_Elites-(Total_Elites-Elites_At_Home)/(1+Range/100)       );
        // 
        // var CB_Soldiers = Math.min( Soldiers_Check1 , Soldiers_Check2 );
        // var CB_Elites   = Math.min( Elites_Check1   , Elites_Check2   );
        // 
        // var CB_Soldiers = Math.min( CB_Soldiers , Number( document.Calculator.EnemySoldiers.value ) );
        // var CB_Elites   = Math.min( CB_Elites   , Number( document.Calculator.EnemyElites.value   ) );
        // 
        // var CB_DefSpecs = Number( document.Calculator.EnemyDefSpecs.value ) ;
        // var CB_Raw_Defense = CB_Soldiers + CB_DefSpecs*Defensive_Specialist_Strength[Military_Race] + CB_Elites*Defensive_Elite_Unit_Strength[Military_Race] ;
        // var CB_DME = Number( document.Calculator.EnemyDME.value );
        // 
        // return Math.round( CB_Raw_Defense * CB_DME/100 ) ;
        // 
        // }
        // 
        // if ( Province_Name == Military_Province_Name ) {
        // 
        // Number( document.getElementById('EnemyModifiedDefense').innerHTML ); // Net Defensive Points at Home from CB
        // Military_Defensive_Points ; // Net Defensive Points at Home from SOM
        // CB_SOM_Range(25) ; // Net Defensive Points at Home from CB+SOM in the Range of 25%
        // CB_SOM_Range(3) ; // Net Defensive Points at Home from CB+SOM in the Range of 3%
        // 
        // CB_SOM_Summary = "<br /><br />"+c0+"** CB+SOM Summary **"+cc+
        // "<br />"+c1+"Net Defense at Home (CB): "+cc+c2+Number( document.getElementById('EnemyModifiedDefense').innerHTML )+cc+
        // "<br />"+c1+"Net Defense at Home (SOM): "+cc+c2+Military_Defensive_Points+cc+
        // "<br />"+c1+"Net Defense at Home (SOM -> 25% Range): "+cc+c2+Math.round( Military_Defensive_Points/0.75 )+cc+
        // "<br />"+c1+"Net Defense at Home (SOM -> 3% Range): "+cc+c2+Math.round( Military_Defensive_Points/0.97 )+cc+
        // "<br />"+c1+"Net Defense at Home (CB+SOM -> 25% Range): "+cc+c2+CB_SOM_Range(25)+cc+
        // "<br />"+c1+"Net Defense at Home (CB+SOM -> 3% Range): "+cc+c2+CB_SOM_Range(3)+cc+
        // "<br />"+c1+"Net Defense at Home (25% Range): "+cc+c2+Math.min( Math.round( Military_Defensive_Points/0.75 ) , CB_SOM_Range(25) )+cc+
        // "<br />"+c1+"Net Defense at Home (3% Range): "+cc+c2+Math.min( Math.round( Military_Defensive_Points/0.97 ) , CB_SOM_Range(3) )+cc;
        // 
        //if ( document.getElementById('Ultima_Popups').checked == false ) document.getElementById('Ultima_floating_window0').style.display = "block";
        // 
        // }
        // 
        // ***********************************************************
        //      Cb+Som With 3 % Range Method (REMOVED)
        // ***********************************************************




        // ***************************
        //       New Code Cb+Som      
        // ***************************

        var CB_SOM_Summary = "";

        var CB_SOM_Soldiers = Number(document.Calculator.EnemySoldiers.value);
        var CB_SOM_DefSpecs = Number(document.Calculator.EnemyDefSpecs.value);
        var CB_SOM_Elites = Number(document.Calculator.EnemyElites.value);
        var CB_SOM_DME = Number(document.Calculator.EnemyDME.value);

        var CB_SOM_Soldiers_Out = Math.round(Military_Soldiers[1] + Military_Soldiers[2] + Military_Soldiers[3] + Military_Soldiers[4]);
        var CB_SOM_Elites_Out = Math.round(Military_Elite_Units[1] + Military_Elite_Units[2] + Military_Elite_Units[3] + Military_Elite_Units[4]);

        CB_SOM_Soldiers = CB_SOM_Soldiers - CB_SOM_Soldiers_Out;
        CB_SOM_Elites = CB_SOM_Elites - CB_SOM_Elites_Out;

        var CB_SOM_Raw_Defense = CB_SOM_Soldiers + CB_SOM_DefSpecs * Defensive_Specialist_Strength[Military_Race] + CB_SOM_Elites * Defensive_Elite_Unit_Strength[Military_Race];

        if (Province_Name == Military_Province_Name) {

            CB_SOM_Summary = "<br /><br />" + c0 + "** CB+SOM Summary **" + cc +
"<br />" + c1 + "Net Defense at Home (CB): " + cc + c2 + Number(document.getElementById('EnemyModifiedDefense').innerHTML) + cc +
"<br />" + c1 + "Net Defense at Home (SOM): " + cc + c2 + Military_Defensive_Points + cc +
"<br />" + c1 + "Net Defense at Home (CB+SOM): " + cc + c2 + Math.round(CB_SOM_Raw_Defense * CB_SOM_DME / 100) + cc;

            document.Calculator.EnemySoldiers.value = CB_SOM_Soldiers;
            document.Calculator.EnemyElites.value = CB_SOM_Elites;

            if (document.getElementById('Ultima_Popups').checked == false) document.getElementById('Ultima_floating_window0').style.display = "block";

        }

        // ***************************
        //       New Code Cb+Som      
        // ***************************








        if (Military_SOM == 0) Military_Title = "Military Intelligence Formatted Report";
        if (Military_SOM == 1) Military_Title = "Military Intel on " + Military_Province_Name + " (" + Military_Kingdom + ":" + Military_Island + ")";


        for (i = 0; i < 5; i++) {

            Military_Soldiers_MEO[i] = Math.round(Military_Soldiers[i] * Military_MEO / 100);
            Military_Soldiers_MED[i] = Math.round(Military_Soldiers[i] * Military_MED / 100);

            Military_Offensive_Units_MEO[i] = Math.round(Military_Offensive_Units[i] * Offensive_Specialist_Strength[Military_Race] * Military_MEO / 100);
            Military_Defensive_Units_MED[i] = Math.round(Military_Defensive_Units[i] * Defensive_Specialist_Strength[Military_Race] * Military_MED / 100);
            Military_Elite_Units_MEO[i] = Math.round(Military_Elite_Units[i] * Offensive_Elite_Unit_Strength[Military_Race] * Military_MEO / 100);
            Military_Elite_Units_MED[i] = Math.round(Military_Elite_Units[i] * Defensive_Elite_Unit_Strength[Military_Race] * Military_MED / 100);

            Military_War_Horses_MEO[i] = Military_War_Horses[i];
            if (Military_War_Horses[i] > Military_Soldiers[i] + Military_Offensive_Units[i] + Military_Elite_Units[i]) Military_War_Horses_MEO[i] = Military_Soldiers[i] + Military_Offensive_Units[i] + Military_Elite_Units[i];
            Military_War_Horses_MEO[i] = Math.round(Military_War_Horses_MEO[i] * Military_MEO / 100);

            Military_Total_MEO[i] = Military_Soldiers_MEO[i] + Military_Offensive_Units_MEO[i] + Military_Elite_Units_MEO[i] + Military_War_Horses_MEO[i];
            Military_Total_MED[i] = Military_Soldiers_MED[i] + Military_Defensive_Units_MED[i] + Military_Elite_Units_MED[i];

        }

        Military_Total_Summary_MEO = Military_Total_MEO[0] + Military_Total_MEO[1] + Military_Total_MEO[2] + Military_Total_MEO[3] + Military_Total_MEO[4];
        Military_Total_Summary_MED = Military_Total_MED[0] + Military_Total_MED[1] + Military_Total_MED[2] + Military_Total_MED[3] + Military_Total_MED[4];

        Military_Total_Captured_Land = Military_Captured_Land[1] + Military_Captured_Land[2] + Military_Captured_Land[3] + Military_Captured_Land[4];

        Military_Training_Offensive_Units_MEO = Math.round(Military_Training_Offensive_Units * Offensive_Specialist_Strength[Military_Race] * Military_MEO / 100);
        Military_Training_Defensive_Units_MED = Math.round(Military_Training_Defensive_Units * Defensive_Specialist_Strength[Military_Race] * Military_MED / 100);
        Military_Training_Elite_Units_MEO = Math.round(Military_Training_Elite_Units * Offensive_Elite_Unit_Strength[Military_Race] * Military_MEO / 100);
        Military_Training_Elite_Units_MED = Math.round(Military_Training_Elite_Units * Defensive_Elite_Unit_Strength[Military_Race] * Military_MED / 100);


        for (i = 0; i < 5; i++) {

            Military_Soldiers_MEO[i] = Add_Commas(Military_Soldiers_MEO[i]);
            Military_Soldiers_MED[i] = Add_Commas(Military_Soldiers_MED[i]);
            Military_Offensive_Units_MEO[i] = Add_Commas(Military_Offensive_Units_MEO[i]);
            Military_Defensive_Units_MED[i] = Add_Commas(Military_Defensive_Units_MED[i]);
            Military_Elite_Units_MEO[i] = Add_Commas(Military_Elite_Units_MEO[i]);
            Military_Elite_Units_MED[i] = Add_Commas(Military_Elite_Units_MED[i]);
            Military_War_Horses_MEO[i] = Add_Commas(Military_War_Horses_MEO[i]);

        }

        Military_Offensive_Points = Add_Commas(Military_Offensive_Points);
        Military_Defensive_Points = Add_Commas(Military_Defensive_Points);
        Military_Total_Summary_MEO = Add_Commas(Military_Total_Summary_MEO);
        Military_Total_Summary_MED = Add_Commas(Military_Total_Summary_MED);
        Military_Total_Captured_Land = Add_Commas(Military_Total_Captured_Land);

        Military_Training_Offensive_Units = Add_Commas(Military_Training_Offensive_Units);
        Military_Training_Defensive_Units = Add_Commas(Military_Training_Defensive_Units);
        Military_Training_Elite_Units = Add_Commas(Military_Training_Elite_Units);
        Military_Training_Thieves = Add_Commas(Military_Training_Thieves);


        function Military_Army_Str(str, i) {

            if (i == 0) str = "<br /><br />" + c0 + "** Standing Army (At Home) **" + cc;
            if (i != 0) str = "<br /><br />" + c0 + "** Army " + str + " (Back in " + Military_Time[i] + " hours) **" + cc;

            if (Military_Soldiers[i] > "0") { Military_Soldiers[i] = Add_Commas(Military_Soldiers[i]); str += "<br />" + c1 + "Soldiers: " + cc + c2 + Military_Soldiers[i] + cc + c1 + " (" + Military_Soldiers_MEO[i] + " offense / " + Military_Soldiers_MED[i] + " defense)" + cc; }
            if (Military_Offensive_Units[i] > "0") { Military_Offensive_Units[i] = Add_Commas(Military_Offensive_Units[i]); str += "<br />" + c1 + Offensive_Specialist_Name[Military_Race] + ": " + cc + c2 + Military_Offensive_Units[i] + cc + c1 + " (" + Military_Offensive_Units_MEO[i] + " offense)" + cc; }
            if (Military_Defensive_Units[i] > "0") { Military_Defensive_Units[i] = Add_Commas(Military_Defensive_Units[i]); str += "<br />" + c1 + Defensive_Specialist_Name[Military_Race] + ": " + cc + c2 + Military_Defensive_Units[i] + cc + c1 + " (" + Military_Defensive_Units_MED[i] + " defense)" + cc; }
            if (Military_Elite_Units[i] > "0") { Military_Elite_Units[i] = Add_Commas(Military_Elite_Units[i]); str += "<br />" + c1 + Elite_Unit_Name[Military_Race] + ": " + cc + c2 + Military_Elite_Units[i] + cc + c1 + " (" + Military_Elite_Units_MEO[i] + " offense / " + Military_Elite_Units_MED[i] + " defense)" + cc; }
            if (Military_War_Horses[i] > "0") { Military_War_Horses[i] = Add_Commas(Military_War_Horses[i]); str += "<br />" + c1 + "War-Horses: " + cc + c2 + Military_War_Horses[i] + cc + c1 + " (up to " + Military_War_Horses_MEO[i] + " additional offense)" + cc; }
            if (Military_Total_MEO[i] > "0") { Military_Total_MEO[i] = Add_Commas(Military_Total_MEO[i]); str += "<br />" + c1 + "Total Attack points: " + cc + c2 + Military_Total_MEO[i] + cc; }
            if (Military_Total_MED[i] > "0") { Military_Total_MED[i] = Add_Commas(Military_Total_MED[i]); str += "<br />" + c1 + "Total Defense points: " + cc + c2 + Military_Total_MED[i] + cc; }
            if (Military_Captured_Land[i] > "0") { Military_Captured_Land[i] = Add_Commas(Military_Captured_Land[i]); str += "<br />" + c1 + "Captured Land: " + cc + c2 + Military_Captured_Land[i] + cc + c1 + " Acres" + cc; }

            return str;

        }

        var Military_Army_String = new Array("", "", "", "", "");

        Military_Army_String[0] = Military_Army_Str("", 0);

        if ((Military_General[1] == 1) && (Military_Total_MEO[1] != "0")) Military_Army_String[1] = Military_Army_Str("#2", 1);
        if ((Military_General[1] == 1) && (Military_Total_MEO[1] != "0") && (Military_General[2] == 1) && (Military_Total_MEO[2] == "0")) Military_Army_String[1] = Military_Army_Str("#2/#3", 1);
        if ((Military_General[1] == 1) && (Military_Total_MEO[1] != "0") && (Military_General[2] == 1) && (Military_Total_MEO[2] == "0") && (Military_General[3] == 1) && (Military_Total_MEO[3] == "0")) Military_Army_String[1] = Military_Army_Str("#2/#3/#4", 1);
        if ((Military_General[1] == 1) && (Military_Total_MEO[1] != "0") && (Military_General[2] == 1) && (Military_Total_MEO[2] == "0") && (Military_General[3] == 1) && (Military_Total_MEO[3] == "0") && (Military_General[4] == 1) && (Military_Total_MEO[4] == "0")) Military_Army_String[1] = Military_Army_Str("#2/#3/#4/#5", 1);

        if ((Military_General[2] == 1) && (Military_Total_MEO[2] != "0")) Military_Army_String[2] = Military_Army_Str("#3", 2);
        if ((Military_General[2] == 1) && (Military_Total_MEO[2] != "0") && (Military_General[3] == 1) && (Military_Total_MEO[3] == "0")) Military_Army_String[2] = Military_Army_Str("#3/#4", 2);
        if ((Military_General[2] == 1) && (Military_Total_MEO[2] != "0") && (Military_General[3] == 1) && (Military_Total_MEO[3] == "0") && (Military_General[4] == 1) && (Military_Total_MEO[4] == "0")) Military_Army_String[2] = Military_Army_Str("#3/#4/#5", 2);

        if ((Military_General[3] == 1) && (Military_Total_MEO[3] != "0")) Military_Army_String[3] = Military_Army_Str("#4", 3);
        if ((Military_General[3] == 1) && (Military_Total_MEO[3] != "0") && (Military_General[4] == 1) && (Military_Total_MEO[4] == "0")) Military_Army_String[3] = Military_Army_Str("#4/#5", 3);

        if ((Military_General[4] == 1) && (Military_Total_MEO[4] != "0")) Military_Army_String[4] = Military_Army_Str("#5", 4);


        textonly = c0 + Military_Title + cc + Copyrights +

"<br />" + c1 + "Race: " + cc + c2 + Race_Name[Military_Race] + cc +
"<br /><br />" + c0 + "** Summary **" + cc +
"<br />" + c1 + "Non-Peasants: " + cc + c2 + Military_Non_Peasants + cc + c1 + "% " + cc +
"<br />" + c1 + "Efficiency (SoM): " + cc + c2 + Military_MEO + cc + c1 + "% off., " + cc + c2 + Military_MED + cc + c1 + "% def., " + cc + c2 + Military_Raw + cc + c1 + "% raw" + cc +
"<br />" + c1 + "Net Offense at Home (from Utopia): " + cc + c2 + Military_Offensive_Points + cc +
"<br />" + c1 + "Net Defense at Home (from Utopia): " + cc + c2 + Military_Defensive_Points + cc +
"<br />" + c1 + "Modified Attack points (calculated): " + cc + c2 + Military_Total_Summary_MEO + cc +
"<br />" + c1 + "Modified Defense points (calculated): " + cc + c2 + Military_Total_Summary_MED + cc;

        textonly += CB_SOM_Summary;

        if (Military_Total_Captured_Land != "0") textonly += "<br />" + c1 + "Total Captured Land: " + cc + c2 + Military_Total_Captured_Land + cc;


        textonly += Military_Army_String[0] + Military_Army_String[1] + Military_Army_String[2] + Military_Army_String[3] + Military_Army_String[4];

        if ((Military_Training_Offensive_Units != "0") || (Military_Training_Defensive_Units != "0") || (Military_Training_Elite_Units != "0") || (Military_Training_Thieves != "0")) textonly += "<br /><br />" + c0 + "** Troops in Training **" + cc;
        if (Military_Training_Offensive_Units != "0") textonly += "<br />" + c1 + Offensive_Specialist_Name[Military_Race] + ": " + cc + c2 + Military_Training_Offensive_Units + cc + c1 + " (" + Military_Training_Offensive_Units_MEO + " offense)" + cc;
        if (Military_Training_Defensive_Units != "0") textonly += "<br />" + c1 + Defensive_Specialist_Name[Military_Race] + ": " + cc + c2 + Military_Training_Defensive_Units + cc + c1 + " (" + Military_Training_Defensive_Units_MED + " defense)" + cc;
        if (Military_Training_Elite_Units != "0") textonly += "<br />" + c1 + Elite_Unit_Name[Military_Race] + ": " + cc + c2 + Military_Training_Elite_Units + cc + c1 + " (" + Military_Training_Elite_Units_MEO + " offense/" + Military_Training_Elite_Units_MED + " defense)" + cc;
        if (Military_Training_Thieves != "0") textonly += "<br />" + c1 + "Thieves: " + cc + c2 + Military_Training_Thieves + cc;


        textonly += "<br /><br />" + c0 + "** Export Line **" + cc + "<br />";

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
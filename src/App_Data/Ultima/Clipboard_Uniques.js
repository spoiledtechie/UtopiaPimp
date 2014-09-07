function Clipboard_Uniques() {


    if (
(content.indexOf(") captured") != -1) ||
(content.indexOf(") and razed") != -1) ||
(content.indexOf("), captured") != -1) ||
(content.indexOf(") recaptured") != -1) ||
(content.indexOf(") invaded and pillaged") != -1) ||
(content.indexOf(") invaded and stole") != -1) ||
(content.indexOf(") killed") != -1) ||
(content.indexOf(") attempted an invasion") != -1) ||
(content.indexOf("), invaded") != -1) ||
(content.indexOf(") and captured") != -1) ||
(content.indexOf(") razed") != -1) ||
(content.indexOf(") ambushed") != -1) ||
(content.indexOf(") attacked and pillaged") != -1) ||
(content.indexOf(") attacked and stole") != -1) ||
(content.indexOf(") and killed") != -1) ||
(content.indexOf(") attempted to invade") != -1)) {

        if (document.getElementById('Ultima_Popups').checked == false) document.getElementById('Ultima_floating_window0').style.display = "block";

        var Uniques_Time = 6; // Minimum_Time
        var Uniques_Side = 0;
        var Uniques_Month = 0;
        var Uniques_Day = 0;
        var Uniques_Year = 0;
        var Uniques_Date = 0;
        var Uniques_Acre = 0;
        var Uniques_Name = "";
        var Uniques_Nam2 = "";
        var Uniques_Loca = "";
        var Uniques_Loc2 = "";
        var Uniques_Color = new Array("#FFFFFF");

        var Uniques_KD_Name = new Array(); var Uniques_Enemy_Name = new Array();
        var Uniques_KD_Time = new Array(); var Uniques_Enemy_Time = new Array();

        var Uniques_KD_Tim2 = new Array(); var Uniques_Enemy_Tim2 = new Array();

        var Uniques_KD_Type = new Array(); var Uniques_Enemy_Type = new Array();
        var Uniques_KD_Unis = new Array(); var Uniques_Enemy_Unis = new Array();
        var Uniques_KD_Acre = new Array(); var Uniques_Enemy_Acre = new Array();
        var Uniques_KD_Acr2 = new Array(); var Uniques_Enemy_Acr2 = new Array();
        var Uniques_KD_Loca = 0; var Uniques_Enemy_Loca = new Array();
        var Uniques_KD_Aid = 0; var Uniques_Enemy_Sort = new Array();
        var Uniques_KD_Total_Uniques = 0; var Uniques_Enemy_Total_Uniques = 0;
        var Uniques_KD_Total_Attacks = 0; var Uniques_Enemy_Total_Attacks = 0;
        var Uniques_KD_Total_Gain = 0; var Uniques_Enemy_Total_Gain = 0;
        var Uniques_KD_Total_Lost = 0; var Uniques_Enemy_Total_Lost = 0;
        var Uniques_KD_Unknown_Acre = 0; var Uniques_Enemy_Unknown_Acre = 0;

        var Uniques_KD_Traditional = 0; var Uniques_Enemy_Traditional = 0; Uniques_Color[1] = "#88BB00"; // Traditional March
        var Uniques_KD_Raze = 0; var Uniques_Enemy_Raze = 0; Uniques_Color[2] = "#448800"; // Raze
        var Uniques_KD_Conquest = 0; var Uniques_Enemy_Conquest = 0; Uniques_Color[3] = "#006600"; // Conquest
        var Uniques_KD_Ambush = 0; var Uniques_Enemy_Ambush = 0; Uniques_Color[4] = "#BB8888"; // Ambush
        var Uniques_KD_Plunder = 0; var Uniques_Enemy_Plunder = 0; Uniques_Color[5] = "#FF6600"; // Plunder
        var Uniques_KD_Learn = 0; var Uniques_Enemy_Learn = 0; Uniques_Color[6] = "#FF8888"; // Learn
        var Uniques_KD_Massacre = 0; var Uniques_Enemy_Massacre = 0; Uniques_Color[7] = "#A7868F"; // Massacre
        var Uniques_KD_Fail = 0; var Uniques_Enemy_Fail = 0; Uniques_Color[8] = "#444444"; // Fail
        var Uniques_KD_Unknown = 0; var Uniques_Enemy_Unknown = 0; Uniques_Color[9] = "#888888"; // Unknown
        var Uniques_KD_Intra = 0; Uniques_Color[10] = "#888888"; // Intra

        var Uniques_Date_Start = 0;
        var Uniques_Date_End = 0;

        content = content + eol;

        while (content.indexOf(eol) != -1) {

            if (content.charAt(0) == " ") content = content.slice(1);
            if (content.charAt(0) == " ") content = content.slice(1);
            if (content.charAt(0) == " ") content = content.slice(1);
            if (content.charAt(0) == " ") content = content.slice(1);
            if (content.charAt(0) == " ") content = content.slice(1);


            temp = content.slice(0, content.indexOf(eol));

            Uniques_Month = temp.slice(0, temp.indexOf(" ")); temp = temp.slice(temp.indexOf(" ") + 1);
            Uniques_Day = temp.slice(0, temp.indexOf(" ")); temp = temp.slice(temp.indexOf(" ") + 1); temp = temp.slice(temp.indexOf(" ") + 1);
            Uniques_Year = temp.slice(2, temp.indexOf(" ")); temp = temp.slice(temp.indexOf(" ") + 1);

            for (t = 0; t < 7; t++) if (Uniques_Month == Months[t]) Uniques_Month = t + 1;

            Uniques_Month = Number(Uniques_Month); // 1..7
            Uniques_Day = Number(Uniques_Day); // 1..24
            Uniques_Year = Number(Uniques_Year); // 0..??
            Uniques_Date = (Uniques_Year * 7 + Uniques_Month - 1) * 24 + Uniques_Day - 1;

            Uniques_Name = temp.slice(0, temp.indexOf(")") + 1);

            if (Uniques_Name.indexOf(" (") != -1) Uniques_Name = Uniques_Name.replace(" (", "(");

            Uniques_Loca = temp.slice(temp.indexOf("("), temp.indexOf(")") + 1);
            Uniques_Type = 0;
            Uniques_Side = 0;

            if ((temp.indexOf("In local kingdom strife") == -1) && (temp.indexOf("In intra-kingdom war") == -1)) {

                if (temp.indexOf(") captured") != -1) { Uniques_Side = 1; Uniques_Type = 1; Uniques_KD_Traditional++; }
                if (temp.indexOf(") and razed") != -1) { Uniques_Side = 1; Uniques_Type = 2; Uniques_KD_Raze++; }
                if (temp.indexOf("), captured") != -1) { Uniques_Side = 1; Uniques_Type = 3; Uniques_KD_Conquest++; }
                if (temp.indexOf(") recaptured") != -1) { Uniques_Side = 1; Uniques_Type = 4; Uniques_KD_Ambush++; }
                if (temp.indexOf(") invaded and pillaged") != -1) { Uniques_Side = 1; Uniques_Type = 5; Uniques_KD_Plunder++; }
                if (temp.indexOf(") invaded and stole") != -1) { Uniques_Side = 1; Uniques_Type = 6; Uniques_KD_Learn++; }
                if (temp.indexOf(") killed") != -1) { Uniques_Side = 1; Uniques_Type = 7; Uniques_KD_Massacre++; }
                if (temp.indexOf(") attempted an invasion") != -1) { Uniques_Side = 1; Uniques_Type = 8; Uniques_KD_Fail++; }

                if (temp.indexOf("), invaded") != -1) { Uniques_Side = 2; Uniques_Type = 3; Uniques_Enemy_Conquest++; } else {
                    if (temp.indexOf(") and captured") != -1) { Uniques_Side = 2; Uniques_Type = 1; Uniques_Enemy_Traditional++; } 
                }
                if (temp.indexOf(") razed") != -1) { Uniques_Side = 2; Uniques_Type = 2; Uniques_Enemy_Raze++; }
                if (temp.indexOf(") ambushed") != -1) { Uniques_Side = 2; Uniques_Type = 4; Uniques_Enemy_Ambush++; }
                if (temp.indexOf(") attacked and pillaged") != -1) { Uniques_Side = 2; Uniques_Type = 5; Uniques_Enemy_Plunder++; }
                if (temp.indexOf(") attacked and stole") != -1) { Uniques_Side = 2; Uniques_Type = 6; Uniques_Enemy_Learn++; }
                if (temp.indexOf(") and killed") != -1) { Uniques_Side = 2; Uniques_Type = 7; Uniques_Enemy_Massacre++; }
                if (temp.indexOf(") attempted to invade") != -1) { Uniques_Side = 2; Uniques_Type = 8; Uniques_Enemy_Fail++; }

            } else { Uniques_Type = 0; Uniques_Side = 1; Uniques_KD_Intra++; }

            if (Uniques_Side == 1) Uniques_KD_Loca = Uniques_Loca;

            if (Uniques_Date_Start == 0) if (Uniques_Side != 0) Uniques_Date_Start = Uniques_Date;

            if (temp.indexOf("has sent an aid shipment") != -1) Uniques_KD_Aid++;

            if ((temp.indexOf("An unknown province from") != -1) && (Uniques_Side == 1)) { Uniques_Type = 9; Uniques_KD_Unknown++; }
            if ((temp.indexOf("An unknown province from") != -1) && (Uniques_Side == 2)) { Uniques_Type = 9; Uniques_Enemy_Unknown++; }

            Uniques_Acre = 0;
            Uniques_Loc2 = 0;
            Uniques_Nam2 = "????";

            if ((temp.indexOf("In local kingdom strife") == -1) && (temp.indexOf("In intra-kingdom war") == -1)) {

                if (temp.indexOf(") captured ") != -1) { temp2 = temp.slice(temp.indexOf(") captured ") + 11); Uniques_Acre = Number(temp2.slice(0, temp2.indexOf(" "))); Uniques_Nam2 = temp2.slice(temp2.indexOf("from ") + 5, temp2.indexOf(")") + 1); }
                if (temp.indexOf(") and razed ") != -1) { temp2 = temp.slice(temp.indexOf(") and razed ") + 12); Uniques_Acre = Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp.slice(temp.indexOf(") invaded ") + 10); Uniques_Nam2 = temp2.slice(0, temp2.indexOf(")") + 1); }
                if (temp.indexOf("), captured ") != -1) { temp2 = temp.slice(temp.indexOf("), captured ") + 12); Uniques_Acre = Number(temp2.slice(0, temp2.indexOf(" "))); Uniques_Nam2 = temp2.slice(temp2.indexOf("from ") + 5, temp2.indexOf(")") + 1); }
                if (temp.indexOf(") recaptured ") != -1) { temp2 = temp.slice(temp.indexOf(") recaptured ") + 13); Uniques_Acre = Number(temp2.slice(0, temp2.indexOf(" "))); Uniques_Nam2 = temp2.slice(temp2.indexOf("from ") + 5, temp2.indexOf(")") + 1); }
                if (temp.indexOf("), invaded ") != -1) { temp2 = temp.slice(temp.indexOf(") and captured ") + 15); Uniques_Acre = Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp.slice(temp.indexOf("), invaded ") + 11); Uniques_Nam2 = temp2.slice(0, temp2.indexOf(")") + 1); } else {
                    if (temp.indexOf(") and captured ") != -1) { temp2 = temp.slice(temp.indexOf(") and captured ") + 15); Uniques_Acre = Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp.slice(temp.indexOf(") invaded ") + 10); Uniques_Nam2 = temp2.slice(0, temp2.indexOf(")") + 1); } 
                }
                if (temp.indexOf(") razed ") != -1) { temp2 = temp.slice(temp.indexOf(") razed ") + 8); Uniques_Acre = Number(temp2.slice(0, temp2.indexOf(" "))); Uniques_Nam2 = temp2.slice(temp2.indexOf("of ") + 3, temp2.indexOf(")") + 1); }
                if (temp.indexOf(") ambushed ") != -1) { temp2 = temp.slice(temp.indexOf(") and took ") + 11); Uniques_Acre = Number(temp2.slice(0, temp2.indexOf(" "))); temp2 = temp.slice(temp.indexOf(") ambushed armies from ") + 23); Uniques_Nam2 = temp2.slice(0, temp2.indexOf(")") + 1); }

                if (Uniques_Nam2.indexOf(" (") != -1) Uniques_Nam2 = Uniques_Nam2.replace(" (", "(");

                if ((temp.indexOf("An unknown province from") != -1) && (Uniques_Side == 1)) { Uniques_KD_Unknown_Acre += Uniques_Acre; }
                if ((temp.indexOf("An unknown province from") != -1) && (Uniques_Side == 2)) { Uniques_Enemy_Unknown_Acre += Uniques_Acre; }

                if (Uniques_Side == 1) { Uniques_KD_Total_Gain += Uniques_Acre; }
                if (Uniques_Side == 2) { Uniques_Enemy_Total_Gain += Uniques_Acre; }

            }



            if ((Uniques_Type != 0) && (Uniques_Side == 1)) {
                Uniques_Check = 0;

                for (i = 0; i < Uniques_KD_Name.length; i++) if (Uniques_Name == Uniques_KD_Name[i]) {
                    Uniques_KD_Time[i][0]++; Uniques_KD_Time[i][Uniques_KD_Time[i][0]] = Uniques_Date;
                    Uniques_KD_Type[i][0]++; Uniques_KD_Type[i][Uniques_KD_Time[i][0]] = Uniques_Type;
                    Uniques_KD_Acre[i] += Uniques_Acre;
                    Uniques_Check = 1;
                }

                if (Uniques_Check == 0) {
                    Uniques_KD_Time[Uniques_KD_Name.length] = new Array();
                    Uniques_KD_Time[Uniques_KD_Name.length][0] = 1;
                    Uniques_KD_Time[Uniques_KD_Name.length][1] = Uniques_Date;
                    Uniques_KD_Type[Uniques_KD_Name.length] = new Array();
                    Uniques_KD_Type[Uniques_KD_Name.length][0] = 1;
                    Uniques_KD_Type[Uniques_KD_Name.length][1] = Uniques_Type;
                    Uniques_KD_Acre[Uniques_KD_Name.length] = Uniques_Acre;
                    Uniques_KD_Acr2[Uniques_KD_Name.length] = 0;
                    Uniques_KD_Tim2[Uniques_KD_Name.length] = 0;
                    Uniques_KD_Name[Uniques_KD_Name.length] = Uniques_Name;
                }

            }




            if ((Uniques_Type != 0) && (Uniques_Side == 2)) {
                Uniques_Check = 0;

                for (i = 0; i < Uniques_Enemy_Name.length; i++) if (Uniques_Name == Uniques_Enemy_Name[i]) {
                    Uniques_Enemy_Time[i][0]++; Uniques_Enemy_Time[i][Uniques_Enemy_Time[i][0]] = Uniques_Date;
                    Uniques_Enemy_Type[i][0]++; Uniques_Enemy_Type[i][Uniques_Enemy_Time[i][0]] = Uniques_Type;
                    Uniques_Enemy_Acre[i] += Uniques_Acre;
                    Uniques_Check = 1;
                }

                if (Uniques_Check == 0) {
                    Uniques_Enemy_Time[Uniques_Enemy_Name.length] = new Array();
                    Uniques_Enemy_Time[Uniques_Enemy_Name.length][0] = 1;
                    Uniques_Enemy_Time[Uniques_Enemy_Name.length][1] = Uniques_Date;
                    Uniques_Enemy_Type[Uniques_Enemy_Name.length] = new Array();
                    Uniques_Enemy_Type[Uniques_Enemy_Name.length][0] = 1;
                    Uniques_Enemy_Type[Uniques_Enemy_Name.length][1] = Uniques_Type;
                    Uniques_Enemy_Loca[Uniques_Enemy_Name.length] = Uniques_Loca;
                    Uniques_Enemy_Acre[Uniques_Enemy_Name.length] = Uniques_Acre;
                    Uniques_Enemy_Acr2[Uniques_Enemy_Name.length] = 0;
                    Uniques_Enemy_Tim2[Uniques_Enemy_Name.length] = 0;
                    Uniques_Enemy_Name[Uniques_Enemy_Name.length] = Uniques_Name;
                }

            }







            if (Uniques_Nam2 != "????") {
                Uniques_Loc2 = Uniques_Nam2.slice(Uniques_Nam2.indexOf("("), Uniques_Nam2.indexOf(")") + 1);

                if (Uniques_Side == 2) {
                    Uniques_Check = 0;
                    for (i = 0; i < Uniques_KD_Name.length; i++) if (Uniques_Nam2 == Uniques_KD_Name[i]) {
                        Uniques_KD_Acr2[i] += Uniques_Acre;
                        Uniques_KD_Tim2[i]++;
                        Uniques_Check = 1;
                    }

                    if (Uniques_Check == 0) {
                        Uniques_KD_Time[Uniques_KD_Name.length] = new Array(); Uniques_KD_Time[Uniques_KD_Name.length][0] = 0;
                        Uniques_KD_Type[Uniques_KD_Name.length] = new Array(); Uniques_KD_Type[Uniques_KD_Name.length][0] = 0;
                        Uniques_KD_Loca[Uniques_KD_Name.length] = Uniques_Loc2;
                        Uniques_KD_Acre[Uniques_KD_Name.length] = 0;
                        Uniques_KD_Acr2[Uniques_KD_Name.length] = Uniques_Acre;
                        Uniques_KD_Tim2[Uniques_KD_Name.length] = 1;
                        Uniques_KD_Name[Uniques_KD_Name.length] = Uniques_Nam2;
                    }

                    Uniques_KD_Total_Lost += Uniques_Acre;

                }


                if (Uniques_Side == 1) {
                    Uniques_Check = 0;
                    for (i = 0; i < Uniques_Enemy_Name.length; i++) if (Uniques_Nam2 == Uniques_Enemy_Name[i]) {
                        Uniques_Enemy_Acr2[i] += Uniques_Acre;
                        Uniques_Enemy_Tim2[i]++;
                        Uniques_Check = 1;
                    }

                    if (Uniques_Check == 0) {
                        Uniques_Enemy_Time[Uniques_Enemy_Name.length] = new Array(); Uniques_Enemy_Time[Uniques_Enemy_Name.length][0] = 0;
                        Uniques_Enemy_Type[Uniques_Enemy_Name.length] = new Array(); Uniques_Enemy_Type[Uniques_Enemy_Name.length][0] = 0;
                        Uniques_Enemy_Loca[Uniques_Enemy_Name.length] = Uniques_Loc2;
                        Uniques_Enemy_Acre[Uniques_Enemy_Name.length] = 0;
                        Uniques_Enemy_Acr2[Uniques_Enemy_Name.length] = Uniques_Acre;
                        Uniques_Enemy_Tim2[Uniques_Enemy_Name.length] = 1;
                        Uniques_Enemy_Name[Uniques_Enemy_Name.length] = Uniques_Nam2;
                    }

                    Uniques_Enemy_Total_Lost += Uniques_Acre;

                }

            }


            if (Uniques_Side != 0) Uniques_Date_End = Uniques_Date;


            content = content.slice(content.indexOf(eol) + 1);

        }






        for (i = 0; i < Uniques_KD_Name.length; i++) {
            Uniques_KD_Unis[i] = 1; temp = 0;
            if (Uniques_KD_Time[i][0] == 0) Uniques_KD_Unis[i] = 0;
            if (Uniques_KD_Time[i][0] > 1) for (t = 1; t <= Uniques_KD_Time[i][0]; t++) {
                if (Uniques_KD_Time[i][0] == 1) Uniques_KD_Unis[i] = 1;
                if (Uniques_KD_Time[i][0] > 1) {
                    if (t < Uniques_KD_Time[i][0]) {
                        if (Uniques_Time > Uniques_KD_Time[i][t + 1] - Uniques_KD_Time[i][t]) temp++;
                        if ((Uniques_Time <= Uniques_KD_Time[i][t + 1] - Uniques_KD_Time[i][t]) || (temp > 3)) { temp = 0; Uniques_KD_Unis[i]++; }
                    } else { if (temp > 4) { temp = 0; Uniques_KD_Unis[i]++; } } 
                } 
            } 
        }


        for (i = 0; i < Uniques_Enemy_Name.length; i++) {
            Uniques_Enemy_Unis[i] = 1; temp = 0;
            if (Uniques_Enemy_Time[i][0] == 0) Uniques_Enemy_Unis[i] = 0;
            if (Uniques_Enemy_Time[i][0] > 1) for (t = 1; t <= Uniques_Enemy_Time[i][0]; t++) {
                if (Uniques_Enemy_Time[i][0] == 1) Uniques_Enemy_Unis[i] = 1;
                if (Uniques_Enemy_Time[i][0] > 1) {
                    if (t < Uniques_Enemy_Time[i][0]) {
                        if (Uniques_Time > Uniques_Enemy_Time[i][t + 1] - Uniques_Enemy_Time[i][t]) temp++;
                        if ((Uniques_Time <= Uniques_Enemy_Time[i][t + 1] - Uniques_Enemy_Time[i][t]) || (temp > 3)) { temp = 0; Uniques_Enemy_Unis[i]++; }
                    } else { if (temp > 4) { temp = 0; Uniques_Enemy_Unis[i]++; } } 
                } 
            } 
        }


        for (i = 0; i < Uniques_KD_Name.length; i++) Uniques_KD_Total_Attacks += Uniques_KD_Time[i][0];
        for (i = 0; i < Uniques_KD_Name.length; i++) Uniques_KD_Total_Uniques += Uniques_KD_Unis[i];
        for (i = 0; i < Uniques_Enemy_Name.length; i++) Uniques_Enemy_Total_Attacks += Uniques_Enemy_Time[i][0];
        for (i = 0; i < Uniques_Enemy_Name.length; i++) Uniques_Enemy_Total_Uniques += Uniques_Enemy_Unis[i];







        var Temp_Array1;
        var Temp_Array2;

        for (i = 0; i < Uniques_KD_Unis.length; i++) for (t = 1; t < Uniques_KD_Unis.length; t++) if (Uniques_KD_Unis[t - 1] < Uniques_KD_Unis[t]) {
            Temp_Array1 = Uniques_KD_Name[t - 1]; Temp_Array2 = Uniques_KD_Name[t]; Uniques_KD_Name[t - 1] = Temp_Array2; Uniques_KD_Name[t] = Temp_Array1;
            Temp_Array1 = Uniques_KD_Time[t - 1]; Temp_Array2 = Uniques_KD_Time[t]; Uniques_KD_Time[t - 1] = Temp_Array2; Uniques_KD_Time[t] = Temp_Array1;
            Temp_Array1 = Uniques_KD_Tim2[t - 1]; Temp_Array2 = Uniques_KD_Tim2[t]; Uniques_KD_Tim2[t - 1] = Temp_Array2; Uniques_KD_Tim2[t] = Temp_Array1;
            Temp_Array1 = Uniques_KD_Type[t - 1]; Temp_Array2 = Uniques_KD_Type[t]; Uniques_KD_Type[t - 1] = Temp_Array2; Uniques_KD_Type[t] = Temp_Array1;
            Temp_Array1 = Uniques_KD_Unis[t - 1]; Temp_Array2 = Uniques_KD_Unis[t]; Uniques_KD_Unis[t - 1] = Temp_Array2; Uniques_KD_Unis[t] = Temp_Array1;
            Temp_Array1 = Uniques_KD_Acre[t - 1]; Temp_Array2 = Uniques_KD_Acre[t]; Uniques_KD_Acre[t - 1] = Temp_Array2; Uniques_KD_Acre[t] = Temp_Array1;
            Temp_Array1 = Uniques_KD_Acr2[t - 1]; Temp_Array2 = Uniques_KD_Acr2[t]; Uniques_KD_Acr2[t - 1] = Temp_Array2; Uniques_KD_Acr2[t] = Temp_Array1;
        }

        for (i = 0; i < Uniques_Enemy_Unis.length; i++) for (t = 1; t < Uniques_Enemy_Unis.length; t++) if (Uniques_Enemy_Unis[t - 1] < Uniques_Enemy_Unis[t]) {
            Temp_Array1 = Uniques_Enemy_Name[t - 1]; Temp_Array2 = Uniques_Enemy_Name[t]; Uniques_Enemy_Name[t - 1] = Temp_Array2; Uniques_Enemy_Name[t] = Temp_Array1;
            Temp_Array1 = Uniques_Enemy_Time[t - 1]; Temp_Array2 = Uniques_Enemy_Time[t]; Uniques_Enemy_Time[t - 1] = Temp_Array2; Uniques_Enemy_Time[t] = Temp_Array1;
            Temp_Array1 = Uniques_Enemy_Tim2[t - 1]; Temp_Array2 = Uniques_Enemy_Tim2[t]; Uniques_Enemy_Tim2[t - 1] = Temp_Array2; Uniques_Enemy_Tim2[t] = Temp_Array1;
            Temp_Array1 = Uniques_Enemy_Type[t - 1]; Temp_Array2 = Uniques_Enemy_Type[t]; Uniques_Enemy_Type[t - 1] = Temp_Array2; Uniques_Enemy_Type[t] = Temp_Array1;
            Temp_Array1 = Uniques_Enemy_Unis[t - 1]; Temp_Array2 = Uniques_Enemy_Unis[t]; Uniques_Enemy_Unis[t - 1] = Temp_Array2; Uniques_Enemy_Unis[t] = Temp_Array1;
            Temp_Array1 = Uniques_Enemy_Acre[t - 1]; Temp_Array2 = Uniques_Enemy_Acre[t]; Uniques_Enemy_Acre[t - 1] = Temp_Array2; Uniques_Enemy_Acre[t] = Temp_Array1;
            Temp_Array1 = Uniques_Enemy_Acr2[t - 1]; Temp_Array2 = Uniques_Enemy_Acr2[t]; Uniques_Enemy_Acr2[t - 1] = Temp_Array2; Uniques_Enemy_Acr2[t] = Temp_Array1;
            Temp_Array1 = Uniques_Enemy_Loca[t - 1]; Temp_Array2 = Uniques_Enemy_Loca[t]; Uniques_Enemy_Loca[t - 1] = Temp_Array2; Uniques_Enemy_Loca[t] = Temp_Array1;
        }



        for (t = 0; t < Uniques_Enemy_Loca.length; t++) {
            Uniques_Check = 0;
            for (i = 0; i < Uniques_Enemy_Sort.length; i++) if (Uniques_Enemy_Loca[t] == Uniques_Enemy_Sort[i]) Uniques_Check = 1;
            if (Uniques_Check == 0) Uniques_Enemy_Sort[Uniques_Enemy_Sort.length] = Uniques_Enemy_Loca[t];
        }




        textonly = c0 + "Kingdom Unique Attacks for " + Uniques_KD_Loca + cc + Copyrights;

        textonly += "<br />" + c1 + "Uniques Report form day " + cc + c2 + Uniques_Date_Start + cc + c1 + " to day " + cc + c2 + Uniques_Date_End + cc + c1 + " (Total " + Math.round(Uniques_Date_End - Uniques_Date_Start + 1) + " utopian days)" + cc;
        textonly += "<br />";
        textonly += "<br />" + c1 + "Total Attacks Made: " + cc + c2 + Uniques_KD_Total_Attacks + cc;
        textonly += "<br />" + c1 + "Total Kingdom Gains: " + cc;

        if (Uniques_KD_Total_Gain < Uniques_KD_Total_Lost) textonly += "<font color='#FF8888'>";
        if (Uniques_KD_Total_Gain == Uniques_KD_Total_Lost) textonly += "<font color='#FFFFFF'>";
        if (Uniques_KD_Total_Gain > Uniques_KD_Total_Lost) textonly += "<font color='#88FF88'>";
        textonly += Math.round(Uniques_KD_Total_Gain - Uniques_KD_Total_Lost) + cc + c1 + " Acres (" + Uniques_KD_Total_Gain + "/" + Uniques_KD_Total_Lost + ")" + cc;

        textonly += "<br />" + c1 + "Total Anonymous Attacks Made: " + cc + c2 + Uniques_KD_Unknown + cc;
        textonly += "<br />" + c1 + "Total Kingdom Anonymous Gains: " + cc + c2 + Uniques_KD_Unknown_Acre + " Acres" + cc;
        textonly += "<br />" + c1 + "Total Uniques Made: " + cc + c2 + Uniques_KD_Total_Uniques + cc;
        textonly += "<br />" + c1 + "Total Aid Shipments: " + cc + c2 + Uniques_KD_Aid + cc;
        textonly += "<br />";
        textonly += "<br />" + c1 + "Total Attacks Suffered: " + cc + c2 + Uniques_Enemy_Total_Attacks + cc;
        textonly += "<br />" + c1 + "Total Enemies Gains: " + cc;

        if (Uniques_Enemy_Total_Gain < Uniques_Enemy_Total_Lost) textonly += "<font color='#FF8888'>";
        if (Uniques_Enemy_Total_Gain == Uniques_Enemy_Total_Lost) textonly += "<font color='#FFFFFF'>";
        if (Uniques_Enemy_Total_Gain > Uniques_Enemy_Total_Lost) textonly += "<font color='#88FF88'>";
        textonly += Math.round(Uniques_Enemy_Total_Gain - Uniques_Enemy_Total_Lost) + cc + c1 + " Acres (" + Uniques_Enemy_Total_Gain + "/" + Uniques_Enemy_Total_Lost + ")" + cc;

        textonly += "<br />" + c1 + "Total Anonymous Attacks Suffered: " + cc + c2 + Uniques_Enemy_Unknown + cc;
        textonly += "<br />" + c1 + "Total Enemies Anonymous Gains: " + cc + c2 + Uniques_Enemy_Unknown_Acre + " Acres" + cc;
        textonly += "<br />" + c1 + "Total Uniques Suffered: " + cc + c2 + Uniques_Enemy_Total_Uniques + cc;


        if (Uniques_KD_Name.length > 0) {
            textonly += "<br /><br />" + c0 + "** Kingdom Uniques **" + cc;
            for (i = 0; i < Uniques_KD_Name.length; i++) {
                if (Uniques_KD_Name[i].indexOf("An unknown province from") == -1) {
                    textonly += "<br />" + c1 + Math.round(i + 1) + ") " + Uniques_KD_Name[i] + " - " + cc + c2 + Uniques_KD_Unis[i] + " Uniques " + cc + c1 + " - Total " + cc;
                    if (Uniques_KD_Acre[i] < Uniques_KD_Acr2[i]) textonly += "<font color='#FF8888'>";
                    if (Uniques_KD_Acre[i] == Uniques_KD_Acr2[i]) textonly += "<font color='#FFFFFF'>";
                    if (Uniques_KD_Acre[i] > Uniques_KD_Acr2[i]) textonly += "<font color='#88FF88'>";
                    textonly += Math.round(Uniques_KD_Acre[i] - Uniques_KD_Acr2[i]) + cc + c1 + " Acres (" + Uniques_KD_Acre[i] + "/" + Uniques_KD_Acr2[i] + ")" + cc;
                }
            }
        }


        if (Uniques_Enemy_Name.length > 0) {

            for (k = 0; k < Uniques_Enemy_Sort.length; k++) {
                m = 0;
                Uniques_Enemy_Acer_Gain = 0;
                Uniques_Enemy_Acer_Lost = 0;
                textonly += "<br /><br />" + c0 + "** Enemy Uniques of " + Uniques_Enemy_Sort[k] + "**" + cc;
                for (i = 0; i < Uniques_Enemy_Name.length; i++) if (Uniques_Enemy_Sort[k] == Uniques_Enemy_Loca[i]) {
                    Uniques_Enemy_Acer_Gain += Uniques_Enemy_Acre[i];
                    Uniques_Enemy_Acer_Lost += Uniques_Enemy_Acr2[i];
                    if (Uniques_Enemy_Name[i].indexOf("An unknown province from") == -1) {
                        m++;
                        textonly += "<br />" + c1 + Math.round(m) + ") " + Uniques_Enemy_Name[i] + " - " + cc + c2 + Uniques_Enemy_Unis[i] + " Uniques " + cc + c1 + " - Total " + cc;
                        if (Uniques_Enemy_Acre[i] < Uniques_Enemy_Acr2[i]) textonly += "<font color='#FF8888'>";
                        if (Uniques_Enemy_Acre[i] == Uniques_Enemy_Acr2[i]) textonly += "<font color='#FFFFFF'>";
                        if (Uniques_Enemy_Acre[i] > Uniques_Enemy_Acr2[i]) textonly += "<font color='#88FF88'>";
                        textonly += Math.round(Uniques_Enemy_Acre[i] - Uniques_Enemy_Acr2[i]) + cc + c1 + " Acres (" + Uniques_Enemy_Acre[i] + "/" + Uniques_Enemy_Acr2[i] + ")" + cc;
                    }

                }


                textonly += "<br />" + c1 + "Total Kingdom Gains: " + cc;
                if (Uniques_Enemy_Acer_Gain < Uniques_Enemy_Acer_Lost) textonly += "<font color='#FF8888'>";
                if (Uniques_Enemy_Acer_Gain == Uniques_Enemy_Acer_Lost) textonly += "<font color='#FFFFFF'>";
                if (Uniques_Enemy_Acer_Gain > Uniques_Enemy_Acer_Lost) textonly += "<font color='#88FF88'>";
                textonly += Math.round(Uniques_Enemy_Acer_Gain - Uniques_Enemy_Acer_Lost) + cc + c1 + " Acres (" + Uniques_Enemy_Acer_Gain + "/" + Uniques_Enemy_Acer_Lost + ")" + cc;


            }

        }

        if (Uniques_KD_Name.length > 0) {

            textonly += "<br /><br />" + c0 + "** Kingdom Uniques Details **" + cc;
            textonly += "<br />";
            textonly += "<font color='" + Uniques_Color[1] + "'>" + "[Traditional March: " + Uniques_KD_Traditional + "]</font> ";
            textonly += "<font color='" + Uniques_Color[2] + "'>" + "[Raze: " + Uniques_KD_Raze + "]</font> ";
            textonly += "<font color='" + Uniques_Color[3] + "'>" + "[Conquest: " + Uniques_KD_Conquest + "]</font> ";
            textonly += "<font color='" + Uniques_Color[4] + "'>" + "[Ambush: " + Uniques_KD_Ambush + "]</font> ";
            textonly += "<font color='" + Uniques_Color[5] + "'>" + "[Plunder: " + Uniques_KD_Plunder + "]</font> ";
            textonly += "<br />";
            textonly += "<font color='" + Uniques_Color[6] + "'>" + "[Learn: " + Uniques_KD_Learn + "]</font> ";
            textonly += "<font color='" + Uniques_Color[7] + "'>" + "[Massacre: " + Uniques_KD_Massacre + "]</font> ";
            textonly += "<font color='" + Uniques_Color[8] + "'>" + "[Fail: " + Uniques_KD_Fail + "]</font> ";
            textonly += "<font color='" + Uniques_Color[9] + "'>" + "[Unknown: " + Uniques_KD_Unknown + "]</font> ";
            textonly += "<font color='" + Uniques_Color[10] + "'>" + "[Intra: " + Uniques_KD_Intra + "]</font> ";
            textonly += "<br />";

            for (i = 0; i < Uniques_KD_Name.length; i++) {
                temp = 0;
                if (Uniques_KD_Name[i].indexOf("An unknown province from") == -1) {
                    textonly += "<br /> " + c1 + Math.round(i + 1) + ") " + Uniques_KD_Name[i] + cc;
                    textonly += c1 + " - Hits (" + Uniques_KD_Time[i][0] + " Made/" + Uniques_KD_Tim2[i] + " Received)" + cc;
                    if (Uniques_KD_Time[i][0] != 0) textonly += c1 + " - " + cc + c2 + Uniques_KD_Unis[i] + " Uniques" + cc + c1 + " - (" + cc;
                    for (t = 1; t <= Uniques_KD_Time[i][0]; t++) {
                        textonly += "<font color='" + Uniques_Color[Uniques_KD_Type[i][t]] + "'>" + Uniques_KD_Time[i][t] + "</font>";
                        if (Uniques_KD_Time[i][0] > 1) {
                            if (t < Uniques_KD_Time[i][0]) {
                                if (Uniques_Time > Uniques_KD_Time[i][t + 1] - Uniques_KD_Time[i][t]) { temp++; if (t < Uniques_KD_Time[i][0]) textonly += " "; }
                                if ((Uniques_Time <= Uniques_KD_Time[i][t + 1] - Uniques_KD_Time[i][t]) || (temp > 3)) { temp = 0; textonly += c1 + ") (" + cc; }
                            } else { if (temp > 4) { temp = 0; textonly += c1 + ") (" + cc; } } 
                        } 
                    }
                    if (Uniques_KD_Time[i][0] != 0) textonly += c1 + ")" + cc;
                }
            }
        }

        if (Uniques_Enemy_Name.length > 0) {

            textonly += "<br /><br />" + c0 + "** Enemy Uniques Details **" + cc;
            textonly += "<br /><font color='" + Uniques_Color[1] + "'>" + "-- Traditional March: " + Uniques_Enemy_Traditional + "</font> ";
            textonly += "<br /><font color='" + Uniques_Color[2] + "'>" + "-- Raze: " + Uniques_Enemy_Raze + "</font> ";
            textonly += "<br /><font color='" + Uniques_Color[3] + "'>" + "-- Conquest: " + Uniques_Enemy_Conquest + "</font> ";
            textonly += "<br /><font color='" + Uniques_Color[4] + "'>" + "-- Ambush: " + Uniques_Enemy_Ambush + "</font> ";
            textonly += "<br /><font color='" + Uniques_Color[5] + "'>" + "-- Plunder: " + Uniques_Enemy_Plunder + "</font> ";
            textonly += "<br /><font color='" + Uniques_Color[6] + "'>" + "-- Learn: " + Uniques_Enemy_Learn + "</font> ";
            textonly += "<br /><font color='" + Uniques_Color[7] + "'>" + "-- Massacre: " + Uniques_Enemy_Massacre + "</font> ";
            textonly += "<br /><font color='" + Uniques_Color[8] + "'>" + "-- Fail: " + Uniques_Enemy_Fail + "</font> ";
            textonly += "<br /><font color='" + Uniques_Color[9] + "'>" + "-- Unknown: " + Uniques_Enemy_Unknown + "</font> ";

            for (k = 0; k < Uniques_Enemy_Sort.length; k++) {
                m = 0;


                for (i = 0; i < Uniques_Enemy_Name.length; i++) if (Uniques_Enemy_Sort[k] == Uniques_Enemy_Loca[i]) {
                    temp = 0; m++;
                    if (Uniques_Enemy_Name[i].indexOf("An unknown province from") == -1) {
                        if (m == 1) textonly += "<br /><br />" + c0 + "** Kingdom " + Uniques_Enemy_Sort[k] + "**" + cc;
                        textonly += "<br /> " + c1 + Math.round(m) + ") " + Uniques_Enemy_Name[i] + cc;
                        textonly += c1 + " - Hits (" + Uniques_Enemy_Time[i][0] + " Made/" + Uniques_Enemy_Tim2[i] + " Received)" + cc;
                        if (Uniques_Enemy_Time[i][0] != 0) textonly += c1 + " - " + cc + c2 + Uniques_Enemy_Unis[i] + " Uniques" + cc + c1 + " - (" + cc;
                        for (t = 1; t <= Uniques_Enemy_Time[i][0]; t++) {
                            textonly += "<font color='" + Uniques_Color[Uniques_Enemy_Type[i][t]] + "'>" + Uniques_Enemy_Time[i][t] + "</font>";
                            if (Uniques_Enemy_Time[i][0] > 1) {
                                if (t < Uniques_Enemy_Time[i][0]) {
                                    if (Uniques_Time > Uniques_Enemy_Time[i][t + 1] - Uniques_Enemy_Time[i][t]) { temp++; if (t < Uniques_Enemy_Time[i][0]) textonly += " "; }
                                    if ((Uniques_Time <= Uniques_Enemy_Time[i][t + 1] - Uniques_Enemy_Time[i][t]) || (temp > 3)) { temp = 0; textonly += c1 + ") (" + cc; }
                                } else { if (temp > 4) { temp = 0; textonly += c1 + ") (" + cc; } } 
                            } 
                        }
                        if (Uniques_Enemy_Time[i][0] != 0) textonly += c1 + ")" + cc;
                    }
                }
            }
        }

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

}
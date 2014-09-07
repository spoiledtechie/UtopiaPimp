function Clipboard_Kingdom() {

    if (content.indexOf("Average Opponent") != -1) {

        if (document.getElementById('Ultima_Popups').checked == false) document.getElementById('Ultima_floating_window0').style.display = "block";

        var Kingdom_Name = "";
        var Kingdom_Total_Provinces = 0;
        var Kingdom_Total_Networth = 0;
        var Kingdom_Total_Land = 0;
        var Kingdom_Avg_Networth = 0;
        var Kingdom_Avg_Land = 0;
        var Kingdom_Networth_Rank = 0;
        var Kingdom_Land_Rank = 0;
        var Kingdom_Total_Kingdoms = 0;
        var Kingdom_Avg_NW_Acre = 0;
        var Kingdom_Dragon_Cost = 0;
        var Kingdom_Min_Dragon_Range = 0;
        var Kingdom_Max_Dragon_Range = 0;
        var Kingdom_Stance = 0;
        var Kingdom_Attitude_Us = "";
        var Kingdom_Attitude_Them = "";
        var Kingdom_Currently_at_WAR = "";
        var Kingdom_Wars = 0;
        var Kingdom_Wins = 0;

        var Kingdom_Province_Name = new Array(25);
        var Kingdom_Province_Race = new Array(25);
        var Kingdom_Province_Land = new Array(25);
        var Kingdom_Province_NetW = new Array(25);
        var Kingdom_Province_NWPA = new Array(25);
        var Kingdom_Province_Rank = new Array(25);

        var Kingdom_Province_Online = new Array(25); for (i = 0; i < 25; i++) Kingdom_Province_Online[i] = 0;
        var Kingdom_Province_Inactive = new Array(25); for (i = 0; i < 25; i++) Kingdom_Province_Inactive[i] = 0;
        var Kingdom_Province_Protection = new Array(25); for (i = 0; i < 25; i++) Kingdom_Province_Protection[i] = 0;


        content = content.slice(content.indexOf("/Acre"));

        Kingdom_Name = content.slice(content.indexOf("Current kingdom is ") + 19, content.indexOf(")") + 1);

        Kingdom_Total_Provinces = CB_Find(content, "Total Provinces", eol);
        Kingdom_Total_Networth = CB_Find(content, "Total Networth", "gc");
        Kingdom_Total_Land = CB_Find(content, "Total Land", " acres");
        Kingdom_Avg_Networth = CB_Find(content, "Total Networth " + Kingdom_Total_Networth + "gc (avg:", "gc");
        Kingdom_Avg_Land = CB_Find(content, "Total Land " + Kingdom_Total_Land + " acres (avg:", " acres");

        if (content.indexOf("Net Worth Rank") != -1) {

            Kingdom_Networth_Rank = CB_Find(content, "Net Worth Rank", " of");
            Kingdom_Land_Rank = CB_Find(content, "Land Rank", " of");
            Kingdom_Total_Kingdoms = CB_Find(content, "Land Rank " + Kingdom_Land_Rank + " of", eol);

        }

        if (content.indexOf("Their Attitude Towards Us:") != -1) Kingdom_Attitude_Us = content.slice(content.indexOf("Their Attitude Towards Us:") + 27, content.indexOf("Our Attitude Towards Them:") - 1);
        if (content.indexOf("Our Attitude Towards Them:") != -1) Kingdom_Attitude_Them = content.slice(content.indexOf("Our Attitude Towards Them:") + 27, content.indexOf("Kingdom Stance:") - 1);

        temp = content.slice(content.indexOf("Stance") + 6, content.indexOf(eol));

        for (i = 0; i < 4; i++) if (temp == Kingdom_Stance_Search_Key[i]) Kingdom_Stance = i;

        if (content.indexOf("kingdom is currently at war with") != -1) {
            temp = content.slice(content.indexOf("kingdom is currently at war with") + 32)
            Kingdom_Currently_at_WAR = temp.slice(0, temp.indexOf(")") + 1);
        }

        content = content.slice(content.indexOf("Wars Won / Concluded Wars") + 26);

        Kingdom_Wins = Number(content.slice(0, content.indexOf(" ")));

        content = content.slice(content.indexOf("/ ") + 2);

        Kingdom_Wars = Number(content.slice(0, content.indexOf(eol)));

        content = content.slice(content.indexOf("Nobility"));
        content = content.slice(content.indexOf(eol) + 1);
        content = content.replace(/- Awaiting Activation -/g, ":::: (");
        content = content.replace(/- Unclaimed -/g, ":::: (");


        temp = content;


        for (i = 0; i < Kingdom_Total_Provinces; i++) {

            temp = content.slice(0, content.indexOf(eol) + 1);

            Kingdom_Province_Rank[i] = temp.slice(temp.lastIndexOf("gc ") + 3, content.indexOf(eol)); temp = temp.slice(0, temp.lastIndexOf("gc "));
            Kingdom_Province_NWPA[i] = temp.slice(temp.lastIndexOf("gc ") + 3); temp = temp.slice(0, temp.lastIndexOf("gc "));
            Kingdom_Province_NetW[i] = temp.slice(temp.lastIndexOf(" acres ") + 7); temp = temp.slice(0, temp.lastIndexOf(" acres"));
            Kingdom_Province_Land[i] = temp.slice(temp.lastIndexOf(" ") + 1); temp = temp.slice(0, temp.lastIndexOf(" "));
            Kingdom_Province_Race[i] = temp.slice(temp.lastIndexOf(" ") + 1); temp = temp.slice(0, temp.lastIndexOf(" "));

            if (Kingdom_Province_Race[i] == "DarkElf") Kingdom_Province_Race[i] = "Dark Elf";

            if (temp.indexOf("*") != -1) {
                temp = temp.slice(0, temp.length - 1);
                Kingdom_Province_Online[i] = 1;
            }

            if (temp.indexOf("+") != -1) {
                temp = temp.slice(0, temp.lastIndexOf("+"));
                Kingdom_Province_Inactive[i] = 1;
            }

            if (temp.indexOf("^") != -1) {
                temp = temp.slice(0, temp.lastIndexOf("^"));
                Kingdom_Province_Protection[i] = 1;
            }

            if (temp.indexOf("(M)") != -1) temp = temp.slice(0, temp.lastIndexOf(" ("));

            Kingdom_Province_Name[i] = temp;

            content = content.slice(content.indexOf(eol) + 1);

        }



        for (i = 0; i < Kingdom_Total_Provinces; i++) if (Kingdom_Province_Land[i] != "DEAD") Kingdom_Province_Land[i] = Number(Kingdom_Province_Land[i]);
        for (i = 0; i < Kingdom_Total_Provinces; i++) if (Kingdom_Province_Land[i] == "DEAD") Kingdom_Province_Rank[i] = "Peasant";
        for (i = 0; i < Kingdom_Total_Provinces; i++) if (Kingdom_Province_Name[i] == "::::") Kingdom_Province_Rank[i] = "Peasant";


        for (i = 0; i < Kingdom_Total_Provinces; i++) Kingdom_Province_NetW[i] = Number(Kingdom_Province_NetW[i]);
        for (i = 0; i < Kingdom_Total_Provinces; i++) Kingdom_Province_NWPA[i] = Math.round(Kingdom_Province_NetW[i] / Kingdom_Province_Land[i] * 100) / 100;



        for (i = 0; i < Kingdom_Total_Provinces; i++) if (Kingdom_Province_Race[i].charAt(Kingdom_Province_Race[i].length - 1) == " ") Kingdom_Province_Race[i] = Kingdom_Province_Race[i].slice(0, Kingdom_Province_Race[i].length - 1);




        var Kingdom_Race_Amount = new Array(10); for (t = 0; t < 10; t++) Kingdom_Race_Amount[t] = 0;

        for (i = 0; i < Kingdom_Total_Provinces; i++) for (t = 0; t < 10; t++) if (Kingdom_Province_Race[i] == Race_Name[t]) {
            Kingdom_Province_Race[i] = Race_Name[t];
            Kingdom_Race_Amount[t]++;

        }





        var temp1 = 0;
        var temp2 = 0;

        var Order_By_Temp = new Array(25);

        var Order_By_Land = new Array(25);
        var Order_By_NetW = new Array(25);
        var Order_By_NWPA = new Array(25);
        var Order_By_Rank = new Array(25);

        for (i = 0; i < Kingdom_Total_Provinces; i++) Order_By_Land[i] = i;
        for (i = 0; i < Kingdom_Total_Provinces; i++) Order_By_NetW[i] = i;
        for (i = 0; i < Kingdom_Total_Provinces; i++) Order_By_NWPA[i] = i;
        for (i = 0; i < Kingdom_Total_Provinces; i++) Order_By_Rank[i] = i;

        for (i = 0; i < Kingdom_Total_Provinces; i++) Order_By_Temp[i] = Kingdom_Province_Land[i];
        for (t = 0; t < Kingdom_Total_Provinces; t++) for (i = 0; i < Kingdom_Total_Provinces - 1; i++) if (Order_By_Temp[i] < Order_By_Temp[i + 1]) {
            temp1 = Order_By_Temp[i]; Order_By_Temp[i] = Order_By_Temp[i + 1]; Order_By_Temp[i + 1] = temp1;
            temp2 = Order_By_Land[i]; Order_By_Land[i] = Order_By_Land[i + 1]; Order_By_Land[i + 1] = temp2;
        }

        for (i = 0; i < Kingdom_Total_Provinces; i++) Order_By_Temp[i] = Kingdom_Province_NetW[i];
        for (t = 0; t < Kingdom_Total_Provinces; t++) for (i = 0; i < Kingdom_Total_Provinces - 1; i++) if (Order_By_Temp[i] < Order_By_Temp[i + 1]) {
            temp1 = Order_By_Temp[i]; Order_By_Temp[i] = Order_By_Temp[i + 1]; Order_By_Temp[i + 1] = temp1;
            temp2 = Order_By_NetW[i]; Order_By_NetW[i] = Order_By_NetW[i + 1]; Order_By_NetW[i + 1] = temp2;
        }

        for (i = 0; i < Kingdom_Total_Provinces; i++) Order_By_Temp[i] = Kingdom_Province_NWPA[i];
        for (t = 0; t < Kingdom_Total_Provinces; t++) for (i = 0; i < Kingdom_Total_Provinces - 1; i++) if (Order_By_Temp[i] < Order_By_Temp[i + 1]) {
            temp1 = Order_By_Temp[i]; Order_By_Temp[i] = Order_By_Temp[i + 1]; Order_By_Temp[i + 1] = temp1;
            temp2 = Order_By_NWPA[i]; Order_By_NWPA[i] = Order_By_NWPA[i + 1]; Order_By_NWPA[i + 1] = temp2;
        }

        for (m = 0; m < Kingdom_Total_Provinces; m++) for (t = 0; t < 2; t++) for (i = 0; i < 10; i++) if (Kingdom_Province_Rank[m] == Rank_Name[t][i]) Order_By_Temp[m] = 9 - i;
        for (t = 0; t < Kingdom_Total_Provinces; t++) for (i = 0; i < Kingdom_Total_Provinces - 1; i++) if (Order_By_Temp[i] > Order_By_Temp[i + 1]) {
            temp1 = Order_By_Temp[i]; Order_By_Temp[i] = Order_By_Temp[i + 1]; Order_By_Temp[i + 1] = temp1;
            temp2 = Order_By_Rank[i]; Order_By_Rank[i] = Order_By_Rank[i + 1]; Order_By_Rank[i + 1] = temp2;
        }


        var Kingdom_Percentage_Land = new Array(25);
        var Kingdom_Percentage_NetW = new Array(25);

        for (i = 0; i < Kingdom_Total_Provinces; i++) Kingdom_Percentage_Land[i] = Math.round(Kingdom_Province_Land[i] / Kingdom_Total_Land * 1000) / 10;
        for (i = 0; i < Kingdom_Total_Provinces; i++) Kingdom_Percentage_NetW[i] = Math.round(Kingdom_Province_NetW[i] / Kingdom_Total_Networth * 1000) / 10;


        Kingdom_Avg_NW_Acre = Math.round(Kingdom_Total_Networth / Kingdom_Total_Land * 100) / 100;
        Kingdom_Dragon_Cost = Math.round(Kingdom_Total_Networth * 1.75);
        Kingdom_Min_Dragon_Range = Math.round(Kingdom_Total_Networth / 1.25);
        Kingdom_Max_Dragon_Range = Math.round(Kingdom_Total_Networth * 1.25);


        // *******************************************************
        //       Extended options for external sites - Start
        // *******************************************************

        var DataSaveForm = document.getElementById('SendData');
        for (i = 0; i < DataSaveForm.elements.length; i++) DataSaveForm.elements[i].value = "";

        for (i = 0; i < Kingdom_Total_Provinces; i++) {

            DataSaveForm.elements.namedItem("prov_" + i.toString() + "_name").value = Kingdom_Province_Name[i];
            DataSaveForm.elements.namedItem("prov_" + i.toString() + "_race").value = Kingdom_Province_Race[i];
            DataSaveForm.elements.namedItem("prov_" + i.toString() + "_nw").value = Kingdom_Province_NetW[i];
            DataSaveForm.elements.namedItem("prov_" + i.toString() + "_nwpa").value = Kingdom_Province_NWPA[i];
            DataSaveForm.elements.namedItem("prov_" + i.toString() + "_land").value = Kingdom_Province_Land[i];
            DataSaveForm.elements.namedItem("prov_" + i.toString() + "_rank").value = Kingdom_Province_Rank[i];
            DataSaveForm.elements.namedItem("prov_" + i.toString() + "_online").value = Kingdom_Province_Online[i];
            DataSaveForm.elements.namedItem("prov_" + i.toString() + "_inactive").value = Kingdom_Province_Inactive[i];
            DataSaveForm.elements.namedItem("prov_" + i.toString() + "_protected").value = Kingdom_Province_Protection[i];
        }

        DataSaveForm.kd_Name.value = Kingdom_Name;
        DataSaveForm.kd_Total_Provinces.value = Kingdom_Total_Provinces;
        DataSaveForm.kd_Total_Networth.value = Kingdom_Total_Networth;
        DataSaveForm.kd_Avg_NW_Acre.value = Kingdom_Avg_NW_Acre;
        DataSaveForm.kd_Total_Land.value = Kingdom_Total_Land;
        DataSaveForm.kd_Stance.value = Kingdom_Stance;
        DataSaveForm.kd_Currently_at_WAR.value = Kingdom_Currently_at_WAR;
        DataSaveForm.kd_Wins.value = Kingdom_Wins;
        DataSaveForm.kd_Wars.value = Kingdom_Wars;

        DataSaveForm.kd_Dragon_Cost.value = Kingdom_Dragon_Cost;
        DataSaveForm.kd_Min_Dragon_Range.value = Kingdom_Min_Dragon_Range;
        DataSaveForm.kd_Max_Dragon_Range.value = Kingdom_Max_Dragon_Range;
        DataSaveForm.kd_Networth_Rank.value = Kingdom_Networth_Rank;
        DataSaveForm.kd_Land_Rank.value = Kingdom_Land_Rank;
        DataSaveForm.kd_Total_Kingdoms.value = Kingdom_Total_Kingdoms;
        DataSaveForm.kd_Attitude_Us.value = Kingdom_Attitude_Us;
        DataSaveForm.kd_Attitude_Them.value = Kingdom_Attitude_Them;

        // *******************************************************
        //       Extended options for external sites - End
        // *******************************************************


        for (i = 0; i < Kingdom_Total_Provinces; i++) Kingdom_Province_Land[i] = Add_Commas(Kingdom_Province_Land[i]);
        for (i = 0; i < Kingdom_Total_Provinces; i++) Kingdom_Province_NetW[i] = Add_Commas(Kingdom_Province_NetW[i]);

        Kingdom_Total_Provinces = Add_Commas(Kingdom_Total_Provinces);
        Kingdom_Total_Networth = Add_Commas(Kingdom_Total_Networth);
        Kingdom_Total_Land = Add_Commas(Kingdom_Total_Land);
        Kingdom_Avg_Networth = Add_Commas(Kingdom_Avg_Networth);
        Kingdom_Avg_Land = Add_Commas(Kingdom_Avg_Land);
        Kingdom_Networth_Rank = Add_Commas(Kingdom_Networth_Rank);
        Kingdom_Land_Rank = Add_Commas(Kingdom_Land_Rank);
        Kingdom_Total_Kingdoms = Add_Commas(Kingdom_Total_Kingdoms);
        Kingdom_Avg_NW_Acre = Add_Commas(Kingdom_Avg_NW_Acre);
        Kingdom_Dragon_Cost = Add_Commas(Kingdom_Dragon_Cost);
        Kingdom_Min_Dragon_Range = Add_Commas(Kingdom_Min_Dragon_Range);
        Kingdom_Max_Dragon_Range = Add_Commas(Kingdom_Max_Dragon_Range);


        textonly = "" + c0 + Kingdom_Name + " Kingdom Analysis" + cc + Copyrights +
"<br />" +
"<br />" + c0 + "** Statistics **" + cc +
"<br />" + c1 + "Provinces in Kingdom: " + cc + c2 + Kingdom_Total_Provinces + cc +
"<br />" + c1 + "Total Networth: " + cc + c2 + Kingdom_Total_Networth + cc + c1 + "gc (Average: " + Kingdom_Avg_Networth + "gc)" + cc +
"<br />" + c1 + "Total Land " + cc + c2 + Kingdom_Total_Land + cc + c1 + " Acres (Average: " + Kingdom_Avg_Land + " Acres)" + cc;

        if (Kingdom_Networth_Rank != 0) textonly += "<br />" + c1 + "Networth Rank: " + cc + c2 + Kingdom_Networth_Rank + cc + c1 + " of " + cc + c2 + Kingdom_Total_Kingdoms + cc;
        if (Kingdom_Land_Rank != 0) textonly += "<br />" + c1 + "Land Rank: " + cc + c2 + Kingdom_Land_Rank + cc + c1 + " of " + cc + c2 + Kingdom_Total_Kingdoms + cc;

        textonly += "<br />" + c1 + "Average Networth per Acre: " + cc + c2 + Kingdom_Avg_NW_Acre + cc + c1 + "gc" + cc +
"<br />" + c1 + "Dragon Cost: " + cc + c2 + Kingdom_Dragon_Cost + cc + c1 + "gc" + cc +
"<br />" + c1 + "Dragon Send Range: " + cc + c2 + Kingdom_Min_Dragon_Range + cc + c1 + "gc - " + cc + c2 + Kingdom_Max_Dragon_Range + cc + c1 + "gc" + cc +

"<br /><br />" + c0 + "** Relations **" + cc;

        if (Kingdom_Currently_at_WAR != "") textonly += "<br />" + c1 + "Currently at WAR with: " + cc + c2 + Kingdom_Currently_at_WAR + cc;

        if (Kingdom_Attitude_Us != "") {
            textonly += "<br />" + c1 + "Attitude towards/from Us: " + cc + c2 + Kingdom_Attitude_Us + cc;
            if (Kingdom_Attitude_Us != Kingdom_Attitude_Them) textonly += c1 + " / " + cc + c2 + Kingdom_Attitude_Them + cc;
        }

        textonly += "<br />" + c1 + "Stance: " + cc + c2 + Kingdom_Stance_Search_Key[Kingdom_Stance] + cc + c1 + " " + Kingdom_Stance_Name[Kingdom_Stance] + cc +
"<br />" + c1 + "War History: " + cc + c2 + Kingdom_Wars + cc + c1 + " Wars" + cc + c1 + ", " + cc + c2 + Kingdom_Wins + cc + c1 + " Wins" + cc;
        if (Kingdom_Wins != 0) textonly += c1 + " (" + Math.round(Kingdom_Wins / Kingdom_Wars * 100) + "%)" + cc;

        temp1 = 0; textonly += "<br /><br />" + c0 + "** Races **" + cc; for (i = 0; i < 10; i++) if (Kingdom_Race_Amount[i] > 0) { temp1++; textonly += "<br />" + c1 + Math.round(temp1) + ". " + Race_Name[i] + ": " + cc + c2 + Kingdom_Race_Amount[i] + cc + c1 + " (" + Math.round(Kingdom_Race_Amount[i] / Kingdom_Total_Provinces * 100) + "%)" + cc; }
        temp1 = 0; textonly += "<br /><br />" + c0 + "** Land **" + cc; for (i = 0; i < Kingdom_Total_Provinces; i++) if ((Kingdom_Province_Name[Order_By_Land[i]] != "::::") && (Kingdom_Province_Land[Order_By_Land[i]] != "DEAD")) { temp1++; textonly += "<br />" + c1 + Math.round(temp1) + ". " + Kingdom_Province_Name[Order_By_Land[i]] + " [" + Kingdom_Province_Race[Order_By_Land[i]] + "] - " + cc + c2 + Kingdom_Province_Land[Order_By_Land[i]] + cc + c1 + " Acres (" + Kingdom_Percentage_Land[Order_By_Land[i]] + "%)" + cc; }
        temp1 = 0; textonly += "<br /><br />" + c0 + "** Networth **" + cc; for (i = 0; i < Kingdom_Total_Provinces; i++) if ((Kingdom_Province_Name[Order_By_NetW[i]] != "::::") && (Kingdom_Province_Land[Order_By_NetW[i]] != "DEAD")) { temp1++; textonly += "<br />" + c1 + Math.round(temp1) + ". " + Kingdom_Province_Name[Order_By_NetW[i]] + " [" + Kingdom_Province_Race[Order_By_NetW[i]] + "] - " + cc + c2 + Kingdom_Province_NetW[Order_By_NetW[i]] + cc + c1 + "gc (" + Kingdom_Percentage_NetW[Order_By_NetW[i]] + "%)" + cc; }
        temp1 = 0; textonly += "<br /><br />" + c0 + "** Networth per Acre **" + cc; for (i = 0; i < Kingdom_Total_Provinces; i++) if ((Kingdom_Province_Name[Order_By_NWPA[i]] != "::::") && (Kingdom_Province_Land[Order_By_NWPA[i]] != "DEAD")) { temp1++; textonly += "<br />" + c1 + Math.round(temp1) + ". " + Kingdom_Province_Name[Order_By_NWPA[i]] + " [" + Kingdom_Province_Race[Order_By_NWPA[i]] + "] - " + cc + c2 + Kingdom_Province_NWPA[Order_By_NWPA[i]] + cc + c1 + "gc" + cc; }
        temp1 = 0; textonly += "<br /><br />" + c0 + "** Ranks **" + cc; for (i = 0; i < Kingdom_Total_Provinces; i++) if ((Kingdom_Province_Name[Order_By_Rank[i]] != "::::") && (Kingdom_Province_Land[Order_By_Rank[i]] != "DEAD")) { temp1++; textonly += "<br />" + c1 + Math.round(temp1) + ". " + Kingdom_Province_Name[Order_By_Rank[i]] + " [" + Kingdom_Province_Race[Order_By_Rank[i]] + "] - " + cc + c2 + Kingdom_Province_Rank[Order_By_Rank[i]] + cc; }

        textonly += "<br /><br />" + c0 + "** Miscellaneous **" + cc;

        for (i = 0; i < 25; i++) if (Kingdom_Province_Online[i] == 1) if ((Kingdom_Province_Name[i] != "::::") && (Kingdom_Province_Land[i] != "DEAD")) textonly += "<br />" + c2 + "ONLINE: " + cc + c1 + Kingdom_Province_Name[i] + " [" + Kingdom_Province_Race[i] + "]" + cc;

        for (i = 0; i < 25; i++) if (Kingdom_Province_Inactive[i] == 1) if ((Kingdom_Province_Name[i] != "::::") && (Kingdom_Province_Land[i] != "DEAD")) textonly += "<br />" + c2 + "INACTIVE: " + cc + c1 + Kingdom_Province_Name[i] + " [" + Kingdom_Province_Race[i] + "]" + cc;

        for (i = 0; i < 25; i++) if (Kingdom_Province_Protection[i] == 1) if ((Kingdom_Province_Name[i] != "::::") && (Kingdom_Province_Land[i] != "DEAD")) textonly += "<br />" + c2 + "PROTECTION: " + cc + c1 + Kingdom_Province_Name[i] + " [" + Kingdom_Province_Race[i] + "]" + cc;

        for (i = 0; i < 25; i++) if (Kingdom_Province_Land[i] == "DEAD") textonly += "<br />" + c2 + "DEAD: " + cc + c1 + Kingdom_Province_Name[i] + " [" + Kingdom_Province_Race[i] + "]" + cc;


        textonly += "<br />";

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
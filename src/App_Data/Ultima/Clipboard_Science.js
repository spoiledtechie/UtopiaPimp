function Clipboard_Science() {

    if (((content.indexOf("Knowledge") != -1) || (EL == 2)) && (content.indexOf("Formatted Report") == -1)) {

        if (document.getElementById('Ultima_Popups').checked == false) document.getElementById('Ultima_floating_window0').style.display = "block";

        if ((content.indexOf("Our thieves visit the research centers of") == -1) && (EL == 0)) {

            if (document.getElementById('Ultima_Popups').checked == false) document.getElementById('Ultima_floating_window2').style.display = "block";






            content = content.slice(content.indexOf("Worth/Acre"));
            content = content.slice(content.indexOf(eol) + 1);

            content = content.slice(content.indexOf(" ") + 1); Sci_Acres = content.slice(0, content.indexOf(" "));
            content = content.slice(content.indexOf(" ") + 1); Sci_Acres = content.slice(0, content.indexOf(" "));
            content = content.slice(content.indexOf(" ") + 1); Sci_Acres = content.slice(0, content.indexOf(" "));
            content = content.slice(content.indexOf(" ") + 1); Sci_Acres = content.slice(0, content.indexOf(" "));
            content = content.slice(content.indexOf(" ") + 1); Sci_Acres = Number(content.slice(0, content.indexOf(" ")));

            content = content.slice(content.indexOf("Total Money"));

            Sci_Learnable = content.slice(content.indexOf("Allocate:"));
            Sci_Learnable = Sci_Learnable.slice(Sci_Learnable.indexOf(" ") + 1);
            Sci_Learnable = Sci_Learnable.slice(0, Sci_Learnable.indexOf(" books"));
            Sci_Learnable = Number(Sci_Learnable);

            Sci_Research_Cost = content.slice(content.indexOf("Cost"));
            Sci_Research_Cost = Sci_Research_Cost.slice(Sci_Research_Cost.indexOf(" ") + 1);
            Sci_Research_Cost = Sci_Research_Cost.slice(0, Sci_Research_Cost.indexOf("gc"));
            Sci_Research_Cost = Number(Sci_Research_Cost);

            Sci_Total_Money = content.slice(content.indexOf("Total Money"));
            Sci_Total_Money = Sci_Total_Money.slice(Sci_Total_Money.indexOf(" ") + 1);
            Sci_Total_Money = Sci_Total_Money.slice(Sci_Total_Money.indexOf(" ") + 1);
            Sci_Total_Money = Sci_Total_Money.slice(0, Sci_Total_Money.indexOf("gc"));
            Sci_Total_Money = Number(Sci_Total_Money);

            Sci_Daily_Income = content.slice(content.indexOf("Daily Income"));
            Sci_Daily_Income = Sci_Daily_Income.slice(Sci_Daily_Income.indexOf(" ") + 1);
            Sci_Daily_Income = Sci_Daily_Income.slice(Sci_Daily_Income.indexOf(" ") + 1);
            Sci_Daily_Income = Sci_Daily_Income.slice(0, Sci_Daily_Income.indexOf("gc"));
            Sci_Daily_Income = Number(Sci_Daily_Income);


            Sci_Available_Alchemy = content.slice(content.indexOf("Alchemy"));
            Sci_Available_Alchemy = Sci_Available_Alchemy.slice(Sci_Available_Alchemy.indexOf(" ") + 1);
            Sci_Available_Alchemy = Sci_Available_Alchemy.slice(0, Sci_Available_Alchemy.indexOf(eol));
            Sci_Bonus_Alchemy = Sci_Available_Alchemy.slice(Sci_Available_Alchemy.indexOf("+") + 1, Sci_Available_Alchemy.indexOf("%"));
            Sci_Progress_Alchemy = Sci_Available_Alchemy.slice(Sci_Available_Alchemy.indexOf("Income"));
            Sci_Progress_Alchemy = Sci_Progress_Alchemy.slice(Sci_Progress_Alchemy.indexOf(" ") + 1);
            Sci_Progress_Alchemy = Sci_Progress_Alchemy.slice(0, Sci_Progress_Alchemy.indexOf(" "));
            Sci_Available_Alchemy = Sci_Available_Alchemy.slice(0, Sci_Available_Alchemy.indexOf(" "));

            Sci_Bonus_Alchemy = Number(Sci_Bonus_Alchemy);
            Sci_Progress_Alchemy = Number(Sci_Progress_Alchemy);
            Sci_Available_Alchemy = Number(Sci_Available_Alchemy);

            Sci_Available_Tools = content.slice(content.indexOf("Tools"));
            Sci_Available_Tools = Sci_Available_Tools.slice(Sci_Available_Tools.indexOf(" ") + 1);
            Sci_Available_Tools = Sci_Available_Tools.slice(0, Sci_Available_Tools.indexOf(eol));
            Sci_Bonus_Tools = Sci_Available_Tools.slice(Sci_Available_Tools.indexOf("+") + 1, Sci_Available_Tools.indexOf("%"));
            Sci_Progress_Tools = Sci_Available_Tools.slice(Sci_Available_Tools.indexOf("Effectiveness"));
            Sci_Progress_Tools = Sci_Progress_Tools.slice(Sci_Progress_Tools.indexOf(" ") + 1);
            Sci_Progress_Tools = Sci_Progress_Tools.slice(0, Sci_Progress_Tools.indexOf(" "));
            Sci_Available_Tools = Sci_Available_Tools.slice(0, Sci_Available_Tools.indexOf(" "));

            Sci_Bonus_Tools = Number(Sci_Bonus_Tools);
            Sci_Progress_Tools = Number(Sci_Progress_Tools);
            Sci_Available_Tools = Number(Sci_Available_Tools);

            Sci_Available_Housing = content.slice(content.indexOf("Housing"));
            Sci_Available_Housing = Sci_Available_Housing.slice(Sci_Available_Housing.indexOf(" ") + 1);
            Sci_Available_Housing = Sci_Available_Housing.slice(0, Sci_Available_Housing.indexOf(eol));
            Sci_Bonus_Housing = Sci_Available_Housing.slice(Sci_Available_Housing.indexOf("+") + 1, Sci_Available_Housing.indexOf("%"));
            Sci_Progress_Housing = Sci_Available_Housing.slice(Sci_Available_Housing.indexOf("Limits"));
            Sci_Progress_Housing = Sci_Progress_Housing.slice(Sci_Progress_Housing.indexOf(" ") + 1);
            Sci_Progress_Housing = Sci_Progress_Housing.slice(0, Sci_Progress_Housing.indexOf(" "));
            Sci_Available_Housing = Sci_Available_Housing.slice(0, Sci_Available_Housing.indexOf(" "));

            Sci_Bonus_Housing = Number(Sci_Bonus_Housing);
            Sci_Progress_Housing = Number(Sci_Progress_Housing);
            Sci_Available_Housing = Number(Sci_Available_Housing);

            Sci_Available_Food = content.slice(content.indexOf("Food "));
            Sci_Available_Food = Sci_Available_Food.slice(Sci_Available_Food.indexOf(" ") + 1);
            Sci_Available_Food = Sci_Available_Food.slice(0, Sci_Available_Food.indexOf(eol));
            Sci_Bonus_Food = Sci_Available_Food.slice(Sci_Available_Food.indexOf("+") + 1, Sci_Available_Food.indexOf("%"));
            Sci_Progress_Food = Sci_Available_Food.slice(Sci_Available_Food.indexOf("Production"));
            Sci_Progress_Food = Sci_Progress_Food.slice(Sci_Progress_Food.indexOf(" ") + 1);
            Sci_Progress_Food = Sci_Progress_Food.slice(0, Sci_Progress_Food.indexOf(" "));
            Sci_Available_Food = Sci_Available_Food.slice(0, Sci_Available_Food.indexOf(" "));

            Sci_Bonus_Food = Number(Sci_Bonus_Food);
            Sci_Progress_Food = Number(Sci_Progress_Food);
            Sci_Available_Food = Number(Sci_Available_Food);

            Sci_Available_Military = content.slice(content.indexOf("Military"));
            Sci_Available_Military = Sci_Available_Military.slice(Sci_Available_Military.indexOf(" ") + 1);
            Sci_Available_Military = Sci_Available_Military.slice(0, Sci_Available_Military.indexOf(eol));
            Sci_Bonus_Military = Sci_Available_Military.slice(Sci_Available_Military.indexOf("+") + 1, Sci_Available_Military.indexOf("%"));
            Sci_Progress_Military = Sci_Available_Military.slice(Sci_Available_Military.indexOf("Combat"));
            Sci_Progress_Military = Sci_Progress_Military.slice(Sci_Progress_Military.indexOf(" ") + 1);
            Sci_Progress_Military = Sci_Progress_Military.slice(0, Sci_Progress_Military.indexOf(" "));
            Sci_Available_Military = Sci_Available_Military.slice(0, Sci_Available_Military.indexOf(" "));

            Sci_Bonus_Military = Number(Sci_Bonus_Military);
            Sci_Progress_Military = Number(Sci_Progress_Military);
            Sci_Available_Military = Number(Sci_Available_Military);

            Sci_Available_Crime = content.slice(content.indexOf("Crime"));
            Sci_Available_Crime = Sci_Available_Crime.slice(Sci_Available_Crime.indexOf(" ") + 1);
            Sci_Available_Crime = Sci_Available_Crime.slice(0, Sci_Available_Crime.indexOf(eol));
            Sci_Bonus_Crime = Sci_Available_Crime.slice(Sci_Available_Crime.indexOf("+") + 1, Sci_Available_Crime.indexOf("%"));
            Sci_Progress_Crime = Sci_Available_Crime.slice(Sci_Available_Crime.indexOf("Effectiveness"));
            Sci_Progress_Crime = Sci_Progress_Crime.slice(Sci_Progress_Crime.indexOf(" ") + 1);
            Sci_Progress_Crime = Sci_Progress_Crime.slice(0, Sci_Progress_Crime.indexOf(" "));
            Sci_Available_Crime = Sci_Available_Crime.slice(0, Sci_Available_Crime.indexOf(" "));

            Sci_Bonus_Crime = Number(Sci_Bonus_Crime);
            Sci_Progress_Crime = Number(Sci_Progress_Crime);
            Sci_Available_Crime = Number(Sci_Available_Crime);

            Sci_Available_Channeling = content.slice(content.indexOf("Channeling"));
            Sci_Available_Channeling = Sci_Available_Channeling.slice(Sci_Available_Channeling.indexOf(" ") + 1);
            Sci_Available_Channeling = Sci_Available_Channeling.slice(0, Sci_Available_Channeling.indexOf(eol));
            Sci_Bonus_Channeling = Sci_Available_Channeling.slice(Sci_Available_Channeling.indexOf("+") + 1, Sci_Available_Channeling.indexOf("%"));
            Sci_Progress_Channeling = Sci_Available_Channeling.slice(Sci_Available_Channeling.indexOf("Production"));
            Sci_Progress_Channeling = Sci_Progress_Channeling.slice(Sci_Progress_Channeling.indexOf(" ") + 1);
            Sci_Progress_Channeling = Sci_Progress_Channeling.slice(0, Sci_Progress_Channeling.indexOf(" "));
            Sci_Available_Channeling = Sci_Available_Channeling.slice(0, Sci_Available_Channeling.indexOf(" "));

            Sci_Bonus_Channeling = Number(Sci_Bonus_Channeling);
            Sci_Progress_Channeling = Number(Sci_Progress_Channeling);
            Sci_Available_Channeling = Number(Sci_Available_Channeling);

            document.getElementById('Sci_Daily_Income').innerHTML = Sci_Daily_Income;

            document.Calculator3.Sci_Avail.value = Sci_Learnable;

            document.Calculator3.Sci_Acres.value = Sci_Acres;

            document.Calculator3.Sci_Available_Alchemy.value = Sci_Available_Alchemy;
            document.Calculator3.Sci_Available_Tools.value = Sci_Available_Tools;
            document.Calculator3.Sci_Available_Housing.value = Sci_Available_Housing;
            document.Calculator3.Sci_Available_Food.value = Sci_Available_Food;
            document.Calculator3.Sci_Available_Military.value = Sci_Available_Military;
            document.Calculator3.Sci_Available_Crime.value = Sci_Available_Crime;
            document.Calculator3.Sci_Available_Channeling.value = Sci_Available_Channeling;

            document.Calculator3.Sci_Progress_Alchemy.value = Sci_Progress_Alchemy;
            document.Calculator3.Sci_Progress_Tools.value = Sci_Progress_Tools;
            document.Calculator3.Sci_Progress_Housing.value = Sci_Progress_Housing;
            document.Calculator3.Sci_Progress_Food.value = Sci_Progress_Food;
            document.Calculator3.Sci_Progress_Military.value = Sci_Progress_Military;
            document.Calculator3.Sci_Progress_Crime.value = Sci_Progress_Crime;
            document.Calculator3.Sci_Progress_Channeling.value = Sci_Progress_Channeling;

            Sci_Acres = addCommas(Sci_Acres.toString());
            Sci_Learnable = addCommas(Sci_Learnable.toString());
            Sci_Total_Money = addCommas(Sci_Total_Money.toString());
            Sci_Daily_Income = addCommas(Sci_Daily_Income.toString());
            Sci_Research_Cost = addCommas(Sci_Research_Cost.toString());

            Sci_Available_Alchemy = addCommas(Sci_Available_Alchemy.toString());
            Sci_Available_Tools = addCommas(Sci_Available_Tools.toString());
            Sci_Available_Housing = addCommas(Sci_Available_Housing.toString());
            Sci_Available_Food = addCommas(Sci_Available_Food.toString());
            Sci_Available_Military = addCommas(Sci_Available_Military.toString());
            Sci_Available_Crime = addCommas(Sci_Available_Crime.toString());
            Sci_Available_Channeling = addCommas(Sci_Available_Channeling.toString());

            Sci_Progress_Alchemy = addCommas(Sci_Progress_Alchemy.toString());
            Sci_Progress_Tools = addCommas(Sci_Progress_Tools.toString());
            Sci_Progress_Housing = addCommas(Sci_Progress_Housing.toString());
            Sci_Progress_Food = addCommas(Sci_Progress_Food.toString());
            Sci_Progress_Military = addCommas(Sci_Progress_Military.toString());
            Sci_Progress_Crime = addCommas(Sci_Progress_Crime.toString());
            Sci_Progress_Channeling = addCommas(Sci_Progress_Channeling.toString());


            textonly = "" + c0 + "Science Intelligence Formatted Report" + cc + Copyrights +

"<br />" + c1 + "Land: " + cc + c2 + Sci_Acres + cc + c1 + " acres" + cc +
"<br />" + c1 + "Daily Income: " + cc + c2 + Sci_Daily_Income + cc + c1 + "gc" + cc +
"<br />" + c1 + "Total Money: " + cc + c2 + Sci_Total_Money + cc + c1 + "gc" + cc +
"<br />" + c1 + "Estimated Research Cost: " + cc + c2 + Sci_Research_Cost + cc + c1 + "gc" + cc +
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

        }










        if ((content.indexOf("Our thieves visit the research centers of") != -1) || (EL == 2)) {

            if (EL == 2) {

                Export_Line = Export_Line_Decryption(content);

                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Sci_Prov = ABC_Decryption(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Sci_Kingdom = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Sci_Island = Number(Export_Line.slice(0, Export_Line.indexOf("_")));
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Sci_Bonus_Alchemy = Number(Export_Line.slice(0, Export_Line.indexOf("_"))) / 10;
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Sci_Bonus_Tools = Number(Export_Line.slice(0, Export_Line.indexOf("_"))) / 10;
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Sci_Bonus_Housing = Number(Export_Line.slice(0, Export_Line.indexOf("_"))) / 10;
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Sci_Bonus_Food = Number(Export_Line.slice(0, Export_Line.indexOf("_"))) / 10;
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Sci_Bonus_Military = Number(Export_Line.slice(0, Export_Line.indexOf("_"))) / 10;
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Sci_Bonus_Crime = Number(Export_Line.slice(0, Export_Line.indexOf("_"))) / 10;
                Export_Line = Export_Line.slice(Export_Line.indexOf("_") + 1); Sci_Bonus_Channeling = Number(Export_Line.slice(0)) / 10;

            } else {

                content = content.slice(content.indexOf("Our thieves visit the research centers of") + 42);
                temp = content.slice(0, content.indexOf(")") + 1);

                Sci_Prov = temp.slice(0, temp.indexOf("("));
                Sci_Kingdom = temp.slice(temp.indexOf("(") + 1, temp.indexOf(":"));
                Sci_Island = temp.slice(temp.indexOf(":") + 1, temp.indexOf(")"));

                content = content.slice(content.indexOf("+") + 1); Sci_Bonus_Alchemy = content.slice(0, content.indexOf("%"));
                content = content.slice(content.indexOf("+") + 1); Sci_Bonus_Tools = content.slice(0, content.indexOf("%"));
                content = content.slice(content.indexOf("+") + 1); Sci_Bonus_Housing = content.slice(0, content.indexOf("%"));
                content = content.slice(content.indexOf("+") + 1); Sci_Bonus_Food = content.slice(0, content.indexOf("%"));
                content = content.slice(content.indexOf("+") + 1); Sci_Bonus_Military = content.slice(0, content.indexOf("%"));
                content = content.slice(content.indexOf("+") + 1); Sci_Bonus_Crime = content.slice(0, content.indexOf("%"));
                content = content.slice(content.indexOf("+") + 1); Sci_Bonus_Channeling = content.slice(0, content.indexOf("%"));

                Sci_Bonus_Alchemy = Number(Sci_Bonus_Alchemy);
                Sci_Bonus_Tools = Number(Sci_Bonus_Tools);
                Sci_Bonus_Housing = Number(Sci_Bonus_Housing);
                Sci_Bonus_Food = Number(Sci_Bonus_Food);
                Sci_Bonus_Military = Number(Sci_Bonus_Military);
                Sci_Bonus_Crime = Number(Sci_Bonus_Crime);
                Sci_Bonus_Channeling = Number(Sci_Bonus_Channeling);

            }

            Export_Line = "545054_" + ABC_Encryption(Sci_Prov) + "_" + Sci_Kingdom + "_" + Sci_Island + "_" + Math.round(Sci_Bonus_Alchemy * 10) + "_" + Math.round(Sci_Bonus_Tools * 10) + "_" + Math.round(Sci_Bonus_Housing * 10) + "_" + Math.round(Sci_Bonus_Food * 10) + "_" + Math.round(Sci_Bonus_Military * 10) + "_" + Math.round(Sci_Bonus_Crime * 10) + "_" + Math.round(Sci_Bonus_Channeling * 10);

            textonly = "" + c0 + "Science Intelligence on " + Sci_Prov + " (" + Sci_Kingdom + ":" + Sci_Island + ")" + cc + Copyrights +
"<br />" +
"<br />" + c0 + "** Effects Summary (Known Science Only) **" + cc +
"<br />" + c2 + Sci_Bonus_Alchemy + "%" + cc + c1 + " Income" + cc +
"<br />" + c2 + Sci_Bonus_Tools + "%" + cc + c1 + " Building Effectiveness" + cc +
"<br />" + c2 + Sci_Bonus_Housing + "%" + cc + c1 + " Population Limits" + cc +
"<br />" + c2 + Sci_Bonus_Food + "%" + cc + c1 + " Food Production" + cc +
"<br />" + c2 + Sci_Bonus_Military + "%" + cc + c1 + " Gains in Combat" + cc +
"<br />" + c2 + Sci_Bonus_Crime + "%" + cc + c1 + " Thievery Effectiveness" + cc +
"<br />" + c2 + Sci_Bonus_Channeling + "%" + cc + c1 + " Magic Effectiveness & Rune Production" + cc +
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
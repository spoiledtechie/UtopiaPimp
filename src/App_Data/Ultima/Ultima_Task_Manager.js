var eol = String.fromCharCode(10); // End Of Line
var Province_Name = "";
var textonly = "";
var content = "";
var EL = 0;

function CB_Find(xcontent, Str, End) {
    var xtemp = xcontent;
    xtemp = xtemp.slice(xtemp.indexOf(Str) + Math.round(Str.length + 1)); xtemp = xtemp.slice(0, xtemp.indexOf(End));
    return Number(xtemp);
}

function Task_Manager() {

    EL = 0;


    content = document.Calculator2.CBtext1.value;

    Task_Military();
    Task_Science();
    Task_Building();
    Task_Thievery();
    Task_Ambush();

    if (content != "") {

        //document.getElementById("flashcopier").innerHTML = "<embed src='input.swf' flashvars='ipt= ' width='400' height='20' type='application/x-shockwave-flash'></embed>";
        document.getElementById("flashcopier").innerHTML = '';


        if ((content.charAt(0) == "C") && (content.charAt(1) == "B")) EL = 1;
        if ((content.charAt(0) == "S") && (content.charAt(1) == "O") && (content.charAt(2) == "S")) EL = 2;
        if ((content.charAt(0) == "S") && (content.charAt(1) == "U") && (content.charAt(2) == "R")) EL = 3;
        if ((content.charAt(0) == "S") && (content.charAt(1) == "O") && (content.charAt(2) == "M")) EL = 4;


        if (EL == 0) {


            if (content.indexOf("Average Opponent") != -1) content = content.replace(/Dark Elf/g, "DarkElf");


            if (content.indexOf("), captured") == -1) content = content.replace(/,/g, "");
            content = content.replace(/	/g, " ");
            content = content.replace(/  /g, " ");

        }

        Clipboard_Throne();    // ******* CB       ******
        Clipboard_Military();  // ******* SOM      ******
        Clipboard_Science();   // ******* Science  ******
        Clipboard_Buildings(); // ******* Survey   ******
        Clipboard_Kingdom();   // ******* Analysis ******
        Clipboard_Uniques();   // ******* Uniques  ******

        document.Calculator2.CBtext1.value = "";

    }

    setTimeout("Task_Manager()", 1000)

}

Task_Manager();
var Months = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December");

function RealTime(UtopiaY, UtopiaM, UtopiaD, UtopiaHH, UtopiaMM) {

    // UtopiaY :: 0-?  :: starting from year 0
    // UtopiaM :: 1-7  :: utopia has only 7 Months in one year
    // UtopiaD :: 1-24 :: there is 24 hours in a day

    var StartD = Number(Age_Start_Date.charAt(0) + Age_Start_Date.charAt(1)); //01.34.6789
    var StartM = Number(Age_Start_Date.charAt(3) + Age_Start_Date.charAt(4));
    var StartY = Number(Age_Start_Date.charAt(6) + Age_Start_Date.charAt(7) + Age_Start_Date.charAt(8) + Age_Start_Date.charAt(9));

    var MaxDay = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
    var RealY = 0;    // RealYear
    var RealM = 0;    // RealMonth
    var RealD = 0;    // RealDay
    var AllDays = UtopiaM - 1 + UtopiaY * 7;
    var DaysIFM = MaxDay[StartM - 1] - StartD + 1; // DaysInFirstMonth

    if (AllDays <= DaysIFM) { RealY = StartY; RealM = StartM; RealD = StartD + AllDays; } else {

        var YCounter = 0;
        var MCounter = 0;
        var DCounter = AllDays - DaysIFM;

        while (DCounter > 0) {
            MCounter = MCounter + 1;
            if (StartM + MCounter > 12) { MCounter = MCounter - 12; YCounter = YCounter + 1; }
            DCounter = DCounter - MaxDay[StartM - 1 + MCounter];
            if (DCounter == 0) { MCounter = MCounter + 1; DCounter = -MaxDay[StartM + MCounter - 1]; }
        }

        RealY = StartY + YCounter;
        RealM = StartM + MCounter;
        RealD = MaxDay[RealM - 1] + DCounter + 1;

    }

    var temp = "th";

    if (RealD == 1) temp = "st";
    if (RealD == 2) temp = "nd";
    if (RealD == 3) temp = "rd";
    if (RealD == 21) temp = "st";
    if (RealD == 22) temp = "nd";
    if (RealD == 23) temp = "rd";
    if (RealD == 31) temp = "st";

    RealD = RealD + temp;

    if (UtopiaMM < 10) UtopiaMM = "0" + UtopiaMM;
    UtopiaHH = UtopiaD - 1;

    return Months[RealM - 1] + " " + RealD + ", " + RealY + " (" + UtopiaHH + ":" + UtopiaMM + " EST/GMT +5)";

}


function openwin1() { document.getElementById('Ultima_floating_window0').style.display = "block"; }
function openwin2() { document.getElementById('Ultima_floating_window1').style.display = "block"; }
function openwin3() { document.getElementById('Ultima_floating_window2').style.display = "block"; }
function openwin4() { document.getElementById('Ultima_floating_window3').style.display = "block"; }
function openwin5() { document.getElementById('Ultima_floating_window4').style.display = "block"; }
function openwin6() { document.getElementById('Ultima_floating_window5').style.display = "block"; }


function copy(inElement) {
    //document.getElementById("flashcopier").innerHTML = '';
    //var str = encodeURIComponent(inElement);
    //var newstring = str.replace(/bijoforever/g, "%0D%0A");
    //var divinfo = '<embed src="clipboard.swf" flashvars="ipt='+newstring+'" width="300" height="20" type="application/x-shockwave-flash"></embed>';
    //document.getElementById("flashcopier").innerHTML = divinfo;

}


function CopyExportLine(inElement) {

    document.getElementById("flashcopier").innerHTML = '';
    var newstring = encodeURIComponent(inElement);
    var divinfo = '<embed src="input.swf" quality="high" flashvars="width=400&ipt=' + newstring + '" wmode="transparent" type="application/x-shockwave-flash" width="400" height="20"></embed>';
    document.getElementById("flashcopier").innerHTML = divinfo;

}

function addCommas(argNum, argThouSeparator, argDecimalPoint) {
    var sThou = (argThouSeparator) ? argThouSeparator : ","
    var sDec = (argDecimalPoint) ? argDecimalPoint : "."
    var aParts = argNum.split(sDec)
    var sInt = aParts[0] + sDec
    var rTest = new RegExp("(\\d)(\\d{3}(\\" + sThou + "|\\" + sDec + "))")
    while (sInt.match(rTest)) { sInt = sInt.replace(rTest, "$1" + sThou + "$2") }
    aParts[0] = sInt.replace(sDec, "")
    return aParts.join(sDec)
}



function Add_Commas(Number) { return addCommas(Number.toString()); }

var a00 = "0";
var a01 = "1";
var a02 = "2";
var a03 = "3";
var a04 = "4";
var a05 = "5";
var a06 = "6";
var a07 = "7";
var a08 = "8";
var a09 = "9";
var a10 = "a";
var a11 = "b";
var a12 = "c";
var a13 = "d";
var a14 = "e";
var a15 = "f";
var a16 = "g";
var a17 = "h";
var a18 = "i";
var a19 = "j";
var a20 = "k";
var a21 = "l";
var a22 = "m";
var a23 = "n";
var a24 = "o";
var a25 = "p";
var a26 = "q";
var a27 = "r";
var a28 = "s";
var a29 = "t";
var a30 = "u";
var a31 = "v";
var a32 = "w";
var a33 = "x";
var a34 = "y";
var a35 = "z";
var a36 = "A";
var a37 = "B";
var a38 = "C";
var a39 = "D";
var a40 = "E";
var a41 = "F";
var a42 = "G";
var a43 = "H";
var a44 = "I";
var a45 = "J";
var a46 = "K";
var a47 = "L";
var a48 = "M";
var a49 = "N";
var a50 = "O";
var a51 = "P";
var a52 = "Q";
var a53 = "R";
var a54 = "S";
var a55 = "T";
var a56 = "U";
var a57 = "V";
var a58 = "W";
var a59 = "X";
var a60 = "Y";
var a61 = "Z";
var a62 = "~";
var a63 = "!";
var a64 = "@";
var a65 = "#";
var a66 = "$";
var a67 = "%";
var a68 = "^";
var a69 = "&";
var a70 = "*";
var a71 = "(";
var a72 = ")";
var a73 = "[";
var a74 = "]";
var a75 = "{";
var a76 = "}";
var a77 = "<";
var a78 = ">";
var a79 = "?";
var a80 = ":";
var a81 = "_";
var a82 = "-";
var a83 = "=";
var a84 = "+";
var a85 = "¢";
var a86 = "|";
var a87 = "¿";
var a88 = ".";
var a89 = ",";
var a90 = "©";
var a91 = "®";
var a92 = "™";
var a93 = "¤";
var a94 = "§";
var a95 = "¥";
var a96 = "«";
var a97 = "»";
var a98 = "£";
var a99 = ";";
var a100 = "¶";
var a101 = "ª";
var a102 = "`";
var a103 = "/";
var a104 = "¡";
var a105 = "¯";
var a106 = "¦";
var a107 = "°";
var a108 = "±";
var a109 = "²";
var a110 = "³";
var a111 = "´";
var a112 = "µ";
var a113 = "·";
var a114 = "¹";
var a115 = "¼";
var a116 = "½";
var a117 = "¾";
var a118 = "¬";
var a119 = "'";
var a120 = "º";

function Export_Line_Encryption(string) {

    var output = "";

    string = string.replace(/_-/g, "__");

    for (i = 0; i < string.length; i++) {

        if (i + 1 == string.length) string += "_";

        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "0")) output += a00;
        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "1")) output += a01;
        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "2")) output += a02;
        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "3")) output += a03;
        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "4")) output += a04;
        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "5")) output += a05;
        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "6")) output += a06;
        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "7")) output += a07;
        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "8")) output += a08;
        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "9")) output += a09;

        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "0")) output += a10;
        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "1")) output += a11;
        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "2")) output += a12;
        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "3")) output += a13;
        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "4")) output += a14;
        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "5")) output += a15;
        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "6")) output += a16;
        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "7")) output += a17;
        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "8")) output += a18;
        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "9")) output += a19;

        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "0")) output += a20;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "1")) output += a21;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "2")) output += a22;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "3")) output += a23;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "4")) output += a24;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "5")) output += a25;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "6")) output += a26;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "7")) output += a27;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "8")) output += a28;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "9")) output += a29;

        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "0")) output += a30;
        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "1")) output += a31;
        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "2")) output += a32;
        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "3")) output += a33;
        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "4")) output += a34;
        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "5")) output += a35;
        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "6")) output += a36;
        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "7")) output += a37;
        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "8")) output += a38;
        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "9")) output += a39;

        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "0")) output += a40;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "1")) output += a41;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "2")) output += a42;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "3")) output += a43;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "4")) output += a44;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "5")) output += a45;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "6")) output += a46;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "7")) output += a47;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "8")) output += a48;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "9")) output += a49;

        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "0")) output += a50;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "1")) output += a51;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "2")) output += a52;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "3")) output += a53;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "4")) output += a54;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "5")) output += a55;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "6")) output += a56;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "7")) output += a57;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "8")) output += a58;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "9")) output += a59;

        if ((string.charAt(i) == "6") && (string.charAt(i + 1) == "0")) output += a60;
        if ((string.charAt(i) == "6") && (string.charAt(i + 1) == "1")) output += a61;
        if ((string.charAt(i) == "6") && (string.charAt(i + 1) == "2")) output += a62;
        if ((string.charAt(i) == "6") && (string.charAt(i + 1) == "3")) output += a63;
        if ((string.charAt(i) == "6") && (string.charAt(i + 1) == "4")) output += a64;
        if ((string.charAt(i) == "6") && (string.charAt(i + 1) == "5")) output += a65;
        if ((string.charAt(i) == "6") && (string.charAt(i + 1) == "6")) output += a66;
        if ((string.charAt(i) == "6") && (string.charAt(i + 1) == "7")) output += a67;
        if ((string.charAt(i) == "6") && (string.charAt(i + 1) == "8")) output += a68;
        if ((string.charAt(i) == "6") && (string.charAt(i + 1) == "9")) output += a69;

        if ((string.charAt(i) == "7") && (string.charAt(i + 1) == "0")) output += a70;
        if ((string.charAt(i) == "7") && (string.charAt(i + 1) == "1")) output += a71;
        if ((string.charAt(i) == "7") && (string.charAt(i + 1) == "2")) output += a72;
        if ((string.charAt(i) == "7") && (string.charAt(i + 1) == "3")) output += a73;
        if ((string.charAt(i) == "7") && (string.charAt(i + 1) == "4")) output += a74;
        if ((string.charAt(i) == "7") && (string.charAt(i + 1) == "5")) output += a75;
        if ((string.charAt(i) == "7") && (string.charAt(i + 1) == "6")) output += a76;
        if ((string.charAt(i) == "7") && (string.charAt(i + 1) == "7")) output += a77;
        if ((string.charAt(i) == "7") && (string.charAt(i + 1) == "8")) output += a78;
        if ((string.charAt(i) == "7") && (string.charAt(i + 1) == "9")) output += a79;

        if ((string.charAt(i) == "8") && (string.charAt(i + 1) == "0")) output += a80;
        if ((string.charAt(i) == "8") && (string.charAt(i + 1) == "1")) output += a81;
        if ((string.charAt(i) == "8") && (string.charAt(i + 1) == "2")) output += a82;
        if ((string.charAt(i) == "8") && (string.charAt(i + 1) == "3")) output += a83;
        if ((string.charAt(i) == "8") && (string.charAt(i + 1) == "4")) output += a84;
        if ((string.charAt(i) == "8") && (string.charAt(i + 1) == "5")) output += a85;
        if ((string.charAt(i) == "8") && (string.charAt(i + 1) == "6")) output += a86;
        if ((string.charAt(i) == "8") && (string.charAt(i + 1) == "7")) output += a87;
        if ((string.charAt(i) == "8") && (string.charAt(i + 1) == "8")) output += a88;
        if ((string.charAt(i) == "8") && (string.charAt(i + 1) == "9")) output += a89;

        if ((string.charAt(i) == "9") && (string.charAt(i + 1) == "0")) output += a90;
        if ((string.charAt(i) == "9") && (string.charAt(i + 1) == "1")) output += a91;
        if ((string.charAt(i) == "9") && (string.charAt(i + 1) == "2")) output += a92;
        if ((string.charAt(i) == "9") && (string.charAt(i + 1) == "3")) output += a93;
        if ((string.charAt(i) == "9") && (string.charAt(i + 1) == "4")) output += a94;
        if ((string.charAt(i) == "9") && (string.charAt(i + 1) == "5")) output += a95;
        if ((string.charAt(i) == "9") && (string.charAt(i + 1) == "6")) output += a96;
        if ((string.charAt(i) == "9") && (string.charAt(i + 1) == "7")) output += a97;
        if ((string.charAt(i) == "9") && (string.charAt(i + 1) == "8")) output += a98;
        if ((string.charAt(i) == "9") && (string.charAt(i + 1) == "9")) output += a99;

        if ((string.charAt(i) == "_") && (string.charAt(i + 1) == "0")) output += a100;
        if ((string.charAt(i) == "_") && (string.charAt(i + 1) == "1")) output += a101;
        if ((string.charAt(i) == "_") && (string.charAt(i + 1) == "2")) output += a102;
        if ((string.charAt(i) == "_") && (string.charAt(i + 1) == "3")) output += a103;
        if ((string.charAt(i) == "_") && (string.charAt(i + 1) == "4")) output += a104;
        if ((string.charAt(i) == "_") && (string.charAt(i + 1) == "5")) output += a105;
        if ((string.charAt(i) == "_") && (string.charAt(i + 1) == "6")) output += a106;
        if ((string.charAt(i) == "_") && (string.charAt(i + 1) == "7")) output += a107;
        if ((string.charAt(i) == "_") && (string.charAt(i + 1) == "8")) output += a108;
        if ((string.charAt(i) == "_") && (string.charAt(i + 1) == "9")) output += a109;

        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "_")) output += a110;
        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "_")) output += a111;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "_")) output += a112;
        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "_")) output += a113;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "_")) output += a114;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "_")) output += a115;
        if ((string.charAt(i) == "6") && (string.charAt(i + 1) == "_")) output += a116;
        if ((string.charAt(i) == "7") && (string.charAt(i + 1) == "_")) output += a117;
        if ((string.charAt(i) == "8") && (string.charAt(i + 1) == "_")) output += a118;
        if ((string.charAt(i) == "9") && (string.charAt(i + 1) == "_")) output += a119;

        if ((string.charAt(i) == "_") && (string.charAt(i + 1) == "_")) output += a120;

        i++;

    }

    return output;

}




function Export_Line_Decryption(string) {

    var output = "";

    for (i = 0; i < string.length; i++) {

        if (string.charAt(i) == a00) output += "00";
        if (string.charAt(i) == a01) output += "01";
        if (string.charAt(i) == a02) output += "02";
        if (string.charAt(i) == a03) output += "03";
        if (string.charAt(i) == a04) output += "04";
        if (string.charAt(i) == a05) output += "05";
        if (string.charAt(i) == a06) output += "06";
        if (string.charAt(i) == a07) output += "07";
        if (string.charAt(i) == a08) output += "08";
        if (string.charAt(i) == a09) output += "09";

        if (string.charAt(i) == a10) output += "10";
        if (string.charAt(i) == a11) output += "11";
        if (string.charAt(i) == a12) output += "12";
        if (string.charAt(i) == a13) output += "13";
        if (string.charAt(i) == a14) output += "14";
        if (string.charAt(i) == a15) output += "15";
        if (string.charAt(i) == a16) output += "16";
        if (string.charAt(i) == a17) output += "17";
        if (string.charAt(i) == a18) output += "18";
        if (string.charAt(i) == a19) output += "19";

        if (string.charAt(i) == a20) output += "20";
        if (string.charAt(i) == a21) output += "21";
        if (string.charAt(i) == a22) output += "22";
        if (string.charAt(i) == a23) output += "23";
        if (string.charAt(i) == a24) output += "24";
        if (string.charAt(i) == a25) output += "25";
        if (string.charAt(i) == a26) output += "26";
        if (string.charAt(i) == a27) output += "27";
        if (string.charAt(i) == a28) output += "28";
        if (string.charAt(i) == a29) output += "29";

        if (string.charAt(i) == a30) output += "30";
        if (string.charAt(i) == a31) output += "31";
        if (string.charAt(i) == a32) output += "32";
        if (string.charAt(i) == a33) output += "33";
        if (string.charAt(i) == a34) output += "34";
        if (string.charAt(i) == a35) output += "35";
        if (string.charAt(i) == a36) output += "36";
        if (string.charAt(i) == a37) output += "37";
        if (string.charAt(i) == a38) output += "38";
        if (string.charAt(i) == a39) output += "39";

        if (string.charAt(i) == a40) output += "40";
        if (string.charAt(i) == a41) output += "41";
        if (string.charAt(i) == a42) output += "42";
        if (string.charAt(i) == a43) output += "43";
        if (string.charAt(i) == a44) output += "44";
        if (string.charAt(i) == a45) output += "45";
        if (string.charAt(i) == a46) output += "46";
        if (string.charAt(i) == a47) output += "47";
        if (string.charAt(i) == a48) output += "48";
        if (string.charAt(i) == a49) output += "49";

        if (string.charAt(i) == a50) output += "50";
        if (string.charAt(i) == a51) output += "51";
        if (string.charAt(i) == a52) output += "52";
        if (string.charAt(i) == a53) output += "53";
        if (string.charAt(i) == a54) output += "54";
        if (string.charAt(i) == a55) output += "55";
        if (string.charAt(i) == a56) output += "56";
        if (string.charAt(i) == a57) output += "57";
        if (string.charAt(i) == a58) output += "58";
        if (string.charAt(i) == a59) output += "59";

        if (string.charAt(i) == a60) output += "60";
        if (string.charAt(i) == a61) output += "61";
        if (string.charAt(i) == a62) output += "62";
        if (string.charAt(i) == a63) output += "63";
        if (string.charAt(i) == a64) output += "64";
        if (string.charAt(i) == a65) output += "65";
        if (string.charAt(i) == a66) output += "66";
        if (string.charAt(i) == a67) output += "67";
        if (string.charAt(i) == a68) output += "68";
        if (string.charAt(i) == a69) output += "69";

        if (string.charAt(i) == a70) output += "70";
        if (string.charAt(i) == a71) output += "71";
        if (string.charAt(i) == a72) output += "72";
        if (string.charAt(i) == a73) output += "73";
        if (string.charAt(i) == a74) output += "74";
        if (string.charAt(i) == a75) output += "75";
        if (string.charAt(i) == a76) output += "76";
        if (string.charAt(i) == a77) output += "77";
        if (string.charAt(i) == a78) output += "78";
        if (string.charAt(i) == a79) output += "79";

        if (string.charAt(i) == a80) output += "80";
        if (string.charAt(i) == a81) output += "81";
        if (string.charAt(i) == a82) output += "82";
        if (string.charAt(i) == a83) output += "83";
        if (string.charAt(i) == a84) output += "84";
        if (string.charAt(i) == a85) output += "85";
        if (string.charAt(i) == a86) output += "86";
        if (string.charAt(i) == a87) output += "87";
        if (string.charAt(i) == a88) output += "88";
        if (string.charAt(i) == a89) output += "89";

        if (string.charAt(i) == a90) output += "90";
        if (string.charAt(i) == a91) output += "91";
        if (string.charAt(i) == a92) output += "92";
        if (string.charAt(i) == a93) output += "93";
        if (string.charAt(i) == a94) output += "94";
        if (string.charAt(i) == a95) output += "95";
        if (string.charAt(i) == a96) output += "96";
        if (string.charAt(i) == a97) output += "97";
        if (string.charAt(i) == a98) output += "98";
        if (string.charAt(i) == a99) output += "99";

        if (string.charAt(i) == a100) output += "_0";
        if (string.charAt(i) == a101) output += "_1";
        if (string.charAt(i) == a102) output += "_2";
        if (string.charAt(i) == a103) output += "_3";
        if (string.charAt(i) == a104) output += "_4";
        if (string.charAt(i) == a105) output += "_5";
        if (string.charAt(i) == a106) output += "_6";
        if (string.charAt(i) == a107) output += "_7";
        if (string.charAt(i) == a108) output += "_8";
        if (string.charAt(i) == a109) output += "_9";

        if (string.charAt(i) == a120) output += "__";

        if (i + 1 == string.length) {

            if (string.charAt(i) == a110) output += "0";
            if (string.charAt(i) == a111) output += "1";
            if (string.charAt(i) == a112) output += "2";
            if (string.charAt(i) == a113) output += "3";
            if (string.charAt(i) == a114) output += "4";
            if (string.charAt(i) == a115) output += "5";
            if (string.charAt(i) == a116) output += "6";
            if (string.charAt(i) == a117) output += "7";
            if (string.charAt(i) == a118) output += "8";
            if (string.charAt(i) == a119) output += "9";

        } else {

            if (string.charAt(i) == a110) output += "0_";
            if (string.charAt(i) == a111) output += "1_";
            if (string.charAt(i) == a112) output += "2_";
            if (string.charAt(i) == a113) output += "3_";
            if (string.charAt(i) == a114) output += "4_";
            if (string.charAt(i) == a115) output += "5_";
            if (string.charAt(i) == a116) output += "6_";
            if (string.charAt(i) == a117) output += "7_";
            if (string.charAt(i) == a118) output += "8_";
            if (string.charAt(i) == a119) output += "9_";

        }


    }


    output = output.replace(/__/g, "_-");

    return output;

}
















function ABC_Decryption(string) {

    var output = "";

    for (i = 0; i < string.length; i++) {


        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "0")) output += a00;
        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "1")) output += a01;
        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "2")) output += a02;
        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "3")) output += a03;
        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "4")) output += a04;
        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "5")) output += a05;
        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "6")) output += a06;
        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "7")) output += a07;
        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "8")) output += a08;
        if ((string.charAt(i) == "0") && (string.charAt(i + 1) == "9")) output += a09;


        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "0")) output += a10;
        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "1")) output += a11;
        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "2")) output += a12;
        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "3")) output += a13;
        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "4")) output += a14;
        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "5")) output += a15;
        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "6")) output += a16;
        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "7")) output += a17;
        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "8")) output += a18;
        if ((string.charAt(i) == "1") && (string.charAt(i + 1) == "9")) output += a19;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "0")) output += a20;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "1")) output += a21;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "2")) output += a22;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "3")) output += a23;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "4")) output += a24;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "5")) output += a25;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "6")) output += a26;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "7")) output += a27;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "8")) output += a28;
        if ((string.charAt(i) == "2") && (string.charAt(i + 1) == "9")) output += a29;
        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "0")) output += a30;
        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "1")) output += a31;
        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "2")) output += a32;
        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "3")) output += a33;
        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "4")) output += a34;
        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "5")) output += a35;


        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "6")) output += a36;
        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "7")) output += a37;
        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "8")) output += a38;
        if ((string.charAt(i) == "3") && (string.charAt(i + 1) == "9")) output += a39;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "0")) output += a40;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "1")) output += a41;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "2")) output += a42;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "3")) output += a43;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "4")) output += a44;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "5")) output += a45;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "6")) output += a46;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "7")) output += a47;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "8")) output += a48;
        if ((string.charAt(i) == "4") && (string.charAt(i + 1) == "9")) output += a49;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "0")) output += a50;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "1")) output += a51;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "2")) output += a52;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "3")) output += a53;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "4")) output += a54;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "5")) output += a55;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "6")) output += a56;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "7")) output += a57;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "8")) output += a58;
        if ((string.charAt(i) == "5") && (string.charAt(i + 1) == "9")) output += a59;
        if ((string.charAt(i) == "6") && (string.charAt(i + 1) == "0")) output += a60;
        if ((string.charAt(i) == "6") && (string.charAt(i + 1) == "1")) output += a61;


        if ((string.charAt(i) == "6") && (string.charAt(i + 1) == "2")) output += " ";
        if ((string.charAt(i) == "6") && (string.charAt(i + 1) == "3")) output += "_";
        if ((string.charAt(i) == "6") && (string.charAt(i + 1) == "4")) output += "-";

        i++;

    }

    return output;

}




function ABC_Encryption(string) {

    var output = "";

    for (i = 0; i < string.length; i++) {


        if (string.charAt(i) == a00) output += "00";
        if (string.charAt(i) == a01) output += "01";
        if (string.charAt(i) == a02) output += "02";
        if (string.charAt(i) == a03) output += "03";
        if (string.charAt(i) == a04) output += "04";
        if (string.charAt(i) == a05) output += "05";
        if (string.charAt(i) == a06) output += "06";
        if (string.charAt(i) == a07) output += "07";
        if (string.charAt(i) == a08) output += "08";
        if (string.charAt(i) == a09) output += "09";


        if (string.charAt(i) == a10) output += "10";
        if (string.charAt(i) == a11) output += "11";
        if (string.charAt(i) == a12) output += "12";
        if (string.charAt(i) == a13) output += "13";
        if (string.charAt(i) == a14) output += "14";
        if (string.charAt(i) == a15) output += "15";
        if (string.charAt(i) == a16) output += "16";
        if (string.charAt(i) == a17) output += "17";
        if (string.charAt(i) == a18) output += "18";
        if (string.charAt(i) == a19) output += "19";
        if (string.charAt(i) == a20) output += "20";
        if (string.charAt(i) == a21) output += "21";
        if (string.charAt(i) == a22) output += "22";
        if (string.charAt(i) == a23) output += "23";
        if (string.charAt(i) == a24) output += "24";
        if (string.charAt(i) == a25) output += "25";
        if (string.charAt(i) == a26) output += "26";
        if (string.charAt(i) == a27) output += "27";
        if (string.charAt(i) == a28) output += "28";
        if (string.charAt(i) == a29) output += "29";
        if (string.charAt(i) == a30) output += "30";
        if (string.charAt(i) == a31) output += "31";
        if (string.charAt(i) == a32) output += "32";
        if (string.charAt(i) == a33) output += "33";
        if (string.charAt(i) == a34) output += "34";
        if (string.charAt(i) == a35) output += "35";


        if (string.charAt(i) == a36) output += "36";
        if (string.charAt(i) == a37) output += "37";
        if (string.charAt(i) == a38) output += "38";
        if (string.charAt(i) == a39) output += "39";
        if (string.charAt(i) == a40) output += "40";
        if (string.charAt(i) == a41) output += "41";
        if (string.charAt(i) == a42) output += "42";
        if (string.charAt(i) == a43) output += "43";
        if (string.charAt(i) == a44) output += "44";
        if (string.charAt(i) == a45) output += "45";
        if (string.charAt(i) == a46) output += "46";
        if (string.charAt(i) == a47) output += "47";
        if (string.charAt(i) == a48) output += "48";
        if (string.charAt(i) == a49) output += "49";
        if (string.charAt(i) == a50) output += "50";
        if (string.charAt(i) == a51) output += "51";
        if (string.charAt(i) == a52) output += "52";
        if (string.charAt(i) == a53) output += "53";
        if (string.charAt(i) == a54) output += "54";
        if (string.charAt(i) == a55) output += "55";
        if (string.charAt(i) == a56) output += "56";
        if (string.charAt(i) == a57) output += "57";
        if (string.charAt(i) == a58) output += "58";
        if (string.charAt(i) == a59) output += "59";
        if (string.charAt(i) == a60) output += "60";
        if (string.charAt(i) == a61) output += "61";


        if (string.charAt(i) == " ") output += "62";
        if (string.charAt(i) == "_") output += "63";
        if (string.charAt(i) == "-") output += "64";

    }

    return output;

}



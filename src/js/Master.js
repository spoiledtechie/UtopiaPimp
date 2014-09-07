/// <reference path="jquery-1.3.intellisense.js" />
//Adds info from the submit data box.
function AddDatajs() {
    document.getElementById("divShowError").innerHTML = 'Working...';
    $("#divShowError").toggleClass("popUp");
    if (document.getElementById("tbAddInfo").value.length > 10) {
        if (document.getElementById("divGuids") != null)
            MainMaster.AddItem(document.getElementById("tbAddInfo").value, '', document.getElementById("divGuids").innerHTML, OnWSAddDataComplete);
        else
            MainMaster.AddItem(document.getElementById("tbAddInfo").value, '', '', OnWSAddDataComplete);
    }
    else {
        document.getElementById("divShowError").innerHTML = "Nothing Submitted";
        setTimeout('CloseWarningPopup()', 10000);
    }
}
//clears the submit info box.
function ClearBox() {
    document.getElementById("tbAddInfo").value = '';
    document.getElementById("tbAddInfo").focus();
}
//Shows the results.
function OnWSAddDataComplete(results) {
    var sub = results.substring(0, 2);
    if (sub == "0,") { //it Failed
        document.getElementById("divShowError").innerHTML = results.substring(2);
        setTimeout('CloseWarningPopup()', 10000);
    }
    else { //it didn't fail.
        document.getElementById("divShowError").innerHTML = results;
        if (location.href.indexOf("kd.aspx", 0) > 1 | location.href.indexOf("ProvinceDetail.aspx", 0) > 1)
            window.location = location.href;
    }
    ClearBox();
}

function CloseWarningPopup(start) {
    $("#divShowError").toggleClass("popUp");
}

function GetTimeLeftForPopup() {
    var minutes = Math.floor(window.start / 60);
    var secs = Math.floor(window.start - (minutes * 60));
    document.getElementById('timeLeft').innerHTML = minutes + "m " + secs + "s";
    window.start -= 1;
    if (window.start != 0)
        GetTimeLeftInTick_Start(window.start);
    else
        MainMaster.RefreshUtopianTime(OnWSRefreshUtopianTimeComplete);
}


function showLegendAddData() {
    $("#divAddDataLegend").toggleClass("popUp");
}
//Retires a Kingdom...
function RetireKingdom(ownerKDID, kdID) {
    MainMaster.RetireKd(ownerKDID, kdID, OnWSRetireKingdomComplete);
}
function OnWSRetireKingdomComplete(results) {
    window.location = location.href;
}

//retires kingdom on the monarch retire page.
function RetireKingdomMulti(ownerKDID, kdID) {
    var obj = $("#RetireKingdom" + kdID);
    // alert(obj);
    if (obj.html() == "Retire") {
        //alert("un");
        obj.attr('innerHTML', 'Un-Retire');
        MainMaster.RetireKd(ownerKDID, kdID);
    }
    else {
        //alert("retire");
        obj.attr('innerHTML', 'Retire');
        MainMaster.UnRetireKd(ownerKDID, kdID);
    }
}

function UpdateKingdomStatus(ownerKDID, kdID) {
    var obj = $("#status" + kdID);
    //alert(obj.val());
    MainMaster.UpdateKingdomStatus(ownerKDID, kdID, obj.val());
    $("#updateStatus" + kdID).attr('value', 'UPDATED');
}


function getTopAd(signedIn) {
    document.getElementById("divAddTop").innerHTML = "<map name='admap37989' id='admap37989' ><area href='http://www.projectwonderful.com/out_nojs.php?r=0&amp;c=0&amp;id=37989&amp;type=5&amp;tag=21916' shape='rect' coords='0,0,728,90' title='' alt='' target='_blank' /></map><table cellpadding='0' border='0' cellspacing='0' width='728' bgcolor=''><tr><td><a href='http://www.projectwonderful.com/out_nojs.php?r=0&amp;c=0&amp;id=37989&amp;type=5' target='_blank'><img src='http://www.projectwonderful.com/nojs.php?id=37989&amp;type=5' width='728' height='90'  border='0' alt='' /></a></td></tr><tr><td bgcolor='' colspan='1'><center><a style='font-size:10px;color:#fff;text-decoration:none;line-height:1.2;font-weight:bold;font-family:Tahoma, verdana,arial,helvetica,sans-serif;text-transform: none;letter-spacing:normal;text-shadow:none;white-space:normal;word-spacing:normal;' href='http://www.projectwonderful.com/advertisehere.php?id=37989&amp;type=5' target='_blank'>Your ad could be here, right now.</a></center></td></tr></table>";
    if (signedIn === "true")
        setTimeout('getTopAd("true")', 7000);
}
function getSideAd() {
    document.getElementById("divAdRight").innerHTML = '<map name="admap38158" id="admap38158"><area href="http://www.projectwonderful.com/out_nojs.php?r=0&amp;c=0&amp;id=38158&amp;type=3&amp;tag=21916" shape="rect" coords="0,0,160,600" title="" alt="" target="_blank" /></map><table cellpadding="0" border="0" cellspacing="0" width="160" bgcolor=""><tr><td><a href="http://www.projectwonderful.com/out_nojs.php?r=0&amp;c=0&amp;id=38158&amp;type=3" target="_blank"> <img src="http://www.projectwonderful.com/nojs.php?id=38158&amp;type=3" width="160" height="600" usemap="#admap38158" border="0" alt="" /></a></td></tr><tr><td width="160" bgcolor="" colspan="1"><center><a style="font-size:10px;color:#FFF;text-decoration:none;line-height:1.2;font-weight:bold;font-family:Tahoma, verdana,arial,helvetica,sans-serif;text-transform: none;letter-spacing:normal;text-shadow:none;white-space:normal;word-spacing:normal;" href="http://www.projectwonderful.com/advertisehere.php?id=38158&amp;type=3" target="_blank">Your ad could be here, right now.</a></center></td></tr></table><map name="admap38154" id="admap38154"><area href="http://www.projectwonderful.com/out_nojs.php?r=0&amp;c=0&amp;id=38154&amp;type=2" shape="rect" coords="0,0,93,24" title="" alt="" target="_blank" /></map><table cellpadding="0" border="0" cellspacing="0" width="93" bgcolor="#333"><tr><td><a href="http://www.projectwonderful.com/out_nojs.php?r=0&amp;c=0&amp;id=38154&amp;type=2" target="blank" ><img src="http://www.projectwonderful.com/nojs.php?id=38154&amp;type=2" width="93" height="24" usemap="#admap38154" border="0" alt="" /></a></td></tr></table>';
    setTimeout('getSideAd()', 7000);
}
function getButtonAd(elemName) {
    document.getElementById(elemName).innerHTML = '<map name="admap38154" id="admap38154"><area href="http://www.projectwonderful.com/out_nojs.php?r=0&amp;c=0&amp;id=38154&amp;type=2&amp;tag=21916" shape="rect" coords="0,0,93,24" title="" alt="" target="_blank" /></map><table cellpadding="0" border="0" cellspacing="0" width="93" bgcolor="#333"><tr><td><a href="http://www.projectwonderful.com/out_nojs.php?r=0&amp;c=0&amp;id=38154&amp;type=2" target="blank" ><img src="http://www.projectwonderful.com/nojs.php?id=38154&amp;type=2" width="93" height="24" usemap="#admap38154" border="0" alt="" /></a></td></tr></table>';
    setTimeout("getButtonAd('" + elemName + "')", 7000);
}
function OnWSRefreshAdd(results) {
    document.getElementById("divAddTop").innerHTML = results;
    var pw_d = document;
    pw_d.projectwonderful_adbox_id = "37989";
    pw_d.projectwonderful_adbox_type = "5";
    pw_d.projectwonderful_foreground_color = "";
    pw_d.projectwonderful_background_color = "";

    setTimeout('getAdd()', 7000);

}

function GetTimeLeftInTick_Start(start) {
    window.start = parseFloat(start);
    setTimeout('GetTimeLeftInTick()', 1000)
}

function GetTimeLeftInTick() {
    var minutes = Math.floor(window.start / 60);
    var secs = Math.floor(window.start - (minutes * 60));
    document.getElementById('timeLeft').innerHTML = minutes + "m " + secs + "s";
    window.start -= 1;
    if (window.start != 0)
        GetTimeLeftInTick_Start(window.start);
    else
        MainMaster.RefreshUtopianTime(OnWSRefreshUtopianTimeComplete);
}
function OnWSRefreshUtopianTimeComplete(results) {
    document.getElementById('liUtopianDateTime').innerHTML = results[0];
    GetTimeLeftInTick_Start(results[1]);
}

function LoadKingdomSummary() {
    if (getParameterByName('kdid').length === 0) {
        MainMaster.LoadKingdomSummary(document.getElementById('divOwnerID').innerHTML, document.getElementById('divOwnerID').innerHTML, document.getElementById('divProvinceID').innerHTML, document.getElementById('divUserID').innerHTML, OnWSKingdomSummary);
    }
    else {
        MainMaster.LoadKingdomSummary(getParameterByName('kdid'), document.getElementById('divOwnerID').innerHTML, document.getElementById('divProvinceID').innerHTML, document.getElementById('divUserID').innerHTML, OnWSKingdomSummary);
    }
}
function OnWSKingdomSummary(results) {
    document.getElementById('kingdomSum').innerHTML = results;
}

function LoadOpHistroySummary() {
    MainMaster.LoadOpsSummary(getParameterByName('kdty'), getParameterByName('kdid'), document.getElementById('divOwnerID').innerHTML, OnWSOpHistroySummary);
}
function OnWSOpHistroySummary(results) {
    document.getElementById('pnlOpHistory').innerHTML = results;
}
function LoadKingdomGrid() {
    MainMaster.LoadKingdomGrid(getParameterByName('kdty'), getParameterByName('st'), getParameterByName('cs'), getParameterByName('kdid'), document.getElementById('divOwnerID').innerHTML, OnWSKingdomGrid);
}
function OnWSKingdomGrid(results) {
    document.getElementById('pnlKDData').innerHTML = results;
    if (getParameterByName('cs').length === 0) {
        $("#tblKingdomInfo").tablesorter({ widgets: ['zebra'], widgetZebra: { css: ['d0', 'd1']} }).tableHover({ clickFunction: 'IdentifyProvince', clickClass: 'click' });
    }
    else {
        var count = document.getElementById('tblKingdomInfo').getElementsByTagName('tr')[0].getElementsByTagName('th').length;
        $("#tblKingdomInfo").tablesorter({ sortList: [[(count - 1), 0]], widgets: ['zebra'], widgetZebra: { css: ['d0', 'd1']} }).tableHover({ clickFunction: 'IdentifyProvince', clickClass: 'click' });
    }
}

//activity Ops and logs.
function showOps(provID, kingID) {
    ActivityLog.GetOps(provID, kingID, OnWSFinishActivityLog);
}
function OnWSFinishActivityLog(results) {
    document.getElementById("divOps").innerHTML = results;
}


function cexport(ctrl) {
    if (window.clipboardData)
        window.clipboardData.setData("Text", ctrl.innerText);
}
function AddNote(provId, ctrlID) {
    KDPage.AddNote(provId, document.getElementById(ctrlID).value, document.getElementById('divOwnerID').innerHTML, document.getElementById('divProvinceID').innerHTML);
    SetTip();
}
var imgID;
function HandleMouseOver(provID) {
    imgID = provID;
    toolTip('Loading...');
    KDPage.GetProvinceNote(provID, document.getElementById('divOwnerID').innerHTML, OnNotesComplete);
}

function OnNotesComplete(results) {
    if (results.length < 5)
        document.getElementById('toolTipSingle').innerHTML = "N/A";
    else
        document.getElementById('toolTipSingle').innerHTML = results;
}

function UpdateMonarchMessage() {
    document.getElementById('loadMonarchInfo').innerHTML = "<textarea id='taMonarchMess' cols='20' rows='2'></textarea><input id='btnChangeMessage' onclick='javascript:ChangeMonarchMessage();' type='button' value='Submit' />";
}

function ChangeMonarchMessage() {
    KDPage.ChangeMonarchMessageKd(document.getElementById('taMonarchMess').value, document.getElementById('divKingdomID').innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSMonarchMessageChangeComplete);
}
function OnWSMonarchMessageChangeComplete(results) {
    document.getElementById('divMonarchMess').innerHTML = results;
}


function DeleteKdLessProvince(kdProvID) {
    KDPage.DeleteKDLessProv(kdProvID, document.getElementById('divOwnerID').innerHTML, OnWSDeleteComplete);
}
function OnWSDeleteComplete(results) {
    window.location.reload();
}
//requests intel by making a server call and adding a question mark on the province.
function RequestIntel(ctrl, type, provID) {
    if (ctrl.innerHTML === "?") {
        gblctrl = ctrl;
        KDPage.DeleteRequestedIntelKd(provID, type, document.getElementById('divOwnerID').innerHTML, OnWSDeleteRequestedIntelComplete);
    }
    else {
        ctrl.innerHTML = "?";
        KDPage.RequestIntelKd(provID, type);
    }
}
var gblctrl;
function OnWSDeleteRequestedIntelComplete(results) {
    if (results === false)
        gblctrl.innerHTML = "?";
    else
        gblctrl.innerHTML = "";
}
function FilterStaValues(ddlNet, ddlAcres, ddlUpdated, setID) {
    var net = document.getElementById(ddlNet);
    var acres = document.getElementById(ddlAcres);
    var updated = document.getElementById(ddlUpdated)

    KDPage.FilterStandardInfo(net.options[net.selectedIndex].value, acres.options[acres.selectedIndex].value, updated.options[updated.selectedIndex].value, setID, OnFilterStatusReturned);
}
function FilterInpValues(netMin, netMax, acresMin, acresMax, updatedMin, updatedMax, setID) {
    KDPage.FilterInpInfo(document.getElementById(netMin).value, document.getElementById(netMax).value, document.getElementById(acresMin).value, document.getElementById(acresMax).value, document.getElementById(updatedMin).value, document.getElementById(updatedMax).value, setID, document.getElementById('divOwnerID').innerHTML, OnFilterStatusReturned);
}
function OnFilterStatusReturned(results) {
    document.getElementById("pnlKDData").innerHTML = results;
    $("#tblKingdomInfo").tablesorter();
}


function SetTip(provID, name) {
    if (provID != null) {
        if (document.getElementById) {
            addToolStyle = document.getElementById("toolTipAdd").style;
        }
        if (is_ie || is_nav6up) {
            addToolStyle.visibility = "visible";
            addToolStyle.display = "none";
        }

        addToolStyle.left = toolTipSTYLE.left;
        addToolStyle.top = toolTipSTYLE.top;

        var content = "<div class=\"toolTipAdd\">Add Note for " + name + "<textarea id=\"taToolTip\" cols=\"20\" rows=\"2\">"
        + "</textarea><br /><input type=\"button\" id=\"toolButton\" value=\"Submit\"/ onclick=\"AddNote('" + provID + "','taToolTip')\">"
        + "<input type=\"button\" id=\"toolButtonC\" value=\"Cancel\"/ onclick=\"SetTip()\"><hr /><div id=\"toolTipDataUpdate\"></div></div>";

        KDPage.GetProvinceNoteDelete(provID, document.getElementById('divOwnerID').innerHTML, OnNotesDataComplete);

        var ctrl = document.getElementById('toolTipAdd');
        if (is_nav4) {
            addToolStyle.document.write(content);
            addToolStyle.document.close();
            addToolStyle.visibility = "visible";

        }

        else if (is_ie || is_nav6up) {
            document.getElementById("toolTipAdd").innerHTML = content;
            addToolStyle.display = 'block'
        }
    }
    else {
        if (is_nav4) {
            addToolStyle.visibility = "invisible";
        }
        else if (is_ie || is_nav6up) {
            addToolStyle.display = 'none'
        }
    }
}
//Deletes the Province Note.
function DeleteProvNote(noteID) {
    KDPage.DeleteProvinceNote(noteID, document.getElementById('divOwnerID').innerHTML, OnNotesDataComplete);
}

function OnNotesDataComplete(results) {
    if (results.length < 5)
        document.getElementById('toolTipDataUpdate').innerHTML = "N/A";
    else
        document.getElementById('toolTipDataUpdate').innerHTML = results;
}

//CE Chooser Code
//Choosing which kingdom to use.
function CEChooseKingdom(ctrl) {
    $(ctrl).toggleClass('ColumnOn');
    $(document.getElementById("akingdom")).toggleClass('ColumnOn');
    CEChooser.SetCE(document.getElementById("amonth").name, document.getElementById("ayear").name, ctrl.name, document.getElementById("CEID").innerHTML, document.getElementById("divOwnerID").innerHTML, document.getElementById("divUserID").innerHTML, OnWSCEComplete);
}
//Choosing which month to use.
function CEChooseMonth(ctrl) {
    $(ctrl).toggleClass('ColumnOn');
    $(document.getElementById("amonth")).toggleClass('ColumnOn');
    if (document.getElementById("akingdom") != null)
        CEChooser.SetCE(ctrl.name, document.getElementById("ayear").name, document.getElementById("akingdom").name, document.getElementById("CEID").innerHTML, document.getElementById("divOwnerID").innerHTML, document.getElementById("divUserID").innerHTML, OnWSCEComplete);
    else
        CEChooser.SetCE(ctrl.name, document.getElementById("ayear").name, "", document.getElementById("CEID").innerHTML, document.getElementById("divOwnerID").innerHTML, document.getElementById("divUserID").innerHTML, OnWSCEComplete);
}
//chosing which year to use.
function CEChooseYear(ctrl) {
    $(ctrl).toggleClass('ColumnOn');
    $(document.getElementById("ayear")).toggleClass('ColumnOn');
    if (document.getElementById("akingdom") != null)
        CEChooser.SetCE(document.getElementById("amonth").name, ctrl.name, document.getElementById("akingdom").name, document.getElementById("CEID").innerHTML, document.getElementById("divOwnerID").innerHTML, document.getElementById("divUserID").innerHTML, OnWSCEComplete);
    else
        CEChooser.SetCE(document.getElementById("amonth").name, ctrl.name, "", document.getElementById("CEID").innerHTML, document.getElementById("divOwnerID").innerHTML, document.getElementById("divUserID").innerHTML, OnWSCEComplete);
}
//choosing which kingdom on Province Personal History
function CEChooseKingdomPersonal(ctrl) {
    $(ctrl).toggleClass('ColumnOn');
    $(document.getElementById("akingdom")).toggleClass('ColumnOn');
    CEChooser.SetCEPersonal(document.getElementById("amonth").name, document.getElementById("ayear").name, ctrl.name, document.getElementById("CEID").innerHTML, document.getElementById("divProvName").innerHTML, document.getElementById("divOwnerID").innerHTML, document.getElementById("divUserID").innerHTML, OnWSCEComplete);
}
//Choosing Which month on Province Personal History
function CEChooseMonthPersonal(ctrl) {
    $(ctrl).toggleClass('ColumnOn');
    $(document.getElementById("amonth")).toggleClass('ColumnOn');
    if (document.getElementById("akingdom") != null)
        CEChooser.SetCEPersonal(ctrl.name, document.getElementById("ayear").name, document.getElementById("akingdom").name, document.getElementById("CEID").innerHTML, document.getElementById("divProvName").innerHTML, document.getElementById("divOwnerID").innerHTML, document.getElementById("divUserID").innerHTML, OnWSCEComplete);
    else
        CEChooser.SetCEPersonal(ctrl.name, document.getElementById("ayear").name, "", document.getElementById("CEID").innerHTML, document.getElementById("divProvName").innerHTML, document.getElementById("divOwnerID").innerHTML, OnWSCEComplete);
}
//Choosing which year on Province Personal History
function CEChooseYearPersonal(ctrl) {
    $(ctrl).toggleClass('ColumnOn');
    $(document.getElementById("ayear")).toggleClass('ColumnOn');
    if (document.getElementById("akingdom") != null)
        CEChooser.SetCEPersonal(document.getElementById("amonth").name, ctrl.name, document.getElementById("akingdom").name, document.getElementById("CEID").innerHTML, document.getElementById("divProvName").innerHTML, document.getElementById("divOwnerID").innerHTML, OnWSCEComplete);
    else
        CEChooser.SetCEPersonal(document.getElementById("amonth").name, ctrl.name, "", document.getElementById("CEID").innerHTML, document.getElementById("divProvName").innerHTML, document.getElementById("divOwnerID").innerHTML, OnWSCEComplete);
}
function OnWSCEComplete(results) {
    document.getElementById("divCEContents").innerHTML = results;
}
function CELoadModal(location, month, year, kingdomIL, CEID, kingdomID) {
    $('#dialog').dialog('open');
    CEChooser.SetCEModalInfo(location, month, year, kingdomIL, CEID, kingdomID, document.getElementById("divUserID").innerHTML, OnWSModalComplete);
}
function ExportModalPopup(location, month, year, kingdomIL, CEID, kingdomID) {
    CEChooser.ExportCEModalInfo(location, month, year, kingdomIL, CEID, kingdomID, document.getElementById("divUserID").innerHTML);
}
function OnWSModalComplete(results) {
    document.getElementById("dialog").innerHTML = results;
    $("#ceModalTable").tablesorter({ widgets: ['zebra'], widgetZebra: { css: ['d0', 'd1']} });
}

//Columns Chooser
//loads the users sets
function LoadFirstColumnSets() {
    ColumnChooser.LoadColumnSets(document.getElementById("divUserID").innerHTML, document.getElementById("divOwnerID").innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSLoadColumnSetsReturn);
}
function OnWSLoadColumnSetsReturn(results) {
    document.getElementById("divColumnSets").innerHTML = results;
    LoadSelectedColumns();
}
//loads the columns
function LoadSelectedColumns() {
    ColumnChooser.LoadColumnsSelected(document.getElementById("activeColumnSet").getAttribute('name'), document.getElementById("divUserID").innerHTML, document.getElementById("divOwnerID").innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSLoadColumnsSelectedReturn);
}
function OnWSLoadColumnsSelectedReturn(results) {
    document.getElementById("divChooseColumns").innerHTML = results;
}
function AddSet() {
    var ctrl = document.getElementById("AddSet");
    ctrl.innerHTML = "<input id='tbAddSetName' type='text' /><input id='btnAddSetName' type='button' value='Add' onclick='AddSetName(this);return false;' />";
}

function AddSetName() {
    var ctrl = document.getElementById("tbAddSetName");
    if (ctrl.value.length > 0)
        ColumnChooser.AddSetName(ctrl.value, document.getElementById("divUserID").innerHTML, document.getElementById("divOwnerID").innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSLoadColumnSetsReturn);
}
function AddKingdomSet() {
    var ctrl = document.getElementById("AddKingdomSet");
    ctrl.innerHTML = "<input id='tbAddKingdomSetName' type='text' /><input id='btnAddKingdomSetName' type='button' value='Add' onclick='AddKingdomSetName();return false;' />";
}
function AddKingdomSetName() {
    var ctrl = document.getElementById("tbAddKingdomSetName");
    if (ctrl.value.length > 0)
        ColumnChooser.AddKingdomSetName(ctrl.value, document.getElementById("divUserID").innerHTML, document.getElementById("divOwnerID").innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSLoadColumnSetsReturn);
}
function ChangeSet(ctrl) {

    if (document.getElementById("activeColumnSet") != null)
        $(document.getElementById("activeColumnSet")).toggleClass('ColumnOn').toggleClass('ColumnOff').attr("id", "");
    $(ctrl).toggleClass('ColumnOff').toggleClass('ColumnOn').attr("id", "activeColumnSet");
    LoadSelectedColumns();
}

function DeleteSet() {
    var ctrl = document.getElementById("activeColumnSet");
    var answer = confirm("Delete Set '" + ctrl.innerHTML + "'")
    if (answer)
        ColumnChooser.DeleteSetName(ctrl.getAttribute('name'), document.getElementById("divUserID").innerHTML, document.getElementById("divOwnerID").innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSLoadColumnSetsReturn);
}
function DeleteKingdomSet() {
    var ctrl = document.getElementById("activeColumnSet");
    var answer = confirm("Delete Set '" + ctrl.innerHTML + "'")
    if (answer)
        ColumnChooser.DeleteKingdomSetName(ctrl.getAttribute('name'), document.getElementById("divUserID").innerHTML, document.getElementById("divOwnerID").innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSLoadColumnSetsReturn);
}


function AddToMySet() {
    var ctrl = document.getElementById("activeColumnSet");
    ColumnChooser.AddToMySet(ctrl.getAttribute('name'), document.getElementById("divUserID").innerHTML, document.getElementById("divOwnerID").innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSLoadColumnSetsReturn);
}
function AddToTheKingdomSets() {
    var ctrl = document.getElementById("activeColumnSet");
    ColumnChooser.AddToKingdomSet(ctrl.getAttribute('name'), document.getElementById("divUserID").innerHTML, document.getElementById("divOwnerID").innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSLoadColumnSetsReturn);
}

function SetColumn(ctrl) {
    var kingdomCheck = document.getElementById("activeColumnSet");
    if (kingdomCheck != null) {
        if (kingdomCheck.getAttribute('ty') === 'k') {
            if (document.getElementById("divMonarch").innerHTML === 'true') //if they are monarch or owner
                ColumnChooser.SetColumnForKingdom(ctrl.getAttribute('name'), document.getElementById("activeColumnSet").getAttribute('name'), document.getElementById("divUserID").innerHTML, document.getElementById("divOwnerID").innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSColumnsUpdateComplete);
        }
        else {
            ColumnChooser.SetColumnForUser(ctrl.getAttribute('name'), document.getElementById("activeColumnSet").getAttribute('name'), document.getElementById("divUserID").innerHTML, document.getElementById("divOwnerID").innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSColumnsUpdateComplete);
        }
        $(ctrl).toggleClass('ColumnOn')
    }
}
function OnWSColumnsUpdateComplete(results) {
    document.getElementById("UserDefinedColumns").innerHTML = results;
}

//moves the columns up in order.
function UpColumn(ctrl) {
    var kingdomCheck = document.getElementById("activeColumnSet");
    if (kingdomCheck != null) {
        if (kingdomCheck.getAttribute('ty') === 'k') {
            if (document.getElementById("divMonarch").innerHTML === 'true') //if they are monarch or owner
                ColumnChooser.UpColumnForKingdom(ctrl.getAttribute('name'), document.getElementById("activeColumnSet").getAttribute('name'), document.getElementById("divOwnerID").innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSColumnsUpdateComplete);
            else
                alert("Only the Monarch or Sub Monarch can edit these sets");
        }
        else {
            ColumnChooser.UpColumnForUser(ctrl.getAttribute('name'), document.getElementById("activeColumnSet").getAttribute('name'), document.getElementById("divUserID").innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSColumnsUpdateComplete);
        }
    }
}
//moves the columns down in order.
function DownColumn(ctrl) {
    var kingdomCheck = document.getElementById("activeColumnSet");
    if (kingdomCheck != null) {
        if (kingdomCheck.getAttribute('ty') === 'k') {
            if (document.getElementById("divMonarch").innerHTML === 'true') //if they are monarch or owner
                ColumnChooser.DownColumnForKingdom(ctrl.getAttribute('name'), document.getElementById("activeColumnSet").getAttribute('name'), document.getElementById("divOwnerID").innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSColumnsUpdateComplete);
            else
                alert("Only the Monarch or Sub Monarch can edit these sets");
        }
        else {
            ColumnChooser.DownColumnForUser(ctrl.getAttribute('name'), document.getElementById("activeColumnSet").getAttribute('name'), document.getElementById("divUserID").innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSColumnsUpdateComplete);
        }
    }
}
function OnWSColumnsMove(results) {
    document.getElementById("RealColumns").innerHTML = results;
}

//Contacts Page
function SaveContactInfo(cityID, countryID, gmtID, nickNameID, stateID, dayID, monthID, yearID, notesID) {
    //function SaveContactInfo() {
    document.getElementById('divItemUpdated').innerHTML = "";
    var i;
    for (i = 0; i <= document.getElementById('tblCN').rows.length - 1; i++) {
        if (document.getElementById('tbCN' + i).value.length > 1)
            ContactList.AddImName(document.getElementById('tbCN' + i).value, document.getElementById('ddlCN' + i).value, document.getElementById('cbCN' + i).value, document.getElementById('divUserID').innerHTML, document.getElementById('divOwnerID').innerHTML);
    }
    for (i = 0; i <= document.getElementById('tblPN').rows.length - 1; i++) {
        if (document.getElementById('tbPN' + i).value.length > 1)
            ContactList.AddPhoneNumbers(document.getElementById('tbPN' + i).value, document.getElementById('ddlPN' + i).value, document.getElementById('cbPN' + i).value, document.getElementById('divUserID').innerHTML, document.getElementById('divOwnerID').innerHTML);
    }
    ContactList.UpdateContact(document.getElementById(cityID).value, document.getElementById(countryID).value, document.getElementById(gmtID).value, document.getElementById(nickNameID).value, document.getElementById(stateID).value, document.getElementById(dayID).value, document.getElementById(monthID).value, document.getElementById(yearID).value, document.getElementById(notesID).value, document.getElementById('divOwnerID').innerHTML, OnWSSaveContactinfoComplete);
}
function OnWSSaveContactinfoComplete(results) {
    if (results == true) {
        document.getElementById('divItemUpdated').innerHTML = "Updated";
    }
}
function DeletePNRow(uid, rowID) {
    ContactList.DeletePhoneNumber(uid, document.getElementById('divOwnerID').innerHTML, document.getElementById('divUserID').innerHTML);
    document.getElementById('tblPN').firstChild.removeChild(document.getElementById('trPN' + rowID));
}
function DeleteIMRow(uid, rowID) {
    ContactList.DeleteImName(uid, document.getElementById('divOwnerID').innerHTML, document.getElementById('divUserID').innerHTML);
    document.getElementById('tblCN').firstChild.removeChild(document.getElementById('trCN' + rowID));
}
function addPNRow(id) {
    var tbody = document.getElementById(id).getElementsByTagName("TBODY")[0];
    var row = document.createElement("TR");
    row.setAttribute('id', 'trPN' + document.getElementById(id).rows.length);

    var td1 = document.createElement("TD");
    var input = document.createElement("INPUT");
    input.setAttribute('id', 'tbPN' + document.getElementById(id).rows.length);
    input.setAttribute('type', 'text');

    var td2 = document.createElement("TD");
    var select = document.createElement("SELECT");
    select.setAttribute('id', 'ddlPN' + document.getElementById(id).rows.length);
    var i;
    for (i = 0; i <= document.getElementById('ddlPN0').options.length - 1; i++) {
        var opt = document.createElement("OPTION");
        opt.value = document.getElementById('ddlPN0').options[i].value;
        opt.text = document.getElementById('ddlPN0').options[i].text;
        select.options.add(opt);
    }

    var td3 = document.createElement("TD");
    var inputCB = document.createElement("INPUT");
    inputCB.setAttribute('id', 'cbPN' + document.getElementById(id).rows.length);
    inputCB.setAttribute('type', 'checkbox');

    td1.appendChild(input);
    td2.appendChild(document.createTextNode("Type: "));
    td2.appendChild(select);
    td3.appendChild(document.createTextNode("Text Messages? "));
    td3.appendChild(inputCB);
    row.appendChild(td1);
    row.appendChild(td2);
    row.appendChild(td3);
    tbody.appendChild(row);
}
function addIMRow(id) {
    var tbody = document.getElementById(id).getElementsByTagName("TBODY")[0];
    var row = document.createElement("TR");
    row.setAttribute('id', 'trCN' + document.getElementById(id).rows.length);

    var td1 = document.createElement("TD");
    var input = document.createElement("INPUT");
    input.setAttribute('id', 'tbCN' + document.getElementById(id).rows.length);
    input.setAttribute('type', 'text');

    var td2 = document.createElement("TD");
    var select = document.createElement("SELECT");
    select.setAttribute('id', 'ddlCN' + document.getElementById(id).rows.length);
    var i; //pops the Dropdown
    for (i = 0; i <= document.getElementById('ddlCN0').options.length - 1; i++) {
        var opt = document.createElement("OPTION");
        opt.value = document.getElementById('ddlCN0').options[i].value;
        opt.text = document.getElementById('ddlCN0').options[i].text;
        select.options.add(opt);
    }

    var td3 = document.createElement("TD");
    var inputCB = document.createElement("INPUT");
    inputCB.setAttribute('id', 'cbCN' + document.getElementById(id).rows.length);
    inputCB.setAttribute('type', 'checkbox');

    td1.appendChild(input);
    td2.appendChild(select);
    row.appendChild(td1);
    row.appendChild(td2);
    td3.appendChild(document.createTextNode("Password? "));
    td3.appendChild(inputCB);
    row.appendChild(td3);
    tbody.appendChild(row);
}

function SelectSOS(provID, sosID) {
    HistoriesProvince.SelectSOSItem(provID, sosID, document.getElementById('divOwnerID').innerHTML, OnWSHistoryUpdate);
}
function SelectSOM(provID, sosID) {
    HistoriesProvince.SelectSOMItem(provID, sosID, document.getElementById('divOwnerID').innerHTML, OnWSHistoryUpdate);
}
function SelectSurvey(provID, sosID) {
    HistoriesProvince.SelectSurveyItem(provID, sosID, document.getElementById('divOwnerID').innerHTML, OnWSHistoryUpdate);
}
function SelectAttack(provID, sosID) {
    HistoriesProvince.SelectAttack(provID, sosID, document.getElementById('divOwnerID').innerHTML, document.getElementById('divUserID').innerHTML, OnWSHistoryUpdate);
}
function OnWSHistoryUpdate(results) {
    document.getElementById("divHistoryUpdate").innerHTML = results;
}
function LoadProvinceDetailPage() {
    HistoriesProvince.GetLastCB(getParameterByName('ID'), document.getElementById('divOwnerID').innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSLastCB);
    HistoriesProvince.GetLastSOS(getParameterByName('ID'), document.getElementById('divOwnerID').innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSLastSOS);
    HistoriesProvince.GetLastSurvey(getParameterByName('ID'), document.getElementById('divOwnerID').innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSLastSurvey);
    HistoriesProvince.GetLastSOM(getParameterByName('ID'), document.getElementById('divOwnerID').innerHTML, document.getElementById('divOwnerID').innerHTML, OnWSLastSOM);
    HistoriesProvince.GetProvinceHistory(getParameterByName('ID'), document.getElementById('divOwnerID').innerHTML, OnWSProvinceHistory);
}

function OnWSLastSurvey(results) {
    document.getElementById('divSurvey').innerHTML = results;
}
function OnWSLastCB(results) {
    document.getElementById('divCB').innerHTML = results;
}
function OnWSLastSOS(results) {
    document.getElementById('divSOS').innerHTML = results;
}
function OnWSLastSOM(results) {
    document.getElementById('divSOM').innerHTML = results;
}
function OnWSProvinceHistory(results) {
    document.getElementById('divHistory').innerHTML = results;
}

//Monarch Page
function AssignMonarch(ctrl, id, name) {
    if (ctrl.innerHTML.indexOf('on.png') > 1) {
        ctrl.innerHTML = "<img class=\"imgLinks\" src=\"http://codingforcharity.org/utopiapimp/img/icons/off.png\" />  " + name;
        MonarchHelper.AddSubMonarch(id, document.getElementById('divOwnerID').innerHTML);
    }
    else if (ctrl.innerHTML.indexOf('off.png') > 1) {
        ctrl.innerHTML = "<img class=\"imgLinks\" src=\"http://codingforcharity.org/utopiapimp/img/icons/on.png\" />   " + name + " (SM)";
        MonarchHelper.AddSubMonarch(id, document.getElementById('divOwnerID').innerHTML);
    }
}
//Sets the kingdom time limit
function SetKdTimeLimit(tbName) {
    MonarchHelper.SetKdLimit(document.getElementById(tbName).value, document.getElementById('divOwnerID').innerHTML, document.getElementById('divUserID').innerHTML, OnWSKdLimitSet);
}
function OnWSKdLimitSet(results) {
    document.getElementById("divMonarchTimeLimit").innerHTML = "Time Limit was set to " + results + " days.";
}
//Sets the attack and op time limit.
function SetKdOpAttackTimeLimit(tbName) {
    MonarchHelper.SetKdOpAttackLimit(document.getElementById(tbName).value, document.getElementById('divOwnerID').innerHTML, document.getElementById('divUserID').innerHTML, OnWSKdOpAttackTimeLimit);
}
function OnWSKdOpAttackTimeLimit(results) {
    document.getElementById("divMonarchOpAttackTime").innerHTML = "Time Limit was set to " + results + " hours.";
}

//target Finder page.
function AddTargetDatajs() {
    document.getElementById("divTargetWarning").innerHTML = 'Working...';
    if (document.getElementById("tbAddTargetInfo").value.length > 10) {
        TargetFinderService.AddTargetData(document.getElementById("tbAddTargetInfo").value, document.getElementById('divUserID').innerHTML, OnWSAddTargetDataComplete);
    }
    else
        document.getElementById("divTargetWarning").innerHTML = "Nothing Submitted";
}

//Shows the results.
function OnWSAddTargetDataComplete(results) {
    ClearTargetBox();
    document.getElementById("divTargetWarning").innerHTML = results + " total provinces for your next search.";
    var count = document.getElementById("ContentPlaceHolder1_spanSearchCount").innerHTML = results;
}

function ClearTargetBox() {
    document.getElementById("tbAddTargetInfo").value = '';
    document.getElementById("tbAddTargetInfo").focus();
}

function runSearchAuto(ddlNet, ddlAcres, ddlUpdated) {
    document.getElementById("divSearchingAuto").innerHTML = "Searching...";
    var races = "";
    var collection = document.getElementById("divRaces").getElementsByTagName('INPUT');
    for (var x = 0; x < collection.length; x++) {
        if (collection[x].type.toUpperCase() == 'CHECKBOX')
            if (collection[x].checked != true)
                races += collection[x].id + ",";
    }
    var honor = "";
    collection = document.getElementById("divHonor").getElementsByTagName('INPUT');
    for (var x = 0; x < collection.length; x++) {
        if (collection[x].type.toUpperCase() == 'CHECKBOX')
            if (collection[x].checked != true)
                honor += collection[x].id + ",";
    }
    var net = document.getElementById(ddlNet);
    var acres = document.getElementById(ddlAcres);
    var updated = document.getElementById(ddlUpdated);
    document.getElementById("divSearchResults").innerHTML = '<center><img src="http://codingforcharity.org/utopiapimp/img/Loading.gif" alt="Loading..." width="50px" /></center>';
    TargetFinderService.SearchTargetInfo(net.options[net.selectedIndex].value, acres.options[acres.selectedIndex].value, updated.options[updated.selectedIndex].value, races, honor, document.getElementById("tbProvinceCount").value, OnSearchComplete);
}

function runSearchInput(netMin, netMax, acresMin, acresMax, updatedMin, updatedMax, setID) {
    document.getElementById("divSearchingInput").innerHTML = "Searching...";
    var races = "";
    var collection = document.getElementById("divRacesInput").getElementsByTagName('INPUT');
    for (var x = 0; x < collection.length; x++) {
        if (collection[x].type.toUpperCase() == 'CHECKBOX')
            if (collection[x].checked != true)
                races += collection[x].id + ",";
    }
    var honor = "";
    collection = document.getElementById("divHonorInput").getElementsByTagName('INPUT');
    for (var x = 0; x < collection.length; x++) {
        if (collection[x].type.toUpperCase() == 'CHECKBOX')
            if (collection[x].checked != true)
                honor += collection[x].id + ",";
    }
    document.getElementById("divSearchResults").innerHTML = '<center><img src="http://codingforcharity.org/utopiapimp/img/Loading.gif" alt="Loading..." width="50px" /></center>';
    TargetFinderService.SearchTargetInfoInput(document.getElementById(netMin).value, document.getElementById(netMax).value, document.getElementById(acresMin).value, document.getElementById(acresMax).value, document.getElementById(updatedMin).value, document.getElementById(updatedMax).value, races, honor, document.getElementById("tbProvinceCount").value, OnSearchComplete);
}

function OnSearchComplete(results) {
    document.getElementById("divSearchResults").innerHTML = results[0];
    $("#tblSearchResults").tablesorter({ widgets: ['zebra'], widgetZebra: { css: ['d0', 'd1']} });
    document.getElementById("divSearchingAuto").innerHTML = "";
    document.getElementById("divSearchingInput").innerHTML = "";
    document.getElementById("ctl00_ContentPlaceHolder1_spanSearchCount").innerHTML = results[1];
}

var agt = navigator.userAgent.toLowerCase();
var is_major = parseInt(navigator.appVersion);
var is_minor = parseFloat(navigator.appVersion);
var is_nav = ((agt.indexOf('mozilla') != -1) && (agt.indexOf('spoofer') == -1)
                && (agt.indexOf('compatible') == -1) && (agt.indexOf('opera') == -1)
                && (agt.indexOf('webtv') == -1) && (agt.indexOf('hotjava') == -1));
var is_nav4 = (is_nav && (is_major == 4));
var is_nav6 = (is_nav && (is_major == 5));
var is_nav6up = (is_nav && (is_major >= 5));
var is_ie = ((agt.indexOf("msie") != -1) && (agt.indexOf("opera") == -1));

var offsetX = 0;
var offsetY = -150;
var opacity = 100;
var toolTipSTYLE;
var addToolStyle;

function initToolTips() {
    if (document.getElementById) {
        toolTipSTYLE = document.getElementById("toolTipLayer").style;
        addToolStyle = document.getElementById("toolTipAdd").style;
    }
    if (is_ie || is_nav6up) {
        toolTipSTYLE.visibility = "visible";
        toolTipSTYLE.display = "none";
        addToolStyle.visibility = "visible";
        addToolStyle.display = "none";
        document.onmousemove = moveToMousePos;
    }
}
function moveToMousePos(e) {
    if (!is_ie) {
        x = e.pageX;
        y = e.pageY;
    } else {
        x = event.x + document.body.scrollLeft;
        y = event.y + document.body.scrollTop;
    }
    toolTipSTYLE.left = x + offsetX + 'px';
    toolTipSTYLE.top = y + offsetY + 'px';
    return true;
}


function toolTip(msg, fg, bg) {
    if (document.getElementById) {
        toolTipSTYLE = document.getElementById("toolTipLayer").style;
    }
    if (is_ie || is_nav6up) {
        toolTipSTYLE.visibility = "visible";
        toolTipSTYLE.display = "none";
        document.onmousemove = moveToMousePos;
    }

    if (toolTip.arguments.length < 1) // if no arguments are passed then hide the tootip
    {
        if (is_nav4)
            toolTipSTYLE.visibility = "hidden";
        else
            toolTipSTYLE.display = "none";
    }
    else // show
    {
        if (!fg) fg = "#777777";
        if (!bg) bg = "#ffffe5";
        var content = "<div class=\"toolTipAdd\" id=\"toolTipSingle\">" + msg + "</div>";
        if (is_nav4) {
            toolTipSTYLE.document.write(content);
            toolTipSTYLE.document.close();
            toolTipSTYLE.visibility = "visible";
        }

        else if (is_ie || is_nav6up) {
            document.getElementById("toolTipLayer").innerHTML = content;
            toolTipSTYLE.display = 'block'
        }
    }
}

//To check if im pressing the CTRL key.  If I am, the keycodeMain will equal 17
var keycodeMain = 0;
document.onkeydown = checkKeycodeDown;
function checkKeycodeDown(e) {
    var keycode;
    if (window.event) keycode = window.event.keyCode;
    else if (e) keycode = e.which;
    if (keycode === 17)
        keycodeMain = keycode;
}
document.onkeyup = checkKeycodeUp;
function checkKeycodeUp(e) {
    var keycode;
    if (window.event) keycode = window.event.keyCode;
    else if (e) keycode = e.which;
    if (keycode === 17)
        keycodeMain = 0;
}


//Gets the querystring parameter by name
function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}

function DeleteIRCChannel(item, name) {
    item.parentNode.parentNode.removeChild(item.parentNode);
    MonarchHelper.DeleteIRCKindomChannel(name, document.getElementById('divOwnerID').innerHTML);
}
function AddIRCChannel() {
    var myUL = document.getElementById("ulIRCChannels");
    var idChannelName = document.getElementById("inAddChannel").value.replace("#", ""); ;
    var idChanPass = document.getElementById("inAddChannelPassword").value;
    //Create the <li> node
    theLi = document.createElement('li');
    theLi.innerHTML = "Channel: #" + idChannelName;
    if (idChanPass != '')
        theLi.innerHTML += " - Password: " + idChanPass
    theLi.innerHTML += " - <span name='" + idChannelName + "' onclick=\"javascript:DeleteIRCChannel(this,'" + idChannelName + "');\" class='deleteButton'>Remove</span>";
    //Append it to the Current list (Places it last on the list)
    myUL.appendChild(theLi);
    MonarchHelper.AddIRCKingdomChannel(idChannelName, idChanPass, document.getElementById('divOwnerID').innerHTML);
}
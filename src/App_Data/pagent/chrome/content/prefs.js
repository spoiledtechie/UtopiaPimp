/* prefs.js
this handles most of the work done by the preferences panel, which includes the queue, logs, and user displays
*/



/* Loads all the user options into the prefs panel, and populates the various lists with data */
function pimpPrefsLoad(e) {
    oPimpOptions = new PimpOptions();
    oPimpOptions.start();

    //bit of a hack - if a user clicks on a menu option to jump to a specific tab on the prefs pane, it uses a userpref to store which pane
    document.getElementById('preftabs').selectedIndex = GM_getValue('start_tab', 0, true);

    initQueueList();
    initLogList();
    initFailedList();
    initDebugList();
    /*
    badprovCheck = GM_getValue('badprovmsg',0,true);
    if( badprovCheck == 1 ) 
    {
    msg = document.getElementById('add-user-message');
    msg.value = 'Your prov. name in Utopia doesn\'t match what Pimp Agent has stored. Please hit "Save user info" to update.';
    GM_setValue('badprovmsg',1,true);
	
	}
    */

    if (compareVer(getExtVersion(), GM_getValue('updatever', '', true))) {
        el = document.getElementById('upt-prefs-oldverwarn');
        //	alert(el);
        el.value = "You have an old version of Pimp Agent! Please update before sending any messages.";

    }
}


function arrayPref(prefname, delim) {
    delim = (typeof delim == 'undefined') ? "\n" : delim;
    var qarr = GM_getValue(prefname, '');
    return (qarr == '') ? [] : qarr.split("\n");
}

function initQueueList() {
    initDataList('upt.queue.queuelist', 'headdat', 'upt-prefs-sendqueue');
}

function initDebugList() {
    var list = getWindowObj('upt.debug.list');
    if (list) {
        emptyList(list);
        qarr = arrayPref('debuglist');

        for (var i = 0; i < qarr.length; i++) {
            var tarr = qarr[i].split('|');
            var item = document.createElement('listitem');
            item.appendChild(document.createElement('listcell')).setAttribute('label', getTime(tarr[0] * 1000));
            item.appendChild(document.createElement('listcell')).setAttribute('label', tarr[1]);
            list.appendChild(item);
        }
    }

    //only make queue button clickable if there are things to send!
    var btn = getWindowObj('upt-prefs-senddebug');
    if (btn)
        btn.disabled = (qarr.length == 0);

}

function sendDebug() {
    qarr = arrayPref('debuglist');
    msg = document.getElementById('upt.debug.msg');
    lbl = document.getElementById('send-debug-msg');

    if (qarr.length > 0 && msg.value != '') {
        debug_postarr = new Array();
        debug_postarr.push("action=senddebug");


        test = Components.classes["@mozilla.org/consoleservice;1"].getService(Components.interfaces.nsIConsoleService);

        var out = {};
        test.getMessageArray(out, {});
        var messages = out.value || [];
        var sendError = new Array();

        for (i in messages) {
            if (messages[i].message.indexOf("JavaScript Error") !== -1 && messages[i].message.indexOf("pimpagent") !== -1) {
                sendError.push(messages[i].message);
            }
        }
        debug_postarr.push("errors=" + escape(sendError.join("\n")));

        debug_postarr.push("data=" + escape(qarr.join("\n")));
        debug_postarr.push("msg=" + escape(msg.value));

        //id info
        d = new Date();
        ctime = Math.floor(d.getTime() / 1000);
        ftime = getTime(ctime * 1000);

        idinfo = "Time: " + ftime + "\nServer: " + GM_getValue('curr_server', '', true) + "\nUser: " + GM_getValue('pimp_username', '') + "\nCurrver: " + getExtVersion();
        debug_postarr.push("idinfo=" + idinfo);
        debug_postarr.push("username=" + GM_getValue('pimp_username', ''));

        //req = new XMLHttpRequest();
        //req.open("POST", getUrl(), true);
        //alert(getUrl()+"thisistheurl");
        /* stuff below sends the request =)
        req.open("POST", "http://localhost/", true);
        req.setRequestHeader('Content-type','application/x-www-form-urlencoded');
        req.send(debug_postarr.join('&'));
        */

        //lbl.value = "Debug message sent! Thanks for helping out!";
        lbl.value = "Debug message not sent! But don't worry, this feature will be re-enabled in time ;)";
        msg.value = '';
    }
    else
        lbl.value = "Please make sure you enter a message describing what went wrong!";




}

function initDataList(listname, headname, btnname) {
    var server = GM_getValue('curr_server', 'wol', true);
    var list = getWindowObj(listname);
    if (list) {
        emptyList(list);
        qarr = arrayPref(server + '_' + headname);

        for (var i = 0; i < qarr.length; i++) {
            var tarr = qarr[i].split('|');
            var item = document.createElement('listitem');
            item.appendChild(document.createElement('listcell')).setAttribute('label', tarr[2]);
            item.appendChild(document.createElement('listcell')).setAttribute('label', tarr[1]);
            onumstr = (tarr[3] && tarr[3].length < 4) ? tarr[3] : '--'; //number of ops
            item.appendChild(document.createElement('listcell')).setAttribute('label', onumstr);

            tarr[0] = getTime(tarr[0] * 1000);
            item.appendChild(document.createElement('listcell')).setAttribute('label', tarr[0]);
            if (tarr[3] && tarr[3].length >= 4) //error messages on failed data log
                item.appendChild(document.createElement('listcell')).setAttribute('label', tarr[3]);
            else if (tarr[4])
                item.appendChild(document.createElement('listcell')).setAttribute('label', tarr[4]);

            list.appendChild(item);
        }
    }

    //only make queue button clickable if there are things to send!
    var btn = getWindowObj(btnname);
    if (btn)
        btn.disabled = (qarr.length == 0);

}

function initFailedList() {
    initDataList('upt.queue.failedlist', 'errorheaddat', 'upt-prefs-sendfailed');
}

function clearFailedQueue() {
    var serv = GM_getValue('curr_server', 'wol', true);
    GM_setValue(serv + '_errordat', '');
    GM_setValue(serv + '_errorheaddat', '');
    initFailedList();
}

function initLogList() {
    var d = new Date();
    var too_old = d.getTime() - 1000 * 60 * 60 * 72; // three days

    var server = GM_getValue('curr_server', 'wol', true);
    var list = getWindowObj('upt.logs.loglist');
    if (list) {
        emptyList(list);
        qarr = arrayPref(server + '_logfile');

        var newqarr = [];
        list.rows = qarr.length;
        for (var i = 0; i < qarr.length; i++) {
            newqarr.push(qarr[i]); // tosses onto 'save stack'
            var tarr = qarr[i].split('|');

            if (tarr[0] > too_old) //good
            {
                var item = document.createElement('listitem');

                var status = (tarr[1] == '1') ? 'Success' : 'Failed';
                item.appendChild(document.createElement('listcell')).setAttribute('label', getTime(tarr[0]));
                listcell = document.createElement('listcell');
                listcell.setAttribute('label', status);
                listcell.style.textAlign = 'center';
                item.appendChild(listcell);
                item.appendChild(document.createElement('listcell')).setAttribute('label', tarr[2]);

                list.appendChild(item);
                list.ensureElementIsVisible(item);
            }
        }
        if (newqarr.length != qarr.length) //some got trimmed
        {
            GM_setValue(server + '_logfile', newqarr.join("\n"));
        }
    }

    //only clear logs if there is something to clear
    var btn = getWindowObj('upt-prefs-clearlogs');
    if (btn)
        btn.disabled = (newqarr.length == 0);
}

/* Updates the user info of the currently selected user. Currently, this is only activated servers and username/password */
function updateUserInfo() {
//Disabling allows me to Auto save WOL as first server.  Therefor not having to edit more PA code. UP 2.1 change
//    PimpUtil.saveCheckbox('use_server_wol');
//    PimpUtil.saveCheckbox('use_server_gen');
//    PimpUtil.saveCheckbox('use_server_bf');

    checkUserInfo(true);
    serv = GM_getValue('curr_server', '', true);
    if (GM_getValue('use_server_' + serv, false) == false)
        GM_setValue('curr_server', '', true);

}


function checkUserInfo(update) {
    if (typeof update == 'undefined')//if its most like a new user
        update = false;

    msg = document.getElementById('add-user-message');
    msg.value = '';

    if (update) //not being updated.
    {
        username = document.getElementById('upt.options.pimp_username').value;
        password = document.getElementById('upt.options.pimp_password').value;
        provincename = document.getElementById('upt.options.pimp_provincename').value;
        msg = document.getElementById('user-prefs-update-label');
        button = document.getElementById('upt.options.saveinfo');
    }
    else {
        username = document.getElementById('upt.newusername').value;
        password = document.getElementById('upt.newpassword').value;
        provincename = document.getElementById('upt.newprovince').value;
        msg = document.getElementById('add-user-message');
        button = document.getElementById('upt-add-user');
    }

    msg.value = '';

    if (!update)//will hit if its not an update but a new user.
    {
        var userdata = GM_getValue('userbinds', '', true); //usernames come from here.
        userdata = (userdata == '') ? [] : userdata.split(',');
        //need to see if username's already in there

        for (i in userdata) {
            if (userdata[i] == username) {
                msg.value = "That username is already entered into the user list.";
                document.getElementById('upt.newusername').value = '';
                document.getElementById('upt.newpassword').value = '';
                document.getElementById('upt.newprovince').value = '';
                return;
            }
        }
    }


    button.disabled = true;
    initialize = false;
    success = false;
    data_to_send = ['username=' + username, 'password=' + password, 'provincename=' + provincename, 'action=checkuser'];
    data_to_send = data_to_send.join('&'); //joins the array with a &
    foundServers = new Array();

    try {

        //debug("sending " + data_to_send);
        //debug("url " + getUrl());
        req = new XMLHttpRequest();
        req.open("POST", getUrl(), true);
        req.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
        req.send(data_to_send);
        req.onreadystatechange = function(aEvt) {
            if (req.readyState == 4) {
                if (req.status == 200) //good return
                {
                    //debug("checkUserInfo: response |" + req.responseText + "|");
                    rlines = req.responseText.split("\n");

                    for (iq = 0; iq < rlines.length; iq++) {
                        //debug("working with line: "+rlines[iq]);
                        rdata = rlines[iq].split(': ');
                        if (rdata.length == 1) //bad return
                        {
                            //qres.push('Could not complete connection with Utopiapimp [Error code: 1, '+rlines[i]+']');
                            msg.value = "You received invalid data from Utopiapimp; please try again later!";
                            break;
                        }
                        else {
                            if (rdata[0] == 'ERROR') {
                                //qres.push('Problem with Utopiapimp: '+rdata[1]);
                                //addLog('Problem with Utopiapimp: '+rdata[1],0);
                                break;
                            }
                            else //the rest will be servers that have info connected
                            {
                                if (!initialize && !update) //haven't made the user yet, do that
                                {
                                    initialize = true;

                                    uid = userdata.push(username) - 1;
                                    GM_setValue('userbinds', userdata.join(','), true);
                                    setUser(uid);
                                }
                                success = true;
                                server = rdata[0];
                                foundServers[server] = true;
                                //debug("caught info for "+server+" update: "+update);
//                                if (!update) removed for UP 2.1
//                                    GM_setValue('use_server_' + server, true);
                                province = rdata[1];
                                GM_setValue(server + '_selfprov', province);
                                GM_setValue('use_server_wol', true);
                                GM_setValue('pimp_password', password);
                                GM_setValue('pimp_username', username);
                                GM_setValue('pimp_provincename', provincename);
                            }


                        }
                    } //end for loop


                    if (success) {
                        servs = ['bf', 'gen', 'wol'];
                        for (i in servs) {
                            if (!foundServers[servs[i]]) {
                                oPimpOptions.deleteServerPref(servs[i]);
//                                GM_setValue('use_server_' + serv[i], false); Scott Removed for 2.1
                                //debug("did not find this! "+servs[i]);
                            }
                        }
                        buildUserList();
                        oPimpOptions.loadUserPrefs();
                        //  GM_setValue('badprovmsg',0,true);
                        if (update)
                            msg.value = "The user has been updated with the new information.";
                        else
                            msg.value = "The user was successfully validated and added to Pimp Agent";
                    }
                    else //need to dump error message
                    {
                        msg.value = req.responseText; //"Your user information is incorrect! Please fix it, then try again.";
                    }

                    if (!update) {
                        document.getElementById('upt.newusername').value = '';
                        document.getElementById('upt.newpassword').value = '';
                        document.getElementById('upt.newprovince').value = '';
                    }



                } //end req.status
                else
                    msg.value = "Could not connection to Utopiapimp, please try again later.";

            }
        };

        //	debug("queuesend results: "+qres.join("\n"));
    }
    catch (e) {
        msg.value = "Could not connection to Utopiapimp, please try again later.";
    }
    button.disabled = false;
}

function buildUserList() {
    list = document.getElementById('upt.userlist');
    emptyList(list);
    userdata = GM_getValue('userbinds', '', true);
    //debug("this is userdata "+userdata);
    userdata = (userdata == '') ? [] : userdata.split(',');
    for (i in userdata) {
        //debug("building user list, adding index: "+i+", which is "+userdata[i]);
        var item = document.createElement('listitem');

        item.setAttribute('label', userdata[i]);
        item.setAttribute('id', 'userlist_' + i);

        item.addEventListener('click', setUserEvent, false);
        list.appendChild(item);
    }

    if (GM_getValue('currentuser', '', true) != '') {
        list.selectedIndex = GM_getValue('currentuser', '', true);
        //	debug("selected index: "+list.selectedIndex);
    }
    else {
        //	debug("about to select user: "+userdata.length);
        if (userdata.length > 0)
            setUser(0);
    }
}


/* PimpOptions is pretty simple. It can delete current users as well as load/update the preferences for the current user */
function PimpOptions()
{ }

PimpOptions.prototype =
{
    start: function() {
        //set up the user page
        buildUserList();
        this.loadUserPrefs();

    },

    deleteCurrentUser: function() {
        //	debug('made it into del current user');
        //chid - for some reason this doesn't work..
        if (GM_getValue('currentuser', '', true) != null) {
            msg = document.getElementById('add-user-message');
            msg.value = "The user has been deleted";
        }
        else {
            msg = document.getElementById('add-user-message');
            msg.value = "No User Selected";
        }
        //first update the array with the user list
        uid = GM_getValue('currentuser', '', true);
        var userdata = GM_getValue('userbinds', '', true);
        userdata = (userdata == '') ? [] : userdata.split(',');
        userdata.splice(uid, 1);
        //Gets all the preferences for the current user.
        prefnames = ['auto_send_ops', 'auto_send_selfintel', 'auto_send_selfintel_span', 'busy', 'curr_server', 'pimp_username', 'pimp_provincename', 'pimp_password',
			'start_tab', 'use_server_bf', 'use_server_gen', 'use_server_wol', 'debuglist', 'use_angel'];
        for (p in prefnames) {
            //debug("about to remove pref: "+prefnames[p]);
            removePref(prefnames[p]);
        }

        //go through each server and delete the relevant prefs
        servs = ['bf', 'wol', 'gen'];
        for (i in servs) {
            this.deleteServerPref(servs[i]);
        }

        //it's the current user, so we need to drop that.
        if (userdata.length > 0)
            setUser(userdata[0]);
        else {
            GM_setValue('currentuser', '', true);
        }

        GM_setValue('userbinds', userdata.join(','), true);
        this.start();
    },

    deleteServerPref: function(serv) {
        prefnames = ['datadat', 'errordat', 'errorheaddat',
			'headdat', 'lastqueue', 'lastselfcb', 'lastselfsos', 'lastselfsom', 'lastselfsurvey', 'logfile', 'selfprov', 'sid'];
        for (p in prefnames) {
            removePref(serv + '_' + prefnames[p]);
        }

    },

    loadUserPrefs: function() {
        //debug("top of load prefs, cuser: "+GM_getValue('currentuser','',true));
        document.getElementById('user-prefs-update-label').value = '';
        if (GM_getValue('currentuser', '', true).length == 0) {
            //debug('doesnt seem to have a user, dont load prefs');
            this.toggleUserPrefs(false);
            return;
        }
        to = GM_getValue('currentuser', '', true).length;
        //	debug('about to load prefs |'+to);
        this.toggleUserPrefs(true);
        //	document.getElementById('user-prefs-caption-label').label = "Info for user: "+GM_getValue('pimp_username','');
        document.getElementById('upt-prefs-saveprefs').disabled = false;
        document.getElementById('upt-prefs-nolabel').hidden = true;
        //load up stuff on Users tab
        servers = ['wol', 'bf', 'gen'];
        for (i in servers) {
            if (GM_getValue(servers[i] + '_selfprov', '') != '') //account
            {
                //document.getElementById('user-prefs-'+servers[i]+'-prov-label').value = GM_getValue(servers[i]+'_selfprov','');
                PimpUtil.initCheckbox('use_server_' + servers[i], false);
                document.getElementById('upt.options.use_server_' + servers[i]).disabled = false;
            }
            else {
                //document.getElementById('user-prefs-'+servers[i]+'-prov-label').value = '(not connected)';
                document.getElementById('upt.options.use_server_' + servers[i]).disabled = true;
            }
        }

        PimpUtil.initText("pimp_username", '');
        PimpUtil.initText("pimp_password", '');
        PimpUtil.initText("pimp_provincename", '');
        PimpUtil.initText("auto_send_selfintel_span", 4);

        PimpUtil.initCheckbox("auto_send_ops", false, true);
        PimpUtil.initCheckbox("auto_send_selfintel", false, true);
        PimpUtil.initCheckbox("use_angel", false, true);
        PimpUtil.initCheckbox("timeout_error", false, true);


        this.toggleSpan(document.getElementById("upt.options.auto_send_selfintel").checked, false);


    },

    /* Used to dim out the specifics of the user preferences on the Users tab. If you simply hide it or make it reappear,
    the box doesn't resize
    */
    toggleUserPrefs: function(toggle) {
        document.getElementById('user-prefs-update-label').value = '';
        labelArr = ['server', 'enabled', 'wol', 'bf', 'gen', 'caption'];
        clabelArr = ['username', 'password', 'provincename'];
        formArr = ['pimp_username', 'pimp_password', 'pimp_provincename'];
        checkArr = ['use_server_wol', 'use_server_bf', 'use_server_gen'];

        for (i in labelArr) {
            document.getElementById('user-prefs-' + labelArr[i] + '-label').style.color = (toggle) ? 'black' : 'gray';
        }

        for (i in clabelArr) {
            document.getElementById('user-prefs-' + clabelArr[i] + '-label').value = '';
            document.getElementById('user-prefs-' + clabelArr[i] + '-label').style.color = (toggle) ? 'black' : 'gray';
        }

        for (i in formArr) {
            document.getElementById('upt.options.' + formArr[i]).value = '';
            document.getElementById('upt.options.' + formArr[i]).disabled = !toggle;
        }
        document.getElementById('upt.options.saveinfo').disabled = !toggle;

        for (i in checkArr) {
            document.getElementById('upt.options.' + checkArr[i]).checked = false;
            document.getElementById('upt.options.' + checkArr[i]).disabled = !toggle;
        }


    },


    onSave: function() {
        //debug("string form: "+String(GM_getValue('currentuser','',true)));
        if (String(GM_getValue('currentuser', '', true)).length == 0) {
            //	debug('no current user, leaving!');
            return;
        }

        PimpUtil.saveText("pimp_username");
        PimpUtil.saveText("pimp_password");
        PimpUtil.saveText("pimp_provincename");

        //check the span, make sure it's 6 or above, numbers only
        var spanamt = document.getElementById('upt.options.auto_send_selfintel_span');
        if (spanamt && spanamt.value.match(/^\d+$/) && spanamt.value * 1 >= 4) {
            PimpUtil.saveText("auto_send_selfintel_span");
        }

        PimpUtil.saveCheckbox("auto_send_ops", false, true);
        PimpUtil.saveCheckbox("auto_send_selfintel", false, true);
        PimpUtil.saveCheckbox("use_angel", false, true);
        PimpUtil.saveCheckbox('use_server_wol');
        PimpUtil.saveCheckbox('use_server_gen');
        PimpUtil.saveCheckbox('use_server_bf');
        PimpUtil.saveCheckbox('timeout_error', false, true);
        prefsWindow = false;
        if (GM_getValue('disabled', false) == true && GM_getValue('pimp_password', '') != '' && GM_getValue('pimp_username', '') != '') {
            addAgentMessage(window.opener.document, "Please reload page!");
            GM_setValue('disabled', false);
        }
        window.close();
    },

    //given value of checkbox
    toggleSpan: function(toggle, set) {
        document.getElementById("upt.options.auto_send_selfintel_span").disabled = !toggle;
        document.getElementById("upt.options.auto_send_selfintel_span.label").style.color = (toggle) ? 'black' : 'gray';
        if (set)
            document.getElementById("upt.options.auto_send_selfintel").checked = !toggle;
    }
}

/* Empties an XUL list */
function emptyList(list) {
    var items = list.getElementsByTagName('listitem');
    while (items.length > 0) {
        list.removeChild(items[0]);
    }
}

/* Tries to dig out elements within the preferences window */
function getWindowObj(name) {
    //debug('his is pw: '+prefsWindow+", asking for "+name);
    if (prefsWindow && prefsWindow.document) {
        //	debug('found it in prefsWindow');
        return prefsWindow.document.getElementById(name);
    }
    if (document.getElementById(name)) {
        //	debug('found it in document');
        return document.getElementById(name);
    }
    return false;

}
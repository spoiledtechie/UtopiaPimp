const PAGENT_URL = "http://utopiapimp.com/pagent.aspx"; 
const PAGENT_URL_ALT = "http://utopiapimp.com/pagent.aspx";
//const PAGENT_URL = "http://localhost:57506/up/pagent.aspx";
//const PAGENT_URL_ALT = "http://localhost:57506/up/pagent.aspx";
const QUEUE_SENDINTERVAL = 5; //in minutes
var prefsWindow = false; //uses this as a passage so elements outside of the prefsWindow can access the prefsWindow
const CURRVER = 31;
var upgradeCheck = false;

function setClip(string)
{
	var clipboard = Components.classes["@mozilla.org/widget/clipboardhelper;1"].getService(Components.interfaces.nsIClipboardHelper);
	clipboard.copyString(string);
}

function getClip()
{
	var clip = Components.classes["@mozilla.org/widget/clipboard;1"]. 	getService(Components.interfaces.nsIClipboard); 
	
	if (!clip) return false; 
	
	var trans = Components.classes["@mozilla.org/widget/transferable;1"]. createInstance(Components.interfaces.nsITransferable); 
	if (!trans) return false; 
	
	trans.addDataFlavor("text/unicode");

	clip.getData(trans,clip.kGlobalClipboard); 
	var str = new Object(); 
	var strLength = new Object(); 
	trans.getTransferData("text/unicode",str,strLength);

	if (str) str = str.value.QueryInterface(Components.interfaces.nsISupportsString); 
	if (str) pastetext = str.data.substring(0,strLength.value / 2);
	return pastetext;
}

function queryAngel(txt,fname)
{
	//if it's coming in from an op page - we need to split it up. 4 pieces, connected by pipes
	
	temp = txt.split("|");
	msg = "<span style=\"color: yellow; text-decoration: underline;\">Looking for Angel...</span>";
	//debug("queryAngel = type is: "+fname);
	if( temp.length == 4 )
	{
		//only need to do this for intel
		if( fname == 'cb' || fname == 'sos' || fname == 'som' || fname == 'survey' )
		{
			getMsgBox('angel',msg);
			setClip(decode64(temp[3]));
			window.setTimeout(function (a,b,c,d,e) { queryAngelReport(a,b,c,d,e); },1000,txt,fname,true,5,randNum);
		}
		else
			queueAgent(txt,fname,true); //regular ops, don't check
	}
	else
	{
		if( fname == 'throne' )
		{
			getMsgBox('angel',msg);
			setClip(txt);
			window.setTimeout(function (a,b,c,d,e) { queryAngelReport(a,b,c,d,e); },1000,txt,fname,false,5,randNum);
		}
		else
			queueAgent(txt,fname,true); //regular ops, don't check
	}
}

function queryAngelReport(txt,type,piped,retrycount,rndNum)
{
	result = getClip();
	eline = result.match(/(\*\* Export Line[\w\W\s]*$)/im);
	
	if( eline != null )
	{
		//debug("here is the export line scrape: "+eline);
		if( piped )
		{
			temp = txt.split("|");
			temp[3] = decode64(temp[3]);
			temp[3] = temp[3] + "[exportline]"+eline[1]+"[/exportline]";
			temp[3] = encode64(temp[3]);
			txt = temp.join("|");
		}
		else
			txt = txt + "[exportline]"+eline[1]+"[/exportline]";
		msg = "<span style=\"color: yellow; text-decoration: underline;\">Export line found!</span>";
		getMsgBox('angel',msg);
		queueAgent(txt,type,true);
	}
	else
	{
		if( retrycount > 0 )
		{
			//debug("could not find export line, retries remaining: "+retrycount);
			window.setTimeout(function (a,b,c,d,e) { queryAngelReport(a,b,c,d,e); },1000,txt,type,piped,retrycount-1,rndNum);
		
			return;
		}
		else
		{
			if( rndNum == randNum )// random number assigned to each page load. Since this searches the 
			//clipboard for 5 seconds, there's a good chance they will load another page before the check is complete
			//(in cases where the export cannot be found). So, if the two numbers are diff, dont show the below
			{
				//debug("could not find export line in scrape, out of retries");
				msg = "<span style=\"color: yellow; text-decoration: underline;\">Can't find Export</span>";
				getMsgBox('angel',msg);
				queueAgent(txt,type,true);
				//debug("type: "+type);
				if( type == 'cb' || type == 'sos' || type == 'som' || type == 'survey' 
				|| type == 'throne' || type == 'buildingadv' || type == 'militaryadv' || type == 'science') //let them repimp
				{
					btn = makeAgentButton(getDoc(),'Requeue Intel');
					btn.disabled = false;
				}
			}
		}
	}
	
	
}

function debug(aMsg) {
 //setTimeout(function() { throw new Error("[debug] " + aMsg); }, 0);
   //return;
   var qarr = GM_getValue('debuglist','');
	qarr = ( qarr == '' ) ? [] : qarr.split("\n");
	d = new Date();
	ctime = Math.floor(d.getTime()/1000);
	newEntry = ctime+"|"+aMsg;
	qarr.push(newEntry);
	//debug("here is the length of qarr: "+qarr.length);
	if( qarr.length > 300 ) //trim
	{
		qarr = qarr.slice(qarr.length-300); //trim off the earliest
	}
	GM_setValue('debuglist',qarr.join("\n"));
} 


/* Name borrowed from greasemonkey. Retrieves user pref. global represents a preference shared by all users, 
	non-global is on a per-user setting */
function GM_getValue(name,def,global)
{
	if( typeof global == 'undefined' || global == false )
		name = 'user'+GM_getValue('currentuser','',true)+"_"+name;
	else
		name = "global_"+name;

	return PimpUtil.getValue(name,def);
}
//Gets the user preference for the user specified.
function GM_getValueUser(name,def,global,userid)
{
	if( typeof global == 'undefined' || global == false )
		name = 'user'+userid+"_"+name;
	else
		name = "global_"+name;

	return PimpUtil.getValue(name,def);
}

function GM_setValue(name,val,global)
{
	if( typeof global == 'undefined' || global == false )
		name = 'user'+GM_getValue('currentuser','',true)+"_"+name;
	else
		name = "global_"+name;
	//if( !pimp )
		
	PimpUtil.setValue(name,val);
}

function removePref(name,global)
{
	if( typeof global == 'undefined' || global == false )
		name = 'user'+GM_getValue('currentuser','',true)+"_"+name;
	else
		name = "global_"+name;

	PimpUtil.remove(name);
}

function getExtVersion() 
{ 
    var  aGUID = "{DCEF1634-DE13-47ce-8CEA-714B0933EAA7}";
    var em = Components.classes["@mozilla.org/extensions/manager;1"].getService(Components.interfaces.nsIExtensionManager);
    	return em.getItemForID(aGUID).version;
 } 

function compareVer(curr,remote)
{
	   var versionChecker = Components.classes["@mozilla.org/xpcom/version-comparator;1"]
                               .getService(Components.interfaces.nsIVersionComparator);
	return versionChecker.compare(remote,curr) > 0 ; //returns true if the remote version is greater than the current
}
/*
var Thing = {
   variable:"value",
   method:function() {
      alert(this.variable);
   }
}
*/
var PimpUtil =
{
	pref:Components.classes["@mozilla.org/preferences-service;1"].
		getService(Components.interfaces.nsIPrefService).
		getBranch("pimpagent."),

	observers:{},

	// whether a preference exists
	exists:function(prefName) {
		return this.pref.getPrefType(prefName) != 0;
	},

	// returns the named preference, or defaultValue if it does not exist
	getValue:function(prefName, defaultValue) {
		var prefType=this.pref.getPrefType(prefName);

	//	debug("getting pref "+this.pref+" name "+prefName+" "+this.pref.PREF_INVALID+" "+prefType);
		// underlying preferences object throws an exception if pref doesn't exist
		if (prefType==this.pref.PREF_INVALID) {
			return defaultValue;
		}
		//debug("prefname: "+prefName);
		switch (prefType) {
			case this.pref.PREF_STRING: return this.pref.getCharPref(prefName);
			case this.pref.PREF_BOOL: return this.pref.getBoolPref(prefName);
			case this.pref.PREF_INT: return this.pref.getIntPref(prefName);
		}
	},

	// sets the named preference to the specified value. values must be strings,
	// booleans, or integers.
	setValue:function(prefName, value) {
		var prefType=typeof(value);

		switch (prefType) {
			case "string":
			case "boolean":
				break;
			case "number":
				if (value % 1 != 0) {
					throw new Error("Cannot set preference to non integral number");
				}
				break;
			default:
				throw new Error("Cannot set preference with datatype: " + prefType);
		}

		// underlying preferences object throws an exception if new pref has a
		// different type than old one. i think we should not do this, so delete
		// old pref first if this is the case.
		if (this.exists(prefName) && prefType != typeof(this.getValue(prefName))) {
			this.remove(prefName);
		}

		// set new value using correct method
		switch (prefType) {
			case "string": this.pref.setCharPref(prefName, value); break;
			case "boolean": this.pref.setBoolPref(prefName, value); break;
			case "number": this.pref.setIntPref(prefName, Math.floor(value)); break;
		}
	},

	// deletes the named preference or subtree
	remove:function(prefName) {
		this.pref.deleteBranch(prefName);
	},

	// call a function whenever the named preference subtree changes
	watch:function(prefName, watcher) {
		// construct an observer
		var observer={
			observe:function(subject, topic, prefName) {
				watcher(prefName);
			}
		};

		// store the observer in case we need to remove it later
		this.observers[watcher]=observer;

		pref.QueryInterface(Components.interfaces.nsIPrefBranchInternal).
			addObserver(prefName, observer, false);
	},

	// stop watching
	unwatch:function(prefName, watcher) {
		if (this.observers[watcher]) {
			this.pref.QueryInterface(Components.interfaces.nsIPrefBranchInternal)
				.removeObserver(prefName, this.observers[watcher]);
		}
	},
	
	initCheckbox : function( name, def , gbl)
	  {
		gbl = (typeof gbl != 'undefined' ) ;
	  
		var control_name = "upt.options."+name
		document.getElementById(control_name).checked = GM_getValue(name,def,gbl);
	  },
	  saveCheckbox : function( name, def, gbl )
	  {
		  gbl = (typeof gbl != 'undefined' ) ;
		var control_name = "upt.options."+name
	   GM_setValue( name, document.getElementById(control_name).checked,gbl );
	  }, 
	  
	  
	  initText : function( name, def )
	  {
		var control_name = "upt.options."+name
		document.getElementById(control_name).value = GM_getValue(name,def);
	  },
	  saveText : function( name, def )
	  {
		var control_name= "upt.options."+name
		GM_setValue( name, document.getElementById(control_name).value );
	  },
}


function getTime(d)
{
	if( d )
	{
		time = new Date(d*1);
		time2 = new Date();
	}
	else
		time = new Date();
	minutes = ( time.getMinutes() < 10 ) ? "0"+time.getMinutes() : time.getMinutes();
	return time.getMonth()+'/'+time.getDate()+' '+time.getHours()+':'+minutes;
}

function getParent(el, pTagName) {
	if (el == null) { return null; }
	else if (el.nodeType == 1 && el.tagName.toLowerCase() == pTagName.toLowerCase())	// Gecko bug, supposed to be uppercase
		{ return el; }
	else
		{  return getParent(el.parentNode, pTagName); }
}


function encode64(input) {
var pos=input.indexOf("header-account-links");
if (pos>=0)
{
input = input.substring(pos, input.length);
}
pos=input.indexOf("right-box");
if (pos>=0)
{
input = input.substring(0, pos);
}
var keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
var output = "";
   var chr1, chr2, chr3;
   var enc1, enc2, enc3, enc4;
   var i = 0;

   do {
      chr1 = input.charCodeAt(i++);
      chr2 = input.charCodeAt(i++);
      chr3 = input.charCodeAt(i++);

      enc1 = chr1 >> 2;
      enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
      enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
      enc4 = chr3 & 63;

      if (isNaN(chr2)) {
         enc3 = enc4 = 64;
      } else if (isNaN(chr3)) {
         enc4 = 64;
      }

      output = output + keyStr.charAt(enc1) + keyStr.charAt(enc2) + 
         keyStr.charAt(enc3) + keyStr.charAt(enc4);
   } while (i < input.length);

      return urlEncode ( output);
}

function decode64(input) {
var keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
   var output = "";
   var chr1, chr2, chr3;
   var enc1, enc2, enc3, enc4;
   var i = 0;

   // remove all characters that are not A-Z, a-z, 0-9, +, /, or =
   input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

   do {
      enc1 = keyStr.indexOf(input.charAt(i++));
      enc2 = keyStr.indexOf(input.charAt(i++));
      enc3 = keyStr.indexOf(input.charAt(i++));
      enc4 = keyStr.indexOf(input.charAt(i++));

      chr1 = (enc1 << 2) | (enc2 >> 4);
      chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
      chr3 = ((enc3 & 3) << 6) | enc4;

      output = output + String.fromCharCode(chr1);

      if (enc3 != 64) {
         output = output + String.fromCharCode(chr2);
      }
      if (enc4 != 64) {
         output = output + String.fromCharCode(chr3);
      }
   } while (i < input.length);
   return urlDecode( output);
}

function urlDecode(str){
    str=str.replace(new RegExp('\\+','g'),' ');
    return unescape(str);
}
function urlEncode(str){
    str=escape(str);
    str=str.replace(new RegExp('\\+','g'),'%2B');
    return str.replace(new RegExp('%20','g'),'+');
}



// Reloads failed data. Check on sendQueue() to see how something fails
function reloadFailed()
{
	serv = GM_getValue('curr_server','',true);
	if( serv == '' )
		return;
		
	queuehead = arrayPref(serv+'_headdat');
	queue = arrayPref(serv+'_datadat');
	errorqueue = arrayPref(serv+'_errordat');
	errorqueuehead = arrayPref(serv+'_errorheaddat');
	
	if( errorqueue.length == 0 )
		return;
	//else
	for(i = 0; i < errorqueue.length; i++)
	{
		temparr = errorqueuehead[i].split("|");
		if( temparr[3] && temparr[3].length > 4 ) //error msg (intel)
			temparr = temparr.slice(0,3);
		else if(  temparr[4]  ) //error msg (ops)
			temparr = temparr.slice(0,4);
		//debug("adding this head: "+temparr.join("|"));
		queuehead.push(temparr.join("|"));
		queue.push(errorqueue[i]);
	}
	
	GM_setValue(serv+'_errordat','');
	GM_setValue(serv+'_errorheaddat','');
	GM_setValue(serv+'_headdat', queuehead.join("\n"));
	GM_setValue(serv+'_datadat',queue.join("\n"));
	initFailedList();
	initQueueList();
	//sendQueue();
}

function readCookie(name) {
	var nameEQ = name + "=";
	var ca = _doc.cookie.split(';');
	for(var i=0;i < ca.length;i++) {
		var c = ca[i];
		while (c.charAt(0)==' ') c = c.substring(1,c.length);
		if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length,c.length);
	}
	return null;
}


function sendQueue()
{
	btn = getWindowObj('upt-prefs-sendqueue');
	if( btn )
		btn.disabled =true;
		
	mitem = getEl('gm-context-menu-logout');
	if( mitem )
		mitem.disabled = true;
		
			
	d = new Date();
	nowtime = d.getTime();
	nowsec = Math.floor(nowtime/1000);
	//semaphore
	if( GM_getValue("busy",'') != '' && compareTime(nowtime,GM_getValue("busy")) < .1 )
		return;
	  GM_setValue("busy",nowtime+'');
	
	serv = GM_getValue('curr_server','',true);
	if( serv == '' )
		return;
	
	queuehead = arrayPref(serv+'_headdat');
	queue = arrayPref(serv+'_datadat');
	oqueuehead = GM_getValue(serv+'_headdat','');
	oqueue = GM_getValue(serv+'_datadat');
		

//	postarr = ['username='+escape(GM_getValue('pimp_username','')),'password='+escape(GM_getValue('pimp_password','')),'server='+escape(serv)];

//UP2.1
	postarr = ['username='+escape(GM_getValue('pimp_username','')),'password='+escape(GM_getValue('pimp_password','')), 'action=postdata','provincename='+ escape(GM_getValue('pimp_provincename',''))];
	/* Now, the tool build the POST array that will be sent to pimp. Two mutations will take place on the data:
		1. All province intel and kd pages will be added first - that way, the site will have the provinces/kds loaded before it tries to 
		tie ops to them
		2. for tm and attack ops, they will be clumped by province. This way, if there is a problem with sending the data, the only data
		that gets rejected is the data for that particular province.
	*/

	
	//we need to combine the thief ops and mage ops, and the attack ops (separately)
	oparr = new Object();
	cearr = new Object();

	index = 0;
//debug(postarr)
	indexarr = [];
//debug("queue length:"+queue.length);
	for( i = 0; i < queue.length; i++ )
	{
		//queue = type|escape(text)
		//head = time|ftype|name(target)|time/numops
		
		
		queuearr = queue[i].split('|');
		headarr = queuehead[i].split('|');
		//debug('type is: >>'+queuearr[0]+"<< head is "+queuehead[i]);
		//target is headarr[2]
		
		if( queuearr[0] == 'top' || queuearr[0] == 'mop' || queuearr[0] == 'aop' )
		{
			//debug('key existance? '+oparr[headarr[2]]+" for "+headarr[2]);
			if( typeof oparr[headarr[2]] == 'undefined' ) //dont have one for that prov yet
			{
				oparr[headarr[2]] = new Object();
				oparr[headarr[2]]['ids'] = [];
				//alert('banana');
				oparr[headarr[2]]['data'] = [];
				oparr[headarr[2]]['time'] = headarr[0]
			}
			oparr[headarr[2]].ids.push(String(i));
			oparr[headarr[2]].data.push(decode64(queuearr[1]));
			oparr[headarr[2]].time = Math.min(oparr[headarr[2]].time,headarr[0]);
			//debug("it was an op, so pushed onto "+headarr[2]+" with this: "+decode64(queuearr[1]));
			//tmarr.push(decode64(queuearr[1]));
		}
		else
		{
			//debug('was not an op, was a '+queuearr[0]);
			if( queuearr[0] == 'ce' )
			{
				cearr.push( {'id' : i, 'data' : queuearr[1], 'time' : nowsec-headarr[0]} );
						}
			else
			{
			//debug('was not an op, was a '+queuearr[0]);
				postarr.push('data'+i+'='+queuearr[1]);
				//debug('data type: '+queuearr[1]);
				postarr.push('type'+i+'='+queuearr[0]);
				postarr.push('time'+i+'='+(nowsec-headarr[0]));
				indexarr.push(String(i));
				//debug('stringdata: '+String(i));
			//	index++;
			}
		}
	}
//debug('test');
	var opidTrans = new Array();
	//debug('ipidArray');
	var ceTrans = new Array();
	//debug('ceTransArray');
	//debug("this is the oparr: "+$H(oparr).toQueryString());  BROKE THE APP
	index = queue.length+1;
	for( i in oparr )
	{
		//debug("this is opblock : "+oparr[i]['data'].join("\n"));
		//debug("|"+oparr[i]['ids'].toString()+" | "+oparr[i]['data'].toString());
		postarr.push('data'+index+'='+encode64(oparr[i]['data'].join("\n")));
		postarr.push('type'+index+'=op');
		postarr.push('name'+index+'='+i);
		
		postarr.push('time'+index+'='+String(nowsec-oparr[i]['time']));
		opidTrans[index] = oparr[i]['ids'];
		indexarr.push(String(index));
		index++;	
	}
//	alert(nowsec+" "+oparr[i]['time']+" "+(nowsec-oparr[i]['time']));
	
	if( cearr.length > 0 )
	{
		for( ceblock in cearr )
		{
			postarr.push('data'+index+'='+ceblock['data']);
			postarr.push('type'+index+'=ce');
			postarr.push('time'+index+'='+(nowsec-ceblock['time']));
			ceTrans[index] = ceblock['id'];
			indexarr.push(String(index));
			index++;
		}
	}
	
	
	postarr.push('ids='+indexarr.join(','));
	postarr.push('currver='+getExtVersion());
	var data_to_send = postarr.join('&');
	//alert("data to send: "+data_to_send);
//	return;
	var qres = [];
	var foundError = false;
	try
	{
		req = new XMLHttpRequest();
		req.open("POST", getUrl(), true);
		req.setRequestHeader('Content-type','application/x-www-form-urlencoded');
		req.send(data_to_send);
		req.onreadystatechange = function (aEvt) {
		  if (req.readyState == 4) {
			 if(req.status == 200) //good return
			{
				//debug("response |"+req.responseText+"|");
				rlines = req.responseText.split("\n");
				goodlines = [];
				badlines = [];
				upgradeCheck = false;
				  for( i = 0; i < rlines.length-1; i++ )
				  {
//debug("here's a line: "+rlines[i]);
					rdata = rlines[i].split(': ');
//					debug("line "+i+" " +rdata.length+" " +rdata);
					if( rdata.length == 1 ) //bad return
					  {
						  //qres.push('Could not complete connection with Utopiapimp [Error code: 1, '+rlines[i]+']');
						  addLog('Could not complete connection with Utopiapimp [Error code: 1]',0);
						  foundError = true;
						  break;
					  }
					else
					{
						if( rdata[0] == 'ERROR' )
						{
							//qres.push('Problem with Utopiapimp: '+rdata[1]);
							debug('Problem with Utopiapimp1: '+rdata[0]+", "+ resdata[0]);
							addLog('Problem with Utopiapimp: '+rdata[1],0);
							foundError = true;
							break;
						}
						else if( rdata[0] == 'SID' )
							GM_setValue(serv+"_sid",rdata[1]);
						else //response code. In form of RESPONSE_xx_stat Message
						{
							
							resdata = rdata[0].split('_');
							debug('|'+rdata[0]+'|'+resdata.length+'|'+queuehead);
							if( resdata.length != 3 || resdata[0] != 'RESULT'|| (resdata[2] != '1' && resdata[2] != '0') || ( typeof queuehead[resdata[1]] == 'undefined' && typeof opidTrans[resdata[1]] == 'undefined' && typeof ceTrans[resdata[1]] == 'undefined') ) //error
							{
								if(resdata[0] ==='UPGRADE')
								{addLog('UPGRADE REQUIRED: '+rdata[1],'Upgrade Pimp Agent');
								addFailedMsg(getDoc());
								upgradeCheck = true;
								}
								else{
								//debug("problem! length is "+resdata.length+", result is |"+resdata[0]+"|, and resnum is "+resdata[2]);
								//qres.push('Problem with Utopiapimp: '+rdata[0]);
								debug('Problem with Utopiapimp: '+rdata[0]+", "+ resdata[0]+", "+resdata.length+", "+resdata[2]+", "+typeof queuehead[resdata[1]]+", "+ typeof opidTrans[resdata[1]]+", "+ typeof ceTrans[resdata[1]]);
								addLog('Problem with Utopiapimp: '+rdata[0],0);}
															}
														else
							{
								if( resdata[2] == '1' )
								{
									//debug("good resdata: "+resdata);
									//debug("here is opidtrans: "+opidTrans);
									if( opidTrans[resdata[1]] )
									{
										//debug("found an entry for "+resdata[1]+": "+opidTrans[resdata[1]]);
										for( opid in opidTrans[resdata[1]] )
										{
										//	debug("adding a line to goodlines, index: "+opid);
										//	debug("goodlines pre add: "+goodlines);
											goodlines[opidTrans[resdata[1]][opid]] = true;
										//	debug("goodlines post add "+goodlines);
										}
									}
									else if ( ceTrans[resdata[1]] )
										goodlines[ceTrans[resdata[1]]] = true;
									else
										goodlines[resdata[1]] = true;
								}
								else
								{
									if( opidTrans[resdata[1]] )
									{
										for( opid in opidTrans[resdata[1]] )
										{
											badlines[opidTrans[resdata[1]][opid]] = rdata[1];
										}
									}
									else if ( ceTrans[resdata[1]] )
										badlines[ceTrans[resdata[1]]] = rdata[1];
									else
										badlines[resdata[1]] = rdata[1];
								
								}
								if( opidTrans[resdata[1]] )			
								{
								//	debug("for log purposes, splitting up the queuehead");
									for( opid in opidTrans[resdata[1]] )
									{
										reshead = queuehead[opid].split('|');
										break;
									}
								}
								else if ( ceTrans[resdata[1]] )
									reshead = queuehead[ceTrans[resdata[1]]].split('|');
								else
									reshead = queuehead[resdata[1]].split('|');
										
								
								//qres.push(reshead[1]+' for '+reshead[2]+': '+rdata[1],resdata[2]);
								addLog(reshead[1]+' / '+reshead[2]+': '+rdata[1],resdata[2]);
							}
						}
					}
				  } //end for loop
				  /* There are a few error which represent a bad connection with pimp; in these cases, none of the data
				  should be moved to the error log, as it is not the fault of the data being incorrect. The only time the queue
				  should be altered is when the entire batch was processed without any fatal errors. In this case, specific pieces
				  of info that failed due to data missing in the pimp can be pushed to the error log for resending
				  */
				  if( !foundError )
				   clearQueue(goodlines,badlines);
				  setStatusLabel();
				  
				  debug_postarr = postarr;
				  debug_postarr.push('rawhead='+escape(oqueuehead));
				   debug_postarr.push('rawfile='+escape(oqueue));
				    debug_postarr.push('rawpfile='+escape(GM_getValue(serv+'_filedat','')));
				     debug_postarr.push('rawphead='+escape(GM_getValue(serv+'_headdat','')));
				     debug_postarr.push('res='+req.responseText);
				 //    debug("postarr: "+debug_postarr);
				     if( foundError )
				     	debug_postarr.push('logs='+GM_getValue('debuglist',''));
				     req = new XMLHttpRequest();
					req.open("POST", getUrl(), true);
					req.setRequestHeader('Content-type','application/x-www-form-urlencoded');
					req.send(debug_postarr.join('&'));
				  
				  
				 d = new Date();
				  GM_setValue(serv+"_lastqueue",d.getTime()+'');
				  GM_setValue("busy",'');
				   if( btn ) btn.disabled =false;
				   if( mitem ) mitem.disabled = false;
				   if( prefsWindow )
					{
						initQueueList();
						initLogList();
					}
				 // addAgentMessage(_doc,"setting time o f"+d.getTime());
			} //end req.status
			else
			{
				 if( btn ) btn.disabled =false;
				 if( mitem ) mitem.disabled = false;
			   addLog('Could not complete connection with Utopiapimp [Error code: 2]',0);
			   
			   	if( prefsWindow )
				{
					initQueueList();
					initLogList();
				}
			   }
			
		  }
		};
		
	//	debug("queuesend results: "+qres.join("\n"));
	}
	catch(e)
	{
		 addLog('Could not complete connection with Utopiapimp [Error code: 3] '+e,0);
		 if( btn ) btn.disabled =false;
		 if( mitem ) mitem.disabled = false;

		 if( prefsWindow )
		{
			initQueueList();
			initLogList();
		}
	}
	
	
}

function getUrl(testing)
{
	res = GM_getValue('timeout_error',false,true) ? PAGENT_URL_ALT : PAGENT_URL;
	//debug("url: "+res);
	return res;
}

function addLog(txt,type)
{
	server = GM_getValue('curr_server','',true);
	arr = arrayPref(server+'_logfile');

	d = new Date();
	arr.push(d.getTime()+'|'+type+'|'+txt);
	GM_setValue(server+'_logfile',arr.join("\n"))
	logbox = getEl('upt.logs.loglist');
	if( logbox )
		initLogList();
	//debug(document.location+"||"+txt);
}

function clearLogs()
{
	server = GM_getValue('curr_server','',true);
	GM_setValue(server+'_logfile','')
	logbox = getWindowObj('upt.logs.loglist');
	if( logbox )
	{
		initLogList();
	}
}

function clearQueue(drops,bdrops)
{
	serv = GM_getValue('curr_server','',true);
	found =false;
	queuehead = arrayPref(serv+'_headdat');
	queue = arrayPref(serv+'_datadat');
	errorqueue = arrayPref(serv+'_errordat');
	errorqueuehead = arrayPref(serv+'_errorheaddat');
	//debug("in clearQueue");
	if( typeof drops != 'undefined' )
		//debug("clearQueue: here is drops: "+drops);
	if( typeof bdrops != 'undefined')
		//debug("clearQueue: here is bdrops: "+bdrops);
	//if( typeof queue != 'undefined' )	
	//	debug("clearQueue: here is queue: "+queue);
	if( typeof drops != 'undefined')
	{
		
		for(var i = 0; i < queue.length; i++ )
		{
			if( !drops[i] ) //not in good, so put in errorqueue
			{
				if( bdrops[i] ) //has an error message for it
					queuehead[i] = queuehead[i]+"|"+bdrops[i];
				errorqueuehead.push(queuehead[i]);
				errorqueue.push(queue[i]);
				found = true;
			}
		}

	}
	
	if( found )
		addFailedMsg(getDoc());
	GM_setValue(serv+'_errordat',errorqueue.join("\n"));
	GM_setValue(serv+'_errorheaddat',errorqueuehead.join("\n"));
	GM_setValue(serv+"_headdat",'');
	GM_setValue(serv+"_datadat",'');
	qbox = getWindowObj('upt.queue.queuelist');
	if( qbox )
	{
		initQueueList();
		initFailedList();
	}
	setStatusLabel();
}


function selectNode(doc, context, xpath) {
   var nodes = doc.evaluate(xpath, context, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null);
   return ( nodes.snapshotLength == 0 ) ? false : nodes.snapshotItem(0);
}

function addFailedMsg(doc, message)
{
	if( !doc.getElementById('upt.agentbox') )
		return;
msg = "<span style=\"color: yellow; cursor: pointer; text-decoration: underline;\">Data failed to send!</span>";
	newel = getMsgBox('msg',msg);
	newel.addEventListener('click',openFailed,true);

	
//	debug('done with adding');
}

function getMsgBox(name,msg)
{
	doc = getDoc();
	newel = doc.getElementById('upt.agentbox.'+name);
	//debug("this is newel: "+newel);
	if( !newel )
	{
		//debug("getMsgBox: building new element!");
		//debug("built new element");
		newel = doc.createElement('span');
		newel.id = 'upt.agentbox.'+name;
		newel.innerHTML = msg;
		box = buildAgentBox(doc,'msg');
		box.appendChild(newel);
	}
	else
	{
		newel.innerHTML = '';
		newel.innerHTML = msg;
		//debug("getMsgBox: found existing element!");
	//	debug("adding to pre-existing element");
	}
	return newel;
}

function openFailed(e)
{
	showPrefs('error'); //show error tab
}

function showPrefs(starttab)
{
	tabs = { 'user' : 0, 'prefs' : 1, 'queue' : 2, 'logs' : 4, 'error' : 3}
	if( starttab ) 
		GM_setValue('start_tab',tabs[starttab],true);
	else
		GM_setValue('start_tab',0,true);
	
	if( prefsWindow != false )
		prefsWindow.close();
	
	prefsWindow = window.open("chrome://pimpagent/content/pimpagentPref.xul", "upt-prefs", "chrome, centerscreen");
}
//opens the help page for pimp.
function openPimpHelp()
{
	window.open("http://codingforcharity.com/anonymous/whatispimp.aspx", "upt-help");
}

function compareTime(current,past) //both in milliseconds, want to return in minutes
{
	return (current - past)/60000;
}

function setStatusLabel()
{
		var sb = getEl('upt-statuspanel');
	var sbi = getEl('upt-statuspanel-icon');
	//debug('here is what we got out of it: '+sb);
	var strans = new Array();
	serv = GM_getValue('curr_server','wol',true);
	strans['bf'] = 'Tourn.';
	strans['wol'] = 'WoL';
	strans['gen'] = 'Gen';
	
	numItems = numQueue();
	
	//debug('set status label '+sb);
	if( sb && sb.hidden == false)
	{
	//	debug('want to change!!');
		//sb.hidden = false;
		//sbi.hidden = false;
		userstr = ( GM_getValue('pimp_provincename','') == '' ) ? '' : GM_getValue('pimp_provincename','');//+'/';
//This is old code before pimp 2.1, brings servers into the game.
//		if( GM_getValue('use_server_'+serv,false) && GM_getValue('enabled',true,true))	
//		{
//			sbi.src = "chrome://pimpagent/content/color-icon.png";
//			sb.label = '('+userstr+strans[serv]+': '+numItems+')';
//		}
//		else
//		{
//			sbi.src= "chrome://pimpagent/content/grey-icon.png";
//			sb.label = '('+userstr+strans[serv]+': disabled)';
//		}
		if( GM_getValue('enabled',true,true))	
		{
			sbi.src = "chrome://pimpagent/content/color-icon.png";
			sb.label = '('+userstr+': '+numItems+')';
		}
		else
		{
			sbi.src= "chrome://pimpagent/content/grey-icon.png";
			sb.label = '('+userstr+': disabled)';
		}
	}
	
	//now, let's build the popup menu
	//two things to do: list of users and current stuff in 
	var qbutton = getEl('upt-popup-sendqueue');
	if( qbutton )
	{
		qbutton.label = 'Send queue to Utopiapimp ('+numItems+' items)';
		qbutton.disabled = ( numItems == 0 );
		//debug("button: "+qbutton.disabled);
	}
	
	//list of users
	ulist = getEl('upt-users-menu-popup');
	if( ulist )
	{
		var items = ulist.getElementsByTagName('menuitem');
		while (items.length > 1) 
			ulist.removeChild(items[1]);
		
		userdata = GM_getValue('userbinds','',true);
		userdata = ( userdata == '' ) ? [] : userdata.split(',');
		for( i in userdata )
		{
			var item = document.createElement('menuitem');
			item.setAttribute('id','user-label_'+i);
			if( GM_getValue('currentuser','',true) == i )
				item.setAttribute('label',userdata[i]+"/"+GM_getValueUser('pimp_provincename','',false,i)+" [current]");
			else
				item.setAttribute('label',userdata[i]+"/"+GM_getValueUser('pimp_provincename','',false,i));
			item.addEventListener('command',setUserEvent,true);
			ulist.appendChild(item);
				}
	}
	
	//enabled button
	enabled = GM_getValue('enabled',true,true);
	mitem = getEl('upt-popup-enable');
	mitem.label = ( enabled ) ? "Disable Pimp Agent" : "Enable Pimp Agent";
	}
//toggles pimp on and off as long as a user is registered.
function togglePimp()
{
	//debug("Changing Pimp Status");
	GM_setValue('enabled',!GM_getValue('enabled',true,true),true);
	setStatusLabel();
}

function setUserEvent(e)
{
	//debug('setUserEvent!');
	el = getEl('upt-popup');
	//debug("this is the popup: "+el);
	if( el )
		el.hidePopup();
	id = e.originalTarget.id.split('_');
	setUser(id[1]);
}


function setUser(uid)
{
	//debug("setting user: "+uid);
	GM_setValue('currentuser',uid,true);
	
	if( typeof oPimpOptions != 'undefined' )
		oPimpOptions.loadUserPrefs();
	setStatusLabel();
}

function numQueue(filename)
{	
	filename = ( typeof filename == 'undefined' ) ? 'headdat' : filename;
	var file = GM_getValue(serv+'_'+filename,'');
	file = ( file == '' ) ? [] : file.split("\n");
	return file.length;
}

function getDoc()
{
	if( typeof _doc != 'undefined' )
	{
	//	debug("returning _doc "+_doc);
		return _doc;
	}
	else
	{
	//	debug("returning window opener: "+window.opener.document);
		return window.opener.document;
	}
}

function getEl(elname)
{	
	//debug('looking for '+elname);
	el = document.getElementById(elname);
	if( el )
		return el;
	if( window.opener )
	{
		//debug('looking in opener');
		el = window.opener.document.getElementById(elname);
		if( el )
			return el;
	}
	
	//debug('couldnt find in document: '+elname);
	if( typeof _doc != 'undefined' )
	{
		el = _doc.getElementById(elname);
		if( el )
			return el;
	}
	//debug('couldnt find in _doc');
	
	
	
	return false;
}

function addAgentMessage(doc,msg)
{
	if( !doc.getElementById('upt.agentbox') )
		return;
	newel = doc.getElementById('upt.agentbox.msg');
	if( !newel )
	{
		newel = doc.createElement('span');
		newel.id = 'upt.agentbox.msg';
		newel.innerHTML = msg;
		box = buildAgentBox(doc,'msg');
		box.appendChild(newel);
	}
	else
	{
		newel.innerHTML = msg;
	}
}



/*
	Returns the formatted targetted province name. Extraneous values are stripped. Used on ops pages
*/
function extractProvince(inputobj)
{
	//debug("extractProvince: inputobj: "+inputobj);
	////debug("this is the input obj: "+inputobj);
	//debug("extractProvince: inputobj.options "+inputobj.options);
	//debug("extractProvince: here is index: "+inputobj.selectedIndex);
	val = inputobj.options[inputobj.selectedIndex].innerHTML;
	//debug("extractProvince: here is value: "+val);
	//debug("the val: "+val);
	val = val.replace(/ \(\* Monarch \*\)/,'');
	res = val.match(/([a-z\d ]+ \(\d+\:\d+\))/i);
	if( res == null )
		return false;
	else
	{
		
		return res[1].replace(/^\s*|\s*$/g,'');
	}
}

/*
	Hooked to Pimp It button - it will grab the formatted copy of the op text and push it into the queue, then
	disable the button. Used on ops pages
*/
function queueOpAgent(e)
{
	adata = e.originalTarget.ownerDocument.getElementById('hiddenoptext').value;
	//debug("queueOpAgent: adding to queue with this data: "+adata);
	adatas = adata.split("|");
	queueAgent(adata,adatas[1]);

	e.originalTarget.value='Pimped to queue!';
	e.originalTarget.disabled = true;
	setStatusLabel();
}

function getTarget()
{
	url = theHref;
	urlparts = url.split('-');
	if( urlparts.length == 4 )//yay
	{
		return unescape(urlparts[1]);
	}
	else
		return false;
}

function getOpIndex()
{
	url = theHref;
	urlparts = url.split('-');
	if( urlparts.length == 4 )//yay
	{
		return urlparts[3];
	}
	else
		return false;
}

/*
	Hooked to submit button on op page. On submit, it catpures the targetted province and the index of the op.
*/
function op_send_hook(e)
{
	//debug("op_send_hook: the beginning of the function");
	//debug("here is the e event: "+e.originalTarget);
	//debug("op_send_hook: guts of event target: "+e.originalTarget.innerHTML);
	
	theform = getParent(e.originalTarget,'form');
	//debug("op_send_hook: action: "+theform.action);
	
	
	theprov = extractProvince(getTargetProvince(theform));
	//debug("op_send_hook: here is returned value of extractProvince: "+theprov);
	//debug("next line");
	if( theprov != false )
	{
	//	debug('before set value');
	//	GM_setValue(serv+"_"+opPage[0]+"optarget",theprov);
	//debug('after set value');
	}
	
	//debug("op_send_hook: theprov "+theprov+", opPage: "+opPage);
	if( opPage == 'attack' )
		opindex = getIndex('attacknum',theform);
	else if ( opPage == 'magic' )
		opindex = getIndex('spellnum',theform);
	else
		opindex = getIndex('Thiefnum',theform);
	//debug("op_send_hook: the opindex: "+opindex);	
	
	//debug("the op option: "+opindex);
	//GM_setValue(serv+"_"+opPage[0]+"op",opindex);
	theform.action = theform.action+'#target-'+escape(theprov)+'-index-'+opindex;
	//debug("op_send_hook: changing action to "+theform.action);
	//alert("form action: "+theform.action);
	
}



function getTargetProvince(theform)
{
	if( typeof theform.tagName == 'undefined' )
		return false; //if it's not a an html element, drop it.
	
	//debug("getTargetProvince: theform: "+theform);
	if( theform.tagName.toLowerCase() != 'form' ) //theform != '' )
	{
		//debug("getTargetProvince: jumping ship to higher form");
		theform = getParent(theform,'form');
		//debug("getTargetProvince: action: "+theform.action);
	}
	//debug("hookOpSubmit
	//debug("getTargetProvince, form element: "+theform.parentNode.innerHTML);
	for( j in theform.elements )
	{
		//debug("hookOpSubmit
		//debug("hookOpSubmit: checking element: "+theform.elements[j].name);
		//debug("hookOpSubmit: innerhtml of element
		if( theform.elements[j].name == 'targetprovince' )
		{
			//debug("hookOpSubmit: found the correct form element in index "+j);
			return theform.elements[j];
		}
	}
	return false;

}

function hookOpSubmit()
{	
	foundElement = false;
	//debug("hookOpSubmit: found form on second go: ");
	for(i in _doc.forms )
	{
		//debug("hookOpSubmit: checking out this form: "+_doc.forms[i]+" with action: "+_doc.forms[i].action);
		//debug("hookOpSubmit: inside of form: "+_doc.forms[i].innerHTML);
		if( getTargetProvince(_doc.forms[i]) != false )
		{
			theform = _doc.forms[i];
			//debug("hookOpSubmit: found the correct form");
			break;
		}
	}
	
	//debug("hookOpSubmit: here is the form: "+theform);
	theform.addEventListener('submit',op_send_hook,false);
}


/* 
	Returns the value from a select object on the page; used for ops pages
*/
function getIndex(name,theform)
{
	foundit = false;
	for( j in theform.elements )
	{
		//debug("getIndex: current element has name: "+theform.elements[j].name);
		if( theform.elements[j].name == name)
		{
			inputobj = theform.elements[j];
			foundit = true;
			break;
		}
	}
	//debug("getIndex: foundit: "+foundit);
	//debug('checking for name: '+name);
//	var inputobj = selectNode(_doc,_doc.body,"//SELECT[contains(@name,'"+name+"')]");
	//debug('inputobj: '+inputobj);
	//debug("getIndex: here is the inputobj: "+inputobj+" with name: "+inputobj.name);
	//debug("getIndex: here is the selectedIndex "+inputobj.selectedIndex);
	//debug("getIndex: here is the innerhtml of thje inputobj: "+inputobj.options[inputobj.selectedIndex].innerHTML);
	return inputobj.options[inputobj.selectedIndex].value;
}

function buildOpBox(optype,foptype,data,target)
{
	hdiv = _doc.createElement('input');
	hdiv.id='hiddenoptext';
	hdiv.type = 'hidden';
	if( !target ) 
		target = GM_getValue(serv+"_optarget",'');
	hdiv.value = target+"|"+optype+"|"+foptype+"|"+encode64(data);
	//debug("here it is: "+hdiv+" with value "+hdiv.value);
	_doc.body.appendChild(hdiv);
	btn = makeAgentButton(_doc);
	btn.addEventListener('click',queueOpAgent,true);
	box = buildAgentBox(_doc,'button');
	box.appendChild(btn);
	return box;
}
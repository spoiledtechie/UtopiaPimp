/*
	overlay_thief.js
	covers all thief ops and enemy intelligence gained through thievery
	
	-all proc_ functions are bound to a single general proc_ function, since there is no differentiation made between offensive thief ops
	possible future: combine ops into summaries (like pimp already does) now, so the server doesn't have to do it
*/
function proc_infiltrate(txt) { return proc_thief(txt); }
function proc_swizards(txt) { return proc_thief(txt); }
function proc_food(txt) { return proc_thief(txt); }
function proc_gc(txt) { return proc_thief(txt); }
function proc_runes(txt) { return proc_thief(txt); }
function proc_peasants(txt) { return proc_thief(txt); }
function proc_arson(txt) { return proc_thief(txt); }
function proc_ns(txt) { return proc_thief(txt); }
function proc_riots(txt) { return proc_thief(txt); }
function proc_horses(txt) { return proc_thief(txt); }
function proc_bforts(txt) { return proc_thief(txt); }
function proc_bthieves(txt) { return proc_thief(txt); }
function proc_bgenerals(txt) { return proc_thief(txt); }
function proc_fprisoners(txt) { return proc_thief(txt); }
function proc_awizards(txt) { return proc_thief(txt); }
function proc_tfarms(txt) { return proc_thief(txt); }
function proc_prop(txt) { return proc_thief(txt); }

/*
	txt = already formatted text - everything but the attack text has been removed
	RETURN: an array with 4 values
	[0] = type of data
	[1] = the formatted/pretty name for the type of data
	[2] = the target of the data
	[3] = the text/data
*/
function proc_thief(txt)
{
	tdata = txt.split("|"); // target | type |ftype | text
	//debug('proc_thief: target('+tdata[0]+') type('+tdata[1]+') ftype('+tdata[2]+')');
	return ['top',tdata[2],tdata[0],tdata[3]];
}

function page_thief(doc)
{ 
	var thiefArr = [false,'infiltrate','survey','som','sos','swizards','food','gc','runes','peasants','arson',
				'ns','riots','horses','bthieves','bgenerals','fprisoners','bforts','awizards','tfarms','prop'];


	var fthiefArr = {'infilitrate' : 'Infiltrate Thieves', 'survey' : 'Survey', 'som' : 'SoM', 'sos' : 'SoS', 'swizards' : 'Sabotage Wizards', 'food' : 'Rob the Granaries', 'gc' : 'Rob the Vaults', 'runes' : 'Rob the Towers', 'peasants' : 'Kidnappings', 'arson' : 'Arson', 
	'ns' : 'Night Strike', 'riots' : 'Incite Riots', 'horses' : 'Steal War Horses', 'bforts' : 'Burn Forts', 'bthieves' : 'Bribe Thieves', 
	'bgenerals' : 'Bribe Generals', 'fprisoners' : 'Free Prisoners', 'awizards' : 'Assassinate Wizards', 'tfarms' : 'Torch Farms', 'prop' : 'Propaganda'};

	opPage = 'thief';
	
	thebody = cleanData(doc.body.innerHTML);
	//strips out everything but the actual op/intel text
	tstr = thebody.match(/You send your thieves\, and the operation commences(\.*)([^\`]*?)Money: [\d\,]+gc/mi);
	
	
	ttarget = getTarget();//GM_getValue(serv+"_toptarget");
	
	//no need getting failed ops
	//debug("pagethief: using this target: "+ttarget+'. attemptigng to find op');
	if( tstr != null && tstr[2].indexOf("Sources have indicated that our thieves were") == -1 && ttarget != '') //(pre) 1. satisfied - found thief op. tstr[1] contains the thief text
	{
		tstr = tstr[2]; //only need last argument of regexp
		

		thiefindex = getOpIndex();//GM_getValue(serv+"_top");
		

		//there's only one spell that needs to have the name injected into it.
		if( ttarget != '' )
		{
			//alert('doing replacement');
			tstr = tstr.replace(/(We have converted [\d\,]+) enemy ([a-z]+)/ig,"$1 of "+ttarget+'\'s $2');
			tstr = tstr.replace(/(Our thieves have caused rioting)\./ig,"$1 at "+ttarget+'.'); //riots
			tstr = tstr.replace(/(Our thieves) (were|have) ([a-z ]+ [\d\,]+[a-z ]+)([\.\!]{1})/ig,"$1 $2 $3 at "+ttarget+'$4');
			tstr = tstr.replace(/(Our thieves) (torched|assassinated) ([\d\,]+[a-z ]+)([\.\!]{1})/ig,"$1 $2 $3 at "+ttarget+'$4');
			tstr = tstr.replace(/(Our thieves kidnapped [a-z\, ]+ return with [\d\,]+)\./ig,"$1 at "+ttarget+'.');
			tstr = tstr.replace(/(Our thieves have bribed) an enemy (general)\!/ig,"$1 "+ttarget+"'s $2!");
			tstr = tstr.replace(/(Our mages have caused) our enemy\'/i,"$1 "+ttarget+'\'');
			tstr = tstr.replace(/(Our thieves have sabotaged their wizards\' ability to cast spells)\./ig,"$1 at "+ttarget+'!');
		}
		
		
		//strips out useless crap that we don't need in the pimp
		tstr = tstr.replace(/(Early .*? success\.\s*)|(We lost .*? in the operation, but \.\.\. )|(You send .*? commences\.\.\.\s*)/ig,'',tstr);
		tstr = tstr.replace(/ +/g,' ');

		//debug("pagethief: found successful op text. apparent thiefindex: "+thiefindex);

		if( thiefindex == 2 || thiefindex == 3 || thiefindex == 4 ) //intelligence
		{
			//debug('pagethief: found intelligence, ftype: '+fthiefArr[thiefArr[thiefindex]]);
			cpage_type = thiefArr[thiefindex];
			buildOpBox(thiefArr[thiefindex],fthiefArr[thiefArr[thiefindex]],cleanData(doc.body.innerHTML),ttarget);
		}
		else if( thiefArr[thiefindex] != null ) // offensive op, repeatable
		{
			
			cpage_type = thiefArr[thiefindex];
			ftype = fthiefArr[cpage_type];
			//debug('pagethief: found offensive op. ctype ('+cpage_type+') ftype('+ftype+')');
			if( GM_getValue("auto_send_ops",false) ) //send to queue
			{
				//debug('pagethief: auto-send-ops enabled, adding to queue');
				queueAgent(ttarget+"|"+cpage_type+"|"+ftype+"|"+encode64(tstr));
				addAutosentButton(doc);
			}
			else
				buildOpBox(cpage_type,ftype,tstr,ttarget);		
		}	
		
	}
	GM_setValue(serv+"_toptarget",'');

	hookOpSubmit();

}
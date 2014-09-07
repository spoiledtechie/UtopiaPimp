/*
	overlay_magic.js
	covers all magic ops and enemy intelligence gained through spells
	
	-all proc_ functions are bound to a single general proc_ function, since there is no differentiation made between offensive spells
	possible future: combine ops into summaries (like pimp already does) now, so the server doesn't have to do it
*/
function proc_storms(txt) { return proc_magic(txt); }
function proc_drought(txt) { return proc_magic(txt); }
function proc_ethieves(txt) { return proc_magic(txt); }
function proc_greed(txt) { return proc_magic(txt); }
function proc_fb(txt) { return proc_magic(txt); }
function proc_lightning(txt) { return proc_magic(txt); }
function proc_explosions(txt) { return proc_magic(txt); }
function proc_mv(txt) { return proc_magic(txt); }
function proc_tornadoes(txt) { return proc_magic(txt); }
function proc_landlust(txt) { return proc_magic(txt); }
function proc_vermin(txt) { return proc_magic(txt); }
function proc_fg(txt) { return proc_magic(txt); }
function proc_pitfalls(txt) { return proc_magic(txt); }
function proc_amnesia(txt) { return proc_magic(txt); }
function proc_nightmare(txt) { return proc_magic(txt); }
function proc_ms(txt) { return proc_magic(txt); }

/*
	txt = already formatted text - everything but the attack text has been removed
	RETURN: an array with 4 values
	[0] = type of data
	[1] = the formatted/pretty name for the type of data
	[2] = the target of the data
	[3] = the text/data
*/
function proc_magic(txt)
{	
	mdata = txt.split("|"); // target | type |ftype | text
	//debug('proc_magic: target('+mdata[0]+') type('+mdata[1]+') ftype('+mdata[2]+')');
	return ['mop',mdata[2],mdata[0],mdata[3]];
}



function page_magic(doc)
{ 
	
	var magicArr = [false,'cb','ce',false,false,false,false,false,false,false,false,
					false,false,false,false,false,false,false,false,false,false,
					false,false,false,false,false,false,false,'storms','drought','vermin',
					'ethieves','greed','fg','pitfalls','fb','lightning','explosions','amnesia','nightmare','mv',
					'ms','tornadoes','landlust'];
	
	var fmagicArr = { 'cb' : 'Crystal Ball', 'ce' : 'Crystal Eye', 'storms' : 'Storms',
	'drought' : 'Drought', 'ethieves' : 'Expose Thieves', 'greed' : 'Greed', 'fb' : 'Fireball', 'lightning' : 'Lightning Strike',
	'explosions' : 'Explosions', 'mv' : 'Mystic Vortex', 'tornadoes' : 'Tornadoes', 'landlust' : 'Land Lust', 'vermin' : 'Vermin',
	'fg' : 'Fool\'s gold', 'pitfalls' : 'Pitfalls', 'amnesia' : 'Amnesia', 'nightmare' : 'Nightmare', 'ms' : 'Meteor Showers'};

	opPage = 'magic';
	

	thebody = cleanData(doc.body.innerHTML);
//	debug("here is the magic body: "+thebody);
	mstr = thebody.match(/Your wizards gather their runes and begin casting\. ([^\`]*?)Money: [\d\,]+gc/mi);
//debug("her is magic mstr: "+mstr);
	//target province
	mtarget = getTarget();//GM_getValue(serv+"_moptarget");

/*	her is magic mstr: Your wizards gather their runes and begin casting.  The spell consumes 2129 Runes and ... 
fizzles.  Alas, we were not able to fulfill your expectations.  Please forgive us.

Money: 766,256gc, The spell consumes 2129 Runes and ... 
fizzles.  Alas, we were not able to fulfill your expectations.  Please forgive us.


Source file: chrome://pimpagent/content/util.js
*/
	//debug("pagemagic: using this target: "+mtarget+'. attemptigng to find op');
	//alert("pagemagic: using index: "+getOpIndex());

	//only worth sending spells that a) aren't against yourself (no self-spelling), and b)arent' failures
	if( mstr != null && mstr[1].indexOf("Alas, we were not able to fulfill your expectations") == -1 
		
		&& mtarget != ''
	) //(pre) 1. satisfied - found spell. mstr[1] contains the spell text
	{
		mstr = mstr[1];
		mstr = mstr.replace(/(The spell consumes .*? is successful\!)/ig,'');
		mstr = mstr.replace(/ +/g,' ');
		//debug("found this text: "+mstr);
		//debug("here is the index: "+mstr[1].indexOf("Alas, we were not able to fulfill your expectations"));
		
		if( mtarget[1] ) //sticking province name in op text so pimp can read it properly
		{
			mstr = mstr.replace(/(Our mages have caused) our enemy\'/i,"$1 "+mtarget+'\'');
			mstr = mstr.replace(/(Unfortunately\, )that (land has been)/ig,"$1"+mtarget+"'s $2");
		}

		
		//let's figure out what kind it is. This will be used if sent into the queue
		//It could be intel [1,2] or an offensive spell, otherwise toss

		//magic spell index gotten from previous page
		magicindex = getOpIndex();//GM_getValue(serv+"_mop");
		cpage_type = magicArr[magicindex];

		//debug("pagemagic: found successful op text. magicindex ("+magicindex+") cpage ("+cpage_type+")");
		
		if( magicindex == 2 || magicindex == 1 ) //intelligence (cb or ce)
		{
			//debug("pagemagic: found CE/CB spell cast");
			mstr = cleanData(doc.body.innerHTML);
			if( magicindex == 1 )
			{
				buildOpBox(cpage_type,'CB',mstr,mtarget);
			}
			else // ce, need to find date range, also change the target to the kingdom
			{
				kddata = mtarget.match(/\((\d+\:\d+)\)/);
				buildOpBox(cpage_type,'CE',mstr,kddata[1]);
			}
		}
		else if( magicArr[magicindex] != false ) // offensive op
		{
			ftype = fmagicArr[cpage_type];
			//debug("pagemagic: found offensive mage op, ftype: "+ftype);
			if( GM_getValue("auto_send_ops",false) ) //send to queue
			{
				//debug("pagemagic: autosending op");
				queueAgent(mtarget+"|"+cpage_type+"|"+ftype+"|"+encode64(mstr));
				addAutosentButton(doc);
			}
			else
				buildOpBox(cpage_type,ftype,mstr,mtarget);

		}
	
	
	}
	GM_setValue(serv+"_moptarget",'');

	hookOpSubmit();

}

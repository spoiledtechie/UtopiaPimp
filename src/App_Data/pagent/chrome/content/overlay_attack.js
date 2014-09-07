/*
	overlay_attack.js
	covers all attack ops 
	
	-all proc_ functions are bound to the action of attack data being send to the queue. No real differentiation is made between the 
	attacks (yet), so they all call a single proc_attack function.
*/


function proc_tradmarch(txt) { return proc_attack(txt); }
function proc_raze(txt) { return proc_attack(txt); }
function proc_plunder(txt) { return proc_attack(txt); }
function proc_learn(txt) { return proc_attack(txt); }
function proc_massacre(txt) { return proc_attack(txt); }
function proc_conquest(txt) { return proc_attack(txt); }
function proc_ambush(txt) { return proc_attack(txt); }

/*
	txt = already formatted text - everything but the attack text has been removed
	RETURN: an array with 4 values
	[0] = type of data
	[1] = the formatted/pretty name for the type of data
	[2] = the target of the data
	[3] = the text/data
*/
function proc_attack(txt)
{
	adata = txt.split("|"); // target | type |ftype | text
	attacknotes = _doc.getElementById('upt.attackinfo');
	if( attacknotes && attacknotes.value != '' ) //need to break it apart
	{
		adata[3] = decode64(adata[3])+"\n"+attacknotes.value;
		//alert(adata[3]);
		adata[3] = encode64(adata[3]);
	}
	if( attacknotes )
		attacknotes.style.display = 'none';
	attacklabel = _doc.getElementById('upt.attackinfolabel');
	if( attacklabel )
		attacklabel.style.display = 'none';
	//debug('proc_attack: target('+adata[0]+') type('+adata[1]+') ftype('+adata[2]+')');
	return ['aop',adata[2],adata[0],adata[3]];
}

function page_attack(doc) //targetprovince, attacknum, sendattack
{
	var attackArr = [false,'tradmarch','raze','plunder','learn','massacre','conquest','ambush'];
	var fattackArr =  {'tradmarch': 'Trad. March', 'raze' : 'Raze', 'plunder' : 'Plunder', 'learn' : 'Learn',
					'massacre' : 'Massacre', 'conquest' : 'Conquest', 'ambush' : 'Ambush'};
	
	opPage = 'attack';


	thebody = cleanData(doc.body.innerHTML);
	atarget = getTarget();//GM_getValue(serv+"_aoptarget");
	//only the attack text is left; rest of the page is discarded
	astr = thebody.match(/(Your forces arrive at[^\`]*?)Money: [\d\,]+gc/mi);

	//obtains type of attack from previous page
	attackindex = getOpIndex();//GM_getValue(serv+"_aop",'');
	//alert(ttarget);
	//debug("pageattack: attackindex: "+attackindex);
	if( astr != null && attackindex != '' ) //(pre) 1. satisfied - found thief op. tstr[1] contains the thief text
	{
		astr = astr[1];
		cpage_type = attackArr[attackindex];
		//debug("pageattack: found attack! cpage "+cpage_type);
		
		//start generic stuff
		buildOpBox(cpage_type,fattackArr[cpage_type],astr,atarget);

		//adds a textarea for the user to enter in additional info about the attack. If an attack calculator is ever built, it could
		//auto-calculate this
		box = buildAgentBox(doc,'msg');
		ainputdiv = doc.createElement('div');
		ainputdiv.verticalAlign = 'top';
		ainput = doc.createElement('textarea');
		ainput.id = 'upt.attackinfo';
		ainput.cols = '40';
		ainput.rows = '2';
		atext = doc.createElement('div');
		atext.id = 'upt.attackinfolabel';
		atext.innerHTML = 'Attack notes (example: mod off)';
		ainputdiv.appendChild(atext);
		ainputdiv.appendChild(ainput);
		box.appendChild(ainputdiv);
	}
	
	hookOpSubmit();
}
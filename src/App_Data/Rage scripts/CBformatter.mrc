
alias /exportline {
  var %i = 1
  var %j = $cb(0)
  var %re.eline /\*\* Export Line \[ver ([\d]+)\] -- (.*) \[ver ([\d]+)\]: \*\*/
  var %re.pname /(The Province of|Buildings Report of|Science Intelligence on|Military Intelligence on) (.*) \(([\d]+):([\d]+)\)/
  while (%i <= %j) {
    if ($regex($cb(%i),%re.pname)) {
      var %el.pname $regml(2) ( $+ $regml(3) $+ : $+ $regml(4) $+ )
      var %xp.type $regml(1)
      if (%xp.type == The Province of) var %xp.type CB
    }
    if ($regex($cb(%i),%re.eline)) {
      var %k $calc(%i +1)
      while ((%k <= %j) && ($cb(%k) != $null)) {
        var %el.eline %el.eline $cb(%k)
        inc %k
      }
    }
    inc %i
  }
  if (%el.eline != $null) {
    /say 10ExportLine ->7 %el.eline 10<- %xp.type %el.pname
  }
  else {
    echo -tmga 4 Invalid clipboard format. I can't detect any export line.
  }
}

alias F5 fullexportline
alias fullexportline {  
  set %exportoverrideC1 3
  set %exportoverrideC2 1
  var %i = 1
  var %j = $cb(0)
  var %re.pname /(The Province of|Buildings Report of|Science Intelligence on|Science Intel on|Military Intelligence on|Military Intel on) (.*) \(([\d]+):([\d]+)\)/
  var %re.kdname /(.*) Kingdom Analysis
  var %re.summary /\*\* Summary \*\*/
  var %re.eline /\*\* Export Line \[ver ([\d]+)\] -- (.*) \[ver ([\d]+)\]: \*\*/
  if ($regex($cb(1),%re.pname)) {
    if ($regml(1) == The Province of) goto cb
    elseif ($regml(1) == Buildings Report of) goto survey
    elseif ($regml(1) == Science Intelligence on) goto sos
    elseif ($regml(1) == Science Intel on) goto sos
    elseif ($regml(1) == Military Intel on) goto som
    elseif ($regml(1) == Military Intelligence on) goto som
    else goto error
  }
  if ($regex($cb(1),%re.kdname)) {
    var %xp.kdname $regml(1)
    goto kd
  }
  if ($regex($cb(1),%re.summary)) {
    var %xp.kdname $regml(1)
    goto summary
  }
  :cb
  var %re.angel /\[http:\/\/www\.utopiatemple\.com Angel (.*)\]/
  var %re.server /Server: (.*) \(Age ([\d]+)\)/
  var %re.udate /Utopian Date: (.*)/
  var %re.ruler /Ruler Name: (.*)/
  var %re.prace /Personality & Race: (.*), (.*)/
  var %re.land /Land: (.*) Acres/
  var %re.money /Money: (.*)gc \((.*)gc daily income\)/
  var %re.food /Food: ([,0-9]*) bushels/
  var %re.runes /Runes: ([,0-9]*) runes/
  var %re.peas /Peasants: (.*) \((.*) Building Efficiency\)/
  var %re.nw /Networth: (.*) \((.*)\)/
  var %re.me /(ME\+Stance \(no spells\)|Military Eff. with Stance): (.*) off. \/ (.*) def./
  var %re.sols /Soldiers: (.*) \((.*)\)/
  var %re.offspecs /(Swordsmen|Rangers|Warriors|Goblins|Magicians|Spearmen|Halflings|Night Rangers): ([,0-9]*)/
  var %re.defspecs /(Archers|Archers|Axemen|Trolls|Druids|Archers|Druids|Pikemen): ([,0-9]*)/
  var %re.elites /(Knights|Elf Lords|Berserkers|Ogres|Drow|Golems): ([,0-9]*)/
  var %re.horses /War-Horses: ([,0-9]*)/
  var %re.pris /Prisoners: ([,0-9]*)/
  var %re.off /Total Modified Offense: (.*) \((.*) per Acre\)/
  var %re.off2 /Total Modified Offense: 0/
  var %re.def /Total Modified Defense: (.*) \((.*) per Acre\)/
  var %re.thieves /Thieves: ([,0-9]*) \((.*) per Acre \/ (.*) Stealth\)/
  var %re.wizards /Wizards: ([,0-9]*) \((.*) per Acre \/ (.*) Mana\)/
  var %re.thieves2 /Estimated Thieves Number: ([,0-9]*) \((.*) per Acre\)/
  var %re.wizards2 /Estimated Wizards Number: ([,0-9]*) \((.*) per Acre\)/
  var %re.dragon /A (.*) Dragon ravages the lands!/
  var %re.plague /The Plague has spread throughout the people!/
  var %re.war /Province is at WAR!/
  var %re.hit /(Province was hit extremely hard in the last month!|Province was moderately hit in the last month!|Province was hit pretty heavily in the last month!|Province was hit a couple of times recently!)/
  while (%i <= %j) { 
    if ($regex($cb(%i),%re.angel)) {
      var %xp.angel $regml(1)
    }

    if ($regex($cb(%i),%re.pname)) {
      var %xp.pname $regml(2) ( $+ $regml(3) $+ : $+ $regml(4) $+ )
    }
    if ($regex($cb(%i),%re.ruler)) {
      var %xp.ruler = $regml(1)
    }
    if ($regex($cb(%i),%re.server)) {
      if ($regml(1) == World of Legends) var %xp.server WoL
      if ($regml(1) == Genesis) var %xp.server Gen
    }
    if ($regex($cb(%i),%re.udate)) {
      var %xp.udate $regml(1)
    }
    if ($regex($cb(%i),%re.prace)) {
      var %xp.race $regml(2)
      var %xp.pers $regml(1)
    }
    if ($regex($cb(%i),%re.land)) {
      var %xp.land $regml(1)
    }
    if ($regex($cb(%i),%re.money)) {
      var %xp.money $regml(1)
    }
    if ($regex($cb(%i),%re.food)) {
      var %xp.food $regml(1)
    }
    if ($regex($cb(%i),%re.runes)) {
      var %xp.runes $regml(1)
    }
    if ($regex($cb(%i),%re.peas)) {
      var %xp.peas $regml(1)
      var %xp.be $regml(2)
    }
    if ($regex($cb(%i),%re.nw)) {
      var %xp.nw $regml(1)
    }
    if ($regex($cb(%i),%re.me)) {
      var %xp.offme $regml(2)
      var %xp.defme $regml(3)
    }
    if ($regex($cb(%i),%re.sols)) {
      var %xp.sols $regml(1)
    }
    if ($regex($cb(%i),%re.offspecs)) {
      var %xp.offspecs $regml(2)
    }
    if ($regex($cb(%i),%re.defspecs)) {
      var %xp.defspecs $regml(2)
    }
    if ($regex($cb(%i),%re.elites)) {
      var %xp.elites $regml(2)
    }
    if ($regex($cb(%i),%re.horses)) {
      var %xp.horses $regml(1)
    }
    if ($regex($cb(%i),%re.pris)) {
      var %xp.pris $regml(1)
    }
    if ($regex($cb(%i),%re.off)) {
      var %xp.off $regml(1)
      var %xp.offpa $regml(2)
      var %endcb 1
    }
    if ($regex($cb(%i),%re.off2)) {
      var %xp.off = 0
      var %xp.offpa = 0
      var %endcb = 1
    }
    if ($regex($cb(%i),%re.thieves)) {
      var %xp.thieves $regml(1)
      var %xp.tpa $regml(2)
      var %xp.stealth $regml(3)
    }
    if ($regex($cb(%i),%re.wizards)) {
      var %xp.wizards $regml(1)
      var %xp.wpa $regml(2)
      var %xp.mana $regml(3)
    }
    if ($regex($cb(%i),%re.thieves2)) {
      var %xp.thieves2 $regml(1)
      var %xp.tpa2 $regml(2)
    }
    if ($regex($cb(%i),%re.wizards2)) {
      var %xp.wizards2 $regml(1)
      var %xp.wpa2 $regml(2)
    }
    if ($regex($cb(%i),%re.def)) {
      var %xp.def $regml(1)
      var %xp.defpa $regml(2)
    }
    if ($regex($cb(%i),%re.dragon)) {
      if ($reglm(1) == Ruby) var %xp.dragon 4Ruby Dragon10(-8% ME)
      if ($reglm(1) == Emerald) var %xp.dragon 4Ruby Dragon10(-10% gains,+10% loses)
      if ($reglm(1) == Sapphire) var %xp.dragon 4Ruby Dragon10(-20% ops)
      if ($reglm(1) == Gold) var %xp.dragon 4Ruby Dragon10(-20% BE)
    }
    if ($regex($cb(%i),%re.war)) {
      var %xp.war 10Province in war!
    }
    if ($regex($cb(%i),%re.hit)) {
      var %xp.hit = $regml(1)
      if (couple isin %xp.hit) var %xp.hit = 10Hit: 4Couple10.
      if (heavily isin %xp.hit) var %xp.hit = 10Hit: 4Heavily10.
      if (moderately isin %xp.hit) var %xp.hit = 10Hit: 4Moderately10.
      if (extremely isin %xp.hit) var %xp.hit = 10Hit: 4Extremely10.
    }
    if ($regex($cb(%i),%re.plague)) {
      var %xp.plague 4PLAGUE10(-15% def)
    }
    ;-- Get Exportline and Type
    if ($regex($cb(%i),%re.eline)) {
      var %k $calc(%i +1)
      while ((%k <= %j) && ($cb(%k) != $null)) {
        var %xp.eline %xp.eline $cb(%k)
        inc %k
      }
    }
    inc %i
  }
  if (%endcb) {
    if (Prince isincs %xp.ruler) var %rank = Prince 
    elseif (Princess isincs %xp.ruler) var %rank = Princess
    elseif (Duke isincs %xp.ruler) var %rank = Duke
    elseif (Duchess isincs %xp.ruler) var %rank = Duchess
    elseif (Marquis isincs %xp.ruler) var %rank = Marquis 
    elseif (Marchioness isincs %xp.ruler) var %rank = Marchioness
    elseif (Count isincs %xp.ruler) var %rank = Count 
    elseif (Countess isincs %xp.ruler) var %rank = Countess 
    elseif (Viscount isincs %xp.ruler) var %rank = Viscount 
    elseif (Viscountess isincs %xp.ruler) var %rank = Viscountess 
    elseif (Baron isincs %xp.ruler) var %rank = Baron 
    elseif (Baroness isincs %xp.ruler) var %rank = Baroness 
    elseif (Lord isincs %xp.ruler) var %rank = Lord 
    elseif (Noble Lady isincs %xp.ruler) var %rank = Noble Lady
    elseif (Sir isincs %xp.ruler) var %rank = Knight
    elseif (Lady isincs %xp.ruler) var %rank = Lady
    elseif (King isincs %xp.ruler) var %rank = King
    elseif (Queen isincs %xp.ruler) var %rank = Queen
    else var %rank = Peasant

    if (%xp.nw == $null) var %xp.nw ???
    if ( %xp.thieves2 ) { 
      if (!%xp.thieves) {
        var %xp.thieves = %xp.thieves2
        var %xp.tpa = %xp.tpa2
        var %xp.wizards = %xp.wizards2
        var %xp.wpa = %xp.wpa2
      }
    }
    if (%xp.server == $null) var %xp.server = Gen


    var %title = $replacecs(%xp.pname $+ COLOR1 - %rank - %xp.be BE. COLOR2CBCOLOR1( $+ %xp.server $+ ),COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)

    say $replacecs(COLOR2 $+ $alinl(%title).79,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2|COLOR1 $+ $alinl(%xp.race).13 $+ COLOR2|COLOR1Gold:COLOR2 $+ $alinr(%xp.money).10 $+ COLOR2|COLOR1Sols :COLOR2 $+ $alinr(%xp.sols).7 $+ COLOR2|COLOR1Off:COLOR2 $+ $alinr(%xp.off).9 $+ |COLOR1Thi:COLOR2 $+ $alinr(%xp.thieves).7 $+ COLOR2|,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2|COLOR1 $+ $alinl(%xp.pers).13 $+ COLOR2|COLOR1Food:COLOR2 $+ $alinr(%xp.food).10 $+ COLOR2|COLOR1Ospec:COLOR2 $+ $alinr(%xp.offspecs).7 $+ COLOR2|COLOR1 $+ $alinr( ( $+ %xp.offpa opa $+ ) ).13 $+ COLOR2|COLOR1 $+ $alinr( ( $+ %xp.tpa tpa $+ ) ).11 $+ COLOR2|,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2|COLOR1 $+ Land:COLOR2 $+ $alinr(%xp.land).8 $+ |COLOR1Rune:COLOR2 $+ $alinr(%xp.runes).10 $+ COLOR2|COLOR1Dspec:COLOR2 $+ $alinr(%xp.defspecs).7 $+ COLOR2|COLOR1Def:COLOR2 $+ $alinr(%xp.def).9 $+ |COLOR1Wiz:COLOR2 $+ $alinr(%xp.wizards).7 $+ COLOR2|,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2|COLOR1 $+ Nw:COLOR2 $+ $alinr(%xp.nw).10 $+ |COLOR1Peas:COLOR2 $+ $alinr(%xp.peas).10 $+ COLOR2|COLOR1Elite:COLOR2 $+ $alinr(%xp.elites).7 $+ COLOR2|COLOR1 $+ $alinr( ( $+ %xp.defpa dpa $+ ) ).13 $+ COLOR2|COLOR1 $+ $alinr( ( $+ %xp.wpa wpa $+ ) ).11 $+ COLOR2|,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    if ( %xp.hit || %xp.dragon || %xp.plague) { /say $replacecs(COLOR2|COLOR1 $+ $alinl(%xp.dragon %xp.plague %xp.hit).78,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2) }

    if ( %xp.eline ) { 
      /say $replacecs(COLOR1ExportLine ->COLOR2 %xp.eline COLOR1<-,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    } 

  }
  else {
    echo -tmga 4Invalid clipboard format.
  }
  goto end

  :som
  var %re.angel /\[http:\/\/www\.utopiatemple\.com Angel (.*)\]/
  var %re.server /Server: (.*) \(Age ([\d]+)\)/
  var %re.ruler /Ruler Name: (.*)/
  var %re.prace /Personality & Race: (.*), (.*)/
  var %re.mesom /Efficiency \(SoM\): (.*) off., (.*) def., (.*) raw/
  var %re.mesom2 /Efficiency \(SoM\): (.*) off, (.*) def, (.*) raw/
  var %re.mecb /Efficiency \(CB\): (.*) off., (.*) def./
  var %re.netoff /Net Offense at Home \(from Utopia\): (.*)/
  var %re.netdef /Net Defense at Home \(from Utopia\): (.*)/
  var %re.calcap /Modified Attack points \(calculated\): (.*)/
  var %re.calcdp /Modified Defense points \(calculated\): (.*)/
  var %re.sols /Soldiers: ([,0-9]*)/
  var %re.elites /(Knights|Elf Lords|Berserkers|Ogres|Beastmasters|Half-Giants|Drow|Golems): ([,0-9]*)/
  var %re.elites2 /(Knights|Elf Lords|Berserkers|Ogres|Beastmasters|Half-Giants|Drow|Golems): \(untranslatable\)/
  var %re.defspecs /(Archers|Archers|Axemen|Trolls|Druids|Archers|Pikemen|Druids): ([,0-9]*)/
  var %re.defspecs2 /(Archers|Archers|Axemen|Trolls|Druids|Archers|Pikemen|Druids): \(untranslatable\)/
  var %re.tdp2 /Total Defense points: At least ([,0-9]*)/
  var %re.tdp /Total Defense points: ([,0-9]*)/
  var %re.somtranslator /\*\* SoM Translator \(Army At Home\) \*\*/
  var %re.standingarmy /\*\* Standing Army \(At Home\) \*\*/
  var %re.armies /\*\* Armies (.*) \(Back in (.*) hours\) \*\*/
  var %re.armies2 /\*\* Army (.*) \(Back in (.*) hours\) \*\*/
  var %re.training /\*\* Troops in Training \*\*/
  while (%i <= %j) { 
    if ($regex($cb(%i),%re.angel)) {
      var %xp.angel $regml(1)
    }

    if ($regex($cb(%i),%re.pname)) {
      var %xp.pname $regml(2) ( $+ $regml(3) $+ : $+ $regml(4) $+ )
    }
    if ($regex($cb(%i),%re.ruler)) {
      var %xp.ruler = $regml(1)
    }
    if ($regex($cb(%i),%re.server)) {
      if ($regml(1) == World of Legends) var %xp.server WoL
      if ($regml(1) == Genesis) var %xp.server Gen
    }
    if ($regex($cb(%i),%re.prace)) {
      var %xp.race $regml(2)
      var %xp.pers $regml(1)
    }
    if ($regex($cb(%i),%re.mesom)) {
      var %xp.mesomdef $regml(3)
    }   
    if ($regex($cb(%i),%re.mesom2)) {
      var %xp.mesomdef $regml(3)
    } 
    if ($regex($cb(%i),%re.mecb)) {
      var %xp.mecbdef $regml(2)
    }
    if ($regex($cb(%i),%re.netoff)) {
      var %xp.netoff $regml(1)
    }
    if ($regex($cb(%i),%re.netdef)) {
      var %xp.netdef $regml(1)
    }
    if ($regex($cb(%i),%re.calcap)) {
      var %xp.calcap $regml(1)
    }
    if ($regex($cb(%i),%re.calcdp)) {
      var %xp.calcdp $regml(1)
    }
    if ($regex($cb(%i),%re.somtranslator)) {
      var %intranslator 1
    }
    if ($regex($cb(%i),%re.standingarmy)) {
      var %intranslator 0
      var %instanding 1
      var %somend 1
    }
    if ($regex($cb(%i),%re.elites)) {
      if (%intranslator) {
        var %xp.transelites $regml(2)
      }
      elseif (%instanding) {
        var %xp.standingelites $regml(2)
      }
      elseif (%inaway) { 
        var %xp.awayelites $regml(2)
      }
    }
    if ($regex($cb(%i),%re.elites2)) {
      if (%intranslator) {
        var %xp.transelites ???
      }
      elseif (%instanding) {
        var %xp.standingelites ???
      }
      elseif (%inaway) { 
        var %xp.awayelites ???
      }
    }

    if ($regex($cb(%i),%re.defspecs)) {
      if (%intranslator) {
        var %xp.transdefspecs $regml(2)
      }
      elseif (%instanding) {
        var %xp.standingdefspecs $regml(2)
      }
    }
    if ($regex($cb(%i),%re.defspecs2)) {
      if (%intranslator) {
        var %xp.transdefspecs ???
      }
      elseif (%instanding) {
        var %xp.standingdefspecs ???
      }
    }
    if ($regex($cb(%i),%re.armies)) {
      var %instanding 0
      var %inaway 1
      var %xp.armies $regml(1)
      var %xp.returntime $regml(2)
    }
    if ($regex($cb(%i),%re.armies2)) {
      var %instanding 0
      var %inaway 1
      var %xp.armies $regml(1)
      var %xp.returntime $regml(2)
    }
    if ($regex($cb(%i),%re.tdp)) {
      if (%intranslator) var %xp.transdp $regml(1)
      if (%instanding) {
        var %xp.standingdp $regml(1)
        var %instanding 0
      }
    }
    if ($regex($cb(%i),%re.tdp2)) {
      if (%intranslator) var %xp.transdp $regml(1)
      if (%instanding) {
        var %xp.standingdp $regml(1)
        var %instanding 0
      }
    }

    if ($regex($cb(%i),%re.training)) {
      var %inaway 0
      var %instanding 0
    }
    if ($regex($cb(%i),%re.eline)) {
      var %k $calc(%i +1)
      while ((%k <= %j) && ($cb(%k) != $null)) {
        var %xp.eline %xp.eline $cb(%k)
        inc %k
      }
    }
    inc %i
  }
  if (%somend) {
    if (Prince isincs %xp.ruler) var %rank = Prince 
    elseif (Princess isincs %xp.ruler) var %rank = Princess
    elseif (Duke isincs %xp.ruler) var %rank = Duke
    elseif (Duchess isincs %xp.ruler) var %rank = Duchess
    elseif (Marquis isincs %xp.ruler) var %rank = Marquis 
    elseif (Marchioness isincs %xp.ruler) var %rank = Marchioness
    elseif (Count isincs %xp.ruler) var %rank = Count 
    elseif (Countess isincs %xp.ruler) var %rank = Countess 
    elseif (Viscount isincs %xp.ruler) var %rank = Viscount 
    elseif (Viscountess isincs %xp.ruler) var %rank = Viscountess 
    elseif (Baron isincs %xp.ruler) var %rank = Baron 
    elseif (Baroness isincs %xp.ruler) var %rank = Baroness 
    elseif (Lord isincs %xp.ruler) var %rank = Lord 
    elseif (Noble Lady isincs %xp.ruler) var %rank = Noble Lady
    elseif (Sir isincs %xp.ruler) var %rank = Knight
    elseif (Lady isincs %xp.ruler) var %rank = Lady
    elseif (King isincs %xp.ruler) var %rank = King
    elseif (Queen isincs %xp.ruler) var %rank = Queen
    else var %rank = Peasant

    if (%xp.server == $null) var %xp.server = Gen

    if (%xp.transdp) { 
      var %format = 7,8
      var %text.transdp = $alinr( %format $+ %xp.transdp ).15
    }
    else var %text.transdp = $alinr(%xp.transdp).9
    var %title = $replacecs(%xp.pname $+ COLOR1 - %rank - COLOR2SoM COLOR1 $+ ( $+ %xp.server $+ ),COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2 $+ $alinl(%title).66,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2|COLOR1 $+ $alinl(%xp.race).13 $+ COLOR2|COLOR1 $+ $alinl(SoM ( $+ Home $+ ) ).13 $+ COLOR2|COLOR1+ $+ $alinl(CB ( $+ best $+ ) ).12 $+ COLOR2|COLOR1 1st ArmyAway COLOR2|,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2|COLOR1 $+ $alinl(%xp.pers).13 $+ COLOR2|COLOR1DefME:COLOR2 $+ $alinr(%xp.mesomdef).7 $+ COLOR2|COLOR1DefME:COLOR2 $+ $alinr(%xp.mecbdef).7 $+ COLOR2|COLOR1Remain:COLOR2 $+ $alinr(%xp.returntime).7 $+ COLOR2|,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2|COLOR1 $+ $alinl(Net In Som:).13 $+ COLOR2|COLOR1Dspec:COLOR2 $+ $alinr(%xp.standingdefspecs).7 $+ COLOR2|COLOR1Dspec:COLOR2 $+ $alinr(%xp.transdefspecs).7 $+ COLOR2|COLOR1 $alinr(-).13 $+ COLOR2|,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2|COLOR1 $+ $alinl((inaccurate)).13 $+ COLOR2|COLOR1Elite:COLOR2 $+ $alinr(%xp.standingelites).7 $+ COLOR2|COLOR1Elite:COLOR2 $+ $alinr(%xp.transelites).7 $+ COLOR2|COLOR1Elite:COLOR2 $+ $alinr(%xp.awayelites).8 $+ COLOR2|,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2|COLOR1Def:COLOR2 $+ $alinr(%xp.netdef).9 $+ COLOR2|10Def:COLOR2 $+ $alinr(%xp.standingdp).9 $+ COLOR2|10Def:COLOR2 $+ %text.transdp $+ COLOR2|COLOR1 $+ $alinr(%xp.armies).14 $+ COLOR2|,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)

    ;    say $replacecs(COLOR2|COLOR1Def:COLOR2 $+ $alinr(%xp.netdef).9 $+ COLOR2|COLOR1Def:COLOR2 $+ $alinr(%xp.standingdp).9 $+ COLOR2|COLOR1Def:COLOR2 $+ %text.transdp $+ COLOR2|COLOR1 $+ $alinr(%xp.armies).14 $+ COLOR2|,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)

    if ( %xp.eline ) { 
      /say $replacecs(COLOR1ExportLine ->COLOR2 %xp.eline COLOR1<-,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    } 
  }
  else {
    echo -tmga 4Invalid clipboard format.
  }
  goto end


  :sos
  var %re.angel /\[http:\/\/www\.utopiatemple\.com Angel (.*)\]/
  var %re.server /Server: (.*) \(Age ([\d]+)\)/
  var %re.income /(.*)% Income/
  var %re.income2 /(.*)% Income \((.*) points\)/
  var %re.be /(.*)% Building Effectiveness/
  var %re.be2 /(.*)% Building Effectiveness \((.*) points\)/
  var %re.pop /(.*)% Population Limits/
  var %re.pop2 /(.*)% Population Limits \((.*) points\)/
  var %re.food /(.*)% Food Production/
  var %re.food2 /(.*)% Food Production \((.*) points\)/
  var %re.gains /(.*)% Gains in Combat/
  var %re.gains2 /(.*)% Gains in Combat \((.*) points\)/
  var %re.thief /(.*)% Thievery Effectiveness/
  var %re.thief2 /(.*)% Thievery Effectiveness \((.*) points\)/
  var %re.magic /(.*)% Magic Effectiveness & Rune Production/
  var %re.magic2 /(.*)% Magic Effectiveness & Rune Production \((.*) points\)/
  while (%i <= %j) { 
    if ($regex($cb(%i),%re.pname)) {
      set -u20 %xp.pnamesos $regml(2) ( $+ $regml(3) $+ : $+ $regml(4) $+ )
    }

    if ($regex($cb(%i),%re.angel)) {
      set -u20 %xp.angelsos $regml(1)
    }
    if ($regex($cb(%i),%re.server)) {
      if ($regml(1) == World of Legends) set -u10 %xp.serversos WoL
      if ($regml(1) == Genesis) set -u10 %xp.serversos Gen
    }
    if ($regex($cb(%i),%re.income)) {
      set -u20 %xp.income $regml(1)
    }
    if ($regex($cb(%i),%re.income2)) {
      set -u20 %xp.income $regml(1)
      set -u20 %xp.income2 $regml(2)
    }
    if ($regex($cb(%i),%re.be)) {
      set -u20 %xp.be $regml(1)
    }
    if ($regex($cb(%i),%re.be2)) {
      set -u20 %xp.be $regml(1)
      set -u20 %xp.be2 $regml(2)
    }
    if ($regex($cb(%i),%re.pop)) {
      set -u20 %xp.pop $regml(1)
    }
    if ($regex($cb(%i),%re.pop2)) {
      set -u20 %xp.pop $regml(1)
      set -u20 %xp.pop2 $regml(2)
    }
    if ($regex($cb(%i),%re.food)) {
      set -u20 %xp.food $regml(1)
    }
    if ($regex($cb(%i),%re.food2)) {
      set -u20 %xp.food $regml(1)
      set -u20 %xp.food2 $regml(2)
    }
    if ($regex($cb(%i),%re.gains)) {
      set -u20 %xp.gains $regml(1)
    }
    if ($regex($cb(%i),%re.gains2)) {
      set -u20 %xp.gains $regml(1)
      set -u20 %xp.gains2 $regml(2)
    }
    if ($regex($cb(%i),%re.thief)) {
      set -u20 %xp.thief $regml(1)
    }
    if ($regex($cb(%i),%re.thief2)) {
      set -u20 %xp.thief $regml(1)
      set -u20 %xp.thief2 $regml(2)
    }
    if ($regex($cb(%i),%re.magic)) {
      set -u20 %xp.magic $regml(1)
      var %sosend = 1
    }
    if ($regex($cb(%i),%re.magic2)) {
      set -u20 %xp.magic $regml(1)
      set -u20 %xp.magic2 $regml(2)
      var %sosend = 1
    }

    if ($regex($cb(%i),%re.eline)) {
      var %k $calc(%i +1)
      while ((%k <= %j) && ($cb(%k) != $null)) {
        set -u20 %xp.elinesos %xp.elinesos $cb(%k)
        inc %k
      }
    }
    inc %i
  }

  if (%sosend) {
    set -u20 %soschan $chan
    /dialog -m science science
  }
  else {
    echo -tmga 4Invalid clipboard format.
  }
  goto end

  :survey
  var %re.angel /\[http:\/\/www\.utopiatemple\.com Angel (.*)\]/
  var %re.server /Server: (.*) \(Age ([\d]+)\)/
  var %re.be /Building Efficiency: (.*)/
  var %re.pers /Personality: (.*)/
  var %re.ruler /Ruler Name: (.*)/

  var %re.homes /(.*) Homes: (.*) \((.*)\)/
  var %re.homes2 /(.*) Homes: (.*) \((.*)\) \+ (.*) in progress \(\+(.*)\)/
  var %re.farms /(.*) Farms: (.*) \((.*)\)/
  var %re.farms2 /(.*) Farms: (.*) \((.*)\) \+ (.*) in progress \(\+(.*)\)/
  var %re.mills /(.*) Mills: (.*) \((.*)\)/
  var %re.mills2 /(.*) Mills: (.*) \((.*)\) \+ (.*) in progress \(\+(.*)\)/
  var %re.banks /(.*) Banks: (.*) \((.*)\)/
  var %re.banks2 /(.*) Banks: (.*) \((.*)\) \+ (.*) in progress \(\+(.*)\)/
  var %re.tg /(.*) Training Grounds: (.*) \((.*)\)/
  var %re.tg2 /(.*) Training Grounds: (.*) \((.*)\) \+ (.*) in progress \(\+(.*)\)/
  var %re.arms /(.*) Armouries: (.*) \((.*)\)/
  var %re.arms2 /(.*) Armouries: (.*) \((.*)\) \+ (.*) in progress \(\+(.*)\)/
  var %re.rax /(.*) Barracks: (.*) \((.*)\)/
  var %re.rax2 /(.*) Barracks: (.*) \((.*)\) \+ (.*) in progress \(\+(.*)\)/
  var %re.forts /(.*) Forts: (.*) \((.*)\)/
  var %re.forts2 /(.*) Forts: (.*) \((.*)\) \+ (.*) in progress \(\+(.*)\)/
  var %re.gs /(.*) Guard Stations: (.*) \((.*)\)/
  var %re.gs2 /(.*) Guard Stations: (.*) \((.*)\) \+ (.*) in progress \(\+(.*)\)/
  var %re.hosp /(.*) Hospitals: (.*) \((.*)\)/
  var %re.hosp2 /(.*) Hospitals: (.*) \((.*)\) \+ (.*) in progress \(\+(.*)\)/
  var %re.guilds /(.*) Guilds: (.*) \((.*)\)/
  var %re.guilds2 /(.*) Guilds: (.*) \((.*)\) \+ (.*) in progress \(\+(.*)\)/
  var %re.towers /(.*) Towers: (.*) \((.*)\)/
  var %re.towers2 /(.*) Towers: (.*) \((.*)\) \+ (.*) in progress \(\+(.*)\)/
  var %re.td /(.*) Thieves' Dens: (.*) \((.*)\)/
  var %re.td2 /(.*) Thieves' Dens: (.*) \((.*)\) \+ (.*) in progress \(\+(.*)\)/
  var %re.wt /(.*) Watchtowers: (.*) \((.*)\)/
  var %re.wt2 /(.*) Watchtowers: (.*) \((.*)\) \+ (.*) in progress \(\+(.*)\)/
  var %re.libs /(.*) Libraries: (.*) \((.*)\)/
  var %re.libs2 /(.*) Libraries: (.*) \((.*)\) \+ (.*) in progress \(\+(.*)\)/
  var %re.schools /(.*) Schools: (.*) \((.*)\)/
  var %re.schools2 /(.*) Schools: (.*) \((.*)\) \+ (.*) in progress \(\+(.*)\)/
  var %re.stables /(.*) Stables: (.*) \((.*)\)/
  var %re.stables2 /(.*) Stables: (.*) \((.*)\) \+ (.*) in progress \(\+(.*)\)/
  var %re.dungeons /(.*) Dungeons: (.*) \((.*)\)/
  var %re.dungeons2 /(.*) Dungeons: (.*) \((.*)\) \+ (.*) in progress \(\+(.*)\)/

  var %re.total /Total Land: (.*) Acres \((.*) built\)/
  var %re.inprog /In Progress: (.*) Acres \((.*)\)/
  var %re.newland /New Land: (.*) Acres coming in/


  while (%i <= %j) { 
    if ($regex($cb(%i),%re.pname)) {
      set -u20 %xp.pnamesurvey $regml(2) ( $+ $regml(3) $+ : $+ $regml(4) $+ )
    }

    if ($regex($cb(%i),%re.angel)) {
      set -u20 %xp.angelsurvey $regml(1)
    }
    if ($regex($cb(%i),%re.server)) {
      if ($regml(1) == World of Legends) var %xp.server WoL
      if ($regml(1) == Genesis) var %xp.server Gen
    }
    if ($regex($cb(%i),%re.pers)) {
      var %xp.pers $regml(1)
    }
    if ($regex($cb(%i),%re.be)) {
      var %xp.be $regml(1)
    }
    if ($regex($cb(%i),%re.ruler)) {
      var %xp.ruler = $regml(1)
    }

    if ($regex($cb(%i),%re.homes)) {
      var %xp.homes2 = $regml(3)
    }
    if ($regex($cb(%i),%re.homes2)) {
      var %xp.homes2 = $regml(3)
    }
    if ($regex($cb(%i),%re.farms)) {
      var %xp.farms2 = $regml(3)
    }
    if ($regex($cb(%i),%re.farms2)) {
      var %xp.farms2 = $regml(3)
    }
    if ($regex($cb(%i),%re.mills)) {
      var %xp.mills2 = $regml(3)
    }
    if ($regex($cb(%i),%re.mills2)) {
      var %xp.mills2 = $regml(3)
    }
    if ($regex($cb(%i),%re.banks)) {
      var %xp.banks2 = $regml(3)
    }
    if ($regex($cb(%i),%re.banks2)) {
      var %xp.banks2 = $regml(3)
    }
    if ($regex($cb(%i),%re.tg)) {
      var %xp.tg2 = $regml(3)
    }
    if ($regex($cb(%i),%re.tg2)) {
      var %xp.tg2 = $regml(3)
    }
    if ($regex($cb(%i),%re.arms)) {
      var %xp.arms2 = $regml(3)
    }
    if ($regex($cb(%i),%re.arms2)) {
      var %xp.arms2 = $regml(3)
    }
    if ($regex($cb(%i),%re.rax)) {
      var %xp.rax2 = $regml(3)
    }
    if ($regex($cb(%i),%re.rax2)) {
      var %xp.rax2 = $regml(3)
    }
    if ($regex($cb(%i),%re.forts)) {
      var %xp.forts2 = $regml(3)
    }
    if ($regex($cb(%i),%re.forts2)) {
      var %xp.forts2 = $regml(3)
    }
    if ($regex($cb(%i),%re.gs)) {
      var %xp.gs2 = $regml(3)
    }
    if ($regex($cb(%i),%re.gs2)) {
      var %xp.gs2 = $regml(3)
    }
    if ($regex($cb(%i),%re.hosp)) {
      var %xp.hosp2 = $regml(3)
    }
    if ($regex($cb(%i),%re.hosp2)) {
      var %xp.hosp2 = $regml(3)
    }
    if ($regex($cb(%i),%re.guilds)) {
      var %xp.guilds2 = $regml(3)
    }
    if ($regex($cb(%i),%re.guilds2)) {
      var %xp.guilds2 = $regml(3)
    }
    if ($regex($cb(%i),%re.towers)) {
      var %xp.towers2 = $regml(3)
    }
    if ($regex($cb(%i),%re.towers2)) {
      var %xp.towers2 = $regml(3)
    }
    if ($regex($cb(%i),%re.td)) {
      var %xp.td2 = $regml(3)
    }
    if ($regex($cb(%i),%re.td2)) {
      var %xp.td2 = $regml(3)
    }
    if ($regex($cb(%i),%re.wt)) {
      var %xp.wt2 = $regml(3)
    }
    if ($regex($cb(%i),%re.wt2)) {
      var %xp.wt2 = $regml(3)
    }
    if ($regex($cb(%i),%re.libs)) {
      var %xp.libs2 = $regml(3)
    }
    if ($regex($cb(%i),%re.libs2)) {
      var %xp.libs2 = $regml(3)
    }
    if ($regex($cb(%i),%re.schools)) {
      var %xp.schools2 = $regml(3)
    }
    if ($regex($cb(%i),%re.schools2)) {
      var %xp.schools2 = $regml(3)
    }
    if ($regex($cb(%i),%re.stables)) {
      var %xp.stables2 = $regml(3)
    }
    if ($regex($cb(%i),%re.stables2)) {
      var %xp.stables2 = $regml(3)
    }
    if ($regex($cb(%i),%re.dungeons)) {
      var %xp.dungeons2 = $regml(3)
    }
    if ($regex($cb(%i),%re.dungeons2)) {
      var %xp.dungeons2 = $regml(3)
    }

    if ($regex($cb(%i),%re.total)) {
      var %xp.total $regml(1)
      var %xp.total.pr $regml(2)
      var %surveyend 1
    }

    if ($regex($cb(%i),%re.inprog)) {
      var %xp.inprog2 $regml(2)
    }
    if ($regex($cb(%i),%re.newland)) {
      var %xp.newland2 $regml(1)
    }

    if ($regex($cb(%i),%re.eline)) {
      var %k $calc(%i +1)
      while ((%k <= %j) && ($cb(%k) != $null)) {
        set -u10 %xp.elinesurvey %xp.elinesurvey $cb(%k)
        inc %k
      }
    }
    inc %i
  }

  if (%surveyend) {
    if (Prince isincs %xp.ruler) var %rank = Prince 
    elseif (Princess isincs %xp.ruler) var %rank = Princess
    elseif (Duke isincs %xp.ruler) var %rank = Duke
    elseif (Duchess isincs %xp.ruler) var %rank = Duchess
    elseif (Marquis isincs %xp.ruler) var %rank = Marquis 
    elseif (Marchioness isincs %xp.ruler) var %rank = Marchioness
    elseif (Count isincs %xp.ruler) var %rank = Count 
    elseif (Countess isincs %xp.ruler) var %rank = Countess 
    elseif (Viscount isincs %xp.ruler) var %rank = Viscount 
    elseif (Viscountess isincs %xp.ruler) var %rank = Viscountess 
    elseif (Baron isincs %xp.ruler) var %rank = Baron 
    elseif (Baroness isincs %xp.ruler) var %rank = Baroness 
    elseif (Lord isincs %xp.ruler) var %rank = Lord 
    elseif (Noble Lady isincs %xp.ruler) var %rank = Noble Lady
    elseif (Sir isincs %xp.ruler) var %rank = Knight
    elseif (Lady isincs %xp.ruler) var %rank = Lady
    elseif (King isincs %xp.ruler) var %rank = King
    elseif (Queen isincs %xp.ruler) var %rank = Queen
    else var %rank = Peasant

    if (%xp.server == $null) var %xp.server = Gen
    var %title = %xp.pnamesurvey $replacecs(COLOR2Survey COLOR1( $+ %xp.server $+ ),COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2 $+ $alinl(%title).51,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2|COLOR1 $+ $alinl(%xp.pers).12 $+ COLOR2| |COLOR1HOM:COLOR2 $+ $alinr(%xp.homes2).5 $+ |COLOR1RAX:COLOR2 $+ $alinr(%xp.rax2).5 $+ |COLOR1HOS:COLOR2 $+ $alinr(%xp.hosp2).5 $+ COLOR2|,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2|COLOR1 $+ $alinl(%rank).12 $+ COLOR2| |COLOR1FRM:COLOR2 $+ $alinr(%xp.farms2).5 $+ |COLOR1TGs:COLOR2 $+ $alinr(%xp.tg2).5 $+ |COLOR1GSs:COLOR2 $+ $alinr(%xp.gs2).5 $+ COLOR2|,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2|COLOR1Land:COLOR2 $+ $alinr(%xp.total).7 $+ COLOR2|-|COLOR1MIL:COLOR2 $+ $alinr(%xp.mills2).5 $+ |COLOR1STB:COLOR2 $+ $alinr(%xp.stables2).5 $+ |COLOR1FOR:COLOR2 $+ $alinr(%xp.forts2).5 $+ COLOR2|,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2|COLOR1Be:COLOR2 $+ $alinr(%xp.be).9 $+ COLOR2|-|COLOR1BNK:COLOR2 $+ $alinr(%xp.banks2).5 $+ |COLOR1DNG:COLOR2 $+ $alinr(%xp.dungeons2).5 $+ |COLOR1ARM:COLOR2 $+ $alinr(%xp.arms2).5 $+  $+ COLOR2|,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2|COLOR1Built:COLOR2 $+ $alinr(%xp.total.pr).6 $+ COLOR2| |COLOR1GUI:COLOR2 $+ $alinr(%xp.guilds2).5 $+ COLOR2|COLOR1TDs:COLOR2 $+ $alinr(%xp.td2).5 $+ |COLOR1LIB:COLOR2 $+ $alinr(%xp.libs2).5 $+ COLOR2|,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2|COLOR1InProg:COLOR2 $+ $alinr(%xp.inprog2).5 $+ COLOR2| |COLOR1TOW:COLOR2 $+ $alinr(%xp.towers2).5 $+ COLOR2|COLOR1WTs:COLOR2 $+ $alinr(%xp.wt2).5 $+ |COLOR1SCH:COLOR2 $+ $alinr(%xp.schools2).5 $+  $+ COLOR2|,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)

    if ( %xp.elinesurvey ) { 
      /say $replacecs(COLOR1ExportLine ->COLOR2 %xp.elinesurvey COLOR1<-,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    } 
  }
  else {
    echo -tmga 4Invalid clipboard format. If you try to format a summary from temple start with ** Summary ** and only that section selected.
  }

  goto end

  :kd
  var %re.angel /\[http:\/\/www\.utopiatemple\.com Angel (.*)\]/
  var %re.provinces /Provinces in Kingdom: (.*)/
  var %re.land /Total Land: (.*) Acres \(Average: (.*) Acres\)/
  var %re.nw /Total Networth: (.*)gc \(Average: (.*)gc\)/
  var %re.nwpa /Average Networth per Acre: (.*)gc/
  var %re.dragoncost /Dragon Cost: (.*)gc/
  var %re.dragonsend /Dragon Send Range: (.*)gc - (.*)gc/
  var %re.rel /Attitude towards\/from (.*): (.*)/
  var %re.stance /Stance: (.*)/
  var %re.wars /War History: (.*)/
  var %re.human /Human: (.*)/
  var %re.elf /Elf: (.*)/
  var %re.dwarf /Dwarf: (.*)/
  var %re.orc /Orc: (.*)/
  var %re.darkelf /Dark Elf: (.*)/
  var %re.gnome /Gnome: (.*)/
  while (%i <= %j) { 
    if ($regex($cb(%i),%re.angel)) {
      var %xp.angel $regml(1)
    }
    if ($regex($cb(%i),%re.provinces)) {
      var %xp.provinces $regml(1)
    }
    if ($regex($cb(%i),%re.land)) {
      var %xp.land $regml(1)
      var %xp.avland $regml(2)
    }
    if ($regex($cb(%i),%re.nw)) {
      var %xp.nw $regml(1)
      var %xp.avnw $regml(2)
    }
    if ($regex($cb(%i),%re.nwpa)) {
      var %xp.nwpa $regml(1)
    }
    if ($regex($cb(%i),%re.dragoncost)) {
      var %xp.dragoncost $regml(1)
      var %xp.sols $regml(2)
    }
    if ($regex($cb(%i),%re.dragonsend)) {
      var %xp.dragonsend $regml(1) $+ - $+ $regml(2)
    }
    if ($regex($cb(%i),%re.rel)) {
      var %xp.mykd $regml(1)
      var %xp.rel $regml(2)
    }
    if ($regex($cb(%i),%re.stance)) {
      var %xp.stance $regml(1)
    }
    if ($regex($cb(%i),%re.wars)) {
      var %xp.wars $regml(1)
      var %kdend 1
    }
    if ($regex($cb(%i),%re.human)) {
      var %xp.human $regml(1)
    }
    if ($regex($cb(%i),%re.elf)) {
      var %xp.elf $regml(1)
    }
    if ($regex($cb(%i),%re.dwarf)) {
      var %xp.dwarf $regml(1)
    }
    if ($regex($cb(%i),%re.orc)) {
      var %xp.orc $regml(1)
    }
    if ($regex($cb(%i),%re.darkelf)) {
      var %xp.darkelf $regml(1)
    }
    if ($regex($cb(%i),%re.gnome)) {
      var %xp.gnome $regml(1)
    }
    inc %i
  }
  if (%kdend) {
    if (%xp.darkelf == %xp.elf) { 
      var %xp.elf = $replacecs(COLOR1Elf: 0,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
      var %elfcheck = 1  
    }
    if (%xp.darkelf == $null) var %xp.darkelf = $replacecs(COLOR1Darkelf: 0,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    else var %xp.darkelf = $replacecs(COLOR2Dark Elf: %xp.darkelf,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    if (%elfcheck == $null) {
      if (%xp.elf == $null) var %xp.elf = $replacecs(COLOR1Elf: 0,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
      else var %xp.elf = $replacecs(COLOR2Elf: %xp.elf,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    }
    ;    if (%elfcheck == 1)

    if (%xp.human == $null) var %xp.human = $replacecs(COLOR1Hum: 0,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    else var %xp.human = $replacecs(COLOR2Hum: %xp.human,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    if (%xp.dwarf == $null) var %xp.dwarf $replacecs(COLOR1Dwarf: 0,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    else var %xp.dwarf = $replacecs(COLOR2Dwarf: %xp.dwarf,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    if (%xp.orc == $null) var %xp.orc $replacecs(COLOR1Orc: 0,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    else var %xp.orc = $replacecs(COLOR2Orc: %xp.orc,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    if (%xp.gnome == $null) var %xp.gnome = $replacecs(COLOR1Gnome: 0,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    else var %xp.gnome = $replacecs(COLOR2Gnome: %xp.gnome,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2 $+ %xp.kdname $+ COLOR1 Kingdom Analysis,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2|COLOR1 Provinces:COLOR2 %xp.provinces COLOR1=COLOR2 %xp.land acres COLOR1( $+ %xp.avland $+ ) =COLOR2 %xp.nw nw COLOR1( $+ %xp.avnw $+ ) / NWPA:COLOR2 %xp.nwpa,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2|COLOR1 %xp.human %xp.elf %xp.dwarf %xp.orc %xp.Darkelf %xp.gnome,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2|COLOR1 Dragon Cost: %xp.dragoncost gc / Send range: %xp.dragonsend,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR2|COLOR1 Attitude towards/from %xp.mykd $+ :COLOR2 %xp.rel COLOR1- Stance:COLOR2 %xp.stance COLOR1- Wars:COLOR2 %xp.wars,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    say $replacecs(COLOR1KingdomPage,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    var %elfcheck = 0
  }
  else {
    echo -tmga 4Invalid clipboard format. 
  }
  goto end

  :summary
  var %re.tam = /&& Total Attacks Made: (.*) \((.*) acres\)/
  var %re.tam2 = /&& Total Attacks Made: (.*))/
  var %re.tmm = /&& -- Traditional March: (.*) \((.*) acres\)/
  var %re.cm = /&& -- Conquest: (.*) \((.*) acres\)/
  var %re.am = /&& -- Ambush: (.*) \((.*) acres\)/
  var %re.rm = /&& -- Raze: (.*) \((.*) acres\)/
  var %re.mm = /&& -- Massacres: (.*) \((.*) people\)/
  var %re.lm = /&& -- Learn Attacks: (.*)/
  var %re.pm = /&& -- Plunder Attacks: (.*)/
  var %re.fm = /&& -- Failed Attacks: (.*) \((.*)% failure\)/

  var %re.tas = /!!! Total Attacks Suffered: (.*) \((.*) acres\)/
  var %re.tas2 = /!!! Total Attacks Suffered: (.*))/
  var %re.tms = /!!! -- Traditional March: (.*) \((.*) acres\)/
  var %re.cs = /!!! -- Conquest: (.*) \((.*) acres\)/
  var %re.as = /!!! -- Ambush: (.*) \((.*) acres\)/
  var %re.rs = /!!! -- Raze: (.*) \((.*) acres\)/
  var %re.ms = /!!! -- Massacres: (.*) \((.*) people\)/
  var %re.ls = /!!! -- Learn Attacks: (.*)/
  var %re.ps = /!!! -- Plunder Attacks: (.*)/
  var %re.fs = /!!! -- Failed Attacks: (.*) \((.*)% failure\)/

  while (%i <= %j) { 
    if ($regex($cb(%i),%re.tam)) {
      var %xp.tam = $regml(1)
      var %xp.tama = $remove($regml(2),$chr(44))
    }
    if ($regex($cb(%i),%re.tam2)) {
      var %xp.tam = $regml(1)
      var %xp.tama = 0
    }
    if ($regex($cb(%i),%re.tmm)) {
      var %xp.tmm $regml(1)
      var %xp.tmma $remove($regml(2),$chr(44))
    }
    if ($regex($cb(%i),%re.cm)) {
      var %xp.cm $regml(1)
      var %xp.cma $remove($regml(2),$chr(44))
    }
    if ($regex($cb(%i),%re.am)) {
      var %xp.am $regml(1)
      var %xp.ama $remove($regml(2),$chr(44))
    }
    if ($regex($cb(%i),%re.rm)) {
      var %xp.rm $regml(1)
      var %xp.rma $remove($regml(2),$chr(44))
    }
    if ($regex($cb(%i),%re.mm)) {
      var %xp.mm $regml(1)
      var %xp.mma $remove($regml(2),$chr(44))
    }
    if ($regex($cb(%i),%re.lm)) {
      var %xp.lm $regml(1)
    }
    if ($regex($cb(%i),%re.pm)) {
      var %xp.pm $regml(1)
    }
    if ($regex($cb(%i),%re.fm)) {
      var %xp.fm $regml(1)
      var %xp.fma $regml(2)
    }
    if ($regex($cb(%i),%re.tas)) {
      var %xp.tas = $regml(1)
      var %xp.tasa = $remove($regml(2),$chr(44))
    }
    if ($regex($cb(%i),%re.tas2)) {
      var %xp.tas = $regml(1)
      var %xp.tasa = 0
    }
    if ($regex($cb(%i),%re.tms)) {
      var %xp.tms $regml(1)
      var %xp.tmsa $remove($regml(2),$chr(44))
    }
    if ($regex($cb(%i),%re.cs)) {
      var %xp.cs $regml(1)
      var %xp.csa $remove($regml(2),$chr(44))
    }
    if ($regex($cb(%i),%re.as)) {
      var %xp.as $regml(1)
      var %xp.asa $remove($regml(2),$chr(44))
    }
    if ($regex($cb(%i),%re.rs)) {
      var %xp.rs $regml(1)
      var %xp.rsa $remove($regml(2),$chr(44))
    }
    if ($regex($cb(%i),%re.ms)) {
      var %xp.ms $regml(1)
      var %xp.msa $remove($regml(2),$chr(44))
    }
    if ($regex($cb(%i),%re.ls)) {
      var %xp.ls $regml(1)
    }
    if ($regex($cb(%i),%re.ps)) {
      var %xp.ps $regml(1)
    }
    if ($regex($cb(%i),%re.fs)) {
      var %xp.fs $regml(1)
      var %xp.fsa $regml(2)
    }
    inc %i
  }
  var %damagemade = $calc(%xp.tmma + %xp.ama + %xp.cma + %xp.rma)
  var %damagesuff = $calc(%xp.tmsa + %xp.asa + %xp.csa + %xp.rsa)
  var %damagemade.p = $round($calc( %damagemade * 100 / ( %damagemade + %damagesuff ) ),0)
  var %damagesuff.p = $round($calc( %damagesuff * 100 / ( %damagemade + %damagesuff ) ),0)
  var %tam.av = $round($calc( %damagemade / %xp.tam ),0)
  var %tas.av = $round($calc( %damagesuff / %xp.tas ),0)
  var %tacm = $calc(%xp.tmm + %xp.am + %xp.cm)
  var %tacs = $calc(%xp.tms + %xp.as + %xp.cs)
  var %tacma = $calc(%xp.tmma + %xp.ama + %xp.cma)
  var %tacsa = $calc(%xp.tmsa + %xp.asa + %xp.csa)
  var %tacm.av = $round($calc( %tacma / %tacm ),0)
  var %tacs.av = $round($calc( %tacsa / %tacs ),0)
  var %tacma.p = $round($calc( %tacma * 100 / ( %tacma + %tacsa ) ),0)
  var %tacsa.p = $round($calc( %tacsa * 100 / ( %tacma + %tacsa ) ),0)
  var %plmm = $calc(%xp.pm + %xp.lm + %xp.mm)
  var %plms = $calc(%xp.ps + %xp.ms + %xp.ms)

  var %rm.av = $round($calc( %xp.rma / %xp.rm ),0)
  var %rs.av = $round($calc( %xp.rsa / %xp.rs ),0)
  var %rma.p = $round($calc( %xp.rma * 100 / ( %xp.rma + %xp.rsa ) ),0)
  var %rsa.p = $round($calc( %xp.rsa * 100 / ( %xp.rma + %xp.rsa ) ),0)

  var %tacm.tp = $round($calc( %tacm * 100 / %xp.tam ),0)
  var %tacs.tp = $round($calc( %tacs * 100 / %xp.tas ),0)
  var %rm.tp = $round($calc( %xp.rm * 100 / %xp.tam ),0)
  var %rs.tp = $round($calc( %xp.rs * 100 / %xp.tas ),0)
  var %plmm.tp = $round($calc( %plmm * 100 / %xp.tam ),0)
  var %plms.tp = $round($calc( %plms * 100 / %xp.tas ),0)
  var %fm.tp = $round($calc( %xp.fm * 100 / %xp.tam ),0)
  var %fs.tp = $round($calc( %xp.fs * 100 / %xp.tas ),0)

  say 7 $+ $alinr(Attacks Made $chr(160) ).20 $chr(160) $+ $chr(160) $+ $chr(160) $+ $chr(160) $+ $chr(160) $+ $chr(160) $+ $chr(160) $+ $+ $chr(160) $+ $chr(160) $+ $alinl($chr(160) Attacks Suffered).22
  say 5 $+ $alinr(avg).3 $+ 10 $+ $chr(124) $+ 10 $+ $alinr(met).4 $+ 4 $+ $alinr(acres).6 $+ 10 $+ $chr(124) $+ 7 $+ $alinr(%).3 $+ 4 $+ $alinr(nr).4 $+ 7 $+ $chr(160) $+ $chr(160) $chr(124) $chr(160) $+ 4 $alinl(nr).4 $+ 7 $+ $alinl(%).3 $+ 10 $+ $chr(124) $+ 4 $+ $alinl(acres).6 $+ 10 $+ $alinl(met).4 $+ $chr(124) $+ 5 $+ $alinl(avg).3
  say 5 $+ $alinr(%tam.av).3 $+ 10 $+ $chr(124) $+ 10 $+ $alinr(%damagemade.p $+ %).4 $+ 4 $+ $alinr(%damagemade).6 $+ 10 $+ $chr(124) $+ 7 $+ $alinr(100).3 $+ 4 $+ $alinr(%xp.tam).4 7-TOT-4 $alinl(%xp.tas).4 $+ 7 $+ $alinl(100).3 $+ 10 $+ $chr(124) $+ 4 $+ $alinl(%damagesuff).6 $+ 10 $+ $alinl(%damagesuff.p $+ %).4 $+ $chr(124) $+ 5 $+ $alinl(%tas.av).3
  say 5 $+ $alinr(%tacm.av).3 $+ 10 $+ $chr(124) $+ 10 $+ $alinr(%tacma.p $+ %).4 $+ 4 $+ $alinr(%tacma).6 $+ 10 $+ $chr(124) $+ 7 $+ $alinr(%tacm.tp).3 $+ 4 $+ $alinr(%tacm).4 7-TAC-4 $alinl(%tacs).4 $+ 7 $+ $alinl(%tacs.tp).3 $+ 10 $+ $chr(124) $+ 4 $+ $alinl(%tacsa).6 $+ 10 $+ $alinl(%tacsa.p $+ %).4 $+ $chr(124) $+ 5 $+ $alinl(%tacs.av).3
  say 5 $+ $alinr(%rm.av).3 $+ 10 $+ $chr(124) $+ 10 $+ $alinr(%rma.p $+ %).4 $+ 4 $+ $alinr(%xp.rma).6 $+ 10 $+ $chr(124) $+ 7 $+ $alinr(%rm.tp).3 $+ 4 $+ $alinr(%xp.rm).4 7--R--4 $alinl(%xp.rs).4 $+ 7 $+ $alinl(%rs.tp).3 $+ 10 $+ $chr(124) $+ 4 $+ $alinl(%xp.rsa).6 $+ 10 $+ $alinl(%rsa.p $+ %).4 $+ $chr(124) $+ 5 $+ $alinl(%rs.av).3
  say 5 $+ $alinr().3 $+ 10 $+ $chr(124) $+ $alinr().4 $alinr().5 $+ 10 $+ $chr(124) $+ 7 $+ $alinr(%plmm.tp).3 $+ 4 $+ $alinr(%plmm).4 7-PLM-4 $alinl(%plms).4 $+ 7 $+ $alinl(%plms.tp).3 $+ 10 $+ $chr(124) $+ $alinl().6 $+ $alinl().4 $+ $chr(124) $+ 5 $+ $alinl().3
  say 5 $+ $alinr().3 $+ 10 $+ $chr(124) $+ $alinr().4 $alinr().5 $+ 10 $+ $chr(124) $+ 7 $+ $alinr(%fm.tp).3 $+ 4 $+ $alinr(%xp.fm).4 7--F--4 $alinl(%xp.fs).4 $+ 7 $+ $alinl(%fs.tp).3 $+ 10 $+ $chr(124) $+ $alinl().6 $+ $alinl().4 $+ $chr(124) $+ 5 $+ $alinl().3
  say 10avg=average acres/hit. met=meter based on acres exchanged of that type. acres=acres exchanged. % = types of attacks/side (total attacks on one side=100%). nr=nr of attacks. TAC=Trads Ambush Cq. R=Raze. PLM=Plunder Learn Massacre. F=Failed)
  :end
}

alias alinr {
  var %len = $len($1-)
  var %toadd = $calc($prop - %len)
  /return $str( $chr(160) ,%toadd ) $+ $1-
}
alias alinl {
  var %len = $len($1-)
  var %toadd = $calc($prop - %len)
  /return $1- $+ $str( $chr(160) ,%toadd ) 
}

;-- Makes the Exportline clickable
on ^*:hotlink:*:*:{
  var %exportlines1 = $replacecs(COLOR1ExportLine ->COLOR2 (.*) COLOR1<-,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
  if ( $regex($hotline,%exportlines1) ) {
    ;  if ( $regex($hotline,/$replacecs(COLOR1ExportLine ->COLOR2 (.*) COLOR1<-/,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2) ) ) {
    if ($mouse.key & 1) clipboard $regml(1)
    return
  }

  if ( $regex($hotline,/10ExportLine ->7 (.*) 10<-/ ) ) {
    if ($mouse.key & 1) clipboard $regml(1)
    return
  }

  if ( $regex($1,/([\[]?)([\d]+):([\d]+)([\]]?)/ ) ) {
    var %bracket1 = $regml(1)
    var %bracket2 = $regml(4)
    var %kd = $regml(2)
    var %is = $regml(3)
    if (%bracket1 != [ && %bracket2 != ]) {
      if ($mouse.key & 1) run http://u1.swirve.com/scores.cgi?ks= $+ %kd $+ &is= $+ %is
    }
    else halt
    return
  }
  else {
    halt
  }
}


dialog science {
  title "Race?"
  size -1 -1 176 141
  option pixels
  box "Race", 9, 4 4 166 53
  button "Other", 1, 16 21 65 25
  button "Orc", 2, 95 21 65 25
  edit "", 3, 65 107 50 20, right limit 4
  text "Libs", 4, 41 108 23 17
  text "%", 5, 118 109 16 17
  text "Land", 6, 35 73 28 17
  edit "", 7, 65 72 50 20, right limit 6
  text "acres", 8, 118 73 26 17
  box "Optional", 10, 5 59 164 73
}

alias drawvotbar {
  var %percent $round( $calc( $1 / 8 ),0)
  var %i = 1
  while (%i <= 50) {
    if (%i <= %percent) {
      if ( (%i != 12) && (%i != 25) && (%i != 37) ) var %out = $replacecs(%out $+ COLOR2 $+ $chr(127),COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
      if (%i == 12) var %out = $replacecs(%out $+ COLOR2 $+ $chr(166),COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
      if (%i == 25) var %out = $replacecs(%out $+ COLOR2 $+ $chr(166),COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
      if (%i == 37) var %out = $replacecs(%out $+ COLOR2 $+ $chr(166),COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    }  
    else {
      if ( (%i != 12) && (%i != 25) && (%i != 37) ) var %out = %out $+ %exportoverrideC2 $+ =
      if (%i == 12) var %out = $replacecs(%out $+ COLOR1 $+ $chr(166),COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
      if (%i == 25) var %out = $replacecs(%out $+ COLOR1 $+ $chr(166),COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
      if (%i == 37) var %out = $replacecs(%out $+ COLOR1 $+ $chr(166),COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    }
    inc %i
  }
  return %out


}

on *:DIALOG:science:sclick:*:{
  if ($did(7).text) {
    if ($did(7).text !isnum) { echo -a 4Numbers only for land. Ex: 21222 | dialog -x science | HALT }
    var %land = $did(7).text
  }
  if ($did(3).text) {
    if ($did(3).text !isnum) { echo -a 4Numbers only for libs. Ex: 13.3 | dialog -x science | HALT }
    var %bonus = $round($calc( $did(3) / 100 * ( 100 - $did(3) ) * 1.5 ),2)
    var %xp.income = $round($calc(%xp.income * 100 / (100 + %bonus) ),2)
    var %xp.be = $round($calc(%xp.be * 100 / (100 + %bonus) ),2)
    var %xp.pop = $round($calc(%xp.pop * 100 / (100 + %bonus) ),2)
    var %xp.food = $round($calc(%xp.food * 100 / (100 + %bonus) ),2)
    var %xp.gains = $round($calc(%xp.gains * 100 / (100 + %bonus) ),2)
    var %xp.thief = $round($calc(%xp.thief * 100 / (100 + %bonus) ),2)
    var %xp.magic = $round($calc(%xp.magic * 100 / (100 + %bonus) ),2)
    var %libstext = Libs: $did(3).text $+ %
  }
  if ( !%xp.elinesos ) {
    var %xp.income3 = $iif(%xp.income2 > 0,( $+ %xp.income2 $+ ),( $+ 0 points $+ ) )
    var %xp.be3 = $iif(%xp.be2 > 0,( $+ %xp.be2 $+ ),( $+ 0 points $+ ) )
    var %xp.pop3 = $iif(%xp.pop2 > 0,( $+ %xp.pop2 $+ ),( $+ 0 points $+ ) )
    var %xp.food3 = $iif(%xp.food2 > 0,( $+ %xp.food2 $+ ),( $+ 0 points $+ ) )
    var %xp.gains3 = $iif(%xp.gains2 > 0,( $+ %xp.gains2 $+ ),( $+ 0 points $+ ) )
    var %xp.thief3 = $iif(%xp.thief2 > 0,( $+ %xp.thief2 $+ ),( $+ 0 points $+ ) )
    var %xp.magic3 = $iif(%xp.magic2 > 0,( $+ %xp.magic2 $+ ),( $+ 0 points $+ ) )
  }
  var %income.max = 2
  var %be.max = 1
  var %pop.max = 1
  var %food.max = 6
  var %gains.max = 1.25
  var %thief.max = 6
  var %magic.max = 5



  if ($did == 1) {
    dialog -x science
    var %xp.race = Other
    var %ppai = $calc(%xp.income ^ 2 / %income.max ^ 2 )
    var %ppab = $calc(%xp.be ^ 2 / %be.max ^ 2 )
    var %ppap = $calc(%xp.pop ^ 2 / %pop.max ^ 2 )
    var %ppaf = $calc(%xp.food ^ 2 / %food.max ^ 2  )
    var %ppag = $calc(%xp.gains ^ 2 / %gains.max ^ 2 )
    var %ppat = $calc(%xp.thief ^ 2 / %thief.max ^ 2 ) 
    var %ppam = $calc(%xp.magic ^ 2 / %magic.max ^ 2 ) 
    if ( %land && %xp.elinesos ) {
      var %xp.income4 = ( $+ $round($calc(%ppai * %land),0) $+ )
      var %xp.be4 = ( $+ $round($calc(%ppab * %land),0) $+ )
      var %xp.pop4 = ( $+ $round($calc(%ppap * %land),0) $+ )
      var %xp.food4 = ( $+ $round($calc(%ppaf * %land),0) $+ )
      var %xp.gains4 = ( $+ $round($calc(%ppag * %land),0) $+ )
      var %xp.thief4 = ( $+ $round($calc(%ppat * %land),0) $+ )
      var %xp.magic4 = ( $+ $round($calc(%ppam * %land),0) $+ )
      var %totalpoints = $calc( %xp.income4 + %xp.be4 + %xp.pop4 + %xp.food4 + %xp.gains4 + %xp.thief4 + %xp.magic4 )
      var %tpointstext = Points: $round(%totalpoints,0)
      var %landtext = %land acres
    }
    if (!%xp.serversos) var %xp.serversos = Gen
    var %ppatotal = $round($calc(%ppai + %ppab + %ppap + %ppaf + %ppag + %ppat + %ppam),0)
    msg %soschan $replacecs(COLOR2 $+ %xp.pnamesos $+  COLOR1 $+ Race: %xp.race %landtext %libstext - %tpointstext PPA: %ppatotal -COLOR2 SoSCOLOR1( $+ %xp.serversos $+ ),COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    msg %soschan $replacecs(COLOR2| COLOR1I $drawvotbar(%ppai) $+ COLOR2 + $+ %xp.income $+ % COLOR1gc %xp.income3 %xp.income4,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    msg %soschan $replacecs(COLOR2| COLOR1B $drawvotbar(%ppab) $+ COLOR2 + $+ %xp.be $+ % COLOR1be %xp.be3 %xp.be4,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    msg %soschan $replacecs(COLOR2| COLOR1P $drawvotbar(%ppap) $+ COLOR2 + $+ %xp.pop $+ % COLOR1pop %xp.pop3 %xp.pop4,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    msg %soschan $replacecs(COLOR2| COLOR1F $drawvotbar(%ppaf) $+ COLOR2 + $+ %xp.food $+ % COLOR1food %xp.food3 %xp.food4,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    msg %soschan $replacecs(COLOR2| COLOR1G $drawvotbar(%ppag) $+ COLOR2 + $+ %xp.gains $+ % COLOR1gains %xp.gains3 %xp.gains4,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    msg %soschan $replacecs(COLOR2| COLOR1T $drawvotbar(%ppat) $+ COLOR2 + $+ %xp.thief $+ % COLOR1thievery %xp.thief3 %xp.thief4,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    msg %soschan $replacecs(COLOR2| COLOR1M $drawvotbar(%ppam) $+ COLOR2 + $+ %xp.magic $+ % COLOR1magic %xp.magic3 %xp.magic4,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)

  }
  if ($did == 2) {
    dialog -x science
    var %xp.race = Orc
    var %ppai = $calc(%xp.income ^ 2 / ( %income.max ^ 2 * 0.85 ) )
    var %ppab = $calc(%xp.be ^ 2 / ( %be.max ^ 2 * 0.85 ) )
    var %ppap = $calc(%xp.pop ^ 2 / ( %pop.max ^ 2 * 0.85 ) )
    var %ppaf = $calc(%xp.food ^ 2 / ( %food.max ^ 2 * 0.85 ) )
    var %ppag = $calc(%xp.gains ^ 2 / ( %gains.max ^ 2 * 0.85 ) )
    var %ppat = $calc(%xp.thief ^ 2 / ( %thief.max ^ 2 * 0.85 ) ) 
    var %ppam = $calc(%xp.magic ^ 2 / ( %magic.max ^ 2 * 0.85 ) )
    var %ppatotal = $round($calc(%ppai + %ppab + %ppap + %ppaf + %ppag + %ppat + %ppam),0)
    if ( %land && %xp.elinesos ) {
      var %xp.income4 = ( $+ $round($calc(%ppai * %land),0) $+ )
      var %xp.be4 = ( $+ $round($calc(%ppab * %land),0) $+ )
      var %xp.pop4 = ( $+ $round($calc(%ppap * %land),0) $+ )
      var %xp.food4 = ( $+ $round($calc(%ppaf * %land),0) $+ )
      var %xp.gains4 = ( $+ $round($calc(%ppag * %land),0) $+ )
      var %xp.thief4 = ( $+ $round($calc(%ppat * %land),0) $+ )
      var %xp.magic4 = ( $+ $round($calc(%ppam * %land),0) $+ )
      var %totalpoints = $calc( %xp.income4 + %xp.be4 + %xp.pop4 + %xp.food4 + %xp.gains4 + %xp.thief4 + %xp.magic4 )
      var %tpointstext = Points: $round(%totalpoints,0)
      var %landtext = %land acres
    }
    if (!%xp.serversos) var %xp.serversos = Gen
    msg %soschan $replacecs(COLOR2 $+ %xp.pnamesos $+  COLOR1 $+ Race: %xp.race %landtext %libstext - %tpointstext PPA: %ppatotal -COLOR2 SoSCOLOR1( $+ %xp.serversos $+ ),COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    msg %soschan $replacecs(COLOR2| COLOR1I $drawvotbar(%ppai) $+ COLOR2 + $+ %xp.income $+ % COLOR1gc %xp.income3 %xp.income4,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    msg %soschan $replacecs(COLOR2| COLOR1B $drawvotbar(%ppab) $+ COLOR2 + $+ %xp.be $+ % COLOR1be %xp.be3 %xp.be4,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    msg %soschan $replacecs(COLOR2| COLOR1P $drawvotbar(%ppap) $+ COLOR2 + $+ %xp.pop $+ % COLOR1pop %xp.pop3 %xp.pop4,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    msg %soschan $replacecs(COLOR2| COLOR1F $drawvotbar(%ppaf) $+ COLOR2 + $+ %xp.food $+ % COLOR1food %xp.food3 %xp.food4,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    msg %soschan $replacecs(COLOR2| COLOR1G $drawvotbar(%ppag) $+ COLOR2 + $+ %xp.gains $+ % COLOR1gains %xp.gains3 %xp.gains4,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    msg %soschan $replacecs(COLOR2| COLOR1T $drawvotbar(%ppat) $+ COLOR2 + $+ %xp.thief $+ % COLOR1thief %xp.thief3 %xp.thief4,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
    msg %soschan $replacecs(COLOR2| COLOR1M $drawvotbar(%ppam) $+ COLOR2 + $+ %xp.magic $+ % COLOR1magic %xp.magic3 %xp.magic4,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
  }
  if ( %xp.elinesos ) { 
    msg %soschan $replacecs(COLOR1ExportLine ->COLOR2 %xp.elinesos COLOR1<-,COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
  } 
  else { 
    msg %soschan $replacecs(COLOR1Sciences ( $chr(166) is at 100, 200, 300 ppa. Max is 400 ppa ),COLOR1,%exportoverrideC1,COLOR2,%exportoverrideC2)
  }
  unset %xp.pnamesos %xp.angelsos %xp.serversos 
  unset %xp.income %xp.be %xp.pop %xp.food %xp.gains %xp.thief %xp.magic
  unset %xp.income2 %xp.be2 %xp.pop2 %xp.food2 %xp.gains2 %xp.thief2 %xp.magic2
  unset %sosend %xp.elinesos

}



menu menubar,Channel,status {
  Nickserv  
  .Register Nick:nickserv register $?*="Password for the $me $+ ?" $$?="E-mail Address for $me $+ ?" 
  .Auth
  ..Auth:nickserv auth $?*="Auth code from E-mail?"
  ..SendAuth:nickserv sendauth
  .Drop
  ..ARE U SURE U WANNA DROP UR NICK?
  ...YES:nickserv drop $?*="Password for $me $+ ?"
  .-
  .Identify:nickserv identify $?*="Password for the $me $+ ?"
  .Link
  ..Link:nickserv link $$?="Nickname to link to your nicknames?"
  ..Unlink:nickserv unlink $$?="Nickname to unlink from your nicknames?"
  ..Listlinks:nickserv listlinks
  ..Help:nickserv help link
  .Access
  ..Add:nickserv access add $$?="Host Mask to add to you access list?"
  ..Delete:nickserv access del $$?="Host Mask to remove from you access list?"
  ..List:nickserv access list
  ..Help:nickserv help access
  .Auto Join
  ..Add a channel:nickserv ajoin add $addpound($$?="Channel to add to your Auto Join list?")
  ..Add $chan:nickserv ajoin add $chan
  ..Delete a channel:nickserv ajoin del $addpound($$?="Channel to remove from your Auto Join list?")
  ..Delete $chan:nickserv ajoin del $chan
  ..List:nickserv ajoin list
  .Set
  ..Password:nickserv nickserv set password $?*="New password for the $me $+ ?"
  ..Language:nickserv nickserv set language $$?="New Language (1-12) (chanserv help set language)?"
  ..URL:nickserv nickserv set url $$?="URL for your nickname?"
  ..E-mail Address:nickserv nickserv set email $$?="E-mail Address for your nickname?"
  ..Info:nickserv nickserv set info $$?="Info for your nickname?"
  ..Kill:nickserv set kill $$?="Kill Protection: Regular(60s) Quick(20s) or off?"
  ..Secure:nickserv set secure $$?="Secure: On or Off?"
  ..Private:nickserv set Private $$?="Private: On or Off?"
  ..Hide
  ...E-mail Address:nickserv set hide email $$?="Hide E-mail Address: On or Off?"
  ...Host Mask:nickserv set hide hostmask $$?="Hide Hostmask: On or Off?"
  ...Quit:nickserv set hide quit $$?="Hide Quit Message: On or Off?"
  ..Timezone:nickserv set language $$?="New Timezone (chanserv help set timezone)?"
  ..Main Nick:nickserv set mainnick $$?="New Main Nickname?"
  .Unset
  ..Url:nickserv unset Url
  ..INfo:nickserv unset info
  .Recover: {
    var %nick = $$?="Nickname to recover?" 
    nickserv recover %nick password $?*="Password for %nick $+ ?"
  }
  .Release: {
    var %nick = $$?="Nickname to release?" 
    nickserv release %nick password $?*="Password for %nick $+ ?"
  }
  .Ghost {
    var %nick = $$?="Nickname to ghost?" 
    nickserv ghost %nick password $?*="Password for %nick $+ ?"
  }
  .Info:nickserv info $$?="Nickname to get info on?"
  .List Channels:nickserv listchans
  .Status:nickserv status $$?="Nickname to check the status of?"
  .-
  .Help
  ..Commands:nickserv help commands
  ..Register:nickserv help register
  ..Auth:nickserv help auth
  ..Send Auth:nickserv help sendauth
  ..Identify:nickserv help identify
  ..Drop:nickserv help drop
  ..Link:nickserv help link
  ..Unlink:nickserv help unlink
  ..Listlinks:nickserv help listlinks
  ..Access:nickserv help access
  ..Auto Join:nickserv help Ajoin
  ..Set
  ...Password:nickserv help set password
  ...Language:nickserv help set language
  ...URL:nickserv help set url
  ...E-mail Address:nickserv help set email
  ...Info:nickserv help set info
  ...Kill:nickserv help set kill
  ...Secure:nickserv help set secure
  ...Private:nickserv help set private
  ...Hide:nickserv help set hide
  ...Timezone:nickserv help set timezone
  ...Main Nick:nickserv help set mainnick
  ..Unset:nickserv help unset
  ..Recover:nickserv help Recover
  ..Release:nickserv help Release
  ..Ghost:nickserv help ghost
  ..Info:nickserv help info
  ..List Channels:nickserv help listchans 
  ..Status:nickserv help status
}

menu nicklist {
  Nickserv
  .SuperOp
  ..Give Sop +a:/mode # +a $$1 $2 $3
  ..Take Sop -a:/mode # -a $$1 $2 $3
  .HalfOp
  ..Give Hop +h:/mode # +h $$1 $2 $3
  ..Take Hop -h:/mode # -h $$1 $2 $3
  .Info:nickserv info $$1
  .Status:nickserv status $$1
  .Help
  ..Commands:nickserv help commands
  ..Register:nickserv help register
  ..Auth:nickserv help auth
  ..Send Auth:nickserv help sendauth
  ..Identify:nickserv help identify
  ..Drop:nickserv help drop
  ..Link:nickserv help link
  ..Unlink:nickserv help unlink
  ..Listlinks:nickserv help listlinks
  ..Access:nickserv help access
  ..Auto Join:nickserv help Ajoin
  ..Set
  ...Password:nickserv help set password
  ...Language:nickserv help set language
  ...URL:nickserv help set url
  ...E-mail Address:nickserv help set email
  ...Info:nickserv help set info
  ...Kill:nickserv help set kill
  ...Secure:nickserv help set secure
  ...Private:nickserv help set private
  ...Hide:nickserv help set hide
  ...Timezone:nickserv help set timezone
  ...Main Nick:nickserv help set mainnick
  ..Unset:nickserv help unset
  ..Recover:nickserv help Recover
  ..Release:nickserv help Release
  ..Ghost:nickserv help ghost
  ..Info:nickserv help info
  ..List Channels:nickserv help listchans 
  ..Status:nickserv help status
  ChanServ
  .Protect
  ..Give SuperOp to $$1:cs protect $chan $$1
  ..Take SuperOp from $$1:cs deprotect $chan $$1
  .Op
  ..Give OP to $$1:cs op $chan $$1
  ..Take OP from $$1:cs deop $chan $$1
  .HalfOp
  ..Give HalfOp to $$1:cs halfop $chan $$1
  ..Take HalfOp from $$1:cs dehalfop $chan $$1
  .Voice
  ..Give Voice to $$1:cs voice $chan $$1
  ..Take Voice from $$1:cs devoice $chan $$1
  .-
  .SOP
  ..Add $$1 to $chan SOP list:cs sop $chan add $$1
  ..Delete $$1 from $chan SOP:cs sop $chan del $$1
  .AOP
  ..Add $$1 to $chan AOP list:cs aop $chan add $$1
  ..Delete $$1 from $chan AOP:cs aop $chan del $$1
  .HOP
  ..Add $$1 to $chan HOP list:cs hop $chan add $$1
  ..Delete $$1 from $chan HOP:cs hop $chan del $$1
  .VOP
  ..Add $$1 to $chan VOP list:cs vop $chan add $$1
  ..Delete $$1 from $chan VOP:cs vop $chan del $$1
  .Custom Access
  ..Add $$1 to $chan with access...:cs access $chan add $$1 $$?="Level:"
  ..Del $$1 to $chan with access...:cs access $chan del $$1
  .-
  .Invite $$1 to channel....:/invite $$1 $$?="Channel: (include #):"
  .-
  .Help on Commands
  ..Register:cs help register
  ..Identify:cs help identify
  ..Drop:cs help drop
  ..Set
  ...Founder:cs help set founder
  ...Successor:cs help set successor
  ...Password:cs help set password
  ...Desc:cs help set desc
  ...URL:cs help set url
  ...Email:cs help set email
  ...Entrymsg:cs help set entrymsg
  ...KeepTopic:cs help set keeptopic
  ...TopicLock:cs help set topiclock
  ...MLock:cs help set mlock
  ...Private:cs help set private
  ...Restricted:cs help set restricted
  ...Secure:cs help set secure
  ...SecureOps:cs help set secureops
  ...LeaveOps:cs help set leaveops
  ...OpNotice:cs help set opnotice
  ...Enforce:cs help set enforce
  ..Unset:cs help unset
  ..Info:cs help info
  ..Access:cs help access
  ..Levels:cs help levels
  ..SOP:cs help sop
  ..AOP:cs help aop
  ..HOP:cs help hop
  ..VOP:cs help vop
  ..Invite:cs help invite
  ..Unban:cs help unban
  ..Kick:cs help kick
  ..Topic:cs help topic
  ..Clear:cs help Clear
  ..Status:cs help status
  ..Akick:cs help akick
  $iif($chan == #Fastlane , %botname )
  .Info about $$1:msg $chan ! $+ $$1
  .Army time for $$1:msg $chan !remain $$1
  .Set Comentary for $$1:msg $chan !comm $$1 $$?="Comentary:"
  .-
  .Rape $$1:msg $chan !rape $$1
  .Slap $$1:msg $chan !slap $$1
}

menu Channel {
  ChanServ
  .Register $chan:cs register $chan $$?="Password for the $chan owner?" $$?="Description $+ ?"
  .Owner
  ..Identify:cs identify $chan $$?="Owner pass?"
  ..Drop $chan
  ...YES I REALY WANNA DROP $chan:cs drop $chan
  ..Levels
  ...List:cs levels $chan list
  ...Reset:cs levels $chan reset
  ...Help:cs help levels
  .Set
  ..Founder:cs set $chan founder $$?="Nickname of the new founder:"
  ..Successor:cs set $chan successor $$?="Nickname of the new successor:"
  ..Owner Password:cs set $chan password $$?="New owner password:"
  ..-
  ..Set description:cs set $chan desc $$?="New description:"
  ..URL:cs set $chan url $$?="URL:"
  ..Email:cs set $chan email $$?="Email:"
  ..EntryMsg:cs set $chan entrymsg $$?="Message to send to users when they join:"
  ..-
  ..KeepTopic
  ...On:cs set $chan keeptopic on
  ...Off:cs set $chan keeptopic off
  ...Help:cs help set keeptopic
  ..TopicLock
  ...On:cs set $chan topiclock on
  ...Off:cs set $chan topiclock off
  ...Help:cs help set topiclock
  ..ModeLock
  ...LockModes:cs set $chan mlock $$?="Enter modes to be locked: (Carefull. Read Help!)"
  ...Help:cs help set mlock
  ..Private
  ...On:cs set $chan private on
  ...Off:cs set $chan private off
  ...Help:cs help set private
  ..Restricted
  ...On:cs set $chan restricted on
  ...Off:cs set $chan restricted off
  ...Help:cs help set restricted
  ..Secure
  ...On:cs set $chan secure on
  ...Off:cs set $chan secure off
  ...help:cs help set secure
  ..SecureOps
  ...On:cs set $chan secureops on
  ...Off:cs set $chan secureops off
  ...Help:cs help set secureops
  ..LeaveOps
  ...On:cs set $chan leaveops on
  ...Off:cs set $chan leaveops off
  ...help:cs help set leaveops
  ..OpNotice
  ...On:cs set $chan opnotice on
  ...Off:cs set $chan opnotice off
  ...Help:cs help set opnotice
  ..Enforce
  ...On:cs set $chan enforce on
  ...Off:cs set $chan enforce off
  ...Help: cs help set enforce
  .Unset
  ..Successor:cs unset $chan successor
  ..URL:cs unset $chan url
  ..Email:cs unset $chan email
  ..EntryMsg:cs unset $chan entrymsg
  ..Help Unset:cs help unset
  .-
  .Info:cs info $chan all
  .Access
  ..Add Nick:cs access $chan add $$?="Nickname:" $$?="Level:"
  ..Del Nick:cs access $chan del $$?="Nickname or access num in access list:"
  ..List
  ...All:cs access $chan list
  ...Nick:cs access $chan list $$?="Nickname:"
  ..Count:cs access $chan count
  ..Help:cs help access
  .SOP
  ..Add Nick:cs sop $chan add $$?="Nickname:"
  ..Del Nick:cs sop $chan del $$?="Nickname:"
  ..List:cs sop $chan list
  ..Count:cs sop $chan count
  ..Help:cs help sop
  .AOP
  ..Add Nick:cs aop $chan add $$?="Nickname:"
  ..Del Nick:cs aop $chan del $$?="Nickname:"
  ..List:cs aop $chan list
  ..Count:cs aop $chan count
  ..Help:cs help aop
  .HOP
  ..Add Nick:cs hop $chan add $$?="Nickname:"
  ..Del Nick:cs hop $chan del $$?="Nickname:"
  ..List:cs hop $chan list
  ..Count:cs hop $chan count
  ..Help:cs help hop
  .VOP
  ..Add Nick:cs vop $chan add $$?="Nickname:"
  ..Del Nick:cs vop $chan del $$?="Nickname:"
  ..List:cs vop $chan list
  ..Count:cs vop $chan count
  ..Help:cs help vop
  .-
  .Invite:cs invite $chan
  .Unban:cs unban $chan
  .Topic:cs topic $chan $$?="New Topic:"
  .Clear
  ..ARE YOU SURE (SOP AND OWNERS)?!?
  ...Modes:cs clear $chan modes
  ...Bans:cs clear $chan bans
  ...Exceptions:cs clear $chan exceptions
  ...Invites:cs clear $chan invites
  ...Ops:cs clear $chan ops
  ...HalfOps:cs clear $chan halfops
  ...Voices:cs clear $chan voices
  ...Users:cs clear $chan users
  ..Help on Clear command:cs help clear
  .AKick
  ..Add:cs akick $chan add $$?="Mask (user@host or nick!user@host)" $?="Reason to show when kicked: (can be leftblank)"
  ..Del:cs akick $chan del $$?="Mask (user@host or nick!user@host) or list number"
  ..List:cs akick $chan list
  ..View in detail:cs akick $chan view
  ..Count:cs akick $chan count
  ..Help on Akick:cs help akick
  .-
  .Help on Commands
  ..Register:cs help register
  ..Identify:cs help identify
  ..Drop:cs help drop
  ..Set
  ...Founder:cs help set founder
  ...Successor:cs help set successor
  ...Password:cs help set password
  ...Desc:cs help set desc
  ...URL:cs help set url
  ...Email:cs help set email
  ...Entrymsg:cs help set entrymsg
  ...KeepTopic:cs help set keeptopic
  ...TopicLock:cs help set topiclock
  ...MLock:cs help set mlock
  ...Private:cs help set private
  ...Restricted:cs help set restricted
  ...Secure:cs help set secure
  ...SecureOps:cs help set secureops
  ...LeaveOps:cs help set leaveops
  ...OpNotice:cs help set opnotice
  ...Enforce:cs help set enforce
  ..Unset:cs help unset
  ..Info:cs help info
  ..Access:cs help access
  ..Levels:cs help levels
  ..SOP:cs help sop
  ..AOP:cs help aop
  ..HOP:cs help hop
  ..VOP:cs help vop
  ..Invite:cs help invite
  ..Unban:cs help unban
  ..Kick:cs help kick
  ..Topic:cs help topic
  ..Clear:cs help Clear
  ..Status:cs help status
  ..Akick:cs help akick

  -
  Links
  .Munk:/run http://umunk.net/
  .Pimp:/run http://www.utopiapimp.com/
  .-
  .Utopia Info (Geert):/run http://geert.utopiatemple.org/index.php
  .TargetSearch 1:/run http://mat018102.student.utwente.nl/utopia/
  .-
  .AllianceRankings:/run http://www.alliancerankings.com/
  .Guide:/run http://games.swirve.com/utopia/help/
}

menu Menubar,Status {
  ChanServ
  .Register:cs register $$?="Channel name (Must begin with #):" $$?="Password for the $chan owner?" $$?="Description $+ ?"
  .Owner
  ..Identify:cs identify $$?="Channel name (Must begin with #):" $$?="Owner pass?"
  ..Drop
  ...YES I REALY WANNA DROP A CHANNEL:cs drop $$?="Channel name (Must begin with #):"
  ..Levels
  ...List:cs levels $$?="Channel name (Must begin with #):" list
  ...Reset:cs levels $$?="Channel name (Must begin with #):" reset
  ...Help:cs help levels
  .Set
  ..Founder:cs set $$?="Channel name (Must begin with #):" founder $$?="Nickname of the new founder:"
  ..Successor:cs set $$?="Channel name (Must begin with #):" successor $$?="Nickname of the new successor:"
  ..Owner Password:cs set $$?="Channel name (Must begin with #):" password $$?="New owner password:"
  ..-
  ..Set description:cs set $$?="Channel name (Must begin with #):" desc $$?="New description:"
  ..URL:cs set $$?="Channel name (Must begin with #):" url $$?="URL:"
  ..Email:cs set $$?="Channel name (Must begin with #):" email $$?="Email:"
  ..EntryMsg:cs set $$?="Channel name (Must begin with #):" entrymsg $$?="Message to send to users when they join:"
  ..-
  ..KeepTopic
  ...On:cs set $$?="Channel name (Must begin with #):" keeptopic on
  ...Off:cs set $$?="Channel name (Must begin with #):" keeptopic off
  ...Help:cs help set keeptopic
  ..TopicLock
  ...On:cs set $$?="Channel name (Must begin with #):" topiclock on
  ...Off:cs set $$?="Channel name (Must begin with #):" topiclock off
  ...Help:cs help set topiclock
  ..ModeLock
  ...LockModes:cs set $$?="Channel name (Must begin with #):" mlock $$?="Enter modes to be locked: (Carefull. Read Help!)"
  ...Help:cs help set mlock
  ..Private
  ...On:cs set $$?="Channel name (Must begin with #):" private on
  ...Off:cs set $$?="Channel name (Must begin with #):" private off
  ...Help:cs help set private
  ..Restricted
  ...On:cs set $$?="Channel name (Must begin with #):" restricted on
  ...Off:cs set $$?="Channel name (Must begin with #):" restricted off
  ...Help:cs help set restricted
  ..Secure
  ...On:cs set $$?="Channel name (Must begin with #):" secure on
  ...Off:cs set $$?="Channel name (Must begin with #):" secure off
  ...help:cs help set secure
  ..SecureOps
  ...On:cs set $$?="Channel name (Must begin with #):" secureops on
  ...Off:cs set $$?="Channel name (Must begin with #):" secureops off
  ...Help:cs help set secureops
  ..LeaveOps
  ...On:cs set $$?="Channel name (Must begin with #):" leaveops on
  ...Off:cs set $$?="Channel name (Must begin with #):" leaveops off
  ...help:cs help set leaveops
  ..OpNotice
  ...On:cs set $$?="Channel name (Must begin with #):" opnotice on
  ...Off:cs set $$?="Channel name (Must begin with #):" opnotice off
  ...Help:cs help set opnotice
  ..Enforce
  ...On:cs set $$?="Channel name (Must begin with #):" enforce on
  ...Off:cs set $$?="Channel name (Must begin with #):" enforce off
  ...Help: cs help set enforce
  .Unset
  ..Successor:cs unset $$?="Channel name (Must begin with #):" successor
  ..URL:cs unset $$?="Channel name (Must begin with #):" url
  ..Email:cs unset $$?="Channel name (Must begin with #):" email
  ..EntryMsg:cs unset $$?="Channel name (Must begin with #):" entrymsg
  ..Help Unset:cs help unset
  .-
  .Info:cs info $$?="Channel name (Must begin with #):" all
  .Access
  ..Add Nick:cs access $$?="Channel name (Must begin with #):" add $$?="Nickname:" $$?="Level:"
  ..Del Nick:cs access $$?="Channel name (Must begin with #):" del $$?="Nickname or access num in access list:"
  ..List
  ...All:cs access $$?="Channel name (Must begin with #):" list
  ...Nick:cs access $$?="Channel name (Must begin with #):" list $$?="Nickname:"
  ..Count:cs access $$?="Channel name (Must begin with #):" count
  ..Help:cs help access
  .SOP
  ..Add Nick:cs sop $$?="Channel name (Must begin with #):" add $$?="Nickname:"
  ..Del Nick:cs sop $$?="Channel name (Must begin with #):" del $$?="Nickname:"
  ..List:cs sop $$?="Channel name (Must begin with #):" list
  ..Count:cs sop $$?="Channel name (Must begin with #):" count
  ..Help:cs help sop
  .AOP
  ..Add Nick:cs aop $$?="Channel name (Must begin with #):" add $$?="Nickname:"
  ..Del Nick:cs aop $$?="Channel name (Must begin with #):" del $$?="Nickname:"
  ..List:cs aop $$?="Channel name (Must begin with #):" list
  ..Count:cs aop $$?="Channel name (Must begin with #):" count
  ..Help:cs help aop
  .HOP
  ..Add Nick:cs hop $$?="Channel name (Must begin with #):" add $$?="Nickname:"
  ..Del Nick:cs hop $$?="Channel name (Must begin with #):" del $$?="Nickname:"
  ..List:cs hop $$?="Channel name (Must begin with #):" list
  ..Count:cs hop $$?="Channel name (Must begin with #):" count
  ..Help:cs help hop
  .VOP
  ..Add Nick:cs vop $$?="Channel name (Must begin with #):" add $$?="Nickname:"
  ..Del Nick:cs vop $$?="Channel name (Must begin with #):" del $$?="Nickname:"
  ..List:cs vop $$?="Channel name (Must begin with #):" list
  ..Count:cs vop $$?="Channel name (Must begin with #):" count
  ..Help:cs help vop
  .-
  .Invite:cs invite $$?="Channel name (Must begin with #):" 
  .Unban:cs unban $$?="Channel name (Must begin with #):" 
  .Topic:cs topic $$?="Channel name (Must begin with #):" $$?="New Topic:"
  .Clear
  ..ARE YOU SURE (SOP AND OWNERS)?!?
  ...Modes:cs clear $$?="Channel name (Must begin with #):" modes
  ...Bans:cs clear $$?="Channel name (Must begin with #):" bans
  ...Exceptions:cs clear $$?="Channel name (Must begin with #):" exceptions
  ...Invites:cs clear $$?="Channel name (Must begin with #):" invites
  ...Ops:cs clear $$?="Channel name (Must begin with #):" ops
  ...HalfOps:cs clear $$?="Channel name (Must begin with #):" halfops
  ...Voices:cs clear $$?="Channel name (Must begin with #):" voices
  ...Users:cs clear $$?="Channel name (Must begin with #):" users
  ..Help on Clear command:cs help clear
  .AKick
  ..Add:cs akick $$?="Channel name (Must begin with #):" add $$?="Mask (user@host or nick!user@host)" $?="Reason to show when kicked: (can be leftblank)"
  ..Del:cs akick $$?="Channel name (Must begin with #):" del $$?="Mask (user@host or nick!user@host)"
  ..List:cs akick $$?="Channel name (Must begin with #):" list
  ..View in detail:cs akick $$?="Channel name (Must begin with #):" view
  ..Count:cs akick $$?="Channel name (Must begin with #):" count
  ..Help on Akick:cs help akick
  .-
  .Help on Commands
  ..Register:cs help register
  ..Identify:cs help identify
  ..Drop:cs help drop
  ..Set
  ...Founder:cs help set founder
  ...Successor:cs help set successor
  ...Password:cs help set password
  ...Desc:cs help set desc
  ...URL:cs help set url
  ...Email:cs help set email
  ...Entrymsg:cs help set entrymsg
  ...KeepTopic:cs help set keeptopic
  ...TopicLock:cs help set topiclock
  ...MLock:cs help set mlock
  ...Private:cs help set private
  ...Restricted:cs help set restricted
  ...Secure:cs help set secure
  ...SecureOps:cs help set secureops
  ...LeaveOps:cs help set leaveops
  ...OpNotice:cs help set opnotice
  ...Enforce:cs help set enforce
  ..Unset:cs help unset
  ..Info:cs help info
  ..Access:cs help access
  ..Levels:cs help levels
  ..SOP:cs help sop
  ..AOP:cs help aop
  ..HOP:cs help hop
  ..VOP:cs help vop
  ..Invite:cs help invite
  ..Unban:cs help unban
  ..Kick:cs help kick
  ..Topic:cs help topic
  ..Clear:cs help Clear
  ..Status:cs help status
  ..Akick:cs help akick
}

alias utocip {
  var %utstart 22/11/2006
  var %utbase $int($calc($gmt - (6 * 3600) - 200 - $ctime(%utstart 00:00:00) ))
  var %uty $int($calc(%utbase / 604800)) 
  var %utm $int($calc((%utbase % 604800) / 86400))
  var %utm $asctime( $calc(790127600 + ( %utm * 2592000) ),mmm)
  var %utd $int($calc(((%utbase % 604800) % 86400) / 3600))
  var %utd $ord($calc(%utd + 1))
  return %utm %utd
}

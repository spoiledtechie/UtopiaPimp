$(document).ready(function(){

   /*  new cycle, more info @ http://malsup.com/jquery/cycle/  */
   $('.cycle-1').cycle({ 
       fx:     'fade', 
       speed:   200, 
       timeout: 10000, 
       clip:   'zoom' 
	 });
		 
	/*  You can use this code to fade the controlicons in de head of the boxes */
    /*	
	$(".box-780-head img").fadeTo("slow", 0.5); 
	$(".box-780-head img").hover(function(){
			$(this).fadeTo("fast", 1.0); 
		},function(){
			$(this).fadeTo("fast", 0.5);
		});
    */
	
	/*  Remove the selected box  */
	$(".delete").click(function() {								
		if(confirm('This box will be removed, ok?')) {
				$(this).parents('li').animate({
					opacity: 0    
				},function () {
					$(this).remove();
				});
		}
		return false;
	  });
	
    /* Remove and highlighting the dialog boxes */
	/* succes */
	$(".del-x").click(function() {
				$(this).parents('.dialog-box-succes').effect("highlight", {}, 400).animate({
					opacity: 0    
				},function () {
					$(this).remove();
				});
		return false;
	  });
	/* error */
	$(".del-x").click(function() {
				$(this).parents('.dialog-box-error').effect("highlight", {}, 400).animate({
					opacity: 0    
				},function () {
					$(this).remove();
				});
		return false;
	  });
	/* warning */
	$(".del-x").click(function() {
				$(this).parents('.dialog-box-warning').effect("highlight", {}, 400).animate({
					opacity: 0    
				},function () {
					$(this).remove();
				});
		return false;
	  });
	/* information */
	$(".del-x").click(function() {
				$(this).parents('.dialog-box-information').effect("highlight", {}, 400).animate({
					opacity: 0    
				},function () {
					$(this).remove();
				});
		return false;
	  });

    /* toggle selected box */
	$(".toggle").click(function(){
	   var id = $(this).attr('id');
		  $("#tog"+ id).slideToggle("slow");
		  
			 if ($('#'+ id + ' img.close').is(":hidden")){
				 $('#'+ id +' img.close').show();
				 $('#'+ id +' img.open').hide();
			  } else {
				 $('#'+ id + ' img.open').show();
				 $('#'+ id + ' img.close').hide();
			  }
	});

    /* This will give every even tr(row) a backgroundcolor */
    /* When hover a row it will light up */
	$("#tb-1 tr, #tb-2 tr").mouseover(function() {
		$(this).addClass("row-over");}).mouseout(function() {
			$(this).removeClass("row-over");
			});
	
           $("#tb-1 tr:even, #tb-2 tr:even").addClass("row"); 
		   
		   
    /* Code for the sortables more info @ http://jqueryui.com/demos/sortable/ */	
	$('#sortable').sortable({ 
				handle:      '.box-780-head',
				//placeholder: 'sortHelper', (css not working correct)
				delay:        250,
				cursor:      'move',
				scroll:       true,
				revert:       true, 
				opacity:      0.7
	});

    /* Code for the sortables more info @ http://jqueryui.com/demos/sortable/ */	
	$('#sortable2').sortable({ 
				handle:      '.box-1000-head',
				//placeholder: 'sortHelper', (css not working correct)
				delay:        250,
				cursor:      'move',
				scroll:       true,
				revert:       true, 
				opacity:      0.7
	});
	
	/* more info about the jQuerydatepicker @ http://jqueryui.com/demos/datepicker/ */
	$("#datepicker").datepicker({
		nextText: '',
		prevText: ''
	});	
	
	// searchfield value more info @ http://mucur.name/system/jquery_example/
	$('#s').example('Search here');
	
	/* lightbox more info @ http://colorpowered.com/colorbox/ */					   
	$("a.lightbox, a.lightbox2").colorbox({transition:"none"});
	
	/* SmartMarkUp Universal Markup Editor more info @ http://www.phpcow.com/smartmarkup/ */
	$('#html').sMarkUp('html', 300);
	
    /* remove/switch to simple mode SmartMarkUp Universal Markup Editor */
	var remove = document.getElementById('remove');
	remove.onclick = function() {
		if (this.rel == 'on') {
			this.innerHTML = 'Switch to advance mode';
			this.rel = 'off';
			$.sMarkUpRemove('#html');
		} else {
			this.innerHTML = 'Switch to simple mode';
			this.rel = 'on';
			$('#html').sMarkUp('html', 300);
		}
		return false;
	};
    
	/* pngfix, supersleight the jQuery version more info @ http://allinthehead.com/retro/338/supersleight-jquery-plugin */
	$('#userid, .dialog-left, #tb-1, #tb-2').supersleight();
    
	// tabs
	$("#tabs").tabs();

    // dialog
	$("#dialog").dialog({
		bgiframe: true,
		height: 140,
		width: 500,
		modal: true,
		buttons: {
			      Ok: function() {
					  $(this).dialog('close');
				      }
			     }
		});

});





	





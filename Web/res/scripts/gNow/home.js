// JavaScript Document
$(document).ready(function(){
	$('#main').ready(function(){
		$('#loading-wrap').css('opacity',0);
		setTimeout(function(){
			$('#loading-wrap').addClass('hidden');			
		}
		,1000);
		
		$('#supernova').css('opacity',1);
		setTimeout(function(){$('#supernova').css('opacity',0);},1500);		
		setTimeout(function(){
			$('#home').css('opacity',1);
			$('#logo').css('top','93%');
		},1500);
		setTimeout(function(){
			$('#supernova').addClass('hidden');
			$('#help1').css('top','5%');
		},3000);		
	});
	
	$('#search').click(function(){			
		$('#txtSearch').css('top',$('#search').offset().top); //Align the img and the textbox
		$('#txtSearch').toggleClass('searchHidden');	
		$('#txtSearch').focus();
	});
});
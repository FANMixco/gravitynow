// JavaScript Document
$(document).ready(function(){
	$('#gravityNow').ready(function(){
		$('#gravityNow').css('opacity',1);
		setTimeout(function(){
			$('#logo').css('opacity',1);
			$('#bottomMnu').css('opacity',1);
			//$.get( "http://localhost/gNow/desc", function( data ) {
			$.get( "http://gnow.azurewebsites.net/desc", function( data ) {															
				$("#descContainer").css('height','90%');
				$("#descContainer").css('top','5%');
				setTimeout(function(){$("#descContainer").html( data );},1000);
			  //
			  //$("#descContainer" ).html( data );
			  //alert( "Load was performed." );
			});
		},1000);			
	});
});
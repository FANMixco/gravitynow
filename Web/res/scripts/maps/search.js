var matches;
$(function () {
    $("#txtSearch").keypress(function (e) {
        if ($("#txtSearch").val() == "")
            return;		
		
        	$('#txtSearch').autocomplete({
				source: function( request, response ) {
	                $.ajax({
	                    url: "http://nominatim.openstreetmap.org/search?q=" + $("#txtSearch").val() + "&format=json",
	                    dataType: "json",	                    
	                    success: function(data) {
	                    		matches = data;	                    		
	                                response($.map(data, function(item, index) {
	                                return {
	                                    label: item.display_name,
	                                    val: {lat:item.lat, lon:item.lon}
	                                    //abbrev: item.abbrev
	                                    };
	                            }));
	                        }
	                    });
	                },
	            minLength: 2,
				position: { my: "left bottom", at: "left top", collision: "flip" },
				select: function (event, ui) { console.log('selected'+ui.item.val); setLocation(ui.item.val);  } 
			});
    });

    function setLocation(data) {
    	console.log('lat: '+data.lat+", lon: "+data.lon);
        $.ajax({
            url: "http://api.geonames.org/srtm3JSON?lat=" + data.lat + "&lng=" + data.lon + "&username=fanmixco",
            async: false,
            dataType: 'json',
            success: function (deserialized) {                

                var g = new gravity();

                var latitude = deserialized.lat;

                var longitude = deserialized.lng;

                var altitude = deserialized.srtm3;

                g.setLocation(latitude, longitude, altitude);

                var coord = new nokia.maps.geo.Coordinate(latitude, longitude),
                standardMarker = new nokia.maps.map.StandardMarker(coord, {text: "S"});
                display.objects.add(standardMarker);
                var gResult = g.getGravity();                

				bubbleMarker2 = new InfoBubbleMarker(
					coord,
					infoBubbles,
					"gravity: "+gResult.toFixed(6)+"m/s&sup2;",
					{ 
						eventDelegationContainer: markersContainer,
                        text: "S" ,
						brush: { color: '#E5D83F' }						
					}
				);
						
				// Add bubbleMarker to the markers container so it will be rendered onto the map
//				markersContainer.objects.add(bubbleMarker);
			
                display.set('zoomLevel', 16);
            	display.set('center', coord);

            }
        });

    }
});
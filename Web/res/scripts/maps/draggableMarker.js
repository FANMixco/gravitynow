var imgLocNew;

nokia.Settings.set("app_id", "xfWoSkntk0Ao3r8C6wZ6");
nokia.Settings.set("app_code", "snDmPQuP-71myYTab77aIA");

nokia.Settings.set("serviceMode", "cit");
		
var infoBubbles = new nokia.maps.map.component.InfoBubbles(),
	markersContainer = new nokia.maps.map.Container();
var display = new nokia.maps.map.Display(document.getElementById("mapContainer"),
					 {
						 "components": [
								   infoBubbles,
								   new nokia.maps.map.component.ZoomBar(),
								   new nokia.maps.map.component.Behavior(),
								   new nokia.maps.map.component.TypeSelector()],
						 "zoomLevel": 4,
						 "center": [13.7945185, -88.896530]
					 });

display.objects.add(markersContainer);

var TOUCH = nokia.maps.dom.Page.browser.touch,
	CLICK = TOUCH ? "tap" : "click";

function extend(B, A) {
	function I() {};
	I.prototype = A.prototype;
	B.prototype = new I;
	B.prototype.constructor = B;
	B.superprototype = A.prototype;
};

var draggableMarker = new nokia.maps.map.StandardMarker([13.7945185, -88.896530],
{
	text: "X",
	brush: { color: '#FF0000' },
	draggable: true,
});

draggableMarker.addListener("drag", function (e) {
	var coordinate = display.pixelToGeo(e.displayX, e.displayY);

	getGravity(coordinate.latitude.toFixed(6),coordinate.longitude.toFixed(6));
}, false);
display.objects.add(draggableMarker);


function getGravity(latitude, longitude, altitude)
{
	//Fxs
	$('#help1').css('top','-100%');
	$('#logo-bg').removeClass('hidden');
	$('#logo').css('opacity',0);
	setTimeout(function(){
		$('#logo').addClass('hidden');			
	}
	,1000);
	$.getJSON( "http://api.geonames.org/srtm3JSON?lat="+latitude+"&lng="+longitude+"&username=fanmixco", function( data ) {

		var latitude = data.lat;

		var longitude = data.lng;

		var altitude = data.srtm3;

		var g = calculateGravity(latitude, altitude);
		
		//$("#Value").text("longitude: "+longitude+" latitude: "+latitude+" altitude: "+altitude+" gravity: "+g);
		$("#result").html(Number(g.toString().match(/^\d+(?:\.\d{0,2})?/))+' m/s&sup2;');
	});
}


var InfoBubbleMarker = function (coord, infoBubbles, bubbleText, props) {
	var container;
	
	if (!((infoBubbles instanceof nokia.maps.map.component.InfoBubbles) &&
		(coord instanceof nokia.maps.geo.Coordinate) && bubbleText)) {
		throw "Invalid arguments given to InfoBubbleMarker constructor";
	}
	
	if (props && props.eventDelegationContainer) {
		container = props.eventDelegationContainer;
		delete props.eventDelegationContainer;
	}
	
	// Call the "super" constructor to initialize properties inherited from StandardMarker
	nokia.maps.map.StandardMarker.call(this, coord, props);
	
		 infoBubbles.openBubble(bubbleText,
	coord);
	
	this.init(infoBubbles, bubbleText, container);
};

// InfoBubble inherits functionality from StandardMarker
extend(InfoBubbleMarker, nokia.maps.map.StandardMarker);

// InfoBubbleMarker constructor function 
InfoBubbleMarker.prototype.init = function (infoBubbles, bubbleText, container) {
	var that = this,
		container,
		// An event callback for the click event on InfoBubbleMarker
		clickHandler = function (evt) {
			/* Using information stored in the event we find out which marker was clicked
			 * and then we trigger that marker's showBubble function
			 */
			var marker = evt.target;
			
			marker.showBubble();
				
			// Prevent event propagation. This will prevent from additional markers to be created on the same point
			evt.stopImmediatePropagation();
		},
		// Helper function to remove infoBubble on drag of marker
		dragStartHandler =function (evt) { 
				var marker = evt.target;
				
				if (marker.infoBubble)
					marker.closeBubble(); 
			}
		// Helper function switch mouse cursor icon 
		mouseCursorHandler = function (evt, cursorType) {
			var marker = evt.target;
		
			if (marker instanceof InfoBubbleMarker) {
				document.body.style.cursor = cursorType;
			}
		};
		
	that.infoBubbles = infoBubbles;
	that.bubbleText = bubbleText;
	
	if (container) {
		/* We are using event delegation: one eventlistener on the map object container
		 * instead of event listener per marker
		 */
		
		if (!container.$IS_INFOBUBBLE_MARKER_CONTAINER) {
			container.addListener(CLICK, clickHandler);
			container.addListener("mouseover", function (evt) { mouseCursorHandler(evt, "pointer"); });
			container.addListener("mouseout", function (evt) { mouseCursorHandler(evt, "default"); });
			container.$IS_INFOBUBBLE_MARKER_CONTAINER = true;
		}
	} else {
		/* Add all of the needed event listeners in one go using 
		 * EventTarget.addListeners
		 */
		that.addEventListeners({
			"tap": clickHandler,
			"click": clickHandler,
			"mouseover": function (evt) { mouseCursorHandler(evt, "pointer"); },
			"mouseout": function (evt) { mouseCursorHandler(evt, "default"); }
		});
	}
	
	// We change the options of infoBubbles so we can have multiple open at the same time
	infoBubbles.options.set("autoClose", false);
};

// Helper property to identify instances of InfoBubbleMarker
InfoBubbleMarker.prototype._type = "infoBubbleMarker";

// Overload standard destroy() of StandardMarker so we hide infoBubble on destruction of marker
InfoBubbleMarker.prototype.destroy = function () {
	this.closeBubble();
	InfoBubbleMarker.superprototype.destroy.call(this);
};

// To change text of information bubble
InfoBubbleMarker.prototype.setBubbleText = function (bubbleText) {
	if (this.infoBubble)
		this.infoBubble.update(bubbleText);
};

// Add to marker functions to hide / show the information bubble
InfoBubbleMarker.prototype.showBubble = function () { 
	if (this.infoBubble)
		this.closeBubble();
	
	this.infoBubble = this.infoBubbles.openBubble(this.bubbleText, this.coordinate);
};

InfoBubbleMarker.prototype.closeBubble = function () {
	if (this.infoBubble) {
		this.infoBubble.close();
		this.infoBubble = null;
	}
};

/* Attach an event listener to map display
 * push info bubble with coordinate information to map
 */
display.addListener(CLICK, function (evt) {
	var coord = display.pixelToGeo(evt.displayX, evt.displayY);
	getCGravity(coord.latitude, coord.longitude,evt);
	
});

function getCGravity(latitude, longitude,evt)
{
	$.ajax({
	  url: "http://api.geonames.org/srtm3JSON?lat="+latitude+"&lng="+longitude+"&username=fanmixco",
	  async: false,
	  dataType: 'json',
	  success: function (data) {
		var latitude = data.lat;

		var longitude = data.lng;

		var altitude = data.srtm3;

		var g = calculateGravity(latitude, altitude);
		
		var coord = display.pixelToGeo(evt.displayX, evt.displayY),
			bubbleMarker = new InfoBubbleMarker(
					coord,
					infoBubbles,
					"gravity: "+g.toFixed(6)+"m/s&sup2;",
					{ 
						eventDelegationContainer: markersContainer,
						brush: { color: "#0FDF43" },
						text: (markersContainer.objects.getLength() + 1)
					}
				);
				
				// Add bubbleMarker to the markers container so it will be rendered onto the map
				markersContainer.objects.add(bubbleMarker);
	
		}
	});
  }

  	
	function calculateGravity(lat, alt)
	{
		var IGF = 9.780327 * (1 + 0.0053024 * Math.pow(Math.sin(lat), 2) - 0.0000058 * Math.pow(Math.sin(2 * lat), 2));

		var FAC = -3.086 * Math.pow(10, -6) * alt;

		var g = IGF + FAC;

		return g;
	}
	
	$("#myLocation").click(function () {
		if ($(this).attr("src") == imgLocNew)
			return;
	
        if (navigator.geolocation)
            navigator.geolocation.getCurrentPosition(showPosition,showError);
        else
            alert("Geolocation is not supported by this browser.");
    });

    function showPosition(position) {
        $.ajax({
	      url: "http://api.geonames.org/srtm3JSON?lat="+position.coords.latitude+"&lng="+position.coords.longitude+"&username=fanmixco",
	      async: false,
	      dataType: 'json',
	      success: function (data) {
		  
				console.log(data);
		        var latitude = data.lat;

		        var longitude = data.lng;

		        var altitude = data.srtm3;

                var g = calculateGravity(latitude, altitude);

                var coord = new nokia.maps.geo.Coordinate(latitude, longitude),
                standardMarker = new nokia.maps.map.StandardMarker(coord, {text: "L"});
                display.objects.add(standardMarker);

                var g = calculateGravity(latitude, altitude);

				bubbleMarker2 = new InfoBubbleMarker(
					coord,
					infoBubbles,
					"gravity: "+g.toFixed(6)+"m/s&sup2;",
					{ 
						eventDelegationContainer: markersContainer,
                        text: "L" ,
						brush: { color: '#1080dd' }						
					}
				);
						
				// Add bubbleMarker to the markers container so it will be rendered onto the map
//				markersContainer.objects.add(bubbleMarker);
			
                display.set('zoomLevel', 16);
            	display.set('center', coord);
				
				$("#myLocation").attr("src", imgLocNew);
          }
        });
    }
	
	function showError(error)
	{
		switch(error.code) 
		{
			case error.PERMISSION_DENIED:
			  alert("User denied the request for Geolocation.");
			  break;
			case error.POSITION_UNAVAILABLE:
			  alert("Location information is unavailable..");
			  break;
			case error.TIMEOUT:
			  alert("The request to get user location timed out.");
			  break;
			case error.UNKNOWN_ERROR:
			  alert("An unknown error occurred.");
			  break;
		}
	}

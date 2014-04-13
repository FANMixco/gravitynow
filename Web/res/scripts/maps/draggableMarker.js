nokia.Settings.set("app_id", "xfWoSkntk0Ao3r8C6wZ6");
nokia.Settings.set("app_code", "snDmPQuP-71myYTab77aIA");

nokia.Settings.set("serviceMode", "cit");
var display = new nokia.maps.map.Display(document.getElementById("mapContainer"),
					 {
						 "components": [
								   new nokia.maps.map.component.ZoomBar(),
								   new nokia.maps.map.component.Behavior(),
								   new nokia.maps.map.component.TypeSelector()],
						 "zoomLevel": 4,
						 "center": [13.7945185, -88.896530]
					 });

//var draggableMarker = new nokia.maps.map.StandardMarker([52.500556, 13.398889],
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
	$('#help1').css('top','-25%');
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

		var IGF = 9.780327 * (1 + 0.0053024 * Math.pow(Math.sin(latitude), 2) - 0.0000058 * Math.pow(Math.sin(2 * latitude), 2));

		var FAC = -3.086 * Math.pow(10, -6) * altitude;

		var g = IGF + FAC;		
		
		//$("#Value").text("longitude: "+longitude+" latitude: "+latitude+" altitude: "+altitude+" gravity: "+g);
		$("#result").html(Number(g.toString().match(/^\d+(?:\.\d{0,2})?/))+' m/s&sup2;');
		console.log(data);
	});
}

$(function () {
    //Section Map
    nokia.Settings.set("app_id", "xfWoSkntk0Ao3r8C6wZ6");
    nokia.Settings.set("app_code", "snDmPQuP-71myYTab77aIA");

    // Use staging environment (remove the line for production environment)
    nokia.Settings.set("serviceMode", "cit");

    // Get the DOM node to which we will append the map
    var mapContainer = document.getElementById("mapContainer");
    // Create a map inside the map container DOM node

    var map = new nokia.maps.map.Display(mapContainer, {
        // Initial center and zoom level of the map
        center: [13.7945185, -88.896530],
        zoomLevel: 2,
        // We add the behavior component to allow panning / zooming of the map
        components: [new nokia.maps.map.component.Behavior(), new nokia.maps.map.component.ZoomBar(), new nokia.maps.map.component.TypeSelector(), new nokia.maps.map.component.ScaleBar()]
    });

    map.addListener("click", function (evt) {
        if ((evt.target.$href === undefined) == false) {
            window.location = evt.target.$href;
        } else if ((evt.target.$click === undefined) == false) {
            var onClickDo = new Function(evt.target.$click);
            onClickDo();
        }
    });

    //Section Map


/*    //Detect event enter
    $('#uID').keypress(function (e) {
        var code = null;

        code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13)
            createMarkers($('#uID').val());

    });*/

    createMarkers();

    /*$.getScript(getLanguage(), function () {

        //$("#uID").attr("PlaceHolder", data.txtUserId);
        //Create Container
        var noteContainer = new NoteContainer({
            id: "infoBubbleStandardMarkerUi",
            parent: document.getElementById("uiContainer"),
            title: data.titleImportant,
            content: data.lblContent
        });
    });*/

    var locations;
    function createMarkers(value) {
        $.ajax({
            url: "http://gnow.hostingsiteforfree.com/webServices/getLocation.php",
            type: "GET",
            dataType: "json",
            success: function (source) {
                locations = source;
                showInfo();
            },
            error: function (dato) {
                //alert("ERROR");
            }
        });
    }

    function showInfo() {
		map.objects.clear();

        var red = { color: "#FF0000" };

        var lat,
            lng,
            markerCoords;

        container = new nokia.maps.map.Container();

        var markers = [];

        for (var idx = 0; idx < locations.length; idx++) {
            var standardMarker;

            loc = locations[idx];

            lat = loc.latitude;
            lng = loc.longitude;

            markerCoords = new nokia.maps.geo.Coordinate(
                                      parseFloat(lat, 10),
                                      parseFloat(lng, 10)
                                  );

			var messageTime = '$.Zebra_Dialog("'+ data.lblGravity + loc.gravity + data.lblUser + loc.fullname + '<br />' + data.lblDateTime + ' <br />' + loc.registrationDate + '");';


            standardMarker = new nokia.maps.map.StandardMarker(markerCoords, { $click: messageTime });
            standardMarker.set("brush", red);

            markers.push(standardMarker);
       /* $.getScript(getLanguage(), function () {
            map.objects.clear();

        var red = { color: "#FF0000" };

        var lat,
            lng,
            markerCoords;

        container = new nokia.maps.map.Container();

        var markers = [];

        for (var idx = 0; idx < locations.length; idx++) {
            var standardMarker;

            loc = locations[idx];

            lat = loc.latitude;
            lng = loc.longitude;

            markerCoords = new nokia.maps.geo.Coordinate(
                                      parseFloat(lat, 10),
                                      parseFloat(lng, 10)
                                  );

			var messageTime = '$.Zebra_Dialog("'+ data.lblGravity + loc.gravity + data.lblUser + loc.fullname + '<br />' + data.lblDateTime + ' <br />' + loc.registrationDate + '");';


            standardMarker = new nokia.maps.map.StandardMarker(markerCoords, { $click: messageTime });
            standardMarker.set("brush", red);

            markers.push(standardMarker);

        }*/


        var markerCluster = new MarkerClusterer();

        markerCluster.objects.addAll(markers);

        map.addComponent(markerCluster);
        //});
    }

}

			});
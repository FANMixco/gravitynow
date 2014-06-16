$(function () {
    $("#txtSearch").keypress(function (e) {
        if ($("#txtSearch").val() == "")
            return;

        if (e.which == 13) {
            $("#result").empty();
            $.ajax({
                url: "http://nominatim.openstreetmap.org/search?q=" + $("#txtSearch").val() + "&format=json",
                async: false,
                dataType: 'json',
                success: function (deserialized) {
                    for (var i = 0; i < deserialized.length; i++)
                        setDataLocation(deserialized[i]);

                }
            });
        }
    });

    function setDataLocation(data) {
        $.ajax({
            url: "http://api.geonames.org/srtm3JSON?lat=" + data.lat + "&lng=" + data.lon + "&username=fanmixco",
            async: false,
            dataType: 'json',
            success: function (deserialized) {
                var completeString = "";

                var g = new gravity();

                var latitude = deserialized.lat;

                var longitude = deserialized.lng;

                var altitude = deserialized.srtm3;

                g.setLocation(latitude, longitude, altitude);

                var gResult = g.getGravity();

                completeString += "<b>Address: </b> <a href='http://bing.com/maps/?cp=" + latitude + "~" + longitude + "&lvl=16&sp=point." + latitude + "_" + longitude + "_' target='_blank'>"
+data.display_name + "</a><br />";
                //                completeString += "<b>Address: </b>" + data.display_name + "<br />";
                completeString += "<b>Latitude: </b>" + latitude.toFixed(6) + "&deg;<br />";
                completeString += "<b>Longitude: </b>" + longitude.toFixed(6) + "&deg;<br />";
                completeString += "<b>Gravity: </b>" + gResult.toFixed(6) + " m/s&#178;<br /><br />";

                $("#result").append(completeString);
            }
        });

    }
});
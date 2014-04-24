$(function () {
    jQuery("#lblResult").fitText();

    getLocation();

    function getLocation() {
        if (navigator.geolocation)
            navigator.geolocation.getCurrentPosition(showPosition, showError);
        else
            $("#lblResult").html("Geolocation is not supported by this browser.");
    }

    function showPosition(position) {

        $.ajax({
            url: "http://api.geonames.org/srtm3JSON?lat=" + position.coords.latitude + "&lng=" + position.coords.longitude + "&username=fanmixco",
            async: false,
            dataType: 'json',
            success: function (data) {

                var g = new gravity();

                var latitude = data.lat;

                var longitude = data.lng;

                var altitude = data.srtm3;

                g.setLocation(latitude, longitude, altitude);

                var gResult = g.getGravity();

                $("#lblResult").html(gResult.toFixed(3) + " m/s&#178;");

                $("#gravityResult").val(gResult);

                $("#lblLatitude").html("latitude: <br />" + latitude.toFixed(6) + "&deg;");

                $("#lblLongitude").html("longitude: <br />" + longitude.toFixed(6) + "&deg;");

                $("#lblAltitude").html("altitude: <br />" + longitude.toFixed(0) + " m");

                changeLocationTable();
            }
        });

    }

    function changeLocationTable() {
        var p = $(".ui-footer");
        var position = p.position();

        $("#location").css("top", position.top - $("#tblLocation").height());
    }

    $(window).resize(function () {
        changeLocationTable();
    });

    function showError(error) {
        switch (error.code) {
            case error.PERMISSION_DENIED:
                $("#lblResult").html("User denied the request for Geolocation.");
                break;
            case error.POSITION_UNAVAILABLE:
                $("#lblResult").html("Location information is unavailable.");
                break;
            case error.TIMEOUT:
                $("#lblResult").html("The request to get user location timed out.");
                break;
            case error.UNKNOWN_ERROR:
                $("#lblResult").html("An unknown error occurred.");
                break;
        }
    }
});
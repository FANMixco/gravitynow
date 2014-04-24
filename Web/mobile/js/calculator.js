$(function () {
    jQuery("#lblResult").fitText();

    $("#txtAltitude,#txtLatitude").focusout(function () {
        getResult();
    });

    $("#cmbUnits").change(function () {
        getResult();
    });

    function getResult() {
        var lat = $("#txtLatitude").val();
        var alt = $("#txtAltitude").val();
        var units = $("#cmbUnits").val();

        if (lat != "" && alt != "" && units != "") {
            var g = new gravity();

            if (units == "ft")
                alt = g.changeToMetres(alt);

            g.setLocation(lat, 0, alt);

            var gResult = g.getGravity();

            switch (gResult) {
                case 0:
                    $("#lblResult").html("Latitude not valid");
                    break;
                case 1:
                    $("#lblResult").html("Height above Everest?");
                    break;
                case 2:
                    $("#lblResult").html("Height below Dead Sea?");
                    break;
                default:
                    if (units=="m")
                        $("#lblResult").html(gResult.toFixed(3) + " m/s&#178;");
                    else
                        $("#lblResult").html(g.changeToFeet(gResult).toFixed(3) + " ft/s&#178;");
                    break;
            }
        }
    }
});
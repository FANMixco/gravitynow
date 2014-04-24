$(function () {

    var data = new gravityPlaces();

    function hide(id, visible) {
        $("#imgPlanet" + id).css("display", visible);
        $("#lblDesc" + id).css("display", visible);
        $("#lblGravity" + id).css("display", visible);

        if (id == 1) {
            $("#lblPercentaje").css("display", visible);
            $("#lblResult").css("display", visible);
            $("#lblPercentaje").val("");
            $("#lblResult").css("");

        }
    }

    $("#cmbPlanet1,#cmbPlanet2").change(function () {
        var current = $(this).attr('id');

        if ($(this).val() == "") {
            hide($(this).attr('id').replace("cmbPlanet", ""), "none");
            return;
        }

        hide($(this).attr('id').replace("cmbPlanet", ""), "block");

        if (current == "cmbPlanet2")
            $("#imgPlanet2").css("display", "block");

        current = current.replace("cmb", "img");

        $("#" + current).attr("src", "../img/Planets/" + $(this).val().toLowerCase() + ".jpg");

        current = current.replace("imgPlanet", "");

        $("#lblDesc" + current).html(data.getDescription($(this)[0].selectedIndex - 1));

        $("#lblGravity" + current).html(data.getGravity($(this)[0].selectedIndex - 1) + " m/s&#178;");

        if ($("#cmbPlanet1").val() != "" && $("#cmbPlanet1").val() != "") {

            $("#lblPercentaje").html(data.percentageGravity($("#cmbPlanet1")[0].selectedIndex - 1, $("#cmbPlanet2")[0].selectedIndex - 1).toFixed(3) + '%');

            var result = data.comparedGravity($("#cmbPlanet2")[0].selectedIndex - 1, $("#cmbPlanet1")[0].selectedIndex - 1);

            var color;

            $("#lblPercentaje").css("display", "block");
            switch (result) {
                case 0:
                    color = "#FE2E2E";
                    $("#lblResult").html("This gravity is smaller in:");
                    break;
                case 1:
                    color = "#01DF01";
                    $("#lblResult").html("This gravity is bigger in:");
                    break;
                case 2:
                    color = "#013ADF";
                    $("#lblResult").html("Gravity is the same.");
                    $("#lblPercentaje").css("display", "none");
                    break;
            }

            $("#lblResult,#lblPercentaje").css("color", color);
            $("#lblResult,#lblPercentaje").css("font-weight", "bold");
        }
    });
});
var gravity = function () {
    var everest = 6168;
    var deadSea = -427;
    var feet = 3.2808399;

    var longitude = 0;
    var latitude = 0;
    var altitude = 0;

    this.getGravity = function () {
        if (latitude < -90 || latitude > 90)
            return 0;

        if (altitude > everest)
            return 1;

        if (altitude < deadSea)
            return 2;

        var IGF = 9.780327 * (1 + 0.0053024 * Math.pow(Math.sin(latitude), 2) - 0.0000058 * Math.pow(Math.sin(2 * latitude), 2));

        var FAC = -3.086 * Math.pow(10, -6) * altitude;

        var g = IGF + FAC;

        return g;
    }

    this.setLocation = function (lat, lng, alt) {
        latitude = lat;
        longitude = lng;
        altitude = alt;
    }


    this.changeToMetres = function (value) {
        return value / feet;
    }

    this.changeToFeet = function (value) {
        return value * feet;
    }

}
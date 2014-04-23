var gravityPlaces = function () {

    var place;

    $.getJSON("../js/places.js", function (json) {
        place = json;
        console.log(place);
    });

    this.comparedGravity = function (id1, id2) {
        var gravity1 = place[id1].gravity;
        var gravity2 = place[id2].gravity;

        if (gravity1 > gravity2)
            return 0;
        else if (gravity1 < gravity2)
            return 1;
        else
            return 2;
    }

    this.percentageGravity = function (id1, id2) {
        result = this.comparedGravity(id1, id2);

        gravity1 = place[id1].gravity;
        gravity2 = place[id2].gravity;

        if (result == 0)
            return (gravity1 * 100) / gravity2;
        else if (result == 1)
            return (gravity2 * 100) / gravity1;
        else
            return 1;
    }

    this.getName = function (id) {
        return place[id].name;
    }

    this.getDescription = function (id) {
        return place[id].description;
    }

    this.getGravity = function (id) {
        return place[id].gravity;
    }
}

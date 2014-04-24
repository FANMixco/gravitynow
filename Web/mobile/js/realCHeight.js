$(function () {

    function detectHeight() {
        var content = $.mobile.getScreenHeight() - $(".ui-header").outerHeight() - $(".ui-footer").outerHeight() - $(".ui-content").outerHeight() + $(".ui-content").height();

        $(".ui-content").height(content);

    }

    detectHeight();

    $(window).resize(function () {
        detectHeight();
    });
});
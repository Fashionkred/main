function showPopup() {

    $('#fadeMask').fadeIn(1000);
    $("#overlay_form").fadeIn(1000);
    //positionPopup();
}
function showSigninPopup() {

    $('#fadeMask').fadeIn(1000);
    $("#overlay_signin").fadeIn(1000);
    //positionPopup();
}

$(".close").click(function () {
    $("#overlay_form").fadeOut(500);
    $("#overlay_signin").fadeOut(500);
    $('#fadeMask, .window').fadeOut(500);
});

function positionPopup() {
    if (!$("#overlay_form").is(':visible')) {
        return;
    }
    $("#overlay_form").css({
        left: -15 + ($(window).width() - $('#overlay_form').width()) / 2,
        top: ($(window).width() - $('#overlay_form').width()) / 7,
        position: 'absolute'
    });
}

//$(window).bind('resize', positionPopup);
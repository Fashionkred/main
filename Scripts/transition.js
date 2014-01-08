//Gallery for main page
var galW = 770;
var N = 1;
//Gallery for create oage
var galCW = 336;
var galWW = 175;

var M = 1;
var L = 1;
var H = 1;

function SetHPLoveButtons() {
    $('.love-button').click(function () {
        return love(this);
    });
    
}

function love(element) {

    var lookid = element.id.substring(element.id.indexOf("-") + 1);
    var userid = $('#MainContent_UserId').text();
    var votetype = "up";
    var pointinc = 1;
    //check the status of subscribe button
    if ($('#'+ element.id).attr('class') == 'love-button') {
        $('#' + element.id).attr("class", "love-button loved");
        $('#' + element.id + 'label').text("Loved");
        $('#' + element.id).hover(function () {
            $(this).attr('class', 'love-button unlove');
            $('#' + element.id + 'label').text("Unlove");
        }, function () {
            $(this).attr('class', 'love-button unlove');
            $('#' + element.id + 'label').text("Loved");
        });
    }
    else {
        votetype = "skip";
        pointinc = 0;
        $('#' + element.id).attr("class", "love-button");
        $('#' + element.id + 'label').text("Love");
        $('#' + element.id).unbind();
    }


    var url = "http://fashionKred.com/look.aspx?lid=" + lookid + "&ref=" + userid;
    var userShare = $('#MainContent_UserShare').text();
    if (userShare == "1" && votetype == "up") {
        FB.api(
            '/me/fashionkred:love',
            'post',
            {
                outfit: url
            },
            function (response) {
                // handle the response
            }
          );
    } 
    
    
    //make the ajax call
    $.ajax({
        url: hostname + "/vote.aspx?lid=" + lookid + "&uid=" + userid + "&votetype=" + votetype + "&lightweight=true" + "&point=" + pointinc + "&callback=?",
        cache: false,
        dataType: 'jsonp',
        jsonpCallback: 'UpdateLookBtn',
        contentType: "application/json",
        timeout: 10000
    });

}

function UpdateLookBtn(data) {

    SetHPLoveButtons();
}

function LoveClick() {
    DisableVoteButtons();
    $('.user-body-wrapper').fadeOut(800);
    $('.user-body').fadeOut(800);
    $('.right-button').fadeOut(800);
    $(".text-button-left").fadeOut(100);
    $(".text-button-left").delay(100).fadeIn(100);
    //send an OG update if votetype is love

    var userShare = $('#MainContent_UserShare').text();
    if (userShare == "1") {
        FB.api(
            '/me/fashionkred:love',
            'post',
            {
                outfit: GetUrl()
            },
            function (response) {
                // handle the response
            }
          );
    }
    window.setTimeout(function () {
        ShowResults('up');
    }, 100);

    PlaceVote('up');
}

function MehClick() {
    DisableVoteButtons();
    $('.user-body-wrapper').fadeOut(800);
    $('.user-body').fadeOut(800);
    $('.left-button').fadeOut(800);
    $(".text-button-right").fadeOut(100);
    $(".text-button-right").delay(100).fadeIn(100);
    window.setTimeout(function () {
        ShowResults('down');
    }, 100);

    PlaceVote('down');
}

function SkipClick() {
    DisableVoteButtons();
    $('.user-body-wrapper').fadeOut(800);
    $('.user-body').fadeOut(800);
    $('.left-button').fadeOut(800);
    $(".text-button-right").fadeOut(100);
    $(".text-button-right").delay(100).fadeIn(100);
    $('.right-button').fadeOut(800);
    $(".text-button-left").fadeOut(100);
    $(".text-button-left").delay(100).fadeIn(100);
    window.setTimeout(function () {
        ShowResults('skip');
    }, 100);

    PlaceVote('skip');
}

function PlaceVote(votetype) {
    var lookid = $('#MainContent_LookId').text();
    var userid = $('#MainContent_UserId').text();

    var pointinc = 1;
    if (votetype == "skip")
        pointinc = 0;
    
    //Get the new look
    $.ajax({
        url: hostname + "/vote.aspx?lid=" + lookid + "&uid=" + userid + "&votetype=" + votetype + "&point=" + pointinc + "&callback=?",
        cache: false,
        dataType: 'jsonp',
        jsonpCallback: 'UpdateLook',
        contentType: "application/json",
        timeout: 10000
    });

}

function UpdateLookData(data) {
    //Update metadata
    var obj = eval(data);

    if (obj.ErrorMessage != null) {
        ErrorVoteMessage(obj.ErrorMessage);
        return false;
    }
    else if (obj.RedirectUrl != null) {
        window.location.replace(obj.RedirectUrl);
    }
    else {

        //update the creator
        $('#MainContent_CreatorImage').attr("src", obj.Look.creator.pic);
        $('#MainContent_CreatorName').text("Made by: " + obj.Look.creator.name);
        $('#MainContent_CreatorName').attr("href", "user.aspx?uid=" + obj.Look.creator.id);
        $('#MainContent_CreatorId').text(obj.Look.creator.id);

        //set subscribe button
        if (obj.IsFollower == 1) {
            $('#MainContent_SubscribePanel').attr("class", "follow-button following");
        }
        else {
            $('#MainContent_SubscribePanel').attr("class", "follow-button");
            $('#MainContent_Subscribe').text("Follow");
            $('#MainContent_SubscribePanel').unbind();
        }

        //update the look
        $('#MainContent_LookId').text(obj.Look.id);
        $('#MainContent_UpVote').text(obj.Look.upVote);
        $('#MainContent_DownVote').text(obj.Look.downVote);
        $('#MainContent_P1Id').text(obj.Look.products[0].id);
        $('#MainContent_P2Id').text(obj.Look.products[1].id);

        //Change the id for previous love icons
        $('#MainContent_P1LoveButton').attr("id", "MainContent_P1LoveButton" + N);
        $('#MainContent_P1Love').attr("id", "MainContent_P1Love" + N);
        $('#MainContent_P1LoveImg').attr("id", "MainContent_P1LoveImg" + N);
        $('#MainContent_P2LoveButton').attr("id", "MainContent_P2LoveButton" + N);
        $('#MainContent_P2Love').attr("id", "MainContent_P2Love" + N);
        $('#MainContent_P2LoveImg').attr("id", "MainContent_P2LoveImg" + N);

        if (obj.Look.products[2] != null) {
            $('#MainContent_P3Id').text(obj.Look.products[2].id);
            $('#MainContent_P3LoveButton').attr("id", "MainContent_P3LoveButton" + N);
            $('#MainContent_P3Love').attr("id", "MainContent_P3Love" + N);
            $('#MainContent_P3LoveImg').attr("id", "MainContent_P3LoveImg" + N);
            //$('#gallery').attr("class", "gallery-3-items");
        }
        else {
            //$('#gallery').attr("class", "gallery");
        }

        $('#slider').append("<li>" + obj.ProductsHtml + "</li>");
        $('#slider').stop(1).delay(1000).animate({ left: -galW * N }, { duration: 1000, easing: 'easeOutSine' });
        N += 1;
        $('.user-body-wrapper').delay(1500).fadeIn(800);
        $('.user-body').delay(1500).fadeIn(800);

        if (obj.VoteType == "DownVote") { //Down vote
            $('.right-button').delay(1500).fadeIn(800);
            $('.left-button').delay(1500).fadeIn(800);
            window.setTimeout(function () {
                $('.text-button-right').animate({ fontSize: "24px" }, 500);
                $(".text-button-right").text("NO MATCH");
            }, 1300);
        }
        else if (obj.VoteType == "UpVote") {
            $('.left-button').delay(1500).fadeIn(800);
            $('.right-button').delay(1500).fadeIn(800);

            window.setTimeout(function () {
                $('.text-button-left').animate({ fontSize: "24px" }, 500);
                $(".text-button-left").text("MATCH");
            }, 1300);
        }
        else {
            $('.left-button').delay(1500).fadeIn(800);
            $('.right-button').delay(1500).fadeIn(800);
            window.setTimeout(function () {
                $(".text-button-left").text("MATCH");
                $(".text-button-right").text("NO MATCH");
            }, 1300);
        }

        //increase points of the user
        var points = parseInt($('#UserPoints').text());
        $('#UserPoints').text(points + 1 + " votes");

        //Update favorites carousel
        document.getElementById('MainContent_Favorites').innerHTML = obj.FavoritesHtml;
    }
}


function UpdateLook(data) {

    //play timer and update panel
    UpdateLookData(data);

    SetLoveButtons();
    SetVoteButtons();
    SetSubscribeButton();
    TrackOutBoundLinks();

    SetCommentBox();

    //if more than 20 in favs then show pop up
    if (N%5 == 0) {
        showPopup();
    }
}

function ErrorVoteMessage(data) {

    //show error message
    alert("Something went wrong! Please try again!");
    SetLoveButtons();
    SetVoteButtons();

    SetCommentBox();
}

function ShowResults(votetype) {
    
    // Update the vote panel
    var upvote = parseInt($('#MainContent_UpVote').text());
    var downvote = parseInt($('#MainContent_DownVote').text());
    
    if (votetype == "up") {
        upvote += 1;
        $('.text-button-left').animate({fontSize: "32px"}, 200);
        $('.text-button-left').text(Math.ceil(upvote * 100 / (upvote + downvote)) + "% VOTED MATCH");
        $('.text-button-disabled').text(Math.floor(downvote * 100 / (upvote + downvote)) + "% VOTED NO MATCH");
    }
    else if (votetype == "down"){
        downvote += 1;
        $('.text-button-right').animate({ fontSize: "32px" }, 200);
        $('.text-button-right').text(Math.floor(downvote * 100 / (upvote + downvote)) + "% VOTED NO MATCH");
        $('.text-button-disabled').text(Math.ceil(upvote * 100 / (upvote + downvote)) + "% VOTED MATCH");
    }
    else {
         var totalVote = upvote + downvote;
         if (totalVote > 0) {
             $('#MainContent_RightTextButtonDisabled').text(Math.floor(downvote * 100 / (upvote + downvote)) + "% VOTED NO MATCH");
             $('#MainContent_LeftTextButtonDisabled').text(Math.ceil(upvote * 100 / (upvote + downvote)) + "% VOTED MATCH");
         }
         else {
             $('#MainContent_RightTextButtonDisabled').text("0% VOTED NO MATCH");
             $('#MainContent_LeftTextButtonDisabled').text("0% VOTED MATCH");
         }
    }
}

//love hover 
$('.text-heart').find('#MainContent_P1LoveImg').hover(function () {
    $(this).attr('src', 'images/heart_filled.jpg');
}, function () {
    $(this).attr('src', 'images/heart_empty.jpg');
});
$('.text-heart').find('#MainContent_P2LoveImg').hover(function () {
    $(this).attr('src', 'images/heart_filled.jpg');
}, function () {
    $(this).attr('src', 'images/heart_empty.jpg');
});
$('.text-heart').find('#MainContent_P3LoveImg').hover(function () {
    $(this).attr('src', 'images/heart_filled.jpg');
}, function () {
    $(this).attr('src', 'images/heart_empty.jpg');
});

var countM = $("#sliderCreate li").length;
var countL = $("#sliderCreate2 li").length;
var countH = $("#sliderCreate3 li").length;

$('#buttons-create-next').on('click', function () {
    if (M < countM) {
        $('#buttons-create-prev').attr('class', 'buttons-create');
        $('#sliderCreate').stop(1).animate({ left: -galCW * M }, { duration: 600, easing: 'easeOutSine' });

        //Find the div which contains current image
        var divID = "#MainContent_sliderCreatediv" + M;
        var pColor = $(divID).find('img').attr("color");
        var pId = $(divID).find('img').attr("id");
        if (pId != null) {
            pId = pId.substring(pId.indexOf("-") + 1);
            $('#MainContent_P1Id').text(pId);
            if (pColor != null) {
                $('#MainContent_P1Color').text(pColor);
            }
        }
        else {
            $('#MainContent_P1Id').text("");
        }
        M += 1;
        if (M == countM) {
            $('#buttons-create-next').attr('class', 'buttons-create-disabled');
        }
    }
});

$('#buttons-create-prev').on('click', function () {
    if (M > 1) {
        $('#buttons-create-next').attr('class', 'buttons-create');
        M -= 2;
        $('#sliderCreate').stop(1).animate({ left: -galCW * M }, { duration: 600, easing: 'easeOutSine' });

        //Find the div which contains current image
        //Find the div which contains current image
        var divID = "#MainContent_sliderCreatediv" + M;
        var pColor = $(divID).find('img').attr("color");
        var pId = $(divID).find('img').attr("id");
        if (pId != null) {
            pId = pId.substring(pId.indexOf("-") + 1);
            $('#MainContent_P1Id').text(pId);
            if (pColor != null) {
                $('#MainContent_P1Color').text(pColor);
            }
        }
        else {
            $('#MainContent_P1Id').text("");
        }
        M += 1;
        if (M == 1) {
            $('#buttons-create-prev').attr('class', 'buttons-create-disabled');
        }
    }
});

$('#buttons-create-next-02').on('click', function () {
    if (L < countL) {
        $('#buttons-create-prev-02').attr('class', 'buttons-create');
        $('#sliderCreate2').stop(1).animate({ left: -galWW * L }, { duration: 600, easing: 'easeOutSine' });

        //Find the div which contains current image
        var divID = "#MainContent_sliderCreate2div" + L;
        var pColor = $(divID).find('img').attr("color");
        var pId = $(divID).find('img').attr("id");
        if (pId != null) {
            pId = pId.substring(pId.indexOf("-") + 1);
            $('#MainContent_P2Id').text(pId);
            if (pColor != null) {
                $('#MainContent_P2Color').text(pColor);
            }
        }
        else {
            $('#MainContent_P2Id').text("");
        }

        L += 1;
        if (L == countL) {
            $('#buttons-create-next-02').attr('class', 'buttons-create-disabled');
        }
    }
});

$('#buttons-create-prev-02').on('click', function () {
    if (L > 1) {
        $('#buttons-create-next-02').attr('class', 'buttons-create');
        L -= 2;
        $('#sliderCreate2').stop(1).animate({ left: -galWW * L }, { duration: 600, easing: 'easeOutSine' });

        //Find the div which contains current image
        var divID = "#MainContent_sliderCreate2div" + L;
        var pColor = $(divID).find('img').attr("color");
        var pId = $(divID).find('img').attr("id");
        if (pId != null) {
            pId = pId.substring(pId.indexOf("-") + 1);
            $('#MainContent_P2Id').text(pId);
            if (pColor != null) {
                $('#MainContent_P2Color').text(pColor);
            }
        }
        else {
            $('#MainContent_P2Id').text("");
        }

        L += 1;
        if (L == 1) {
            $('#buttons-create-prev-02').attr('class', 'buttons-create-disabled');
        }
    }
});

$('#buttons-create-next-03').on('click', function () {
    if (H < countH) {
        $('#buttons-create-prev-03').attr('class', 'buttons-create');
        $('#sliderCreate3').stop(1).animate({ left: -galWW * H }, { duration: 600, easing: 'easeOutSine' });

        //Find the div which contains current image
        var divID = "#MainContent_sliderCreate3div" + H;
        var pId = $(divID).find('img').attr("id");
        var pColor = $(divID).find('img').attr("color");
        if (pId != null) {
            pId = pId.substring(pId.indexOf("-") + 1);
            $('#MainContent_P3Id').text(pId);
            if (pColor != null) {
                $('#MainContent_P3Color').text(pColor);
            }
        }
        else {
            $('#MainContent_P3Id').text("");
        }

        H += 1;
        if (H == countH) {
            $('#buttons-create-next-03').attr('class', 'buttons-create-disabled');
        }
    }
});

$('#buttons-create-prev-03').on('click', function () {
    if (H > 1) {
        $('#buttons-create-next-03').attr('class', 'buttons-create');
        H -= 2;
        $('#sliderCreate3').stop(1).animate({ left: -galWW * H }, { duration: 600, easing: 'easeOutSine' });

        //Find the div which contains current image
        var divID = "#MainContent_sliderCreate3div" + H;
        var pId = $(divID).find('img').attr("id");
        var pColor = $(divID).find('img').attr("color");
        if (pId != null) {
            pId = pId.substring(pId.indexOf("-") + 1);
            $('#MainContent_P3Id').text(pId);
            if (pColor != null) {
                $('#MainContent_P3Color').text(pColor);
            }
        }
        else {
            $('#MainContent_P3Id').text("");
        }

        H += 1;
        if (H == 1) {
            $('#buttons-create-prev-03').attr('class', 'buttons-create-disabled');
        }
    }
});

function sliderselect(slider, position) {

    if (slider == 1) {

        M = position;

        $('#sliderCreate').stop(1).animate({ left: -galCW * M }, { duration: 600, easing: 'easeOutSine' });

        //Find the div which contains current image
        var divID = "#MainContent_sliderCreatediv" + M;
        var pId = $(divID).find('img').attr("id");
        var pColor = $(divID).find('img').attr("color");
        if (pId != null) {
            pId = pId.substring(pId.indexOf("-") + 1);
            $('#MainContent_P1Id').text(pId);
            if (pColor != null) {
                $('#MainContent_P1Color').text(pColor);
            }
        }
        M += 1;

        //set the prev button always selected 
        $('#buttons-create-prev').attr('class', 'buttons-create');
        if (M == countM) {
            $('#buttons-create-next').attr('class', 'buttons-create-disabled');
        }
    }
    else if (slider == 2) {

        L = position;

        $('#sliderCreate2').stop(1).animate({ left: -galWW * L }, { duration: 600, easing: 'easeOutSine' });

        //Find the div which contains current image
        var divID = "#MainContent_sliderCreate2div" + L;
        var pId = $(divID).find('img').attr("id");
        var pColor = $(divID).find('img').attr("color");
        if (pId != null) {
            pId = pId.substring(pId.indexOf("-") + 1);
            $('#MainContent_P2Id').text(pId);
            if (pColor != null) {
                $('#MainContent_P2Color').text(pColor);
            }
        }
        L += 1;

        $('#buttons-create-prev-02').attr('class', 'buttons-create');
        if (L == countL) {
            $('#buttons-create-next-02').attr('class', 'buttons-create-disabled');
        }
    }
    else if (slider == 3) {
        H = position;

        $('#sliderCreate3').stop(1).animate({ left: -galWW * H }, { duration: 600, easing: 'easeOutSine' });

        //Find the div which contains current image
        var divID = "#MainContent_sliderCreate3div" + H;
        var pId = $(divID).find('img').attr("id");
        var pColor = $(divID).find('img').attr("color");
        if (pId != null) {
            pId = pId.substring(pId.indexOf("-") + 1);
            $('#MainContent_P3Id').text(pId);
            if (pColor != null) {
                $('#MainContent_P3Color').text(pColor);
            }
        }
        H += 1;

        $('#buttons-create-prev-03').attr('class', 'buttons-create');
        if (H == countH) {
            $('#buttons-create-next-03').attr('class', 'buttons-create-disabled');
        }
    }
}

//carousel

var imagesLenght01 = $('div.fav-image').length;
var imagesLenght02 = $('div.fav-image_2').length;
var imagesLenght03 = $('div.fav-image_3').length;
var trigger01 = -50 - (imagesLenght01 - 7) * 120;
var trigger02 = -50 - (imagesLenght02 - 7) * 120;
var trigger03 = -50 - (imagesLenght03 - 7) * 120;

function loopL() {
    if ($('.carousel-favorites').position().left >= trigger01) {
        $('.carousel-favorites').stop().animate({ left: '-=20' }, 70, 'linear', loopL);
    }
}

function loopR() {
    if ($('.carousel-favorites').position().left <= -imagesLenght01) {
        $('.carousel-favorites').stop().animate({ left: '+=20' }, 70, 'linear', loopR);
    }
}

function stop() {
    $('.carousel-favorites').stop();
}

$(".next-carousel").hover(function () {
    loopL();
}, function () {
    stop();
});

$(".prev-carousel").hover(function () {
    loopR();
}, function () {
    stop();
});

function loopL_2() {
    if ($('.carousel-favorites_2').position().left >= trigger02) {
        $('.carousel-favorites_2').stop().animate({ left: '-=20' }, 70, 'linear', loopL_2);
    }
}

function loopR_2() {
    if ($('.carousel-favorites_2').position().left <= -imagesLenght02) {
        $('.carousel-favorites_2').stop().animate({ left: '+=20' }, 70, 'linear', loopR_2);
    }
}

function stop_2() {
    $('.carousel-favorites_2').stop();
}

$(".next-carousel_2").hover(function () {
    loopL_2();
}, function () {
    stop_2();
});

$(".prev-carousel_2").hover(function () {
    loopR_2();
}, function () {
    stop_2();
});

function loopL_3() {
    if ($('.carousel-favorites_3').position().left >= trigger03) {
        $('.carousel-favorites_3').stop().animate({ left: '-=20' }, 70, 'linear', loopL_3);
    }
}

function loopR_3() {
    if ($('.carousel-favorites_3').position().left <= -imagesLenght03) {
        $('.carousel-favorites_3').stop().animate({ left: '+=20' }, 70, 'linear', loopR_3);
    }
}

function stop_3() {
    $('.carousel-favorites_3').stop();
}

$(".next-carousel_3").hover(function () {
    loopL_3();
}, function () {
    stop_3();
});

$(".prev-carousel_3").hover(function () {
    loopR_3();
}, function () {
    stop_3();
});
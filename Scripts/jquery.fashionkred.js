var hostname = "https://fashionkred.com";

hostname = "http://localhost:17544/FashionKred";

Number.prototype.formatMoney = function (c, d, t) {
    var n = this, c = isNaN(c = Math.abs(c)) ? 2 : c, d = d == undefined ? "," : d, t = t == undefined ? "." : t, s = n < 0 ? "-" : "", i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};

function SetVoteButtons() {
    $('.left-button').click(function () { return LoveClick() });
    $('.right-button').click(function () { return MehClick() });
    $('.skip-button').click(function () { return SkipClick() });

    //share buttons
    var lookid = $('#MainContent_LookId').text();
    var imageurl = hostname + "/images/looks/" + lookid + ".jpg";
    $('#fb').attr("href", "https://www.facebook.com/sharer/sharer.php?u=" + encodeURIComponent(GetReferrerUrl()));
    $('#email').attr("href", "mailto:?subject=Check%20out%20this%20look%20at%20FashionKred&body=" + encodeURIComponent(GetReferrerUrl()));
    $('#pinterest a').attr("href", "//pinterest.com/pin/create/button/?url=" + encodeURIComponent(GetReferrerUrl()) + "&media=" + imageurl + "&description=Found this look at Fashionkred!");
    $('#tumblr a').attr("href", "http://www.tumblr.com/share/photo?source=" + encodeURIComponent(imageurl) + "&caption=Found%20this%20look%20at%20Fashionkred!" + "&clickthru=" + encodeURIComponent(GetReferrerUrl()));
 
}
function SetLoveButtons(){
    $('#MainContent_P1LoveButton').click(function () { return love('P1') });
    $('#MainContent_P2LoveButton').click(function () { return love('P2') });
    $('#MainContent_P3LoveButton').click(function () { return love('P3') });

}
function DisableVoteButtons() {
    $('.left-button').unbind();
    $('.right-button').unbind();
    $('.skip-button').unbind();
    DisableLoveButtons();
}
function SetSubscribeButton() {
    $('#MainContent_SubscribePanel').click(function () { return subscribe() });
    if($('#MainContent_SubscribePanel').attr("class") == "follow-button following")
    {
        $('#MainContent_Subscribe').text("Following");
        $('#MainContent_SubscribePanel').hover(function () {
            $(this).attr('class', 'follow-button unfollow');
            $('#MainContent_Subscribe').text("Unfollow");
        }, function () {
            $(this).attr('class', 'follow-button following');
            $('#MainContent_Subscribe').text("Following");
        });
    }
}
function DisableSubscribeButton() {
    $('#MainContent_SubscribePanel').unbind();
}

function DisableLoveButtons() {
    $('#MainContent_P1LoveButton').unbind();
    $('#MainContent_P2LoveButton').unbind();
    $('#MainContent_P3LoveButton').unbind();
}
function SetSiginPopup() {
    var unsigned = $('#MainContent_Unsigned').text();

    if(unsigned == "1")
        showSigninPopup();
}

function SetFinishButton(){
    $('#FinishButton').click(function () { return FinishButtonClick() });
    $('#submit-button-done').click(function() { return FinishSubmit() });
}

function DisableFinishButton(){
    $('#FinishButton').unbind();
}
function DisableSubmitButton() {
    $('#submit-button-done').unbind();
}
function FinishButtonClick(){  
    DisableFinishButton();
    
    var pid1 = $('#MainContent_P1Id').text();
    var pid2 = $('#MainContent_P2Id').text();
    //var pid3 = $('#MainContent_P3Id').text();
    
    //if either pid1 or pid2 is empty - flag a message
    if(pid1 == "" || pid2 == ""){
        CantFinishLook();
        SetFinishButton();
        return;
    }

    createlook();

    //show the share option
    $('#fadeMask').fadeIn(1000); 
    $("#create-share-popup").fadeIn(1000);
    positionPopup();
}
 
function FinishSubmit(){
    DisableSubmitButton();
    $("#create-share-popup").fadeOut(500);
    $('#fadeMask, .window').fadeOut(500);

    //Clear out the pids and lookid
    $('#MainContent_LookId').text("");
    $('#MainContent_P1Id').text("");
    $('#MainContent_P2Id').text("");
    $('#MainContent_P3Id').text("");

    FinishSuccess();

    //Set the buttons
    SetFinishButton();
    
}

function positionPopup(){
    if(!$("#create-share-popup").is(':visible')){
    return;
}

$("#create-share-popup").css({
left: -10 + ($(window).width() - $('#create-share-popup').width()) / 2,
top: ($(window).width() - $('#create-share-popup').width()) / 7,
position:'absolute'
});
}

$(window).bind('resize',positionPopup);

function TrackOutBoundLinks(){
    //Add outbound link js for OutBoundLink Css class
    $('.OutBoundLink').click(function () {
        trackOutboundLink(this, 'ShopStyle Links', this.href);
        return false;
    });
}

function SetPdtPageButtons() {
    $('#MainContent_LoveButton').click(function () { return pdtlove() });
}

function GetUrl() {
    var lookid = $('#MainContent_LookId').text();
    var url = "http://fashionKred.com/look.aspx?lid=" + lookid;
    return url;
}
function GetReferrerUrl() {
    var lookid = $('#MainContent_LookId').text();
    var userid = $('#MainContent_UserId').text();
    var url = "http://fashionKred.com/look.aspx?lid=" + lookid + "&ref=" + userid;
    return url;
}

function GetProductUrl(pid) {
    var url = "http://fashionKred.com/product.aspx?pid=" + pid;
    return url;
}
function GetUserUrl(uid) {
    var url = "http://fashionKred.com/user.aspx?uid=" + uid;
    return url;
}
function fbAppRequest(){
 FB.ui({method: 'apprequests',
    message: 'Get Outfit advice from friends'
  }, requestCallback);  
  //return false; 
}
 function requestCallback(response) {
        // Handle callback here
      }
function fbShare() {

   window.open('https://www.facebook.com/sharer/sharer.php?u=' + GetReferrerUrl(), '_blank', 'width=536,height=350,location=no,menubar=no,left=200,top=200,scrollbars=no,resizable=no');
//    FB.ui({
//          method: 'send',
//          name: 'FashionKred: Find your best outfits!',
//          link: GetUrl(),
//          });
    //return false;
}

function SetCommentBox(){
    
    //set the comment section
    if($('#fbCommentDiv iframe').length > 0) {
        //iframe exists - just change the url
    
        var lookid = $('#MainContent_LookId').text();
        var iframeUrl = "https://www.facebook.com/plugins/comments.php?api_key=155821997899161&locale=en_US&sdk=joey&channel_url=http%3A%2F%2Fstatic.ak.facebook.com%2Fconnect%2Fxd_arbiter.php%3Fversion%3D19%23cb%3Df33b4830ce4bc0e%26origin%3Dhttp%253A%252F%252Ffashionkred.com%252Ff28898b74f7c6f8%26domain%3Dfashionkred.com%26relation%3Dparent.parent&numposts=2&width=470&href=http%3A%2F%2FfashionKred.com%2Flook.aspx%3Flid%3D";
        $('.fb_ltr').attr("src", iframeUrl + lookid);
    }
    else
    {
        document.getElementById("fbCommentDiv").innerHTML = "<fb:comments href='" + GetUrl() + "' width='470' num-posts='2'></fb:comments>";
        FB.XFBML.parse($('#fbCommentDiv'));
    
    }
    
  
}
// can't finish part sample
function CantFinishLook() {
    $('.create-finish').fadeOut(400);
    $('.create-finish').delay(1500).fadeIn(400);
}

function createlook() {

    var userid = $('#MainContent_UserId').text();

    var pids = new Array();
    var pcolors = new Array();
    var pcats = new Array();

    for (var i = 0; i < 3; i++) {
        pids[i] = $('#MainContent_P' + (i+1) + 'Id').text();
        pcolors[i] = $('#MainContent_P' + (i+1) + 'Color').text();
        pcats[i] = $('#MainContent_P' + (i+1) + 'Cat').text();
    }

    var CoverPdt = $('#MainContent_CoverPdt').text();
    var productmap= "";
    for (i = 0; i < 3; i++) {
        if (i > 0)
            productmap += ("|");

        productmap += (pids[i] + "," + pcolors[i] + "," + pcats[i] + ",");
        if (CoverPdt == i)
            productmap += 1;
        else
            productmap += 0;
    }
    
    var tags = $('#tags').val().split(',');
    var tagmap;
    if (tags.length > 0) {
        tagmap = tags[0] ;
        for (i = 1; i < tags.length; i++) {
            tagmap += ("|" + tags[i]);
        }
    }
    
    var title = $('#title').val();
    var originalLookId = $('#MainContent_OriginalLook').text();

    //Post look to DB
    $.ajax({
        url: hostname + "/createlook.aspx?productmap=" + encodeURIComponent(productmap) + "&tagmap=" + encodeURIComponent(tagmap)
         +"&title=" +encodeURIComponent(title) + "&uid="  + userid + "&originalLookId=" + originalLookId + "&callback=?",
        cache: false,
        dataType: 'jsonp',
        jsonpCallback: 'UpdateCreateLook',
        contentType: 'application/json',
        timeout: 10000
    });
}

function UpdateCreateLook(data) {
    
    //update the carousel
     var obj = eval( data );

    if (obj.ErrorMessage != null) {
        ErrorVoteMessage(obj.ErrorMessage);
        return false;
    }

    //update the look
    $('#MainContent_LookId').text(obj.Look.id);

    var imageurl = hostname + "/images/looks/" + obj.Look.id + ".jpg";
    var description = encodeURIComponent(obj.LookDescription);


    $('#outfit').attr("src", imageurl);
    $('#fb').attr("href", "https://www.facebook.com/sharer/sharer.php?u=" + encodeURIComponent(GetReferrerUrl()));
    $('#email').attr("href", "mailto:?subject=Check%20out%20my%20outfit%20at%20FashionKred&body=" + description );
    $('#pinterest a').attr("href", "//pinterest.com/pin/create/button/?url=" + encodeURIComponent(GetReferrerUrl()) + "&media=" + imageurl + "&description=" + description);
    $('#twitter a').attr("href", "https://twitter.com/intent/tweet?hashtags=nordstrom%2Coutfit%2Cfashionkred&related=nordstorm&text=Check out my outfit @fashionkred" + "&tw_p=tweetbutton&url=" + encodeURIComponent(GetReferrerUrl()));
    $('#tumblr a').attr("href", "http://www.tumblr.com/share/photo?source=" + encodeURIComponent(imageurl) + "&caption=" + description + "&clickthru=" + encodeURIComponent(GetReferrerUrl()));
    TrackOutBoundLinks();
    //send OG update if sharing is on
    var userShare = $('#MainContent_UserShare').text();
    if (userShare == "1") {
        FB.api(
            '/me/fashionkred:create',
            'post',
            {
                outfit: GetUrl()
                //'fb:explicitly_shared':true
            },
            function (response) {
                // handle the response

            }
        );
    }

}

function FinishSuccess(){  
  $('.create-success').fadeIn(400);
  $('.create-success').delay(2000).fadeOut(400);

  $('#sliderCreate').stop(1).animate({ left: 0 }, { duration: 600, easing: 'easeOutSine' });
  M = 1;

  $('#sliderCreate2').stop(1).animate({ left: 0 }, { duration: 600, easing: 'easeOutSine' });
  L = 1;

  $('#sliderCreate3').stop(1).animate({ left: 0 }, { duration: 600, easing: 'easeOutSine' });
  H = 1;

  //Set the buttons in default state
  $('#buttons-create-prev').attr('class', 'buttons-create-disabled');
  $('#buttons-create-next').attr('class', 'buttons-create');
  $('#buttons-create-prev-02').attr('class', 'buttons-create-disabled');
  $('#buttons-create-next-02').attr('class', 'buttons-create');
  $('#buttons-create-prev-03').attr('class', 'buttons-create-disabled');
  $('#buttons-create-next-03').attr('class', 'buttons-create');
//  $('.left-item-wrapper').delay(500).fadeOut(400).queue(function(n) {
//    $(this).html("<div class='create-big-shape'><div class='create-content-text'><strong>SELECT A DRESS</strong> <br/>FROM FAVORITES</div></div><br/>");
//    n();}).fadeIn(400);
//  $('.right-item-wrapper').delay(500).fadeOut(400).queue(function(n){
//    $(this).html("<div class='create-big-shape'><div class='create-content-text'><strong>SELECT A SHOE</strong> <br/>FROM FAVORITES</div></div><br/>");
//    n();}).fadeIn(400);
}

function ErrorLookMessage(){
    alert("Something went wrong - Please try again!");
    SetFinishButton();
}

function love(product) {
    
    DisableLoveButtons();
    var productId, loveCount;
    var love = 1;

    if(product == "P1"){
        productId = $('#MainContent_P1Id').text();
        loveCount = parseInt($('#MainContent_P1Love').text());

        //check the status of lovebutton
        if($('#MainContent_P1LoveButton').attr('class') == 'text-heart'){
            $('#MainContent_P1Love').text(loveCount + 1);
            $('#MainContent_P1LoveButton').attr("class", "text-heart-red");
            $('#MainContent_P1LoveImg').attr("src", "images/heart_filled.jpg");
            $('#MainContent_P1LoveImg').unbind();
        }
        else{
            love = 0;
            if(loveCount > 0){
                $('#MainContent_P1Love').text(loveCount - 1);
            }
            $('#MainContent_P1LoveButton').attr("class", "text-heart");
            $('#MainContent_P1LoveImg').attr("src", "images/heart_empty.jpg");
            $('#MainContent_P1LoveImg').hover(function () {
                    $(this).attr('src', 'images/heart_filled.jpg');
                }, function () {
                    $(this).attr('src', 'images/heart_empty.jpg');
                });
        }
    }
    else if (product == "P2"){
        productId = $('#MainContent_P2Id').text();
        loveCount = parseInt($('#MainContent_P2Love').text());

        if($('#MainContent_P2LoveButton').attr('class') == 'text-heart'){
            $('#MainContent_P2Love').text(loveCount + 1);
            $('#MainContent_P2LoveButton').attr("class", "text-heart-red");
            $('#MainContent_P2LoveImg').attr("src", "images/heart_filled.jpg");
            $('#MainContent_P2LoveImg').unbind();
        }
        else{
            love = 0;

            if(loveCount > 0){
                $('#MainContent_P2Love').text(loveCount - 1);
            }
            $('#MainContent_P2LoveButton').attr("class", "text-heart");
            $('#MainContent_P2LoveImg').attr("src", "images/heart_empty.jpg");
            $('#MainContent_P2LoveImg').hover(function () {
                    $(this).attr('src', 'images/heart_filled.jpg');
                }, function () {
                    $(this).attr('src', 'images/heart_empty.jpg');
                });
        }
        }
        else if (product == "P3") {
            productId = $('#MainContent_P3Id').text();
            loveCount = parseInt($('#MainContent_P3Love').text());

            if ($('#MainContent_P3LoveButton').attr('class') == 'text-heart') {
                $('#MainContent_P3Love').text(loveCount + 1);
                $('#MainContent_P3LoveButton').attr("class", "text-heart-red");
                $('#MainContent_P3LoveImg').attr("src", "images/heart_filled.jpg");
                $('#MainContent_P3LoveImg').unbind();
            }
            else {
                love = 0;

                if (loveCount > 0) {
                    $('#MainContent_P3Love').text(loveCount - 1);
                }
                $('#MainContent_P3LoveButton').attr("class", "text-heart");
                $('#MainContent_P3LoveImg').attr("src", "images/heart_empty.jpg");
                $('#MainContent_P3LoveImg').hover(function () {
                    $(this).attr('src', 'images/heart_filled.jpg');
                }, function () {
                    $(this).attr('src', 'images/heart_empty.jpg');
                });
            }
        }

    var userid = $('#MainContent_UserId').text();
    var lookid = $('#MainContent_LookId').text();

    //send an OG update
    var userShare = $('#MainContent_UserShare').text();
    
    if(love == 1 && userShare == "1"){
        FB.api(
            '/me/fashionkred:love',
            'post',
            {
                outfit: GetProductUrl(productId)
            },
            function (response) {
                // handle the response
              
            }
        );
    }
    else{
        //send delete OG update
    }

    //Post love to DB
    $.ajax({
        url: hostname + "/love.aspx?pid=" + productId + "&lid=" + lookid + "&uid=" + userid + "&heart=" + love+ "&callback=?",
        cache: false,
        dataType:'jsonp',
        jsonpCallback: 'UpdateProduct',
        contentType: 'application/json',
        timeout: 10000
    });
}

function pdtlove() {
    
    var productId = $('#MainContent_PId').text();
    var loveCount = parseInt($('#MainContent_LoveCount').text());
    $('#MainContent_LoveCount').text(loveCount + 1);
    var userid = $('#MainContent_UserId').text();
    
    
    //send an OG update
    var userShare = $('#MainContent_UserShare').text();
    if (userShare == "1") {
        FB.api(
        '/me/fashionkred:love',
        'post',
        {
            outfit: GetProductUrl(productId)
        },
        function (response) {
            // handle the response

        }
        );
    }
    

    //Post love to DB
    $.ajax({
        url: hostname + "/love.aspx?pid=" + productId + "&uid=" + userid + "&callback=?",
        cache: false,
        dataType:'jsonp',
        jsonpCallback: 'UpdateProduct',
        contentType: 'application/json',
        timeout: 10000
    });
}

function UpdateProduct(data){
    
    //handle data response
    SetLoveButtons();

    //update the carousel
     var obj = eval(data);

     if (obj.ErrorMessage != null) {
         ErrorVoteMessage(obj.ErrorMessage);
         return false;
     }
     else if (obj.RedirectUrl != null) {
         window.location.replace(obj.RedirectUrl);
     }
     else {
         //Update favorites carousel
         document.getElementById('MainContent_Favorites').innerHTML = obj.FavoritesHtml;
     }
    TrackOutBoundLinks();
}

function ErrorLoveMessage(data){
    //handle error response
    //alert("something went wrong! Please Try again!");
    SetLoveButtons();
}
function subscribe() {

    DisableSubscribeButton();
    var subscribe = 1;
    var subscriberid =  $('#MainContent_CreatorId').text();

    //check the status of subscribe button
    if ($('#MainContent_SubscribePanel').attr('class') == 'follow-button') {
        $('#MainContent_SubscribePanel').attr("class", "follow-button following");
        $('#MainContent_Subscribe').text("Following");
        $('#MainContent_SubscribePanel').hover(function () {
            $(this).attr('class', 'follow-button unfollow');
            $('#MainContent_Subscribe').text("Unfollow");
        }, function () {
            $(this).attr('class', 'follow-button following');
            $('#MainContent_Subscribe').text("Following");
        });
    }
    else {
        subscribe = 0;

        $('#MainContent_SubscribePanel').attr("class", "follow-button");
        $('#MainContent_Subscribe').text("Follow");
        $('#MainContent_SubscribePanel').unbind();
    }



    var userid = $('#MainContent_UserId').text();
    var lookid = $('#MainContent_LookId').text();

    //send an OG update
    var userShare = $('#MainContent_UserShare').text();

    if (subscribe == 1 && userShare == "1") {
        FB.api(
          'me/og.follows',
          'post',
          {
              profile: GetUserUrl(subscriberid)
          },
          function (response) {
              // handle the response
          }
        );
    }
    else {
        //send delete OG update
    }

    //Post love to DB
    $.ajax({
        url: hostname + "/subscribe.aspx?uid=" + userid + "&sid=" + subscriberid + "&subscribe=" + subscribe + "&callback=?",
        cache: false,
        dataType: 'jsonp',
        jsonpCallback: 'UpdateSubcribe',
        contentType: 'application/json',
        timeout: 10000
    });
}
function UpdateSubcribe(data) {

    //handle data response
    SetSubscribeButton();

    //update the carousel
    var obj = eval(data);

    if (obj.ErrorMessage != null) {
        ErrorVoteMessage(obj.ErrorMessage);
        return false;
    }
    else if (obj.RedirectUrl != null) {
        window.location.replace(obj.RedirectUrl);
    }
    
}

function GetProduct(pid, position){
    $.ajax({
        url: hostname + "/getproduct.aspx?pid=" + pid + "&pos=" + position + "&callback=?",
        cache: false,
        dataType:'jsonp',
        jsonpCallback: 'UpdateProductPanel',
        contentType: 'application/json',
        timeout: 10000
    });
}

function UpdateProductPanel(data){
    
    //update the carousel
     var obj = eval(data);

    if (obj.ErrorMessage != null) {
        ErrorVoteMessage(obj.ErrorMessage);
        return false;
    }
    else {
        
        
        //Update product carousel
        switch(obj.Panel){
            case 1:
                $('#MainContent_P1Id').text(obj.ProductId);
                document.getElementById('LeftProduct').innerHTML = obj.ProductHtml;
                //document.getElementById('MainContent_LeftProductContent').innerHTML = obj.ProductContentHtml;
                break;
            case 2:
                $('#MainContent_P2Id').text(obj.ProductId);
                document.getElementById('RightProduct').innerHTML = obj.ProductHtml;
                //document.getElementById('MainContent_RightProductContent').innerHTML = obj.ProductContentHtml;
                break;
        }
         
    }

    TrackOutBoundLinks();

}
function GetProductsByCategory(uid, contestid, position, category) {

    var color;
    switch (position) {
        case 1:
            color = $('#MainContent_P1Color').text();
            break;
        case 2:
            color = $('#MainContent_P2Color').text();
            break;
        case 3:
            color = $('#MainContent_P3Color').text();
            break;

    }
    $.ajax({
        url: hostname + "/getproductsbycolor.aspx?uid=" + uid + "&pos=" + position + "&cid=" + contestid + "&color=" + color + "&category=" + category + "&callback=?",
        cache: false,
        dataType: 'jsonp',
        jsonpCallback: 'UpdateProductsPanel',
        contentType: 'application/json',
        timeout: 10000
    });
}

function GetProductsByColor(uid, contestid, position, color) {

    var category;
    switch (position) {
        case 1:
            category = $('#MainContent_P1Cat').text();
            break;
        case 2:
            category = $('#MainContent_P2Cat').text();
            break;
        case 3:
            category = $('#MainContent_P3Cat').text();
            break;

    }
    $.ajax({
        url: hostname + "/getproductsbycolor.aspx?uid=" + uid + "&pos=" + position + "&cid=" + contestid + "&color=" + color + "&category=" + category +  "&callback=?",
        cache: false,
        dataType: 'jsonp',
        jsonpCallback: 'UpdateProductsPanel',
        contentType: 'application/json',
        timeout: 10000
    });
}
function UpdateProductsPanel(data) {

    //update the carousel
    var obj = eval(data);

    if (obj.ErrorMessage != null) {
        ErrorVoteMessage(obj.ErrorMessage);
        return false;
    }
    else {


        //Update product carousel
        switch (obj.Position) {
            case 1:
                $('.carousel-favorites').html(obj.FavoriteHtml);
                $('.carousel-favorites').attr("style", "left: 0px;");
                $('#sliderCreate').html(obj.SliderHtml);
                $('#sliderCreate').attr("style", "left: 0px;");
                countM = $("#sliderCreate li").length;
                imagesLenght01 = $('div.fav-image').length;
                trigger01 = -50 - (imagesLenght01 - 7) * 120;
                M = 1;
                //Set the buttons in default state
                $('#buttons-create-prev').attr('class', 'buttons-create-disabled');
                $('#buttons-create-next').attr('class', 'buttons-create');
                $('#MainContent_P1Color').text(obj.Color);
                $('#MainContent_P1Cat').text(obj.Category);
                //find the color button and select it
                //$('.carousel-favorites').find('a.' + obj.Color).css("display", "none");
                break;
            case 2:
                $('.carousel-favorites_2').html(obj.FavoriteHtml);
                $('.carousel-favorites_2').attr("style", "left: 0px;");
                $('#sliderCreate2').html(obj.SliderHtml);
                $('#sliderCreate2').attr("style", "left: 0px;");
                countL = $("#sliderCreate2 li").length;
                L = 1;
                imagesLenght02 = $('div.fav-image_2').length;
                trigger02 = -50 - (imagesLenght02 - 7) * 120;
                $('#buttons-create-prev-02').attr('class', 'buttons-create-disabled');
                $('#buttons-create-next-02').attr('class', 'buttons-create');
                $('#MainContent_P2Color').text(obj.Color);
                $('#MainContent_P2Cat').text(obj.Category);
                
                break;
            case 3:
                $('.carousel-favorites_3').html(obj.FavoriteHtml);
                $('.carousel-favorites_3').attr("style", "left: 0px;");
                $('#sliderCreate3').html(obj.SliderHtml);
                $('#sliderCreate3').attr("style", "left: 0px;");
                countH = $("#sliderCreate3 li").length;
                H = 1;
                imagesLenght03 = $('div.fav-image_3').length;
                trigger03 = -50 - (imagesLenght03 - 7) * 120;
                $('#buttons-create-prev-03').attr('class', 'buttons-create-disabled');
                $('#buttons-create-next-03').attr('class', 'buttons-create');
                $('#MainContent_P3Color').text(obj.Color);
                $('#MainContent_P3Cat').text(obj.Category);
                
                break;
        }

    }

    TrackOutBoundLinks();

}
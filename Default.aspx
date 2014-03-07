<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" ClientIDMode="Static"
    Debug="true" CodeFile="Default.aspx.cs" Inherits="ShopSenseDemo._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

    <!-- CSS Implementing Plugins -->
    <link rel="stylesheet" href="plugins/font-awesome/css/font-awesome.css" />
    <!-- CSS Theme -->
    <link rel="stylesheet" href="Styles/responsiveslides.css" />
    <link rel="stylesheet" href="Styles/demo.css" />
    <script type="text/javascript" src="Scripts/jquery-1.8.2.min.js"></script>


        <script language="javascript" type="text/javascript">


        function OnLoveClick(uid, lid, votetype, point, lightweight, db, isloved, upvote) {

            // alert(document.getElementById("hdnchkuser").value);
            if (document.getElementById("hdnchkuser").value == "0") {
                $("#overlay_signin").show();

                return false;
            }

            else {


                var getclass = '';
                var checkclass = 'Love' + lid;
                var getspan = 'span' + lid;

                //  var splitvalues = document.getElementById(getspan).innerHTML.split('Love');

                // alert(splitvalues[0]);

                getclass = document.getElementById(checkclass).className;




                //  alert(isloved);
                if (getclass == "love") {

                    var db = document.getElementById("hdnDb").value;
                    votetype = "up";

                    //db = 'server=localhost;Database=Fashionkred;Integrated Security=True;" providerName="System.Data.SqlClient';
                    // alert('hello');
                    jQuery.ajax({
                        url: 'WebServices.aspx/UpdateLove',
                        type: "POST",
                        data: "{'UserIdMain':" + uid + ", 'lookIdMain':'" + lid + "','voteTypeMain':'" + votetype + "','pointMain':" + point + ",'lightweightMain':'" + lightweight + "', 'db':'" + db + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            //  alert('v');

                            var countold = '';
                            var count = '';
                            var splitvalues = document.getElementById(getspan).innerHTML.split('Love');

                            countold = splitvalues[0];

                            //alert(countold);
                            count = parseInt(countold) + 1;

                            // alert(count);

                            document.getElementById(getspan).innerHTML = count + ' Love';

                            document.getElementById(checkclass).className = 'love active';

                        }
                    });

                }
                else {
                    //              alert('true');
                    var db = document.getElementById("hdnDb").value;
                    votetype = "down";

                    //db = 'server=localhost;Database=Fashionkred;Integrated Security=True;" providerName="System.Data.SqlClient';
                    // alert('hello');
                    jQuery.ajax({
                        url: 'WebServices.aspx/UpdateLove',
                        type: "POST",
                        data: "{'UserIdMain':" + uid + ", 'lookIdMain':'" + lid + "','voteTypeMain':'" + votetype + "','pointMain':" + point + ",'lightweightMain':'" + lightweight + "', 'db':'" + db + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            //   alert('else');

                            var countold = '';
                            var count = '';
                            // count = document.getElementById(getspan).innerHTML.indexOf("Love");

                            var splitvalues = document.getElementById(getspan).innerHTML.split('Love');

                            countold = splitvalues[0];

                            //alert(countold);
                            count = parseInt(countold) - 1;

                            // alert(count);

                            document.getElementById(getspan).innerHTML = count + ' Love';

                            document.getElementById(checkclass).className = 'love';

                        }
                    });
                }
            }
        }

        function BindLooks(tagId, userId, pageId, db) {
            $("#imgloading").show();
            var db = document.getElementById("hdnDb").value;
            jQuery.ajax({
                url: 'WebServices.aspx/BindLooks',
                type: "POST",
                data: "{'tagId':'" + tagId + "', 'userId':'" + userId + "','pageId':'" + pageId + "','db':'" + db + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    debugger;

                   



                    //  alert('v');

                    var myArray = data.d;

                    //alert(myArray);


                    if (myArray.length > 0) {


                        for (i = 0; i <= myArray.length; i++) {
                            //alert(myArray.length);
                            //alert(html);

                            lid = myArray[i].id;

                            createrId = myArray[i].creator.id;
                            imgsrc = myArray[i].creator.pic;
                            orginallookid = myArray[i].originalLookId;
                            creatername = myArray[i].creator.name;
                            title = myArray[i].title;

                            upvote = myArray[i].upVote;

                            restylecount = myArray[i].restyleCount;

                            viewCount = myArray[i].viewCount;

                            isloved = myArray[i].isLoved;

                            var tag = myArray[i].tags;

                            var product = myArray[i].products;

                            



                            AppendHTML(createrId, imgsrc, orginallookid, creatername, title, tag, product, lid, upvote, restylecount, viewCount, isloved);

                        }


                    }

                    $("#imgloading").hide();
                },
                error: function (response) {
                    alert(response.responseText);
                    settings.error(response);
                    if (settings.debug) { alert("Error Calling Method \"" + settings.methodName + "\"\n\n+" + response.responseText); }
                }
            });

            

        }

        var html = '';
        function AppendHTML(createrId, imgsrc, orginallookid, creatername, title, tag, product, lid, upvote, restylecount, viewCount, isloved) {
            //alert(imgurl);

            var orglookid = '';


            if (orginallookid == "0") {
                orglookid = 'styled by';
            }
            else {
                orglookid = 're-styled by';
            }

            var coverCSS;



            html += '<div class="container border-top"><div class="margin-bottom-49 row-fluid"> <div class="span3 re-style"><div class="posts margin-bottom-20"><dl class="dl-horizontal"><dt><a href="profile.aspx"><img src=' + imgsrc + ' /></a></dt><dd><p class="Restyle-new">' + orglookid + '</p><a style="color: #D018B6;" href=user.aspx?userid=' + createrId + '>' + creatername + '</a></dd><h5>' + title + '</span></h5></dl></div>';

            AppendTags(tag);

            AppendinnerLook(product, lid);

            AppendButtons(lid, upvote, restylecount, viewCount, isloved);



            $("#divMainLook").append(html);

            //clear html content once added in page.
            html = '';

        }


        function AppendTags(tag) {

            html += '<div class="span9" id="restyle">';

            for (j = 0; j < tag.length; j++) {
                var tagid = tag[j].id;
                var tagname = tag[j].name;
                html += '<a id="lnktag' + tagid + '"href=default.aspx?tagid=' + tagid + '><button class="btn-u" type="button">' + tagname + ' </button></a>';

            }

            html += '</div></div>';
        }

        function isCover(iscover) {
            var coverCSS = '';
            if (iscover == true) {
                coverCSS = 'view view-tenth span4';
            }
            else {
                coverCSS = 'view view-tenth span3';
            }

            return coverCSS;
        }


        function AppendinnerLook(product, lid) {



            html += '<div class="span9 re-style"><div class="view_col">';

            for (k = 0; k < product.length; k++) {
                //var pid = product[k].id;
                var purl = product[k].images[0].url;

                var chkcover = product[k].isCover;

                var coverCSS = isCover(chkcover);


                html += '<div id="divimgLook" class="' + coverCSS + '" ><a href="javascript:void(0)" onclick="RedirecttoLookPage(' + lid + ')"><img id="imgLook' + lid + '" src=' + purl + ' class="mar-bottom11" /></a></div>';

            }
        }


        function AppendButtons(lid, upvote, restylecount, viewCount, isloved) {
            // html += '</div><div class="span10 grey-bg"><div class="top"><div class="number"><span> Love</span> <span> Restyle</span> <span>Comment</span> <span>share</span></div><div class="clearfix"></div><ul class="loginbar comment-block1"><li><a href="javascript:void(0)" >Love</a></li><li><a href="look.aspx" class="re-style1">Restyle</a></li><li><a href="#" class="comment1">comment</a></li><li><a href="#" class="share">share</a></li></ul></div></div></div></div></div>';
            var lovedClass = '';
            // alert(isloved);

            if (isloved == false) {
                lovedClass = 'love';
            }
            else {
                lovedClass = 'love active';
            }
            html += '</div><div class="span10 grey-bg"><div class="top"><div class="number"><span id="span' + lid + '">' + upvote + ' Love</span> <span>' + restylecount + ' Restyle</span> <span>' + upvote + ' Comment</span> <span>' + viewCount + ' share</span></div><div class="clearfix"></div><ul class="loginbar comment-block1"><li><a id="Love' + lid + '" class="' + lovedClass + '" onclick=OnLoveClick(\"4\",' + lid + ',\"up\",\"0\",\"True\",\"\",' + isloved + ',' + upvote + ') >Love</a></li><li><a href="createlook.aspx" class="re-style1">Restyle</a></li><li><a href="#" class="comment1">comment</a></li><li><a href="#" class="share">share</a></li></ul></div></div></div></div></div>';
        }

        //function BindLooks(tagId,userId, PageId,db) {

        //    var db = document.getElementById("hdnDb").value;



        //    jQuery.ajax({
        //        url: 'WebServices.aspx/BindLooks',
        //        type: "POST",
        //        data: "{'tagId':'" + tagId + "', 'userid':" + userId + ",'pageId':'" + PageId + "', 'db':'" + db + "'}",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (data) {
        //        alert('v');
        //        }
        //    });

        //}


        // Code for Lazy loading, load next records on bottom of page//
        var strName = jQuery.noConflict();


        strName(window).scroll(function () {
            var documentHeight = strName(document).height();
            var windowHeight = strName(window).height();
            documentHeight = documentHeight - windowHeight;
            if ((strName(window).scrollTop()) == (documentHeight)) {
              
                var itemct = document.getElementById("hdnitemscount").value;

                itemct = parseInt(itemct) + 15;

                document.getElementById("hdnitemscount").value = itemct;

                //alert(itemct);

                var tagid = GetQueryStringParams('tagid');

                BindLooks(tagid, '0', itemct, '');
            }
        });

    </script>

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <input type="hidden" runat="server" id="hdnchkuser" />

    <input type="hidden" runat="server" id="hdnDb" />

    <input type="hidden" runat="server" id="hdnitemscount" value="1" />

    <input type="hidden" runat="server" id="hdnScpostion" />

    <asp:Panel ID="LookContent" runat="server" />


    <!--=== Top ===-->
    <div class="top">
        <div class="header">
            <div class="container border-dotted">
                <div class="logo">
                    <a href="index.html">
                        <img id="logo-header" src="Images/logo.png" alt="Logo"></a>
                </div>
                <div class="loginbar pull-right">
                    <div class="navbar">
                        <div class="navbar-inner">
                            <a class="btn btn-navbar" data-toggle="collapse" data-target=".nav-collapse"><span
                                class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span>
                            </a>
                            <!-- /nav-collapse -->
                            <div class="nav-collapse collapse">
                                <ul class="nav top-2">
                                    <li class="active yellow"><a href="#" class="dropdown-toggle" data-toggle="dropdown">Show<br />
                                        All looks </a></li>
                                    <li class="green"><a href="" class="dropdown-toggle" data-toggle="dropdown">Create<br />
                                        a Look</a> </li>
                                    <li class="pink">
                                        <a href="user.aspx" title="Settings">
                                            <asp:Image ID="UserImage" CssClass="rounded avatar" runat="server" /></a>
                                        <asp:HyperLink ID="LogInLink" runat="server" Visible="false" Style="margin-right: 8px;"></asp:HyperLink>
                                        <asp:HyperLink ID="UserName" class="dropdown-toggle" data-toggle="dropdown" runat="server" />
                                        <asp:Label ID="UserPoints" runat="server" /></li>
                                    <li class="comments"><a class="search"><i class="search-btn icon-remove">3
                                                                           </i> Notifications
                                    </a></li>
                                </ul>
                                <div class="search-open">
                                    <div class="input-append">
                                        <div class="span3">
                                            <div class="posts margin-bottom-20">
                                                <dl class="dl-horizontal">
                                                    <dt><a href="#">
                                                        <img src="Images/thumb2.png" alt="" /></a></dt>
                                                    <dd>Sofia C
                                                        <p>
                                                            <a href="#">re-styled your look</a>
                                                        </p>
                                                    </dd>
                                                </dl>
                                                <dl class="dl-horizontal">
                                                    <dt><a href="#">
                                                        <img src="Images/thumb2.png" alt="" /></a></dt>
                                                    <dd>Laura Win
                                                        <p>
                                                            <a href="#">mentioned you</a>
                                                        </p>
                                                    </dd>
                                                </dl>
                                                <dl class="dl-horizontal">
                                                    <dt><a href="#">
                                                        <img src="Images/thumb2.png" alt="" /></a></dt>
                                                    <dd>Steve Mars
                                                        <p>
                                                            <a href="#">mentioned you</a>
                                                        </p>
                                                    </dd>
                                                </dl>
                                                <dl class="dl-horizontal">
                                                    <dt><a href="#">
                                                        <img src="Images/thumb2.png" alt="" /></a></dt>
                                                    <dd>Marianne Grosz
                                                        <p>
                                                            <a href="#">created a new collection</a>
                                                        </p>
                                                    </dd>
                                                </dl>
                                                <dl class="dl-horizontal">
                                                    <dt><a href="#">
                                                        <img src="Images/thumb2.png" alt="" /></a></dt>
                                                    <dd>Celia Costanza
                                                        <p>
                                                            <a href="#">commented on your look</a>
                                                        </p>
                                                    </dd>
                                                </dl>
                                            </div>
                                            <!--/posts-->
                                        </div>
                                    </div>
                                </div>
                                <!-- /nav-collapse -->
                            </div>
                            <!-- /navbar-inner -->
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!--/top-->
    </div>
    <!--/top-->
    <!--=== End Top ===-->
    <!--banner-->
    <div class="container">
        <div class="row-fluid margin-bottom-45">
            <!--left-->
            <div class="span7 div5">
                <div id="myCarousel" class="carousel slide">
                    <div class="carousel-inner">
                        <div class="item active">
                            <img src="Images/1.png" alt="">
                            <div class="carousel-caption">
                                <p>
                                    Cras justo odio, dapibus ac facilisis in, egestas.
                                </p>
                            </div>
                        </div>
                        <div class="item">
                            <img src="Images/colllection-img.png" alt="">
                            <div class="carousel-caption">
                                <p>
                                    Cras justo odio, dapibus ac facilisis in, egestas.
                                </p>
                            </div>
                        </div>
                        <div class="item">
                            <img src="Images/fashion_week.png" alt="">
                            <div class="carousel-caption">
                                <p>
                                    Cras justo odio, dapibus ac facilisis in, egestas.
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="carousel-arrow">
                        <a class="left carousel-control" href="#myCarousel" data-slide="prev"><i class="icon-angle-left"></i></a><a class="right carousel-control" href="#myCarousel" data-slide="next"><i
                            class="icon-angle-right"></i></a>
                    </div>
                </div>
            </div>
            <!--mid-->
            <div class="span3 comment-block">
                <div class="posts margin-bottom-20">
                    <dl class="dl-horizontal">
                        <dt><a href="#">
                            <img src="Images/thumb2.png" alt="" /></a></dt>
                        <dd>Sofia C
                            <p>
                                <a href="#">re-styled your look</a>
                            </p>
                        </dd>
                    </dl>
                    <dl class="dl-horizontal">
                        <dt><a href="#">
                            <img src="Images/thumb2.png" alt="" /></a></dt>
                        <dd>Laura Win
                            <p>
                                <a href="#">mentioned you</a>
                            </p>
                        </dd>
                    </dl>
                    <dl class="dl-horizontal">
                        <dt><a href="#">
                            <img src="Images/thumb2.png" alt="" /></a></dt>
                        <dd>Steve Mars
                            <p>
                                <a href="#">mentioned you</a>
                            </p>
                        </dd>
                    </dl>
                    <dl class="dl-horizontal">
                        <dt><a href="#">
                            <img src="Images/thumb2.png" alt="" /></a></dt>
                        <dd>Marianne Grosz
                            <p>
                                <a href="#">created a new collection</a>
                            </p>
                        </dd>
                    </dl>
                    <dl class="dl-horizontal">
                        <dt><a href="#">
                            <img src="Images/thumb2.png" alt="" /></a></dt>
                        <dd>Celia Costanza
                            <p>
                                <a href="#">commented on your look</a>
                            </p>
                        </dd>
                    </dl>
                </div>
                <!--/posts-->
            </div>
            <!--right-->
            <div class="span2 img-right">
                <img src="Images/right_image.png">
            </div>
        </div>
    </div>
    <!--block1-->
    <div id="divMainLook">
    </div>

    <div id="imgloading" style="margin-left: 50%;">
        <img  runat="server" src="~/assets/images/ajax-loader.gif" />
    </div>
    <div class="container border-top">
    </div>
    <!--=== Copyright ===-->
    <!--/copyright-->
    <!--=== End Copyright ===-->
    <!-- JS Global Compulsory -->
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="scripts/responsiveslides.min.js"></script>
    <script src="scripts/responsiveslides.js"></script>
    <script>

        // Slideshow 3
        $("#slider3").responsiveSlides({
            manualControls: '#slider3-pager',
            maxwidth: 540
        });

    </script>
    <script type="text/javascript" src="Scripts/jquery-1.8.2.min.js"></script>
    <script type="text/javascript" src="plugins/bootstrap/js/bootstrap.min.js"></script>
    <!-- JS Implementing Plugins -->
    <!-- JS Page Level -->
    <script type="text/javascript" src="Scripts/app.js"></script>

    <script src="Scripts/jquery-1.8.2.min.js"></script>
    <script type="text/javascript">
        function GetQueryStringParams(sParam) {
            var sPageURL = window.location.search.substring(1);
            var sURLVariables = sPageURL.split('&');
            for (var i = 0; i < sURLVariables.length; i++) {
                var sParameterName = sURLVariables[i].split('=');
                if (sParameterName[0] == sParam) {
                    return sParameterName[1];
                }
            }
        }

        // function used for redirecting to look.aspx page//
        function RedirecttoLookPage(lid) {

            var s = $(document).scrollTop();
            window.location.href = "look.aspx?lid=" + lid + "&sp=" + s.toString();
        }



    </script>
    <!--[if lt IE 9]>
    <script src="assets/js/respond.js"></script>
<![endif]-->





    <div id="overlay_signin" style="display: none;" class="PopupWindowOuter">
        <div class="close">
            <a href="#">
                <img src="images/close_popup.png" alt="Close"></a>
        </div>
        <div class="login-body">
            <div class="left-pane">
                <img src="images/nordstromlook.jpg" alt="Earn commission on sales!" style="line-height: 20px;" />
            </div>
            <div class="right-pane">
                <div class="contest-call">
                    Vote on looks or create your own! Earn 10% commission on purchases from your looks!
                </div>
                <a href="default.aspx?login=1">
                    <div class="fb-custom">
                        <span style="padding-left: 20px; line-height: 48px;">Signup With Facebook</span>
                    </div>
                </a>
                <div style="text-align: center;">
                    Already a member?&nbsp;<a href="default.aspx?login=1"><span>Sign In</span></a>
                </div>
            </div>
        </div>
    </div>
    <!-- metadata for page -->
    <asp:Label ID="Unsigned" runat="server" Style="display: none;" />
    <asp:Label ID="UserShare" runat="server" Style="display: none;" />
    <asp:Label ID="UserId" runat="server" Style="display: none;" />
    <!--js for the page -->
    <script type="text/javascript">

        $(document).ready(function () {

            var qStr = GetQueryStringParams('tagid');
            var pageId = 1;
            BindLooks(qStr, '0', pageId, '');


            var qStr = GetQueryStringParams('sp');

            // alert('v');
            $(document).scrollTop(qStr);


            SetSiginPopup();
            SetHPLoveButtons();

            App.init();
            App.initSliders();
            Index.initParallaxSlider();

        });

    </script>
</asp:Content>

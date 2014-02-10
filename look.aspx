<%@ Page Language="C#" AutoEventWireup="true" CodeFile="look.aspx.cs" Inherits="Outfit"
    Debug="true" MasterPageFile="~/HomeMaster.master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <!-- CSS Global Compulsory-->
    <link rel="stylesheet" href="assets/plugins/bootstrap/css/bootstrap.min.css">
    <link rel="stylesheet" href="assets/css/style.css">
    <link rel="stylesheet" href="assets/plugins/bootstrap/css/bootstrap-responsive.min.css">
    <!-- CSS Implementing Plugins -->
    <!--  <link rel="stylesheet" type="text/css" href="assets/css/demo1.css" />-->
    <link rel="stylesheet" type="text/css" href="assets/css/elastislide.css" />
    <link rel="stylesheet" type="text/css" href="assets/css/custom.css" />
    <link rel="stylesheet" href="assets/plugins/font-awesome/css/font-awesome.css">
    <!-- CSS Theme -->
    <link rel="stylesheet" href="assets/css/responsiveslides.css">
    <link rel="stylesheet" href="assets/css/demo.css">
    <link rel="stylesheet" href="assets/plugins/bxslider/jquery.bxslider.css">
    <!-- CSS Theme -->
    <!--  <link rel="stylesheet" href="assets/css/responsiveslides.css">
    <link rel="stylesheet" href="assets/css/demo.css">
    <link rel="stylesheet" href="assets/plugins/bxslider/jquery.bxslider.css">-->
    <!--  <link rel="stylesheet" href="assets/plugins/bxslider/jquery.bxslider.css">   -->
    <script type="text/javascript" src="assets/js/jquery-1.8.2.min.js"></script>
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


        $(document).ready(function () {

            var qStr = GetQueryStringParams('sp');

            var qEdt = GetQueryStringParams('edt');
            // alert(qEdt);

            debugger;
            if (qEdt == 'yes') {

                var Tid = setInterval(function () { OpenNextPage(2, Tid) }, 100);

                $("#divfirst .elastislide-next").hide();

                //OpenNextPage(2);

            }
            else {
                //  alert('no');
            }

            $("#imgback").click(function () {
                var qStr = GetQueryStringParams('sp');
                window.location.href = "index.html?sp=" + qStr;
            });
            $('#arrow').click(function () {
                //alert('');

                $('.view-tenth').removeClass('test').css('border', '');
                $('.DisplayImages').css('display', 'none');
                $('.price').addClass('hide');
                $('.border-dotted1').css('display', 'none');

                $('#carousel').parent().removeClass('elastislide-carousel1');

                $('#carousel2').parent().removeClass('elastislide-carousel1');


                $('.view-tenth').find('.img-bg').removeClass('img-bg1');
                $('.view-tenth').removeClass('view1');
                $('#carousel').parent().parent().removeClass('elastislide-horizontal1');

                $('#carousel2').parent().parent().removeClass('elastislide-horizontal1');

                //$('#carousel').parent().removeClass().addClass('elastislide-carousel'); 
                $('#carousel').find('li').each(function () { $(this).attr("style", "min-height:''") })

                $('#carousel2').find('li').each(function () { $(this).attr("style", "min-height:''") })
                return false;
            });
        });



        function ShowButtons(obj) {
            //alert('jbvjk');


            console.log($('#divA' + obj).height());
            debugger;
           // $('.DisplayImagesfimilar').css('display', 'none !important');
            $('.DisplayImages').css('display', 'none');

            $('.price').addClass('hide');
            $('.border-dotted1').css('display', 'none');

           
            $('#ImgA' + obj + ',#ImgF' + obj + ',#Imgp' + obj).css('display', 'block');
            $('.view-tenth').removeClass('test').css('border', '').css('margin-right', '');
            $('.view-tenth').addClass('view1');
            $('.view-tenth').find('.img-bg').addClass('img-bg1');
            //$('.view-tenth').children('a').children('div').removeClass().addClass('img-bg1')

            $('#divA' + obj).addClass('test').css('border', 'dashed #C2CBD3 2px');



            $('#divA' + obj).addClass('test').css('width', '20%');
            //alert($('#divA' + obj).css());
            //$('#divA' + obj).addClass('price').css('border', 'dashed #C2CBD3 2px');
            //$('#divA'+obj).children('a').children('div').removeClass().addClass('img-bg')
            //$('#divA' + obj).find('
            $('#arrow').show();
            $('#carousel').parent().addClass('elastislide-carousel1');
            //$('#carousel').parent().parent().addClass('elastislide-horizontal1');
            $('#divA' + obj).find('.img-bg').removeClass('img-bg1');
            $('#divA' + obj).removeClass('view1');

            $('#divA' + obj).find('.price').removeClass('hide');
            $('#divA' + obj).find('.border-dotted1').css('display', 'block');
            var $height = $('#divA' + obj).height();
            console.log($height);
            //$('.test').style('margin-right','15px','important');


            $('.test').attr("style", "margin-right: 15px !important;padding-top:12px;  margin-top:-14px !important; border:dashed #C2CBD3 2px; min-height:" + $height + "px!important;");


            //$('#carousel').css('padding-top', '15px');
            var liHeight = $('#divA' + obj).height() - 10;
            $('#carousel').find('li:not(.test)').each(function () { $(this).attr("style", "min-height:" + liHeight + "px!important;") })

            //$('.test').attr('style', 'margin-right: 15px !important;border:dashed #C2CBD3 2px');

            //alert($('.elastislide-wrapper').scrollTop());
            //$(document).scrollTop($('.elastislide-wrapper').scrollTop());

            //location.hash = '#arrow';
            $("html, body").animate({ scrollTop: 0 }, 1000);
            //$('html, body').animate({'scrollTop': $('#carousel').offset().top}, 500);
            console.log($('#divA' + obj).height());
            $('#divCommentsBlock').addClass('grey_text');
            $('#divCollapseimage').addClass('grey_text');


            $('.test img').css('margin-top', '0px');


           
               

            return false;
        }


        //similar garments functions


        function ShowButtonssimilar(obj) {
            //alert('similar');


            debugger;
            var dd = $("#carousel2").attr("style");
            dd = dd.replace("max-height: 222px;", "");
            $("#carousel2").attr("style", dd);

            $(".hidedata").addClass('hide');
            $(".hidedata1").addClass('hidetitle');
            $(".price").addClass('hide');

            console.log($('#divA' + obj).height());

            console.log($('#divB' + obj).height());
            //debugger;
            $('.DisplayImages1').css('display', 'none');


            $('.border-dotted1').css('display', 'none');


            $('#ImgC' + obj).css('display', 'block');
            $('.view-tenth').removeClass('test').css('border', '').css('margin-right', '');
            $('.view-tenth').addClass('view1');
            $('.view-tenth').find('.imgd-bg').addClass('imgd-bg1');
            //$('.view-tenth').children('a').children('div').removeClass().addClass('img-bg1')
            $('#divB' + obj).addClass('test').css('border', 'dashed #C2CBD3 2px');


            $('#divB' + obj).addClass('test').css('width', '20%');
            //alert($('#divA' + obj).css());
            //$('#divA' + obj).addClass('price').css('border', 'dashed #C2CBD3 2px');
            //$('#divA'+obj).children('a').children('div').removeClass().addClass('img-bg')
            //$('#divA' + obj).find('

            ///////$('#carousel').parent().addClass('elastislide-carousel1');
            //$('#carousel').parent().parent().addClass('elastislide-horizontal1');
            $('#divB' + obj).find('.imgd-bg').removeClass('imgd-bg1');
            $('#divB' + obj).removeClass('view1');

            $('#divB' + obj).find('.price').removeClass('hide');
            $('#divB' + obj).find('.hidedata').removeClass('hide');
            $('#divB' + obj).find('.hidedata1').removeClass('hidetitle');
            
            

            

            // $('#divB' + obj).find('.border-dotted1').css('display', 'block');
            var $height = $('#divA' + obj).height();
            var $heightsim = $('#divB' + obj).height();

            var $mmhi = $heightsim - 228;

            console.log($height);
            //   console.log($heightsim);
            console.log($mmhi);

            //$('.test').style('margin-right','15px','important');

            //alert($heightsim);

            // $height
            $('.test').attr("style", "margin-right: 15px !important; margin-top:-60px !important; border:dashed #C2CBD3 2px; min-height:" + $mmhi + "px!important;");
            //$('#carousel').find('li:not(.test)').each(function () { $(this).attr("style", "min-height:" + $('#divA' + obj).height() + "px!important;") })
            var liHeight = $('#divB' + obj).height() - 10;
            $('#carousel2').find('li:not(.test)').each(function () { $(this).attr("style", "min-height:" + liHeight + "px!important;") })
            $('#carousel2').css('padding-top', '61px');
            // $('#carousel2').find('ul:not(.test)').each(function () { $(this).attr("style", "max-height:" + $('#divB' + obj).height() + "px!important;") })
            //$('.test').attr('style', 'margin-right: 15px !important;border:dashed #C2CBD3 2px');

            //alert($('.elastislide-wrapper').scrollTop());
            //$(document).scrollTop($('.elastislide-wrapper').scrollTop());

            //location.hash = '#arrow';
            //$('html, body').animate({
            //    'scrollTop': $('#carousel').offset().top
            //}, 2000);            

            $("html, body").animate({ scrollTop: 64 }, 1000);
            $('.elastislide-carousel ul li').css('margin-right', '0');
            //console.log($('#divA' + obj).height());

            $(".button1").hide();

            $(similar_garments).click(function (e) {

                // $('#somediv').hide();
                $('.DisplayImages1').css('display', 'none');
                $('.border-dotted1').css('display', 'none');
                $('.view-tenth').removeClass('test').css('border', '').css('margin-right', '').css('margin-top', '');
                $('.view-tenth').addClass('view1');
                $('.view-tenth').find('.imgd-bg').removeClass('imgd-bg1');
                $('.elastislide-carousel ul li').css('margin-right', '1.2%');
                //$('.test').attr("style", "margin-right: 15px !important; margin-top:0px !important; border:dashed #C2CBD3 2px; min-height:" + $mmhi + "px!important;");
                //$('#carousel').find('li:not(.test)').each(function () { $(this).attr("style", "min-height:" + $('#divA' + obj).height() + "px!important;") })                
                $('#carousel2').css('padding-top', '0px');
                $('.elastislide-carousel ul li').css('margin-top', '0');
                $(".button1").show();

                $(".hidedata").addClass('hide');
                $(".hidedata1").addClass('hidetitle');
                $(".price").addClass('hide');
            });

            $('#divB' + obj).click(function (e) {
                e.stopPropagation();
            });

            return false;

            //$('#carousel2').find('li').each(function () { $(this).attr("style", "min-height:'278px'") })
        }

        function OpenNextPagesimilar(obj) {
            //debugger;
            //alert('asd');
            var j = jQuery.noConflict();

            var pid = $(obj).parent("li").attr("id")

            var img = $("#" + pid + " img").attr('src');
            var imgs = $("#" + pid + " span").html();
            var imgp = $("#" + pid + " p:eq(0)").html();
            var imgpp = $("#" + pid + " p:eq(1)").html();

            createCookie("img", img, 1);
            createCookie("imgs", imgs, 1);
            createCookie("imgp", imgp, 1);
            createCookie("imgpp", imgpp, 1);

            window.location.href = "edit-look.html";
        }

        ////////


        function createCookie(name, value, days) {
            var expires;

            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                expires = "; expires=" + date.toGMTString();
            } else {
                expires = "";
            }
            document.cookie = escape(name) + "=" + escape(value) + expires + "; path=/";
        }

        function readCookie(name) {
            var nameEQ = escape(name) + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) === ' ') c = c.substring(1, c.length);
                if (c.indexOf(nameEQ) === 0) return unescape(c.substring(nameEQ.length, c.length));
            }
            return null;
        }

        function eraseCookie(name) {
            createCookie(name, "", -1);
        }

       
        function OpenNextPage(obj, Tid) {


            debugger;


            var first = 'evening-dresses';
            var second = '';
            var third = '335';
            var fourth = '1';
            var fifth = 'server=localhost;Database=Fashionkred;Integrated Security=True;';
            //BindSimilarGar(first, second, third, fourth, fifth);

            

            BindSimilarGar(first, second, third, fourth, fifth);

            var liId = 'divA' + obj;
            createCookie("liId", liId, 1);


            if (Tid != 0) {
                clearInterval(Tid);
            }



            //debugger;
            //var dd = $("#carousel2").attr("style");
            //dd = dd.replace("max-height: 0px;", "");
            //$("#carousel2").attr("style", dd);



            $('.hide_txt').hide();

            $('#_hideheader').hide();
            $('#backbutton').hide();
            $('#similar_garments').show();

            $('.coment_hide').hide();
            //  $('.view-tenth').removeClass('test').css('border', '');
            $('.DisplayImages').css('display', 'none');
            //  $('.price').addClass('hide');
            //  $('.border-dotted1').css('display', 'none');

            // $('#carousel').parent().removeClass('elastislide-carousel1');
            //  $('.view-tenth').find('.img-bg').removeClass('img-bg1');
            //   $('.view-tenth').removeClass('view1');
            //   $('#carousel').parent().parent().removeClass('elastislide-horizontal1');
            //$('#carousel').parent().removeClass().addClass('elastislide-carousel'); 
            // $('#carousel').find('li').each(function () { $(this).attr("style", "min-height:''") })


            //duplicated arrow click code to revert elements to their initial stage



            $('.view-tenth').removeClass('test').css('border', '');
            $('.DisplayImages').css('display', 'none');
            $('.price').addClass('hide');
            $('.border-dotted1').css('display', 'none');

            $('#carousel').parent().removeClass('elastislide-carousel1');
            $('.view-tenth').find('.img-bg').removeClass('img-bg1');
            $('.view-tenth').removeClass('view1');
            $('#carousel').parent().parent().removeClass('elastislide-horizontal1');

            //$('.container').css('margin-top', '-150px');

            $(".container").animate({ "margin-top": '-74px' }, 10);

            $("#divfirst .elastislide-next").hide();

            $('.elastislide-horizontal ul li').css('min-height', '0px').css('margin-top', '0px');

            $('#carousel2').find('li').each(function () { $(this).attr("style", "min-height:'278px'") })

            $('#Div1').show();

            $('.border-dotted').css('border', 'none');

            // $("#carousel a").removeAttr("onclick");

            $("html, body").animate({ scrollTop: 0 }, "fast");

            $("#carousel a").attr("onClick", "OnBackClick();");

            // Add blur effect on selected image on div1
            $('#divA' + obj).find('.img-bg').addClass('img-bgblur');
            $('#carousel').css('max-height', '262px');


            GrayBackgroundOnEdit();









        }


        // new webmethod calling


        var id;
        var name;
        var brandName;
        var retailer;
        var price;
        var imgurl;
        
        function BindSimilarGar(catid, colid, brnid, retid, db) {

            debugger;


            jQuery.ajax({
                url: 'look.aspx/BindSimilarItems',
                type: "POST",
                data: "{'categoryId':'" + catid + "', 'colorId':'" + colid + "','brandId':" + brnid + ",'retailerId':" + retid + ",'db':'" + db + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {


                    var myArray = data.d;

                   

                    if (myArray.length > 0) {

                        var htmlTop = '<li id="div1B1" class="view view-tenth"><a href="javascript:void(0)"><div class="imgd-bg"><div class="green-bg"><img src="assets/images/green-bg.png" alt="" /><div class="text2"><span>2</span><p>Hinge<br />Tanks &<br />Camisoles<br /></p><em>IN</em><p>Red</p></div></div></div></a></li>';
                        $(".Appenddivsim").append(htmlTop);
                        for (i = 1; i <= myArray[2].Value.length - 1; i++) {

                            //for (i = 0; i <= 8; i++) {

                            id = i;
                            name = myArray[2].Value[i].name;
                            brandName = myArray[2].Value[i].brandName;
                            retailer = myArray[2].Value[i].retailer;
                            price = myArray[2].Value[i].price;
                            imgurl = myArray[2].Value[i].images[0].url;


                            // alert(i);
                            AppendHTML(id, name, brandName, retailer, price, imgurl);

                        }

                    }

                    //alert('v');

                   
                    


                    

                    var carousel2 = $('#carousel2').elastislide();

                   






                }

            });

            
        }

        function AppendHTML(id, name, brandname, retailer, price, imgurl) {
            //alert(imgurl);
            

           
            var html = '<li class="view view-tenth" id="divB' + id + '"><a href="javascript:void(0)" onclick="return ShowButtonssimilar(' + id + ');"><div><img id="img1111" src="' + imgurl + '"></div><span class="hidedata1 hidetitle">' + name + '</span><p class="hidedata hide">' + brandname + '</p><p class="price hide Pink">Barneys New York</p><p class="price hide">$' + price + '</p><div class="border-dotted1 hide"></div></a><input class="DisplayImages1" id="ImgC' + id + '" type="image" src="assets/images/add-look.PNG" onclick="OpenNextPagesimilar(this);"/></li>';

            
            $(".Appenddivsim").append(html);

            



        }

       


        function OnBackClick() {
            window.location.href = "edit-look.html";
        }

        function GrayBackgroundOnEdit() {
            $('#divMainContainer').addClass('graybackground');
        }

        function CollapseCarousel() {
            $('.view-tenth').removeClass('test').css('border', '');
            $('.DisplayImages').css('display', 'none');
            $('.price').addClass('hide');
            $('.border-dotted1').css('display', 'none');

            $('#carousel').parent().removeClass('elastislide-carousel1');

            $('#carousel2').parent().removeClass('elastislide-carousel1');


            $('.view-tenth').find('.img-bg').removeClass('img-bg1');
            $('.view-tenth').removeClass('view1');
            $('#carousel').parent().parent().removeClass('elastislide-horizontal1');

            $('#carousel2').parent().parent().removeClass('elastislide-horizontal1');

            //$('#carousel').parent().removeClass().addClass('elastislide-carousel'); 
            $('#carousel').find('li').each(function () { $(this).attr("style", "min-height:''") })

            $('#carousel2').find('li').each(function () { $(this).attr("style", "min-height:''") })

            $('#divCommentsBlock').removeClass('grey_text');
            $('#divCollapseimage').removeClass('grey_text');

            return false;
        }



    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="backbutton">
        <img id="imgback" src="Images/newbackbutton.png" alt="" />
    </div>
    <div id="Div1" class="similarbbtn">
        <img id="img1" src="Images/up-arrow.png" onclick="OnBackClick()" alt="" />
    </div>
    <!--=== End Top ===-->
    <!--banner-->
    <!--block1-->
    <div class="container">
        <div id="_hideheader" class="margin-bottom-49 row-fluid">
            <!--left-->
            <div class="span8 re-style">
                <div class="posts margin-bottom-20 view-look">
                    <dl class="dl-horizontal">
                        <dt><a href="#">
                            <asp:Image ID="imgLookUser" runat="server" /></a></dt>
                        <dd>
                            <asp:Literal ID="lblLookUserName" runat="server"></asp:Literal>
                            <span>asked for your opinion </span>
                        </dd>
                    </dl>
                </div>
            </div>
            <div class="span2 icons">
                <a href="#">
                    <img src="assets/images/heart-icon.png"></a> <a href="#">
                        <img src="assets/images/share1.png"></a> <a href="#">
                            <img src="assets/images/restyle1.png"></a>
            </div>
        </div>
        <div class="row-fluid margin-bottom-0 border-dotted  main_div1" id="divfirst">
            <ul id="carousel" class="elastislide-list recent-work">
                <asp:Repeater ID="dlSingleLook" runat="server" OnItemDataBound="dlSingleLook_ItemDataBound">
                    <ItemTemplate>
                        <li id='divA<%# DataBinder.Eval(Container.DataItem, "id")%>' class="view view-tenth">
                            <a href="javascript:void(0)" onclick="return ShowButtons(<%# DataBinder.Eval(Container.DataItem, "id")%>);">
                                <div class="img-bg">
                                    <asp:Image ID="imgLook" runat="server" />
                                </div>
                                <div class="hide_txt">
                                    <span>
                                        <%# DataBinder.Eval(Container.DataItem, "name")%></span> <span>
                                            <%# DataBinder.Eval(Container.DataItem, "brandName")%></span>
                                    <p>
                                        Bobeau Ivory
                                    </p>
                                    <em>
                                        <%# DataBinder.Eval(Container.DataItem, "retailer")%></em>
                                    <p class="price hide">
                                        <%# DataBinder.Eval(Container.DataItem, "price")%>
                                    </p>
                                </div>
                                <div class="border-dotted1 hide">
                                </div>
                            </a>
                            <input class="DisplayImages" id='ImgA<%# DataBinder.Eval(Container.DataItem, "id")%>'
                                type="image" src="Images/AddtoCloset1.PNG" />
                            <a href="javascript:void(0);" 
                                onclick='OpenNextPage(<%# DataBinder.Eval(Container.DataItem, "id")%>,0);'>
                                <%--<img src='assets/images/findsimitems.PNG' />--%>

                                 <input onclick="return false;" id='ImgF<%# DataBinder.Eval(Container.DataItem, "id")%>' type="image" class="DisplayImages" src="assets/images/findsimitems.PNG" />
                            </a>
                           
                            <%-- <input class="DisplayImages" id='ImgF<%# DataBinder.Eval(Container.DataItem, "id")%>'
                                type="image" src="assets/images/findsimitems.PNG" onclick="return OpenNextPage(1, 0);" />--%>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <a href="" class="arrow1 DisplayImages" id="arrow" name="arrow">
                <img src="assets/images/arrow-bottom.PNG" />
            </a>
        </div>
        <div class="row-fluid margin-bottom-0 HideSimItem" id="similar_garments">
            <div class="button1">
                <span class="edit-look">34 items found. Select one to swap into this look</span>
            </div>
                        <ul id="carousel2" class="elastislide-lists recent-work Appenddivsim">
            </ul>


            <a href="" class="arrow1 DisplayImages" id="a1" name="arrow">
                <img src="assets/images/arrow-bottom.PNG" />
            </a>
        </div>
        <div id="divCommentsBlock" onclick="CollapseCarousel()" class="span10 grey-bg coment_hide">
            <div class="top">
                <div class="number">
                    <span>
                        <asp:Literal ID="lblLoveCount" runat="server"></asp:Literal></span> <span>
                            <asp:Literal ID="lblRestyleCount" runat="server"></asp:Literal></span> <span>4 Comment</span>
                    <span>4 share</span>
                </div>
                <div class="clearfix">
                </div>
                <ul class="loginbar comment-block1">
                    <li><a href="#" class="love active">Love</a></li>
                    <li><a href="#" class="re-style1">Restyle</a></li>
                    <li><a href="#" class="comment1">comment</a></li>
                    <li><a href="#" class="share">share</a></li>
                </ul>
            </div>
        </div>
    </div>
    <!--block2-->
    <div id="divCollapseimage" onclick="CollapseCarousel()">
        <div id="divbottomtags" class="container border-top coment_hide">
            <div class="margin-bottom-49 row-fluid">
                <div class="span7 re-style">
                    <!-- left -->
                    <div class="posts margin-bottom-20 com">
                        <dl class="dl-horizontal">
                            <h5 class="txt mar-bottom11">
                                <asp:Literal ID="lblLookTitle" runat="server"></asp:Literal>
                                <span class="div7">#summerstyle Jenny Slade Sandy Jae?,</span>
                            </h5>
                            <dt><a href="#">
                                <img src="assets/images/thumb9.png" alt="" /></a></dt>
                            <dd>Sarah K
                                <p class="text1">
                                    <a href="#">pretty freaking gorgeous, lady!</a>
                                </p>
                            </dd>
                        </dl>
                        <dl class="dl-horizontal">
                            <dt><a href="#">
                                <img src="assets/images/thumb10.png" alt="" /></a></dt>
                            <dd>Rose Summerfield
                                <p class="text1">
                                    <a href="#">you're on a real kick with the pink this month, aren't you? :-) Ok, you
                                        have me convinced. I need a pair!</a>
                                </p>
                            </dd>
                        </dl>
                    </div>
                </div>
                <!--right-->
                <div class="span3 btns" id="restyle">
                    <asp:Repeater ID="dlTags" runat="server">
                        <ItemTemplate>
                            <button class="btn-u" type="button">
                                <%# DataBinder.Eval(Container.DataItem, "name")%></button>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript" src="assets/js/modernizr.custom.js"></script>
    <script type="text/javascript" src="assets/plugins/bootstrap/js/bootstrap.min.js"></script>
    <!-- JS Implementing Plugins -->
    <script type="text/javascript" src="assets/plugins/flexslider/jquery.flexslider-min.js"></script>
    <!--/*<script type="text/javascript" src="assets/plugins/horizontal-parallax/js/sequence.jquery-min.js"></script>
<script type="text/javascript" src="assets/plugins/horizontal-parallax/js/horizontal-parallax.js"></script>*/-->
    <!--<script type="text/javascript" src="assets/plugins/bxslider/jquery.bxslider.js"></script>
-->
    <!-- JS Page Level -->
    <script type="text/javascript" src="assets/js/app.js"></script>
    <script src="assets/js/jquery-1.8.2.min.js"></script>
    <script src="assets/js/modernizr.custom.17475.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
    <script type="text/javascript" src="assets/js/jquerypp.custom.js"></script>
    <script type="text/javascript" src="assets/js/jquery.elastislide.js"></script>
    <script type="text/javascript">



        $('#carousel').elastislide();
        
        var carousel2 = $('#carousel2').elastislide();

       

    </script>
    <!--[if lt IE 9]>
    <script src="assets/js/respond.js"></script>
<![endif]-->
    <!-- metadata for page -->
    <asp:Label ID="LookId" runat="server" Style="display: none" />
    <asp:Label ID="P1Id" runat="server" Style="display: none;" />
    <asp:Label ID="P2Id" runat="server" Style="display: none;" />
    <asp:Label ID="P3Id" runat="server" Style="display: none;" />
    <asp:Label ID="UpVote" runat="server" Style="display: none;" />
    <asp:Label ID="DownVote" runat="server" Style="display: none;" />
    <asp:Label ID="UserId" runat="server" Style="display: none;" />
    <asp:Label ID="UserShare" runat="server" Style="display: none;" />
    <asp:Label ID="Unsigned" runat="server" Style="display: none;" />
    <asp:Label ID="CreatorId" runat="server" Style="display: none;" />
    <!--js for the page -->
    <hr />
</asp:Content>

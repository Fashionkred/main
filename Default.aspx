<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true"
    Debug="true" CodeFile="Default.aspx.cs" Inherits="ShopSenseDemo._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

  <!-- CSS Implementing Plugins -->
  <link rel="stylesheet" href="plugins/font-awesome/css/font-awesome.css"/>
    <!-- CSS Theme -->
    <link rel="stylesheet" href="Styles/responsiveslides.css"/>
    <link rel="stylesheet" href="Styles/demo.css"/>

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

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
                                    <li class="active yellow"><a href="#" class="dropdown-toggle" data-toggle="dropdown">
                                        Show<br />
                                        All looks </a></li>
                                    <li class="green"><a href="" class="dropdown-toggle" data-toggle="dropdown">Create<br />
                                        a Look</a> </li>
                                    <li class="pink">
                                       <a href="user.aspx" title="Settings"><asp:Image ID="UserImage" CssClass="rounded avatar" runat="server" /></a>
                                       <asp:HyperLink ID="LogInLink" runat="server" Visible="false" Style="margin-right:8px;"
                                       ></asp:HyperLink>
                                        <asp:HyperLink ID="UserName" class="dropdown-toggle" data-toggle="dropdown" runat="server" />
                                          <asp:Label ID="UserPoints" runat="server" /></li>
                                    <li class="comments"><a class="search"><i class="search-btn icon-remove">3</i> Notifications
                                    </a></li>
                                </ul>
                                <div class="search-open">
                                    <div class="input-append">
                                        <div class="span3">
                                            <div class="posts margin-bottom-20">
                                                <dl class="dl-horizontal">
                                                    <dt><a href="#">
                                                        <img src="Images/thumb2.png" alt="" /></a></dt>
                                                    <dd>
                                                        Sofia C
                                                        <p>
                                                            <a href="#">re-styled your look</a></p>
                                                    </dd>
                                                </dl>
                                                <dl class="dl-horizontal">
                                                    <dt><a href="#">
                                                        <img src="Images/thumb2.png" alt="" /></a></dt>
                                                    <dd>
                                                        Laura Win
                                                        <p>
                                                            <a href="#">mentioned you</a></p>
                                                    </dd>
                                                </dl>
                                                <dl class="dl-horizontal">
                                                    <dt><a href="#">
                                                        <img src="Images/thumb2.png" alt="" /></a></dt>
                                                    <dd>
                                                        Steve Mars
                                                        <p>
                                                            <a href="#">mentioned you</a></p>
                                                    </dd>
                                                </dl>
                                                <dl class="dl-horizontal">
                                                    <dt><a href="#">
                                                        <img src="Images/thumb2.png" alt="" /></a></dt>
                                                    <dd>
                                                        Marianne Grosz
                                                        <p>
                                                            <a href="#">created a new collection</a></p>
                                                    </dd>
                                                </dl>
                                                <dl class="dl-horizontal">
                                                    <dt><a href="#">
                                                        <img src="Images/thumb2.png" alt="" /></a></dt>
                                                    <dd>
                                                        Celia Costanza
                                                        <p>
                                                            <a href="#">commented on your look</a></p>
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
                                    Cras justo odio, dapibus ac facilisis in, egestas.</p>
                            </div>
                        </div>
                        <div class="item">
                            <img src="Images/colllection-img.png" alt="">
                            <div class="carousel-caption">
                                <p>
                                    Cras justo odio, dapibus ac facilisis in, egestas.</p>
                            </div>
                        </div>
                        <div class="item">
                            <img src="Images/fashion_week.png" alt="">
                            <div class="carousel-caption">
                                <p>
                                    Cras justo odio, dapibus ac facilisis in, egestas.</p>
                            </div>
                        </div>
                    </div>
                    <div class="carousel-arrow">
                        <a class="left carousel-control" href="#myCarousel" data-slide="prev"><i class="icon-angle-left">
                        </i></a><a class="right carousel-control" href="#myCarousel" data-slide="next"><i
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
                        <dd>
                            Sofia C
                            <p>
                                <a href="#">re-styled your look</a></p>
                        </dd>
                    </dl>
                    <dl class="dl-horizontal">
                        <dt><a href="#">
                            <img src="Images/thumb2.png" alt="" /></a></dt>
                        <dd>
                            Laura Win
                            <p>
                                <a href="#">mentioned you</a></p>
                        </dd>
                    </dl>
                    <dl class="dl-horizontal">
                        <dt><a href="#">
                            <img src="Images/thumb2.png" alt="" /></a></dt>
                        <dd>
                            Steve Mars
                            <p>
                                <a href="#">mentioned you</a></p>
                        </dd>
                    </dl>
                    <dl class="dl-horizontal">
                        <dt><a href="#">
                            <img src="Images/thumb2.png" alt="" /></a></dt>
                        <dd>
                            Marianne Grosz
                            <p>
                                <a href="#">created a new collection</a></p>
                        </dd>
                    </dl>
                    <dl class="dl-horizontal">
                        <dt><a href="#">
                            <img src="Images/thumb2.png" alt="" /></a></dt>
                        <dd>
                            Celia Costanza
                            <p>
                                <a href="#">commented on your look</a></p>
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
    <asp:Repeater id="dlLook" runat="server" OnItemDataBound="dlLook_ItemDataBound">    
    <ItemTemplate>    
    
    <div class="container border-top">
        <div class="margin-bottom-49 row-fluid">
            <!--left-->
            <div class="span3 re-style">
                <div class="posts margin-bottom-20">
                    <dl class="dl-horizontal">
                        <dt><a href="profile.aspx">
                            <img src='<%# DataBinder.Eval(Container.DataItem, "creator.pic")%>' alt="" /></a></dt>
                        <dd>
                            <p>
                                <a href="profile.aspx"><%# DataBinder.Eval(Container.DataItem, "originalLookId").ToString() == "0" ? "styled by" : "re-styled by"%> </a></p>
                            <%# DataBinder.Eval(Container.DataItem, "creator.name")%>
                        </dd>
                        <h5>
                           <%# DataBinder.Eval(Container.DataItem, "title")%> </span></h5>
                    </dl>
                </div>
                <div class="span9" id="restyle">
                 <asp:Repeater ID="dlTags" runat="server" >
                <ItemTemplate>
                    <button class="btn-u" type="button">
                        <%# DataBinder.Eval(Container.DataItem, "name")%></button>                    
                </div>
                 </ItemTemplate>
                    </asp:Repeater>
            </div>
            <!--right-->
            <div class="span9 re-style">
                <div class="view_col">
                <asp:Repeater ID="dlInnerLook" runat="server" OnItemDataBound="dlInnerLook_ItemDataBound">
                <ItemTemplate>
                
                    <div id="divimgLook" runat="server" >
                    <a href='look.aspx?lid=<%# DataBinder.Eval(Container.Parent.Parent, "DataItem.id")%>' >
                   <asp:Image ID="imgLook" runat="server" class="mar-bottom11" /></a> 
                    </div>
                    </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="span10 grey-bg">
                    <div class="top">
                        <div class="number">
                            <span><%# DataBinder.Eval(Container.DataItem, "upvote")%> Love</span> <span><%# DataBinder.Eval(Container.DataItem, "restylecount")%> Restyle</span> <span><%# DataBinder.Eval(Container.DataItem, "upvote")%> Comment</span> <span><%# DataBinder.Eval(Container.DataItem, "viewcount")%> share</span>
                        </div>
                        <div class="clearfix">
                        </div>
                        <ul class="loginbar comment-block1">
                        <li><a href="#" class='<%# DataBinder.Eval(Container.DataItem, "isLoved").ToString() == "False" ? "love" : "love active"%>'>Love</a></li>                            
                            <li><a href="#" class="re-style1">Restyle</a></li>
                            <li><a href="#" class="comment1">comment</a></li>
                            <li><a href="#" class="share">share</a></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </ItemTemplate>
   </asp:Repeater>
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
    <script type="text/javascript">
        jQuery(document).ready(function () {
            App.init();
            App.initSliders();
            Index.initParallaxSlider();
        });
    </script>
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
        $(document).ready(function () {
            var qStr = GetQueryStringParams('sp');
            $(document).scrollTop(qStr);
            $(".mar-bottom11").click(function () {
                var s = $(document).scrollTop();
                window.location.href = "view-look1.html?sp=" + s.toString();
            });
        });



    </script>
    <!--[if lt IE 9]>
    <script src="assets/js/respond.js"></script>
<![endif]-->





    <div id="overlay_signin" style="display: none">
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
                    Vote on looks or create your own! Earn 10% commission on purchases from your looks!</div>
                <a href="default.aspx?login=1">
                    <div class="fb-custom">
                        <span style="padding-left: 20px; line-height: 48px;">Signup With Facebook</span></div>
                </a>
                <div style="text-align: center;">
                    Already a member?&nbsp;<a href="default.aspx?login=1"><span>Sign In</span></a></div>
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
            SetSiginPopup();
            SetHPLoveButtons();

        });
    
    </script>
</asp:Content>

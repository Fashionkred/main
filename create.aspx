<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="create.aspx.cs" Inherits="create" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="Server">
    <%--<script> !function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0]; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = "https://platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } } (document, "script", "twitter-wjs");</script> 
    --%>
    <%--<script type="text/javascript">
    (function (d, buildThese) {
        var homeScript, newScript, n = buildThese.length, i;
        for (i = 0; i < n; i = i + 1) {
            newScript = d.createElement('SCRIPT');
            newScript.type = 'text/javascript';
            newScript.async = true;
            newScript.src = buildThese[i];
            homeScript = d.getElementsByTagName('SCRIPT')[0];
            homeScript.parentNode.insertBefore(newScript, homeScript);
        }
    } (document, [
    '//assets.pinterest.com/js/pinit.js'
    /* load more third-party JavaScript here */
  ])
  );
</script>--%>
<script src="http://platform.tumblr.com/v1/share.js"></script>

<div>
    <div class="create-success">
        <div class="image-success">
            <img src="images/create_added.jpg">
        </div>
        <div class="content-success">
            Create more looks or go back to <a href="look.aspx">voting!</a>
        </div>
    </div>
    <%--<div class="create-body-wrapper">
       <div class="combine-3-button">
				<div id="content-create-look">
					<div style="color: #e21a2c;"><strong>TO COMBINE 3 ITEMS</strong></div></br>
					click here
				</div>
		</div>
</div>--%>
    <div class="items-wrapper">
        <div class="more-item-wrapper">
            <div class="gallery-create">
                <ul id="sliderCreate" runat="server" clientidmode="Static">
                    <li>
                        <div class="create-big-shape">
                            <div class="create-content-text">
                                <strong>SELECT A DRESS</strong>
                                <br />
                                FROM FAVORITES</div>
                        </div>
                    </li>
                </ul>
            </div>
            
            <div class="buttons-create-wrapper">
				<div class="buttons-create-disabled" id="buttons-create-prev">
					<img src="images/left_arrow_button.png" alt="Previous">
				</div>   

				<div class="buttons-create" id="buttons-create-next">
					<img src="images/right_arrow_button.png" alt="Next">
				</div>
			</div>
        </div>
        <div class="plus-content-3items">
            +</div>
        <div class="left-item-wrapper">
            <div class="gallery-create-small">
                <ul id="sliderCreate2" runat="server" clientidmode="Static" >
                    <li>
                        <div class="create-small-shape">
                            <div class="create-content-small-text">
                                <strong>SELECT A HANDBAG</strong>
                                <br />
                                FROM FAVORITES</div>
                        </div>
                    </li>
                </ul>
            </div>
            
            <div class="buttons-create-wrapper">
				<div class="buttons-create-disabled" id="buttons-create-prev-02">
					<img src="images/left_arrow_button.png" alt="Previous">
				</div>   

				<div class="buttons-create" id="buttons-create-next-02">
					<img src="images/right_arrow_button.png" alt="Next">
				</div>
			</div>

            <div class="gallery-create-small">
                <ul id="sliderCreate3" runat="server" clientidmode="Static" >
                    <li>
                        <div class="create-small-shape">
                            <div class="create-content-small-text">
                                <strong>SELECT A SHOE</strong>
                                <br />
                                FROM FAVORITES</div>
                        </div>
                    </li>
                </ul>
            </div>
            
            <div class="buttons-create-wrapper">
				<div class="buttons-create-disabled" id="buttons-create-prev-03">
					<img src="images/left_arrow_button.png" alt="Previous">
				</div>   

				<div class="buttons-create" id="buttons-create-next-03">
					<img src="images/right_arrow_button.png" alt="Next">
				</div>
			</div>
        </div>
    </div>

    <div class="button-wrapper">
        <center>
            <span><strong>Title:  </strong></span>
            <input id="title" type="text" style="width:400px;" />
            <span><strong>Tags:  </strong></span>
            <input id="tags" type="text" style="width:200px;" />
        </center>
    </div>
    <div class="buttons-wrapper">
        <div class="create-cant-finish">
            <div class="text-button-finish">
                    <div style="font-size: 54px;">
                        <strong>CAN'T FINISH</strong>
                    </div>
            <br/> YOU NEED TO ADD AT LEAST TWO ITEMS FROM FAVORITES BELOW.
            </div>
        </div>
        <div class="create-finish" id="FinishButton">
            <div class="text-button-finish">
                    <div style="font-size: 54px;">
                        <strong>FINISH</strong>
                    </div>
                    </br> AND ASK YOUR FRIENDS.
            </div>
        </div>
    </div>

  </div>  

    <asp:Panel id="Favorites" runat="server" CssClass="favs">
        <div class="favorites">
            FAVORITES:
        </div>
    </asp:Panel>
    
    <div id="create-share-popup" style="display: none;">
        <%--<div id="blue-tick">
            <img src="images/create_share_tick.jpg">
        </div>--%>
        <div id="blue-text-popup">
            Share your look
        </div>
        <div id="gray-text-popup">
            Earn 10% commission on purchases from your sets!
        </div>
        <%--<div id="share-link-wrapper">
		<div id="share-input">
			<div id="icon-share-palnet">
				<img src="images/planet_share_image.jpg">
			</div>

			<div id="share-link-text">
				http://goo.gl/e1B4T
			</div>
		</div>
	</div>
        --%>
        <div id="share-text-wrapper-left">
            <img alt="look" id="outfit" style="height: 265px;" />
            <br />
        </div>
        <div id="share-text-wrapper-right">
            
            <div id="pinterest" class="switch btnS">
                <a class="OutBoundLink" target="_blank" href="//pinterest.com/pin/create/button/?url=http%3A%3A%2F%2Ffashionkred.com&media=http%3A%2F%2Ffashionkred.com%2Flook.aspx%3Flid%3D36&description=Check%20out%20my%20outfit%20%40fashionkred!">
                    <img src="images/pinterest_pin-it_icon.png" /></a>
            </div>
            <div class="switch btnS">
                <a id="fb" class="OutBoundLink" href="javascript:fbShare();">
                    <img src="images/fb-share-button.png" alt="Share on facebook" />
                </a>
            </div>
            <div id="twitter" class="switch btnS">
                <a class="OutBoundLink" target="_blank" href="https://twitter.com/intent/tweet?hashtags=nordstrom%2Coutfit%2Cfashionkred&related=nordstorm&text=Check%20out%20my%20outfit%20%40fashionkred!&tw_p=tweetbutton&url=">
                    <img src="images/tweet-button.png" alt="Tweet" title="Share a tweet with your followers" /></a>
            </div>
            
            
            <div id="tumblr" class="switch btnS">
                <a class="OutBoundLink" target="_blank" href="http://www.tumblr.com/share/photo?source=http%3A%3A%2F%2Ffashionkred.com&caption=what%20do%20you%20think%20of%20the%20outfit%20I%20created%20at%20FashionKred%&clickthru=" title="Share on Tumblr" style="display:inline-block; text-indent:-9999px; overflow:hidden; width:81px; height:20px; background:url('http://platform.tumblr.com/v1/share_1.png') top left no-repeat transparent;">Share on Tumblr</a>
            </div>
            <div class="switch btnS">
                <a class="OutBoundLink" id="email" href="mailto:?subject=Check%20out%20my%20outfit%20at%20FashionKred&body=what%20do%20you%20think%20of%20the%20outfit%20I%20created%20at%20FashionKred%3F%20Check%20it%20out%20here%0A%0A">
                    <img src="images/icon_sendmail.png" alt="Send mail to a friend" title="Send mail to a friend" />Send
                    Mail</a>
            </div>
        </div>
        <%--		<div id="share-text-wrapper-right">
			
				
			<div class="switch btnS">
					<input type="checkbox" checked>
					<label><i></i></label>
		    </div>

		    <div class="switch btnS">
					<input type="checkbox">
					<label><i></i></label>
		    </div>
		</div>--%>
        <div id="share-button-wrapper-done">
            <div id="submit-button-done">
                <div id="tick-white">
                    <img src="images/tick_white.png">
                </div>
                <div id="submit-button-done-text">
                    Done!
                </div>
            </div>
        </div>
    </div>
    <!-- metadata for the page -->
    <asp:label id="LookId" runat="server" style="display: none" />
    <asp:label id="P1Id" runat="server" style="display: none;" />
    <asp:label id="P2Id" runat="server" style="display: none;" />
    <asp:label id="P3Id" runat="server" style="display: none;" />
    <asp:label id="P1Color" runat="server" Text="Clear" style="display: none;" />
    <asp:label id="P2Color" runat="server" Text="Clear" style="display: none;" />
    <asp:label id="P3Color" runat="server" Text="Clear" style="display: none;" />
    <asp:label id="P1Cat" runat="server"  style="display: none;" />
    <asp:label id="P2Cat" runat="server" style="display: none;" />
    <asp:label id="P3Cat" runat="server"  style="display: none;" />
    <asp:label id="CoverPdt" runat="server" style="display: none;" />
    <asp:label id="UserId" runat="server" style="display: none;" />
    <asp:Label ID="UserShare" runat="server" Style="display: none;" />
    <asp:Label ID="OriginalLook" runat="server" Style="display: none;" />
    <!--js for the page -->
    <script type="text/javascript">

        $(document).ready(function () {
            SetFinishButton();
            
        });
    
    </script>
</asp:Content>

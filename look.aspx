<%@ Page Language="C#" AutoEventWireup="true" CodeFile="look.aspx.cs" Inherits="Outfit" Debug="true"
    MasterPageFile="~/Site.master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <div class="body-wrapper">
            <asp:Panel ID="UserImage" runat="server" CssClass="user-body-wrapper">
				<asp:Image ID="CreatorImage"  CssClass="rounded avatar" runat="server" />
			</asp:Panel>
            
            <asp:Panel ID="BackButton" runat="server" Visible="false" CssClass="button-wrapper">
				<a href="look.aspx" class="button-back"><img src="images/left_arrow_button.png" title="Go back"></a>
			</asp:Panel>

			<div class="user-body">
				<strong><asp:HyperLink ID="CreatorName" runat="server" Style="color:#333;"/></strong>
                <br />
				<asp:Panel ID="SubscribePanel" runat="server" CssClass="follow-button">
				    <asp:Label ID="Subscribe" runat="server" Text="Follow" />
			    </asp:Panel>
                
			</div>
            
            <asp:Panel ID="VotePanel" runat="server" Visible="false" CssClass="vote-msg">
				<strong><asp:Label ID="VoteMsg" runat="server"/></strong>
			</asp:Panel>
            
           
            
            <asp:Panel ID="CreateLook" runat="server" CssClass="content-create-look">
             <asp:Panel ID="SharePanel" runat="server">
             <div class="share-btn" style="padding-right: 6px;">Share</div>
				 <div id="pinterest" class="share-btn">
                    <a class="OutBoundLink" target="_blank" href="//pinterest.com/pin/create/button/?url=http%3A%3A%2F%2Ffashionkred.com&media=http%3A%2F%2Ffashionkred.com%2Flook.aspx%3Flid%3D36&description=Check%20out%20my%20outfit%20%40fashionkred!">
                        <img src="images/pinterest_16.png" alt="Pin It" title="Pin on Pinterest" /></a>
                </div>
                <div class="share-btn">
                    <a class="OutBoundLink" id="fb" target="_blank" href="javascript:fbShare();">
                        <img src="images/facebook_16.png" alt="Share on facebook" title="Share on Facebook" />
                    </a>
                </div>
                <div id="tumblr" class="share-btn">
                    <a class="OutBoundLink" target="_blank" href="http://www.tumblr.com/share/photo?source=http%3A%3A%2F%2Ffashionkred.com&caption=what%20do%20you%20think%20of%20the%20outfit%20I%20created%20at%20FashionKred%&clickthru=" title="Share on Tumblr" style="display:inline-block; text-indent:-9999px; overflow:hidden; width:16px; height:16px; background:url('images/tumblr_16.png') top left no-repeat transparent;"></a>
                </div>
                <div class="share-btn">
                    <a class="OutBoundLink" id="email" target="_blank" href="mailto:?subject=Check%20out%20my%20outfit%20at%20FashionKred&body=what%20do%20you%20think%20of%20the%20outfit%20I%20created%20at%20FashionKred%3F%20Check%20it%20out%20here%0A%0A">
                        <img src="images/email_16.png" alt="Send mail to a friend" title="Send mail to a friend" /></a>
                </div>
			</asp:Panel>
            
            <a href="create.aspx">
				<hr /><div style="color: #e21a2c; margin-top: -14px;"><strong>CREATE YOUR LOOK</strong></div>
		    </a>
	</asp:Panel>
            <asp:Panel ID="ShareButton" runat="Server" Visible="false" CssClass="share-button">
				<a href="javascript:fbShare();"><div id="button-share-content">
					<strong>SHARE IT</strong>
				</div></a>
			</asp:Panel>
</div>        
    <br />
   <asp:Panel ID="gallery" runat="server" CssClass="gallery" clientidmode="Static">
	<ul id="slider">

    <li>
	<div class="items-wrapper">
		<asp:Panel ID="Left" runat="server" CssClass="left-item-wrapper">
			<asp:HyperLink ID="P1Title" runat="server" Target="_blank" CssClass="OutBoundLink">
                <div class="product-image">
                        <asp:Image ID="P1Image" runat="server" />
                   
                </div>
            </asp:HyperLink>
			<br/>

            <asp:Panel ID="P1LoveButton" runat="server" CssClass = "text-heart"> 
                                       
			 <asp:Label ID="P1Love" runat="server"/> 
			 <asp:Image ID="P1LoveImg" runat="server" Style="vertical-align: middle; width: 24px;" ImageUrl="images/heart_empty.jpg" Title="Love It" Alt="Love it" />
			 <asp:HyperLink ID="P1BuyButton" Target="_blank" runat="server" Text="Buy" CssClass="buylink"/>
            </asp:Panel>
            
            
			<div class="item-content">
				<%--<asp:HyperLink ID="P1Brand" runat="server" CssClass="brandTitle OutBoundLink" Target="_blank" />--%>
				<div style="color: #e21a2c;"><asp:HyperLink ID="P1Label" runat="server" CssClass="pdtTitle OutBoundLink" Target="_blank" /></div>
			</div>
			<div class="price-content">
				<asp:Label ID="P1Price" runat="server" CssClass="inline" />
                <asp:Label ID="P1SalePrice" runat="server" CssClass="inline hide" />
			</div>
		</asp:Panel>

		<asp:Panel ID="Plus" runat="server" CssClass="plus-content">+</asp:Panel>

        <asp:Panel ID="RightPanel" runat="server" CssClass="right-item-wrapper">
        <center>
		    <asp:Panel ID="RightUpper" runat="server">
			    <asp:HyperLink ID="P2Title" runat="server" Target="_blank" CssClass="OutBoundLink">
                    <asp:Panel runat="server" ID="P2ImageDiv" CssClass="product-image">
                    
                            <asp:Image ID="P2Image" runat="server" />
                    </asp:Panel>
                </asp:HyperLink>
			    <br/>

                <asp:Panel ID="P2LoveButton" runat="server" CssClass="text-heart">
                    <asp:Label ID="P2Love" runat="server" />
				    <asp:Image ID="P2LoveImg" Style="vertical-align: middle; width:24px;" ImageUrl="images/heart_empty.jpg" Title="Love it" AlternateText="Love it" runat="server" />
                    <asp:HyperLink ID="P2BuyButton" Target="_blank" runat="server" Text="Buy" CssClass="buylink"/>
            </asp:Panel>

			    <div class="item-content">
				    <%--<asp:HyperLink ID="P2Brand" runat="server" CssClass="brandTitle OutBoundLink" Target="_blank" />--%>
				    <div style="color: #e21a2c;"><asp:HyperLink ID="P2Label" runat="server" CssClass="pdtTitle OutBoundLink" Target="_blank"/></div>
			    </div>
			    <div class="price-content">
				    <asp:Label ID="P2Price" runat="server" CssClass="inline"/>
                    <asp:Label ID="P2SalePrice" runat="server" CssClass="inline hide" />
			    </div>

		    </asp:Panel>
            <asp:Panel ID="RightLower" runat="server" CssClass="" Visible="false">
			    <asp:HyperLink ID="P3Title" runat="server" Target="_blank" CssClass="OutBoundLink">
                    <div class="product-small-image">
                    
                            <asp:Image ID="P3Image" runat="server" />
                    </div>
                </asp:HyperLink>
			    <br/>

                <asp:Panel ID="P3LoveButton" runat="server" CssClass="text-heart">
                    <asp:Label ID="P3Love" runat="server" />
				    <asp:Image ID="P3LoveImg" Style="vertical-align: middle;width:24px;" ImageUrl="images/heart_empty.jpg" Title="Love it" AlternateText="Love it" runat="server" />
                    <asp:HyperLink ID="P3BuyButton" Target="_blank" runat="server" Text="Buy" CssClass="buylink"/>
                </asp:Panel>

			    <div class="item-content">
				    <%--<asp:HyperLink ID="P3Brand" runat="server" CssClass="brandTitle OutBoundLink" Target="_blank" />--%>
				    <div style="color: #e21a2c;"><asp:HyperLink ID="P3Label" runat="server" CssClass="pdtTitle OutBoundLink" Target="_blank"/></div>
			    </div>
			    <div class="price-content">
				    <asp:Label ID="P3Price" runat="server" CssClass="inline"/>
                    <asp:Label ID="P3SalePrice" runat="server" CssClass="inline hide" />
			    </div>

		    </asp:Panel>
         </center>
        </asp:Panel>
	</div>
    </li>
    </ul>
  </asp:Panel>  

  <div class="buttons-wrapper">
		<asp:Panel ID="LeftButtonDisabled" runat="server" CssClass="left-button-disabled">
			<asp:Panel ID="LeftTextButtonDisabled" runat="server" CssClass="text-button-disabled"></asp:Panel>
		</asp:Panel>
		<asp:Panel ID="RightButtonDisabled" runat="Server" CssClass="right-button-disabled">
			<asp:Panel ID="RightTextButtonDisabled" runat="server" CssClass="text-button-disabled"></asp:Panel>
		</asp:Panel>
		<asp:Panel ID="LeftButton" runat="server" CssClass="left-button">
			<div class="text-button-left">MATCH</div>
		</asp:Panel>
		<asp:Panel ID="RightButton" runat="server" CssClass="right-button">
			<div class="text-button-right">NO MATCH</div>
		</asp:Panel>
        
        <asp:Panel ID="SkipButton" runat="server" CssClass="skip-button">
			<div class="text-button-skip">SKIP</div>
	</asp:Panel>
	</div>
    

 <div class="fav">
	<div class="favorites">
		FAVORITES:
	</div>

	<div class="carousel">
		<div class="prev-carousel">
			<img src="images/carousel_left.png" alt="Previous">
		</div>
		<div class="next-carousel">
			<img src="images/carousel_right.png" alt="Next">
		</div>
		<div class="carousel-favorites">
            <asp:Panel ID="Favorites" runat="server" >
            </asp:Panel>
		</div>
	</div>
    
	<div class="comments-text">
		<div style="color: #e21a2c;"><strong>WHAT DO YOU THINK?</strong></div><br/>
		ADD YOUR COMMENT
        <div class="facebook-comments">
            <div id="fbCommentDiv" />
        </div>
	</div>
</div>

<div id="overlay_form" style="display:none">
	<div class="close">
		<a href="#"><img src="images/close_popup.png" alt="Close"></a>
	</div>
	<div class="img-popup">
		<img alt="favorites" src="images/hearts.png" />
	</div>
	<div class="text-popup">
		5 outfits voted in a row - Congratulations! <br />
		Why don’t you create your own outfit <br /> 
		and ask your friends?
	</div>
	<div class="img-popup-2">
		<img src="images/shape_popup.png" />
	</div>

    <a href="create.aspx">
	    <div class="popup-button">
		    Create an Outfit
	    </div>
    </a>
</div>
<div id="overlay_signin" style="display:none">
	<div class="close">
		<a href="#"><img src="images/close_popup.png" alt="Close"></a>
	</div>
	<div class="login-body">
        <div class="left-pane">
            <img src="images/nordstromlook.jpg" alt="Earn commission on sales!" style="line-height:20px;" /> 
        </div>
        <div class="right-pane">
            <div class="contest-call">Vote on looks or create your own! Earn 10% commission on purchases from your looks!</div>
            
            <a href="/?login=1"><div class="fb-custom"><span style="padding-left:20px; line-height:48px;">Signup With Facebook</span></div></a>
            <div style="text-align:center;">Already a member?&nbsp;<a href="/?login=1"><span>Sign In</span></a></div>
        </div>
    
    </div>
</div>
   
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
    <script type="text/javascript">

        $(document).ready(function () {
            SetVoteButtons();
            SetLoveButtons();
            SetSiginPopup();
            SetSubscribeButton();
            TrackOutBoundLinks();
            //last 
            SetCommentBox();
            
        });
    
    </script>
    <hr />
</asp:Content>

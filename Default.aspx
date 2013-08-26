<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    Debug="true" CodeFile="Default.aspx.cs" Inherits="ShopSenseDemo._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Panel ID="LookContent" runat="server" Style="padding-top:40px;"/>

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
            
            <a href="default.aspx?login=1"><div class="fb-custom"><span style="padding-left:20px; line-height:48px;">Signup With Facebook</span></div></a>
            <div style="text-align:center;">Already a member?&nbsp;<a href="default.aspx?login=1"><span>Sign In</span></a></div>
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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="user.aspx.cs" Inherits="user" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="UserProfileHeader">
        <div class="UserProfileImage">
            <asp:Image ID="UserImage" runat="server" Height="153px" Width="153px" />
        </div>
        <div class="UserInformation">
            <h2 class="UserName"> <asp:Label ID="UserName" runat="server" /></h2>
            <asp:Panel ID="UserLinks" CssClass="UserLinks" runat="server"></asp:Panel>
            <asp:Panel ID="SubscribePanel" runat="server" CssClass="follow-button" Style="margin-top:20px; font-family:Trebuchet MS;" >
				    <asp:Label ID="Subscribe" runat="server" Text="Follow" />
			</asp:Panel>
        </div>
    </div>

    <div class="UserInfoBar">
        <ul class="UserStats">
            <li><asp:HyperLink ID="SetsLink" runat="server" /></li>
            <li><asp:HyperLink ID="LovesLink" runat="server" /></li>
        </ul>
        <ul class="followersFollowingLinks">
            <li><asp:HyperLink ID="FollowingLink" runat="server" /></li>
            <li><asp:HyperLink ID="FollowersLink" runat="server" /></li>
        </ul>
    </div>

    <asp:Panel ID="UserProfileContent" runat="server" />

    <!--metadata for the page -->
    <asp:Label ID="CreatorId" runat="server" Style="display: none;" />
    <asp:Label ID="UserId" runat="server" Style="display: none;" />
   <asp:Label ID="UserShare" runat="server" Style="display: none;" />

    <!--js for the page -->
    <script type="text/javascript">

        $(document).ready(function () {
            SetHPLoveButtons();
            SetSubscribeButton();
        });
    
    </script>
</asp:Content>

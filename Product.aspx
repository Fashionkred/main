<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Product.aspx.cs" Inherits="ProductPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <div id="fb-root">
    </div>
    <script>
        window.fbAsyncInit = function () {
            FB.init({
                appId: '155821997899161',
                channelUrl: '//FashionKred/channel.html',
                frictionlessRequests: true,
                status: true, // check login status
                cookie: true, // enable cookies to allow the server to access the session
                xfbml: true  // parse XFBML
            });
        };

        (function (d, debug) {
            var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
            if (d.getElementById(id)) { return; }
            js = d.createElement('script'); js.id = id; js.async = true;
            js.src = "//connect.facebook.net/en_US/all" + (debug ? "/debug" : "") + ".js#xfbml=1";
            ref.parentNode.insertBefore(js, ref);

        } (document, /*debug*/false));
    </script>
    <center>
        <h1>
            <strong>
                <asp:Label ID="BrandName" runat="server" /></strong><br />
        </h1>
        <hr />
        <h2>
            <asp:Label ID="ProductName" runat="server" /></h2>
        <br />
        <br />
        <table>
            <tr>
                <td>
                    <asp:Image ID="ProductImg" runat="server" Height="410px" Width="328px" />
                </td>
                <td>
                    <asp:Label ID="ProductDesc" runat="server" /><br />
                    <br />
                    <asp:Label ID="Colors" runat="server" /><br />
                    <asp:Label ID="Sizes" runat="server" /><br />
                    <strong>Price: </strong><asp:Label ID="Price" runat="server" />&nbsp;<asp:Label ID="SalePrice" Visible="false"
                        runat="server" /><br />
                    <asp:Label ID="Retailer" runat="server" /><br />
                    <br />
                    <asp:HyperLink ID="LoveLink" runat="server">
                    <asp:Panel ID="LoveButton" CssClass="button" runat="server">
                        <asp:Label ID="LoveCount" runat="server" />
                        <span>Love</span></asp:Panel>
                    </asp:HyperLink>
                    <asp:HyperLink ID="BuyLink" runat="server" CssClass="OutBoundLink">
                        <asp:Panel ID="BuyButton" CssClass="button" runat="server">
                            <span>Buy</span></asp:Panel>
                    </asp:HyperLink>
                </td>
            </tr>
        </table>
    </center>
    <!-- metadata for page -->
    <asp:Label ID="PId" runat="server" Style="display: none" />
    <asp:Label ID="UserId" runat="server" Style="display: none;" />
    <!--js for the page -->
    <script type="text/javascript">

        $(document).ready(function () {
            SetPdtPageButtons();
            
        });
    
    </script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="settings.aspx.cs" Inherits="settings" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="Server">
<div style="padding:8px;">
    <h1 style="color: #FB58A3;font-size:48px;">SETTINGS</h1>
    <h3 style="color: #999999; font-size: 18px;
    font-style: italic;
    font-weight: normal;
    line-height: 20px;">Customize your fashionkred settings</h3>
    <h3 style="color: #999999;font-size: 24px;">PRIVATE BROWSING</h3>
						
						    <label style="font-size: 18px;color: #333333;line-height: 30px;
    padding-bottom: 12px;">
	
			    <asp:CheckBox id="Private" Checked="TRUE" runat="server" />
          
								    Private browsing - disables posting activities to facebook<br />
                <br/>
								
							
						    <asp:Button ID="Update" runat="server" Text="Update" CssClass="settings-button"
            onclick="Update_Click" />
								
							
						    </label>
						  </div>
</asp:Content>


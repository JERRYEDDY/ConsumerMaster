<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="ConsumerMaster.About" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="false" />
    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="demo" DecoratedControls="All" EnableRoundedCorners="false" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    </telerik:RadAjaxManager>
    <h4>Home</h4>
    <div class="demo-container no-bg">
    <div id="grid">
    </div>
    <div>
        <br/>
        <telerik:RadPanelBar ID="RadPanelBar1" runat="server"></telerik:RadPanelBar>
        <table>
            <tr>
                <td>
                    <img src="Images/AWCLogo1.png" />
                </td>
            </tr>
            <tr>
                <td>
                    Consumer Export File 
                </td>
                <td class="space"/>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton2" CssClass="downloadButton" ValidationGroup="FileDownloadValidationGroup" 
                                       OnClick="AWCConsumerExportDownload_Click" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Service Export File 
                </td>
                <td class="space"/>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton1" CssClass="downloadButton" ValidationGroup="FileDownloadValidationGroup" 
                                       OnClick="AWCServiceExportDownload_Click" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div>
</asp:Content>

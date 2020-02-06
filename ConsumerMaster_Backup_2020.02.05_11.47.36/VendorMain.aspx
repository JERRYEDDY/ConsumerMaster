<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VendorMain.aspx.cs" Inherits="ConsumerMaster.VendorMain" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="demo-container no-bg">
        <div id="grid">
        </div>
        <div>
            <br/>
            <telerik:RadPanelBar ID="RadPanelBar1" runat="server"></telerik:RadPanelBar>
            <table>
                <tr>
                    <td>
                        <img src="Images/VendorLogo.png" width="231" height="54" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Consumer Export File 
                    </td>
                    <td class="space"/>
                    <td>
                        <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton2" CssClass="downloadButton" ValidationGroup="FileDownloadValidationGroup" OnClick="VendorConsumerExportDownload_Click" runat="server">
                            <Icon PrimaryIconCssClass="rbDownload"></Icon>
                        </telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        Service Export File 
                    </td>
                    <td class="space"/>
                    <td>
                        <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton1" CssClass="downloadButton" ValidationGroup="FileDownloadValidationGroup" OnClick="VendorServiceExportDownload_Click" runat="server" >
                            <Icon PrimaryIconCssClass="rbDownload"></Icon>
                        </telerik:RadButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

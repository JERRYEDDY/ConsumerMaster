<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientConversionMain.aspx.cs" Inherits="ConsumerMaster.ClientConversionMain" %>
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
                        <img src="Images/PathwaysLogo.png" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Client All File 
                    </td>
                    <td class="space"/>
                    <td>
                        <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton4" CssClass="downloadButton" ValidationGroup="FileDownloadValidationGroup" OnClick="ClientAllDownload_Click" runat="server">
                            <Icon PrimaryIconCssClass="rbDownload"></Icon>
                        </telerik:RadButton>
                    </td>
                </tr>
<%--                <tr>
                    <td>
                        Client Information File 
                    </td>
                    <td class="space"/>
                    <td>
                        <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton1" CssClass="downloadButton" ValidationGroup="FileDownloadValidationGroup" OnClick="ClientInformationDownload_Click" runat="server">
                            <Icon PrimaryIconCssClass="rbDownload"></Icon>
                        </telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        Client Diagnosis File 
                    </td>
                    <td class="space"/>
                    <td>
                        <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton2" CssClass="downloadButton" ValidationGroup="FileDownloadValidationGroup" OnClick="ClientDiagnosisDownload_Click" runat="server">
                            <Icon PrimaryIconCssClass="rbDownload"></Icon>
                        </telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        Client Benefits File 
                    </td>
                    <td class="space"/>
                    <td>
                        <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton3" CssClass="downloadButton" ValidationGroup="FileDownloadValidationGroup" OnClick="ClientBenefitsDownload_Click" runat="server">
                            <Icon PrimaryIconCssClass="rbDownload"></Icon>
                        </telerik:RadButton>
                    </td>
                </tr>--%>
            </table>
        </div>
    </div>
</asp:Content>

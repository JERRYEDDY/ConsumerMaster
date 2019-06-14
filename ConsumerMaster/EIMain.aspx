<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EIMain.aspx.cs" Inherits="ConsumerMaster.EIMain" %>
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
                        <img src="Images/EILogo1.png" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Consumer Export File 
                    </td>
                    <td class="space"/>
                    <td>
                        <telerik:RadDropDownList RenderMode="Lightweight" ID="EIConsumerList" runat="server"  Width="350px" DefaultMessage="Select Partner" ValidationGroup="EIConsumerValidationGroup" />
                        <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton5" CssClass="downloadButton" ValidationGroup="EIConsumerValidationGroup"  OnClick="EIConsumerExportDownload_Click" runat="server" >
                            <Icon PrimaryIconCssClass="rbDownload"></Icon>
                        </telerik:RadButton>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="EIConsumerList" ForeColor="red" Display="Dynamic" ErrorMessage="You must select a trading partner!" CssClass="validationClass" ValidationGroup="EIConsumerValidationGroup" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Service Export File 
                    </td>
                    <td class="space"/>
                    <td>
                        <telerik:RadDropDownList RenderMode="Lightweight" ID="EIServiceList" runat="server"  Width="350px" DefaultMessage="Select Partner" ValidationGroup="EIServiceValidationGroup" />
                        <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton7" CssClass="downloadButton" ValidationGroup="EIServiceValidationGroup" OnClick="EIServiceExportDownload_Click" runat="server" >
                            <Icon PrimaryIconCssClass="rbDownload"></Icon>
                        </telerik:RadButton>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="EIServiceList" ForeColor="red" Display="Dynamic" ErrorMessage="You must select a trading partner!" CssClass="validationClass" ValidationGroup="EIServiceValidationGroup" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

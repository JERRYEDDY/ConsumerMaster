<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ATFMain.aspx.cs" Inherits="ConsumerMaster.ATFMain" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="demo-container no-bg">
        <p>
            <asp:Label runat="server" ID="Label4" />
        </p>
        <table>
            <tr>
                <td>
                    <img src="Images/ATFLogo.png" width="231" height="54" />
                </td>
            </tr>
            <tr>
                <td>
                    Consumer Export File 
                </td>
                <td class="space"/>
                <td>
                    <telerik:RadDropDownList RenderMode="Lightweight" ID="ATFConsumerList" runat="server"  Width="350px" DefaultMessage="Select Partner" 
                        ValidationGroup="ATFConsumerValidationGroup" />
                    <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton5" CssClass="downloadButton" ValidationGroup="ATFConsumerValidationGroup"  OnClick="ATFConsumerExportDownload_Click" runat="server" >
                        <Icon PrimaryIconCssClass="rbDownload"></Icon>
                    </telerik:RadButton>
                   <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ATFConsumerList" ForeColor="red" Display="Dynamic" ErrorMessage="You must select a trading partner!" CssClass="validationClass" ValidationGroup="ATFConsumerValidationGroup" />
                </td>
            </tr>
            <tr>
                <td>
                    Service Export File 
                </td>
                <td class="space"/>
                <td>
                    <telerik:RadDropDownList RenderMode="Lightweight" ID="ATFServiceList" runat="server"  Width="350px" DefaultMessage="Select Partner" 
                        ValidationGroup="ATFServiceValidationGroup" />
                    <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton7" CssClass="downloadButton" ValidationGroup="ATFServiceValidationGroup" OnClick="ATFServiceExportDownload_Click" runat="server" >
                        <Icon PrimaryIconCssClass="rbDownload"></Icon>
                    </telerik:RadButton>
                   <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="ATFServiceList" ForeColor="red" Display="Dynamic" ErrorMessage="You must select a trading partner!" CssClass="validationClass" ValidationGroup="ATFServiceValidationGroup" />
                </td>
            </tr>
        </table>
        <asp:Literal ID="itemsClientSide" runat="server" />
    </div> 
</asp:Content>

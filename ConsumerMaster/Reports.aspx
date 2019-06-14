<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="ConsumerMaster.Reports" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="false" />
    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="demo" DecoratedControls="All" EnableRoundedCorners="false" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    </telerik:RadAjaxManager>
    <h4>Reports</h4>
    <div class="demo-container no-bg">
    <div id="grid">
    </div>
    <div>
        <br/>
        <telerik:RadPanelBar ID="RadPanelBar1" runat="server"></telerik:RadPanelBar>
        <table>
            <tr>
                <td>
                    <img src="Images/ATFLogo1.png" />
                </td>
            </tr>
            <tr>
                <td>
                    Consumer Ratio Report 
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadDropDownList RenderMode="Lightweight" ID="ATFPartnerList" runat="server"  Width="350px" DefaultMessage="Select Partner" ValidationGroup="ATFConsumerRatioReportGroup" />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ATFPartnerList" ForeColor="red" Display="Dynamic" ErrorMessage="You must select a trading partner!" CssClass="validationClass" ValidationGroup="ATFConsumerRatioReportGroup" />
                </td>
            </tr>
            <tr><td></td></tr>
            <tr><td></td></tr>
            <tr><td></td></tr>
            <tr><td></td></tr>
            <tr>
                <td>
                    <telerik:RadDatePicker RenderMode="Lightweight" ID="StartDatePicker" runat="server" DateInput-Label="From: " />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="StartDatePicker" ForeColor="red" Display="Dynamic" ErrorMessage="You must select start date!"  ValidationGroup="ATFConsumerRatioReportGroup"/>
                    <telerik:RadDatePicker RenderMode="Lightweight" ID="EndDatePicker" runat="server" DateInput-Label="To: " />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="EndDatePicker" ForeColor="red" Display="Dynamic" ErrorMessage="You must select end date!"  ValidationGroup="ATFConsumerRatioReportGroup"/>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="StartDatePicker" ForeColor="red" EnableClientScript="true" ControlToValidate="EndDatePicker" Type="Date" Operator="GreaterThan" ErrorMessage="The end date must be after the start one." ValidationGroup="ATFConsumerRatioReportGroup" />
                    <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton6" CssClass="downloadButton" ValidationGroup="ATFConsumerRatioReportGroup" OnClick="ATFConsumerRatioReportDownload_Click" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </div>
</asp:Content>

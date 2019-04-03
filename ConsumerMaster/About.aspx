<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="ConsumerMaster.About" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<style>

.container-1 {
    display: flex;
}

.container-1 div {
    border: 1px #ccc solid;
    padding:10px
}

.box-1{

}

.box-2{

}

.box-3 {

}
</style>   

<div class="container-1">
    <div class="box-1">
        <h3>Agency With Choice</h3>
        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>
    </div>    
    <div class="box-2">
        <img src="Images/ATFLogo1.png" />
        <table>
            <tr>
                <td>
                    <telerik:RadDropDownList RenderMode="Lightweight" ID="ATFPartnerList" runat="server"  Width="350px" DefaultMessage="Select Partner" ValidationGroup="ATFConsumerRatioReportGroup" />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ATFPartnerList" Display="Dynamic" ErrorMessage="You must select a trading partner!" CssClass="validationClass" ValidationGroup="ATFConsumerRatioReportGroup" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadDatePicker RenderMode="Lightweight" ID="StartDatePicker" runat="server" DateInput-Label="From: " />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="StartDatePicker" Display="Dynamic" ErrorMessage="You must select start date!"  ValidationGroup="ATFConsumerRatioReportGroup"/>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadDatePicker RenderMode="Lightweight" ID="EndDatePicker" runat="server" DateInput-Label="To: " />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="EndDatePicker" Display="Dynamic" ErrorMessage="You must select end date!"  ValidationGroup="ATFConsumerRatioReportGroup"/>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="StartDatePicker" EnableClientScript="true" ControlToValidate="EndDatePicker" Type="Date" Operator="GreaterThan" ErrorMessage="The end date must be after the start one." ValidationGroup="ATFConsumerRatioReportGroup" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton6" CssClass="downloadButton" ValidationGroup="ATFConsumerRatioReportGroup" OnClick="ATFConsumerRatioReportDownload_Click" runat="server" />
                </td>
            </tr>
        </table>
    </div> 
    <div class="box-3">
        <h3>Early Intervention</h3>
        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>
    </div> 
</div>    


</asp:Content>

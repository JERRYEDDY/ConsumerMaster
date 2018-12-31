<%@ Page Title="ATF Billing Entry" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ATFBillingEntry.aspx.cs" Inherits="ConsumerMaster.ATFBillingEntry" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3><%: Title %>.</h3>
   
    <telerik:RadSpreadsheet ID="RadSpreadsheet1" runat="server" />

</asp:Content>

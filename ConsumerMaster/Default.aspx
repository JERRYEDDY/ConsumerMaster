<%@ Page Title="Home Page" Language="C#" MasterPageFile=".\Site.Master" AutoEventWireup="True" CodeBehind="Default.aspx.cs" Inherits="ConsumerMaster._Default" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<style>
    .images {
        display: inline;
        margin: 0px;
        padding: 0px;
        vertical-align:middle;
        width:230px;
    }
    #content {
        display: block;
        margin: 0px;
        padding: 0px;
        position: relative;
        top: 10px;
        height: auto;
        max-width: auto;
        overflow-y: hidden;
        overflow-x:auto;
        word-wrap:normal;
        white-space:nowrap;
    }
</style>
    <h4>Home</h4>
    <div id="content">
        <a href="AWCMain"><img src="Images/AWCLogo.png" class="images"/></a>
        <a href="ATFMain"><img src="Images/ATFLogo.png" class="images"/></a>
        <a href="EIMain"><img src="Images/EILogo.png" class="images"/></a>
        <a href="ResidentialMain"><img src="Images/ResidentialLogo.png" class="images"/></a>
        <a href="VendorMain"><img src="Images/VendorLogo.png" class="images"/></a>
    </div>
</asp:Content>

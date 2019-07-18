<%@ Page Title="Home Page" Language="C#" MasterPageFile=".\Site.Master" AutoEventWireup="True" CodeBehind="Default.aspx.cs" Inherits="ConsumerMaster._Default" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<style>
    .content {
        display: flex;
    }
    .content div {
        border: 1px #ccc solid;
        padding: 10px;
    }
    .box-1 {
        flex: 1;
    }
    .box-2 {
        flex: 1;
    }
    .box-3 {
        flex: 1;
    }
    .box-4 {
        flex: 1;
    }
    .box-5 {
        flex: 1;
    }
</style>
    <h4>Home</h4>
    <div class="content">
        <div class="box-1">
            <a href="AWCMain"><img src="Images/AWCLogo.png" class="images"/></a>
        </div>
        <div class="box-2">
            <a href="ATFMain"><img src="Images/ATFLogo.png" class="images"/></a>
        </div>
        <div class="box-3">
            <a href="EIMain"><img src="Images/EILogo.png" class="images"/></a>
        </div>
        <div class="box-4">
            <a href="ResidentialMain"><img src="Images/ResidentialLogo.png" class="images"/></a>
        </div>
        <div class="box-5">
            <a href="VendorMain"><img src="Images/VendorLogo.png" class="images"/></a>
        </div>
    </div>
</asp:Content>

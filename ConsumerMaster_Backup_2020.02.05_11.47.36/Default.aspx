<%@ Page Title="Home Page" Language="C#" MasterPageFile=".\Site.Master" AutoEventWireup="True" CodeBehind="Default.aspx.cs" Inherits="ConsumerMaster._Default" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
 <style>
.content {
    display: grid;
    grid-template-columns:repeat(5, 1fr);
}
</style>   
<h4>Home</h4>
<div class="content">
    <div>
        <a href="AWCMain"><img src="Images/AWCLogo.png" class="images"/></a>
    </div>
    <div>
        <a href="ATFMain"><img src="Images/ATFLogo.png" class="images"/></a>
    </div>
    <div>
        <a href="EIMain"><img src="Images/EILogo.png" class="images"/></a>
    </div>
    <div>
        <a href="ResidentialMain"><img src="Images/ResidentialLogo.png" class="images"/></a>
    </div>
    <div>
        <a href="VendorMain"><img src="Images/VendorLogo.png" class="images"/></a>
    </div>
</div>
</asp:Content>

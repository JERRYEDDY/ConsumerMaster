<%@ Page Title="Home Page" Language="C#" MasterPageFile=".\Site.Master" AutoEventWireup="True" CodeBehind="Default.aspx.cs" Inherits="ConsumerMaster._Default" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<style>
.space {
  background: none;
  width: 0.1rem;
}
.ddl option  {
    font-size: 30px;
}
.RadTreeView,
.RadTreeView a.rtIn,
.RadTreeView .rtEdit .rtIn input
{
   font-size:10px;
} 
</style>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="false" />
    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="demo" DecoratedControls="All" EnableRoundedCorners="false" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    </telerik:RadAjaxManager>
    <h4>Home</h4>
    <div class="demo-container no-bg">
        <div id="grid">
        </div>
        <div>
            <br/>
            <telerik:RadPanelBar ID="RadPanelBar2" runat="server"></telerik:RadPanelBar>
        </div>
    </div><br/><br/>
    
    <telerik:RadFormDecorator RenderMode="Lightweight" ID="FormDecorator1" runat="server" DecoratedControls="all" DecorationZoneID="decorationZone"></telerik:RadFormDecorator>
    <asp:SqlDataSource ID="TradingPartnerDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id AS trading_partner_id, name FROM TradingPartners"/>
    <asp:SqlDataSource ID="CompProcCodeDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id, name FROM CompositeProcedureCodes"/>
</asp:Content>

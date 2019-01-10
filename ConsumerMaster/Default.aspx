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
<%--    <link href="styles.css" rel="stylesheet" type="text/css" />--%>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="false" />
    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="demo" DecoratedControls="All" EnableRoundedCorners="false" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
<%--        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>--%>
    </telerik:RadAjaxManager>
    <h4>Home</h4>
    <div class="demo-container no-bg">
        <div id="grid">
        </div>
        <div>
            <br/>
            <telerik:RadPanelBar ID="RadPanelBar1" runat="server"></telerik:RadPanelBar>
            <table>
                <tr>
                    <td>
                        <img src="Images/AWCLogo1.png" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Consumer Export File 
                    </td>
                    <td class="space"/>
                    <td>
                        <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton2" CssClass="downloadButton" ValidationGroup="FileDownloadValidationGroup" 
                                           OnClick="AWCConsumerExportDownload_Click" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Service Export File 
                    </td>
                    <td class="space"/>
                    <td>
                        <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton1" CssClass="downloadButton" ValidationGroup="FileDownloadValidationGroup" 
                                           OnClick="AWCServiceExportDownload_Click" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div><br/><br/>
    
    <telerik:RadFormDecorator RenderMode="Lightweight" ID="FormDecorator1" runat="server" DecoratedControls="all" DecorationZoneID="decorationZone"></telerik:RadFormDecorator>
    
    <div class="demo-containers">
        <div class="demo-container" id="decorationZone">
            <fieldset>
            <legend>Trading Partners / Composite Procedure Code</legend>
                <table>
                    <tr>
                        <td>
                        <telerik:RadTreeView RenderMode="Lightweight" ID="RadTreeView1" runat="server" Height="350px" Width="300" TabIndex="1"></telerik:RadTreeView>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="ddl" DataSourceID="TradingPartnerDataSource" DataTextField="name" 
                                                          DataValueField="trading_partner_id" Font-Size="10"></asp:DropDownList>
                                        <telerik:RadButton ID="RadButton4" runat="server" Text="Add Trading Partner" OnClick="AddTradingPartnerNode_Click"></telerik:RadButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadDropDownList RenderMode="Lightweight" ID="RadDropDownList2" runat="server"  Width="220px"
                                                                 DropDownHeight="200" DataSourceID="CompositeProcedureCodeDataSource" DataTextField="name" DefaultMessage="Select Composite"
                                                                 DataValueField="id" TabIndex="3" >
                                        </telerik:RadDropDownList>
                                        <telerik:RadButton ID="RadButton3" runat="server" Text="Add Proc Code" OnClick="AddCompositeProcedureCodeNode_Click"></telerik:RadButton>
                                        <telerik:RadButton ID="RadButton5" runat="server" Text="Delete Selected" OnClick="DeleteSelectedNode_Click"></telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>   
    </div>
    <asp:TreeView ID="TreeView1" runat="server"></asp:TreeView>


    <asp:XmlDataSource runat="server" ID="XmlDataSource1" DataFile="TreeView.xml" XPath="/Tree/Node"></asp:XmlDataSource>
    <asp:SqlDataSource ID="TradingPartnerDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id AS trading_partner_id, name FROM TradingPartners"/>
    <asp:SqlDataSource ID="CompositeProcedureCodeDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id, name FROM CompositeProcedureCodes"/>

</asp:Content>

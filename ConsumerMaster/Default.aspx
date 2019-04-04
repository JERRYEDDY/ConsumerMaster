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
                        <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton2" CssClass="downloadButton" ValidationGroup="FileDownloadValidationGroup" OnClick="AWCConsumerExportDownload_Click" runat="server">
                            <Icon PrimaryIconCssClass="rbDownload"></Icon>
                        </telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        Service Export File 
                    </td>
                    <td class="space"/>
                    <td>
                        <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton1" CssClass="downloadButton" ValidationGroup="FileDownloadValidationGroup" OnClick="AWCServiceExportDownload_Click" runat="server" >
                            <Icon PrimaryIconCssClass="rbDownload"></Icon>
                        </telerik:RadButton>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <br/>
            <telerik:RadPanelBar ID="RadPanelBar2" runat="server"></telerik:RadPanelBar>
            
            <div class="demo-container size-thin">
                <h4>Consumer Export File</h4>
                <telerik:RadDropDownList RenderMode="Lightweight" ID="TPRadDropDownList" runat="server"  Width="350px" DropDownHeight="200" DefaultMessage="Select Trading Partner" ValidationGroup="TPValidationGroup" />
                <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton3" CssClass="downloadButton" ValidationGroup="TPValidationGroup" OnClick="ConsumerExportDownload_Click" runat="server" >
                    <Icon PrimaryIconCssClass="rbDownload"></Icon>
                </telerik:RadButton>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="TPRadDropDownList" Display="Dynamic" ErrorMessage="You must select a trading partner!" CssClass="validationClass" ValidationGroup="TPValidationGroup" />
                <p>
                    <asp:Label runat="server" ID="Label2" />
                </p>
            </div>
            <div class="demo-container size-thin">
                <h3>EI Service Export</h3>
                <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton4" CssClass="downloadButton" ValidationGroup="FileDownloadValidationGroup" OnClick="EIServiceExportDownload_Click" runat="server" >
                    <Icon PrimaryIconCssClass="rbDownload"></Icon>
                </telerik:RadButton>
                <p>
                    <asp:Label runat="server" ID="Label1" />
                </p>
            </div>   

            <div class="demo-container size-thin">
                <p>
                    <asp:Label runat="server" ID="Label4" />
                </p>
                <table>
                    <tr>
                        <td>
                            <img src="Images/ATFLogo1.png" />
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
                           <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ATFConsumerList" Display="Dynamic" ErrorMessage="You must select a trading partner!" CssClass="validationClass" ValidationGroup="ATFConsumerValidationGroup" />
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
                           <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="ATFServiceList" Display="Dynamic" ErrorMessage="You must select a trading partner!" CssClass="validationClass" ValidationGroup="ATFServiceValidationGroup" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            MultiSelect Checkbox 
                        </td>
                        <td class="space"/>
                        <td>
                            <telerik:RadComboBox RenderMode="Lightweight" ID="RadComboBox1" runat="server" CheckBoxes="true" Width="400" />
                            <telerik:RadButton RenderMode="Lightweight" ID="Button1" runat="server" Text="Get Checked Items" OnClick="Button1_Click"  />
                        </td>
                    </tr>
                </table>
                <asp:Literal ID="itemsClientSide" runat="server" />
            </div>    
        </div>
    </div><br/><br/>
    
    <telerik:RadFormDecorator RenderMode="Lightweight" ID="FormDecorator1" runat="server" DecoratedControls="all" DecorationZoneID="decorationZone"></telerik:RadFormDecorator>
    <asp:SqlDataSource ID="TradingPartnerDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id AS trading_partner_id, name FROM TradingPartners"/>
    <asp:SqlDataSource ID="CompProcCodeDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id, name FROM CompositeProcedureCodes"/>

</asp:Content>

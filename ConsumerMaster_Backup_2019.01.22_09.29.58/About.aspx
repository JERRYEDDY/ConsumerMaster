<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="ConsumerMaster.About" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="true" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid3" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid3" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid3">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid3"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <div class="demo-container no-bg">
        <h3>Customers:</h3>
        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowPaging="true" PageSize="5" DataSourceID="SqlDataSource1"
            OnItemCommand="RadGrid1_ItemCommand">
            <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView DataKeyNames="CustomerID">
            </MasterTableView>
            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
        </telerik:RadGrid>
        <br />
        <br />
        <h3>Orders:</h3>
        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid2" ShowStatusBar="true" runat="server" AllowPaging="True"
            PageSize="5" DataSourceID="SqlDataSource2">
            <MasterTableView Width="100%" AutoGenerateColumns="False" DataKeyNames="OrderID"
                DataSourceID="SqlDataSource2">
                <Columns>
                    <telerik:GridBoundColumn DataField="OrderID" DataType="System.Int32" HeaderText="OrderID"
                        ReadOnly="True" SortExpression="OrderID" UniqueName="OrderID">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="OrderDate" DataType="System.DateTime" HeaderText="OrderDate"
                        SortExpression="OrderDate" UniqueName="OrderDate" DataFormatString="{0:d}">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ShipCountry" HeaderText="ShipCountry" SortExpression="ShipCountry"
                        UniqueName="ShipCountry">
                    </telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
            <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
        </telerik:RadGrid>
        <br />
        <br />
        <h3>Orders details:</h3>
        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid3" ShowStatusBar="true" runat="server" AllowPaging="True"
            PageSize="5" DataSourceID="SqlDataSource3">
            <MasterTableView Width="100%" AutoGenerateColumns="False" DataKeyNames="OrderID"
                DataSourceID="SqlDataSource3">
                <Columns>
                    <telerik:GridBoundColumn DataField="UnitPrice" HeaderText="Unit price" SortExpression="UnitPrice"
                        UniqueName="UnitPrice">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity"
                        UniqueName="Quantity">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Discount" HeaderText="Discount (%)" SortExpression="Discount"
                        UniqueName="Discount">
                    </telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
        </telerik:RadGrid>
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>"
        ProviderName="System.Data.SqlClient" SelectCommand="SELECT CustomerID, CompanyName, ContactName, Address FROM Customers"
        runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>"
        ProviderName="System.Data.SqlClient" SelectCommand="SELECT [OrderID], [OrderDate], [CustomerID], [ShipCountry] FROM [Orders] WHERE ([CustomerID] = @CustomerID)"
        runat="server">
        <SelectParameters>
            <asp:ControlParameter ControlID="RadGrid1" DefaultValue="ALFKI" Name="CustomerID"
                PropertyName="SelectedValue" Type="String"></asp:ControlParameter>
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource3" ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>"
        ProviderName="System.Data.SqlClient" SelectCommand="SELECT [OrderID], [UnitPrice], [Quantity], [Discount] FROM [Order Details] WHERE ([OrderID] = @OrderID)"
        runat="server">
        <SelectParameters>
            <asp:ControlParameter ControlID="RadGrid2" Name="OrderID" DefaultValue="10643" PropertyName="SelectedValues['OrderID']"
                Type="Int32"></asp:ControlParameter>
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

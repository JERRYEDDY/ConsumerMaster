<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ConsumerMaster._Default" %>

<%@ Register Assembly="Infragistics4.Web.v18.2, Version=18.2.20182.143, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<%--    <link rel="stylesheet" href="/Styles/StyleSheet.css" type="text/css" media="screen" /> --%>  

<style type="text/css">
.gridview {
        font-family:"arial";
        background-color:#FFFFFF;
        width: 100%;
        font-size: small;
}
.gridview th {
        background: #7AC142;
        padding: 5px;
        font-size:small;

}
.gridview th a{
        color: #003300;
        text-decoration: none;
}
.gridview th a:hover{
        color: #003300;
        text-decoration: underline;
}
.gridview td  {
        background: #D9EDC9;
        color: #333333;
        font: small "arial";
        padding: 4px;
}
.gridview tr.even td {
        background: #FFFFFF;
}
.gridview td a{
        color: #003300;
        font: bold small "arial";
        padding: 2px;
        text-decoration: none;
}
.gridview td a:hover {
        color: red;
        font-weight: bold;
        text-decoration:underline;
}

</style>
<div>
    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="consumer_internal_number" 
        DataSourceID="SqlDataSource1" CssClass="gridview" AlternatingRowStyle-CssClass="even">
        <Columns>
            <asp:BoundField DataField="consumer_internal_number" HeaderText="Number" ReadOnly="True" 
                SortExpression="consumer_internal_number" />
            <asp:BoundField DataField="consumer_first" HeaderText="First Name" 
                SortExpression="consumer_first" />
            <asp:BoundField DataField="consumer_last" HeaderText="Last Name" 
                SortExpression="consumer_last" />
            <asp:BoundField DataField="date_of_birth" HeaderText="Birth Date" 
                SortExpression="date_of_birth" DataFormatString="{0:MM-dd-yyyy}"/>
            <asp:BoundField DataField="address_line_1" HeaderText="Address1" 
                SortExpression="address_line_1" />
            <asp:BoundField DataField="city" HeaderText="City" SortExpression="city" />
            <asp:BoundField DataField="state" HeaderText="State" 
                SortExpression="state" />
            <asp:BoundField DataField="zip_code" HeaderText="Zip Code" 
                SortExpression="zip_code" />
            <asp:BoundField DataField="identifier" HeaderText="ID" 
                SortExpression="identifier" />
            <asp:BoundField DataField="gender" HeaderText="Gender" SortExpression="gender" />
            <asp:BoundField DataField="diagnosis" HeaderText="Diagnosis" SortExpression="diagnosis" />
            <asp:BoundField DataField="nickname_first" HeaderText="Nick" SortExpression="nickname_first" />
            <asp:BoundField DataField="nickname_last" HeaderText="Name" SortExpression="nickname_last" />
        </Columns>
    </asp:GridView>
</div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" 
        SelectCommand="SELECT * FROM [Consumers]"></asp:SqlDataSource>
<div></div>
    <p></p>
    <br />
    <br />

    <div>
        <ig:WebDataGrid ID="WebDataGrid1" Height="400" Width="100%" DataSourceID="SqlDataSource2" runat="server" AutoGenerateColumns="False" ShowFooter="True" AltItemCssClass="gridview">
            <Columns>
                <ig:BoundDataField DataFieldName="consumer_internal_number" Key="consumer_internal_number">
                    <Header Text="Number" />
                </ig:BoundDataField>
                <ig:BoundDataField DataFieldName="consumer_first" Key="consumer_first">
                    <Header Text="First Name" />
                </ig:BoundDataField>
                <ig:BoundDataField DataFieldName="consumer_last" Key="consumer_last">
                    <Header Text="Last Name" />
                </ig:BoundDataField>
                <ig:BoundDataField DataFieldName="date_of_birth" Key="date_of_birth">
                    <Header Text="Birth Date" />
                </ig:BoundDataField>
                <ig:BoundDataField DataFieldName="address_line_1" Key="address_line_1">
                    <Header Text="Address1" />
                </ig:BoundDataField>
                <ig:BoundDataField DataFieldName="city" Key="city">
                    <Header Text="City" />
                </ig:BoundDataField>
                <ig:BoundDataField DataFieldName="zip_code" Key="zip_code">
                    <Header Text="Zip Code" />
                </ig:BoundDataField>
                <ig:BoundDataField DataFieldName="identifier" Key="identifier">
                    <Header Text="ID" />
                </ig:BoundDataField>
                <ig:BoundDataField DataFieldName="gender" Key="gender">
                    <Header Text="Gender" />
                </ig:BoundDataField>
                <ig:BoundDataField DataFieldName="diagnosis" Key="diagnosis">
                    <Header Text="Diagnosis" />
                </ig:BoundDataField>
                <ig:BoundDataField DataFieldName="nickname_first" Key="nickname_first">
                    <Header Text="Nick" />
                </ig:BoundDataField>
                <ig:BoundDataField DataFieldName="nickname_last" Key="nickname_last">
                    <Header Text="Name" />
                </ig:BoundDataField>
            </Columns>        
            <Behaviors>
                <ig:Paging PageSize="13" QuickPages="5"></ig:Paging>
            </Behaviors>
        </ig:WebDataGrid>
    </div>
    <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>"
    SelectCommand="SELECT * FROM [Consumers]">
    </asp:SqlDataSource>

</asp:Content>

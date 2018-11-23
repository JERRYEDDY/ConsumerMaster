<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ConsumerMaster._Default" %>

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
                SortExpression="date_of_birth" />
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

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" 
        SelectCommand="SELECT * FROM [Consumers]"></asp:SqlDataSource>
</asp:Content>

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

<style type="text/css">
               <%-- body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        table
        {
            border: 1px solid #ccc;
            border-collapse: collapse;
            background-color: #fff;
        }
        table th
        {
            background-color: #B8DBFD;
            color: #333;
            font-weight: bold;
        }
        table th, table td
        { background-color: #B8DBFD;
            padding: 5px;
            border: 1px solid #ccc;
        }
        table, table table td
        {
            border: 3px solid #ccc;
        }
  --%>
    .style1
    {
        width: 184px;
    }
    </style>

    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False"  AllowPaging="True"
    OnPageIndexChanging="OnPageIndexChanging" PageSize="6" DataKeyNames="BId"
        OnRowDataBound="OnRowDataBound" OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit"
        OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting" 
        EmptyDataText="No records has been added." 
        Style="margin:20px 0px 0px 25px;" BackColor="White" BorderColor="#3366CC" 
        BorderStyle="None" BorderWidth="1px" CellPadding="4" Height="250px" 
        Width="1035px" >
        <Columns>
            <asp:TemplateField HeaderText="First" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblConsumerFirst" runat="server" Text='<%# Eval("consumer_first") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtConsumerFirst"  style = "Width:100px;" runat="server" Text='<%# Eval("consumer_first") %>'></asp:TextBox>
                </EditItemTemplate>

            <asp:TemplateField HeaderText="Last" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblConsumerLast" runat="server" Text='<%# Eval("consumer_last") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtConsumerLast"  style = "Width:100px;" runat="server" Text='<%# Eval("consumer_last") %>'></asp:TextBox>
                </EditItemTemplate>

<ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ProvinceName" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblProvinceName" runat="server" Text='<%# Eval("Provincename") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtProvinceName"  style = "Width:100px;" runat="server" Text='<%# Eval("Provincename") %>'></asp:TextBox>
                </EditItemTemplate>

<ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="CityName" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblCityname" runat="server" Text='<%# Eval("Cityname") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtCityname"  style = "Width:100px;" runat="server" Text='<%# Eval("Cityname") %>'></asp:TextBox>
                </EditItemTemplate>

<ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField><asp:TemplateField HeaderText="Number" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblNumber" runat="server" Text='<%# Eval("Number") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtNumber" style = "Width:100px;" runat="server" Text='<%# Eval("Number") %>'></asp:TextBox>
                </EditItemTemplate>

<ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField><asp:TemplateField HeaderText="Name" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtName" style = "Width:100px;" runat="server" Text='<%# Eval("Name") %>'></asp:TextBox>
                </EditItemTemplate>

<ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField><asp:TemplateField HeaderText="ContentType" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblContentType" runat="server" Text='<%# Eval("ContentType") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtContentType" style = "Width:100px;" runat="server" Text='<%# Eval("ContentType") %>'></asp:TextBox>
                </EditItemTemplate>

<ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField><asp:TemplateField HeaderText="Data" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblData" runat="server" Text='<%# Eval("Data") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtData" style = "Width:100px;" runat="server" Text='<%# Eval("Data") %>'></asp:TextBox>
                </EditItemTemplate>

<ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField>
           <asp:TemplateField> <ItemTemplate>
                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnClick="DownloadFile"
                            CommandArgument='<%# Eval("BId") %>'></asp:LinkButton>
                    </ItemTemplate></asp:TemplateField>
            <asp:CommandField ButtonType="Link" ShowEditButton="true" ShowDeleteButton="true"
                ItemStyle-Width="100" >
<ItemStyle Width="100px"></ItemStyle>
            </asp:CommandField>
        </Columns>
        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
        <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
        <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Center" 
            Font-Bold="True" Font-Italic="True" Font-Underline="True" Width="20px" />
        <RowStyle BackColor="White" ForeColor="#003399" />
        <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
        <SortedAscendingCellStyle BackColor="#EDF6F6" />
        <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
        <SortedDescendingCellStyle BackColor="#D6DFDF" />
        <SortedDescendingHeaderStyle BackColor="#002876" />
    </asp:GridView>
    <br />

 <table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse; margin:10px 0px 0px 25px;">
        <tr>
            <td style="width: 100px; background-color:#003399; color:#CCCCFF;">
              <b> Username:</b><br />
                <asp:TextBox ID="txtUsername" runat="server" Width="120" />
            </td>
            <td style="width: 100px;background-color:#003399; color:#CCCCFF;">
              <b> Provincename:</b><br />
                <asp:TextBox ID="txtProvinceName" runat="server" Width="120" />
            </td>
            <td style="width: 100px;background-color:#003399; color:#CCCCFF;">
               <b>Cityname:</b><br />
                <asp:TextBox ID="txtCityname" runat="server" Width="120" />
            </td>
            <td style="width: 100px;background-color:#003399; color:#CCCCFF;">
              <b> Number:</b><br />
                <asp:TextBox ID="txtNumber" runat="server" Width="120" />
            </td>
            <td style="width: 100px;background-color:#003399; color:#CCCCFF;">
              <b> Name:</b><br />
                <asp:TextBox ID="txtName" runat="server" Width="120" />
            </td>
            <td style="width: 100px;background-color:#003399; color:#CCCCFF;">
              <b> ContentType:</b><br />
                <asp:TextBox ID="txtContentType" runat="server" Width="120" />
            </td>
            <td style="width: 100px;background-color:#003399; color:#CCCCFF;">
               <b>Data:</b><br />
                <asp:TextBox ID="txtData" runat="server" Width="120" />
            </td>
            <td style="background-color:#003399; color:#CCCCFF;" class="style1">
                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-info" Text="Add" 
                    OnClick="Insert" Width="187px" />
            </td>
        </tr>
    </table>





</asp:Content>

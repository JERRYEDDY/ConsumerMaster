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
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Last" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblConsumerLast" runat="server" Text='<%# Eval("consumer_last") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtConsumerLast"  style = "Width:100px;" runat="server" Text='<%# Eval("consumer_last") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Birth Date" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblBirthDate" runat="server" Text='<%# Eval("date_of_birth") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtBirthDate"  style = "Width:100px;" runat="server" Text='<%# Eval("date_of_birth") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Address1" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblAddress1" runat="server" Text='<%# Eval("address_line_1") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtAddress1"  style = "Width:100px;" runat="server" Text='<%# Eval("address_line_1") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Address2" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblAddress2" runat="server" Text='<%# Eval("address_line_2") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtAddress2"  style = "Width:100px;" runat="server" Text='<%# Eval("address_line_2") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="City" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblCity" runat="server" Text='<%# Eval("city") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtCity" style = "Width:100px;" runat="server" Text='<%# Eval("city") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="State" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblState" runat="server" Text='<%# Eval("state") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtState" style = "Width:100px;" runat="server" Text='<%# Eval("state") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField>
 
            <asp:TemplateField HeaderText="Zip" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblZipCode" runat="server" Text='<%# Eval("zip_code") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtZipCode" style = "Width:100px;" runat="server" Text='<%# Eval("zip_code") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField>
 
            <asp:TemplateField HeaderText="Identifier" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblIdentifier" runat="server" Text='<%# Eval("identifier") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtIdentifier" style = "Width:100px;" runat="server" Text='<%# Eval("identifier") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Gender" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblGender" runat="server" Text='<%# Eval("gender") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtGender" style = "Width:100px;" runat="server" Text='<%# Eval("gender") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Diagnosis" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblDiagnosis" runat="server" Text='<%# Eval("diagnosis") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtDiagnosis" style = "Width:100px;" runat="server" Text='<%# Eval("diagnosis") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Nick" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblNicknameFirst" runat="server" Text='<%# Eval("nickname_first") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtNicknameFirst" style = "Width:100px;" runat="server" Text='<%# Eval("nickname_first") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Name" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Label ID="lblNicknameLast" runat="server" Text='<%# Eval("nickname_last") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtNicknameLast" style = "Width:100px;" runat="server" Text='<%# Eval("nickname_last") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle Width="120px"></ItemStyle>
            </asp:TemplateField>

            <asp:TemplateField> 
                <ItemTemplate>
                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnClick="DownloadFile" CommandArgument='<%# Eval("consumer_internal_number") %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ButtonType="Link" ShowEditButton="true" ShowDeleteButton="true" ItemStyle-Width="100" >
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
              <b> Consumer First:</b><br />
                <asp:TextBox ID="txtConsumerFirst" runat="server" Width="120" />
            </td>
            <td style="width: 100px;background-color:#003399; color:#CCCCFF;">
              <b> Consumer Last:</b><br />
                <asp:TextBox ID="txtConsumerLast" runat="server" Width="120" />
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
            <td style="width: 100px;background-color:#003399; color:#CCCCFF;">
              <b> Number:</b><br />
                <asp:TextBox ID="TextBox1" runat="server" Width="120" />
            </td>
            <td style="width: 100px;background-color:#003399; color:#CCCCFF;">
              <b> Name:</b><br />
                <asp:TextBox ID="TextBox2" runat="server" Width="120" />
            </td>
            <td style="width: 100px;background-color:#003399; color:#CCCCFF;">
              <b> ContentType:</b><br />
                <asp:TextBox ID="TextBox3" runat="server" Width="120" />
            </td>
            <td style="width: 100px;background-color:#003399; color:#CCCCFF;">
               <b>Data:</b><br />
                <asp:TextBox ID="TextBox4" runat="server" Width="120" />
            </td>

            <td style="width: 100px;background-color:#003399; color:#CCCCFF;">
              <b> Number:</b><br />
                <asp:TextBox ID="TextBox5" runat="server" Width="120" />
            </td>
            <td style="width: 100px;background-color:#003399; color:#CCCCFF;">
              <b> Name:</b><br />
                <asp:TextBox ID="TextBox6" runat="server" Width="120" />
            </td>
            <td style="width: 100px;background-color:#003399; color:#CCCCFF;">
              <b> ContentType:</b><br />
                <asp:TextBox ID="TextBox7" runat="server" Width="120" />
            </td>
            <td style="width: 100px;background-color:#003399; color:#CCCCFF;">
               <b>Data:</b><br />
                <asp:TextBox ID="TextBox8" runat="server" Width="120" />
            </td>



            <td style="background-color:#003399; color:#CCCCFF;" class="style1">
                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-info" Text="Add" 
                    OnClick="Insert" Width="187px" />
            </td>
        </tr>
    </table>





</asp:Content>

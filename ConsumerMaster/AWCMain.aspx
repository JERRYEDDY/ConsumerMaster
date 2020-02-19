<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AWCMain.aspx.cs" Inherits="ConsumerMaster.AWCMain" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<script>
    function validateUpload(sender, args) {
        var upload = $find("<%=RadAsyncUpload1.ClientID%>");
        args.IsValid = upload.getUploadedFiles().length != 0;
    }
</script>

    <div class="demo-container no-bg">
        <p>
            <asp:Label runat="server" ID="Label4" />
        </p>
        <table>
            <tr>
                <td>
                    <img src="Images/AWCLogo.png" width="231" height="54" />
                </td>
            </tr>
            <tr>
                <td>
                    <h4>Payroll Reports</h4>
                </td>
                <td>
                    &nbsp
                </td>
            </tr>
            <tr>
                <td>
                    Time & Distance Filename .xlsx (Weekly): 
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload1" MaxFileInputsCount="1" AllowedFileExtensions="xlsx"  />
<%--                    <asp:CustomValidator runat="server" ID="CustomValidator" ClientValidationFunction="validateUpload" ErrorMessage="Select a valid Time and Distance CSV file"></asp:CustomValidator>--%>
               </td>
            </tr>
            <tr>
                <td>
                    40 Hours Report
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton1" runat="server" text="Process" OnClick="RadButton1_Click"/>
                </td>
            </tr>
            <tr>
                <td>
                    29 Hours Report
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton2" runat="server" text="Process" OnClick="RadButton2_Click"/>
                </td>
            </tr>
            <tr>
                <td>
                    Overlap Shifts Report
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton3" runat="server" text="Process" OnClick="RadButton3_Click"/>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
               </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
               </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
               </td>
            </tr>
            <tr>
                <td>
                    <h4>Other Reports</h4>
                </td>
                <td>
                    &nbsp
                </td>
            </tr>
            <tr>
                <td>
                    Client Client Address Filename .xlsx: 
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncClient" MaxFileInputsCount="1" AllowedFileExtensions="xlsx"  />
<%--                    <asp:CustomValidator runat="server" ID="CustomValidator1" ClientValidationFunction="validateUpload" ErrorMessage="Select a valid Client Authorization Excel file"></asp:CustomValidator>--%>
               </td>
            </tr>
            <tr>
                <td>
                    Client Staff Filename .xlsx: 
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUploadStaff" MaxFileInputsCount="1" AllowedFileExtensions="xlsx"  />
<%--                    <asp:CustomValidator runat="server" ID="CustomValidator1" ClientValidationFunction="validateUpload" ErrorMessage="Select a valid Client Authorization Excel file"></asp:CustomValidator>--%>
               </td>
            </tr>
            <tr>
                <td>
                    Client Authorization Filename .xlsx: 
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUploadAuthorization" MaxFileInputsCount="1" AllowedFileExtensions="xlsx"  />
<%--                    <asp:CustomValidator runat="server" ID="CustomValidator1" ClientValidationFunction="validateUpload" ErrorMessage="Select a valid Client Authorization Excel file"></asp:CustomValidator>--%>
               </td>
            </tr>
            <tr>
                <td>
                    Client Staff Authorization Report
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton4" runat="server" text="Process" OnClick="RadButton4_Click"/>
                </td>
            </tr>
            <tr>
                <td>
                    Word Document
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton5" runat="server" text="Process" OnClick="Download_Click"/>
                </td>
            </tr>
        </table>
        <asp:Literal ID="itemsClientSide" runat="server" />
    </div> 
</asp:Content>

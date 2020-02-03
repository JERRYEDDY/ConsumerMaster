<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AWCMain.aspx.cs" Inherits="ConsumerMaster.AWCMain" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                    Time and Distance Report Filename: 
                </td>
                <td class="space"/>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload1" MaxFileInputsCount="1" AllowedFileExtensions="xlsx" OnFileUploaded="RadAsyncUpload1_FileUploaded" PostbackTriggers="RadButton1" />
               </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton1" runat="server" text="40 Hours" OnClick="RadButton1_Click"/>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton2" runat="server" text="29 Hours" OnClick="RadButton1_Click"/>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton3" runat="server" text="Overlap" OnClick="RadButton1_Click"/>
                </td>
            </tr>
        </table>
        <asp:Literal ID="itemsClientSide" runat="server" />
    </div> 
</asp:Content>

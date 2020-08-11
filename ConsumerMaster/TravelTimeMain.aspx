<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TravelTimeMain.aspx.cs" Inherits="ConsumerMaster.TravelTimeMain" %>

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
                    <h5>Payroll Reports</h5>
                </td>
                <td>
                    &nbsp
                </td>
                
            </tr>
            <tr>
                <td>
                    CellTrak Time & Distance Filename .xlsx: 
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload1" MaxFileInputsCount="1" AllowedFileExtensions="xlsx"  />
               </td>
            </tr>
            <tr>
                <td>
                    Travel Time Report
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>&nbsp;&nbsp;
                    <asp:CheckBox RenderMode="Lightweight" ID="ShiftCheckBox" runat="server" Text="&nbsp;Filter by Shift" />
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButtonTravel" runat="server" text="Process" OnClick="RadButtonTravel_Click"/>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
        <asp:Literal ID="itemsClientSide" runat="server" />
    </div> 
</asp:Content>

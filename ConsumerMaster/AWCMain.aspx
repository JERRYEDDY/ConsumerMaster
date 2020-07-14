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
                    <h5>Payroll Reports</h5>
                </td>
                <td>
                    &nbsp
                </td>
                
            </tr>
            <tr>
                <td>
                    Time & Distance Filename .xlsx: 
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
                    Mismatched Services Report
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButtonMismatchServices" runat="server" text="Process" OnClick="RadButtonMismatchServices_Click"/>
                </td>
            </tr>
<%--            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;Shift Only: 
                    <asp:CheckBox RenderMode="Lightweight" id="ShiftCheckBox" runat="server"></asp:CheckBox>
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButtonTravel" runat="server" text="Process" OnClick="RadButtonTravel_Click"/>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButtonTravel2" runat="server" text="Excel" OnClick="RadButtonTravel_Click2"/>
                </td>
                <td>
                    &nbsp;
               </td>
            </tr>--%>
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
                    <h5> Unit Utilization Report</h5>
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
                    <h5>Service Note Audit Reports</h5>
                </td>
                <td>
                    &nbsp
                </td>
            </tr>
            <tr>
            <tr>
                <td>
                    Closed Activities Filename .xlsx: 
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUploadClosedActivities" MaxFileInputsCount="1" AllowedFileExtensions="xlsx"  />
               </td>
            </tr>
            <tr>
                <td>
                    Audit Log Filename .xlsx: 
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUploadAuditLog" MaxFileInputsCount="1" AllowedFileExtensions="xlsx"  />
               </td>
            </tr>
            <tr>
                <td>
                    Service Note Audit Report
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton5" runat="server" text="Process" OnClick="RadButton5_Click"/>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp
                </td>
                <td>
                    &nbsp
                </td>
            </tr>
        </table>
        <asp:Literal ID="itemsClientSide" runat="server" />
    </div> 
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AWCMain.aspx.cs" Inherits="ConsumerMaster.AWCMain" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


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
                    Travel Time Report
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;Shift Only: 
                    <asp:CheckBox RenderMode="Lightweight" id="ShiftCheckBox" runat="server"></asp:CheckBox>
<%--                    &nbsp;&nbsp;&nbsp;Max Duration: 
                    <telerik:RadNumericTextBox RenderMode="Lightweight" id="MaxDurationTextBox" runat="server" InputType="Number" MinValue="0" MaxValue="999999" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator='' MaxLength="5"></telerik:RadNumericTextBox>--%>
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButtonTravel" runat="server" text="Process" OnClick="RadButtonTravel_Click"/>
                </td>
            </tr>
<%--            <tr>
                <td>
                    SSRS 2016 Report Viewer
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RVButton" runat="server" text="Process" OnClick="RVButton_Click"/>
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
<%--            <tr>
                <td colspan="2">
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1000px" Height="700px" ProcessingMode="Remote"></rsweb:ReportViewer>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
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
                    <h4> Unit Utilization Report</h4>
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
                    <h4>Data Integrity Reports</h4>
                </td>
                <td>
                    &nbsp
                </td>
            </tr>
            <tr>
            <tr>
                <td>
                    Client Roster Filename .xlsx: 
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUploadClientRoster" MaxFileInputsCount="1" AllowedFileExtensions="xlsx"  />
<%--                    <asp:CustomValidator runat="server" ID="CustomValidator" ClientValidationFunction="validateUpload" ErrorMessage="Select a valid Time and Distance CSV file"></asp:CustomValidator>--%>
               </td>
            </tr>
            <tr>
                <td>
                    Client Authorization List Filename .xlsx: 
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncClientAuthorizationList" MaxFileInputsCount="1" AllowedFileExtensions="xlsx"  />
<%--                    <asp:CustomValidator runat="server" ID="CustomValidator" ClientValidationFunction="validateUpload" ErrorMessage="Select a valid Time and Distance CSV file"></asp:CustomValidator>--%>
               </td>
            </tr>
            <tr>
                <td>
                    Client Staff List Filename .xlsx: 
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncClientStaffList" MaxFileInputsCount="1" AllowedFileExtensions="xlsx"  />
<%--                    <asp:CustomValidator runat="server" ID="CustomValidator" ClientValidationFunction="validateUpload" ErrorMessage="Select a valid Time and Distance CSV file"></asp:CustomValidator>--%>
               </td>
            </tr>
            <tr>
                <td>
                    Client Data Integrity Report
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton5" runat="server" text="Process" OnClick="RadButton5_Click"/>
                </td>
            </tr>
            <tr>
                <td>
                    Button 6
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton6" runat="server" text="Disable Button 7" OnClick="RadButton6_Click"/>
                </td>
            </tr>
            <tr>
                <td>
                    Button 7
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton7" runat="server" text="Button 7"/>
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
            <tr>
<%--                <rsweb:ReportViewer ID="ReportViewer1" runat="server"></rsweb:ReportViewer>--%>
            </tr>
        </table>
        <asp:Literal ID="itemsClientSide" runat="server" />
    </div> 
</asp:Content>

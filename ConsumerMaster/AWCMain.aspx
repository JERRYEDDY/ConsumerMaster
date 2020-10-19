<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AWCMain.aspx.cs" Inherits="ConsumerMaster.AWCMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="demo-container no-bg">
        <p>
            <asp:Label runat="server" ID="Label4" />
        </p>
        <table>
            <tr>
                <td>
<%--                    <asp:Image ID="AWCLogo" runat="server" ImageUrl="~/Images/AWCLogo.png" Width="231" Height="54" />--%>
<%--                    <img src="/Images/AWCLogo.png" width="231" height="54" />--%>
                    <img alt="logo" src="<%= Page.ResolveUrl("~/Images/AWCLogo.png")%>" />
                </td>
            </tr>
            <tr>
                <td>
                    <h5>Payroll/Billing Reports</h5>
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
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload1" MaxFileInputsCount="1" AllowedFileExtensions="xlsx" OnValidatingFile="RadUpload1_ValidatingFile" />
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
            </tr>
            <tr>
                <td>
                    Travel Time Report
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
                    NS Billing Authorizations Filename .xlsx:
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload2" MaxFileInputsCount="1" AllowedFileExtensions="xlsx"  />
               </td>
            </tr>
            <tr>
                <td>
                    Service Exception Report
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButtonBAException" runat="server" text="Process" OnClick="RadButtonServicesException_Click"/>
               </td>
            </tr>
            <tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    HCSIS Billing Authorizations Filename .xlsx:
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload3" MaxFileInputsCount="1" AllowedFileExtensions="xlsx" />
               </td>
            </tr>
            <tr>
                <td>
                    Service Exception (HCSIS) Report
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButtonBAExceptionHCSIS" runat="server" text="Process" OnClick="RadButtonServicesExceptionHCSIS_Click"/>
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
            <tr style="border-bottom:1px solid black">
                <td colspan="100%">
                </td>
            </tr>
            <tr> ... </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
<%--            <tr>
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
            </tr>--%>
<%--            <tr>
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
               </td>
            </tr>
            <tr>
                <td>
                    Client Staff Filename .xlsx: 
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUploadStaff" MaxFileInputsCount="1" AllowedFileExtensions="xlsx"  />
               </td>
            </tr>
            <tr>
                <td>
                    Client Authorization Filename .xlsx: 
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUploadAuthorization" MaxFileInputsCount="1" AllowedFileExtensions="xlsx"  />
               </td>
            </tr>
            <tr>
                <td>
                    Client Staff Authorization Report
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton4" runat="server" text="Process" OnClick="RadButton4_Click"/>
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
        </table>
        <asp:Literal ID="itemsClientSide" runat="server" />
    </div> 
</asp:Content>

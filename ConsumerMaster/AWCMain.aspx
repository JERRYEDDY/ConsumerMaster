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
<%--            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    CellTrak Scheduled Actual Filename Week 1 .xlsx:
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload5" MaxFileInputsCount="1" AllowedFileExtensions="xlsx" OnValidatingFile="RadAsyncUpload_ValidatingFile" />
               </td>
            </tr>
            <tr>
                <td>
                    CellTrak Scheduled Actual Filename Week 2 .xlsx:
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload6" MaxFileInputsCount="1" AllowedFileExtensions="xlsx" OnValidatingFile="RadAsyncUpload_ValidatingFile" />
               </td>
            </tr>
            <tr>
                <td>
                    Payroll File
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton5" runat="server" text="Process" OnClick="RadButtonPayrollFile_Click" ValidationGroup="TwoFileValid"/>
               </td>
            </tr>--%>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
            <tr style="border-bottom:1px solid black">
                <td colspan="100%">
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    CellTrak Time & Distance Filename .csv: 
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload1" MaxFileInputsCount="1" AllowedFileExtensions="csv" OnValidatingFile="RadAsyncUpload_ValidatingFile"/>
               </td>
            </tr>
            <tr>
                <td>
                    40 Hours Report
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton1" runat="server" text="Process" OnClick="RadButton1_Click" ValidationGroup="OneFileValid"/>
                </td>
            </tr>
            <tr>
                <td>
                    29 Hours Report
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton2" runat="server" text="Process" OnClick="RadButton2_Click" ValidationGroup="OneFileValid"/>
                </td>
            </tr>
            <tr>
                <td>
                    Overlap Shifts Report
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton3" runat="server" text="Process" OnClick="RadButton3_Click" ValidationGroup="OneFileValid"/>
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
                    <telerik:RadButton RenderMode="Lightweight" id="RadButtonTravel" runat="server" text="Process" OnClick="RadButtonTravel_Click" ValidationGroup="OneFileValid"/>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
<%--            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    NS Billing Authorizations Filename .xlsx:
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload2" MaxFileInputsCount="1" AllowedFileExtensions="xlsx" OnValidatingFile="RadAsyncUpload_ValidatingFile" />
               </td>
            </tr>--%>
            <tr>
                <td>
                    Service Exception Report
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButtonBAException" runat="server" text="Process" OnClick="RadButtonServicesException_Click" ValidationGroup="TwoFileValid"/>
               </td>
            </tr>
            <tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
<%--            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    HCSIS Billing Authorizations Filename .xlsx:
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload3" MaxFileInputsCount="1" AllowedFileExtensions="xlsx" OnValidatingFile="RadAsyncUpload_ValidatingFile" />
               </td>
            </tr>
            <tr>
                <td>
                    Service Exception (HCSIS) Report
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButtonBAExceptionHCSIS" runat="server" text="Process" OnClick="RadButtonServicesExceptionHCSIS_Click" ValidationGroup="ThreeFileValid"/>
               </td>
            </tr>--%>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr style="border-bottom:1px solid black">
                <td colspan="100%">
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
<%--            <tr>
                <td>
                    Sandata Export Visits Filename .xlsx:
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload4" MaxFileInputsCount="1" AllowedFileExtensions="xlsx" />
              </td>
            </tr>
            <tr>
                <td>
                    EVV Visits Comparison Report
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton4" runat="server" text="Process" OnClick="RadButtonEVV_VisitsComparison_Click" />
               </td>
            </tr>
            <tr>
            <tr>
                <td>
               </td>
            </tr>--%>
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

﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AWCMain.aspx.cs" Inherits="ConsumerMaster.AWCMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
<script>
    function validateUpload(sender, args) {
        var upload = $find("<%=RadAsyncUpload1.ClientID%>");
        args.IsValid = upload.getUploadedFiles().length != 0;
    }
</script>

    <div class="demo-container no-bg">
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
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    Mismatched Services Report
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButtonMismatchedServices" runat="server" text="Process" OnClick="RadButtonMismatchedServices_Click" ValidationGroup="TwoFileValid"/>
               </td>
            </tr>
            <tr>
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
            <tr>
                <td>
                    CellTrak Scheduled & Actual Filename .csv: 
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload2" MaxFileInputsCount="1" AllowedFileExtensions="csv" OnValidatingFile="RadAsyncUpload_ValidatingFile"/>
               </td>
            </tr>
             <tr>
                <td>
                    Payroll Processing Report
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButtonPayrollProcessingReport" runat="server" text="Process" OnClick="RadButtonPayrollProcessingReport_Click" ValidationGroup="ThreeFileValid"/>
               </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
<%--            <tr style="border-bottom:1px solid black">
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
                    Netsmart Client Services Filename Week 1 .csv:
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncWeek1" MaxFileInputsCount="1" AllowedFileExtensions="csv" />
              </td>
            </tr>
            <tr>
                <td>
                    Netsmart Client Services Filename Week 2 .csv:
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncWeek2" MaxFileInputsCount="1" AllowedFileExtensions="csv" />
              </td>
            </tr>
            <tr>
                <td>
                    AWC Payroll File
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton4" runat="server" text="Process" OnClick="RadButtonPayrollFile_Click" />
               </td>
            </tr>
            <tr>
            <tr>
                <td>
                    &nbsp;
               </td>
            </tr>--%>
<%--            <tr style="border-bottom:1px solid black">
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
                    Netsmart Client Services for Billing Filename .csv:
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUploadNCS" MaxFileInputsCount="1" AllowedFileExtensions="csv" />
              </td>
            </tr>
            <tr>
                <td>
                    Sandata Export Visits Filename .csv:
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUploadSEV" MaxFileInputsCount="1" AllowedFileExtensions="csv" />
              </td>
            </tr>
            <tr>
                <td>
                    EVV Visits Comparison
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButton5" runat="server" text="Process" OnClick="RadButtonEVV_VisitsComparison_Click" />
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

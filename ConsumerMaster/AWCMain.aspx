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
            <tr>
                <td>
                    CellTrak Time & Distance Filename .xlsx: 
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload1" MaxFileInputsCount="1" AllowedFileExtensions="xlsx" OnValidatingFile="RadAsyncUpload_ValidatingFile"/>
                    <asp:CustomValidator runat="server" ID="CustomValidator" ClientValidationFunction="validateUpload" 
                        ErrorMessage="CellTrak Time & Distance report file is required" ValidationGroup="OneFileValid" Display="None" />
                    <asp:CustomValidator runat="server" ID="CustomValidator3" ClientValidationFunction="validateUpload" 
                        ErrorMessage="CellTrak Time & Distance report file is required" ValidationGroup="TwoFileValid" Display="None" />
                    <asp:CustomValidator runat="server" ID="CustomValidator4" ClientValidationFunction="validateUpload" 
                        ErrorMessage="CellTrak Time & Distance report file is required" ValidationGroup="ThreeFileValid" Display="None" />
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
                    NS Billing Authorizations Filename .xlsx:
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload2" MaxFileInputsCount="1" AllowedFileExtensions="xlsx" OnValidatingFile="RadAsyncUpload_ValidatingFile" />
                    <asp:CustomValidator runat="server" ID="CustomValidator2" ClientValidationFunction="validateUpload" 
                        ErrorMessage="NS Billing Authorization report file is required" ValidationGroup="TwoFileValid" Display="None" />
                    <asp:CustomValidator runat="server" ID="CustomValidator5" ClientValidationFunction="validateUpload" 
                        ErrorMessage="NS Billing Authorization report file is required" ValidationGroup="ThreeFileValid" Display="None" />
               </td>
            </tr>
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
            <tr>
                <td>
                    HCSIS Billing Authorizations Filename .xlsx:
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload3" MaxFileInputsCount="1" AllowedFileExtensions="xlsx" OnValidatingFile="RadAsyncUpload_ValidatingFile" />
                    <asp:CustomValidator runat="server" ID="CustomValidator1" ClientValidationFunction="validateUpload" 
                        ErrorMessage="HCSIS Billing Authorization report file is required" ValidationGroup="ThreeFileValid" ForeColor="#FF3300" Display="None" />
               </td>
            </tr>
            <tr>
                <td>
                    Service Exception (HCSIS) Report
                </td>
                <td>
                    <telerik:RadButton RenderMode="Lightweight" id="RadButtonBAExceptionHCSIS" runat="server" text="Process" OnClick="RadButtonServicesExceptionHCSIS_Click" ValidationGroup="ThreeFileValid"/>
               </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    Sandata Export Visits Filename .xlsx:
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload4" MaxFileInputsCount="1" AllowedFileExtensions="xlsx" />
<%--                    <asp:CustomValidator runat="server" ID="CustomValidator6" ClientValidationFunction="validateUpload" 
                        ErrorMessage="Sandata Export Visits report file is required" ForeColor="#FF3300" Display="None" />--%>
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
                    <asp:ValidationSummary id="ValidationSummary1" DisplayMode="BulletList" runat="server" HeaderText="You must enter a value in the following fields:" Font-Names="verdana" Font-Size="12" ShowMessageBox="True" ShowSummary="False" ValidationGroup="OneFileValid" />
                    <asp:ValidationSummary id="ValidationSummary2" DisplayMode="BulletList" runat="server" HeaderText="You must enter a value in the following fields:" Font-Names="verdana" Font-Size="12" ShowMessageBox="True" ShowSummary="False" ValidationGroup="TwoFileValid" />
                    <asp:ValidationSummary id="ValidationSummary3" DisplayMode="BulletList" runat="server" HeaderText="You must enter a value in the following fields:" Font-Names="verdana" Font-Size="12" ShowMessageBox="True" ShowSummary="False" ValidationGroup="ThreeFileValid" />
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

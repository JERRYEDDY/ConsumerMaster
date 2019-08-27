<%@ Page Title="CompositeProcedureCodes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CompositeProcedureCodes.aspx.cs" Inherits="ConsumerMaster.CompositeProcedureCodes" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<link href="Styles/Flexbox.css" rel="stylesheet" />
    <style>
        b {
        font-color: red;
        }
    </style>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <p id="divMsgs" runat="server">
        <asp:Label ID="Label1" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="#FF8080">
        </asp:Label>
        <asp:Label ID="Label2" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="#00C000">
        </asp:Label>
    </p>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <h4>Composite Procedure Codes</h4>
    <div id="demo" class="demo-container no-bg">
        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowPaging="True" ShowFooter="true" AllowSorting="True" AutoGenerateColumns="False" ShowStatusBar="true" PageSize="12" 
            OnNeedDataSource="RadGrid1_NeedDataSource" OnInsertCommand="RadGrid1_InsertCommand" OnUpdateCommand="RadGrid1_UpdateCommand" OnDeleteCommand="RadGrid1_DeleteCommand">
            <PagerStyle Mode="NextPrevAndNumeric" />
            <MasterTableView CommandItemDisplay="Top" Name="CompositeProcedureCodes" DataKeyNames="id" EditMode="EditForms">
                <CommandItemTemplate>
                    <div style="padding: 5px 5px;">
                        <telerik:RadButton ID="RadButton1" runat="server" Text="Add new record" Skin="Default" RenderMode="Lightweight" CommandName="InitInsert">
                            <Icon PrimaryIconCssClass="rbAdd" />
                        </telerik:RadButton>
                        <telerik:RadButton ID="RadButton2" runat="server" Text="Edit selected" Skin="Default" RenderMode="Lightweight" CommandName="EditSelected">
                            <Icon PrimaryIconCssClass="rbEdit" />
                        </telerik:RadButton>&nbsp;&nbsp;
                    </div>
                </CommandItemTemplate> 
                <Columns>
                    <telerik:GridBoundColumn DataField="cpc_id" HeaderText="CPC Id" SortExpression="cpc_id" UniqueName="cpc_id">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage="This field cpc_id is required" />
                            <ModelErrorMessage BackColor="Red" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="cpc_name" HeaderText="CPC Name" SortExpression="cpc_name" UniqueName="cpc_name">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage="This field cpc_name is required" />
                            <ModelErrorMessage BackColor="Red" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="trading_partner_name" HeaderText="Trading Partner" SortExpression="trading_partner_name" UniqueName="trading_partner_name">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage="This field trading_partner_name is required" />
                            <ModelErrorMessage BackColor="Red" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="cpc_taxonomy_code" HeaderText="Taxonomy Code" SortExpression="cpc_taxonomy_code" UniqueName="cpc_taxonomy_code" ReadOnly="True">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage="This field cpc_taxonomy_code is required" />
                            <ModelErrorMessage BackColor="Red" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ConfirmText="Delete this Referring Provider record?" Text="Delete" CommandName="Delete" />
                </Columns>   
                <EditFormSettings EditFormType="Template">
                    <FormTemplate>
                        <section class="form-group">
                            <h4><b>Composite Procedure Codes Info</b></h4>
                            <hr style="border: 3px solid gray;" />
                            <p id="formInstructions"><mark>Fields marked with an asterisk (*) are required.</mark></p>
                            <ul class="form-fields">
                            <li>
                                <label for="cpc_id">CPC Id *:</label> 
                                <telerik:RadTextBox ID="cpc_id" runat="server" Text='<%# Bind("cpc_id") %>' TabIndex="1"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="red" Display="Dynamic" ControlToValidate="cpc_id" ErrorMessage="CPC Id is required"  ValidationGroup="FormValidationGroup" />
                            </li>
                            <li>
                                <label for="cpc_name">CPC Name *:</label> 
                                <telerik:RadTextBox ID="cpc_name" runat="server" Text='<%# Bind("cpc_name") %>' TabIndex="2"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="red" Display="Dynamic" ControlToValidate="last_name" ErrorMessage="CPC name is required"  ValidationGroup="FormValidationGroup" />
                            </li>
                            <li>
<%--                                <label for="npi_number">NPI No *:</label> 
                                <telerik:RadMaskedTextBox ID="npi_number" Text='<%# Bind("npi_number") %>' runat="server" TabIndex="4" MaxLength="10" Mask="##########" Width="80"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="red" Display="Dynamic" ControlToValidate="npi_number" ErrorMessage="NPI # is required"  ValidationGroup="FormValidationGroup" />
                                <asp:RegularExpressionValidator Display="Dynamic" ID="MaskedTextBoxRegularExpressionValidator" runat="server" ErrorMessage="10 digit number is required" 
                                                                ControlToValidate="npi_number" ValidationExpression="^\d{10}$"/>--%>
                            </li>
                        </section>
                        <section class="form-submit">
                                <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>' runat="server" CausesValidation="True"
                                      CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' ValidationGroup="FormValidationGroup"></asp:Button>&nbsp;
                                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CommandName="Cancel"></asp:Button>
                                <hr style="border: 3px solid gray;" />
                        </section>
                    </FormTemplate>
                </EditFormSettings>                 
            </MasterTableView>
            <ClientSettings AllowKeyboardNavigation="true">    
                <Selecting AllowRowSelect="True" UseClientSelectColumnOnly="True"></Selecting>
            </ClientSettings>
            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
        </telerik:RadGrid>
    </div>
</asp:Content>

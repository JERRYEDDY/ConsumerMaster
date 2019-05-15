<%@ Page Title="RenderingProviders" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RenderingProviders.aspx.cs" Inherits="ConsumerMaster.RenderingProviders" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<link href="Styles/Flexbox.css" rel="stylesheet" />
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
    <h4>Rendering Providers</h4>
    <div id="demo" class="demo-container no-bg">
        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowPaging="True" ShowFooter="true" AllowSorting="True" AutoGenerateColumns="False" ShowStatusBar="true" PageSize="12" 
            OnNeedDataSource="RadGrid1_NeedDataSource" OnInsertCommand="RadGrid1_InsertCommand" OnUpdateCommand="RadGrid1_UpdateCommand" OnDeleteCommand="RadGrid1_DeleteCommand">
            <PagerStyle Mode="NextPrevAndNumeric" />
            <MasterTableView CommandItemDisplay="Top" Name="RenderingProviders" DataKeyNames="id" EditMode="EditForms">
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
                    <telerik:GridBoundColumn DataField="first_name" HeaderText="First Name" SortExpression="first_name" UniqueName="first_name">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage="This field first_name is required" />
                            <ModelErrorMessage BackColor="Red" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="last_name" HeaderText="Last Name" SortExpression="last_name" UniqueName="last_name">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage="This field last_name is required" />
                            <ModelErrorMessage BackColor="Red" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ma_number" HeaderText="MA Number" SortExpression="ma_number" UniqueName="ma_number">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage="This field ma_number is required" />
                            <ModelErrorMessage BackColor="Red" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>     
                    <telerik:GridBoundColumn DataField="npi_number" HeaderText="NPI Number" SortExpression="npi_number" UniqueName="npi_number">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage="This field npi_number is required" />
                            <ModelErrorMessage BackColor="Red" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn DataField="name" HeaderText="Name" SortExpression="name" UniqueName="name" ReadOnly="True">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage="This field name is required" />
                            <ModelErrorMessage BackColor="Red" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ConfirmText="Delete this Referring Provider record?" Text="Delete" CommandName="Delete" />
                </Columns>   
                <EditFormSettings EditFormType="Template">
                    <FormTemplate>
                        <section class="form-group">
                            <h4>Referring Provider Info</h4>
                            <p id="formInstructions">Fields marked with an asterisk (*) are required.</p>
                            <ul class="form-fields">
                            <li>
                                <label for="first_name">First Name<em>*</em></label> 
                                <telerik:RadTextBox ID="first_name" runat="server" Text='<%# Bind("first_name") %>' TabIndex="1"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="first_name" ErrorMessage="First name is required"  ValidationGroup="FormValidationGroup" />
                            </li>
                            <li>
                                <label for="last_name">Last Name *</label> 
                                <telerik:RadTextBox ID="last_name" runat="server" Text='<%# Bind("last_name") %>' TabIndex="2"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ControlToValidate="last_name" ErrorMessage="Last name is required"  ValidationGroup="FormValidationGroup" />
                            </li>
                            <li>
                                <label for="ma_number">MA Number *</label> 
                                <telerik:RadMaskedTextBox ID="ma_number" Text='<%# Bind("ma_number") %>' runat="server" TabIndex="3" MaxLength="13" Mask="#############" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ControlToValidate="ma_number" ErrorMessage="MA number is required"  ValidationGroup="FormValidationGroup"/>
                            </li>
                            <li>
                                <label for="npi_number">NPI Number:</label> 
                                <telerik:RadMaskedTextBox ID="npi_number" Text='<%# Bind("npi_number") %>' runat="server" TabIndex="4" MaxLength="10" Mask="##########"/>
                            </li>
                        </section>
                        <section class="form-submit">
                                <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>' runat="server" CausesValidation="True"
                                      CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' ValidationGroup="FormValidationGroup"></asp:Button>&nbsp;
                                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CommandName="Cancel"></asp:Button>
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

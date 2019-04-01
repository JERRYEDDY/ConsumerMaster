<%@ Page Title="Physicians" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Physicians.aspx.cs" Inherits="ConsumerMaster.Physicians" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<link href="Styles/Flexbox.css" rel="stylesheet" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
<%--    <telerik:RadFormDecorator RenderMode="Lightweight" runat="server" DecorationZoneID="demo" EnableRoundedCorners="false" DecoratedControls="All" />   
    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="demo" DecoratedControls="All" EnableRoundedCorners="false" />--%>
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
<%--    DataSourceID="SqlDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True" OnItemDeleted="RadGrid1_ItemDeleted" OnItemInserted="RadGrid1_ItemInserted" OnItemUpdated="RadGrid1_ItemUpdated" --%>
    <h4>Physicians</h4>
    <div id="demo" class="demo-container no-bg">
        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowPaging="True" ShowFooter="true" AllowSorting="True" AutoGenerateColumns="False" ShowStatusBar="true" PageSize="12" 
            OnNeedDataSource="RadGrid1_NeedDataSource" OnInsertCommand="RadGrid1_InsertCommand" OnUpdateCommand="RadGrid1_UpdateCommand" OnDeleteCommand="RadGrid1_DeleteCommand">
            <PagerStyle Mode="NextPrevAndNumeric" />
            <MasterTableView CommandItemDisplay="Top" Name="Physicians" DataKeyNames="id" EditMode="EditForms">
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
<%--                    <telerik:GridBoundColumn DataField="id" HeaderText="id" SortExpression="id" UniqueName="id" ReadOnly="true" />--%>
                    <telerik:GridBoundColumn DataField="referring_provider_id" HeaderText="referring_provider_id" SortExpression="referring_provider_id" UniqueName="referring_provider_id">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage="This field referring_provider_id is required" />
                            <ModelErrorMessage BackColor="Red" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="referring_provider_first_name" HeaderText="referring_provider_first_name" SortExpression="referring_provider_first_name" UniqueName="referring_provider_first_name">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage="This field referring_provider_first_name is required" />
                            <ModelErrorMessage BackColor="Red" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="referring_provider_last_name" HeaderText="referring_provider_last_name" SortExpression="referring_provider_last_name" UniqueName="referring_provider_last_name">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage="This field referring_provider_last_name is required" />
                            <ModelErrorMessage BackColor="Red" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="referring_provider_name" HeaderText="referring_provider_name" SortExpression="referring_provider_name" UniqueName="referring_provider_name" ReadOnly="True">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage="This field referring_provider_name is required" />
                            <ModelErrorMessage BackColor="Red" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="referring_provider_npi" HeaderText="referring_provider_npi" SortExpression="referring_provider_npi" UniqueName="referring_provider_npi">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage="This field referring_provider_npi is required" />
                            <ModelErrorMessage BackColor="Red" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ConfirmText="Delete this Physician record?" Text="Delete" CommandName="Delete" />
                </Columns>   
                <EditFormSettings EditFormType="Template">
                    <FormTemplate>
                        <section class="form-group">
                            <h4>Physician Info</h4>
                            <ul class="form-fields">
                            <li>
                                <label for="referring_provider_id">Provider Id:</label> 
                                <telerik:RadTextBox ID="referring_provider_id" runat="server" Text='<%# Bind("referring_provider_id") %>' TabIndex="1"/>
                                <asp:RequiredFieldValidator ID="TextBoxRequiredFieldValidator" runat="server" Display="Dynamic" ControlToValidate="referring_provider_id" ErrorMessage="Referring provider id is required" ValidationGroup="FormValidationGroup"/>
                            </li>
                            <li>
                                <label for="referring_provider_first_name">Provider First Name :</label> 
                                <telerik:RadTextBox ID="referring_provider_first_name" runat="server" Text='<%# Bind("referring_provider_first_name") %>' TabIndex="2"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="referring_provider_first_name" ErrorMessage="Referring provider first name is required"  ValidationGroup="FormValidationGroup" />
                            </li>
                            <li>
                                <label for="referring_provider_last_name">Provider Last Name :</label> 
                                <telerik:RadTextBox ID="referring_provider_last_name" runat="server" Text='<%# Bind("referring_provider_last_name") %>' TabIndex="2"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ControlToValidate="referring_provider_last_name" ErrorMessage="Referring provider last name is required"  ValidationGroup="FormValidationGroup" />
                            </li>
                            <li>
                                <label for="referring_provider_npi">Provider NPI:</label> 
                                <telerik:RadTextBox ID="referring_provider_npi" Text='<%# Bind( "referring_provider_npi") %>' runat="server" TabIndex="4"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ControlToValidate="referring_provider_npi" ErrorMessage="Referring Provider NPI is required"  ValidationGroup="FormValidationGroup"/>
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
<%--    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>"
        DeleteCommand="DELETE FROM Physicians WHERE id = @id"
        InsertCommand="INSERT INTO Physicians (rendering_provider_id, rendering_provider_name, rendering_provider_first_name, rendering_provider_last_name, rendering_provider_npi) VALUES (@rendering_provider_id, @rendering_provider_first_name, @rendering_provider_last_name, @rendering_provider_npi)"
        SelectCommand="SELECT * FROM Physicians" 
        UpdateCommand="UPDATE Physicians SET rendering_provider_id = @rendering_provider_id, rendering_provider_first_name = @rendering_provider_first_name, rendering_provider_last_name = @rendering_provider_last_name, rendering_provider_npi = @rendering_provider_npi WHERE id = @id">
        <DeleteParameters>
            <asp:Parameter Name="id " Type="Int32"></asp:Parameter>
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="rendering_provider_id" Type="String"></asp:Parameter>
            <asp:Parameter Name="rendering_provider_first_name" Type="String"></asp:Parameter>
            <asp:Parameter Name="rendering_provider_last_name" Type="String"></asp:Parameter>
            <asp:Parameter Name="rendering_provider_npi" Type="String"></asp:Parameter>
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="rendering_provider_id" Type="String"></asp:Parameter>
            <asp:Parameter Name="rendering_provider_first_name" Type="String"></asp:Parameter>
            <asp:Parameter Name="rendering_provider_last_name" Type="String"></asp:Parameter>
            <asp:Parameter Name="rendering_provider_npi" Type="String"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>  --%>
</asp:Content>

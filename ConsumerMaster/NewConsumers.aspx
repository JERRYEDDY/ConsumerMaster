﻿<%@ Page Title="NewConsumers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewConsumers.aspx.cs" Inherits="ConsumerMaster.NewConsumers" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<link href="Styles/Flexbox.css" rel="stylesheet" />
<script type="text/javascript" language="javascript"> 
    function RowClick(sender, eventArgs)  
    {  
        var tableView = eventArgs.get_tableView();   
        var rowIndex = eventArgs.get_itemIndexHierarchical(); 
 
        tableView.editItem(rowIndex); 
    } 
</script>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server"></telerik:RadWindowManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadFormDecorator RenderMode="Lightweight" ID="FormDecorator1" runat="server" DecoratedControls="all" DecorationZoneID="decorationZone" />
    <div class="demo-container no-bg">
        <h5><strong>NEW CONSUMERS:</strong></h5>
        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowPaging="true" PageSize="10"  AutoGenerateColumns="False" 
                         OnNeedDataSource="RadGrid1_NeedDataSource" OnInsertCommand="RadGrid1_InsertCommand" OnUpdateCommand="RadGrid1_UpdateCommand" OnDeleteCommand="RadGrid1_DeleteCommand"                        
                         AllowFilteringByColumn="true" AllowSorting="true">
            <MasterTableView CommandItemDisplay="Top" Name="Consumers" DataKeyNames="consumer_internal_number" EditMode="EditForms">
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
                <DetailTables>
                    <telerik:GridTableView DataKeyNames="consumer_internal_number" DataSourceID="SqlDataSource2" Width="100%" runat="server" CommandItemDisplay="Top" Name="TradingPartners" Caption="Trading Partners" AllowFilteringByColumn="false">
                        <ParentTableRelation>
                            <telerik:GridRelationFields DetailKeyField="consumer_internal_number" MasterKeyField="consumer_internal_number"></telerik:GridRelationFields>
                        </ParentTableRelation>
                        <Columns>
                            <telerik:GridBoundColumn DataField="consumer_internal_number" UniqueName="consumer_internal_number" HeaderText="CIN" ReadOnly="true" HeaderStyle-Width="50px" ItemStyle-Width="50px" AllowFiltering="false" />
                            <telerik:GridDropDownColumn DataField="trading_partner_id" UniqueName="trading_partner_name" ListTextField="name" ListValueField="trading_partner_id" DataSourceID="TradingPartnerDataSource" HeaderText="Trading Partner"
                                                        DropDownControlType="RadComboBox" AllowSorting="true" ItemStyle-Width="300px" >
                            </telerik:GridDropDownColumn>
                            <telerik:GridButtonColumn ConfirmText="Delete these details record?" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn2">
                                <HeaderStyle Width="20px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton"></ItemStyle>
                            </telerik:GridButtonColumn>
                        </Columns>                        
                    </telerik:GridTableView> 
                </DetailTables>
                <Columns>
                    <telerik:GridBoundColumn DataField="consumer_internal_number" HeaderText="CIN" ReadOnly="true" HeaderStyle-Width="50px" ItemStyle-Width="50px" AllowFiltering="false" />
                    <telerik:GridBoundColumn DataField="consumer_first" HeaderText="First Name" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="120px" ItemStyle-Width="120px" AllowFiltering="false"/>
                    <telerik:GridBoundColumn DataField="consumer_last" HeaderText="Last Name" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="120px" ItemStyle-Width="120px" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" ShowFilterIcon="False" />
                    <telerik:GridBoundColumn DataField="city" HeaderText="City" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="150px" ItemStyle-Width="150px" AllowFiltering="false" />                        
                    <telerik:GridBoundColumn DataField="state" HeaderText="State" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="50px" ItemStyle-Width="50px" AllowFiltering="false" />
                    <telerik:GridBoundColumn DataField="identifier" HeaderText="Identifier" ColumnEditorID="GridTextBoxEditor" AllowFiltering="false" />                       
                    <telerik:GridBoundColumn DataField="diagnosis" HeaderText="Diagnosis" ColumnEditorID="GridTextBoxEditor" AllowFiltering="false" />
                    <telerik:GridButtonColumn ConfirmText="Delete this consumer?" ConfirmDialogType="RadWindow" ConfirmTitle="Delete" ButtonType="FontIconButton" CommandName="Delete" />
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <FormTemplate>
                        <section class="form-group">
                            <h4>Consumer Info</h4>
                            <p id="formInstructions">Fields marked with an asterisk (*) are required.</p>
                            <ul class="form-fields">
                            <li>
                                <label for="consumer_first">First Name *</label> 
                                <telerik:RadTextBox ID="consumer_first" runat="server" Text='<%# Bind("consumer_first") %>' TabIndex="1"/>
                                <asp:RequiredFieldValidator ID="TextBoxRequiredFieldValidator" runat="server" Display="Dynamic" ControlToValidate="consumer_first" ErrorMessage="Consumer first name is required" ValidationGroup="FormValidationGroup"/>
                            </li>
                            <li>
                                <label for="consumer_last">Last name *</label> 
                                <telerik:RadTextBox ID="consumer_last" runat="server" Text='<%# Bind("consumer_last") %>' TabIndex="2"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="consumer_last" ErrorMessage="Consumer last name is required"  ValidationGroup="FormValidationGroup" />
                            </li>
                            <li>
                                <label for="date_of_birth">Birth Date *</label> 
                                <telerik:RadDatePicker ID="date_of_birth" runat="server" MinDate="1/1/1900" DbSelectedDate='<%# Bind("date_of_birth") %>' TabIndex="3"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ControlToValidate="date_of_birth" ErrorMessage="Date of birth is required"  ValidationGroup="FormValidationGroup"/>
                            </li>
                            <li>
                                <label for="address_line_1">Address1 *</label> 
                                <telerik:RadTextBox ID="address_line_1" Text='<%# Bind( "address_line_1") %>' runat="server" TabIndex="4"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ControlToValidate="address_line_1" ErrorMessage="Address1 is required"  ValidationGroup="FormValidationGroup"/>
                            </li>                                    
                            <li>
                                <label for="address_line_2">Address2:</label> 
                                <telerik:RadTextBox ID="address_line_2" Text='<%# Bind( "address_line_2") %>' runat="server" TabIndex="5"/>
                            </li>                                   
                            <li>
                                <label for="city">City *</label> 
                                <telerik:RadTextBox ID="city" Text='<%# Bind("city") %>' runat="server" TabIndex="6"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="Dynamic" ControlToValidate="city" ErrorMessage="City is required"  ValidationGroup="FormValidationGroup"/>
                            </li>
                            <li>
                                <label for="state">State *</label> 
                                <telerik:RadComboBox runat="server" ID="state" DataSourceID="StatesSqlDataSource" SelectedValue='<%# Bind("state") %>' 
                                                     DataTextField="name" DataValueField="abbreviation" TabIndex="7" EmptyMessage="Select" Height="200" />
                                <asp:RequiredFieldValidator ID="Validator" ControlToValidate="state" ErrorMessage="State is required" runat="server" Display="Dynamic"  ValidationGroup="FormValidationGroup"/>
                            </li>                                    
                            <li>
                                <label for="zip_code">Zip Code *</label> 
                                <telerik:RadMaskedTextBox ID="zip_code" Text='<%# Bind("zip_code") %>' runat="server" TabIndex="8" Mask="#####-####" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic" ControlToValidate="zip_code" ErrorMessage="Zip code is required"  ValidationGroup="FormValidationGroup"/>
                            </li>  
                            <li>
                                <label for="identifier">Identifier *</label> 
                                <telerik:RadMaskedTextBox ID="identifier" Text='<%# Bind("identifier") %>' runat="server" TabIndex="8" Mask="##########" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Display="Dynamic" ControlToValidate="identifier" ErrorMessage="Identifier is required" />
                            </li>                                     
                            <li>
                                <label for="gender">Gender *</label> 
                                <telerik:RadRadioButtonList ID="gender" runat="server" Layout="Flow" Columns="2" SelectedValue='<%# Bind("gender") %>' TabIndex="10" ValidationGroup="GenderGroup" Direction="Horizontal">
                                    <Items>
                                        <telerik:ButtonListItem Text="Male" Value="M"/>
                                        <telerik:ButtonListItem Text="Female" Value="F"/>
                                    </Items>
                                </telerik:RadRadioButtonList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="gender"  Display="Dynamic" ErrorMessage="Gender is required" ValidationGroup="GenderGroup" />
                            </li>                                      
                            <li>
                                <label for="diagnosis_code">Diagnosis Code *</label> 
                                <telerik:RadTextBox ID="diagnosis_code" Text='<%# Bind("diagnosis") %>' runat="server" TabIndex="11"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" Display="Dynamic" ControlToValidate="diagnosis_code" ErrorMessage="Diagnosis code is required" ValidationGroup="FormValidationGroup" />
                            </li>  
                            <li>
                                <label for="nickname_first">Nickname First:</label> 
                                <telerik:RadTextBox ID="nickname_first" Text='<%# Bind("nickname_first") %>' runat="server" TabIndex="12"/>
                            </li>                                      
                            <li>
                                <label for="nickname_last">Nickname Last:</label> 
                                <telerik:RadTextBox ID="nickname_last" Text='<%# Bind("nickname_last") %>' runat="server" TabIndex="13"/>
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
            <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">    
                <Selecting AllowRowSelect="True" UseClientSelectColumnOnly="True"/>
                <ClientEvents OnRowClick="RowClick" /> 
            </ClientSettings>
            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
            <GroupingSettings CaseSensitive="false"></GroupingSettings>
        </telerik:RadGrid>
        <br />
        <br />
        <br />
        <br />
    </div>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
       ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>"
       DeleteCommand="DELETE FROM ConsumerTradingPartner WHERE consumer_internal_number = @consumer_internal_number AND trading_partner_id = @trading_partner_id"
       InsertCommand="INSERT INTO ConsumerTradingPartner (consumer_internal_number, trading_partner_id) VALUES (@consumer_internal_number, @trading_partner_id)"
       SelectCommand="SELECT ctp.consumer_internal_number, ctp.trading_partner_id, tp.name AS trading_partner_name FROM ConsumerTradingPartner ctp INNER JOIN TradingPartners tp ON ctp.trading_partner_id = tp.id WHERE consumer_internal_number = @consumer_internal_number">
       <SelectParameters>
            <asp:ControlParameter ControlID="RadGrid1" Name="consumer_internal_number" PropertyName="SelectedValues['consumer_internal_number']" Type="Int32" />
       </SelectParameters>
       <DeleteParameters>
           <asp:Parameter Name="trading_partner_id" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="consumer_internal_number" Type="Int32"></asp:Parameter>
       </DeleteParameters>
       <InsertParameters>
           <asp:ControlParameter ControlID="RadGrid1" Name="consumer_internal_number" PropertyName="SelectedValues['consumer_internal_number']" Type="Int32" />
            <asp:Parameter Name="trading_partner_id" Type="Int32"></asp:Parameter>
       </InsertParameters>
    </asp:SqlDataSource> 

<asp:SqlDataSource ID="StatesSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT name, abbreviation FROM States"/>
<asp:SqlDataSource ID="TradingPartnerDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id AS trading_partner_id, name FROM TradingPartners"/>
<asp:SqlDataSource ID="PartnerProgramDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id, partner_name, program_name FROM PartnerPrograms2"/>
<asp:SqlDataSource ID="CompositeProcedureCodeDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id, name FROM CompositeProcedureCodes"/>
</asp:Content>

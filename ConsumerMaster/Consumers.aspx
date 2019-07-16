<%@ Page Title="Consumers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Consumers.aspx.cs" Inherits="ConsumerMaster.Consumers" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Styles/Flexbox.css" rel="stylesheet" />
    <style type="text/css">
        .textBoxLabel {
            display: block !important;
        }
        .header {
            padding: 5px;
            text-align: center;
            background: #1abc9c;

        }
    </style>
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
        <h5><strong>CONSUMERS:</strong></h5>
        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowPaging="true" PageSize="15"  AutoGenerateColumns="False" 
                         OnNeedDataSource="RadGrid1_NeedDataSource" OnInsertCommand="RadGrid1_InsertCommand" OnUpdateCommand="RadGrid1_UpdateCommand" OnDeleteCommand="RadGrid1_DeleteCommand"                        
                         AllowFilteringByColumn="true" AllowSorting="true">
            <MasterTableView CommandItemDisplay="Top" Name="Consumers" DataKeyNames="consumer_internal_number" EditMode="EditForms">
                <CommandItemTemplate>
                    <table>
                        <tr>
                            <td width="30%">
                                <telerik:RadButton ID="RadButton1" runat="server" Text="Add new" Skin="Default" RenderMode="Lightweight" CommandName="InitInsert">
                                    <Icon PrimaryIconCssClass="rbAdd" />
                                </telerik:RadButton>
                            </td>
                            <td width="40%">
                                <telerik:RadButton ID="RadButton2" runat="server" Text="Edit selected" Skin="Default" RenderMode="Lightweight" CommandName="EditSelected">
                                    <Icon PrimaryIconCssClass="rbEdit" />
                                </telerik:RadButton>
                            </td>
                            <td width="30%">
                                <telerik:RadButton ID="RadButton3" runat="server" Text="Refresh" Skin="Default" RenderMode="Lightweight" CommandName="RebindGrid">
                                    <Icon PrimaryIconCssClass="rbRefresh" />
                                </telerik:RadButton>
                            </td>
                        </tr>
                    </table>                   
                </CommandItemTemplate>    
                <ColumnGroups>
                    <telerik:GridColumnGroup HeaderText="Trading" Name="Trading" HeaderStyle-HorizontalAlign="Center"/>
                </ColumnGroups>                
                <Columns>
                    <telerik:GridBoundColumn DataField="consumer_internal_number" HeaderText="CIN" ReadOnly="true" HeaderStyle-Width="50px" ItemStyle-Width="50px" AllowFiltering="false" />
                    <telerik:GridBoundColumn DataField="consumer_first" HeaderText="First Name" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="170px" ItemStyle-Width="170px" AllowFiltering="false"/>
                    <telerik:GridBoundColumn DataField="consumer_last" HeaderText="Last Name" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="170px" ItemStyle-Width="170px" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" ShowFilterIcon="False" />
                    <telerik:GridBoundColumn DataField="identifier" HeaderText="Identifier" ColumnEditorID="GridTextBoxEditor" AllowFiltering="false" />                       
                    <telerik:GridBoundColumn DataField="tpName1" ColumnGroupName="Trading" HeaderText="Partner1" ColumnEditorID="GridTextBoxEditor" AllowFiltering="false" />                    
                    <telerik:GridBoundColumn DataField="tpName2" ColumnGroupName="Trading" HeaderText="Partner2" ColumnEditorID="GridTextBoxEditor" AllowFiltering="false" />    
                    <telerik:GridBoundColumn DataField="tpName3" ColumnGroupName="Trading" HeaderText="Partner3" ColumnEditorID="GridTextBoxEditor" AllowFiltering="false" /> 
                    <telerik:GridBoundColumn DataField="rpName" HeaderText="Referring" ColumnEditorID="GridTextBoxEditor" AllowFiltering="false" /> 
                    <telerik:GridButtonColumn ConfirmText="Delete this consumer?" ConfirmDialogType="RadWindow" ConfirmTitle="Delete" ButtonType="FontIconButton" CommandName="Delete" />
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <FormTemplate>
                        <section class="form-group">
                            <h4><b>Consumer Info</b></h4>
                            <hr style="border: 3px solid gray;" />
                            <p id="formInstructions"><mark>Fields marked with an asterisk (*) are required.</mark></p>
                            <ul class="form-fields">
                            <li>
                                <label>First name *</label>
                                <telerik:RadTextBox ID="consumer_first" runat="server" Text='<%# Bind("consumer_first") %>' TabIndex="1" Width="315"/>
                                <asp:RequiredFieldValidator ID="TextBoxRequiredFieldValidator" runat="server" ForeColor="red" Display="Dynamic" ControlToValidate="consumer_first" ErrorMessage="Consumer first name is required" ValidationGroup="FormValidationGroup"/>
                            </li>
                            <li>
                                <label>Last name *</label> 
                                <telerik:RadTextBox ID="consumer_last" runat="server" Text='<%# Bind("consumer_last") %>' TabIndex="2" Width="315"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="red" Display="Dynamic" ControlToValidate="consumer_last" ErrorMessage="Consumer last name is required"  ValidationGroup="FormValidationGroup" />
                            </li>
                            <li>
                                <label>Birth date *</label>
                                <telerik:RadDatePicker ID="date_of_birth" runat="server" MinDate="1/1/1900" DbSelectedDate='<%# Bind("date_of_birth") %>' TabIndex="3" Width="100"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="red" Display="Dynamic" ControlToValidate="date_of_birth" ErrorMessage="Date of birth is required"  ValidationGroup="FormValidationGroup"/>
                            </li>
                            <li>
                                <label>Address1 *</label> 
                                <telerik:RadTextBox ID="address_line_1" Text='<%# Bind( "address_line_1") %>' runat="server" TabIndex="4" Width="315"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="red" Display="Dynamic" ControlToValidate="address_line_1" ErrorMessage="Address1 is required"  ValidationGroup="FormValidationGroup"/>
                            </li>                                    
                            <li>
                                <label>Address2:</label> 
                                <telerik:RadTextBox ID="address_line_2" Text='<%# Bind( "address_line_2") %>' runat="server" TabIndex="5" Width="315" />
                            </li>                                   
                            <li>
                                <label>City *</label> 
                                <telerik:RadTextBox ID="city" Text='<%# Bind("city") %>' runat="server" TabIndex="6"  />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ForeColor="red" Display="Dynamic" ControlToValidate="city" ErrorMessage="City is required"  ValidationGroup="FormValidationGroup"/>
                            </li>
                            <li>
                                <label>State *</label>
                                <telerik:RadComboBox runat="server" ID="state" DataSourceID="StatesSqlDataSource" SelectedValue='<%# Bind("state") %>' 
                                                     DataTextField="name" DataValueField="abbreviation" TabIndex="7" EmptyMessage="Select" Height="200" />
                                <asp:RequiredFieldValidator ID="Validator" ControlToValidate="state" ErrorMessage="State is required" runat="server" ForeColor="red" Display="Dynamic"  ValidationGroup="FormValidationGroup"/>
                            </li>                                    
                            <li>
                                <label>Zip Code *</label> 
                                <telerik:RadMaskedTextBox ID="zip_code" Text='<%# Bind("zip_code") %>' runat="server" TabIndex="8" Mask="#####-####" Width="75"  />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ForeColor="red" Display="Dynamic" ControlToValidate="zip_code" ErrorMessage="Zip code is required"  ValidationGroup="FormValidationGroup"/>
                            </li>  
                            <li>
                                <label>Identifier *</label> 
                                <telerik:RadMaskedTextBox ID="identifier" Text='<%# Bind("identifier") %>' runat="server" TabIndex="8" Mask="##########" Width="75"  />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ForeColor="red" Display="Dynamic" ControlToValidate="identifier" ErrorMessage="Identifier is required" ValidationGroup="FormValidationGroup"/>
                                <asp:RegularExpressionValidator Display="Dynamic" ID="MaskedTextBoxRegularExpressionValidator" runat="server" ErrorMessage="10 digit number is required" 
                                                                ControlToValidate="identifier" ValidationExpression="^\d{10}$"/>
                            </li>                                     
                            <li>
                                <label>Gender *</label>
                                <telerik:RadRadioButtonList ID="gender" runat="server"  SelectedValue='<%# Bind("gender") %>' TabIndex="10" ValidationGroup="GenderGroup" Direction="Vertical" CssClass="RBL">
                                    <Items>
                                        <telerik:ButtonListItem Text="Male" Value="M"/>
                                        <telerik:ButtonListItem Text="Female" Value="F"/>
                                    </Items>
                                </telerik:RadRadioButtonList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="gender" ForeColor="red" Display="Dynamic" ErrorMessage="Gender is required"  ValidationGroup="FormValidationGroup" />
                            </li>                                      
                            <li>
                                <label>Diagnosis Code *</label> 
                                <telerik:RadTextBox ID="diagnosis_code" Text='<%# Bind("diagnosis") %>' runat="server" TabIndex="11" Width="55"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ForeColor="red" Display="Dynamic" ControlToValidate="diagnosis_code" ErrorMessage="Diagnosis code is required" ValidationGroup="FormValidationGroup" />
                            </li>  
                            <li>
                                <label>Trading Partner 1 *</label> 
                                <telerik:RadComboBox ID="cbTradingPartner1" runat="server" SelectedValue='<%# Bind("tpId1") %>' EmptyMessage="Select" DataSourceID="TradingPartnerDataSource" DataTextField="name" DataValueField="trading_partner_id" AppendDataBoundItems="true" Width="315" DropDownWidth="315">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="None" Value="0"  runat="server"/>
                                    </Items>
                                </telerik:RadComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor="red" Display="Dynamic" ControlToValidate="cbTradingPartner1" ErrorMessage="Trading partner 1 is required" ValidationGroup="FormValidationGroup" />
                            </li>                                      
                            <li>
                                <label>Trading Partner 2:</label> 
                                <telerik:RadComboBox ID="cbTradingPartner2" runat="server" SelectedValue='<%# Bind("tpId2") %>' EmptyMessage="Select" DataSourceID="TradingPartnerDataSource" DataTextField="name" DataValueField="trading_partner_id" AppendDataBoundItems="true" Width="315" DropDownWidth="315">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="None" Value="0"  runat="server"/>
                                    </Items>
                                </telerik:RadComboBox>
                            </li>  
                            <li>
                                <label>Trading Partner 3:</label> 
                                <telerik:RadComboBox ID="cbTradingPartner3" runat="server" SelectedValue='<%# Bind("tpId3") %>' EmptyMessage="Select" DataSourceID="TradingPartnerDataSource" DataTextField="name" DataValueField="trading_partner_id" AppendDataBoundItems="true" Width="315" DropDownWidth="315">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="None" Value="0"  runat="server"/>
                                    </Items>
                                </telerik:RadComboBox>                                
                            </li> 
                            <p id="formInstructions2"><mark>Referring providers are required for Early Intervention Direct Therapy Consumers only.</mark></p>
                            <li>
                                <label>Referring Provider:</label> 
                                <telerik:RadComboBox ID="cbReferringProvider" runat="server" SelectedValue='<%# Bind("referring_provider_id") %>' EmptyMessage="Select" DataSourceID="ReferringProviderDataSource" DataTextField="name" DataValueField="id" AppendDataBoundItems="true" Width="315" DropDownWidth="315">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="None" Value="0"  runat="server"/>
                                    </Items>
                                </telerik:RadComboBox>                                
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
<asp:SqlDataSource ID="TradingPartnerDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id, short_name FROM TradingPartners"/>
<asp:SqlDataSource ID="PartnerProgramDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id, partner_name, program_name FROM PartnerPrograms2"/>
<asp:SqlDataSource ID="CompositeProcedureCodeDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id, name FROM CompositeProcedureCodes"/>
<asp:SqlDataSource ID="ReferringProviderDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id, name FROM ReferringProviders ORDER BY name"></asp:SqlDataSource>

</asp:Content>

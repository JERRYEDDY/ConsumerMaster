<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Consumers2.aspx.cs" Inherits="ConsumerMaster.Consumers2" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" Skin="Office2010Silver" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <div class="demo-container no-bg">
        <h5><strong>CONSUMERS:</strong></h5>
        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowPaging="true" PageSize="10" DataSourceID="SqlDataSource1" AutoGenerateColumns="False" AllowAutomaticDeletes="True" 
                         AllowAutomaticInserts="True" AllowAutomaticUpdates="True" OnItemDeleted="RadGrid1_ItemDeleted" OnItemInserted="RadGrid1_ItemInserted" OnItemUpdated="RadGrid1_ItemUpdated" 
                         OnInsertCommand="RadGrid1_InsertCommand" AllowFilteringByColumn="true" >
            <MasterTableView CommandItemDisplay="Top" Name="Consumers" DataSourceID="SqlDataSource1" DataKeyNames="consumer_internal_number">
                <Columns>
                    <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" HeaderStyle-Width="50px" ItemStyle-Width="50px"/>
                    <telerik:GridBoundColumn DataField="consumer_internal_number" HeaderText="No." ReadOnly="true" HeaderStyle-Width="50px" ItemStyle-Width="50px" AllowFiltering="false" />
                    <telerik:GridBoundColumn DataField="consumer_first" HeaderText="First Name" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="120px" ItemStyle-Width="120px" AllowFiltering="false"/>
                    <telerik:GridBoundColumn DataField="consumer_last" HeaderText="Last Name" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="120px" ItemStyle-Width="120px" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" ShowFilterIcon="False" />
                    <telerik:GridBoundColumn DataField="city" HeaderText="City" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="150px" ItemStyle-Width="150px" AllowFiltering="false" />                        
                    <telerik:GridBoundColumn DataField="state" HeaderText="State" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="50px" ItemStyle-Width="50px" AllowFiltering="false" />
                    <telerik:GridBoundColumn DataField="identifier" HeaderText="Identifier" ColumnEditorID="GridTextBoxEditor" AllowFiltering="false" />                       
                    <telerik:GridBoundColumn DataField="diagnosis" HeaderText="Diagnosis" ColumnEditorID="GridTextBoxEditor" AllowFiltering="false" />
                    <telerik:GridClientDeleteColumn ConfirmText="Are you sure you want to delete the selected row?" HeaderText="Delete"><HeaderStyle Width="70px"/></telerik:GridClientDeleteColumn>       
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <FormTemplate>
                        <table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="0" rules="none" style="border-collapse: collapse;">
                            <tr class="EditFormHeader">
                                <td colspan="2">
                                    <b>Consumer Info</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="Table3" width="450px" border="0" class="module">
                                        <tr>
                                            <td>First Name:</td>
                                            <td>
                                                <telerik:RadTextBox ID="txtConsumerFirst" runat="server" Text='<%# Bind("consumer_first") %>' TabIndex="1"/>
                                                <asp:RequiredFieldValidator ID="TextBoxRequiredFieldValidator" runat="server" Display="Dynamic" ControlToValidate="txtConsumerFirst"
                                                                            ErrorMessage="Consumer first name is required" ValidationGroup="FormValidationGroup"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Last Name:</td>
                                            <td>
                                                <telerik:RadTextBox ID="txtConsumerLast" runat="server" Text='<%# Bind("consumer_last") %>' TabIndex="2"/>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="txtConsumerLast"
                                                                            ErrorMessage="Consumer last name is required"  ValidationGroup="FormValidationGroup" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Birth Date:</td>
                                            <td>
                                                <telerik:RadDatePicker ID="dpBirthDate" runat="server" MinDate="1/1/1900" DbSelectedDate='<%# Bind("date_of_birth") %>' TabIndex="3"/>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ControlToValidate="dpBirthDate"
                                                                            ErrorMessage="Date of birth is required"  ValidationGroup="FormValidationGroup"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Address1:</td>
                                            <td>
                                                <telerik:RadTextBox ID="txtAddress1" Text='<%# Bind( "address_line_1") %>' runat="server" TabIndex="4"/>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ControlToValidate="txtAddress1"
                                                                            ErrorMessage="Address1 is required"  ValidationGroup="FormValidationGroup"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Address2:</td>
                                            <td>
                                                <telerik:RadTextBox ID="txtAddress2" Text='<%# Bind( "address_line_2") %>' runat="server" TabIndex="5"/>
                                            </td>
                                        </tr>
                                        <tr>
                                        <tr>
                                            <td>City:</td>
                                            <td>
                                                <telerik:RadTextBox ID="txtCity" Text='<%# Bind("city") %>' runat="server" TabIndex="6"/>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="Dynamic" ControlToValidate="txtCity"
                                                                            ErrorMessage="City is required"  ValidationGroup="FormValidationGroup"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>State:</td>
                                            <td>
                                                <telerik:RadDropDownList ID="ddlStates" runat="server" DataSourceID="StatesSqlDataSource" SelectedValue='<%# Bind("state") %>'
                                                                         DataTextField="Name" DataValueField="Abbreviation" TabIndex="7" DefaultMessage="Select" /> 
                                                <asp:RequiredFieldValidator ID="Validator" ControlToValidate="ddlStates" 
                                                                            ErrorMessage="State is required" runat="server" Display="Dynamic"  ValidationGroup="FormValidationGroup"/>
                                            </td>
                                        </tr>               
                                        <tr>
                                            
                                            <td><telerik:RadLabel ID="RadLabel1" runat="server" Text="Zip Code:" /></td>
                                            <td>
                                                <telerik:RadMaskedTextBox ID="txtZipCode" Text='<%# Bind("zip_code") %>' runat="server" TabIndex="8" Mask="#####-####" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic" ControlToValidate="txtZipCode"
                                                                            ErrorMessage="Zip code is required"  ValidationGroup="FormValidationGroup"/>
                                            </td>
                                        </tr>               
                                        <tr>
                                            <td>Identifier:</td>
                                            <td>
<%--                                                <telerik:RadTextBox ID="txtIdentifer" Text='<%# Bind("identifier") %>' runat="server" TabIndex="9"/>--%>
                                                <telerik:RadMaskedTextBox ID="txtIdentifer" Text='<%# Bind("identifier") %>' runat="server" TabIndex="9" DisplayMask="##########" Mask="##########"/>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Display="Dynamic" ControlToValidate="txtIdentifer"
                                                                            ErrorMessage="Identifier is required" />
                                                <asp:RegularExpressionValidator ID="rvDigits" runat="server" ControlToValidate="txtIdentifer" 
                                                                                ErrorMessage="10 digit number is required" ValidationExpression="[0-9]{10}" />
                                            </td>
                                        </tr>              
                                        <tr>
                                            <td>Gender:</td>
                                            <td>
                                                <telerik:RadRadioButtonList ID="rblGender" runat="server" Layout="Flow" Columns="2" SelectedValue='<%# Bind("gender") %>' TabIndex="10" 
                                                                            ValidationGroup="GenderGroup" Direction="Horizontal">
                                                    <Items>
                                                        <telerik:ButtonListItem Text="Male" Value="M"/>
                                                        <telerik:ButtonListItem Text="Female" Value="F"/>
                                                    </Items>
                                                </telerik:RadRadioButtonList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="rblGender"  Display="Dynamic" TabIndex="11"
                                                                            ErrorMessage="Gender is required" ValidationGroup="GenderGroup" />
                                            </td>
                                        </tr>                
                                        <tr>
                                            <td>Diagnosis code:</td>
                                            <td>
                                                <telerik:RadTextBox ID="txtDiagnosis" Text='<%# Bind("diagnosis") %>' runat="server" TabIndex="12"/>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" Display="Dynamic" ControlToValidate="txtDiagnosis"
                                                                            ErrorMessage="Diagnosis code is required" ValidationGroup="FormValidationGroup" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Nickname First:</td>
                                            <td>
                                                <telerik:RadTextBox ID="txtNicknameFirst" Text='<%# Bind("nickname_first") %>' runat="server" TabIndex="13"/>
                                            </td>
                                        </tr>               
                                        <tr>
                                            <td>Nickname Last:</td>
                                            <td>
                                                <telerik:RadTextBox ID="txtNicknameLast" Text='<%# Bind("nickname_last") %>' runat="server" TabIndex="14"/>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="vertical-align: top">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>' runat="server" CausesValidation="True"
                                                CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' ValidationGroup="FormValidationGroup"></asp:Button>&nbsp;
                                                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CommandName="Cancel"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </FormTemplate>
                </EditFormSettings>              
            </MasterTableView>
            <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
        </telerik:RadGrid>
        <br />
        <br />
        <h5><strong>PARTNER/PROGRAM:</strong></h5>
        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid2" ShowStatusBar="true" runat="server" AllowPaging="True" PageSize="5" DataSourceID="SqlDataSource2">
            <MasterTableView Width="100%" AutoGenerateColumns="False" DataKeyNames="consumer_internal_number" DataSourceID="SqlDataSource2" CommandItemDisplay="Top">
                <Columns>
                    <telerik:GridEditCommandColumn UniqueName="EditCommandColumn2">
                        <HeaderStyle Width="20px"></HeaderStyle>
                        <ItemStyle CssClass="MyImageButton"></ItemStyle>
                    </telerik:GridEditCommandColumn>
                    <telerik:GridBoundColumn DataField="consumer_internal_number" HeaderText="No." ReadOnly="true" HeaderStyle-Width="50px" ItemStyle-Width="50px" AllowFiltering="false" />
                    <telerik:GridBoundColumn DataField="partner_name" HeaderText="Partner" HeaderStyle-Width="150px" ItemStyle-Width="150px" AllowFiltering="false" />                    
                    <telerik:GridBoundColumn DataField="program_name" HeaderText="Program" HeaderStyle-Width="150px" ItemStyle-Width="150px" AllowFiltering="false" />     
                    
                    


<%--                    <telerik:GridDropDownColumn UniqueName="TPDropDownListColumn" ListTextField="name" ListValueField="trading_partner_id" DataSourceID="TradingPartnerDataSource" 
                                    HeaderText="Trading Partner" DataField="trading_partner_id" DropDownControlType="RadComboBox" AllowSorting="true" HeaderStyle-Width="500px"/>--%>

                    <telerik:GridButtonColumn ConfirmText="Delete these details record?" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn2">
                        <HeaderStyle Width="20px"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton"></ItemStyle>
                    </telerik:GridButtonColumn>
                </Columns>
            </MasterTableView>
            <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
        </telerik:RadGrid>
        <br />
        <br />
    </div>
    
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>"
        DeleteCommand="DELETE FROM [Consumers] WHERE [consumer_internal_number] = @consumer_internal_number"
        InsertCommand="INSERT INTO [Consumers] ([consumer_first], [consumer_last], [date_of_birth], [address_line_1], [address_line_2], [city], [state], [zip_code], [identifier], [gender], 
        [diagnosis], [nickname_first], [nickname_last]) 
        VALUES (@consumer_first, @consumer_last, @date_of_birth, @address_line_1, @address_line_2, @city, @state, @zip_code, @identifier, @gender, @diagnosis, @nickname_first, @nickname_last)"
        SelectCommand="SELECT * FROM [Consumers] ORDER BY consumer_last" 
        UpdateCommand="UPDATE [Consumers] SET [consumer_first] = @consumer_first, [consumer_last] = @consumer_last, [date_of_birth] = @date_of_birth, [address_line_1] = @address_line_1, 
        [address_line_2] = @address_line_2, [city] = @city, [state] = @state, [zip_code] = @zip_code, [identifier] = @identifier, [gender] = @gender, [diagnosis] = @diagnosis, 
        [nickname_first] = @nickname_first, [nickname_last] = @nickname_last WHERE [consumer_internal_number] = @consumer_internal_number">
        <DeleteParameters>
            <asp:Parameter Name="consumer_internal_number" Type="Int32"></asp:Parameter>
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="consumer_first" Type="String"></asp:Parameter>
            <asp:Parameter Name="consumer_last" Type="String"></asp:Parameter>
            <asp:Parameter Name="date_of_birth" Type="DateTime"></asp:Parameter>
            <asp:Parameter Name="address_line_1" Type="String"></asp:Parameter>
            <asp:Parameter Name="address_line_2" Type="String"></asp:Parameter>
            <asp:Parameter Name="city" Type="String"></asp:Parameter>
            <asp:Parameter Name="state" Type="String"></asp:Parameter>
            <asp:Parameter Name="zip_code" Type="String"></asp:Parameter>
            <asp:Parameter Name="identifier" Type="String"></asp:Parameter>
            <asp:Parameter Name="gender" Type="String"></asp:Parameter>
            <asp:Parameter Name="diagnosis" Type="String"></asp:Parameter>
            <asp:Parameter Name="nickname_first" Type="String"></asp:Parameter>
            <asp:Parameter Name="nickname_last" Type="String"></asp:Parameter>
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="consumer_first" Type="String"></asp:Parameter>
            <asp:Parameter Name="consumer_last" Type="String"></asp:Parameter>
            <asp:Parameter Name="date_of_birth" Type="DateTime"></asp:Parameter>
            <asp:Parameter Name="address_line_1" Type="String"></asp:Parameter>
            <asp:Parameter Name="address_line_2" Type="String"></asp:Parameter>
            <asp:Parameter Name="city" Type="String"></asp:Parameter>
            <asp:Parameter Name="state" Type="String"></asp:Parameter>
            <asp:Parameter Name="zip_code" Type="String"></asp:Parameter>
            <asp:Parameter Name="identifier" Type="String"></asp:Parameter>
            <asp:Parameter Name="gender" Type="String"></asp:Parameter>
            <asp:Parameter Name="diagnosis" Type="String"></asp:Parameter>
            <asp:Parameter Name="nickname_first" Type="String"></asp:Parameter>
            <asp:Parameter Name="nickname_last" Type="String"></asp:Parameter>
            <asp:Parameter Name="consumer_internal_number" Type="Int32"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>        
   
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                       ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>"
                       DeleteCommand="DELETE FROM [ConsumerTradingPartner] WHERE [consumer_internal_number] = @consumer_internal_number"
                       InsertCommand="INSERT INTO [ConsumerTradingPartner] ([consumer_internal_number], [trading_partner_id]) VALUES (@consumer_internal_number, @trading_partner_id)"
                       SelectCommand="SELECT cpp.consumer_internal_number, pp.partner_name, pp.program_name FROM [ConsumerPartnerProgram] cpp INNER JOIN [PartnerPrograms2] pp ON cpp.partner_program_id = pp.id WHERE [consumer_internal_number] = @consumer_internal_number" 
                       UpdateCommand="UPDATE [ConsumerTradingPartner] SET [trading_partner_id] = @trading_partner_id WHERE [consumer_internal_number] = @consumer_internal_number">
        <SelectParameters>
            <asp:ControlParameter ControlID="RadGrid1" Name="consumer_internal_number" PropertyName="SelectedValues['consumer_internal_number']" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="consumer_internal_number" Type="Int32"></asp:Parameter>
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="consumer_internal_number" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="trading_partner_id" Type="Int32"></asp:Parameter>
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="trading_partner_id" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="consumer_internal_number" Type="Int32"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource> 


<asp:SqlDataSource ID="StatesSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT Name, Abbreviation FROM States"/>
<asp:SqlDataSource ID="TradingPartnerDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id AS trading_partner_id, name FROM TradingPartners"/>
<asp:SqlDataSource ID="PartnerProgramDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id, partner_name, program_name FROM PartnerPrograms2"/>
<asp:SqlDataSource ID="CompositeProcedureCodeDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id, name FROM CompositeProcedureCodes"/>
</asp:Content>

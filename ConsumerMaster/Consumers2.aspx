<%@ Page Title="Consumers2" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Consumers2.aspx.cs" Inherits="ConsumerMaster.Consumers2" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<style>
    .register-form {
        line-height: 1.4;
    }
    .form-group {
        background: #F6DDCE;
        margin-bottom:1em;
        padding: 10px;
    }
    .form-group ul {
        list-style: none;
        margin: 0 0 2em;
        padding: 0;
    }
    .form-group li {
        margin-bottom: 0.5em;
    }
    .form-group h3 {
        margin-bottom: 1em;
    }
    .form-fields input[type="text"],
    .form-fields input[type="tel"],
    .form-fields input[type="url"],
    .form-fields input[type="email"],
    .form-fields input[type="number"],
    .form-fields select {
        box-sizing: border-box;
        padding: 0.6em 0.8em;
        color: #999;
        background: #f7f7f7;
        border: 1px solid #e1e1e1;
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
        font-size: 0.9em;
        text-decoration: none;
        line-height: normal;
        max-height: 3em;
    }
    .form-fields input[type="text"]:focus,
    .form-fields input[type="tel"]:focus,
    .form-fields input[type="url"]:focus,
    .form-fields input[type="email"]:focus,
    .form-fields input[type="number"]:focus,
    .form-food textarea:focus,
    .form-fields select:focus {
        color: #333;
        border: 1px solid #C17CCF;
        outline: none;
        background: #f2f2f2;
    }
    .register-btn {
        border-radius: 0px 2px 2px 0px;
        box-sizing: content-box;
        background: #8B798C;
        font-weight: 300;
        text-transform: uppercase;
        color: white;
        padding: 0.35em 0.75em;
        border: none;
        font-size: 1.1em;
        text-decoration: none;
        cursor: pointer;	
    }
    .register-btn:hover, .register-btn:focus {
        background: #C17CCF;
    }
    /*flex it*/
    .form-fields li {
        display: flex;
        flex-wrap: wrap;
    }
    .form-fields input[type="text"],
    .form-fields input[type="tel"],
    .form-fields input[type="url"],
    .form-fields input[type="email"],
    .form-fields input[type="number"],
    .form-fields select {
        flex: 1 0 230px;
    }
    .form-fields label {
        flex: 1 0 90px;
        max-width: 150px;
    }

</style>

    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
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
    <telerik:RadFormDecorator RenderMode="Lightweight" ID="FormDecorator1" runat="server" DecoratedControls="all" DecorationZoneID="decorationZone" />
    <div class="demo-container no-bg">
        <h5><strong>CONSUMERS2:</strong></h5>
        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowPaging="true" PageSize="10"  AutoGenerateColumns="False" AllowFilteringByColumn="true" >
            <MasterTableView CommandItemDisplay="Top" Name="Consumers" DataSourceID="SqlDataSource1" DataKeyNames="consumer_internal_number">
                <Columns>
                    <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" HeaderStyle-Width="50px" ItemStyle-Width="50px"/>
                    <telerik:GridBoundColumn DataField="consumer_internal_number" HeaderText="CIN" ReadOnly="true" HeaderStyle-Width="50px" ItemStyle-Width="50px" AllowFiltering="false" />
                    <telerik:GridBoundColumn DataField="consumer_first" HeaderText="First Name" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="120px" ItemStyle-Width="120px" AllowFiltering="false"/>
                    <telerik:GridBoundColumn DataField="consumer_last" HeaderText="Last Name" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="120px" ItemStyle-Width="120px" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" ShowFilterIcon="False" />
                    <telerik:GridBoundColumn DataField="city" HeaderText="City" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="150px" ItemStyle-Width="150px" AllowFiltering="false" />                        
                    <telerik:GridBoundColumn DataField="state" HeaderText="State" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="50px" ItemStyle-Width="50px" AllowFiltering="false" />
                    <telerik:GridBoundColumn DataField="identifier" HeaderText="Identifier" ColumnEditorID="GridTextBoxEditor" AllowFiltering="false" />                       
                    <telerik:GridBoundColumn DataField="diagnosis" HeaderText="Diagnosis" ColumnEditorID="GridTextBoxEditor" AllowFiltering="false" />
                    <telerik:GridTemplateColumn HeaderText="Trading Partners">
                        <ItemTemplate>
                            <telerik:RadComboBox RenderMode="Lightweight" ID="RadComboBox1" runat="server" CheckBoxes="true" DropDownAutoWidth="Enabled" Enabled="False"
                                                 DataSourceID="TradingPartnerDataSource" DataTextField="name" DataValueField="trading_partner_id" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridClientDeleteColumn ConfirmText="Are you sure you want to delete the selected row?" HeaderText="Delete"><HeaderStyle Width="70px"/></telerik:GridClientDeleteColumn>       
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <FormTemplate>
                        <section class="form-group">
                            <h4>Consumer Info</h4>
                            <ul class="form-fields">
                            <li>
                                <label for="first_name">First Name:</label> 
                                <telerik:RadTextBox ID="first_name" runat="server" Text='<%# Bind("consumer_first") %>'/>
                                <asp:RequiredFieldValidator ID="TextBoxRequiredFieldValidator" runat="server" Display="Dynamic" ControlToValidate="first_name"
                                                            ErrorMessage="Consumer first name is required" ValidationGroup="FormValidationGroup"/>
                            </li>
                            <li>
                                <label for="last_name">Last name :</label> 
                                <telerik:RadTextBox ID="last_name" runat="server" Text='<%# Bind("consumer_last") %>' TabIndex="1"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="last_name"
                                                            ErrorMessage="Consumer last name is required"  ValidationGroup="FormValidationGroup" />
                            </li>
                            <li>
                                <label for="birth_date">Birth Date:</label> 
                                <telerik:RadDatePicker ID="birth_date" runat="server" MinDate="1/1/1900" DbSelectedDate='<%# Bind("date_of_birth") %>' TabIndex="4"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ControlToValidate="birth_date"
                                                            ErrorMessage="Date of birth is required"  ValidationGroup="FormValidationGroup"/>
                            </li>
                            <li>
                                <label for="address1">Address1:</label> 
                                <telerik:RadTextBox ID="address1" Text='<%# Bind( "address_line_1") %>' runat="server" TabIndex="8"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ControlToValidate="address1"
                                                            ErrorMessage="Address1 is required"  ValidationGroup="FormValidationGroup"/>
                            </li>                                    
                            <li>
                                <label for="address2">Address2:</label> 
                                <telerik:RadTextBox ID="address2" Text='<%# Bind( "address_line_2") %>' runat="server" TabIndex="9"/>
                            </li>                                   
                            <li>
                                <label for="city">City:</label> 
                                <telerik:RadTextBox ID="city" Text='<%# Bind("city") %>' runat="server" TabIndex="11"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="Dynamic" ControlToValidate="city"
                                                            ErrorMessage="City is required"  ValidationGroup="FormValidationGroup"/>
                            </li>
                            <li>
                                <label for="state">State:</label> 
                                <telerik:RadDropDownList ID="state" runat="server" DataSourceID="StatesSqlDataSource" SelectedValue='<%# Bind("state") %>'
                                                         DataTextField="name" DataValueField="abbreviation" TabIndex="12" DefaultMessage="Select" /> 
                                <asp:RequiredFieldValidator ID="Validator" ControlToValidate="state" 
                                                            ErrorMessage="State is required" runat="server" Display="Dynamic"  ValidationGroup="FormValidationGroup"/>
                            </li>                                    
                            <li>
                                <label for="zip_code">Zip Code:</label> 
                                <telerik:RadMaskedTextBox ID="zip_code" Text='<%# Bind("zip_code") %>' runat="server" TabIndex="13" Mask="#####-####" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic" ControlToValidate="zip_code"
                                                            ErrorMessage="Zip code is required"  ValidationGroup="FormValidationGroup"/>
                            </li>  
                            <li>
                                <label for="identifier">Identifier:</label> 
                                <telerik:RadTextBox ID="identifier" Text='<%# Bind("identifier") %>' runat="server" TabIndex="14"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Display="Dynamic" ControlToValidate="identifier"
                                                            ErrorMessage="Identifier is required" />
                                <asp:RegularExpressionValidator ID="rvDigits" runat="server" ControlToValidate="identifier" 
                                                                ErrorMessage="10 digit number is required" ValidationExpression="[0-9]{10}" />
                            </li>                                     
                            <li>
                                <label for="gender">Gender:</label> 
                                <telerik:RadRadioButtonList ID="gender" runat="server" Layout="Flow" Columns="2" SelectedValue='<%# Bind("gender") %>' TabIndex="15" 
                                                            ValidationGroup="GenderGroup" Direction="Horizontal">
                                    <Items>
                                        <telerik:ButtonListItem Text="Male" Value="M"/>
                                        <telerik:ButtonListItem Text="Female" Value="F"/>
                                    </Items>
                                </telerik:RadRadioButtonList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="gender"  Display="Dynamic"
                                                            ErrorMessage="Gender is required" ValidationGroup="GenderGroup" />
                            </li>                                      
                            <li>
                                <label for="diagnosis_code">Diagnosis Code:</label> 
                                <telerik:RadTextBox ID="diagnosis_code" Text='<%# Bind("diagnosis") %>' runat="server" TabIndex="16"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" Display="Dynamic" ControlToValidate="diagnosis_code"
                                                            ErrorMessage="Diagnosis code is required" ValidationGroup="FormValidationGroup" />
                            </li>  
                            <li>
                                <label for="nickname_first">Nickname First:</label> 
                                <telerik:RadTextBox ID="nickname_first" Text='<%# Bind("nickname_first") %>' runat="server" TabIndex="17"/>
                            </li>                                      
                            <li>
                                <label for="nickname_last">Nickname Last:</label> 
                                <telerik:RadTextBox ID="nickname_last" Text='<%# Bind("nickname_last") %>' runat="server" TabIndex="18"/>
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
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
        </telerik:RadGrid>
        <br />
        <br />
        <br />
        <br />
    </div>
    
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>"
        DeleteCommand="DELETE FROM Consumers WHERE consumer_internal_number = @consumer_internal_number"
        InsertCommand="INSERT INTO Consumers (consumer_first, consumer_last, date_of_birth, address_line_1, address_line_2, city, state, zip_code, identifier, gender, diagnosis, nickname_first, nickname_last) VALUES (@consumer_first, @consumer_last, @date_of_birth, @address_line_1, @address_line_2, @city, @state, @zip_code, @identifier, @gender, @diagnosis, @nickname_first, @nickname_last)"
        SelectCommand="SELECT consumer_internal_number, consumer_first, consumer_last, date_of_birth, address_line_1, address_line_2, city, state, zip_code, identifier, gender, diagnosis, nickname_first, nickname_last FROM Consumers ORDER BY consumer_last" 
        UpdateCommand="UPDATE Consumers SET consumer_first = @consumer_first, consumer_last = @consumer_last, date_of_birth = @date_of_birth, address_line_1 = @address_line_1, address_line_2 = @address_line_2, city = @city, state = @state, zip_code = @zip_code, identifier = @identifier, gender = @gender, diagnosis = @diagnosis, nickname_first = @nickname_first, nickname_last = @nickname_last WHERE consumer_internal_number = @consumer_internal_number">
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
                       DeleteCommand="DELETE FROM ConsumerTradingPartner WHERE consumer_internal_number = @consumer_internal_number"
                       InsertCommand="INSERT INTO ConsumerTradingPartner (consumer_internal_number, trading_partner_id) VALUES (@consumer_internal_number, @trading_partner_id)"
                       SelectCommand="SELECT ctp.consumer_internal_number, ctp.trading_partner_id, tp.name AS trading_partner_name FROM ConsumerTradingPartner ctp INNER JOIN TradingPartners tp ON ctp.trading_partner_id = tp.id WHERE consumer_internal_number = @consumer_internal_number"
                       UpdateCommand="UPDATE ConsumerTradingPartner SET trading_partner_id = @trading_partner_id WHERE consumer_internal_number = @consumer_internal_number">
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

<asp:SqlDataSource ID="StatesSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT name, abbreviation FROM States"/>
<asp:SqlDataSource ID="TradingPartnerDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id AS trading_partner_id, name FROM TradingPartners"/>
<asp:SqlDataSource ID="PartnerProgramDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id, partner_name, program_name FROM PartnerPrograms2"/>
<asp:SqlDataSource ID="CompositeProcedureCodeDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id, name FROM CompositeProcedureCodes"/>
</asp:Content>

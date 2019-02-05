﻿<%@ Page Title="Consumers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConsumersNew.aspx.cs" Inherits="ConsumerMaster.Consumers" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<style>
.space {
  background: none;
  width: 0.1rem;
}
</style>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="false" />
    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="demo" DecoratedControls="All" EnableRoundedCorners="false" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <h4>Consumers</h4>
    <div class="demo-container no-bg">
        <div id="grid">
            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowPaging="True" ShowFooter="true" AllowSorting="True" AutoGenerateColumns="False" ShowStatusBar="true" 
                     PageSize="15" OnDetailTableDataBind="RadGrid1_DetailTableDataBind" OnNeedDataSource="RadGrid1_NeedDataSource" OnPreRender="RadGrid1_PreRender" AllowFilteringByColumn="true">
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView TableLayout="Fixed" CommandItemDisplay="Top" Name="Consumers" DataKeyNames="consumer_internal_number">
                        <CommandItemSettings AddNewRecordText="Add New Consumer" />
                        <DetailTables>
                            <telerik:GridTableView DataKeyNames="partner_id" Width="100%" runat="server" CommandItemDisplay="Top" Name="Partners" Caption="Partners" AllowFilteringByColumn="false" >
 			                    <DetailTables>
<%--                                    <telerik:GridTableView DataKeyNames="consumer_internal_number" DataSourceID="SqlDataSource3" Width="100%" runat="server" CommandItemDisplay="Top" 
                                                           Name="CompProcCode" Caption="Composite Procedure" AllowFilteringByColumn="false" >
                                        <ParentTableRelation>
                                            <telerik:GridRelationFields DetailKeyField="consumer_internal_number" MasterKeyField="consumer_internal_number"></telerik:GridRelationFields>
                                            <telerik:GridRelationFields DetailKeyField="trading_partner_id" MasterKeyField="trading_partner_id"></telerik:GridRelationFields>
                                        </ParentTableRelation>
                                        <Columns>
                                            <telerik:GridEditCommandColumn UniqueName="EditCommandColumn2">
                                                <HeaderStyle Width="20px"></HeaderStyle>
                                                <ItemStyle CssClass="MyImageButton"></ItemStyle>
                                            </telerik:GridEditCommandColumn>
                                            <telerik:GridDropDownColumn UniqueName="CPCDropDownListColumn" ListTextField="name" ListValueField="id" DataSourceID="CompProcCodeDataSource" 
                                                            HeaderText="CompProcCode" DataField="cpc_id" DropDownControlType="RadComboBox" AllowSorting="true" HeaderStyle-Width="300px"/>
                                            <telerik:GridButtonColumn ConfirmText="Delete these details record?" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn2">
                                                <HeaderStyle Width="20px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton"></ItemStyle>
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                        <SortExpressions>
                                            <telerik:GridSortExpression FieldName="consumer_internal_number"></telerik:GridSortExpression>
                                        </SortExpressions>
                                    </telerik:GridTableView>--%>
			                    </DetailTables>                               
                                <Columns>
                                    <telerik:GridEditCommandColumn UniqueName="EditCommandColumn2">
                                        <HeaderStyle Width="20px"></HeaderStyle>
                                        <ItemStyle CssClass="MyImageButton"></ItemStyle>
                                    </telerik:GridEditCommandColumn>
                                    <telerik:GridBoundColumn DataField="name" HeaderText="Name" ReadOnly="true" HeaderStyle-Width="50px" ItemStyle-Width="50px" AllowFiltering="false" />
<%--                                    <telerik:GridDropDownColumn UniqueName="TPDropDownListColumn" ListTextField="name" ListValueField="trading_partner_id" DataSourceID="TradingPartnerDataSource" 
                                                    HeaderText="Trading Partner" DataField="trading_partner_id" DropDownControlType="RadComboBox" AllowSorting="true" HeaderStyle-Width="500px"/>--%>
                                    <telerik:GridButtonColumn ConfirmText="Delete these details record?" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn2">
                                        <HeaderStyle Width="20px"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton"></ItemStyle>
                                    </telerik:GridButtonColumn>
                                </Columns>
                                <SortExpressions>
                                    <telerik:GridSortExpression FieldName="consumer_internal_number"></telerik:GridSortExpression>
                                </SortExpressions>
                            </telerik:GridTableView>
                        </DetailTables>                    
                        <Columns>
                            <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" HeaderStyle-Width="50px" ItemStyle-Width="50px"/>
                            <telerik:GridBoundColumn DataField="consumer_internal_number" HeaderText="No." ReadOnly="true" HeaderStyle-Width="50px" ItemStyle-Width="50px" AllowFiltering="false" />
                            <telerik:GridBoundColumn DataField="consumer_first" HeaderText="First Name" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="120px" ItemStyle-Width="120px" AllowFiltering="false"/>
                            <telerik:GridBoundColumn DataField="consumer_last" HeaderText="Last Name" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="120px" ItemStyle-Width="120px" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true"/>
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
                                                        <telerik:RadTextBox ID="txtConsumerFirst" runat="server" Text='<%# Bind("consumer_first") %>'/>
                                                        <asp:RequiredFieldValidator ID="TextBoxRequiredFieldValidator" runat="server" Display="Dynamic" ControlToValidate="txtConsumerFirst"
                                                                                    ErrorMessage="Consumer first name is required" ValidationGroup="FormValidationGroup"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Last Name:</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtConsumerLast" runat="server" Text='<%# Bind("consumer_last") %>' TabIndex="1"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="txtConsumerLast"
                                                                                    ErrorMessage="Consumer last name is required"  ValidationGroup="FormValidationGroup" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Birth Date:</td>
                                                    <td>
                                                        <telerik:RadDatePicker ID="dpBirthDate" runat="server" MinDate="1/1/1900" DbSelectedDate='<%# Bind("date_of_birth") %>' TabIndex="4"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ControlToValidate="dpBirthDate"
                                                                                    ErrorMessage="Date of birth is required"  ValidationGroup="FormValidationGroup"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Address1:</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtAddress1" Text='<%# Bind( "address_line_1") %>' runat="server" TabIndex="8"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ControlToValidate="txtAddress1"
                                                                                    ErrorMessage="Address1 is required"  ValidationGroup="FormValidationGroup"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Address2:</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtAddress2" Text='<%# Bind( "address_line_2") %>' runat="server" TabIndex="9"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                <tr>
                                                    <td>City:</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtCity" Text='<%# Bind("city") %>' runat="server" TabIndex="11"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="Dynamic" ControlToValidate="txtCity"
                                                                                    ErrorMessage="City is required"  ValidationGroup="FormValidationGroup"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>State:</td>
                                                    <td>
                                                        <telerik:RadDropDownList ID="ddlStates" runat="server" DataSourceID="StatesSqlDataSource" SelectedValue='<%# Bind("state") %>'
                                                                                 DataTextField="Name" DataValueField="Abbreviation" TabIndex="12" DefaultMessage="Select" /> 
                                                        <asp:RequiredFieldValidator ID="Validator" ControlToValidate="ddlStates" 
                                                                                    ErrorMessage="State is required" runat="server" Display="Dynamic"  ValidationGroup="FormValidationGroup"/>
                                                    </td>
                                                </tr>               
                                                <tr>
                                                    
                                                    <td><telerik:RadLabel ID="RadLabel1" runat="server" Text="Zip Code:" /></td>
                                                    <td>
                                                        <telerik:RadMaskedTextBox ID="txtZipCode" Text='<%# Bind("zip_code") %>' runat="server" TabIndex="13" Mask="#####-####" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic" ControlToValidate="txtZipCode"
                                                                                    ErrorMessage="Zip code is required"  ValidationGroup="FormValidationGroup"/>
                                                    </td>
                                                </tr>               
                                                <tr>
                                                    <td>Identifier:</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtIdentifer" Text='<%# Bind("identifier") %>' runat="server" TabIndex="14"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Display="Dynamic" ControlToValidate="txtIdentifer"
                                                                                    ErrorMessage="Identifier is required" />
                                                        <asp:RegularExpressionValidator ID="rvDigits" runat="server" ControlToValidate="txtIdentifer" 
                                                                                        ErrorMessage="10 digit number is required" ValidationExpression="[0-9]{10}" />
                                                    </td>
                                                </tr>              
                                                <tr>
                                                    <td>Gender:</td>
                                                    <td>
                                                        <telerik:RadRadioButtonList ID="rblGender" runat="server" Layout="Flow" Columns="2" SelectedValue='<%# Bind("gender") %>' TabIndex="15" 
                                                                                    ValidationGroup="GenderGroup" Direction="Horizontal">
                                                            <Items>
                                                                <telerik:ButtonListItem Text="Male" Value="M"/>
                                                                <telerik:ButtonListItem Text="Female" Value="F"/>
                                                            </Items>
                                                        </telerik:RadRadioButtonList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="rblGender"  Display="Dynamic"
                                                                                    ErrorMessage="Gender is required" ValidationGroup="GenderGroup" />
                                                    </td>
                                                </tr>                
                                                <tr>
                                                    <td>Diagnosis code:</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtDiagnosis" Text='<%# Bind("diagnosis") %>' runat="server" TabIndex="16"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" Display="Dynamic" ControlToValidate="txtDiagnosis"
                                                                                    ErrorMessage="Diagnosis code is required" ValidationGroup="FormValidationGroup" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Nickname First:</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtNicknameFirst" Text='<%# Bind("nickname_first") %>' runat="server" TabIndex="17"/>
                                                    </td>
                                                </tr>               
                                                <tr>
                                                    <td>Nickname Last:</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtNicknameLast" Text='<%# Bind("nickname_last") %>' runat="server" TabIndex="18"/>
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
                <ClientSettings>
                </ClientSettings>
           </telerik:RadGrid> 
        </div>
    </div>

    <asp:SqlDataSource ID="StatesSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT Name, Abbreviation FROM States"/>
    <asp:SqlDataSource ID="TradingPartnerDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id AS trading_partner_id, name FROM TradingPartners"/>
    <asp:SqlDataSource ID="CompositeProcedureCodeDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id, name FROM CompositeProcedureCodes"/>

</asp:Content>
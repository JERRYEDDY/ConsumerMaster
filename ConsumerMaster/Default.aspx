<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="True" CodeBehind="Default.aspx.cs" Inherits="ConsumerMaster._Default" %>
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
    <h4>Pathways Consumers</h4>
    <div class="demo-container no-bg">

        <div id="grid">
            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowPaging="True" ShowFooter="true" AllowSorting="True" AutoGenerateColumns="False" ShowStatusBar="true" 
                     PageSize="12" DataSourceID="SqlDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True" OnItemDeleted="RadGrid1_ItemDeleted" 
                     OnItemInserted="RadGrid1_ItemInserted" OnItemUpdated="RadGrid1_ItemUpdated" OnInsertCommand="RadGrid1_InsertCommand">
                <PagerStyle Mode="NumericPages"></PagerStyle>
                <MasterTableView  TableLayout="Fixed" CommandItemDisplay="Top" Name="Consumers" DataSourceID="SqlDataSource1" DataKeyNames="consumer_internal_number" Caption="CONSUMERS">
                        <CommandItemSettings AddNewRecordText="Add New Consumer" />
                        <DetailTables>
                            <telerik:GridTableView DataKeyNames="consumer_internal_number" DataSourceID="SqlDataSource2" Width="100%" runat="server" CommandItemDisplay="Top" Name="TradingPartners" Caption="TRADING PARTNERS">
                                <ParentTableRelation>
                                    <telerik:GridRelationFields DetailKeyField="consumer_internal_number" MasterKeyField="consumer_internal_number"></telerik:GridRelationFields>
                                </ParentTableRelation>
                                <Columns>
                                    <telerik:GridEditCommandColumn UniqueName="EditCommandColumn2">
                                        <HeaderStyle Width="20px"></HeaderStyle>
                                        <ItemStyle CssClass="MyImageButton"></ItemStyle>
                                    </telerik:GridEditCommandColumn>
                                    <telerik:GridBoundColumn SortExpression="consumer_internal_number" HeaderText="No." DataField="consumer_internal_number" UniqueName="consumer_internal_number" ReadOnly="true" Display="True" Visible="False" />
                                    
                                    <telerik:GridBoundColumn SortExpression="trading_partner_id" HeaderText="TPId" DataField="trading_partner_id" UniqueName="trading_partner_id" Visible="False" />
                                         
                                    <telerik:GridBoundColumn SortExpression="id" HeaderText="TPId" DataField="id" UniqueName="id" Visible="False" />
                                                                       
                                    <telerik:GridBoundColumn SortExpression="name" HeaderText="Name" DataField="name" UniqueName="name"/>
                                                                       
                                    <telerik:GridBoundColumn SortExpression="string" HeaderText="String" DataField="string" UniqueName="string"/>
                                    
                                    <telerik:GridButtonColumn ConfirmText="Delete these details record?" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn2">
                                        <HeaderStyle Width="20px"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton"></ItemStyle>
                                    </telerik:GridButtonColumn>
                                </Columns>
                                <EditFormSettings EditFormType="Template">
                                     <FormTemplate>
                                        <table id="Table4" cellspacing="2" cellpadding="1" width="100%" border="0" rules="none" style="border-collapse: collapse;">
                                            <tr class="EditFormHeader">
                                                <td colspan="2">
                                                    <b>Trading Partner Info</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table id="Table3" width="450px" border="0" class="module">
                                                        <tr>
                                                            <td>TPId</td>
                                                            <td>
                                                                <telerik:RadTextBox ID="txtTradingPartnerId" runat="server" Text='<%# Bind("trading_partner_id") %>'/>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadDropDownList ID="ddlTradingPartners" runat="server" DataSourceID="TradingPartnerDataSource" SelectedValue='<%# Bind("trading_partner_id") %>'
                                                                                         DataTextField="name" DataValueField="id" TabIndex="12" Width="400px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="vertical-align: top">
                                                    <table id="Table1" cellspacing="1" cellpadding="1" width="500" border="0" class="module">
                                                    </table>
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
                                                    <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                                        runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'></asp:Button>&nbsp;
                                                                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CommandName="Cancel"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </FormTemplate>                                   
                                </EditFormSettings>
                                <SortExpressions>
                                    <telerik:GridSortExpression FieldName="consumer_internal_number"></telerik:GridSortExpression>
                                </SortExpressions>
                            </telerik:GridTableView>
                        </DetailTables>                    
                        <Columns>
                            <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" HeaderStyle-Width="50px" ItemStyle-Width="50px"/>
                            <telerik:GridBoundColumn DataField="consumer_internal_number" HeaderText="No." ReadOnly="true" HeaderStyle-Width="50px" ItemStyle-Width="50px"/>
                            <telerik:GridBoundColumn DataField="consumer_first" HeaderText="First Name" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="120px" ItemStyle-Width="120px" />
                            <telerik:GridBoundColumn DataField="consumer_last" HeaderText="Last Name" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="120px" ItemStyle-Width="120px" />
                            <telerik:GridBoundColumn DataField="city" HeaderText="City" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="150px" ItemStyle-Width="150px"/>                        
                            <telerik:GridBoundColumn DataField="state" HeaderText="State" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="50px" ItemStyle-Width="50px"/>
                            <telerik:GridBoundColumn DataField="identifier" HeaderText="Identifier" ColumnEditorID="GridTextBoxEditor"/>                       
                            <telerik:GridBoundColumn DataField="diagnosis" HeaderText="Diagnosis" ColumnEditorID="GridTextBoxEditor"/>
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
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Last Name:</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtConsumerLast" runat="server" Text='<%# Bind("consumer_last") %>' TabIndex="1"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Birth Date:</td>
                                                    <td>
                                                        <telerik:RadDatePicker ID="dpBirthDate" runat="server" MinDate="1/1/1900" DbSelectedDate='<%# Bind("date_of_birth") %>' TabIndex="4"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Address1:</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtAddress1" Text='<%# Bind( "address_line_1") %>' runat="server" TabIndex="8"/>
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
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>State:</td>
                                                    <td>
                                                        <telerik:RadDropDownList ID="ddlStates" runat="server" DataSourceID="StatesSqlDataSource" SelectedValue='<%# Bind("state") %>'
                                                                                 DataTextField="Name" DataValueField="Abbreviation" TabIndex="12"/> 
                                                    </td>
                                                </tr>               
                                                <tr>
                                                    
                                                    <td><telerik:RadLabel ID="RadLabel1" runat="server" Text="Zip Code:" /></td>
                                                    <td>
                                                        <telerik:RadMaskedTextBox ID="ktxtZipCode" Text='<%# Bind("zip_code") %>' runat="server" TabIndex="13" Mask="#####-####" />
                                                    </td>
                                                </tr>               
                                                <tr>
                                                    <td>Identifier:</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtIdentifer" Text='<%# Bind("identifier") %>' runat="server" TabIndex="14"/>
                                                    </td>
                                                </tr>              
                                                <tr>
                                                    <td>Gender:</td>
                                                    <td>
                                                        <telerik:RadRadioButtonList runat="server" ID="rblGender" Layout="Flow" Columns="2" SelectedValue='<%# Bind("gender") %>' TabIndex="15">
                                                            <Items>
                                                                <telerik:ButtonListItem Text="Male" Value="M"/>
                                                                <telerik:ButtonListItem Text="Female" Value="F"/>
                                                            </Items>
                                                        </telerik:RadRadioButtonList>
                                                    </td>
                                                </tr>                
                                                <tr>
                                                    <td>Diagnosis:</td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtDiagnosis" Text='<%# Bind("diagnosis") %>' runat="server" TabIndex="16"/>
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
                                            <table id="Table1" cellspacing="1" cellpadding="1" width="500" border="0" class="module">
                                            </table>
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
                                            <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                                runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'></asp:Button>&nbsp;
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
        <div>
            <br/>
            <telerik:RadPanelBar ID="RadPanelBar1" runat="server"></telerik:RadPanelBar>
            <table>
                <tr>
                    <td>
                        <img src="Images/AWCLogo1.png" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Consumer Export File 
                    </td>
                    <td class="space"/>
                    <td>
                        <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton2" CssClass="downloadButton" OnClick="ConsumerExportDownload_Click" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Service Export File 
                    </td>
                    <td class="space"/>
                    <td>
                        <telerik:RadButton RenderMode="Lightweight" Text="Download" ID="RadButton1" CssClass="downloadButton" OnClick="ServiceExportDownload_Click" runat="server" />
                    </td>
                </tr>
            </table>
        </div>

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>"
            DeleteCommand="DELETE FROM [Consumers] WHERE [consumer_internal_number] = @consumer_internal_number"
            InsertCommand="INSERT INTO [Consumers] ([consumer_first], [consumer_last], [date_of_birth], [address_line_1], [address_line_2], [city], [state], [zip_code], [identifier], [gender], 
            [diagnosis], [nickname_first], [nickname_last]) 
            VALUES (@consumer_first, @consumer_last, @date_of_birth, @address_line_1, @address_line_2, @city, @state, @zip_code, @identifier, @gender, @diagnosis, @nickname_first, @nickname_last)"
            SelectCommand="SELECT * FROM [Consumers]" 
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
            SelectCommand="SELECT * FROM [ConsumerTradingPartner] AS ctp INNER JOIN [TradingPartners] AS tp ON ctp.trading_partner_id = tp.id WHERE [consumer_internal_number] = @consumer_internal_number" 
            UpdateCommand="UPDATE [ConsumerTradingPartner] SET [trading_partner_id] = @trading_partner_id WHERE [consumer_internal_number] = @consumer_internal_number">
           <SelectParameters>
               <asp:ControlParameter ControlID="RadGrid1" Name="consumer_internal_number" PropertyName="SelectedValues['consumer_internal_number']" Type="Int32" />
           </SelectParameters>
             <DeleteParameters>
                <asp:Parameter Name="consumer_internal_number" Type="Int32"></asp:Parameter>
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="consumer_internal_number" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="trading_partner_id" Type="String"></asp:Parameter>
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="trading_partner_id" Type="String"></asp:Parameter>
                <asp:Parameter Name="consumer_internal_number" Type="Int32"></asp:Parameter>
            </UpdateParameters>
        </asp:SqlDataSource>    

        <asp:SqlDataSource ID="StatesSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT Name, Abbreviation FROM States"/>
        <asp:SqlDataSource ID="TradingPartnerDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>" SelectCommand="SELECT id, name FROM TradingPartners"/>
    </div>
</asp:Content>

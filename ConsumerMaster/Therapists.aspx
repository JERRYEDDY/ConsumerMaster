<%@ Page Title="Therapists" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Therapists.aspx.cs" Inherits="ConsumerMaster.Therapists" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="false" />
    <telerik:RadFormDecorator RenderMode="Lightweight" runat="server" DecorationZoneID="demo" EnableRoundedCorners="false" DecoratedControls="All" />   
    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="demo" DecoratedControls="All" EnableRoundedCorners="false" />
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
    <h4>Therapists</h4>
    <div id="demo" class="demo-container no-bg">
        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowPaging="True" ShowFooter="true" AllowSorting="True" AutoGenerateColumns="False" ShowStatusBar="true" 
                         PageSize="12" DataSourceID="SqlDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True" OnItemDeleted="RadGrid1_ItemDeleted" 
                         OnItemInserted="RadGrid1_ItemInserted" OnItemUpdated="RadGrid1_ItemUpdated">
            <PagerStyle Mode="NextPrevAndNumeric" />
            <MasterTableView CommandItemDisplay="Top" Name="Therapists" DataSourceID="SqlDataSource1" DataKeyNames="id">
                <CommandItemSettings AddNewRecordText="Add New Therapist" />
                <Columns>
                    <telerik:GridEditCommandColumn UniqueName="EditCommandColumn"></telerik:GridEditCommandColumn>
                    <telerik:GridBoundColumn DataField="id" HeaderText="id" SortExpression="id" UniqueName="id" ReadOnly="true" />
                    <telerik:GridBoundColumn DataField="rendering_provider_id" HeaderText="rendering_provider_id" SortExpression="rendering_provider_id" UniqueName="rendering_provider_id">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage="This field rendering_provider_id is required" />
                            <ModelErrorMessage BackColor="Red" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="rendering_provider_first_name" HeaderText="rendering_provider_first_name" SortExpression="rendering_provider_first_name" UniqueName="rendering_provider_first_name">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage="This field rendering_provider_first_name is required" />
                            <ModelErrorMessage BackColor="Red" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="rendering_provider_last_name" HeaderText="rendering_provider_last_name" SortExpression="rendering_provider_last_name" UniqueName="rendering_provider_last_name">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage="This field rendering_provider_last_name is required" />
                            <ModelErrorMessage BackColor="Red" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="rendering_provider_name" HeaderText="rendering_provider_name" SortExpression="rendering_provider_name" UniqueName="rendering_provider_name" ReadOnly="True">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage="This field rendering_provider_name is required" />
                            <ModelErrorMessage BackColor="Red" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="rendering_provider_npi" HeaderText="rendering_provider_npi" SortExpression="rendering_provider_npi" UniqueName="rendering_provider_npi">
                        <ColumnValidationSettings EnableRequiredFieldValidation="true" EnableModelErrorMessageValidation="true">
                            <RequiredFieldValidator ForeColor="Red" ErrorMessage="This field rendering_provider_npi is required" />
                            <ModelErrorMessage BackColor="Red" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ConfirmText="Delete this Therapist record?" Text="Delete" CommandName="Delete" />
                </Columns>    
            </MasterTableView>
        </telerik:RadGrid>
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnStringDb1 %>"
        DeleteCommand="DELETE FROM Therapists WHERE id = @id"
        InsertCommand="INSERT INTO Therapists (rendering_provider_id, rendering_provider_name, rendering_provider_first_name, rendering_provider_last_name, rendering_provider_npi) VALUES (@rendering_provider_id, @rendering_provider_first_name, @rendering_provider_last_name, @rendering_provider_npi)"
        SelectCommand="SELECT * FROM Therapists" 
        UpdateCommand="UPDATE Therapists SET rendering_provider_id = @rendering_provider_id, rendering_provider_first_name = @rendering_provider_first_name, rendering_provider_last_name = @rendering_provider_last_name, rendering_provider_npi = @rendering_provider_npi WHERE id = @id">
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
    </asp:SqlDataSource>  
</asp:Content>

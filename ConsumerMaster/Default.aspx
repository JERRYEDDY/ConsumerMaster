<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ConsumerMaster._Default" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="false" />
    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="demo" DecoratedControls="All" EnableRoundedCorners="false" />
    <h4>Pathways Consumers</h4>
    <div class="demo-container no-bg">
        <div id="grid">
            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowPaging="True" ShowFooter="true" AllowSorting="True" AutoGenerateColumns="False" ShowStatusBar="true" 
                    OnNeedDataSource="RadGrid1_NeedDataSource" OnPreRender="RadGrid1_PreRender">
                <MasterTableView  TableLayout="Fixed" CommandItemDisplay="Top" DataKeyNames="consumer_internal_number">
                    <Columns>
                        <telerik:GridBoundColumn DataField="consumer_internal_number" HeaderText="No." ReadOnly="true"/>
                        <telerik:GridBoundColumn DataField="consumer_first" HeaderText="First Name" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="120px" ItemStyle-Width="120px" />
                        <telerik:GridBoundColumn DataField="consumer_last" HeaderText="Last Name" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="150px" ItemStyle-Width="150px" />
                        <telerik:GridBoundColumn DataField="date_of_birth" HeaderText="Birth Date" ColumnEditorID="GridTextBoxEditor" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                        <telerik:GridBoundColumn DataField="address_line_1" HeaderText="Address1" ColumnEditorID="GridTextBoxEditor" HeaderStyle-Width="200px" ItemStyle-Width="200px" />
<%--                        <telerik:GridBoundColumn DataField="address_line_2" HeaderText="Address2" ColumnEditorID="GridTextBoxEditor"/>--%>
                        <telerik:GridBoundColumn DataField="city" HeaderText="City" ColumnEditorID="GridTextBoxEditor"/>                        
                        <telerik:GridBoundColumn DataField="state" HeaderText="State" ColumnEditorID="GridTextBoxEditor"/>
                        <telerik:GridBoundColumn DataField="zip_code" HeaderText="Zip Code" ColumnEditorID="GridTextBoxEditor"/>
                        <telerik:GridBoundColumn DataField="identifier" HeaderText="Identifier" ColumnEditorID="GridTextBoxEditor"/>                       
                        <telerik:GridBoundColumn DataField="gender" HeaderText="Gender" ColumnEditorID="GridTextBoxEditor"/>
                        <telerik:GridBoundColumn DataField="diagnosis" HeaderText="Diagnosis" ColumnEditorID="GridTextBoxEditor"/>
                        <telerik:GridBoundColumn DataField="nickname_first" HeaderText="Nick" ColumnEditorID="GridTextBoxEditor"/>                      
                        <telerik:GridBoundColumn DataField="nickname_last" HeaderText="Name" ColumnEditorID="GridTextBoxEditor"/>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn"></telerik:GridEditCommandColumn>
                        <telerik:GridClientDeleteColumn HeaderText="Delete">
                            <HeaderStyle Width="70px" />
                        </telerik:GridClientDeleteColumn>       
                    </Columns>
                    <EditFormSettings UserControlName="ConsumerDetailsCS.ascx" EditFormType="WebUserControl">
                        <EditColumn UniqueName="EditCommandColumn1"></EditColumn>
                    </EditFormSettings>
                </MasterTableView>
                <ClientSettings>

                </ClientSettings>
           </telerik:RadGrid> 
        </div>
    </div>
</asp:Content>

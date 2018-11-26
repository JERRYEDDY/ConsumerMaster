<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConsumerDetailsCS.ascx.cs" Inherits="ConsumerMaster.ConsumerDetailsCS" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="0" rules="none"
    style="border-collapse: collapse;">
    <tr class="EditFormHeader">
        <td colspan="2">
            <b>Consumer Details</b>
        </td>
    </tr>
    <tr>
        <td>
            <table id="Table3" width="450px" border="0" class="module">
                <tr>
                    <td class="title" style="font-weight: bold;" colspan="2">Consumer Info:</td>
                </tr>
                <tr>
                    <td>First Name:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("consumer_first") %>'>
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Last Name:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("consumer_last") %>'>' TabIndex="1">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Birth Date:
                    </td>
                    <td>
                        <telerik:RadDatePicker ID="RadDatePicker1" runat="server" MinDate="1/1/1900" DbSelectedDate='<%# Bind("date_of_birth") %>'
                                               TabIndex="4">
                        </telerik:RadDatePicker>
                    </td>
                </tr>
                <tr>
                    <td>Address1:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox2" Text='<%# Bind( "address_line_1") %>' runat="server" TabIndex="8">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Address2:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox3" Text='<%# Bind( "address_line_2") %>' runat="server" TabIndex="9">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>

                <tr>
                    <td>City:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox4" Text='<%# Bind("city") %>' runat="server" TabIndex="11">
                        </asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>State:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox5" Text='<%# Bind("state") %>' runat="server" TabIndex="12">
                        </asp:TextBox>
                    </td>
                </tr>               
                <tr>
                    <td>Zip Code:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox9" Text='<%# Bind("zip_code") %>' runat="server" TabIndex="13">
                        </asp:TextBox>
                    </td>
                </tr>               
                
                <tr>
                    <td>Identifier:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox10" Text='<%# Bind("identifier") %>' runat="server" TabIndex="14">
                        </asp:TextBox>
                    </td>
                </tr>              
                <tr>
                    <td>Gender:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox11" Text='<%# Bind("gender") %>' runat="server" TabIndex="15">
                        </asp:TextBox>
                    </td>
                </tr>                
                <tr>
                    <td>Diagnosis:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox12" Text='<%# Bind("diagnosis") %>' runat="server" TabIndex="16">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Nickname First:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox13" Text='<%# Bind("nickname_first") %>' runat="server" TabIndex="17">
                        </asp:TextBox>
                    </td>
                </tr>               
                <tr>
                    <td>Nickname Last:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox14" Text='<%# Bind("nickname_last") %>' runat="server" TabIndex="18">
                        </asp:TextBox>
                    </td>
                </tr>
            </table>
        </td>
        <td style="vertical-align: top">
            <table id="Table1" cellspacing="1" cellpadding="1" width="250" border="0" class="module">
                <tr>
                    <td>Notes:
                    </td>
                </tr>
                <tr>
                    <td>
<%--                        <asp:TextBox ID="TextBox1" Text='<%# DataBinder.Eval(Container, "DataItem.Notes") %>' runat="server" TextMode="MultiLine"
                            Rows="5" Columns="40" TabIndex="5">
                        </asp:TextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td>Address:
                    </td>
                </tr>
                <tr>
                    <td>
<%--                        <asp:TextBox ID="TextBox6" Text='<%# DataBinder.Eval(Container, "DataItem.Address") %>' runat="server" TextMode="MultiLine"
                            Rows="2" Columns="40" TabIndex="6">
                        </asp:TextBox>
                    </td>--%>
                </tr>
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
                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                                        CommandName="Cancel"></asp:Button>
        </td>
    </tr>
</table>
<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="ConsumerMaster.About" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<style>

.container-1 {
    display: flex;
}

.container-1 div {
    border: 1px #ccc solid;
    padding:10px
}

.box-1{

}

.box-2{

}

.box-3 {

}
</style>   

<div class="container-1">
    <div class="box-1">
        <h3>Box One</h3>
        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>
    </div>    
    <div class="box-2">
        <h3>Box Two</h3>
        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>
    </div> 
    <div class="box-3">
        <h3>Box Three</h3>
        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>
    </div> 
</div>    


</asp:Content>

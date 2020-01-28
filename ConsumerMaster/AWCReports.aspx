<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AWCReports.aspx.cs" Inherits="ConsumerMaster.AWCReports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns='http://www.w3.org/1999/xhtml'>
<head runat="server">
    <title>Telerik ASP.NET Example</title>
    <link rel="stylesheet" type="text/css" href="styles.css" />
    <script type="text/javascript" src="scripts.js"></script>
</head>
 
<body>
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="true" />
    <qsf:MessageBox ID="InformationBox1" Icon="Info" Type="Info" runat="server">
        <p>
            The Drag and Drop functionality relies on the HTML5 File API and Drag-And-Drop modules,
            which means that it works in modern browsers only:
        </p>
        <p>Firefox 4+, Google Chrome, IE10+, Edge.</p>
    </qsf:MessageBox>
 
    <div class="demo-container size-wide">
        <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="RadAsyncUpload1" MultipleFileSelection="Automatic" DropZones=".DropZone1,#DropZone2"  />
 
        <div class="DropZone1">
            <p>Custom Drop Zone</p>
            <p>Drop Files Here</p>
        </div>
        <div id="DropZone2">
            <p>Custom Drop Zone</p>
            <p>Drop Files Here</p>
        </div>
    </div>
 
    <telerik:RadScriptBlock runat="server">
        <script type="text/javascript">
            //<![CDATA[
            Sys.Application.add_load(function () {
                demo.initialize();
            });
            //]]>
        </script>
    </telerik:RadScriptBlock>
</body>
</html>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xmlview.aspx.cs" Inherits="DotNetUnitTests.xmlview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%= Request.QueryString["title"] %></title>
</head>
<body>
    <h1><%= Request.QueryString["test"] %></h1>
    <h3>Enter an XML file containing an entity:</h3>
    <textarea rows="15" cols="150" name="payload" form="theform"><%
                                                                     string path = Request.PhysicalApplicationPath;
                                                                     string xmltext = System.IO.File.ReadAllText(path + "/resources/xxetest.xml");                                                                 
                                                                     Response.Write(xmltext);
    %></textarea>
    <form id="theform" action="results.aspx" method="get" autocomplete="off" runat="server">
        <input type="hidden" name="var" value="<%= Request.QueryString["var"] %>" />
        <input type="submit" value="Submit" />
    </form>
    <br /><br /><a href="index.aspx">&lt&lt&lt back to tests</a>
</body>
</html>

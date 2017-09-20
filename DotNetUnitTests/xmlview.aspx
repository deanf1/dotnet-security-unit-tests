<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xmlview.aspx.cs" Inherits="DotNetUnitTests.xmlview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%= Request.QueryString["title"] %></title>
</head>
<body>
    <h1><%= Request.QueryString["test"] %></h1>
    <a href="codeview.aspx?type=xml&var=<%= Request.QueryString["var"] %>">View code for this test</a>
    <br /><br />
    <h3>Enter an XML file containing an entity:</h3>
    <textarea rows="15" cols="150" name="payload" form="theform"><%
                                                                     string path = Request.PhysicalApplicationPath;
                                                                     string xmlText = System.IO.File.ReadAllText(path + "/resources/xxetest.xml");                                                                 
                                                                     Response.Write(xmlText);
    %></textarea>

    <% Response.Write("<form id=\"theform\" action=\"TestCases/XXETestCases/" + Request.QueryString["var"] + ".aspx\" method=\"get\" autocomplete=\"off\" runat=\"server\">"); %>
        <input type="hidden" name="var" value="<%= Request.QueryString["var"] %>" />
        <input type="submit" value="Submit" />
    </form>
    <br /><br /><a href="index.aspx">&lt&lt&lt back to tests</a>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xpathview.aspx.cs" Inherits="DotNetUnitTests.xpathview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%= Request.QueryString["title"] %></title>
</head>
<body>
    <h1><%= Request.QueryString["test"] %></h1>
    <a href="codeview.aspx?type=xpath&test=<%= Request.QueryString["test"] %>">View code for this test</a>
    <br /><br />
    <h3>The following is the XML file the query will be performed on:</h3>
    <textarea rows="15" cols="150" name="payload" form="theform" disabled="disabled"><%
                                                                     string path = Request.PhysicalApplicationPath;
                                                                     string xmlText = System.IO.File.ReadAllText(path + "/resources/students.xml");                                                                 
                                                                     Response.Write(xmlText);
    %></textarea>
    <form id="theform" action="xpathresults.aspx" method="get" autocomplete="off" runat="server">
        <input type="hidden" name="var" value="<%= Request.QueryString["var"] %>" />
        <h3>The injection given below will attempt to fetch all &lt;Student&gt; nodes instead of just the entered one by adding <mark>&apos; or &apos;a&apos;=&apos;a</mark> to the end.</h3>
        Enter first name: <input title="Payload" name="payload" value="Bobby&apos; or &apos;a&apos;=&apos;a" />
        <input type="submit" value="Submit" />
    </form>
    <br /><br /><a href="xpath.aspx">&lt&lt&lt back to tests</a>
</body>
</html>

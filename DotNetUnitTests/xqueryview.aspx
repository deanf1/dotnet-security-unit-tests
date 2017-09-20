<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xqueryview.aspx.cs" Inherits="DotNetUnitTests.xqueryview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%= Request.QueryString["title"] %></title>
</head>
<body>
    <h1><%= Request.QueryString["test"] %></h1>
    <a href="codeview.aspx?type=xquery&var=<%= Request.QueryString["var"] %>">View code for this test</a>
    <br /><br />
    <h3>The following is the XML file the query will be performed on:</h3>
    <textarea rows="15" cols="150" name="payload" form="theform" disabled="disabled"><%
                                                                     string path = Request.PhysicalApplicationPath;
                                                                     string xmlText = System.IO.File.ReadAllText(path + "/resources/students.xml");                                                                 
                                                                     Response.Write(xmlText);
    %></textarea>
    <% Response.Write("<form id=\"theform\" action=\"TestCases/XQueryTestCases/" + Request.QueryString["var"] + ".aspx\" method=\"get\" autocomplete=\"off\" runat=\"server\">"); %>
        <input type="hidden" name="var" value="<%= Request.QueryString["var"] %>" />
        <h3>The injection given below will attempt to fetch all &lt;Student&gt; nodes instead of just the entered one by adding <mark>&quot; or &quot;a&quot;=&quot;a</mark> to the end.</h3>
        Enter first name: <input title="Payload" name="payload" value="Bobby&quot; or &quot;a&quot;=&quot;a" />
        <input type="submit" value="Submit" />
    </form>
    <br /><br /><a href="xquery.aspx">&lt&lt&lt back to tests</a>
</body>
</html>

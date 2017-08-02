<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xquery.aspx.cs" Inherits="DotNetUnitTests.xquery" %>
<%@ Import Namespace="Microsoft.Win32" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>.NET XQuery Tests</title>
</head>
<body>

<h1>XQuery Injection Vulnerability Tests</h1>
<a href="index.aspx">.NET XXE Injection Tests</a> | <a href="nhibernate.aspx">NHibernate Injection Tests</a> | <a href="xpath.aspx">XPath Injection Tests</a> | <a href="xquery.aspx">XQuery Injection Tests</a>
<%  Response.Write("<h3>");
    Response.Write("Current .NET Framework Version: " + HttpRuntime.TargetFramework.ToString());
    Response.Write("<br />" + "Unsafe Tests: tbd" + "<br />" + "Safe Tests: tbd");
    Response.Write("</h3>");
%>

<ul>
    <li><a href="http://www.saxonica.com/html/documentation/dotnetdoc/">net.sf.saxon.s9api (link to <b>Saxonica Saxon9</b> docs)</a>
        <ul>
            <li><a href="xqueryview.aspx?title=Unsafe%20XQuery&test=Saxon%3A%20Unsafe%20when%20Using%20String%20Concatenation%20on%20XQuery%20Expression%20Example&var=xqueryunsafeconcat">Unsafe when Using String Concatenation on XQuery Expression Example</a></li>
			<li><a href="xqueryview.aspx?title=Unsafe%20XQuery&test=Saxon%3A%20Unsafe%20when%20Using%20String%20Placeholders%20on%20XQuery%20Expression%20Example&var=xqueryunsafeplaceholder">Unsafe when Using String Placeholders on XQuery Expression Example</a></li>
			<li><a href="xqueryview.aspx?title=Safe%20XQuery&test=Saxon%3A%20Safe%20when%20Using%20Bind%20Variables%20on%20XQuery%20Expression%20Example&var=xquerysafebind">Safe when Using Bind Variables on XQuery Expression Example</a></li>
			<li><a href="xqueryview.aspx?title=Safe%20XQuery&test=Saxon%3A%20Safe%20when%20Whitelisting%20on%20XQuery%20Expression%20Example&var=xquerysafelist">Safe when Whitelisting on XQuery Expression Example</a></li>
            <li><a href="xqueryview.aspx?title=Safe%20XQuery&test=Saxon%3A%20Safe%20when%20Escaping%20Quotation%20Marks%20and%20Semicolons%20on%20XQuery%20Expression%20Example&var=xquerysafeescape">Safe when Escaping Quotation Marks and Semicolons on XQuery Expression Example</a></li>
		</ul>
    <br /></li>
</ul>

<br /><br />
References:
<ol>
    <li><a href="http://www.saxonica.com/html/documentation/dotnet/dotnetapi.html">Saxon API for .NET Guide</a></li>
</ol>

</body>
</html>
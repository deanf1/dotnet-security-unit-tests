<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xpath.aspx.cs" Inherits="DotNetUnitTests.xpath" %>
<%@ Import Namespace="Microsoft.Win32" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>.NET XPath Tests</title>
</head>
<body>

<h1>XPath Injection Vulnerability Tests</h1>
<a href="index.aspx">.NET XXE Injection Tests</a> | <a href="nhibernate.aspx">NHibernate Injection Tests</a> | <a href="xpath.aspx">XPath Injection Tests</a> | <a href="xquery.aspx">XQuery Injection Tests</a>
<%  Response.Write("<h3>");
    Response.Write("Current .NET Framework Version: " + HttpRuntime.TargetFramework.ToString());
    Response.Write("<br />" + "Unsafe Tests: tbd" + "<br />" + "Safe Tests: tbd");
    Response.Write("</h3>");
%>

<ul>
    <li><a href="https://msdn.microsoft.com/en-us/library/system.xml.xpath(v=vs.110).aspx">System.Xml.<b>XPath</b> (link to MSDN docs)</a>
        <ul>
            <li><a href="xpathview.aspx?title=Unsafe%20XPath&test=XPath%3A%20Unsafe%20when%20Using%20String%20Concatenation%20on%20XPath%20Expression%20Example&var=xpathunsafeconcat">Unsafe when Using String Concatenation on XPath Expression Example</a></li>
			<li><a href="xpathview.aspx?title=Unsafe%20XPath&test=XPath%3A%20Unsafe%20when%20Using%20String%20Placeholders%20on%20XPath%20Expression%20Example&var=xpathunsafeplaceholder">Unsafe when Using String Placeholders on XPath Expression Example</a></li>
			<li><a href="xpathview.aspx?title=Safe%20XPath&test=XPath%3A%20Safe%20when%20Whitelisting%20on%20XPath%20Expression%20Example&var=xpathsafelist">Safe when Whitelisting on XPath Expression Example</a></li>
			<li><a href="xpathview.aspx?title=Unsafe%20XPath&test=XPath%3A%20Unsafe%20when%20Escaping%20Apostrophes%20on%20XPath%20Expression%20Example&var=xpathunsafeescape">Unsafe when Escaping Apostrophes on XPath Expression Example</a></li>
		</ul>
    <br /></li>
</ul>

<br /><br />
References:
<ol>
    <li><a href="https://www.owasp.org/index.php/XPATH_Injection">OWASP: XPath Injection</a></li>
</ol>

</body>
</html>
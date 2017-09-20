<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="nhibernate.aspx.cs" Inherits="DotNetUnitTests.nhibernate" %>
<%@ Import Namespace="Microsoft.Win32" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>.NET NHibernate Tests</title>
</head>
<body>

<h1>NHibernate Injection Vulnerability Tests</h1>
<a href="index.aspx">.NET XXE Injection Tests</a> | <a href="nhibernate.aspx">NHibernate Injection Tests</a> | <a href="xpath.aspx">XPath Injection Tests</a> | <a href="xquery.aspx">XQuery Injection Tests</a>
<%  Response.Write("<h3>");
    Response.Write("Current .NET Framework Version: " + HttpRuntime.TargetFramework.ToString());
    Response.Write("<br />" + "Unsafe Tests: 6" + "<br />" + "Safe Tests: 4");
    Response.Write("</h3>");
%>

<% int testCount = 20; %>
<ul>
    <li>SELECT Statements
        <ol start="<%= testCount %>">
            <li><a href="hqlview.aspx?title=Safe%20NHibernate&test=SELECT%3A%20Safe%20when%20Using%20Built-in%20Functions%20Example&var=SafeDefault">Safe when Using Built-in Functions Example</a></li>
            <li><a href="hqlview.aspx?title=Unsafe%20NHibernate&test=SELECT%3A%20Unsafe%20when%20Using%20String%20Concatenation%20on%20Custom%20HQL%20Queries%20%28CreateQuery%29%20Example&var=Unsafe">Unsafe when Using String Concatenation on Custom HQL Queries (CreateQuery) Example</a></li>
            <li><a href="hqlview.aspx?title=Unsafe%20NHibernate&test=SELECT%3A%20Unsafe%20when%20Using%20String%20Concatenation%20on%20Custom%20SQL%20Queries%20%28CreateSQLQuery%29%20Example&var=UnsafeSQL">Unsafe when Using String Concatenation on Custom SQL Queries (CreateSQLQuery) Example</a></li>
            <li><a href="hqlview.aspx?title=Unsafe%20NHibernate&test=SELECT%3A%20Unsafe%20when%20Using%20String%20Placeholders%20on%20Custom%20HQL%20Queries%20%28CreateQuery%29%20Example&var=UnsafeHQLStringPlace">Unsafe when Using String Placeholders on Custom HQL Queries (CreateQuery) Example</a></li>
            <li><a href="hqlview.aspx?title=Unsafe%20NHibernate&test=SELECT%3A%20Unsafe%20when%20Using%20String%20Placeholders%20on%20Custom%20SQL%20Queries%20%28CreateSQLQuery%29%20Example&var=UnsafeSQLStringPlace">Unsafe when Using String Placeholders on Custom SQL Queries (CreateSQLQuery) Example</a></li>
            <li><a href="hqlview.aspx?title=Safe%20NHibernate&test=SELECT%3A%20Safe%20when%20Parameterizing%20Custom%20HQL%20Queries%20%28CreateQuery%29%20Example&var=SafeParam">Safe when Parameterizing Custom HQL Queries (CreateQuery) Example</a></li>
            <li><a href="hqlview.aspx?title=Safe%20NHibernate&test=SELECT%3A%20Safe%20when%20Parameterizing%20Custom%20SQL%20Queries%20%28CreateSQLQuery%29%20Example&var=SafeParamSQL">Safe when Parameterizing Custom SQL Queries (CreateSQLQuery) Example</a></li>
        </ol>
    <br /></li>

    <li>DELETE Statements
        <ol start="<%= testCount += 7 %>">
            <li><a href="hqlview.aspx?title=Unsafe%20NHibernate&test=DELETE%3A%20Unsafe%20when%20Using%20String%20Concatenation%20on%20Custom%20HQL%20Queries%20Example&var=DeleteUnsafe">Unsafe when Using String Concatenation on Custom HQL Queries Example</a></li>
            <li><a href="hqlview.aspx?title=Unsafe%20NHibernate&test=DELETE%3A%20Unsafe%20when%20Using%20String%20Placeholders%20on%20Custom%20HQL%20Queries%20Example&var=DeleteUnsafeStringPlace">Unsafe when Using String Placeholders on Custom HQL Queries Example</a></li>
            <li><a href="hqlview.aspx?title=Safe%20NHibernate&test=DELETE%3A%20Safe%20when%20Parameterizing%20Custom%20HQL%20Queries%20Example&var=DeleteSafeParam">Safe when Parameterizing Custom HQL Queries Example</a></li>
        </ol>
    <br /></li>
</ul>

<br /><br />
References:
<ol>
    <li><a href="http://nhibernate.info/doc/index.html">NHibernate Docs</a></li>
    <li><a href="https://www.owasp.org/index.php/Hibernate">OWASP: Hibernate</a></li>
    <li><a href="https://www.owasp.org/index.php/SQL_Injection">OWASP: SQL Injection</a></li>
</ol>

</body>
</html>
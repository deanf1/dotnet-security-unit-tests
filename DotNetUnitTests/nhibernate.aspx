﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="nhibernate.aspx.cs" Inherits="DotNetUnitTests.nhibernate" %>
<%@ Import Namespace="Microsoft.Win32" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>.NET NHibernate Tests</title>
</head>
<body>

<h1>NHibernate Injection Vulnerability Tests</h1>
<a href="index.aspx">.NET XXE Injection Tests</a> | <a href="nhibernate.aspx">NHibernate Injection Tests</a>
<%  Response.Write("<h3>");
    Response.Write("Current .NET Framework Version: " + HttpRuntime.TargetFramework.ToString());
    Response.Write("<br />" + "Unsafe Tests: 1" + "<br />" + "Safe Tests: 2");
    Response.Write("</h3>");
%>

<ul>
    <li><a href="hqlview.aspx?title=Safe%20NHibernate&test=NHibernate%3A%20Safe%20when%20Using%20Built-in%20Functions%20Example&var=safedefault">Safe when Using Built-in Functions Example</a></li>
    <li><a href="hqlview.aspx?title=Unsafe%20NHibernate&test=NHibernate%3A%20Unsafe%20when%20Using%20String%20Concatenation%20on%20Custom%20HQL%20Queries%20Example&var=unsafe">Unsafe when Using String Concatenation on Custom HQL Queries Example</a></li>
    <li><a href="hqlview.aspx?title=Unsafe%20NHibernate&test=NHibernate%3A%20Unsafe%20when%20Using%20String%20Concatenation%20on%20Custom%20SQL%20Queries%20Example&var=unsafesql">Unsafe when Using String Concatenation on Custom SQL Queries Example</a></li>
    <li><a href="hqlview.aspx?title=Safe%20NHibernate&test=NHibernate%3A%20Safe%20when%20Parameterizing%20Custom%20HQL%20Queries%20Example&var=safeparam">Safe when Parameterizing Custom HQL Queries Example</a></li>
    <li><a href="hqlview.aspx?title=Safe%20NHibernate&test=NHibernate%3A%20Safe%20when%20Parameterizing%20Custom%20SQL%20Queries%20Example&var=safeparamsql">Safe when Parameterizing Custom SQL Queries Example</a></li>
</ul>

<br /><br />
References:
<ol>
    <li><a href="https://www.owasp.org/index.php/Hibernate">OWASP: Hibernate</a><br /></li>
    <li><a href="https://www.owasp.org/index.php/SQL_Injection">OWASP: SQL Injection</a></li>
</ol>

</body>
</html>
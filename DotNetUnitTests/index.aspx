<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="DotNetUnitTests.index" %>
<%@ Import Namespace="Microsoft.Win32" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>.NET XXE Tests</title>
</head>
<body>

<h1>XXE Injection Vulnerability Tests for .NET XML Parsers</h1>
<a href="index.aspx">.NET XXE Injection Tests</a> | <a href="nhibernate.aspx">NHibernate Injection Tests</a> | <a href="xpath.aspx">XPath Injection Tests</a> | <a href="xquery.aspx">XQuery Injection Tests</a>
<%  Response.Write("<h3>");
    Response.Write("Current .NET Framework Version: " + HttpRuntime.TargetFramework.ToString());
    if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
        Response.Write("<br />" + "Unsafe Tests: 6" + "<br />" + "Safe Tests: 13"); // Counts for if .NET 4.6 or greater
    else
        Response.Write("<br />" + "Unsafe Tests: 9" + "<br />" + "Safe Tests: 10"); // Counts for all other .NET versions
    Response.Write("</h3>");
%>

<p>Parsers (in alphabetical order by parser):</p>
<ul>
    <li><a href="https://msdn.microsoft.com/en-us/library/system.xml.linq(v=vs.110).aspx">System.Xml.<b>Linq</b> (link to MSDN docs)</a>
        <ul>
            <li><a href="xmlview.aspx?title=Safe%20LINQ&test=LINQ%3A%20XElement%3A%20Safe%20by%20Default%20Example&var=linqxelementsafe">XElement: Safe by Default Example</a></li>
            <li><a href="xmlview.aspx?title=Safe%20LINQ&test=LINQ%3A%20XDocument%3A%20Safe%20by%20Default%20Example&var=linqxdocumentsafe">XDocument: Safe by Default Example</a></li>
            <li><a href="xmlview.aspx?title=Safe%20LINQ&test=LINQ%3A%20XDocument%3A%20Unsafe%20when%20Providing%20an%20Unsafe%20XML%20Parser%20Example&var=linqxdocumentunsafe">XDocument: Unsafe when Providing an Unsafe XML Parser Example</a></li>
        </ul>
    <br /></li>
    
    <li><a href="https://msdn.microsoft.com/en-us/library/system.xml.xmldictionaryreader(v=vs.110).aspx">System.Xml.<b>XmlDictionaryReader</b> (link to MSDN docs)</a>
        <ul>
            <li><a href="xmlview.aspx?title=Safe%20XmlDictionaryReader&test=XmlDictionaryReader%3A%20Safe%20by%20Default%20Example&var=xmldictionaryreadersafe">Safe by Default Example</a></li>
            <li><a href="xmlview.aspx?title=Unsafe%20XmlDictionaryReader&test=XmlDictionaryReader%3A%20Unsafe%20when%20Providing%20an%20Unsafe%20XML%20Parser%20Example&var=xmldictionaryreaderunsafe">Unsafe when Providing an Unsafe XML Parser Example</a></li>
        </ul>
    <br /></li>

    <li><a href="https://msdn.microsoft.com/en-us/library/system.xml.xmldocument(v=vs.110).aspx">System.Xml.<b>XmlDocument</b> (link to MSDN docs)</a>
        <ul>
            <%  if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
                    Response.Write("<li><a href=\"xmlview.aspx?title=Safe%20XmlDocument&test=XmlDocument%3A%20Safe%20by%20Default%20in%20Current%20.NET%20Version%20%284.5.2%20and%20above%29%20Example&var=xmldocumentsafe452\">Safe by Default in Current .NET Version (4.5.2 and above) Example</a></li>");
                else
                    Response.Write("<li><a href=\"xmlview.aspx?title=Unsafe%20XmlDocument&test=XmlDocument%3A%20Unsafe%20by%20Default%20in%20Current%20.NET%20Version%20%284.5.1%20and%20lower%29%20Example&var=xmldocumentsafe452\">Unsafe by Default in Current .NET Version (4.5.1 and lower) Example</a></li>");
            %>
            <li><a href="xmlview.aspx?title=Safe%20XmlDocument&test=XmlDocument%3A%20Safe%20when%20Setting%20the%20XmlResolver%20to%20null%20Example&var=xmldocumentsafe">Safe when Setting the XmlResolver to null Example</a></li>
            <li><a href="xmlview.aspx?title=Unsafe%20XmlDocument&test=XmlDocument%3A%20Unsafe%20when%20Resolving%20Entities%20Manually%20Example&var=xmldocumentunsafe">Unsafe when Resolving Entities Manually Example</a></li>
        </ul>
    <br /></li>

    <li><a href="https://msdn.microsoft.com/en-us/library/system.xml.xmlnodereader(v=vs.110).aspx">System.Xml.<b>XmlNodeReader</b> (link to MSDN docs)</a>
        <ul>
            <li><a href="xmlview.aspx?title=Safe%20XmlNodeReader&test=XmlNodeReader%3A%20Safe%20by%20Default%20Example&var=xmlnodereadersafe">Safe by Default Example</a></li>
            <li><a href="xmlview.aspx?title=Safe%20XmlNodeReader&test=XmlNodeReader%3A%20Safe%20when%20Wrapping%20in%20an%20Unsafe%20XmlReader%20Example&var=xmlnodereadersafexmlreader">Safe when Wrapping in an Unsafe XmlReader Example</a></li>
        </ul>
    <br /></li>

    <li><a href="https://msdn.microsoft.com/en-us/library/system.xml.xmlreader(v=vs.110).aspx">System.Xml.<b>XmlReader</b> (link to MSDN docs)</a>
        <ul>
            <li><a href="xmlview.aspx?title=Safe%20XmlReader&test=XmlReader%3A%20Safe%20by%20Default%20Example&var=xmlreadersafe">Safe by Default Example</a></li>
            <li><a href="xmlview.aspx?title=Unsafe%20XmlReader&test=XmlReader%3A%20Unsafe%20when%20Resolving%20Entities%20Manually%20Example&var=xmlreaderunsafe">Unsafe when Resolving Entities Manually Example</a></li>
        </ul>
    <br /></li>
	
    <li><a href="https://msdn.microsoft.com/en-us/library/system.xml.xmltextreader(v=vs.110).aspx">System.Xml.<b>XmlTextReader</b> (link to MSDN docs)</a>
        <ul>
            <%  if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
                    Response.Write("<li><a href=\"xmlview.aspx?title=Safe%20XmlTextReader&test=XmlTextReader%3A%20Safe%20by%20Default%20in%20Current%20.NET%20Version%20%284.5.2%20and%20above%29%20Example&var=xmltextreadersafe452\">Safe by Default in Current .NET Version (4.5.2 and above) Example</a></li>");
                else
                    Response.Write("<li><a href=\"xmlview.aspx?title=Unsafe%20XmlTextReader&test=XmlTextReader%3A%20Unsafe%20by%20Default%20in%20Current%20.NET%20Version%20%284.5.1%20and%20lower%29%20Example&var=xmltextreadersafe452\">Unsafe by Default in Current .NET Version (4.5.1 and lower) Example</a></li>");
            %>
            <li><a href="xmlview.aspx?title=Safe%20XmlTextReader&test=XmlTextReader%3A%20Safe%20when%20Prohibiting%20DTDs%20Example&var=xmltextreadersafe">Safe when Prohibiting DTDs Example</a></li>
            <li><a href="xmlview.aspx?title=Unsafe%20XmlTextReader&test=XmlTextReader%3A%20Unsafe%20when%20Resolving%20Entities%20Manually%20Example&var=xmltextreaderunsafe">Unsafe when Resolving Entities Manually Example</a></li>
        </ul>
    <br /></li>
	
    <li><a href="https://msdn.microsoft.com/en-us/library/system.xml.xpath.xpathnavigator(v=vs.110).aspx">System.Xml.XPath.<b>XPathNavigator</b> (link to MSDN docs)</a>
        <ul>
            <%  if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
                    Response.Write("<li><a href=\"xmlview.aspx?title=Safe%20XPathNavigator&test=XPathNavigator%3A%20Safe%20by%20Default%20in%20Current%20.NET%20Version%20%284.5.2%20and%20above%29%20Example&var=xpathnavigatorsafe452\">Safe by Default in Current .NET Version (4.5.2 and above) Example</a></li>");
                else
                    Response.Write("<li><a href=\"xmlview.aspx?title=Unsafe%20XPathNavigator&test=XPathNavigator%3A%20Unsafe%20by%20Default%20in%20Current%20.NET%20Version%20%284.5.1%20and%20lower%29%20Example&var=xpathnavigatorsafe452\">Unsafe by Default in Current .NET Version (4.5.1 and lower) Example</a></li>");
            %>
            <li><a href="xmlview.aspx?title=Safe%20XPathNavigator&test=XPathNavigator%3A%20Safe%20when%20Providing%20a%20Safe%20XML%20Parser%20Example&var=xpathnavigatorsafe">Safe when Providing a Safe XML Parser Example</a></li>
        </ul>
    <br /></li>
	
    <li><a href="https://msdn.microsoft.com/en-us/library/system.xml.xsl.xslcompiledtransform(v=vs.110).aspx">System.Xml.Xsl.<b>XslCompiledTransform</b> (link to MSDN docs)</a>
        <ul>
            <li><a href="xmlview.aspx?title=Safe%20XslCompiledTransform&test=XslCompiledTransform%3A%20Safe%20by%20Default%20Example&var=xslcompiledtransformsafe">Safe by Default Example</a></li>
            <li><a href="xmlview.aspx?title=Unsafe%20XslCompiledTransform&test=XslCompiledTransform%3A%20Unsafe%20when%20Providing%20an%20Unsafe%20XML%20Parser%20Example&var=xslcompiledtransformunsafe">Unsafe when Providing an Unsafe XML Parser Example</a></li>
        </ul>
    </li>
</ul>

<br /><br />
References (note: test results in references may not be accurate to the test results here):
<ol>
    <li><a href="https://www.owasp.org/index.php/XML_External_Entity_(XXE)_Prevention_Cheat_Sheet#.NET">OWASP XML External Entity (XXE) Prevention Cheat Sheet</a></li>
    <li><a href="https://www.jardinesoftware.net/2016/05/26/xxe-and-net/">"XXE and .NET" by James Jardine</a></li>
    <li><a href="https://www.jardinesoftware.net/2016/09/12/xxe-in-net-and-xpathdocument/">"XXE in .NET and XPathDocument" by James Jardine</a></li>
    <li><a href="https://blogs.msdn.microsoft.com/xmlteam/2005/11/16/introducing-xslcompiledtransform/">Microsoft XML Team: Introducing XslCompiledTransform</a></li>
    <li><a href="https://github.com/dotnet/docs/blob/master/docs/visual-basic/programming-guide/concepts/linq/linq-to-xml-security.md">.NET Team: LINQ to XML Security</a></li> 
    <li><a href="http://www.dotnetframework.org/default.aspx/4@0/4@0/DEVDIV_TFS/Dev10/Releases/RTMRel/ndp/fx/src/Xml/System/Xml/Xslt/XslCompiledTransform@cs/1305376/XslCompiledTransform@cs">XslCompiledTransform.cs Source Code</a></li>  
</ol>

</body>
</html>
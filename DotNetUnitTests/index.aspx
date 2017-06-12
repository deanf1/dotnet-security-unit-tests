<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="xxetestwebdotnet.index" %>
<%@ Import Namespace="Microsoft.Win32" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>.NET XXE Tests</title>
</head>
<body>

<h1>XXE Vulnerability Tests for .NET XML Parsers</h1>
<%  Response.Write("<h3>");
    Response.Write("Current .NET Framework Version: " + HttpRuntime.TargetFramework.ToString());
    if (HttpRuntime.TargetFramework.Minor >= 6)
        Response.Write("<br />" + "Unsafe Tests: tbd" + "<br />" + "Safe Tests: tbd"); // Counts for if .NET 4.6 or greater
    else
        Response.Write("<br />" + "Unsafe Tests: tbd" + "<br />" + "Safe Tests: tbd"); // Counts for all other .NET versions
    Response.Write("</h3>");
%>

<p>Parsers (in alphabetical order by parser):</p>
<ul>
    <li><a href="https://msdn.microsoft.com/en-us/library/system.xml.linq(v=vs.110).aspx">System.Xml.<b>Linq</b> (link to MSDN docs)</a>
        <ul>
            <li><a href="xmlview.aspx?title=Safe%20Linq&test=Safe%20By%20Default%20Example&var=linqsafe">Safe? by Default Example (TODO)</a></li>
        </ul>
    </li><br />
    
    <li><a href="https://msdn.microsoft.com/en-us/library/system.xml.xmldictionaryreader(v=vs.110).aspx">System.Xml.<b>XmlDictionaryReader</b> (link to MSDN docs)</a>
        <ul>
            <li><a href="xmldictionaryreaderunsafedefault.jsp">? by Default Example (TODO)</a></li>
        </ul>
    </li><br />

    <li><a href="https://msdn.microsoft.com/en-us/library/system.xml.xmldocument(v=vs.110).aspx">System.Xml.<b>XmlDocument</b> (link to MSDN docs)</a>
        <ul>
            <%  if (HttpRuntime.TargetFramework.Minor >= 6)
                    Response.Write("<li><a href=\"xmlview.aspx?title=Safe%20XmlDocument&test=Safe%20By%20Default%20in%20.NET%204.6%2B%20Example&var=xmldocumentsafe46\">Safe by Default in .NET 4.6+ Example</a></li>");
                else
                    Response.Write("<li><a href=\"xmlview.aspx?title=Unsafe%20XmlDocument&test=Unsafe%20By%20Default%20in%20Current%20.NET%20Version%20Example&var=xmldocumentsafe46\">Unsafe by Default in Current .NET Version Example</a></li>");
            %>
            <li><a href="xmlview.aspx?title=Safe%20XmlDocument&test=Safe%20when%20Setting%20the%20XmlResolver%20to%20null%20Example&var=xmldocumentsafe">Safe when Setting the XmlResolver to null Example</a></li>
        </ul>
    </li><br />

    <li><a href="https://msdn.microsoft.com/en-us/library/system.xml.xmlreader(v=vs.110).aspx">System.Xml.<b>XmlReader</b> (link to MSDN docs)</a>
        <ul>
            <li><a href="xmlview.aspx?title=Safe%20XmlReader&test=Safe%20By%20Default%20Example&var=xmlreadersafe">Safe by Default Example</a></li>
            <li><a href="xmlview.aspx?title=Unsafe%20XmlReader&test=Unsafe%20when%20Turning%20on%20DTDs%20Manually%20Example&var=xmlreaderunsafe">Unsafe when Turning on DTDs Manually Example</a></li>
        </ul>
    </li><br />
	
    <li><a href="https://msdn.microsoft.com/en-us/library/system.xml.xmltextreader(v=vs.110).aspx">System.Xml.<b>XmlTextReader</b> (link to MSDN docs)</a>
        <ul>
            <li><a href="xmlview.aspx?title=Unsafe%20XmlTextReader&test=Unsafe%20By%20Default%20Example&var=xmltextreaderunsafe">Unsafe by Default Example</a></li>
            <li><a href="xmlview.aspx?title=Safe%20XmlTextReader&test=Safe%20when%20Prohibiting%20DTDs%20Example&var=xmltextreadersafe">Safe when Prohibiting DTDs Example</a></li>
        </ul>
    </li><br />
	
    <li><a href="https://msdn.microsoft.com/en-us/library/system.xml.xpath.xpathnavigator(v=vs.110).aspx">System.Xml.XPath.<b>XPathNavigator</b> (link to MSDN docs)</a>
        <ul>
            <li><a href="xmlreaderunsafedefault.jsp">? by Default Example (TODO)</a></li>
        </ul>
    </li><br />
	
    <li><a href="https://msdn.microsoft.com/en-us/library/system.xml.xsl.xslcompiledtransform(v=vs.110).aspx">System.Xml.Xsl.<b>XslCompiledTransform</b> (link to MSDN docs)</a>
        <ul>
            <li><a href="xmlview.aspx?title=Safe%20XslCompiledTransform&test=Safe%20By%20Default%20Example&var=xslcompiledtransformsafe">Safe by Default Example</a></li>
            <li><a href="xmlview.aspx?title=Unsafe%20XslCompiledTransform&test=Unsafe%20when%20Providing%20an%20Unsafe%20XML%20Parser%20Example&var=xslcompiledtransformunsafe">Unsafe when Providing an Unsafe XML Parser Example</a></li>
        </ul>
    </li>
</ul>

<br /><br />
References:
<ol>
    <li><a href="https://www.owasp.org/index.php/XML_External_Entity_(XXE)_Prevention_Cheat_Sheet#.NET">OWASP XML External Entity (XXE) Prevention Cheat Sheet</a><br /></li>
    <li><a href="https://www.jardinesoftware.net/2016/05/26/xxe-and-net/">"XXE and .NET" by James Jardine</a></li>
    <li><a href="https://blogs.msdn.microsoft.com/xmlteam/2005/11/16/introducing-xslcompiledtransform/">Microsoft XML Team: Introducing XslCompiledTransform</a></li>
</ol>

</body>
</html>
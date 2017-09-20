<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="codeview.aspx.cs" Inherits="DotNetUnitTests.codeview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Code View</title>
</head>
<body>
    <%
        string appPath = Request.PhysicalApplicationPath;
        string[] code = null;
        if (Request.QueryString["type"].Equals("xml"))
            code = System.IO.File.ReadAllLines(appPath + "/TestCases/XXETestCases/" + Request.QueryString["var"] + ".aspx.cs");
        else if (Request.QueryString["type"].Equals("hql"))
            code = System.IO.File.ReadAllLines(appPath + "/TestCases/HQLTestCases/" + Request.QueryString["var"] + ".aspx.cs");
        else if (Request.QueryString["type"].Equals("xpath"))
            code = System.IO.File.ReadAllLines(appPath + "/TestCases/XPathTestCases/" + Request.QueryString["var"] + ".aspx.cs");
        else if (Request.QueryString["type"].Equals("xquery"))
            code = System.IO.File.ReadAllLines(appPath + "/TestCases/XQueryTestCases/" + Request.QueryString["var"] + ".aspx.cs");

        Response.Write("<pre>");
        foreach (string line in code)
        {
            Response.Write(line + "<br />");
        }
        Response.Write("</pre>");

         %>

    <br /><br />
    <a href="<%= Request.UrlReferrer.ToString() %>">&lt&lt&lt back to test</a>
</body>
</html>

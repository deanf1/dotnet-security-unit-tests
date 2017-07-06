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
            code = System.IO.File.ReadAllLines(appPath + "results.aspx.cs");
        else if (Request.QueryString["type"].Equals("hql"))
            code = System.IO.File.ReadAllLines(appPath + "hqlresults.aspx.cs");

        Response.Write("<pre>");
        bool printFlag = false;
        foreach (string line in code)
        {
            if (line.Contains(Request.QueryString["test"]))
                printFlag = true;

            if (printFlag)
                Response.Write(line + "<br />");
            
            if (line.Contains("#endregion"))
                printFlag = false;               
        }
        Response.Write("</pre>");

         %>

    <br /><br />
    <a href="<%= Request.UrlReferrer.ToString() %>">&lt&lt&lt back to test</a>
</body>
</html>

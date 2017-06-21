<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hqlview.aspx.cs" Inherits="DotNetUnitTests.hqlview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%= Request.QueryString["title"] %></title>
</head>
<body>
    <h1><%= Request.QueryString["test"] %></h1>
    <h3>The Student table has the following rows: ID, LastName, FirstName, Username, Password</h3>
    <h3>This form performs a HQL query that gets the row with the first name entered below.</h3>
    <h3>The injection given below will attempt to fetch all table rows instead of the just entered one by adding <mark>' OR 'a'='a</mark> to the end</h3>
    <form id="theform" action="hqlresults.aspx" method="get" autocomplete="off" runat="server">
        <input type="hidden" name="var" value="<%= Request.QueryString["var"] %>" />
        Enter first name: <input type="text" name="payload" value="Bobby' OR 'a'='a" />
        <input type="submit" value="Submit" />
    </form>
    <br /><br /><a href="nhibernate.aspx">&lt&lt&lt back to tests</a>
</body>
</html>

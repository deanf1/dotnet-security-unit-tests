using System;
using System.Collections.Generic;
using System.Web;

namespace DotNetUnitTests.TestCases.XPathTestCases
{
    public class XPathTestCasePage : System.Web.UI.Page
    {
        private string _xPathText = HttpContext.Current.Request.QueryString["payload"];

        /**
         * The vulnerable XPath query
         */
        protected string xPathText { get { return _xPathText; } set { _xPathText = value; } }

        /**
         * The path of the project files
         */
        protected string appPath { get { return HttpContext.Current.Request.PhysicalApplicationPath; } }

        /**
         *  Prints the results
         */
        protected void PrintResults(bool expectedSafe, List<string> resultList)
        {
            bool actuallySafe = true;
            if (resultList.Count > 1)
            {
                actuallySafe = false;
            }

            Response.Write("<h3>");
            Response.Write("Expected result: " + (expectedSafe ? "Safe" : "Unsafe") + "<br />");
            Response.Write("Actual result: " + (actuallySafe ? "XPath query is safe! 😊" : "Unsafe query was injected! 😭") + "<br />");
            Response.Write("</h3>");
            Response.Write("<b> Results of Query (" + (actuallySafe ? "Should be a thrown exception" : "Should be all Students") + "):</b><br /><pre>");

            // print Students
            foreach (String student in resultList)
            {
                Response.Write(student + "<br />");
            }
            Response.Write("</pre>");
        }
    }
}
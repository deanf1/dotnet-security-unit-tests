using System;
using System.Collections.Generic;
using System.Web;

namespace DotNetUnitTests.TestCases.XQueryTestCases
{
    public class XQueryTestCasePage : System.Web.UI.Page
    {
        private string _xQueryText = HttpContext.Current.Request.QueryString["payload"];

        /**
         * The vulnerable XPath query
         */
        protected string xQueryText { get { return _xQueryText; } set { _xQueryText = value; } }

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
            Response.Write("Actual result: " + (actuallySafe ? "XQuery query is safe! 😊" : "Unsafe query was injected! 😭") + "<br />");
            Response.Write("</h3>");
            Response.Write("<b> Results of Query (" + (actuallySafe ? "Should be a thrown exception, one Student, or empty result" : "Should be all Students") + "):</b><br /><pre>");

            // print Students
            foreach (String student in resultList)
            {
                Response.Write(student + "<br />");
            }
            Response.Write("</pre>");
        }
    }
}
using System;
using System.Web;

namespace DotNetUnitTests.TestCases.XXETestCases
{
    public class XXETestCasePage : System.Web.UI.Page
    {
        private string _xmlText = HttpContext.Current.Request.QueryString["payload"];

        /**
         * The vulnerable XML payload
         */
        protected string xmlText { get { return _xmlText; } set { _xmlText = value; } }

        /**
         * The path of the project files
         */
        protected string appPath { get { return HttpContext.Current.Request.PhysicalApplicationPath; } }

        /**
         * Prints the results
         */
        protected void PrintResults(bool expectedSafe, bool actuallySafe, string xmlContent)
        {
            Response.Write("<h3>");
            Response.Write("Expected result: " + (expectedSafe ? "Safe" : "Unsafe") + "<br />");
            Response.Write("Actual result: " + (actuallySafe ? "XML Parser is safe! 😊" : "Unsafe! XXE was injected! 😭") + "<br />");
            Response.Write("</h3>");
            if (!actuallySafe)
                Response.Write("<b>" + "XML Content (Should contain \"INJECTION SUCCESSFUL\" or your custom XML Entity):" + "</b>" + "<br />" + "<textarea rows=\"10\" cols=\"150\" readonly>" + xmlContent + "</textarea>");
            else
            {
                if (xmlContent.Equals("") || String.IsNullOrWhiteSpace(xmlContent))
                    Response.Write("<b>" + "XML Content:" + "</b>" + "<br />" + "The XML file is blank" + "<br /><br />");
                else
                    Response.Write("<b>" + "XML Content:" + "</b>" + "<br />" + "<textarea rows=\"10\" cols=\"150\" readonly>" + xmlContent + "</textarea><br /><br />");
            }
        }

        /**
         * Prints the results if there is an exception
         */
        protected void PrintResults(bool expectedSafe, bool actuallySafe, Exception ex)
        {
            PrintResults(expectedSafe, actuallySafe, "XML was not parsed due to a thrown exception");
            Response.Write("<b>" + "Stack Trace: " + "</b>" + "<br />" + ex.ToString());
        }

        /**
         * Makes sure that the base URI of the external entity file is correct so that it is referenced correctly
         */
        protected string FixXMLBaseURI(string xmlText, string appPath)
        {
            int index = xmlText.IndexOf("SYSTEM \"") + "SYSTEM \"".Length;
            xmlText = xmlText.Insert(index, appPath + "resources/");
            return xmlText;
        }
    }
}
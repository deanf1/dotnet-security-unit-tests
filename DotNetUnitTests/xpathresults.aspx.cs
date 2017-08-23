using System;
using System.Collections.Generic;
using System.Xml.XPath;

namespace DotNetUnitTests
{
    /**
     * Exception for whitelisting XPath query parameter
     */
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException(string message) : base(message) { }
    }

    public partial class xpathresults : System.Web.UI.Page
    {
      
        /**
         *  Detects which test case we're running, runs it, and prints the results
         */
        private void PerformTest(string xPathText)
        {
            string appPath = Request.PhysicalApplicationPath;

            switch (Request.QueryString["var"])
            {
                #region XPath: Unsafe when Using String Concatenation on XPath Expression Example
                /**
                 * Proves that XPath is vulnerable to injection when using string concatenation on the XPath expression
                 */
                case "xpathunsafeconcat":
                    {
                        bool expectedSafe = false;

                        try
                        {
                            // parse the XML
                            XPathDocument doc = new XPathDocument(appPath + "resources/students.xml");
                            XPathNavigator nav = doc.CreateNavigator();

                            // query the XML
                            string query = "/Students/Student[FirstName/text()='" + Request.QueryString["payload"] + "']";  // unsafe!
                            XPathNodeIterator iter = nav.Select(query).Current.SelectDescendants(XPathNodeType.Text, true);

                            // interpret the result of the query
                            List<string> resultList = new List<string>();
                            while (iter.MoveNext())
                            {
                                string student = "";
                                student += "Last Name: " + iter.Current.Value + "\n";   iter.MoveNext();    // last name
                                student += "First Name: " + iter.Current.Value + "\n";  iter.MoveNext();    // first name
                                student += "Username: " + iter.Current.Value + "\n";    iter.MoveNext();    // username
                                student += "Password: " + iter.Current.Value + "\n";                        // password
                                resultList.Add(student);
                            }

                            // print the results on the query
                            PrintResults(expectedSafe, resultList);

                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex.ToString());
                        }

                        break;
                    }
                #endregion

                #region XPath: Unsafe when Using String Placeholders on XPath Expression Example
                /**
                 * Proves that XPath is vulnerable to injection when using string placeholders on the XPath expression
                 */
                case "xpathunsafeplaceholder":
                    {
                        bool expectedSafe = false;

                        try
                        {
                            // parse the XML
                            XPathDocument doc = new XPathDocument(appPath + "resources/students.xml");
                            XPathNavigator nav = doc.CreateNavigator();

                            // query the XML
                            string query = String.Format("/Students/Student[FirstName/text()='{0}']", Request.QueryString["payload"]);  // unsafe!
                            XPathNodeIterator iter = nav.Select(query).Current.SelectDescendants(XPathNodeType.Text, true);

                            // interpret the result of the query
                            List<string> resultList = new List<string>();
                            while (iter.MoveNext())
                            {
                                string student = "";
                                student += "Last Name:\t" + iter.Current.Value + "\n"; iter.MoveNext();    // last name
                                student += "First Name:\t" + iter.Current.Value + "\n"; iter.MoveNext();    // first name
                                student += "Username:\t" + iter.Current.Value + "\n"; iter.MoveNext();    // username
                                student += "Password:\t" + iter.Current.Value + "\n";                        // password
                                resultList.Add(student);
                            }

                            // print the results on the query
                            PrintResults(expectedSafe, resultList);

                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex.ToString());
                        }

                        break;
                    }
                #endregion

                #region XPath: Safe when Whitelisting on XPath Expression Example
                /*
                 * Proves that XPath is safe from injection when whitelisting the XPath expression
                 */
                case "xpathsafelist":
                    {
                        bool expectedSafe = true;

                        try
                        {
                            // parse the XML
                            XPathDocument doc = new XPathDocument(appPath + "resources/students.xml");
                            XPathNavigator nav = doc.CreateNavigator();

                            // query the XML
                            string query;
                            if (Request.QueryString["payload"].Contains("'"))
                            {
                                PrintResults(expectedSafe, new List<string>());
                                throw new InvalidParameterException("First Name parameter must not contain apostrophes");
                            }
                            else
                            {
                                query = String.Format("/Students/Student[FirstName/text()='{0}']", Request.QueryString["payload"]);  // safe in here!
                            }
                            XPathNodeIterator iter = nav.Select(query).Current.SelectDescendants(XPathNodeType.Text, true);

                            // interpret the result of the query
                            List<string> resultList = new List<string>();
                            while (iter.MoveNext())
                            {
                                string student = "";
                                student += "Last Name:\t" + iter.Current.Value + "\n";  iter.MoveNext();    // last name
                                student += "First Name:\t" + iter.Current.Value + "\n"; iter.MoveNext();    // first name
                                student += "Username:\t" + iter.Current.Value + "\n";   iter.MoveNext();    // username
                                student += "Password:\t" + iter.Current.Value + "\n";                       // password
                                resultList.Add(student);
                            }

                            // print the results on the query
                            PrintResults(expectedSafe, resultList);

                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex.ToString());
                        }

                        break;
                    }
                #endregion

                #region XPath: Unsafe when Escaping Apostrophes on XPath Expression Example
                /**
                 * Proves that XPath is unsafe from injection when using string concatenation while escaping apostrophes on
                 * the XPath expression
                 */
                case "xpathunsafeescape":
                    {
                        bool expectedSafe = false;

                        try
                        {
                            // parse the XML
                            XPathDocument doc = new XPathDocument(appPath + "resources/students.xml");
                            XPathNavigator nav = doc.CreateNavigator();

                            // query the XML
                            string userInputFixed = Request.QueryString["payload"].Replace("'", "&apos;");
                            string query = "/Students/Student[FirstName/text()='" + userInputFixed + "']";  // unsafe!
                            XPathNodeIterator iter = nav.Select(query).Current.SelectDescendants(XPathNodeType.Text, true);

                            // interpret the result of the query
                            List<string> resultList = new List<string>();
                            while (iter.MoveNext())
                            {
                                string student = "";
                                student += "Last Name:\t" + iter.Current.Value + "\n"; iter.MoveNext();    // last name
                                student += "First Name:\t" + iter.Current.Value + "\n"; iter.MoveNext();    // first name
                                student += "Username:\t" + iter.Current.Value + "\n"; iter.MoveNext();    // username
                                student += "Password:\t" + iter.Current.Value + "\n";                        // password
                                resultList.Add(student);
                            }

                            // print the results on the query
                            PrintResults(expectedSafe, resultList);

                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex.ToString());
                        }

                        break;
                    }
                #endregion

                // default case
                default:
                    Response.Write("Error: Test case not found");
                    break;
            }
        }

        /**
         *  Prints the results
         */
        private void PrintResults(bool expectedSafe, List<string> resultList)
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

        protected void Page_Load(object sender, EventArgs e)
        {
            string xPathText = Request.QueryString["payload"];

            PerformTest(xPathText);
        }
    }
}
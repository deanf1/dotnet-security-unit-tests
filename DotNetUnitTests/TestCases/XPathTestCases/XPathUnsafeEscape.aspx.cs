using System;
using System.Collections.Generic;
using System.Xml.XPath;

namespace DotNetUnitTests.TestCases.XPathTestCases
{
    public partial class XPathUnsafeEscape : XPathTestCasePage
    {
        /**
         * XPath: Unsafe when Escaping Apostrophes on XPath Expression Example
         * Proves that XPath is unsafe from injection when using string concatenation while escaping apostrophes on
         * the XPath expression
         */
        protected void Page_Load(object sender, EventArgs e)
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
        }
    }
}
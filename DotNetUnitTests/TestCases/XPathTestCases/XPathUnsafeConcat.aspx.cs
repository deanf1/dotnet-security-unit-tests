using System;
using System.Collections.Generic;
using System.Xml.XPath;

namespace DotNetUnitTests.TestCases.XPathTestCases
{
    public partial class XPathUnsafeConcat : XPathTestCasePage
    {
        /**
         * XPath: Unsafe when Using String Concatenation on XPath Expression Example
         * Proves that XPath is vulnerable to injection when using string concatenation on the XPath expression
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
                string query = "/Students/Student[FirstName/text()='" + Request.QueryString["payload"] + "']";  // unsafe!
                XPathNodeIterator iter = nav.Select(query).Current.SelectDescendants(XPathNodeType.Text, true);

                // interpret the result of the query
                List<string> resultList = new List<string>();
                while (iter.MoveNext())
                {
                    string student = "";
                    student += "Last Name: " + iter.Current.Value + "\n"; iter.MoveNext();  // last name
                    student += "First Name: " + iter.Current.Value + "\n"; iter.MoveNext(); // first name
                    student += "Username: " + iter.Current.Value + "\n"; iter.MoveNext();   // username
                    student += "Password: " + iter.Current.Value + "\n";                    // password
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
﻿using System;
using System.Collections.Generic;
using System.Xml.XPath;

namespace DotNetUnitTests.TestCases.XPathTestCases
{
    public partial class XPathSafeList : XPathTestCasePage
    {
        /**
         * XPath: Safe when Whitelisting on XPath Expression Example
         * Proves that XPath is safe from injection when whitelisting the XPath expression
         */
        protected void Page_Load(object sender, EventArgs e)
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
                    student += "Last Name:\t" + iter.Current.Value + "\n"; iter.MoveNext();    // last name
                    student += "First Name:\t" + iter.Current.Value + "\n"; iter.MoveNext();    // first name
                    student += "Username:\t" + iter.Current.Value + "\n"; iter.MoveNext();    // username
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
        }
    }
}
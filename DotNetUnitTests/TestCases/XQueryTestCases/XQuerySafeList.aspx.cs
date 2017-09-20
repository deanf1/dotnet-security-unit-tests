using System;
using System.Collections.Generic;
using net.sf.saxon.s9api;
using javax.xml.transform.stream;

namespace DotNetUnitTests.TestCases.XQueryTestCases
{
    public partial class XQuerySafeList : XQueryTestCasePage
    {
       /**
        * Saxon: Safe when Whitelisting on XQuery Expression Example
        * Proves that Saxon is safe from injection when whitelisting the XQuery expression
        */
        protected void Page_Load(object sender, EventArgs e)
        {
            bool expectedSafe = false;

            try
            {
                // parse the XML
                Processor processor = new Processor(false);
                DocumentBuilder doc = processor.newDocumentBuilder();
                XdmNode node = doc.build(new StreamSource(appPath + "/resources/students.xml"));

                // query the XML
                string query;
                if (Request.QueryString["payload"].Contains("\"") || Request.QueryString["payload"].Contains(";"))
                {
                    PrintResults(expectedSafe, new List<string>());
                    throw new InvalidParameterException("First Name parameter must not contain quotes or semicolons");
                }
                else
                {
                    query = "for $s in //Students/Student " +
                            "where $s/FirstName = \"" + Request.QueryString["payload"] + "\" " +
                            "return $s";    // safe in here!
                }
                XQueryCompiler xqComp = processor.newXQueryCompiler();
                XQueryExecutable xqExec = xqComp.compile(query);
                XQueryEvaluator xqEval = xqExec.load();
                xqEval.setContextItem(node);
                xqEval.evaluate();

                // interpret the result of the query
                List<string> resultList = new List<string>();
                foreach (XdmValue value in xqEval)
                {
                    resultList.Add(value.ToString());
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
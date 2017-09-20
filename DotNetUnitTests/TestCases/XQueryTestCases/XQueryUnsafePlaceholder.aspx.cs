using System;
using System.Collections.Generic;
using net.sf.saxon.s9api;
using javax.xml.transform.stream;

namespace DotNetUnitTests.TestCases.XQueryTestCases
{
    public partial class XQueryUnsafePlaceholder : XQueryTestCasePage
    {
       /**
        * Saxon: Unsafe when Using String Placeholders on XQuery Expression Example
        * Proves that Saxon is vulnerable to injection when using string placeholders on the XQuery expression
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
                string query = String.Format("for $s in //Students/Student " +
                                "where $s/FirstName = \"{0}\" " +
                                "return $s", Request.QueryString["payload"]);  // unsafe!
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
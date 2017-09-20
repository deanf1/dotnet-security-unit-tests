using System;
using System.Collections.Generic;
using net.sf.saxon.s9api;
using javax.xml.transform.stream;

namespace DotNetUnitTests.TestCases.XQueryTestCases
{
    public partial class XQuerySafeEscape : XQueryTestCasePage
    {
       /**
        * Saxon: Safe when Escaping Quotation Marks and Semicolons on XQuery Expression Example
        * Proves that Saxon is safe from injection when using character escaping on the XQuery expression
        */
        protected void Page_Load(object sender, EventArgs e)
        {
            bool expectedSafe = true;

            try
            {
                // parse the XML
                Processor processor = new Processor(false);
                DocumentBuilder doc = processor.newDocumentBuilder();
                XdmNode node = doc.build(new StreamSource(appPath + "/resources/students.xml"));

                // query the XML
                string newPayload = (Request.QueryString["payload"].Replace(";", "&#59")).Replace("\"", "&quot;");
                string query = "for $s in //Students/Student " +
                                "where $s/FirstName = \"" + newPayload + "\" " +
                                "return $s";  // safe!
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
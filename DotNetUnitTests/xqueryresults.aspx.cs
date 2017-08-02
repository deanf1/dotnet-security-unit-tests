using System;
using System.Collections.Generic;
using net.sf.saxon.s9api;
using javax.xml.transform.stream;
using System.IO;
using System.Xml;

namespace DotNetUnitTests
{

    public partial class xqueryresults : System.Web.UI.Page
    {
      
        /**
         *  Detects which test case we're running, runs it, and prints the results
         */
        private void PerformTest(string hqltext)
        {
            string appPath = Request.PhysicalApplicationPath;

            switch (Request.QueryString["var"])
            {
                #region Saxon: Unsafe when Using String Concatenation on XQuery Expression Example
                /**
                 * Proves that Saxon is vulnerable to injection when using string concatenation on the XQuery expression
                 */
                case "xqueryunsafeconcat":
                    {
                        bool expectedSafe = false;

                        try
                        {
                            // parse the XML
                            Processor processor = new Processor(false);
                            DocumentBuilder doc = processor.newDocumentBuilder();
                            XdmNode node = doc.build(new StreamSource(appPath + "/resources/students.xml"));       

                            // query the XML
                            string query = "for $s in //Students/Student " +
                                            "where $s/FirstName = \"" + Request.QueryString["payload"] + "\" " +
                                            "return $s";  // unsafe!
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

                        break;
                    }
                #endregion

                #region Saxon: Unsafe when Using String Placeholders on XQuery Expression Example
                /**
                 * Proves that Saxon is vulnerable to injection when using string placeholders on the XQuery expression
                 */
                case "xqueryunsafeplaceholder":
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

                        break;
                    }
                #endregion

                #region Saxon: Safe when Using Bind Variables on XQuery Expression Example
                /**
                 * Proves that Saxon is safe from injection when using bind variables on the XQuery expression
                 */
                case "xquerysafebind":
                    {
                        bool expectedSafe = true;

                        try
                        {
                            // parse the XML
                            Processor processor = new Processor(false);
                            DocumentBuilder doc = processor.newDocumentBuilder();
                            XdmNode node = doc.build(new StreamSource(appPath + "/resources/students.xml"));

                            // query the XML
                            string query =  "declare variable $name as xs:string external; " +
                                            "for $s in //Students/Student " +
                                            "where $s/FirstName = $name " +
                                            "return $s";  // safe!
                            XQueryCompiler xqComp = processor.newXQueryCompiler();
                            XQueryExecutable xqExec = xqComp.compile(query);
                            XQueryEvaluator xqEval = xqExec.load();
                            xqEval.setContextItem(node);
                            xqEval.setExternalVariable(new QName("name"), new XdmAtomicValue(Request.QueryString["payload"]));
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

                        break;
                    }
                #endregion

                #region Saxon: Safe when Whitelisting on XQuery Expression Example
                /*
                 * Proves that Saxon is safe from injection when whitelisting the XQuery expression
                 */
                case "xquerysafelist":
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

                        break;
                    }
                #endregion

                #region Saxon: Safe when Escaping Quotation Marks and Semicolons on XQuery Expression Example
                /**
                 * Proves that Saxon is safe from injection when using character escaping on the XQuery expression
                 */
                case "xquerysafeescape":
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

        protected void Page_Load(object sender, EventArgs e)
        {
            string hqltext = Request.QueryString["payload"];

            PerformTest(hqltext);
        }
    }
}
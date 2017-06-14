using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace xxetestwebdotnet
{
    public partial class results : System.Web.UI.Page
    {

        /**
         *  Detects which test case we're running, runs it, and prints the results
         */
        private void PerformTest(string xmltext)
        {
            string appPath = Request.PhysicalApplicationPath;

            switch (Request.QueryString["var"])
            {
                #region Linq: XElement: Safe by Default Example
                case "linqxelementsafe":
                    {
                        bool expectedSafe = true;

                        XElement xelement = XElement.Load(appPath + "resources/xxetestuser.xml");

                        try
                        {
                            // parsing the XML                          
                            StringBuilder sb = new StringBuilder();
                            foreach (var element in xelement.Elements())
                            {
                                sb.Append(element.ToString());
                            }

                            // testing the result
                            if (sb.ToString().Contains("SUCCESSFUL"))
                                PrintResults(expectedSafe, false, sb.ToString());   // unsafe: successful XXE injection
                            else
                                PrintResults(expectedSafe, true, sb.ToString());    // safe: empty or unparsed XML
                        }
                        catch (Exception ex)
                        {
                            PrintResults(expectedSafe, true, ex);   // safe: exception thrown when parsing XML
                        }

                        break;
                    }
                #endregion

                #region Linq: XDocument: Safe by Default Example
                case "linqxdocumentsafe":
                    {
                        bool expectedSafe = true;

                        XDocument xdocument = XDocument.Load(appPath + "resources/xxetestuser.xml");

                        try
                        {
                            // parsing the XML                          
                            StringBuilder sb = new StringBuilder();
                            foreach (var element in xdocument.Elements())
                            {
                                sb.Append(element.ToString());
                            }

                            // testing the result
                            if (sb.ToString().Contains("SUCCESSFUL"))
                                PrintResults(expectedSafe, false, sb.ToString());   // unsafe: successful XXE injection
                            else
                                PrintResults(expectedSafe, true, sb.ToString());    // safe: empty or unparsed XML
                        }
                        catch (Exception ex)
                        {
                            PrintResults(expectedSafe, true, ex);   // safe: exception thrown when parsing XML
                        }

                        break;
                    }
                #endregion

                #region Linq: XDocument: Unsafe when Providing an Unsafe XML Parser Example
                case "linqxdocumentunsafe":
                    {
                        bool expectedSafe = false;

                        XmlReaderSettings settings = new XmlReaderSettings();
                        settings.DtdProcessing = DtdProcessing.Parse;   // unsafe!

                        // forcing unsafe in .NET versions 4.5.2+
                        if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
                        {
                            XmlUrlResolver res = new XmlUrlResolver();
                            //res.ResolveUri(new Uri(Environment.CurrentDirectory), "resources/xxetestuser.xml"); // works but not needed
                            settings.XmlResolver = res;
                        }
                        XmlReader reader = XmlReader.Create(appPath + "resources/xxetestuser.xml", settings);
                        XDocument xdocument = XDocument.Load(reader);    // unsafe!

                        try
                        {
                            // parsing the XML                          
                            StringBuilder sb = new StringBuilder();
                            foreach (var element in xdocument.Elements())
                            {
                                sb.Append(element.ToString());
                            }

                            // testing the result
                            if (sb.ToString().Contains("SUCCESSFUL"))
                                PrintResults(expectedSafe, false, sb.ToString());   // unsafe: successful XXE injection
                            else
                                PrintResults(expectedSafe, true, sb.ToString());    // safe: empty or unparsed XML
                        }
                        catch (Exception ex)
                        {
                            PrintResults(expectedSafe, true, ex);   // safe: exception thrown when parsing XML
                        }
                        finally
                        {
                            reader.Close();
                        }

                        break;
                    }
                #endregion

                #region XmlDictionaryReader: Safe by Default Example
                case "xmldictionaryreadersafe":
                    {
                        bool expectedSafe = true;

                        XmlDictionaryReader dict = XmlDictionaryReader.CreateTextReader(Encoding.ASCII.GetBytes(xmltext), XmlDictionaryReaderQuotas.Max);

                        try
                        {
                            // parsing the XML  
                            StringBuilder sb = new StringBuilder();
                            while (dict.Read())
                            {
                                sb.Append(dict.Value);
                            }

                            // testing the result
                            if (sb.ToString().Contains("SUCCESSFUL"))
                                PrintResults(expectedSafe, false, sb.ToString());   // unsafe: successful XXE injection
                            else
                                PrintResults(expectedSafe, true, sb.ToString());    // safe: empty or unparsed XML
                        }
                        catch (Exception ex)
                        {
                            PrintResults(expectedSafe, true, ex);   // safe: exception thrown when parsing XML
                        }
                        finally
                        {
                            dict.Close();
                        }

                        break;
                    }
                #endregion
                    
                #region XmlDictionaryReader: Unsafe when Providing an Unsafe XML Parser Example
                case "xmldictionaryreaderunsafe":
                    {
                        bool expectedSafe = false;

                        XmlReaderSettings settings = new XmlReaderSettings();
                        settings.DtdProcessing = DtdProcessing.Parse;   // unsafe!

                        // forcing unsafe in .NET versions 4.5.2+
                        if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
                        {
                            XmlUrlResolver res = new XmlUrlResolver();
                            //res.ResolveUri(new Uri(Environment.CurrentDirectory), "resources/xxetestuser.xml"); // works but not needed
                            settings.XmlResolver = res;
                        }

                        XmlReader reader = XmlReader.Create(appPath + "resources/xxetestuser.xml", settings);
                        XmlDictionaryReader dict = XmlDictionaryReader.CreateDictionaryReader(reader);
                        
                        try
                        {
                            // parsing the XML  
                            StringBuilder sb = new StringBuilder();
                            while (dict.Read())
                            {
                                sb.Append(dict.Value);
                            }

                            // testing the result
                            if (sb.ToString().Contains("SUCCESSFUL"))
                                PrintResults(expectedSafe, false, sb.ToString());   // unsafe: successful XXE injection
                            else
                                PrintResults(expectedSafe, true, sb.ToString());    // safe: empty or unparsed XML
                        }
                        catch (Exception ex)
                        {
                            PrintResults(expectedSafe, true, ex);   // safe: exception thrown when parsing XML
                        }
                        finally
                        {
                            dict.Close();
                            reader.Close();
                        }

                        break;
                    }
                #endregion

                #region XmlDocument: Safe by Default in .NET Version 4.5.2 and above / Unsafe by Default in .NET Version 4.5.1 and lower Example
                case "xmldocumentsafe452":
                    {
                        bool expectedSafe = false;
                        if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
                            expectedSafe = true;

                        try
                        {
                            // parsing the XML   
                            XmlDocument doc = new XmlDocument();    
                            doc.Load(appPath + "resources/xxetestuser.xml");    // unsafe! (safe in .NET versions 4.5.2+)
                            string innerText = doc.InnerText;

                            // testing the result
                            if (innerText.Contains("SUCCESSFUL"))
                                PrintResults(expectedSafe, false, innerText);   // unsafe: successful XXE injection
                            else
                                PrintResults(expectedSafe, true, innerText);    // safe: empty or unparsed XML
                        }
                        catch (Exception ex)
                        {
                            PrintResults(expectedSafe, true, ex);   // safe: exception thrown when parsing XML
                        }

                        break;
                    }
                #endregion

                #region XmlDocument: Safe when Setting the XmlResolver to null Example 
                case "xmldocumentsafe":
                    {
                        bool expectedSafe = true;

                        try
                        {
                            // parsing the XML  
                            XmlDocument doc = new XmlDocument();
                            doc.XmlResolver = null; // safety measure
                            doc.Load(appPath + "resources/xxetestuser.xml");
                            string innerText = doc.InnerText;

                            // testing the result
                            if (innerText.Contains("SUCCESSFUL"))
                                PrintResults(expectedSafe, false, innerText);   // unsafe: successful XXE injection
                            else
                                PrintResults(expectedSafe, true, innerText);    // safe: empty or unparsed XML
                        }
                        catch (Exception ex)
                        {
                            PrintResults(expectedSafe, true, ex);   // safe: exception thrown when parsing XML
                        }

                        break;
                    }
                #endregion

                #region XmlReader: Safe by Default Example
                case "xmlreadersafe":
                    {
                        bool expectedSafe = true;

                        XmlReader reader = XmlReader.Create(appPath + "resources/xxetestuser.xml");

                        try
                        {
                            // parsing the XML                          
                            StringBuilder sb = new StringBuilder();
                            while (reader.Read())
                            {
                                sb.Append(reader.Value);
                            }

                            // testing the result
                            if (sb.ToString().Contains("SUCCESSFUL"))
                                PrintResults(expectedSafe, false, sb.ToString());   // unsafe: successful XXE injection
                            else
                                PrintResults(expectedSafe, true, sb.ToString());    // safe: empty or unparsed XML
                        }
                        catch (Exception ex)
                        {
                            PrintResults(expectedSafe, true, ex);   // safe: exception thrown when parsing XML
                        }
                        finally
                        {
                            reader.Close();
                        }

                        break;
                    }
                #endregion

                #region XmlReader: Unsafe when Turning on DTDs Manually Example
                case "xmlreaderunsafe":
                    {
                        bool expectedSafe = false;

                        XmlReaderSettings settings = new XmlReaderSettings();
                        settings.DtdProcessing = DtdProcessing.Parse;   // unsafe!

                        // forcing unsafe in .NET versions 4.5.2+
                        if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
                        {
                            XmlUrlResolver res = new XmlUrlResolver();
                            //res.ResolveUri(new Uri(Environment.CurrentDirectory), "resources/xxetestuser.xml"); // works but not needed
                            settings.XmlResolver = res;
                        }
                        XmlReader reader = XmlReader.Create(appPath + "resources/xxetestuser.xml", settings);

                        try
                        {
                            // parsing the XML
                            StringBuilder sb = new StringBuilder();
                            while (reader.Read())
                            {
                                sb.Append(reader.Value);
                            }

                            // testing the result
                            if (sb.ToString().Contains("SUCCESSFUL"))
                                PrintResults(expectedSafe, false, sb.ToString());   // unsafe: successful XXE injection
                            else
                                PrintResults(expectedSafe, true, sb.ToString());    // safe: empty or unparsed XML
                        }
                        catch (Exception ex)
                        {
                            PrintResults(expectedSafe, true, ex);   // safe: exception thrown when parsing XML
                        }
                        finally
                        {
                            reader.Close();
                        }

                        break;
                    }
                #endregion

                #region XmlNodeReader: Safe by Default Example
                case "xmlnodereadersafe":
                    {
                        bool expectedSafe = true;

                        XmlDocument doc = new XmlDocument();
                        doc.Load(appPath+ "resources/xxetestuser.xml"); // unsafe! (safe in .NET versions 4.5.2+)
                        XmlNodeReader reader = new XmlNodeReader(doc);  // safe even though the XmlDocument might not be!

                        try
                        {
                            // parsing the XML
                            StringBuilder sb = new StringBuilder();
                            while (reader.Read())
                            {
                                sb.Append(reader.Value);
                            }

                            // testing the result
                            if (sb.ToString().Contains("SUCCESSFUL"))
                                PrintResults(expectedSafe, false, sb.ToString());   // unsafe: successful XXE injection
                            else
                                PrintResults(expectedSafe, true, sb.ToString());    // safe: empty or unparsed XML
                        }
                        catch (Exception ex)
                        {
                            PrintResults(expectedSafe, true, ex);   // safe: exception thrown when parsing XML
                        }
                        finally
                        {
                            reader.Close();
                        }

                        break;
                    }
                #endregion

                #region XmlNodeReader: Safe when Wrapping in an Unsafe XmlReader Example
                case "xmlnodereadersafexmlreader":
                    {
                        bool expectedSafe = true;

                        XmlDocument doc = new XmlDocument();
                        doc.Load(appPath + "resources/xxetestuser.xml"); // unsafe! (safe in .NET versions 4.5.2+)
                        XmlNodeReader reader = new XmlNodeReader(doc);  // safe even though the XmlDocument might not be!

                        XmlReaderSettings settings = new XmlReaderSettings();
                        settings.DtdProcessing = DtdProcessing.Parse;   // unsafe!

                        // forcing unsafe in .NET versions 4.5.2+
                        if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
                        {
                            XmlUrlResolver res = new XmlUrlResolver();
                            //res.ResolveUri(new Uri(Environment.CurrentDirectory), "resources/xxetestuser.xml"); // works but not needed
                            settings.XmlResolver = res;
                        }

                        XmlReader xmlReader = XmlReader.Create(reader, settings);   // safe even though XmlReaderSettings unsafe!

                        try
                        {
                            // parsing the XML
                            StringBuilder sb = new StringBuilder();
                            while (xmlReader.Read())
                            {
                                sb.Append(xmlReader.Value);
                            }

                            // testing the result
                            if (sb.ToString().Contains("SUCCESSFUL"))
                                PrintResults(expectedSafe, false, sb.ToString());   // unsafe: successful XXE injection
                            else
                                PrintResults(expectedSafe, true, sb.ToString());    // safe: empty or unparsed XML
                        }
                        catch (Exception ex)
                        {
                            PrintResults(expectedSafe, true, ex);   // safe: exception thrown when parsing XML
                        }
                        finally
                        {
                            reader.Close();
                            xmlReader.Close();
                        }

                        break;
                    }
                #endregion

                #region XmlTextReader: Unsafe by Default Example
                case "xmltextreadersafe452":
                    {
                        bool expectedSafe = false;
                        if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
                            expectedSafe = true;

                        XmlTextReader reader = new XmlTextReader(appPath + "resources/xxetestuser.xml");    // unsafe! (safe in .NET version 4.5.2+)

                        try {
                            // parsing the XML
                            StringBuilder sb = new StringBuilder();
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Element)
                                {
                                    sb.Append(reader.ReadElementContentAsString());
                                }
                            }

                            // testing the result
                            if (sb.ToString().Contains("SUCCESSFUL"))
                                PrintResults(expectedSafe, false, sb.ToString());   // unsafe: successful XXE injection
                            else
                                PrintResults(expectedSafe, true, sb.ToString());    // safe: empty or unparsed XML
                            
                        }
                        catch (Exception ex)
                        {
                            PrintResults(expectedSafe, true, ex);   // safe: exception thrown when parsing XML
                        }
                        finally
                        {
                            reader.Close();
                        }

                        break;
                    }
                #endregion

                #region XmlTextReader: Safe when Prohibiting DTDs Example
                case "xmltextreadersafe":
                    {
                        bool expectedSafe = true;

                        XmlTextReader reader = new XmlTextReader(appPath + "resources/xxetestuser.xml");

                        try
                        {      
                            // parsing the XML
                            reader.DtdProcessing = DtdProcessing.Prohibit;  // safety measure
                            StringBuilder sb = new StringBuilder();
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Element)
                                {
                                    sb.Append(reader.ReadElementContentAsString());
                                }
                            }

                            // testing the result
                            if (sb.ToString().Contains("SUCCESSFUL"))
                                PrintResults(expectedSafe, false, sb.ToString());   // unsafe: successful XXE injection
                            else
                                PrintResults(expectedSafe, true, sb.ToString());    // safe: empty or unparsed XML    
                        }
                        catch (Exception ex)
                        {
                            PrintResults(expectedSafe, true, ex);   // safe: exception thrown when parsing XML
                        }
                        finally
                        {
                            reader.Close();
                        }

                        break;
                    }
                #endregion

                #region XmlTextReader: Unsafe when Resolving Entities Manually Example
                case "xmltextreaderunsafe":
                    {
                        bool expectedSafe = false;

                        XmlTextReader reader = new XmlTextReader(appPath + "resources/xxetestuser.xml");    // unsafe! (safe in .NET versions 4.5.2+)

                        // forcing unsafe in .NET versions 4.5.2+
                        XmlUrlResolver res = new XmlUrlResolver();
                        //res.ResolveUri(new Uri(Environment.CurrentDirectory), "resources/xxetestuser.xml"); // works but not needed
                        reader.XmlResolver = res;

                        try
                        {
                            // parsing the XML
                            StringBuilder sb = new StringBuilder();
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Element)
                                {
                                    sb.Append(reader.ReadElementContentAsString());
                                }
                            }

                            // testing the result
                            if (sb.ToString().Contains("SUCCESSFUL"))
                                PrintResults(expectedSafe, false, sb.ToString());   // unsafe: successful XXE injection
                            else
                                PrintResults(expectedSafe, true, sb.ToString());    // safe: empty or unparsed XML

                        }
                        catch (Exception ex)
                        {
                            PrintResults(expectedSafe, true, ex);   // safe: exception thrown when parsing XML
                        }
                        finally
                        {
                            reader.Close();
                        }

                        break;
                    }
                #endregion

                #region XslCompiledTransform: Safe by Default Example
                case "xslcompiledtransformsafe":
                    {
                        bool expectedSafe = true;
                        try
                        {
                            // parsing the XML
                            XslCompiledTransform transformer = new XslCompiledTransform();
                            transformer.Load(appPath + "resources/test.xsl");
                            StringWriter output = new StringWriter();
                            transformer.Transform(appPath + "resources/xxetestuser.xml", new XsltArgumentList(), output);

                            // testing the result
                            if (output.ToString().Contains("SUCCESSFUL"))
                                PrintResults(expectedSafe, false, output.ToString());   // unsafe: successful XXE injection
                            else
                                PrintResults(expectedSafe, true, output.ToString());    // safe: empty or unparsed XML
                        }
                        catch (Exception ex)
                        {
                            PrintResults(expectedSafe, true, ex);   // safe: exception thrown when parsing XML
                        }

                        break;
                    }
                #endregion

                #region XslCompiledTransform: Unsafe when Providing an Unsafe XML Parser Example
                case "xslcompiledtransformunsafe":
                    {
                        bool expectedSafe = false;

                        XmlTextReader reader = new XmlTextReader(appPath + "resources/xxetestuser.xml");    // unsafe! (safe in .NET version 4.5.2+)

                        // forcing unsafe in .NET versions 4.5.2+
                        XmlUrlResolver res = new XmlUrlResolver();
                        //res.ResolveUri(new Uri(Environment.CurrentDirectory), "resources/xxetestuser.xml"); // works but not needed
                        reader.XmlResolver = res;

                        try
                        {
                            // parsing the XML
                            XslCompiledTransform transformer = new XslCompiledTransform();
                            transformer.Load(appPath + "resources/test.xsl");
                            
                            StringWriter output = new StringWriter();
                            transformer.Transform(reader, new XsltArgumentList(), output);

                            // testing the result
                            if (output.ToString().Contains("SUCCESSFUL"))
                                PrintResults(expectedSafe, false, output.ToString());   // unsafe: successful XXE injection
                            else
                                PrintResults(expectedSafe, true, output.ToString());    // safe: empty or unparsed XML
                        }
                        catch (Exception ex)
                        {
                            PrintResults(expectedSafe, true, ex);   // safe: exception thrown when parsing XML
                        }
                        finally
                        {
                            reader.Close();
                        }

                        break;
                    }
                #endregion

                // Default case
                default:
                    Response.Write("Error: Test case not found");
                    break;
            }
        }

        /**
         *  Prints the results
         */
        private void PrintResults(bool expectedSafe, bool actuallySafe, string xmlContent)
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
         *  Prints the results if there is an exception
         */
        private void PrintResults(bool expectedSafe, bool actuallySafe, Exception ex)
        {
            PrintResults(expectedSafe, actuallySafe, "XML was not parsed due to a thrown exception");
            Response.Write("<b>" +"Stack Trace: " + "</b>" + "<br />" + ex.ToString());
        }

        /**
         * Starts the tests on page load
         */
        protected void Page_Load(object sender, EventArgs e) 
        {
            // gets the user's input
            string xmltext = Request.QueryString["payload"];

            // writes user's input to a file
            string appPath = Request.PhysicalApplicationPath;
            using (StreamWriter file = new StreamWriter(appPath + "/resources/xxetestuser.xml", false))
            {
                file.Write(xmltext);
                file.Close();
            }   
            
            PerformTest(xmltext);
        }
    }
}
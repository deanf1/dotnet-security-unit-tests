using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace DotNetUnitTests
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
                   
                #region LINQ: XElement: Safe by Default Example
                /**
                 * XElement is always safe due to the fact that it ignores anything in the XML that isn't an element.
                 */
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

                #region LINQ: XDocument: Safe by Default Example
                /**
                 * XDocument has DTDs disabled by default, making it not parse entities by default.
                 * (Source: https://github.com/dotnet/docs/blob/master/docs/visual-basic/programming-guide/concepts/linq/linq-to-xml-security.md)
                 */
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

                #region LINQ: XDocument: Unsafe when Providing an Unsafe XML Parser Example
                /**
                 * If you create your XDocument with an unsafe XML parser, it makes the XDocument unsafe as well.
                 */
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
                /**
                 * When using a default XmlDictionaryReader, upon attempting to read the XML file it will throw an exception when it sees the DTD.
                 */
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
                /**
                 * If you create your XmlDictionaryReader with an unsafe XML parser, it makes the XmlDictionaryReader unsafe as well.
                 */
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

                #region XmlDocument: Safe by Default in Current .NET Version (4.5.2 and above) Example / XmlDocument: Unsafe by Default in Current .NET Version (4.5.1 and lower) Example
                /**
                 * In .NET version 4.5.2, Microsoft made a change to parsers that implement an XmlResolver object that makes it null by default, making the parser ignore DTDs by default.
                 * However, in all previous versions, these parsers are unsafe by default.
                 */
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
                /**
                 * By setting the XmlDocument's XmlResolver to null, it makes the XmlDoucment not parse entities in all .NET versions.
                 */
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

                #region XmlDocument: Unsafe when Resolving Entities Manually Example
                /**
                 * By creating your own nonnull XmlResolver object (in this case, an XmlUrlResolver) and giving it to the XmlDocument, the XmlDocument will parse the entities.
                 */
                case "xmldocumentunsafe":
                    {
                        bool expectedSafe = false;

                        try
                        {
                            // parsing the XML  
                            XmlDocument doc = new XmlDocument();

                            // forcing unsafe in .NET versions 4.5.2+
                            if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
                            {
                                XmlUrlResolver res = new XmlUrlResolver();
                                //res.ResolveUri(new Uri(Environment.CurrentDirectory), "resources/xxetestuser.xml"); // works but not needed
                                doc.XmlResolver = res;
                            }
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

                #region XmlNodeReader: Safe by Default Example
                /**
                 * XmlNodeReader will ignore DTDs by default, even when created with an unsafe XmlDocument.
                 */
                case "xmlnodereadersafe":
                    {
                        bool expectedSafe = true;

                        XmlDocument doc = new XmlDocument();

                        // forcing unsafe in .NET versions 4.5.2+
                        if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
                        {
                            XmlUrlResolver res = new XmlUrlResolver();
                            //res.ResolveUri(new Uri(Environment.CurrentDirectory), "resources/xxetestuser.xml"); // works but not needed
                            doc.XmlResolver = res;
                        }
                        doc.Load(appPath + "resources/xxetestuser.xml"); // unsafe! (safe in .NET versions 4.5.2+)

                        XmlNodeReader reader = new XmlNodeReader(doc);  // safe even though the XmlDocument is not!

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
                /**
                 * XmlNodeReader will ignore DTDs even when created with an unsafe XmlDocument and wrapped in an unsafe XmlReader.
                 */
                case "xmlnodereadersafexmlreader":
                    {
                        bool expectedSafe = true;

                        XmlDocument doc = new XmlDocument();

                        // forcing unsafe in .NET versions 4.5.2+
                        if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
                        {
                            XmlUrlResolver res = new XmlUrlResolver();
                            //res.ResolveUri(new Uri(Environment.CurrentDirectory), "resources/xxetestuser.xml"); // works but not needed
                            doc.XmlResolver = res;
                        }
                        doc.Load(appPath + "resources/xxetestuser.xml"); // unsafe! (safe in .NET versions 4.5.2+)

                        XmlNodeReader reader = new XmlNodeReader(doc);  // safe even though the XmlDocument is not!

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

                #region XmlReader: Safe by Default Example
                /**
                 * XmlReader has DtdProcessing set to Prohibit by default, throwing an exception when it reads a DTD.
                 */
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

                #region XmlReader: Unsafe when Resolving Entities Manually Example
                /**
                 * By giving the XmlReader a XmlReaderSettings object that has DtdProcessing set to Parse, the XmlReader will parse entities.
                 * In .NET versions 4.5.2 and up, however, it is still safe because the XmlReaderSettings has a null XmlResolver object.
                 * By creating your own nonnull XmlResolver object (in this case, an XmlUrlResolver) and giving it to the XmlReaderSettings that the XmlReader receives,
                 * the XmlReader will parse the entities.
                 */
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

                #region XmlTextReader: Safe by Default in Current .NET Version (4.5.2 and above) Example / XmlTextReader: Unsafe by Default in Current .NET Version (4.5.1 and lower) Example
                /**
                 * In .NET version 4.5.2, Microsoft made a change to parsers that implement an XmlResolver object that makes it null by default, making the parser ignore DTDs by default.
                 * However, in all previous versions, these parsers are unsafe by default.
                 */
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
                /**
                 * By setting the XmlTextReader's DtdProcessing to Prohibit (not Prohibit by default like its parent XmlReader), it throws an excpetion when it reads the DTD in all .NET versions.
                 */
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
                /**
                 * By creating your own nonnull XmlResolver object (in this case, an XmlUrlResolver) and giving it to the XmlTextReader, the XmlTextReader will parse the entities.
                 */
                case "xmltextreaderunsafe":
                    {
                        bool expectedSafe = false;

                        XmlTextReader reader = new XmlTextReader(appPath + "resources/xxetestuser.xml");    // unsafe! (safe in .NET versions 4.5.2+)

                        // forcing unsafe in .NET versions 4.5.2+
                        if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
                        {
                            XmlUrlResolver res = new XmlUrlResolver();
                            //res.ResolveUri(new Uri(Environment.CurrentDirectory), "resources/xxetestuser.xml"); // works but not needed
                            reader.XmlResolver = res;
                        }

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

                #region XPathNavigator: Safe by Default in Current .NET Version (4.5.2 and above) Example / XPathNavigator: Unsafe by Default in Current .NET Version (4.5.1 and lower) Example
                /**
                 * XPathNavigator is safe in .NET versions 4.5.2 and up, and unsafe in versions 4.5.1 and under, because it implements IXPathNavigable objects (such as XmlDocument).
                 * This causes XPathNavigator to be just as safe as they are: parsing entities in any version before 4.5.2 only.
                 * These objects are private to the class however, so XPathNavigator can not be forced to be unsafe in .NET versions 4.5.2 and later.
                 */
                case "xpathnavigatorsafe452":
                    {
                        bool expectedSafe = false;
                        if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
                            expectedSafe = true;

                        XPathDocument doc = new XPathDocument(appPath + "resources/xxetestuser.xml");
                        XPathNavigator nav = doc.CreateNavigator(); // unsafe!

                        try
                        {
                            // parsing the XML
                            string xml = nav.InnerXml.ToString();

                            // testing the result
                            if (xml.Contains("SUCCESSFUL"))
                                PrintResults(expectedSafe, false, xml);   // unsafe: successful XXE injection
                            else
                                PrintResults(expectedSafe, true, xml);    // safe: empty or unparsed XML

                        }
                        catch (Exception ex)
                        {
                            PrintResults(expectedSafe, true, ex);   // safe: exception thrown when parsing XML
                        }

                        break;
                    }
                #endregion

                #region XPathNavigator: Safe when Providing a Safe XML Parser Example
                /**
                 * By creating a XPathDocument from a safe XML parser, it makes the XPathNavigator safe as well.
                 */
                case "xpathnavigatorsafe":
                    {
                        bool expectedSafe = true;

                        XmlReader reader = XmlReader.Create(appPath + "resources/xxetestuser.xml");   

                        try
                        {
                            // parsing the XML
                            XPathDocument doc = new XPathDocument(reader);
                            XPathNavigator nav = doc.CreateNavigator(); 
                            string xml = nav.InnerXml.ToString();

                            // testing the result
                            if (xml.Contains("SUCCESSFUL"))
                                PrintResults(expectedSafe, false, xml);   // unsafe: successful XXE injection
                            else
                                PrintResults(expectedSafe, true, xml);    // safe: empty or unparsed XML

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
                /**
                 * XslCompiledTransform is safe by default because it uses an XmlReader by default, which is safe by default.
                 * (Source: http://www.dotnetframework.org/default.aspx/4@0/4@0/DEVDIV_TFS/Dev10/Releases/RTMRel/ndp/fx/src/Xml/System/Xml/Xslt/XslCompiledTransform@cs/1305376/XslCompiledTransform@cs)
                 */
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
                /**
                 * If you transform with an specified XML parser that is unsafe as input, it makes the output from the XslCompiledTransform parse the entities.
                 */
                case "xslcompiledtransformunsafe":
                    {
                        bool expectedSafe = false;

                        XmlTextReader reader = new XmlTextReader(appPath + "resources/xxetestuser.xml");    // unsafe! (safe in .NET version 4.5.2+)

                        // forcing unsafe in .NET versions 4.5.2+
                        if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
                        {
                            XmlUrlResolver res = new XmlUrlResolver();
                            //res.ResolveUri(new Uri(Environment.CurrentDirectory), "resources/xxetestuser.xml"); // works but not needed
                            reader.XmlResolver = res;
                        }

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

                // default case
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
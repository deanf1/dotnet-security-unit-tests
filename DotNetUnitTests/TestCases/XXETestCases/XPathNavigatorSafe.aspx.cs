using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace DotNetUnitTests.TestCases.XXETestCases
{
    public partial class XPathNavigatorSafe : XXETestCasePage
    {
        /**
         * XPathNavigator: Safe when Providing a Safe XML Parser Example
         * By creating a XPathDocument from a safe XML parser, it makes the XPathNavigator safe as well.
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            bool expectedSafe = true;

            XmlReader reader = XmlReader.Create(new MemoryStream(Encoding.ASCII.GetBytes(xmlText)), new XmlReaderSettings(), appPath + "resources/");

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
        }
    }
}
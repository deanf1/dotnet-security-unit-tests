using System;
using System.Xml;

namespace DotNetUnitTests.TestCases.XXETestCases
{
    public partial class XmlDocumentSafe : XXETestCasePage
    {
        /**
         * XmlDocument: Safe when Setting the XmlResolver to null Example
         * By setting the XmlDocument's XmlResolver to null, it makes the XmlDoucment not parse entities in all .NET versions.
         */
        protected void Page_Load(object sender, EventArgs e)
        {         
            bool expectedSafe = true;

            try
            {
                // parsing the XML  
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null; // safety measure
                xmlText = FixXMLBaseURI(xmlText, appPath);  // makes sure that the external entity gets referenced at the correct base URI
                doc.LoadXml(xmlText);
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
        }
    }
}
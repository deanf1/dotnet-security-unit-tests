using System;
using System.Web;
using System.Xml;

namespace DotNetUnitTests.TestCases.XXETestCases
{
    public partial class XmlDocumentSafe452 : XXETestCasePage
    {
        /**
         * XmlDocument: Safe by Default in Current .NET Version (4.5.2 and above) Example / XmlDocument: Unsafe by Default in Current .NET Version (4.5.1 and lower) Example
         * In .NET version 4.5.2, Microsoft made a change to parsers that implement an XmlResolver object that makes it null by default, making the parser ignore DTDs by default.
         * However, in all previous versions, these parsers are unsafe by default.
         */
        protected void Page_Load(object sender, EventArgs e)
        {              
            bool expectedSafe = false;

            if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
                expectedSafe = true;

            try
            {
                // parsing the XML   
                XmlDocument doc = new XmlDocument();
                xmlText = FixXMLBaseURI(xmlText, appPath);  // makes sure that the external entity gets referenced at the correct base URI    
                doc.LoadXml(xmlText);    // unsafe! (safe in .NET versions 4.5.2+)
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
using System;
using System.Web;
using System.Xml;

namespace DotNetUnitTests.TestCases.XXETestCases
{
    public partial class XmlDocumentUnsafe : XXETestCasePage
    {
        /**
         * XmlDocument: Unsafe when Resolving Entities Manually Example
         * By creating your own nonnull XmlResolver object (in this case, an XmlUrlResolver) and giving it to the XmlDocument, the XmlDocument will parse the entities.
         */
        protected void Page_Load(object sender, EventArgs e)
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
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml.XPath;

namespace DotNetUnitTests.TestCases.XXETestCases
{
    public partial class XPathNavigatorSafe452 : XXETestCasePage
    {
        /**
         * XPathNavigator: Safe by Default in Current.NET Version (4.5.2 and above) Example / XPathNavigator: Unsafe by Default in Current.NET Version (4.5.1 and lower) Example
         * XPathNavigator is safe in .NET versions 4.5.2 and up, and unsafe in versions 4.5.1 and under, because it implements IXPathNavigable objects (such as XmlDocument).
         * This causes XPathNavigator to be just as safe as they are: parsing entities in any version before 4.5.2 only.
         * These objects are private to the class however, so XPathNavigator can not be forced to be unsafe in .NET versions 4.5.2 and later.
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            bool expectedSafe = false;

            if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
                expectedSafe = true;

            xmlText = FixXMLBaseURI(xmlText, appPath);  // makes sure that the external entity gets referenced at the correct base URI

            XPathDocument doc = new XPathDocument(new MemoryStream(Encoding.ASCII.GetBytes(xmlText)));
            //XPathDocument doc = new XPathDocument(appPath + "resources/xxetestuser.xml");
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
        }
    }
}
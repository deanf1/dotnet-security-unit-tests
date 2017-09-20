using System;
using System.Text;
using System.Web;
using System.Xml;

namespace DotNetUnitTests.TestCases.XXETestCases
{
    public partial class XmlNodeReaderSafeXmlReader : XXETestCasePage
    {
        /**
         * XmlNodeReader: Safe when Wrapping in an Unsafe XmlReader Example
         * XmlNodeReader will ignore DTDs even when created with an unsafe XmlDocument and wrapped in an unsafe XmlReader.
         */
        protected void Page_Load(object sender, EventArgs e)
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
            xmlText = FixXMLBaseURI(xmlText, appPath);  // makes sure that the external entity gets referenced at the correct base URI
            doc.LoadXml(xmlText); // unsafe! (safe in .NET versions 4.5.2+)

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
        }
    }
}
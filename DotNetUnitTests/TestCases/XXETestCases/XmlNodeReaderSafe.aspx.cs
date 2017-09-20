using System;
using System.Text;
using System.Web;
using System.Xml;

namespace DotNetUnitTests.TestCases.XXETestCases
{
    public partial class XmlNodeReaderSafe : XXETestCasePage
    {
        /**
         * XmlNodeReader: Safe by Default Example
         * XmlNodeReader will ignore DTDs by default, even when created with an unsafe XmlDocument.
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
        }
    }
}
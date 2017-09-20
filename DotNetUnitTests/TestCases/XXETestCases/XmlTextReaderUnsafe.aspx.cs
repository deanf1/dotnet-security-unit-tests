using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;

namespace DotNetUnitTests.TestCases.XXETestCases
{
    public partial class XmlTextReaderUnsafe : XXETestCasePage
    {
        /**
         * XmlTextReader: Unsafe when Resolving Entities Manually Example
         * By creating your own nonnull XmlResolver object (in this case, an XmlUrlResolver) and giving it to the XmlTextReader, the XmlTextReader will parse the entities.
         */
        protected void Page_Load(object sender, EventArgs e)
        {         
            bool expectedSafe = false;

            XmlTextReader reader = new XmlTextReader(appPath + "resources/", new MemoryStream(Encoding.ASCII.GetBytes(xmlText)));   // unsafe! (safe in .NET version 4.5.2+)
            //XmlTextReader reader = new XmlTextReader(appPath + "resources/xxetestuser.xml");    // unsafe! (safe in .NET versions 4.5.2+)

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
        }
    }
}
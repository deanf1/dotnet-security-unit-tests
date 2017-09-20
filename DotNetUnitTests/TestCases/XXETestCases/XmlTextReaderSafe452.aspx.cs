using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;

namespace DotNetUnitTests.TestCases.XXETestCases
{
    public partial class XmlTextReaderSafe452 : XXETestCasePage
    {
        /**
         * XmlTextReader: Safe by Default in Current .NET Version (4.5.2 and above) Example / XmlTextReader: Unsafe by Default in Current .NET Version (4.5.1 and lower) Example
         * In .NET version 4.5.2, Microsoft made a change to parsers that implement an XmlResolver object that makes it null by default, making the parser ignore DTDs by default.
         * However, in all previous versions, these parsers are unsafe by default.
         */
        protected void Page_Load(object sender, EventArgs e)
        {         
            bool expectedSafe = false;
            if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
                expectedSafe = true;

            XmlTextReader reader = new XmlTextReader(appPath + "resources/", new MemoryStream(Encoding.ASCII.GetBytes(xmlText)));   // unsafe! (safe in .NET version 4.5.2+)
            //XmlTextReader reader = new XmlTextReader(appPath + "resources/xxetestuser.xml");    // unsafe! (safe in .NET version 4.5.2+)

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
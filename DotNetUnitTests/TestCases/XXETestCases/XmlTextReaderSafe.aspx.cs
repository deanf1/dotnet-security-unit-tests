using System;
using System.IO;
using System.Text;
using System.Xml;

namespace DotNetUnitTests.TestCases.XXETestCases
{
    public partial class XmlTextReaderSafe : XXETestCasePage
    {
        /**
         * XmlTextReader: Safe when Prohibiting DTDs Example
         * By setting the XmlTextReader's DtdProcessing to Prohibit (not Prohibit by default like its parent XmlReader), it throws an excpetion when it reads the DTD in all .NET versions.
         */
        protected void Page_Load(object sender, EventArgs e)
        {         
            bool expectedSafe = true;

            XmlTextReader reader = new XmlTextReader(appPath + "resources/", new MemoryStream(Encoding.ASCII.GetBytes(xmlText)));   // unsafe! (safe in .NET version 4.5.2+)
            //XmlTextReader reader = new XmlTextReader(appPath + "resources/xxetestuser.xml");

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
        }
    }
}
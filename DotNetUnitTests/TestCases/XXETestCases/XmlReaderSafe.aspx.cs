using System;
using System.IO;
using System.Text;
using System.Xml;

namespace DotNetUnitTests.TestCases.XXETestCases
{
    public partial class XmlReaderSafe : XXETestCasePage
    {
        /**
         * XmlReader: Safe by Default Example
         * XmlReader has DtdProcessing set to Prohibit by default, throwing an exception when it reads a DTD.
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            bool expectedSafe = true;

            XmlReader reader = XmlReader.Create(new MemoryStream(Encoding.ASCII.GetBytes(xmlText)), new XmlReaderSettings(), appPath + "resources/");
            //XmlReader reader = XmlReader.Create(appPath + "resources/xxetestuser.xml");

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
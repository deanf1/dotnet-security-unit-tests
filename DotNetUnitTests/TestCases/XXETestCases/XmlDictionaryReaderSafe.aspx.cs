using System;
using System.Text;
using System.Xml;

namespace DotNetUnitTests.TestCases.XXETestCases
{
    public partial class XmlDictionaryReaderSafe : XXETestCasePage
    {
        /**
         * XmlDictionaryReader: Safe by Default Example
         * When using a default XmlDictionaryReader, upon attempting to read the XML file it will throw an exception when it sees the DTD.
         */
        protected void Page_Load(object sender, EventArgs e)
        {         
            bool expectedSafe = true;

            xmlText = FixXMLBaseURI(xmlText, appPath);  // makes sure that the external entity gets referenced at the correct base URI
            XmlDictionaryReader dict = XmlDictionaryReader.CreateTextReader(Encoding.ASCII.GetBytes(xmlText), XmlDictionaryReaderQuotas.Max);

            try
            {
                // parsing the XML  
                StringBuilder sb = new StringBuilder();
                while (dict.Read())
                {
                    sb.Append(dict.Value);
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
                dict.Close();
            }
        }
    }
}
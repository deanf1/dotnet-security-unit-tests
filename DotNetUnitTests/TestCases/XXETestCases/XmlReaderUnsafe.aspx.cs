using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;

namespace DotNetUnitTests.TestCases.XXETestCases
{
    public partial class XmlReaderUnsafe : XXETestCasePage
    {
        /**
         * XmlReader: Unsafe when Resolving Entities Manually Example
         * By giving the XmlReader a XmlReaderSettings object that has DtdProcessing set to Parse, the XmlReader will parse entities.
         * In .NET versions 4.5.2 and up, however, it is still safe because the XmlReaderSettings has a null XmlResolver object.
         * By creating your own nonnull XmlResolver object (in this case, an XmlUrlResolver) and giving it to the XmlReaderSettings that the XmlReader receives,
         * the XmlReader will parse the entities.
         */
        protected void Page_Load(object sender, EventArgs e)
        {         
            bool expectedSafe = false;

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;   // unsafe!

            // forcing unsafe in .NET versions 4.5.2+
            if (HttpRuntime.TargetFramework.Minor >= 6 || HttpRuntime.TargetFramework.ToString().Equals("4.5.2"))
            {
                XmlUrlResolver res = new XmlUrlResolver();
                //res.ResolveUri(new Uri(Environment.CurrentDirectory), "resources/xxetestuser.xml"); // works but not needed
                settings.XmlResolver = res;
            }
            XmlReader reader = XmlReader.Create(new MemoryStream(Encoding.ASCII.GetBytes(xmlText)), settings, appPath + "resources/");
            //XmlReader reader = XmlReader.Create(appPath + "resources/xxetestuser.xml", settings);

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
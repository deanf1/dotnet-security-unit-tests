using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace DotNetUnitTests.TestCases.XXETestCases
{
    public partial class LINQXDocumentUnsafe : XXETestCasePage
    {
        /**
         * LINQ: XDocument: Unsafe when Providing an Unsafe XML Parser Example
         * If you create your XDocument with an unsafe XML parser, it makes the XDocument unsafe as well.
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
            XDocument xdocument = XDocument.Load(reader);    // unsafe!

            try
            {
                // parsing the XML                          
                StringBuilder sb = new StringBuilder();
                foreach (var element in xdocument.Elements())
                {
                    sb.Append(element.ToString());
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
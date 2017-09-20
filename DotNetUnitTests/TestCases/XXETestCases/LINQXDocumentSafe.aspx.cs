using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace DotNetUnitTests.TestCases.XXETestCases
{
    public partial class LINQXDocumentSafe : XXETestCasePage
    {
        /**
         * LINQ: XDocument: Safe by Default Example
         * XDocument has DTDs disabled by default, making it not parse entities by default.
         * (Source: https://github.com/dotnet/docs/blob/master/docs/visual-basic/programming-guide/concepts/linq/linq-to-xml-security.md)
         */
        protected void Page_Load(object sender, EventArgs e)
        {         
            bool expectedSafe = true;

            XDocument xdocument = XDocument.Load(new MemoryStream(Encoding.ASCII.GetBytes(xmlText)));
            //XDocument xdocument = XDocument.Load(appPath + "resources/xxetestuser.xml");

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
        }
    }
}
using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace DotNetUnitTests.TestCases.XXETestCases
{
    public partial class LINQXElementSafe : XXETestCasePage
    {
        /**
         * LINQ: XElement: Safe by Default Example
         * XElement is always safe due to the fact that it ignores anything in the XML that isn't an element.
         */
        protected void Page_Load(object sender, EventArgs e)
        {         
            bool expectedSafe = true;

            XElement xelement = XElement.Load(new MemoryStream(Encoding.ASCII.GetBytes(xmlText)));
            //XElement xelement = XElement.Load(appPath + "resources/xxetestuser.xml");

            try
            {
                // parsing the XML                          
                StringBuilder sb = new StringBuilder();
                foreach (var element in xelement.Elements())
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
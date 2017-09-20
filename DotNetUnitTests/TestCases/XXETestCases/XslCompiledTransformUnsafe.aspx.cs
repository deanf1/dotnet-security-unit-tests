using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Xsl;

namespace DotNetUnitTests.TestCases.XXETestCases
{
    public partial class XslCompiledTransformUnsafe : XXETestCasePage
    {
        /**
         * XslCompiledTransform: Unsafe when Providing an Unsafe XML Parser Example
         * If you transform with an specified XML parser that is unsafe as input, it makes the output from the XslCompiledTransform parse the entities.
         */
        protected void Page_Load(object sender, EventArgs e)
        {         
            bool expectedSafe = false;

            XmlTextReader reader = new XmlTextReader(appPath + "resources/", new MemoryStream(Encoding.ASCII.GetBytes(xmlText)));   // unsafe! (safe in .NET version 4.5.2+)

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
                XslCompiledTransform transformer = new XslCompiledTransform();
                transformer.Load(appPath + "resources/test.xsl");

                StringWriter output = new StringWriter();
                transformer.Transform(reader, new XsltArgumentList(), output);

                // testing the result
                if (output.ToString().Contains("SUCCESSFUL"))
                    PrintResults(expectedSafe, false, output.ToString());   // unsafe: successful XXE injection
                else
                    PrintResults(expectedSafe, true, output.ToString());    // safe: empty or unparsed XML
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
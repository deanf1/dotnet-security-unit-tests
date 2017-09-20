using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace DotNetUnitTests.TestCases.XXETestCases
{
    public partial class XslCompiledTransformSafe : XXETestCasePage
    {
        /**
         * XslCompiledTransform: Safe by Default Example
         * XslCompiledTransform is safe by default because it uses an XmlReader by default, which is safe by default.
         * (Source: http://www.dotnetframework.org/default.aspx/4@0/4@0/DEVDIV_TFS/Dev10/Releases/RTMRel/ndp/fx/src/Xml/System/Xml/Xslt/XslCompiledTransform@cs/1305376/XslCompiledTransform@cs)
         */
        protected void Page_Load(object sender, EventArgs e)
        {         
            bool expectedSafe = true;

            try
            {
                // parsing the XML
                XslCompiledTransform transformer = new XslCompiledTransform();
                transformer.Load(appPath + "resources/test.xsl");
                StringWriter output = new StringWriter();
                transformer.Transform(XmlReader.Create(new MemoryStream(Encoding.ASCII.GetBytes(xmlText)), new XmlReaderSettings(), appPath + "resources/"), new XsltArgumentList(), output);
                //transformer.Transform(appPath + "resources/xxetestuser.xml", new XsltArgumentList(), output);

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
        }
    }
}
using System;

namespace DotNetUnitTests
{
    public partial class hqlview : System.Web.UI.Page
    {
        protected string payloadValue;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["var"].Contains("delete"))
                this.payloadValue = "Test' OR FirstName='Target";   // DELETE payload
            else
                this.payloadValue = "Bobby' OR 'a'='a"; // SELECT payload
        }
    }
}
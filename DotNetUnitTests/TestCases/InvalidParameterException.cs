using System;

namespace DotNetUnitTests.TestCases
{
    /**
     * Exception for whitelisting XPath and XQuery query parameters
     */
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException(string message) : base(message) { }
    }
}
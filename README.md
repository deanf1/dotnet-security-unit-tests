# .NET Security Unit Tests
A web application that contains several unit tests for the purpose of .NET security

(Based on the web application I added to [a Java console application with similar tests](https://github.com/aspectsecurity/security-unit-tests) by [Dave Wichers](https://github.com/davewichers))

## Overview
The purpose of this web app is to test the following vulnerabilities in .NET (click to view their respective code):
- [XML External Enitity (XXE) Injection](./DotNetUnitTests/TestCases/XXETestCases)
   - A summary of these tests can be found in the [OWASP XXE Prevention Cheat Sheet](https://www.owasp.org/index.php/XML_External_Entity_(XXE)_Prevention_Cheat_Sheet#.NET)
- [NHibernate Query Language (HQL) Injection](./DotNetUnitTests/TestCases/HQLTestCases)
- [XPath Query Language Injection](./DotNetUnitTests/TestCases/XPathTestCases)
- [XQuery Query Language Injection](./DotNetUnitTests/TestCases/XQueryTestCases)

The code can be analyzed by static code tools or deployed as a web application and analyzed via dynamic tools. The underlying C# code can also be used as examples for how to make .NET code safe/unsafe. You can also find detailed comments in the code with each test explaining why it is safe/unsafe.

## Installation
Please see the [INSTRUCTIONS.txt](./INSTRUCTIONS.txt) file for information on deploying the web app, as well as instructions for running all the tests programmatically.

(Note: you will need Visual Studio installed)

## License
```
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
```

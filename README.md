# .NET Security Unit Tests
A web application that contains several unit tests for the purpose of .NET security

(Based on the web application I added to [a Java console application with similar tests](https://github.com/aspectsecurity/security-unit-tests) by [Dave Wichers](https://github.com/davewichers))
## Overview
The purpose of this web app is to test the following vulnerabilities in .NET:
- XML External Enitity (XXE) Injection
- NHibernate Query Language (HQL) Injection (coming soon)

The code can be analyzed by static code tools or deployed as a web application and analyzed via dynamic tools. The underlying code can also be used as examples for how to make .NET code safe/unsafe.

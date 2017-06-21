using System;
using NHibernate;
using NHibernate.Cfg;
using System.Collections.Generic;

namespace DotNetUnitTests
{
    public partial class hqlresults : System.Web.UI.Page
    {
        /**
         *  Detects which test case we're running, runs it, and prints the results
         */
        private void PerformTest(string hqltext)
        {
            switch (Request.QueryString["var"])
            {
                #region NHibernate: Safe when Using Built-in Functions Example
                /*case "safedefault":
                    {
                        bool expectedSafe = true;

                        break;
                    }*/
                #endregion

                /**
                 * By doing string concatenation in the CreateQuery method, the HQL query is just as unsafe as any unsafe SQL query
                 */
                #region NHibernate: Unsafe when Using String Concatenation on Custom HQL Queries Example
                case "unsafe":
                    {
                        bool expectedSafe = false;

                        // Creating the database session
                        ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
                        ISession session = sessionFactory.OpenSession();

                        // Creating and receiving the results the custom HQL query
                        IQuery query = session.CreateQuery("FROM DotNetUnitTests.Student WHERE FirstName = '" + hqltext + "';");    // unsafe!
                        IList<object> students = query.List<object>();

                        // testing the result
                        TestResults(students, hqltext, expectedSafe);

                        break;
                    }
                #endregion

                /**
                 * By parameterizing the user input, we can succesfully block any HQL injection attempts
                 */
                #region NHibernate: Safe when Parameterizing Custom HQL Queries Example
                case "safeparam":
                    {
                        bool expectedSafe = true;

                        // Creating the database session
                        ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
                        ISession session = sessionFactory.OpenSession();

                        // Creating and receiving the results the custom HQL query
                        IQuery query = session.CreateQuery("FROM DotNetUnitTests.Student WHERE FirstName = :name");
                        query.SetParameter("name", hqltext);    // safe!
                        IList<object> students = query.List<object>();

                        // testing the result
                        TestResults(students, hqltext, expectedSafe);

                        break;
                    }
                #endregion

                // default case
                default:
                    Response.Write("Error: Test case not found");
                    break;
            }
        }

        /**
         *  Tests the result and changes the print type accordingly
         */
        private void TestResults(IList<object> students, string hqltext, bool expectedSafe)
        {
            // using the default injection
            if (hqltext.Equals("Bobby' OR 'a'='a"))
            {
                if (expectedSafe)
                    PrintResults(expectedSafe, true, false, students);
                else
                    PrintResults(expectedSafe, false, false, students);
            }

            // using a custom injection that uses a semicolon or apostrophe
            else if (hqltext.Contains(";") || hqltext.Contains("'"))
                PrintResults(expectedSafe, false, true, students);

            else
            {
                // using a custom injection that returns extra rows or deletes entries
                if (students.Count > 1 || students.Count == 0)
                    PrintResults(expectedSafe, false, true, students);

                // using a query thats safe
                else
                    PrintResults(expectedSafe, true, true, students);
            }
        }

        /**
         *  Prints the results
         */
        private void PrintResults(bool expectedSafe, bool actuallySafe, bool custom, IList<object> students)
        {
            Response.Write("<h3>");
            Response.Write("Expected result: " + (expectedSafe ? "Safe" : "Unsafe") + "<br />");
            Response.Write("Actual result: " + (actuallySafe ? "NHibernate is safe! 😊" : "Unsafe query was injected! 😭") + "<br />");
            Response.Write("</h3>");
            if (!custom)
            {
                if (actuallySafe)
                    Response.Write("<b>" + "Query Result (should contain the Student where the first name is <mark>Bobby' OR 'a'='a</mark>):" + "</b>" + "<br />");
                else
                    Response.Write("<b>" + "Query Result (should return all Student entries instead of just Bobby):" + "</b>" + "<br />");
            }
            else
                Response.Write("<b>" + "Result of your custom query:" + "</b>" + "<br />");

            // print table view
            Response.Write("<table>");
            Response.Write("<tr> <th>ID</th> <th>Last Name</th> <th>First Name</th> <th>Username</th> <th>Password</th> </tr>");
            foreach (Student student in students)
            {
                Response.Write("<tr>");
                Response.Write("<td>" + student.ID + "</td>");
                Response.Write("<td>" + student.LastName + "</td>");
                Response.Write("<td>" + student.FirstName + "</td>");
                Response.Write("<td>" + student.Username + "</td>");
                Response.Write("<td>" + student.Password + "</td>");
                Response.Write("</tr>");
            }
            Response.Write("</table>");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string hqltext = Request.QueryString["payload"];
            PerformTest(hqltext);
        }
    }
}
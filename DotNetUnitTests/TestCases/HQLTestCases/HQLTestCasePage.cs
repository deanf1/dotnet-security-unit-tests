using System.Collections.Generic;
using System.Web;

namespace DotNetUnitTests.TestCases.HQLTestCases
{
    public class HQLTestCasePage : System.Web.UI.Page
    {
        private string _hqlText = HttpContext.Current.Request.QueryString["payload"];

        /**
         * The vulnerable HQL query
         */
        protected string hqlText { get { return _hqlText; } set { _hqlText = value; } }

        /**
         *  Tests the result of the query and changes the print type accordingly
         */
        protected void TestResults(IList<Student> students, string hqlText, bool expectedSafe)
        {
            // using the default injection
            if (hqlText.Equals("Bobby' OR 'a'='a") || hqlText.Equals("Test' OR FirstName='Target"))
            {
                if (expectedSafe)
                    PrintResults(expectedSafe, true, false, students);
                else
                    PrintResults(expectedSafe, false, false, students);
            }

            // using a custom injection that uses a semicolon or apostrophe
            else if (hqlText.Contains(";") || hqlText.Contains("'"))
                PrintResults(expectedSafe, false, true, students);

            else
            {
                // using a custom injection in SELECT that returns extra rows or deletes entries
                if ((students.Count > 1 || students.Count == 0) && !Request.QueryString["var"].Contains("Delete"))
                    PrintResults(expectedSafe, false, true, students);

                // using a query thats safe
                else
                    PrintResults(expectedSafe, true, true, students);
            }
        }

        /**
         *  Prints the results
         */
        protected void PrintResults(bool expectedSafe, bool actuallySafe, bool custom, IList<Student> students)
        {
            Response.Write("<h3>");
            Response.Write("Expected result: " + (expectedSafe ? "Safe" : "Unsafe") + "<br />");
            Response.Write("Actual result: " + (actuallySafe ? "NHibernate is safe! 😊" : "Unsafe query was injected! 😭") + "<br />");
            Response.Write("</h3>");
            if (!custom)
            {
                if (Request.QueryString["var"].Contains("Delete"))
                {
                    if (actuallySafe)
                        Response.Write("<b>" + "Query Result (should contain both Test User and Target User):" + "</b>" + "<br />");
                    else
                        Response.Write("<b>" + "Query Result (should be an empty table):" + "</b>" + "<br />");
                }
                else
                {
                    if (actuallySafe)
                        Response.Write("<b>" + "Query Result (should contain the Student where the first name is <mark>Bobby' OR 'a'='a</mark>):" + "</b>" + "<br />");
                    else
                        Response.Write("<b>" + "Query Result (should return all Student entries instead of just Bobby):" + "</b>" + "<br />");
                }
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
    }
}
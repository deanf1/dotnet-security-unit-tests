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
        private void PerformTest(string hqlText)
        {

            switch (Request.QueryString["var"])
            {
                #region SELECT: Safe when Using Built-in Functions Example
                /**
                 * By using NHibernate's built-in functions that aim to make executing querys more object-oriented, the input query is inherently parameterized.
                 */
                case "safedefault":
                    {
                        bool expectedSafe = true;

                        // creating the database session
                        ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
                        ISession session = sessionFactory.OpenSession();

                        // creating and receiving the results of the HQL query
                        ICriteria criteria = session.CreateCriteria<Student>();
                        criteria.Add(NHibernate.Criterion.Expression.Eq("FirstName", hqlText));   // safe!
                        IList<Student> students = criteria.List<Student>();

                        // testing the result
                        TestResults(students, hqlText, expectedSafe);

                        session.Close();
                        sessionFactory.Close();

                        break;
                    }
                #endregion

                #region SELECT: Unsafe when Using String Concatenation on Custom HQL Queries (CreateQuery) Example
                /**
                 * By doing string concatenation in the CreateQuery method, the HQL query is just as vulnerable to injection as any unsafe SQL query.
                 */
                case "unsafe":
                    {
                        bool expectedSafe = false;

                        // creating the database session
                        ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
                        ISession session = sessionFactory.OpenSession();

                        // creating and receiving the results of the custom HQL query
                        IQuery query = session.CreateQuery("FROM Student WHERE FirstName = '" + hqlText + "';");    // unsafe!
                        IList<Student> students = query.List<Student>();

                        // testing the result
                        TestResults(students, hqlText, expectedSafe);

                        session.Close();
                        sessionFactory.Close();

                        break;
                    }
                #endregion

                #region SELECT: Unsafe when Using String Concatenation on Custom SQL Queries (CreateSQLQuery) Example
                /**
                 * By doing string concatenation in the CreateSQLQuery method, the SQL query is vulnerable to injection.
                 */
                case "unsafesql":
                    {
                        bool expectedSafe = false;

                        // creating the database session
                        ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
                        ISession session = sessionFactory.OpenSession();

                        // creating and receiving the results of the custom SQL query
                        ISQLQuery query = session.CreateSQLQuery("SELECT * FROM Student WHERE FirstName = '" + hqlText + "';");    // unsafe!
                        query.AddEntity(typeof(Student));

                        IList<Student> students = query.List<Student>();

                        // testing the result
                        TestResults(students, hqlText, expectedSafe);

                        session.Close();
                        sessionFactory.Close();

                        break;
                    }
                #endregion

                #region SELECT: Unsafe when Using String Placeholders on Custom HQL Queries (CreateQuery) Example
                /**
                 * By using string placeholders in the CreateQuery method, the HQL query is just as vulnerable to injection as any unsafe SQL query.
                 */
                case "unsafehqlstringplace":
                    {
                        bool expectedSafe = false;

                        // creating the database session
                        ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
                        ISession session = sessionFactory.OpenSession();

                        // creating and receiving the results of the custom HQL query
                        IQuery query = session.CreateQuery(String.Format("FROM Student WHERE FirstName = '{0}';", hqlText));    // unsafe!
                        IList<Student> students = query.List<Student>();

                        // testing the result
                        TestResults(students, hqlText, expectedSafe);

                        session.Close();
                        sessionFactory.Close();

                        break;
                    }
                #endregion

                #region SELECT: Unsafe when Using String Placeholders on Custom SQL Queries (CreateSQLQuery) Example
                /**
                 * By using string placeholders in the CreateSQLQuery method, the SQL query is vulnerable to injection.
                 */
                case "unsafesqlstringplace":
                    {
                        bool expectedSafe = false;

                        // creating the database session
                        ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
                        ISession session = sessionFactory.OpenSession();

                        // creating and receiving the results of the custom SQL query
                        ISQLQuery query = session.CreateSQLQuery(String.Format("SELECT * FROM Student WHERE FirstName = '{0}';", hqlText));    // unsafe!
                        query.AddEntity(typeof(Student));

                        IList<Student> students = query.List<Student>();

                        // testing the result
                        TestResults(students, hqlText, expectedSafe);

                        session.Close();
                        sessionFactory.Close();

                        break;
                    }
                #endregion

                #region SELECT: Safe when Parameterizing Custom HQL Queries (CreateQuery) Example
                /**
                 * By parameterizing the user input, we can succesfully block any HQL injection attempts.
                 */
                case "safeparam":
                    {
                        bool expectedSafe = true;

                        // creating the database session
                        ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
                        ISession session = sessionFactory.OpenSession();

                        // creating and receiving the results of the custom HQL query
                        IQuery query = session.CreateQuery("FROM Student WHERE FirstName = :name");
                        query.SetParameter("name", hqlText);    // safe!
                        IList<Student> students = query.List<Student>();

                        // testing the result
                        TestResults(students, hqlText, expectedSafe);

                        session.Close();
                        sessionFactory.Close();

                        break;
                    }
                #endregion

                #region SELECT: Safe when Parameterizing Custom SQL Queries (CreateSQLQuery) Example
                /**
                 * By parameterizing the user input, we can succesfully block any SQL injection attempts.
                 */
                case "safeparamsql":
                    {
                        bool expectedSafe = true;

                        // creating the database session
                        ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
                        ISession session = sessionFactory.OpenSession();

                        // creating and receiving the results of the custom SQL query
                        ISQLQuery query = session.CreateSQLQuery("SELECT * FROM Student WHERE FirstName = :name");
                        query.AddEntity(typeof(Student));
                        query.SetParameter("name", hqlText);    // safe!

                        IList<Student> students = query.List<Student>();

                        // testing the result
                        TestResults(students, hqlText, expectedSafe);

                        session.Close();
                        sessionFactory.Close();

                        break;
                    }
                #endregion

                #region DELETE: Safe when Using Built-in Functions Example [TEST REMOVED]
                /**
                 * By using NHibernate's built-in functions that aim to make executing querys more object-oriented, the input query is inherently parameterized.
                 * NOTE: TEST REMOVED: Delete(object obj) only works when first fetching existing objects from the table.
                 *                     I chose not to do this as the saftey would fall back to that SELECT query, making this test irrelevant for DELETE.
                 *                     You can not delete a copy of the object, it has to be the same one.
                 *                     Therefore, this test doesn't actually work.
                 */
                case "deletesafedefault":
                    {
                        /**
                        //bool expectedSafe = true;

                        // creating the database session
                        ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
                        ISession session = sessionFactory.OpenSession();

                        // inserting the User students that will (hopefully) be deleted
                        Student test = new Student("User", "Test", "test", "deleteme");
                        Student target = new Student("User", "Target", "target", "deleteme2");
                        session.Save(test);
                        session.Save(target);

                        // delete the inputted user (doesn't work)
                        Student input = new Student();
                        input.FirstName = hqlText;
                        session.Delete(input);
                        session.Flush();

                        // testing the result
                        //TestResults(students, hqlText, expectedSafe);

                        session.Close();
                        sessionFactory.Close();
                        */

                        break;
                    }
                #endregion

                #region DELETE: Unsafe when Using String Concatenation on Custom HQL Queries Example
                /**
                 * By doing string concatenation in the Delete method, the query is vulnerable to injection.
                 */
                case "deleteunsafe":
                    {
                        bool expectedSafe = false;

                        // creating the database session
                        ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
                        ISession session = sessionFactory.OpenSession();

                        // inserting the User students that will (hopefully) be deleted
                        Student test = new Student("User", "Test", "test", "deleteme");
                        Student target = new Student("User", "Target", "target", "deleteme2");
                        session.Save(test);
                        session.Save(target);

                        // delete the inputted user
                        session.Delete("FROM Student WHERE FirstName = '" + hqlText + "';");
                        session.Flush();

                        // getting the User students to see what the results of the DELETE were
                        IQuery query = session.CreateQuery("FROM Student WHERE FirstName = 'Test' OR FirstName = 'Target';");
                        IList<Student> students = query.List<Student>();

                        // testing the result
                        TestResults(students, hqlText, expectedSafe);

                        session.Close();
                        sessionFactory.Close();

                        break;
                    }
                #endregion

                #region DELETE: Unsafe when Using String Placeholders on Custom HQL Queries Example
                /**
                 * By using string placeholders in the Delete method, the query is vulnerable to injection.
                 */
                case "deleteunsafestringplace":
                    {
                        bool expectedSafe = false;

                        // creating the database session
                        ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
                        ISession session = sessionFactory.OpenSession();

                        // inserting the User students that will (hopefully) be deleted
                        Student test = new Student("User", "Test", "test", "deleteme");
                        Student target = new Student("User", "Target", "target", "deleteme2");
                        session.Save(test);
                        session.Save(target);

                        // delete the inputted user
                        session.Delete(String.Format("FROM Student WHERE FirstName = '{0}';", hqlText));
                        session.Flush();

                        // getting the User students to see what the results of the DELETE were
                        IQuery query = session.CreateQuery("FROM Student WHERE FirstName = 'Test' OR FirstName = 'Target';");
                        IList<Student> students = query.List<Student>();

                        // testing the result
                        TestResults(students, hqlText, expectedSafe);

                        session.Close();
                        sessionFactory.Close();

                        break;
                    }
                #endregion

                #region DELETE: Safe when Parameterizing Custom HQL Queries Example
                /**
                 * By parameterizing the user input, we can succesfully block any HQL injection attempts. The only way to properly do this is write a delete query in the CreateQuery
                 * method and add the parameters there. 
                 */
                case "deletesafeparam":
                    {
                        bool expectedSafe = true;

                        // creating the database session
                        ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
                        ISession session = sessionFactory.OpenSession();

                        // inserting the User students that will (hopefully) be deleted
                        Student test = new Student("User", "Test", "test", "deleteme");
                        Student target = new Student("User", "Target", "target", "deleteme2");
                        session.Save(test);
                        session.Save(target);

                        // delete the inputted user
                        IQuery query = session.CreateQuery("DELETE FROM Student WHERE FirstName = :name");
                        query.SetParameter("name", hqlText);    // safe!
                        query.ExecuteUpdate();

                        // getting the User students to see what the results of the DELETE were
                        IQuery postQuery = session.CreateQuery("FROM Student WHERE FirstName = 'Test' OR FirstName = 'Target';");
                        IList<Student> students = postQuery.List<Student>();

                        // testing the result
                        TestResults(students, hqlText, expectedSafe);

                        session.Close();
                        sessionFactory.Close();

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
         *  Tests the result of the query and changes the print type accordingly
         */
        private void TestResults(IList<Student> students, string hqlText, bool expectedSafe)
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
                if ((students.Count > 1 || students.Count == 0) && !Request.QueryString["var"].Contains("delete"))
                    PrintResults(expectedSafe, false, true, students);

                // using a query thats safe
                else
                    PrintResults(expectedSafe, true, true, students);
            }
        }

        /**
         *  Prints the results
         */
        private void PrintResults(bool expectedSafe, bool actuallySafe, bool custom, IList<Student> students)
        {
            Response.Write("<h3>");
            Response.Write("Expected result: " + (expectedSafe ? "Safe" : "Unsafe") + "<br />");
            Response.Write("Actual result: " + (actuallySafe ? "NHibernate is safe! 😊" : "Unsafe query was injected! 😭") + "<br />");
            Response.Write("</h3>");
            if (!custom)
            {
                if (Request.QueryString["var"].Contains("delete"))
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

        protected void Page_Load(object sender, EventArgs e)
        {
            string hqlText = Request.QueryString["payload"];

            PerformTest(hqlText);
        }
    }
}
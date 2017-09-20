using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;

namespace DotNetUnitTests.TestCases.HQLTestCases
{
    public partial class UnsafeSQLStringPlace : HQLTestCasePage
    {
        /**
         * SELECT: Unsafe when Using String Placeholders on Custom SQL Queries (CreateSQLQuery) Example
         * By using string placeholders in the CreateSQLQuery method, the SQL query is vulnerable to injection.
         */
        protected void Page_Load(object sender, EventArgs e)
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
        }
    }
}
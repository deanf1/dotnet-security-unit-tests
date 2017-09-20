using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;

namespace DotNetUnitTests.TestCases.HQLTestCases
{
    public partial class UnsafeSQL : HQLTestCasePage
    {
        /**
         * SELECT: Unsafe when Using String Concatenation on Custom SQL Queries (CreateSQLQuery) Example
         * By doing string concatenation in the CreateSQLQuery method, the SQL query is vulnerable to injection.
         */
        protected void Page_Load(object sender, EventArgs e)
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
        }
    }
}
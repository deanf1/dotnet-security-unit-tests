using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;

namespace DotNetUnitTests.TestCases.HQLTestCases
{
    public partial class Unsafe : HQLTestCasePage
    {
        /**
         * SELECT: Unsafe when Using String Concatenation on Custom HQL Queries (CreateQuery) Example
         * By doing string concatenation in the CreateQuery method, the HQL query is just as vulnerable to injection as any unsafe SQL query.
         */
        protected void Page_Load(object sender, EventArgs e)
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
        }
    }
}
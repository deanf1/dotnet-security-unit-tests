using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;

namespace DotNetUnitTests.TestCases.HQLTestCases
{
    public partial class UnsafeHQLStringPlace : HQLTestCasePage
    {
        /**
         * SELECT: Unsafe when Using String Placeholders on Custom HQL Queries (CreateQuery) Example
         * By using string placeholders in the CreateQuery method, the HQL query is just as vulnerable to injection as any unsafe SQL query.
         */
        protected void Page_Load(object sender, EventArgs e)
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
        }
    }
}
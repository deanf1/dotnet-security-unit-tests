using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;

namespace DotNetUnitTests.TestCases.HQLTestCases
{
    public partial class SafeDefault : HQLTestCasePage
    {
        /**
         * SELECT: Safe when Using Built-in Functions Example
         * By using NHibernate's built-in functions that aim to make executing querys more object-oriented, the input query is inherently parameterized.
         */
        protected void Page_Load(object sender, EventArgs e)
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
        }
    }
}
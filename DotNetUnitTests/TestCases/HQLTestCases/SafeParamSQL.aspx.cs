using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;

namespace DotNetUnitTests.TestCases.HQLTestCases
{
    public partial class SafeParamSQL : HQLTestCasePage
    {
        /**
         * SELECT: Safe when Parameterizing Custom SQL Queries (CreateSQLQuery) Example
         * By parameterizing the user input, we can succesfully block any SQL injection attempts.
         */
        protected void Page_Load(object sender, EventArgs e)
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
        }
    }
}
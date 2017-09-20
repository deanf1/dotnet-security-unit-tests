using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;

namespace DotNetUnitTests.TestCases.HQLTestCases
{
    public partial class DeleteUnsafeStringPlace : HQLTestCasePage
    {
        /**
         * DELETE: Unsafe when Using String Placeholders on Custom HQL Queries Example
         * By using string placeholders in the Delete method, the query is vulnerable to injection.
         */
        protected void Page_Load(object sender, EventArgs e)
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
        }
    }
}
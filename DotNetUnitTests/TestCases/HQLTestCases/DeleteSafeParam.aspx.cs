using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;

namespace DotNetUnitTests.TestCases.HQLTestCases
{
    public partial class DeleteSafeParam : HQLTestCasePage
    {
        /**
         * DELETE: Safe when Parameterizing Custom HQL Queries Example
         * By parameterizing the user input, we can succesfully block any HQL injection attempts. The only way to properly do this is write a delete query in the CreateQuery
         * method and add the parameters there. 
         */
        protected void Page_Load(object sender, EventArgs e)
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
        }
    }
}
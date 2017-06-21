using NHibernate.Dialect;
using NHibernate.Driver;

using System;
using DotNetUnitTests;
using NHibernate;
using NHibernate.Cfg;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetUnitTests
{
    public partial class nhibernate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /* Configuration cfg = new Configuration();

             string ds = "Data Source = (LocalDB)\\MSSQLLocalDB;";
             string ic = "Initial Catalog = Sample.mdf;";
             string ins = "Integrated Security = True;";
             string ConnectTimeout = "15";
             string Encrypt = "False";
             string TrustServerCertificate = "False";
             string ApplicationIntent = "ReadWrite";
             string MultiSubnetFailover = "False";

             cfg.DataBaseIntegration(x => { x.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\dfleming\\Documents\\Visual Studio 2015\\Projects\\DotNetUnitTests\\DotNetUnitTests\\App_Data\\Sample.mdf\";Integrated Security=True";


                 x.Driver<SqlClientDriver>();
                 x.Dialect<MsSql2008Dialect>();
             });

             cfg.AddAssembly(Assembly.GetExecutingAssembly());

             var sefact = cfg.BuildSessionFactory();*/


            //using (var session = sefact.OpenSession())
            //{
            /*Configuration config = new Configuration();
            config.DataBaseIntegration(x => {
                x.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\dfleming\\Documents\\Visual Studio 2015\\Projects\\DotNetUnitTests\\DotNetUnitTests\\App_Data\\Sample.mdf\";Integrated Security=True";


                x.Driver<SqlClientDriver>();
                x.Dialect<MsSql2008Dialect>();
            });*/
            //config.AddAssembly(Assembly.GetCallingAssembly());
          /*  ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
            ISession session = sessionFactory.OpenSession();

                using (var tx = session.BeginTransaction())
                {
                    IQuery q = session.CreateSQLQuery("INSERT INTO Student (LastName, FirstName) VALUES ('Userq', 'Test');");
                    q.ExecuteUpdate();
                    tx.Commit();
                    Response.Write(tx.WasCommitted);
                }

                string name = "Userq";
                IQuery query = session.CreateQuery("FROM DotNetUnitTests.Student WHERE LastName = :name OR 'a'='a'");
            query.SetParameter("name", name);
                IList<object> students = query.List<object>();
                if (students.Count == 0)
                    Response.Write("None found");
                else
                    foreach (Student s in students)
                {
                    Response.Write(s);
                }
            //}*/
        }
    }
}
using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Automapping;

namespace NhibernateBasics
{
    public class SessionFactory
    {
        private static volatile ISessionFactory iSessionFactory;
        private static object syncRoot = new Object();

        public static ISession OpenSession
        {
            get
            {
                if (iSessionFactory == null)
                {
                    lock (syncRoot)
                    {
                        if (iSessionFactory == null)
                        {
                            iSessionFactory = BuilSessionFactory();
                        }
                    }
                }
                return iSessionFactory.OpenSession();
            }
        }

        private static ISessionFactory BuilSessionFactory()
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.AppSettings["connection_string"];

                return Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2012
                    .ConnectionString(connectionString))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>())
                    .ExposeConfiguration(BuildSchema)
                    .BuildSessionFactory();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                throw ex;
            }
        }

        //Create Session
        private static AutoPersistenceModel CreateMappings()
        {
            return AutoMap
                .Assembly(System.Reflection.Assembly.GetExecutingAssembly())
                .Where(testc => testc.Namespace == "NhibernateBasics.Model");
        }

        private static void BuildSchema(NHibernate.Cfg.Configuration config)
        {

        }
    }
}

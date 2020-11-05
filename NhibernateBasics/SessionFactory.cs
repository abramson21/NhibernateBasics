using System.Configuration;
using System.Reflection;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Automapping;

using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace NhibernateBasics
{
    public static class SessionFactory
    {
        private static volatile ISessionFactory sessionFactory;

        private static readonly object syncRoot = new object();

        public static ISession OpenSession
        {
            get
            {
                if (sessionFactory == null)
                {
                    lock (syncRoot)
                    {
                        if (sessionFactory == null)
                        {
                            sessionFactory = BuilSessionFactory();
                        }
                    }
                }

                return sessionFactory.OpenSession();
            }
        }

        private static Assembly TargetAssembly => Assembly.GetExecutingAssembly();

        private static ISessionFactory BuilSessionFactory()
        {
            var connectionString = ConfigurationManager.AppSettings["connection_string"];

            var configuration = MsSqlConfiguration.MsSql2012
                    .ConnectionString(connectionString)
                    .ShowSql()
                    .FormatSql();

            return Fluently.Configure()
                .Database(configuration)
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssembly(TargetAssembly);
                    m.AutoMappings.Add(CreateAutoMappings);
                })
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private static AutoPersistenceModel CreateAutoMappings()
        {
            return AutoMap
                .Assembly(TargetAssembly)
                .Where(x => x.Namespace == "NhibernateBasics.Model");
        }

        private static void BuildSchema(NHibernate.Cfg.Configuration configuration)
        {
            new SchemaExport(configuration).Execute(true, true, true);
        }
    }
}

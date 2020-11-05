using System.Configuration;
using System.Reflection;

using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

using NHibernate;
using NHibernate.Tool.hbm2ddl;

using NhibernateBasics.Model;

namespace NhibernateBasics
{
    /// <summary>
    /// Фабрика сессий для взаимодействия с БД.
    /// </summary>
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
                            sessionFactory = BuildSessionFactory();
                        }
                    }
                }

                return sessionFactory.OpenSession();
            }
        }

        private static Assembly TargetAssembly => Assembly.GetExecutingAssembly();

        private static ISessionFactory BuildSessionFactory()
        {
            var connectionString = ConfigurationManager.AppSettings["connection_string"];

            var configuration = MsSqlConfiguration.MsSql2012.ConnectionString(connectionString);
#if DEBUG
            configuration = configuration.ShowSql().FormatSql();
#endif

            return Fluently.Configure()
                .Database(configuration)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Employee>())
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private static AutoPersistenceModel CreateAutoMappings()
        {
            return AutoMap
                .Assembly(TargetAssembly)
                .Where(x => x.Namespace == "NhibernateBasics.Model");
        }

        /// <summary>
        /// Метод, порождающий таблицы (если их не было в схеме) по конфигурации.
        /// </summary>
        /// <param name="configuration"> Конфигурация ORM, содержащая правила отображения (маппинги). </param>
        private static void BuildSchema(NHibernate.Cfg.Configuration configuration)
        {
            new SchemaExport(configuration).Execute(true, true, false);
        }
    }
}

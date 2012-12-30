using System;
using System.IO;
using System.Security;
using NHibernate;
using NHibernate.Cfg;


namespace HAC.Domain
{
    public class DataConfig
    {
        private static ISessionFactory _sessionFactory;
        private static bool _startupComplete = false;
        private static readonly object _locker = new object();

        public static ISession GetSession()
        {
            if (_sessionFactory == null)
            {
                InitializeSessionFactory();
            }
            ISession session = _sessionFactory.OpenSession();
            session.BeginTransaction();
            return session;
        }

        public static void EnsureStartup()
        {
            if (_startupComplete) return;
            lock (_locker)
            {
                if (_startupComplete) return;
                DataConfig.PerformStartup();
                _startupComplete = true;
            }
        }


        private static void PerformStartup()
        {
            //Initializer.RegisterBytecodeProvider();
            //Initializer.RegisterBytecodeProvider();
            InitializeSessionFactory();
            //InitializeRepositories();
        }

        private static void InitializeSessionFactory()
        {
           // NHibernate.Cfg.Environment.UseReflectionOptimizer = false;
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(Member).Assembly);
            //cfg.AddAssembly(typeof(Events).Assembly);
            _sessionFactory = cfg.BuildSessionFactory();
            //Configuration configuration =
            //    BuildConfiguration();
            //_sessionFactory =
            //    configuration.BuildSessionFactory();
        }

        //    .Database(SQLiteConfiguration.Standard.UsingFile("firstProject.db")
        //.ProxyFactoryFactory("NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle"))

        //public static Configuration BuildConfiguration()
        //{
        //    return Fluently.Configure()
        //                   .Database(MsSqlConfiguration.MsSql2008
        //                                              .ConnectionString(c => c.FromConnectionStringWithKey("live_con")))
        //                                                 //.ConnectionString(c => c.FromAppSetting("dev_con")))  @"Server=MARSHAL\SQLEXPRESS;Database=HAC2;Integrated Security=True;"))
        //        //   .ProxyFactoryFactory("NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle")
        //                                                   .Mappings(m => m.FluentMappings.AddFromAssemblyOf<EventsMapping>()
        //                                                   .Conventions.Add(FluentNHibernate.Conventions.Helpers.DefaultLazy.Never()))
        //                                                        //.Mappings(m => m.HbmMappings.AddFromAssemblyOf<Member>()
        //                                                        .BuildConfiguration();
        //}

       


        private static void InitializeRepositories()
        {
            //Func<IVisitorRepository> builder =
            //() => new VisitorRepository();
            //VisitorRepositoryFactory.RepositoryBuilder =
            //builder;
        }
    }
}
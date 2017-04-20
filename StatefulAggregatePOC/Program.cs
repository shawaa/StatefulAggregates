using System;
using System.Data;
using System.IO;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Event.Default;
using NHibernate.Tool.hbm2ddl;
using StatefulAggregatePOC.Domain;
using StatefulAggregatePOC.Infrastucture;
using StatefulAggregatePOC.Persistence;

namespace StatefulAggregatePOC
{
    public class Program
    {
        static void Main(string[] args)
        {App_Start.NHibernateProfilerBootstrapper.PreStart();

            ISessionFactory sessionFactory = CreateSessionFactory();

            Guid personId = CreatePerson(sessionFactory);
            Console.WriteLine();
            GetPersonAndPrintDetails(sessionFactory, personId);

            Console.WriteLine();
            ChangePersonName(sessionFactory, personId);
            Console.WriteLine();
            GetPersonAndPrintDetails(sessionFactory, personId);

            Console.WriteLine();
            ChangePersonName(sessionFactory, personId);
            Console.WriteLine();
            GetPersonAndPrintDetails(sessionFactory, personId);

            Console.WriteLine();
            ChangePostcode(sessionFactory, personId);
            Console.WriteLine();
            GetPersonAndPrintDetails(sessionFactory, personId);
        }

        private static void ChangePostcode(ISessionFactory sessionFactory, Guid personId)
        {
            using (ISession session = sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            using (PersonRepository personRepository = new PersonRepository(session))
            {
                Person person = personRepository.Get(personId);
                Console.WriteLine("Change Postcode");
                person.ChangePostcode("NG27GL");
                transaction.Commit();
            }
        }

        private static void ChangePersonName(ISessionFactory sessionFactory, Guid personId)
        {
            using (ISession session = sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            using (PersonRepository personRepository = new PersonRepository(session))
            {
                Person person = personRepository.Get(personId);
                Console.WriteLine("Change Name to Andy");
                person.ChangeName("Andy", "Shaw");
                transaction.Commit();
            }
        }

        private static Guid CreatePerson(ISessionFactory sessionFactory)
        {
            Guid personId;

            using (ISession session = sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            using (PersonRepository personRepository = new PersonRepository(session))
            {
                Person person = new Person("Andrew", "Sure", "NG23SN");
                personId = person.Id;
                Console.WriteLine("Create Person");
                personRepository.Add(person);
                transaction.Commit();
            }
            return personId;
        }

        private static void GetPersonAndPrintDetails(ISessionFactory sessionFactory, Guid personId)
        {
            using (ISession session = sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            using (PersonRepository personRepository = new PersonRepository(session))
            {
                Person person = personRepository.Get(personId);
                Console.WriteLine($"Got Person -> {person.Description}");
                transaction.Commit();
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            Configuration configuration = new Configuration();
            configuration.EventListeners.FlushEntityEventListeners = new IFlushEntityEventListener[] { new UpdateAggregateStateFlushEntityEventListener(), new DefaultFlushEntityEventListener() };
            configuration.EventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[] { new AggregateRootVersionListener() };
            configuration.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[] { new AggregateRootVersionListener() };
            configuration.EventListeners.FlushEventListeners = new IFlushEventListener[] { new AtlasDefaultFlushEventListener() };

            //configuration.EventListeners.FlushEventListeners = new IFlushEventListener[] { new DefaultFlushEventListener(), new LoggingFlushEventListener(),  };
            //configuration.EventListeners.PostLoadEventListeners = new IPostLoadEventListener[] { new DefaultPostLoadEventListener(),new LoggingPostLoadEventListener(),  };
            //configuration.EventListeners.AutoFlushEventListeners = new IAutoFlushEventListener[] { new DefaultAutoFlushEventListener(),new LoggingAutoFlushEventListener(),  };
            //configuration.EventListeners.DirtyCheckEventListeners = new IDirtyCheckEventListener[] { new DefaultDirtyCheckEventListener(),new LoggingDirtyCheckEventListener(),  };
            //configuration.EventListeners.EvictEventListeners = new IEvictEventListener[] { new DefaultEvictEventListener(),new LoggingEvictEventListener(),  };
            //configuration.EventListeners.FlushEntityEventListeners = new IFlushEntityEventListener[] { new DefaultFlushEntityEventListener(),new LoggingFlushEntityEventListener(),  };
            //configuration.EventListeners.LoadEventListeners = new ILoadEventListener[] { new DefaultLoadEventListener(),new LoggingLoadEventListener(),  };
            //configuration.EventListeners.LockEventListeners = new ILockEventListener[] { new DefaultLockEventListener(),new LoggingLockEventListener(),  };
            //configuration.EventListeners.PersistEventListeners = new IPersistEventListener[] { new DefaultPersistEventListener(),new LoggingPersistEventListener(),  };
            //configuration.EventListeners.PersistOnFlushEventListeners = new IPersistEventListener[] { new DefaultPersistEventListener(),new LoggingPersistEventListener(),  }; 
            //configuration.EventListeners.RefreshEventListeners = new IRefreshEventListener[] { new DefaultRefreshEventListener(),new LoggingRefreshEventListener(),  };
            //configuration.EventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[] { new LoggingPreUpLoggingdateEventListener(),  };
            //configuration.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[] { new LoggingPreInsertEventListener(),  };
            //configuration.EventListeners.PostCommitInsertEventListeners = new IPostInsertEventListener[] { new LoggingPostCommitInsertEventListener(),  };
            //configuration.EventListeners.PostInsertEventListeners = new IPostInsertEventListener[] { new LoggingPostCommitInsertEventListener(),  };
            //configuration.EventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[] { new LoggingPostUpdateEventListener(),  };


            //configuration.Interceptor = new TransientInterceptor();
            NHibernateProfiler.Initialize();

            return Fluently.Configure(configuration)
                .Database(SQLiteConfiguration.Standard.UsingFile("StatefulAggregatePoc.db").IsolationLevel(IsolationLevel.ReadCommitted))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>())
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
            if (File.Exists("StatefulAggregatePoc.db"))
                File.Delete("StatefulAggregatePoc.db");

            new SchemaExport(config)
              .Create(false, true);
        }
    }
}


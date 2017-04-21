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
            GetPersonAndPrintDetails(sessionFactory, personId);

            ChangePersonName(sessionFactory, personId);
            GetPersonAndPrintDetails(sessionFactory, personId);

            ChangePersonName(sessionFactory, personId);
            GetPersonAndPrintDetails(sessionFactory, personId);

            ChangePostcode(sessionFactory, personId);
            GetPersonAndPrintDetails(sessionFactory, personId);

            StartNewJob(sessionFactory, personId);
            GetPersonAndPrintDetails(sessionFactory, personId);

            EndJob(sessionFactory, personId);
            GetPersonAndPrintDetails(sessionFactory, personId);
        }

        private static void ChangePostcode(ISessionFactory sessionFactory, Guid personId)
        {
            using (ISession session = sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            using (PersonRepository personRepository = new PersonRepository(session))
            {
                Person person = personRepository.Get(personId);
                Console.WriteLine("Change Postcode (Change member)");
                person.ChangePostcode("NG27GL");
                transaction.Commit();
            }
        }

        private static void StartNewJob(ISessionFactory sessionFactory, Guid personId)
        {
            using (ISession session = sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            using (PersonRepository personRepository = new PersonRepository(session))
            {
                Person person = personRepository.Get(personId);
                Console.WriteLine("Start new Job (Change a collection)");
                person.StartJob("Super Dev", new DateTime(2001, 1, 1));
                transaction.Commit();
            }
        }

        private static void EndJob(ISessionFactory sessionFactory, Guid personId)
        {
            using (ISession session = sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            using (PersonRepository personRepository = new PersonRepository(session))
            {
                Person person = personRepository.Get(personId);
                Console.WriteLine("End Job (Change item in a collection)");
                person.EndJob("Super Dev", new DateTime(2003, 1, 1));
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
                Console.WriteLine("Change Name to Andy (Edit aggregate root)");
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
                Person person = new Person("Andrew", "Sure", "NG235SN");
                personId = person.Id;
                person.StartJob("Dev", new DateTime(2000, 1, 1));
                Console.WriteLine("Create Person (Create aggregate)");
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


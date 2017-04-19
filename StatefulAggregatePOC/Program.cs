using System;
using System.IO;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
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
        {
            ISessionFactory sessionFactory = CreateSessionFactory();

            Guid personId = CreatePerson(sessionFactory);
            GetPersonAndPrintDetails(sessionFactory, personId);

            ChangePersonName(sessionFactory, personId);
            GetPersonAndPrintDetails(sessionFactory, personId);

            ChangePersonName(sessionFactory, personId);
            GetPersonAndPrintDetails(sessionFactory, personId);

            ChangePostcode(sessionFactory, personId);
            GetPersonAndPrintDetails(sessionFactory, personId);
        }

        private static void ChangePostcode(ISessionFactory sessionFactory, Guid personId)
        {
            Console.WriteLine("Change Postcode");

            using (ISession session = sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            using (PersonRepository personRepository = new PersonRepository(session))
            {
                Person person = personRepository.Get(personId);
                person.ChangePostcode("NG27GL");
                transaction.Commit();
            }
        }

        private static void ChangePersonName(ISessionFactory sessionFactory, Guid personId)
        {
            Console.WriteLine("Change Name to Andy");

            using (ISession session = sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            using (PersonRepository personRepository = new PersonRepository(session))
            {
                Person person = personRepository.Get(personId);
                person.ChangeName("Andy", "Shaw");
                transaction.Commit();
            }
        }

        private static Guid CreatePerson(ISessionFactory sessionFactory)
        {
            Guid personId;
            Console.WriteLine("Create Person");

            using (ISession session = sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            using (PersonRepository personRepository = new PersonRepository(session))
            {
                Person person = new Person("Andrew", "Sure", "NG23SN");
                personId = person.Id;

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

            return Fluently.Configure(configuration)
                .Database(SQLiteConfiguration.Standard.UsingFile("StatefulAggregatePoc.db"))
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

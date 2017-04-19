using System;
using NHibernate;
using StatefulAggregatePOC.Domain;

namespace StatefulAggregatePOC.Persistence
{
    public class PersonRepository : IDisposable
    {
        private readonly ISession _session;

        public PersonRepository(ISession session)
        {
            _session = session;
        }

        public void Add(Person person)
        {
            _session.SaveOrUpdate(person.GetSerializableState());
        }

        public void Dispose()
        {
            _session.Dispose();
        }

        public Person Get(Guid personId)
        {
            PersonState personState = _session.QueryOver<PersonState>().SingleOrDefault();
            Person person = new Person(personState);
            personState.AggregateRoot = person;

            return person;
        }
    }
}
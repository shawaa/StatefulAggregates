using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.UserTypes;
using StatefulAggregatePOC.Infrastucture;

namespace StatefulAggregatePOC.Domain
{
    public class Person : IAggregateRoot
    {
        private string _firstName;

        private string _lastName;

        private readonly PersonAddress _personAddress;

        private readonly IList<Job> _jobs;

        public Person(string firstName, string lastName, string postCode)
        {
            Id = Guid.NewGuid();
            Version = 1;
            
            _firstName = firstName;
            _lastName = lastName;

            _personAddress = new PersonAddress(postCode);
            _jobs = new List<Job>();
        }

        internal Person(PersonState personState)
        {
            Id = personState.Id;
            Version = personState.Version;

            _firstName = personState.FirstName;
            _lastName = personState.LastName;

            _personAddress = new PersonAddress(personState);
            _jobs = personState.Jobs.Select(x => new Job(x)).ToList();
        }

        public Guid Id { get; }

        public int Version { get; }

        public string Description => $"V{Version}: {_firstName} {_lastName} of {_personAddress.PostCode} doing job {_jobs.OrderBy(x => x.StartDate).Last().JobTitle}";

        public void ChangeName(string newFirstName, string newLastName)
        {
            _firstName = newFirstName;
            _lastName = newLastName;
        }

        public void StartJob(string jobTitle, DateTime startDate)
        {
            _jobs.Add(new Job(startDate, jobTitle));
        }

        public void ChangePostcode(string postcode)
        {
            _personAddress.ChangePostCode(postcode);
        }

        public IAggregateState GetSerializableState()
        {
            PersonState aggregateState = new PersonState
            {
                Id = Id,
                Version = Version,
                AggregateRoot = this,
                FirstName = _firstName,
                LastName = _lastName,
            };
            
            aggregateState.Jobs = _jobs.Select(x => x.GetJobState(aggregateState)).ToList();
            aggregateState.PersonAddressState = _personAddress.GetSerializableState(aggregateState);

            return aggregateState;
        }
    }
}
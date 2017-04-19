using System;
using StatefulAggregatePOC.Infrastucture;

namespace StatefulAggregatePOC.Domain
{
    public class Person : IAggregateRoot
    {
        private string _firstName;

        private string _lastName;

        private readonly PersonAddress _personAddress;

        public Person(string firstName, string lastName, string postCode)
        {
            Id = Guid.NewGuid();
            Version = 1;

            _firstName = firstName;
            _lastName = lastName;
            _personAddress = new PersonAddress(postCode);
        }

        internal Person(PersonState personState)
        {
            Id = personState.Id;
            Version = personState.Version;

            _firstName = personState.FirstName;
            _lastName = personState.LastName;
            _personAddress = new PersonAddress(personState);
        }

        public Guid Id { get; }

        public int Version { get; }

        public string Description => $"V{Version}: {_firstName} {_lastName} of {_personAddress.PostCode}";

        public void ChangeName(string newFirstName, string newLastName)
        {
            _firstName = newFirstName;
            _lastName = newLastName;
        }

        public void ChangePostcode(string postcode)
        {
            _personAddress.ChangePostCode(postcode);
        }

        public ISerializableAggregateState GetSerializableState()
        {
            PersonAddressState personAddressState = _personAddress.GetSerializableState();

            PersonState aggregateState = new PersonState
            {
                Id = Id,
                Version = Version,
                FirstName = _firstName,
                LastName = _lastName,
                PersonAddressState = personAddressState,
                AggregateRoot = this
            };

            personAddressState.Person = aggregateState;

            return aggregateState;
        }
    }
}
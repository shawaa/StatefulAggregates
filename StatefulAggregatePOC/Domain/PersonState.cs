using System;
using StatefulAggregatePOC.Infrastucture;

namespace StatefulAggregatePOC.Domain
{
    public class PersonState : ISerializableAggregateState
    {
        public virtual IAggregateRoot AggregateRoot { get; set; }

        public virtual Guid Id { get; set; }

        public virtual int Version { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual PersonAddressState PersonAddressState { get; set; }

        public virtual bool Equals(ISerializableAggregateState other)
        {
            if (!other.GetType().IsAssignableFrom(typeof(PersonState))) return false;

            PersonState otherPerson = (PersonState) other;

            bool detailsAreEqual = Id == otherPerson.Id
                    && FirstName == otherPerson.FirstName
                    && LastName == otherPerson.LastName;

            bool addressesAreEqual = PersonAddressState.Id == otherPerson.PersonAddressState.Id
                && PersonAddressState.Person.Id == otherPerson.PersonAddressState.Person.Id
                && PersonAddressState.PostCode == otherPerson.PersonAddressState.PostCode;

            if (detailsAreEqual && addressesAreEqual) return true;

            return false;
        }

        public virtual void Update(ISerializableAggregateState newState)
        {
            if(!newState.GetType().IsAssignableFrom(typeof(PersonState))) throw new InvalidOperationException();

            PersonState newPersonState = (PersonState)newState;

            FirstName = newPersonState.FirstName;
            LastName = newPersonState.LastName;

            PersonAddressState.PostCode = newPersonState.PersonAddressState.PostCode;
        }

        public virtual bool IsSaved { get; set; }
    }
}
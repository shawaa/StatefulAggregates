using System;

namespace StatefulAggregatePOC.Domain
{
    public class PersonAddress
    {
        private readonly Guid _id;

        public PersonAddress(string postcode)
        {
            PostCode = postcode;
            _id = Guid.NewGuid();
        }

        public PersonAddress(PersonState personState)
        {
            _id = personState.PersonAddressState.Id;
            PostCode = personState.PersonAddressState.PostCode;

            personState.PersonAddressState.Person = personState;
        }

        public string PostCode { get; private set; }

        internal void ChangePostCode(string newPostCode)
        {
            PostCode = newPostCode;
        }

        public PersonAddressState GetSerializableState(PersonState personState)
        {
            return new PersonAddressState
            {
                Id = _id,
                PostCode = PostCode,
                Person = personState
            };
        }
    }
}
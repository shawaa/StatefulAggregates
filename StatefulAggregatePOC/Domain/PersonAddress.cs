using System;

namespace StatefulAggregatePOC.Domain
{
    public sealed class PersonAddress : IPersonAddress
    {
        private readonly Guid _id;

        public PersonAddress(string postcode)
        {
            PostCode = postcode;
            _id = Guid.NewGuid();
        }

        public PersonAddress(PersonAddressState personAddressState)
        {
            _id = personAddressState.Id;
            PostCode = personAddressState.PostCode;
        }

        public string PostCode { get; private set; }

        public void ChangePostCode(string newPostCode)
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
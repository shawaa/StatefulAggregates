using System;
using StatefulAggregatePOC.Infrastucture;

namespace StatefulAggregatePOC.Domain
{
    public class PersonAddressState : IPersistable
    {
        public virtual Guid Id { get; set; }

        public virtual string PostCode { get; set; }

        public virtual PersonState Person { get; set; }
    }
}
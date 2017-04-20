using System;
using StatefulAggregatePOC.Infrastucture;

namespace StatefulAggregatePOC.Domain
{
    public class PersonAddressState : ISerializableAggregateMemberState
    {
        public virtual Guid Id { get; set; }

        public virtual string PostCode { get; set; }

        public virtual PersonState Person { get; set; }

        public virtual ISerializableAggregateState AggregateRootState => Person;
    }
}
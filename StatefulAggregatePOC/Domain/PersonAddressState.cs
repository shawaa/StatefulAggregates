using System;

namespace StatefulAggregatePOC.Domain
{
    public class PersonAddressState : IAggregateStatePart
    {
        public virtual Guid Id { get; set; }

        public virtual string PostCode { get; set; }

        public virtual PersonState Person { get; set; }

        public virtual IAggregateState AggregateRootState => Person;
    }
}
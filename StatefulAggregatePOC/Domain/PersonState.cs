using System;
using System.Collections.Generic;

namespace StatefulAggregatePOC.Domain
{
    public class PersonState : IAggregateState
    {
        public virtual IAggregateRoot AggregateRoot { get; set; }

        public virtual Guid Id { get; set; }

        public virtual int Version { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual PersonAddressState PersonAddressState { get; set; }

        public virtual IList<JobState> Jobs { get; set; }
    }
}
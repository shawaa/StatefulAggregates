using System;

namespace StatefulAggregatePOC.Domain
{
    public class JobState : IAggregateStatePart
    {
        public virtual Guid Id { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public virtual string JobTitle { get; set; }

        public virtual PersonState PersonState { get; set; }

        public virtual IAggregateState AggregateRootState => PersonState;
    }
}
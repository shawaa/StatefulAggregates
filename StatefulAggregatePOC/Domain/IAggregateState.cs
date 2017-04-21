using System;

namespace StatefulAggregatePOC.Domain
{
    public interface IAggregateState
    {
        IAggregateRoot AggregateRoot { get; set; }

        Guid Id { get; set; }

        int Version { get; set; }
    }
}
using System;

namespace StatefulAggregatePOC.Infrastucture
{
    public interface IAggregateState
    {
        IAggregateRoot AggregateRoot { get; set; }

        Guid Id { get; set; }

        int Version { get; set; }
    }
}
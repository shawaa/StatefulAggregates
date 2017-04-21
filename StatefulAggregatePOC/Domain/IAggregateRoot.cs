using System;

namespace StatefulAggregatePOC.Domain
{
    public interface IAggregateRoot
    {
        IAggregateState GetState();

        Guid Id { get; }

        int Version { get; }
    }
}
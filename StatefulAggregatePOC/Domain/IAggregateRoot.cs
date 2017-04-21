using System;

namespace StatefulAggregatePOC.Domain
{
    public interface IAggregateRoot
    {
        IAggregateState GetSerializableState();

        Guid Id { get; }

        int Version { get; }
    }
}
using System;

namespace StatefulAggregatePOC.Infrastucture
{
    public interface IAggregateRoot
    {
        ISerializableAggregateState GetSerializableState();

        Guid Id { get; }

        int Version { get; }
    }
}
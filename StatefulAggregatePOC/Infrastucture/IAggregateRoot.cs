using System;

namespace StatefulAggregatePOC.Infrastucture
{
    public interface IAggregateRoot
    {
        IAggregateState GetSerializableState();

        Guid Id { get; }

        int Version { get; }
    }
}
using System;

namespace StatefulAggregatePOC.Infrastucture
{
    public interface ISerializableAggregateState
    {
        IAggregateRoot AggregateRoot { get; set; }

        Guid Id { get; set; }

        int Version { get; set; }

        bool Equals(ISerializableAggregateState other);

        void Update(ISerializableAggregateState newState);
    }
}
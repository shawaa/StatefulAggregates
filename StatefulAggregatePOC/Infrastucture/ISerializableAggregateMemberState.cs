namespace StatefulAggregatePOC.Infrastucture
{
    public interface ISerializableAggregateMemberState : IPersistable
    {
        ISerializableAggregateState AggregateRootState { get; }
    }
}
namespace StatefulAggregatePOC.Infrastucture
{
    public interface ISerializableAggregateMemberState
    {
        ISerializableAggregateState AggregateRootState { get; }
    }
}
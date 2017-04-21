namespace StatefulAggregatePOC.Domain
{
    public interface IAggregateStatePart
    {
        IAggregateState AggregateRootState { get; }
    }
}
namespace StatefulAggregatePOC.Infrastucture
{
    public interface IAggregateStatePart
    {
        IAggregateState AggregateRootState { get; }
    }
}
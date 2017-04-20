namespace StatefulAggregatePOC.Infrastucture
{
    public interface IPersistable
    {
        bool IsSaved { get; set; }
    }
}
namespace StatefulAggregatePOC.Domain
{
    public interface IPersonAddress
    {
        string PostCode { get; }

        void ChangePostCode(string newPostCode);

        PersonAddressState GetSerializableState(PersonState personState);
    }
}
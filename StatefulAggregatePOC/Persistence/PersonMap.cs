using FluentNHibernate.Mapping;
using StatefulAggregatePOC.Domain;

namespace StatefulAggregatePOC.Persistence
{
    public class PersonMap : ClassMap<PersonState>
    {
        public PersonMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            Version(x => x.Version);

            Map(x => x.FirstName);

            Map(x => x.LastName);

            HasOne(x => x.PersonAddressState)
                .Cascade.All()
                .PropertyRef(x => x.Person);
        }
    }
}
using System.Security.Cryptography.X509Certificates;
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

            HasMany(x => x.Jobs)
                .Cascade.AllDeleteOrphan()
                .Inverse();

            HasOne(x => x.PersonAddressState)
                .Cascade.All()
                .PropertyRef(x => x.Person);
        }
    }
}
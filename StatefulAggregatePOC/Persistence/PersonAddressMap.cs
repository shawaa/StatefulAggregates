using FluentNHibernate.Mapping;
using StatefulAggregatePOC.Domain;

namespace StatefulAggregatePOC.Persistence
{
    public class PersonAddressMap : ClassMap<PersonAddressState>
    {
        public PersonAddressMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.PostCode);

            References(x => x.Person);
        }
    }
}
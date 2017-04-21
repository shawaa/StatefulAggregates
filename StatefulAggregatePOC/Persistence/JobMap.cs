using FluentNHibernate.Mapping;
using StatefulAggregatePOC.Domain;

namespace StatefulAggregatePOC.Persistence
{
    public class JobMap : ClassMap<JobState>
    {
        public JobMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.JobTitle).Not.Nullable();

            Map(x => x.StartDate).Not.Nullable();

            Map(x => x.EndDate).Nullable();

            References(x => x.PersonState);
        }
    }
}
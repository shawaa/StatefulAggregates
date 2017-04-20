using NHibernate;
using NHibernate.Event;

namespace StatefulAggregatePOC.Infrastucture
{
    public class AggregateRootVersionListener : IPreUpdateEventListener, IPreInsertEventListener
    {
        public bool OnPreInsert(PreInsertEvent @event)
        {
            return ForceAggregateRootVersion(@event);
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            return ForceAggregateRootVersion(@event);
        }

        private static bool ForceAggregateRootVersion(AbstractPreDatabaseOperationEvent @event)
        {
            ISerializableAggregateMemberState memberState = @event.Entity as ISerializableAggregateMemberState;
            if (memberState == null)
            {
                return false;
            }

            @event.Session.Lock(memberState.AggregateRootState, LockMode.Force);

            return false;
        }
    }
}
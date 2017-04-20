using System.Linq;
using NHibernate;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;

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
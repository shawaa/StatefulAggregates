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
            IAggregateStatePart statePart = @event.Entity as IAggregateStatePart;
            if (statePart == null)
            {
                return false;
            }

            if (!IsAggregateRootDirty(@event.Session, statePart.AggregateRootState))
            {
                @event.Session.Lock(statePart.AggregateRootState, LockMode.Force);
            }

            return false;
        }

        private static bool IsAggregateRootDirty(ISession session, IAggregateState entity)
        {
            ISessionImplementor sessionImplementation = session.GetSessionImplementation();
            IPersistenceContext persistenceContext = sessionImplementation.PersistenceContext;

            if (entity.IsProxy())
            {
                entity = (IAggregateState)persistenceContext.Unproxy(entity);
            }

            EntityEntry entityEntry = persistenceContext.GetEntry(entity);

            IEntityPersister entityPersister = sessionImplementation.GetEntityPersister(null, entity);

            object[] oldState = entityEntry.LoadedState;
            object[] currentState = entityPersister.GetPropertyValues(entity, sessionImplementation.EntityMode);

            int[] findDirty = entityEntry.Persister.FindDirty(currentState, oldState, entity, sessionImplementation);
            bool hasDirtyCollection = currentState.OfType<IPersistentCollection>().Any(x => x.IsDirty);

            return (findDirty != null) || hasDirtyCollection;
        }
    }
}
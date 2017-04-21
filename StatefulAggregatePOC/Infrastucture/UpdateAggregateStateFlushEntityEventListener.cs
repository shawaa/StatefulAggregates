using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Event;

namespace StatefulAggregatePOC.Infrastucture
{
    public class UpdateAggregateStateFlushEntityEventListener : IFlushEntityEventListener
    {
        public void OnFlushEntity(FlushEntityEvent @event)
        {
            ICollection entities = @event.Session.PersistenceContext.EntityEntries.Keys;

            foreach (object entity in entities)
            {
                ISerializableAggregateState oldState = entity as ISerializableAggregateState;

                if (oldState == null)
                {
                    continue;
                }

                ISerializableAggregateState newState = oldState.AggregateRoot.GetSerializableState();
                
                @event.Session.Merge(newState);
            }
        }
    }
}
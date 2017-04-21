using System.Collections;
using System.Collections.Generic;
using NHibernate.Event;

namespace StatefulAggregatePOC.Infrastucture
{
    public class UpdateAggregateStateFlushEntityEventListener : IFlushEntityEventListener
    {
        public void OnFlushEntity(FlushEntityEvent @event)
        {
            ICollection entities = @event.Session.PersistenceContext.EntityEntries.Keys;

            IList<IAggregateState> aggregateStates = new List<IAggregateState>();

            foreach (object entity in entities)
            {
                IAggregateState oldState = entity as IAggregateState;

                if (oldState == null)
                {
                    continue;
                }

                IAggregateState newState = oldState.AggregateRoot.GetSerializableState();
                
                aggregateStates.Add(newState);
            }

            foreach (IAggregateState aggregateState in aggregateStates)
            {
                @event.Session.Merge(aggregateState);
            }
        }
    }
}
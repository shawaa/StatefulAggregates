using System.Collections;
using System.Collections.Generic;
using NHibernate.Event;
using StatefulAggregatePOC.Domain;

namespace StatefulAggregatePOC.Infrastucture
{
    public class UpdateAggregateStateFlushEntityEventListener : IFlushEntityEventListener
    {
        public void OnFlushEntity(FlushEntityEvent @event)
        {
            //Why does this not work when changing an item in a collection? Is @event.Entity a different thing to @event.Session.PersistenceContext.EntityEntries.Keys[0]?
            //IAggregateState aggregateState = (@event.Entity as IAggregateStatePart)?.AggregateRootState ?? @event.Entity as IAggregateState;
            //@event.Session.Merge(aggregateState);

            ICollection entities = @event.Session.PersistenceContext.EntityEntries.Keys;

            IList<IAggregateState> aggregateStates = new List<IAggregateState>();

            foreach (object entity in entities)
            {
                IAggregateState oldState = entity as IAggregateState;

                if (oldState == null)
                {
                    continue;
                }

                IAggregateState newState = oldState.AggregateRoot.GetState();

                aggregateStates.Add(newState);
            }

            foreach (IAggregateState aggregateState in aggregateStates)
            {
                @event.Session.Merge(aggregateState);
            }
        }
    }
}
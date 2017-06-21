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
            object entity = @event.Entity;

            IAggregateState oldState = entity as IAggregateState;

            if (oldState == null)
            {
                return;
            }

            IAggregateState newState = oldState.AggregateRoot.GetState();

            @event.Session.Merge(newState);
        }
    }
}

using System.Collections;
using NHibernate.Event;

namespace StatefulAggregatePOC.Infrastucture
{
    public class UpdateAggregateStateFlushEntityEventListener : IFlushEntityEventListener
    {
        public void OnFlushEntity(FlushEntityEvent @event)
        {
            ICollection entities = @event.Session.PersistenceContext.EntityEntries.Keys;

            //Console.WriteLine("\tFlushEntityEvent was raised");

            foreach (object entity in entities)
            {
                ISerializableAggregateState oldState = entity as ISerializableAggregateState;

                if (oldState == null)
                {
                    continue;
                }

                ISerializableAggregateState newState = oldState.AggregateRoot.GetSerializableState();

                bool isDirt = !newState.Equals(oldState);

                if (isDirt)
                {
                    oldState.Update(newState);
                }
            }
        }
    }
}
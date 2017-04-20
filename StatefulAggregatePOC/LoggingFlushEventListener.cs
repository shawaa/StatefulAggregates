using System;
using NHibernate.Event;

namespace StatefulAggregatePOC
{
    public class LoggingFlushEventListener : IFlushEventListener
    {
        public void OnFlush(FlushEvent @event)
        {
            Console.WriteLine(@event.GetType().Name);
        }
    }
}
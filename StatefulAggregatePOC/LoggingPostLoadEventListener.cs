using System;
using NHibernate.Event;

namespace StatefulAggregatePOC
{
    public class LoggingPostLoadEventListener : IPostLoadEventListener
    {
        public void OnPostLoad(PostLoadEvent @event)
        {
            Console.WriteLine(@event.GetType().Name);
        }
    }
}
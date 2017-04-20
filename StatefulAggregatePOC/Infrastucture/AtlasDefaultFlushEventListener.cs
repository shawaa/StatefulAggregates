using NHibernate.Event;
using NHibernate.Event.Default;

namespace StatefulAggregatePOC.Infrastucture
{
    public class AtlasDefaultFlushEventListener : DefaultFlushEventListener
    {
        /// <summary>
        /// Original implementation from NHibernate AbstractFlushingEventListener
        /// </summary>
        /// <param name="session"></param>
        protected override void PerformExecutions(IEventSource session)
        {
            try
            {
                session.ConnectionManager.FlushBeginning();

                // Implementation as per patch from from https://nhibernate.jira.com/browse/NH-3226
                session.PersistenceContext.Flushing = true;

                // we need to lock the collection caches before
                // executing entity inserts/updates in order to
                // account for bidi associations
                session.ActionQueue.PrepareActions();
                session.ActionQueue.ExecuteActions();
            }
            finally
            {
                // Implementation as per patch from from https://nhibernate.jira.com/browse/NH-3226
                session.PersistenceContext.Flushing = false;
                session.ConnectionManager.FlushEnding();
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Event;

namespace StatefulAggregatePOC
{
    public class LoggingAutoFlushEventListener : IAutoFlushEventListener
    {
        public void OnAutoFlush(AutoFlushEvent @event)
        {
            Console.WriteLine(@event.GetType().Name);
        }
    }

    public class LoggingDeleteEventListener : IDeleteEventListener
    {
        public void OnDelete(DeleteEvent @event)
        {
            Console.WriteLine(@event.GetType().Name);
        }

        public void OnDelete(DeleteEvent @event, ISet<object> transientEntities)
        {
            Console.WriteLine(@event.GetType().Name);
        }
    }

    public class LoggingDirtyCheckEventListener : IDirtyCheckEventListener
    {
        public void OnDirtyCheck(DirtyCheckEvent @event)
        {
            Console.WriteLine(@event.GetType().Name);
        }
    }

    public class LoggingEvictEventListener : IEvictEventListener
    {
        public void OnEvict(EvictEvent @event)
        {
            Console.WriteLine(@event.GetType().Name);
        }
    }

    public class LoggingFlushEntityEventListener : IFlushEntityEventListener
    {
        public void OnFlushEntity(FlushEntityEvent @event)
        {
            Console.WriteLine(@event.GetType().Name);
        }
    }

    public class LoggingLoadEventListener : ILoadEventListener
    {
        public void OnLoad(LoadEvent @event, LoadType loadType)
        {
            Console.WriteLine(@event.GetType().Name);
        }
    }

    public class LoggingLockEventListener : ILockEventListener
    {
        public void OnLock(LockEvent @event)
        {
            Console.WriteLine(@event.GetType().Name);
        }
    }

    public class LoggingPersistEventListener : IPersistEventListener
    {
        public void OnPersist(PersistEvent @event)
        {
            Console.WriteLine(@event.GetType().Name);
        }

        public void OnPersist(PersistEvent @event, IDictionary createdAlready)
        {
            Console.WriteLine(@event.GetType().Name);
        }
    }

    public class LoggingPersistOnFlushEventListener : IPersistEventListener
    {
        public void OnPersist(PersistEvent @event)
        {
            Console.WriteLine(@event.GetType().Name);
        }

        public void OnPersist(PersistEvent @event, IDictionary createdAlready)
        {
            Console.WriteLine(@event.GetType().Name);
        }
    }

    public class LoggingRefreshEventListener : IRefreshEventListener
    {
        public void OnRefresh(RefreshEvent @event)
        {
            Console.WriteLine(@event.GetType().Name);
        }

        public void OnRefresh(RefreshEvent @event, IDictionary refreshedAlready)
        {
            Console.WriteLine(@event.GetType().Name);
        }
    }

    public class LoggingPreUpLoggingdateEventListener : IPreUpdateEventListener
    {
        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            Console.WriteLine(@event.GetType().Name);
            return false;
        }
    }

    public class LoggingPreInsertEventListener : IPreInsertEventListener
    {
        public bool OnPreInsert(PreInsertEvent @event)
        {
            Console.WriteLine(@event.GetType().Name);
            return false;
        }
    }

    public class LoggingPostCommitInsertEventListener : IPostInsertEventListener
    {
        public void OnPostInsert(PostInsertEvent @event)
        {
            Console.WriteLine(@event.GetType().Name);
        }
    }

    public class LoggingPostInsertEventListener : IPostInsertEventListener
    {
        public void OnPostInsert(PostInsertEvent @event)
        {
            Console.WriteLine(@event.GetType().Name);
        }
    }

    public class LoggingPostUpdateEventListener : IPostUpdateEventListener
    {
        public void OnPostUpdate(PostUpdateEvent @event)
        {
            Console.WriteLine(@event.GetType().Name);
        }
    }
}
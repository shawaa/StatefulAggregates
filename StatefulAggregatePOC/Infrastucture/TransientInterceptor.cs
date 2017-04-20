using NHibernate;
using NHibernate.Type;

namespace StatefulAggregatePOC.Infrastucture
{
    public class TransientInterceptor : EmptyInterceptor
    {
        public override bool? IsTransient(object entity)
        {
            return !(entity as IPersistable)?.IsSaved;
        }

        public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            IPersistable assignedEntity = entity as IPersistable;

            if (assignedEntity != null)
            {
                assignedEntity.IsSaved = true;
            }

            return false;
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            IPersistable assignedEntity = entity as IPersistable;

            if (assignedEntity != null)
            {
                assignedEntity.IsSaved = true;
            }

            return false;
        }
    }
}
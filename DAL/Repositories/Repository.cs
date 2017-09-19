using System.Linq;
using System.Data.Entity;

namespace DAL
{

    public class Repository<T> : System.Object, IRepository<T> where T : Models.BaseEntity
    {
        public Repository(Models.DatabaseContext databaseContext)
        {
            if (databaseContext == null)
            {
                throw (new System.ArgumentNullException("databaseContext"));
            }

            DatabaseContext = databaseContext;
            DbSet = DatabaseContext.Set<T>();
        }

        protected System.Data.Entity.DbSet<T> DbSet { get; set; }
        protected Models.DatabaseContext DatabaseContext { get; set; }

        public virtual void Insert(T entity)
        {
            if (entity == null)
            {
                throw (new System.ArgumentNullException("entity"));
            }

            DbSet.Add(entity);

        }

        public virtual void Update(T entity)
        {
            if (entity == null)
            {
                throw (new System.ArgumentNullException("entity"));
            }

            // **************************************************
            // Just For Debug!
            // **************************************************
            EntityState oEntityState =
                DatabaseContext.Entry(entity).State;
            // **************************************************
            // /Just For Debug!
            // **************************************************

            if (oEntityState == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            // **************************************************
            // Just For Debug!
            // **************************************************
            oEntityState =
                DatabaseContext.Entry(entity).State;
            // **************************************************
            // /Just For Debug!
            // **************************************************

            DatabaseContext.Entry(entity).State =
                EntityState.Modified;

            // **************************************************
            // Just For Debug!
            // **************************************************
            oEntityState =
                DatabaseContext.Entry(entity).State;
            // **************************************************
            // /Just For Debug!
            // **************************************************
        }

        public virtual void Delete(T entity)
        {
            if (entity == null)
            {
                throw (new System.ArgumentNullException("entity"));
            }

            // **************************************************
            // Just For Debug!
            // **************************************************
            EntityState oEntityState =
                DatabaseContext.Entry(entity).State;
            // **************************************************
            // /Just For Debug!
            // **************************************************

            if (oEntityState == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            // **************************************************
            // Just For Debug!
            // **************************************************
            oEntityState =
                DatabaseContext.Entry(entity).State;
            // **************************************************
            // /Just For Debug!
            // **************************************************

            DbSet.Remove(entity);

            // **************************************************
            // Just For Debug!
            // **************************************************
            oEntityState =
                DatabaseContext.Entry(entity).State;
            // **************************************************
            // /Just For Debug!
            // **************************************************
        }

        public virtual void DeleteByList(System.Collections.Generic.List<T> entities)
        {
            if (entities == null)
            {
                throw (new System.ArgumentNullException("entities"));
            }

            foreach (T item in entities)
            {
                Delete(item);
            }
        }

        public virtual bool DeleteById(System.Guid id)
        {

            T oEntity = DbSet.Find(id);

            if (oEntity == null)
            {
                return (false);
            }
            else
            {
                Delete(oEntity);

                return (true);
            }
        }

        public virtual T GetById(System.Guid id)
        {
            return (DbSet.Find(id));
        }

        public virtual System.Linq.IQueryable<T> Get()
        {
            return (DbSet);
        }

        public IQueryable<T> Get
            (System.Linq.Expressions.Expression<System.Func<T, bool>> predicate)
        {
            return (DbSet.Where(predicate));
        }

        public virtual System.Collections.Generic.IEnumerable<T> GetWithRawSql
            (string query, params object[] parameters)
        {
            return (DbSet.SqlQuery(query, parameters).ToList());
        }
    }
}

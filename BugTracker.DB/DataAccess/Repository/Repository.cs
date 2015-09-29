using BugTracker.DB.Entities;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BugTracker.DB.DataAccess
{
    public class Repository<T> : IRepository<T> where T : Entity<long>, new()
    {
        protected NHibernate.ISession Session { get; set; }

        public Repository(ISession session)
        {
            Session s = session as Session;
            this.Session = s.NHSession;
        }

        public virtual T Load(object id)
        {
            return this.Session.Load<T>(id);
        }

        public virtual T GetById(object id)
        {
            return this.Session.Get<T>(id);
        }

        public virtual ICollection<T> List()
        {
            return this.Session.CreateCriteria(typeof(T)).List<T>();
        }

        public virtual IQueryable<T> Query()
        {
            return this.Session.Query<T>();
        }

        public virtual void Save(T entity)
        {
            if (!this.Session.Transaction.IsActive)
            {
                throw new InvalidOperationException("Write operations must use transaction");
            }

            this.Session.Save(entity);
        }

        public virtual void SaveOrUpdate(T entity)
        {
            if (this.Session.Transaction == null)
            {
                throw new InvalidOperationException("Write operations must use transaction");
            }

            entity.Updated = DateTime.Now;
            this.Session.SaveOrUpdate(entity);
        }

        public virtual void Delete(T entity)
        {
            if (this.Session.Transaction == null)
            {
                throw new InvalidOperationException("Write operations must use transaction");
            }

            this.Session.Delete(entity);
        }

        public virtual long RowCount()
        {
            return this.Session.QueryOver<T>().RowCountInt64();
        }
    }
}

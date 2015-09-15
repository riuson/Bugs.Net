using BugTracker.DB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Classes
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        protected NHibernate.ISession Session { get; set; }

        public Repository(BugTracker.DB.Interfaces.ISession session)
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

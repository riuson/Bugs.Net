using BugTracker.DB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Classes
{
    public class Repository<T> : IRepository<T> where T : new()
    {
        protected NHibernate.ISession Session { get; set; }

        public Repository(BugTracker.DB.Interfaces.ISession session)
        {
            Session s = session as Session;
            this.Session = s.NHSession;
        }

        public T Load(object id)
        {
            return this.Session.Load<T>(id);
        }

        public T GetById(object id)
        {
            return this.Session.Get<T>(id);
        }

        public ICollection<T> List()
        {
            return this.Session.CreateCriteria(typeof(T)).List<T>();
        }

        public void Save(T entity)
        {
            this.Session.Save(entity);
        }

        public void SaveOrUpdate(T entity)
        {
            this.Session.SaveOrUpdate(entity);
        }

        public void Delete(T entity)
        {
            this.Session.Delete(entity);
        }
    }
}

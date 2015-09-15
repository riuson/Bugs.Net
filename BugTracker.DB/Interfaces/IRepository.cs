using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Interfaces
{
    public interface IRepository<T> where T : new()
    {
        T Load(object id);
        T GetById(object id);
        ICollection<T> List();
        void Save(T entity);
        void SaveOrUpdate(T entity);
        void Delete(T entity);
        long RowCount();
    }
}

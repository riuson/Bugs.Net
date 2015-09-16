using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Interfaces
{
    public interface IRepository<T> where T : new()
    {
        /// <summary>
        /// Load entity by Id
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <returns>Entity</returns>
        T Load(object id);
        /// <summary>
        /// Try get entity by Id. If entity not found, returns null.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <returns>Entity</returns>
        T GetById(object id);
        /// <summary>
        /// Get collection of entities in repository
        /// </summary>
        /// <returns>Collecion of entities</returns>
        ICollection<T> List();
        /// <summary>
        /// Save entity in repository
        /// </summary>
        /// <param name="entity">Entity to save</param>
        void Save(T entity);
        /// <summary>
        /// Save or update entity to repository
        /// </summary>
        /// <param name="entity">Entity to save or update</param>
        void SaveOrUpdate(T entity);
        /// <summary>
        /// Delete entity from repository
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        void Delete(T entity);
        /// <summary>
        /// Number of entities in repository
        /// </summary>
        /// <returns>Number of entities</returns>
        long RowCount();
    }
}

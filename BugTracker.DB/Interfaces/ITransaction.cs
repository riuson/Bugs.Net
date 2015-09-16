using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Interfaces
{
    public interface ITransaction
    {
        /// <summary>
        /// Commit transaction on session dispose
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollback transaction on session dispose
        /// </summary>
        void Rollback();
    }
}

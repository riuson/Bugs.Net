using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.DataAccess
{
    public interface ISession : IDisposable
    {
        /// <summary>
        /// Transaction in session
        /// </summary>
        ITransaction Transaction { get; }
    }
}
